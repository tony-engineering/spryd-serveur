using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spryd.Server.Controllers
{
    public class BeaconController : ApiController
    {
        private IBeaconDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconController()
        {
            dal = new BeaconDal();
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
        [Route("beacon/all")]
        [HttpGet]
        public List<Beacon> GetBeacons()
        {
            return dal.GetBeacons();
        }

        /// <summary>
        /// Get beacon by id
        /// </summary>
        /// <returns></returns>
        [Route("beacon/{id}")]
        [HttpGet]
        public Beacon GetBeaconById(int id)
        {
            var beacon = dal.GetBeaconById(id);
            if (beacon == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Beacon " + id + " is null."));
            return dal.GetBeaconById(id);
        }
    }
}
