using System.Web.Http;
using Owin;

namespace Remote.Spotify
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultPage",
                routeTemplate: "{page}",
                defaults: new {controller = "Page"}
                );

            app.MapHubs();
            app.UseWebApi(config);
        }
    }
}