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
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("session_id")]
        public int SessionId { get; set; }
        [Column("is_creator")]
        public bool IsCreator { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("last_activity")]
        public DateTime? LastActivity { get; set; }
    }
}