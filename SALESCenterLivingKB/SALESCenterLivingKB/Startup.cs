using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SALESCenterLivingKB.Startup))]
namespace SALESCenterLivingKB
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
