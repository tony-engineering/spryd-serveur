using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Spryd.Serveur.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    public class UserController : ApiController
    {
        private IDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Tests constructor
        /// </summary>
        /// <param name="fakeDal"></param>
        public UserController(IDal fakeDal)
        {
            dal = fakeDal;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("User/{userId}")]
        [HttpGet]
        public User GetUser(int userId)
        {
            User user = dal.GetUserById(userId);
            if(user == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userId + " is null."));
            return dal.GetUserById(userId);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        [Route("User")]
        [HttpPost]
        public User AddUser([FromBody] User user)
        {
            CheckUser(user);
            dal.AddUser(user);

            return user;
        }

        /// <summary>
        /// Check user
        /// </summary>
        /// <param name="user"></param>
        private void CheckUser(User user)
        {
            if (user == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User is null."));
            if(String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "One or more missing parameters"));
        }
    }
}