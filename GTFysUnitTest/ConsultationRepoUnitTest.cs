using GTFys.Application;
using GTFys.Domain;
using GTFys.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class ConsultationRepoUnitTest
    {
        [TestMethod]
        public async Task ConsultationRepo_BookConsultation_Successful()
        {
            // Arrange 
            ConsultationRepo consultationRepo = new ConsultationRepo();
            // Create instances of Patient
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
            int physioID = 1;  // Replace with the actual physio ID
            UITreatmentType treatmentType = UITreatmentType.FirstConsultation;
            DateTime startTime = new DateTime(2023, 11, 15, 8, 0, 0);  // Randomly in the past

            // Act 
            bool bookingSuccessful = await consultationRepo.BookConsultation(patient, physioID, treatmentType, startTime);

            // Assert 
            Assert.IsTrue(bookingSuccessful, "Consultation booking failed");
        }
    

        [TestMethod]
        public async Task ConsultationRepo_BookConsultation_Failure()
        {
            // Arrange 
            ConsultationRepo consultationRepo = new ConsultationRepo();
            Patient patient = new Patient();  // Expected to fail without patientID
            int physioID = 1;  // Replace with the actual physio ID
            UITreatmentType treatmentType = UITreatmentType.FirstConsultation;

            // Act           
            bool bookingSuccessful = await consultationRepo.BookConsultation(patient, physioID, treatmentType, DateTime.Now);

            // Assert 
            Assert.IsFalse(bookingSuccessful, "Consultation booking unexpectedly succeeded");
        }

        //[TestMethod]
        //public async Task ConsultationRepo_DeleteOldestConsultation_Successful()
        //{
        //    // Arrange
        //    ConsultationRepo consultationRepo = new ConsultationRepo();
        //    DatabaseAccess dbAccess = new DatabaseAccess();

        //    // Act
        //    int? oldestConsultationID = await dbAccess.GetOldestConsultationIDAsync();
        //    bool deletionSuccessful = false;

        //    if (oldestConsultationID.HasValue) {
        //        deletionSuccessful = await consultationRepo.DeleteConsultation(oldestConsultationID.Value);
        //    }

        //    // Assert
        //    Assert.IsTrue(deletionSuccessful, "Consultation deletion failed");
        //}

    }

}
