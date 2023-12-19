using GTFys.Domain;
using GTFys.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static GTFys.UI.PatientBookConsultation;

namespace GTFys.Application
{
    public class ConsultationRepo
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        public ConsultationRepo()
        {

        }

        // Method to book a consultation 
        public async Task<bool> BookConsultation(Patient patient, int physioID, UITreatmentType treatmentType, DateTime startTime)
        {
            // Determine TreatmentType
            TreatmentType type = MapToModelTreatmentType(treatmentType);

            Consultation consultation = new Consultation(patient, physioID, type, startTime);

            try {
                var query = "gtspBookConsultation";

                var parameters = new {
                    TreatmentType = consultation.TreatmentType.ToString(),
                    StartTime = consultation.StartTime,
                    EndTime = consultation.EndTime,
                    Duration = consultation.Duration,
                    Price = consultation.Price,
                    PhysioID = physioID,
                    PatientID = consultation.Patient.PatientID,
                };

                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters, CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        // Method to delete a consultation
        public async Task<bool> DeleteConsultation(int consultationID)
        {
            try {
                var query = "gtspDeleteConsultation";

                // Create parameters
                var parameters = new {
                    ConsultationID = consultationID
                };

                // Execute the stored procedure to delete the patient
                var rowsAffected = await dbAccess.ExecuteNonQueryAsync(query, parameters, CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }


        // Maps the UITreatmentType to Consultation TreatmentType
        public static TreatmentType MapToModelTreatmentType(UITreatmentType uiTreatmentType)
        {
            switch (uiTreatmentType) {
                case UITreatmentType.FirstConsultation:
                    return TreatmentType.FirstConsultation;
                case UITreatmentType.TrainingInstruction:
                    return TreatmentType.TrainingInstruction;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uiTreatmentType), uiTreatmentType, null);
            }
        }


    }
}
