using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    // The Physio class represents information about a physiotherapist
    public class Physio
    {
        // Properties representing various attributes of a physiotherapist
        public int PhysioID { get; set; }              // Unique identifier for the physiotherapist
        public string CPR { get; set; }                // CPR number of the physiotherapist
        public string FirstName { get; set; }          // First name of the physiotherapist
        public string LastName { get; set; }           // Last name of the physiotherapist
        public string Username { get; set; }           // Username chosen by the physiotherapist
        public string Password { get; set; }           // Password for physiotherapist authentication
        public string PhoneNumber { get; set; }           // Phone number of the physiotherapist
        public string Email { get; set; }              // Email address of the physiotherapist
        public string Address { get; set; }            // Residential address of the physiotherapist
        public string City { get; set; }               // City of residence for the physiotherapist
        public int ZipCode { get; set; }               // Postal code of the physiotherapist's location
        public string ProfilePicture { get; set; }     // Profile picture uploaded by physiotherapist or generated 

        // Constructor to initialize a Physio object with essential attributes
        public Physio(string cpr, string firstName, string lastName, string username, string password,
            string phoneNumber, string email, string address, string city, int zipCode, string imagePath)
        {
            // Set the properties of the Physio object based on the provided parameters
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
            ProfilePicture = imagePath; //  imagePath: Path to the profile picture of the physiotherapist
        }

        // Overloaded constructor to authenticate login
        public Physio(string username, string password)
        {
            // Set the properties of the Physio object based on the provided parameters
            Username = username;
            Password = password;
        }
    }


}
