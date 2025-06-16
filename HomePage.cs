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
using Microsoft.Reporting.WinForms;
namespace labDB_Interface
{
    public partial class HomePage : Form
    {
        string connstr = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public HomePage()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                Location = new Point(10, 100),
                Size = new Size(750, 400),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                ProcessingMode = ProcessingMode.Local
            };

            string query = @"
                    SELECT s.DegreeProgram, COUNT(s.StudentID) AS StudentCount
                    FROM Students s
                    GROUP BY s.DegreeProgram";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportEmbeddedResource = "labDB_Interface.Report1.rdlc"; // Adjust namespace and name
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            reportViewer.RefreshReport();

            this.Controls.Add(reportViewer);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin userAdminForm = new ManagerUserAdmin();
            userAdminForm.Show(); // Use ShowDialog() if you want it modal
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin userAdminForm = new ManagerUserAdmin();
            userAdminForm.Show(); // Use ShowDialog() if you want it modal
            this.Hide();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            ManagerJobAdminHome managerHome = new ManagerJobAdminHome();
            managerHome.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ManagerJobAdminHome managerHome = new ManagerJobAdminHome();
            managerHome.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SystemAnalytics systemAnalytics = new SystemAnalytics();
            systemAnalytics.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }
    }
}
