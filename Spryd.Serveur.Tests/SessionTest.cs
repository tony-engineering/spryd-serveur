using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Server.Controllers;
using Spryd.Serveur.Models;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;

namespace Spryd.Server.Tests
{
    [TestClass]
    public class SessionTest
    {
        private ISessionDal dal;
        private SessionController sessionController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["DatabaseAuthString"];
            dal = new SessionDal(connectionString);

            sessionController = new SessionController(dal);
            sessionController.Request = new HttpRequestMessage();
            sessionController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CreateSession_Success()
        {
            Session newSessionParameters = new Session("Session test", 1);

            long newSessionId = dal.AddSession(newSessionParameters);

            // TODO: getSessionByID + checkSession
        }
    }
}
