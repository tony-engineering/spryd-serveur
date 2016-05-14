using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Data access layer for User
    /// </summary>
    public class UserDal : IUserDal
    {
        private MySqlConnection connection;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserDal(ConnectionStringSettings connectionString)
        {

            // Create DB connection
            connection = new MySqlConnection(connectionString.ConnectionString);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public long AddUser(User user)
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

            return cmd.LastInsertedId;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            User user = new User();
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id,name, surname,email,create_date,update_date FROM user where id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var result = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
            if (result.Read())
            {
                user.Id = result.GetInt32("id");
                user.Name = result.GetString("name");
                user.Surname = result.GetString("surname");
                user.Email = result.GetString("email");
                user.CreateDate = result.GetDateTime("create_date");
                user.UpdateDate = result.GetDateTime("update_date");
            }
            else
                throw new UserNotFoundException("User with id "+id+" not found.");

            connection.Close();

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

            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id,name, surname,email,password,create_date,update_date FROM user";

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

            return users;
        }
    }
}