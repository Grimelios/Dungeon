using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Messaging
{
	public class MessageHandle
	{
		public MessageHandle(int messageType, int messageIndex)
		{
			MessageType = messageType;
			MessageIndex = messageIndex;
		}

		public int MessageType { get; }
		public int MessageIndex { get; }
	}
}
