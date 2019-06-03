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
    public partial class Students : Form
    {
        int selectstudentId;
        public Students()
        {
            InitializeComponent();
            FormRefresh();
        }
        public void FormRefresh()
        {
            using (var db = new AcademyDB())
            {
                fillgrid(db.Students.ToList());
            }
        }
        public void fillgrid(List<Student> list)
        {
            dataGridView1.Rows.Clear();
            using (var db = new AcademyDB())
            {
                foreach (Student st in list)
                {
                    if (st.Deleted == false && st.GroupID != null)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = st.id.ToString();
                        row.Cells[1].Value = st.Name;
                        row.Cells[2].Value = st.Surname;
                        row.Cells[3].Value = st.Email;
                        row.Cells[4].Value = st.Phone;
                        row.Cells[5].Value = st.Fee;
                        row.Cells[6].Value = st.Group.Name;
                        row.Cells[7].Value = st.StartDate;

                        dataGridView1.Rows.Add(row);
                    }
                }
                comboBox1.DataSource = db.Groups.ToList();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "id";

                
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Student st = new Student();
            Group grp = comboBox1.SelectedItem as Group;
            

            st.Name = textBox1.Text;
            st.Surname = textBox2.Text;          
            st.Email = textBox3.Text;
            st.Phone = textBox4.Text;
            st.Fee = Convert.ToDouble(textBox5.Text);
            st.GroupID = grp.id;
            st.StartDate = dateTimePicker1.Value;
            st.Deleted = false;


            using (var db = new AcademyDB())
            {
                db.Students.Add(st);
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Student added");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Student didn't add.Fill all inputs!");
                }
            }

        }

        //update
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int IndexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[IndexRow];
            if(row.Cells[0].Value!=null){
                selectstudentId = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                textBox5.Text = row.Cells[5].Value.ToString();
                comboBox1.SelectedValue = row.Cells[6].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[7].Value);
            }
                 
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {           
          
            using (var db = new AcademyDB())
            {
                Student student = db.Students.Where(std => std.id == selectstudentId).FirstOrDefault();
                Student st = new Student();
                Group grp = comboBox1.SelectedItem as Group;       

                st.Name = textBox1.Text;
                st.Surname = textBox2.Text;
                st.Email = textBox3.Text;
                st.Phone = textBox4.Text;
                st.Fee = Convert.ToDouble(textBox5.Text);
                st.GroupID= grp.id;
                st.StartDate = dateTimePicker1.Value;
               


                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Student updated");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Student updated!");
                }

            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Student st = db.Students.FirstOrDefault(em => em.id == selectstudentId);
                //
                st.Deleted = true;
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

        private void Button1_Click(object sender, EventArgs e)
        {
            string searchtxt = textBox9.Text.Trim();
            using (var db = new AcademyDB())
            {
                List<Student> stdList = db.Students.Where(std =>
                                                std.Name.Contains(searchtxt) ||
                                                std.Surname.Contains(searchtxt) ||
                                                std.Email.Contains(searchtxt) ||
                                                std.Phone.Contains(searchtxt)||
                                                std.Group.Name.Contains(searchtxt)
                                                ).ToList();
                fillgrid(stdList);
            }
        }

        private void EmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }

        private void StudentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Students s = new Students();
            s.ShowDialog();
        }

        private void GroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grp g = new Grp();
            g.ShowDialog();
        }

        private void TasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskForm t = new TaskForm();
            t.ShowDialog();
        }

        private void ClassroomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassroomForm crf = new ClassroomForm();
            crf.ShowDialog();
        }

        private void EducationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Education edu = new Education();
            edu.ShowDialog();
        }
    }
}
