using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace labDB_Interface
{
    public partial class RegisterForm : Form
    {
        // Connection string from your previous code
        private readonly string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        // List of valid roles - only Student and Recruiter as specified
        private readonly string[] validRoles = { "Student", "Recruiter" };

        public RegisterForm()
        {
            InitializeComponent();

            // Set password masking
            textBox3.PasswordChar = '*';
            textBox3.UseSystemPasswordChar = true;

            // Set default role
            textBox4.Text = validRoles[0]; // Default to Student
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // Center the form on screen
            this.CenterToScreen();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Get form values
            string fullName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string password = textBox3.Text;
            string role = textBox4.Text.Trim();

            // Validate inputs
            if (!ValidateInputs(fullName, email, password, role))
            {
                return; // Validation failed
            }

            // Check if email already exists
            if (EmailExists(email))
            {
                MessageBox.Show("This email address is already registered.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // All validations passed, register the user
            if (RegisterUser(fullName, email, password, role))
            {
                MessageBox.Show("Registration successful! Your account is pending approval.", "Registration Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private bool ValidateInputs(string fullName, string email, string password, string role)
        {
            // Validate Full Name
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Validate Email
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            // Check email format using regex
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            // Validate Password
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            // Validate Role
            if (string.IsNullOrEmpty(role) || !Array.Exists(validRoles, r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Please enter a valid role (Student or Recruiter).", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return false;
            }

            return true;
        }

        private bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Assume email exists to prevent registration
            }
        }

        private bool RegisterUser(string fullName, string email, string password, string role)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Insert into Users and get new UserID
                    string userInsertQuery = @"
                INSERT INTO Users (FullName, Email, PasswordHash, Role, IsApproved) 
                VALUES (@FullName, @Email, @PasswordHash, @Role, @IsApproved);
                SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(userInsertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", password); // hash in real app
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@IsApproved", false);

                        int newUserId = Convert.ToInt32(cmd.ExecuteScalar());

                        if (role == "Student")
                        {
                            // Generate StudentID starting from 80
                            int newStudentId = 80;
                            string getMaxIdQuery = "SELECT ISNULL(MAX(StudentID), 79) FROM Students";
                            using (SqlCommand getIdCmd = new SqlCommand(getMaxIdQuery, conn))
                            {
                                newStudentId = Convert.ToInt32(getIdCmd.ExecuteScalar()) + 1;
                            }

                            // Extract FAST_ID from email (part before '@')
                            string fastId = email.Split('@')[0];

                            string insertStudent = "INSERT INTO Students (StudentID, UserID, FAST_ID) VALUES (@StudentID, @UserID, @FAST_ID)";
                            using (SqlCommand studentCmd = new SqlCommand(insertStudent, conn))
                            {
                                studentCmd.Parameters.AddWithValue("@StudentID", newStudentId);
                                studentCmd.Parameters.AddWithValue("@UserID", newUserId);
                                studentCmd.Parameters.AddWithValue("@FAST_ID", fastId);
                                studentCmd.ExecuteNonQuery();
                            }
                        }
                        else if (role == "Recruiter")
                        {
                            // Generate RecruiterID starting from 20
                            int newRecruiterId = 20;
                            string getMaxIdQuery = "SELECT ISNULL(MAX(RecruiterID), 19) FROM Recruiters";
                            using (SqlCommand getIdCmd = new SqlCommand(getMaxIdQuery, conn))
                            {
                                newRecruiterId = Convert.ToInt32(getIdCmd.ExecuteScalar()) + 1;
                            }

                            string insertRecruiter = "INSERT INTO Recruiters (RecruiterID, UserID) VALUES (@RecruiterID, @UserID)";
                            using (SqlCommand recruiterCmd = new SqlCommand(insertRecruiter, conn))
                            {
                                recruiterCmd.Parameters.AddWithValue("@RecruiterID", newRecruiterId);
                                recruiterCmd.Parameters.AddWithValue("@UserID", newUserId);
                                recruiterCmd.ExecuteNonQuery();
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            commonLoginRegister commonLoginRegister = new commonLoginRegister();
            commonLoginRegister.Show();
            this.Hide();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void RegisterForm_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}