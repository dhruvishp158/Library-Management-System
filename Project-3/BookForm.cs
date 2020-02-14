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
namespace Project_3
{
    public partial class BookForm : Form
    {
        HiTechDBEntities dbEntities = new HiTechDBEntities();
        public BookForm()
        {
            InitializeComponent();
        }

        private void BookForm_Load(object sender, EventArgs e)
        {
            //Load all the information in Combobox when Form Load
            var listAuthors = (from athr in dbEntities.Authors
                               select athr).ToList<Author>();

            foreach (Author item2 in listAuthors)
            {
                comboBoxAuthor.Items.Add(item2.FirstName.ToString());
            }

            var listPublisher = (from pub in dbEntities.Publishers
                               select pub).ToList<Publisher>();
            
            foreach (Publisher item3 in listPublisher)
            {
                comboBoxPublisher.Items.Add(item3.Name.ToString());
            }

            var listCategory = (from cat in dbEntities.Categories
                                 select cat).ToList<Categories>();

            foreach (Categories item4 in listCategory)
            {
                comboBoxCategory.Items.Add(item4.CategoryName.ToString());
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Validate
            Book bk = new Book();

            string input = "";
            input = textBoxISBN.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("ISBN must be a number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxISBN.Clear();
                textBoxISBN.Focus();
                return;

            }
            
            input = textBoxTitle.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTitle.Clear();
                textBoxTitle.Focus();
                return;
            }
            
            input = textBoxPrice.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("Price must be number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Clear();
                textBoxPrice.Focus();
                return;

            }

            input = textBoxQOH.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("must be a number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxQOH.Clear();
                textBoxQOH.Focus();
                return;

            }

            //Add
            String pubCombo = comboBoxPublisher.SelectedItem.ToString();
            String catCombo = comboBoxCategory.SelectedItem.ToString();
           
            Publisher pub = new Publisher();
            Categories cat = new Categories();
            pub.PublisherId = (from cnt in dbEntities.Publishers where cnt.Name == pubCombo select cnt).First<Publisher>().PublisherId;
            cat.CategoryId = (from ct in dbEntities.Categories where ct.CategoryName == catCombo select ct).First<Categories>().CategoryId;

            MessageBox.Show(pub.PublisherId.ToString()+" "+cat.CategoryId.ToString());
            bk.ISBN = textBoxISBN.Text;
            bk.Title = textBoxTitle.Text;
            bk.UnitPrice = Convert.ToInt32(textBoxPrice.Text);
            bk.YearPlublished = Convert.ToDateTime(textBoxYear.Text);
            bk.QOH = Convert.ToInt32(textBoxQOH.Text);
            bk.PublisherId = Convert.ToInt32(pub.PublisherId);
            bk.CategoryId= Convert.ToInt32(cat.CategoryId);

            dbEntities.Books.Add(bk);
            int count=dbEntities.SaveChanges();
            if(count>0)
            {
                MessageBox.Show("Inserted Successfully");
            }
            else
            {
                MessageBox.Show("Error Inserting Data");
            }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //List
            var listBook = (from ls in dbEntities.Books join ct in
                            dbEntities.Categories on ls.CategoryId equals ct.CategoryId join pu in
                            dbEntities.Publishers on ls.PublisherId equals pu.PublisherId 
                            select new
                            {
                                ISBN = ls.ISBN,
                                Title = ls.Title,
                                UnitPrice = ls.UnitPrice,
                                YearPublished = ls.YearPlublished,
                                QOH = ls.QOH,
                                Name=pu.Name,
                                CategoryName=ct.CategoryName
                                
                            }).Distinct();


            listViewBook.Items.Clear();
            foreach (var item in listBook)
            {
                ListViewItem _item = new ListViewItem(item.ISBN.ToString());
                _item.SubItems.Add(item.Title);
                _item.SubItems.Add(item.UnitPrice.ToString());
                _item.SubItems.Add(item.YearPublished.ToString());
                _item.SubItems.Add(item.QOH.ToString());
                _item.SubItems.Add(item.CategoryName);
                _item.SubItems.Add(item.Name);
                listViewBook.Items.Add(_item);
            }
        }

        private void listViewBook_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Search
            int searchId = Convert.ToInt32(textBoxISBN.Text);
            Book bk = new Book();
            bk = dbEntities.Books.Where(x => x.ISBN == searchId.ToString()).FirstOrDefault<Book>();
            
            listViewBook.Items.Clear();
            if((bk!=null))
            {
                Publisher pub = new Publisher();
                Categories cat = new Categories();
                Author at = new Author();

                pub.Name = (from cnt in dbEntities.Publishers where cnt.PublisherId == bk.PublisherId select cnt).First<Publisher>().Name;
                cat.CategoryName = (from ct in dbEntities.Categories where ct.CategoryId == bk.CategoryId select ct).First<Categories>().CategoryName;
                at.FirstName= (from abcd in dbEntities.Authors from bcde in abcd.Books where bcde.ISBN == bk.ISBN select abcd).First<Author>().FirstName;


                textBoxISBN.Text = bk.ISBN.ToString();
                textBoxPrice.Text = bk.UnitPrice.ToString();
                textBoxQOH.Text = bk.QOH.ToString();
                textBoxYear.Text = bk.YearPlublished.ToString();
                textBoxTitle.Text = bk.Title;
                comboBoxPublisher.Text = pub.Name;
                comboBoxCategory.Text = cat.CategoryName;
                comboBoxAuthor.Text = at.FirstName;
            }
            else
            {
                MessageBox.Show("ID not found.!");
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Update
            int searchId = Convert.ToInt32(textBoxISBN.Text);
            Book bk = new Book();
            bk = dbEntities.Books.Where(x => x.ISBN == searchId.ToString()).FirstOrDefault<Book>();
            
            if ((bk != null))
            {
                comboBoxAuthor.DropDownStyle = ComboBoxStyle.DropDownList;
                String pubCombo = comboBoxPublisher.SelectedItem.ToString();
                String catCombo = comboBoxCategory.SelectedItem.ToString();

                Publisher pub = new Publisher();
                Categories cat = new Categories();
                Author au = new Author();
                pub.PublisherId = (from cnt in dbEntities.Publishers where cnt.Name == pubCombo select cnt).First<Publisher>().PublisherId;
                cat.CategoryId = (from ct in dbEntities.Categories where ct.CategoryName == catCombo select ct).First<Categories>().CategoryId;
                //au.AuthorId = (from x in dbEntities.Authors where x.FirstName == comboBoxAuthor.SelectedItem.ToString() select x).First<Author>().AuthorId;
                
                bk.ISBN = textBoxISBN.Text;
                bk.Title = textBoxTitle.Text;
                bk.UnitPrice = Convert.ToInt32(textBoxPrice.Text);
                bk.YearPlublished = Convert.ToDateTime(textBoxYear.Text);
                bk.QOH = Convert.ToInt32(textBoxQOH.Text);
                bk.PublisherId = Convert.ToInt32(pub.PublisherId);
                bk.CategoryId = Convert.ToInt32(cat.CategoryId);
                //bk.Authors.First<Author>().AuthorId = au.AuthorId;

                
                dbEntities.SaveChanges();
                MessageBox.Show("Updated Successfully");
            }
            else
            {
                MessageBox.Show("ISBN nor found.!");
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Delete
            int searchId = Convert.ToInt32(textBoxISBN.Text);
            Book bk = new Book();
            bk = dbEntities.Books.Where(x => x.ISBN == searchId.ToString()).FirstOrDefault<Book>();

            if ((bk != null))
            {
                dbEntities.Books.Remove(bk);
                dbEntities.SaveChanges();
                MessageBox.Show("Deleted Successfully");
                textBoxISBN.Clear();
                textBoxPrice.Clear();
                textBoxQOH.Clear();
                textBoxYear.Clear();
                textBoxYear.Clear();
                textBoxYear.Clear();
                textBoxYear.Clear();
                textBoxYear.Clear();
                textBoxYear.Clear();
                textBoxTitle.Clear();
            }
            else
            {
                MessageBox.Show("ISBN nor found.!");
                textBoxISBN.Clear();
                textBoxISBN.Focus();
            }
        }   

        private void btnExit_Click(object sender, EventArgs e)
        {
            string clr = "";
            clr = MessageBox.Show("Do You Want To Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
            if(clr=="Yes" || clr=="yes")
            {
                this.Close();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnAssignProject_Click(object sender, EventArgs e)
        {

            //Author Page
            Author athr = new Author();

            string input = "";
            input = textBoxAuthorID.Text.Trim();
            if (!Validator.IsValidId(input))
            {
                MessageBox.Show("Author ID must be a number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAuthorID.Clear();
                textBoxAuthorID.Focus();
                return;

            }

            input = textBoxAuthorLastName.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAuthorLastName.Clear();
                textBoxAuthorLastName.Focus();
                return;
            }
            input = textBoxAuthorFirstName.Text.Trim();
            if (!(Validator.IsValidName(input)))
            {
                MessageBox.Show("Name must contain letters or space(s)", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAuthorFirstName.Clear();
                textBoxAuthorFirstName.Focus();
                return;
            }

            athr.AuthorId = Convert.ToInt32(textBoxAuthorID.Text);
            athr.FirstName = textBoxAuthorFirstName.Text;
            athr.LastName = textBoxAuthorLastName.Text;
            athr.Email = textBoxAuthorEmail.Text;
           
            dbEntities.Authors.Add(athr);
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

        private void btnListAll_Click(object sender, EventArgs e)
        {
            //List Author
            var listInformation =  
                                  from Author in dbEntities.Authors
                                  
                                  select new
                                  {
                                      AuthorId = Author.AuthorId,
                                      FirstName = Author.FirstName, 
                                      LastName = Author.LastName,
                                      Email=Author.Email

                                  };

            listViewAuthorInformation.Items.Clear();
            foreach (var item in listInformation)
            {
                ListViewItem _item = new ListViewItem(item.AuthorId.ToString());
                _item.SubItems.Add(item.FirstName);
                _item.SubItems.Add(item.LastName);
                _item.SubItems.Add(item.Email);
                listViewAuthorInformation.Items.Add(_item);

            }


        }

        private void comboBoxAuthorID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnExitTab2_Click(object sender, EventArgs e)
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
