using MySql.Data.MySqlClient;
using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
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
        public int AddSession(Session session)
        {
            using (DbConnection c = new DbConnection())
            {
                c.Sessions.Add(session);
                c.SaveChanges();
                return session.Id;
            }
        }

        /// <summary>
        /// End session
        /// </summary>
        /// <param name="idSession"></param>
        public void EndSession(int idSession)
        {
            using (DbConnection c = new DbConnection())
            {
                var session = c.Sessions.Where(s => s.Id == idSession).FirstOrDefault();
                if (session == null)
                    return;
                session.EndDate = DateTime.Now;
                c.SaveChanges();
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
        /// Before ending a session, this method end user's current session by sending userSession endDate to Now
        /// </summary>
        /// <param name="idSession"></param>
        public void GetUsersOutOfSession(int idSession)
        {
            using (DbConnection c = new DbConnection())
            {
                c.UserSession.Where(us => us.SessionId == idSession && us.EndDate == null).ToList().ForEach(u => u.EndDate = DateTime.Now);
                c.SaveChanges();
            }            
        }

        /// <summary>
        /// Indicates if there is already a session running in this sprydZone
        /// </summary>
        /// <param name="sprydZoneId"></param>
        /// <returns></returns>
        public bool IsAlreadySessionRunningInSprydZone(int sprydZoneId)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Sessions.Any(s => s.SprydZoneId == sprydZoneId && s.EndDate == null);
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

        /// <summary>
        /// Indicate if the session is still running
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public bool IsSessionRunning(int idSession)
        {
            using (DbConnection c = new DbConnection())
            {
                return c.Sessions.Any(s => s.Id == idSession && s.EndDate == null);
            }
        }
    }
}