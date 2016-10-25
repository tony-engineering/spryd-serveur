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
        private ISprydContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconDal(ISprydContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get beacon by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Beacon GetBeaconById(int id)
        {
            return _context.Beacons.Where(b => b.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get beacon by technical ID
        /// </summary>
        /// <param name="technicalId"></param>
        /// <returns></returns>
        public Beacon GetBeaconByTechnicalId(string technicalId)
        {
            return _context.Beacons.Where(b => b.TechnicalId == technicalId).FirstOrDefault();
        }

        /// <summary>
        /// Lists all Beacons
        /// </summary>
        /// <returns></returns>
        public List<Beacon> GetBeacons()
        {
            return _context.Beacons.ToList();
        }
    }
}