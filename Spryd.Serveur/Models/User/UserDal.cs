using MySql.Data.MySqlClient;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Configuration;
using log4net;

namespace Spryd.Server.Models
{
    /// <summary>
    /// Data access layer for User
    /// </summary>
    public class UserDal : IUserDal
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISprydContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserDal(ISprydContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public long AddUser(User user)
        {

            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
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

        /// <summary>
        /// Get user by mail and password
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByIdPassword(string identifier, string password)
        {
            return _context.Users.Where(u => u.Email == identifier && u.Password == password).FirstOrDefault();
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <returns></returns>
        public List<User> ListUsers()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Get current session join by the mobile application
        /// A current session is identified when there is a StartDate but no EndDate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Session GetCurrentSession(int userId)
        {
            int? sessionId = _context.UserSession.Where(u => u.UserId == userId && u.StartDate != null && u.EndDate == null).Select(u => u.SessionId).FirstOrDefault();
            if (sessionId == null)
                return null;
            return _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
        }

        /// <summary>
        /// Indicate if user exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserExist(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        /// <summary>
        /// Add a user in a session
        /// </summary>
        /// <param name="userSession"></param>
        public void AddUserSession(UserSession userSession)
        {
            _context.UserSession.Add(userSession);
            _context.SaveChanges();
        }

        /// <summary>
        /// End the user session by setting the user session date at now
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idSession"></param>
        public void EndUserSession(int idUser, int idSession)
        {
            var userSessionToEnd =
                _context.UserSession
                .Where(us => us.UserId == idUser && us.SessionId == idSession && us.EndDate == null)
                .OrderByDescending(us => us.Id)
                .FirstOrDefault(); // get the last session joined
            if (userSessionToEnd == null)
                return;
            userSessionToEnd.EndDate = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// Indicate if the user is already in the session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public bool IsUserInSession(int idSession, int idUser)
        {
            return _context.UserSession.Any(us => us.UserId == idUser && us.SessionId == idSession && us.EndDate == null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUserExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        /// <summary>
        /// Update user last activity
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        public void UpdateUserLastActivity(int idSession, int idUser)
        {
            var userSession = _context.UserSession
                .Where(u => u.UserId == idUser && u.SessionId == idSession)
                .OrderByDescending(u => u.Id)
                .FirstOrDefault();
            userSession.LastActivity = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// Indique si l'utilisateur est toujours dans une session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public bool IsAllActivitiesEnded(int idSession, int idUser)
        {
            return _context.UserSession.ToList().FindAll(us => us.UserId == idUser).All(us =>
                                    us.Session.Id == idSession
                                    && us.EndDate != null);
        }
    }
}