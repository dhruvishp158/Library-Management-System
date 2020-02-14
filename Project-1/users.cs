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
    public partial class users : Form
    {
        public users()
        {
            InitializeComponent();
        }

        private void users_Load(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            //Validate
            string input = "";
            input = textBoxEmpId.Text.Trim();
            if (!Validator.IsValidId(input, 5))
            {
                MessageBox.Show("User ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }

            HiTechUser use = new HiTechUser();
            int tempId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            if (!(use.IsUniqueUserId(tempId)))
            {
                MessageBox.Show("User ID already exists.", "Duplicate UserID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Job Title must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxJobTitle.Clear();
                textBoxJobTitle.Focus();
                return;
            }

            //valid data
           use.UserId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            use.FirstName = textBoxFirstName.Text.Trim();
            use.LastName = textBoxLastName.Text.Trim();
            use.JobTiTle = textBoxJobTitle.Text.Trim();
            use.Password = txtUserPwd.Text.Trim();
            use.SaveUser(use);
            MessageBox.Show("User record has been saved successfully.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxOption.SelectedIndex;
            List<HiTechUser> listUser = new List<HiTechUser>();
            switch (selectedIndex)
            {
                case 0: 
                    HiTechUser use = new HiTechUser();
                    use = use.searchUser(Convert.ToInt32(textBoxInput.Text));
                    if (use != null)
                    {
                        textBoxEmpId.Text = use.UserId.ToString();
                        textBoxFirstName.Text = use.FirstName;
                        textBoxLastName.Text = use.LastName;
                        textBoxJobTitle.Text = use.JobTiTle;
                        txtUserPwd.Text = use.Password;

                    }
                    else
                    {
                        MessageBox.Show("User record not found!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 1: //Search employee by First Name
                    HiTechUser anUse = new HiTechUser();
                    listUser = anUse.SearchUser(1, textBoxInput.Text.Trim());
                    listViewEmployee.Items.Clear();
                    if (listUser.Count != 0)
                    {
                        foreach (HiTechUser use1 in listUser)
                        {
                            ListViewItem item = new ListViewItem(use1.UserId.ToString());
                            item.SubItems.Add(use1.FirstName);
                            item.SubItems.Add(use1.LastName);
                            item.SubItems.Add(use1.JobTiTle);
                            listViewEmployee.Items.Add(item);

                        }
                    }
                    else
                    {
                        MessageBox.Show("User list is empty" + "\n" + "Please enter User Data", "No User Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;
                case 2: //Search employee by LastName
                    HiTechUser anUsr2 = new HiTechUser();
                    listUser = anUsr2.SearchUser(2, textBoxInput.Text.Trim());
                    listViewEmployee.Items.Clear();
                    if (listUser.Count != 0)
                    {
                        foreach (HiTechUser use1 in listUser)
                        {
                            ListViewItem item = new ListViewItem(use1.UserId.ToString());
                            item.SubItems.Add(use1.FirstName);
                            item.SubItems.Add(use1.LastName);
                            item.SubItems.Add(use1.JobTiTle);
                            item.SubItems.Add(use1.Password);
                            listViewEmployee.Items.Add(item);

                        }
                    }
                    else
                    {
                        MessageBox.Show("User list is empty" + "\n" + "Please enter User Data", "No User Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;
                default:
                    break;
            }
        }

        private void buttonListAll_Click(object sender, EventArgs e)
        {
            //List
            HiTechUser use = new HiTechUser();
            List<HiTechUser> listUse = new List<HiTechUser>();
            listUse = use.ListUsers();
            listViewEmployee.Items.Clear();
            if (listUse.Count != 0)
            {
                foreach (HiTechUser anUse in listUse)
                {
                    ListViewItem item = new ListViewItem(anUse.UserId.ToString());
                    item.SubItems.Add(anUse.FirstName);
                    item.SubItems.Add(anUse.LastName);
                    item.SubItems.Add(anUse.JobTiTle);
                    item.SubItems.Add(anUse.Password);
                    listViewEmployee.Items.Add(item);

                }
            }
            else
            {
                MessageBox.Show("User list is empty" + "\n" + "Please enter User Data", "No User Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //Update
            String input = "";
            input = textBoxEmpId.Text.Trim();
            if (!Validator.IsValidId(input, 5))
            {
                MessageBox.Show("User ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }
            int tempID = Convert.ToInt32(input);
            HiTechUser use = new HiTechUser();
            if (!(use.IsUniqueUserId(tempID)))
            {
                use.UserId = tempID;
                use.FirstName = textBoxFirstName.Text.Trim();
                use.LastName = textBoxLastName.Text.Trim();
                use.JobTiTle = textBoxJobTitle.Text.Trim();
                use.Password = txtUserPwd.Text.Trim();
                use.updateUser(use);
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
                MessageBox.Show("User ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmpId.Clear();
                textBoxEmpId.Focus();
                return;

            }
            HiTechUser use = new HiTechUser();
            int tempId = Convert.ToInt32(textBoxEmpId.Text.Trim());
            if (!(use.IsUniqueUserId(tempId)))
            {
                MessageBox.Show("User ID exists.", "Found EmployeeID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                use.UserId = Convert.ToInt32(textBoxEmpId.Text.Trim());

                use.DeleteUser(use);
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

            if(ext=="Yes" || ext=="yes")
            {
                this.Close();
            }
        }

        private void textBoxEmpId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
