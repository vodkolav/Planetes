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
}
