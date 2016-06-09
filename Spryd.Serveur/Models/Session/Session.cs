using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    [Table("session")]
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

        [Column("start_date")]
        [JsonIgnore]
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        [JsonIgnore]
        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [Column("spryd_zone_id")]
        [JsonProperty("sprydZoneId")]
        public int SprydZoneId { get; set; }
    }
}