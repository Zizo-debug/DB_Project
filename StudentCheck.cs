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
    public partial class StudentCheck : Form
    {
        // Database connection string
        private readonly string connectionString = @"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        // DataGridView to display student check-in data
        private DataGridView dgvStudentCheckIns;

        public StudentCheck()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // Initialize the DataGridView if it doesn't exist in the designer
            if (dgvStudentCheckIns == null)
            {
                dgvStudentCheckIns = new DataGridView();
                dgvStudentCheckIns.Name = "dgvStudentCheckIns";
                dgvStudentCheckIns.Location = new Point(280, 200);
                dgvStudentCheckIns.Size = new Size(760, 400);
                dgvStudentCheckIns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvStudentCheckIns.BackgroundColor = Color.White;
                dgvStudentCheckIns.BorderStyle = BorderStyle.Fixed3D;
                dgvStudentCheckIns.AllowUserToAddRows = false;
                dgvStudentCheckIns.AllowUserToDeleteRows = false;
                dgvStudentCheckIns.ReadOnly = true;
                dgvStudentCheckIns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvStudentCheckIns.RowHeadersVisible = false;
                dgvStudentCheckIns.MultiSelect = false;
                dgvStudentCheckIns.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                this.Controls.Add(dgvStudentCheckIns);
            }
        }

        private void StudentCheck_Load(object sender, EventArgs e)
        {
            LoadStudentCheckInData();
        }

        private void LoadStudentCheckInData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT s.FAST_ID, u.FullName, u.Email, sc.CheckInTime
                        FROM StudentBoothCheckIn sc
                        INNER JOIN Students s ON sc.StudentID = s.StudentID
                        INNER JOIN Users u ON s.UserID = u.UserID
                        INNER JOIN Booths b ON sc.BoothID = b.BoothID
                        ORDER BY sc.CheckInTime DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvStudentCheckIns.DataSource = dataTable;

                    // Format the columns
                    if (dgvStudentCheckIns.Columns.Contains("CheckInTime"))
                    {
                        dgvStudentCheckIns.Columns["CheckInTime"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt";
                    }

                    // Set column headers
                    if (dgvStudentCheckIns.Columns.Contains("FAST_ID"))
                        dgvStudentCheckIns.Columns["FAST_ID"].HeaderText = "Fast ID";

                    if (dgvStudentCheckIns.Columns.Contains("FullName"))
                        dgvStudentCheckIns.Columns["FullName"].HeaderText = "Full Name";

                    if (dgvStudentCheckIns.Columns.Contains("Email"))
                        dgvStudentCheckIns.Columns["Email"].HeaderText = "Email";

                    if (dgvStudentCheckIns.Columns.Contains("CheckInTime"))
                        dgvStudentCheckIns.Columns["CheckInTime"].HeaderText = "Check In Time";

                    if (dgvStudentCheckIns.Columns.Contains("BoothName"))
                        dgvStudentCheckIns.Columns["BoothName"].HeaderText = "Booth Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student check-in data: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add refresh button to reload data
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudentCheckInData();
        }

        // Navigation methods
        private void button6_Click(object sender, EventArgs e)
        {
            VisitorMonitoring home = new VisitorMonitoring();
            home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BoothCordHome home = new BoothCordHome();
            home.Show();
            this.Hide();
        }

        // Legacy empty methods
        private void label18_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
    }
}