using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    public class User
    {
        public User()
        {

        }

        [JsonConstructor]
        public User(string name, string surname, string email,string password, DateTime createDate = new DateTime(), DateTime updateDate = new DateTime())
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            CreateDate = createDate;
            UpdateDate = updateDate;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("createDate")]
        public DateTime? CreateDate { get; set; }
        [JsonProperty("updateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}