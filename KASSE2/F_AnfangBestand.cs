using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace IS_KASSE
{
    public partial class F_AnfangBestand : Form
    {
        public double verilenpara = 0;
        public F_AnfangBestand()
        {
            InitializeComponent();
            if (Program.GlobalAyarlar["BKOMMA"] == 0)
            {
                pnlPunkt.Visible = true;
            }
            else
            {
                pnlPunkt.Visible = false;
            }
        }

        private void btnIki_Click(object sender, EventArgs e)
        {

        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            verilenpara = 0;
            if (txtOdenen.Text == "0")
            {
                txtOdenen.Text = "";
            }
            KryptonButton btn = sender as KryptonButton;
            txtOdenen.Text += btn.Text;

            if (double.TryParse(txtOdenen.Text, out verilenpara))
            {
                if(Program.GlobalAyarlar["BKOMMA"] == 0)
                {
                    verilenpara = (verilenpara / 100);
                    
                }
                else
                {
                    
                }

            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            txtOdenen.Text = "0";
            
            verilenpara = 0;
        }

        private void btnSheck_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
