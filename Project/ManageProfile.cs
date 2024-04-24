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
    public partial class ManageProfile : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        public ManageProfile()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ManageProfile_Load(object sender, EventArgs e)
        {
            gridviewload();
        }

        public void gridviewload()
        {

            SqlCommand cmd = new SqlCommand("select Cid,Name,Phone,Email,Address,City,Limit from Customer", con);
            //SqlCommand cmd = new SqlCommand(s, con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string s = "select Cid,Name,Phone,Email,Address,City,Limit from Customer where Name='" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(s, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No such data is present");
                    gridviewload();
                }
            }
            else
            {
                MessageBox.Show("Please enter Customer's name in field");
                gridviewload();
                // MessageBox.Show("No Such Data is present");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox2.Text != "" && textBox6.Text != "" && textBox7.Text !="")
            {
                string s = "update Customer set name='" + textBox2.Text + "',Phone='" + textBox3.Text + "',Email='" + textBox4.Text + "',Address='" + textBox5.Text + "',City='" + textBox6.Text + "',Limit='"+textBox7.Text+"' where Cid='" + label10.Text + "'";
                SqlCommand cmd = new SqlCommand(s, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Successfully Updated");
                gridviewload();
                // panel2.Visible = false;
                panel2.Enabled = false;

                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox2.Text = "";
                textBox6.Text = "";

            }
            else
            {
                MessageBox.Show("Please fill All the informations");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != "")
            {
                try
                {
                    DialogResult d = MessageBox.Show("Are you sure???", "Delete opration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.Yes)
                    {
                       string Id;
                        Id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        string name = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

                       // string pname = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

                        //copying id into label for updating purpose
                        // label2.Text = Id;
                        string sql = null;

                        //getting field 
                        sql = "delete from Customer where CId='"+Id+"' and Name='" + name + "'  ";
                        SqlCommand cmd = new SqlCommand(sql, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Data Successfully deleted");
                        gridviewload();

                    }
                }

                catch (Exception g)
                {
                    MessageBox.Show("Kindly select the row to perform delete operation !", "Information");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != "")
            {
                try
                {
                    label10.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

                    textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                    textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                    textBox7.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                    
                    panel2.Visible = true;
                    panel2.Enabled = true;

                }
                catch (Exception g)
                {
                    MessageBox.Show("Kindly select the row to perform Edit operation !", "Information");
                }
            }
            else
            {
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

    }
}
