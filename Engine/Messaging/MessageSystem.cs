using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Interfaces;

namespace Engine.Messaging
{
	using ReceiverFunction = Action<int, object, float>;

	public static class MessageSystem
	{
		private static Dictionary<int, List<ReceiverFunction>> functionMap;

		static MessageSystem()
		{
			functionMap = new Dictionary<int, List<ReceiverFunction>>();
		}

		public static void Subscribe(IReceiver receiver, int messageType, ReceiverFunction function)
		{
			// Initializing the handle list here simplifies contructors for receiving classes (since they don't all
			// need to individually create those lists).
			if (receiver.MessageHandles == null)
			{
				receiver.MessageHandles = new List<MessageHandle>();
			}

			if (!functionMap.TryGetValue(messageType, out List<ReceiverFunction> functions))
			{
				functions = new List<ReceiverFunction>();
				functionMap.Add(messageType, functions);
			}

			int index = -1;

			// When a class subscribes to a message type, its callback is stored in the first open slot in the function
			// list (or appended to the end if all slots are filled)
			for (int i = 0; i < functions.Count; i++)
			{
				if (functions[i] == null)
				{
					index = i;

					break;
				}
			}

			// This means that no open slots were found, so the new function must be appended instead.
			if (index == -1)
			{
				functions.Add(function);
			}
			else
			{
				functions[index] = function;
			}
			
			// It's assumed that the same object won't subscribe to the same message type more than once. If multiple
			// callbacks are needed, a single lambda can be used to call several functions.
			receiver.MessageHandles.Add(new MessageHandle(messageType, index));
		}

		public static void Unsubscribe(IReceiver receiver, int messageType = -1)
		{
			// By default, the given receiver is unsubscribed from all message types. Passing a message type explicitly
			// unsubscribes from only that type.
			if (messageType != -1)
			{

			}
		}

		public static void ProcessChanges()
		{
		}

		public static void Send(int messageType, object data, float dt = 0)
		{
			if (!functionMap.TryGetValue(messageType, out List<ReceiverFunction> list))
			{
				return;
			}

			foreach (var receiver in list)
			{
				receiver?.Invoke(messageType, data, dt);
			}
		}
	}
}
