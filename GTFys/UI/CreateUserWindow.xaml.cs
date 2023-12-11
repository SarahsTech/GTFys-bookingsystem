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

        // Create an instance of the PatientRepo for handling patient-related operations
        PatientRepo patientRepo = new PatientRepo();

        // Event handler for the back button
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Close the current window (CreateUserWindow)
            this.Close();


            // PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
            // patientFrontPageWindow.Show();
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
                string imagePath = (ProfilePicture.Source as BitmapImage)?.UriSource?.LocalPath;

                // Call the PatientCreateUser method to attempt user creation
                bool isUserCreated = await patientRepo.PatientCreateUser(
                    firstName, lastName, username, password, email, phone, cpr, address, zipCode, city, imagePath
                );

                // Check if the user was created successfully
                if (isUserCreated) {
                    // Show a success message to the user
                    MessageBox.Show("Profil oprettet!", "Opret profil", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Close the current window (CreateUserWindow)
                    this.Close();

                    // Open a new instance of the PatientLoginWindow
                    PatientLoginWindow patientLoginWindow = new PatientLoginWindow();
                    patientLoginWindow.Show();
                }
                else {
                    // Show an error message to the user
                    MessageBox.Show("Fejl ved oprettelse af profil. \nLæs venligst informationen igennem og prøv igen.", "Fejl ved oprettelse", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and show an error message
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an instance of OpenFileDialog
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Set the filter to allow only image files
                openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All Files (*.*)|*.*";

                // Show the dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == true)
                {
                    // Get the selected file path
                    string imagePath = openFileDialog.FileName;

                    // Set the source of the Image control
                    ProfilePicture.Source = new BitmapImage(new Uri(imagePath));
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
