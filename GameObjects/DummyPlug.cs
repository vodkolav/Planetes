﻿using System.Threading.Tasks;
using GameObjects.Model;
using PolygonCollision;

namespace GameObjects
{
    /// <summary>
    /// Needed for Bots - to disregard all UI calls from bot client 
    /// </summary>
    public class DummyPlug : IUI
    {
        public GameClient C { get ; set ; }

        public string Text { get ; set ; }

        public Size VisorSize { get; } = new Size(500, 500);

        //public bool InvokeRequired{get => false;}

        public void AnnounceDeath(string message)
        {
        }

        public void bindHUDS()
        {
        }

        public void CloseLobby()
        {
        }

        public void DrawGraphics()
        {
        }

        //public object Invoke(Delegate method, params object[] args)
        //{
        //    return new object();
        //}

        public async Task LeaveLobby()
        {
            await Task.Delay(0);
        }

        public void Notify(Notification type, string message)
        {
        }

        public void Start()
        {
        }

        public void GameOver()
        {
        }

        public void AnnounceRespawn(string message)
        {
            
        }

        //public void StartGraphics()
        //{
        //}

        public void UpdateLobby(GameState go)
        {
        }
    }
}
