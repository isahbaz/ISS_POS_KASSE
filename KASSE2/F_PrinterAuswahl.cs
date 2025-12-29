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
    public partial class F_PrinterAuswahl : Form
    {
        public int sonuc = 0;
        public F_PrinterAuswahl()
        {
            InitializeComponent();
        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            sonuc = 1;
            this.Close();
        }

        private void btnBon_Click(object sender, EventArgs e)
        {
            sonuc = 0;
            this.Close();
        }
    }
}
