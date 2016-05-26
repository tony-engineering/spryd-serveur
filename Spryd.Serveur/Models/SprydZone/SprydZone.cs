using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [Column("beacon_id")]
        [JsonProperty("beacon")]
        public Beacon Beacon { get; set; }
    }
}