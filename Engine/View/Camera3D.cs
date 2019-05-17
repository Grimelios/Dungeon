using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces;
using Engine.Interfaces._3D;
using Engine.Messaging;
using GlmSharp;

namespace Engine.View
{
	public class Camera3D : IReceiver, ITransformable3D, IDynamic
	{
		private mat4 projection;
		private CameraController3D controller;

		private bool isOrthographic;

		public Camera3D()
		{
			Orientation = quat.Identity;

			MessageSystem.Subscribe(this, CoreMessageTypes.ResizeRender, (messageType, data, dt) =>
			{
				RecomputeProjection();
			});
		}

		public List<MessageHandle> MessageHandles { get; set; }

		public bool IsOrthographic
		{
			get => isOrthographic;
			set
			{
				isOrthographic = value;
				RecomputeProjection();
			}
		}

		public vec3 Position { get; set; }
		public quat Orientation { get; set; }
		public mat4 ViewProjection { get; private set; }
		public mat4 ViewProjectionInverse { get; private set; }

		public void SetTransform(vec3 position, quat orientation)
		{
			Position = position;
			Orientation = orientation;
		}

		private void RecomputeProjection()
		{
			const float OrthoWidth = 8;
			const float OrthoHeight = 4.5f;

			ivec2 dimensions = Resolution.RenderDimensions;

			projection = isOrthographic
				? mat4.Ortho(-OrthoWidth, OrthoWidth, -OrthoHeight, OrthoHeight, 0.1f, 100)
				: mat4.PerspectiveFov(90, dimensions.x, dimensions.y, 0.1f, 100);
		}

		public void Attach(CameraController3D controller)
		{
			this.controller = controller;

			controller.Camera = this;
		}

		public void Update(float dt)
		{
			controller?.Update();

			mat4 view = new mat4(Orientation) * mat4.Translate(-Position.x, -Position.y, -Position.z);

			ViewProjection = projection * view;
			ViewProjectionInverse = ViewProjection.Inverse;
		}
	}
}
