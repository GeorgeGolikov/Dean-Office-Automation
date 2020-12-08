using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace course_project
{
    public partial class LoginForm2 : Form
    {
        private int userTypeIndex;
        private OleDbDataAdapter da;

        public LoginForm2()
        {
            InitializeComponent();
        }

        public LoginForm2(int userTypeIndex)
        {
            InitializeComponent();
            this.userTypeIndex = userTypeIndex;

            connectToDB();

            string query = "SELECT * FROM ";
            switch (userTypeIndex)
            {
                case 0:
                    this.Text = "Я - Декан";

                    query += "Director";
                    da = new OleDbDataAdapter(query, cn);
                    da.Fill(dataSet, "Director");               
                    
                    userBox.DataSource = dataSet.Tables["Director"];
                    userBox.ValueMember = "DirectorID";
                    userBox.DisplayMember = "FIO";                  
                    break;
                case 1:
                    this.Text = "Я - Студент";

                    query += "Students";
                    da = new OleDbDataAdapter(query, cn);
                    da.Fill(dataSet, "Students");

                    userBox.DataSource = dataSet.Tables["Students"];
                    userBox.ValueMember = "StudentID";
                    userBox.DisplayMember = "FIO";
                    break;
                case 2:
                    this.Text = "Я - Методист";

                    query += "Methodists";
                    da = new OleDbDataAdapter(query, cn);
                    da.Fill(dataSet, "Methodists");

                    userBox.DataSource = dataSet.Tables["Methodists"];
                    userBox.ValueMember = "MethodistID";
                    userBox.DisplayMember = "FIO";
                    break;
                default:
                    Console.WriteLine("Default case");
                    MessageBox.Show("Пользователь не задан!", " Внимание!", MessageBoxButtons.OK);
                    this.Close();
                    break;
            }
        }

        private void connectToDB()
        {
            try
            {
                cn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed!!", "Connection", MessageBoxButtons.OK);
                this.Close();
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string userID = userBox.SelectedValue.ToString();
            string userFIO = userBox.Text;
            string pwdFromBox = pwdBox.Text;

            string pwdFromDB;

            switch(userTypeIndex)
            {
                case 0:
                    pwdFromDB = (string) dataSet.Tables["Director"].Select("DirectorID = " + userID)[0]["Pwd"];
                    if (pwdFromBox.Equals(pwdFromDB))
                    {
                        DirectorForm directorForm = new DirectorForm(userID, userFIO);
                        directorForm.Show();
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", " Внимание!", MessageBoxButtons.OK);
                    break;
                case 1:
                    pwdFromDB = (string) dataSet.Tables["Students"].Select("StudentID = " + userID)[0]["Pwd"];
                    if (pwdFromBox.Equals(pwdFromDB))
                    {
                        StudentForm studentForm = new StudentForm(userID, userFIO);
                        studentForm.Show();
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", " Внимание!", MessageBoxButtons.OK);
                    break;
                case 2:
                    pwdFromDB = (string) dataSet.Tables["Methodists"].Select("MethodistID = " + userID)[0]["Pwd"];
                    if (pwdFromBox.Equals(pwdFromDB))
                    {
                        MethodistForm methodistForm = new MethodistForm(userID, userFIO);
                        methodistForm.Show();
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", " Внимание!", MessageBoxButtons.OK);
                    break;
                default:
                    Console.WriteLine("Default case");
                    MessageBox.Show("Пользователь не задан!", " Внимание!", MessageBoxButtons.OK);
                    this.Close();
                    break;
            }
        }

        private void LoginForm2_Load(object sender, EventArgs e)
        {          

        }
    }
}
