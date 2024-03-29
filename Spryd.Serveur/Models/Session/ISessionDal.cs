﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spryd.Server.Models
{
    /// <summary>
    /// Interface de la couche d'accès aux données
    /// </summary>
    public interface ISessionDal
    {
        int AddSession(Session session);
        List<User> GetSessionAllUsers(int sessionId);
        Session GetSessionById(long sessionId);
        bool IsSessionExist(int idSession);
        bool IsAlreadySessionRunningInSprydZone(int sprydZoneId);
        bool IsSessionRunning(int idSession);
        void EndSession(int idSession);
        void GetUsersOutOfSession(int idSession);
        bool IsCreatorOfSession(int idSession, int idUser);
        void AddSharedItem(SharedItem sharedItem);
        List<SharedItem> GetSharedItems(int idSession);
        List<User> GetSessionUsers(int idSession);
        void GetInactiveUsersOutOfSession(int idSession);
        bool IsGoodPassword(int idSession, string password);
        bool IsSharedItemExist(int idSession, int idSharedItem);
        SharedItem GetSharedItemById(int idSession, int idSharedItem);
    }
}
