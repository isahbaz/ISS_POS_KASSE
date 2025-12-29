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
    public partial class F_HandyAufladeDR : Form
    {
        TextBox aktifnesne = null;
        public string code = "";
        public delegate void TelNrEingabeDelegate(string Tel, int sira);
        public event TelNrEingabeDelegate TelNrEvent;
        public F_HandyAufladeDR()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            if (aktifnesne != null)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btn = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;

                int sayi;
                if (int.TryParse(btn.Text, out sayi))
                {
                    aktifnesne.Text += sayi.ToString();
                }
            }
        }
        string tel1 = "", tel2 = "";
        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            code = textBox1.Text;


            // this.TelNrEvent(code);
            if (tel1 == "")
            {
                tel1 = code;
                textBox1.Text = "";
                this.TelNrEvent(code, 1);
            }
            else if (tel1 != "")
            {
                tel2 = code;
                this.TelNrEvent(code, 2);
                if (tel1 == tel2)
                {
                    btnVerkauf.Enabled = true;
                }
            }
        }

        private void F_HandyAufladeDR_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            this.ActiveControl = textBox1;
            textBox1.Focus();
            aktifnesne = textBox1;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            code = "";
            this.TelNrEvent(code, 3);
            this.Close();
        }

        private void btnVerkauf_Click(object sender, EventArgs e)
        {
            this.TelNrEvent(code, 3);
            this.Close();
        }
    }
}
