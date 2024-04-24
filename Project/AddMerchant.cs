using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace Finger_ATM
{
    public partial class AddMerchant : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        public AddMerchant()
        {
            InitializeComponent();
        }
        public static string aa = ""; 
      
        private void button2_Click(object sender, EventArgs e)
        {

        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox8.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && textBox9.Text != "" && textBox18.Text != "")
            {
                string email = "Select Email from Merchant where Email='" + textBox4.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter(email, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {

                    string s = "insert into Merchant(MId,Name,Officename,Phone,Email,LId,Address,City,Pincode,Password) values ('" + label3.Text + "','" + textBox2.Text + "','" + textBox8.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + textBox9.Text + "','" + textBox18.Text + "')";
                    aa = label3.Text;
                    SqlCommand cmd = new SqlCommand(s, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                  //  MessageBox.Show("Insertion done");
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];

                    //textBox2.Text ="";
                    //textBox8.Text ="";

                    //textBox3.Text ="";

                    //textBox4.Text ="";

                    //textBox5.Text ="";

                    //textBox6.Text ="";

                    //textBox7.Text ="";

                    //textBox9.Text ="";

                }
                else
                {
                    MessageBox.Show("Email Id Already Exist");
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox9.Text = "";
                    textBox8.Text = "";
                    textBox18.Text = "";

                }
            }
            else
            {
                MessageBox.Show("Please fill All info");
            }
        }

        private void AddMerchant_Load(object sender, EventArgs e)
        {
            Mid();
            Bid();
        }

        public void Bid()
        {
            string str = "select top 1 BId from Bank order by Bid desc";
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string i = ds.Tables[0].Rows[0][0].ToString();

                int a = Convert.ToInt32(i);
                a++;
                label23.Text = a.ToString();
            }
            else
            {
                label23.Text = "1001";
            }
        }

        public void Mid()
        {
            string str = "select top 1 MId from Merchant order by MId desc";
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string i = ds.Tables[0].Rows[0][0].ToString();

                int a = Convert.ToInt32(i);
                a++;
                label3.Text = a.ToString();
            }
            else
            {
                label3.Text = "20001";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }



        public void eemail(string mailid, string pass)
        {
            string msg = "Dear " + textBox2.Text + ", Mail From Biometric Transaction System,  Userid :  " + mailid + "and Password : " + pass + ".";
            string url = ("http://smail.azurewebsites.net/Email.aspx?Title=Verification&emailid=" + mailid + "&Sub= Biometric Transaction System&Msg=" + msg + "");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            HttpWebResponse res1 = (HttpWebResponse)httpWebRequest.GetResponse();

        } 
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (textBox16.Text != "" && textBox15.Text != "" && textBox14.Text != "" && textBox13.Text != "" && textBox12.Text != "" && textBox11.Text != "" && textBox10.Text != "")
            {
                string u = AddMerchant.aa;
                string s = "insert into Bank(BId,Type,ID,Bank,AccNo,Branch,IFSC,CardNo,CVV,Month,Year,Balance) values('" + label23.Text + "','Merchant','" + u + "','" + textBox17.Text + "','" + textBox16.Text + "','" + textBox15.Text + "','" + textBox14.Text + "','" + textBox13.Text + "','" + textBox12.Text + "','" + textBox10.Text + "','" + textBox1.Text + "','" + textBox11.Text + "')";
                SqlCommand cmd = new SqlCommand(s, con);
                con.Open();
                cmd.ExecuteNonQuery();
              //  MessageBox.Show("Insertion Sucessfully");
                this.Hide();
                Admin am = new Admin();
                am.Show();
                eemail(textBox4.Text, textBox18.Text);

                con.Close();
            }
            else
            {
                MessageBox.Show("Please fill Proper Info");
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (textBox4.Text.Length > 0 && textBox4.Text.Trim().Length != 0)
            {
                if (!rEmail.IsMatch(textBox4.Text.Trim()))
                {
                    MessageBox.Show("check email id");
                    //textBox3.SelectAll();
                    //e.Cancel = true;
                    textBox4.Clear();
                    textBox4.Focus();
                }
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[7-9]\d{9}$");
            if (textBox3.Text.Length > 0 && textBox3.Text.Trim().Length != 0)
            {
                if (!rEmail.IsMatch(textBox3.Text.Trim()))
                {
                    MessageBox.Show("Please enter a valid Phone Number !!", "Information");
                    // textBox5.SelectAll();
                    // e.Cancel = true;
                    textBox3.Clear();
                    textBox3.Focus();
                }
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                e.Handled = true;
            }
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch(textBox10.Text, "[^1-12]"))
            //{
            //    MessageBox.Show("Please enter valid month.");
            //    textBox10.Text = "";
            //    textBox10.Focus();
            //    //textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            //}
            if (textBox10.Text != "")
            {
                int mon = Convert.ToInt32(textBox10.Text);
                if (mon > 12)
                {
                    MessageBox.Show("Please enter valid month.");
                    textBox10.Text = "";
                    textBox10.Focus();
                    // textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                }
            }
        }

        private void textBox13_Validating(object sender, CancelEventArgs e)
        {
            if (textBox13.Text.Length == 16)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox13.Text, "[^[0-9]{15}]"))
                {

                    MessageBox.Show("Please enter valid month.");
                    textBox13.Text = "";
                    textBox13.Focus();
                    //textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);

                    //else
                    //{
                    //    MessageBox.Show("Enter valid exactly 16 digits");
                    ////}
                }
                //else
                //{
                //    MessageBox.Show("error");
                //}
            }
            else
            {
                MessageBox.Show("Enter exactly 16 digits");
                textBox13.Text = "";
                textBox13.Focus();
                   
            }
        }

        private void textBox12_Validating(object sender, CancelEventArgs e)
        {
            if (textBox12.Text.Length == 3)
            {
            }
            else
            {
                MessageBox.Show("Exactly 3 Digits Only");
                textBox12.Text = "";
                textBox12.Focus();
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

     

    }
}
