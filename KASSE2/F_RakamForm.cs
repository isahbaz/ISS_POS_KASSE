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
    public partial class F_RakamForm : Form
    {
        TextBox aktifnesne = null;
        public string BelegNo = "";
        public bool islem = false;
        public F_RakamForm()
        {
            InitializeComponent();
            label1.Text = "Bitte geben Sie ein Beleg-Nr ein:";
        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            ComponentFactory.Krypton.Toolkit.KryptonButton btn = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;

            int sayi;
            if (int.TryParse(btn.Text, out sayi))
            {
                textBox1.Text += sayi.ToString();
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            int belno = 0;
            if (int.TryParse(textBox1.Text, out belno))
            {
            BelegNo = textBox1.Text;
            islem = true;
            this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            islem = false;
            this.Close();
        }
    }
}
