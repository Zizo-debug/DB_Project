using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class viewApplicants : Form
    {
        //string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        private BindingSource applicantsBindingSource = new BindingSource();
        public viewApplicants()
        {
            InitializeComponent();
           
        }

        private void student_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            GenerateReport2 home = new GenerateReport2();
            home.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            postJob home = new postJob();
            home.Show();
            this.Hide();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            postJob postJobForm = new postJob();
            postJobForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            viewApplicants va = new viewApplicants();
            va.Show();
            this.Hide();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        public static class LoggedInUser
        {
            public static int UserID { get; set; }
            public static string UserName { get; set; }
            // Add more properties as needed
        }
        /* private int GetCompanyIdFromDatabase()
         {
             int companyId = -1;
             int userId = LoggedInUser.UserID; // Or however you track logged-in user

             string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
             string query = "SELECT CompanyID FROM Companies WHERE UserID = @UserID";

             using (SqlConnection con = new SqlConnection(connectionString))
             {
                 using (SqlCommand cmd = new SqlCommand(query, con))
                 {
                     cmd.Parameters.AddWithValue("@UserID", userId);
                     try
                     {
                         con.Open();
                         object result = cmd.ExecuteScalar();
                         if (result != null)
                             companyId = Convert.ToInt32(result);
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show("Error fetching CompanyID: " + ex.Message);
                     }
                 }
             }

             return companyId;
         }

         */
        private void LoadApplications()
        {
            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            string query = @"
        SELECT 
            a.ApplicationID,
            j.Title AS JobTitle,
            a.ApplicationDate
            
        FROM 
            Applications a
        JOIN 
            Students s ON a.StudentID = s.StudentID
        JOIN 
            JobPostings j ON a.JobID = j.JobID
        WHERE 
            j.CompanyID = @CompanyID";

            DataTable applicantsDataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@CompanyID", 1); // Replace with dynamic if needed

                try
                {
                    con.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(applicantsDataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading applicants: " + ex.Message);
                }
            }

            dgvApplications.AutoGenerateColumns = true;
            applicantsBindingSource.DataSource = applicantsDataTable;
            dgvApplications.DataSource = applicantsBindingSource;
            dgvApplications.Refresh();
        }


        /* private int GetRecruiterIdFromUserId(int userId)
         {
             int recruiterId = -1;
             string query = "SELECT RecruiterID FROM Recruiters WHERE UserID = @UserID";
             using (SqlConnection con = new SqlConnection(connectionString))
             using (SqlCommand cmd = new SqlCommand(query, con))
             {
                 cmd.Parameters.AddWithValue("@UserID", userId);
                 con.Open();
                 var result = cmd.ExecuteScalar();
                 if (result != null)
                 {
                     recruiterId = Convert.ToInt32(result);
                 }
             }
             return recruiterId;
         }
        */
        private void dgvApplications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void applicationsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void viewApplicants_Load(object sender, EventArgs e)
        {
            LoadApplications(); // Call your custom method
        }


        private void btnShortlist_Click_1(object sender, EventArgs e)
        {
            // Check if any row is selected
            if (dgvApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one application.");
                return;
            }/*

            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            // Replace with actual user session logic
            int userId = LoggedInUser.UserID;
            int recruiterId = GetRecruiterIdFromUserId(userId);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (DataGridViewRow row in dgvApplications.SelectedRows)
                {
                    // Ensure the ApplicationID column exists in the DataGridView, and get its value
                    int applicationId = Convert.ToInt32(row.Cells[0].Value);

                    // Query to insert into Interviews table
                    string insertQuery = @"
                INSERT INTO Interviews (ApplicationID, RecruiterID, InterviewTime, InterviewLocation, Result)
                VALUES (@ApplicationID, @RecruiterID, @InterviewTime, @InterviewLocation, 'Pending')";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@ApplicationID", applicationId);
                        cmd.Parameters.AddWithValue("@RecruiterID", recruiterId);
                        cmd.Parameters.AddWithValue("@InterviewTime", DateTime.Now.AddDays(1)); // Temporary value for the interview time (1 day after current time)
                        cmd.Parameters.AddWithValue("@InterviewLocation", "To Be Decided"); // Temporary value for the interview location

                        // Execute the insert query
                        cmd.ExecuteNonQuery();
                    }
                }
            */
                // Inform the user that selected applications have been moved to Interviews
                MessageBox.Show("Selected applications have been moved to Interviews.");

                // Optionally refresh the grid or load applications again if necessary
                LoadApplications();
            }

        private void button6_Click(object sender, EventArgs e)
        {
            manageinterviews home = new manageinterviews();
            home.Show();
            this.Hide();
        }
    }

}
