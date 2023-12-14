using GTFys.Application;
using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PhysioFrontPageWindow.xaml
    /// </summary>
    public partial class PhysioFrontPageWindow : Window
    {

        public PhysioFrontPageWindow()
        {
            InitializeComponent();

            // Load content of the front page
            LoadFrontPage();
        }

        private void btnProfilePage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PhysioProfilePage
            PhysioProfilePage physioProfilePage = new PhysioProfilePage();
            this.Content = physioProfilePage;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Show a message with option to accept or cancel
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil logge ud?", "Log ud", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK) {
                // User has confirmed

                // Set the current physio to null
                PhysioService.CurrentPhysio = null;

                // Open a new instance of LoginWindow
                PhysioLoginWindow physioLoginWindow = new PhysioLoginWindow();
                // Close the current window hosting the page
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null) {
                    physioLoginWindow.Show();
                    parentWindow.Close();
                }
            }
            else {
                // User has cancelled
            }
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

            //// Check if a treatment type has been selected
            //bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;

            //// Check if at least one physio has been selected
            //bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;

            //// Additional conditions for your if statement
            //if (isTreatmentTypeSelected && isPhysioSelected && selectedDate != null) {
            //    GetAvailableTimes();
            //}
            //else {
            //    return;
            //}

        }


        private void dgConstultations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //// Check if a treatment type has been selected
            //bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;
            //// Check if at least one physio has been selected
            //bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;
            //// Check if a date is selected
            //bool isDateSelected = calendarView.SelectedDate.HasValue;
            //// Check if an item is selected in the DataGrid
            //bool isItemSelected = dgAvailableTimes.SelectedItem != null;
            //// Enable or disable the "Book Consultation" button based on conditions
            //btnBookConsultation.IsEnabled = isTreatmentTypeSelected && isPhysioSelected && isDateSelected && isItemSelected;
        }


        private void btnPatients_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PhysioProfilePage
            PatientsOverview patientsOverview = new PatientsOverview();
            this.Content = patientsOverview;
        }

        private void LoadFrontPage()
        {
            // Load the profile picture of the logged in physio (CurrentPhysio)
            
            if (!string.IsNullOrEmpty(PhysioService.CurrentPhysio?.ProfilePicture)) {
                byte[] imageBytes = Convert.FromBase64String(PhysioService.CurrentPhysio?.ProfilePicture);
                imgProfilePicture.Source = GetImageFromDatabase(imageBytes);
            }
            else {
                // Set the default profile picture if the profile picture is null or empty
                imgProfilePicture.Source = new BitmapImage(new Uri("/GTFys;component/Images/DefaultProfilePicture.jpeg", UriKind.Relative));
            }
        }

        // Method to convert bytes to image 
        private BitmapImage GetImageFromDatabase(byte[] imageData)
        {
            BitmapImage image = null;

            using (MemoryStream stream = new MemoryStream(imageData)) {
                image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }
            return image;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
