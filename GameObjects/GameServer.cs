using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;

namespace GameObjects
{
    public class GameServer
	{
		private readonly static Lazy<GameServer> _instance = new Lazy<GameServer>(() => new GameServer());
		private Random rnd = new Random();
		public Thread thrdGameLoop;
		public bool Connected { get; set; } = false;
		public IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
		public ClsGameObjects gameObjects;
		IDisposable webapp;
		ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
		ManualResetEvent _pauseEvent = new ManualResetEvent(true);

		public GameServer()
		{
			gameObjects = new ClsGameObjects(GameConfig.WorldSize);
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
			

			thrdGameLoop = new Thread(GameLoop)
			{
				Name = "GameLoop"
			};
			thrdGameLoop.Start();
			Console.WriteLine("Fight!");
			gameObjects.GameOn = true;
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
			DateTime dt;// = DateTime.UtcNow;
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
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
					tdiff = DateTime.UtcNow - dt;
					Thread.Sleep((ClsGameObjects.FrameInterval - tdiff).Duration()); //this is bad. There should be timer instead
					Console.WriteLine(gameObjects.frameNum + "| " + tdiff.ToString());
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

			Console.WriteLine(string.Format("Server listening at {0}", url));

			while (true)
			{
				//Thread.Sleep(100);

				if (Connected)
				{
					Start();					
					return;
				}
			}
		}
	}
}
