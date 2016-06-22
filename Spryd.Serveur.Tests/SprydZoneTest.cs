using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using System.Net.Http;
using System.Web.Http;
using Spryd.Server.Models;

namespace Spryd.Server.Tests
{
    [TestClass]
    public class SprydZoneTest
    {
        private FakeDal dal;
        private SprydZoneController sprydZoneController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            dal = new FakeDal();

            sprydZoneController = new SprydZoneController(dal);
            sprydZoneController.Request = new HttpRequestMessage();
            sprydZoneController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// Get a list of nearby spryd zone with a list of technical beacon ID
        /// </summary>
        [TestMethod]
        public void GetNearbySprydZone_Success()
        {
            dal.AddSprydZone(new SprydZone("", new Beacon() { TechnicalId = "1"}));
            dal.AddSprydZone(new SprydZone("", new Beacon() { TechnicalId = "2" }));
            dal.AddSprydZone(new SprydZone("", new Beacon() { TechnicalId = "3" }));

            var listNearbyZone = sprydZoneController.GetNearbySprydZones(new string[] { "1", "3" });

            Assert.AreEqual(2, listNearbyZone.Count);
        }

        /// <summary>
        /// Add a sprydZone and retrieve it
        /// </summary>
        [TestMethod]
        public void GetSprydZone_Success()
        {
            var sprydZone = new SprydZone();
            dal.AddSprydZone(sprydZone);
            Assert.AreEqual(sprydZone, sprydZoneController.GetSprydZoneById(1));
        }

        /// <summary>
        /// Get unknown spryd zone throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetUnknownSprydZone_ThrowsException()
        {
            sprydZoneController.GetSprydZoneById(1);
        }
    }
}
