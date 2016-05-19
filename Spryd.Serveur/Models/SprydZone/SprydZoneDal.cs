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
        /// Get nearby SprydZone searched by beacons technical ID
        /// </summary>
        /// <param name="listBeacons"></param>
        /// <returns></returns>
        public List<SprydZone> GetNearbySprydZone(List<Beacon> listBeacons)
        {
            using (DbConnection c = new DbConnection())
            {
                return (from sprydZone in c.SprydZones
                        join beacon in listBeacons on sprydZone.Beacon.TechnicalId equals beacon.TechnicalId
                        select sprydZone).ToList();
            }
            
        }
    }
}