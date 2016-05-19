using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spryd.Server.Models
{
    public interface ISprydZoneDal
    {
        List<SprydZone> GetNearbySprydZone(List<Beacon> listBeacons);
    }
}
