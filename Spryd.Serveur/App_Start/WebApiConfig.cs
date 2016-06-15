using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

            // Set logging configuration
            log4net.Config.XmlConfigurator.Configure();

            ApiUrl = ConfigurationManager.AppSettings["SprydURL"];

            string subPath = ConfigurationManager.AppSettings["SharedItemsRepository"]; // your code goes here

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(subPath)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));

            SharedItemsRepository = HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["SharedItemsRepository"];
        }
    }
}
