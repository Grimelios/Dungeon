using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core._2D;
using Engine.Graphics._2D;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Messaging;
using Engine.Shaders;
using GlmSharp;
using static Engine.GL;

namespace Engine.Graphics._3D
{
	public class ShadowMapVisualizer : IReceiver, IRenderable2D, IDisposable
	{
		private const int DefaultSize = 250;

		private Sprite sprite;

		public ShadowMapVisualizer(RenderTarget shadowMapTarget)
		{
			Shader shader = new Shader();
			shader.Attach(ShaderTypes.Vertex, "Sprite.vert");
			shader.Attach(ShaderTypes.Fragment, "ShadowMapVisualization.frag");
			shader.AddAttribute<float>(2, GL_FLOAT);
			shader.AddAttribute<float>(2, GL_FLOAT);
			shader.AddAttribute<byte>(4, GL_UNSIGNED_BYTE, true);
			shader.CreateProgram();

			sprite = new Sprite(shadowMapTarget, null, Alignments.Left | Alignments.Bottom);
			sprite.Shader = shader;

			DisplaySize = DefaultSize;

			MessageSystem.Subscribe(this, CoreMessageTypes.ResizeWindow, (messageType, data, dt) =>
			{
				sprite.Position = new vec2(0, Resolution.WindowHeight);
			});
		}

		public List<MessageHandle> MessageHandles { get; set; }

		public int DisplaySize
		{
			set => sprite.ScaleTo(value, value);
		}

		public void Dispose()
		{
			sprite.Dispose();

			MessageSystem.Unsubscribe(this);
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
