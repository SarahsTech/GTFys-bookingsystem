using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    public class Physio
    {
        public int PhysioID { get; set; }

        public string PhysioCPR {  get; set; }

        public string PhysioUsername { get; set; }

        public string PhysioPassword { get; set; }

        public string PhysioProfilePicture { get; set; }

        public int PhysioPhoneNumber { get; set; }

        public string PhysioEmail { get; set; }

        public string PhysioAddress { get; set; }

        public string PhysioCity { get; set; }

        public int PhysioPostalCode { get; set; }

        // Constructor to initiliaze physio properties
        public Physio(int physioID, string physioCPR, string physioUsername, string physioPassword,
            int physioPhoneNumber, string physioEmail, string physioAddress, string physioCity, int physioPostalCode)
        {
            // Set the values of physio properties based on constructor parameters
            PhysioID = physioID;
            PhysioCPR = physioCPR;
            PhysioUsername = physioUsername;
            PhysioPassword = physioPassword;
            PhysioPhoneNumber = physioPhoneNumber;
            PhysioEmail = physioEmail;
            PhysioAddress = physioAddress;
            PhysioCity = physioCity;
            PhysioPostalCode = physioPostalCode;
        }

        // Overloaded constructor to initialize physio properties with image 
        public Physio(int physioID, string physioCPR, string physioUsername, string physioPassword, string imagePath,
            int physioPhoneNumber, string physioEmail, string physioAddress, string physioCity, int physioPostalCode)
        {
            // Set the values of physio properties based on constructor parameters
            PhysioID = physioID;
            PhysioCPR = physioCPR;
            PhysioUsername = physioUsername;
            PhysioPassword = physioPassword;
            PhysioProfilePicture = imagePath;
            PhysioPhoneNumber = physioPhoneNumber;
            PhysioEmail = physioEmail;
            PhysioAddress = physioAddress;
            PhysioCity = physioCity;
            PhysioPostalCode = physioPostalCode;
        }
    }
}
