using GTFys.ViewModels;
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
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        public CreateUserWindow()
        {
            InitializeComponent();
        }

        // Event handler for the back button
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of PatientFrontPageWindow
            PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
            patientFrontPageWindow.Show();
        }

        // Event handler for the button click to create a new user
        private async void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user input from the textboxes
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

                // Create an instance of the PatientRepo for handling patient-related operations
                PatientRepo patientRepo = new PatientRepo();

                // Call the PatientCreateUser method to attempt user creation
                bool isUserCreated = await patientRepo.PatientCreateUser(
                    firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath
                );

                // Check if the user was created successfully
                if (isUserCreated)
                {
                    // Show a success message
                    MessageBox.Show("Profil er oprettet!");

                    // Close the current window (CreateUserWindow)
                    this.Close();

                    // Open a new instance of the PatientLoginWindow
                    PatientLoginWindow patientLoginWindow = new PatientLoginWindow();
                    patientLoginWindow.Show();
                }
                else
                {
                    // Show an error message if user creation failed
                    MessageBox.Show("Det lykkedes ikke at oprette en profil.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and show an error message
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


    }
}
