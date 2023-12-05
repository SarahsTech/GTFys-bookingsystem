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
    /// Interaction logic for CreatePatientWindow.xaml
    /// </summary>
    public partial class CreatePatientWindow : Window
    {
        public CreatePatientWindow()
        {
            InitializeComponent();
        }

        // Instantiate the PatientRepo for handling patient data
        PatientRepo patientRepo = new PatientRepo();

        private async void btnCreatePatient_Click(object sender, RoutedEventArgs e)
        {
            try {
                // Retrieve input from the textboxes
                string firstName = tbFirstName.Text;
                string lastName = tbLastName.Text;
                string username = tbUsername.Text;
                string password = tbPassword.Text;
                string email = tbEmail.Text;
                string phone = tbPhone.Text;
                string cpr = tbCPR.Text;
                string address = tbAddress.Text;
                int zipCode = int.Parse(tbZipCode.Text);
                string city = tbCity.Text;
                string imagePath = tbProfilePicture.Text;

                // Call the PatientCreateUser method to attempt user creation
                bool isUserCreated = await patientRepo.PatientCreateUser(
                    firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath);

                // Check if the user was created successfully
                if (isUserCreated) {
                    // Show a success message to the user
                    MessageBox.Show("Patient profil oprettet!", "Opret patient", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Close the current window
                    this.Close();
                }
                else {
                    // Show an error message to the user
                    MessageBox.Show("Fejl ved oprettelse af patient. \nLæs venligst informationen igennem og prøv igen.", "Fejl ved oprettelse", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                // Handle exceptions and show an error message
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Close this window
            this.Close();
        }
    }
}
