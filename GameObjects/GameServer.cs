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

		public void StartServer()
		{
			Console.WriteLine("Begin Game");
			//Console.ReadLine();

			gameObjects.GameOver = false;

			thrdGameLoop = new Thread(GameLoop)
			{
				Name = "GameLoop"
			};
			thrdGameLoop.Start();
		}

		public void AbortGame()
		{
			gameObjects.GameOver = true;
			if (thrdGameLoop != null)
				thrdGameLoop.Abort();
			webapp.Dispose();
		}


		private async  void GameLoop()
		{
			while (!gameObjects.GameOver)
			{
				Thread.Sleep(ClsGameObjects.FrameInterval);
				gameObjects.Frame();
                //IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

                try
                {
					string gobj = JsonConvert.SerializeObject(gameObjects);
					await hubContext.Clients.All.upd(gameObjects);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
				
				await hubContext.Clients.All.hi("i say again" + rnd.Next(1,100).ToString());
				//hubContext.Clients.All.addMessage("server", "ServerMessage");
				//Console.WriteLine("Server Sending addMessage\n");
			}
		}

		public void Listen(string url)
		{
			//string url = @"http://localhost:8030/";
			webapp = WebApp.Start<Startup>(url);

			Console.WriteLine(string.Format("Server listening at {0}", url));
		}
	}
}
