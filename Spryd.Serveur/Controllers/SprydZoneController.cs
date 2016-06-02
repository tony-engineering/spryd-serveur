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
        [HttpGet]
        public List<SprydZone> GetNearbySprydZones([FromUri] string[] values)
        {
            if (values.IsNullOrEmpty())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Need a list of beacon technical Id"));
            return dal.GetNearbySprydZone(values.ToList());
        }
        
        /// <summary>
        /// Get all Spryd Zones with their current session
        /// </summary>
        /// <returns></returns>
        [Route("zone/all")]
        [HttpGet]
        public List<KeyValuePair<SprydZone, Session>> GetAllSprydZones()
        {
            var listSprydZoneState = new Dictionary<SprydZone, Session>();
            foreach (var sprydZone in dal.GetAllSprydZones())
                listSprydZoneState.Add(sprydZone, dal.GetSprydZoneCurrentession(sprydZone.Id));
            
            return listSprydZoneState.ToList();
        }

        /// <summary>
        /// Get Spryd Zone by Id
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        [Route("zone/{zoneId}")]
        [HttpGet]
        public KeyValuePair<SprydZone, Session> GetSprydZoneById(int zoneId)
        {
            if (!dal.IsSprydZoneExist(zoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "SprydZone " + zoneId + " does not exist."));
            return new KeyValuePair<SprydZone, Session>(dal.GetSprydZoneById(zoneId),dal.GetSprydZoneCurrentession(zoneId));
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
            var currentSession = dal.GetSprydZoneCurrentession(zoneId);
            if(currentSession == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "SprydZone " + zoneId + " doesn't have a current session."));
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
