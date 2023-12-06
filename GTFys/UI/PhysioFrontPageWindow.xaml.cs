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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PhysioFrontPageWindow.xaml
    /// </summary>
    public partial class PhysioFrontPageWindow : Window
    {

        public PhysioFrontPageWindow()
        {
            InitializeComponent();
        }

        private void btnProfilePage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PhysioProfilePage
            PhysioProfilePage physioProfilePage = new PhysioProfilePage();
            this.Content = physioProfilePage;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Show a message with option to accept or cancel
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil logge ud?", "Log ud", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK) {
                // User has confirmed

                // Set the current physio to null
                PhysioService.CurrentPhysio = null;

                // Close the current window and open the login window
                this.Close();
                PhysioLoginWindow physioLoginWindow = new PhysioLoginWindow();
                physioLoginWindow.Show();
            }
            else {
                // User has cancelled
            }
        }

        private void btnPatients_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PhysioProfilePage
            PatientsOverview patientsOverview = new PatientsOverview();
            this.Content = patientsOverview;
        }
    }
}
