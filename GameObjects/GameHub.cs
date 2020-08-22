using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PolygonCollision;

namespace GameObjects
{
	[HubName("GameHub")]
	public class GameHub : Hub
	{	
		//groundwork for proper connections and users implementation 
		//private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
		GameServer _gameServer;

		public GameHub(GameServer srv)
		{
			_gameServer = srv;
		}

		public void Command(string who, Tuple<Action, HOTAS> command)
		{
			//string name = Context.User.Identity.Name;

			_gameServer.gameObjects.players.Single(p => p.Name == who).Act(command);

			//foreach (var connectionId in _connections.GetConnections(who))
			//{
			//	Clients.Client(connectionId).addChatMessage(name + ": " + message);
			//}
		}

		public void Aim(string who, Tuple<Action, Vector> command)
		{
			_gameServer.gameObjects.players.Single(p => p.Name == who).Aim(command.Item2);						
		}

		public void Over()
		{
			_gameServer.Stop();
		}

		public override Task OnConnected()
		{
			//groundwork for proper connections implementation  
			//string name = Context.User.Identity.Name;
			//_connections.Add(name, Context.ConnectionId);

			_gameServer.Connected = true;
			Clients.All.UpdateModel(_gameServer.gameObjects);		
			return base.OnConnected();

		}

		public override Task OnDisconnected(bool stopCalled)
		{
			//string name = Context.User.Identity.Name;
			//_connections.Remove(name, Context.ConnectionId);

			Console.WriteLine("disconnected");
			return Clients.All.disconnected("diconnected");
		}

		public override Task OnReconnected()
		{
			//string name = Context.User.Identity.Name;

			//if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
			//{
			//	_connections.Add(name, Context.ConnectionId);
			//}

			return base.OnReconnected();
		}
	}
}
