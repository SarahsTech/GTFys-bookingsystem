using GTFys.Application;
using GTFys.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            // Load content of the front page
            LoadFrontPage();
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

                // Open a new instance of LoginWindow
                PhysioLoginWindow physioLoginWindow = new PhysioLoginWindow();
                // Close the current window hosting the page
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null) {
                    physioLoginWindow.Show();
                    parentWindow.Close();
                }
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

        private void LoadFrontPage()
        {
            // Load the profile picture of the logged in physio (CurrentPhysio)
            
            if (!string.IsNullOrEmpty(PhysioService.CurrentPhysio?.ProfilePicture)) {
                byte[] imageBytes = Convert.FromBase64String(PhysioService.CurrentPhysio?.ProfilePicture);
                imgProfilePicture.Source = GetImageFromDatabase(imageBytes);
            }
            else {
                // Set the default profile picture if the profile picture is null or empty
                imgProfilePicture.Source = new BitmapImage(new Uri("/GTFys;component/Images/DefaultProfilePicture.jpeg", UriKind.Relative));
            }
        }

        // Method to convert bytes to image 
        private BitmapImage GetImageFromDatabase(byte[] imageData)
        {
            BitmapImage image = null;

            using (MemoryStream stream = new MemoryStream(imageData)) {
                image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }
            return image;
        }
    }
}
