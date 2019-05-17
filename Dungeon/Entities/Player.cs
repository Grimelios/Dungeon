using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Entities.Core;
using Engine.Core._3D;
using Engine.Input.Data;
using Engine.Interfaces;
using Engine.Messaging;
using Engine.Utility;
using GlmSharp;

namespace Dungeon.Entities
{
	public class Player : Entity, IReceiver
	{
		private const int AscendIndex = (int)PlayerSkills.Ascend;
		private const int JumpIndex = (int)PlayerSkills.Jump;
		private const int HighJumpIndex = (int)PlayerSkills.HighJump;
		private const int VaultIndex = (int)PlayerSkills.Vault;
		private const int WallJumpIndex = (int)PlayerSkills.WallJump;

		private PlayerControls controls;
		private Model model;

		private bool[] skillsUnlocked;
		private bool[] skillsEnabled;

		public Player()
		{
			controls = new PlayerControls();
			skillsUnlocked = new bool[Utilities.EnumCount<PlayerSkills>()];
			skillsEnabled = new bool[skillsUnlocked.Length];

			MessageSystem.Subscribe(this, CoreMessageTypes.Input, (messageType, data, dt) =>
			{
				ProcessInput((FullInputData)data);
			});
		}

		public List<MessageHandle> MessageHandles { get; set; }

		public override void Initialize(Scene scene)
		{
			model = new Model("Player.obj");
			scene.ModelBatch.Add(model);

			base.Initialize(scene);
		}

		private void ProcessInput(FullInputData data)
		{
			ProcessRunning(data);
		}

		private void ProcessRunning(FullInputData data)
		{
			bool left = data.Query(controls.RunLeft, InputStates.Held);
			bool right = data.Query(controls.RunRight, InputStates.Held);

			if (left ^ right)
			{
				vec3 p = Position;
				p.x += 0.03f * (left ? -1 : 1);
				Position = p;
			}
		}

		public void UnlockSkill(PlayerSkills skill)
		{
			int index = (int)skill;

			skillsUnlocked[index] = true;
			skillsEnabled[index] = CheckSkillEnabledOnUnlock(skill);
		}

		private bool CheckSkillEnabledOnUnlock(PlayerSkills skill)
		{
			return true;
		}

		public override void Update(float dt)
		{
			model.Position = Position;
			model.Orientation *= quat.FromAxisAngle(dt / 2, vec3.UnitY);
		}
	}
}
