using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace IS_KASSE
{
    public partial class F_GrossSummeBesteatigung : Form
    {
        public double summe = 0;
        public int bestatigung = -1;
        public F_GrossSummeBesteatigung()
        {
            InitializeComponent();
        }

        private void btnBon_Click(object sender, EventArgs e)
        {
            bestatigung = 1;
            this.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void F_GrossSummeBesteatigung_Load(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
            label1.Text += "\n"+summe.ToString("C");
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
