using System;
using System.Collections.Generic;
using Engine.Core;
using Engine.Core._2D;
using Engine.Core._3D;
using Engine.Interfaces;
using Engine.Interfaces._3D;
using Engine.Shaders;
using Engine.View;
using GlmSharp;
using static Engine.GL;

namespace Engine.Graphics._3D
{
	public class ModelBatch : IRenderTargetUser, IRenderable3D, IDisposable
	{
		private Shader modelShader;
		private Shader shadowMapShader;
		private RenderTarget shadowMapTarget;
		private Texture defaultTexture;
		private mat4 lightMatrix;
		private List<ModelHandle> handles;

		private uint bufferId;
		private uint indexBufferId;

		// These sizes are updated as data is buffered to the GPU. The data isn't actually stored here.
		private int bufferSize;
		private int indexBufferSize;
		private int maxIndex;

		public ModelBatch(int bufferSize, int indexBufferSize)
		{
			const int ShadowMapSize = 2048;
			
			GLUtilities.AllocateBuffers(bufferSize, indexBufferSize, out bufferId, out indexBufferId, GL_STATIC_DRAW);

			modelShader = new Shader();
			modelShader.Attach(ShaderTypes.Vertex, "ModelShadow.vert");
			modelShader.Attach(ShaderTypes.Fragment, "ModelShadow.frag");
			modelShader.AddAttribute<float>(3, GL_FLOAT);
			modelShader.AddAttribute<float>(2, GL_FLOAT);
			modelShader.AddAttribute<float>(3, GL_FLOAT);
			modelShader.CreateProgram();
			modelShader.Bind(bufferId, indexBufferId);
			modelShader.Use();
			modelShader.SetUniform("shadowSampler", 0);
			modelShader.SetUniform("textureSampler", 1);

			shadowMapShader = new Shader();
			shadowMapShader.Attach(ShaderTypes.Vertex, "ShadowMap.vert");
			shadowMapShader.Attach(ShaderTypes.Fragment, "ShadowMap.frag");
			shadowMapShader.AddAttribute<float>(3, GL_FLOAT, false, false, sizeof(float) * 5);
			shadowMapShader.CreateProgram();
			shadowMapShader.Bind(bufferId, indexBufferId);

			shadowMapTarget = new RenderTarget(ShadowMapSize, ShadowMapSize, RenderTargetFlags.Depth);
			defaultTexture = ContentCache.GetTexture("Grey.png");
			handles = new List<ModelHandle>();

			// These default values are arbitrary, just to make sure something shows up.
			LightDirection = vec3.UnitX;
			LightColor = Color.White;
			AmbientIntensity = 0.1f;
		}

		public vec3 LightDirection { get; set; }
		public Color LightColor { get; set; }

		public float AmbientIntensity { get; set; }

		public unsafe void Add(Model model)
		{
			Mesh mesh = model.Mesh;

			var points = mesh.Points;
			var source = mesh.Source;
			var normals = mesh.Normals;
			var vertices = mesh.Vertices;

			float[] buffer = new float[vertices.Length * 8];

			for (int i = 0; i < vertices.Length; i++)
			{
				var v = vertices[i];
				var p = points[v.x];
				var s = source[v.y];
				var n = normals[v.z];

				int start = i * 8;

				buffer[start] = p.x;
				buffer[start + 1] = p.y;
				buffer[start + 2] = p.z;
				buffer[start + 3] = s.x;
				buffer[start + 4] = s.y;
				buffer[start + 5] = n.x;
				buffer[start + 6] = n.y;
				buffer[start + 7] = n.z;
			}

			ushort[] indices = mesh.Indices;

			int size = sizeof(float) * buffer.Length;
			int indexSize = sizeof(ushort) * indices.Length;

			handles.Add(new ModelHandle(model, indices.Length, indexBufferSize, maxIndex));
			maxIndex += mesh.MaxIndex + 1;

			glBindBuffer(GL_ARRAY_BUFFER, bufferId);

			fixed (float* address = &buffer[0])
			{
				glBufferSubData(GL_ARRAY_BUFFER, bufferSize, (uint)size, address);
			}

			bufferSize += size;

			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId);

			fixed (ushort* address = &indices[0])
			{
				glBufferSubData(GL_ELEMENT_ARRAY_BUFFER, indexBufferSize, (uint)indexSize, address);
			}

			indexBufferSize += indexSize;
		}

		public unsafe void Dispose()
		{
			uint[] buffers =
			{
				bufferId,
				indexBufferId
			};

			fixed (uint* address = &buffers[0])
			{
				glDeleteBuffers(2, address);
			}

			modelShader.Dispose();
			shadowMapShader.Dispose();
			shadowMapTarget.Dispose();
		}

		public void DrawTargets()
		{
			const int OrthoSize = 8;

			glDisable(GL_CULL_FACE);

			mat4 lightView = mat4.LookAt(-LightDirection * 10, vec3.Zero, vec3.UnitY);
			mat4 lightProjection = mat4.Ortho(-OrthoSize, OrthoSize, -OrthoSize, OrthoSize, 0.1f, 100);

			lightMatrix = lightProjection * lightView;

			shadowMapTarget.Apply();
			shadowMapShader.Apply();

			foreach (ModelHandle handle in handles)
			{
				Model model = handle.Model;

				model.RecomputeWorldMatrix();
				shadowMapShader.SetUniform("lightMatrix", lightMatrix * model.WorldMatrix);
				Draw(handle);
			}
		}

		public void Draw(Camera3D camera)
		{
			glEnable(GL_CULL_FACE);
			glCullFace(GL_BACK);
			glActiveTexture(GL_TEXTURE0);
			glBindTexture(GL_TEXTURE_2D, shadowMapTarget.Id);
			glActiveTexture(GL_TEXTURE1);
			glBindTexture(GL_TEXTURE_2D, defaultTexture.Id);

			modelShader.Apply();
			modelShader.SetUniform("lightDirection", LightDirection);
			modelShader.SetUniform("lightColor", LightColor.ToVec3());
			modelShader.SetUniform("ambientIntensity", AmbientIntensity);

			// See http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-16-shadow-mapping/.
			mat4 biasMatrix = new mat4
			(
				0.5f, 0.0f, 0.0f, 0,
				0.0f, 0.5f, 0.0f, 0,
				0.0f, 0.0f, 0.5f, 0,
				0.5f, 0.5f, 0.5f, 1
			);

			mat4 cameraMatrix = camera.ViewProjection;

			foreach (ModelHandle handle in handles)
			{
				Model model = handle.Model;
				mat4 world = model.WorldMatrix;
				quat orientation = model.Orientation;

				modelShader.SetUniform("orientation", orientation.ToMat4);
				modelShader.SetUniform("mvp", cameraMatrix * world);
				modelShader.SetUniform("lightBiasMatrix", biasMatrix * lightMatrix * world);

				Draw(handle);
			}
		}

		private unsafe void Draw(ModelHandle handle)
		{
			glDrawElementsBaseVertex(GL_TRIANGLES, (uint)handle.Count, GL_UNSIGNED_SHORT, (void*)handle.Offset,
				handle.BaseVertex);
		}
	}
}
