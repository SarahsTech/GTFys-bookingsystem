using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GTFys.Domain;

namespace GTFys.Application
{
    public class PhysioRepo
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        public PhysioRepo()
        {
           
        }

        //Method for authenticating a physiotherapist login
        // Returns a boolean indicating whether the authentication was successful or not
        public async Task<bool> PhysioAuthenticateLogin(string username, string password)
        {

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the physiotherapist
            var result = await dbAccess.AuthenticateLoginAsync(username, password, typeof(Physio));

            // The result set of AuthenticateLoginAsync
            bool isAuthenticated = result.isAuthenticated;
            // Set the CurrentPhysio to the authenticated physio
            PhysioService.CurrentPhysio = (Physio)result.userData;

            // Return the result of the authentication (true if successful, false otherwise)
            return (isAuthenticated);
        }

        // Method to update physio profile information
        public async Task<bool> PhysioUpdateUser(string firstName, string lastName, string username, string password,
            string email, string phone, string cpr, string address, int zipCode, string city, string imagePath)
        {
            try
            {
                var query = "gtspUpdatePhysio";

                byte[] imageBytes = (!string.IsNullOrEmpty(imagePath)) ? File.ReadAllBytes(imagePath) : null;

                var parameters = new {
                    CPR = cpr,
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Password = password,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    ZipCode = zipCode,
                    City = city,
                    ProfilePicture = imageBytes
                };

                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters, CommandType.StoredProcedure);

                // Set the updated values to the current physios information               
                var updatedInfoQuery = $"SELECT * FROM gtPHYSIO WHERE CPR = @CPR";
                var updatedParameters = new { CPR = cpr };

                // Fetch the updated physio and update the current physio
                var physio = await dbAccess.ExecuteQueryFirstOrDefaultAsync(updatedInfoQuery, updatedParameters, typeof(Physio));
                PhysioService.CurrentPhysio = (Physio)physio; 

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
