using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    public class SprydZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Beacon Beacon { get; set; }
    }
}