using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    [Table("shared_item")]
    public class SharedItem
    {
        [JsonProperty("id")]
        [Column("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        [Column("text")]
        public string Text { get; set; }

        [JsonProperty("createDate")]
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("sessionId")]
        [Column("session_id")]
        public int SessionId { get; set; }

        [JsonProperty("path")]
        [Column("path")]
        public string Path { get; set; }
    }
}