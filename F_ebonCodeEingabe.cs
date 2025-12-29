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
    public partial class F_ebonCodeEingabe : Form
    {
        TextBox aktifnesne = null;
        public string code = "";
        public F_ebonCodeEingabe()
        {
            InitializeComponent();
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

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            code = textBox1.Text;
            this.Close();

        }

        private void F_ebonCodeEingabe_Load(object sender, EventArgs e)
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
    }
}
