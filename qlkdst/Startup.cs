using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(qlkdst.Startup))]
namespace qlkdst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
