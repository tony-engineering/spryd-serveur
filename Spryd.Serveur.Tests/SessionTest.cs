using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using Spryd.Serveur.Models;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using Spryd.Serveur.Tests;
using Spryd.Server.Models;
using System.Linq;

namespace Spryd.Server.Tests
{
    [TestClass]
    public class SessionTest
    {
        private FakeDal dal;
        private SessionController sessionController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            dal = new FakeDal();

            sessionController = new SessionController(dal,dal,dal);
            sessionController.Request = new HttpRequestMessage();
            sessionController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CreateSession_Success()
        {
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession newSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3}
            };

            Session newSession = sessionController.AddSession(newSessionParameters);

            Assert.AreEqual(dal.GetSessionById(1), newSession);
            Assert.AreEqual(dal.GetSessionUsers(newSession.Id).First().Id, 1);
        }
    }
}
