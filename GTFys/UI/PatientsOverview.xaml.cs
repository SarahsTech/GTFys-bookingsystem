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
    /// Interaction logic for PatientsOverview.xaml
    /// </summary>
    public partial class PatientsOverview : Page
    {
        public PatientsOverview()
        {
            InitializeComponent();
        }

        private void btnCreatePatient_Click(object sender, RoutedEventArgs e)
        {
            // Create and instance of CreatePatientWindow
            CreatePatientWindow createPatientWindow = new CreatePatientWindow();
            createPatientWindow.ShowDialog();
        }

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
