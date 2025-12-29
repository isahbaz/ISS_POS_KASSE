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
    public partial class F_GenericSoru : Form
    {
        public bool sonuc = false;
        public F_GenericSoru()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            sonuc = true;
            this.Close();
        }

        private void btnHayir_Click(object sender, EventArgs e)
        {
            sonuc = false;
            this.Close();
        }
    }
}
