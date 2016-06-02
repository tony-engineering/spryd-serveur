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
    /// <summary>
    /// User controller
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SessionController : ApiController
    {
        private ISessionDal sessionDal;
        private IUserDal userDal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionController()
        {
            sessionDal = new SessionDal();
            userDal = new UserDal();
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

            // Create session and add the user to the session 
            userDal.AddUserSession(userSession);
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userSession.UserId + " is null."));

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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " is null."));
            return sessionDal.GetSessionUsers(idSession);
        }
    }
}
