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
    public partial class F_PreisCheck : Form
    {
        public string ArtikelName = "", Preis="";
        
        public F_PreisCheck()
        {
            InitializeComponent();
        }

        private void F_PreisCheck_Load(object sender, EventArgs e)
        {
            label1.Text = ArtikelName;
            label2.Text = Preis;
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
