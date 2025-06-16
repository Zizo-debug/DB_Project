using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using labDB_Interface;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace labDB_Interface
{
    public partial class LoginForm : Form
    {
        // Connection string - should ideally be stored in a configuration file
        private readonly string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public LoginForm()
        {
            InitializeComponent();
            // Set the password char for the masked text box (already done if using designer)
            maskedTextBox1.PasswordChar = '*';
            maskedTextBox1.UseSystemPasswordChar = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = maskedTextBox1.Text;

            // Basic validation
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Use parameterized query to prevent SQL injection
                    string query = "SELECT UserID, FullName, Role, IsApproved FROM Users WHERE Email = @Email AND PasswordHash = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // User exists with matching credentials
                                bool isApproved = Convert.ToBoolean(reader["IsApproved"]);

                                if (!isApproved)
                                {
                                    MessageBox.Show("Your account is pending approval.", "Access Denied",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                                // Successful login
                                int userId = Convert.ToInt32(reader["UserID"]);
                                string fullName = reader["FullName"].ToString();
                                string role = reader["Role"].ToString();

                                MessageBox.Show($"Welcome {fullName}!", "Login Successful",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Store user information and redirect based on role
                                UserSession.CurrentUserId = userId;
                                UserSession.CurrentUserName = fullName;
                                UserSession.CurrentUserRole = role;

                                // Open appropriate form based on role
                                OpenDashboardByRole(role);
                            }
                            else
                            {
                                MessageBox.Show("Invalid email or password.", "Login Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenDashboardByRole(string role)
        {
            if (role == "Student")
            {
                StudentDashBoard Home = new StudentDashBoard();
                Home.Show();
                this.Hide();
            }
            else if (role == "Recruiter")
            {
                RecruiterDashBoard Home = new RecruiterDashBoard();
                Home.Show();
                this.Hide();
            }
        }

        // Create a Register button click handler if you need one
        private void registerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open registration form
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        // Other event handlers (empty ones can be removed)
        private void PictureBox1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void LoginForm_Load(object sender, EventArgs e) { }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm rs = new RegisterForm();
            rs.Show();
            this.Hide();
        }
    }

    // Static class to store session information
    public static class UserSession
    {
        public static int CurrentUserId { get; set; }
        public static string CurrentUserName { get; set; }
        public static string CurrentUserRole { get; set; }
    }
}