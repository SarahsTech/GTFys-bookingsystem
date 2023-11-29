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
        public int PhysioID { get; set; }                // Unique identifier for the physiotherapist
        public string PhysioCPR { get; set; }            // CPR number of the physiotherapist
        public string PhysioFirstName { get; set; }      // First name of the physiotherapist
        public string PhysioLastName { get; set; }       // Last name of the physiotherapist
        public string PhysioUsername { get; set; }       // Username chosen by the physiotherapist
        public string PhysioPassword { get; set; }       // Password for physiotherapist authentication
        public int PhysioPhoneNumber { get; set; }       // Phone number of the physiotherapist
        public string PhysioEmail { get; set; }          // Email address of the physiotherapist
        public string PhysioAddress { get; set; }        // Residential address of the physiotherapist
        public string PhysioCity { get; set; }           // City of residence for the physiotherapist
        public int PhysioZipCode { get; set; }        // Postal code of the physiotherapist's location
        public string PhysioProfilePicture { get; set; } // Profile picture uploaded by physiotherapist or generated 

        // Constructor to initialize a Physio object with essential attributes
        public Physio(int physioID, string physioCPR, string physioFirstName, string physioLastName, string physioUsername, string physioPassword,
            int physioPhoneNumber, string physioEmail, string physioAddress, string physioCity, int physioZipCode, string imagePath)
        {
            // Set the properties of the Physio object based on the provided parameters
            PhysioID = physioID;
            PhysioCPR = physioCPR;
            PhysioFirstName = physioFirstName;
            PhysioLastName = physioLastName;
            PhysioUsername = physioUsername;
            PhysioPassword = physioPassword;
            PhysioPhoneNumber = physioPhoneNumber;
            PhysioEmail = physioEmail;
            PhysioAddress = physioAddress;
            PhysioCity = physioCity;
            PhysioZipCode = physioZipCode;
            PhysioProfilePicture = imagePath; //  imagePath: Path to the profile picture of the physiotherapist
        }
    }

}
