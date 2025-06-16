using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class Monitor : Form
    {
        private class EventResource
        {
            public int BoothID { get; set; }
            public string BoothLocation { get; set; }
            public string CompanyName { get; set; }
            public int VisitorCount { get; set; }
            public string EventTitle { get; set; }
            public string CoordinatorName { get; set; }
            public int SpaceUsedPercent { get; set; } // For visualization purposes
            public int StaffAssigned { get; set; }    // Can be fetched from another table if available
        }

        private List<EventResource> resourceList = new List<EventResource>();
        private ComboBox comboEvents;
        private Button btnRefresh;
        private Panel resourcePanel;

        public Monitor()
        {
            InitializeComponent();
            InitializeCustomControls();
            this.Load += Monitor_Load;
        }

        private void InitializeCustomControls()
        {
            // Event selector
            Label lblEvent = new Label
            {
                Text = "Select Event:",
                Location = new Point(250, 80),
                AutoSize = true
            };
            this.Controls.Add(lblEvent);

            comboEvents = new ComboBox
            {
                Location = new Point(350, 76),
                Size = new Size(300, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboEvents.SelectedIndexChanged += ComboEvents_SelectedIndexChanged;
            this.Controls.Add(comboEvents);

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Refresh Data",
                Location = new Point(670, 75),
                Size = new Size(120, 26)
            };
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // Panel to contain resource monitors (enables scrolling)
            resourcePanel = new Panel
            {
                Location = new Point(250, 120),
                Size = new Size(550, 400),
                AutoScroll = true
            };
            this.Controls.Add(resourcePanel);
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            LoadEvents();
            if (comboEvents.Items.Count > 0)
                comboEvents.SelectedIndex = 0;
        }

        private void LoadEvents()
        {
            comboEvents.Items.Clear();
            Dictionary<int, string> eventDict = new Dictionary<int, string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    string query = "SELECT EventID, Title FROM JobFairEvents ORDER BY Date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int eventId = reader.GetInt32(0);
                            string title = reader.GetString(1);

                            // Store in dictionary for later retrieval
                            eventDict[eventId] = title;

                            // Add to combo box
                            comboEvents.Items.Add(new ComboBoxItem { ID = eventId, Text = title });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEvents.SelectedItem != null)
            {
                int selectedEventId = ((ComboBoxItem)comboEvents.SelectedItem).ID;
                LoadResourceData(selectedEventId);
                GenerateResourceMonitorUI();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (comboEvents.SelectedItem != null)
            {
                int selectedEventId = ((ComboBoxItem)comboEvents.SelectedItem).ID;
                LoadResourceData(selectedEventId);
                GenerateResourceMonitorUI();
            }
        }

        private void LoadResourceData(int eventId)
        {
            resourceList.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
                {
                    conn.Open();

                    string query = @"
                        SELECT b.BoothID, b.Location, c.CompanyName, b.VisitorCount, j.Title, 
                               b.CoordinatorID, 
                               CASE 
                                  WHEN u.FullName IS NULL THEN 'Unassigned' 
                                  ELSE u.FullName 
                               END as CoordinatorName
                        FROM Booths b
                        JOIN Companies c ON b.CompanyID = c.CompanyID
                        JOIN JobFairEvents j ON b.EventID = j.EventID
                        LEFT JOIN Users u ON b.CoordinatorID = u.UserID
                        WHERE b.EventID = @EventID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", eventId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // For this example, calculating staff assigned and space used randomly
                                // In a real app, you would fetch this from another table
                                Random rand = new Random();

                                try
                                {
                                    var resource = new EventResource
                                    {
                                        BoothID = reader.GetInt32(0),
                                        BoothLocation = reader.IsDBNull(1) ? "Not Assigned" : reader.GetString(1),
                                        CompanyName = reader.GetString(2),
                                        VisitorCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                        EventTitle = reader.GetString(4),
                                        // Cast CoordinatorID to string to avoid type mismatch
                                        CoordinatorName = reader.IsDBNull(6) ? "Unassigned" : reader.GetValue(6).ToString(),

                                        // In a real application, these would come from your database
                                        SpaceUsedPercent = rand.Next(30, 100),
                                        StaffAssigned = rand.Next(1, 6)
                                    };

                                    resourceList.Add(resource);
                                }
                                catch (InvalidCastException ex)
                                {
                                    // Log the error for debugging
                                    Console.WriteLine($"Cast error: {ex.Message} for row data");
                                    // Continue with next record without crashing
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading resource data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateResourceMonitorUI()
        {
            // Clear previous controls
            resourcePanel.Controls.Clear();

            int y = 10;
            foreach (var resource in resourceList)
            {
                // Booth and Company Info
                GroupBox boothBox = new GroupBox
                {
                    Text = $"Booth {resource.BoothID} - {resource.CompanyName}",
                    Location = new Point(10, y),
                    Size = new Size(510, 140),
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };

                // Location
                Label lblLocation = new Label
                {
                    Text = $"Location: {resource.BoothLocation}",
                    Location = new Point(10, 25),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular)
                };
                boothBox.Controls.Add(lblLocation);

                // Coordinator
                Label lblCoord = new Label
                {
                    Text = $"Coordinator: {resource.CoordinatorName}",
                    Location = new Point(10, 45),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular)
                };
                boothBox.Controls.Add(lblCoord);

                // Visitor Count
                Label lblVisitors = new Label
                {
                    Text = $"Visitor Count: {resource.VisitorCount}",
                    Location = new Point(10, 65),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular)
                };
                boothBox.Controls.Add(lblVisitors);

                // Staff Assigned
                Label lblStaff = new Label
                {
                    Text = $"Staff Assigned: {resource.StaffAssigned}",
                    Location = new Point(10, 85),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular)
                };
                boothBox.Controls.Add(lblStaff);

                // Space Usage Progress
                Label lblSpace = new Label
                {
                    Text = $"Space Usage: {resource.SpaceUsedPercent}%",
                    Location = new Point(250, 25),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular)
                };
                boothBox.Controls.Add(lblSpace);

                // Progress Bar
                ProgressBar pbSpace = new ProgressBar
                {
                    Location = new Point(250, 45),
                    Size = new Size(250, 20),
                    Value = resource.SpaceUsedPercent,
                    Style = ProgressBarStyle.Continuous
                };
                if (resource.SpaceUsedPercent >= 90)
                    pbSpace.ForeColor = Color.Red;
                else if (resource.SpaceUsedPercent >= 75)
                    pbSpace.ForeColor = Color.Orange;
                else
                    pbSpace.ForeColor = Color.Green;
                boothBox.Controls.Add(pbSpace);

                // Status indicator
                bool needsAttention = resource.SpaceUsedPercent >= 90 || resource.StaffAssigned < 2 ||
                                     resource.CoordinatorName == "Unassigned" || resource.BoothLocation == "Not Assigned";

                Label status = new Label
                {
                    Text = needsAttention ? "⚠ Needs Attention" : "✓ OK",
                    ForeColor = needsAttention ? Color.Red : Color.Green,
                    Location = new Point(250, 85),
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    AutoSize = true
                };
                boothBox.Controls.Add(status);

                // Action button for quick access
                Button btnManage = new Button
                {
                    Text = "Manage",
                    Location = new Point(425, 85),
                    Size = new Size(75, 23)
                };

                int boothId = resource.BoothID; // Preserve for the lambda
                btnManage.Click += (sender, e) => ManageBooth(boothId);
                boothBox.Controls.Add(btnManage);

                resourcePanel.Controls.Add(boothBox);
                y += 150;
            }

            // Add summary statistics at the bottom
            AddSummaryStats(y);
        }

        private void AddSummaryStats(int yPosition)
        {
            if (resourceList.Count == 0) return;

            GroupBox statsBox = new GroupBox
            {
                Text = "Event Summary",
                Location = new Point(10, yPosition),
                Size = new Size(510, 100),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            // Calculate stats
            int totalVisitors = 0;
            int totalBooths = resourceList.Count;
            int unassignedBooths = 0;
            int needsAttention = 0;

            foreach (var resource in resourceList)
            {
                totalVisitors += resource.VisitorCount;
                if (resource.BoothLocation == "Not Assigned")
                    unassignedBooths++;
                if (resource.SpaceUsedPercent >= 90 || resource.StaffAssigned < 2 ||
                    resource.CoordinatorName == "Unassigned" || resource.BoothLocation == "Not Assigned")
                    needsAttention++;
            }

            // Total visitors
            Label lblTotalVisitors = new Label
            {
                Text = $"Total Visitors: {totalVisitors}",
                Location = new Point(10, 25),
                AutoSize = true,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            statsBox.Controls.Add(lblTotalVisitors);

            // Booth stats
            Label lblTotalBooths = new Label
            {
                Text = $"Total Booths: {totalBooths} (Unassigned: {unassignedBooths})",
                Location = new Point(10, 45),
                AutoSize = true,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            statsBox.Controls.Add(lblTotalBooths);

            // Attention needed
            Label lblAttention = new Label
            {
                Text = $"Booths Needing Attention: {needsAttention}",
                Location = new Point(10, 65),
                AutoSize = true,
                Font = new Font("Arial", 9, FontStyle.Regular),
                ForeColor = needsAttention > 0 ? Color.Red : Color.Green
            };
            statsBox.Controls.Add(lblAttention);

            // Add to panel
            resourcePanel.Controls.Add(statsBox);
        }

        private void ManageBooth(int boothId)
        {
            // This would open a new form or dialog to manage the specific booth
            MessageBox.Show($"Opening management options for Booth {boothId}", "Manage Booth", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // In a real implementation, you might do something like:
            // BoothManagement form = new BoothManagement(boothId);
            // form.ShowDialog();
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
            // Implementation for button8 if needed
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }
    }

    // Helper class to store the ID and Text in ComboBox
    public class ComboBoxItem
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}