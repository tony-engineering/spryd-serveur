using Spryd.Server.Models;
using System.Collections.Generic;

namespace Spryd.Server.Controllers
{
    public interface IBeaconDal
    {
        List<Beacon> GetBeacons();
        Beacon GetBeaconById(int id);
        Beacon GetBeaconByTechnicalId(string technicalId);
    }
}