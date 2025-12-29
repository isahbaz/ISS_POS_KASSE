using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;
using Microsoft.PointOfService;
//using tar;

namespace IS_KASSE
{
    public partial class F_Tisch : Form
    {

        List<int> masaID = new List<int>();
        Dictionary<int, FisOlustur> masalar = new Dictionary<int, FisOlustur>();
        Tarih tarih = new Tarih();
        int aktifmasaId = -1, LastMasaID=-1;
        
        FisOlustur aktifMasaFisi = new FisOlustur();

        MySqlConnection conn = new MySqlConnection();
        db baglanti = new db();

        public delegate void girilenPara(double Para);
        public event girilenPara ParaVermeEvent;

        TischAktuellBon bonIcerik;
        YetkiCheck yetkiCheck;
        /*
         * Satis Butonlari
         */
        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();

        public int grupid = 0;
        public double fiyat = 0;
        public double karmiktari = 0;
        public int artikelid = 0;
        public string UrunAd = "";
        public bool sonuc = false;
        public int ebene = 0;
        public int limit = 40;
        public int count = 0;
        int fisGoruntu = 0;
        public int ToplamKayitSay;
        MySqlDataAdapter daArtikel = null;
        DataTable dtArtikel = null;

        string angebotSembol = "";

        public delegate void KundenDisplayDelagate(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verilenPara, double paraUstu, int odemeTur);
        public event KundenDisplayDelagate KundenDisplayEvent;

        public F_Tisch()
        {
            InitializeComponent();
        }

        private void F_Tisch_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 33; i++)
            {
                masaID.Add(i);
            }
            masaID.Sort();
            ButtonOlustur();
        }
        private void ButtonOlustur()
        {           
            conn = baglanti.myconn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (conn)
            {
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikelgrup where gruptur>1 AND gruptur<>3 AND gruptur<>7 AND gruptur<>8  order by grupad", conn);
                DataTable dtArtikel = new DataTable("artikelgrup");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {
                    flowLayoutPanel2.Controls.Clear();
                    for (int i = 0; i < rowCount; i++)
                    {

                        ComponentFactory.Krypton.Toolkit.KryptonButton BtnGrup = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                        BtnGrup.Location = new System.Drawing.Point(3, 3); //grupid
                        BtnGrup.Name = dtArtikel.Rows[i].ItemArray[0].ToString();
                        BtnGrup.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                        BtnGrup.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                              | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                              | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnGrup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                        BtnGrup.Size = new System.Drawing.Size(120, 85);
                        BtnGrup.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                        BtnGrup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                              | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                              | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnGrup.StateNormal.Border.Rounding = 6;
                        BtnGrup.StateNormal.Border.Width = 3;
                        BtnGrup.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Navy;
                        BtnGrup.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnGrup.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[3]);//gruptur
                        BtnGrup.Values.Text = dtArtikel.Rows[i].ItemArray[1].ToString();//grupad
                        BtnGrup.Tag = Program.MwStList[2]; //dtArtikel.Rows[i].ItemArray[2];//mwst
                        BtnGrup.Click += new EventHandler(btnDiv7_Click);

                        flowLayoutPanel2.Controls.Add(BtnGrup);

                    }

                }
                else
                {
                    flowLayoutPanel2.Controls.Clear();
                }
                DBdenMasaOlustur(); //varsa db deki masalari yukler
                timer1.Enabled = true;
            }
        }

      
        public void btnDiv7_Click(object sender, EventArgs e)
        {
            double girilenFiyat = 0;
            KryptonButton btnSecilenGrup = sender as KryptonButton;
            if (!int.TryParse(btnSecilenGrup.Name, out grupid))
            {
                return;
            }
            if (btnSecilenGrup.TabIndex == 2)
            {
                F_TischProdukt urunler = new F_TischProdukt();
                urunler.grupid = grupid;
                urunler.TishInhaltEvent += new F_TischProdukt.tishInhalt(GenericButton);
                urunler.ShowDialog();
                DBdenMasaOlustur();
            }
            else if (btnSecilenGrup.TabIndex == 5 || btnSecilenGrup.TabIndex == 6)
            {
                if (double.TryParse(txtGiris.Text, out girilenFiyat))
                {
                    girilenFiyat = VkPreis();
                    if (aktifmasaId != -1)
                    {
                        SatisYap satisYap = new SatisYap();
                        satisYap.Adet = 1;
                        try
                        {
                            satisYap.Fisno = aktifMasaFisi.SatisAnaId;
                        }
                        catch
                        {
                        }
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = Convert.ToInt32(btnSecilenGrup.Tag); ;
                        satisYap.Satisfiyat = girilenFiyat;
                        satisYap.Tarih = tarih.unixdate(DateTime.Now);
                        satisYap.Toplamtutar = Math.Round(girilenFiyat, 2);
                        satisYap.UrunId = 0;
                        satisYap.UrunAd = btnSecilenGrup.Values.Text;
                        //satisYap.Birimkar = (satisYap.Satisfiyat - arananArtikel.EkPreis) * adet;
                        satisYap.Grubid = Convert.ToInt32(btnSecilenGrup.Name);
                        //satisYap.Fand = arananArtikel.Fand;
                        // satisYap.Fand2 = arananArtikel.Fand2;
                        
                        satisYap.dbMasaIslem(aktifMasaFisi, satisYap);
                        aktifMasaFisi.SatisKalem.Add(satisYap);
                        aktifMasaFisi.FisiKapat();
                        txtGiris.Text = "";
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["99"];
                        frmerror.ShowDialog();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["100"];
                    frmerror.ShowDialog();
                }
            }


        }
        public double VkPreis()
        {

            double yeniPreis;
            double yeniAdet;
            int Xyer = 0;
            Xyer = txtGiris.Text.IndexOf('X');
            if (Xyer == -1)
            {
                if (double.TryParse(txtGiris.Text, out yeniPreis))
                { 
                    if (Program.GlobalAyarlar["KOMMA"] == 0)
                    {
                       return   yeniPreis / 100;
                    }
                    else
                    {
                        return yeniPreis;
                    }
                }
                else
                {
                    // MessageBox.Show("HATALI FIYAT GIRDINIZ!");
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["24"];
                    frmerror.ShowDialog();
                    return 0;
                    

                }
            }
            else
            {
                if (double.TryParse(txtGiris.Text.Substring(0, Xyer), out yeniAdet))
                {
                    
                    if (double.TryParse(txtGiris.Text.Substring(Xyer + 1, txtGiris.Text.Length - (Xyer + 1)), out yeniPreis))
                    {
                        if (Program.GlobalAyarlar["KOMMA"] == 0)
                        {
                            return yeniPreis / 100;
                        }
                        else
                        {
                            return yeniPreis;
                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["24"];
                        frmerror.ShowDialog();
                        return 0;
                       
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["24"];
                    frmerror.ShowDialog();
                    return 0;

                }

            }


        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            masalar[aktifmasaId] = aktifMasaFisi;
            //MasalariYukle();
            DBdenMasaOlustur();
        }

        private void MasalariYukle()
        {
            /* masaID.Sort();
             int id = masaID[0];
             aktifmasaId = id;
             masaID.RemoveAt(0);
             string masano = "";
             if (txtGiris.Text != "")
             {
                 masano = txtGiris.Text;
             }
             else
             {
                 masano = aktifmasaId.ToString();
             }*/
            flowLayoutPanel1.Controls.Clear();
            foreach (KeyValuePair<int, FisOlustur> bilgi in masalar)
            {



                ComponentFactory.Krypton.Toolkit.KryptonButton BtnGrup = new ComponentFactory.Krypton.Toolkit.KryptonButton();

                BtnGrup.Location = new System.Drawing.Point(3, 3); //grupid
                BtnGrup.Name = bilgi.Key.ToString();

                BtnGrup.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                BtnGrup.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                BtnGrup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                BtnGrup.Size = new System.Drawing.Size(120, 85);
                if (aktifmasaId == bilgi.Key)
                {
                    BtnGrup.StateNormal.Border.Color1 = System.Drawing.SystemColors.HighlightText;
                    BtnGrup.StateNormal.Back.Color2 = Color.Red;
                    this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.DarkCyan;
                }
                else
                {
                    this.kryptonButton4.StateNormal.Back.Color1 = System.Drawing.Color.PapayaWhip;
                    this.kryptonButton4.StateNormal.Back.Color2 = System.Drawing.Color.LightYellow;
                    this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                }
              
               
                this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                BtnGrup.StateNormal.Border.Rounding = 6;
                BtnGrup.StateNormal.Border.Width = 3;
                BtnGrup.StateNormal.Content.LongText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                BtnGrup.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                BtnGrup.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                BtnGrup.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                BtnGrup.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                BtnGrup.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                BtnGrup.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                BtnGrup.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                BtnGrup.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                BtnGrup.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                //BtnGrup.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[3]);//gruptur
                BtnGrup.Values.Text = bilgi.Value.Masano.ToString();//grupad
                BtnGrup.Tag = bilgi;
                BtnGrup.Values.ExtraText = Program.lang["97"] + bilgi.Key.ToString() + "\n" + Program.lang["98"] + bilgi.Value.toplamtutar.ToString("C") + "\nBed: " + new User().UserBul(Program.bedID) + "\r\n";
                BtnGrup.Click += new System.EventHandler(btnMasa);
                flowLayoutPanel1.Controls.Add(BtnGrup);
            }
        }


        private void GenericButton(object sender, EventArgs e)
        {
            if (aktifmasaId != -1)
            {
                aktifMasaFisi = masalar[aktifmasaId];
                Dictionary<object, object> Info = new Dictionary<object, object>();
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                Info = (Dictionary<object, object>)btnsecilen.Tag;
                fiyat = Convert.ToDouble(Info["preis"]);
                //fiyat = Convert.ToDouble(btnsecilen.Tag);
                artikelid = Convert.ToInt32(Info["artikelid"]);
                karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2); //Convert.ToDouble(btnsecilen.Name);
                UrunAd = btnsecilen.Values.Text.Substring(0, btnsecilen.Values.Text.ToString().IndexOf('\n'));

                SatisYap satisYap = new SatisYap();
                satisYap.Adet = 1;
                satisYap.Fisno = aktifMasaFisi.SatisAnaId;
                satisYap.KasaNo = Program.kasano;
                satisYap.Mwst = Program.MwStList[2]; // Convert.ToDouble(Info["mwst"]);//
                satisYap.Satisfiyat = fiyat;
                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                satisYap.Toplamtutar = Math.Round(fiyat, 2);
                satisYap.UrunId = artikelid;
                satisYap.UrunAd = UrunAd;
                //satisYap.Birimkar = (satisYap.Satisfiyat - arananArtikel.EkPreis) * adet;
                satisYap.Grubid = Convert.ToInt32(Info["grupid"]); ;
                //satisYap.Fand = arananArtikel.Fand;
                // satisYap.Fand2 = arananArtikel.Fand2;
                satisYap.dbMasaIslem(aktifMasaFisi, satisYap);
                aktifMasaFisi.SatisKalem.Add(satisYap);
                
                aktifMasaFisi.FisiKapat();
                DBdenMasaOlustur();
                this.KundenDisplayEvent(UrunAd,satisYap.Adet.ToString(),satisYap.Satisfiyat.ToString("#0.00"),satisYap.Toplamtutar.ToString("#0.00"),"0",0,0,0,0);
                if (Convert.ToInt16(Info["bereich"]) != 0)
                {
                    backgroundWorker1.RunWorkerAsync(satisYap);
                  
                }
               // MasalariYukle();
                // MasaBilgisiGuncelle();
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["99"];
                frmerror.ShowDialog();
              
                //this.Close();
            }

        }

       

        private void btnMasaOlustur_Click(object sender, EventArgs e)
        {
            if (fisGoruntu == 0)
            {
                MasaOlustur();
                txtGiris.Text = "";
            }
        }
        private void DBdenMasaOlustur()
        {
            tarih = new Tarih();
            //timer1.Enabled = false;
            bool masavar = false;
            conn = baglanti.myconn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            masalar.Clear();
            //flowLayoutPanel1.Controls.Clear();
            masaID.Clear();
            for (int i = 1; i < 33; i++)
            {
                masaID.Add(i);
            }
            using (conn)
            {
                try
                {

                    string MasaSQL = "SELECT * FROM masamaster WHERE tarih>" + tarih.dunBaslangic();
                    MySqlDataAdapter daMasa = new MySqlDataAdapter(MasaSQL, conn);
                    DataTable dtMasa = new DataTable();
                    dtMasa.Rows.Clear();
                    daMasa.Fill(dtMasa);
                    KeyValuePair<int, FisOlustur> bilgi = new KeyValuePair<int, FisOlustur>();
                    if (dtMasa.Rows.Count > 0)
                    {
                        timer1.Enabled = false;
                        masaID.Sort();
                        for (int i = 0; i < dtMasa.Rows.Count; i++)
                        {

                            int id = Convert.ToInt16(dtMasa.Rows[i].ItemArray[2]);
                            string masano = dtMasa.Rows[i].ItemArray[1].ToString();
                            foreach (Control cnt in flowLayoutPanel1.Controls)
                            {
                                if (cnt is KryptonButton)
                                {

                                    KryptonButton cnt1 = cnt as KryptonButton;
                                    if (cnt1.Name == id.ToString())
                                    {

                                        bilgi = (KeyValuePair<int, FisOlustur>)cnt1.Tag;
                                        FisOlustur ff = bilgi.Value;
                                        ff.FisiKapat();
                                        cnt1.Values.ExtraText = Program.lang["97"] + masano + "\n" + Program.lang["98"] + ff.toplamtutar.ToString("C") + "\nBed: " + new User().UserBul(ff.kasiyerno) + "\r\n";
                                        masaID.Remove(id);
                                        masavar = true;
                                        masalar.Add(id, ff);
                                        if (aktifmasaId == id)
                                        {

                                            cnt1.StateNormal.Border.Color1 = System.Drawing.SystemColors.HighlightText;
                                            cnt1.StateNormal.Back.Color2 = Color.Red;
                                            this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.DarkCyan;
                                            masavar = true;
                                        }
                                        else
                                        {
                                            cnt1.StateNormal.Back.Color1 = System.Drawing.Color.PapayaWhip;
                                            cnt1.StateNormal.Back.Color2 = System.Drawing.Color.LightYellow;
                                            cnt1.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                                        }
                                        break;

                                    }
                                    masavar = false;
                                }
                                else
                                {
                                    flowLayoutPanel1.Controls.Remove(cnt);
                                }
                            }
                            if (masavar == false)
                            {
                                //aktifmasaId = id;
                                masaID.Remove(id);


                                FisOlustur newmasa = new FisOlustur();
                                newmasa.kasiyerno = (int)dtMasa.Rows[i].ItemArray[7];
                                newmasa.tarih = (double)dtMasa.Rows[i].ItemArray[3];
                                newmasa.MasaID = id;
                                newmasa.Masano = masano;
                                newmasa.KasaNo = (int)dtMasa.Rows[i].ItemArray[4];
                                newmasa.MasaDBid = (long)dtMasa.Rows[i].ItemArray[0];

                                bilgi = new KeyValuePair<int, FisOlustur>(id, newmasa);

                                masalar.Add(id, newmasa);
                                ComponentFactory.Krypton.Toolkit.KryptonButton BtnGrup = new ComponentFactory.Krypton.Toolkit.KryptonButton();

                                BtnGrup.Location = new System.Drawing.Point(3, 3); //grupid
                                BtnGrup.Name = id.ToString();


                                BtnGrup.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                                BtnGrup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                                BtnGrup.Size = new System.Drawing.Size(120, 85);
                                if (aktifmasaId == id)
                                {

                                    BtnGrup.StateNormal.Border.Color1 = System.Drawing.SystemColors.HighlightText;
                                    BtnGrup.StateNormal.Back.Color2 = Color.Red;
                                    this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.DarkCyan;
                                    masavar = true;
                                }
                                else
                                {
                                    BtnGrup.StateNormal.Back.Color1 = System.Drawing.Color.PapayaWhip;
                                    BtnGrup.StateNormal.Back.Color2 = System.Drawing.Color.LightYellow;
                                    BtnGrup.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                                }

                                BtnGrup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                                BtnGrup.StateNormal.Border.Rounding = 6;

                                BtnGrup.StateNormal.Border.Width = 3;
                                BtnGrup.StateNormal.Content.LongText.Font = new System.Drawing.Font("Arial", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                                BtnGrup.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                                BtnGrup.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                                BtnGrup.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                                BtnGrup.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                                BtnGrup.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                                BtnGrup.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                                BtnGrup.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                                BtnGrup.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                                BtnGrup.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                                BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                                //BtnGrup.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[3]);//gruptur
                                BtnGrup.Values.Text = masano;//grupad
                                BtnGrup.Tag = bilgi;

                                string MasaBilgiSQL = "SELECT * FROM masadetail WHERE masadbid=" + dtMasa.Rows[i].ItemArray[0].ToString();
                                MySqlCommand myMasaBilgi = new MySqlCommand(MasaBilgiSQL, conn);
                                MySqlDataReader drMasaBilgi = myMasaBilgi.ExecuteReader();
                                if (drMasaBilgi.HasRows)
                                {
                                    while (drMasaBilgi.Read())
                                    {


                                        SatisYap satisYap = new SatisYap();
                                        satisYap.Adet = 1;
                                        satisYap.Fisno = drMasaBilgi.GetInt32(2);
                                        satisYap.KasaNo = drMasaBilgi.GetInt16(9);
                                        satisYap.Mwst = drMasaBilgi.GetInt16(4);
                                        satisYap.Satisfiyat = drMasaBilgi.GetDouble(5);
                                        satisYap.Tarih = drMasaBilgi.GetDouble(1);
                                        satisYap.Toplamtutar = drMasaBilgi.GetDouble(7);
                                        satisYap.UrunId = drMasaBilgi.GetInt32(3);
                                        satisYap.UrunAd = drMasaBilgi.GetString(11);
                                        //satisYap.Birimkar = (satisYap.Satisfiyat - arananArtikel.EkPreis) * adet;
                                        satisYap.Grubid = drMasaBilgi.GetInt32(10);
                                        satisYap.MasaDetailId = drMasaBilgi.GetInt32(0);
                                        //satisYap.Fand = arananArtikel.Fand;
                                        // satisYap.Fand2 = arananArtikel.Fand2;
                                        newmasa.SatisKalem.Add(satisYap);



                                    }
                                    newmasa.FisiKapat();
                                }
                                BtnGrup.Values.ExtraText = Program.lang["97"] + masano + "\n" + Program.lang["98"] + newmasa.toplamtutar.ToString("C") + "\nBed: " + new User().UserBul(newmasa.kasiyerno) + "\r\n";
                                BtnGrup.Click += new System.EventHandler(btnMasa);
                                flowLayoutPanel1.Controls.Add(BtnGrup);
                                drMasaBilgi.Close();

                            }

                        }

                        timer1.Enabled = true;



                    }
                    else
                    {
                        flowLayoutPanel1.Controls.Clear();
                    }
                }

                catch (Exception ww)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = ww.Message;
                    frmerror.ShowDialog();
                    return;
                }
            }

           /* if (masavar == false)
            {
                aktifMasaFisi = null;
                aktifmasaId = -1;
            }*/
            //timer1.Enabled = true;
        }
        private void MasaOlustur()
        {
            int verilenMasaNo = 0;
            int id = 0;


            if (masaID.Count > 0)
            {
                if (txtGiris.Text != "")
                {
                    if (int.TryParse(txtGiris.Text, out verilenMasaNo))
                    {

                    }
                }
                int indexid = masaID.IndexOf(verilenMasaNo);
                if (indexid != -1)
                {

                    //masaID.Sort();
                    id = masaID[masaID.IndexOf(verilenMasaNo)];
                    //aktifmasaId = id;
                    masaID.RemoveAt(indexid);

                }
                else if (indexid == -1 && txtGiris.Text != "")
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["101"];
                    frmerror.ShowDialog();
                    return;
                }
                else
                {
                    masaID.Sort();
                    id = masaID[0];
                    //aktifmasaId = id;
                    masaID.RemoveAt(0);
                }
                string masano = id.ToString();
                /* if (txtGiris.Text != "")
                 {
                     masano = txtGiris.Text;
                 }
                 else
                 {
                     masano = aktifmasaId.ToString();
                 }*/
                FisOlustur newmasa = new FisOlustur();
                newmasa.FisYarat(0);
                newmasa.kasiyerno = Program.bedID;
                newmasa.tarih = tarih.unixdate(DateTime.Now);
                newmasa.MasaID = id;
                newmasa.Masano = masano;
                newmasa.KasaNo = Program.kasano;
                KeyValuePair<int, FisOlustur> bilgi = new KeyValuePair<int, FisOlustur>();
                bilgi = new KeyValuePair<int, FisOlustur>(id, newmasa);

                masalar.Add(id, newmasa);
                ComponentFactory.Krypton.Toolkit.KryptonButton BtnGrup = new ComponentFactory.Krypton.Toolkit.KryptonButton();

                BtnGrup.Location = new System.Drawing.Point(3, 3); //grupid
                BtnGrup.Name = id.ToString();

                BtnGrup.StateNormal.Back.Color1 = System.Drawing.Color.PapayaWhip;
                BtnGrup.StateNormal.Back.Color2 = System.Drawing.Color.LightYellow;
                BtnGrup.StateNormal.Border.Color1 = System.Drawing.Color.Navy;

                BtnGrup.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                BtnGrup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                BtnGrup.Size = new System.Drawing.Size(120, 85);
                
                BtnGrup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                      | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                BtnGrup.StateNormal.Border.Rounding = 6;

                BtnGrup.StateNormal.Border.Width = 3;
                BtnGrup.StateNormal.Content.LongText.Font = new System.Drawing.Font("Arial", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                BtnGrup.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                BtnGrup.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                BtnGrup.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                BtnGrup.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                BtnGrup.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                BtnGrup.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                BtnGrup.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                BtnGrup.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                BtnGrup.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                //BtnGrup.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[3]);//gruptur
                BtnGrup.Values.Text = masano;//grupad
                BtnGrup.Tag = bilgi;
                BtnGrup.Values.ExtraText = Program.lang["97"] + masano + "\n" + Program.lang["98"] + newmasa.toplamtutar.ToString("C") + "\nBed: " + new User().UserBul(Program.bedID) + "\r\n";
                BtnGrup.Click += new System.EventHandler(btnMasa);
                flowLayoutPanel1.Controls.Add(BtnGrup);
                newmasa.MasaDBid = newmasa.dbMasaYarat(newmasa);
               // aktifMasaFisi = newmasa;
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["102"];
                frmerror.ShowDialog();
                return;
            }


        }
        private void btnMasa(object sender, EventArgs e)
        {
            try
            {

                KryptonButton btnTiklanan = sender as KryptonButton;
                // btnTiklanan.StateNormal.Border.Color1 = System.Drawing.SystemColors.HighlightText;
                // btnTiklanan.StateCommon.Back.Color1 = Color.Yellow;
                //btnTiklanan.StateNormal.Back.Color1 = System.Drawing.SystemColors.HighlightText;
                //btnTiklanan.StateTracking.Back.Color1 = Color.Yellow;

                KeyValuePair<int, FisOlustur> bilgi = (KeyValuePair<int, FisOlustur>)btnTiklanan.Tag;
                aktifmasaId = bilgi.Key;
                aktifMasaFisi = bilgi.Value;
                if (LastMasaID != aktifmasaId)
                {
                    LastMasaID = aktifmasaId;
                    AktifMasaIcerigiDisplayaYukle();


                }
                foreach (KryptonButton btn in flowLayoutPanel1.Controls)
                {
                    if (btn.Name != btnTiklanan.Name)
                    {
                        // btn.StateNormal.
                        btn.StateNormal.Back.Color1 = System.Drawing.Color.PapayaWhip;
                        btn.StateNormal.Back.Color2 = System.Drawing.Color.LightYellow;
                        btn.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                        // btn.StateNormal.Border.Color1.R = "0";
                    }
                    else
                    {
                        btn.StateNormal.Border.Color1 = System.Drawing.Color.LimeGreen;
                        btn.StateNormal.Back.Color1 = System.Drawing.Color.LightPink;
                        btn.StateNormal.Back.Color2 = System.Drawing.Color.Azure;

                    }
                }
            }
            catch (Exception ff)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = ff.Message;
                frmerror.ShowDialog();
            }

        }

        private void AktifMasaIcerigiDisplayaYukle()
        {
            try
            {
                this.KundenDisplayEvent("", "", "", "", "", 9,0,0,0);
                foreach (SatisYap satislar in aktifMasaFisi.SatisKalem)
                {


                    this.KundenDisplayEvent(satislar.UrunAd, satislar.Adet.ToString("#0.00"), satislar.Satisfiyat.ToString("C"), satislar.Toplamtutar.ToString("C"), "TOTAL :" + aktifMasaFisi.toplamtutar.ToString("C"), 0,0,0,0);

                }
            }
            catch (Exception gg)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = gg.Message;
                frmerror.ShowDialog();
            }
        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            KryptonButton sayiBtn = sender as KryptonButton;
            txtGiris.Text += sayiBtn.Text;
            //Program.VerilenPara = Convert.ToDouble(txtGiris.Text);
            if(fisGoruntu==1)
            {
                bonIcerik.RuckGeldHesapla(Convert.ToDouble(txtGiris.Text));
            }
         
        }
       /* public event EventHandler Gonder;
        public event EventHandler Gonder
        {

        }
    */
        public void DisplayFromBonInhalt(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verilenPara, double paraUstu, int odemetur)
        {
            this.KundenDisplayEvent(urunad,  adet,  satisfiyat,  postoplam,  toplamtutar,  islemtur,verilenPara, paraUstu, odemetur );
        }
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.KundenDisplayEvent("", "","","","",9,0,0,0);
            timer1.Enabled = false;
            if (fisGoruntu == 0)
            {
                if (aktifMasaFisi != null)
                {
                    btnMasaOlustur.Enabled = false;
                    if (aktifMasaFisi.toplamtutar > 0)
                    {
                        flowLayoutPanel1.Controls.Clear();
                        bonIcerik = new TischAktuellBon();
                        bonIcerik.MevcutMasalar = masalar;
                        bonIcerik.MevcutMasalar.Remove(aktifMasaFisi.MasaID);
                        bonIcerik.KundenDisplayFisInhaltEvent +=new TischAktuellBon.KundenDisplayDelagateFisInhalt(DisplayFromBonInhalt);
                        //bonIcerik.Gonder+=new TischAktuellBon.girilenPara(btnBir_Click);
                        //bonIcerik.
                        bonIcerik.fis = aktifMasaFisi;
                        this.SuspendLayout();
                        foreach (Panel pp in bonIcerik.panelGetir())
                        {
                            flowLayoutPanel1.Controls.Add(pp);
                        }

                        this.ResumeLayout(false);
                        fisGoruntu = 1;
                        aktifmasaId = -1;

                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["103"];
                        frmerror.ShowDialog();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["99"];
                    frmerror.ShowDialog();
                }
            }
            else
            {
                btnMasaOlustur.Enabled = true;
                DBdenMasaOlustur();
                fisGoruntu = 0;
                timer1.Enabled = true;
            }
            

        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            long lastid;

           
            using (conn = baglanti.myconn())
            {
                // conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                string userTakipSql = "UPDATE usertakip SET offlinezaman=" + tarih.unixdate(DateTime.Now) + " WHERE userid=" + Program.bedID + " and tarih=" + tarih.bugunBaslangic() + " and  onlinezaman<>0 and offlinezaman=0 and kasaid=" + Program.kasano;

                MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                if (coUserTakip.ExecuteNonQuery() > 0)
                {
                    lastid = coUserTakip.LastInsertedId;
                    conn.Close();

                }
                FBedChange frmchange = new FBedChange();
                frmchange.ShowDialog();
               
            }
        }

        private void kryptonButton35_Click(object sender, EventArgs e)
        {
            double kalan=0;
            txtGiris.Text = "";
            if (fisGoruntu == 1)
            {
                bonIcerik.RuckGeldHesapla(double.TryParse(txtGiris.Text,out kalan )?kalan:0);
            }
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            KryptonButton btnTik = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(btnTik.Name);//
            if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
            {


                try
                {
                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                }
                catch
                {
                }
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["6"];
                frmerror.ShowDialog();
            }
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (fisGoruntu == 0)
            {
                DBdenMasaOlustur();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SatisYap basilacakitem = (SatisYap)e.Argument;
            FisBarkodlu SiparisFisi = new FisBarkodlu();
            SiparisFisi.SiparisYaz(basilacakitem);
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

        }
       




    }
}


/*
 *  //ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup6 = new KryptonGroup();
                    this.kryptonGroup6.Location = new System.Drawing.Point(30, 30);
                    this.kryptonGroup6.Margin = new System.Windows.Forms.Padding(15);
                    this.kryptonGroup6.Name = "kryptonGroup6";
                    // 
                    // kryptonGroup6.Panel
                    // 
                    this.kryptonGroup6.Panel.Controls.Add(this.label6);
                    this.kryptonGroup6.Panel.Controls.Add(this.panel8);
                    this.kryptonGroup6.Panel.Margin = new System.Windows.Forms.Padding(5);
                    this.kryptonGroup6.Panel.Padding = new System.Windows.Forms.Padding(3);
                    this.kryptonGroup6.Size = new System.Drawing.Size(206, 181);
                    this.kryptonGroup6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.kryptonGroup6.StateCommon.Border.Rounding = 3;
                    if (Convert.ToInt16(dtMenuPlu.Rows[a].ItemArray[6]) == 1)
                    {
                        this.kryptonGroup6.StateNormal.Border.Color1 = System.Drawing.Color.Green;
                    }
                    else
                    {
                        this.kryptonGroup6.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                    }
                    this.kryptonGroup6.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.kryptonGroup6.StateNormal.Border.Rounding = 5;
                    this.kryptonGroup6.StateNormal.Border.Width = 1;
                    this.kryptonGroup6.TabIndex = 4;
                    // 
                    // label6
                    // 
                    this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                    this.label6.Font = new System.Drawing.Font("Constantia", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.label6.Location = new System.Drawing.Point(3, 120);
                    this.label6.Name = "label6";
                    this.label6.Size = new System.Drawing.Size(191, 52);
                    this.label6.TabIndex = 1;
                    this.label6.Text = dtMenuPlu.Rows[a].ItemArray[9].ToString();
                    this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    // 
                    // btnMenge
                    // 
                    this.btnMenge.Location = new System.Drawing.Point(232, 47);
                    this.btnMenge.Name = "btnMenge";
                    this.btnMenge.Size = new System.Drawing.Size(64, 36);
                    this.btnMenge.StateCommon.Back.Color1 = System.Drawing.Color.YellowGreen;
                    this.btnMenge.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                    this.btnMenge.StateCommon.Border.Color1 = System.Drawing.Color.DarkGreen;
                    this.btnMenge.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.btnMenge.StateCommon.Border.Rounding = 4;
                    this.btnMenge.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                    this.btnMenge.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                    this.btnMenge.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.btnMenge.TabIndex = 5;
                    this.btnMenge.Values.Text = "1X";
                    // 
                    // btnMengeMinus
                    // 
                    this.btnMengeMinus.Location = new System.Drawing.Point(232, 89);
                    this.btnMengeMinus.Name = "btnMengeMinus";
                    this.btnMengeMinus.Size = new System.Drawing.Size(64, 36);
                    this.btnMengeMinus.StateCommon.Back.Color1 = System.Drawing.Color.DarkOrange;
                    this.btnMengeMinus.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                    this.btnMengeMinus.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                    this.btnMengeMinus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.btnMengeMinus.StateCommon.Border.Rounding = 4;
                    this.btnMengeMinus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                    this.btnMengeMinus.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                    this.btnMengeMinus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.btnMengeMinus.TabIndex = 7;
                    this.btnMengeMinus.Values.Text = "-";
                    aktifPanel.Controls.Add(kryptonGroup6);*/
