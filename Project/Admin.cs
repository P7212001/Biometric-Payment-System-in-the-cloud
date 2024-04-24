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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void createProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCust obj = new AddCust();
            obj.MdiParent = this;
            obj.Show();
        }

        private void createBankProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
         }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //HomePage obj = new HomePage();
            //obj.MdiParent = this;
            //obj.Show();
        }

        private void manageProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageProfile obj = new ManageProfile();
            obj.MdiParent = this;
            obj.Show();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMerchant m = new AddMerchant();
           m.MdiParent = this;
            m.Show();
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageMerchant m = new ManageMerchant();
            m.MdiParent = this;
            m.Show();
        }

        private void tranactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminTransaction m = new AdminTransaction();
            m.MdiParent = this;
            m.Show();

        }
    }
}
