using System;
using System.Data;

using System.Windows.Forms;

using System.Data.SqlClient;
using HiTech_Library.BLL;
using HiTech_Library.DataAccess;
using HiTech_Library.Validation;

namespace Project_2
{
    public partial class Customer : Form
    {
        SqlDataAdapter da;
        DataSet dsHiTechDB;
        DataTable dtCustomers;
        SqlCommandBuilder sqlBuilder;
        Customers cs = new Customers();

        public Customer()
        {
            InitializeComponent();
        }

        private void Customer_Load(object sender, EventArgs e)
        {

            //Create an object of type DataSet and name it as CollegeDS
            dsHiTechDB = new DataSet("HitechDs");

            //Create an object of type DataTable and name it as Students
            // and add the object to the DataSet object
            dtCustomers = new DataTable("Customers");
            dsHiTechDB.Tables.Add(dtCustomers);
            //dsCollegeDB.Tables.Add("Students");

            // Create columns , add columns to the DataTable Students
            dtCustomers.Columns.Add("CustomerId", typeof(int));
            dtCustomers.Columns.Add("Name", typeof(string));
            dtCustomers.Columns.Add("Street", typeof(string));
            dtCustomers.Columns.Add("City", typeof(string));
            dtCustomers.Columns.Add("PostalCode", typeof(string));
            dtCustomers.Columns.Add("CreditLimit", typeof(int));
            dtCustomers.Columns.Add("PhoneNumber", typeof(int));
            dtCustomers.Columns.Add("FaxNumber", typeof(string));


            dtCustomers.PrimaryKey = new DataColumn[] { dtCustomers.Columns["CustomerId"] };
            da = new SqlDataAdapter("SELECT * FROM Customers", UtilityDB.ConnectDB());
            sqlBuilder = new SqlCommandBuilder(da);
        }

        private void buttonListCustomer_Click(object sender, EventArgs e)
        {
            Customers cs = new Customers();
            dataGridViewCustomer.DataSource = cs.ListCustomer();
        }

        private void buttonListCustomerDS_Click(object sender, EventArgs e)
        {
            da.Fill(dsHiTechDB.Tables["Customers"]);
            dataGridViewCudtomerDS.DataSource = dsHiTechDB.Tables["Customers"];
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Validate
            string input = "";
            input = textBoxCustomerId.Text.Trim();
            if (!Validator.IsValidId(input, 4))
            {
                MessageBox.Show("Customer ID must be 4-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCustomerId.Clear();
                textBoxCustomerId.Focus();
                return;

            }
            Customers cust = new Customers();
            int tempId = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            if (!(cust.IsUniqueCustomerId(tempId)))
            {
                MessageBox.Show("Customer ID already exists.", "Duplicate EmployeeID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCustomerId.Clear();
                textBoxCustomerId.Focus();
                return;
            }
            input = textBoxFirstName.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFirstName.Clear();
                textBoxFirstName.Focus();
                return;
            }
            input = textBoxStreet.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Street must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxStreet.Clear();
                textBoxStreet.Focus();
                return;
            }
            input = textBoxCity.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("City must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCity.Clear();
                textBoxCity.Focus();
                return;
            }
            input = textBoxPostalCode.Text.Trim();
            if (!(Validator.IsValidPostalCode(input)))
            {
                MessageBox.Show("Postal code must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPostalCode.Clear();
                textBoxPostalCode.Focus();
                return;
            }
            input = textBoxCreditLimit.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("Credit  number has to be a number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCreditLimit.Clear();
                textBoxCreditLimit.Focus();
                return;

            }
            input = textBoxPhoneNumber.Text.Trim();
            if (!Validator.IsValidId(input, 10))
            {
                MessageBox.Show("Phone Number must be 10-digit number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPhoneNumber.Clear();
                textBoxPhoneNumber.Focus();
                return;
       
            }        
           
            input = textBoxFaxNumber.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Fax Number must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFaxNumber.Clear();
                textBoxFaxNumber.Focus();
                return;
            }
            

            
            //===========================SAVE DATA=========================
            DataRow drCurrent = dtCustomers.NewRow();
            drCurrent["CustomerId"] = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            drCurrent["Name"] = textBoxFirstName.Text.Trim();
            drCurrent["Street"] = textBoxStreet.Text.Trim();
            drCurrent["City"] = textBoxCity.Text.Trim();
            drCurrent["PostalCode"] = textBoxPostalCode.Text.Trim();
            drCurrent["PhoneNumber"] = Convert.ToInt32(textBoxPhoneNumber.Text.Trim());
            drCurrent["CreditLimit"] = Convert.ToInt32(textBoxCreditLimit.Text.Trim());
            drCurrent["FaxNumber"] = textBoxFaxNumber.Text.Trim();
            dtCustomers.Rows.Add(drCurrent);
            MessageBox.Show(drCurrent.RowState.ToString());
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            int searchId = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            DataRow drStudent = dtCustomers.Rows.Find(searchId);
            if (drStudent != null)
            {
                textBoxCustomerId.Text = drStudent["CustomerId"].ToString();
                textBoxCreditLimit.Text = drStudent["CreditLimit"].ToString();
                textBoxPhoneNumber.Text = drStudent["PhoneNumber"].ToString();


                textBoxFirstName.Text = drStudent["Name"].ToString();
                textBoxFaxNumber.Text = drStudent["FaxNumber"].ToString();
                textBoxPostalCode.Text = drStudent["PostalCode"].ToString();
                textBoxStreet.Text = drStudent["Street"].ToString();
                textBoxCity.Text = drStudent["City"].ToString();


            }   
            else
            {
                MessageBox.Show("Customer not found!", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            //Update
            int searchId = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            DataRow drCustomers = dtCustomers.Rows.Find(searchId);

            drCustomers["CustomerId"] = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            drCustomers["Name"] = textBoxFirstName.Text.Trim();
            drCustomers["Street"] = textBoxStreet.Text.Trim();
            drCustomers["City"] = textBoxCity.Text.Trim();
            drCustomers["PostalCode"] = textBoxPostalCode.Text.Trim();
            drCustomers["PhoneNumber"] = Convert.ToInt32(textBoxPhoneNumber.Text.Trim());
            drCustomers["CreditLimit"] = Convert.ToInt32(textBoxCreditLimit.Text.Trim());
            drCustomers["FaxNumber"] = textBoxFaxNumber.Text.Trim();
            MessageBox.Show(drCustomers.RowState.ToString());
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //Delete
            int searchId = Convert.ToInt32(textBoxCustomerId.Text.Trim());
            DataRow drCustomer = dtCustomers.Rows.Find(searchId);
            drCustomer.Delete();
            MessageBox.Show(drCustomer.RowState.ToString());
        }

        private void buttonUpdateDB_Click(object sender, EventArgs e)
        {

            //Update Database
            da.Update(dsHiTechDB.Tables["Customers"]);
            MessageBox.Show("Database has been updated successfully", "Confirmation");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            string ext = "";
            ext = MessageBox.Show("Do You Want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();

            if (ext == "Yes" || ext == "yes")
            {
                this.Close();
            }
        }
    }
}
