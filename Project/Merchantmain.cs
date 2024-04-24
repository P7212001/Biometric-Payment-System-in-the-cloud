using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finger_ATM
{
    public partial class Merchantmain : Form
    {
        public Merchantmain()
        {
            InitializeComponent();
        }

        private void tranactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTransactionMerchant m = new NewTransactionMerchant();
            m.MdiParent = this;
            m.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //HomePage obj = new HomePage();
            //obj.MdiParent = this;
            //obj.Show();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MerchantProfile m = new MerchantProfile();
            m.MdiParent = this;
            m.Show();
        }

        private void merchantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MerchantTransaction m = new MerchantTransaction();
            m.MdiParent = this;
            m.Show();
    

        }
    }
}
