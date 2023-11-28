using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace GTFys.ViewModels
{
    public class DatabaseConnection
    {
        private string ConnectionString;

        public DatabaseConnection()
        {
            // Connecting to database
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        // Method to establish a database connection
        public IDbConnection Connect()
        {
            IDbConnection connection = new SqlConnection(ConnectionString);
            try
            {
                // Open the database connection
                connection.Open();

                Console.WriteLine("Connected to database");
            }

            catch (Exception ex) 
            {
                // Handle any exceptions that occur during the connection
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }

            return connection;
        }
    }
}
