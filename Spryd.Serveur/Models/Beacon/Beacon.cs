using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    [Table("beacon")]
    public class Beacon
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [Column("technical_id")]
        [JsonProperty("technicalId")]
        public string TechnicalId { get; set; }
    }
}