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
    public partial class F_Kiosk_Bearbeiten : Form
    {
        public FisOlustur yeniFis = null;
        public double summe = 0;
        public F_Kiosk_Bearbeiten()
        {
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
            InitializeComponent();
           
        }

        private void F_Kiosk_Bearbeiten_Load(object sender, EventArgs e)
        {
            lblToplam.Text = summe.ToString("C");
            try
            {

                if (yeniFis != null)
                {
                    if (yeniFis.SatisKalem.Count > 0)
                    {
                        for (int i = 0; i < yeniFis.SatisKalem.Count; i++)
                        {
                            pnlArtMain = new System.Windows.Forms.Panel();
                            kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
                            txtMenge = new System.Windows.Forms.TextBox();
                            btnDel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                            btnMinus = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                            btnPlus = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                            lblName = new System.Windows.Forms.Label();
                            pnlBild = new System.Windows.Forms.Panel();
                            ComponentFactory.Krypton.Toolkit.KryptonButton btnPreis = new ComponentFactory.Krypton.Toolkit.KryptonButton();

                            SatisYap satispos = new SatisYap();
                            satispos = yeniFis.SatisKalem[i];

                            // 
                            // pnlArtMain
                            // 
                            pnlArtMain.Controls.Add(kryptonGroup1);
                            pnlArtMain.Location = new System.Drawing.Point(18, 18);
                            pnlArtMain.Name = "pnlArtMain";
                            pnlArtMain.Size = new System.Drawing.Size(763, 272);
                            pnlArtMain.TabIndex = 10;
                            // 
                            // kryptonGroup1
                            // 
                            kryptonGroup1.Location = new System.Drawing.Point(89, 15);
                            kryptonGroup1.Margin = new System.Windows.Forms.Padding(15);
                            kryptonGroup1.Name = "kryptonGroup1";
                            // 
                            // kryptonGroup1.Panel
                            // 
                            kryptonGroup1.Panel.Controls.Add(txtMenge);
                            kryptonGroup1.Panel.Controls.Add(btnDel);
                            kryptonGroup1.Panel.Controls.Add(btnMinus);
                            kryptonGroup1.Panel.Controls.Add(btnPlus);
                            kryptonGroup1.Panel.Controls.Add(lblName);
                            kryptonGroup1.Panel.Controls.Add(pnlBild);
                            kryptonGroup1.Panel.Controls.Add(btnPreis);
                            kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(5);
                            kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
                            kryptonGroup1.Size = new System.Drawing.Size(609, 236);
                            kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            kryptonGroup1.StateCommon.Border.Rounding = 3;
                            kryptonGroup1.StateNormal.Border.Color1 = System.Drawing.Color.Chocolate;
                            kryptonGroup1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            kryptonGroup1.StateNormal.Border.Rounding = 5;
                            kryptonGroup1.StateNormal.Border.Width = 1;
                            kryptonGroup1.TabIndex = 6;
                            // 
                            // txtMenge
                            // 
                            txtMenge.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            txtMenge.Location = new System.Drawing.Point(238, 183);
                            txtMenge.Multiline = true;
                            txtMenge.Name = "txtMenge";
                            txtMenge.Size = new System.Drawing.Size(74, 41);
                            txtMenge.TabIndex = 8;
                            txtMenge.Text = satispos.Adet.ToString(); ;
                            txtMenge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                            // 
                            // btnPreis
                            // 

                            btnPreis.Location = new System.Drawing.Point(477, 100);
                            btnPreis.Name = "btnPreis";
                            btnPreis.Size = new System.Drawing.Size(61, 29);
                            btnPreis.StateCommon.Back.Color1 = System.Drawing.Color.White;
                            btnPreis.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                            btnPreis.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                            btnPreis.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            btnPreis.StateCommon.Border.Rounding = 4;
                            btnPreis.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            btnPreis.TabIndex = 4;
                            btnPreis.Values.Text = satispos.Satisfiyat.ToString("C");
                            // 
                            // btnDel
                            // 
                            btnDel.Location = new System.Drawing.Point(477, 6);
                            btnDel.Name = "btnDel";
                            btnDel.Size = new System.Drawing.Size(74, 70);
                            btnDel.StateCommon.Back.Color1 = System.Drawing.Color.Snow;
                            btnDel.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                            btnDel.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.garbage_bin_10420_1_;
                            btnDel.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                            btnDel.StateCommon.Border.Color1 = System.Drawing.Color.Crimson;
                            btnDel.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            btnDel.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
                            btnDel.StateCommon.Border.Rounding = 4;
                            btnDel.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                            btnDel.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            btnDel.TabIndex = 7;
                            btnDel.Tag = satispos;
                            btnDel.Values.Text = "-";
                            btnDel.Click += new System.EventHandler(this.PosSil);
                            // 
                            // btnMinus
                            // 
                            btnMinus.Location = new System.Drawing.Point(153, 183);
                            btnMinus.Name = "btnMinus";
                            btnMinus.Size = new System.Drawing.Size(74, 41);
                            btnMinus.StateCommon.Back.Color1 = System.Drawing.Color.Orange;
                            btnMinus.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                            btnMinus.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.minus_circle_red_symbol_22248;
                            btnMinus.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
                            btnMinus.StateCommon.Border.Color1 = System.Drawing.Color.Crimson;
                            btnMinus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            btnMinus.StateCommon.Border.Rounding = 4;
                            btnMinus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                            btnMinus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            btnMinus.Tag = satispos;
                            btnMinus.TabIndex = 6;
                            btnMinus.Values.Text = "-";
                            btnMinus.Click += new System.EventHandler(this.SatisAzalt);
                            // 
                            // btnPlus
                            // 
                            btnPlus.Location = new System.Drawing.Point(321, 184);
                            btnPlus.Name = "btnPlus";
                            btnPlus.Size = new System.Drawing.Size(74, 40);
                            btnPlus.StateCommon.Back.Color1 = System.Drawing.Color.YellowGreen;
                            btnPlus.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                            btnPlus.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.green_add_button_12011;
                            btnPlus.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
                            btnPlus.StateCommon.Border.Color1 = System.Drawing.Color.DarkGreen;
                            btnPlus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            btnPlus.StateCommon.Border.Rounding = 4;
                            btnPlus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                            btnPlus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            btnPlus.TabIndex = 5;
                            btnPlus.Tag = satispos;
                            btnPlus.Values.Text = "";
                            btnPlus.Click += new System.EventHandler(this.SatisEkle);
                            // 
                            // lblName
                            // 
                            lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                            lblName.Font = new System.Drawing.Font("Constantia", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            lblName.Location = new System.Drawing.Point(6, 130);
                            lblName.Name = "lblName";
                            lblName.Size = new System.Drawing.Size(591, 37);
                            lblName.TabIndex = 1;
                            lblName.Text = satispos.UrunAd;
                            lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            // 
                            // pnlBild
                            // 
                            pnlBild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                            pnlBild.BackgroundImage = global::IS_KASSE.Properties.Resources.classic_burger;
                            pnlBild.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                            pnlBild.Location = new System.Drawing.Point(189, 6);
                            pnlBild.Name = "pnlBild";
                            pnlBild.Size = new System.Drawing.Size(162, 121);
                            pnlBild.TabIndex = 0;
                            flpTop.Controls.Add(pnlArtMain);
                        }
                    }
                }
            }
            catch (Exception dd)
            {

            }
        }
        private void SatisEkle(object sender, EventArgs e)
        {
            try
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                btnsecilen.StateNormal.Border.Width = 5;
                Control cntParent = btnsecilen.Parent;
                SatisYap satisYap = (SatisYap)btnsecilen.Tag;
                satisYap.Adet = satisYap.Adet + 1;
                satisYap.Toplamtutar = satisYap.Satisfiyat * satisYap.Adet;
                summe = summe + (satisYap.Satisfiyat);
                lblToplam.Text = (summe).ToString("C");
                btnsecilen.Tag = satisYap;
                foreach (Control cnt in cntParent.Controls)
                {
                    if (cnt.Name == "btnPlus")
                    {
                        cnt.Tag = satisYap;
                    }
                    if (cnt.Name == "txtMenge")
                    {
                        cnt.Text = satisYap.Adet.ToString(); ;
                    }

                }
            }
            catch(Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }
        private void SatisAzalt(object sender, EventArgs e)
        {
            double posSumme = 0;
            try
            {

                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                Control cntParent = btnsecilen.Parent;
                btnsecilen.StateNormal.Border.Width = 5;
                SatisYap satisYap = (SatisYap)btnsecilen.Tag;
                if (satisYap.Adet > 1)
                {
                    satisYap.Adet = satisYap.Adet - 1;
                    summe = summe - (satisYap.Satisfiyat);
                    lblToplam.Text = (summe).ToString("C");
                }
                else
                {
                    posSumme = satisYap.Toplamtutar;
                    satisYap = null;
                    summe = summe - (posSumme);
                    lblToplam.Text = (summe).ToString("C");
                }
                if (satisYap != null)
                {
                    foreach (Control cnt in cntParent.Controls)
                    {
                        if (cnt.Name == "btnPlus")
                        {
                            cnt.Tag = satisYap;
                        }
                        if (cnt.Name == "txtMenge")
                        {
                            cnt.Text = satisYap.Adet.ToString(); ;
                        }

                    }
                }
                else
                {
                    cntParent.Parent.Dispose();
                    
                }
                //satisYap.Toplamtutar = satisYap.Satisfiyat * satisYap.Adet;
               

            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }
        private void PosSil(object sender, EventArgs e)
        {
            double posSumme = 0;
            try
            {

                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                btnsecilen.StateNormal.Border.Width = 5;
                SatisYap satisYap = (SatisYap)btnsecilen.Tag;
                
                    posSumme = satisYap.Toplamtutar;
                summe = summe - (posSumme);
                    satisYap = null;
                    lblToplam.Text = (summe).ToString("C");
                (btnsecilen.Parent).Parent.Dispose();
                
                //satisYap.Toplamtutar = satisYap.Satisfiyat * satisYap.Adet;


            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnBestellen_Click(object sender, EventArgs e)
        {
            if(yeniFis!=null)
            {
                yeniFis.SatisKalem.Clear();
            }
            Control[] cont1;
            cont1 = flpTop.Controls.Find("btnPlus", true);
            if (cont1.Length != 0)
            {
                MessageBox.Show(cont1.Length.ToString() + cont1[0].Name);
                foreach (Control cont in cont1)
                {
                    SatisYap satisYap1 = new SatisYap();
                    if (cont.Name == "btnPlus")
                    {
                        satisYap1 = (SatisYap)cont.Tag;
                        yeniFis.SatisKalem.Add(satisYap1);
                        continue;
                    }

                }
                yeniFis.FisiKapat();
                this.Close();
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
