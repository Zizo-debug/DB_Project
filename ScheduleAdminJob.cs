using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class ScheduleAdminJob : Form
    {
        private DateTime selectedDate;

        public ScheduleAdminJob()
        {
            InitializeComponent();

            // Add calendar control
            MonthCalendar calendar = new MonthCalendar
            {
                Location = new Point(200, 100),
                MaxSelectionCount = 1
            };
            calendar.DateSelected += Calendar_DateSelected;
            this.Controls.Add(calendar);
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDate = e.Start;

            using (TimeWindowDialog dialog = new TimeWindowDialog(selectedDate))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string start = dialog.StartTime;
                    string end = dialog.EndTime;
                    string eventName = dialog.EventName;
                    string venue = dialog.Venue;

                    using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-BRTULLM\SQLEXPRESS;Initial Catalog=finalProject;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
                    {
                        conn.Open();

                        string insertQuery = @"INSERT INTO JobFairEvents (Title, Description, Date, StartTime, EndTime, Venue)
                                               VALUES (@Title, @Description, @Date, @StartTime, @EndTime, @Venue)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Title", eventName);
                            cmd.Parameters.AddWithValue("@Description", "-");
                            cmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                            cmd.Parameters.AddWithValue("@StartTime", DateTime.Parse(start).ToString("HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@EndTime", DateTime.Parse(end).ToString("HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@Venue", venue);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Event '{eventName}' scheduled on {selectedDate.ToShortDateString()} from {start} to {end} at {venue}.", "Event Scheduled");
                }
            }
        }

        private void ScheduleAdminJob_Load(object sender, EventArgs e) { }

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
    }

    public class TimeWindowDialog : Form
    {
        public string StartTime => startBox.SelectedItem?.ToString();
        public string EndTime => endBox.SelectedItem?.ToString();
        public string EventName => nameBox.Text;
        public string Venue => venueBox.Text;

        private ComboBox startBox, endBox;
        private TextBox nameBox, venueBox;

        public TimeWindowDialog(DateTime date)
        {
            this.Text = "Schedule Event";
            this.Size = new Size(400, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                RowCount = 6,
                ColumnCount = 2
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            for (int i = 0; i < 6; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lblDate = new Label { Text = $"Date: {date.ToShortDateString()}", Anchor = AnchorStyles.Left | AnchorStyles.Right, AutoSize = true };
            Label lblStart = new Label { Text = "Start Time:", Anchor = AnchorStyles.Left, AutoSize = true };
            Label lblEnd = new Label { Text = "End Time:", Anchor = AnchorStyles.Left, AutoSize = true };
            Label lblName = new Label { Text = "Event Name:", Anchor = AnchorStyles.Left, AutoSize = true };
            Label lblVenue = new Label { Text = "Venue:", Anchor = AnchorStyles.Left, AutoSize = true };

            startBox = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            endBox = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            nameBox = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            venueBox = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };

            for (int hour = 9; hour <= 17; hour++)
            {
                string ampm = hour < 12 ? "AM" : "PM";
                int displayHour = hour <= 12 ? hour : hour - 12;
                if (displayHour == 0) displayHour = 12;

                string time = $"{displayHour}:00 {ampm}";
                startBox.Items.Add(time);
                endBox.Items.Add(time);
            }

            startBox.SelectedIndex = 0;
            endBox.SelectedIndex = endBox.Items.Count - 1;

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            Button btnOK = new Button { Text = "OK", Width = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Width = 80, DialogResult = DialogResult.Cancel };

            btnOK.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(venueBox.Text))
                {
                    MessageBox.Show("Please enter both event name and venue.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                if (startBox.SelectedIndex >= endBox.SelectedIndex)
                {
                    MessageBox.Show("End time must be after start time.", "Invalid Time Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            };

            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnOK);

            panel.Controls.Add(lblDate, 0, 0);
            panel.SetColumnSpan(lblDate, 2);

            panel.Controls.Add(lblStart, 0, 1);
            panel.Controls.Add(startBox, 1, 1);

            panel.Controls.Add(lblEnd, 0, 2);
            panel.Controls.Add(endBox, 1, 2);

            panel.Controls.Add(lblName, 0, 3);
            panel.Controls.Add(nameBox, 1, 3);

            panel.Controls.Add(lblVenue, 0, 4);
            panel.Controls.Add(venueBox, 1, 4);

            panel.Controls.Add(buttonPanel, 0, 5);
            panel.SetColumnSpan(buttonPanel, 2);

            this.Controls.Add(panel);
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
}
