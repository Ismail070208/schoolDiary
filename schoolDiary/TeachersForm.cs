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

    public partial class TeachersForm : Form
    {
        
        

        public TeachersForm()
        {
            InitializeComponent();
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            listBoxTeachers.Items.Clear();

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "SELECT Id, Name, Subject FROM Teachers ORDER BY Id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listBoxTeachers.Items.Add(new Teacher
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Subject = reader["Subject"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане на учители:\n" + ex.Message);
            }
        }

        private void TeachersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherName.Text) ||
            string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Моля, попълни всички полета!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "INSERT INTO Teachers (Name, Subject) VALUES (@name, @subject)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", txtTeacherName.Text);
                    cmd.Parameters.AddWithValue("@subject", txtSubject.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Учителят е добавен успешно!");
                LoadTeachers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при добавяне:\n" + ex.Message);
            }
        }

        private void btnDeleteTeacher_Click(object sender, EventArgs e)
        {
            if (listBoxTeachers.SelectedItem == null)
            {
                MessageBox.Show("Моля, избери учител!");
                return;
            }

            Teacher t = (Teacher)listBoxTeachers.SelectedItem;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = "DELETE FROM Teachers WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", t.Id);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Учителят е изтрит!");
                LoadTeachers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при изтриване:\n" + ex.Message);
            }
        }
    }
    }

