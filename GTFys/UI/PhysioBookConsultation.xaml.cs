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
        // Create an instance of ConsultationRepo
        ConsultationRepo consultationRepo = new ConsultationRepo();

        private async void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {     

            // Get the selected date and time combined
            DateTime selectedDateTime = GetSelectedDateTime();

            if (selectedDateTime == null) {
                return;
            }

            // Check which checkboxes are checked and set PhysioID's
            SetPhysioIDs(out int physioID1, out int physioID2);

            // Check which treatment type is selected and set duration
            (UITreatmentType treatmentType, int duration) = GetSelectedTreatmentTypeAndDuration();

            // Result of PhysioBookConsultation 
            bool isConsultationBooked = false;

            // If values are selected, display available times
            if (physioID1 > 0 && selectedDateTime > DateTime.MinValue && duration > 0) {
                // Call the PhysioBookConsultation method to attempt booking
                isConsultationBooked = await consultationRepo.BookConsultation(
                    PatientService.CurrentPatient,
                    physioID1,
                    treatmentType, selectedDateTime);
            }

            // Check if consultation is booked succesfully 
            if (isConsultationBooked) {
                // Show a success message to the user
                MessageBox.Show("Booking af konsultation bekræftet!", "Booking bekræftelse", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open a new instance of the PhysioFrontPage
                PhysioFrontPageWindow frontPage = new PhysioFrontPageWindow();
                frontPage.Show();

                // Close the current window
                this.Close();
            }
            else {
                // Show an error message to the user
                MessageBox.Show("Fejl ved booking af konsultation. \nLæs venligst informationen igennem og prøv igen.", "Fejl ved booking", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DateTime GetSelectedDateTime()
        {
            DateTime selectedDateTime = DateTime.MinValue;

            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;

            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
                selectedDate = null;
                // Show an error message to the user
                MessageBox.Show("Vælg en ugedag! \nPrøv igen.", "Booking mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                return DateTime.MinValue;
            }

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

            return selectedDateTime;
        }

        private void SetPhysioIDs(out int physioID1, out int physioID2)
        {
            physioID1 = 0;
            physioID2 = 0;

            if (cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == true) {
                physioID1 = 1;
                physioID2 = 2;
                return; 
            } else if (cbPhysio1.IsChecked == true && cbPhysio2.IsChecked == false) {
                physioID1 = 1;
                return; 
            } else if(cbPhysio1.IsChecked == false && cbPhysio2.IsChecked == true) {
                physioID1 = 2;
                return; 
            }
        }
        private (UITreatmentType, int) GetSelectedTreatmentTypeAndDuration()
        {
            UITreatmentType treatmentType = UITreatmentType.FirstConsultation;
            int duration = 0;

            // Check which treatment type is selected and set duration
            if (rbFirstConsultation.IsChecked == true) {
                treatmentType = UITreatmentType.FirstConsultation;
                duration = 60; // Set the duration for FirstConsultation in minutes
            }
            else if (rbTrainingInstruction.IsChecked == true) {
                treatmentType = UITreatmentType.TrainingInstruction;
                duration = 45; // Set the duration for TrainingInstruction in minutes
            }

            return (treatmentType, duration);
        }

        private void dgAvailableTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if a treatment type has been selected
            bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;
            // Check if at least one physio has been selected
            bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;
            // Check if a date is selected
            bool isDateSelected = calendarView.SelectedDate.HasValue;
            // Check if an item is selected in the DataGrid
            bool isItemSelected = dgAvailableTimes.SelectedItem != null;
            // Enable or disable the "Book Consultation" button based on conditions
            btnBookConsultation.IsEnabled = isTreatmentTypeSelected && isPhysioSelected && isDateSelected && isItemSelected;
        }

        private void rbFirstConsultation_Checked(object sender, RoutedEventArgs e)
        {
            if (calendarView.SelectedDate != null) {
                if(cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true) {
                    // After updating the treatment type, call GetAvailableTimes
                    GetAvailableTimes();
                }              
            }
        }

        private void rbTrainingInstruction_Checked(object sender, RoutedEventArgs e)
        {
            if (calendarView.SelectedDate != null) {
                if (cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true) {
                    // After updating the treatment type, call GetAvailableTimes
                    GetAvailableTimes();
                }
            }
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            // Check if both checkboxes are unchecked or only one is checked
            if (cbPhysio1.IsChecked == false && cbPhysio2.IsChecked == false) {
                // Set IsChecked property of both checkboxes to true
                cbPhysio1.IsChecked = true;
                cbPhysio2.IsChecked = true;
                // After updating the checkboxes, call GetAvailableTimes
                GetAvailableTimes();
            }else if (cbPhysio1.IsChecked == false || cbPhysio2.IsChecked == false) {
                // Set IsChecked property of both checkboxes to true
                cbPhysio1.IsChecked = true;
                cbPhysio2.IsChecked = true;
                // After updating the checkboxes, call GetAvailableTimes
                GetAvailableTimes();
            }
            else if (cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true) {
                // Uncheck both checkboxes
                cbPhysio1.IsChecked = false;
                cbPhysio2.IsChecked = false;
                dgAvailableTimes.ItemsSource = null; 
            }
        }

        private void CheckboxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (calendarView.SelectedDate != null) {
                if ((rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true) 
                    && (cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true)) {
                    // After updating the checkboxes, call GetAvailableTimes
                    GetAvailableTimes();
                }
                else {
                    // Both checkboxes are unchecked, clear datagrid
                    dgAvailableTimes.ItemsSource = null;
                }
            }
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
            // Use the new methods to get the necessary values
            int physioID1, physioID2, duration;
            DateTime? date = calendarView.SelectedDate;

            SetPhysioIDs(out physioID1, out physioID2);
            (UITreatmentType treatmentType, int tempDuration) = GetSelectedTreatmentTypeAndDuration();
            duration = tempDuration;

            // If values are selected, display available times
            if (physioID1 > 0 && date != DateTime.MinValue && duration > 0) {
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

        
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of the login window if needed
            PhysioFrontPageWindow frontPage = new PhysioFrontPageWindow();

            // Close the current window hosting the page
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null) {
                frontPage.Show();
                parentWindow.Close();
            }
        }

    }

    // TreatmentType to map the type of treatment
    public enum UITreatmentType
    {
        FirstConsultation,
        TrainingInstruction
    }
}
