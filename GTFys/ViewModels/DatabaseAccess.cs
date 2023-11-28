using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.ViewModels
{
    public class DatabaseAccess
    {

        public DatabaseAccess()
        {
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sqlStatement, object parameter = null)
        {
            using (IDbConnection connection = new DatabaseConnection().Connect())
            using (SqlCommand command = new SqlCommand(sqlStatement, (SqlConnection)connection))
            {
                if (parameter != null)
                {
                    foreach (var property in parameter.GetType().GetProperties())
                    {
                        command.Parameters.AddWithValue("@" +  property.Name, parameter.GetValue(parameter));
                    }
                }
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
