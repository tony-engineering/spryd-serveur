using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Data access layer
    /// </summary>
    public class Dal : IDal
    {
        private MySqlConnection connection;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Dal()
        {
            // Create DB auth string
            string connectionString = "SERVER=127.0.0.1; DATABASE=sprydioflxadm; UID=root; PASSWORD=";
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO user (name, surname,email,password,create_date,update_date) VALUES (@name, @surname, @email, @password, @create_date, @update_date)";

                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@surname", user.Surname);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_date", DateTime.Now);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            User user = new User();
            try
            {
                connection.Open();

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = "SELECT id,name, surname,email,password,create_date,update_date FROM user where id = @id";
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

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            };

            return user;
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<User> ListUsers()
        {
            List<User> users = new List<User>();
            try
            {
                connection.Open();

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = "SELECT id,name, surname,email,password,create_date,update_date FROM user";

                //var result = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);

                using (MySqlDataReader result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        User user = new User();

                        user.Id = result.GetInt32("id");
                        user.Name = result.GetString("name");
                        user.Surname = result.GetString("surname");
                        user.Email = result.GetString("email");
                        user.Password = result.GetString("password");
                        user.CreateDate = result.GetDateTime("create_date");
                        user.UpdateDate = result.GetDateTime("update_date");

                        users.Add(user);
                    }
                }

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            };

            return users;
        }

        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="user"></param>
        public void AddSession(Session session)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO session (name, password, start_date, spryd_zone_id) VALUES (@name, @password, @start_date, @spryd_zone_id)";
                    
                cmd.Parameters.AddWithValue("@name", session.Name);
                cmd.Parameters.AddWithValue("@password", session.Password);
                cmd.Parameters.AddWithValue("@start_date", DateTime.Now);
                cmd.Parameters.AddWithValue("@spryd_zone_id", session.SprydZoneId);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }


    }
}