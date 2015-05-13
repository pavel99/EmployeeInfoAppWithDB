using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace EmployeeInfoApp
{
    public partial class EmployeeInfoUI : Form
    {
        public EmployeeInfoUI()
        {
            InitializeComponent();
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        public int employeeID;

       

        private string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        private int employeeId;
        private void saveButton_Click(object sender, EventArgs e)
        {
           
            Employee anEmployee=new Employee();
            anEmployee.name = nameTextBox.Text;
            anEmployee.address = addresstextBox.Text;
            anEmployee.email = emailTextBox.Text;
            anEmployee.sallry = float.Parse(sallaryTextBox.Text);

            if (IsEmailExists((anEmployee.email)))
            {
                MessageBox.Show("Email Already Exists");
            }
            else
            {





                SqlConnection connection = new SqlConnection(connectionString);

                string query = "INSERT INTO Employee Values('" + anEmployee.name + "','" + anEmployee.address + "','" +
                               anEmployee.email + "','" + anEmployee.sallry + "')";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();



              

                if (rowAffected > 0)
                {
                    MessageBox.Show("Inserted Successfully!");
                    ShowAllEmployees();



                }
                else
                {
                    MessageBox.Show("Insertion Failed");
                }
                
            }
            nameTextBox.Clear();
            emailTextBox.Clear();
            sallaryTextBox.Clear();
            addresstextBox.Clear();

        }

        public bool IsEmailExists(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);

             bool isEmailExists = false;

            string query = "SELECT Email FROM Employee Where Email='"+email+"'";
            


            

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

           
            while (reader.Read())
            {
                isEmailExists = true;



            }
            reader.Close();
            connection.Close();
            return isEmailExists;


        }

        public void ShowAllEmployees()
        {
            SqlConnection connection = new SqlConnection(connectionString);

           

            string query = "SELECT * FROM Employee";


       

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeList = new List<Employee>();

            while (reader.Read())
            {

                Employee anEmployee = new Employee();
                anEmployee.Id = int.Parse(reader["Id"].ToString());
                anEmployee.name = reader["Name"].ToString();
                anEmployee.address = reader["Address"].ToString();
                anEmployee.email = reader["Email"].ToString();
                anEmployee.sallry = float.Parse(reader["Sallary"].ToString());

                employeeList.Add(anEmployee);

            }
            reader.Close();
            connection.Close();

             
            ShowEmployeeInfo(employeeList);
        }



        public void ShowEmployeeInfo(List<Employee> emloyeeList)
        {
            employeeListView.Items.Clear();
            foreach (var employee in emloyeeList)
            {
                ListViewItem item = new ListViewItem(employee.Id.ToString());
                item.SubItems.Add(employee.name);
                item.SubItems.Add(employee.address);
                item.SubItems.Add(employee.email);
                item.SubItems.Add((employee.sallry).ToString());

                employeeListView.Items.Add(item);
            }
            
        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
           ShowAllEmployees();
        }

        private void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            updateButton.Enabled = true;
            deleteButton.Enabled = true;
            ListViewItem item = employeeListView.SelectedItems[0];

            int id = int.Parse(item.Text.ToString());

            Employee employee = GetEmployeeByID(id);

            employeeID = id;
            if (employee != null)
            {
              //  employeeId = employee.Id;

                nameTextBox.Text = employee.name;
                addresstextBox.Text = employee.address;
                emailTextBox.Text = employee.email;
                sallaryTextBox.Text = (employee.sallry).ToString();
            }

           
        }

        Employee GetEmployeeByID(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            //2. write query 

            string query = "SELECT * FROM Employee WHERE Id ='" + id + "'";


            // 3. execute query 

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeList= new List<Employee>();

            while (reader.Read())
            {

                Employee employee= new Employee();
                employee.Id = int.Parse(reader["Id"].ToString());
                employee.name = reader["Name"].ToString();
                employee.address = reader["Address"].ToString();
                employee.email= reader["Email"].ToString();
                employee.sallry = float.Parse(reader["Sallary"].ToString());

                employeeList.Add(employee);

            }
            reader.Close();
            connection.Close();

            return employeeList.FirstOrDefault();

        }

        private void updateButton_Click(object sender, EventArgs e)
        {

            string name = nameTextBox.Text;
            string address = addresstextBox.Text;
            string email = emailTextBox.Text;
            float sallary = float.Parse(sallaryTextBox.Text);
            if (IsEmailExists(email, employeeID))
            {
                MessageBox.Show("Email already exists");
            }
            else
            {








                SqlConnection connection = new SqlConnection(connectionString);




                string query = "UPDATE Employee SET Name ='" + name + "',Address='" + address + "',Email='" + email +
                               "',Sallary='" + sallary + "' WHERE Id='" + employeeID + "'";




                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();





                if (rowAffected > 0)
                {
                    MessageBox.Show("Updated Successfully!");


                    ShowAllEmployees();

                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }
            }
        

    }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                      "Confirm Delete!!",
                                      MessageBoxButtons.YesNo);
            if(confirmResult == DialogResult.Yes)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "DELETE FROM Employee Where Id='" + employeeID + "'";




                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();





                if (rowAffected > 0)
                {

                    MessageBox.Show("Deleted Successfully!");


                    ShowAllEmployees();

                }
                else
                {
                    MessageBox.Show("Deletion  Failed!");
                }
            }
        else
            {
                MessageBox.Show("Item is not deleted");
            }
            nameTextBox.Clear();
            addresstextBox.Clear();
            emailTextBox.Clear();
            sallaryTextBox.Clear();



        }
        public bool IsEmailExists(string email,int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool isEmailExists = false;

            string query = "SELECT Email FROM Employee Where Email='" + email + "' AND Id !="+id;





            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                isEmailExists = true;



            }
            reader.Close();
            connection.Close();
            return isEmailExists;


        }
        


        }
    }

