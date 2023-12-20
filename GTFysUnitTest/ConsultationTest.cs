using GTFys.Application;
using GTFys.Domain;
using System.Windows;

namespace GTFysUnitTest
{
    [TestClass]
    public class ConsultationTest
    {
        // Domain layer: Consultation 
        [TestMethod]
        public void ConsultationTest1()
        {
            // Arrange 
            // Create instances of Patient and Physio for the consultation
            Patient patient = new Patient {
                PatientID = 69,
                CPR = "1234567890",
                FirstName = "Lars",
                LastName = "Andersen",
                Username = "lars123",
                Password = "secure123",
                Phone = "12345678",
                Email = "lars@example.com",
                Address = "Hovedgaden 123",
                City = "København",
                ZipCode = 1000,
                ProfilePicture = null
            };

            Physio physio = new Physio {
                PhysioID = 1,
                CPR = "9876543210",
                FirstName = "Henrik",
                LastName = "Hougs Kjær",
                Username = "gtfyshk",
                Password = "gtfyshk",
                Phone = "+4529402103",
                Email = "henrik@gtfys.com",
                Address = "Dyssegårdsvej 119",
                City = "Dyssegård",
                ZipCode = 2900,
                ProfilePicture = null
            };

            // Choose a treatment type for the consultation               
            TreatmentType treatmentType = TreatmentType.FirstConsultation; 

            // Set a realistic start time for the consultation
            DateTime startTime = new DateTime(2024, 1, 15, 8, 0, 0);

            // Act 
            // Create an instance of the Consultation class
            GTFys.Domain.Consultation consultation = new GTFys.Domain.Consultation(patient, physio, treatmentType, startTime);

            // Assert 
            // Check if the properties of the Consultation class are set correctly
            Assert.AreEqual(patient, consultation.Patient, "Patient property not set correctly");
            Assert.AreEqual(physio, consultation.Physio, "Physio property not set correctly");
            Assert.AreEqual(treatmentType, consultation.TreatmentType, "TreatmentType property not set correctly");
            Assert.AreEqual(startTime, consultation.StartTime, "StartTime property not set correctly");

            // Check if the Duration is calculated and set correctly
            // Duration property expected as 60 
            Assert.AreEqual(60, consultation.Duration, "Duration property not set correctly");

            // Check if the EndTime is calculated and set correctly
            // // Price property expected as StartTime + 60 minutes 
            Assert.AreEqual(startTime.AddMinutes(60), consultation.EndTime, "EndTime property not set correctly");

            // Check if the Price is set correctly based on the TreatmentType
            // Price property expected as 800 
            Assert.AreEqual(800, consultation.Price, "Price property not set correctly");
        }

    }
}