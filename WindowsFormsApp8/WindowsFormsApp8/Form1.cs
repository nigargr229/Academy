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
    public partial class Form1 : Form
    {
        int selectedemployerId;
        public Form1()
        {
            InitializeComponent();
            FormRefresh();
        }

        public void FormRefresh()
        {
            using(var db = new AcademyDB())
            {
               fillgrid(db.Employees.ToList());
            }
        }

        public void fillgrid(List<Employee> list)
        {
            dataGridView1.Rows.Clear();
            using (var db= new AcademyDB() )
            {
                double? maxSalary = 0;
                foreach(Employee emp in list)
                {
                    if (emp.Deleted == false)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = emp.id.ToString();
                        row.Cells[1].Value = emp.Name;
                        row.Cells[2].Value = emp.Surname;
                        row.Cells[3].Value = emp.Email;
                        row.Cells[4].Value = emp.Phone;

                        row.Cells[5].Value = emp.Program.Name;
                        row.Cells[6].Value = emp.StartTime;
                        row.Cells[7].Value = emp.Salary;
                        row.Cells[8].Value = emp.Position.Name;
                        dataGridView1.Rows.Add(row);
                    }
                    if (emp.Salary > maxSalary)
                    {
                        maxSalary = emp.Salary;
                    }
                }
                comboBox1.DataSource = db.Positions.ToList();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "id";

                comboBox2.DataSource = db.Programs.ToList();
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "id";

                numericUpDown2.Value = Convert.ToDecimal(maxSalary);
            }
        }
        //adding

        private void Btnadd_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            Position posid = comboBox1.SelectedItem as Position;
            Model.Program program = comboBox2.SelectedItem as Model.Program;   

            employee.Name = textBox1.Text;
            employee.Surname = textBox2.Text;
            employee.SpecialityID = program.id;
            employee.Email = textBox4.Text;
            employee.Phone = textBox5.Text;
            employee.StartTime = dateTimePicker1.Value;
            employee.Salary = Convert.ToDouble(textBox6.Text);
            employee.PositionID = posid.id;
            employee.Deleted = false;
            using(var db = new AcademyDB())
            {
                db.Employees.Add(employee);
                if(db.SaveChanges() != 0)
                {
                    MessageBox.Show("Employee added");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Employee didn't add.Fill all inputs!");
                }
            }
            
        }

        

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int IndexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[IndexRow];

            selectedemployerId =Convert.ToInt32( row.Cells[0].Value);

            textBox1.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();
            comboBox2.SelectedValue = row.Cells[5].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row.Cells[6].Value);
            textBox6.Text = row.Cells[7].Value.ToString();
            comboBox1.SelectedValue = row.Cells[8].Value.ToString();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
             using(var db = new AcademyDB())
            {
              Employee employee=  db.Employees.Where(emp => emp.id == selectedemployerId).FirstOrDefault();
                Model.Program program = comboBox2.SelectedItem as Model.Program;
                Position position = comboBox1.SelectedItem as Position;

                employee.Name = textBox1.Text;
                employee.Surname = textBox2.Text;
                employee.Email = textBox4.Text;
                employee.Phone = textBox5.Text;
                employee.SpecialityID = program.id;
                employee.StartTime = dateTimePicker1.Value;
                employee.Salary = Convert.ToDouble( textBox6.Text);
                employee.PositionID = position.id;


                if (db.SaveChanges() != 0)
                {
                    MessageBox.Show("Employee updated");
                    FormRefresh();
                }
                else
                {
                    MessageBox.Show("Employee didn't update!");
                }

            }          
        }

        private void Button2_Click(object sender, EventArgs e)
        {
           using(var db = new AcademyDB())
            {
                Employee emp = (Employee) db.Employees.Where(em => em.id == selectedemployerId).FirstOrDefault();
               
                emp.Deleted = true;
                if(db.SaveChanges() != 0)
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

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            int minSalary = Convert.ToInt32(numericUpDown1.Text);
            int maxSalary = Convert.ToInt32(numericUpDown2.Text);

            string searchtxt = teachsrc.Text.Trim();
            using (var db = new AcademyDB())
            {
                List<Employee> empList = db.Employees.Where(emp => 
                                                (emp.Name.Contains(searchtxt) || 
                                                emp.Surname.Contains(searchtxt) ||
                                                emp.Email.Contains(searchtxt) || 
                                                emp.Phone.Contains(searchtxt)) && 
                                                (emp.Salary >= minSalary && emp.Salary <= maxSalary)).ToList();
                fillgrid(empList);
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

        private void ClassroomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassroomForm crf = new ClassroomForm();
            crf.ShowDialog();
        }

        private void EmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Education edu = new Education();
            edu.ShowDialog();
        }
    }
   
}
