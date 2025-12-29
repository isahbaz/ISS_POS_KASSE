using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace IS_KASSE
{
    public partial class F_TischProdukt : Form
    {
        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();


        public int grupid = -1;
        public double fiyat = 0;
        public double karmiktari = 0;
        public int artikelid = 0;
        public string UrunAd = "";
        public bool sonuc = false;
        public int ebene = 0;
        public int limit = 40;
        public int count = 0;
        int rowCount = 0;
        public int ToplamKayitSay;
        MySqlDataAdapter daArtikel = null;
        DataTable dtArtikel = null;

        public delegate void tishInhalt(object sender, EventArgs e);
        public event tishInhalt TishInhaltEvent;

        public F_TischProdukt()
        {
            InitializeComponent();
        }
        
        private void F_TischProdukt_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string apppath = Application.StartupPath;
            // waageYarat();

            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
            KryptonButton btnSecilenGrup = sender as KryptonButton;
            if ( grupid<0)
            {
                return;
            }
            //aktifnesne = textBox1;

            //using (conn)
            //{
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();


            }
            if (count == 0)
            {
                using (conn)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 ORDER BY artikelad", conn);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    rowCount = dtArtikel.Rows.Count;
                    ToplamKayitSay = rowCount;
                    if (ToplamKayitSay < limit)
                        limit = ToplamKayitSay;
                }
            }
            else
            {
                using (conn)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 ORDER BY artikelad LIMIT " + count + "," + limit, conn);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    rowCount = dtArtikel.Rows.Count;
                }
            }

            if (rowCount > 0)
            {


                for (int i = 0; i < limit; i++)
                {
                    Dictionary<object, object> Info = new Dictionary<object, object>();
                    Info.Add("karmiktari", dtArtikel.Rows[i].ItemArray[10]);
                    Info.Add("artikelid", dtArtikel.Rows[i].ItemArray[0]);
                    Info.Add("mwst", dtArtikel.Rows[i].ItemArray[6]);
                    Info.Add("barkod", dtArtikel.Rows[i].ItemArray[1]);
                    Info.Add("grupid", dtArtikel.Rows[i].ItemArray[3]);
                    Info.Add("alisfiyat", dtArtikel.Rows[i].ItemArray[7]);
                    Info.Add("preis", dtArtikel.Rows[i].ItemArray[8]);
                    Info.Add("bereich", dtArtikel.Rows[i].ItemArray[32]);

                    ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                    BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                    BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[10].ToString();
                    BtnArtikel1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                    BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                    BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                    BtnArtikel1.Size = new System.Drawing.Size(162, 98);
                    BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                    if (dtArtikel.Rows[i].ItemArray[19].ToString() != "")
                    {
                        try
                        {
                            BtnArtikel1.StateNormal.Back.Image = Image.FromFile(dtArtikel.Rows[i].ItemArray[19].ToString());
                            BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                        }
                        catch (Exception ee)
                        {
                            string msj = ee.Message;

                        }
                    }
                    // BtnArtikel1.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.mayd1;
                    BtnArtikel1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                    BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                    BtnArtikel1.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                    BtnArtikel1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    BtnArtikel1.StateNormal.Border.Rounding = 6;
                    BtnArtikel1.StateNormal.Border.Width = 3;
                    BtnArtikel1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                    BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                    BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                    BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                    BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                    BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                    BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                    BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                    BtnArtikel1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    /* if (Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]) < 0)
                     {
                         BtnArtikel1.TabIndex = 0;
                     }
                     else
                     {
                         BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                     }*/
                    BtnArtikel1.Values.Text =  dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + string.Format("{0:f}", (dtArtikel.Rows[i].ItemArray[8])) + "EUR/St\r " + " [" + dtArtikel.Rows[i].ItemArray[1] + "]\r";
                    // BtnArtikel1.Values.Text = angebotSembol+dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + dtArtikel.Rows[i].ItemArray[8].ToString() + "[" + dtArtikel.Rows[i].ItemArray[1] + "]"; 
                    BtnArtikel1.Tag = Info;//dtArtikel.Rows[i].ItemArray[8];
                    BtnArtikel1.Click += new EventHandler(GenericButton);

                    flowLayoutPanel1.Controls.Add(BtnArtikel1);

                }
                if (ToplamKayitSay > limit + count)
                {
                    this.btnEbeneNext.Location = new System.Drawing.Point(2, 2);
                    this.btnEbeneNext.Name = "btnEbeneNext";
                    this.btnEbeneNext.Size = new System.Drawing.Size(162, 98);
                    this.btnEbeneNext.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    this.btnEbeneNext.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.btnEbeneNext.TabIndex = 51;
                    this.btnEbeneNext.Values.Text = "WEITER";
                    this.btnEbeneNext.Click += new System.EventHandler(this.btnEbeneNext_Click);
                    flowLayoutPanel1.Controls.Add(btnEbeneNext);

                }
            }

            this.kryptonButton4.Location = new System.Drawing.Point(2, 2);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Size = new System.Drawing.Size(162, 98);
            this.kryptonButton4.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton4.TabIndex = 51;
            this.kryptonButton4.Values.Text = Program.lang["56"];
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);

            flowLayoutPanel1.Controls.Add(kryptonButton4);
            // }
        }
        private void GenericButton(object sender, EventArgs e)
        {
           /* if (aktifmasaId != -1)
            {
                aktifMasaFisi = masalar[aktifmasaId];*/
                Dictionary<object, object> Info = new Dictionary<object, object>();
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                this.TishInhaltEvent(btnsecilen, e);
               /* Info = (Dictionary<object, object>)btnsecilen.Tag;
                fiyat = Convert.ToDouble(Info["preis"]);
                //fiyat = Convert.ToDouble(btnsecilen.Tag);
                artikelid = Convert.ToInt32(Info["artikelid"]);
                karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2); //Convert.ToDouble(btnsecilen.Name);
                UrunAd = btnsecilen.Values.Text;

                SatisYap satisYap = new SatisYap();
                satisYap.Adet = 1;
                satisYap.Fisno = aktifMasaFisi.SatisAnaId;
                satisYap.KasaNo = Program.kasano;
                satisYap.Mwst = Convert.ToInt32(Info["mwst"]); ;

                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                satisYap.Toplamtutar = Math.Round(fiyat, 2);
                satisYap.UrunId = artikelid;
                satisYap.UrunAd = UrunAd;
                //satisYap.Birimkar = (satisYap.Satisfiyat - arananArtikel.EkPreis) * adet;
                satisYap.Grubid = Convert.ToInt32(Info["grupid"]); ;
                //satisYap.Fand = arananArtikel.Fand;
                // satisYap.Fand2 = arananArtikel.Fand2;
                aktifMasaFisi.SatisKalem.Add(satisYap);

                aktifMasaFisi.FisiKapat();
                // MasaBilgisiGuncelle();
            }
            else
            {
                MessageBox.Show("Lutfen Öncelikle islem Yapacaginiz Masayi Seciniz!");
            }*/

        }
        private void btnEbeneNext_Click(object sender, EventArgs e)
        {
            F_ObstGemuseStuck frmobstGemuseStuck = new F_ObstGemuseStuck();
            ebene++;
            count = count + limit;
            frmobstGemuseStuck.ebene = ebene;
            frmobstGemuseStuck.grupid = grupid;
            frmobstGemuseStuck.count = count;
            frmobstGemuseStuck.ToplamKayitSay = ToplamKayitSay;
            if ((ebene + 1) * limit < ToplamKayitSay)
            {
                frmobstGemuseStuck.limit = limit;
            }
            else
            {
                if (count > ToplamKayitSay)
                {
                    count = count - limit;
                    frmobstGemuseStuck.count = count;
                    // lim

                }
                frmobstGemuseStuck.limit = ToplamKayitSay - count;
            }
            frmobstGemuseStuck.ShowDialog();
            if (frmobstGemuseStuck.sonuc == true)
            {
                fiyat = frmobstGemuseStuck.fiyat;
                artikelid = frmobstGemuseStuck.artikelid;
                karmiktari = frmobstGemuseStuck.karmiktari;
                UrunAd = frmobstGemuseStuck.UrunAd;
                sonuc = true;
                this.Close();
            }
        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
