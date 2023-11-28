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
    // The DatabaseConnection class manages database connections using ADO.NET
    public class DatabaseConnection
    {
        // Private field to store the connection string
        private string ConnectionString;

        // Constructor to initialize the DatabaseConnection object
        public DatabaseConnection()
        {
            // Create a configuration instance to read the connection string from appsettings.json
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Retrieve the connection string from the configuration
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        // Method to establish a database connection
        // Returns: An IDbConnection representing the open database connection
        public IDbConnection Connect()
        {
            // Create a new SqlConnection with the stored connection string
            IDbConnection connection = new SqlConnection(ConnectionString);

            try
            {
                // Open the database connection
                connection.Open();

                Console.WriteLine("Connected to the database");
            }
            catch (Exception ex)
            {
                // Handle and display any exceptions that occur during connection
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }

            // Return the open database connection
            return connection;
        }

        // Method to disconnect from a database
        // Parameters:
        //   connection: The IDbConnection to be closed
        public void Disconnect(IDbConnection connection)
        {
            try
            {
                // Close the database connection
                connection.Close();

                Console.WriteLine("Disconnected from the database");
            }
            catch (Exception ex)
            {
                // Handle and display any exceptions that occur during disconnection
                Console.WriteLine($"Error disconnecting from the database: {ex.Message}");
            }
        }
    }

}
