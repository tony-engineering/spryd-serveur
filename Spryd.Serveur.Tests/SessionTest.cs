using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using Spryd.Server.Models;
using System.Net.Http;
using System.Web.Http;
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
        /// Create session with existing user,session and sprydzone
        /// </summary>
        [TestMethod]
        public void CreateSession_Success()
        {
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession newSessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() { SprydZoneId = 3}
            };

            Session newSession = sessionController.AddSession(newSessionParameters);

            Assert.AreEqual(dal.GetSessionById(1), newSession);
            Assert.AreEqual(dal.GetSessionUsers(newSession.Id).First().Id, 1);
        }

        /// <summary>
        /// Create a session with an unknown user
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void CreateSessionWithoutUser_Failed_ThrowsException()
        {
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession newSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };

            sessionController.AddSession(newSessionParameters);
        }

        /// <summary>
        /// Create a session in an unknown spryd zone
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void CreateSessionInNotExistingSprydZone_Failed_ThrowsException()
        {
            dal.AddUser(new User());
            UserSession newSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };

            sessionController.AddSession(newSessionParameters);
        }

        /// <summary>
        /// Create a session in a spryd zone where there is already a session running
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void CreateSessionInSameSprydZone_Failed_ThrowsException()
        {
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession firstSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };

            UserSession secondSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };
            sessionController.AddSession(firstSessionParameters);
            sessionController.AddSession(secondSessionParameters);
        }

        /// <summary>
        /// Create a session then end it, create a second session on the same spryd zone
        /// </summary>
        [TestMethod]
        public void CreateSessionInSameSprydZoneButAfterEndingFirstSession_Success()
        {
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession firstSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };

            UserSession secondSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };
            sessionController.AddSession(firstSessionParameters);

            sessionController.EndSession(1);

            sessionController.AddSession(secondSessionParameters);
            var i = dal.GetSprydZoneCurrentession(3);
            var z = dal.GetCurrentSession(1);
            Assert.AreEqual(dal.GetSprydZoneCurrentession(3), dal.GetCurrentSession(1));
        }

        /// <summary>
        /// Join an existing running session
        /// </summary>
        [TestMethod]
        public void JoinSession_Success()
        {
            dal.AddUser(new User());
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2);

            Assert.AreEqual(1, dal.GetCurrentSession(2).Id);
            Assert.AreEqual(dal.GetSprydZoneCurrentession(3), dal.GetCurrentSession(2));
        }

        /// <summary>
        /// Join a session in which you are the creator
        /// Throw exception because you are already in the session since the creation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void JoinSessionAlreadyJoined_ThrowException()
        {
            dal.AddUser(new User());
            dal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() { SprydZoneId = 3 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 1);
        }

        /// <summary>
        /// Join an unknown session
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void JoinUnknownSession_ThrowException()
        {
            dal.AddUser(new User());
            sessionController.JoinSession(1, 1);
        }
    }
}
