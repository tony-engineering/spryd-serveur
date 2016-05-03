using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spryd.Serveur.Controllers
{
    public class BeaconController : ApiController
    {
        private IDal dal;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public BeaconController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Constructeur pour les tests
        /// </summary>
        /// <param name="fakeDal"></param>
        public BeaconController(IDal fakeDal)
        {
            dal = fakeDal;
        }

        /// <summary>
        /// Récupère la liste de tous les beacons
        /// </summary>
        /// <returns></returns>
        [Route("Beacons")]
        [HttpGet]
        public List<Beacon> GetBeacons()
        {
            return dal.GetBeacons();
        }
    }
}
