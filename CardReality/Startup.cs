using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CardReality.Startup))]
namespace CardReality
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
