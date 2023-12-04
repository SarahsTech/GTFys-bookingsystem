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

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
