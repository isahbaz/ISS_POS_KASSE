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
    public partial class F_Kiosk_Zahlung : Form
    {
        public int odemeSekli = 0;
        public F_Kiosk_Zahlung()
        {
            InitializeComponent();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {

        }

        private void btnAuserhaus_Click(object sender, EventArgs e)
        {
            odemeSekli = 1;
            this.Close();
        }

        private void btnImhaus_Click(object sender, EventArgs e)
        {
            odemeSekli = 2;
            this.Close();
        }

        private void btnAbbrechen_Click(object sender, EventArgs e)
        {
            odemeSekli = 0;
            this.Close();
        }
    }
}
