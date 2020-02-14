using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using HiTech_Library.Business;
using HiTech_Library.DataAccess;
using HiTech_Library.Validation;
using System.Text.RegularExpressions;

namespace Project_1
{
    public partial class employees : Form
    {
        public employees()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //Validation

            string input = "";
            input = textBoxEmpId.Text.Trim();
            if (!Validator.IsValidId(input, 5))
            {
                MessageBox.Show("Employee ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }

            HiTechEmployee emp = new HiTechEmployee();
            int tempId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            if (!(emp.IsUniqueEmpId(tempId)))
            {
                MessageBox.Show("Employee ID already exists.", "Duplicate EmployeeID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;
            }

            input = textBoxFirstName.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("First name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFirstName.Clear();
                textBoxFirstName.Focus();
                return;
            }

            input = textBoxLastName.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Last name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLastName.Clear();
                textBoxLastName.Focus();
                return;
            }

            input = textBoxJobTitle.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Job Title must  contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxJobTitle.Clear();
                textBoxJobTitle.Focus();
                return;
            }



            //valid data
            emp.EmployeeId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            emp.FirstName = textBoxFirstName.Text.Trim();
            emp.LastName = textBoxLastName.Text.Trim();
            emp.JobTitle = textBoxJobTitle.Text.Trim();

            emp.SaveEmployee(emp);
            MessageBox.Show("Employee record has been saved successfully.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {

            int selectedIndex = comboBoxOption.SelectedIndex;
            List<HiTechEmployee> listEmployee = new List<HiTechEmployee>(); 
            switch (selectedIndex)
            {
                case 0: //Search employee by EmployeeId
                    HiTechEmployee emp = new HiTechEmployee();
                    emp = emp.searchemployee(Convert.ToInt32(textBoxInput.Text));
                    if (emp != null)
                    {
                        textBoxEmpId.Text = emp.EmployeeId.ToString();
                        textBoxFirstName.Text = emp.FirstName;
                        textBoxLastName.Text = emp.LastName;
                        textBoxJobTitle.Text = emp.JobTitle;
                    }
                    else
                    {
                        MessageBox.Show("Employee record not found!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 1: //Search employee by First Name
                    HiTechEmployee anEmp = new HiTechEmployee();
                    listEmployee = anEmp.SearchEmployee(1, textBoxInput.Text.Trim());
                    listViewEmployee.Items.Clear();
                    if (listEmployee.Count != 0)
                    {
                        foreach (HiTechEmployee emp1 in listEmployee)
                        {
                            ListViewItem item = new ListViewItem(emp1.EmployeeId.ToString());
                            item.SubItems.Add(emp1.FirstName);
                            item.SubItems.Add(emp1.LastName);
                            item.SubItems.Add(emp1.JobTitle);
                            listViewEmployee.Items.Add(item);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee list is empty" + "\n" + "Please enter Employee Data", "No Employee Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;
                case 2: //Search employee by LastName
                    HiTechEmployee anEmp2 = new HiTechEmployee();
                    listEmployee = anEmp2.SearchEmployee(2, textBoxInput.Text.Trim());
                    listViewEmployee.Items.Clear();
                    if (listEmployee.Count != 0)
                    {
                        foreach (HiTechEmployee emp1 in listEmployee)
                        {
                            ListViewItem item = new ListViewItem(emp1.EmployeeId.ToString());
                            item.SubItems.Add(emp1.FirstName);
                            item.SubItems.Add(emp1.LastName);
                            item.SubItems.Add(emp1.JobTitle);
                            listViewEmployee.Items.Add(item);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee list is empty" + "\n" + "Please enter Employee Data", "No Employee Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;
                default:
                    break;
            }
        }

        private void buttonListAll_Click(object sender, EventArgs e)
        {
            //List DATA
            HiTechEmployee emp = new HiTechEmployee();
            List<HiTechEmployee> listEmp = new List<HiTechEmployee>();
            listEmp = emp.ListEmployee();
            listViewEmployee.Items.Clear(); 
            if (listEmp.Count != 0)
            {
                foreach (HiTechEmployee anEmp in listEmp)
                {
                    ListViewItem item = new ListViewItem(anEmp.EmployeeId.ToString());
                    item.SubItems.Add(anEmp.FirstName);
                    item.SubItems.Add(anEmp.LastName);
                    item.SubItems.Add(anEmp.JobTitle);
                    listViewEmployee.Items.Add(item);


                }
            }
            else
            {
                MessageBox.Show("Employee list is empty" + "\n" + "Please enter Employee Data", "No Employee Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //Update
            String input = "";
            input = textBoxEmpId.Text.Trim();
            if (!Validator.IsValidId(input, 5))
            {
                MessageBox.Show("Employee ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }
            int tempID = Convert.ToInt32(input);
            HiTechEmployee emp = new HiTechEmployee();
            if (!(emp.IsUniqueEmpId(tempID)))
            {
                emp.EmployeeId = tempID;
                emp.FirstName = textBoxFirstName.Text.Trim();
                emp.LastName = textBoxLastName.Text.Trim();
                emp.JobTitle = textBoxJobTitle.Text.Trim();
                emp.updateEmployee(emp);
                MessageBox.Show(tempID + " ,This ID is updated");
            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //Delete
            string input = "";
            input = textBoxEmpId.Text.Trim();
            if (!Validator.IsValidId(input, 5))
            {
                MessageBox.Show("Employee ID must be 4-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }
            HiTechEmployee emp = new HiTechEmployee();
            int tempId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            if (!(emp.IsUniqueEmpId(tempId)))
            {
                MessageBox.Show("Employee ID exists.", "Found EmployeeID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                emp.EmployeeId = Convert.ToInt32(textBoxEmpId.Text.Trim());

                emp.DeleteEmployee(emp);
                MessageBox.Show("Deleted");
                return;
            }
            else
            {
                MessageBox.Show(tempId + "This ID doesnot Exists...");
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
            }

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            string ext = "";
            ext = MessageBox.Show("Do You Want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();

            if (ext == "Yes" || ext == "yes")
            {
                this.Close();
            }
        }

        private void employees_Load(object sender, EventArgs e)
        {
            HiTechEmployee emp = new HiTechEmployee();
            List<HiTechEmployee> listEmp = new List<HiTechEmployee>();
            listEmp = emp.ListEmployee();
            listViewEmployee.Items.Clear();
            if (listEmp.Count != 0)
            {
                foreach (HiTechEmployee anEmp in listEmp)
                {
                    comboBox2.Items.Add(anEmp.FirstName.ToString());
                    //ListViewItem item = new ListViewItem(anEmp.EmployeeId.ToString());
                    //item.SubItems.Add(anEmp.FirstName);
                    //item.SubItems.Add(anEmp.LastName);
                    //item.SubItems.Add(anEmp.JobTitle);
                    //listViewEmployee.Items.Add(item);


                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
