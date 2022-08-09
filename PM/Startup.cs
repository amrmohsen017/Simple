using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PM.Startup))]
namespace PM
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR("/hubs/test" , new HubConfiguration()); // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.MapSignalR();
        }
    }
}
