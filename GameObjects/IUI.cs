using GameObjects.Model;
using PolygonCollision;

namespace GameObjects
{
    public interface IUI
    {
        string Text { get; set; }


        Size VisorSize { get;  }

        void DrawGraphics();

        void Notify(Notification type, string message);

        void AnnounceDeath(string message);

        void AnnounceRespawn(string message);

        void CloseLobby(); //TODO: drop this. UI needs to determine it by itself.

        void UpdateLobby(GameState go);

        void Start();

        void GameOver();
    }
}
