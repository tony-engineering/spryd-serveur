using log4net;
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
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISprydContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionDal(ISprydContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="session"></param>
        public int AddSession(Session session)
        {
            try
            {
                _context.Sessions.Add(session);
                _context.SaveChanges();
                return session.Id;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }

        }

        /// <summary>
        /// Add a shared item to the session
        /// </summary>
        /// <param name="sharedItem"></param>
        public void AddSharedItem(SharedItem sharedItem)
        {
            try
            {
                _context.SharedItems.Add(sharedItem);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        /// <summary>
        /// End session
        /// </summary>
        /// <param name="idSession"></param>
        public void EndSession(int idSession)
        {
            try
            {
                var session = _context.Sessions.Where(s => s.Id == idSession).FirstOrDefault();
                if (session == null)
                    return;
                session.EndDate = DateTime.Now;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        /// <summary>
        /// Get session by Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Session GetSessionById(long sessionId)
        {
            try
            {
                return _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Get session all users
        /// </summary>
        /// <returns></returns>
        public List<User> GetSessionAllUsers(int sessionId)
        {
            try
            {
                return (from user in _context.Users
                        join userSession in _context.UserSession on user.Id equals userSession.UserId
                        where userSession.SessionId == sessionId
                        && userSession.EndDate == null
                        select user).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Get items shared in a session
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public List<SharedItem> GetSharedItems(int idSession)
        {
            try
            {
                return _context.SharedItems.Where(s => s.SessionId == idSession).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Before ending a session, this method end user's current session by sending userSession endDate to Now
        /// </summary>
        /// <param name="idSession"></param>
        public void GetUsersOutOfSession(int idSession)
        {
            try
            {
                _context.UserSession.Where(us => us.SessionId == idSession && us.EndDate == null).ToList().ForEach(u => u.EndDate = DateTime.Now);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        /// <summary>
        /// Indicates if there is already a session running in this sprydZone
        /// </summary>
        /// <param name="sprydZoneId"></param>
        /// <returns></returns>
        public bool IsAlreadySessionRunningInSprydZone(int sprydZoneId)
        {
            try
            {
                return _context.Sessions.Any(s => s.SprydZoneId == sprydZoneId && s.EndDate == null);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Indicate if this user is the creator of this session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public bool IsCreatorOfSession(int idSession, int idUser)
        {
            try
            {
                return _context.UserSession.Any(us => us.SessionId == idSession && us.UserId == idUser && us.IsCreator == true);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Indicate if the session exist
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public bool IsSessionExist(int idSession)
        {
            try
            {
                return _context.Sessions.Any(s => s.Id == idSession);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Indicate if the session is still running
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public bool IsSessionRunning(int idSession)
        {
            try
            {
                return _context.Sessions.Any(s => s.Id == idSession && s.EndDate == null);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Get users active since the last 60 secondes
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public List<User> GetSessionUsers(int idSession)
        {
            try
            {
                return (from user in _context.Users
                        join userSession in _context.UserSession on user.Id equals userSession.UserId
                        where userSession.SessionId == idSession
                        && userSession.EndDate == null
                        select user).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Get inactive users out of session
        /// Inactive users : lastActivity is more than 1 minute ago
        /// If creator is inactive --> end session
        /// </summary>
        /// <param name="idSession"></param>
        public void GetInactiveUsersOutOfSession(int idSession)
        {
            try
            {
                var limitDelayActivity = DateTime.Now.AddMinutes(-1);
                var isCreatorInactive = _context.UserSession.Any(us => us.SessionId == idSession && us.EndDate == null && us.LastActivity < limitDelayActivity && us.IsCreator == true);
                if (isCreatorInactive) // if creator inactive, end session 
                {
                    GetUsersOutOfSession(idSession);
                    EndSession(idSession);
                }
                else // else, get inactive users out of session
                {
                    _context.UserSession.Where(us => us.SessionId == idSession && us.EndDate == null && us.LastActivity < limitDelayActivity && us.IsCreator == false)
                        .ToList()
                        .ForEach(u => u.EndDate = DateTime.Now);

                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Check if password mathes session password
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsGoodPassword(int idSession, string password)
        {
            try
            {
                return _context.Sessions.Any(s => s.Id == idSession && s.Password == password);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Check if this shared item exist in this session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idSharedItem"></param>
        /// <returns></returns>
        public bool IsSharedItemExist(int idSession, int idSharedItem)
        {
            try
            {
                return _context.SharedItems.Any(s => s.SessionId == idSession && s.Id == idSharedItem);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Get shared item in a session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idSharedItem"></param>
        /// <returns></returns>
        public SharedItem GetSharedItemById(int idSession, int idSharedItem)
        {
            try
            {
                return _context.SharedItems.Where(s => s.SessionId == idSession && s.Id == idSharedItem).FirstOrDefault() ;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }
    }
}