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
    public partial class F_AbmeldungNeue : Form
    {
        public Int32 lasBedId = -1;
        int time = 10;
       
        public F_AbmeldungNeue()
        {
            InitializeComponent();
        }

        private void F_AbmeldungNeue_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void btnAbmelden_Click(object sender, EventArgs e)
        {
                lasBedId = -1;

            this.Close();
           
           
        }

        private void F_AbmeldungNeue_FormClosed(object sender, FormClosedEventArgs e)
        {
             
           
        }

        private void btnAbbruch_Click(object sender, EventArgs e)
        {
            lasBedId = 0;
            this.Close();
            
        }

        private void btnPausieren_Click(object sender, EventArgs e)
        {
            this.Close();

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = time.ToString();
            time--;
            if (time == 0)
            {
                lasBedId = -1;
                timer1.Enabled = false;
                this.Close();
            }
        }
    }
}
