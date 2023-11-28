using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Models
{
    public class TreatmentType
    {
        private string Type { get; }

        private TimeSpan Time { get; }

        private decimal Cost { get; }

        // Constructor to initiliaze TreatmentType properties
        public TreatmentType(string type, TimeSpan time, decimal cost)
        {
            Type = type;
            Time = time;
            Cost = cost;
        }

    }
}
