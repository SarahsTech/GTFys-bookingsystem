using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Application
{
    // Singleton service to hold the patient object
    public class PatientService
    {
        // Static property to hold the current patient object for the entire application
        private static Patient _currentPatient;

        // Property to get or set the current patient object
        public static Patient CurrentPatient
        {
            // Getter for accessing the current patient object
            get
            { return _currentPatient; }

            // Setter for updating the current patient object
            set
            { _currentPatient = value; }
        }

        // Method to initialize CurrentPatient with a given patientID
        public async static void InitializePatient(int patientID)
        {
            // Create a new Patient object or fetch it from the database based on the patientID
            // For simplicity, assuming there is a method GetPatientById in your data access layer
            // that retrieves a Patient object by its ID

            var query = "SELECT * FROM gtPATIENT WHERE PatientID = @PatientID";
            var parameters = new { PatientID = patientID };

            DatabaseAccess dbAccess = new DatabaseAccess();

            var patient = await dbAccess.ExecuteQueryFirstOrDefaultAsync(query, parameters, typeof(Patient));

            // Set the CurrentPatient to the retrieved/fetched patient
            CurrentPatient = (Patient)patient;
        }
    }

}
