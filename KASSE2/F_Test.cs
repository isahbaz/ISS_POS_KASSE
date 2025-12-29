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
    public partial class F_Test : Form
    {
        public F_Test()
        {
            InitializeComponent();
        }

        private void F_Test_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            TischAktuellBon tt = new TischAktuellBon();
            Panel pp = tt.panelGetir()[0];
            pp.Visible = true;
            
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBar_Click(object sender, EventArgs e)
        {

        }

        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnUmbuchung_Click(object sender, EventArgs e)
        {

        }

        private void btnMasa_Click(object sender, EventArgs e)
        {

        }

     }
}
