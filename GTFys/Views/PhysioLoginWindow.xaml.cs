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
            bool isAuthenticated = await physioRepo.physioAuthenticateLogin(tbPhysioUsername.Text, tbPhysioPassword.Text);

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
                // Vis en fejlmeddelelse til brugeren
                MessageBox.Show("Login fejlede. Kontroller dit brugernavn og adgangskode og prøv igen.", "Login Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
