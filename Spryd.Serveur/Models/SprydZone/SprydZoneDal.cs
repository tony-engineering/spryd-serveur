using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spryd.Server.Models;
using log4net;

namespace Spryd.Server.Models
{
    public class SprydZoneDal : ISprydZoneDal
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public SprydZoneDal()
        {

        }

        /// <summary>
        /// Add a spryd zone
        /// </summary>
        /// <param name="sprydZone"></param>
        public void AddSprydZone(SprydZone sprydZone)
        {
            using (DbConnection c = new DbConnection())
            {
                c.SprydZones.Add(sprydZone);
            }
        }

        /// <summary>
        /// Get all SprydZones
        /// </summary>
        /// <returns></returns>
        public List<SprydZone> GetAllSprydZones()
        {
            using (DbConnection c = new DbConnection())
            {
                return c.SprydZones.Include("Beacon").ToList();
            }
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
            using (DbConnection c = new DbConnection())
            {
                return (from sprydZone in c.SprydZones.Include("Beacon").ToList()
                        join beaconId in listBeaconId on sprydZone.Beacon.TechnicalId equals beaconId
                        select sprydZone).ToList();
            }
            
        }

        /// <summary>
        /// Get SprydZone by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SprydZone GetSprydZoneById(int id)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.SprydZones.Include("Beacon").ToList().Where(z => z.Id == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get Spryd Zone current session
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public Session GetSprydZoneCurrentession(int zoneId)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Sessions.Where(s => s.SprydZoneId == zoneId && s.StartDate != null && s.EndDate == null).FirstOrDefault();
            }
        }

        /// <summary>
        /// Indicate if the SprydZone exist
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public bool IsSprydZoneExist(int zoneId)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.SprydZones.Any(s => s.Id == zoneId);
            }
        }
    }
}