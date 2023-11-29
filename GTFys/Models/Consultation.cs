using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    // Enum to represent the treatment type 
    // The values represents the costs of the treatment type
    public enum TreatmentTypes 
    {
        FirstConsultation = 800,
        TrainingInstruction = 550
    }

    // The Consultation class represents a consultation session
    public class Consultation
    {
        // Properties for the Patient, Physio, TreatmentType, and DateTime of the consultation
        public Patient Patient { get; set; }             // The patient involved in the consultation
        public Physio Physio { get; set; }               // The physiotherapist involved in the consultation
        public TreatmentTypes TreatmentType { get; set; }// The type of treatment provided during the consultation
        public DateTime StartTime { get; set; }          // The date and starttime of the consultation
        public DateTime EndTime { get; set; }            // The date and endtime of the consultation
        public TimeSpan TimeSpan { get; set; }           // Timespan is calculated and set in the constructor, represents the length of a consultation
        public double Price { get; set; }                // Price is set in the constructur determined by the chosen treatment type

        // Constructor to initialize Consultation properties
        public Consultation(Patient patient, Physio physio, TreatmentTypes treatmenType, DateTime startTime)
        {
            // Set the properties of the Consultation object based on the provided parameters
            Patient = patient;
            Physio = physio;
            TreatmentType = treatmenType;
            StartTime = startTime;

            // Calculate and set TimeSpan based on TreatmentType
            SetTimeSpan();

            // Set Price based on TreatmentType
            SetPrice();

            // Calculate and set EndTime based on StartTime and TimeSpan
            EndTime = StartTime + TimeSpan;
        }

        // Method to set the TimeSpan based on TreatmentType
        private void SetTimeSpan()
        {
            // Default timespan
            TimeSpan = TimeSpan.Zero;

            // Update timespan based on TreatmentType
            switch (TreatmentType) {
                case TreatmentTypes.FirstConsultation:
                    // First consultation is 1 hour (60 minutes)
                    TimeSpan = TimeSpan.FromMinutes(60);
                    break;
                case TreatmentTypes.TrainingInstruction:
                    // Training instruction is 45 minutes
                    TimeSpan = TimeSpan.FromMinutes(45);
                    break;
                    // Add more cases if new treatment types are added in the future
            }
        }

        // Method to set the Price based on TreatmentType
        private void SetPrice()
        {
            // Set Price based on the chosen TreatmentType
            switch (TreatmentType) {
                case TreatmentTypes.FirstConsultation:
                    Price = 800;
                    break;
                case TreatmentTypes.TrainingInstruction:
                    Price = 550;
                    break;
                    // Add more cases if new treatment types are added in the future
            }
        }


    }
}
