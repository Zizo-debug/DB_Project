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
    public partial class SystemAnalytics : Form
    {
        // Connection string for your database
        private string connectionString = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        // Add the button as a class field
        private Button btnGenerate;


        public SystemAnalytics()
        {
            InitializeComponent();

            // Initialize Generate Reports button
            btnGenerate = new Button
            {
                Text = "Generate Reports",
                Location = new Point(593, 570),
                Size = new Size(184, 38),
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(btnGenerate);
        }


        private void label7_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Reports connection
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin home = new ManagerUserAdmin();
            home.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AssignBoothLocation home = new AssignBoothLocation();
            home.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Monitor home = new Monitor();
            home.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SystemAnalytics home = new SystemAnalytics();
            home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void SystemAnalytics_Load_1(object sender, EventArgs e)
        {
            // Empty event handler - appears to be duplicated in the designer
        }

        private void SystemAnalytics_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            GenerateReport3 home = new GenerateReport3();
            home.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            GenerateReport4 home = new GenerateReport4();
            home.Show();
            this.Hide();
        }
    }
}