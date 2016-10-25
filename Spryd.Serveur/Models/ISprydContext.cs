using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spryd.Server.Models
{
    public interface ISprydContext : IDisposable
    {
        IDbSet<Beacon> Beacons { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<Session> Sessions { get; set; }
        IDbSet<SprydZone> SprydZones { get; set; }
        IDbSet<SharedItem> SharedItems { get; set; }
        IDbSet<UserSession> UserSession { get; set; }

        int SaveChanges();
    }
}
