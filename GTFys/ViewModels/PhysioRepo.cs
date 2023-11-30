using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public async Task<bool> physioAuthenticateLogin(string username, string password)
        {
            // Create a new Physio object with the provided username and password
            Physio physio = new Physio(username, password);

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the physiotherapist
            bool physioAuthResult = await dbAccess.AuthenticateLoginAsync(physio);

            // Return the result of the authentication (true if successful, false otherwise)
            return physioAuthResult;
        }

        // Method to update physio profile information
        public async Task<bool> physioUpdateUser(Physio physio)
        {
            try
            {
                var query = "UPDATE gtPHYSIO SET " +
                 "CPR = @CPR, " +
                 "FirstName = @FirstName, " +
                 "LastName = @LastName, " +
                 "Username = @Username, " +
                 "Password = @Password, " +
                 "Phone = @Phone, " +
                 "Email = @Email, " +
                 "Adress = @Address, " +
                 "ZipCode = @ZipCode, " +
                 "City = @City, " +
                 "ProfilePicture = @ProfilePicture";

                var parameters = new {
                    physio.CPR,
                    physio.FirstName,
                    physio.LastName,
                    physio.Username,
                    physio.Password,
                    physio.Phone,
                    physio.Email,
                    physio.Address,
                    physio.City,
                    physio.ZipCode,
                };

                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved opdatering af fysioterapuet: {ex.Message}");
                // Log the error, and rethrow the exception
                throw;
            }
        }

    }
}
