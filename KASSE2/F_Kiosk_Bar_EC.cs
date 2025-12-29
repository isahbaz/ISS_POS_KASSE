using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_Kiosk_Bar_EC : Form
    {
        public int odemeSekliBarEc=0;
        public F_Kiosk_Bar_EC()
        {
            InitializeComponent();
        }

        private void btnBar_Click(object sender, EventArgs e)
        {
            odemeSekliBarEc = 1;
            this.Close();
        }

        private void btnEC_Click(object sender, EventArgs e)
        {
            odemeSekliBarEc = 2;
            this.Close();
        }

        private void btnAbbrechen_Click(object sender, EventArgs e)
        {
            odemeSekliBarEc = 0;
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            odemeSekliBarEc = 3;
            this.Close();
        }
    }
}
