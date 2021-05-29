using System.Threading.Tasks;

namespace GameObjects
{
    /// <summary>
    /// Needed for Bots - to disregard all UI calls from bot client 
    /// </summary>
    public class DummyUI : IUI
    {
        public GameClient C { get ; set ; }

        public string Text { get ; set ; }

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

        public async Task LeaveLobby()
        {
            await Task.Delay(0);
        }

        public void Notify(string message)
        {
        }

        public void Start()
        {
        }
        
        public void UpdateLobby(GameState go)
        {
        }
    }
}
