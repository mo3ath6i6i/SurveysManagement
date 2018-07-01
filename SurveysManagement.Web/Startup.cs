using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SurveysManagement.Web.Startup))]
namespace SurveysManagement.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
