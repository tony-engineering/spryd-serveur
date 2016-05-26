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
        List<SprydZone> GetNearbySprydZone(List<string> listBeaconsId);
        List<SprydZone> GetAllSprydZones();
        SprydZone GetSprydZoneById(int id);
        Session GetSprydZoneCurrentession(int zoneId);
        bool IsSprydZoneExist(int zoneId);
    }
}
