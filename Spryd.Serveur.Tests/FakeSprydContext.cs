using Spryd.Server.Models;
using Spryd.Server.Tests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spryd.Server.Tests
{
    public class FakeSprydContext : ISprydContext
    {
        public FakeSprydContext()
        {
            Beacons = new FakeDbSet<Beacon>();
            Users = new FakeDbSet<User>();
            Sessions = new FakeDbSet<Session>();
            SprydZones = new FakeDbSet<SprydZone>();
            SharedItems = new FakeDbSet<SharedItem>();
            UserSession = new FakeDbSet<UserSession>();
        }

        public IDbSet<Beacon> Beacons { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Session> Sessions { get; set; }
        public IDbSet<SprydZone> SprydZones { get; set; }
        public IDbSet<SharedItem> SharedItems { get; set; }
        public IDbSet<UserSession> UserSession { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
