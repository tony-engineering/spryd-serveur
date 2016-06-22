using Spryd.Server.Models;
using System.Collections.Generic;

namespace Spryd.Server.Models
{
    public interface IBeaconDal
    {
        List<Beacon> GetBeacons();
        Beacon GetBeaconById(int id);
        Beacon GetBeaconByTechnicalId(string technicalId);
    }
}