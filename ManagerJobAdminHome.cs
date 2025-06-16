using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
namespace labDB_Interface
{
    public partial class ManagerJobAdminHome : Form
    {
        public ManagerJobAdminHome()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin home = new ManagerUserAdmin();
            home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ScheduleAdminJob home = new ScheduleAdminJob();
            home.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AssignBoothLocation home = new AssignBoothLocation();
            home.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Monitor home = new Monitor();
            home.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
