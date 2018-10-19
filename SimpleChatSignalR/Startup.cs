using Microsoft.Owin;
using Owin;
using SimpleChatSignalR;

namespace SimpleChatSignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}