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

namespace GTFys.Views
{
    /// <summary>
    /// Interaction logic for PatientLoginWindow.xaml
    /// </summary>
    public partial class PatientLoginWindow : Window
    {
        public PatientLoginWindow()
        {
            InitializeComponent();
        }

        PatientRepo patientRepo = new PatientRepo();

        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            bool isAuthenticated = await patientRepo.patientAuthenticateLogin(tbPatientUsername.Text, tbPatientPassword.Text);

            if (isAuthenticated)
            {
                PatientFrontPageWindow patientFrontPageWindow = new PatientFrontPageWindow();
                patientFrontPageWindow.Show();
                this.Close();
            }
            else
            {
                /* It's a good practice to handle user interface interactions and display messages in code-behind or ViewModel
                 * once you have the response about whether the login is successful or not. This separates the presentation logic
                 * from your data access logic, making your code more modular and easier to maintain.
                 */
                // Show an error message to the user
                MessageBox.Show("Login failed. Check your username and password and try again.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
