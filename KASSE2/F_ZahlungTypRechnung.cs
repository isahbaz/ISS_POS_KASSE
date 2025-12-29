using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iss_Rea;

namespace IS_KASSE
{
    public partial class F_ZahlungTypRechnung : Form
    {
        public int Zahlungstyp = 1;
        public isstoRea Rea;
        public F_ZahlungTypRechnung()
        {
            InitializeComponent();
        }

        private void F_ZahlungTypRechnung_Load(object sender, EventArgs e)
        {

        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            Zahlungstyp = 1;
            this.Close();
        }

        private void btnBon_Click(object sender, EventArgs e)
        {
            Zahlungstyp = 0;
            this.Close();
        }
    }
}
