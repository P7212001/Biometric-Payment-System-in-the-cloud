using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Finger_ATM
{
    public partial class CustomerLogin : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        public CustomerLogin()
        {
            InitializeComponent();
        }
        public static string a2 = ""; 
      
        private void button1_Click(object sender, EventArgs e)
        {
            string s = "Select Email,Password from Customer where Email='" + textBox1.Text + "' and Password='" + textBox2.Text + "'";
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                a2 = textBox1.Text;
               // MessageBox.Show("Login successfully");
                con.Close();
                Customermain f = new Customermain();
                f.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Please Check your Id or Password");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
            }
        }
    }
}
