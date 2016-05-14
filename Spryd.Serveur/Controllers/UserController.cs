﻿using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Spryd.Serveur.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    public class UserController : ApiController
    {
        private IUserDal dal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserController()
        {
            dal = new UserDal(WebApiConfig.connectionString);
        }

        /// <summary>
        /// Tests constructor
        /// </summary>
        /// <param name="fakeDal"></param>
        public UserController(IUserDal fakeDal)
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
            long newUserId = dal.AddUser(user);

            return dal.GetUserById((int) newUserId);
        }

        /// <summary>
        /// List Users
        /// </summary>
        /// <param name="user"></param>
        [Route("User/all")]
        [HttpGet]
        public List<User> ListUsers()
        {
            List<User> users = dal.ListUsers();

            return users;
        }

        /// <summary>
        /// Check user
        /// </summary>
        /// <param name="user"></param>
        private void CheckUser(User user)
        {
            if(String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Surname))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "One or more missing parameters."));
        }
    }
}