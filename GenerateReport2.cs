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
    public partial class GenerateReport2 : Form
    {
        string connstr = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public GenerateReport2()
        {
            InitializeComponent();
        }


        private void GenerateReport2_Load(object sender, EventArgs e)
        {

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
            u.FullName AS RecruiterName,
            c.CompanyName,
            COUNT(i.InterviewID) AS TotalInterviews
        FROM 
            Recruiters r
        JOIN 
            Users u ON r.UserID = u.UserID
        JOIN 
            Companies c ON r.CompanyID = c.CompanyID
        LEFT JOIN 
            Interviews i ON r.RecruiterID = i.RecruiterID
        GROUP BY 
            u.FullName, c.CompanyName
        ORDER BY 
            TotalInterviews DESC";

            LoadReport("Report4.rdlc", query, "DataSet1");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string query = @"
        SELECT 
            u.FullName AS RecruiterName,
            c.CompanyName,
            COUNT(i.InterviewID) AS TotalInterviews,
            COUNT(CASE WHEN i.Result = 'Selected' THEN 1 END) AS TotalOffers,
            CAST(
                COUNT(CASE WHEN i.Result = 'Selected' THEN 1 END) * 100.0 / 
                NULLIF(COUNT(i.InterviewID), 0) AS DECIMAL(5,2)
            ) AS OfferRatioPercent
        FROM 
            Interviews i
        JOIN 
            Recruiters r ON i.RecruiterID = r.RecruiterID
        JOIN 
            Users u ON r.UserID = u.UserID
        JOIN 
            Companies c ON r.CompanyID = c.CompanyID
        GROUP BY 
            u.FullName, c.CompanyName
        ORDER BY 
            OfferRatioPercent DESC";

            LoadReport("Report5.rdlc", query, "DataSet1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = @"
        SELECT 
            u.FullName AS RecruiterName,
            c.CompanyName,
            COUNT(rv.ReviewID) AS TotalReviews,
            AVG(CAST(rv.Rating AS FLOAT)) AS AverageRating
        FROM 
            Reviews rv
        JOIN 
            Interviews i ON rv.InterviewID = i.InterviewID
        JOIN 
            Recruiters r ON i.RecruiterID = r.RecruiterID
        JOIN 
            Users u ON r.UserID = u.UserID
        JOIN 
            Companies c ON r.CompanyID = c.CompanyID
        GROUP BY 
            u.FullName, c.CompanyName
        ORDER BY 
            AverageRating DESC";

            LoadReport("Report6.rdlc", query, "DataSet1");
        }
    }
}
