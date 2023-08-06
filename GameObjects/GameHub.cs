using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PolygonCollision;
using System;
using System.Linq;
using System.Threading.Tasks;
using GameObjects.Model;
using Action = GameObjects.Model.Action;

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

        public void Command(int who, Tuple<Action, HOTAS> command)
        {
            lock (_gameServer.gameObjects)
            {
                try
                {
                    _gameServer.gameObjects.Players.Single(p => p.ID == who).Act(command);
                }
                catch (Exception e)
                {
                    Logger.Log(e, LogLevel.Debug);
                }
            }
        }

        //TODO: get rid of those tuples. just use regular params
        public void Do(int who, Tuple<Action, Vector> command)
        {
            lock (_gameServer.gameObjects)
            {
                try
                {
                    _gameServer.gameObjects.Players.Single(p => p.ID == who).Act(command);
                }
                catch (Exception e)
                {
                    Logger.Log(e, LogLevel.Debug);
                }
            }
        }

        public void Over()
        {
            _gameServer.Stop();
        }

        public void JoinLobby(PlayerInfo playerInfo)
        {
            try
            {
                int playerID = _gameServer.Join(Context.ConnectionId, playerInfo);
                Clients.Client(Context.ConnectionId).JoinedLobby(playerID);
                Clients.All.UpdateLobby(_gameServer.gameObjects);
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }


        public void LeaveLobby(int playerID)
        {
            _gameServer.Leave(playerID);
            //Clients.All.UpdateModel(_gameServer.gameObjects);
        }

        public void Start()
        {
            _gameServer.Start();
        }

        public override Task OnConnected()
        {
            //groundwork for proper connections implementation
            //string name = Context.User.Identity.Name;
            //_connections.Add(name, Context.ConnectionId);

            return base.OnConnected();

        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //string name = Context.User.Identity.Name;
            //_connections.Remove(name, Context.ConnectionId);
            Logger.Log("disconnected", LogLevel.Info);            
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
