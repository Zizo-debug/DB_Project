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
    public partial class scheduleinterviews : Form
    {
        // Connection string - consider moving this to a config file for production use
        private readonly string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public scheduleinterviews()
        {
            InitializeComponent();
            this.Load += new EventHandler(scheduleinterviews_Load);
        }

        // Static class to store logged in user info across the application
        public static class LoggedInUser
        {
            public static int UserID { get; set; }
            public static string UserName { get; set; }
        }

        private void scheduleinterviews_Load(object sender, EventArgs e)
        {
            // Display current UserID for debugging
            MessageBox.Show("Current UserID before: " + LoggedInUser.UserID);

            // If UserID is 0 (default), try to find a valid recruiter for testing
            if (LoggedInUser.UserID == 0)
            {
                CheckForRecruiters();
            }

            // Show UserID after potential update
            MessageBox.Show("Current UserID after: " + LoggedInUser.UserID);

            // Attempt to load scheduled interviews
            LoadScheduledInterviews();
        }

        // Method to find and set a valid recruiter for testing purposes
        private void CheckForRecruiters()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 5 r.RecruiterID, r.UserID, u.FullName, u.Email " +
                                  "FROM Recruiters r JOIN Users u ON r.UserID = u.UserID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder("Available recruiters:\n");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.AppendLine($"RecruiterID: {row["RecruiterID"]}, UserID: {row["UserID"]}, " +
                                         $"Name: {row["FullName"]}, Email: {row["Email"]}");
                        }
                        MessageBox.Show(sb.ToString());

                        // Use the first recruiter for testing
                        if (dt.Rows.Count > 0)
                        {
                            int testUserId = Convert.ToInt32(dt.Rows[0]["UserID"]);
                            string testUserName = dt.Rows[0]["Email"].ToString();

                            LoggedInUser.UserID = testUserId;
                            LoggedInUser.UserName = testUserName;

                            MessageBox.Show($"Set test user to: UserID={testUserId}, Name={testUserName}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No recruiters found in the database. Please add at least one recruiter.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking for recruiters: " + ex.Message);
            }
        }

        // Method to load scheduled interviews to the DataGridView
        private void LoadScheduledInterviews()
        {
            try
            {
                int userId = LoggedInUser.UserID;
                if (userId == 0)
                {
                    MessageBox.Show("UserID is not set! Please login properly.");
                    return;
                }

                int recruiterId = GetRecruiterIdFromUserId(userId);

                if (recruiterId == -1)
                {
                    MessageBox.Show("Could not find a recruiter record for the current user (UserID: " + userId + ")");
                    return;
                }

                // SQL query to get interviews - includes more joined fields for better display
                string query = @"
                    SELECT 
                        i.InterviewID,
                        a.ApplicationID,
                        j.Title AS JobTitle,
                        s.FAST_ID AS StudentID,
                        u.FullName AS StudentName, 
                        i.InterviewTime, 
                        i.InterviewLocation, 
                        i.Result
                    FROM Interviews i
                    JOIN Applications a ON i.ApplicationID = a.ApplicationID
                    JOIN Students s ON a.StudentID = s.StudentID
                    JOIN Users u ON s.UserID = u.UserID
                    JOIN JobPostings j ON a.JobID = j.JobID
                    WHERE i.RecruiterID = @RecruiterID";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecruiterID", recruiterId);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    MessageBox.Show("Rows loaded: " + dt.Rows.Count);

                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dt;

                    // Format date/time columns for better display
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        if (col.Name.Contains("Time") || col.Name.Contains("Date"))
                        {
                            col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading interviews: " + ex.Message);
            }
        }

        // Method to get RecruiterID from UserID
        private int GetRecruiterIdFromUserId(int userId)
        {
            int recruiterId = -1; // default if not found

            string query = "SELECT RecruiterID FROM Recruiters WHERE UserID = @UserID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        recruiterId = Convert.ToInt32(result);
                        MessageBox.Show("Found recruiter ID: " + recruiterId);
                    }
                    else
                    {
                        // More informative error message
                        MessageBox.Show("No recruiter record found for UserID: " + userId +
                            "\nPlease make sure this user is registered as a recruiter in the database.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error when getting recruiter ID: " + ex.Message);
                }
            }

            return recruiterId;
        }

        // Method to get UserID from login credentials
        private int GetUserIdFromDatabase(string username, string password)
        {
            int userId = -1; // default if not found

            string query = "SELECT UserID FROM Users WHERE Email = @Username AND PasswordHash = @PasswordHash AND IsApproved = 1";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", password); // assuming password is already hashed, if not you need to hash it here before comparing

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during login: " + ex.Message);
                }
            }
            return userId;
        }

        // Login method revised to return success/failure rather than showing a new form
        public bool Login(string username, string password)
        {
            // Query database to authenticate user and get their UserID
            int userId = GetUserIdFromDatabase(username, password);

            if (userId >= 0) // user found
            {
                LoggedInUser.UserID = userId;
                LoggedInUser.UserName = username;
                return true;
            }
            else
            {
                MessageBox.Show("Invalid login!");
                return false;
            }
        }

        // Method to schedule an interview
        private void ScheduleInterview(int applicationId, int recruiterId, DateTime interviewTime, string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please enter an interview location.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string insertQuery = @"INSERT INTO Interviews (ApplicationID, RecruiterID, InterviewTime, InterviewLocation, Result) 
                                       VALUES (@AppID, @RecID, @Time, @Loc, 'Pending')";

                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@AppID", applicationId);
                    cmd.Parameters.AddWithValue("@RecID", recruiterId);
                    cmd.Parameters.AddWithValue("@Time", interviewTime);
                    cmd.Parameters.AddWithValue("@Loc", location);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Interview scheduled successfully.");
                        LoadScheduledInterviews(); // Refresh the grid
                    }
                    else
                    {
                        MessageBox.Show("Failed to schedule interview. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scheduling interview: " + ex.Message);
            }
        }

        // Method to update an interview result
        private void UpdateInterviewResult(int interviewId, string result)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE Interviews SET Result = @Result WHERE InterviewID = @InterviewID";

                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@Result", result);
                    cmd.Parameters.AddWithValue("@InterviewID", interviewId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Interview result updated successfully.");
                        LoadScheduledInterviews(); // Refresh the grid
                    }
                    else
                    {
                        MessageBox.Show("Failed to update interview result. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating interview result: " + ex.Message);
            }
        }

        // Method to load pending applications
        private void LoadPendingApplications()
        {
            try
            {
                int recruiterId = GetRecruiterIdFromUserId(LoggedInUser.UserID);
                if (recruiterId == -1) return;

                string query = @"
                    SELECT 
                        a.ApplicationID,
                        j.JobID,
                        j.Title AS JobTitle,
                        u.FullName AS StudentName,
                        s.FAST_ID,
                        s.GPA,
                        a.ApplicationDate
                    FROM Applications a
                    JOIN Students s ON a.StudentID = s.StudentID
                    JOIN Users u ON s.UserID = u.UserID
                    JOIN JobPostings j ON a.JobID = j.JobID
                    WHERE j.CompanyID = (SELECT CompanyID FROM Recruiters WHERE RecruiterID = @RecruiterID)
                    AND NOT EXISTS (
                        SELECT 1 FROM Interviews i WHERE i.ApplicationID = a.ApplicationID
                    )";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecruiterID", recruiterId);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pending applications: " + ex.Message);
            }
        }

        // For testing - a method to manually set the logged-in user
        public void SetTestUser(int userId, string username)
        {
            LoggedInUser.UserID = userId;
            LoggedInUser.UserName = username;
            MessageBox.Show("Test user set: ID=" + userId + ", Name=" + username);
        }

        // Button click to schedule an interview
        private void button9_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an application to schedule an interview.");
                return;
            }

            try
            {
                int applicationId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ApplicationID"].Value);
                int recruiterId = GetRecruiterIdFromUserId(LoggedInUser.UserID);

                if (recruiterId == -1)
                {
                    return; // Error message already shown in GetRecruiterIdFromUserId
                }

                DateTime interviewTime = dateTimePicker1.Value;
                string location = textBox1.Text;

                ScheduleInterview(applicationId, recruiterId, interviewTime, location);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing selection: " + ex.Message);
            }
        }

        // Button to update interview result
        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an interview to update.");
                return;
            }

            try
            {
                int interviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["InterviewID"].Value);

                // Show dialog to select result
                using (Form resultForm = new Form())
                {
                    resultForm.Text = "Update Interview Result";
                    resultForm.Size = new Size(300, 200);
                    resultForm.StartPosition = FormStartPosition.CenterParent;

                    ComboBox resultCombo = new ComboBox();
                    resultCombo.Items.AddRange(new object[] { "Pending", "Selected", "Rejected" });
                    resultCombo.SelectedIndex = 0;
                    resultCombo.Location = new Point(50, 50);
                    resultCombo.Width = 200;

                    Button submitButton = new Button();
                    submitButton.Text = "Update";
                    submitButton.Location = new Point(100, 100);
                    submitButton.DialogResult = DialogResult.OK;

                    resultForm.Controls.Add(resultCombo);
                    resultForm.Controls.Add(submitButton);
                    resultForm.AcceptButton = submitButton;

                    if (resultForm.ShowDialog() == DialogResult.OK)
                    {
                        string result = resultCombo.SelectedItem.ToString();
                        UpdateInterviewResult(interviewId, result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating interview: " + ex.Message);
            }
        }

        // Navigation buttons
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

        // Empty event handlers from original code - kept for reference
        private void student_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void RecruiterDashBoard_Load(object sender, EventArgs e) { }
    }
}