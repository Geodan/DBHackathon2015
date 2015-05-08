using System.Web.Http;

namespace Blackbird.Services
{
    public static class WebApiConfig
    {
        public const string DEFAULT_ROUTE_NAME = "MyDefaultRoute";

        public static void Register(HttpConfiguration config)
        {
            // home route
            config.Routes.MapHttpRoute(
                "Home", // Route name 
                "", // URL with parameters 
                new { controller = "Home" } // Parameter defaults 
            ); 

            config.Routes.MapHttpRoute(
                name: DEFAULT_ROUTE_NAME,
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
