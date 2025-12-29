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
    public partial class F_GenericError : Form
    {
        public F_GenericError()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonButton14_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("TeamViewerQS.exe");
            }
            catch
            {
            }
        }
    }
}
