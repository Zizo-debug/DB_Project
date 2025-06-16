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
    public partial class ManagerUserAdmin : Form
    {
        // Database connection string
        private string connectionString = @"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        // DataGridView for displaying users
        private DataGridView dgvUsers;

        // Buttons for user actions
        private Button btnApproveAll;
        private Button btnRefresh;

        public ManagerUserAdmin()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Initialize the DataGridView
            dgvUsers = new DataGridView();
            dgvUsers.Name = "dgvUsers";
            dgvUsers.Location = new Point(274, 200);
            dgvUsers.Size = new Size(750, 400);
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.MultiSelect = false;
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            this.Controls.Add(dgvUsers);

            // Add Approve All button
            btnApproveAll = new Button();
            btnApproveAll.Name = "btnApproveAll";
            btnApproveAll.Text = "Approve All";
            btnApproveAll.BackColor = Color.LightGreen;
            btnApproveAll.Location = new Point(924, 135);
            btnApproveAll.Size = new Size(100, 30);
            btnApproveAll.Click += new EventHandler(btnApproveAll_Click);
            this.Controls.Add(btnApproveAll);

            // Add Refresh button
            btnRefresh = new Button();
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(814, 135);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            this.Controls.Add(btnRefresh);

            // Set up the DataGridView columns
            SetupDataGridColumns();

            // Add button column for individual approval/disapproval
            AddActionButtonColumn();
        }

        private void SetupDataGridColumns()
        {
            dgvUsers.Columns.Clear();
            dgvUsers.Columns.Add("UserID", "UserID");
            dgvUsers.Columns.Add("FullName", "FullName");
            dgvUsers.Columns.Add("UserType", "User Type");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("GPACompany", "GPA/Company");
            dgvUsers.Columns.Add("Status", "Status");

            // Set column properties
            foreach (DataGridViewColumn col in dgvUsers.Columns)
            {
                col.ReadOnly = true;
            }
        }

        private void AddActionButtonColumn()
        {
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Action";
            btnColumn.Name = "ActionColumn";
            // Don't set Text property for custom button text per row
            btnColumn.UseColumnTextForButtonValue = false; // This is critical - must be FALSE
            dgvUsers.Columns.Add(btnColumn);

            // Add event handler for button clicks
            dgvUsers.CellClick += new DataGridViewCellEventHandler(dgvUsers_CellClick);
        }

        private void ManagerUserAdmin_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.Rows.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT u.UserID, u.FullName, u.Role as UserType, u.Email, u.IsApproved,
                               CASE 
                                   WHEN u.Role = 'Student' THEN 
                                        ISNULL(CAST(s.GPA AS VARCHAR), 'N/A')
                                   WHEN u.Role = 'Recruiter' THEN 
                                        ISNULL(c.CompanyName, 'N/A')
                                   ELSE 'N/A'
                               END as GPACompany
                        FROM Users u
                        LEFT JOIN Students s ON u.UserID = s.UserID
                        LEFT JOIN Recruiters r ON u.UserID = r.UserID
                        LEFT JOIN Companies c ON r.CompanyID = c.CompanyID
                        ORDER BY u.IsApproved DESC, u.UserID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string fullName = reader.GetString(1);
                                string userType = reader.GetString(2);
                                string email = reader.GetString(3);
                                bool isApproved = reader.GetBoolean(4);
                                string gpaCompany = reader.GetString(5);

                                string status = isApproved ? "Approved" : "Pending";

                                dgvUsers.Rows.Add(userId, fullName, userType, email, gpaCompany, status);
                            }
                        }
                    }
                }

                // Format the rows based on approval status
                FormatRowsByApprovalStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatRowsByApprovalStatus()
        {
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "Approved")
                {
                    // Show "Disapprove" button for approved users
                    DataGridViewButtonCell buttonCell = row.Cells["ActionColumn"] as DataGridViewButtonCell;
                    buttonCell.Value = "Disapprove";
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    // Show "Approve" button for pending users
                    DataGridViewButtonCell buttonCell = row.Cells["ActionColumn"] as DataGridViewButtonCell;
                    buttonCell.Value = "Approve";
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is a button cell and not the header
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvUsers.Columns["ActionColumn"].Index)
            {
                int userId = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["UserID"].Value);
                string fullName = dgvUsers.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                string email = dgvUsers.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                string role = dgvUsers.Rows[e.RowIndex].Cells["UserType"].Value.ToString();
                string extraInfo = dgvUsers.Rows[e.RowIndex].Cells["GPACompany"].Value.ToString();
                string status = dgvUsers.Rows[e.RowIndex].Cells["Status"].Value.ToString();

                // Get the button cell's value (which should be "Approve" or "Disapprove")
                DataGridViewButtonCell buttonCell = dgvUsers.Rows[e.RowIndex].Cells["ActionColumn"] as DataGridViewButtonCell;
                string action = buttonCell.Value?.ToString();

                if (action == "Approve")
                {
                    ApproveUser(userId, fullName, email, role, extraInfo);
                }
                else if (action == "Disapprove")
                {
                    DisapproveUser(userId, fullName);
                }

                LoadUsers(); // Refresh the grid
            }
        }

        private void btnApproveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int approvedCount = 0;

                foreach (DataGridViewRow row in dgvUsers.Rows)
                {
                    if (row.Cells["Status"].Value.ToString() == "Pending")
                    {
                        int userId = Convert.ToInt32(row.Cells["UserID"].Value);
                        string fullName = row.Cells["FullName"].Value.ToString();
                        string email = row.Cells["Email"].Value.ToString();
                        string role = row.Cells["UserType"].Value.ToString();
                        string extraInfo = row.Cells["GPACompany"].Value.ToString();

                        if (ApproveUser(userId, fullName, email, role, extraInfo))
                        {
                            approvedCount++;
                        }
                    }
                }

                MessageBox.Show($"Successfully approved {approvedCount} users.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadUsers(); // Refresh the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving all users: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private bool ApproveUser(int userId, string fullName, string email, string role, string extraInfo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Approve the user
                    string updateUserQuery = "UPDATE Users SET IsApproved = 1 WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(updateUserQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Insert into the appropriate table based on role
                    if (role == "Student")
                    {
                        // Check if student already exists
                        if (!StudentExists(userId, conn, transaction))
                        {
                            decimal gpa = 0;
                            if (decimal.TryParse(extraInfo, out gpa))
                            {
                                string insertStudentQuery = @"
                                    INSERT INTO Students (StudentID, UserID, FAST_ID, DegreeProgram, CurrentSemester, GPA) 
                                    VALUES (@UserID, @UserID, @FAST_ID, @DegreeProgram, @Semester, @GPA)";
                                using (SqlCommand cmd = new SqlCommand(insertStudentQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@UserID", userId);
                                    cmd.Parameters.AddWithValue("@FAST_ID", "FAST" + userId.ToString("D5")); // Generate FAST ID
                                    cmd.Parameters.AddWithValue("@DegreeProgram", "BSCS"); // Default or get from registration
                                    cmd.Parameters.AddWithValue("@Semester", 1); // Default or get from registration
                                    cmd.Parameters.AddWithValue("@GPA", gpa);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else if (role == "Recruiter")
                    {
                        // Check if recruiter already exists
                        if (!RecruiterExists(userId, conn, transaction))
                        {
                            int companyId = GetOrCreateCompanyId(extraInfo, conn, transaction);
                            string insertRecruiterQuery = @"
                                INSERT INTO Recruiters (RecruiterID, UserID, CompanyID, Designation, ContactNo) 
                                VALUES (@UserID, @UserID, @CompanyID, @Designation, @ContactNo)";
                            using (SqlCommand cmd = new SqlCommand(insertRecruiterQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserID", userId);
                                cmd.Parameters.AddWithValue("@CompanyID", companyId);
                                cmd.Parameters.AddWithValue("@Designation", "HR Representative"); // Default
                                cmd.Parameters.AddWithValue("@ContactNo", "N/A"); // Default
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show($"User '{fullName}' has been approved.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error approving user: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool DisapproveUser(int userId, string fullName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE Users SET IsApproved = 0 WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"User '{fullName}' has been disapproved.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to disapprove user '{fullName}'.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disapproving user: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool StudentExists(int userId, SqlConnection conn, SqlTransaction transaction)
        {
            string query = "SELECT COUNT(*) FROM Students WHERE UserID = @UserID";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private bool RecruiterExists(int userId, SqlConnection conn, SqlTransaction transaction)
        {
            string query = "SELECT COUNT(*) FROM Recruiters WHERE UserID = @UserID";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private int GetOrCreateCompanyId(string companyName, SqlConnection conn, SqlTransaction transaction)
        {
            // First check if company exists
            string query = "SELECT CompanyID FROM Companies WHERE CompanyName = @Name";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Name", companyName);
                var result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Create the company if it doesn't exist
                    string insertQuery = @"
                        INSERT INTO Companies (CompanyName, Sector, Website, Address) 
                        VALUES (@Name, 'Unknown', 'N/A', 'N/A');
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@Name", companyName);
                        return Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ManagerJobAdminHome managerHome = new ManagerJobAdminHome();
            managerHome.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ModifyUserAdmin modifyUser = new ModifyUserAdmin();
            //modifyUser.Show();
            //this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SystemAnalytics systemAnalytics = new SystemAnalytics();
            systemAnalytics.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        // Additional empty event handlers from the designer
        private void label7_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label15_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void panel11_Paint(object sender, PaintEventArgs e) { }
        private void label19_Click(object sender, EventArgs e) { }
        private void label50_Click(object sender, EventArgs e) { }
        private void label29_Click(object sender, EventArgs e) { }
        private void label10_Click_1(object sender, EventArgs e) { }
        private void button10_Click_1(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }
        private void button15_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void label20_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label17_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }
        private void label6_Click_1(object sender, EventArgs e) { }
        private void ManagerUserAdmin_Load_1(object sender, EventArgs e) { }

        // Add any missing click handlers from error output
        private void panel15_Paint(object sender, PaintEventArgs e) { }
        private void button15_Click_1(object sender, EventArgs e) { }
        private void panel11_Paint_1(object sender, PaintEventArgs e) { }
        private void button10_Click(object sender, EventArgs e) { }
        private void label20_Click_1(object sender, EventArgs e) { }
        private void label19_Click_1(object sender, EventArgs e) { }
        private void label14_Click_1(object sender, EventArgs e) { }
        private void label12_Click_1(object sender, EventArgs e) { }
        private void label17_Click_1(object sender, EventArgs e) { }
        private void label18_Click(object sender, EventArgs e) { }
    }
}