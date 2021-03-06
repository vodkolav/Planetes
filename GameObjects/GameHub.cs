﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PolygonCollision;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                _gameServer.gameObjects.players.Single(p => p.ID == who).Act(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }     
        }

        public void Aim(int who, Tuple<Action, Vector> command)
        {
            try
            {
                _gameServer.gameObjects.players.Single(p => p.ID == who).Aim(command.Item2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Over()
        {
            _gameServer.Stop();
        }

        public void JoinLobby(string PlayerName)
        {
            int playerID = _gameServer.Join( Context.ConnectionId, PlayerName);
            Clients.Client(Context.ConnectionId).JoinedLobby(playerID);
            Clients.All.UpdateLobby(_gameServer.gameObjects);
            //Clients.All.UpdateModel(_gameServer.gameObjects);
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
