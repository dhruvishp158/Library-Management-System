using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech_Library.Validation;

namespace Project_4
{
    public partial class OrderForm : Form
    {
        HiTechDBEntities1 dbEntities = new HiTechDBEntities1();
        public OrderForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Validate
            Order ord = new Order();
            OrderLine ol = new OrderLine();

            string input = "";
            input = textBoxOrderNumber.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("Order Number must be a number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxOrderNumber.Clear();
                textBoxOrderNumber.Focus();
                return;

            }

            input = textBoxNumberOfBooks.Text.Trim();
            if (!(Validator.IsValidId(input)))
            {
                MessageBox.Show("Number Of Books must be a number", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNumberOfBooks.Clear();
                textBoxNumberOfBooks.Focus();
                return;
            }

            //Save
            String CustomerCombo = comboBoxCustomer.SelectedItem.ToString();
            String BookCombo = comboBoxBook.SelectedItem.ToString();
           
            Customer cust = new Customer();
            Book bk = new Book();
            
            cust.CustomerId = (from cnt in dbEntities.Customers where cnt.Name == CustomerCombo select cnt).First<Customer>().CustomerId;
            bk.ISBN = (from ct in dbEntities.Books where ct.Title == BookCombo select ct).First<Book>().ISBN;

            

            ord.OrderNumber = Convert.ToInt32(textBoxOrderNumber.Text);
            ord.Quantity= Convert.ToInt32(textBoxNumberOfBooks.Text);
            ord.CustomerId = Convert.ToInt32(cust.CustomerId);
            ord.EmployeeId = Convert.ToInt32(comboBoxEmployee.SelectedItem.ToString());
            ol.Date = Convert.ToDateTime(textBoxDate.Text);
            ol.ISBN = bk.ISBN;
            ol.OrderNumber = Convert.ToInt32(textBoxOrderNumber.Text);

            dbEntities.Orders.Add(ord);
            dbEntities.OrderLines.Add(ol);
            int count = dbEntities.SaveChanges();
            if (count > 0)
            {
                MessageBox.Show("Inserted Successfully");
            }
            else
            {
                MessageBox.Show("Error Inserting Data");
            }
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            //Load all the information in Combobox when Form Load
            var listCustomer = (from cst in dbEntities.Customers
                               select cst).ToList<Customer>();

            foreach (Customer item in listCustomer)
            {
                comboBoxCustomer.Items.Add(item.Name.ToString());
            }
            var listEmployee = (from cst in dbEntities.HiTechEmployees
                                where cst.JobTitle== "Order Clerk"
                                select cst).ToList<HiTechEmployee>();

            foreach (HiTechEmployee item in listEmployee)
            {
                comboBoxEmployee.Items.Add(item.EmployeeId.ToString());
            }

            var listBook = (from bk in dbEntities.Books
                                select bk).ToList<Book>();

            foreach (Book item in listBook)
            {
                comboBoxBook.Items.Add( item.Title.ToString());
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //List

            var orderInfo = from cust in dbEntities.Customers
                             join order in dbEntities.Orders on cust.CustomerId equals order.CustomerId
                             join orderl in dbEntities.OrderLines on order.OrderNumber equals orderl.OrderNumber
                             join bks in dbEntities.Books on orderl.ISBN equals bks.ISBN
                             select new
                             {
                                 Name=cust.Name,
                                 OrderNumber=order.OrderNumber,
                                 Quantity=order.Quantity,
                                 Date=orderl.Date,
                                 Title=bks.Title,
                                 EmployeeId=order.EmployeeId
                             };

            
            

            listViewOrder.Items.Clear();
            foreach (var item in orderInfo)
            {
                
                ListViewItem _item = new ListViewItem(item.OrderNumber.ToString());
                _item.SubItems.Add(item.Name);
                _item.SubItems.Add(item.Quantity.ToString());
                _item.SubItems.Add(item.Date.ToString());
                _item.SubItems.Add(item.Title.ToString());
                _item.SubItems.Add(item.EmployeeId.ToString());

                listViewOrder.Items.Add(_item);
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search
            int searchId = Convert.ToInt32(textBoxOrderNumber.Text);
            Order ol = new Order();
            ol = dbEntities.Orders.Where(x => x.OrderNumber == searchId).FirstOrDefault<Order>();

            listViewOrder.Items.Clear();
            if ((ol != null))
            {
                Customer cust = new Customer();
                Book bk = new Book();
                OrderLine orl = new OrderLine();
                orl.Date = (from tbl in dbEntities.OrderLines where tbl.OrderNumber == ol.OrderNumber select tbl).First<OrderLine>().Date;
                orl.ISBN= (from tl in dbEntities.OrderLines where tl.OrderNumber == ol.OrderNumber select tl).First<OrderLine>().ISBN;
                cust.Name = (from cnt in dbEntities.Customers where cnt.CustomerId == ol.CustomerId select cnt).First<Customer>().Name;
                bk.Title = (from tt in dbEntities.Books where tt.ISBN == orl.ISBN select tt).First<Book>().Title;



                textBoxOrderNumber.Text = ol.OrderNumber.ToString();
                textBoxDate.Text = orl.Date.ToString();
                textBoxNumberOfBooks.Text = ol.Quantity.ToString();
                comboBoxEmployee.Text = ol.EmployeeId.ToString();
                comboBoxBook.Text = bk.Title;
                comboBoxCustomer.Text = cust.Name;
            }
            else
            {
                MessageBox.Show("Order Number not found.!");
                textBoxOrderNumber.Clear();
                textBoxOrderNumber.Focus();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Update
            int searchId = Convert.ToInt32(textBoxOrderNumber.Text);
            Order ol = new Order();
            OrderLine orl = new OrderLine();

            ol = dbEntities.Orders.Where(x => x.OrderNumber == searchId).FirstOrDefault<Order>();
            orl = dbEntities.OrderLines.Where(x => x.OrderNumber == searchId).FirstOrDefault<OrderLine>();

            if ((ol != null))
            {
                Customer cust = new Customer();
                Book bk = new Book();
                String bokCombo = comboBoxBook.SelectedItem.ToString();
                String catCombo = comboBoxCustomer.SelectedItem.ToString();

                cust.CustomerId = (from cnt in dbEntities.Customers where cnt.Name == catCombo select cnt).First<Customer>().CustomerId;
                //orl.ISBN= (from a in dbEntities.Books where a.Title == bokCombo select a).First<Book>().ISBN;
                //MessageBox.Show(orl.ISBN);
                //orl.ISBN = orl.ISBN.ToString();
                comboBoxBook.DropDownStyle = ComboBoxStyle.DropDownList;
                ol.OrderNumber =Convert.ToInt32( textBoxOrderNumber.Text);
                ol.EmployeeId = Convert.ToInt32(comboBoxEmployee.SelectedItem.ToString());
                ol.Quantity = Convert.ToInt32(textBoxNumberOfBooks.Text);
                ol.CustomerId =Convert.ToInt32(cust.CustomerId);
                orl.Date = Convert.ToDateTime(textBoxDate.Text.ToString());
                dbEntities.SaveChanges();
                MessageBox.Show("Updated Successfully");
            }
            else
            {
                MessageBox.Show("Order Number nor found.!");
                textBoxOrderNumber.Clear();
                textBoxOrderNumber.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Delete
            int searchId = Convert.ToInt32(textBoxOrderNumber.Text);
            Order ol = new Order();
            OrderLine orl = new OrderLine();

            ol = dbEntities.Orders.Where(x => x.OrderNumber == searchId).FirstOrDefault<Order>();
            orl = dbEntities.OrderLines.Where(x => x.OrderNumber == searchId).FirstOrDefault<OrderLine>();

            if ((ol != null))
            {
                dbEntities.Orders.Remove(ol);
                dbEntities.OrderLines.Remove(orl);
                dbEntities.SaveChanges();
                MessageBox.Show("Deleted Successfully");
                textBoxDate.Clear();
                textBoxNumberOfBooks.Clear();
                textBoxOrderNumber.Clear();
            }
            else
            {
                MessageBox.Show("ISBN nor found.!");
                textBoxOrderNumber.Clear();
                textBoxOrderNumber.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string clr = "";
            clr = MessageBox.Show("Do You Want To Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
            if (clr == "Yes" || clr == "yes")
            {
                this.Close();
            }
        }
    }
}
