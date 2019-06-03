using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp8.Model;

namespace WindowsFormsApp8
{
    public partial class ClassroomForm : Form
    {
        int selectroomId;
        public ClassroomForm()
        {
            InitializeComponent();
            FormRefresh();
        }
        public void FormRefresh()
        {
            using (var db = new AcademyDB())
            {
                fillgrid(db.Classrooms.ToList());
            }
        }
        public void fillgrid(List<Classroom> list)
        {
            dataGridView1.Rows.Clear();
            using (var db = new AcademyDB())
            {
                foreach (Classroom classroom in list)
                {
                    if (classroom.Deleted == false)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = classroom.id.ToString();
                        row.Cells[1].Value = classroom.Name;
                        row.Cells[2].Value = classroom.Capacity;

                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Classroom classroom = new Classroom();
            classroom.Name = textBox1.Text;
            classroom.Capacity = Convert.ToInt32 (textBox2.Text);
            using (var db = new AcademyDB())
            {
                db.Classrooms.Add(classroom);
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Classroom added");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Classroom didn't add.Fill all inputs!");
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int IndexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[IndexRow];

            selectroomId = Convert.ToInt32(row.Cells[0].Value);

            textBox1.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[2].Value.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Classroom classroom = db.Classrooms.Where(r => r.id == selectroomId).FirstOrDefault();

                classroom.Name = textBox1.Text;
                classroom.Capacity = Convert.ToInt32(textBox2.Text);

                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Classroom updated");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Classroom didn't update!");
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Classroom classroom = db.Classrooms.FirstOrDefault(cr => cr.id == selectroomId);
                //
                classroom.Deleted = true;
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Deleted");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Error!");
                }
            }
        }
    }
}
