using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.View
{
	public abstract class CameraController3D
	{
		public Camera3D Camera { get; set; }

		public abstract void Update();
	}
}
