using System;
using System.Collections.Generic;
using Spryd.Serveur.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using Spryd.Server.Models;
using System.Linq;

namespace Spryd.Serveur.Controllers
{
    internal class BeaconDal : IBeaconDal
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconDal()
        {
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