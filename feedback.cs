using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using labDB_Interface;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace labDB_Interface
{
    public partial class feedback : Form
    {
        private string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public feedback()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Empty for now, can be used for custom painting if needed
        }

        private void StudentDashBoard_Load(object sender, EventArgs e)
        {
            // Empty for now, can be used for initialization if needed
        }

        private void button4_Click(object sender, EventArgs e)
        {
            JobFairs Home = new JobFairs();
            Home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            JobSearch Home = new JobSearch();
            Home.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Logic for rating 1 if needed
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Logic for rating 2 if needed
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Logic for rating 3 if needed
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Logic for rating 4 if needed
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            // Logic for rating 5 if needed
        }

        private void buttonSubmit_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Please enter both Interview ID and Your ID.");
                    return;
                }

                if (!int.TryParse(textBox2.Text, out int interviewId) || !int.TryParse(textBox3.Text, out int studentId))
                {
                    MessageBox.Show("Interview ID and Your ID must be valid integers.");
                    return;
                }

                // Get selected rating
                int rating = 0;
                if (radioButton1.Checked) rating = 1;
                else if (radioButton2.Checked) rating = 2;
                else if (radioButton3.Checked) rating = 3;
                else if (radioButton4.Checked) rating = 4;
                else if (radioButton5.Checked) rating = 5;
                else
                {
                    MessageBox.Show("Please select a rating.");
                    return;
                }

                string comments = textBox1.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the interview exists and its status
                    string checkInterviewQuery = @"
                        SELECT i.Result, i.ApplicationID, a.StudentID
                        FROM Interviews i
                        JOIN Applications a ON i.ApplicationID = a.ApplicationID
                        WHERE i.InterviewID = @InterviewID";

                    using (SqlCommand cmd = new SqlCommand(checkInterviewQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@InterviewID", interviewId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Interview ID does not exist.");
                                return;
                            }

                            string result = reader["Result"].ToString();
                            int interviewStudentId = Convert.ToInt32(reader["StudentID"]);

                            // Check if the interview belongs to the student
                            if (interviewStudentId != studentId)
                            {
                                MessageBox.Show("This interview does not belong to you.");
                                return;
                            }

                            // Check if the interview is conducted
                            if (result != "Conducted")
                            {
                                MessageBox.Show("Cannot submit feedback. The interview has not been conducted yet.");
                                return;
                            }
                        }
                    }

                    // Check if feedback already exists for this interview
                    string checkFeedbackQuery = @"
                        SELECT COUNT(*)
                        FROM Reviews
                        WHERE InterviewID = @InterviewID AND StudentID = @StudentID";

                    using (SqlCommand cmd = new SqlCommand(checkFeedbackQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@InterviewID", interviewId);
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        int feedbackCount = (int)cmd.ExecuteScalar();
                        if (feedbackCount > 0)
                        {
                            MessageBox.Show("Feedback for this interview has already been submitted.");
                            return;
                        }
                    }

                    // Insert the feedback into the Reviews table
                    string insertQuery = @"
                        INSERT INTO Reviews (InterviewID, StudentID, Rating, Feedback)
                        VALUES (@InterviewID, @StudentID, @Rating, @Feedback)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@InterviewID", interviewId);
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Feedback", comments);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Feedback submitted successfully!");

                    // Clear the form
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox1.Clear();
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting feedback: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            StudentDashBoard st = new StudentDashBoard();
            st.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            StudentProfile st = new StudentProfile();
            st.Show();
            this.Hide();
        }

    }
}