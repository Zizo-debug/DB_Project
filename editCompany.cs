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
using System.Xml.Linq;

namespace labDB_Interface
{
    public partial class editCompany : Form
    {
        public editCompany()
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

        private void editCompany_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {

            // Step 1: Get the CompanyID from the user input (textbox)
            int recruiterCompanyID;
            bool isValidCompanyID = int.TryParse(textBox1.Text, out recruiterCompanyID); // Validate if the input is a valid integer

            if (!isValidCompanyID || recruiterCompanyID <= 0)
            {
                MessageBox.Show("Please enter a valid Company ID.");
                return;
            }

            // Step 2: Define the connection string (ensure it's correct)
            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            // Step 3: Update the company details in the Companies table
            string updateQuery = @"
        UPDATE Companies
        SET CompanyName = @CompanyName, Sector = @Sector, Website = @Website, Address = @Address
        WHERE CompanyID = @CompanyID";

            // Step 4: Use SqlConnection and SqlCommand to execute the update query
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.Parameters.AddWithValue("@CompanyName", textBox2.Text); // Textbox value for Name
                cmd.Parameters.AddWithValue("@Sector", textBox3.Text); // Textbox value for Sector
                cmd.Parameters.AddWithValue("@Website", textBox4.Text); // Textbox value for Website
                cmd.Parameters.AddWithValue("@Address", textBox5.Text); // Textbox value for About
                cmd.Parameters.AddWithValue("@CompanyID", recruiterCompanyID); // The CompanyID entered by the user

                cmd.ExecuteNonQuery(); // Execute the update query

                MessageBox.Show("Company profile updated successfully.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            manageinterviews va = new manageinterviews();
            va.Show();
            this.Hide();
        }
    }
}