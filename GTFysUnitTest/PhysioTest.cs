using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class PhysioTest
    {
        [TestMethod]
        public void PhysioInitializationTest()
        {
            // Arrange 
            string cpr = "9876543210";
            string firstName = "Jane";
            string lastName = "Smith";
            string username = "jane_smith";
            string password = "securepass";
            string phone = "987654321";
            string email = "jane.smith@example.com";
            string address = "456 Oak St";
            string city = "Townsville";
            int zipCode = 54321;
            string profilePicturePath = "path/to/physio/profile/picture.jpg";

            // Act 
            Physio physio = new Physio(cpr, firstName, lastName, username, password, phone, email, address, city, zipCode, profilePicturePath);

            // Assert 
            Assert.AreEqual(cpr, physio.CPR, "CPR property not set correctly");
            Assert.AreEqual(firstName, physio.FirstName, "FirstName property not set correctly");
            Assert.AreEqual(lastName, physio.LastName, "LastName property not set correctly");
            Assert.AreEqual(username, physio.Username, "Username property not set correctly");
            Assert.AreEqual(password, physio.Password, "Password property not set correctly");
            Assert.AreEqual(phone, physio.Phone, "Phone property not set correctly");
            Assert.AreEqual(email, physio.Email, "Email property not set correctly");
            Assert.AreEqual(address, physio.Address, "Address property not set correctly");
            Assert.AreEqual(city, physio.City, "City property not set correctly");
            Assert.AreEqual(zipCode, physio.ZipCode, "ZipCode property not set correctly");
            Assert.AreEqual(profilePicturePath, physio.ProfilePicture, "ProfilePicture property not set correctly");
        }
    }

}
