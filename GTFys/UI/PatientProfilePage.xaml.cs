using GTFys.Application;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PatientProfilePage.xaml
    /// </summary>
    public partial class PatientProfilePage : Page
    {
        private string selectedImagePath;

        public PatientProfilePage()
        {
            InitializeComponent();

            // Load patient information when the page is initialized
            LoadPatientInfo();
        }

        // Instantiate the PatientRepo for handling patient data
        PatientRepo patientRepo = new PatientRepo();

        // Event handler for the button to update patient information
        private async void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            bool updateSuccessful = await patientRepo.PatientUpdateUser(tbFirstName.Text, tbLastName.Text, tbUsername.Text,
                tbPassword.Text, tbEmail.Text, tbPhone.Text, tbCPR.Text, tbAddress.Text,
                Convert.ToInt32(tbZipCode.Text), tbCity.Text, selectedImagePath);

            if (updateSuccessful)
            {
                // Show a success message to the user
                MessageBox.Show("Din opdatering succesfuld!", "Succesfuld opdatering", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload the physio information to update the displayed values
                LoadPatientInfo();
            }
            else
            {
                // Show an error message to the user
                MessageBox.Show("Fejl ved opdatering. Læs venligst informationen igennem og prøv igen.", "Fejl ved opdatering", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp",
                Multiselect = false
            };

            // Show OpenFileDialog
            bool? result = openFileDialog.ShowDialog();

            // Check if the user selected a file
            if (result == true)
            {
                // Get the selected file path
                selectedImagePath = openFileDialog.FileName;

                // Load the selected image into the Image control
                iProfilePicture.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        // Method to load patient information into the UI
        private void LoadPatientInfo()
        {
            // Set all values in the textboxes to the values of the logged-in patient (CurrentPatient)
            tbCPR.Text = PatientService.CurrentPatient?.CPR;
            tbFirstName.Text = PatientService.CurrentPatient?.FirstName;
            tbLastName.Text = PatientService.CurrentPatient?.LastName;
            tbUsername.Text = PatientService.CurrentPatient?.Username;
            tbPassword.Text = PatientService.CurrentPatient?.Password;
            tbPhone.Text = PatientService.CurrentPatient?.Phone;
            tbEmail.Text = PatientService.CurrentPatient?.Email;
            tbAddress.Text = PatientService.CurrentPatient?.Address;
            tbCity.Text = PatientService.CurrentPatient?.City;
            tbZipCode.Text = PatientService.CurrentPatient?.ZipCode.ToString();
            selectedImagePath = PhysioService.CurrentPhysio?.ProfilePicture;
        }

        // Event handler for the button to go back
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of PatientFrontPageWindow
            PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
            patientFrontPageWindow.Show();
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the logged-in user's CPR
                string loggedInUserCPR = PatientService.CurrentPatient?.CPR;

                // Check if CPR is available
                if (!string.IsNullOrEmpty(loggedInUserCPR))
                {
                    // Create an instance of your repository
                    PatientRepo patientRepo = new PatientRepo();

                    // Call the repository method to delete the profile
                    bool isDeleted = await patientRepo.DeletePatientProfile(loggedInUserCPR);

                    if (isDeleted) {
                        // Show a success message to the user
                        MessageBox.Show("Din profil blev slettet!", "Slet profil", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else {
                        // Show an error message to the user
                        MessageBox.Show("Sletning mislykkedes. \nPrøv venligst igen.", "Sletning mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        
    }

}
