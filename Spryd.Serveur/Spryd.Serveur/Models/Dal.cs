using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Couche d'accès aux données MySQL
    /// </summary>
    public class Dal : IDal
    {
        private MySqlConnection connection;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Dal()
        {
            // Création de la chaîne de connexion
            string connectionString = "SERVER=127.0.0.1; DATABASE=sprydioflxadm; UID=root; PASSWORD=";
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Ajouter un utilisateur
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            try
            {
                // Ouverture de la connexion SQL
                connection.Open();

                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "INSERT INTO user (name, surname,email,password,create_date,update_date) VALUES (@name, @surname, @email, @password, @create_date, @update_date)";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@surname", user.Surname);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_date", DateTime.Now);

                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();

                // Fermeture de la connexion
                connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        /// <summary>
        /// Récupère la liste des beacons
        /// </summary>
        /// <returns></returns>
        public List<Beacon> GetBeacons()
        {
            List<Beacon> beaconList = new List<Beacon>();
            try
            {
                // Ouverture de la connexion SQL
                connection.Open();

                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "SELECT id, technical_id FROM beacon";

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                while (result.Read())
                {
                    beaconList.Add(new Beacon()
                    {
                        Id = result.GetInt32("id"),
                        TechnicalId = result.GetString("technical_id")
                    });                    
                };

                // Fermeture de la connexion
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            };

            return beaconList;
        }

        /// <summary>
        /// Récupérer un utilisateur par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            User user = new User();
            try
            {
                // Ouverture de la connexion SQL
                connection.Open();

                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = connection.CreateCommand();

                // Requête SQL
                //cmd.CommandText = "INSERT INTO user (name, surname,email,password,create_date,update_date) VALUES (@name, @surname, @email, @password, @create_date, @update_date)";
                cmd.CommandText = "SELECT id,name, surname,email,password,create_date,update_date FROM user where id = @id";
                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@id", id);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (result.Read())
                {
                    user.Id = result.GetInt32("id");
                    user.Name = result.GetString("name");
                    user.Surname = result.GetString("surname");
                    user.Email = result.GetString("email");
                    user.Password = result.GetString("password");
                    user.CreateDate = result.GetDateTime("create_date");
                    user.UpdateDate = result.GetDateTime("update_date");
                }
                else
                    return null;

                // Fermeture de la connexion
                connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            };

            return user;
        }
    }
}