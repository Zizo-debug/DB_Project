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
    public partial class StudentProfile : Form
    {
        public StudentProfile()
        {
            InitializeComponent();
        }

        private void student_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }


        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void StudentDashBoard_Load(object sender, EventArgs e)
        {

        }


        private void button6_Click_1(object sender, EventArgs e)
        {
            interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            feedback Home = new feedback();
            Home.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            JobFairs Home = new JobFairs();
            Home.Show();
            this.Hide();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            JobSearch Home = new JobSearch();
            Home.Show();
            this.Hide();
        }


        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            feedback Feedback = new feedback();
            Feedback.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fastId = textBox1.Text.Trim();
            string degreeProgram = comboBox1.Text.Trim();
            string semesterText = comboBox2.Text.Trim();
            string gpaText = textBox2.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(fastId) || string.IsNullOrEmpty(degreeProgram) ||
                string.IsNullOrEmpty(semesterText) || string.IsNullOrEmpty(gpaText))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(semesterText, out int currentSemester))
            {
                MessageBox.Show("Invalid semester. Please select a valid number.");
                return;
            }

            if (!decimal.TryParse(gpaText, out decimal gpa) || gpa < 0 || gpa > 4)
            {
                MessageBox.Show("Invalid GPA. Enter a number between 0.00 and 4.00.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE Students
            SET DegreeProgram = @DegreeProgram,
                CurrentSemester = @CurrentSemester,
                GPA = @GPA
            WHERE FAST_ID = @FastID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DegreeProgram", degreeProgram);
                cmd.Parameters.AddWithValue("@CurrentSemester", currentSemester);
                cmd.Parameters.AddWithValue("@GPA", gpa);
                cmd.Parameters.AddWithValue("@FastID", fastId);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No matching FAST-ID found. Please check your input.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            StudentDashBoard sd = new StudentDashBoard();
            sd.Show();
            this.Hide();
        }
    }
}