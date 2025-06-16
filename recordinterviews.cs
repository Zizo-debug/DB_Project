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
    public partial class recordinterviews : Form
    {
        public recordinterviews()
        {
            InitializeComponent();
            this.Load += new EventHandler(recordinterviews_Load); 
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

        private void RecruiterDashBoard_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            manageinterviews va = new manageinterviews();
            va.Show();
            this.Hide();
        }
        
        private void button9_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an interview to update.");
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a result.");
                return;
            }

            int interviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["InterviewID"].Value);
            string selectedResult = comboBox1.SelectedItem.ToString();

            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE Interviews SET Result = @Result WHERE InterviewID = @ID";

                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.Parameters.AddWithValue("@Result", selectedResult);
                cmd.Parameters.AddWithValue("@ID", interviewId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Interview result updated successfully.");
                LoadShortlistedApplications();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void recordinterviews_Load(object sender, EventArgs e)
        {
            LoadShortlistedApplications();
        }
        private void LoadShortlistedApplications()
        {
            string query = @"SELECT 
    i.InterviewID, 
    a.ApplicationID, 
    u.FullName AS StudentName, 
    j.Title AS JobTitle
FROM Interviews i
JOIN Applications a ON i.ApplicationID = a.ApplicationID
JOIN Students s ON a.StudentID = s.StudentID
JOIN Users u ON s.UserID = u.UserID
JOIN JobPostings j ON a.JobID = j.JobID";

            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;
            }
        }

    }
}