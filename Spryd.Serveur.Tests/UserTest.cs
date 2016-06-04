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
        private FakeDal dal;
        private UserController userController;

        /// <summary>
        /// Init test environement
        /// </summary>
        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            dal = new FakeDal();

            userController = new UserController(dal);
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// Get valid user
        /// </summary>
        [TestMethod]
        public void GetExistingUser_Success()
        {
            User newUser = new User() { Id = 1, Name = "Haroon", Surname = "ANWARBAIG", Email = "haroon@spryd.io", Password = "azerty" };
            dal.AddUser(newUser);
            Assert.AreEqual(userController.GetUser(1), newUser);
        }

        /// <summary>
        /// Add user without info
        /// throws exception because some attributes are imposed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void AddUserWihoutInfo_ThrowException()
        {
            User newUser = new User();
            userController.AddUser(newUser);
        }

        /// <summary>
        /// Get not existing user throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetNotExistingUser_ThrowsException()
        {
            userController.GetUser(-1);
        }

        /// <summary>
        /// Adds a user with no data, exception expected
        /// </summary>
        [TestMethod]
        public void AddUser_Success()
        {
            User newUser = new User("Spriiid", "Youre", "data@spryd.io", "superpwd");
            User addedUser = userController.AddUser(newUser);

            Assert.AreEqual(newUser.Id, 1);
        }

        /// <summary>
        /// Success to authenticate a User
        /// Gets an identifier and password in input
        /// Outputs the AuthenticationResult object
        /// </summary>
        [TestMethod]
        public void AuthenticateUser_Success()
        {
            string identifier = "data@spryd.io";
            string password = "superpwd";
            dal.AddUser(new User() { Email = identifier, Password = password });
            AuthenticationResult authResult = userController.Authenticate(new AuthentificationRequest(identifier, password));

            Assert.IsTrue(authResult.IsSuccess);
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

            AuthenticationResult authResult = userController.Authenticate(new AuthentificationRequest(identifier, password));

            Assert.IsFalse(authResult.IsSuccess);
        }
    }
}
