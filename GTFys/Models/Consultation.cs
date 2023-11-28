using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    // The Consultation class represents a consultation session
    public class Consultation
    {
        // Properties for the Patient, Physio, TreatmentType, and DateTime of the consultation
        public Patient Patient { get; set; }             // The patient involved in the consultation
        public Physio Physio { get; set; }               // The physiotherapist involved in the consultation
        public TreatmentType TreatmentType { get; set; } // The type of treatment provided during the consultation
        public DateTime DateAndTime { get; set; }        // The date and time of the consultation

        // Constructor to initialize Consultation properties
        public Consultation(Patient patient, Physio physio, TreatmentType treatmenType, DateTime dateTime)
        {
            // Set the properties of the Consultation object based on the provided parameters
            Patient = patient;
            Physio = physio;
            TreatmentType = treatmenType;
            DateAndTime = dateTime;
        }
    }
}
