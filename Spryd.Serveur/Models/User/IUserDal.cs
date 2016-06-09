
using Spryd.Server.Models;
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
    public interface IUserDal
    {
        User GetUserById(int id);
        bool IsUserExist(int id);
        long AddUser(User user);
        List<User> ListUsers();
        AuthenticationResult Authenticate(AuthentificationRequest authenticationRequest);
        User GetUserByIdPassword(string identifier, string password);
        Session GetCurrentSession(int userId);
        void AddUserSession(UserSession userSession);
        bool IsUserInSession(int idSession, int idUser);
        void EndUserSession(int idUser, int idSession);
        bool IsUserExist(string email);
    }
}
