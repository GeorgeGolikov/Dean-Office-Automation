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
    public partial class StudentForm : Form
    {
        private string userID;
        private string userFIO;
        private OleDbConnection cn;

        private OleDbDataAdapter da;

        public StudentForm()
        {
            InitializeComponent();
        }

        public StudentForm(string userID, string userFIO, OleDbConnection cn)
        {
            InitializeComponent();

            this.userID = userID;
            this.userFIO = userFIO;
            this.cn = cn;

            this.Text += " " + userFIO;

            string query = "SELECT Naming, Summ, DateStarted, OrderName, ScholOrderID FROM " +
                           "ScholarshipOrders JOIN Students " +
                           "ON ScholarshipOrders.ScholOrderID = Students.ScholarshipOrderID " +
                           "WHERE StudentID = " + userID;
            string tableName = "ScholarshipOrders";
            executeQuery(query, tableName);

            genStudFioTextBox.Text = userFIO;
            object naming = getOneValueFromDataSet(tableName, "Naming");
            if (naming != null)
            {
                genStudScholTextBox.Text = (string)naming;
            }
            object summ = getOneValueFromDataSet(tableName, "Summ");
            if (summ != null)
            {
                genStudScholTextBox.Text = summ.ToString();
            }            
            object date = getOneValueFromDataSet(tableName, "DateStarted");
            if (date != null)
            {
                genStudScholDateTimePicker.Value = (DateTime)date;
            }
            else
            {
                genStudScholDateTimePicker.Value = new DateTime(1970, 1, 1);
            }
            object orderName = getOneValueFromDataSet(tableName, "OrderName");
            if (orderName != null)
            {
                genStudScholOrderTextBox.Text = (string)orderName;
            }            

            query = "SELECT * FROM Groups";
            tableName = "Groups";
            executeQuery(query, tableName);
            genChooseGroupBox.ValueMember = "GroupID";
            genChooseGroupBox.DisplayMember = "Naming";
            genChooseGroupBox.DataSource = dataSet.Tables[tableName];
            statChooseGroupBox.ValueMember = "GroupID";
            statChooseGroupBox.DisplayMember = "Naming";
            statChooseGroupBox.DataSource = dataSet.Tables[tableName];
            newGroupBox.ValueMember = "GroupID";
            newGroupBox.DisplayMember = "Naming";
            newGroupBox.DataSource = dataSet.Tables[tableName];


            query = "SELECT Naming AS Предмет, Valuee AS Оценка FROM Grades " +
                    "JOIN Disciplines ON Grades.GradeID = Disciplines.DisciplineID " +
                    "WHERE Grades.StudentID = " + userID;
            tableName = "Exams";
            executeQuery(query, tableName);
            gradesList.DataSource = dataSet.Tables[tableName];

            query = "select avg(Valuee) AS Average from Grades where StudentID = " + userID;
            tableName = "AverageGrade";
            executeQuery(query, tableName);
            DataRow[] row = dataSet.Tables[tableName].Select();
            if (row.Length != 0)
            {
                averageGradeBox.Text = row[0]["Average"].ToString();
            }            

            query = "SELECT Groups.Naming FROM Groups JOIN Schedule ON Groups.GroupID = Schedule.GroupID " +
                    "JOIN Disciplines ON Schedule.DisciplineID = Disciplines.DisciplineID " +
                    "WHERE Disciplines.Naming = 'Математика' " +
                    "GROUP BY(Groups.Naming) " +
                    "HAVING COUNT(Disciplines.Naming) > 1 " +
                    "UNION " +
                    "SELECT Groups.Naming FROM Groups " +
                    "JOIN Schedule ON Groups.GroupID = Schedule.GroupID " +
                    "JOIN Disciplines ON Schedule.DisciplineID = Disciplines.DisciplineID " +
                    "WHERE Disciplines.Naming = 'Физика' " +
                    "GROUP BY(Groups.Naming) " +
                    "HAVING COUNT(Disciplines.Naming) > 1";
            tableName = "GroupsMore";
            executeQuery(query, tableName);
            groupsMoreList.DataSource = dataSet.Tables[tableName];

            query = "SELECT Summ AS Сумма, Cause AS Причина, DateIssued AS Дата_заявлено, " +
                "DateAcceptedOrRejected AS Дата_результат, Naming AS Статус " +
                "FROM RequestsScholarship " +
                "JOIN Statuses ON RequestsScholarship.StatusID = Statuses.StatusID " +
                "WHERE StudentID = " + userID;
            tableName = "ScholReqs";
            executeQuery(query, tableName);
            scholReqList.DataSource = dataSet.Tables[tableName];

            query = "SELECT Groups.Naming AS Группа, Cause AS Причина, DateIssued AS Дата_заявлено,	" +
                    "DateAcceptedOrRejected AS Дата_результат, Statuses.Naming AS Статус " +
                    "FROM RequestsGroupChange JOIN Groups ON RequestsGroupChange.OldGroupID = Groups.GroupID " +
                    "JOIN Statuses ON RequestsGroupChange.StatusID = Statuses.StatusID " +
                    "WHERE RequestsGroupChange.StudentID = " + userID;
            tableName = "GroupChanges";
            executeQuery(query, tableName);
            groupChangeList.DataSource = dataSet.Tables[tableName];

            query = "SELECT Cause AS Причина, DateIssued AS Дата_заявлено, " +
                    "DateAcceptedOrRejected AS Дата_результат, Statuses.Naming AS Статус " +
                    "FROM RequestsDropOut JOIN Statuses ON RequestsDropOut.StatusID = Statuses.StatusID " +
                    "WHERE RequestsDropOut.StudentID = " + userID;
            tableName = "DropOutChanges";
            executeQuery(query, tableName);
            dropOutList.DataSource = dataSet.Tables[tableName];
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

        private void statChooseGroupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = statChooseGroupBox.SelectedValue.ToString();

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

        private void sendButton_Click(object sender, EventArgs e)
        {
            string str = sumBox.Text;
            int num;
            bool isNum = int.TryParse(str, out num);

            if (isNum)
            {
                string sqlExpression = "sp_loadReqFromStudent";
                OleDbCommand command = new OleDbCommand(sqlExpression, cn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                OleDbParameter idParam = new OleDbParameter
                {
                    ParameterName = "@StudentID",
                    Value = int.Parse(userID)
                };
                Console.WriteLine(idParam.Value);
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
                    Value = dateBox.Value.Date
                };
                Console.WriteLine(dateParam.Value);
                command.Parameters.Add(idParam);
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

        private void send2Button_Click(object sender, EventArgs e)
        {
            int newGroupIDD = int.Parse(newGroupBox.SelectedValue.ToString());
            string cause = cause2Box.Text;
            DateTime date = date2Box.Value;

            int oldGroupIDD;
            string query = "SELECT GroupID FROM Students WHERE StudentID = " + userID;
            executeQuery(query, "OldGroup");
            DataRow[] row = dataSet.Tables["OldGroup"].Select();
            if (row.Length != 0)
            {
                oldGroupIDD = int.Parse(row[0]["GroupID"].ToString());
            }
            else
            {
                MessageBox.Show("Ошибка с номером старой группы!", " Внимание!", MessageBoxButtons.OK);
                return;
            }

            string sqlExpression = "sp_loadReqCGFromStudent";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OleDbParameter idParam = new OleDbParameter
            {
                ParameterName = "@StudentID",
                Value = int.Parse(userID)
            };
            Console.WriteLine(idParam.Value);
            OleDbParameter oldGroupID = new OleDbParameter
            {
                ParameterName = "@OldGroupID",
                Value = oldGroupIDD
            };
            Console.WriteLine(oldGroupID.Value);
            OleDbParameter newGroupID = new OleDbParameter
            {
                ParameterName = "@NewGroupID",
                Value = newGroupIDD
            };
            Console.WriteLine(newGroupID.Value);
            OleDbParameter causeParam = new OleDbParameter
            {
                ParameterName = "@Cause",
                Value = cause
            };
            Console.WriteLine(causeParam.Value);
            OleDbParameter dateParam = new OleDbParameter
            {
                ParameterName = "@DateIssued",
                Value = date.Date
            };
            Console.WriteLine(dateParam.Value);
            command.Parameters.Add(idParam);
            command.Parameters.Add(oldGroupID);
            command.Parameters.Add(newGroupID);
            command.Parameters.Add(causeParam);
            command.Parameters.Add(dateParam);

            var result = command.ExecuteReader();

            if (result.HasRows)
            {
                DataTable table = dataSet.Tables["GroupChanges"];
                if (table != null)
                {
                    table.Clear();
                    table.Load(result);
                }
                groupChangeList.DataSource = table;
            }
            result.Close();
        }

        private void send3Button_Click(object sender, EventArgs e)
        {
            string cause = cause3Box.Text;
            DateTime date = date3Box.Value;

            string sqlExpression = "sp_loadReqDropFromStudent";
            OleDbCommand command = new OleDbCommand(sqlExpression, cn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OleDbParameter idParam = new OleDbParameter
            {
                ParameterName = "@StudentID",
                Value = int.Parse(userID)
            };
            Console.WriteLine(idParam.Value);           
            OleDbParameter causeParam = new OleDbParameter
            {
                ParameterName = "@Cause",
                Value = cause
            };
            Console.WriteLine(causeParam.Value);
            OleDbParameter dateParam = new OleDbParameter
            {
                ParameterName = "@DateIssued",
                Value = date.Date
            };
            Console.WriteLine(dateParam.Value);
            command.Parameters.Add(idParam);
            command.Parameters.Add(causeParam);
            command.Parameters.Add(dateParam);

            var result = command.ExecuteReader();

            if (result.HasRows)
            {
                DataTable table = dataSet.Tables["DropOutChanges"];
                if (table != null)
                {
                    table.Clear();
                    table.Load(result);
                }
                dropOutList.DataSource = table;
            }
            result.Close();
        }

        private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        private void dataGrid1_Navigate_1(object sender, NavigateEventArgs ne)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }     

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void statisticsTab_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }        

        private void date2Box_ValueChanged(object sender, EventArgs e)
        {

        }

        private void newGroupBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cause2Box_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void scheduleList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
