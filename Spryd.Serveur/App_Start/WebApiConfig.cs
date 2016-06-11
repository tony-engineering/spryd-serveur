using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Spryd.Server
{
    public static class WebApiConfig
    {
        public static ConnectionStringSettings connectionString;
        public static String SharedItemsRepository;
        public static String ApiUrl;

        public static void Register(HttpConfiguration config)
        {
            // Connection String
            connectionString = ConfigurationManager.ConnectionStrings["DbConnection"];


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
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            ApiUrl = ConfigurationManager.AppSettings["SprydURL"];
            SharedItemsRepository = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["SharedItemsRepository"]);
        }
    }
}
