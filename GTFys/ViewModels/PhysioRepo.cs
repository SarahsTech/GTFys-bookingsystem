using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.ViewModels
{
    internal class PhysioRepo
    {
        private DatabaseAccess dbAccess;
        public PhysioRepo(DatabaseAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }
        public async Task<bool> PhysioAuthenticateLogin(string username, string password)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM gtPhysio WHERE PhysioUsername = @Username AND PhysioPassword = @Password";
                var parameters = new { Username = username, Password = password };

                var result = await dbAccess.ExecuteQueryAsync<int>(query, parameters);

                return result.FirstOrDefault() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating physiotherapist: {ex.Message}");
                return false;
            }
        }
    }
}
