using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spryd.Serveur.Models;

namespace Spryd.Server.Models
{
    public class SprydZoneDal : ISprydZoneDal
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SprydZoneDal()
        {

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
        /// <param name="listBeacons"></param>
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

        public SprydZone GetSprydZoneById(int id)
        {
            using (DbConnection c = new DbConnection())
            {
                var sprydZone = c.SprydZones.Where(z => z.Id == id).FirstOrDefault();
                
                return sprydZone;
            }
        }
    }
}