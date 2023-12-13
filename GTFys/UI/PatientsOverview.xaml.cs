using GTFys.Application;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

            // Load patients from the database
            LoadPatientGrid();
        }

        private void btnCreatePatient_Click(object sender, RoutedEventArgs e)
        {
            // Create and instance of CreatePatientWindow
            CreatePatientWindow createPatientWindow = new CreatePatientWindow();
            createPatientWindow.ShowDialog();
        }

        private async void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            // Show a dialog to get confirmation from the user 
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil slette denne profil?", "Bekræftelse", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check answer from user 
            if (result == MessageBoxResult.Yes) {
                try {
                    // Deletion going through 
                    if (PatientService.CurrentPatient != null) {
                        int patientID = PatientService.CurrentPatient.PatientID;

                        // Create an instance of your repository
                        PatientRepo patientRepo = new PatientRepo();

                        // Call the repository method to delete the profile
                        bool isDeleted = await patientRepo.DeletePatientProfile(patientID);

                        if (isDeleted) {
                            // Show a success message to the user
                            MessageBox.Show("Patientens profil blev slettet!", "Slet profil", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Set the current patient to null
                            PatientService.CurrentPatient = null;
                            // Reload patient grid
                            LoadPatientGrid(); 
                        }
                        else {
                            // Show an error message to the user
                            MessageBox.Show("Sletning mislykkedes! \nPrøv igen.", "Sletning mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Check if user has selected a patient
            if(PatientService.CurrentPatient != null) {              
                // Open book consultation window
                PhysioBookConsultation bookConsultation = new PhysioBookConsultation();
                // Close the current window hosting the page
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null) {
                    bookConsultation.Show();
                    parentWindow.Close();
                }
            }
            else {
                // Show an error message to the user
                MessageBox.Show("Vælg en patient inden du booker en konsultation! \nPrøv igen.", "Fejl ved booking", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPatientGrid()
        {
            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect()) {

                    string query = "gtspGetAllPatients";

                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {

                        // Create a datatable and read from the database
                        DataTable dt = new DataTable();
                        SqlDataReader reader = command.ExecuteReader();
                        // Load the content to the datatable
                        dt.Load(reader);
                        dgPatients.ItemsSource = dt.DefaultView;

                        // Add the column names to the ComboBox
                        foreach (DataRow dr in dt.Rows) {
                            foreach (DataColumn column in dt.Columns) {
                                
                                string columnName = column.ColumnName;
                                if(columnName != "ID") {
                                    cbSearchPatient.Items.Add($"{columnName}");
                                }
                            }
                            return; 
                        }
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("Der opstod en fejl ved indlæsning af patienter! \n" + ex.Message);
            }
        }

        // Adds the values of the selected patient to textboxes
        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Converts the object type to a datagrid object
            DataGrid dg = (DataGrid)sender;
            // Casts the selected row in the datagrid to a DataRowView
            DataRowView rowSelected = (DataRowView)dg.SelectedItem;
            // Gets the values from the datagrid and adds it to their respective textboxes
            if(rowSelected != null ) {
                // Read the values of the selected row
                int patientID = Convert.ToInt32(rowSelected.Row["ID"].ToString());
                string name = rowSelected["Navn"].ToString();
                string phone = rowSelected["Telefon"].ToString();
                string email = rowSelected["Email"].ToString();
                string cpr = rowSelected["CPR"].ToString();

                // Initialize the PatientService with the selected patientID
                // Sets CurrentPatient to selected patient
                PatientService.InitializePatient(patientID);

                // Display values in textboxes
                tbName.Text = name;
                tbPhone.Text = phone;
                tbEmail.Text = email;
                tbCPR.Text = cpr;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = cbSearchPatient.Text; 
            if (string.IsNullOrEmpty(searchCriteria)) {
                MessageBox.Show("Vælg et søgekriterie inden du søger.", "Fejl ved søgning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Search for the chosen column with search value
            string searchValue = tbSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchValue)) {
                LoadGridSearchPatient(searchValue, searchCriteria);
            }
            else {
                MessageBox.Show("Indtast venligst noget i søgefeltet.", "Fejl ved søgning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Search patients by selected search criteria
        private void LoadGridSearchPatient(string searchValue, string searchColumn)
        {
            // Variables to build query
            string column = searchColumn;
            string value = searchValue;
            SqlDbType parameterType = SqlDbType.NVarChar; // Default parameter type for LIKE clause

            // Set variables to search the right column names and values
            if (column == "ID") {
                parameterType = SqlDbType.Int; // Specify parameter type for PatientID
            }

            // Construct the SQL query based on the chosen criteria
            string query = $"SELECT * FROM gtvwPatientInfo WHERE {column} LIKE @{column}";

            // Call LoadPatient to search and load an updated grid for patients with the given criteria
            LoadPatient(query, column, value, parameterType);
        }

        // Load patients based on the provided SQL query and parameters
        private void LoadPatient(string query, string column, string value, SqlDbType parameterType)
        {
            try {
                // Establish a new database connection
                using (IDbConnection connection = new DatabaseConnection().Connect()) {
                    // Create a new SQL command using the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection)) {
                        // Add parameter to the SQL command
                        command.Parameters.AddWithValue($"@{column}", parameterType).Value = $"%{value}%"; // Adjust for LIKE clause

                        // Create a new DataTable to hold the result
                        DataTable dt = new DataTable();

                        // Execute the command and load the result into the DataTable
                        SqlDataReader reader = command.ExecuteReader();
                        dt.Load(reader);

                        // Set the DataTable as the ItemsSource for the DataGrid
                        dgPatients.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex) {
                // Show a user-friendly error message
                MessageBox.Show($"Fejl ved indlæsning af patienter!\n{ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new instance of the login window if needed
            PhysioFrontPageWindow frontPage = new PhysioFrontPageWindow();

            // Close the current window hosting the page
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null) {
                frontPage.Show();
                parentWindow.Close();
            }
        }
    }
}
