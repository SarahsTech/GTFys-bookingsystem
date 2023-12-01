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

            LoadPhysioInfo();
        }
        PhysioRepo physioRepo = new PhysioRepo();
        private async void btnUpdatePhysio_Click(object sender, RoutedEventArgs e)
        {
            // Check whether tbProfilePicture.Text is empty, and if it is, set the value to null 
            string profilePicture = string.IsNullOrEmpty(tbProfilePicture.Text) ? null : tbProfilePicture.Text;

            bool updateSuccessful = await physioRepo.PhysioUpdateUser(tbFirstName.Text, tbLastName.Text, tbUsername.Text,
                tbPassword.Text, tbEmail.Text, tbPhone.Text, tbCPR.Text, tbAddress.Text, Convert.ToInt32(tbZipCode.Text), tbCity.Text, profilePicture);

            if (updateSuccessful) {
                // Show a success message to the user
                MessageBox.Show("Din opdatering succesfuld!", "Succesfuld opdatering", MessageBoxButton.OK, MessageBoxImage.Information);

                // You can also navigate to another page or perform additional actions on success
            }
            else {
                // Show an error message to the user
                MessageBox.Show("Fejl ved opdatering. Læs venligst informationen igennem og prøv igen.", "Fejl ved opdatering", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPhysioInfo()
        {
            // Set all values in the textbox to the values of the logged in physio (CurrentPhysio)
            tbCPR.Text = PhysioService.CurrentPhysio?.CPR;
            tbFirstName.Text = PhysioService.CurrentPhysio?.FirstName;
            tbLastName.Text = PhysioService.CurrentPhysio?.LastName;
            tbUsername.Text = PhysioService.CurrentPhysio?.Username;
            tbPassword.Text = PhysioService.CurrentPhysio?.Password;
            tbPhone.Text = PhysioService.CurrentPhysio?.Phone;
            tbEmail.Text = PhysioService.CurrentPhysio?.Email;
            tbAddress.Text = PhysioService.CurrentPhysio?.Address;
            tbCity.Text = PhysioService.CurrentPhysio?.City;
            tbZipCode.Text = PhysioService.CurrentPhysio?.ZipCode.ToString();
            tbProfilePicture.Text = PhysioService.CurrentPhysio?.ProfilePicture;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go back to the previous page
            PhysioFrontPageWindow frontPage = new PhysioFrontPageWindow();
            Content = frontPage;
        }

    }
}
