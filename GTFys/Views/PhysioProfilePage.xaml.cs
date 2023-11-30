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

namespace GTFys.Views
{
    /// <summary>
    /// Interaction logic for PhysioProfilePage.xaml
    /// </summary>
    public partial class PhysioProfilePage : Page
    {
        public PhysioProfilePage()
        {
            InitializeComponent();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go back to the previous page
            NavigationService.GoBack();
        }

        private void btnUpdatePhysio_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
