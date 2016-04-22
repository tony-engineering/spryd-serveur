using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Password { get; set; }
        public SprydZone SprydZone { get; set; }
    }
}