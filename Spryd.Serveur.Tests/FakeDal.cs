using Spryd.Server.Models;
using System.Collections.Generic;
using System.Linq;
using Spryd.Server.Controllers;
using System;

namespace Spryd.Server.Tests
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
            var userSession = listUserSessions.Where(u => u.UserId == userId && u.StartDate != null && u.EndDate == null).FirstOrDefault();
            if (userSession == null)
                return null;
            return userSession.Session;
        }

        public void AddUserSession(UserSession userSession)
        {
            // get session info
            if (userSession.SessionId == 0)
                userSession.SessionId = GetSessionById(userSession.Session.Id).Id;
            else if (userSession.Session == null)
                userSession.Session = GetSessionById(userSession.SessionId);

            //Get new ID user session
            if (listUserSessions.Count == 0)
                userSession.Id = 1;
            else
                userSession.Id = listUserSessions.Max(u => u.Id) + 1;

            listUserSessions.Add(userSession);
        }

        public bool IsUserInSession(int idSession, int idUser)
        {
            return listUserSessions.Any(us => us.UserId == idUser && us.Session.Id == idSession && us.EndDate == null);
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
            if (listSprydZones.Count == 0)
                sprydZone.Id = 1;
            else
                sprydZone.Id = listSprydZones.Max(u => u.Id) + 1;

            listSprydZones.Add(sprydZone);
        }

        public void EndSession(int idSession)
        {
            var session = listSessions.Where(s => s.Id == idSession).FirstOrDefault();
            if (session == null)
                return;
            session.EndDate = DateTime.Now;
        }

        public void GetUsersOutOfSession(int idSession)
        {
            listUserSessions.Where(us => us.Session.Id == idSession && us.EndDate == null).ToList().ForEach(u => u.EndDate = DateTime.Now);
        }

        public void EndUserSession(int idUser, int idSession)
        {
            var userSessionToEnd = listUserSessions.Where(us => us.UserId == idUser && us.SessionId == idSession && us.EndDate == null)
                .OrderByDescending(us => us.Id)
                .FirstOrDefault();
            if (userSessionToEnd == null)
                return;
            userSessionToEnd.EndDate = DateTime.Now;
        }

        public bool IsCreatorOfSession(int idSession, int idUser)
        {
            return listUserSessions.Any(us => us.SessionId == idSession && us.UserId == idUser && us.IsCreator == true);
        }

        public bool IsUserExist(string email)
        {
            return listUsers.Any(u => u.Email == email);
        }

        public void AddSharedItem(SharedItem sharedItem)
        {
            if (listSharedItems.Count == 0)
                sharedItem.Id = 1;
            else
                sharedItem.Id = listSharedItems.Max(u => u.Id) + 1;

            listSharedItems.Add(sharedItem);
        }
    }
}
