using GTFys.Application;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PatientFrontPageWindow.xaml
    /// </summary>
    public partial class PatientFrontPageWindow : Window
    {
        public PatientFrontPageWindow()
        {
            InitializeComponent();

            // Load content of the front page
            LoadFrontPage();
        }

        // Event handler for the button to navigate to the patient profile page
        private void btnProfilePage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PatientProfilePage
            PatientProfilePage patientProfilePage = new PatientProfilePage();
            this.Content = patientProfilePage;
        }

        // Event handler for the button to log out
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Show a message with the option to accept or cancel
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil logge ud?", "Log ud", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK)
            {
                // User has confirmed

                // Set the current patient to null
                PatientService.CurrentPatient = null;

                // Open a new instance of LoginWindow
                PatientLoginWindow loginWindow = new PatientLoginWindow();
                // Close the current window hosting the page
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null) {
                    loginWindow.Show();
                    parentWindow.Close();
                }
            }
            else
            {
                // User has canceled the log-out operation
            }
        }

        private void LoadFrontPage()
        {
            // Load the profile picture of the logged in patient (CurrentPatient)
            if (!string.IsNullOrEmpty(PatientService.CurrentPatient?.ProfilePicture)) {
                byte[] imageBytes = Convert.FromBase64String(PatientService.CurrentPatient?.ProfilePicture);
                imgProfilePicture.Source = GetImageFromDatabase(imageBytes);
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
    }
}
