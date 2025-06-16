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
    public partial class GenerateReport3 : Form
    {
        string connstr = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public GenerateReport3()
        {
            InitializeComponent();
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
        private void button1_Click(object sender, EventArgs e)
        {
            string query = @"
        SELECT 
            COUNT(DISTINCT s.StudentID) AS TotalApplicants,
            COUNT(DISTINCT CASE WHEN i.Result = 'Selected' THEN s.StudentID END) AS PlacedStudents,
            CAST(
                COUNT(DISTINCT CASE WHEN i.Result = 'Selected' THEN s.StudentID END) * 100.0 / 
                NULLIF(COUNT(DISTINCT s.StudentID), 0) AS DECIMAL(5,2)
            ) AS PlacementRate
        FROM 
            Applications a
        JOIN 
            Students s ON a.StudentID = s.StudentID
        LEFT JOIN 
            Interviews i ON i.ApplicationID = a.ApplicationID";

            LoadReport("Report7.rdlc", query, "DataSet1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = @"
        SELECT 
            s.DegreeProgram AS Department,
            COUNT(DISTINCT s.StudentID) AS TotalApplicants,
            COUNT(DISTINCT CASE WHEN i.Result = 'Selected' THEN s.StudentID END) AS PlacedStudents,
            CAST(
                COUNT(DISTINCT CASE WHEN i.Result = 'Selected' THEN s.StudentID END) * 100.0 /
                NULLIF(COUNT(DISTINCT s.StudentID), 0) AS DECIMAL(5,2)
            ) AS PlacementRate
        FROM 
            Applications a
        JOIN 
            Students s ON a.StudentID = s.StudentID
        LEFT JOIN 
            Interviews i ON i.ApplicationID = a.ApplicationID
        GROUP BY 
            s.DegreeProgram
        ORDER BY 
            PlacementRate DESC";

            LoadReport("Report8.rdlc", query, "DataSet1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = @"
SELECT 
    AVG(SalaryMin * 1000) AS AverageMinSalary,
    AVG(SalaryMax * 1000) AS AverageMaxSalary
FROM (
    SELECT 
        CAST(LEFT(j.SalaryRange, CHARINDEX('k', j.SalaryRange) - 1) AS FLOAT) AS SalaryMin,
        CAST(
            SUBSTRING(
                j.SalaryRange,
                CHARINDEX('-', j.SalaryRange) + 1,
                CHARINDEX('k', j.SalaryRange, CHARINDEX('-', j.SalaryRange)) - CHARINDEX('-', j.SalaryRange) - 1
            ) AS FLOAT
        ) AS SalaryMax
    FROM 
        Interviews i
    JOIN 
        Applications a ON i.ApplicationID = a.ApplicationID
    JOIN 
        JobPostings j ON a.JobID = j.JobID
    WHERE 
        i.Result = 'Selected'
        AND j.SalaryRange LIKE '%k-%k%' -- Ensure pattern like '60k-80k'
) AS ParsedSalaries;
";

            LoadReport("Report9.rdlc", query, "DataSet1");
        }

        private void GenerateReport3_Load(object sender, EventArgs e)
        {

        }
    }
}
