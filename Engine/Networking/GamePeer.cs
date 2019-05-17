using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Engine.Networking
{
	public class GamePeer
	{
		private NetPeer peer;

		public GamePeer()
		{
			NetPeerConfiguration config = new NetPeerConfiguration("Zeldo");
			peer = new NetPeer(config);
		}

		public void Refresh()
		{
			NetIncomingMessage message;

			while ((message = peer.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
				}

				peer.Recycle(message);
			}
		}
	}
}
