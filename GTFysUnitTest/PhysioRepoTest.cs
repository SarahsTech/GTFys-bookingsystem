using GTFys.Application;
using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class PhysioRepoTest
    {
        // Application layer: PhysioRepo 
        [TestMethod]
        public async Task PhysioRepo_AuthenticateLogin_Successful()
        {
            // Arrange 
            PhysioRepo physioRepo = new PhysioRepo();

            // Set up physio credentials for authentication (using Henrik Hougs Kjær's information)
            string username = "gtfyshk";
            string password = "gtfyshk";

            // Act 
            bool isAuthenticated = await physioRepo.PhysioAuthenticateLogin(username, password);

            // Assert 
            // Check if authentication is successful
            Assert.IsTrue(isAuthenticated, "Physio authentication failed");
        }

        [TestMethod]
        public async Task PhysioRepo_AuthenticateLogin_Failure()
        {
            // Arrange 
            PhysioRepo physioRepo = new PhysioRepo();

            // Set up incorrect physio credentials for authentication
            // (You may choose invalid credentials for a failed authentication)
            string username = "invalid_user";
            string password = "invalid_password";

            // Act 
            bool isAuthenticated = await physioRepo.PhysioAuthenticateLogin(username, password);

            // Assert 
            // Check if authentication fails
            Assert.IsFalse(isAuthenticated, "Physio authentication unexpectedly succeeded");
        }

        [TestMethod]
        public async Task PhysioRepo_UpdateUser_Successful()
        {
            // Arrange 
            PhysioRepo physioRepo = new PhysioRepo();

            // Set up physio information for updating (using Henrik Hougs Kjær's information)
            string firstName = "Henrik";
            string lastName = "Hougs Kjær";
            string username = "gtfyshk";
            string password = "gtfyshk";
            string email = "henrik@gtfys.com";
            string phone = "+4529402103";
            string cpr = "9876543210";
            string address = "Dyssegårdsvej 119";
            int zipCode = 2870;
            string city = "Dyssegård";
            string imagePath = null;

            // Act 
            bool updateSuccessful = await physioRepo.PhysioUpdateUser(
                firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath
            );

            // Assert 
            Assert.IsTrue(updateSuccessful, "Physio information update failed");
        }


    }
}
