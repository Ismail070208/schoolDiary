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
    
    public partial class StudentsForm : Form
    {
        

        public StudentsForm()
        {
            InitializeComponent();
            LoadClasses();
            LoadStudents();
        }

        private void LoadClasses()
        {
            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "SELECT Id, ClassName \r\nFROM Classes \r\nORDER BY Grade, Section";
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

            listBoxStudents.Items.Clear();

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT Students.Id, FirstName, LastName, Classes.ClassName
                FROM Students
                JOIN Classes ON Students.ClassId = Classes.Id
                WHERE ClassId = @classId
                ORDER BY Students.Id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@classId", (int)comboClass.SelectedValue);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listBoxStudents.Items.Add(
    new StudentItem
    {
        Id = (int)reader["Id"],
        Text = $"{reader["FirstName"]} {reader["LastName"]} ({reader["ClassName"]})"
    }
);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на ученици:\n" + ex.Message);
            }

        }


        private void StudentsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Моля, въведи име и фамилия!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO Students (FirstName, LastName, ClassId)
                                   VALUES (@fn, @ln, @classId)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@fn", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@ln", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@classId", comboClass.SelectedValue);

                    cmd.ExecuteNonQuery();  
                }

                MessageBox.Show("Ученикът е добавен успешно!");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при добавяне на ученик:\n" + ex.Message);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxStudents.SelectedItem == null)
            {
                MessageBox.Show("Моля, избери ученик!");
                return;
            }

            StudentItem selected = listBoxStudents.SelectedItem as StudentItem;

            if (selected == null)
            {
                MessageBox.Show("Грешка: няма ID!");
                return;
            }

            int studentId = selected.Id;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "DELETE FROM Students WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", studentId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Ученикът е изтрит!");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при изтриване:\n" + ex.Message);
            }

        }

        private void comboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }
    }
}
