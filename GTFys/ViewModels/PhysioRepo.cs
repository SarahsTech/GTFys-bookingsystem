using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTFys.Models;

namespace GTFys.ViewModels
{
    public class PhysioRepo
    {
        private DatabaseAccess dbAccess;
        public PhysioRepo(DatabaseAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        // Method for authenticating a physiotherapist login
        // Returns a boolean indicating whether the authentication was successful or not
        public async Task<bool> physioAuthenticateLogin(string physioUsername, string physioPassword)
        {
            // Create a new Physio object with the provided username and password
            Physio physio = new Physio(physioUsername, physioPassword);

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the physiotherapist
            bool physioAuthResult = await dbAccess.AuthenticateLoginAsync(physio);

            // Return the result of the authentication (true if successful, false otherwise)
            return physioAuthResult;
        }

        public async Task<bool> physioUpdateUser(Physio physio)
        {
            try
            {
                var query = "UPDATE gtPHYSIO SET " +
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
