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
    /// Contrôleur d'un utilisateur
    /// </summary>
    public class UserController : ApiController
    {
        private IDal dal;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public UserController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Constructeur pour les tests
        /// </summary>
        /// <param name="fakeDal"></param>
        public UserController(IDal fakeDal)
        {
            dal = fakeDal;
        }

        /// <summary>
        /// Récupère un utilisateur par son ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("User/{userId}")]
        [HttpGet]
        public User GetUser(int userId)
        {
            User user = dal.GetUserById(userId);
            if(user == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Le user " + userId + " n'existe pas."));
            return dal.GetUserById(userId);
        }

        /// <summary>
        /// Ajoute un utilisateur
        /// </summary>
        /// <param name="user"></param>
        [Route("User")]
        [HttpPost]
        public void AddUser([FromBody] User user)
        {
            CheckUser(user);
            dal.AddUser(user);
        }

        /// <summary>
        /// Validation de l'utilisateur
        /// </summary>
        /// <param name="user"></param>
        private void CheckUser(User user)
        {
            if (user == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Aucun paramètre n'a été reçu."));
            if(String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Paramètres indispensable manquant."));
        }
    }
}