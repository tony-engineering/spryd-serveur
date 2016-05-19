using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    public class DbConnection : DbContext
    {
        public DbSet<Beacon> Beacons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SprydZone> SprydZones { get; set; }
        public DbSet<SharedItem> SharedItems { get; set; }
    }
}