using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data;

namespace IS_KASSE
{
    public class Artikel
    {
        public bool urunvarmi = false;
        private long _artikelId;

        public long ArtikelId
        {
            get { return _artikelId; }
            set { _artikelId = value; }
        }
        private string _barkodNo;

        public string BarkodNo
        {
            get { return _barkodNo; }
            set { _barkodNo = value; }
        }
        private string _artikelAd;

        public string ArtikelAd
        {
            get { return _artikelAd; }
            set { _artikelAd = value; }
        }
        private int _grubid;

        public int Grubid
        {
            get { return _grubid; }
            set { _grubid = value; }
        }
        private int _liferantId;//

        public int LiferantId
        {
            get { return _liferantId; }
            set { _liferantId = value; }
        }
        private int _firmaId;// A liferatntı da B liferantıda X marka ürün satabilir. Bu x firma adı, sera mesela

        public int FirmaId
        {
            get { return _firmaId; }
            set { _firmaId = value; }
        }
        private int _mwst;

        public int Mwst
        {
            get { return _mwst; }
            set { _mwst = value; }
        }
        private double _ekPreis;

        public double EkPreis
        {
            get { return _ekPreis; }
            set { _ekPreis = value; }
        }
        private double _vkPreis;

        public double VkPreis
        {
            get { return _vkPreis; }
            set { _vkPreis = value; }
        }
        private double _einheit;

        public double Einheit
        {
            get { return _einheit; }
            set { _einheit = value; }
        }
        private int _artikeltur;//tartilma olayı

        public int Artikeltur
        {
            get { return _artikeltur; }
            set { _artikeltur = value; }
        }
        private Int32 _toplamStok;

        public Int32 ToplamStok
        {
            get { return _toplamStok; }
            set { _toplamStok = value; }
        }
        private double _karMiktari;

        public double KarMiktari
        {
            get { return _karMiktari; }
            set { _karMiktari = value; }
        }
        private int _angebotVarmi;

        public int AngebotVarmi
        {
            get { return _angebotVarmi; }
            set { _angebotVarmi = value; }
        }
        private double _angebotBaslamaTarihi;

        public double AngebotBaslamaTarihi
        {
            get { return _angebotBaslamaTarihi; }
            set { _angebotBaslamaTarihi = value; }
        }
        private double _angebotBitistarihi;

        public double AngebotBitistarihi
        {
            get { return _angebotBitistarihi; }
            set { _angebotBitistarihi = value; }
        }
        private double _angebotFiyati;

        public double AngebotFiyati
        {
            get { return _angebotFiyati; }
            set { _angebotFiyati = value; }
        }
        private int _fand;

        public int Fand
        {
            get { return _fand; }
            set { _fand = value; }
        }
        private int _fand2;

        public int Fand2
        {
            get { return _fand2; }
            set { _fand2 = value; }
        }
        private int _fand3;

        public int Fand3
        {
            get { return _fand3; }
            set { _fand3 = value; }
        }
        private int _punkterabatdurum;

        public int Punkterabatdurum
        {
            get { return _punkterabatdurum; }
            set { _punkterabatdurum = value; }
        }
        private int _partnerschaft;

        public int Partnerschaft
        {
            get { return _partnerschaft; }
            set { _partnerschaft = value; }
        }

        private int _gruptur;

        public int Gruptur
        {
            get { return _gruptur; }
            set { _gruptur = value; }
        }
        private double _tara;

        public double Tara
        {
            get { return _tara; }
            set { _tara = value; }
        }

        private double _vkPreis2;

        public double VkPreis2
        {
            get { return _vkPreis2; }
            set { _vkPreis2 = value; }
        }
        private double _vkPreis3;

        public double VkPreis3
        {
            get { return _vkPreis3; }
            set { _vkPreis3 = value; }
        }
        private int _stafellpreisflag;

        public int StafellpreisFlag
        {
            get { return _stafellpreisflag; }
            set { _stafellpreisflag = value; }
        }

        private int _kundenPreisFlag;

        public int KundenPreisFlag
        {
            get { return _kundenPreisFlag; }
            set { _kundenPreisFlag = value; }
        }

        private int _partnerProduktFlag;

        public int PartnerProduktFlag
        {
            get { return _partnerProduktFlag; }
            set { _partnerProduktFlag = value; }
        }
        public void ArtikelBulPLU(string BarkodNumarasi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    BarkodNumarasi = BarkodNumarasi.Replace("\r", "");
                    BarkodNumarasi = BarkodNumarasi.Replace("\n", "");
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * FROM artikel WHERE barkod like " + BarkodNumarasi , conn);
                    DataTable dtArtikel = new DataTable("artikel");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    {
                        try
                        {
                            this.urunvarmi = true;
                            this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                            this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                            this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                            this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                            this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                            this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                            this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                            this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                            this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                            this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                            this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                            this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                            this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                            this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                            this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                            this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                            this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                            this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                            this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                            this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                            this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                            this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                            this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                            this.VkPreis2 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[42]);
                            this.VkPreis3 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[43]);
                            this.StafellpreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[38]);
                            this.KundenPreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[37]);
                            this.PartnerProduktFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[46]);
                            conn.Close();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        this.urunvarmi = false;
                    }
                }
                catch (MySqlException myex)
                {
                    this.urunvarmi = false;
                    System.Windows.Forms.MessageBox.Show("ERROR!\n ERROR TEXT(Nr:Art_232): " + myex.Message + "\nERROR NR: " + myex.Number);
                }
            }

        }
        public void ArtikelBul(string BarkodNumarasi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    BarkodNumarasi = BarkodNumarasi.Replace("\r", "");
                    BarkodNumarasi = BarkodNumarasi.Replace("\n", "");
                    string SQL = "";
                    if (BarkodNumarasi.Length >= 8)
                    {
                        SQL="SELECT * FROM artikel WHERE barkod like '" + BarkodNumarasi+"%'";
                    }
                    else
                    {
                        BarkodNumarasi = Convert.ToInt64(BarkodNumarasi).ToString(); // für Arkadash Markt:Aborgin 014
                        SQL="SELECT * FROM artikel WHERE barkod  like '" + BarkodNumarasi+"'";
                    }
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter( SQL, conn);
                    DataTable dtArtikel = new DataTable("artikel");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    {
                        try
                        {
                            this.urunvarmi = true;
                            this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                            this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                            this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                            this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                            this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                            this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                            this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                            this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                            this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                            this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                            this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                            this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                            this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                            this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                            this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                            this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                            this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                            this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                            this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                            this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                            this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                            this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                            this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                            this.VkPreis2 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[42]);
                            this.VkPreis3 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[43]);
                            this.StafellpreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[38]);
                            this.KundenPreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[37]);
                            this.PartnerProduktFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[46]);
                            conn.Close();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        this.urunvarmi = false;
                    }
                }
                catch (MySqlException myex)
                {
                    this.urunvarmi = false;
                    System.Windows.Forms.MessageBox.Show("HATA!\n ORJINAL HATA MESAJI(Nr:Art_232): " + myex.Message + "\nORJINAL HATA NO: " + myex.Number);
                }
            }

        }
        public void ArtikelBulFleich(string BarkodNumarasi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikel where barkod LIKE '" + BarkodNumarasi + "%'", conn);
                    DataTable dtArtikel = new DataTable("artikel");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    {
                        try
                        {
                            this.urunvarmi = true;
                            this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                            this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                            this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                            this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                            this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                            this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                            this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                            this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                            this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                            this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                            this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                            this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                            this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                            this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                            this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                            this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                            this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                            this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                            this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                            this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                            this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                            this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                            this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                            conn.Close();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        this.urunvarmi = false;
                    }
                }
                catch (MySqlException myex)
                {
                    this.urunvarmi = false;
                    System.Windows.Forms.MessageBox.Show("HATA!\n ORJINAL HATA MESAJI(Nr:Art_232): " + myex.Message + "\nORJINAL HATA NO: " + myex.Number);
                }
            }

        }
        public void ArtikelBulGewicht(string BarkodNumarasi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikel where barkod like  '" + BarkodNumarasi + "%' ORDER BY satisfiyat DESC", conn);
                    DataTable dtArtikel = new DataTable("artikel");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    {
                        try
                        {
                            this.urunvarmi = true;
                            this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                            this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                            this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                            this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                            this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                            this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                            this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                            this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                            this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                            this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                            this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                            this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                            this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                            this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                            this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                            this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                            this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                            this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                            this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                            this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                            this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                            this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                            this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                            conn.Close();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        this.urunvarmi = false;
                    }
                }
                catch (MySqlException myex)
                {
                    this.urunvarmi = false;
                    System.Windows.Forms.MessageBox.Show("HATA!\n ORJINAL HATA MESAJI(Nr:Art_232): " + myex.Message + "\nORJINAL HATA NO: " + myex.Number);
                }
            }

        }
        public void ArtikelBulGewichtOnlyPLU(string BarkodNumarasi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikel where barkod like  '" + BarkodNumarasi + "' ORDER BY satisfiyat DESC", conn);
                    DataTable dtArtikel = new DataTable("artikel");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    {
                        try
                        {
                            this.urunvarmi = true;
                            this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                            this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                            this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                            this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                            this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                            this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                            this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                            this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                            this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                            this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                            this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                            this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                            this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                            this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                            this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                            this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                            this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                            this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                            this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                            this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                            this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                            this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                            this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                            conn.Close();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        this.urunvarmi = false;
                    }
                }
                catch (MySqlException myex)
                {
                    this.urunvarmi = false;
                    System.Windows.Forms.MessageBox.Show("HATA!\n ORJINAL HATA MESAJI(Nr:Art_232): " + myex.Message + "\nORJINAL HATA NO: " + myex.Number);
                }
            }

        }
        public void ArtikelBulID(decimal id)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            using (conn = baglanti.myconn())
            {
                conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikel where artikelid=" + id, conn);
                DataTable dtArtikel = new DataTable("artikel");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {
                    this.urunvarmi = true;
                    this.ArtikelId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                    this.BarkodNo = dtArtikel.Rows[0].ItemArray[1].ToString();
                    this.ArtikelAd = dtArtikel.Rows[0].ItemArray[2].ToString();
                    this.Grubid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                    this.LiferantId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[4]);
                    this.FirmaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[5]);
                    this.Mwst = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[6]);
                    this.EkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                    this.VkPreis = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                    this.Einheit = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[9]);
                    this.KarMiktari = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[10]);
                    this.ToplamStok = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[11]);
                    this.Artikeltur = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[12]);
                    this.AngebotVarmi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[13]);
                    this.AngebotBaslamaTarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                    this.AngebotBitistarihi = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[15]);
                    this.AngebotFiyati = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[16]);
                    this.Fand = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[22]);
                    this.Fand2 = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[27]);
                    this.Partnerschaft = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[28]);
                    this.Punkterabatdurum = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[30]);
                    this.Gruptur = new ArtikelGrup(Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3])).GrupTur;
                    this.Tara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[31]);
                    this.VkPreis2 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[42]);
                    this.VkPreis3 = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[43]);
                    this.StafellpreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[38]);
                    this.KundenPreisFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[37]);
                    this.PartnerProduktFlag = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[46]);
                    conn.Close();

                }
                else
                {
                    this.urunvarmi = false;
                }
            }
        }
        public List<Dictionary<List<string>, double>> UrunIliskileri(string barkod, List<Dictionary<List<string>, double>> mevcutIliskiler)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            List<string> barkodlarList = new List<string>();
            Dictionary<List<string>, double> iliski = new Dictionary<List<string>, double>();
            List<Dictionary<List<string>, double>> iliskiler = new List<Dictionary<List<string>, double>>();
            using (conn = baglanti.myconn())
            {
                conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                try
                {
                    //System.Windows.Forms.MessageBox.Show(BarkodNumarasi);
                    string partnerProdukt = "SELECT * FROM partnerprodukt  WHERE partnersbarkod like '%" + barkod + "%'";
                    MySqlCommand cmdPartnerProdukt = new MySqlCommand(partnerProdukt, conn);
                    MySqlDataReader drPartners = cmdPartnerProdukt.ExecuteReader();

                    if (drPartners.HasRows)
                    {
                        int i = 0;
                        while (drPartners.Read())
                        {
                            string[] barkodlar = drPartners.GetString(1).Split(',');
                            foreach (string brkd in barkodlar)
                            {
                                barkodlarList.Add(brkd);
                            }
                            iliski.Add(barkodlarList, drPartners.GetDouble(2));
                            if (mevcutIliskiler.Count > 0)
                            {
                                foreach (Dictionary<List<string>, double> mevcutIliski in mevcutIliskiler)
                                {
                                    bool result2 = mevcutIliski.OrderBy(r => r.Key).SequenceEqual(iliski.OrderBy(r => r.Key));
                                    if (result2 != true)
                                    {
                                        iliskiler.Add(iliski);
                                    }

                                }
                            }
                            else
                            {
                                iliskiler.Add(iliski);
                            }


                            i++;
                            //iliskiler.Add(iliski);
                        }
                        return iliskiler;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

        }

    }
}
