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
    public partial class F_Preisauswahl : Form
    {
        public double Preis1, Preis2, Preis3 = 0, seilenPreis=0;
        public string ArtikelName = "";
        public F_Preisauswahl()
        {
            InitializeComponent();
        }

        private void F_Preisauswahl_Load(object sender, EventArgs e)
        {
            if (Preis1 > 0)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnBon1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                btnBon1.Location = new System.Drawing.Point(224, 55);
                btnBon1.Name = "btnBon1";
                btnBon1.Size = new System.Drawing.Size(200, 263);
                btnBon1.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                btnBon1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                btnBon1.StateNormal.Border.Rounding = 2;
                btnBon1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBon1.TabIndex = 3;
                btnBon1.Values.Text = ArtikelName+"\n"+Preis1.ToString("C");
                btnBon1.Tag = Preis1; //dtArtikel.Rows[i].ItemArray[8];
                flp1.Controls.Add(btnBon1);
                btnBon1.Click += new System.EventHandler(btnBon_Click);
            }
            if (Preis2 > 0)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnBon = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                btnBon.Location = new System.Drawing.Point(224, 55);
                btnBon.Name = "btnBon";
                btnBon.Size = new System.Drawing.Size(200, 263);
                btnBon.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                btnBon.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                btnBon.StateNormal.Border.Rounding = 2;
                btnBon.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBon.TabIndex = 3;
                btnBon.Values.Text = ArtikelName + "\n" + Preis2.ToString("C");
                btnBon.Tag = Preis2; //dtArtikel.Rows[i].ItemArray[8];
                flp1.Controls.Add(btnBon);
                btnBon.Click += new System.EventHandler(btnBon_Click);
            }
            if (Preis3 > 0)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnBon2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                btnBon2.Location = new System.Drawing.Point(224, 55);
                btnBon2.Name = "btnBon2";
                btnBon2.Size = new System.Drawing.Size(200, 263);
                btnBon2.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                btnBon2.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                btnBon2.StateNormal.Border.Rounding = 2;
                btnBon2.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBon2.TabIndex = 3;
                btnBon2.Values.Text = ArtikelName + "\n" + Preis3.ToString("C");
                btnBon2.Tag = Preis3; //dtArtikel.Rows[i].ItemArray[8];
                flp1.Controls.Add(btnBon2);
                btnBon2.Click += new System.EventHandler(btnBon_Click);
            }
            // 
        }

        private void btnBon_Click(object sender, EventArgs e)
        {
            ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
            seilenPreis = Convert.ToDouble(btnsecilen.Tag);
            Close();
        }
    }
}
