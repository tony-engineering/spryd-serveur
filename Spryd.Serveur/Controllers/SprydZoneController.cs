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

        /// <summary>
        /// Get nearby SprydZone searched by beacons technical ID
        /// Need technicalId in URL parameters
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [Route("zone/nearby")]
        [HttpPost]
        public List<SprydZone> GetNearbySprydZones([FromUri] string[] values)
        {
            if (values.IsNullOrEmpty())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Need a list of beacon technical Id"));
            return dal.GetNearbySprydZone(values.ToList());
        }
        
        /// <summary>
        /// Get all Spryd Zones
        /// </summary>
        /// <returns></returns>
        [Route("zone/all")]
        [HttpGet]
        public List<SprydZone> GetAllSprydZones()
        {
            return dal.GetAllSprydZones();
        }

        /// <summary>
        /// Get Spryd Zone by Id
        /// </summary>
        /// <param name="sprydZoneId"></param>
        /// <returns></returns>
        [Route("zone/{zoneId}")]
        [HttpGet]
        public SprydZone GetSprydZoneById(int zoneId)
        {
            if (!dal.IsSprydZoneExist(zoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "SprydZone " + zoneId + " is null."));
            return dal.GetSprydZoneById(zoneId);
        }

        /// <summary>
        /// Get the current session of a spryd zone
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        [Route("zone/{zoneId}/currentSession")]
        [HttpGet]
        public Session GetSprydZoneCurrentSession(int zoneId)
        {
            if (!dal.IsSprydZoneExist(zoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "SprydZone " + zoneId + " is null."));
            return dal.GetSprydZoneCurrentession(zoneId);
        }
    }

    public static class MyExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            return !enumerable.Any();
        }
    }
}
