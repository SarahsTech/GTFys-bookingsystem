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
    /// Interaction logic for PatientLoginWindow.xaml
    /// </summary>
    public partial class PatientLoginWindow : Window
    {
        // Constructor for PatientLoginWindow
        public PatientLoginWindow()
        {
            InitializeComponent();
        }

        // Create an instance of PatientRepo for handling patient-related operations
        PatientRepo patientRepo = new PatientRepo();

        // Event handler for the login button click
        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the entered username and password from the textboxes
            string username = tbPatientUsername.Text;
            string password = tbPatientPassword.Password.ToString();

            // Perform authentication for the patient using PatientRepo
            bool isAuthenticated = await patientRepo.PatientAuthenticateLogin(username, password);

            // Check if authentication was successful
            if (isAuthenticated)
            {
                // If authenticated, open the PatientFrontPageWindow and close the current window
                PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
                patientFrontPageWindow.Show();
                this.Close();
            }
            else
            {
                // If authentication failed, show an error message to the user
                MessageBox.Show("Login fejlede. Kontroller dit brugernavn og adgangskode og prøv igen.", "Login Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the CreateUserWindow
            CreateUserWindow createUserWindow = new CreateUserWindow();

            // Show the CreateUserWindow
            createUserWindow.Show();

            // Close the current window
            this.Close();
        }

    }


}
