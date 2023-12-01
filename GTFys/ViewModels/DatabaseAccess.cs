using GTFys.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTFys.ViewModels
{
    public class DatabaseAccess
    {
        public DatabaseAccess()
        {
        }

        public async Task<(bool isAuthenticated, object userData)> AuthenticateLoginAsync(string username, string password, Type userType)
        {
            try {

                // Use the type information to determine the table to query
                string tableName = userType == typeof(Physio) ? "gtPHYSIO" : "gtPATIENT";

                var query = $"SELECT * FROM {tableName} WHERE Username = @Username AND Password = @Password";
                var parameters = new { Username = username, Password = password };

                // Fetch the user data along with the authentication check
                var user = await ExecuteQueryFirstOrDefaultAsync(query, parameters, userType);

                // Check if authentication is successful
                var isAuthenticated = user != null;

                // Return two values containing authentication status and user data
                return (isAuthenticated, user);
            }
            catch (Exception ex) {
                MessageBox.Show($"Forkert brugernavn eller adgangskode: {ex.Message}");
                // Return two values indicating authentication failure and null user data
                return (false, null);
            }
        }

        // Execute a non-query asynchronously, such as an SQL command or stored procedure, and return the number of affected rows.
        // Returns an integer representing the number of rows affected by the executed command.
        // Used for operations that doesn't return data such as INSERT, UPDATE and DELETE. 
        public async Task<int> ExecuteNonQueryAsync(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            // Establish a new database connection
            using (IDbConnection connection = new DatabaseConnection().Connect()) {
                // Create a new SQL command using the provided query and connection
                using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {
                    // Set the command type (e.g., Text or StoredProcedure)
                    command.CommandType = commandType;

                    // If parameters are provided, add them to the command
                    if (parameters != null) {
                        foreach (var property in parameters.GetType().GetProperties()) {
                            // Add parameters to the command based on the properties of the parameters object
                            command.Parameters.AddWithValue("@" + property.Name, property.GetValue(parameters));
                        }
                    }

                    // Execute the command asynchronously and return the number of affected rows
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Returns the result set of the query
        // Used for login authentication 
        public async Task<object> ExecuteScalarAsync(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            // Establish a new database connection
            using (IDbConnection connection = new DatabaseConnection().Connect()) {
                // Create a new SQL command using the provided query and connection
                using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {
                    // Set the command type (e.g., Text or StoredProcedure)
                    command.CommandType = commandType;

                    // If parameters are provided, add them to the command
                    if (parameters != null) {
                        foreach (var property in parameters.GetType().GetProperties()) {
                            // Add parameters to the command based on the properties of the parameters object
                            command.Parameters.AddWithValue("@" + property.Name, property.GetValue(parameters));
                        }
                    }

                    // Execute the command asynchronously and retrieve the scalar result
                    var result = await command.ExecuteScalarAsync();

                    // Return the result as an object
                    return result;
                }
            }
        }
        // Method to execute a SQL query and return the result as a DataRow
        public async Task<object> ExecuteQueryAsync(string query, object parameters)
        {
            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect()) {
                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {
                        // If parameters are provided, add them to the command
                        if (parameters != null) {
                            foreach (var property in parameters.GetType().GetProperties()) {
                                // Add parameters to the command based on the properties of the parameters object
                                command.Parameters.AddWithValue("@" + property.Name, property.GetValue(parameters));
                            }
                        }

                        // Execute the command and retrieve the result as a SqlDataReader
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                            // Check if there are any rows in the result set
                            if (await reader.ReadAsync()) { 

                                // Create a DataTable to hold the result
                                DataTable table = new DataTable();
                                table.Load(reader);

                                // Assuming the query returns only one row, return the first DataRow
                                var result = table.Rows.Count > 0 ? table.Rows[0] : null;

                                // Log the result for debugging
                                Debug.WriteLine($"Query result: {result}");

                                return result; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                // Handle exceptions, log errors, or throw them for further handling
                Debug.WriteLine($"Error executing query: {ex.Message}");
                throw;
            }

            // Return null if no data is retrieved
            return null;
        }


        // Method to execute a SQL query and retrieve the first result as an instance of a specified type
        private async Task<object> ExecuteQueryFirstOrDefaultAsync(string query, object parameters, Type resultType)
        {
            // Use ExecuteQuery method to fetch data from the database
            var resultData = await ExecuteQueryAsync(query, parameters);

            // Check if any data was retrieved before populating the properties
            if (resultData != null && resultData is DataRow row) { // If resultData is of type DataRow, then assign it to the variable row

                // Create an instance of the resultType to hold the retrieved data
                var result = Activator.CreateInstance(resultType);

                foreach (var property in resultType.GetProperties()) {
                    // Add logic to map column names to property names
                    // This assumes that the database column names excactly match the property names
                    var columnName = property.Name;

                    // Extract the value for the current property from resultData
                    var valueFromDatabase = row[columnName];

                    // Convert the value from the database to the property type
                    var convertedValue = Convert.ChangeType(valueFromDatabase, property.PropertyType);

                    // Set the property value
                    property.SetValue(result, convertedValue);
                }
                // Return the populated result object
                return result; 
            }
            // If no data was retrieved, return null
            return null;
        }


    }

}
