using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Interface de la couche d'accès aux données
    /// </summary>
    public interface IDal
    {
        User GetUserById(int id);
        void AddUser(User user);
        void AddSession(Session session);
        List<User> ListUsers();
        List<Beacon> GetBeacons();
    }
}
