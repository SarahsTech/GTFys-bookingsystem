using GTFys.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;
            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
                // Show an error message to the user
                MessageBox.Show("Vælg en ugedag! \nPrøv igen.", "Booking mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgAvailableTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            // Check if both checkboxes are unchecked or only one is checked
            if (!(cbFysio1.IsChecked == true && cbFysio2.IsChecked == true) || (cbFysio1.IsChecked == false && cbFysio2.IsChecked == false)) {
                // Set IsChecked property of both checkboxes to true
                cbFysio1.IsChecked = true;
                cbFysio2.IsChecked = true;
            }
            else {
                // Uncheck both checkboxes
                cbFysio1.IsChecked = false;
                cbFysio2.IsChecked = false;
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
            calendarView.SelectedDatesChanged += calendarView_SelectionChanged;
        }

        private void calendarView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;

            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            //Close the window an go back to the patient window
            this.Close();
            PatientsOverview patientsOverview = new PatientsOverview();
            Content = patientsOverview;
        }

        private void calendarView_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
