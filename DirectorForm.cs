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
    public partial class DirectorForm : Form
    {
        private string userID;
        private string userFIO;
        private OleDbConnection cn;

        private OleDbDataAdapter da;

        public DirectorForm()
        {
            InitializeComponent();
        }

        public DirectorForm(string userID, string userFIO, OleDbConnection cn)
        {
            InitializeComponent();

            this.userID = userID;
            this.userFIO = userFIO;
            this.cn = cn;

            this.Text += " " + userFIO;

            setStudentsComboboxes();

            string query = "SELECT * FROM Groups";
            string tableName = "Groups";
            executeQuery(query, tableName);
            groupsBox.ValueMember = "GroupID";
            groupsBox.DisplayMember = "Naming";
            groupsBox.DataSource = dataSet.Tables[tableName];

            groups2Box.ValueMember = "GroupID";
            groups2Box.DisplayMember = "Naming";
            groups2Box.DataSource = dataSet.Tables[tableName];

            group3Box.ValueMember = "GroupID";
            group3Box.DisplayMember = "Naming";
            group3Box.DataSource = dataSet.Tables[tableName];
                        
            newGroupBox.ValueMember = "GroupID";
            newGroupBox.DisplayMember = "Naming";
            newGroupBox.DataSource = dataSet.Tables[tableName];

            query = "SELECT * FROM Statuses";
            tableName = "Statuses";
            executeQuery(query, tableName);
            reqStatusBox.ValueMember = "StatusID";
            reqStatusBox.DisplayMember = "Naming";
            reqStatusBox.DataSource = dataSet.Tables[tableName];

            setMethsComboboxes();
            setDirectorInstrsList();
        }

        private void setDirectorInstrsList()
        {
            string query = "SELECT MethodistID, FIO, Summ, Cause, DateIssued, DateCompleted, Naming " +
                "FROM DirectorInstructions " +
                "JOIN Students ON DirectorInstructions.StudentID = Students.StudentID " +
                "JOIN Statuses ON DirectorInstructions.StatusID = Statuses.StatusID";
            string tableName = "DirectorInstrs";
            executeQuery(query, tableName);
            tasksList.DataSource = dataSet.Tables[tableName];
        }

        private void setStudentsComboboxes()
        {
            string query = "SELECT * FROM Students";
            string tableName = "Students";
            executeQuery(query, tableName);
            studentsBox.ValueMember = "StudentID";
            studentsBox.DisplayMember = "FIO";
            studentsBox.DataSource = dataSet.Tables[tableName];

            studentScholBox.ValueMember = "StudentID";
            studentScholBox.DisplayMember = "FIO";
            studentScholBox.DataSource = dataSet.Tables[tableName];

            studentGroupBox.ValueMember = "StudentID";
            studentGroupBox.DisplayMember = "FIO";
            studentGroupBox.DataSource = dataSet.Tables[tableName];

            students2Box.ValueMember = "StudentID";
            students2Box.DisplayMember = "FIO";
            students2Box.DataSource = dataSet.Tables[tableName];
        }

        private void executeQuery(string query, string tableName)
        {
            da = new OleDbDataAdapter(query, cn);
            DataTable table = dataSet.Tables[tableName];
            if (table != null)
            {
                dataSet.Tables[tableName].Clear();
            }
            da.Fill(dataSet, tableName);
        }

        private object getOneValueFromDataSet(string tableName, string property)
        {
            DataRow[] row = dataSet.Tables[tableName].Select();
            if (row.Length != 0)
            {
                return row[0][property];
            }
            return null;
        }

        private void studentsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object stud = studentsBox.SelectedValue;
            string studID;
            if (stud != null)
            {
                studID = stud.ToString();
                string query = "select avg(Valuee) AS Average from Grades where StudentID = " + studID;
                string tableName = "AverageGrade";
                executeQuery(query, tableName);
                DataRow[] row = dataSet.Tables[tableName].Select();
                if (row.Length != 0)
                {
                    averageGradeBox.Text = row[0]["Average"].ToString();
                }
            }                 
        }

        private void groupsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = groupsBox.SelectedValue.ToString();

            string query = "SELECT Naming, SUM(case when Students.SexID = 1 then 1 else 0 end) AS Boys, " +
                           "SUM(case when Students.SexID = 2 then 1 else 0 end) AS Girls " +
                           "FROM Groups, Students " +
                           "WHERE Groups.GroupID = Students.GroupID AND Students.GroupID = " + group +
                           " GROUP BY(Naming)";
            string tableName = "BoysAndGirlsInGroup";
            executeQuery(query, tableName);

            DataRow[] rows = dataSet.Tables[tableName].Select();
            if (rows.Length != 0)
            {
                numGirlsBox.Text = rows[0]["Girls"].ToString();
                numBoysBox.Text = rows[0]["Boys"].ToString();
            }
            else
            {
                numGirlsBox.Text = "-";
                numBoysBox.Text = "-";
            }
        }

        private void reqTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typeVal = reqTypeBox.SelectedItem.ToString();

            string sqlExpression = "sp_loadReqDirector";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OleDbParameter type = new OleDbParameter
            {
                ParameterName = "@Type",
                Value = typeVal
            };
            command.Parameters.Add(type);

            var result = command.ExecuteReader();

            if (result.HasRows)
            {
                //DataTable table = dataSet.Tables["DirectorReqs"];
                DataTable table = new DataTable("DirectorReqs");
                //if (table != null)
                //{
                //    table.Clear();                    
                //}
                table.Load(result);
                reqList.DataSource = table;

                reqIdBox.ValueMember = "ID_заявки";
                reqIdBox.DisplayMember = "ID_заявки";
                reqIdBox.DataSource = table;                
            }
            result.Close();
        }

        private void setReqStatus_Click(object sender, EventArgs e)
        {            
            object req = reqIdBox.SelectedValue;
            object status = reqStatusBox.SelectedValue;
            if (req == null || status == null)
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
            }
            else
            {
                string typeVal = reqTypeBox.SelectedItem.ToString();
                int reqIDD = int.Parse(req.ToString());
                int statusIDD = int.Parse(status.ToString());
                DateTime datee = dateBox.Value;

                string sqlExpression = "sp_loadReqDirectorWithChange";
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                OleDbParameter type = new OleDbParameter
                {
                    ParameterName = "@Type",
                    Value = typeVal
                };
                OleDbParameter reqID = new OleDbParameter
                {
                    ParameterName = "@RequestID",
                    Value = reqIDD
                };
                OleDbParameter statusID = new OleDbParameter
                {
                    ParameterName = "@StatusID",
                    Value = statusIDD
                };
                OleDbParameter date = new OleDbParameter
                {
                    ParameterName = "@DateAcRej",
                    Value = datee.Date
                };
                command.Parameters.Add(type);
                command.Parameters.Add(reqID);
                command.Parameters.Add(statusID);
                command.Parameters.Add(date);

                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    DataTable table = new DataTable("DirectorReqs");

                    table.Load(result);
                    reqList.DataSource = table;

                    reqIdBox.ValueMember = "ID_заявки";
                    reqIdBox.DisplayMember = "ID_заявки";
                    reqIdBox.DataSource = table;
                }
                result.Close();
            }
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            string fio = fioBox.Text;
            DateTime dateOfBirth = dateBirthBox.Value;
            int sexID = sexBox.SelectedIndex + 1;
            string groupID = groupsBox.SelectedValue.ToString();

            if (fio.Equals("") || sexID == 0 || groupID.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }

            string sqlExpression = "INSERT INTO Students (FIO, Birthday, SexID, GroupID, Pwd) " +
                                    "VALUES ('" + fio +
                                    "','" + dateOfBirth.Date.Year + "-" + dateOfBirth.Date.Month + "-" +
                                    dateOfBirth.Date.Day +
                                    "'," + sexID.ToString() + "," + groupID + ",'123')";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);

            int number = command.ExecuteNonQuery();
            Console.WriteLine("Добавлено объектов: {0}", number); // 1
            setStudentsComboboxes();
            MessageBox.Show("Студент зачислен!", " Внимание!", MessageBoxButtons.OK);
        }

        private void group3Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = group3Box.SelectedValue.ToString();

            string query = "SELECT FIO, Birthday FROM Students WHERE GroupID = " + group;
            string tableName = "StudentsList";
            executeQuery(query, tableName);

            groupList.DataSource = dataSet.Tables[tableName];
        }

        private void setScholBut_Click(object sender, EventArgs e)
        {
            string studID = studentScholBox.SelectedValue.ToString();
            string type = typeBox.Text;
            string sum = sumBox.Text;
            DateTime date = date2Box.Value;
            string orderName = orderNameBox.Text;

            int num;
            bool isNum = int.TryParse(sum, out num);

            if (studID.Equals("") || type.Equals("") || !isNum || orderName.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                int summ = int.Parse(sum);
                string sqlExpression = "INSERT INTO ScholarshipOrders (Naming, Summ, DateStarted, OrderName) " +
                                    "VALUES ('" + type +
                                    "'," + sum + ",'" + date.Date.Year + "-" + date.Date.Month + "-" +
                                    date.Date.Day +
                                    "','" + orderName + "')";
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number); // 1

                sqlExpression = "SELECT ScholOrderID FROM ScholarshipOrders WHERE " +
                    "Naming ='" + type + "' AND Summ = " + sum + " AND DateStarted = '" +
                    date.Date.Year + "-" + date.Date.Month + "-" + date.Date.Day + "' AND OrderName = '"
                    + orderName + "'";
                command = new OleDbCommand(sqlExpression, cn);

                var result = command.ExecuteReader();

                string scholOrderID = "";
                if (result.HasRows)
                {
                    DataTable table = new DataTable("ScholOrderIDs");

                    table.Load(result);

                    scholOrderID = table.Select()[0]["ScholOrderID"].ToString();                    
                }
                result.Close();

                if (!scholOrderID.Equals(""))
                {
                    sqlExpression = "UPDATE Students SET ScholarshipOrderID = " + scholOrderID +
                                    " WHERE StudentID = " + studID;

                    command = new OleDbCommand(sqlExpression, cn);
                    number = command.ExecuteNonQuery();
                    Console.WriteLine("Добавлено объектов: {0}", number); // 1
                    MessageBox.Show("Стипендия назначена!", " Внимание!", MessageBoxButtons.OK);
                }                
            }
        }

        private void dropStudentButton_Click(object sender, EventArgs e)
        {
            string studID = studentGroupBox.SelectedValue.ToString();
            if (studID.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "DELETE FROM Students WHERE StudentID = " + studID;
                                   
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number); // 1
                setStudentsComboboxes();
                MessageBox.Show("Студент отчислен!", " Внимание!", MessageBoxButtons.OK);
            }
        }

        private void groupChangeButton_Click_1(object sender, EventArgs e)
        {
            string studID = studentGroupBox.SelectedValue.ToString();
            string group = newGroupBox.SelectedValue.ToString();
            if (studID.Equals("") || group.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "UPDATE Students SET GroupID = " + group +
                                       " WHERE StudentID = " + studID;

                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Изменено объектов: {0}", number); // 1
                MessageBox.Show("Студент переведен!", " Внимание!", MessageBoxButtons.OK);
            }
        }

        private void methTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typeVal = methTypeBox.SelectedItem.ToString();

            string sqlExpression = "sp_loadMethodists";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OleDbParameter type = new OleDbParameter
            {
                ParameterName = "@Type",
                Value = typeVal
            };
            command.Parameters.Add(type);

            var result = command.ExecuteReader();

            if (result.HasRows)
            {
                DataTable table = new DataTable("TypesMeths");
                if (table != null)
                {
                    table.Clear();
                    table.Load(result);
                    methodistsList.DataSource = table;
                }                
            }
            else
            {
                DataTable table = new DataTable("TypesMeths");
                if (table != null)
                {
                    table.Clear();
                    methodistsList.DataSource = table;
                }
            }
            result.Close();
        }

        private void dropMethodistButton_Click(object sender, EventArgs e)
        {
            string methID = methodistsBox.SelectedValue.ToString();
            if (methID.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "DELETE FROM Methodists WHERE MethodistID = " + methID;

                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number); // 1
                setMethsComboboxes();
                MessageBox.Show("Методист уволен!", " Внимание!", MessageBoxButtons.OK);
            }
        }

        private void setMethsComboboxes()
        {
            string query = "SELECT * FROM Methodists";
            string tableName = "Methodists";
            executeQuery(query, tableName);
            methodistsBox.ValueMember = "MethodistID";
            methodistsBox.DisplayMember = "FIO";
            methodistsBox.DataSource = dataSet.Tables[tableName];
        }

        private void hireMeth_Click(object sender, EventArgs e)
        {
            string fio = methFioBox.Text;
            DateTime dateOfBirth = dateBirthMeth.Value;
            int sexID = sexMethBox.SelectedIndex + 1;
            string salary = salaryBox.Text;
            int num;
            bool isNum = int.TryParse(salary, out num);

            if (fio.Equals("") || sexID == 0 || !isNum)
            {
                MessageBox.Show("Одно или более полей некорректны!", " Внимание!", MessageBoxButtons.OK);
                return;
            }

            string sqlExpression = "INSERT INTO Methodists (FIO, Birthday, SexID, Salary, Pwd) " +
                                    "VALUES ('" + fio +
                                    "','" + dateOfBirth.Date.Year + "-" + dateOfBirth.Date.Month + "-" +
                                    dateOfBirth.Date.Day +
                                    "'," + sexID.ToString() + "," + salary + ", '12')";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);

            int number = command.ExecuteNonQuery();
            Console.WriteLine("Добавлено объектов: {0}", number); // 1
            setMethsComboboxes();
            MessageBox.Show("Методист добавлен!", " Внимание!", MessageBoxButtons.OK);
        }

        private void newTaskButton_Click(object sender, EventArgs e)
        {
            string studID = students2Box.SelectedValue.ToString();
            string cause = causeTaskBox.Text;
            string sum = sumTaskBox.Text;
            DateTime date = dateTaskBox.Value;

            int num;
            bool isNum = int.TryParse(sum, out num);

            if (studID.Equals("") || cause.Equals("") || !isNum)
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "INSERT INTO DirectorInstructions " +
                                       "(StudentID, Summ, Cause, DateIssued, StatusID) " +
                                    "VALUES (" + studID +
                                    "," + sum +
                                    ",'" + cause +
                                    "','" + date.Date.Year + "-" + date.Date.Month + "-" +
                                    date.Date.Day +
                                    "', 1)";
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);

                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number); // 1
                setDirectorInstrsList();
                MessageBox.Show("Задание добавлено!", " Внимание!", MessageBoxButtons.OK);                
            }
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tasksTab_Click(object sender, EventArgs e)
        {

        }        

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void executeStats(string sqlExpression)
        {
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);

            var result = command.ExecuteReader();

            if (result.HasRows)
            {
                DataTable table = new DataTable("highSchol");
                if (table != null)
                {
                    table.Clear();
                    table.Load(result);
                    mainGrid.DataSource = table;
                }
            }
            else
            {
                DataTable table = new DataTable("highSchol");
                if (table != null)
                {
                    table.Clear();
                    mainGrid.DataSource = table;
                }
            }
            result.Close();
        }

        private void highScholButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT DISTINCT Groups.Naming FROM Groups " +
                "JOIN Students ON Students.GroupID = Groups.GroupID " +
                "JOIN ScholarshipOrders ON Students.ScholarshipOrderID = ScholarshipOrders.ScholOrderID " +
                "WHERE ScholarshipOrders.Naming = 'повышенная'; ";
            executeStats(sqlExpression);
        }

        private void only45Button_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT * FROM Groups WHERE EXISTS (SELECT* FROM Students " +
                "JOIN Grades ON Students.StudentID = Grades.StudentID " +
                "WHERE Groups.GroupID = Students.GroupID AND(Grades.Valuee = 5 OR Grades.Valuee = 4)) " +
                "INTERSECT " +
                "SELECT* FROM Groups WHERE NOT EXISTS(SELECT* FROM Students " +
                "JOIN Grades ON Students.StudentID = Grades.StudentID " +
                "WHERE Groups.GroupID = Students.GroupID AND (Grades.Valuee = 3  OR Grades.Valuee = 2))";
            executeStats(sqlExpression);
        }

        private void wantCGButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT * FROM Groups WHERE EXISTS ( " +
                "SELECT* FROM Students, RequestsGroupChange " +
                "WHERE Students.StudentID = RequestsGroupChange.StudentID " +
                "AND Students.GroupID = Groups.GroupID); ";
            executeStats(sqlExpression);
        }

        private void more2FreeButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT DISTINCT Groups.Naming FROM Groups " +
                "JOIN Schedule ON Groups.GroupID = Schedule.GroupID " +
                "GROUP BY(Groups.Naming) " +
                "HAVING COUNT(DISTINCT Schedule.DayOfWeekID) < 5; ";
            executeStats(sqlExpression);
        }

        private void highestNumDropButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT Groups.Naming FROM Groups " +
                "JOIN Students ON Students.GroupID = Groups.GroupID " +
                "JOIN RequestsDropOut ON RequestsDropOut.StudentID = Students.StudentID " +
                "GROUP BY Groups.Naming " +
                "HAVING COUNT(RequestsDropOut.StudentID) = ( " +
                "SELECT MAX(number) FROM(SELECT COUNT(RequestsDropOut.StudentID) " +
                "AS number, Groups.Naming FROM Groups " +
                "JOIN Students ON Students.GroupID = Groups.GroupID " +
                "JOIN RequestsDropOut ON RequestsDropOut.StudentID = Students.StudentID " +
                " GROUP BY Groups.Naming) AS A); ";
            executeStats(sqlExpression);
        }

        private void mathPhysButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT Groups.Naming FROM Groups" +
                "    JOIN Schedule ON Groups.GroupID = Schedule.GroupID" +
                "        JOIN Disciplines ON Schedule.DisciplineID = Disciplines.DisciplineID" +
                "            WHERE Disciplines.Naming = 'Математика'" +
                "            GROUP BY(Groups.Naming)" +
                "                HAVING COUNT(Disciplines.Naming) > 1 " +
                "UNION " +
                "SELECT Groups.Naming FROM Groups " +
                "    JOIN Schedule ON Groups.GroupID = Schedule.GroupID " +
                "        JOIN Disciplines ON Schedule.DisciplineID = Disciplines.DisciplineID" +
                "            WHERE Disciplines.Naming = 'Физика'" +
                "            GROUP BY(Groups.Naming)" +
                "                HAVING COUNT(Disciplines.Naming) > 1";
            executeStats(sqlExpression);
        }

        private void studsScholsButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT * FROM Students" +
                " LEFT JOIN ScholarshipOrders ON " +
                "Students.ScholarshipOrderID = ScholarshipOrders.ScholOrderID";
            executeStats(sqlExpression);
        }

        private void threeButton_Click(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT FIO, Birthday, Groups.Naming AS GroupNum, " +
                "Disciplines.Naming AS SubjectN, Valuee AS Grade " +
                "FROM Students " +
                "JOIN Groups ON Students.GroupID = Groups.GroupID " +
                "JOIN Grades ON Students.StudentID = Grades.StudentID " +
                "JOIN Disciplines ON Grades.DisciplineID = Disciplines.DisciplineID " +
                "WHERE Grades.Valuee = 3 " +
                "EXCEPT " +
                "SELECT FIO, Birthday, Groups.Naming AS GroupNum, " +
                "Disciplines.Naming AS SubjectN, Valuee AS Grade " +
                "FROM Students " +
                "JOIN Groups ON Students.GroupID = Groups.GroupID " +
                "JOIN Grades ON Students.StudentID = Grades.StudentID " +
                "JOIN Disciplines ON Grades.DisciplineID = Disciplines.DisciplineID " +
                "WHERE Disciplines.Naming = 'Математика'";
            executeStats(sqlExpression);
        }

        private void averageGradeButton_Click(object sender, EventArgs e)
        {

        }
    }
}
