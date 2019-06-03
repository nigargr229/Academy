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
    public partial class Grp : Form
    {
        int selectgrpId;
        public Grp()
        {
            InitializeComponent();
            FormRefresh();
        }
        public void FormRefresh()
        {
            using (var db = new AcademyDB())
            {
                fillgrid(db.Groups.ToList());
            }
        }
        public void fillgrid(List<Group> list)
        {
            dataGridView1.Rows.Clear();
            using (var db = new AcademyDB())
            {
                foreach (Group grp in list)
                {
                    if (grp.Deleted == false)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = grp.id.ToString();
                        row.Cells[1].Value = grp.Name;
                        row.Cells[2].Value = grp.Classroom.Name;
                        row.Cells[3].Value = db.Employees.FirstOrDefault(em=> em.id == grp.TeacherID).Name;
                        row.Cells[4].Value = db.Employees.FirstOrDefault(em => em.id == grp.MentorID).Name;
                        row.Cells[5].Value = grp.Capacity;
                        row.Cells[6].Value = grp.Program.Name;                     

                        dataGridView1.Rows.Add(row);
                    }
                }
                comboBox1.DataSource = db.Classrooms.ToList();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "id";

                comboBox2.DataSource = db.Employees.Where(em =>em.SpecialityID != null).ToList();
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "id";

                comboBox3.DataSource = db.Employees.Where(em => em.Position.Name=="Mentor").ToList();
                comboBox3.DisplayMember = "Name";
                comboBox3.ValueMember = "id";

                comboBox4.DataSource = db.Programs.ToList();
                comboBox4.DisplayMember = "Name";
                comboBox4.ValueMember = "id";
            }

        }

        private void Btnsave_Click(object sender, EventArgs e)
        {
            Group grp = new Group();
            Model.Classroom cls  = comboBox1.SelectedItem as Model.Classroom;
            Employee empl1  = comboBox2.SelectedItem as Employee;
            Employee empl2  = comboBox3.SelectedItem as Employee;
            Model.Program pr  = comboBox4.SelectedItem as Model.Program;

            grp.Name = textBox1.Text;
            grp.ClassroomID = cls.id;
            grp.TeacherID = empl1.id;
            grp.MentorID = empl2.id;
            grp.Capacity = Convert.ToInt32(textBox2.Text);
            grp.ProgramID = pr.id;
           
            grp.Deleted = false;
            using (var db = new AcademyDB())
            {
                db.Groups.Add(grp);
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Group added");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Group didn't add.Fill all inputs!");
                }
            }
        }
        //updatingrow
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int IndexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[IndexRow];
            if (e.RowIndex > -1)
            {
                selectgrpId = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = row.Cells[1].Value.ToString();
                comboBox1.SelectedValue = row.Cells[2].Value.ToString();
                comboBox2.SelectedValue = row.Cells[3].Value.ToString();
                comboBox3.SelectedValue = row.Cells[4].Value.ToString();
                textBox2.Text = row.Cells[5].Value.ToString();
                comboBox4.SelectedValue = row.Cells[6].Value.ToString();              
            }
        }

        private void Btnupdate_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Group group = db.Groups.Where(std => std.id == selectgrpId).FirstOrDefault();
                Group st = new Group();
                Model.Classroom cls = comboBox1.SelectedItem as Model.Classroom;
                Employee empl1 = comboBox2.SelectedItem as Employee;
                Employee empl2 = comboBox3.SelectedItem as Employee;
                Model.Program pr = comboBox4.SelectedItem as Model.Program;

                group.Name = textBox1.Text;
                group.ClassroomID = cls.id;
                group.TeacherID = empl1.id;
                group.MentorID = empl2.id;
                group.Capacity = Convert.ToInt32(textBox2.Text);
                group.ProgramID = pr.id;

         
                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Group updated");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Group didn't update!");
                }

            }
        }

        private void Btndelete_Click(object sender, EventArgs e)
        {
            using (var db = new AcademyDB())
            {
                Group grp = db.Groups.FirstOrDefault(em => em.id == selectgrpId);
                //
                grp.Deleted = true;
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

        private void Btnsrc_Click(object sender, EventArgs e)
        {
            string searchtxt = textBox8.Text.Trim();
            using (var db = new AcademyDB())
            {
                List<Group> grpList = db.Groups.Where(grp =>
                                                grp.Name.Contains(searchtxt) ||
                                                grp.Classroom.Name.Contains(searchtxt)).ToList();
                foreach(Employee emp in db.Employees.ToList())
                {
                    Group grp = db.Groups.FirstOrDefault(g => g.TeacherID == emp.id || g.MentorID == emp.id);
                    if(grp != null &&emp.Name.Contains(searchtxt) && !grpList.Contains(grp))
                    {
                        grpList.Add(grp);
                    }
                }
                fillgrid(grpList);
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

        private void TasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskForm t = new TaskForm();
            t.ShowDialog();
        }

        private void GroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grp g = new Grp();
            g.ShowDialog();
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
