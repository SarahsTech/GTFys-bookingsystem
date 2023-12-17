using GTFys.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTFys.Application
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
            catch (Exception ex)
            {
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

            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect())
                {
                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection))
                    {

                        // Set the command type (e.g., Text or StoredProcedure)
                        command.CommandType = commandType;

                        // If parameters are provided, add them to the command
                      
                        if (parameters != null)
                        {
                            foreach (var property in parameters.GetType().GetProperties())
                            {

                                // Add parameters to the command based on the properties of the parameters object
                                var paramName = "@" + property.Name;

                                if (property.PropertyType == typeof(byte[]))
                                {

                                    // Handle byte[] (image) type
                                    var parameter = new SqlParameter(paramName, SqlDbType.Image);
                                    parameter.Value = property.GetValue(parameters) ?? DBNull.Value;
                                    command.Parameters.Add(parameter);
                                }
                                else
                                {
                                    // Handle other types
                                    var parameter = new SqlParameter(paramName, property.GetValue(parameters) ?? DBNull.Value);
                                    command.Parameters.Add(parameter);
                                }

                                // Log the parameter name and value for debugging
                                Debug.WriteLine($"Parameter: {property.Name}, Value: {property.GetValue(parameters)}");
                            }
                        }

                        // Execute the command asynchronously and return the number of affected rows
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        
                        return rowsAffected > 0 ? rowsAffected : 0;
                    }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
                Debug.WriteLine("Error occurred: " + Environment.NewLine + ex.ToString());
                return -1;
        }
    }

        // Method to execute a SQL query and return the result as a DataRow
        public async Task<object> ExecuteQueryAsync(string query, object parameters, CommandType commandType = CommandType.Text)
        {
            try
            {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect())
                {
                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection))
                    {
                        // If parameters are provided, add them to the command
                        if (parameters != null)
                        {
                            foreach (var property in parameters.GetType().GetProperties())
                            {
                                // Add parameters to the command based on the properties of the parameters object
                                command.Parameters.AddWithValue("@" + property.Name, property.GetValue(parameters));
                            }
                        }

                        // Execute the command and retrieve the result as a SqlDataReader
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

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
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or throw them for further handling
                Debug.WriteLine($"Error executing query: {ex.Message}");
                throw;
            }
        }


        // Method to execute a SQL query and retrieve the first result as an instance of a specified type
        public async Task<object> ExecuteQueryFirstOrDefaultAsync(string query, object parameters, Type resultType, CommandType commandType = CommandType.Text)
        {
            var resultData = await ExecuteQueryAsync(query, parameters, commandType);

            if (resultData != null && resultData is DataRow row)
            {
                var result = Activator.CreateInstance(resultType);

                foreach (var property in resultType.GetProperties())
                {
                    var columnName = property.Name;
                    var valueFromDatabase = row[columnName];
                    object convertedValue;

                    if (valueFromDatabase == DBNull.Value)
                    {
                        convertedValue = null;
                    }
                    else
                    {
                        if (typeof(byte[]).IsAssignableFrom(valueFromDatabase.GetType()))
                        {
                            byte[] byteArray = (byte[])valueFromDatabase;
                            convertedValue = Convert.ToBase64String(byteArray);
                        }
                        else
                        {
                            convertedValue = Convert.ChangeType(valueFromDatabase, property.PropertyType);
                        }
                    }

                    property.SetValue(result, convertedValue);
                }

                return result;
            }
            return null;
        }

    }

}
