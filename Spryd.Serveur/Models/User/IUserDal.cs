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
    public interface IUserDal
    {
        User GetUserById(int id);
        long AddUser(User user);
        List<User> ListUsers();
        AuthenticationResult Authenticate(string identifier, string password);
    }
}
