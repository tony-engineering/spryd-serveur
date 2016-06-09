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

namespace Spryd.Server.Models
{
    /// <summary>
    /// Data access layer for User
    /// </summary>
    public class UserDal : IUserDal
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserDal()
        {
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public long AddUser(User user)
        {
            using (DbConnection c = new DbConnection())
            {
                c.Users.Add(user);
                c.SaveChanges();
                return user.Id;
            }
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
            using (DbConnection c = new DbConnection())
            {
                return c.Users.Where(u => u.Email == identifier && u.Password == password).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Users.Where(u => u.Id == id).FirstOrDefault(); ;
            }
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <returns></returns>
        public List<User> ListUsers()
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Users.ToList();
            }
        }

        /// <summary>
        /// Get current session join by the mobile application
        /// A current session is identified when there is a StartDate but no EndDate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Session GetCurrentSession(int userId)
        {
            using (DbConnection c = new DbConnection())
            {
                int? sessionId = c.UserSession.Where(u => u.UserId == userId && u.StartDate != null && u.EndDate == null).Select(u => u.SessionId).FirstOrDefault();
                if(sessionId == null)
                    return null;
                return c.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Indicate if user exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserExist(int id)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Users.Any(u => u.Id == id);
            }
        }

        /// <summary>
        /// Add a user in a session
        /// </summary>
        /// <param name="userSession"></param>
        public void AddUserSession(UserSession userSession)
        {
            using (DbConnection c = new DbConnection())
            {
                c.UserSession.Add(userSession);
                c.SaveChanges();
            }
        }

        /// <summary>
        /// End the user session by setting the user session date at now
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idSession"></param>
        public void EndUserSession(int idUser, int idSession)
        {
            using (DbConnection c = new DbConnection())
            {
                var userSessionToEnd = c.UserSession.Where(us => us.UserId == idUser && us.SessionId == idSession).FirstOrDefault();
                if (userSessionToEnd == null)
                    return;
                userSessionToEnd.EndDate = DateTime.Now;
                c.SaveChanges();
            }
        }

        /// <summary>
        /// Indicate if the user is already in the session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public bool IsUserInSession(int idSession, int idUser)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.UserSession.Any(us => us.UserId == idUser && us.SessionId == idSession && us.EndDate == null);
            }
        }
    }
}