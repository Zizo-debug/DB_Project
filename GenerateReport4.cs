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
    public partial class GenerateReport4 : Form
    {
        string connstr = "Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public GenerateReport4()
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
            b.BoothID,
            b.Location,
            b.VisitorCount
        FROM 
            Booths b";
        

            LoadReport("Report10.rdlc", query, "DataSet1");
        }

        private void GenerateReport4_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = @"
        SELECT 
            DATEPART(HOUR, CheckInTime) AS VisitHour,
            COUNT(*) AS VisitCount
        FROM 
            StudentBoothCheckIn
        GROUP BY 
            DATEPART(HOUR, CheckInTime)
        ORDER BY 
            VisitCount DESC;";

            LoadReport("Report11.rdlc", query, "DataSet1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = @"
    SELECT 
        b.BoothID, 
        b.Location, 
        COUNT(sbc.StudentID) AS VisitorCount, 
        bc.CoordinatorID, 
        u.Fullname AS CoordinatorName,
        bc.ShiftTime,
        DATEDIFF(MINUTE, 
            CAST(SUBSTRING(bc.ShiftTime, 1, CHARINDEX('-', bc.ShiftTime) - 1) AS DATETIME), 
            CAST(SUBSTRING(bc.ShiftTime, CHARINDEX('-', bc.ShiftTime) + 1, LEN(bc.ShiftTime)) AS DATETIME)
        ) AS ShiftDurationMinutes
    FROM Booths b
    LEFT JOIN StudentBoothCheckIn sbc ON b.BoothID = sbc.BoothID
    LEFT JOIN BoothCoordinators bc ON b.BoothID = bc.AssignedBoothID
    LEFT JOIN Users u ON bc.UserID = u.UserID
    GROUP BY b.BoothID, b.Location, bc.CoordinatorID, u.Fullname, bc.ShiftTime
    ORDER BY VisitorCount DESC, ShiftDurationMinutes DESC;";

            LoadReport("Report12.rdlc", query, "DataSet1");
        }
    }
}
