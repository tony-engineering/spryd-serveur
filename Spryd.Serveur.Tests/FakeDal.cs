﻿
using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spryd.Server.Models;

namespace Spryd.Serveur.Tests
{
    /// <summary>
    /// Couche d'accès aux données statics pour les tests unitaires
    /// </summary>
    public class FakeDal : IUserDal
    {
        private List<User> listUsers;
		private List<Beacon> listBeacons;
        private List<Session> listSessions;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public FakeDal()
        {
            listUsers = new List<User>();
            listBeacons = new List<Beacon>();
            listSessions = new List<Session>();
        }

        /// <summary>
        /// Ajouter un utilisateur
        /// </summary>
        /// <param name="user"></param>
        public long AddUser(User user)
        {
            listUsers.Add(user);
            return 1;
        }

        public List<Beacon> GetBeacons()
        {
            return listBeacons;
        }

        /// <summary>
        /// Récupérer un utilisateur par son ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            return listUsers.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Récupère la liste de tous les utilisateurs
        /// </summary>
        /// <returns></returns>
        public List<User> ListUsers()
        {
            return listUsers;
        }

        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="user"></param>
        public void AddSession(Session session)
        {
            listSessions.Add(session);
        }

        public User Authenticate(string identifier, string password)
        {
            throw new NotImplementedException();
        }

        AuthenticationResult IUserDal.Authenticate(AuthentificationRequest authenticationRequest)
        {
            throw new NotImplementedException();
        }

        public bool IsUserExist(int id)
        {
            throw new NotImplementedException();
        }

        public Session GetCurrentSession(int userId)
        {
            throw new NotImplementedException();
        }

        public void AddUserSession(UserSession userSession)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInSession(int idSession, int idUser)
        {
            throw new NotImplementedException();
        }
    }
}
