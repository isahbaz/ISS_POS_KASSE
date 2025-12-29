using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace IS_KASSE
{
    public partial class F_KundenDisplay_Top : Form
    {
        //public delegate void KundenDisplayFormDelegate(string Sonsatilan, string Total);
        //public event KundenDisplayFormDelegate KundenDisplayText;
        public F_KundenDisplay_Top()
        {
            InitializeComponent();
            if (File.Exists(Application.StartupPath + "\\LogoKD.jpg"))
            {
                Image img = Image.FromFile(Application.StartupPath + "\\LogoKD.jpg");
                panel2.BackgroundImage = img;
            }
        }

        private void F_KundenDisplay_Top_Load(object sender, EventArgs e)
        {
            lblTotal.Text = Program.IsletmeAyarlar["isletme"];
            

        }
        public void VerkaufInfo(string Lastid, String Total)
        {
            try
            {
                if (Lastid != "\n x ")
                {
                    Application.DoEvents();
                    CheckForIllegalCrossThreadCalls = false;
                    lblLast.Text = Lastid;
                    lblTotal.Text = Total;
                }
                else
                {
                    Application.DoEvents();
                    CheckForIllegalCrossThreadCalls = false;
                    lblLast.Text = Total;
                    lblTotal.Text = "";
                }
            }
            catch
            {
            }
        }

        private void lblLast_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
