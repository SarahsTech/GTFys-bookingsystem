using GTFys.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTFys.ViewModels
{
    public class PatientRepo
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        public PatientRepo()
        {

        }

        // Method for authenticating a patient login
        // Returns a boolean indicating whether the authentication was successful or not

        public async Task<bool> PatientAuthenticateLogin(string username, string password)
        {

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the patient
            var result = await dbAccess.AuthenticateLoginAsync(username, password, typeof(Patient));

            // The result set of AuthenticateLoginAsync
            bool isAuthenticated = result.isAuthenticated;

        // public async Task<bool> patientAuthenticateLogin(string username, string password)
        //{
        //    // Create a new Patient object with the provided username and password
        //    Patient patient = new Patient(username, password);

        //    // Call the generic AuthenticateLoginAsync method in DatabaseAccess
        //    // to perform the authentication for the patient
        //    bool patientAuthResult = await dbAccess.AuthenticateLoginAsync(patient);

        //    // Return the result of the authentication (true if successful, false otherwise)
        //    return patientAuthResult;
        //}

        // Method to insert a new patient using the stored procedure gtspInsertPatient
        // Returns a boolean indicating whether the insertion was successful or not
        public async Task<bool> PatientCreateUser(Patient patient)
        {
            bool insertionSuccessful = false;
            try {
                // Stored procedure name
                var storedProcedure = "gtspInsertPatient";

                // Parameters for the stored procedure, using the properties of the Patient object
                var parameters = new {
                    patient.FirstName,
                    patient.LastName,
                    patient.Username,
                    patient.Password,
                    patient.CPR,
                    patient.Phone,
                    patient.Email,
                    patient.Address,
                    patient.ZipCode,
                    patient.City,
                    patient.ProfilePicture,
                };


            // Execute the stored procedure and retrieve the output parameter
            var rowsAffected = await dbAccess.ExecuteNonQueryAsync(storedProcedure, parameters, CommandType.StoredProcedure);

            // Set the CurrentPatient to the authenticated patient
            PatientService.CurrentPatient = (Patient)result.userData;

            // Return the result of the authentication (true if successful, false otherwise)
            return (isAuthenticated);
        }

        // Method to update patient profile information
        public async Task<bool> PatientUpdateUser(string firstName, string lastName, string username, string password,
            string email, string phone, string cpr, string address, int zipCode, string city, string imagePath)
        {
            try
            {
                var query = "gtspUpdatePatient";

                byte[] imageBytes = (!string.IsNullOrEmpty(imagePath)) ? File.ReadAllBytes(imagePath) : null;

                var parameters = new
                {
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

            bool updateSuccessful = false;
            try {
                // SQL query to update patient profile in the "gtPATIENT" table
                var query = "UPDATE gtPATIENT SET " +
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


                // Parameters for the SQL query, using the properties of the Patient object
                var parameters = new {
                    patient.CPR,
                    patient.FirstName,
                    patient.LastName,
                    patient.Username,
                    patient.Password,
                    patient.Phone,
                    patient.Email,
                    patient.Address,
                    patient.City,
                    patient.ZipCode,
                    // Assuming ProfilePicture property exists in the Patient class
                    patient.ProfilePicture,
                };

                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters, CommandType.StoredProcedure);

                // Set the updated values to the current patient's information               
                var updatedInfoQuery = $"SELECT * FROM gtPATIENT WHERE CPR = @CPR";
                var updatedParameters = new { CPR = cpr };

                // Fetch the updated patient and update the current patient
                var patient = await dbAccess.ExecuteQueryFirstOrDefaultAsync(updatedInfoQuery, updatedParameters, typeof(Patient));
                PatientService.CurrentPatient = (Patient)patient;

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved opdatering af patient: {ex.Message}");
                // Log the error, and rethrow the exception
                throw;
            }
        }
    }

}
