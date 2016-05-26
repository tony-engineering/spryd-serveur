using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

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

        public bool Equals(Beacon comparedBeacon)
        {
            if (Id == comparedBeacon.Id
                && TechnicalId == comparedBeacon.TechnicalId)
                return true;
            else
                return false;
        }

        public bool IsValid()
        {
            bool idValid, technicalIdValid;
            idValid = Id > 0 ? true : false;
            technicalIdValid = !string.IsNullOrEmpty(TechnicalId) ? true : false;

            return
                idValid
                && technicalIdValid;
        }

        public override string ToString()
        { 
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}