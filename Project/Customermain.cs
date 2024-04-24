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
    public partial class Customermain : Form
    {
        public Customermain()
        {
            InitializeComponent();
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
            CustomerProfile m = new CustomerProfile();
            m.MdiParent = this;
            m.Show();

        }

        private void merchantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerTransaction m = new CustomerTransaction();
            m.MdiParent = this;
            m.Show();

        }
    }
}
