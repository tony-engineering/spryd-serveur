using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    [Table("spryd_zone")]
    public class SprydZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Beacon Beacon { get; set; }
    }
}