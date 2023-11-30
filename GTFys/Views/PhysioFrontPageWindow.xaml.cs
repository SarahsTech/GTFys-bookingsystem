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
            PhysioProfilePage physioProfilePage = new PhysioProfilePage();
            Content = physioProfilePage;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Show a message with option to accept or cancel
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil logge ud?", "Log ud", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK) {
                // User has confirmed
                this.Close();
                PhysioLoginWindow physioLoginWindow = new PhysioLoginWindow();
                physioLoginWindow.Show();
            }
            else {
                // User has cancelled
            }
        }
    }
}
