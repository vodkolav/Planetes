using System.Threading.Tasks;
using GameObjects.Model;

namespace GameObjects
{
    /// <summary>
    /// Needed for Bots - to disregard all UI calls from bot client 
    /// </summary>
    public class DummyPlug : IUI
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

        //public object Invoke(Delegate method, params object[] args)
        //{
        //    return new object();
        //}

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

        //public void StartGraphics()
        //{
        //}

        public void UpdateLobby(GameState go)
        {
        }
    }
}
