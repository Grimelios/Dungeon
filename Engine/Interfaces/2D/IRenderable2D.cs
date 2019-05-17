using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;
using Engine.Graphics._2D;

namespace Engine.Interfaces._2D
{
	public interface IRenderable2D
	{
		void Draw(SpriteBatch sb);
	}
}
