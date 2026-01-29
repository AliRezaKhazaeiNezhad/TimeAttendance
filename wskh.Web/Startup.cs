using AutoMapper;
using Microsoft.Owin;
using Owin;
using wskh.Core;
using wskh.FingerTec;

[assembly: OwinStartupAttribute(typeof(wskh.Web.Startup))]
namespace wskh.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
