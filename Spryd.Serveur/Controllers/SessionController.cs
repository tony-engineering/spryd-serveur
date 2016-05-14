using Spryd.Serveur;
using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
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
        /// Create session
        /// </summary>
        /// <param name="session"></param>
        [Route("Session")]
        [HttpPost]
        public Session AddSession([FromBody] Session session)
        {
            dal.AddSession(session);

            return session;
        }
    }
}
