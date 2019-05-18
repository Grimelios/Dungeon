using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics._3D;
using Engine.Interfaces;
using Engine.Interfaces._3D;
using Engine.View;

namespace Dungeon.Entities.Core
{
	public class Scene : IDynamic, IRenderable3D
	{
		private List<Entity> entities;

		public Scene()
		{
			entities = new List<Entity>();
			ModelBatch = new ModelBatch(100000, 10000);
		}

		public Camera3D Camera { get; set; }
		public ModelBatch ModelBatch { get; }

		public void Add(Entity entity)
		{
			entity.Initialize(this);
			entities.Add(entity);
		}

		public void Update(float dt)
		{
			entities.ForEach(e => e.Update(dt));
		}

		public void Draw(Camera3D camera)
		{
		}
	}
}
