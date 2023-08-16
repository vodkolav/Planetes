using System.Threading.Tasks;
using GameObjects.Model;
using PolygonCollision;

namespace GameObjects
{
    public interface IUI
    {
        string Text { get; set; }

        GameClient C { get; set; }

        Size VisorSize { get;  }

        void DrawGraphics();

        void bindHUDS();

        void Notify(Notification type, string message);

        void AnnounceDeath(string message);

        void AnnounceRespawn(string message);

        Task LeaveLobby();

        void CloseLobby();

        void UpdateLobby(GameState go);

        void Start();

        void GameOver();
    }
}
