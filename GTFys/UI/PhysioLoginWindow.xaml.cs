using GTFys.Application;
using GTFys.Domain;
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
            string password = tbPhysioPassword.Text;

            bool isAuthenticated = await physioRepo.PhysioAuthenticateLogin(username, password);

            if (isAuthenticated) {
                PhysioFrontPageWindow physioFrontPageWindow = new PhysioFrontPageWindow();
                physioFrontPageWindow.Show();
                this.Close();
            }
            else {

                /* Det er en god praksis at håndtere brugergrænsefladeinteraktioner og visning af beskeder i code-behind eller ViewModel, 
                 * når du har fået svar på, om login er godkendt eller ej. Dette adskiller præsentationslogikken fra din dataadgangslogik, 
                 * hvilket gør din kode mere modulær og lettere at vedligeholde.
                 */ 
                // Show an error message to the user
                MessageBox.Show("Login fejlede. Kontroller dit brugernavn og adgangskode og prøv igen.", "Login Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
