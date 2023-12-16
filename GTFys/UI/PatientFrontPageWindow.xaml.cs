using GTFys.Application;
using GTFys.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for PatientFrontPageWindow.xaml
    /// </summary>
    public partial class PatientFrontPageWindow : Window
    {
        public PatientFrontPageWindow()
        {
            InitializeComponent();

            // Load content of the front page
            LoadFrontPage();

            // Load consultations from the database
            LoadConsultationGrid();
        }

        // Event handler for the button to navigate to the patient profile page
        private void btnProfilePage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of PatientProfilePage
            PatientProfilePage patientProfilePage = new PatientProfilePage();
            this.Content = patientProfilePage;
        }

        // Event handler for the button to log out
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Show a message with the option to accept or cancel
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil logge ud?", "Log ud", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK)
            {
                // User has confirmed

                // Set the current patient to null
                PatientService.CurrentPatient = null;

                // Open a new instance of LoginWindow
                PatientLoginWindow loginWindow = new PatientLoginWindow();
                // Close the current window hosting the page
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null) {
                    loginWindow.Show();
                    parentWindow.Close();
                }
            }
            else
            {
                // User has canceled the log-out operation
            }
        }

        private void LoadFrontPage()
        {
            // Load the profile picture of the logged in patient (CurrentPatient)
            if (!string.IsNullOrEmpty(PatientService.CurrentPatient?.ProfilePicture)) {
                byte[] imageBytes = Convert.FromBase64String(PatientService.CurrentPatient?.ProfilePicture);
                imgProfilePicture.Source = GetImageFromDatabase(imageBytes);
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

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of PatientBookConsultation window
            PatientBookConsultation consultationWindow = new PatientBookConsultation();
            // Close the current window hosting the page
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null) {
                consultationWindow.Show();
                parentWindow.Close();
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
            MessageBoxResult result = MessageBox.Show("Hvis du opdaterer, vil din nuværende konsultation blive slettet. Bagefter vil du få muligheden for at booke en ny konsultation." +
                "\nEr du sikker på, at du vil opdatere konsultationen?", "Bekræftelse", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check answer from user 
            if (result == MessageBoxResult.Yes && consultationID > 0) {
                try {

                    ConsultationRepo consultationRepo = new ConsultationRepo();

                    // Call the repository method to delete the consultation
                    bool isDeleted = await consultationRepo.DeleteConsultation(consultationID);

                    if (isDeleted) {
                        // Show a message with OK/Cancel option
                        MessageBoxResult bookConsultationResult = MessageBox.Show("Din nuværende konsultation blev slettet!\nTryk 'OK' for at booke en ny konsultation.", "Opdatér konsultation", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                        // Reload consultation grid
                        LoadConsultationGrid();

                        // Check if OK is pressed
                        if (bookConsultationResult == MessageBoxResult.OK) {                         
                            // Open a new instance of PatientBookConsultation window
                            PatientBookConsultation consultationWindow = new PatientBookConsultation();
                            // Close the current window hosting the page
                            Window parentWindow = Window.GetWindow(this);
                            if (parentWindow != null) {
                                consultationWindow.Show();
                                parentWindow.Close();
                            }
                        }
                        else {
                            // Show an error message to the user
                            MessageBox.Show("Opdatering mislykkedes! \nPrøv igen.", "Opdatering mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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

        private void LoadConsultationGrid()
        {
            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect()) {

                    string query = "gtspGetConsultationDetailsPatient";
                    int patientID = PatientService.CurrentPatient.PatientID;

                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the PatientID parameter
                        command.Parameters.AddWithValue("@patientID", patientID);

                        // Create a datatable and read from the database
                        DataTable dt = new DataTable();
                        SqlDataReader reader = command.ExecuteReader();

                        // Load the content to the datatable
                        dt.Load(reader);
                        dgConstultations.ItemsSource = dt.DefaultView;
                    }
                    return;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("Der opstod en fejl ved indlæsning af patienter! \n" + ex.Message);
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
                string price = rowSelected["Pris"].ToString();

                // Display values in textboxes
                tbPhysio.Text = name;
                tbTime.Text = time;
                tbTreatmentType.Text = treatmentType;
                tbDate.Text = date;
                tbPrice.Text = price;
            }
        }


    }
}
