using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Spryd.Serveur.Models
{
    public class AuthentificationRequest
    {
        [JsonConstructor]
        public AuthentificationRequest(string identifier, string password)
        {
            Identifier = identifier;
            Password = password;
        }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}