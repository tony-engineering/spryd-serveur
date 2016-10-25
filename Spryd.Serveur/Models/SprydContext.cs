using Spryd.Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    public class SprydContext : DbContext, ISprydContext
    {
        public IDbSet<Beacon> Beacons { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Session> Sessions { get; set; }
        public IDbSet<SprydZone> SprydZones { get; set; }
        public IDbSet<SharedItem> SharedItems { get; set; }
        public IDbSet<UserSession> UserSession { get; set; }
    }
}