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
    public partial class manageinterviews : Form
    {
        public manageinterviews()
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            GenerateReport2 home = new GenerateReport2();
            home.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            postJob home = new postJob();
            home.Show();
            this.Hide();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            postJob postJobForm = new postJob();
            postJobForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            viewApplicants va = new viewApplicants();
            va.Show();
            this.Hide();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void RecruiterDashBoard_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            scheduleinterviews va = new scheduleinterviews();
            va.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            recordinterviews va = new recordinterviews();
            va.Show();
            this.Hide();
        }
    }
}