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

        /// <summary>
        /// Méthode lancée au début de chaque test pour initialiser le controlleur et la DAL
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
        /// Ajout et récupération d'un utilisateur complet
        /// </summary>
        [TestMethod]
        public void GetExistingUser_Success()
        {
            User newUser = new User() { Id = 1, Name = "Haroon", Surname = "ANWARBAIG", Email = "haroon@spryd.io", Password = "azerty" };
            dal.AddUser(newUser);
            Assert.AreEqual(userController.GetUser(1), newUser);
        }

        /// <summary>
        /// Ajoute un utilisateur sans donnée
        /// Jète une exception car Email et Password sont obligatoire
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void AddUserWihoutInfo_ThrowException()
        {
            User newUser = new User();
            userController.AddUser(newUser);
        }

        /// <summary>
        /// Récupère un user inexistant et renvoi une exception not found
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetNotExistingUser_ThrowsException()
        {
            userController.GetUser(1);
        }

    }
}
