using Spryd.Serveur.Models;
using System.Collections.Generic;

namespace Spryd.Serveur.Controllers
{
    public interface IBeaconDal
    {
        List<Beacon> GetBeacons();
        Beacon GetBeaconById(int id);
        Beacon GetBeaconByTechnicalId(string technicalId);
    }
}