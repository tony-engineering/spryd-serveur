using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// Constructor used for tests (possible to put a different DB)
        /// </summary>
        /// <param name="testDal"></param>
        public BeaconController(IBeaconDal testDal)
        {
            dal = testDal;
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
