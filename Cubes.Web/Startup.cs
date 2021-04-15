using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cubes.Web.Startup))]
namespace Cubes.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
