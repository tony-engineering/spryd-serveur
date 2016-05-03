using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    public class Beacon
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("technical_id")]
        public string TechnicalId { get; set; }
    }
}