using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dotnetfundaMongoDB.Startup))]
namespace dotnetfundaMongoDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
