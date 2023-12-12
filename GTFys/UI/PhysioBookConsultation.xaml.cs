using GTFys.Application;
using GTFys.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PhysioBookConsultation.xaml
    /// </summary>
    ///  
    public partial class PhysioBookConsultation : Window
    {
        public PhysioBookConsultation()
        {
            InitializeComponent();

            // Set DataContext to the current patient
            DataContext = PatientService.CurrentPatient;

            // Calls AddBlackOutDates() to black out weekends for booking
            AddBlackOutDates();
        }
        // Create an instance of the PhysioRepo 
        PhysioRepo physioRepo = new PhysioRepo();

        private async void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {         
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;
            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
                selectedDate = null;
                // Show an error message to the user
                MessageBox.Show("Vælg en ugedag! \nPrøv igen.", "Booking mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if a treatment type has been selected
            bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;

            // Check if at least one physio has been selected
            bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;

            bool isDateSelected = selectedDate.HasValue;

            // Check if an item is selected in the DataGrid
            bool isItemSelected = dgAvailableTimes.SelectedItem != null;

            int physioID1 = 0, physioID2 = 0, duration = 0;

            // Variable to hold the date and time combined
            DateTime selectedDateTime = DateTime.MinValue; // or any other default value

            // Check if an item is selected in the DataGrid
            if (dgAvailableTimes.SelectedItem != null) {
                // Assuming "TimeColumn" is the name of the column containing the time
                string timeString = ((DataRowView)dgAvailableTimes.SelectedItem)["Ledige tider"].ToString();

                // Parse the time string to a TimeSpan
                if (TimeSpan.TryParse(timeString, out TimeSpan selectedTime)) {

                    // Check if a date is selected
                    if (selectedDate.HasValue) {
                        // Combine the selected date and time
                        selectedDateTime = selectedDate.Value.Date + selectedTime;

                    }
                }
            }

            // Check which checkboxes are checked and set PhysioID's
            if (!(cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == true)) {
                // Set PhysioID1 to 1 and PhysioID2 to 2
                physioID1 = 1;
                physioID2 = 2;
            }
            else if (cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == false) {
                // Set PhysioID1 to 1
                physioID1 = 1;
            }
            else if (cbPhysio2.IsChecked == true && cbPhysio1.IsChecked == false) {
                // Set PhysioID2 to 2
                physioID1 = 2;
            }

            // Holds the chosen TreatmentType
            UITreatmentType treatmentType = UITreatmentType.FirstConsultation; // Default value

            // Check which treatment type is selected and set duration 
            if (rbFirstConsultation.IsChecked == true) {
                treatmentType = UITreatmentType.FirstConsultation;
            }
            else if (rbTrainingInstruction.IsChecked == true) {
                treatmentType = UITreatmentType.TrainingInstruction;
            }

            // Result of PhysioBookConsultation 
            bool isConsultationBooked = false; 

            // Additional conditions for your if statement
            if (isTreatmentTypeSelected && isPhysioSelected && isDateSelected && isItemSelected) {
                // If values are selected, display available times
                if (physioID1 > 0 && selectedDateTime > DateTime.MinValue && duration > 0) {
                    // Call the PhysioBookConsultation method to attempt booking
                        isConsultationBooked = await physioRepo.PhysioBookConsultation(
                        PatientService.CurrentPatient,
                        PhysioService.CurrentPhysio,
                        treatmentType, selectedDateTime); 
                }
            }

            // Check if consultation is booked succesfully 
            if (isConsultationBooked) {
                // Show a success message to the user
                MessageBox.Show("Booking af konsultation bekræftet!", "Booking bekræftelse", MessageBoxButton.OK, MessageBoxImage.Information);

                //// Close the current window (CreateUserWindow)
                //this.Close();

                //// Open a new instance of the PatientLoginWindow
                //PatientLoginWindow patientLoginWindow = new PatientLoginWindow();
                //patientLoginWindow.Show();
            }
            else {
                // Show an error message to the user
                MessageBox.Show("Fejl ved booking af konsultation. \nLæs venligst informationen igennem og prøv igen.", "Fejl ved booking", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void dgAvailableTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;

            // Check if a treatment type has been selected
            bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;

            // Check if at least one physio has been selected
            bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;

            bool isDateSelected = selectedDate.HasValue;

            // Check if an item is selected in the DataGrid
            bool isItemSelected = dgAvailableTimes.SelectedItem != null;

            if (isTreatmentTypeSelected && isPhysioSelected && isDateSelected && isItemSelected) {
                btnBookConsultation.IsEnabled = true;
            }
            else {
                btnBookConsultation.IsEnabled = false;
            }
        }
        private void rbFirstConsultation_Checked(object sender, RoutedEventArgs e)
        {
            // After updating the treatment type, call GetAvailableTimes
            GetAvailableTimes();
        }

        private void rbTrainingInstruction_Checked(object sender, RoutedEventArgs e)
        {
            // After updating the treatment type, call GetAvailableTimes
            GetAvailableTimes();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            // Check if both checkboxes are unchecked or only one is checked
            if (!(cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == true) || (cbPhysio1.IsChecked == false && cbPhysio2.IsChecked == false)) {
                // Set IsChecked property of both checkboxes to true
                cbPhysio1.IsChecked = true;
                cbPhysio2.IsChecked = true;
            }
            else {
                // Uncheck both checkboxes
                cbPhysio1.IsChecked = false;
                cbPhysio2.IsChecked = false;
            }
            // After updating the checkboxes, call GetAvailableTimes
            GetAvailableTimes();
        }

        private void CheckboxSelectionChanged(object sender, RoutedEventArgs e)
        {
            // After updating the checkboxes, call GetAvailableTimes
            GetAvailableTimes();
        }

        // Black out weekends for booking
        private void AddBlackOutDates()
        {
            DateTime d1 = new DateTime(2023, 01, 01);
            List<DateTime> dateStart = new List<DateTime>();

            while (d1.Year == 2023) {
                if ((d1.DayOfWeek == DayOfWeek.Saturday) || (d1.DayOfWeek == DayOfWeek.Sunday))
                    dateStart.Add(d1);
                d1 = d1.AddDays(1);
            }

            foreach (DateTime dateTime in dateStart) {
                calendarView.BlackoutDates.Add(new CalendarDateRange(dateTime));
            }

            // Attach the SelectionChanged event handler
            calendarView.SelectedDatesChanged += calendarView_SelectedDatesChanged;
        }

        private void calendarView_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;

            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
                selectedDate = null;
            }

            // Check if a treatment type has been selected
            bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;

            // Check if at least one physio has been selected
            bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;

            // Additional conditions for your if statement
            if (isTreatmentTypeSelected && isPhysioSelected && selectedDate != null) {
                GetAvailableTimes();
            }
            else {
                return; 
            }

        }

        private async void GetAvailableTimes()
        {
            int physioID1 = 0, physioID2 = 0, duration = 0;
            DateTime? date = calendarView.SelectedDate;

            // Check which checkboxes are checked and set PhysioID's
            if (!(cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == true)) {
                // Set PhysioID1 to 1 and PhysioID2 to 2
                physioID1 = 1; 
                physioID2 = 2;
            }
            else if (cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == false) {
                // Set PhysioID1 to 1
                physioID1 = 1;
            }
            else if (cbPhysio2.IsChecked == true && cbPhysio1.IsChecked == false) {
                // Set PhysioID2 to 2
                physioID1 = 2;
            }

            // Check which treatment type is selected and set duration 
            if (rbFirstConsultation.IsChecked == true) {
                // Set duration to 60 
                duration = 60; 
            }
            else if (rbTrainingInstruction.IsChecked == true) {
                // Set duration to 45 
                duration = 45;
            }

            // If values are selected, display available times
            if(physioID1 > 0 && date != null && duration > 0) {

                try {
                    // Establish a new database connection
                    using (IDbConnection connection = new DatabaseConnection().Connect()) {

                        string query = "gtspGetAvailableConsultationTimes";

                        // Create a new SQL command using the provided query and connection
                        using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {

                            // Add parameters to the query
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@PhysioID1", physioID1);
                            command.Parameters.AddWithValue("@PhysioID2", physioID2);
                            command.Parameters.AddWithValue("@Date", date);
                            command.Parameters.AddWithValue("@Duration", duration);

                            // Create a datatable and read from the database
                            DataTable dt = new DataTable();
                            SqlDataReader reader = command.ExecuteReader();
                            // Load the content to the datatable
                            dt.Load(reader);
                            dgAvailableTimes.ItemsSource = dt.DefaultView;

                        }
                    }
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                    MessageBox.Show("Der opstod en fejl ved indlæsning af ledige tider! \n" + ex.Message);
                }
            }          
        }

        // TreatmentType to map the type of treatment
        public enum UITreatmentType
        {
            FirstConsultation,
            TrainingInstruction
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            //Close the window an go back to the patient window
            this.Close();
            PatientsOverview patientsOverview = new PatientsOverview();
            Content = patientsOverview;
        }

    }
}
