using CardReality.Enums;
using CardReality.Services;
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
            LocalizationService.CurrentLanguage = Language.Bg;
        }
    }
}
