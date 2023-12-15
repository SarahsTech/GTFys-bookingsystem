using GTFys.Application;
using GTFys.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace GTFys.UI
{
    /// <summary>
    /// Interaction logic for PhysioFrontPageWindow.xaml
    /// </summary>
    public partial class PhysioFrontPageWindow : Window
    {
        private int consultationID; 

        public PhysioFrontPageWindow()
        {
            InitializeComponent();

            // Load content of the front page
            LoadFrontPage();

            // Load consultations from the database
            LoadConsultationGrid();
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

        private void calendarView_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected date from the calendar control
            DateTime? selectedDate = calendarView.SelectedDate;

            // Check if a date is selected and if it is a weekend
            if (selectedDate.HasValue && (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)) {
                // Clear the selected date to prevent weekend selection
                calendarView.SelectedDate = null;
                selectedDate = null;
            }

            //// Check if a treatment type has been selected
            //bool isTreatmentTypeSelected = rbFirstConsultation.IsChecked == true || rbTrainingInstruction.IsChecked == true;

            //// Check if at least one physio has been selected
            //bool isPhysioSelected = cbPhysio1.IsChecked == true || cbPhysio2.IsChecked == true;

            //// Additional conditions for your if statement
            //if (isTreatmentTypeSelected && isPhysioSelected && selectedDate != null) {
            //    GetAvailableTimes();
            //}
            //else {
            //    return;
            //}

        }


        private void dgConstultations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Converts the object type to a datagrid object
            DataGrid dg = (DataGrid)sender;
            // Casts the selected row in the datagrid to a DataRowView
            DataRowView rowSelected = (DataRowView)dg.SelectedItem;
            // Gets the values from the datagrid and adds it to their respective textboxes
            if (rowSelected != null) {
                // Read the values of the selected row
                consultationID = Convert.ToInt32(rowSelected.Row["ID"].ToString());
                string name = rowSelected["Navn"].ToString();
                string time = rowSelected["Tid"].ToString();
                string treatmentType = rowSelected["Behandlingstype"].ToString();
                string date = rowSelected["Date"].ToString();

                // Display values in textboxes
                tbPatient.Text = name;
                tbTime.Text = time;
                tbTreatmentType.Text = treatmentType;
                tbDate.Text = date;
            }

            //Enable or disable the "Book Consultation" button based on conditions
            if(rowSelected != null) {
                btnBookConsultation.IsEnabled = true;
            }
        }

        private void btnPatients_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of Patientsoverview page
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

        private void LoadConsultationGrid()
        {

            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect()) {
                    
                    string query = "gtspGetConsultationDetailsPhysio";
                    int physioID = PhysioService.CurrentPhysio.PhysioID;

                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {

                        command.CommandType = CommandType.StoredProcedure;

                        // Add the PhysioID parameter
                        command.Parameters.AddWithValue("@PhysioID", physioID);

                        // Create a datatable and read from the database
                        DataTable dt = new DataTable();
                        SqlDataReader reader = command.ExecuteReader();

                        // Load the content to the datatable
                        dt.Load(reader);
                        dgConstultations.ItemsSource = dt.DefaultView;

                        // Add the column names to the ComboBox
                        cbSearchConsultation.Items.Add("Navn");
                        cbSearchConsultation.Items.Add("CPR");
                    }
                return;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("Der opstod en fejl ved indlæsning af patienter! \n" + ex.Message);
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteConsultation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateConsultation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of Patientsoverview page
            PatientsOverview patientsOverview = new PatientsOverview();
            this.Content = patientsOverview;
        }
    }
}
