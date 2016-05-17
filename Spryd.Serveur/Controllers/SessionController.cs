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
            dal = new SessionDal(WebApiConfig.connectionString);
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
        [Route("Session")]
        [HttpPost]
        public Session AddSession([FromBody] Session session)
        {
            long newSessionId = dal.AddSession(session);

            // TODO: remplacer session par dal.getSessionById();
            return session;
        }
    }
}
