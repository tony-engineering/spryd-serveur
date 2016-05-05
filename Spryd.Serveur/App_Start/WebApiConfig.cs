using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Spryd.Serveur
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API services configuration

            // Web API Routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Output in JSON
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Enable Cross Origin Requests
            config.EnableCors();
            //var corsAttr = new EnableCorsAttribute("http://example.com", "*", "*");
            //config.EnableCors(corsAttr);
        }
    }
}
