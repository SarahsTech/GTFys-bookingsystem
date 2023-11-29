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
        public string PatientFirstName { get; set; }      // First name of the patient
        public string PatientLastName { get; set; }       // Last name of the patient
        public string PatientUsername { get; set; }       // Username chosen by the patient
        public string PatientPassword { get; set; }       // Password for patient authentication
        public string PatientProfilePicture { get; set; } // Profile picture uploaded by patient or generated 
        public int PatientPhoneNumber { get; set; }       // Phone number of the patient
        public string PatientEmail { get; set; }          // Email address of the patient
        public string PatientAddress { get; set; }        // Residential address of the patient
        public string PatientCity { get; set; }           // City of residence for the patient
        public int PatientZipCode { get; set; }        // Zip code of the patient's location

        // Constructor to initialize a Patient object with an additional profile picture attribute
        public Patient(string patientCPR, string patientFirstName, string patientLastName, string patientUsername, string patientPassword,
            int patientPhoneNumber, string patientEmail, string patientAddress, string patientCity, int patientZipCode, string imagePath)
        {
            // Set the properties of the Patient object based on the provided parameters
            PatientCPR = patientCPR;
            PatientFirstName = patientFirstName;
            PatientLastName = patientLastName;
            PatientUsername = patientUsername;
            PatientPassword = patientPassword;      
            PatientPhoneNumber = patientPhoneNumber;
            PatientEmail = patientEmail;
            PatientAddress = patientAddress;
            PatientCity = patientCity;
            PatientZipCode = patientZipCode;
            PatientProfilePicture = imagePath; // imagePath: Path to the profile picture of the patient
        }

        // Overloaded constructor to authenticate login
        public Patient(string patientUsername, string patientPassword)
        {
            // Set the properties of the Physio object based on the provided parameters
            PatientUsername = patientUsername;
            PatientPassword = patientPassword;
        }

    }

}
