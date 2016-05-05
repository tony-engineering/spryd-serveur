using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Serveur.Models;
using Spryd.Serveur.Controllers;
using System.Web.Http;
using System.Net.Http;

namespace Spryd.Serveur.Tests
{
    [TestClass]
    public class UserTest
    {
        private IDal dal;
        private UserController userController;

        [TestInitialize]
        public void InitializeTestingEnvironnement()
        {
            dal = new FakeDal();
            userController = new UserController(dal);
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        [TestMethod]
        public void GetExistingUser_Success()
        {
            User newUser = new User() { Id = 1, Name = "Haroon", Surname = "ANWARBAIG", Email = "haroon@spryd.io", Password = "azerty" };
            dal.AddUser(newUser);
            Assert.AreEqual(userController.GetUser(1), newUser);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetNotExistingUser_ThrowsException()
        {
            //Récupère un user inexistant et renvoi une exception not found
            userController.GetUser(1);
        }
    }
}
