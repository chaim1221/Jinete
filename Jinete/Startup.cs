using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jinete.Startup))]
namespace Jinete
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
