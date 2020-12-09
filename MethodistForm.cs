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
    public partial class MethodistForm : Form
    {
        private string userID;
        private string userFIO;
        private OleDbConnection cn;

        private OleDbDataAdapter da;

        public MethodistForm()
        {
            InitializeComponent();
        }

        public MethodistForm(string userID, string userFIO, OleDbConnection cn)
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

            //groups2Box.ValueMember = "GroupID";
            //groups2Box.DisplayMember = "Naming";
            //groups2Box.DataSource = dataSet.Tables[tableName];

            group3Box.ValueMember = "GroupID";
            group3Box.DisplayMember = "Naming";
            group3Box.DataSource = dataSet.Tables[tableName];

            newGroupBox.ValueMember = "GroupID";
            newGroupBox.DisplayMember = "Naming";
            newGroupBox.DataSource = dataSet.Tables[tableName];

            genChooseGroupBox.ValueMember = "GroupID";
            genChooseGroupBox.DisplayMember = "Naming";
            genChooseGroupBox.DataSource = dataSet.Tables[tableName];

            //query = "SELECT * FROM Statuses";
            //tableName = "Statuses";
            //executeQuery(query, tableName);
            //reqStatusBox.ValueMember = "StatusID";
            //reqStatusBox.DisplayMember = "Naming";
            //reqStatusBox.DataSource = dataSet.Tables[tableName];

            query = "SELECT * FROM DirectorInstructions";
            tableName = "DirectorInstructions";
            executeQuery(query, tableName);
            tasksBox.ValueMember = "InstructionID";
            tasksBox.DisplayMember = "InstructionID";
            tasksBox.DataSource = dataSet.Tables[tableName];

            query = "SELECT * FROM Dayss";
            tableName = "Dayss";
            executeQuery(query, tableName);
            dayWeekBox.ValueMember = "DayID";
            dayWeekBox.DisplayMember = "DayNaming";
            dayWeekBox.DataSource = dataSet.Tables[tableName];

            query = "SELECT * FROM Disciplines";
            tableName = "Disciplines";
            executeQuery(query, tableName);
            disciplineBox.ValueMember = "DisciplineID";
            disciplineBox.DisplayMember = "Naming";
            disciplineBox.DataSource = dataSet.Tables[tableName];

            query = "SELECT * FROM ClassesTypes";
            tableName = "ClassesTypes";
            executeQuery(query, tableName);
            classTypeBox.ValueMember = "TypeID";
            classTypeBox.DisplayMember = "Naming";
            classTypeBox.DataSource = dataSet.Tables[tableName];

            setMethsComboboxes();

            setDirectorInstrsList();

            query = "SELECT FIO, Summ AS Сумма, Cause AS Причина, DateIssued AS Дата_заявлено, " +
                "DateAcceptedOrRejected AS Дата_результат, Naming AS Статус " +
                "FROM RequestsScholarshipM " +
                "JOIN Statuses ON RequestsScholarshipM.StatusID = Statuses.StatusID " +
                "JOIN Students ON RequestsScholarshipM.StudentID = Students.StudentID " +
                "WHERE MethodistID = " + userID;
            tableName = "ScholReqs";
            executeQuery(query, tableName);
            scholReqList.DataSource = dataSet.Tables[tableName];
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
            
            studentsReqMBox.ValueMember = "StudentID";
            studentsReqMBox.DisplayMember = "FIO";
            studentsReqMBox.DataSource = dataSet.Tables[tableName];

            studentGroupBox.ValueMember = "StudentID";
            studentGroupBox.DisplayMember = "FIO";
            studentGroupBox.DataSource = dataSet.Tables[tableName];

            //students2Box.ValueMember = "StudentID";
            //students2Box.DisplayMember = "FIO";
            //students2Box.DataSource = dataSet.Tables[tableName];
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

        private void setDirectorInstrsList()
        {
            string query = "SELECT InstructionID, MethodistID, FIO, Summ, Cause, DateIssued, DateCompleted, Naming " +
                "FROM DirectorInstructions " +
                "JOIN Students ON DirectorInstructions.StudentID = Students.StudentID " +
                "JOIN Statuses ON DirectorInstructions.StatusID = Statuses.StatusID";
            string tableName = "DirectorInstrs";
            executeQuery(query, tableName);
            tasksList.DataSource = dataSet.Tables[tableName];
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
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

        private void highScholButton_Click_1(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT DISTINCT Groups.Naming FROM Groups " +
                "JOIN Students ON Students.GroupID = Groups.GroupID " +
                "JOIN ScholarshipOrders ON Students.ScholarshipOrderID = ScholarshipOrders.ScholOrderID " +
                "WHERE ScholarshipOrders.Naming = 'повышенная'; ";
            executeStats(sqlExpression);
        }

        private void only45Button_Click_1(object sender, EventArgs e)
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

        private void highestNumDropButton_Click_1(object sender, EventArgs e)
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

        private void mathPhysButton_Click_1(object sender, EventArgs e)
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

        private void studsScholsButton_Click_1(object sender, EventArgs e)
        {
            string sqlExpression = "SELECT * FROM Students" +
                " LEFT JOIN ScholarshipOrders ON " +
                "Students.ScholarshipOrderID = ScholarshipOrders.ScholOrderID";
            executeStats(sqlExpression);
        }

        private void threeButton_Click_1(object sender, EventArgs e)
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

        private void sendReqBut_Click(object sender, EventArgs e)
        {
            string studID = studentScholBox.SelectedValue.ToString();
            string str = sum2Box.Text;
            int num;
            bool isNum = int.TryParse(str, out num);
            bool isNum2 = int.TryParse(studID, out num);

            if (isNum && isNum2)
            {
                string sqlExpression = "sp_loadReqFromMeth";
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                OleDbParameter methID = new OleDbParameter
                {
                    ParameterName = "@MethodistID",
                    Value = int.Parse(userID)
                };
                Console.WriteLine(methID.Value);
                OleDbParameter studentID = new OleDbParameter
                {
                    ParameterName = "@StudentID",
                    Value = int.Parse(studID)
                };
                Console.WriteLine(studentID.Value);
                OleDbParameter sumParam = new OleDbParameter
                {
                    ParameterName = "@Sum",
                    Value = num
                };
                Console.WriteLine(sumParam.Value);
                OleDbParameter causeParam = new OleDbParameter
                {
                    ParameterName = "@Cause",
                    Value = causeBox.Text
                };
                Console.WriteLine(causeParam.Value);
                OleDbParameter dateParam = new OleDbParameter
                {
                    ParameterName = "@DateIssued",
                    Value = dateReqBox.Value.Date
                };
                Console.WriteLine(dateParam.Value);
                command.Parameters.Add(methID);
                command.Parameters.Add(studentID);
                command.Parameters.Add(sumParam);
                command.Parameters.Add(causeParam);
                command.Parameters.Add(dateParam);

                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    DataTable table = dataSet.Tables["ScholReqs"];
                    if (table != null)
                    {
                        table.Clear();
                        table.Load(result);
                    }
                    scholReqList.DataSource = table;
                }
                result.Close();
            }
            else
            {
                MessageBox.Show("Введите сумму корректно!", " Внимание!", MessageBoxButtons.OK);
            }
        }

        private void group3Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = group3Box.SelectedValue.ToString();

            string query = "SELECT FIO, Birthday FROM Students WHERE GroupID = " + group;
            string tableName = "StudentsList";
            executeQuery(query, tableName);

            groupList.DataSource = dataSet.Tables[tableName];
        }

        private void groupChangeButton_Click(object sender, EventArgs e)
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

        private void commitTaskButton_Click(object sender, EventArgs e)
        {
            string methID = methodistsBox.SelectedValue.ToString();
            string taskID = tasksBox.SelectedValue.ToString();
            DateTime date = dateTaskBox.Value;

            if (methID.Equals("") || taskID.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "UPDATE DirectorInstructions SET MethodistID = " + methID +
                                ", DateCompleted = '" +
                                date.Date.Year + "-" + date.Date.Month + "-" + date.Date.Day +
                                "', StatusID = 2" +
                                " WHERE InstructionID = " + taskID;

                OleDbCommand command = new OleDbCommand(sqlExpression, cn);

                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number); // 1
                setDirectorInstrsList();
                MessageBox.Show("Задание выполнено!", " Внимание!", MessageBoxButtons.OK);
            }
        }

        private void genChooseGroupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = genChooseGroupBox.SelectedValue.ToString();

            string query = "SELECT FIO, Birthday FROM Students WHERE GroupID = " + group;
            string tableName = "StudentsList";
            executeQuery(query, tableName);

            groupList.DataSource = dataSet.Tables[tableName];


            query = "SELECT DayNaming AS День, Disciplines.Naming AS Предмет, TimeLine AS Время," +
                "Room AS Ауд, ClassesTypes.Naming AS Тип FROM Schedule " +
                "JOIN Dayss ON Schedule.DayOfWeekID = Dayss.DayID " +
                "JOIN Disciplines ON Schedule.DisciplineID = Disciplines.DisciplineID " +
                "JOIN ClassesTypes ON Schedule.TypeID = ClassesTypes.TypeID " +
                "WHERE Schedule.GroupID = " + group;
            tableName = "ScheduleList";
            executeQuery(query, tableName);

            scheduleList.DataSource = dataSet.Tables[tableName];
        }

        private void addScheduleButton_Click(object sender, EventArgs e)
        {
            string groupID = genChooseGroupBox.SelectedValue.ToString();
            string dayWeekID = dayWeekBox.SelectedValue.ToString();
            string discID = disciplineBox.SelectedValue.ToString();
            string typeID = classTypeBox.SelectedValue.ToString();
            string time = textTimeBox.Text;
            string room = roomBox.Text;

            int num;
            bool isNum = int.TryParse(room, out num);

            if (!isNum || dayWeekID.Equals("") || discID.Equals("") || typeID.Equals("") ||
                time.Equals("") || room.Equals("") || groupID.Equals(""))
            {
                MessageBox.Show("Одно из полей не задано!", " Внимание!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string sqlExpression = "INSERT INTO Schedule (DayOfWeekID, DisciplineID, TimeLine, " +
                                "GroupID, Room, TypeID) " +
                                "VALUES (" + dayWeekID + "," + discID + ",'" + time + "'," +
                                groupID + "," + room + "," + typeID + ")";

                OleDbCommand command = new OleDbCommand(sqlExpression, cn);

                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number); // 1                
                MessageBox.Show("Занятие добавлено!", " Внимание!", MessageBoxButtons.OK);
            }
        }
    }
}
