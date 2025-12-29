using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using Microsoft.PointOfService;
using iss_Rabat;
using tar;

namespace IS_KASSE
{
    public partial class F_Storno : Form
    {
        public int fisno;
        int seciliItem = -1;
        tar.Tarih tarih = new tar.Tarih();
        FisOlustur eskiFis = new FisOlustur();
        FisOlustur stornoFis = null;
        FisOlustur tekrarFis = null;
        //KeyValuePair<string, int> silinenler = new KeyValuePair<string, int>();
        List<KeyValuePair<string, double>> silinenler = new List<KeyValuePair<string, double>>();
        List<long> silinenSatisDetayidler = new List<long>();
        db baglan = null;
        MySqlConnection conn = null;
        
        public F_Storno()
        {
            Program.bonDruck = true;
            InitializeComponent();
            if (Program.IsletmeAyarlar["markt"] == "1")
            {
                btnBewirtung.Visible = false;
            }
        }
        private void StornoFisCheck(int fisNo)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
            conn.Open();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();


            }
            MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * FROM satisana WHERE stornoilgi=" + fisNo, conn);
            DataTable dtArtikel = new DataTable("user");
            dtArtikel.Clear();
            daArtikel.Fill(dtArtikel);
            int rowCount = dtArtikel.Rows.Count;
            if (rowCount > 0)
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["84"] + string.Join(",", dtArtikel.Rows[0].ItemArray[0]);
                frmError.ShowDialog();
                stornoFis = null;
                lblStorno.Text = Program.lang["84"] + dtArtikel.Rows[0].ItemArray[0].ToString();
                lblStorno.Visible = true;
                kryptonButton1.Enabled = false;
                btnKaydet.Enabled = false;
                kryptonButton3.Enabled = false;
            }
            else
            {
            }
            }
            catch
            {
            }


        }
        private void F_Storno_Load(object sender, EventArgs e)
        {
            silinenSatisDetayidler.Clear();
            lblStorno.Visible = false;
            int position = 1;
            listView1.Items.Clear();

            //FisOlustur eskiFis = new FisOlustur();
            if (eskiFis.FisCagir(fisno) == true)
            {
                StornoFisCheck(fisno);
                lFisno.Text = fisno.ToString();
                lblFisNo.Text = fisno.ToString();
                lblFisOdenmeSekli.Text = odemesekliBul(eskiFis.odemesekli);
                lblFisTarih.Text = tarih.tarih((long)eskiFis.tarih);
                lblKasiyer.Text = kasiyerBul(eskiFis.kasiyerno);
                lblTutar.Text = (eskiFis.toplamtutar - eskiFis.Rabattutar).ToString("C");
                txtToplam.Text = (eskiFis.toplamtutar - eskiFis.Rabattutar).ToString("C");
                if (eskiFis.odemesekli == 2)
                {
                    if (eskiFis.StornoIlgi != 0)
                    {
                        lblStorno.Text = Program.lang["84"] +eskiFis.StornoIlgi.ToString() ;
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }
                    else
                    {
                        lblStorno.Text = Program.lang["85"]; ;
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }

                }
                foreach (SatisYap satislar in eskiFis.SatisKalem)
                {
                    int listViewElaman = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[listViewElaman].SubItems.Add(satislar.UrunAd.ToString() + "{" + satislar.Adet + "x" + satislar.Satisfiyat.ToString("C") + "}");
                    listView1.Items[listViewElaman].SubItems.Add(satislar.Toplamtutar.ToString("C"));
                    if (satislar.Stornodurum == 1)
                    {
                        listView1.Items[listViewElaman].SubItems.Add("*");
                    }
                    else
                    {
                        listView1.Items[listViewElaman].SubItems.Add("-");

                    }

                    if (satislar.UrunId != 99999)
                    {
                        position++;
                    }
                }


            }
            else
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["85"];
                frmError.ShowDialog();
                this.Close();
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }

        }

        private string kasiyerBul(int p)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
            conn.Open();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();


            }
            MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from user where userid=" + p, conn);
            DataTable dtArtikel = new DataTable("user");
            dtArtikel.Clear();
            daArtikel.Fill(dtArtikel);
            int rowCount = dtArtikel.Rows.Count;
            if (rowCount > 0)
            {
                return dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
            }
            else
            {
                return "Tanımsız";
            }
        }

        private string odemesekliBul(int p)
        {
            if (p == 0)
            {
                return "EC KARTE";
            }
            else if (p == 1)
            {
                return "BAR";
            }
            else if (p == 2)
            {
                return "Storno";
            }
            else
            {
                return "TANIMSIZ";
            }
        }

        private void btnYukari_Click(object sender, EventArgs e)
        {
            int sayi = listView1.Items.Count;
            seciliItem = -1;
            ListViewItem eleman;
            for (int i = 0; i < sayi; i++)
            {
                eleman = listView1.Items[i];
                if (eleman.Selected)
                {
                    seciliItem = i;
                    break;

                }

            }
            if (seciliItem > 0)
            {
                listView1.Items[seciliItem - 1].Selected = true;
                seciliItem = seciliItem - 1;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
            else
            {
                listView1.Items[sayi - 1].Selected = true;
                seciliItem = sayi - 1;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
            if (listView1.Items[seciliItem].SubItems[3].Text == "*")
            {
                kryptonButton3.Enabled = false;
            }
            else
            {
                kryptonButton3.Enabled = true;
            }
            if (listView1.Items.Count > 0)
            {
                //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void btnAsagi_Click(object sender, EventArgs e)
        {
            int sayi = listView1.Items.Count;
            seciliItem = -1;
            ListViewItem eleman;
            for (int i = 0; i < sayi; i++)
            {
                eleman = listView1.Items[i];
                if (eleman.Selected)
                {
                    seciliItem = i;
                    break;

                }

            }
            if (seciliItem < sayi - 1)
            {
                listView1.Items[seciliItem + 1].Selected = true;
                seciliItem = seciliItem + 1;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
            else
            {
                listView1.Items[0].Selected = true;
                seciliItem = 0;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
            if (listView1.Items[seciliItem].SubItems[3].Text == "*")
            {
                kryptonButton3.Enabled = false;
            }
            else
            {
                kryptonButton3.Enabled = true;
            }
            if (listView1.Items.Count > 0)
            {
                //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            if (seciliItem == -1)
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["89"]  ;
                frmError.ShowDialog();
            }
            else
            {
                if (listView1.Items.Count > 1)
                {
                    if (listView1.Items[seciliItem].SubItems[3].Text != "*")
                    {

                        //MessageBox.Show(seciliItem.ToString());
                        //MessageBox.Show(eskiFis.SatisKalem[seciliItem].UrunAd);
                        if (stornoFis == null)
                        {
                            stornoFis = new FisOlustur();
                            stornoFis.StornoFisYarat(0);
                            stornoFis.StornoIlgi = fisno;
                            stornoFis.Bon_name = "Position Storno";
                        }
                        SatisYap stornoSatis = new SatisYap();

                        decimal silinenUrunId = eskiFis.SatisKalem[seciliItem].UrunId;
                        double silinenmiktar = eskiFis.SatisKalem[seciliItem].Adet;
                        silinenSatisDetayidler.Add(eskiFis.SatisKalem[seciliItem].Satisid);

                        Tarih tarih = new Tarih();


                        if (silinenUrunId != 0)
                        {
                            /*daha sonra stok geri yukleme için*/
                            /* silinenler.Add(new KeyValuePair<string, de>("urunid", eskiFis.SatisKalem[seciliItem].UrunId));
                             silinenler.Add(new KeyValuePair<string, double>("adet", eskiFis.SatisKalem[seciliItem].Adet));
                             */
                            Artikel stornoUrun = new Artikel();
                            stornoUrun.ArtikelBulID(silinenUrunId);


                            stornoSatis = new SatisYap();
                            stornoSatis.Adet = eskiFis.SatisKalem[seciliItem].Adet;
                            stornoSatis.Fisno = stornoFis.SatisAnaId;
                            stornoSatis.KasaNo = Program.kasano;
                            stornoSatis.Mwst = stornoUrun.Mwst;
                            stornoSatis.Ustid_id = stornoSatis.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (stornoSatis.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            stornoSatis.Satisfiyat = eskiFis.SatisKalem[seciliItem].Satisfiyat;
                            stornoSatis.Tarih = tarih.unixdate(DateTime.Now);
                            stornoSatis.Toplamtutar = -eskiFis.SatisKalem[seciliItem].Satisfiyat * eskiFis.SatisKalem[seciliItem].Adet;
                            stornoSatis.UrunId = stornoUrun.ArtikelId;
                            stornoSatis.UrunAd = stornoUrun.ArtikelAd;
                            stornoSatis.Birimkar = 0;
                            stornoSatis.Grubid = stornoUrun.Grubid;
                            stornoSatis.Stornodurum = 1;
                            stornoSatis.StornoIlgi = fisno;
                            stornoSatis.Ustid_id = eskiFis.SatisKalem[seciliItem].Ustid_id;
                            stornoFis.paraustu = 0;
                            stornoFis.verilenpara = 0;
                            stornoFis.StornoIlgi = fisno;

                            stornoFis.SatisKalem.Add(stornoSatis);

                            stornoFis.FisiKapat();
                        }
                        else
                        {
                            UrunGrup stornoGrub = new UrunGrup(eskiFis.SatisKalem[seciliItem].Grubid);

                            double SatilanAdet = eskiFis.SatisKalem[seciliItem].Adet;
                            double SatisFiyat = eskiFis.SatisKalem[seciliItem].Satisfiyat;

                            if (SatilanAdet == 0) return;
                            if (SatisFiyat == 0) return;

                            stornoSatis = new SatisYap();
                            stornoSatis.Adet = SatilanAdet;
                            stornoSatis.Fisno = stornoFis.SatisAnaId;
                            stornoSatis.KasaNo = Program.kasano;
                            stornoSatis.Mwst = (int)stornoGrub.Mwst;
                            stornoSatis.Satisfiyat = SatisFiyat;
                            stornoSatis.Tarih = tarih.unixdate(DateTime.Now);
                            stornoSatis.Toplamtutar = -SatisFiyat * SatilanAdet;
                            stornoSatis.UrunId = 0;
                            stornoSatis.Birimkar = 0;
                            stornoSatis.UrunAd = eskiFis.SatisKalem[seciliItem].UrunAd;
                            stornoSatis.Stornodurum = 1;
                            stornoSatis.StornoIlgi = fisno;
                            stornoSatis.Ustid_id = stornoSatis.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (stornoSatis.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            stornoFis.StornoIlgi = fisno;
                            stornoFis.SatisKalem.Add(stornoSatis);
                            stornoSatis.Grubid = eskiFis.SatisKalem[seciliItem].Grubid;
                            stornoFis.FisiKapat();

                        }

                        eskiFis.SatisKalem.RemoveAt(seciliItem);
                        int i = 1;
                        listView1.Items.Clear();

                        foreach (SatisYap kalanlar in eskiFis.SatisKalem)
                        {
                            int listViewElaman = listView1.Items.Count;
                            listView1.Items.Add(i.ToString());
                            listView1.Items[listViewElaman].SubItems.Add(kalanlar.UrunAd.ToString() + "{" + kalanlar.Adet + "x" + kalanlar.Satisfiyat.ToString("C") + "}");
                            listView1.Items[listViewElaman].SubItems.Add(kalanlar.Toplamtutar.ToString("C"));
                            if (kalanlar.Stornodurum == 1)
                            {
                                listView1.Items[listViewElaman].SubItems.Add("*");
                            }
                            else
                            {
                                listView1.Items[listViewElaman].SubItems.Add("-");
                            }



                            /* int listViewElaman = listView1.Items.Count;
                             listView1.Items.Add(position.ToString());
                             listView1.Items[listViewElaman].SubItems.Add(urun.ArtikelAd.ToString() + "{" + urun.VkPreis + "x" + adet + "}");
                             listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));*/

                            i++;
                        }
                        eskiFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + eskiFis.toplamtutar.ToString("C");
                        //position = listView1.Items.Count + 1;
                        seciliItem = -1;
                    }
                    else
                    {
                        F_GenericError frmError = new F_GenericError();
                        frmError.lblMesaj.Text = Program.lang["90"]  ;
                        frmError.ShowDialog();
                        stornoFis = null;
                    }

                }
                else
                {
                    F_GenericError frmError = new F_GenericError();
                    frmError.lblMesaj.Text = Program.lang["90"];
                    frmError.ShowDialog();
                    return;
                }
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            Tarih tarih = new Tarih();
            List<SatisYap> silinecekler = new List<SatisYap>();
            List<SatisYap> YeniSatisKalemler = new List<SatisYap>();
            long ffno = 0;
            if (stornoFis == null)
            {
                stornoFis = new FisOlustur();
                stornoFis.StornoFisYarat(0);
                stornoFis.Bon_name = "Bon Storno";
                stornoFis.Bon_typ_id = (int)Bontype.AVBelegstorno;
                stornoFis.Bon_typ = "AVBelegstorno";

            }
            //SatisYap stornoSatis = new SatisYap();
            eskiFis.TseFinishSignatur = stornoFis.TseFinishSignatur;
            eskiFis.TseLogtime = stornoFis.TseLogtime;
            eskiFis.TseLogTimeStart = stornoFis.TseLogTimeStart;
            eskiFis.TseProcessData = stornoFis.TseProcessData;
            eskiFis.Tseseriennr = stornoFis.Tseseriennr;
            eskiFis.TseSignaturzahler = stornoFis.TseSignaturzahler;
            eskiFis.TseStartedTransaction = stornoFis.TseStartedTransaction;
            eskiFis.Datum_start = stornoFis.Datum_start;
            eskiFis.Bon_typ = stornoFis.Bon_typ;
            eskiFis.Bon_typ_id = stornoFis.Bon_typ_id;
            eskiFis.Bon_name = stornoFis.Bon_name;
            stornoFis = eskiFis;

            List<long> ilgiliStornoFisNo = new List<long>();
            List<SatisYap> eskiFisKalemleri;
            eskiFisKalemleri = eskiFis.SatisKalem.ToList<SatisYap>();
            stornoFis.SatisKalem.Clear();
            //eskiFis.SatisKalem = null;
            int aa = eskiFisKalemleri.Count;
            //SatisYap negatifSatisKalem = new SatisYap();
            //SatisYap eskikalemler= new SatisYap();
            //stornoFis.SatisKalem.Clear();
            int satkalsayi = eskiFis.SatisKalem.Count;
            foreach (SatisYap fiskalemleri in eskiFisKalemleri)
            {
                if (fiskalemleri.Stornodurum == 0)
                {
                    //Tarih tarih = new Tarih();
                    SatisYap negatifSatisKalem = new SatisYap();
                    silinecekler.Add(fiskalemleri);
                    negatifSatisKalem.Toplamtutar = -fiskalemleri.Toplamtutar;
                    if (fiskalemleri.Grubid == 7 || fiskalemleri.Grubid == 43 || fiskalemleri.Grubid == 6)
                    {
                        baglan = new db();
                        conn = baglan.myconn();
                        //bool ilkislem = true;
                        string UpdateSQL = "";
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        using (conn)
                        {
                            string sqlRbatTyp = "SELECT * FROM rabatt WHERE bonnr=" + eskiFis.SatisAnaId;
                            MySqlDataAdapter daRabatTyp = new MySqlDataAdapter(sqlRbatTyp, conn);
                            DataTable dtRabatTyp = new DataTable();
                            dtRabatTyp.Rows.Clear();
                            daRabatTyp.Fill(dtRabatTyp);
                            if (dtRabatTyp.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtRabatTyp.Rows.Count; a++) // lies alle rabattype
                                {
                                    if (Convert.ToInt16(dtRabatTyp.Rows[a].ItemArray[4]) == 0) //algemain oder position
                                    {
                                        RabattMain rbt = new RabattMain();
                                        rbt.RabatName = dtRabatTyp.Rows[a].ItemArray[2].ToString(); ;
                                        rbt.RabattAlani = 0;

                                        //yeniFis.Rabat += indirim;
                                        if (Convert.ToInt16(dtRabatTyp.Rows[a].ItemArray[3]) == 0) //Procent
                                        {
                                            rbt.RabattTyp = 0;
                                        }
                                        else //oder nachlass
                                        {
                                            rbt.RabattTyp = 1;
                                        }
                                        rbt.RabatArt = 0;
                                        rbt.RabatMenge = Convert.ToDouble(dtRabatTyp.Rows[a].ItemArray[11]);
                                        rbt.Grupid = 7;
                                        stornoFis.RabatList.Add(rbt);
                                        
                                        //yeniFis.FisiKapat();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        negatifSatisKalem.Adet = fiskalemleri.Adet;
                        negatifSatisKalem.Birimkar = fiskalemleri.Birimkar;
                        negatifSatisKalem.Fand = fiskalemleri.Fand;
                        negatifSatisKalem.Fand2 = fiskalemleri.Fand2;
                        negatifSatisKalem.Fisno = fiskalemleri.Fisno;
                        negatifSatisKalem.Grubid = fiskalemleri.Grubid;
                        negatifSatisKalem.KasaNo = Program.kasano;
                        negatifSatisKalem.KasiyerId = Program.bedID;
                        negatifSatisKalem.Mwst = fiskalemleri.Mwst;
                        negatifSatisKalem.Satisfiyat = fiskalemleri.Satisfiyat;
                        negatifSatisKalem.Satisid = fiskalemleri.Satisid;
                        negatifSatisKalem.Stornodurum = fiskalemleri.Stornodurum;
                        negatifSatisKalem.StornoIlgi = fiskalemleri.StornoIlgi;
                        negatifSatisKalem.Tarih = tarih.unixdate(DateTime.Now);
                        negatifSatisKalem.UrunAd = fiskalemleri.UrunAd;
                        negatifSatisKalem.UrunId = fiskalemleri.UrunId;
                        negatifSatisKalem.Ustid_id = fiskalemleri.Ustid_id;
                        negatifSatisKalem.Imhaus = fiskalemleri.Imhaus;
                        negatifSatisKalem.Gv_typ_id = fiskalemleri.Gv_typ_id;
                        negatifSatisKalem.Znr = fiskalemleri.Znr;
                        negatifSatisKalem.Gruptur = fiskalemleri.Gruptur;
                        //stornoFis.SatisKalem.Remove(fiskalemleri);
                        YeniSatisKalemler.Add(negatifSatisKalem);
                        negatifSatisKalem = null;
                    }
                }
                else
                {
                    if (!ilgiliStornoFisNo.Contains(fiskalemleri.StornoIlgi))
                    {
                        ilgiliStornoFisNo.Add(fiskalemleri.StornoIlgi);
                    }
                }

            }
            stornoFis.SatisAnaId = ffno;
            stornoFis.paraustu = 0;
            stornoFis.verilenpara = 0;
            stornoFis.toplamtutar = -stornoFis.toplamtutar;


            stornoFis.StornoIlgi = fisno;
            stornoFis.tarih = tarih.unixdate(DateTime.Now);
            stornoFis.odemesekli = 2;

            /*   if (silinecekler.Count > 0)
               {
                   foreach (SatisYap silineneleman in silinecekler)
                   {
                 stornoFis.SatisKalem.Remove(silineneleman);
                   }
               }*/

            if (YeniSatisKalemler.Count > 0)
            {
                foreach (SatisYap yeniItem in YeniSatisKalemler)
                {
                    stornoFis.SatisKalem.Add(yeniItem);
                }

                stornoFis.ToplamStorno = stornoFis.toplamtutar;
                stornoFis.ToplamEc = 0;
                stornoFis.ToplamBar = 0;
                stornoFis.ToplamScheck = 0;
                stornoFis.FisiKapat();
                //stornoFis.ToplamScheck = stornoFis.toplammwst
                //stornoFis.toplamtutar = -stornoFis.toplamtutar;
                if (stornoFis.FisiSonlandır(2,stornoFis.SatisKalem ) == true)
                {
                    ffno = stornoFis.SatisAnaId;
                    StornoKompleKaydet(ffno);
                    foreach (RabattMain rbt in stornoFis.RabatList)
                    {
                        if (rbt.Grupid != 999)
                        {
                            SatisYap satisYap = new SatisYap();
                            satisYap.Adet = 1;
                            satisYap.Fisno = stornoFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = 0;
                            satisYap.Ustid_id=satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                            satisYap.Tarih = tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                            satisYap.UrunId = 0;
                            satisYap.Birimkar = 0;
                            satisYap.UrunAd = rbt.RabatName;
                            satisYap.Grubid = rbt.Grupid;
                           
                            stornoFis.SatisKalem.Add(satisYap);
                        }
                    }
                    foreach (SatisYap satisIcerik in stornoFis.SatisKalem)
                    {
                        satisIcerik.Fisno = ffno;
                        satisIcerik.Toplamtutar = satisIcerik.Toplamtutar;
                        satisIcerik.Kaydet();
                    }
                    
                    if (Program.bonDruck == true)
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                        stornoFis.Dublikat = false;
                        Program.BonBeleg.OdemeTur = 2;
                        Program.BonBeleg.basilacakFis = stornoFis;
                        is1.Start();
                    }
                    else
                    {
                        Program.bonDruck = true;
                    }

                    eskiFis = null;
                    this.Close();

                }
            }
            else
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["84"] + string.Join(",", ilgiliStornoFisNo);
                frmError.ShowDialog();
                stornoFis = null;
            }

        }




        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (stornoFis != null)
            {
                stornoFis.odemesekli = 2;
                stornoFis.ToplamScheck = stornoFis.toplamtutar;
                stornoFis.ToplamEc = 0;
                stornoFis.ToplamBar = 0;
                stornoFis.ToplamScheck = 0;
                int listViewElaman = listView1.Items.Count;
                if (listViewElaman > 0)
                {
                    foreach (SatisYap stsYap  in stornoFis.SatisKalem)
                    {
                        if (stsYap.Grubid != 7 || stsYap.Grubid != 43 || stsYap.Grubid != 6)
                        {
                            baglan = new db();
                            conn = baglan.myconn();
                            //bool ilkislem = true;
                            string UpdateSQL = "";
                            if (conn.State == ConnectionState.Closed)
                            {
                                conn.Open();
                            }
                            using (conn)
                            {
                                string sqlRbatTyp = "SELECT * FROM rabatt WHERE bonnr=" + eskiFis.SatisAnaId;
                                MySqlDataAdapter daRabatTyp = new MySqlDataAdapter(sqlRbatTyp, conn);
                                DataTable dtRabatTyp = new DataTable();
                                dtRabatTyp.Rows.Clear();
                                daRabatTyp.Fill(dtRabatTyp);
                                if (dtRabatTyp.Rows.Count > 0)
                                {
                                    for (int a = 0; a < dtRabatTyp.Rows.Count; a++) // lies alle rabattype
                                    {
                                        if (Convert.ToInt16(dtRabatTyp.Rows[a].ItemArray[4]) == 0) //algemain oder position
                                        {
                                            RabattMain rbt = new RabattMain();
                                            rbt.RabatName = dtRabatTyp.Rows[a].ItemArray[2].ToString(); ;
                                            rbt.RabattAlani = 0;

                                            //yeniFis.Rabat += indirim;
                                            if (Convert.ToInt16(dtRabatTyp.Rows[a].ItemArray[3]) == 0) //Procent
                                            {
                                                rbt.RabattTyp = 0;
                                            }
                                            else //oder nachlass
                                            {
                                                rbt.RabattTyp = 1;
                                            }
                                            rbt.RabatArt = 0;
                                            rbt.RabatMenge = Convert.ToDouble(dtRabatTyp.Rows[a].ItemArray[11]);
                                            rbt.Grupid = 7;
                                            stornoFis.RabatList.Add(rbt);

                                            //yeniFis.FisiKapat();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stornoFis.ToplamStorno = stornoFis.toplamtutar;
                    if (stornoFis.FisiSonlandır(2, stornoFis.SatisKalem) == true)
                    {
                        foreach (RabattMain rbt in stornoFis.RabatList)
                        {
                            if (rbt.Grupid != 999)
                            {
                                SatisYap satisYap = new SatisYap();
                                satisYap.Adet = 1;
                                satisYap.Fisno = stornoFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = 0;
                                satisYap.Ustid_id=satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                satisYap.UrunId = 0;
                                satisYap.Birimkar = 0;
                                satisYap.UrunAd = rbt.RabatName;
                                satisYap.Grubid = rbt.Grupid;
                                stornoFis.SatisKalem.Add(satisYap);
                            }
                        }
                        StornoKaydet(fisno, stornoFis.SatisAnaId, silinenSatisDetayidler);
                        foreach (SatisYap satisIcerik in stornoFis.SatisKalem)
                        {
                            //satisIcerik.StornoKaydet(fisno);
                            satisIcerik.Fisno = stornoFis.SatisAnaId;
                            satisIcerik.Kaydet();
                        }
                        CheckForIllegalCrossThreadCalls = false;
                        Thread is1 = new Thread(new ThreadStart(this.fisyaz));

                        Program.BonBeleg.OdemeTur = 2;
                        Program.BonBeleg.basilacakFis = stornoFis;
                        is1.Start();

                        eskiFis = null;
                        this.Close();

                        //System.GC.SuppressFinalize(yeniFis);


                    }
                }
                else
                {
                    F_GenericError frmError = new F_GenericError();
                    frmError.lblMesaj.Text = Program.lang["90"];
                    frmError.ShowDialog();
                    stornoFis = null;
                }
            }
        }
        public void fisyaz2()
        {
            try
            {
                if (Program.PrinterLib == ".NET")
                {
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.basilacakFis.toplamtutar = barkodluFis.basilacakFis.toplamtutar - barkodluFis.basilacakFis.Rabattutar - barkodluFis.basilacakFis.PuanRabat;
                    barkodluFis.FisYaz();
                    /*  if (Program.GlobalAyarlar["LOGO"] == 1)
                      {
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.FisYaz();

                      }
                      else
                      {
                    Program.BonBeleg.FisYazdir();
                      }*/
                }
                else
                {
                    /*  FisOPOS barkodluFis = new FisOPOS();
                      // MessageBox.Show("barkodluFis");
                      barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                      barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                      barkodluFis.FisYaz();*/
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;

                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.basilacakFis.toplamtutar = barkodluFis.basilacakFis.toplamtutar - barkodluFis.basilacakFis.Rabattutar - barkodluFis.basilacakFis.PuanRabat;
                    barkodluFis.FisYaz();

                }
            }
            catch
            {
            }
            

        }
        public void fisyaz()
        {
            /*
            Program.BonBeleg.FisYazdir();
            this.Close();
             * */
            for (int i = 0; i < 2; i++)
            {
                if (Program.PrinterLib == ".NET")
                {
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.FisYaz();
                    /*  if (Program.GlobalAyarlar["LOGO"] == 1)
                      {
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.FisYaz();

                      }
                      else
                      {
                    Program.BonBeleg.FisYazdir();
                      }*/
                }
                else
                {
                    /*  FisOPOS barkodluFis = new FisOPOS();
                      // MessageBox.Show("barkodluFis");
                      barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                      barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                      barkodluFis.FisYaz();*/
                    FisBarkodlu barkodluFis = new FisBarkodlu();
                    barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                    barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                    barkodluFis.FisYaz();

                }
                
            }
            try
            {
                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                if (Program.printerType == "bixolon")
                {
                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                    
                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)25) + ((char)255));
                    
                    // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                }
                else
                {
                    // 
                }
                //KundenDisplay();
                // knddsply.VerkaufInfo("Herzlich Willkommen!", Program.IsletmeAyarlar["isletme"]);
                // knddsply.VerkaufInfo("BAR :\n" + yeniFis.verilenpara.ToString("C") + "\nRÜCKGELD \n:" + yeniFis.paraustu.ToString("C"), "");
            }
            catch { }



        }
        public void StornoKaydet(long fisno, long stornoIlgiNo, List<long> silinenSatisDetayidler)
        {
            baglan = new db();
            conn = baglan.myconn();
            //bool ilkislem = true;
            string UpdateSQL = "";
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            VirgulAyikla vA = new VirgulAyikla();
            //satisid``tarih``fisno``urunid``mwst``satisfiyat``adet``toplamtutar``birimkar``kasano`
            foreach (int id in silinenSatisDetayidler)
            {
                UpdateSQL = "update satisdetay SET  storno=1, stornoilgi=" + stornoIlgiNo + " WHERE fisno= " + fisno + " and satisid=" + id;
                //MessageBox.Show(InsertSQL);

                MySqlCommand coInsert = new MySqlCommand(UpdateSQL, conn);
                if (coInsert.ExecuteNonQuery() > 0)
                {
                    // MessageBox.Show("Hersey Yolunda!");
                }
            }
            conn.Close();
        }
        public void StornoKompleKaydet(long fff)
        {
            baglan = new db();
            conn = baglan.myconn();
            //bool ilkislem = true;
            string UpdateSQL = "";

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            VirgulAyikla vA = new VirgulAyikla();
            //satisid``tarih``fisno``urunid``mwst``satisfiyat``adet``toplamtutar``birimkar``kasano`
            foreach (SatisYap id in stornoFis.SatisKalem)
            {
                UpdateSQL = "update satisdetay SET  storno=1, stornoilgi=" + fff + " WHERE satisid= " + id.Satisid;
                //MessageBox.Show(InsertSQL);

                MySqlCommand coInsert = new MySqlCommand(UpdateSQL, conn);
                if (coInsert.ExecuteNonQuery() > 0)
                {
                    // MessageBox.Show("Hersey Yolunda!");
                }
            }

            conn.Close();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(listView1.Items[3].SubItems[2].Text);
        }

        private void btnTekrar_Click(object sender, EventArgs e)
        {
            if (tekrarFis != null)
            {
                tekrarFis = null;
            }
            if (tekrarFis == null)
            {
                tekrarFis = new FisOlustur();
            }
            if (tekrarFis.FisCagir(fisno) == true)
            {
                tekrarFis.Bewirtung = 0;
                CheckForIllegalCrossThreadCalls = false;
                Thread is2 = new Thread(new ThreadStart(this.fisyaz2));
                Program.BonBeleg.OdemeTur = tekrarFis.odemesekli;
                Program.BonBeleg.basilacakFis = tekrarFis;
                is2.Start();
            }
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            if (eskiFis != null)
            {
                eskiFis = null;
            }
            eskiFis = new FisOlustur();
            silinenSatisDetayidler.Clear();
            lblStorno.Visible = false;
            int position = 1;
            listView1.Items.Clear();
            fisno = fisno - 1;

            //FisOlustur eskiFis = new FisOlustur();
            if (eskiFis.FisCagir(fisno) == true)
            {
                StornoFisCheck(fisno);
                lFisno.Text = fisno.ToString();
                lblFisNo.Text = fisno.ToString();
                lblFisOdenmeSekli.Text = odemesekliBul(eskiFis.odemesekli);
                lblFisTarih.Text = tarih.tarih((long)eskiFis.tarih);
                lblKasiyer.Text = kasiyerBul(eskiFis.kasiyerno);
                lblTutar.Text = eskiFis.toplamtutar.ToString("C");
                txtToplam.Text = eskiFis.toplamtutar.ToString("C");
                if (eskiFis.odemesekli == 2)
                {
                    if (eskiFis.StornoIlgi != 0)
                    {
                        lblStorno.Text = Program.lang["84"] + eskiFis.StornoIlgi.ToString();
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }
                    else
                    {
                        lblStorno.Text = Program.lang["86"];
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }

                }
                else
                {
                    kryptonButton3.Enabled = true;
                    kryptonButton1.Enabled = true;
                    btnKaydet.Enabled = true;
                }
                foreach (SatisYap satislar in eskiFis.SatisKalem)
                {
                    int listViewElaman = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[listViewElaman].SubItems.Add(satislar.UrunAd.ToString() + "{" + satislar.Adet + "x" + satislar.Satisfiyat.ToString("C") + "}");
                    listView1.Items[listViewElaman].SubItems.Add(satislar.Toplamtutar.ToString("C"));
                    if (satislar.Stornodurum == 1)
                    {
                        listView1.Items[listViewElaman].SubItems.Add("*");
                    }
                    else
                    {
                        listView1.Items[listViewElaman].SubItems.Add("-");

                    }

                    if (satislar.UrunId != 99999)
                    {
                        position++;
                    }
                }


            }
            else
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["85"]; ;
                frmError.ShowDialog();
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            if (eskiFis != null)
            {
                eskiFis = null;
            }
            eskiFis = new FisOlustur();
            silinenSatisDetayidler.Clear();
            lblStorno.Visible = false;
            int position = 1;
            listView1.Items.Clear();
            fisno = fisno + 1;

            //FisOlustur eskiFis = new FisOlustur();
            if (eskiFis.FisCagir(fisno) == true)
            {
                StornoFisCheck(fisno);
                lFisno.Text = fisno.ToString();
                lblFisNo.Text = fisno.ToString();
                lblFisOdenmeSekli.Text = odemesekliBul(eskiFis.odemesekli);
                lblFisTarih.Text = tarih.tarih((long)eskiFis.tarih);
                lblKasiyer.Text = kasiyerBul(eskiFis.kasiyerno);
                lblTutar.Text = eskiFis.toplamtutar.ToString("C");
                txtToplam.Text = eskiFis.toplamtutar.ToString("C");
                if (eskiFis.odemesekli == 2)
                {
                    if (eskiFis.StornoIlgi != 0)
                    {
                        lblStorno.Text = Program.lang["84"] +eskiFis.StornoIlgi.ToString() ;
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }
                    else
                    {
                        lblStorno.Text = Program.lang["85"];
                        lblStorno.Visible = true;
                        kryptonButton1.Enabled = false;
                        btnKaydet.Enabled = false;
                        kryptonButton3.Enabled = false;
                    }

                }
                else
                {
                    kryptonButton3.Enabled = true;
                    kryptonButton1.Enabled = true;
                    btnKaydet.Enabled = true;
                }
                foreach (SatisYap satislar in eskiFis.SatisKalem)
                {
                    int listViewElaman = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[listViewElaman].SubItems.Add(satislar.UrunAd.ToString() + "{" + satislar.Adet + "x" + satislar.Satisfiyat.ToString("C") + "}");
                    listView1.Items[listViewElaman].SubItems.Add(satislar.Toplamtutar.ToString("C"));
                    if (satislar.Stornodurum == 1)
                    {
                        listView1.Items[listViewElaman].SubItems.Add("*");
                    }
                    else
                    {
                        listView1.Items[listViewElaman].SubItems.Add("-");

                    }

                    if (satislar.UrunId != 99999)
                    {
                        position++;
                    }
                }


            }
            else
            {
                F_GenericError frmError = new F_GenericError();
                frmError.lblMesaj.Text = Program.lang["85"]; ;
                frmError.ShowDialog();
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void btnBewirtung_Click(object sender, EventArgs e)
        {
            if (tekrarFis == null)
            {
                tekrarFis = new FisOlustur();
            }
            if (tekrarFis.FisCagir(fisno) == true)
            {
                tekrarFis.Bewirtung = 1;
                CheckForIllegalCrossThreadCalls = false;
                Thread is2 = new Thread(new ThreadStart(this.fisyaz2));
                Program.BonBeleg.OdemeTur = tekrarFis.odemesekli;
                Program.BonBeleg.basilacakFis = tekrarFis;
                is2.Start();
            }
        }
    }
}
