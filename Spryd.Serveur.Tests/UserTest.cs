using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Serveur.Models;
using Spryd.Serveur.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace Spryd.Serveur.Tests
{
    [TestClass]
    public class UserTest
    {
        private IUserDal dal;
        private UserController userController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["DatabaseAuthString"];
            dal = new UserDal(connectionString);

            userController = new UserController(dal);
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// Gets a user from database and checks if it is valid
        /// </summary>
        [TestMethod]
        public void GetExistingUser_Success()
        {
            User gotUser = userController.GetUser(1);

            Assert.IsTrue(gotUser.IsValid());
        }

        /// <summary>
        /// Checks if all users in database are valid
        /// </summary>
        [TestMethod]
        public void AllUsersAreValid_Success()
        {
            List<User> users = dal.ListUsers();

            users.ForEach(user => {
                if (!user.IsValid())
                    throw new NotValidUserException("User "+user+" is not valid.");
                }
            );
        }


        /// <summary>
        /// Adds a user with no data, exception expected
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void AddUserWihoutInfo_ThrowException()
        {
            User newUser = new User();
            userController.AddUser(newUser);
        }

        /// <summary>
        /// Adds a user with no data, exception expected
        /// </summary>
        [TestMethod]
        public void AddUser_Success()
        {
            User newUser = new User("Spriiid", "Youre", "data@spryd.io", "superpwd");
            User addedUser = userController.AddUser(newUser);

            Assert.IsTrue(addedUser.IsValid());
        }

        /// <summary>
        /// Tries to get a non existing user
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void GetNotExistingUser_ThrowsException()
        {
            userController.GetUser(-1);
        }

        /// <summary>
        /// Success to authenticate a User
        /// Gets an identifier and password in input
        /// Outputs the AuthenticationResult object
        /// </summary>
        [TestMethod]
        public void AuthenticateUser_Success()
        {
            User authenticatedUser = null;
            string identifier = "data@spryd.io";
            string password = "superpwd";

            AuthenticationResult authResult = dal.Authenticate(identifier, password);
            if (authResult.IsSuccess)
                authenticatedUser = authResult.User;

            Assert.IsTrue(authenticatedUser.IsValid());
        }

        /// <summary>
        /// Fails to authenticate a User
        /// Gets an identifier and password in input
        /// Outputs the AuthenticationResult object
        /// </summary>
        [TestMethod]
        public void AuthenticateUser_Fail()
        {
            string identifier = "data@spryd.io";
            string password = "superpwd_fail";

            AuthenticationResult authResult = dal.Authenticate(identifier, password);

            Assert.IsFalse(authResult.IsSuccess);
        }
    }
}
