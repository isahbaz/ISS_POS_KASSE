using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxWMPLib;

namespace IS_KASSE
{
    public partial class F_KundenDisplay : Form
    {
        public delegate void KundenDisplayFormDelegate(string Sonsatilan, string Total);
        public event KundenDisplayFormDelegate KundenDisplayText;
        

        public F_KundenDisplay()
        {
            InitializeComponent();
            //axWindowsMediaPlayer1.URL =  @"http://go.microsoft.com/fwlink/?LinkId=95772";
        }

        private void F_KundenDisplay_Load(object sender, EventArgs e)
        {
            lblTotal.Text = Program.IsletmeAyarlar["isletme"];
        }
        public void VerkaufInfo(string Lastid, String Total)
        {
            Application.DoEvents();
            CheckForIllegalCrossThreadCalls = false;
            lblLast.Text =  Lastid;
            lblTotal.Text = Total;
        }
    }
}
