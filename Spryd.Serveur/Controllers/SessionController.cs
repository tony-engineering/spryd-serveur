﻿using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Spryd.Server.Controllers
{
    public class SessionController : ApiController
    {
        private ISessionDal sessionDal;
        private IUserDal userDal;
        private ISprydZoneDal sprydZoneDal;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionController()
        {
            sessionDal = new SessionDal();
            userDal = new UserDal();
            sprydZoneDal = new SprydZoneDal();
        }

        /// <summary>
        /// Constructor used for tests (possible to put a different DB)
        /// </summary>
        /// <param name="sessionFakeDal"></param>
        /// <param name="userFakeDal"></param>
        /// <param name="sprydZoneFakeDal"></param>
        public SessionController(ISessionDal sessionFakeDal, IUserDal userFakeDal, ISprydZoneDal sprydZoneFakeDal)
        {
            sessionDal = sessionFakeDal;
            userDal = userFakeDal;
            sprydZoneDal = sprydZoneFakeDal;
        }

        /// <summary>
        /// Create session
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [Route("session")]
        [HttpPost]
        public Session AddSession([FromBody] UserSession userSession)
        {
            ValidateUserSession(userSession);

            sessionDal.AddSession(userSession.Session); // Create the session    
            userSession.SessionId = userSession.Session.Id; // Get incremental session Id generated by the database        
            userDal.AddUserSession(userSession); // Add the creator to the session 

            return userSession.Session;
        }

        /// <summary>
        /// Validate user session informations before creating the session
        /// </summary>
        /// <param name="userSession"></param>
        private void ValidateUserSession(UserSession userSession)
        {
            // Check if the creator user exist
            if (!userDal.IsUserExist(userSession.UserId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + userSession.UserId + " does not exist."));

            // Check if the new session's sprydzone exist
            if (!sprydZoneDal.IsSprydZoneExist(userSession.Session.SprydZoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no existing Spryd Zone with ID : " + userSession.Session.SprydZoneId + "."));

            // Check if there is not already a running session in this spryd zone
            if (sessionDal.IsAlreadySessionRunningInSprydZone(userSession.Session.SprydZoneId))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is already a running session in Spryd Zone : " + userSession.Session.SprydZoneId + "."));

            userSession.Session.StartDate = DateTime.Now;
            userSession.LastActivity = DateTime.Now;
            userSession.StartDate = DateTime.Now;
            userSession.IsCreator = true;
        }

        /// <summary>
        /// Get session users
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        [Route("session/{idSession}/users")]
        [HttpGet]
        public List<User> GetSessionUsers(int idSession)
        {
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));
            var listUsers = sessionDal.GetSessionUsers(idSession);

            if (listUsers.IsNullOrEmpty()) // s'il n'y a aucun
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, "There is no users in session " + idSession));
            return sessionDal.GetSessionUsers(idSession);
        }

        /// <summary>
        /// Join a session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [Route("session/{idSession}/user/{idUser}/join")]
        [HttpPost]
        public Session JoinSession(int idSession, int idUser)
        {
            CheckJoiningUserAndSession(idSession, idUser);

            var userSession = new UserSession()
            {
                UserId = idUser,
                SessionId = idSession,
                IsCreator = false,
                StartDate = DateTime.Now,
                LastActivity = DateTime.Now
            };

            userDal.AddUserSession(userSession);
            return sessionDal.GetSessionById(idSession);
        }

        /// <summary>
        /// User leave the session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        [Route("session/{idSession}/user/{idUser}/leave")]
        [HttpPost]
        public Session LeaveSession(int idSession, int idUser)
        {
            CheckLeavingUserAndSession(idSession, idUser);
            userDal.EndUserSession(idUser, idSession);
            IfLeavingUserIsCreator_EndSession(idSession, idUser);

            return sessionDal.GetSessionById(idSession);
        }

        /// <summary>
        /// If user is the session's creator, it ends the session of all participants and end the session
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        private void IfLeavingUserIsCreator_EndSession(int idSession, int idUser)
        {
            if (sessionDal.IsCreatorOfSession(idSession, idUser))
            {
                sessionDal.GetUsersOutOfSession(idSession);
                sessionDal.EndSession(idSession);
            }
        }

        /// <summary>
        /// End the session
        /// </summary>
        /// <param name="idSession"></param>
        [Route("session/{idSession}/end")]
        [HttpPost]
        public Session EndSession(int idSession)
        {
            IsSessionRunning(idSession);
            sessionDal.GetUsersOutOfSession(idSession);
            sessionDal.EndSession(idSession);

            return sessionDal.GetSessionById(idSession);
        }

        /// <summary>
        /// Check if session is running
        /// </summary>
        /// <param name="idSession"></param>
        private void IsSessionRunning(int idSession)
        {
            // Check if the session exist
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));
            // Check if session is still going on
            if (!sessionDal.IsSessionRunning(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "Session " + idSession + " is already over."));
        }

        /// <summary>
        /// Check if this session is available and if this user can join it
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        private void CheckJoiningUserAndSession(int idSession, int idUser)
        {
            // Check if the session exist
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));

            // Check if the creator user exist
            if (!userDal.IsUserExist(idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + idUser + " does not exist."));

            // Check if the user has already join this session
            if (userDal.IsUserInSession(idSession, idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "User " + idUser + " already join session " + idSession + "."));

            // Check if session is still going on
            if (!sessionDal.IsSessionRunning(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "Session " + idSession + " is over."));
        }

        /// <summary>
        /// Check if the session is still available and user info
        /// </summary>
        /// <param name="idSession"></param>
        /// <param name="idUser"></param>
        private void CheckLeavingUserAndSession(int idSession, int idUser)
        {
            // Check if the session exist
            if (!sessionDal.IsSessionExist(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Session " + idSession + " does not exist."));

            // Check if the creator user exist
            if (!userDal.IsUserExist(idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User " + idUser + " does not exist."));

            // Check if the user is still in this session
            if (!userDal.IsUserInSession(idSession, idUser))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "User " + idUser + " is not in session " + idSession + "."));

            // Check if session is still going on
            if (!sessionDal.IsSessionRunning(idSession))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "Session " + idSession + " is over."));
        }

        /// <summary>
        /// Share an item in this session
        /// </summary>
        /// <returns></returns>
        [Route("session/{idSession}/sharedItem")]
        [HttpPost]
        public HttpResponseMessage AddSharedItem(int idSession)
        {
            IsSessionRunning(idSession);

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest,"No files received.");

            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var filePath = WebApiConfig.SharedItemsRepository + postedFile.FileName;
                postedFile.SaveAs(filePath);
                
                sessionDal.AddSharedItem(new SharedItem()
                {
                    CreateDate = DateTime.Now,
                    Path = filePath,
                    Text = postedFile.FileName,
                    SessionId = idSession
                });
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
