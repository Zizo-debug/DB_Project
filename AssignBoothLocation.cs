using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class AssignBoothLocation : Form
    {
        private Dictionary<string, int> boothMap = new Dictionary<string, int>();
        ComboBox comboBoxBooths = new ComboBox();
        TextBox txtLocation = new TextBox();
        Button btnAssignBooth = new Button();
        ListBox listBoxAssignments = new ListBox();
        Label labelInfo = new Label();

        public AssignBoothLocation()
        {
            InitializeComponent();
            //this.Load += AssignBoothLocation_Load;
        }

        /*private void AssignBoothLocation_Load(object sender, EventArgs e)
        {
            // ComboBox for booth selection
            comboBoxBooths.Location = new Point(300, 130);
            comboBoxBooths.Size = new Size(200, 30);
            this.Controls.Add(comboBoxBooths);

            // TextBox for entering new location
            txtLocation.Location = new Point(300, 160);
            txtLocation.Size = new Size(200, 30);
            txtLocation.Text = "Enter new location";
            txtLocation.ForeColor = Color.Gray;
            txtLocation.Enter += (s, ev) =>
            {
                if (txtLocation.Text == "Enter new location")
                {
                    txtLocation.Text = "";
                    txtLocation.ForeColor = Color.Black;
                }
            };
            txtLocation.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtLocation.Text))
                {
                    txtLocation.Text = "Enter new location";
                    txtLocation.ForeColor = Color.Gray;
                }
            };
            this.Controls.Add(txtLocation);

            // Assign Booth Button
            btnAssignBooth.Text = "Assign Booth";
            btnAssignBooth.Location = new Point(300, 200);
            btnAssignBooth.Size = new Size(200, 30);
            //btnAssignBooth.Click += BtnAssignBooth_Click;
            this.Controls.Add(btnAssignBooth);

            // Info label
            labelInfo.Location = new Point(300, 240);
            labelInfo.Size = new Size(400, 30);
            this.Controls.Add(labelInfo);

            // ListBox for assignments
            listBoxAssignments.Location = new Point(300, 280);
            listBoxAssignments.Size = new Size(400, 100);
            this.Controls.Add(listBoxAssignments);

            //sLoadBoothsFromDatabase();
        }

       private void LoadBoothsFromDatabase()
        {
            comboBoxBooths.Items.Clear();
            boothMap.Clear(); // Clear the previous mapping

            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalproject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
            {
                conn.Open();
                string query = "SELECT BoothID, Location FROM Booths";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int boothId = reader.GetInt32(0);
                        string location = reader.IsDBNull(1) ? "Not Assigned" : reader.GetString(1);
                        string displayText = $"Booth {boothId} - {location}";

                        comboBoxBooths.Items.Add(displayText);
                        boothMap[displayText] = boothId; // Save mapping of display text to booth ID
                    }
                }
            }

            if (comboBoxBooths.Items.Count > 0)
                comboBoxBooths.SelectedIndex = 0;
        }
private void BtnAssignBooth_Click(object sender, EventArgs e)
        {
            if (comboBoxBooths.SelectedItem == null || string.IsNullOrWhiteSpace(txtLocation.Text) || txtLocation.Text == "Enter new location")
            {
                MessageBox.Show("Please select a booth and enter a valid location.");
                return;
            }

            string selectedText = comboBoxBooths.SelectedItem.ToString();

            // Get the booth ID from the dictionary using the selected text
            if (!boothMap.TryGetValue(selectedText, out int boothId))
            {
                MessageBox.Show("Booth ID not found. Please try again.");
                return;
            }

            string newLocation = txtLocation.Text.Trim();

            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-COI7Q9N7\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"))
            {
                conn.Open();
                string updateQuery = "UPDATE Booths SET Location = @Location WHERE BoothID = @BoothID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Location", newLocation);
                    cmd.Parameters.AddWithValue("@BoothID", boothId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        listBoxAssignments.Items.Add($"Booth {boothId} assigned to '{newLocation}'");
                        labelInfo.Text = $"Booth {boothId} updated successfully.";
                        LoadBoothsFromDatabase(); // Refresh list
                    }
                    else
                    {
                        MessageBox.Show("Update failed.");
                    }
                }
            }
        }*/

        private void label7_Click(object sender, EventArgs e)
        {
            // Optional: handle title label click, or leave empty
        }

         private void AssignBoothLocation_Load_1(object sender, EventArgs e)
         {
             // Call your main form load method
             //AssignBoothLocation_Load(sender, e);
         }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin home = new ManagerUserAdmin();
            home.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Monitor home = new Monitor();
            home.Show();
            this.Hide();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
        
    }
}