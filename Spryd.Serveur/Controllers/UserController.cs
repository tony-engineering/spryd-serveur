
using log4net;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Spryd.Server.Controllers
{
    public class UserController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserController(ISprydContext context)
        {
            dal = new UserDal(context);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("user/{userId}")]
        [HttpGet]
        public User GetUser(int userId)
        {
            if (!dal.IsUserExist(userId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userId + " is null."));
                
            return dal.GetUserById(userId);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        [Route("user")]
        [HttpPost]
        public User AddUser([FromBody] User user)
        {
            ValidateUser(user);
            long newUserId = dal.AddUser(user);

            return dal.GetUserById((int) newUserId);
        }

        /// <summary>
        /// List Users
        /// </summary>
        [Route("user/all")]
        [HttpGet]
        public List<User> ListUsers()
        {
            List<User> users = dal.ListUsers();

            return users;
        }

        /// <summary>
        /// Authenticates a User
        /// </summary>
        /// <returns>The result of authentification</returns>
        [Route("user/authenticate")]
        [HttpPost]
        public AuthenticationResult Authenticate([FromBody] AuthentificationRequest authentificationRequest)
        {
            return dal.Authenticate(authentificationRequest);
        }

        /// <summary>
        /// Get current session of a user who joined it by his mobile application
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("user/{userId}/currentSession")]
        [HttpGet]
        public Session GetCurrentSession(int userId)
        {
            if(!dal.IsUserExist(userId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userId + " is null."));
            var currentSession = dal.GetCurrentSession(userId);
            if (currentSession == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userId + " has not join a session."));
            return currentSession;
        }

        /// <summary>
        /// Check user
        /// TODO: should this method stay here ?
        /// </summary>
        /// <param name="user"></param>
        private void ValidateUser(User user)
        {
            if(String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Surname))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "One or more missing parameters."));
            if(dal.IsUserExist(user.Email))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User " + user.Email + " already exist."));
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
        }
    }
}