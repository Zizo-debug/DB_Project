using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.Data.SqlClient;

namespace labDB_Interface
{
    public partial class GenerateReport : Form
    {
        string connstr = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public GenerateReport()
        {
            InitializeComponent();
        }

        private void GenerateReport_Load(object sender, EventArgs e)
        {
            // Optional initialization
        }


        private void LoadReport(string reportName, string query, string dataSourceName)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportViewer reportViewer = new ReportViewer
            {
                Location = new Point(10, 100),
                Size = new Size(750, 400),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                ProcessingMode = ProcessingMode.Local
            };

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportEmbeddedResource = $"labDB_Interface.{reportName}"; // e.g. "labDB_Interface.Report1.rdlc"
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, dt));
            reportViewer.RefreshReport();

            this.Controls.Clear();
            this.Controls.Add(reportViewer);
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query1 = @"
        SELECT s.DegreeProgram, COUNT(s.StudentID) AS StudentCount
        FROM Students s
        GROUP BY s.DegreeProgram";
            LoadReport("Report1.rdlc", query1, "DeptWiseStudent");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query2 = @"
        SELECT TOP 10 u.FullName, s.GPA
        FROM Students s
        JOIN Users u ON s.UserID = u.UserID
        ORDER BY s.GPA DESC";

            LoadReport("Report2.rdlc", query2, "DataSet1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query3 = @"
        SELECT TOP 10 sk.SkillName, COUNT(ss.StudentID) AS Frequency
        FROM StudentSkills ss
        JOIN Skills sk ON ss.SkillID = sk.SkillID
        GROUP BY sk.SkillName
        ORDER BY Frequency DESC";

            LoadReport("Report3.rdlc", query3, "skills");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
