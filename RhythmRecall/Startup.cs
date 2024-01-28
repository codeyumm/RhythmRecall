using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RhythmRecall.Startup))]
namespace RhythmRecall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
