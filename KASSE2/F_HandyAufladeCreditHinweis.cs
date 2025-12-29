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
    public partial class F_HandyAufladeCreditHinweis : Form
    {
        public string guthaben ="";
        public F_HandyAufladeCreditHinweis()
        {
            InitializeComponent();
        }

        private void F_HandyAufladeCreditHinweis_Load(object sender, EventArgs e)
        {
            label1.Text += " " + guthaben;
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
