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
        public int PatientID { get; set; }                        // Unique identifier for the patient
        public string CPR { get; set; }                    // CPR number of the patient
        public string FirstName { get; set; }              // First name of the patient
        public string LastName { get; set; }               // Last name of the patient
        public string Username { get; set; }               // Username chosen by the patient
        public string Password { get; set; }               // Password for patient authentication
        public string ProfilePicture { get; set; }         // Profile picture uploaded by patient or generated 
        public string PhoneNumber { get; set; }               // Phone number of the patient
        public string Email { get; set; }                  // Email address of the patient
        public string Address { get; set; }                // Residential address of the patient
        public string City { get; set; }                   // City of residence for the patient
        public int ZipCode { get; set; }                   // Zip code of the patient's location

        // Constructor to initialize a Patient object with an additional profile picture attribute
        public Patient(string cpr, string firstName, string lastName, string username, string password,
            string phoneNumber, string email, string address, string city, int zipCode, string imagePath)
        {
            // Set the properties of the Patient object based on the provided parameters
            CPR = cpr;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            ZipCode = zipCode;
            ProfilePicture = imagePath; // imagePath: Path to the profile picture of the patient
        }

        // Overloaded constructor to authenticate login
        public Patient(string username, string password)
        {
            // Set the properties of the Patient object based on the provided parameters
            Username = username;
            Password = password;
        }
    }


}
