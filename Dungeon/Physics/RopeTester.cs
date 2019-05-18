using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core;
using Engine.Graphics._2D;
using Engine.Input.Data;
using Engine.Interfaces;
using Engine.Interfaces._2D;
using Engine.Messaging;
using GlmSharp;

namespace Dungeon.Physics
{
	public class RopeTester : IReceiver, IDynamic, IRenderable2D
	{
		private VerletRope rope;

		public RopeTester()
		{
			rope = new VerletRope(20, 20);

			MessageSystem.Subscribe(this, CoreMessageTypes.Mouse, (messageType, data, dt) =>
			{
				ProcessMouse((MouseData)data);
			});
		}

		public List<MessageHandle> MessageHandles { get; set; }

		private void ProcessMouse(MouseData data)
		{
			rope.Position = data.Location;
		}

		public void Update(float dt)
		{
			rope.Update(dt);
		}

		public void Draw(SpriteBatch sb)
		{
			var points = rope.Points;

			for (int i = 0; i < points.Length - 1; i++)
			{
				sb.DrawLine(points[i].Position, points[i + 1].Position, Color.White);
			}
		}
	}
}
