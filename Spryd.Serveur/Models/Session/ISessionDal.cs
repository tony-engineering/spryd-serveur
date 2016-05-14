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
    public interface ISessionDal
    {
        void AddSession(Session session);
    }
}
