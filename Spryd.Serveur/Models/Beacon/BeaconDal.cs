using System;
using System.Collections.Generic;
using Spryd.Server.Models;
using System.Configuration;
using System.Linq;
using log4net;

namespace Spryd.Server.Models
{
    public class BeaconDal : IBeaconDal
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconDal()
        {
        }

        /// <summary>
        /// Get beacon by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Beacon GetBeaconById(int id)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Beacons.Where(b => b.Id == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get beacon by technical ID
        /// </summary>
        /// <param name="technicalId"></param>
        /// <returns></returns>
        public Beacon GetBeaconByTechnicalId(string technicalId)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Beacons.Where(b => b.TechnicalId == technicalId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lists all Beacons
        /// </summary>
        /// <returns></returns>
        public List<Beacon> GetBeacons()
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Beacons.ToList();
            }
        }
    }
}