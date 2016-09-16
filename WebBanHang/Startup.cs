using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanHang.Startup))]
namespace WebBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
