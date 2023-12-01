using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GTFys.Models;

namespace GTFys.ViewModels
{
    public class PhysioRepo
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        public PhysioRepo()
        {
           
        }

        // Method for authenticating a physiotherapist login
        // Returns a boolean indicating whether the authentication was successful or not
        public async Task<bool> PhysioAuthenticateLogin(string username, string password)
        {
            // Create a new Physio object with the provided username and password
            // Physio physio = new Physio(username, password);

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
                var query = "UPDATE gtPHYSIO SET " +
                 "CPR = @CPR, " +
                 "FirstName = @FirstName, " +
                 "LastName = @LastName, " +
                 "Username = @Username, " +
                 "Password = @Password, " +
                 "Phone = @Phone, " +
                 "Email = @Email, " +
                 "Address = @Address, " +
                 "ZipCode = @ZipCode, " +
                 "City = @City, " +
                 "ProfilePicture = @ProfilePicture";

                byte[] imageBytes = null;

                // Check if imagePath has a value
                if (!string.IsNullOrEmpty(imagePath)) {
                    // Read the image file and convert it to bytes
                    imageBytes = File.ReadAllBytes(imagePath);
                }

                var parameters = new {
                    CPR = cpr,
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Password = password,
                    Phone = phone,
                    Email = email,
                    Address = address,
                    ZipCode = zipCode,
                    City = city,
                    ProfilePicture = imageBytes // Make sure ProfilePicture is of type byte[]
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
