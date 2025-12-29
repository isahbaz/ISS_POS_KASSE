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
    public partial class F_HandAufladeDAOnay : Form
    {
        public string nr1 = "", nr2 = "";
        public F_HandAufladeDAOnay()
        {
            InitializeComponent();
        }

        private void F_HandAufladeDAOnay_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
            /**/
            this.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
            int formWidth = this.Width;
            int formHeight = this.Height;
            int screen1W = Screen.PrimaryScreen.WorkingArea.Width;
            int screen2W = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Width;

            int screen2H = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Height;


            int top = screen1W + (screen2W - formWidth) / 2;
            int left = (screen2H - formHeight) / 2;
            this.Location = new Point(top, left);
            lblNr1.Text = nr1;
            lblNr2.Text = nr2;
        }
        public void TelNrYaz(string TelNr, int Sira)
        {
            //textBox1.Text = TelNr;
            CheckForIllegalCrossThreadCalls = false;
            if (Sira == 1)
            {
                lblNr1.Text = TelNr;
            }
            else if (Sira == 2)
            {
                lblNr2.Text = TelNr;
            }
        }
    }
}
