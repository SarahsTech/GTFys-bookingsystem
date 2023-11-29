using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTFys.Models;

namespace GTFys.ViewModels
{
    internal class PhysioRepo
    {
        private DatabaseAccess dbAccess;
        public PhysioRepo(DatabaseAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }
        public async Task<bool> physioAuthenticateLogin(string physioUsername, string physioPassword)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM gtPhysio WHERE PhysioUsername = @PhysioUsername AND PhysioPassword = @PhysioPassword";
                var parameters = new { PhysioUsername = physioUsername, PhysioPassword = physioPassword };

                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters);

                // Check if any rows were affected (indicating successful authentication)
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating physiotherapist: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> physioUpdateUser(Physio physio)
        {
            try
            {
                var query = "UPDATE gtPhysio SET " +
                    "PhysioCPR = @PhysioCPR, " +
                    "PhysioFirstName = @PhysioFirstName, " +
                    "PhysioLastName = @PhysioLastName, " +
                    "PhysioUsername = @PhysioUsername, " +
                    "PhysioPassword = @PhysioPassword, " +
                    "PhysioPhoneNumber = @PhysioPhoneNumber, " +
                    "PhysioEmail = @PhysioEmail, " +
                    "PhysioAdress = @PhysioAdress, " +
                    "PhysioZipCode = @PhysioZipCode, " +
                    "PhysioCity = @PhysioCity, " +
                    "PhysioProfilePicture = @PhysioProfilePicture";

                var parameters = new
                {
                    physio.PhysioCPR,
                    physio.PhysioFirstName,
                    physio.PhysioLastName,
                    physio.PhysioUsername,
                    physio.PhysioPassword,
                    physio.PhysioPhoneNumber,
                    physio.PhysioEmail,
                    physio.PhysioAddress,
                    physio.PhysioCity,
                    physio.PhysioZipCode,
                };
                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating physiotherapist: {ex.Message}");
                // Log the error, and rethrow the exception
                throw;
            }
        }

    }
}
