using GTFys.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<bool> AuthenticateLoginAsync(object user)
        {
            try {
                // Use reflection to determine the type of the object
                Type userType = user.GetType();

                // Use the type information to determine the table to query
                string tableName = userType == typeof(Physio) ? "gtPHYSIO" : "gtPATIENT";

                // Get property values using reflection
                string username = (string)userType.GetProperty("Username").GetValue(user);
                string password = (string)userType.GetProperty("Password").GetValue(user);

                var query = $"SELECT COUNT(*) FROM {tableName} WHERE Username = @Username AND Password = @Password";
                var parameters = new { Username = username, Password = password };

                int count = await ExecuteScalarAsync<int>(query, parameters);

                // Only if theres 1 match i the table the login is approved
                return count == 1;
            }
            catch (Exception ex) {
                MessageBox.Show($"Forkert brugernavn eller adgangskode: {ex.Message}");
                return false;
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

        // Return the converted result (count of rows) 
        // Used for login authentication 
        public async Task<int> ExecuteScalarAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text)
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

                    // Convert the result to the specified type (int in this case)
                    return Convert.ToInt32(result);
                }
            }
        }


    }

}
