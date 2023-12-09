using GTFys.Application;
using Microsoft.Win32;
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
            try {

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
                if (!string.IsNullOrEmpty(PatientService.CurrentPatient?.ProfilePicture)) {
                    byte[] imageBytes = Convert.FromBase64String(PatientService.CurrentPatient?.ProfilePicture);
                    iProfilePicture.Source = GetImageFromDatabase(imageBytes);
                }    
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        // Event handler for the button to go back
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of LoginWindow
            PatientFrontPageWindow frontPage = new PatientFrontPageWindow();
            // Close the current window hosting the page
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null) {
                frontPage.Show();
                parentWindow.Close();
            }
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the logged-in user's CPR
                int patientID = PatientService.CurrentPatient.PatientID;

                // Show a message with the option to accept or cancel
                MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil slette din profil?", "Slet profil", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (result == MessageBoxResult.OK) {
                    // User has confirmed
                    // Check if PatientiD is available
                    if (patientID != null) {

                        // Create an instance of your repository
                        PatientRepo patientRepo = new PatientRepo();

                        // Call the repository method to delete the profile
                        bool isDeleted = await patientRepo.DeletePatientProfile(patientID);

                        if (isDeleted) {
                            // Show a success message to the user
                            MessageBox.Show("Din profil blev slettet!", "Slet profil", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Set the current patient to null
                            PatientService.CurrentPatient = null;

                            // Open a new instance of the login window if needed
                            PatientLoginWindow loginWindow = new PatientLoginWindow();

                            // Close the current window hosting the page
                            Window parentWindow = Window.GetWindow(this);
                            if (parentWindow != null) {
                                loginWindow.Show();
                                parentWindow.Close();
                            }

                        }
                        else {
                            // Show an error message to the user
                            MessageBox.Show("Sletning mislykkedes. \nPrøv venligst igen.", "Sletning mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    
                }
                else {
                    // User has canceled the delete profile operation
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
