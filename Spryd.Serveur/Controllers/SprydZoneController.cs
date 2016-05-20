using Spryd.Server.Models;
using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spryd.Server.Controllers
{
    public class SprydZoneController : ApiController
    {
        private ISprydZoneDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SprydZoneController()
        {
            dal = new SprydZoneDal();
        }

        [Route("Beacons/zones")]
        [HttpPost]
        public List<SprydZone> GetNearbySprydZones([FromBody] List<Beacon> listBeacons)
        {
            return dal.GetNearbySprydZone(listBeacons);
        }

        [Route("Beacons/zones/all")]
        [HttpGet]
        public List<SprydZone> GetAllSprydZones()
        {
            return dal.GetAllSprydZones();
        }

        [Route("Beacons/zones/{zoneId}")]
        [HttpGet]
        public SprydZone GetSprydZoneById(int sprydZoneId)
        {
            return dal.GetSprydZoneById(sprydZoneId);
        }
    }
}
