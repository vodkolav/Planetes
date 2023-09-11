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
            //seems like this is irrelevant change,  the app still gets disconnected 
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(300); // 5 min 
            app.MapSignalR();
        }
    }
}
