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
using Engine.Shapes._2D;
using GlmSharp;
using static Engine.GLFW;

namespace Dungeon.Physics._2D
{
	public class PhysicsTester : IReceiver, IDynamic, IRenderable2D
	{
		private World2D world;

		public PhysicsTester()
		{
			Body2D staticBox = new Body2D(new Rectangle(300, 75), null);
			staticBox.Position = new vec2(400, 300);
			staticBox.IsStatic = true;

			Body2D dynamicBox = new Body2D(new Capsule(60, 20), null);
			dynamicBox.Position = new vec2(400, 150);

			world = new World2D();
			world.Add(staticBox);
			world.Add(dynamicBox);

			MessageSystem.Subscribe(this, CoreMessageTypes.Keyboard, (messageType, data, dt) =>
			{
				ProcessKeyboard((KeyboardData)data);
			});
		}

		public List<MessageHandle> MessageHandles { get; set; }

		private void ProcessKeyboard(KeyboardData data)
		{
			const int Speed = 250;

			bool left = data.Query(GLFW_KEY_A, InputStates.Held);
			bool right = data.Query(GLFW_KEY_D, InputStates.Held);
			bool up = data.Query(GLFW_KEY_W, InputStates.Held);
			bool down = data.Query(GLFW_KEY_S, InputStates.Held);

			var body = world.DynamicBodies[0];
			var v = body.Velocity;

			if (left ^ right)
			{
				v.x = left ? -Speed : Speed;
			}
			else
			{
				v.x = 0;
			}

			if (up ^ down)
			{
				v.y = up ? -Speed : Speed;
			}
			else
			{
				v.y = 0;
			}

			body.Velocity = v;
		}

		public void Update(float dt)
		{
			world.Step(dt);
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (var body in world.StaticBodies)
			{
				sb.Draw((Rectangle)body.Shape, Color.White);
			}

			foreach (var body in world.DynamicBodies)
			{
				sb.Draw((Capsule)body.Shape, 10, Color.Cyan);
			}
		}
	}
}
