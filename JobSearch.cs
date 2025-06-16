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

namespace labDB_Interface
{
    public partial class JobSearch : Form
    {
        // Connection string - update with your actual connection details
        private string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        // DataTable to store job listings
        private DataTable jobsTable = new DataTable();

        public JobSearch()
        {
            InitializeComponent();
            SetupJobListingControls();

            // Load jobs when form loads
            this.Load += JobSearch_Load;
        }

        private void JobSearch_Load(object sender, EventArgs e)
        {
            // Initialize controls and load data
            InitializeSearchControls();
            LoadJobListings();
        }

        private void InitializeSearchControls()
        {
            // Add search textbox
            TextBox txtSearch = new TextBox();
            txtSearch.Name = "txtSearch";
            txtSearch.Location = new Point(192, 125);
            txtSearch.Size = new Size(230, 25);
            txtSearch.Text = "Search job title or skills...";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Search job title or skills...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search job title or skills...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            Controls.Add(txtSearch);

            // Add search button
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(427, 125);
            btnSearch.Size = new Size(70, 25);
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            // Add filter combobox for job type
            ComboBox cboJobType = new ComboBox();
            cboJobType.Name = "cboJobType";
            cboJobType.Location = new Point(192, 95);
            cboJobType.Size = new Size(130, 25);
            cboJobType.Items.AddRange(new object[] { "All Types", "Internship", "Full-time", "Part-time", "Contract" });
            cboJobType.SelectedIndex = 0;
            cboJobType.SelectedIndexChanged += Filter_Changed;
            Controls.Add(cboJobType);

            // Add salary range filter combobox
            ComboBox cboSalary = new ComboBox();
            cboSalary.Name = "cboSalary";
            cboSalary.Location = new Point(330, 95);
            cboSalary.Size = new Size(130, 25);
            cboSalary.Items.AddRange(new object[] { "All Salaries", "Below 25k", "25k-50k", "50k-100k", "Above 100k" });
            cboSalary.SelectedIndex = 0;
            cboSalary.SelectedIndexChanged += Filter_Changed;
            Controls.Add(cboSalary);
        }

        private void SetupJobListingControls()
        {
            // Clear any existing job listing labels
            // Create a temporary list to avoid collection modification issues
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control ctrl in Controls)
            {
                if (ctrl.Name != null && (ctrl.Name.StartsWith("lblJob") || ctrl.Name.StartsWith("btnApply")))
                {
                    controlsToRemove.Add(ctrl);
                }
            }

            foreach (Control ctrl in controlsToRemove)
            {
                Controls.Remove(ctrl);
                ctrl.Dispose();
            }
        }

        private void LoadJobListings(string searchText = "", string jobTypeFilter = "All Types", string salaryFilter = "All Salaries")
        {
            try
            {
                // Clear existing job listings
                List<Control> controlsToRemove = new List<Control>();

                foreach (Control ctrl in Controls)
                {
                    if (ctrl.Name != null && (ctrl.Name.StartsWith("lblJob") || ctrl.Name.StartsWith("btnApply")))
                    {
                        controlsToRemove.Add(ctrl);
                    }
                }

                foreach (Control ctrl in controlsToRemove)
                {
                    Controls.Remove(ctrl);
                    ctrl.Dispose();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Build the query based on search and filter criteria
                    StringBuilder queryBuilder = new StringBuilder("SELECT JobID, Title, Description, JobType, SalaryRange, RequiredSkills FROM JobPostings WHERE 1=1");

                    // Add search filter if provided
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        queryBuilder.Append(" AND (Title LIKE @Search OR RequiredSkills LIKE @Search)");
                    }

                    // Add job type filter if selected
                    if (jobTypeFilter != "All Types")
                    {
                        queryBuilder.Append(" AND JobType = @JobType");
                    }

                    // Add salary filter if selected
                    if (salaryFilter != "All Salaries")
                    {
                        // More robust salary filtering
                        switch (salaryFilter)
                        {
                            case "Below 25k":
                                queryBuilder.Append(" AND (");
                                queryBuilder.Append(" ISNUMERIC(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1) AS INT) < 25");
                                queryBuilder.Append(" )");
                                break;
                            case "25k-50k":
                                queryBuilder.Append(" AND (");
                                queryBuilder.Append(" ISNUMERIC(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1) AS INT) >= 25");
                                queryBuilder.Append(" AND (CHARINDEX('-', SalaryRange) > 0 AND ISNUMERIC(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1) AS INT) <= 50)");
                                queryBuilder.Append(" )");
                                break;
                            case "50k-100k":
                                queryBuilder.Append(" AND (");
                                queryBuilder.Append(" ISNUMERIC(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, 1, PATINDEX('%[^0-9]%', SalaryRange + 'x') - 1) AS INT) >= 50");
                                queryBuilder.Append(" AND (CHARINDEX('-', SalaryRange) > 0 AND ISNUMERIC(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1) AS INT) <= 100)");
                                queryBuilder.Append(" )");
                                break;
                            case "Above 100k":
                                queryBuilder.Append(" AND (");
                                queryBuilder.Append(" CHARINDEX('-', SalaryRange) > 0");
                                queryBuilder.Append(" AND ISNUMERIC(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1)) = 1");
                                queryBuilder.Append(" AND CAST(SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, PATINDEX('%[^0-9]%', SUBSTRING(SalaryRange, CHARINDEX('-', SalaryRange) + 1, LEN(SalaryRange)) + 'x') - 1) AS INT) > 100");
                                queryBuilder.Append(" )");
                                break;
                        }
                    }

                    // Limit results
                    queryBuilder.Append(" ORDER BY JobID DESC");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        // Add parameters if needed
                        if (!string.IsNullOrEmpty(searchText))
                        {
                            command.Parameters.AddWithValue("@Search", "%" + searchText + "%");
                        }

                        if (jobTypeFilter != "All Types")
                        {
                            command.Parameters.AddWithValue("@JobType", jobTypeFilter);
                        }

                        try
                        {
                            // Execute the query
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Clear existing data
                                jobsTable.Clear();

                                // Load into data table
                                jobsTable.Load(reader);

                                // Display the jobs
                                DisplayJobListings();
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show("SQL Error: " + sqlEx.Message + "\nQuery: " + queryBuilder.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job listings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayJobListings()
        {
            int startY = 180;
            int rowHeight = 30;

            // Remove existing job listing controls
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control ctrl in Controls)
            {
                if (ctrl.Name != null && (ctrl.Name.StartsWith("lblJob") || ctrl.Name.StartsWith("btnApply")))
                {
                    controlsToRemove.Add(ctrl);
                }
            }

            foreach (Control ctrl in controlsToRemove)
            {
                Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            // Display up to 8 jobs (or however many are available)
            for (int i = 0; i < Math.Min(jobsTable.Rows.Count, 8); i++)
            {
                DataRow job = jobsTable.Rows[i];

                // Create job listing label
                Label lblJob = new Label();
                lblJob.Name = "lblJob" + i;
                lblJob.Location = new Point(176, startY + (i * rowHeight));
                lblJob.Size = new Size(420, 20);
                lblJob.Text = $"{job["Title"]}  | {TruncateText(job["Description"].ToString(), 20)}  | {job["JobType"]}  | {job["SalaryRange"]}  | {TruncateText(job["RequiredSkills"].ToString(), 15)}";
                lblJob.Tag = job["JobID"]; // Store JobID for reference
                Controls.Add(lblJob);

                // Create Apply button for each job
                Button btnApply = new Button();
                btnApply.Name = "btnApply" + i;
                btnApply.Text = "Apply";
                btnApply.Size = new Size(60, 25);
                btnApply.Location = new Point(600, startY + (i * rowHeight) - 3);
                btnApply.Tag = job["JobID"]; // Store JobID for reference
                btnApply.Click += BtnApply_Click;
                Controls.Add(btnApply);
            }

            // Add "No jobs found" message if no results
            if (jobsTable.Rows.Count == 0)
            {
                Label lblNoJobs = new Label();
                lblNoJobs.Name = "lblJobNoResults";
                lblNoJobs.Location = new Point(300, 250);
                lblNoJobs.Size = new Size(200, 20);
                lblNoJobs.Text = "No job listings found.";
                lblNoJobs.ForeColor = Color.Red;
                Controls.Add(lblNoJobs);
            }
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength) + "...";
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            TextBox txtSearch = Controls.Find("txtSearch", true).FirstOrDefault() as TextBox;
            ComboBox cboJobType = Controls.Find("cboJobType", true).FirstOrDefault() as ComboBox;
            ComboBox cboSalary = Controls.Find("cboSalary", true).FirstOrDefault() as ComboBox;

            if (txtSearch != null && cboJobType != null && cboSalary != null)
            {
                string searchText = txtSearch.Text;
                string jobTypeFilter = cboJobType.SelectedItem.ToString();
                string salaryFilter = cboSalary.SelectedItem.ToString();

                LoadJobListings(searchText, jobTypeFilter, salaryFilter);
            }
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            // When filter changes, reapply search with current filters
            TextBox txtSearch = Controls.Find("txtSearch", true).FirstOrDefault() as TextBox;
            ComboBox cboJobType = Controls.Find("cboJobType", true).FirstOrDefault() as ComboBox;
            ComboBox cboSalary = Controls.Find("cboSalary", true).FirstOrDefault() as ComboBox;

            if (txtSearch != null && cboJobType != null && cboSalary != null)
            {
                string searchText = txtSearch.Text;
                string jobTypeFilter = cboJobType.SelectedItem.ToString();
                string salaryFilter = cboSalary.SelectedItem.ToString();

                LoadJobListings(searchText, jobTypeFilter, salaryFilter);
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Button btnApply = sender as Button;
            if (btnApply != null)
            {
                int jobId = Convert.ToInt32(btnApply.Tag);

                // Here you would open a job application form or dialog
                // For now, we'll just show a confirmation message
                DialogResult result = MessageBox.Show("Are you sure you want to apply for this job?", "Confirm Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // In a real application, you would get the student ID from their login session
                        int studentId = 1; // Placeholder - replace with actual student ID

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // First check if the student has already applied for this job
                            // Using the correct table name from your schema (Applications, not JobApplications)
                            string checkQuery = "SELECT COUNT(*) FROM Applications WHERE StudentID = @StudentID AND JobID = @JobID";

                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                            {
                                checkCommand.Parameters.AddWithValue("@StudentID", studentId);
                                checkCommand.Parameters.AddWithValue("@JobID", jobId);

                                int count = (int)checkCommand.ExecuteScalar();

                                if (count > 0)
                                {
                                    MessageBox.Show("You have already applied for this job.", "Application Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            // Insert the job application using the correct table name (Applications)
                            string insertQuery = @"
                                INSERT INTO Applications (StudentID, JobID, ApplicationDate)
                                VALUES (@StudentID, @JobID, @ApplicationDate)";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@StudentID", studentId);
                                command.Parameters.AddWithValue("@JobID", jobId);
                                command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now);

                                command.ExecuteNonQuery();

                                MessageBox.Show("Your application has been submitted successfully!", "Application Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error submitting application: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Existing event handlers
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e)
        {
            JobFairs Home = new JobFairs();
            Home.Show();
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void StudentDashBoard_Load(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }

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

        private void JobSearch_Load_1(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            StudentProfile studentProfile = new StudentProfile();
            studentProfile.Show();
            this.Hide();    
        }
    }
}