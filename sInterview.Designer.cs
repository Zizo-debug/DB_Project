using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace labDB_Interface
{
    partial class sInterview : Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sInterview));
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBoxRecruiters = new System.Windows.Forms.ComboBox();
            this.comboBoxApplications = new System.Windows.Forms.ComboBox();
            this.comboBoxTimeSlots = new System.Windows.Forms.ComboBox();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.buttonSchedule = new System.Windows.Forms.Button();
            this.labelRecruiters = new System.Windows.Forms.Label();
            this.labelApplications = new System.Windows.Forms.Label();
            this.labelTimeSlots = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewInterviews = new System.Windows.Forms.DataGridView();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterviews)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.comboBoxRecruiters);
            this.panel2.Controls.Add(this.comboBoxApplications);
            this.panel2.Controls.Add(this.comboBoxTimeSlots);
            this.panel2.Controls.Add(this.textBoxLocation);
            this.panel2.Controls.Add(this.buttonSchedule);
            this.panel2.Controls.Add(this.labelRecruiters);
            this.panel2.Controls.Add(this.labelApplications);
            this.panel2.Controls.Add(this.labelTimeSlots);
            this.panel2.Controls.Add(this.labelLocation);
            this.panel2.Location = new System.Drawing.Point(12, 96);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(224, 343);
            this.panel2.TabIndex = 12;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button4.Location = new System.Drawing.Point(24, 270);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(192, 46);
            this.button4.TabIndex = 3;
            this.button4.Text = "Go Back";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // comboBoxRecruiters
            // 
            this.comboBoxRecruiters.FormattingEnabled = true;
            this.comboBoxRecruiters.Location = new System.Drawing.Point(24, 30);
            this.comboBoxRecruiters.Name = "comboBoxRecruiters";
            this.comboBoxRecruiters.Size = new System.Drawing.Size(192, 28);
            this.comboBoxRecruiters.TabIndex = 4;
            this.comboBoxRecruiters.SelectedIndexChanged += new System.EventHandler(this.comboBoxRecruiters_SelectedIndexChanged);
            // 
            // comboBoxApplications
            // 
            this.comboBoxApplications.FormattingEnabled = true;
            this.comboBoxApplications.Location = new System.Drawing.Point(24, 90);
            this.comboBoxApplications.Name = "comboBoxApplications";
            this.comboBoxApplications.Size = new System.Drawing.Size(192, 28);
            this.comboBoxApplications.TabIndex = 5;
            // 
            // comboBoxTimeSlots
            // 
            this.comboBoxTimeSlots.FormattingEnabled = true;
            this.comboBoxTimeSlots.Location = new System.Drawing.Point(24, 150);
            this.comboBoxTimeSlots.Name = "comboBoxTimeSlots";
            this.comboBoxTimeSlots.Size = new System.Drawing.Size(192, 28);
            this.comboBoxTimeSlots.TabIndex = 6;
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(24, 210);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(192, 26);
            this.textBoxLocation.TabIndex = 7;
            // 
            // buttonSchedule
            // 
            this.buttonSchedule.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSchedule.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSchedule.Location = new System.Drawing.Point(24, 240);
            this.buttonSchedule.Name = "buttonSchedule";
            this.buttonSchedule.Size = new System.Drawing.Size(192, 30);
            this.buttonSchedule.TabIndex = 8;
            this.buttonSchedule.Text = "Schedule Interview";
            this.buttonSchedule.UseVisualStyleBackColor = false;
            this.buttonSchedule.Click += new System.EventHandler(this.buttonSchedule_Click_1);
            // 
            // labelRecruiters
            // 
            this.labelRecruiters.AutoSize = true;
            this.labelRecruiters.Location = new System.Drawing.Point(24, 10);
            this.labelRecruiters.Name = "labelRecruiters";
            this.labelRecruiters.Size = new System.Drawing.Size(74, 20);
            this.labelRecruiters.TabIndex = 10;
            this.labelRecruiters.Text = "Recruiter";
            // 
            // labelApplications
            // 
            this.labelApplications.AutoSize = true;
            this.labelApplications.Location = new System.Drawing.Point(24, 70);
            this.labelApplications.Name = "labelApplications";
            this.labelApplications.Size = new System.Drawing.Size(87, 20);
            this.labelApplications.TabIndex = 11;
            this.labelApplications.Text = "Application";
            // 
            // labelTimeSlots
            // 
            this.labelTimeSlots.AutoSize = true;
            this.labelTimeSlots.Location = new System.Drawing.Point(24, 130);
            this.labelTimeSlots.Name = "labelTimeSlots";
            this.labelTimeSlots.Size = new System.Drawing.Size(75, 20);
            this.labelTimeSlots.TabIndex = 12;
            this.labelTimeSlots.Text = "Time Slot";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(24, 190);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(70, 20);
            this.labelLocation.TabIndex = 13;
            this.labelLocation.Text = "Location";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.pictureBox8);
            this.panel1.Controls.Add(this.pictureBox7);
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.pictureBox5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 86);
            this.panel1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.label1.Location = new System.Drawing.Point(209, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 54);
            this.label1.TabIndex = 1;
            this.label1.Text = "Student Dashboard";
            // 
            // dataGridViewInterviews
            // 
            this.dataGridViewInterviews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInterviews.Location = new System.Drawing.Point(248, 100);
            this.dataGridViewInterviews.Name = "dataGridViewInterviews";
            this.dataGridViewInterviews.RowHeadersWidth = 62;
            this.dataGridViewInterviews.Size = new System.Drawing.Size(630, 330);
            this.dataGridViewInterviews.TabIndex = 9;
            this.dataGridViewInterviews.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewInterviews_CellContentClick);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox8.Image")));
            this.pictureBox8.Location = new System.Drawing.Point(792, 12);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(83, 60);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 6;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Click += new System.EventHandler(this.pictureBox8_Click);
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(704, 12);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(83, 60);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 5;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(12, 12);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(83, 60);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 4;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(119, 12);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(83, 60);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 3;
            this.pictureBox5.TabStop = false;
            // 
            // sInterview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.dataGridViewInterviews);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "sInterview";
            this.Text = "Schedule Interview";
            this.Load += new System.EventHandler(this.sInterview_Load_1);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterviews)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);

        }

        private void sInterview_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Windows Form Designer generated code
        private Panel panel2;
        private Panel panel1;
        private PictureBox pictureBox8;
        private PictureBox pictureBox7;
        private PictureBox pictureBox6;
        private PictureBox pictureBox5;
        private Label label1;
        private Button button4;
        private ComboBox comboBoxRecruiters;
        private ComboBox comboBoxApplications;
        private ComboBox comboBoxTimeSlots;
        private TextBox textBoxLocation;
        private Button buttonSchedule;
        private DataGridView dataGridViewInterviews;
        private Label labelRecruiters;
        private Label labelApplications;
        private Label labelTimeSlots;
        private Label labelLocation;
        #endregion
    }
}