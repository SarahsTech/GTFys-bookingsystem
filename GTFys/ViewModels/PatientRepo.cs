using GTFys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         public async Task<bool> patientAuthenticateLogin(string patientUsername, string patientPassword)
        {
            // Create a new Patient object with the provided username and password
            Patient patient = new Patient(patientUsername, patientPassword);

            // Call the generic AuthenticateLoginAsync method in DatabaseAccess
            // to perform the authentication for the patient
            bool patientAuthResult = await dbAccess.AuthenticateLoginAsync(patient);

            // Return the result of the authentication (true if successful, false otherwise)
            return patientAuthResult;
        }


    }
}
