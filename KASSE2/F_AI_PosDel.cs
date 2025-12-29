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
    public partial class F_AI_PosDel : Form
    {
        public String ArtikelName = "";
        public int Antwort = 0;
        public F_AI_PosDel()
        {
            InitializeComponent();
        }

        private void F_AI_PosDel_Load(object sender, EventArgs e)
        {
            lblProdukt.Text += ArtikelName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Antwort = 1;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Antwort = 2;
            this.Close();
        }
    }
}
