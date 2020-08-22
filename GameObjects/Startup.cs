using Owin;
using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace GameObjects
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			GlobalHost.DependencyResolver.Register(
			typeof(GameHub),
			() => new GameHub(GameServer.Instance));
			app.MapSignalR(); // "/signalr", new HubConfiguration());
		}
	}
}
