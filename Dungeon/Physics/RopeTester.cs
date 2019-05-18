using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Core;
using Engine.Core._3D;
using Engine.Graphics._2D;
using Engine.Graphics._3D;
using Engine.Input.Data;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Interfaces._3D;
using Engine.Messaging;
using Engine.Utility;
using Engine.View;
using GlmSharp;

namespace Dungeon.Physics
{
	public class RopeTester : IReceiver, IDynamic
	{
		private VerletRope rope;
		private Model[] models;

		private float lightAngle;

		public RopeTester()
		{
			const int Segments = 12;
			const int Length = 30;

			rope = new VerletRope(Segments, Length);
			rope.Position = new vec2(400, 300);

			models = new Model[Segments];
			Batch = new ModelBatch(1000000, 100000);

			for (int i = 0; i < Segments; i++)
			{
				Model model = new Model("Chain.obj");
				models[i] = model;
				Batch.Add(model);
			}

			MessageSystem.Subscribe(this, CoreMessageTypes.Mouse, (messageType, data, dt) =>
			{
				ProcessMouse((MouseData)data);
			});
		}

		public ModelBatch Batch { get; }
		public List<MessageHandle> MessageHandles { get; set; }

		private void ProcessMouse(MouseData data)
		{
			rope.Position = data.Location;
		}

		public void Update(float dt)
		{
			const float Divisor = 7.7f;

			rope.Update(dt);

			var points = rope.Points;

			for (int i = 0; i < points.Length - 1; i++)
			{
				var point1 = points[i];
				var point2 = points[i + 1];

				vec2 p = (point1.Position + point2.Position) / 2;

				Model model = models[i];
				model.Position = new vec3(p.x / (Resolution.WindowWidth / Divisor) - 4,
					-p.y / (Resolution.WindowHeight / Divisor) + 4, 0);
				model.Orientation = quat.FromAxisAngle(i * 0.7f, vec3.UnitY);
			}

			vec2 l = Utilities.Direction(lightAngle);

			Batch.LightDirection = new vec3(l.x, 0, l.y);
			lightAngle += dt / 2;
		}
		
		/*
		public void Draw(SpriteBatch sb)
		{
			var points = rope.Points;

			for (int i = 0; i < points.Length - 1; i++)
			{
				sb.DrawLine(points[i].Position, points[i + 1].Position, Color.White);
			}
		}
		*/
	}
}
