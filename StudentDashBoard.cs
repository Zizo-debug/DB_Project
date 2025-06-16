using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labDB_Interface
{
    public partial class StudentDashBoard : Form
    {
        public StudentDashBoard()
        {
            InitializeComponent();
        }

        private void student_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        
        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void StudentDashBoard_Load(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            feedback Home = new feedback();
            Home.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            JobFairs Home = new JobFairs();
            Home.Show();
            this.Hide();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            JobSearch Home = new JobSearch();
            Home.Show();
            this.Hide();
        }


        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            feedback Feedback = new feedback();
            Feedback.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            StudentProfile st= new StudentProfile();
            st.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateReport gt = new GenerateReport();
            gt.Show();
            this.Hide();
        }
    }
}