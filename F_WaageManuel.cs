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
    public partial class F_WaageManuel : Form
    {
        TextBox aktifnesne = null;
        public double adet=0;
        public double fiyat = 0;
        public bool sonuc = false;
        public F_WaageManuel()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
            aktifnesne = textBox1;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.ActiveControl = textBox2;
            aktifnesne = textBox2;
        }

        private void btnUc_Click(object sender, EventArgs e)
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

        private void btnNokta_Click(object sender, EventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (aktifnesne != null)
            {
                aktifnesne.Text = "";
            }
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                double yeniadet = 0;
                if (double.TryParse(textBox2.Text, out fiyat))
                {
                    if(double.TryParse(textBox1.Text, out adet))
                    {
                        int uzu = textBox1.Text.Length;
                        if (uzu < 3)
                        {
                            while (textBox1.Text.Length < 4)
                            {
                                textBox1.Text = "0" + textBox1.Text;
                            }
                        }
                        string virgullu = textBox1.Text.Substring(textBox1.Text.Length - 3, 3);
                        string tam = textBox1.Text.Substring(0, textBox1.Text.Length - 3);
                        string fiyatvirgullu = tam + "," + virgullu;
                        if (double.TryParse(fiyatvirgullu, out yeniadet))
                        {
                            adet = yeniadet;
                            sonuc = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["40"];
                    frmerror.ShowDialog();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["43"]; ;
                    frmerror.ShowDialog();

                }
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["42"]; ;
                frmerror.ShowDialog();
            }

            
        }

        private void F_WaageManuel_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            aktifnesne = textBox1;
        }

        private void btnNokta_Click_1(object sender, EventArgs e)
        {
            textBox2.Text += ",";
        }

        private void kryptonPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
