using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_2FactorLogin.Startup))]
namespace _2FactorLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
