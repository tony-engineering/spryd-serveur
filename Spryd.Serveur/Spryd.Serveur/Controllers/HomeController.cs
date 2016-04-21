using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spryd.Serveur.Controllers
{
    public class HomeController : Controller
    {
        // GET: Login
        [Route("User")]
        [HttpGet]
        public string Index()
        {
            return "Hello World";
        }
    }
}