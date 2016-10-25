using Microsoft.Practices.Unity;
using Spryd.Server.Controllers;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Spryd.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Démarrage de l'application
        /// </summary>
        protected void Application_Start()
        {
            //Initialisation générale
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            
        }
    }
}
