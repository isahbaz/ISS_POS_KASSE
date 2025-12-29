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
   
    public partial class F_TSEexport : Form
    {
        public DateTime startDate;
        public DateTime endDate;
        public int sonuc = -1; //0 Iptal, 1: Voll, 2: Selected Date
        public F_TSEexport()
        {
            InitializeComponent();
        }

        private void F_TSEexport_Load(object sender, EventArgs e)
        {

        }

        private void btnVoll_Click(object sender, EventArgs e)
        {
            sonuc = 1;
            this.Close();
        }

        private void btnDatum_Click(object sender, EventArgs e)
        {
            sonuc = 2;
            startDate = dateTimePicker1.Value;
            endDate = dateTimePicker2.Value;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            sonuc = 0;
            this.Close();
        }
    }
}
