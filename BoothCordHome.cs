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
    public partial class BoothCordHome : Form
    {
        public BoothCordHome()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            StudentCheck home = new StudentCheck();
            home.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            VisitorMonitoring home = new VisitorMonitoring();
            home.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StudentCheck home = new StudentCheck();
            home.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VisitorMonitoring home = new VisitorMonitoring();
            home.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void BoothCordHome_Load(object sender, EventArgs e)
        {

        }
    }
}
