using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameObjects
{
    public class GameServer
	{
		private readonly static Lazy<GameServer> _instance = new Lazy<GameServer>(() => new GameServer());
		private Random rnd = new Random();
		public Thread thrdGameLoop;
		public IHubContext hubContext;
		public GameState gameObjects;
		IDisposable webapp;
		ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
		ManualResetEvent _pauseEvent = new ManualResetEvent(true);

		public GameServer()
		{
			gameObjects = new GameState(GameConfig.WorldSize);			
			hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
		}

		public void Join(int playerID, string ConnectionID, string PlayerName)
		{			
			Player newplayer = new Player(playerID, ConnectionID, PlayerName, gameObjects);		    
			gameObjects.players.Add(newplayer);
			//string gobj = JsonConvert.SerializeObject(gameObjects); //only for debugging - to check what got serialized
			hubContext.Clients.All.UpdateLobby(gameObjects);			
		}

		public void DispatchMessages()
        {
			if (gameObjects.messageQ.Count != 0)
			{
				foreach (var mes in gameObjects.messageQ.GetConsumingEnumerable())
				{
					string to = mes.Item1;
					string mess = mes.Item2;
					hubContext.Clients.Client(to).Notify(mess);
				}
			}
		}

        public static GameServer Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public void Start()
		{
			InitWarringParties();
			thrdGameLoop = new Thread(GameLoop)
			{
				Name = "GameLoop"
			};
			thrdGameLoop.Start();
			Console.WriteLine("Fight!");
			gameObjects.GameOn = true;
			hubContext.Clients.All.Start();
		}

		/// <summary>
		/// Define enemies for each player
		/// </summary>
        private void InitWarringParties()
        {			
			//simplest case: Free-For-All (All-Against-All)
			foreach (Player p1 in gameObjects.players)
			{
				foreach(Player p2 in gameObjects.players)
                {
					p1.FeudWith(p2);
                }				
			}
        }

        public void Stop(string message = "Game aborted")
		{
			// Signal the shutdown event
			_shutdownEvent.Set();
			Console.WriteLine(message);

			gameObjects.GameOn = false;
			//if (thrdGameLoop != null)
			//	thrdGameLoop.Abort();
			webapp.Dispose();

			// Make sure to resume any paused threads
			_pauseEvent.Set();

			// Wait for the thread to exit
			thrdGameLoop.Join();
		}

		public void Pause()
		{
			_pauseEvent.Reset();
			Console.WriteLine("Game paused");
		}

		public void Resume()
		{
			_pauseEvent.Set();
			Console.WriteLine("Game resumed");
		}

		private async  void GameLoop()
		{
			DateTime dt;// maybe replace this with stopwatch
			TimeSpan tdiff;
			while (gameObjects.GameOn)
			{
				_pauseEvent.WaitOne(Timeout.Infinite);

				if (_shutdownEvent.WaitOne(0))
					break;
				
				dt = DateTime.UtcNow;				
				if (gameObjects.Frame())
				{
					try
					{
						//string gobj = JsonConvert.SerializeObject(gameObjects); only for debugging - to check what got serialized
						await hubContext.Clients.All.UpdateModel(gameObjects);
						Task.Run( DispatchMessages);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
					tdiff = DateTime.UtcNow - dt;
					Thread.Sleep((GameState.FrameInterval - tdiff).Duration()); //this is bad. There should be timer instead
					Console.WriteLine("frameNum: " + gameObjects.frameNum);// + "| " + tdiff.ToString()
				}
				else
                {
					Stop();
                }				
			}
		}
		
		public void Listen(string url)
		{			
			webapp = WebApp.Start<Startup>(url);

			Console.WriteLine(string.Format("Lobby open at {0}", url));
		}
	}
}
