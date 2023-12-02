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
    /// Interaction logic for PatientFrontPageWindow.xaml
    /// </summary>
    public partial class PatientFrontPageWindow : Window
    {
        public PatientFrontPageWindow()
        {
            InitializeComponent();
        }

        private void btnProfilePage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PatientProfilePage
            PatientProfilePage patientProfilePage = new PatientProfilePage();
            this.Content = patientProfilePage;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
