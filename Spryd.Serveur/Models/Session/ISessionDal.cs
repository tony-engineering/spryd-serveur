using System;
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
        List<User> GetSessionUsers(int sessionId);
        Session GetSessionById(long sessionId);
        bool IsSessionExist(int idSession);
        bool IsAlreadySessionRunningInSprydZone(int sprydZoneId);
        bool IsSessionRunning(int idSession);
        void EndSession(int idSession);
        void GetUsersOutOfSession(int idSession);
        bool IsCreatorOfSession(int idSession, int idUser);
        void AddSharedItem(SharedItem sharedItem);
    }
}
