using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

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
