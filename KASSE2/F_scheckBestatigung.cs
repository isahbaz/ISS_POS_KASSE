using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_scheckBestatigung : Form
    {
        public int onay=-1;
        public F_scheckBestatigung()
        {
            InitializeComponent();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            onay = -1;
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            F_AusweisPruf ausweis = new F_AusweisPruf();
            ausweis.ShowDialog();
        }

        private void F_scheckBestatigung_Load(object sender, EventArgs e)
        {

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            onay = 1;
            this.Close();
        }
    }
}
