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
    public partial class ModifyUserAdmin : Form
    {
        public ModifyUserAdmin()
        {
            InitializeComponent();
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ManagerJobAdminHome managerHome = new ManagerJobAdminHome();
            managerHome.Show();
            this.Hide();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerUserAdmin manager = new ManagerUserAdmin();
            manager.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            HomePage Home = new HomePage();
            Home.Show();
            this.Hide();
        }

        private void ModifyUserAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
