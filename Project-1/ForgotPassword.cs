using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech_Library.Business;
using HiTech_Library.Validation;

namespace Project_1
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string ext = "";
            ext = MessageBox.Show("Do You Want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();

            if (ext == "Yes" || ext == "yes")
            {
                this.Close();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text.Trim();
            string resetPassword = textBoxNewPassword.Text.Trim();

           //Validate the ID and Reset Password
            if (!Validator.IsValidId(userName, 5))
            {
                MessageBox.Show("User ID must be 5-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxUserName.Clear();
                textBoxUserName.Focus();
                return;

            }
            int tempID = Convert.ToInt32(userName);
            HiTechUser use = new HiTechUser();
            if (!(use.IsUniqueUserId(tempID)))
            {
                use.UserId = tempID;
                use.Password = textBoxPassword.Text.Trim();
                use.newPassword(use);
                MessageBox.Show("Password updated");
            }
            else
            {
                MessageBox.Show("User ID does not exist", "Inavalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxUserName.Clear();
                textBoxUserName.Focus();
                textBoxPassword.Clear();
            }

        }
    }
}
