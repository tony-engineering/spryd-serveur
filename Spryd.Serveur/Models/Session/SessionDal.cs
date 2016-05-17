using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spryd.Serveur.Models
{
    /// <summary>
    /// Data access layer for Session
    /// </summary>
    public class SessionDal : ISessionDal
    {
        private MySqlConnection connection;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionDal(ConnectionStringSettings connectionString)
        {
            // Create DB connection
            connection = new MySqlConnection(connectionString.ConnectionString);
        }

        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="user"></param>
        public long AddSession(Session session)
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

            return cmd.LastInsertedId;
        }
    }
}