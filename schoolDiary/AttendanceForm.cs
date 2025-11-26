using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace schoolDiary
{
   
    public partial class AttendanceForm : Form
    {
        public AttendanceForm()
        {
            InitializeComponent();
            LoadClasses();
            LoadTeachers();
        }

        private void LoadClasses()
        {
            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT Id, ClassName FROM Classes ORDER BY Grade, Section";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboClass.DataSource = dt;
                    comboClass.DisplayMember = "ClassName";
                    comboClass.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на класовете:\n" + ex.Message);
            }
        }

        private void LoadStudents()
        {
            if (comboClass.SelectedValue == null || comboClass.SelectedValue is DataRowView)
                return;

            comboStudent.DataSource = null;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT Id, FirstName + ' ' + LastName AS FullName
                        FROM Students
                        WHERE ClassId = @classId
                        ORDER BY LastName, FirstName";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@classId", comboClass.SelectedValue);

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboStudent.DataSource = dt;
                    comboStudent.DisplayMember = "FullName";
                    comboStudent.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на ученици:\n" + ex.Message);
            }

            LoadAttendance(); // обновяваме ListView
        }

        private void LoadTeachers()
        {
            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT Id, Name FROM Teachers ORDER BY Name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboTeacher.DataSource = dt;
                    comboTeacher.DisplayMember = "Name";
                    comboTeacher.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на учители:\n" + ex.Message);
            }
        }

        private void LoadAttendance() // за оправяне
        {
            listViewAttendance.Items.Clear();

            if (comboStudent.SelectedValue == null || comboStudent.SelectedValue is DataRowView)
                return;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT a.Id, a.Date, t.Name AS TeacherName, a.IsPresent, a.Note
                        FROM Attendance a
                        JOIN Teachers t ON a.TeacherId = t.Id
                        WHERE a.StudentId = @sid
                        ORDER BY a.Date";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sid", comboStudent.SelectedValue);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(Convert.ToDateTime(reader["Date"]).ToShortDateString());
                        item.SubItems.Add(reader["TeacherName"].ToString());
                        item.SubItems.Add((bool)reader["IsPresent"] ? "Присъствал" : "Отсъствал");
                        item.SubItems.Add(reader["Note"].ToString());
                        item.Tag = reader["Id"];
                        listViewAttendance.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на присъствия:\n" + ex.Message);
            }
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboStudent.SelectedValue == null || comboTeacher.SelectedValue == null)
            {
                MessageBox.Show("Моля, избери ученик и учител!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO Attendance (StudentId, TeacherId, Date, IsPresent, Note)
                        VALUES (@sid, @tid, @date, @present, @note)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sid", comboStudent.SelectedValue);
                    cmd.Parameters.AddWithValue("@tid", comboTeacher.SelectedValue);
                    cmd.Parameters.AddWithValue("@date", datePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@present", chkPresent.Checked);
                    cmd.Parameters.AddWithValue("@note", txtNote.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Присъствието е записано!");
                LoadAttendance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при запис:\n" + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewAttendance.SelectedItems.Count == 0)
            {
                MessageBox.Show("Моля, избери запис за изтриване!");
                return;
            }

            int attendanceId = (int)listViewAttendance.SelectedItems[0].Tag;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "DELETE FROM Attendance WHERE Id=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", attendanceId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Записът е изтрит!");
                LoadAttendance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при изтриване:\n" + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewAttendance.SelectedItems.Count == 0)
            {
                MessageBox.Show("Моля, избери запис за редакция!");
                return;
            }

            int attendanceId = (int)listViewAttendance.SelectedItems[0].Tag;
            ListViewItem item = listViewAttendance.SelectedItems[0];

            datePicker.Value = DateTime.Parse(item.SubItems[0].Text);
            chkPresent.Checked = item.SubItems[2].Text == "Присъствал";
            txtNote.Text = item.SubItems[3].Text;

            btnSave.Click -= btnSave_Click;
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    using (SqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();

                        string sql = @"
                            UPDATE Attendance
                            SET Date=@date, IsPresent=@present, Note=@note
                            WHERE Id=@id";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@date", datePicker.Value.Date);
                        cmd.Parameters.AddWithValue("@present", chkPresent.Checked);
                        cmd.Parameters.AddWithValue("@note", txtNote.Text);
                        cmd.Parameters.AddWithValue("@id", attendanceId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Записът е актуализиран!");
                    LoadAttendance();

                    
                    btnSave.Click += btnSave_Click;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Грешка при редакция:\n" + ex.Message);
                }
            };
        }

        private void comboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void comboStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAttendance();
        }
    }
}
