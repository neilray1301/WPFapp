using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
namespace WpfApp1
{
   
    public partial class MainWindow : Window
    {
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\neilr\source\repos\WpfApp1\WpfApp1\Database1.mdf;Integrated Security = True"   );
        public MainWindow()
        {
            InitializeComponent();
            Bindgrid();
            btnDelete.Visibility = System.Windows.Visibility.Hidden;
        }

       private void Bindgrid()
        {
            
            if(conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from EmpTable", conn);
          
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
           da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();
            if(dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
            
        }
        private void btnAdd_Click(object sender,RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            if(conn.State != ConnectionState.Open)
            {
                conn.Open();

            }
            cmd.Connection = conn;

            if(txtEmpId.Text != "")
            {
                if (txtEmpId.IsEnabled == true)
                {
                    if(ddlGender.Text!="Select Gender"){
                        SqlCommand cmd1 = new SqlCommand("insertvalues",conn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add("@Empid", SqlDbType.Int).Value = txtEmpId.Text;
                        cmd1.Parameters.Add("@EmpName", SqlDbType.Text).Value = txtEmpName.Text;
                        cmd1.Parameters.Add("@EmpGender", SqlDbType.Text).Value = ddlGender.Text;
                        cmd1.Parameters.Add("@EmpContact", SqlDbType.Text).Value = txtContact.Text;
                        cmd1.Parameters.Add("@EmpAddress", SqlDbType.Text).Value = txtAddress.Text;
                        cmd1.ExecuteNonQuery();
                        Bindgrid();
                        MessageBox.Show("Added Succesfully");

                        ClearAll();
                        btnDelete.Visibility = System.Windows.Visibility.Hidden;

                    }
                    else
                    {
                        MessageBox.Show("Please select Gender Option");
                    }

                }
                else
                {
                    SqlCommand cmd1 = new SqlCommand("updatevalues", conn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@Empid", SqlDbType.Int).Value = txtEmpId.Text;
                    cmd1.Parameters.Add("@EmpName", SqlDbType.Text).Value = txtEmpName.Text;
                    cmd1.Parameters.Add("@EmpGender", SqlDbType.Text).Value = ddlGender.SelectedItem.ToString();
                    cmd1.Parameters.Add("@EmpContact", SqlDbType.Text).Value = txtContact.Text;
                    cmd1.Parameters.Add("@EmpAddress", SqlDbType.Text).Value = txtAddress.Text;
                    cmd1.ExecuteNonQuery();
                    Bindgrid();
                    MessageBox.Show("Employee updated succesfully");
                    ClearAll();
                    btnDelete.Visibility = System.Windows.Visibility.Hidden;

                }
            }
            else
            {
                MessageBox.Show("Please Add Employee Id...");
            }
        }
        private void btnCancel_Click(object sender,RoutedEventArgs e)
        {
            ClearAll();
        }
        private void ClearAll()
        {
            txtEmpId.Text = "";
            txtEmpName.Text = "";
            ddlGender.SelectedIndex = 0;
            txtContact.Text = "";
            txtAddress.Text = "";
            btnAdd.Content = "Add";
            txtEmpId.IsEnabled = true;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtEmpId.Text = row["Id"].ToString();
                txtEmpName.Text = row["EmpName"].ToString();
                ddlGender.Text = row["Gender"].ToString();
                txtContact.Text = row["Contact"].ToString();
                txtAddress.Text = row["Address"].ToString();
                txtEmpId.IsEnabled = false;
                btnAdd.Content = "Update";
                btnDelete.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please Select Any Employee From List...");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                SqlCommand cmd = new SqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                SqlCommand cmd1 = new SqlCommand("deletevalues", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtEmpId.Text);
               // cmd1.Parameters.Add("@EmpName", SqlDbType.Text).Value = txtEmpName.Text;
                //cmd1.Parameters.Add("@EmpGender", SqlDbType.Text).Value = ddlGender.SelectedValue.ToString();
                //cmd1.Parameters.Add("@EmpContact", SqlDbType.Text).Value = txtContact.Text;
                //cmd1.Parameters.Add("@EmpAddress", SqlDbType.Text).Value = txtAddress.Text;
                cmd1.ExecuteNonQuery();
                Bindgrid();
                MessageBox.Show("Employee Deleted Successfully...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Please Select Any Employee From List...");
            }
        }

        //Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
