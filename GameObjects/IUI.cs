using System;
using System.Threading.Tasks;

namespace GameObjects
{
    public interface IUI
    {
        string Text { get; set; }

        bool InvokeRequired { get; }

        GameClient C { get; set; }

        void StartGraphics();

        void DrawGraphics();

        void bindHUDS(GameState gameObjects);

        void Notify(string message);

        void AnnounceDeath();

        Task LeaveLobby();

        void CloseLobby();

        object Invoke(Delegate method, params object[] args);

        void UpdateLobby(GameState go);
    }


    /// <summary>
    /// Needed for Bots - to disregard all UI calls from bot client 
    /// </summary>
    public class DummyPlug : IUI
    {
        public GameClient C { get ; set ; }

        public string Text { get ; set ; }

        public bool InvokeRequired{get => false;}
          
        public void AnnounceDeath()
        {
        }

        public void bindHUDS(GameState gameObjects)
        {
        }

        public void CloseLobby()
        {
        }

        public void DrawGraphics()
        {
        }

        public object Invoke(Delegate method, params object[] args)
        {
            return new object();
        }

        public async Task LeaveLobby()
        {
            await Task.Delay(0);
        }

        public void Notify(string message)
        {
        }

        public void StartGraphics()
        {
        }

        public void UpdateLobby(GameState go)
        {
        }
    }
}
