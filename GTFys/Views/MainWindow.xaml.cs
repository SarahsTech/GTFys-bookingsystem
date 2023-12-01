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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent(); // Initializes the components defined in XAML
        }

        // Event handler for the physio button click
        private void btnPhysio_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the PhysioLoginWindow
            PhysioLoginWindow physioLoginWindow = new PhysioLoginWindow();

            // Show the PhysioLoginWindow
            physioLoginWindow.Show();
        }

        // Event handler for the patient button click
        private void btnPatient_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the PatientLoginWindow
            PatientLoginWindow patientLoginWindow = new PatientLoginWindow();

            // Show the PatientLoginWindow
            patientLoginWindow.Show();
        }
    }
}

