using System;
using System.Threading.Tasks;

namespace GameObjects
{
    public interface IUI
    {
        string Text { get; set; }

        bool InvokeRequired { get; }

        Ilobby L { get; set; }

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

    public interface Ilobby
    {
        void UpdateLobby(GameState go);

        void Close();

        bool OpenLobby_WaitForGuestsAndBegin();
    }

    /// <summary>
    /// Needed for Bots - to disregard all UI calls from bot client 
    /// </summary>
    public class DummyPlug : IUI
    {
        public string Text { get ; set ; }

        public bool InvokeRequired{get => false;}

        public Ilobby L { get ; set ; }

        public void AnnounceDeath()
        {
            return;
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
