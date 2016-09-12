using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RMS.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("http://localhost:50163", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "CustomApi",
                routeTemplate: "api/{controller}/{calendarId}/{startDate}/{endDate}",
                defaults: new 
                {
                    calendarId = RouteParameter.Optional,
                    startDate = RouteParameter.Optional,
                    endDate = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
