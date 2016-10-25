using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using System.Net.Http;
using System.Web.Http;
using Spryd.Server.Models;
using System.Linq;

namespace Spryd.Server.Tests
{
    [TestClass]
    public class BeaconTest
    {
        private FakeSprydContext _context;
        private BeaconController beaconController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            _context = new FakeSprydContext();

            beaconController = new BeaconController(_context);
            beaconController.Request = new HttpRequestMessage();
            beaconController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// Check if all beacons are valid
        /// </summary>
        [TestMethod]
        public void ValidateBeacon_Success()
        {
            var listBeacon = _context.Beacons;
            Assert.IsFalse(listBeacon.Any(b => b.IsValid() == false));
        }

        /// <summary>
        /// Tries to get a non existing user
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetNotExistingBeacon_ThrowsException()
        {
            beaconController.GetBeaconById(-1);
        }
    }
}
