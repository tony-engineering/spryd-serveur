using Newtonsoft.Json;
using Spryd.Serveur.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    [Table("user_session")]
    public class UserSession
    {
        [JsonIgnore]
        [JsonProperty("id")]
        [Column("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        [Column("user_id")]
        public int UserId { get; set; }

        [JsonProperty("sessionId")]
        [Column("session_id")] 
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Session { get; set; }

        [JsonIgnore]
        [JsonProperty("isCreator")]
        [Column("is_creator")]
        public bool IsCreator { get; set; }

        [JsonIgnore]
        [JsonProperty("startDate")]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [JsonIgnore]
        [JsonProperty("endDate")]
        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        [JsonProperty("lastActivity")]
        [Column("last_activity")]
        public DateTime LastActivity { get; set; }
    }
}