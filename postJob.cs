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
    public partial class postJob : Form
    {
        private readonly string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        int companyId;
        public postJob()
        {
            InitializeComponent();
            companyId = GetCompanyIdFromDatabase();
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
            editCompany home = new editCompany();
            home.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private int GetCompanyIdFromDatabase()
        {
            // TEMPORARY TESTING HACK: Randomly assign CompanyID from 1 to 21
            Random rand = new Random();
            return rand.Next(1, 22); // Generates a number from 1 to 21
        }




        private void button9_Click_1(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connection and insert query
            string query = @"INSERT INTO JobPostings 
    (CompanyID, Title, Description, JobType, SalaryRange, RequiredSkills, Location)
    VALUES 
    (@CompanyID, @Title, @Description, @JobType, @SalaryRange, @Skills, @Location)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", companyId); 
                        cmd.Parameters.AddWithValue("@Title", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@JobType", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@SalaryRange", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Skills", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@Location", textBox5.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Job posted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Optional: clear form fields
                        textBox1.Clear();
                        textBox2.Clear();
                        comboBox1.SelectedIndex = -1;
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while posting job: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            viewApplicants home =new viewApplicants();
            home.Show();
            this.Hide();
        }
    }
}