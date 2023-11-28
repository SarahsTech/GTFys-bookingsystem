using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    public class Consultation
    {
        public Patient Patient { get; set; }
        
        public Physio Physio { get; set; }

        public TreatmentType TreatmentType { get; set; }
        
        public DateTime DateTime { get; set; }

        // Constructor to initiliaze Consultation properties
        public Consultation(Patient patient, Physio physio, TreatmentType treatmenType, DateTime dateTime)
        {
            Patient = patient;
            Physio = physio;
            TreatmentType = treatmenType;
            DateTime = dateTime;
        }
    }
}