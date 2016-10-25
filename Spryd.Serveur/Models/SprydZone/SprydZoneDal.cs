using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spryd.Server.Models;
using log4net;
using System.Data.Entity;

namespace Spryd.Server.Models
{
    public class SprydZoneDal : ISprydZoneDal
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISprydContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SprydZoneDal(ISprydContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a spryd zone
        /// </summary>
        /// <param name="sprydZone"></param>
        public void AddSprydZone(SprydZone sprydZone)
        {
            _context.SprydZones.Add(sprydZone);
        }

        /// <summary>
        /// Get all SprydZones
        /// </summary>
        /// <returns></returns>
        public List<SprydZone> GetAllSprydZones()
        {
            return _context.SprydZones.Include("Beacon").ToList();
        }

        /// <summary>
        /// Get nearby SprydZone searched by beacons technical ID
        /// </summary>
        /// <param name="listBeaconId"></param>
        /// <returns></returns>
        public List<SprydZone> GetNearbySprydZone(List<string> listBeaconId)
        {
            if (listBeaconId == null)
                return new List<SprydZone>();

            return (from sprydZone in _context.SprydZones.Include("Beacon").ToList()
                    join beaconId in listBeaconId on sprydZone.Beacon.TechnicalId equals beaconId
                    select sprydZone).ToList();
        }

        /// <summary>
        /// Get SprydZone by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SprydZone GetSprydZoneById(int id)
        {
            return _context.SprydZones.Include("Beacon").ToList().Where(z => z.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get Spryd Zone current session
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public Session GetSprydZoneCurrentession(int zoneId)
        {
            return _context.Sessions.Where(s => s.SprydZoneId == zoneId && s.StartDate != null && s.EndDate == null).FirstOrDefault();
        }

        /// <summary>
        /// Indicate if the SprydZone exist
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public bool IsSprydZoneExist(int zoneId)
        {
            return _context.SprydZones.Any(s => s.Id == zoneId);
        }
    }
}