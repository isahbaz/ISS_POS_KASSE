using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iss_tse_v2;

namespace IS_KASSE
{
    public partial class F_Splash : Form
    {
        int pbarValue = 100;
      
        public F_Splash()
        {
            InitializeComponent();
            lblID.Text = Program.ept.ReadSWID();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            kryptonLabel2.Text= "Version: "+fvi.FileVersion;
            //WormStore ws= new WormStore();
            lblTseVers.Text ="Swissbit-"+ WormStore.version();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            progressBar1.Increment(40);
            while (Program.connection == false)
            {
                pbarValue = pbarValue + 20;
            }
            if (progressBar1.Maximum == pbarValue)
            {
                timer1.Stop();
                this.Close();
            }
        }
    }
}
