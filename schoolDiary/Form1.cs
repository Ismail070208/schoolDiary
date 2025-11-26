using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace schoolDiary
{
    public partial class Form1 : Form
    {
        public static SchoolData1 Data = new SchoolData1();

        public Form1()
        {
            InitializeComponent();

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при връзката с базата: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new StudentsForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new TeachersForm().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AttendanceForm().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new GradesForm().ShowDialog();
        }
    }
}
