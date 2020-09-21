using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;

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
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(400);
            app.MapSignalR();
        }
    }
}
