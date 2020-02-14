using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using HiTech_Library.Business;
using HiTech_Library.DataAccess;
using HiTech_Library.BLL;
using HiTech_Library.DAL;
using HiTech_Library.Validation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HiTech_Library.Business
{

    //------------------Employee--------------//

    public class HiTechEmployee
    {
        private int employeeId;
        private string firstName;
        private string lastName;
        private string jobTitle;
        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string JobTitle { get => jobTitle; set => jobTitle = value; }

        //Unique ID
        public bool IsUniqueEmpId(int id)
        {
            return (EmployeeDB.IsUniqueId(id));
        }
        //save
        public void SaveEmployee(HiTechEmployee emp)
        {
            EmployeeDB.SaveRecord(emp);
        }
        //search
        public HiTechEmployee searchemployee(int empid)
        {
            return (EmployeeDB.SearchRecord(empid));
        }
        public List<HiTechEmployee> SearchEmployee(int option, string input)
        {
            return (EmployeeDB.SearchRecord(option, input));
        }
        ////List
        public List<HiTechEmployee> ListEmployee()
        {
            return (EmployeeDB.ListRecord());
        }
        ////Update
        public void updateEmployee(HiTechEmployee emp)
        {
            EmployeeDB.updateRecord(emp);
        }
        //Delete
        public void DeleteEmployee(HiTechEmployee emp)
        {
            EmployeeDB.DeleteRecord(emp);
        }


    }

//--------------------------------------------------USER BUSINESS--------------------------------------------
    public class HiTechUser
    {
        int userId;
        string firstName;
        string lastName;
        string password;
        string jobTiTle;

        public int UserId { get => userId; set => userId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Password { get => password; set => password = value; }
        public string JobTiTle { get => jobTiTle; set => jobTiTle = value; }


        //Unique ID
        public bool IsUniqueUserId(int id)
        {
            return (UserDB.IsUniqueId(id));
        }
        //save
        public void SaveUser(HiTechUser use)
        {
            UserDB.SaveRecord(use);
        }
        //search
        public HiTechUser searchUser(int userId)
        {
            return (UserDB.SearchRecord(userId));
        }
        public List<HiTechUser> SearchUser(int option, string input)
        {
            return (UserDB.SearchRecord(option, input));
        }
        //LOGIN
        public bool ForLogIn(int id,string pwd,bool flag, string job)
        {
           return UserDB.logIn(id,pwd,flag,job);
        }
        ////List
        public List<HiTechUser> ListUsers()
        {
            return (UserDB.ListRecord());
        }
        ////Update
        public void updateUser(HiTechUser emp)
        {
            UserDB.updateRecord(emp);
        }

        //Update Forgot Password

            public void newPassword(HiTechUser use)
        {
            UserDB.updateNewPassword(use);
        }
        //Delete
        public void DeleteUser(HiTechUser emp)
        {
            UserDB.DeleteRecord(emp);
        }


    }
}

namespace HiTech_Library.DataAccess
{
    public static class UtilityDB
    {
        public static SqlConnection ConnectDB()
        {
            SqlConnection connDB = new SqlConnection();
            connDB.ConnectionString = ConfigurationManager.ConnectionStrings["HiTechDBConnection"].ConnectionString;
            connDB.Open();
            return connDB;
        }
    }

    //----------------------EMPLOYEE DB-----------------------------------
    public static class EmployeeDB
    {
        //Unique ID
        public static bool IsUniqueId(int tempId)
        {

            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT * FROM HiTechEmployees " +
                                " WHERE EmployeeId= " + tempId;
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            if (id != 0)
            {
                return false;
            }
            return true;

        }
        //SAVE
        public static void SaveRecord(HiTechEmployee emp)
        {
            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "INSERT INTO HiTechEmployees(EmployeeId,FirstName,LastName,JobTitle) " +
                              " VALUES(@EmployeeId,@FirstName,@LastName,@JobTitle) ";
            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@LastName", emp.LastName);
            cmd.Parameters.AddWithValue("@JobTitle", emp.JobTitle);
            cmd.ExecuteNonQuery();
            connDB.Close();

        }
        //SEARCH 
        public static HiTechEmployee SearchRecord(int empId)
        {
            HiTechEmployee emp = new HiTechEmployee();
            //Connect DB
            SqlConnection connDB = UtilityDB.ConnectDB();
            //SqlCommand object
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT * FROM HiTechEmployees " +
                              "WHERE EmployeeId = @EmployeeId ";
            cmd.Parameters.AddWithValue("@EmployeeId", empId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                emp.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                emp.FirstName = reader["FirstName"].ToString();
                emp.LastName = reader["LastName"].ToString();
                emp.JobTitle = reader["JobTitle"].ToString();
            }
            else
            {
                emp = null;
            }
            connDB.Close();
            //Close DB

            return emp;
        }

        public static List<HiTechEmployee> SearchRecord(int option, string name)
        {
            List<HiTechEmployee> listEmp = new List<HiTechEmployee>();
            using (SqlConnection connDB = UtilityDB.ConnectDB())
            {
                switch (option)
                {
                    case 1: // search by FirstName
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connDB;
                        cmd.CommandText = "SELECT * FROM HiTechEmployees " +
                                          "WHERE FirstName = @FirstName ";

                        cmd.Parameters.AddWithValue("@FirstName", name);
                        SqlDataReader reader = cmd.ExecuteReader();
                        HiTechEmployee emp;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                emp = new HiTechEmployee();
                                emp.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                emp.FirstName = reader["FirstName"].ToString();
                                emp.LastName = reader["LastName"].ToString();
                                emp.JobTitle = reader["JobTitle"].ToString();
                                listEmp.Add(emp);
                            }

                        }
                        else
                        {
                            listEmp = null;
                        }
                        break;
                    case 2:
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = connDB;
                        cmd2.CommandText = "SELECT * FROM HiTechEmployees " +
                                          "WHERE LastName = @LastName ";

                        cmd2.Parameters.AddWithValue("@LastName", name);
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        HiTechEmployee emp2;
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                emp2 = new HiTechEmployee();
                                emp2.EmployeeId = Convert.ToInt32(reader2["EmployeeId"]);
                                emp2.FirstName = reader2["FirstName"].ToString();
                                emp2.LastName = reader2["LastName"].ToString();
                                emp2.JobTitle = reader2["JobTitle"].ToString();
                                listEmp.Add(emp2);
                            }

                        }
                        else
                        {
                            listEmp = null;
                        }
                        break;
                    default:
                        break;
                }
            }

            return listEmp;
        }
        //LIST ALL
        public static List<HiTechEmployee> ListRecord()
        {
            List<HiTechEmployee> listEmp = new List<HiTechEmployee>();

            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HiTechEmployees", connDB);
            SqlDataReader reader = cmd.ExecuteReader();
            HiTechEmployee emp;
            while (reader.Read())
            {
                emp = new HiTechEmployee();
                emp.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                emp.FirstName = reader["FirstName"].ToString();
                emp.LastName = reader["LastName"].ToString();
                emp.JobTitle = reader["JobTitle"].ToString();
                listEmp.Add(emp);
            }
            return listEmp;
        }
        //Update
        public static void updateRecord(HiTechEmployee emp)
        {
            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "UPDATE HiTechEmployees SET FirstName=@FirstName , LastName=@LastName, JobTitle=@JobTitle " +
                              "WHERE EmployeeId=@EmployeeID";
            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@LastName", emp.LastName);
            cmd.Parameters.AddWithValue("@JobTitle", emp.JobTitle);
            cmd.ExecuteNonQuery();
            connDB.Close();
        }
        //DELETE
        public static void DeleteRecord(HiTechEmployee emp)
        {
            SqlConnection connDb = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDb;
            cmd.CommandText = "DELETE FROM HiTechEmployees WHERE EmployeeId=" + emp.EmployeeId;
            cmd.ExecuteNonQuery();
            connDb.Close();

        }

    }

    ////----------------------USER DB-----------------------------------
    public static class UserDB
    {


        //Unique ID
        public static bool IsUniqueId(int tempId)
        {

            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT * FROM HiTechUsers " +
                                " WHERE UserId= " + tempId;
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            if (id != 0)
            {
                return false;
            }
            return true;

        }
        //SAVE
       
        public static void SaveRecord(HiTechUser use)
        {
            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "INSERT INTO HiTechUsers(UserId,FirstName,LastName,Password,JobTitle) " +
                              " VALUES(@UserId,@FirstName,@LastName,@Password,@JobTitle) ";
            cmd.Parameters.AddWithValue("@UserId", use.UserId);
            cmd.Parameters.AddWithValue("@FirstName", use.FirstName);
            cmd.Parameters.AddWithValue("@LastName", use.LastName);
            cmd.Parameters.AddWithValue("@Password", use.Password);
            cmd.Parameters.AddWithValue("@JobTitle", use.JobTiTle);
            cmd.ExecuteNonQuery();
            connDB.Close();

        }
        //LOGIN
        public static bool logIn(int usrId,string pwd,bool flag,string job)
        {
           
            HiTechUser use = new HiTechUser();
            //Connect DB
            SqlConnection connDB = UtilityDB.ConnectDB();
            //SqlCommand object
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT Password FROM HiTechUsers " +
                              "WHERE UserId = @UserId AND JobTitle=@JobTitle";
            cmd.Parameters.AddWithValue("@UserId", usrId);
            cmd.Parameters.AddWithValue("@Password", pwd);
            cmd.Parameters.AddWithValue("@JobTitle", job);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                if(reader.HasRows==true)
                {
                    if (pwd == reader["Password"].ToString())
                    {
                    
                        flag = true;
                        return flag;

                    }
                   
                }
            }
           
            connDB.Close();
            return flag;
        }
    

        //SEARCH 
        public static HiTechUser SearchRecord(int userId)
        {
            HiTechUser use = new HiTechUser();
            //Connect DB
            SqlConnection connDB = UtilityDB.ConnectDB();
            //SqlCommand object
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT * FROM HiTechUsers " +
                              "WHERE UserId = @UserId ";
            cmd.Parameters.AddWithValue("@UserId", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                use.UserId = Convert.ToInt32(reader["UserId"]);
                use.FirstName = reader["FirstName"].ToString();
                use.LastName = reader["LastName"].ToString();
                use.JobTiTle = reader["JobTitle"].ToString();
                use.Password = reader["Password"].ToString();
            }
            else
            {
                use = null;
            }
            connDB.Close();
            //Close DB

            return use;
        }

        public static List<HiTechUser> SearchRecord(int option, string name)
        {
            List<HiTechUser> listUse = new List<HiTechUser>();
            using (SqlConnection connDB = UtilityDB.ConnectDB())
            {
                switch (option)
                {
                    case 1: // search by FirstName
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connDB;
                        cmd.CommandText = "SELECT * FROM HiTechUsers " +
                                          "WHERE FirstName = @FirstName ";

                        cmd.Parameters.AddWithValue("@FirstName", name);
                        SqlDataReader reader = cmd.ExecuteReader();
                        HiTechUser use;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                use = new HiTechUser();
                                use.UserId = Convert.ToInt32(reader["UserId"]);
                                use.FirstName = reader["FirstName"].ToString();
                                use.LastName = reader["LastName"].ToString();
                                use.JobTiTle = reader["JobTitle"].ToString();
                                use.Password = reader["Password"].ToString();
                                listUse.Add(use);
                            }

                        }
                        else
                        {
                            listUse = null;
                        }
                        break;
                    case 2:
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = connDB;
                        cmd2.CommandText = "SELECT * FROM HiTechUsers " +
                                          "WHERE LastName = @LastName ";

                        cmd2.Parameters.AddWithValue("@LastName", name);
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        HiTechUser use2;
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                use2 = new HiTechUser();
                               use2.UserId = Convert.ToInt32(reader2["UserId"]);
                                use2.FirstName = reader2["FirstName"].ToString();
                                use2.LastName = reader2["LastName"].ToString();
                                use2.JobTiTle = reader2["JobTitle"].ToString();
                                use2.Password = reader2["Password"].ToString();
                                listUse.Add(use2);
                            }

                        }
                        else
                        {
                            listUse = null;
                        }
                        break;
                    default:
                        break;
                }
            }

            return listUse;
        }
        //    //LIST ALL
        public static List<HiTechUser> ListRecord()
        {
            List<HiTechUser> listUse = new List<HiTechUser>();

            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HiTechUsers", connDB);
            SqlDataReader reader = cmd.ExecuteReader();
            HiTechUser use;
            while (reader.Read())
            {
                use = new HiTechUser();
                use.UserId = Convert.ToInt32(reader["UserId"]);
                use.FirstName = reader["FirstName"].ToString();
                use.LastName = reader["LastName"].ToString();
                use.JobTiTle = reader["JobTitle"].ToString();
                use.Password = reader["Password"].ToString();
                listUse.Add(use);
            }
            return listUse;
        }
        //    //Update
        public static void updateRecord(HiTechUser use)
        {
            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "UPDATE HiTechUsers SET FirstName=@FirstName , LastName=@LastName, JobTitle=@JobTitle, Password=@Password " +
                              "WHERE UserId=@UserId";
            cmd.Parameters.AddWithValue("@UserId", use.UserId);
            cmd.Parameters.AddWithValue("@FirstName", use.FirstName);
            cmd.Parameters.AddWithValue("@LastName", use.LastName);
            cmd.Parameters.AddWithValue("@JobTitle", use.JobTiTle);
            cmd.Parameters.AddWithValue("@Password", use.Password);
            cmd.ExecuteNonQuery();
            connDB.Close();
        }
        //Forgot Password
        public static void updateNewPassword(HiTechUser use)
        {
            SqlConnection connDB = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "UPDATE HiTechUsers SET Password=@Password " +
                              "WHERE UserId=@UserId";
            cmd.Parameters.AddWithValue("@UserId", use.UserId);
            cmd.Parameters.AddWithValue("@Password", use.Password);
            cmd.ExecuteNonQuery();
            connDB.Close();
        }
        //    //DELETE
        public static void DeleteRecord(HiTechUser use)
        {
            SqlConnection connDb = UtilityDB.ConnectDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDb;
            cmd.CommandText = "DELETE FROM HiTechUsers WHERE UserId=" + use.UserId;
            cmd.ExecuteNonQuery();
            connDb.Close();

        }

    }
}


namespace HiTech_Library.Validation
{
    public static class Validator
    {
        public static bool IsValidId(string input, int length)
        {
            if (!Regex.IsMatch(input, @"^\d{" + length + "}$"))
            {
                MessageBox.Show("Invalid ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }
        public static bool IsValidId(string input)
        {
            if (!Regex.IsMatch(input, @"^\d+$"))
            {
                MessageBox.Show("Invalid ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }
        public static bool IsValidPostalCode(string input)
        {
            if (!Regex.IsMatch(input, @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$"))
            {
                MessageBox.Show("Invalid Postal Code i.e H3H1K7", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }
        
        public static bool IsValidName(string input)
        {
            if (input.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < input.Length; i++)
            {
                if ((!(Char.IsLetter(input[i]))) && (!(Char.IsWhiteSpace(input[i]))))
                {
                    return false;
                }

            }
            return true;
        }

    }
}


//========================PROJECT-2==========================================
namespace HiTech_Library.BLL
{

    public class Customers
    {
       
        private string firstName;
        private int customerId;
        private int phoneNumber;
        private int creditLimit;
        private string street;
        private string city;
        private string postalCode;
        private string faxNumber;

        public string FirstName { get => firstName; set => firstName = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int CreditLimit { get => creditLimit; set => creditLimit = value; }
        public string Street { get => street; set => street = value; }
        public string City { get => city; set => city = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public string FaxNumber { get => faxNumber; set => faxNumber = value; }

        public bool IsUniqueCustomerId(int id)
        {
            return (CustomerDB.IsUniqueId(id));
        }
        public List<Customers> ListCustomer()
        {
            return (CustomerDB.ListRecord());
        }

    }
}

namespace HiTech_Library.DAL
{

    public static class CustomerDB
    {
        public static bool IsUniqueId(int tempId)
        {

            //connect the database
            SqlConnection connDB = UtilityDB.ConnectDB();
            //create and customize an object of type SqlCommand
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connDB;
            cmd.CommandText = "SELECT * FROM Customers " +
                                " WHERE CustomerId= " + tempId;
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            if (id != 0)
            {
                return false;
            }
            return true;
        }
        //This is for Dataset
        public static List<Customers> ListRecord()
        {
            List<Customers> listS = new List<Customers>();
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                Customers cstmr;
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cstmr = new Customers();
                        cstmr.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                        cstmr.PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                        cstmr.CreditLimit = Convert.ToInt32(reader["CreditLimit"]);
                        cstmr.FirstName = reader["Name"].ToString();
                        cstmr.Street = reader["Street"].ToString();
                        cstmr.City = reader["City"].ToString();
                        cstmr.FaxNumber = reader["FaxNumber"].ToString();
                        cstmr.PostalCode = reader["PostalCode"].ToString();

                        listS.Add(cstmr);

                    }

                }
                else
                {
                    listS = null;
                }

            }

            return listS;
        }


    }
}