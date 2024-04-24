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
    public partial class MerchantProfile : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        public MerchantProfile()
        {
            InitializeComponent();
        }
        string u = MerchantLogin.a1;
           

        private void MerchantProfile_Load(object sender, EventArgs e)
        {

            merchant();
            Bank();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
           

        }

        public void merchant()
        {
            string s = "Select MId,Name,Officename,Phone,Email,LId,Address,City,PinCode from Merchant where Email='" + u + "'";
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                label8.Text = ds.Tables[0].Rows[0][0].ToString();
                label9.Text = ds.Tables[0].Rows[0][1].ToString();
                label10.Text = ds.Tables[0].Rows[0][2].ToString();
                label12.Text = ds.Tables[0].Rows[0][3].ToString();
                label11.Text = ds.Tables[0].Rows[0][4].ToString();
                label14.Text = ds.Tables[0].Rows[0][5].ToString();
                label16.Text = ds.Tables[0].Rows[0][6].ToString();
                label18.Text = ds.Tables[0].Rows[0][7].ToString();
                label20.Text = ds.Tables[0].Rows[0][8].ToString();
            }
        }
        public void Bank()
        {

            string s1 = "select MId from Merchant where Email='" + u + "' ";
            SqlDataAdapter sd = new SqlDataAdapter(s1, con);
            DataSet dd = new DataSet();
            sd.Fill(dd);
            if (dd.Tables[0].Rows.Count > 0)
            {
                string sss = dd.Tables[0].Rows[0][0].ToString();
                string w = "select BId,Bank,AccNo,Branch,IFSC,CardNo,CVV,Month,Year,Balance from Bank where ID='" + sss + "' and Type='Merchant'";
                SqlDataAdapter da1 = new SqlDataAdapter(w, con);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    label41.Text = ds1.Tables[0].Rows[0][0].ToString();

                    label40.Text = ds1.Tables[0].Rows[0][1].ToString();

                    label36.Text = ds1.Tables[0].Rows[0][2].ToString();

                    label38.Text = ds1.Tables[0].Rows[0][3].ToString();

                    label24.Text = ds1.Tables[0].Rows[0][4].ToString();

                    label22.Text = ds1.Tables[0].Rows[0][5].ToString();

                    label28.Text = ds1.Tables[0].Rows[0][6].ToString();

                    label30.Text = ds1.Tables[0].Rows[0][7].ToString();

                    label32.Text = ds1.Tables[0].Rows[0][8].ToString();

                    label34.Text = ds1.Tables[0].Rows[0][9].ToString();
                }

            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        
    }
}
