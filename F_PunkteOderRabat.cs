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
    public partial class F_PunkteOderRabat : Form
    {
        public int sonuc = -1;
        public F_PunkteOderRabat()
        {
            InitializeComponent();
        }

        private void btnEvet_Click(object sender, EventArgs e)
        {
            sonuc = 1;
            this.Close();
        }

        private void btnHayir_Click(object sender, EventArgs e)
        {
            sonuc = 2;
            this.Close();
        }
    }
}
