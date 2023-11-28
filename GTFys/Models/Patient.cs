using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    public class Patient
    {
        public int PatientID { get; set; }

        public string PatientCPR { get; set; }

        public string PatientUsername { get; set; }

        public string PatientPassword { get; set; }

        public string PatientProfilePicture { get; set; }

        public int PatientPhoneNumber { get; set; }

        public string PatientEmail { get; set; }

        public string PatientAddress { get; set; }

        public string PatientCity { get; set; }

        public int PatientPostalCode { get; set; }

        // Constructor to initiliaze physio properties
        public Patient(int patientID, string patientCPR, string patientUsername, string patientPassword,
            int patientPhoneNumber, string patientEmail, string patientAddress, string patientCity, int patientPostalCode)
        {
            // Set the values of physio properties based on constructor parameters
            PatientID = patientID;
            PatientCPR = patientCPR;
            PatientUsername = patientUsername;
            PatientPassword = patientPassword;
            PatientPhoneNumber = patientPhoneNumber;
            PatientEmail = patientEmail;
            PatientAddress = patientAddress;
            PatientCity = patientCity;
            PatientPostalCode = patientPostalCode;
        }

        // Overloaded constructor to initialize physio properties with image 
        public Patient(int patientID, string patientCPR, string patientUsername, string patientPassword,string imagePath,
            int patientPhoneNumber, string patientEmail, string patientAddress, string patientCity, int patientPostalCode)
        {
            // Set the values of physio properties based on constructor parameters
            PatientID = patientID;
            PatientCPR = patientCPR;
            PatientUsername = patientUsername;
            PatientPassword = patientPassword;
            PatientProfilePicture = imagePath;
            PatientPhoneNumber = patientPhoneNumber;
            PatientEmail = patientEmail;
            PatientAddress = patientAddress;
            PatientCity = patientCity;
            PatientPostalCode = patientPostalCode;
        }
    }
}
