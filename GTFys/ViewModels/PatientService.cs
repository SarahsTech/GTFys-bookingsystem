using GTFys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.ViewModels
{
    // Singleton service to hold the patient object
    public class PatientService
    {
        // Static property to hold the current patient object for the entire application
        private static Patient _currentPatient;

        // Property to get or set the current patient object
        public static Patient CurrentPatient
        {
            // Getter for accessing the current patient object
            get
            { return _currentPatient; }

            // Setter for updating the current patient object
            set
            { _currentPatient = value; }
        }
    }

}
