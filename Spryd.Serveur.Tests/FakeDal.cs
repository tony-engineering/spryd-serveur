﻿using Spryd.Serveur.Models;
using System.Collections.Generic;
using System.Linq;
using Spryd.Server.Models;
using Spryd.Serveur.Controllers;
using System;

namespace Spryd.Serveur.Tests
{
    /// <summary>
    /// Couche d'accès aux données statics pour les tests unitaires
    /// </summary>
    public class FakeDal : IUserDal, IBeaconDal, ISessionDal, ISprydZoneDal
    {
        private List<User> listUsers;
		private List<Beacon> listBeacons;
        private List<Session> listSessions;
        private List<UserSession> listUserSessions;
        private List<SprydZone> listSprydZones;
        private List<SharedItem> listSharedItems;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public FakeDal()
        {
            listUsers = new List<User>();
            listBeacons = new List<Beacon>();
            listSessions = new List<Session>();
            listSharedItems = new List<SharedItem>();
            listSprydZones = new List<SprydZone>();
            listUserSessions = new List<UserSession>();
        }

        /// <summary>
        /// Ajouter un utilisateur
        /// </summary>
        /// <param name="user"></param>
        public long AddUser(User user)
        {
            if (listUsers.Count == 0)
                user.Id = 1;
            else
                user.Id = listUsers.Max(u => u.Id) + 1;
            listUsers.Add(user);
            return user.Id;
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
        public int AddSession(Session session)
        {
            if (listSessions.Count == 0)
                session.Id = 1;
            else
                session.Id = listSessions.Max(u => u.Id) + 1;
            listSessions.Add(session);
            return session.Id;
        }


        public bool IsUserExist(int id)
        {
            return listUsers.Any(u => u.Id == id);
        }

        public Session GetCurrentSession(int userId)
        {
            int? sessionId = listUserSessions.Where(u => u.UserId == userId && u.StartDate != null && u.EndDate == null).Select(u => u.SessionId).FirstOrDefault();
            if (sessionId == null)
                return null;
            return listSessions.Where(s => s.Id == sessionId).FirstOrDefault();
        }

        public void AddUserSession(UserSession userSession)
        {
            userSession.SessionId = userSession.Session.Id;
            listUserSessions.Add(userSession);
        }

        public bool IsUserInSession(int idSession, int idUser)
        {
            return listUserSessions.Any(us => us.UserId == idUser && us.SessionId == idUser && us.EndDate == null);
        }

        public User GetUserByIdPassword(string identifier, string password)
        {
            return listUsers.Where(u => u.Email == identifier && u.Password == password).FirstOrDefault();
        }

        public AuthenticationResult Authenticate(AuthentificationRequest authenticationRequest)
        {
            var user = GetUserByIdPassword(authenticationRequest.Identifier, authenticationRequest.Password);

            AuthenticationResult authResult = new AuthenticationResult();

            if (user == null)
                authResult.IsSuccess = false;
            else
            {
                authResult.User = user;
                authResult.IsSuccess = true;
            }

            return authResult;
        }

        public Beacon GetBeaconById(int id)
        {
            return listBeacons.Where(b => b.Id == id).FirstOrDefault();
        }

        public Beacon GetBeaconByTechnicalId(string technicalId)
        {
            return listBeacons.Where(b => b.TechnicalId == technicalId).FirstOrDefault();
        }

        public List<User> GetSessionUsers(int sessionId)
        {
            return (from user in listUsers
                    join userSession in listUserSessions on user.Id equals userSession.UserId
                    where userSession.SessionId == sessionId
                    select user).ToList();
        }

        public Session GetSessionById(long sessionId)
        {
            return listSessions.Where(s => s.Id == sessionId).FirstOrDefault();
        }

        public bool IsSessionExist(int idSession)
        {
            return listSessions.Any(s => s.Id == idSession);
        }

        public bool IsAlreadySessionRunningInSprydZone(int sprydZoneId)
        {
            return listSessions.Any(s => s.SprydZoneId == sprydZoneId && s.EndDate == null);
        }

        public bool IsSessionRunning(int idSession)
        {
            return listSessions.Any(s => s.Id == idSession && s.EndDate == null);
        }

        public List<SprydZone> GetNearbySprydZone(List<string> listBeaconsId)
        {
            if (listBeaconsId == null)
                return new List<SprydZone>();

            return (from sprydZone in listSprydZones
                    join beaconId in listBeaconsId on sprydZone.Beacon.TechnicalId equals beaconId
                    select sprydZone).ToList();
            
        }

        public List<SprydZone> GetAllSprydZones()
        {
            return listSprydZones;
        }

        public SprydZone GetSprydZoneById(int id)
        {
            return listSprydZones.Where(s => s.Id == id).FirstOrDefault();
        }

        public Session GetSprydZoneCurrentession(int zoneId)
        {
            return listSessions.Where(s => s.SprydZoneId == zoneId && s.StartDate != null && s.EndDate == null).FirstOrDefault();
        }

        public bool IsSprydZoneExist(int zoneId)
        {
            return listSprydZones.Any(s => s.Id == zoneId);
        }

        public void AddSprydZone(SprydZone sprydZone)
        {
            listSprydZones.Add(sprydZone);
        }

    }
}
