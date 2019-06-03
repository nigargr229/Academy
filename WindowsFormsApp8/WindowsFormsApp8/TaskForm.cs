using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp8.Model;

namespace WindowsFormsApp8
{
    public partial class TaskForm : Form
    {
        int selectedtaskId;
        public TaskForm()
        {
            InitializeComponent();
            FormRefresh();
        }
        public void FormRefresh()
        {
            using (var db = new AcademyDB())
            {
                fillgrid(db.Tasks.ToList());
            }
        }
        public void fillgrid(List<Task> list)
        {
            dataGridView1.Rows.Clear();
            using (var db = new AcademyDB())
            {
                foreach (Task task in list)
                {
                    if (task.Deleted == false )
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = task.id.ToString();
                        row.Cells[1].Value = task.Name;
                        row.Cells[2].Value = task.Dedline;
                        row.Cells[3].Value = task.Group.Name;


                        dataGridView1.Rows.Add(row);
                    }
                }
                comboBox1.DataSource = db.Groups.ToList();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "id";
            }
        }


        //Adding new task
        private void Button1_Click(object sender, EventArgs e)
        {
            Task task = new Task();
            Group group = comboBox1.SelectedItem as Group;

            task.Name = textBox1.Text;
            task.Dedline = dateTimePicker1.Value;
            task.GroupID = group.id;

            task.Deleted = false;
            using (var db = new AcademyDB())
            {
                db.Tasks.Add(task);
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Task added");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Task didn't add.Fill all inputs!");
                }
            }
        }
        //updatingrowtaskcodes
      

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int IndexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[IndexRow];

            selectedtaskId = Convert.ToInt32(row.Cells[0].Value);

            textBox1.Text = row.Cells[1].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row.Cells[2].Value);       
            comboBox1.SelectedValue = row.Cells[3].Value.ToString();
       
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Task task = db.Tasks.Where(t => t.id == selectedtaskId).FirstOrDefault();
                Group group = comboBox1.SelectedItem as Group;
             

                task.Name = textBox1.Text;
                task.Dedline = dateTimePicker1.Value;
                task.GroupID = group.id;
               
           


                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Task updated");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Task didn't update!");
                }

            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Task task = (Task)db.Tasks.Where(t => t.id == selectedtaskId).FirstOrDefault();

                task.Deleted = true;
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

        private void Button4_Click(object sender, EventArgs e)
        {
            string searchtxt = textBox4.Text.Trim();
            using (var db = new AcademyDB())
            {
                List<Task> taskList = db.Tasks.Where(t =>t.Name.Contains(searchtxt)).ToList();
                foreach (Task task in db.Tasks.ToList())
                {
                    Group grp = db.Groups.FirstOrDefault(g => g.id == task.id);
                    if (grp != null && grp.Name.Contains(searchtxt))
                    {
                        taskList.Add(task);
                    }
                }
                fillgrid(taskList);
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

        private void GroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grp g = new Grp();
            g.ShowDialog();
        }

        private void TasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskForm t = new TaskForm();
            t.ShowDialog();
        }

        private void ClassroomsToolStripMenuItem_Click(object sender, EventArgs e)
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
