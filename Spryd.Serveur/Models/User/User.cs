using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Spryd.Server.Models
{
    [Table("user")]
    public class User
    {
        /// <summary>
        /// Empty constructor for User class
        /// </summary>
        public User()
        {

        }

        /// <summary>
        /// Constructor for User class
        /// ", NullValueHandling=NullValueHandling.Ignore" ignores empty fields for JSON generation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createDate"></param>
        /// <param name="updateDate"></param>
        [JsonConstructor]
        public User(string name, string surname, string email, string password, DateTime createDate = new DateTime(), DateTime updateDate = new DateTime())
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

        [JsonIgnore]
        [JsonProperty("password", NullValueHandling=NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonIgnore]
        [Column("create_date")]
        [JsonProperty("createDate")]
        public DateTime? CreateDate { get; set; }

        [JsonIgnore]
        [Column("update_date")]
        [JsonProperty("updateDate")]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Overrides Equality
        /// We don't test password because we don't always get it's value
        /// </summary>
        /// <param name="comparedUser"></param>
        /// <returns></returns>
        public bool Equals(User comparedUser)
        {
            if (Name == comparedUser.Name
                && Surname == comparedUser.Surname
                && Email == comparedUser.Email
                && CreateDate == comparedUser.CreateDate
                && UpdateDate == comparedUser.UpdateDate)
                return true;
            else
                return false;
        }

        public bool IsValid()
        {
            bool idValid, nameValid, surnameValid, emailValid, createDateValid, updateDateValid;
            idValid = Id > 0 ? true : false;
            nameValid = !String.IsNullOrEmpty(Name) ?  true : false;
            surnameValid = !String.IsNullOrEmpty(Surname) ? true : false;
            emailValid = !String.IsNullOrEmpty(Email) ? true : false;
            createDateValid = CreateDate != null ? true : false;
            updateDateValid = UpdateDate != null ? true : false;

            return
                idValid
                && nameValid
                && surnameValid
                && emailValid
                && createDateValid
                && updateDateValid;
        }

        public override String ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

    }
}