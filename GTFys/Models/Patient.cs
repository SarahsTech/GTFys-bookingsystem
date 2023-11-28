using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    // The Patient class represents information about a patient
    public class Patient
    {
        // Properties representing various attributes of a patient
        public int PatientID { get; set; }                // Unique identifier for the patient
        public string PatientCPR { get; set; }            // CPR number of the patient
        public string PatientUsername { get; set; }       // Username chosen by the patient
        public string PatientPassword { get; set; }       // Password for patient authentication
        public string PatientProfilePicture { get; set; } // Profile picture uploaded by patient or generated 
        public int PatientPhoneNumber { get; set; }       // Phone number of the patient
        public string PatientEmail { get; set; }          // Email address of the patient
        public string PatientAddress { get; set; }        // Residential address of the patient
        public string PatientCity { get; set; }           // City of residence for the patient
        public int PatientPostalCode { get; set; }        // Postal code of the patient's location

        // Constructor to initialize a Patient object with an additional profile picture attribute
        public Patient(int patientID, string patientCPR, string patientUsername, string patientPassword, string imagePath,
            int patientPhoneNumber, string patientEmail, string patientAddress, string patientCity, int patientPostalCode)
        {
            // Set the properties of the Patient object based on the provided parameters
            PatientID = patientID;
            PatientCPR = patientCPR;
            PatientUsername = patientUsername;
            PatientPassword = patientPassword;
            PatientProfilePicture = imagePath; // imagePath: Path to the profile picture of the patient
            PatientPhoneNumber = patientPhoneNumber;
            PatientEmail = patientEmail;
            PatientAddress = patientAddress;
            PatientCity = patientCity;
            PatientPostalCode = patientPostalCode;
        }
    }

}
