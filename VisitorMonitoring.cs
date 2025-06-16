using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class VisitorMonitoring : Form
    {
        private Dictionary<int, (string Location, int VisitorCount)> boothData; // BoothID => (Location, Count)
        private string connectionString = @"Data Source=DESKTOP-BRTULLM\\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"; // Replace with your actual connection string

        public VisitorMonitoring()
        {
            InitializeComponent();
        }

        private void VisitorMonitoring_Load(object sender, EventArgs e)
        {
            LoadBoothData();
            GenerateBoothUI();
        }

        private void LoadBoothData()
        {
            boothData = new Dictionary<int, (string, int)>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BoothID, Location, VisitorCount FROM Booths";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int boothId = reader.GetInt32(0);
                        string location = reader.GetString(1);
                        int visitorCount = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                        boothData[boothId] = (location, visitorCount);
                    }
                }
            }
        }

        private void GenerateBoothUI()
        {
            int yPos = 120;

            foreach (var kvp in boothData)
            {
                int boothId = kvp.Key;
                string location = kvp.Value.Location;
                int visitCount = kvp.Value.VisitorCount;

                string labelName = $"lbl_Booth{boothId}";

                Label lbl = new Label
                {
                    Text = $"{location}: {visitCount} visits",
                    Location = new Point(250, yPos),
                    AutoSize = true,
                    Name = labelName
                };
                this.Controls.Add(lbl);

                Button btn = new Button
                {
                    Text = "Add Visit",
                    Location = new Point(500, yPos - 5),
                    Tag = boothId
                };
                btn.Click += BoothVisitButton_Click;
                this.Controls.Add(btn);

                yPos += 40;
            }
        }

        private void BoothVisitButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int boothId = (int)clickedButton.Tag;

            // Update database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE Booths SET VisitorCount = VisitorCount + 1 WHERE BoothID = @BoothID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@BoothID", boothId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Reload booth data and update UI
            LoadBoothData();

            var labelToUpdate = this.Controls
                .OfType<Label>()
                .FirstOrDefault(lbl => lbl.Name == $"lbl_Booth{boothId}");

            if (labelToUpdate != null)
            {
                var updated = boothData[boothId];
                labelToUpdate.Text = $"{updated.Location}: {updated.VisitorCount} visits";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StudentCheck home = new StudentCheck();
            home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BoothCordHome home = new BoothCordHome();
            home.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
