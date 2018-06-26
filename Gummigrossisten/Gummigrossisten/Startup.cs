using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gummigrossisten.Startup))]
namespace Gummigrossisten
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
