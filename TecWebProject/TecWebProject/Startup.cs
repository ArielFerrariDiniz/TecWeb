using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TecWebProject.Startup))]
namespace TecWebProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
