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
    public partial class MerchantTransaction : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        public MerchantTransaction()
        {
            InitializeComponent();
        }

        private void MerchantTransaction_Load(object sender, EventArgs e)
        {
            string u = MerchantLogin.a1;
            string b = "select MId from Merchant where Email='" + u + "'";
            SqlDataAdapter sd = new SqlDataAdapter(b, con);
            DataSet dd = new DataSet();
            sd.Fill(dd);
            if (dd.Tables[0].Rows.Count > 0)
            {
                string sss = dd.Tables[0].Rows[0][0].ToString();

                string w = "select TId,UserId,UserName,MerId,MerchantName,Amt,Date,Note From Trans where MerId='" + sss + "'";
                SqlCommand cmd = new SqlCommand(w, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    this.dataGridView1.Columns["Tid"].Width = 100;
                    this.dataGridView1.Columns["UserId"].Width = 100;
                    this.dataGridView1.Columns["UserName"].Width = 180;
                    this.dataGridView1.Columns["MerId"].Width = 100;
                    this.dataGridView1.Columns["MerchantName"].Width = 180;
                    this.dataGridView1.Columns["Amt"].Width = 120;
                    this.dataGridView1.Columns["Date"].Width = 180;
                    this.dataGridView1.Columns["Note"].Width = 300;
            
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string u = MerchantLogin.a1;
            string b = "select MId from Merchant where Email='" + u + "'";
            SqlDataAdapter sd = new SqlDataAdapter(b, con);
            DataSet dd = new DataSet();
            sd.Fill(dd);
            if (dd.Tables[0].Rows.Count > 0)
            {
                string sss = dd.Tables[0].Rows[0][0].ToString();

                string s1 = "select TId,UserId,UserName,MerId,MerchantName,Amt,Date,Note From Trans where Date>='" + dateTimePicker2.Text + "' AND Date<='" + dateTimePicker1.Text + "' and MerId='" + sss + "'";
                SqlCommand cmd = new SqlCommand(s1, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    this.dataGridView1.Columns["Tid"].Width = 100;
                    this.dataGridView1.Columns["UserId"].Width = 100;
                    this.dataGridView1.Columns["UserName"].Width = 180;
                    this.dataGridView1.Columns["MerId"].Width = 100;
                    this.dataGridView1.Columns["MerchantName"].Width = 180;
                    this.dataGridView1.Columns["Amt"].Width = 120;
                    this.dataGridView1.Columns["Date"].Width = 180;
                    this.dataGridView1.Columns["Note"].Width = 300;

                }
                else
                {
                    dataGridView1.DataSource = "";

                    MessageBox.Show("No Data Present for particular Date");
            
                }
            }
        }
    }
    
}
