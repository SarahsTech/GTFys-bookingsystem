using GTFys.Application;
using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for PhysioLoginWindow.xaml
    /// </summary>
    public partial class PhysioLoginWindow : Window
    {
        public PhysioLoginWindow()
        {
            InitializeComponent();
        }
        PhysioRepo physioRepo = new PhysioRepo();

        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string username = tbPhysioUsername.Text;
            string password = tbPhysioPassword.Password.ToString();

            bool isAuthenticated = await physioRepo.PhysioAuthenticateLogin(username, password);

            if (isAuthenticated) {
                PhysioFrontPageWindow physioFrontPageWindow = new PhysioFrontPageWindow();
                physioFrontPageWindow.Show();
                this.Close();
            }
            else {
                // Show an error message to the user
                MessageBox.Show("Login fejlede. Kontroller dit brugernavn og adgangskode og prøv igen.", "Login Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Set currents to null
            PhysioService.CurrentPhysio = null;
            PatientService.CurrentPatient = null;

            // Open the main window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close(); // Close the current window
        }

    }
}
