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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PatientProfilePage.xaml
    /// </summary>
    public partial class PatientProfilePage : Page
    {
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
            // Call the PatientRepo to update patient information
            bool updateSuccessful = await patientRepo.PatientUpdateUser(tbFirstName.Text, tbLastName.Text, tbUsername.Text,
                tbPassword.Text, tbEmail.Text, tbPhone.Text, tbCPR.Text, tbAddress.Text, Convert.ToInt32(tbZipCode.Text), tbCity.Text, tbProfilePicture.Text);

            if (updateSuccessful)
            {
                // Show a success message to the user
                MessageBox.Show("Din opdatering succesfuld!", "Succesfuld opdatering", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload the patient information to update the displayed values
                LoadPatientInfo();
            }
            else
            {
                // Show an error message to the user
                MessageBox.Show("Fejl ved opdatering. Læs venligst informationen igennem og prøv igen.", "Fejl ved opdatering", MessageBoxButton.OK, MessageBoxImage.Error);
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
            tbProfilePicture.Text = PatientService.CurrentPatient?.ProfilePicture;
        }

        // Event handler for the button to go back
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of PatientFrontPageWindow
            PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
            patientFrontPageWindow.Show();
        }

        private async void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
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

                    if (isDeleted)
                    {
                        MessageBox.Show("Profil er slettet!");
                        // Optionally, navigate to another window or perform additional actions here.
                    }
                    else
                    {
                        MessageBox.Show("Det lykkedes ikke at slette profilen.");
                    }
                }
                else
                {
                    MessageBox.Show("Fejl: Kunne ikke hente CPR for den loggede ind bruger.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


    }

}
