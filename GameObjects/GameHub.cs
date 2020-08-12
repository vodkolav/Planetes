using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace GameObjects
{
	[HubName("GameHub")]
	public class GameHub : Hub
	{
		GameServer _gameServer;

		public GameHub(GameServer srv)
		{
			_gameServer = srv;
		}

        public async  void Broadcast()
        {
            Clients.All.hi();
            await Clients.All.upd(_gameServer.gameObjects);
        }

        public override Task OnConnected()
		{

			_gameServer.Connected = true;
			Clients.All.upd(_gameServer.gameObjects);		
			return base.OnConnected();

		}

		public override Task OnDisconnected(bool stopCalled)
		{
            Console.WriteLine("disconnected");
			return Clients.All.disconnected("diconnected");
		}
	}
}
