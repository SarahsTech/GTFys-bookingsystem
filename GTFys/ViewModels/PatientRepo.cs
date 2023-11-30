using GTFys.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTFys.ViewModels
{
    public class PatientRepo
    {
        private DatabaseAccess dbAccess;
        public PatientRepo(DatabaseAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        // Method for authenticating a patient login
        // Returns a boolean indicating whether the authentication was successful or not
         public async Task<bool> patientAuthenticateLogin(string username, string password)
        {
            // Create a new Patient object with the provided username and password
            Patient patient = new Patient(username, password);

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the patient
            bool patientAuthResult = await dbAccess.AuthenticateLoginAsync(patient);

            // Return the result of the authentication (true if successful, false otherwise)
            return patientAuthResult;
        }

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

                // Return true if at least one row was affected, indicating a successful insertion
                if (rowsAffected > 0) {
                    MessageBox.Show($"Indsættelse succesfuld.");
                    insertionSuccessful = true;
                }
                else {
                    MessageBox.Show($"Fejl ved oprettelse af patient.");
                    insertionSuccessful = false;
                }
                return insertionSuccessful;
            }
            catch (Exception ex) {
                // Show a message box with the error details
                MessageBox.Show($"Fejl ved indsættelse af patient: {ex.Message}");

                // Log the error, and rethrow the exception for further handling
                throw;
            }
        }

        // Method to update patient profile information
        // Returns a boolean indicating whether the profile update was successful or not
        public async Task<bool> PatientUpdateUser(Patient patient)
        {
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

                // Execute the SQL query and get the number of affected rows
                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters);

                // Return true if at least one row was affected, indicating a successful update
                if(rowsAffected > 0) {
                    MessageBox.Show($"Opdatering succesfuld.");
                    updateSuccessful = true;
                }
                else {
                    MessageBox.Show($"Fejl ved opdatering af patient.");
                    updateSuccessful = false;
                }
                return updateSuccessful;
            }
            catch (Exception ex) {
                // Show a message box with the error details
                MessageBox.Show($"Fejl ved opdatering af patient: {ex.Message}");

                // Log the error, and rethrow the exception for further handling
                throw;
            }
        }

        // Method to delete a patient based on the CPR
        // Returns a boolean indicating whether the deletion was successful or not
        public async Task<bool> PatientDeleteUser(string patientCPR)
        {
            bool deletionSuccessful = false;
            try {
                // SQL query to delete a patient from the "gtPATIENT" table based on CPR
                var query = "DELETE FROM gtPATIENT WHERE PatientCPR = @PatientCPR";

                // Parameters for the SQL query
                var parameters = new { PatientCPR = patientCPR };

                // Execute the SQL query and get the number of affected rows
                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters);

                // Return true if at least one row was affected, indicating a successful deletion
                if (rowsAffected > 0) {
                    MessageBox.Show($"Sletning succesfuld.");
                    deletionSuccessful = true;
                }
                else {
                    MessageBox.Show($"Fejl ved sletning af patient.");
                    deletionSuccessful = false;
                }
                return deletionSuccessful;
            }
            catch (Exception ex) {
                // Show a message box with the error details
                MessageBox.Show($"Fejl ved sletning af patient: {ex.Message}");

                // Log the error, and rethrow the exception for further handling
                throw;
            }
        }


    }
}
