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
    public partial class F_FeinKostBedinerAuswahl : Form
    {
        public List<User> UserList;
        public long SecilenUser = 0;
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        public SatisYap satilanPozition = null;
        public F_FeinKostBedinerAuswahl()
        {
            InitializeComponent();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
            SecilenUser = Convert.ToInt32(btnsecilen.Tag);
            Close();
        }

        private void F_FeinKostBedinerAuswahl_Load(object sender, EventArgs e)
        {
            if (satilanPozition != null)
            {
                lblMenge.Text = satilanPozition.Adet.ToString();
                lblName.Text = satilanPozition.UrunAd;
                lblSumme.Text = satilanPozition.Toplamtutar.ToString();
            }
            else
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                lblMenge.Visible = false;
                lblName.Visible = false;
                lblSumme.Visible = false;
                if (UserList.Count >= 7)
                {
                    this.Size = new Size(980, 280);


                }
                else
                {
                    this.Size = new Size((140 * (UserList.Count + 1) + 50), 150);

                }
            }
            if (UserList.Count > 0)
            {
                if (UserList.Count >= 7)
                {
                     flp1.Size = new Size(980, 280);
                    
                    
                }
                else
                {
                     flp1.Size = new Size((140 * (UserList.Count+1)+50), 150);

                }
                for (int i = 0; i < UserList.Count; i++)
                {
                    ComponentFactory.Krypton.Toolkit.KryptonButton btnBon1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                    //btnBon1.Location = new System.Drawing.Point(224, 55);
                    btnBon1.Name = "btnBon1";
                    btnBon1.Size = new System.Drawing.Size(140, 140);
                    btnBon1.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255-i*2)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                    btnBon1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    btnBon1.StateNormal.Border.Rounding = 2;
                    btnBon1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnBon1.TabIndex = 3;
                    btnBon1.Values.Text = UserList[i].AdSoyad;
                    btnBon1.Tag = UserList[i].Userid; //dtArtikel.Rows[i].ItemArray[8];
                    flp1.Controls.Add(btnBon1);
                    btnBon1.Click += new System.EventHandler(btnUser_Click);
                }
                
                this.kryptonButton4.Location = new System.Drawing.Point(2, 2);
                this.kryptonButton4.Name = "kryptonButton4";
                this.kryptonButton4.Size = new System.Drawing.Size(140, 140);
                this.kryptonButton4.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
                this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                this.kryptonButton4.TabIndex = 51;
                this.kryptonButton4.Values.Text = "ABBRUCH";
                this.kryptonButton4.Tag = -1;
                this.kryptonButton4.Click += new System.EventHandler(this.btnUser_Click);
                flp1.Controls.Add(kryptonButton4);
            }
        }

        private void flp1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
