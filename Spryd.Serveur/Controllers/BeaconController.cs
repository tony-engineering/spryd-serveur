using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spryd.Serveur.Controllers
{
    public class BeaconController : ApiController
    {
        private IBeaconDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconController()
        {
            dal = new BeaconDal(WebApiConfig.connectionString);
        }

        /// <summary>
        /// Lists all Beacons
        /// </summary>
        /// <returns></returns>
        [Route("Beacons")]
        [HttpGet]
        public List<Beacon> GetBeacons()
        {
            return dal.GetBeacons();
        }
    }
}
