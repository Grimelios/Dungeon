using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.View;

namespace Engine.Interfaces._3D
{
	public interface IRenderable3D
	{
		void Draw(Camera3D camera);
	}
}
