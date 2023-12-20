using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        public void PatientInitializationTest()
        {
            // Arrange 
            string cpr = "1234567890";
            string firstName = "John";
            string lastName = "Doe";
            string username = "john_doe";
            string password = "password123";
            string phone = "123456789";
            string email = "john.doe@example.com";
            string address = "123 Main St";
            string city = "Cityville";
            int zipCode = 12345;
            string profilePicturePath = "path/to/profile/picture.jpg";

            // Act 
            Patient patient = new Patient(cpr, firstName, lastName, username, password, phone, email, address, city, zipCode, profilePicturePath);

            // Assert 
            Assert.AreEqual(cpr, patient.CPR, "CPR property not set correctly");
            Assert.AreEqual(firstName, patient.FirstName, "FirstName property not set correctly");
            Assert.AreEqual(lastName, patient.LastName, "LastName property not set correctly");
            Assert.AreEqual(username, patient.Username, "Username property not set correctly");
            Assert.AreEqual(password, patient.Password, "Password property not set correctly");
            Assert.AreEqual(phone, patient.Phone, "Phone property not set correctly");
            Assert.AreEqual(email, patient.Email, "Email property not set correctly");
            Assert.AreEqual(address, patient.Address, "Address property not set correctly");
            Assert.AreEqual(city, patient.City, "City property not set correctly");
            Assert.AreEqual(zipCode, patient.ZipCode, "ZipCode property not set correctly");
            Assert.AreEqual(profilePicturePath, patient.ProfilePicture, "ProfilePicture property not set correctly");
        }
    }

}
