using GTFys.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class PatientRepoUnitTest
    {
        [TestMethod]
        public async Task PatientRepo_CreateUser_Successful()
        {
            // Arrange 
            PatientRepo patientRepo = new PatientRepo();

            // Set up patient information for creation
            string firstName = "John";
            string lastName = "Doe";
            string username = "johndoe";
            string password = "password123";
            string email = "john.doe@example.com";
            string phone = "+1234567890";
            string cpr = "1234567890";
            string address = "123 Main St";
            int zipCode = 12345;
            string city = "City";
            string imagePath = null;

            // Act 
            bool creationSuccessful = await patientRepo.PatientCreateUser(
                firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath
            );

            // Assert 
            // Check if the creation is successful
            Assert.IsTrue(creationSuccessful, "Patient creation failed");
        }

        [TestMethod]
        public async Task PatientRepo_AuthenticateLogin_Successful()
        {
            // Arrange 
            PatientRepo patientRepo = new PatientRepo();

            // Set up patient information for authentication
            string username = "johndoe";
            string password = "password123";

            // Act 
            bool authenticationSuccessful = await patientRepo.PatientAuthenticateLogin(username, password);

            // Assert 
            // Check if the authentication is successful
            Assert.IsTrue(authenticationSuccessful, "Patient authentication failed");
        }

        [TestMethod]
        public async Task PatientRepo_AuthenticateLogin_Failure()
        {
            // Arrange 
            PatientRepo patientRepo = new PatientRepo();

            // Set up invalid patient information for authentication
            string username = "invalidusername";
            string password = "invalidpassword";

            // Act 
            bool authenticationSuccessful = await patientRepo.PatientAuthenticateLogin(username, password);

            // Assert 
            // Check if the authentication fails
            Assert.IsFalse(authenticationSuccessful, "Patient authentication unexpectedly succeeded");
        }

        [TestMethod]
        public async Task PatientRepo_UpdateUser_Successful()
        {
            // Arrange 
            PatientRepo patientRepo = new PatientRepo();

            // Set up patient information for updating
            string firstName = "UpdatedFirstName";
            string lastName = "UpdatedLastName";
            string username = "johndoe";
            string password = "updated_password";
            string email = "updated_patient@example.com";
            string phone = "+1234567890";
            string cpr = "1234567890";
            string address = "Updated Street 123";
            int zipCode = 12345;
            string city = "Updated City";
            string imagePath = null;

            // Act 
            bool updateSuccessful = await patientRepo.PatientUpdateUser(
                firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath
            );

            // Assert 
            // Check if the update is successful
            Assert.IsTrue(updateSuccessful, "Patient information update failed");
        }
    }

}

