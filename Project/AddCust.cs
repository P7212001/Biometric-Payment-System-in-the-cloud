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
using System.Net.Mail;
using System.Net;

namespace Finger_ATM
{
    public partial class AddCust : Form
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

        public AddCust()
        {
            InitializeComponent();
        }
        public static string aa = ""; 
        private void AddCust_Load(object sender, EventArgs e)
        {
            m_LedOn = false;
            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            m_FPM = new SGFingerPrintManager();
            EnumerateBtn_Click(sender, e);
            Cid();
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

        public void Cid()
        {
            string str = "select top 1 CId from Customer order by CId desc";
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string i = ds.Tables[0].Rows[0][0].ToString();

                int a = Convert.ToInt32(i);
                a++;
                label20.Text = a.ToString();
            }
            else
            {
                label20.Text = "10001";
            }
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

                    button1.Enabled = true;
                }
                else
                    DisplayError("OpenDevice()", iError);
            }
            catch (Exception ep)
            {
                button1.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 iError;
            
            Int32 img_qlty;

            fp_image = new Byte[m_ImageWidth * m_ImageHeight];
            img_qlty = 0;

            iError = m_FPM.GetImage(fp_image);

            m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);


            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                DrawImage(fp_image, pictureBox1);
                iError = m_FPM.CreateTemplate(fp_image, m_RegMin1);

                if (iError == (Int32)SGFPMError.ERROR_NONE) 
                {
                    pictureBox1.Image.Save(@"Finger\" + label20.Text + ".jpg");
                    DialogResult d = MessageBox.Show("Finger Captured Successfully", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (d == DialogResult.OK)
                    {

                    }
                }
               
                else
                    DisplayError("CreateTemplate()", iError);
            }
            else
            {
                MessageBox.Show("Finger Capturing Failed", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            m_LedOn = false;
            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            m_FPM = new SGFingerPrintManager();
            EnumerateBtn_Click(sender, e);
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


                if (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                   // DrawImage(fp_image, pictureBox2);
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
                }
                if (id != "")
                {
                    MessageBox.Show(id);//
                }
                else
                {
                    MessageBox.Show("Not Matched");
                }
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && textBox9.Text != "" && pictureBox1.Image != null)
            {
                string email = "Select Email from Customer where Email='" + textBox4.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter(email, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {



                    string s = "insert into Customer(CId,Name,Phone,Email,Address,City,Limit,Finger,Password) values('" + label20.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "',@Finger,'" + textBox9.Text + "')";
                    aa = label20.Text;
                    SqlCommand cmd = new SqlCommand(s, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Finger", fp_image);
                    cmd.ExecuteNonQuery();
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];
                  
                    con.Close();

                    //SqlCommand cmd = new SqlCommand("Insert into Customer (Finger) Values (@Finger)", con);
                    //con.Open();

                    //cmd.ExecuteNonQuery();
                    //con.Close();
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
                    pictureBox1.Image = null;
                    textBox9.Text = "";

                }

            }
            else
            {
                MessageBox.Show("Fill All information");
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            if ( textBox16.Text != "" && textBox15.Text != "" && textBox14.Text != "" && textBox13.Text != "" && textBox12.Text != "" && textBox11.Text != "" && textBox10.Text != "")
            {
                string u = AddCust.aa;
                string s = "insert into Bank(BId,Type,ID,Bank,AccNo,Branch,IFSC,CardNo,CVV,Month,Year,Balance) values('" + label23.Text + "','Customer','" + u + "','" + textBox16.Text + "','" + textBox15.Text + "','" + textBox14.Text + "','" + textBox13.Text + "','" + textBox12.Text + "','" + textBox10.Text + "','" + textBox1.Text + "','"+textBox8.Text+"','" + textBox11.Text + "')";
                SqlCommand cmd = new SqlCommand(s, con);
                con.Open();
                cmd.ExecuteNonQuery();
              //  MessageBox.Show("Insertion Sucessfully");
                this.Hide();
                Admin ad = new Admin();
                ad.Show();
                eemail(textBox4.Text,textBox9.Text);

                con.Close();
            }
            else
            {
                MessageBox.Show("Please fill Proper Info");
            }
        }

        private void label10_Click(object sender, EventArgs e)
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

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            string u = AddCust.aa;

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

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

            }
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            {


            }
            else
            {
                MessageBox.Show("You Can Only Enter A Number!");
                textBox15.Text = "";
                textBox15.Focus();
                e.Handled = true;
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true)
            {
                e.Handled = true;
            }
            else
            {

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
            //char keypress = e.KeyChar;
            //if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back))
            //{


            //}
            //else
            //{
            //    MessageBox.Show("You Can Only Enter A Number!");
            //    e.Handled = true;
            //}
           // if (char.IsDigit(e.KeyChar))
            //{
                //Count the digits already in the text.  I'm using linq:
               // if ((sender as TextBox).Text.Count(Char.IsDigit) >= 12)
                    //e.Handled = true;
            //}
           /* if (Convert.ToInt32(textBox1.Text) < 1 && Convert.ToInt32(textBox1.Text)>12)
            {
                MessageBox.Show("Enter Month Betweeen 1 to 12");
            }*/
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                int mon = Convert.ToInt32(textBox1.Text);
                if (mon > 12)
                {
                    MessageBox.Show("Please enter valid month.");
                    textBox1.Text = "";
                    textBox1.Focus();
                    // textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                }
            }
        }

        private void textBox12_Validating(object sender, CancelEventArgs e)
        {
            if (textBox12.Text.Length == 16)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox12.Text, "[^[0-9]{15}]"))
                {

                    MessageBox.Show("Please enter valid month.");
                    textBox12.Text = "";
                    textBox12.Focus();
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
                textBox12.Text = "";
                textBox12.Focus();
                   
            }
        }

        private void textBox10_Validating(object sender, CancelEventArgs e)
        {
            if (textBox10.Text.Length == 3)
            {
            }
            else
            {
                MessageBox.Show("Exactly 3 Digits Only");
                textBox10.Text = "";
                textBox10.Focus();
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            




































































































































































































































































        }
    }
}
