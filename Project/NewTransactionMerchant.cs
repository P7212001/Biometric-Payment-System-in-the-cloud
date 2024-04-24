using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SecuGen.FDxSDKPro.Windows;
using System.Data.SqlClient;
using System.IO;

namespace Finger_ATM
{
    public partial class NewTransactionMerchant : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SG2NWPLS19SQL-v09.mssql.shr.prod.sin2.secureserver.net;Initial Catalog=DMerchantPay;Persist Security Info=True;User Id=DMerchantPay;Password=m^4Ty56k8");
        private SGFingerPrintManager m_FPM;
        private bool m_LedOn = false;
        private Int32 m_ImageWidth;
        private Int32 m_ImageHeight;
        private Byte[] m_RegMin1;
        private Byte[] m_RegMin2;
        private Byte[] m_VrfMin;
        Byte[] fp_image;
        private SGFPMDeviceList[] m_DevList; // Used for EnumerateDevice

        public NewTransactionMerchant()
        {
            InitializeComponent();
        }
        public static string aa1 = ""; 
        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select Finger,CId from Customer", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                con.Close();
                MessageBox.Show("No Data Present", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Int32 iError;
                Byte[] fp_image;
                Int32 img_qlty;
                string id = "";

                fp_image = new Byte[m_ImageWidth * m_ImageHeight];
                img_qlty = 0;

                iError = m_FPM.GetImage(fp_image);

                m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);


                if  (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                    DrawImage(fp_image, pictureBox2);
                    iError = m_FPM.CreateTemplate(fp_image, m_RegMin1);

                    if (iError == (Int32)SGFPMError.ERROR_NONE)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            fp_image = (byte[])(ds.Tables[0].Rows[i][0]);

                            iError = m_FPM.CreateTemplate(fp_image, m_VrfMin);

                            MemoryStream ms = new MemoryStream();
                            bool matched1 = false;
                            SGFPMSecurityLevel secu_level;

                            secu_level = (SGFPMSecurityLevel)5;

                            iError = m_FPM.MatchTemplate(m_RegMin1, m_VrfMin, secu_level, ref matched1);

                            if (iError == (Int32)SGFPMError.ERROR_NONE)
                            {
                                if (matched1)
                                {
                                    id = ds.Tables[0].Rows[i][1].ToString();
                                }
                            }
                            else
                            {
                                DisplayError("MatchTemplate()", iError);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                if (id != "")
                {
                   
                    // MessageBox.Show(id);
                    string s1 = "Select c.Name,c.Phone,c.Email,c.Limit,b.Bank,b.AccNo,b.Branch from Customer c inner join Bank b on c.CId=b.ID where ID='" + id + "' and Type='Customer' ";
                    SqlDataAdapter da1 = new SqlDataAdapter(s1, con);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        aa1 = id;
                        panel1.Visible = true;
                        textBox6.Text = ds1.Tables[0].Rows[0][0].ToString();
                        textBox3.Text = ds1.Tables[0].Rows[0][1].ToString();
                        textBox4.Text = ds1.Tables[0].Rows[0][2].ToString();
                        textBox7.Text = ds1.Tables[0].Rows[0][3].ToString();
                        textBox16.Text = ds1.Tables[0].Rows[0][4].ToString();
                        textBox15.Text = ds1.Tables[0].Rows[0][5].ToString();
                        textBox14.Text = ds1.Tables[0].Rows[0][6].ToString();
                    }

                }
                else
                {
                    MessageBox.Show("Not Matched");
                }
            }
        }

        private void NewTransactionMerchant_Load(object sender, EventArgs e)
        {
            m_LedOn = false;
            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            m_FPM = new SGFingerPrintManager();
            EnumerateBtn_Click(sender, e);
            Tid();

        }


        private void EnumerateBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Int32 iError;
                string enum_device;

                // Enumerate Device
                iError = m_FPM.EnumerateDevice();

                // Get enumeration info into SGFPMDeviceList
                m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];

                m_DevList[0] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(0, m_DevList[0]);
                enum_device = m_DevList[0].DevName.ToString() + " : " + m_DevList[0].DevID;

                SGFPMDeviceName device_name;
                Int32 device_id;

                device_name = m_DevList[0].DevName;
                device_id = m_DevList[0].DevID;

                iError = m_FPM.Init(device_name);
                iError = m_FPM.OpenDevice(device_id);

                if (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                    GetBtn_Click(sender, e);

                   // button1.Enabled = true;
                }
                else
                    DisplayError("OpenDevice()", iError);
            }
            catch (Exception ep)
            {
               // button1.Enabled = false;
            }
        }

        private void GetBtn_Click(object sender, System.EventArgs e)
        {
            SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
            Int32 iError = m_FPM.GetDeviceInfo(pInfo);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                m_ImageWidth = pInfo.ImageWidth;
                m_ImageHeight = pInfo.ImageHeight;
                ASCIIEncoding encoding = new ASCIIEncoding();
            }
        }

        void DisplayError(string funcName, int iError)
        {
            string text = "";

            switch (iError)
            {
                case 0:                             //SGFDX_ERROR_NONE				= 0,
                    text = "Error none";
                    break;

                case 1:                             //SGFDX_ERROR_CREATION_FAILED	= 1,
                    text = "Can not create object";
                    break;

                case 2:                             //   SGFDX_ERROR_FUNCTION_FAILED	= 2,
                    text = "Function Failed";
                    break;

                case 3:                             //   SGFDX_ERROR_INVALID_PARAM	= 3,
                    text = "Invalid Parameter";
                    break;

                case 4:                          //   SGFDX_ERROR_NOT_USED			= 4,
                    text = "Not used function";
                    break;

                case 5:                                //SGFDX_ERROR_DLLLOAD_FAILED	= 5,
                    text = "Can not create object";
                    break;

                case 6:                                //SGFDX_ERROR_DLLLOAD_FAILED_DRV	= 6,
                    text = "Can not load device driver";
                    break;
                case 7:                                //SGFDX_ERROR_DLLLOAD_FAILED_ALGO = 7,
                    text = "Can not load sgfpamx.dll";
                    break;

                case 51:                //SGFDX_ERROR_SYSLOAD_FAILED	   = 51,	// system file load fail
                    text = "Can not load driver kernel file";
                    break;

                case 52:                //SGFDX_ERROR_INITIALIZE_FAILED  = 52,   // chip initialize fail
                    text = "Failed to initialize the device";
                    break;

                case 53:                //SGFDX_ERROR_LINE_DROPPED		   = 53,   // image data drop
                    text = "Data transmission is not good";
                    break;

                case 54:                //SGFDX_ERROR_TIME_OUT			   = 54,   // getliveimage timeout error
                    text = "Time out";
                    break;

                case 55:                //SGFDX_ERROR_DEVICE_NOT_FOUND	= 55,   // device not found
                    text = "Device not found";
                    break;

                case 56:                //SGFDX_ERROR_DRVLOAD_FAILED	   = 56,   // dll file load fail
                    text = "Can not load driver file";
                    break;

                case 57:                //SGFDX_ERROR_WRONG_IMAGE		   = 57,   // wrong image
                    text = "Wrong Image";
                    break;

                case 58:                //SGFDX_ERROR_LACK_OF_BANDWIDTH  = 58,   // USB Bandwith Lack Error
                    text = "Lack of USB Bandwith";
                    break;

                case 59:                //SGFDX_ERROR_DEV_ALREADY_OPEN	= 59,   // Device Exclusive access Error
                    text = "Device is already opened";
                    break;

                case 60:                //SGFDX_ERROR_GETSN_FAILED		   = 60,   // Fail to get Device Serial Number
                    text = "Device serial number error";
                    break;

                case 61:                //SGFDX_ERROR_UNSUPPORTED_DEV		   = 61,   // Unsupported device
                    text = "Unsupported device";
                    break;

                // Extract & Verification error
                case 101:                //SGFDX_ERROR_FEAT_NUMBER		= 101, // utoo small number of minutiae
                    text = "The number of minutiae is too small";
                    break;

                case 102:                //SGFDX_ERROR_INVALID_TEMPLATE_TYPE		= 102, // wrong template type
                    text = "Template is invalid";
                    break;

                case 103:                //SGFDX_ERROR_INVALID_TEMPLATE1		= 103, // wrong template type
                    text = "1st template is invalid";
                    break;

                case 104:                //SGFDX_ERROR_INVALID_TEMPLATE2		= 104, // vwrong template type
                    text = "2nd template is invalid";
                    break;

                case 105:                //SGFDX_ERROR_EXTRACT_FAIL		= 105, // extraction fail
                    text = "Minutiae extraction failed";
                    break;

                case 106:                //SGFDX_ERROR_MATCH_FAIL		= 106, // matching  fail
                    text = "Matching failed";
                    break;
            }

            text = funcName + " Error # " + iError + " :" + text;
            MessageBox.Show(text, "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void DrawImage(Byte[] imgData, PictureBox picBox)
        {
            int colorval;
            Bitmap bmp = new Bitmap(m_ImageWidth, m_ImageHeight);
            picBox.Image = (Image)bmp;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    colorval = (int)imgData[(j * m_ImageWidth) + i];
                    bmp.SetPixel(i, j, Color.FromArgb(colorval, colorval, colorval));
                }
            }
            picBox.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(textBox1.Text) <= Convert.ToDouble(textBox7.Text))
            {
                double amt = Convert.ToDouble(textBox1.Text);
                //double limit = Convert.ToDouble(textBox7.Text);
                //double total = limit - amt;
                //MessageBox.Show("Amount is " + total);

               //update balance in Customers bank (-)
               
                string s = "select Balance from Bank where ID='" + aa1 + "' and Type='Customer'";
                 SqlDataAdapter da = new SqlDataAdapter(s, con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double i = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        double t = i - amt;
                        string s1 = "Update Bank set Balance='" + t + "' where ID='" + aa1 + "' and Type='Customer'";
                        SqlCommand cmd = new SqlCommand(s1, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }

                    //update balance in Merchants bank (+) 
                     string u = MerchantLogin.a1;
                     string b = "select MId,Name from Merchant where Email='"+u+"'";
                     SqlDataAdapter sd = new SqlDataAdapter(b,con);
                     DataSet dd = new DataSet();
                     sd.Fill(dd);
                      if(dd.Tables[0].Rows.Count>0)
                         {
                             string sss = dd.Tables[0].Rows[0][0].ToString();
                             string na = dd.Tables[0].Rows[0][1].ToString();
                     string w = "select Balance from Bank where ID='" + sss + "' and Type='Merchant'";
                    SqlDataAdapter da1 = new SqlDataAdapter(w, con);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        double i = Convert.ToDouble(ds1.Tables[0].Rows[0][0].ToString());
                        double t1 = i + amt;
                       string m = "Update Bank set Balance='" + t1 + "' where ID='" + sss + "' and Type='Merchant' ";
                            SqlCommand cmd1 = new SqlCommand(m, con);
                          con.Open();
                          cmd1.ExecuteNonQuery();
                          con.Close();

                          string trans = "insert into Trans(TId,UserId,UserName,MerId,MerchantName,Amt,Date,Note) values('"+label7.Text+"','"+aa1+"','"+textBox6.Text+"','"+sss+"','"+na+"','"+textBox1.Text+"','"+System.DateTime.Now.ToString("yyyy-MM-dd")+"','"+textBox2.Text+"')";
                          SqlCommand cm = new SqlCommand(trans,con);
                          con.Open();
                          cm.ExecuteNonQuery();
                          MessageBox.Show("Insertion Sucessfully done in trans table");
                          con.Close();
                          panel1.Visible = false;
                          textBox1.Text = "";
                          textBox2.Text = "";
                          pictureBox2.Image = null;
                    }
                  }

            }
            else
            {
                MessageBox.Show("Sorry!!!Amount Should be less than or eqauls to Your Limit!!!");
                //this.Hide();
                //Merchantmain a = new Merchantmain();
                //a.Show();
            }
        }

        public void Tid()
        {
            string str = "select top 1 TId from Trans order by TId desc";
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string i = ds.Tables[0].Rows[0][0].ToString();

                int a = Convert.ToInt32(i);
                a++;
                label7.Text = a.ToString();
            }
            else
            {
                label7.Text = "50001";
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}