using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hangman.Web.Startup))]
namespace Hangman.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
