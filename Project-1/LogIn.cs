using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech_Library.Business;
using HiTech_Library.DataAccess;
using HiTech_Library.Validation;
using Project_2;
using Project_4;
using Project_3;
namespace Project_1
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AboutBoxHiTech box = new AboutBoxHiTech();
            box.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ext = "";
            ext = MessageBox.Show("Do You Want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();

            if (ext == "Yes" || ext == "yes")
            {
                this.Close();
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //Open the form as per the LogIn Information
            int id1 = Convert.ToInt32(txtLoginID.Text.Trim());
            int index = comboBoxOption.SelectedIndex;
            string pwd1 = txtPwd.Text;
            bool flag = false;
            HiTechUser use3 = new HiTechUser();
            string jobIndex = "";
            switch (index)
            {
                case 0:
                    jobIndex = "MIS Manager";
                    break;
                case 1:
                    jobIndex = "Sales Manager";
                    break;
                case 2:
                    jobIndex = "Inventory Controller";
                    break;
                case 3:
                    jobIndex = "Order Clerk";
                    break;
                default:
                    MessageBox.Show("You have to select job title first");
                    break;
            }
            
            
            flag=use3.ForLogIn(id1,pwd1,flag,jobIndex);
            if (flag == true && index==0)
            {
                MessageBox.Show("Log In Successful");
                AfterLogIn es = new AfterLogIn();
                es.ShowDialog();
            }
            else if (flag == true && index == 1)
            {
                MessageBox.Show("Log In Successful");
                Project_2.Customer cs = new Project_2.Customer();
                cs.ShowDialog();

                
            }
            else if (flag == true && index == 2)
            {
                MessageBox.Show("Log In Successful");
                BookForm bk = new BookForm();
                bk.ShowDialog();
            }
            else if (flag == true && index == 3)
            {
                MessageBox.Show("Log In Successful");
                OrderForm of = new OrderForm();
                of.ShowDialog();
            }
            else
            {
                MessageBox.Show("User Id or Password Invalid","Warning!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtLoginID.Clear();
                txtLoginID.Focus();
            }

            //if (id=="1111")
            //{
            //    MessageBox.Show("Log In Successful");

            //    AfterLogIn es = new AfterLogIn();
            //    es.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("No");
            //    txtLoginID.Clear();
            //    txtLoginID.Focus();
            //}


        }

        private void txtForgetPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword fpwd = new ForgotPassword();
            fpwd.ShowDialog();

        }
    }
}
