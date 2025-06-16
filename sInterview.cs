using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using labDB_Interface;

namespace labDB_Interface
{
    public partial class sInterview : Form
    {
        private string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public sInterview()
        {
            InitializeComponent();
        }

        private void scheduleInterview_Load(object sender, EventArgs e)
        {
            LoadRecruiters();
            LoadApplications();
            LoadTimeSlots();
            LoadScheduledInterviews();
        }

        private void LoadRecruiters()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT r.RecruiterID, u.FullName 
                                  FROM Recruiters r 
                                  JOIN Users u ON r.UserID = u.UserID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxRecruiters.DisplayMember = "FullName";
                    comboBoxRecruiters.ValueMember = "RecruiterID";
                    comboBoxRecruiters.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recruiters: " + ex.Message);
            }
        }

        private void LoadApplications()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT a.ApplicationID, s.FAST_ID 
                                  FROM Applications a 
                                  JOIN Students s ON a.StudentID = s.StudentID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxApplications.DisplayMember = "FAST_ID";
                    comboBoxApplications.ValueMember = "ApplicationID";
                    comboBoxApplications.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message);
            }
        }

        private void LoadTimeSlots()
        {
            try
            {
                comboBoxTimeSlots.Items.Clear();
                DateTime startDate = DateTime.Today;

                for (int i = 0; i < 7; i++)
                {
                    DateTime currentDate = startDate.AddDays(i);
                    for (int hour = 9; hour < 17; hour++)
                    {
                        DateTime slot = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, hour, 0, 0);
                        if (IsTimeSlotAvailable(slot))
                        {
                            comboBoxTimeSlots.Items.Add(slot.ToString("yyyy-MM-dd HH:mm"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading time slots: " + ex.Message);
            }
        }

        private bool IsTimeSlotAvailable(DateTime slot)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) 
                              FROM Interviews 
                              WHERE InterviewTime = @InterviewTime";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InterviewTime", slot);
                    int count = (int)cmd.ExecuteScalar();
                    return count == 0;
                }
            }
        }

        private void LoadScheduledInterviews()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT i.InterviewID, s.FAST_ID, u.FullName AS RecruiterName, 
                                  i.InterviewTime, i.InterviewLocation, i.Result
                                  FROM Interviews i
                                  JOIN Applications a ON i.ApplicationID = a.ApplicationID
                                  JOIN Students s ON a.StudentID = s.StudentID
                                  JOIN Recruiters r ON i.RecruiterID = r.RecruiterID
                                  JOIN Users u ON r.UserID = u.UserID
                                  ORDER BY i.InterviewTime";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewInterviews.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading scheduled interviews: " + ex.Message);
            }
        }

        private void buttonSchedule_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxRecruiters.SelectedValue == null ||
                    comboBoxApplications.SelectedValue == null ||
                    comboBoxTimeSlots.SelectedItem == null ||
                    string.IsNullOrEmpty(textBoxLocation.Text))
                {
                    MessageBox.Show("Please fill all required fields.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Interviews (ApplicationID, RecruiterID, InterviewTime, InterviewLocation, Result)
                                  VALUES (@ApplicationID, @RecruiterID, @InterviewTime, @InterviewLocation, 'Pending')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", comboBoxApplications.SelectedValue);
                        cmd.Parameters.AddWithValue("@RecruiterID", comboBoxRecruiters.SelectedValue);
                        cmd.Parameters.AddWithValue("@InterviewTime", DateTime.Parse(comboBoxTimeSlots.SelectedItem.ToString()));
                        cmd.Parameters.AddWithValue("@InterviewLocation", textBoxLocation.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Interview scheduled successfully!");
                LoadTimeSlots();
                LoadScheduledInterviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scheduling interview: " + ex.Message);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            StudentProfile st = new StudentProfile();
            st.Show();
            this.Hide();
        }

        private void dataGridViewInterviews_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxRecruiters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void sInterview_Load_1(object sender, EventArgs e)
        {
            LoadRecruiters();
            LoadApplications();
            LoadTimeSlots();
            LoadScheduledInterviews();
        }

    }
}