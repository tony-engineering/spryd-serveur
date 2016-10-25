using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using Spryd.Server.Models;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System;

namespace Spryd.Server.Tests
{
    [TestClass]
    public class SessionTest
    {
        private FakeSprydContext context;
        private SessionDal sessionDal;
        private UserDal userDal;
        private SprydZoneDal sprydZoneDal;
        private SessionController sessionController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            context = new FakeSprydContext();
            sessionDal = new SessionDal(context);
            userDal = new UserDal(context);
            sprydZoneDal = new SprydZoneDal(context);

            sessionController = new SessionController(context);
            sessionController.Request = new HttpRequestMessage();
            sessionController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// Create session with existing user,session and sprydzone
        /// </summary>
        [TestMethod]
        public void CreateSession_Success()
        {
            userDal.AddUser(new User()); // id = 1
            sprydZoneDal.AddSprydZone(new SprydZone()); // id = 1
            UserSession newSessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() { SprydZoneId = 1}
            };

            Session newSession = sessionController.AddSession(newSessionParameters);

            Assert.AreEqual(sessionDal.GetSessionById(1), newSession);
            Assert.AreEqual(sessionDal.GetSessionAllUsers(newSession.Id).First().Id, 1);
        }

        /// <summary>
        /// Create a session with an unknown user
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void CreateSessionWithoutUser_Failed_ThrowsException()
        {
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 3 });
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
            userDal.AddUser(new User());
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
            userDal.AddUser(new User()); // id = 1
            sprydZoneDal.AddSprydZone(new SprydZone()); // id = 1
            UserSession firstSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 1 }
            };

            UserSession secondSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 1 }
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
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession firstSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id = 1, SprydZoneId = 1 }
            };

            UserSession secondSessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id = 2, SprydZoneId = 1 }
            };

            sessionController.AddSession(firstSessionParameters);
            sessionController.EndSession(1);
            sessionController.AddSession(secondSessionParameters);

            Assert.AreEqual(sprydZoneDal.GetSprydZoneCurrentession(1), userDal.GetCurrentSession(1));
        }

        /// <summary>
        /// Join an existing running session
        /// </summary>
        [TestMethod]
        public void JoinSession_Success()
        {
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id =1, SprydZoneId = 1 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2,null);

            Assert.AreEqual(1, userDal.GetCurrentSession(2).Id);
            Assert.AreEqual(sprydZoneDal.GetSprydZoneCurrentession(1), userDal.GetCurrentSession(2));
        }

        /// <summary>
        /// Join a session in which you are the creator
        /// Throw exception because you are already in the session since the creation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void JoinSessionAlreadyJoined_ThrowException()
        {
            userDal.AddUser(new User() { Id = 1 });
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() {Id = 1, SprydZoneId = 3 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 1, null);
        }

        [TestMethod]
        public void JoinSession_GoodPassword_Success()
        {
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() {Id =1, SprydZoneId = 3 , Password ="azerty"}
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2, "azerty");

            Assert.IsTrue(userDal.IsUserInSession(1,2));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void JoinSession_WrongPassword_ThrowException()
        {
            userDal.AddUser(new User());
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 3 });
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                SessionId = 1,
                Session = new Session() { SprydZoneId = 3 , Password = "azerty" }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 1, "patati");
        }

        /// <summary>
        /// Join an unknown session
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void JoinUnknownSession_ThrowException()
        {
            userDal.AddUser(new User());
            sessionController.JoinSession(1, 1,null);
        }

        /// <summary>
        /// If the creator leave the session, users are getting out of the session and it ends the session
        /// </summary>
        [TestMethod]
        public void CreatorLeaveSession_EndSession_Success()
        {
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id =1, SprydZoneId = 1 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2,null);
            sessionController.LeaveSession(1, 1); // creator leave the session, so it get users out of the session and end the session

            Assert.IsFalse(sessionDal.IsSessionRunning(1)); // session over
            Assert.AreEqual(null,sprydZoneDal.GetSprydZoneCurrentession(1)); // no more session running in spryd zone 1

            Assert.AreEqual(null, userDal.GetCurrentSession(2)); // user 2 have no more current session
        }

        /// <summary>
        /// Leave the session
        /// </summary>
        [TestMethod]
        public void UserLeaveSession_Success()
        {
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id = 1, SprydZoneId = 1 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2,null); // user 2 join session 1

            Assert.IsTrue(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?
            sessionController.LeaveSession(1, 2); // user 2 leave the session 1
            Assert.IsFalse(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?

            Assert.IsTrue(sessionDal.IsSessionRunning(1)); // session continue to run
            Assert.AreEqual(null, userDal.GetCurrentSession(2)); // user 2 have no more current session
            Assert.AreEqual(1, userDal.GetCurrentSession(1).Id); // user 1 is still in the session
        }

        /// <summary>
        /// User leave a session which is already over so it throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void UserLeaveSessionAlreadyOver_ThrowException()
        {
            userDal.AddUser(new User()); // id = 1
            sprydZoneDal.AddSprydZone(new SprydZone()); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 1 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.EndSession(1);
            sessionController.LeaveSession(1, 1); // user 1 leave the session 1 but session is already over so exception
        }

        /// <summary>
        /// User leave session two times, both userSession are ended
        /// </summary>
        [TestMethod]
        public void UserLeaveSessionTwice_Success()
        {
            userDal.AddUser(new User() { Id = 1 }); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() {Id = 1, SprydZoneId = 1 }
            };
            sessionController.AddSession(sessionParameters);
            sessionController.JoinSession(1, 2,null); // user 2 join session 1

            Assert.IsTrue(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?
            sessionController.LeaveSession(1, 2); // user 2 leave the session 1
            Assert.IsFalse(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?
            
            Assert.AreEqual(null, userDal.GetCurrentSession(2)); // user 2 have no more current session

            sessionController.JoinSession(1, 2,null); // user 2 join session 1
            Assert.IsTrue(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?

            sessionController.LeaveSession(1, 2); // user 2 leave the session 1 again
            Assert.IsFalse(userDal.IsUserInSession(1, 2)); // is user 2 in session 1 ?

            Assert.AreEqual(null, userDal.GetCurrentSession(2)); // user 2 have no more current session
        }

        /// <summary>
        /// User creates session
        /// Don't leave it properly (kills app, still in session)
        /// Other user joins
        /// List users from left session after 70 secs of inactivity
        /// All users are kicked
        /// Session is ended
        /// </summary>
        [TestMethod]
        public void ScenarioInactiveCreatorSessionClosedWhenParticipantJoins_Success()
        {
            userDal.AddUser(new User() { Id = 1}); // id = 1
            userDal.AddUser(new User() { Id = 2 }); // id = 2
            sprydZoneDal.AddSprydZone(new SprydZone() { Id = 1 }); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                Id = 1,
                UserId = 1,
                Session = new Session() { Id = 1, SprydZoneId = 1 }, // id session = 1
                IsCreator = true,
                LastActivity = DateTime.Now.AddSeconds(-70) // force inactive
            };

            sessionController.AddSession(sessionParameters);
            Assert.IsTrue(userDal.IsUserInSession(1, 1));

            sessionController.JoinSession(1, 2);
            Assert.IsTrue(userDal.IsUserInSession(1, 2));

            sessionController.GetSessionUsers(1); // list users from session 1

            userDal.IsAllActivitiesEnded(1, 1);
            Assert.IsTrue(userDal.IsAllActivitiesEnded(1, 1));
            Assert.IsTrue(userDal.IsAllActivitiesEnded(1, 2));

            Assert.IsFalse(userDal.IsUserInSession(1, 1));
            Assert.IsFalse(userDal.IsUserInSession(1, 2));
            Assert.IsFalse(sessionDal.IsSessionRunning(1));
        }

        /// <summary>
        /// User creates session
        /// Don't leave it properly (kills app, still in session)
        /// He joins session again
        /// </summary>
        [TestMethod]
        public void ScenarioInactiveCreatorJoinsAgain_Success()
        {
            userDal.AddUser(new User()); // id = 1
            sprydZoneDal.AddSprydZone(new SprydZone()); // id = 1
            UserSession sessionParameters = new UserSession()
            {
                UserId = 1,
                Session = new Session() { SprydZoneId = 1 }, // id session = 1
                IsCreator = true,
                LastActivity = DateTime.Now.AddSeconds(-70) // force inactive
            };

            sessionController.AddSession(sessionParameters);
            Assert.IsTrue(userDal.IsUserInSession(1, 1));

            sessionController.JoinSession(1, 1);
            Assert.IsTrue(userDal.IsUserInSession(1, 1));

            sessionController.GetSessionUsers(1); // list users from session 1

            Assert.IsTrue(sessionDal.IsSessionRunning(1));
            Assert.IsTrue(userDal.IsUserInSession(1, 1));
        }
    }
}
