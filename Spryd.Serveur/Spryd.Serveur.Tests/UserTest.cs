using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spryd.Serveur.Models;
using Spryd.Serveur.Controllers;
using System.Web.Http;

namespace Spryd.Serveur.Tests
{
    [TestClass]
    public class UserTest
    {
        private IDal dal;
        private UserController userController;

        [TestInitialize]
        public void FillUserList()
        {
            dal = new FakeDal();
            userController = new UserController(dal);            
        }

        [TestMethod]
        public void GetExistingUser_Success()
        {
            User newUser = new User() { Id = 1, Name = "Haroon", Surname = "ANWARBAIG", Email = "haroon@spryd.io", Password = "azerty" };
            dal.AddUser(newUser);
            Assert.AreEqual(userController.GetUser(1), newUser);
        }

        [TestMethod]
        public void GetNotExistingUser_Fail()
        {
            //var response = new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            //Assert.AreEqual(userController.GetUser(1), response);
            // à continuer
        }
    }
}
