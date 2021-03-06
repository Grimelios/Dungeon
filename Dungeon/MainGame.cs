﻿using System;
using System.Collections.Generic;
using Dungeon.Entities;
using Dungeon.Entities.Core;
using Dungeon.Physics._2D;
using Engine;
using Engine.Core._2D;
using Engine.Graphics._2D;
using Engine.Interfaces;
using Engine.Messaging;
using Engine.Utility;
using Engine.View;
using GlmSharp;
using static Engine.GL;

namespace Dungeon
{
	public class MainGame : Game, IReceiver, IDisposable
	{
		private SpriteBatch sb;
		private Sprite mainSprite;
		private RenderTarget mainTarget;
		private Camera3D camera;
		private Scene scene;
		private List<IRenderTargetUser> renderTargetUsers;

		private PhysicsTester physicsTester;

		public MainGame() : base("Dungeon")
		{
			sb = new SpriteBatch();
			camera = new Camera3D();
			camera.IsOrthographic = true;
			camera.Orientation *= quat.FromAxisAngle(0, vec3.UnitX);
			camera.Position = new vec3(0, 0, 1) * camera.Orientation;

			mainTarget = new RenderTarget(Resolution.RenderWidth, Resolution.RenderHeight,
				RenderTargetFlags.Color | RenderTargetFlags.Depth);
			mainSprite = new Sprite(mainTarget, null, Alignments.Left | Alignments.Top);
			mainSprite.Mods = SpriteModifiers.FlipVertical;

			Player player = new Player();
			player.UnlockSkill(PlayerSkills.Jump);

			scene = new Scene();
			scene.Camera = camera;
			scene.Add(player);
			scene.ModelBatch.LightDirection = Utilities.Normalize(new vec3(1, -0.2f, 0));

			renderTargetUsers = new List<IRenderTargetUser>();
			renderTargetUsers.Add(scene.ModelBatch);

			physicsTester = new PhysicsTester();

			MessageSystem.Subscribe(this, CoreMessageTypes.ResizeWindow, (messageType, data, dt) =>
			{
				mainSprite.ScaleTo(Resolution.WindowWidth, Resolution.WindowHeight);
			});

			MessageSystem.ProcessChanges();
			MessageSystem.Send(CoreMessageTypes.ResizeRender, Resolution.RenderDimensions);
			MessageSystem.Send(CoreMessageTypes.ResizeWindow, Resolution.WindowDimensions);
		}

		public List<MessageHandle> MessageHandles { get; set; }

		public void Dispose()
		{
			mainTarget.Dispose();
			mainSprite.Dispose();
			renderTargetUsers.ForEach(u => u.Dispose());

			MessageSystem.Unsubscribe(this);
		}

		protected override void Update(float dt)
		{
			//scene.Update(dt);
			camera.Update(dt);
			physicsTester.Update(dt);

			MessageSystem.ProcessChanges();
		}

		protected override void Draw()
		{
			glEnable(GL_DEPTH_TEST);
			glEnable(GL_CULL_FACE);
			glDepthFunc(GL_LEQUAL);
			
			//renderTargetUsers.ForEach(u => u.DrawTargets());
			//mainTarget.Apply();
			//scene.ModelBatch.Draw(camera);

			glBindFramebuffer(GL_FRAMEBUFFER, 0);
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glViewport(0, 0, (uint)Resolution.WindowWidth, (uint)Resolution.WindowHeight);
			glDisable(GL_DEPTH_TEST);
			glDisable(GL_CULL_FACE);
			glDepthFunc(GL_NEVER);

			//mainSprite.Draw(sb);
			physicsTester.Draw(sb);
			sb.Flush();
		}
	}
}
