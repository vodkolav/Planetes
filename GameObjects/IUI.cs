using System;
using System.Threading.Tasks;

namespace GameObjects
{
    public interface IUI
    {
        string Text { get; set; }

        GameClient C { get; set; }

        void DrawGraphics();

        void bindHUDS(GameState gameObjects);

        void Notify(string message);

        void AnnounceDeath(string message);

        Task LeaveLobby();

        void CloseLobby();

        void UpdateLobby(GameState go);

        void Start();
    }
}
