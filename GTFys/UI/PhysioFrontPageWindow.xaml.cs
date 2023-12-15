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

        public PhysioFrontPageWindow()
        {
            InitializeComponent();

            // Load content of the front page
            LoadFrontPage();

            // Load consultations from the database
            LoadConsultationGrid(null, DateTime.Now);
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

            // Load the consultation grid for the selected date
            if(selectedDate != null && selectedDate >= DateTime.Today) {
                LoadConsultationGrid(null, selectedDate);
            } else if(selectedDate < DateTime.Today) {
                dgConstultations.ItemsSource = null;
            }

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
                string name = rowSelected["Navn"].ToString();
                string time = rowSelected["Tid"].ToString();
                string treatmentType = rowSelected["Behandlingstype"].ToString();
                string date = rowSelected["Dato"].ToString();

                // Display values in textboxes
                tbPatient.Text = name;
                tbTime.Text = time;
                tbTreatmentType.Text = treatmentType;
                tbDate.Text = date;
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

        private void LoadConsultationGrid(string searchText = null, DateTime? consultationDate = null)
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

                        // Check and add @SearchText parameter
                        if (!string.IsNullOrEmpty(searchText)) {
                            command.Parameters.AddWithValue("@SearchText", searchText);
                        }

                        // Check and add @ConsultationDate parameter
                        if (consultationDate.HasValue) {
                            command.Parameters.AddWithValue("@ConsultationDate", consultationDate.Value);
                        }

                        // Create a datatable and read from the database
                        DataTable dt = new DataTable();
                        SqlDataReader reader = command.ExecuteReader();

                        // Load the content to the datatable
                        dt.Load(reader);
                        dgConstultations.ItemsSource = dt.DefaultView;

                        if(cbSearchConsultation == null) {
                            // Add the column names to the ComboBox
                            cbSearchConsultation.Items.Add("Navn");
                            cbSearchConsultation.Items.Add("CPR");
                        }                       
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
            string searchCriteria = cbSearchConsultation.Text;
            if (string.IsNullOrEmpty(searchCriteria)) {
                MessageBox.Show("Vælg et søgekriterie inden du søger.", "Fejl ved søgning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Search for the chosen column with search value
            string searchValue = tbSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchValue)) {
                LoadConsultationGrid(searchValue);
            }
            else {
                MessageBox.Show("Indtast venligst noget i søgefeltet.", "Fejl ved søgning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnDeleteConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Attempt to cast the sender to a Button
            Button btn = sender as Button;

            // Check if the cast was successful
            if (btn == null) {
                MessageBox.Show("The sender is not a Button.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Retrieve the associated DataGrid from the Tag property
            DataGrid dg = btn.Tag as DataGrid;
  
            if (dg == null || dg.SelectedItem == null) {
                MessageBox.Show("Du skal vælge en konsultation inden du sletter den.", "Fejl ved sletning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Store the consultationID
            int consultationID = Convert.ToInt32(((DataRowView)dg.SelectedItem).Row["ID"]);

            // Show a dialog to get confirmation from the user 
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil slette denne konsultation?", "Bekræftelse", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check answer from user 
            if (result == MessageBoxResult.Yes && consultationID > 0) {
                try {

                    ConsultationRepo consultationRepo = new ConsultationRepo();

                    // Call the repository method to delete the consultation
                    bool isDeleted = await consultationRepo.DeleteConsultation(consultationID);

                    if (isDeleted) {
                        // Show a success message to the user
                        MessageBox.Show("Konsultationen blev slettet!", "Slet konsultation", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Reload consultation grid
                        LoadConsultationGrid();
                    }
                    else {
                        // Show an error message to the user
                        MessageBox.Show("Sletning mislykkedes! \nPrøv igen.", "Sletning mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else {
                return;
            }
            
        }

        private async void btnUpdateConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Attempt to cast the sender to a Button
            Button btn = sender as Button;

            // Retrieve the associated DataGrid from the Tag property
            DataGrid dg = btn.Tag as DataGrid;

            if (dg == null || dg.SelectedItem == null) {
                MessageBox.Show("Du skal vælge en konsultation inden du opdaterer den.", "Fejl ved sletning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Store the consultationID
            int consultationID = Convert.ToInt32(((DataRowView)dg.SelectedItem).Row["ID"]);

            // Show a dialog to get confirmation from the user 
            MessageBoxResult result = MessageBox.Show("Hvis du opdaterer, vil den nuværende konsultation blive slettet. Bagefter vil du få muligheden for at booke en ny konsultation." +
                "\nEr du sikker på, at du vil opdatere konsultationen?", "Bekræftelse", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check answer from user 
            if (result == MessageBoxResult.Yes && consultationID > 0) {
                try {

                    ConsultationRepo consultationRepo = new ConsultationRepo();

                    // Call the repository method to delete the consultation
                    bool isDeleted = await consultationRepo.DeleteConsultation(consultationID);

                    if (isDeleted) {
                        // Show a success message to the user
                        MessageBoxResult bookConsultationResult = MessageBox.Show("Den nuværende konsultation blev slettet!\nTryk 'OK' for at booke en ny konsultation.", "Opdatér konsultation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        // Reload consultation grid
                        LoadConsultationGrid();

                        // Check if OK is pressed
                        if (bookConsultationResult == MessageBoxResult.OK) {
                            // Create an instance of Patientsoverview page
                            PatientsOverview patientsOverview = new PatientsOverview();
                            this.Content = patientsOverview;
                        }               
                    }
                    else {
                        // Show an error message to the user
                        MessageBox.Show("Opdatering mislykkedes! \nPrøv igen.", "Opdatering mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else {
                return;
            }
        }

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of Patientsoverview page
            PatientsOverview patientsOverview = new PatientsOverview();
            this.Content = patientsOverview;
        }
    }
}
