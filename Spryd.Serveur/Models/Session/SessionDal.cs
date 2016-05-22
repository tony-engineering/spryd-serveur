﻿using MySql.Data.MySqlClient;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Data access layer for Session
    /// </summary>
    public class SessionDal : ISessionDal
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionDal()
        {
        }

        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="session"></param>
        public long AddSession(Session session)
        {
            using (DbConnection c = new DbConnection())
            {
                c.Sessions.Add(session);
                c.SaveChanges();
                return session.Id;
            }
        }

        /// <summary>
        /// Get session by Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Session GetSessionById(long sessionId)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Sessions.Where(s =>s.Id == sessionId).FirstOrDefault() ;
            }
        }

        /// <summary>
        /// Get session users
        /// </summary>
        /// <returns></returns>
        public List<User> GetSessionUsers(int sessionId)
        {
            using (DbConnection c = new DbConnection())
            {
                return (from user in c.Users
                 join userSession in c.UserSession on user.Id equals userSession.UserId
                 where userSession.SessionId == sessionId
                 select user).ToList();
            }
        }

        /// <summary>
        /// Indicate if the session exist
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public bool IsSessionExist(int idSession)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Sessions.Any(s => s.Id == idSession);
            }
        }
    }
}