using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(baseDeDatosNube.Startup))]
namespace baseDeDatosNube
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
