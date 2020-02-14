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
    public partial class AfterLogIn : Form
    {
        public AfterLogIn()
        {
            InitializeComponent();
        }
        //Navigate to EMployee
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            employees emp = new employees();
            emp.ShowDialog();
        }
        //Navigate to user
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            users use = new users();
            use.ShowDialog();
        }

        private void AfterLogIn_Load(object sender, EventArgs e)
        {
            HiTechUser use = new HiTechUser();
            
            lblAfterLogIn.Text = "Welcome Manager";
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
