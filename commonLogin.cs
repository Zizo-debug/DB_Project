using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.VisualBasic.ApplicationServices;

namespace labDB_Interface
{
    public partial class commonLoginRegister : Form
    {
        public commonLoginRegister()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm loginForm = new RegisterForm();
            loginForm.Show();
            this.Hide();
        }

        private void commonLoginRegister_Load(object sender, EventArgs e)
        {

        }

        private void commonLoginRegister_Load_1(object sender, EventArgs e)
        {

        }
    }
}