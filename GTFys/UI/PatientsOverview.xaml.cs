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

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (PatientService.CurrentPatient != null) {

            }
        }

        private void btnBookConsultation_Click(object sender, RoutedEventArgs e)
        {

            if(PatientService.CurrentPatient != null) {

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
                            cbSearchPatient.Items.Add(dr.ToString());
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
                string cpr = rowSelected["CPR-nummer"].ToString();

                // Initialize the PatientService with the selected patientID
                // Sets CurrentPatient to selected patient
                PatientService.InitializePatient(patientID);

                // Display values in textboxes
                tbName.Text = name;
                tbPhone.Text = phone;
                tbEmail.Text = email;
                tbEmail.Text = cpr;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
