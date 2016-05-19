using MySql.Data.MySqlClient;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Spryd.Serveur.Models
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
            AuthenticationResult authResult = new AuthenticationResult();

            try
            {
                authResult.User = GetUserByIdPassword(authenticationRequest.Identifier, authenticationRequest.Password);
                authResult.IsSuccess = true;
            }
            catch(UserNotFoundException e)
            {
                authResult.IsSuccess = false;
            }

            return authResult;
        }

        /// <summary>
        /// Get user by mail and password
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private User GetUserByIdPassword(string identifier, string password)
        {
            using (DbConnection c = new DbConnection())
            {
                var user = c.Users.Where(u => u.Email == identifier && u.Password == password).FirstOrDefault();
                if (user == null)
                    throw new UserNotFoundException("User with identifier " + identifier + " and password password " + password + " not found.");
                return user;
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
                var user = c.Users.Where(u => u.Id == id).FirstOrDefault();
                if(user == null)
                    throw new UserNotFoundException("User with id " + id + " not found.");
                return user;
            }
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<User> ListUsers()
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Users.ToList();
            }
        }
    }
}