using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(University.Auth.Startup))]
namespace University.Auth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
