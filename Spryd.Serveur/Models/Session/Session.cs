using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    public class Session
    {
        public Session()
        {

        }

        [JsonConstructor]
        public Session(string name, int sprydZoneId, string password = null, DateTime startDate = new DateTime())
        {
            Name = name;
            StartDate = startDate;
            Password = password;
            SprydZoneId = sprydZoneId;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("sprydZoneId")]
        public int SprydZoneId { get; set; }
    }
}