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
    /// Interaction logic for PhysioBookConsultation.xaml
    /// </summary>
    public partial class PhysioBookConsultation : Window
    {
        public PhysioBookConsultation()
        {
            InitializeComponent();

            // Set DataContext to the current patient
            DataContext = PatientService.CurrentPatient;
        }

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgAvailableTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            // Check if both checkboxes are unchecked or only one is checked
            if (!(cbFysio1.IsChecked == true && cbFysio2.IsChecked == true) || (cbFysio1.IsChecked == false && cbFysio2.IsChecked == false)) {
                // Set IsChecked property of both checkboxes to true
                cbFysio1.IsChecked = true;
                cbFysio2.IsChecked = true;
            }
            else {
                // Uncheck both checkboxes
                cbFysio1.IsChecked = false;
                cbFysio2.IsChecked = false;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            //Close the window an go back to the patient window
            this.Close();
            PatientsOverview patientsOverview = new PatientsOverview();
            Content = patientsOverview;
        }
    }
}
