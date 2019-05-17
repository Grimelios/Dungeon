using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Messaging;

namespace Engine.Interfaces
{
	public interface IReceiver
	{
		List<MessageHandle> MessageHandles { get; set; }
	}
}
