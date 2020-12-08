using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace course_project
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            int userTypeIndex = userTypeBox.SelectedIndex;
            if (userTypeIndex == -1)
            {
                MessageBox.Show("Пользователь не задан!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            LoginForm2 loginForm2 = new LoginForm2(userTypeIndex);
            loginForm2.Show();         
        }
    }
}
