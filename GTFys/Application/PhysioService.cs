using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFys.Application
{
    // Singleton service to hold the physio object
    public class PhysioService
    {
        // Static property to hold the current physio object for the entire application
        private static Physio _currentPhysio;

        // Property to get or set the current physio object
        public static Physio CurrentPhysio
        {
            // Getter for accessing the current physio object
            get
            {   return _currentPhysio;   }

            // Setter for updating the current physio object
            set
            {   _currentPhysio = value; }
        }
    }

}
