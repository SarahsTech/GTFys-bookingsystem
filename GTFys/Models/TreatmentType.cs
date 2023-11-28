using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    // The TreatmentType class represents a type of treatment with specific attributes
    public class TreatmentType
    {
        // Private properties representing attributes of a treatment type
        private string Type { get; set; }       // The type or name of the treatment
        private TimeSpan Time { get; set; }     //The duration of the treatment (represented as a TimeSpan)
        private decimal Cost { get; set; }      // The cost of the treatment

        // Constructor to initialize a TreatmentType object
        public TreatmentType(string type, TimeSpan time, decimal cost)
        {
            // Set the private properties of the TreatmentType object based on the provided parameters
            Type = type;
            Time = time;
            Cost = cost;
        }
    }

}
