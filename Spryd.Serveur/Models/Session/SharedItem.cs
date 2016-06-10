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
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("session_id")]
        public int SessionId { get; set; }

        [Column("path")]
        public string Path { get; set; }
    }
}