using System;
using System.Collections.Generic;
using Spryd.Serveur.Models;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Spryd.Serveur.Controllers
{
    internal class BeaconDal : IBeaconDal
    {
        private MySqlConnection connection;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeaconDal(ConnectionStringSettings connectionString)
        {
            // Create DB connection
            connection = new MySqlConnection(connectionString.ConnectionString);
        }

        /// <summary>
        /// Lists all Beacons
        /// </summary>
        /// <returns></returns>
        public List<Beacon> GetBeacons()
        {
            List<Beacon> beaconList = new List<Beacon>();

            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();

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

            connection.Close();

            return beaconList;
        }
    }
}