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
        private ISessionDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionController()
        {
            dal = new SessionDal();
        }

        /// <summary>
        /// Constructor used for tests (possible to put a different DB)
        /// </summary>
        /// <param name="connectionString"></param>
        public SessionController(ISessionDal testDal)
        {
            dal = testDal;
        }

        /// <summary>
        /// Create session
        /// </summary>
        /// <param name="session"></param>
        [Route("session")]
        [HttpPost]
        public Session AddSession([FromBody] Session session)
        {
            long newSessionId = dal.AddSession(session);
            return dal.GetSessionById(newSessionId);
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
            if (!dal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " is null."));
            return dal.GetSessionUsers(idSession);
        }
    }
}
