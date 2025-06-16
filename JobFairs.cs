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
    public partial class JobFairs : Form
    {
        public JobFairs()
        {
            InitializeComponent();
        }

        private void student_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            JobSearch Home = new JobSearch();
            Home.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            feedback Home = new feedback();
            Home.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

           interviews Home = new interviews();
            Home.Show();
            this.Hide();
        }

        private void JobFairs_Load(object sender, EventArgs e)
        {

        }

        private void JobFairs_Load_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            StudentProfile st= new StudentProfile();
            st.Show();
            this.Hide();
        }
    }
}