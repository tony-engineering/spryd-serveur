using Microsoft.Practices.Unity;
using Spryd.Server.App_Start;
using Spryd.Server.Controllers;
using Spryd.Server.Models;
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
        public static string SharedItemsRepository;

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

            //Injection des dépendances
            var container = new UnityContainer();
            container.RegisterType<ISprydContext, SprydContext>();
            config.DependencyResolver = new UnityResolver(container);

            // Output in JSON
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Enable Cross Origin Requests
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            // Set logging configuration
            log4net.Config.XmlConfigurator.Configure();

            // Définition de l'URL de dépôt des sharedItems
            string subPath = ConfigurationManager.AppSettings["SharedItemsRepository"]; // Dépôt des sharedItems
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(subPath)))//Check si le répertoire existe
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));// sinon il le créé
            SharedItemsRepository = HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["SharedItemsRepository"];
        }
    }
}
