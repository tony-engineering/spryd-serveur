using Spryd.Server.Models;
using Spryd.Serveur;
using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Spryd.Server.Controllers
{
    public class SessionController : ApiController
    {
        private ISessionDal sessionDal;
        private IUserDal userDal;
        private ISprydZoneDal sprydZoneDal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionController()
        {
            sessionDal = new SessionDal();
            userDal = new UserDal();
            sprydZoneDal = new SprydZoneDal();
        }

        /// <summary>
        /// Constructor used for tests (possible to put a different DB)
        /// </summary>
        /// <param name="testDal"></param>
        public SessionController(ISessionDal testDal)
        {
            sessionDal = testDal;
        }

        /// <summary>
        /// Create session
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [Route("session")]
        [HttpPost]
        public Session AddSession([FromBody] UserSession userSession)
        {
            ValidateUserSession(userSession);
                        
            sessionDal.AddSession(userSession.Session); // Create the session            
            userDal.AddUserSession(userSession); // Add the creator to the session 

            return userSession.Session;
        }

        /// <summary>
        /// Validate user session informations before creating the session
        /// </summary>
        /// <param name="userSession"></param>
        private void ValidateUserSession(UserSession userSession)
        {
            // Check if the creator user exist
            if (!userDal.IsUserExist(userSession.UserId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userSession.UserId + " does not exist."));

            // Check if the new session's sprydzone exist
            if (!sprydZoneDal.IsSprydZoneExist(userSession.Session.SprydZoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no existing Spryd Zone with ID : " + userSession.Session.SprydZoneId + "."));

            // Check if there is not already a running session in this spryd zone
            if (sessionDal.IsAlreadySessionRunningInSprydZone(userSession.Session.SprydZoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is already a running session in Spryd Zone : " + userSession.Session.SprydZoneId + "."));

            userSession.Session.StartDate = DateTime.Now;
            userSession.LastActivity = DateTime.Now;
            userSession.StartDate = DateTime.Now;
            userSession.IsCreator = true;
        }

        /// <summary>
        /// Get session users
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        [Route("session/{idSession}/users")]
        [HttpGet]
        public List<User> GetSessionUsers(int idSession)
        {
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));
            return sessionDal.GetSessionUsers(idSession);
        }

        /// <summary>
        /// Join a session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [Route("session/{idSession}/user/{idUser}/join")]
        [HttpPost]
        public Session JoinSession(int idSession, int idUser)
        {
            CheckUserAndSession(idSession, idUser);
            
            var userSession = new UserSession()
            {
                UserId = idUser,
                SessionId = idSession,
                IsCreator = false,
                StartDate = DateTime.Now,
                LastActivity = DateTime.Now
            };

            userDal.AddUserSession(userSession);
            return sessionDal.GetSessionById(idSession);
        }

        /// <summary>
        /// Check if this session is available and if this user can join it
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        private void CheckUserAndSession(int idSession, int idUser)
        {
            // Check if the session exist
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));

            // Check if the creator user exist
            if (!userDal.IsUserExist(idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + idUser + " does not exist."));

            // Check if the user has already join this session
            if (userDal.IsUserInSession(idSession, idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "User " + idUser + " already join session " + idSession + "."));

            // Check if session is still going on
            if (!sessionDal.IsSessionRunning(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "Session " + idSession + " is over."));
        }
    }
}
