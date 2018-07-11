using Castle.Windsor;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Tornado.API.Windsor;

[assembly: OwinStartup(typeof(Tornado.API.Startup))]
namespace Tornado.API
{
    public class Startup
    {
        #region Private helper methods

        private static WindsorDependencyResolver CreateDependencyResolver()
        {
            var container = new WindsorContainer().Install(
                new ControllerInstaller(),
                new DependencyInstaller());

            return new WindsorDependencyResolver(container);
        }

        #endregion

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration {DependencyResolver = CreateDependencyResolver()};

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}
