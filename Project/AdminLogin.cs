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
    public partial class AdminLogin : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8"); 
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                SqlCommand cmd = new SqlCommand("Select Pass from Admin where Id = '" + textBox1.Text + "'", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0].ToString() == textBox2.Text)
                    {
                        con.Close();
                        //MessageBox.Show("Welcome Admin");
                        this.Hide();
                        Admin am = new Admin();
                        am.Show();
                    }
                    else
                    {
                        con.Close();
                        MessageBox.Show("Invalid Password", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = "";
                        textBox2.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ID", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter both the fields");
            }
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {
            
        }
    }
}
