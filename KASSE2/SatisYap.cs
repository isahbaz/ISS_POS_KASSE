using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.Layout;
using iss_ebon;
using iss_Ronsson;
//using Mysqlx.Crud;
using Newtonsoft.Json.Linq;
//using static Mysqlx.Notice.Warning.Types;
using System.IO;

namespace IS_KASSE
{
    public class SatisYap
    {

        MySqlConnection conn1 = new MySqlConnection();
        private double _tarih;
        private string _urunAd;
        private long _fisno;
        private decimal _urunId;
        private double _mwst;
        private double _satisfiyat;
        private double _alisfiyat;
        private double _toplamtutar;
        private double _adet;
        private double _birimkar;
        private int _kasaNo;
        private int _grubid;
        private int _kasiyerId;
        private int _fand;
        private long _satisid;
        private long _stornoIlgi;
        private int _fand2;
        private int _angebotvarmi;
        private double _angebotfiyat;
        private int _punkterabatdurum;
        private string _barkod;
        private double _birimid;
        private double _nettosatisfiyat;
        private double _prozisyonRabatBetrag;
        private Int32 _pozRabatMainId;
        public int staffelFlag { get; set; }
        public int parnerProduktFlag { get; set; }
        public int staffelOK { get; set; }
        public int partnerOK { get; set; }
        public List<SatisYap> stafellProdukte = new List<SatisYap>();
        public Int32 PozRabatMainId
        {
            get { return _pozRabatMainId; }
            set { _pozRabatMainId = value; }
        }

        public double ProzisyonRabatBetrag
        {
            get { return _prozisyonRabatBetrag; }
            set { _prozisyonRabatBetrag = value; }
        }

        

        public double Nettosatisfiyat
        {
            get { return _nettosatisfiyat; }
            set { _nettosatisfiyat = value; }
        }

        public double Birimid
        {
            get { return _birimid; }
            set { _birimid = value; }
        }
        public string Barkod
        {
            get { return _barkod; }
            set { _barkod = value; }
        }

        public int Punkterabatdurum
        {
            get { return _punkterabatdurum; }
            set { _punkterabatdurum = value; }
        }


        public double Angebotfiyat
        {
            get { return _angebotfiyat; }
            set { _angebotfiyat = value; }
        }

        public int Angebotvarmi
        {
            get { return _angebotvarmi; }
            set { _angebotvarmi = value; }
        }


        public int Fand2
        {
            get { return _fand2; }
            set { _fand2 = value; }
        }

        public long StornoIlgi
        {
            get { return _stornoIlgi; }
            set { _stornoIlgi = value; }
        }

        public long Satisid
        {
            get { return _satisid; }
            set { _satisid = value; }
        }
        private int _stornodurum;




        public int Stornodurum
        {
            get { return _stornodurum; }
            set { _stornodurum = value; }
        }

        public int Fand
        {
            get { return _fand; }
            set { _fand = value; }
        }

        public int KasiyerId
        {
            get { return Program.bedID; }
            set { _kasiyerId = Program.bedID; }
        }


        public string UrunAd
        {
            get { return _urunAd; }
            set { _urunAd = value; }
        }

        public double Tarih
        {
            get { return _tarih; }
            set { _tarih = value; }
        }


        public long Fisno
        {
            get { return _fisno; }
            set { _fisno = value; }
        }


        public decimal UrunId
        {
            get { return _urunId; }
            set { _urunId = value; }
        }


        public double Mwst
        {
            get { return _mwst; }
            set { _mwst = value; }
        }


        public double Satisfiyat
        {
            get { return _satisfiyat; }
            set { _satisfiyat = value; }
        }

        public double Alisfiyat
        {
            get { return _alisfiyat; }
            set { _alisfiyat = value; }
        }


        public double Toplamtutar
        {
            get { return _toplamtutar; }
            set { _toplamtutar = value; }
        }


        public double Adet
        {
            get { return _adet; }
            set { _adet = value; }
        }


        public double Birimkar
        {
            get { return _birimkar; }
            set { _birimkar = value; }
        }


        public int KasaNo
        {
            get { return _kasaNo; }
            set { _kasaNo = value; }
        }



        public int Grubid
        {
            get { return _grubid; }
            set { _grubid = value; }
        }
        private int _iliskilendirme;

        public int Iliskilendirme
        {
            get { return _iliskilendirme; }
            set { _iliskilendirme = value; }
        }

        private int _gruptur;

        public int Gruptur
        {
            get { return _gruptur; }
            set { _gruptur = value; }
        }
        private double _einheit;

        public double Einheit
        {
            get { return _einheit; }
            set { _einheit = value; }
        }
        private long _masaDBId;

        public long MasaDBId
        {
            get { return _masaDBId; }
            set { _masaDBId = value; }
        }

        private long _masaDetailId;

        public long MasaDetailId
        {
            get { return _masaDetailId; }
            set { _masaDetailId = value; }
        }
        private List<StaffelInfo> _staffelInfo;

        public List<StaffelInfo> StaffelInfo
        {
            get { return _staffelInfo; }
            set { _staffelInfo = value; }
        }
        private int gv_typ_id;

        public int Gv_typ_id
        {
            get { return gv_typ_id; }
            set { gv_typ_id = value; }
        }
        private int _imhaus;

        public int Imhaus
        {
            get { return _imhaus; }
            set { _imhaus = value; }
        }
        private int _ustid_id;

        public int Ustid_id
        {
            get { return _ustid_id; }
            set { _ustid_id = value; }
        }
        private int _znr;

        public int Znr
        {
            get { return _znr; }
            set { _znr = value; }
        }
        public int isHandyAuflade { get; set; }

        private Int32 _cardid;

        public Int32 Cardid
        {
            get { return _cardid; }
            set { _cardid = value; }
        }
        private string _internBarcode;

        public string InternBarcode
        {
            get { return _internBarcode; }
            set { _internBarcode = value; }
        }

        public bool IsKolli { get ; set ; }

        public double KolliKistaInhalt { get; set; }

        public int isSelbstKioskBestellung { get; set; }
        public string kioskBestellungMenuPLU { get; set; }

        public long kioskBestellID { get; set; }
       
        public string erpUUID { get; set; }
        public string erpID { get; set; } = "0";
        public string erpArtKey { get; set; }
        public string pathForAI { get; set; }
        public double AIScare {  get; set; }   
        public SatisYap()
        {
            /* db baglan = new db();
             conn = baglan.myconn();
             if (conn.State == ConnectionState.Closed)
             {
                 conn.Open();
             }
             */
            this.Gv_typ_id = (int)GVTypEnum.Umsatz;
        }

        public SatisYap ShallowCopy()
        {
            return (SatisYap)this.MemberwiseClone();
        }  
        public void Kaydet()
        {
            
            
            string InsertSQL = "";
            db baglan = new db();
            conn1 = baglan.myconn();
            if (conn1.State == ConnectionState.Closed)
            {
                conn1.Open();
            }
            VirgulAyikla vA = new VirgulAyikla(); //InsertSQL, conn1
            MySqlCommand coInsert = new MySqlCommand();
            //satisid``tarih``fisno``urunid``mwst``satisfiyat``adet``toplamtutar``birimkar``kasano`
          //MessageBox.Show(InsertSQL);
            if (this._barkod == null) this._barkod = "0";
            coInsert.Parameters.AddWithValue("@tarih", this._tarih);
            coInsert.Parameters.AddWithValue("@fisno", this._fisno);
            coInsert.Parameters.AddWithValue("@grupid", this._grubid);
            coInsert.Parameters.AddWithValue("@urunid", this._urunId);
            coInsert.Parameters.AddWithValue("@mwst", this._mwst);
            coInsert.Parameters.AddWithValue("@satisfiyat", this._satisfiyat);
            coInsert.Parameters.AddWithValue("@adet", this.IsKolli==false?this._adet:KolliKistaInhalt);
            coInsert.Parameters.AddWithValue("@toplamtutar", this._toplamtutar);
            coInsert.Parameters.AddWithValue("@birimkar", this._birimkar);
            coInsert.Parameters.AddWithValue("@kasano", this._kasaNo);
            coInsert.Parameters.AddWithValue("@kasiyerno", Program.bedID);
            coInsert.Parameters.AddWithValue("@urunad", this._urunAd);
            coInsert.Parameters.AddWithValue("@storno", this._stornodurum);
            coInsert.Parameters.AddWithValue("@stornoilgi", this._stornoIlgi);
            coInsert.Parameters.AddWithValue("@barkod", this._barkod);
            coInsert.Parameters.AddWithValue("@birimid", this.Birimid);
            coInsert.Parameters.AddWithValue("@nettosatisfiyat", this.Nettosatisfiyat);
            coInsert.Parameters.AddWithValue("@gv_typ_id", this.Gv_typ_id);
            coInsert.Parameters.AddWithValue("@imhaus", this.Imhaus);
            coInsert.Parameters.AddWithValue("@usid_id", this.Ustid_id);
            coInsert.Parameters.AddWithValue("@internBarcode", this.InternBarcode==null?"0":this.InternBarcode);
              InsertSQL = "INSERT INTO satisdetay SET tarih=@tarih,fisno= @fisno, grupid=@grupid, urunid=@urunid, mwst=@mwst" +
                ", satisfiyat=@satisfiyat, adet=@adet, toplamtutar=@toplamtutar,birimkar=@birimkar ,kasano=@kasano, kasiyerno=@kasiyerno,"+
                "urunAd=@urunad, storno=@storno, stornoilgi=@stornoilgi, barkod=@barkod,birimid=@birimid, nettovkpreis=@nettosatisfiyat,"+
                " gv_typ_id=@gv_typ_id, ust_id=@usid_id, imhaus=@imhaus, internbarcode=@internBarcode ";
              coInsert.CommandText = InsertSQL;
              coInsert.Connection = conn1;
            
            try
            {
                if (coInsert.ExecuteNonQuery() > 0)
                {
                    if (this.pathForAI != "") // AI icin Resim Loglama
                    {
                        try
                        {
                            string AISQL = "INSERT INTO `aiimagelog`( `path`, `score`, `level`, `bonnr`, barcode) VALUES(@path, @score, @level, @bonnr, @barcode)";
                            MySqlCommand cmdAIIns = new MySqlCommand();
                            cmdAIIns.Parameters.AddWithValue("@path", this.pathForAI);
                            cmdAIIns.Parameters.AddWithValue("@score", this.AIScare);
                            cmdAIIns.Parameters.AddWithValue("@level", 2); //1 :Ögrenem 2:Tahmin
                            cmdAIIns.Parameters.AddWithValue("@bonnr", this.Fisno); 
                                 cmdAIIns.Parameters.AddWithValue("@barcode", this.Barkod); 
                            cmdAIIns.CommandText = AISQL;
                            cmdAIIns.Connection = conn1;
                            cmdAIIns.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else
                    {
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("SQL:urun ID:" + this._urunId + ",\n" + this._urunAd +"\n"+ InsertSQL + "\n" + ex.Message + " " + ex.Number);
                new Log().AddtoLogFile("SQL:" + InsertSQL +
                "Urunad:" + this.UrunAd + "\n" +
                "Urunid:"+this.UrunId + "\n"  +
                "fisno:" + this.Fisno + "\n" +
                "grupid:" + this.Grubid + "\n" +
                "mwst:" + this.Mwst + "\n" +
                "satisfiyat:" + this.Satisfiyat + "\n" +
                "adet:" + this.Adet + "\n" +
                "toplamtutar:" + this.Toplamtutar + "\n" +
                "birimkar:" + this.Birimkar + "\n" +
                "kasano:" + this.KasaNo + "\n" +
                "kasiyerno:" + Program.bedID + "\n" +
                "storno:" + this.Stornodurum+ "\n" +
                "stornoilgi:" + this.StornoIlgi + "\n" +
                "barkod:" + this.Barkod + "\n" +
                "birimid:" + this.Birimid + "\n" +
                "nettosatisfiyat:" + this.Nettosatisfiyat + "\n" +
                ex.Message + " " +
                ex.Number, "Satisyap-295");
            }

            conn1.Close();
        }
        public void BestellungKaydet()
        {


            string InsertSQL = "";
            db baglan = new db();
            conn1 = baglan.myconn();
            if (conn1.State == ConnectionState.Closed)
            {
                conn1.Open();
            }
            VirgulAyikla vA = new VirgulAyikla(); //InsertSQL, conn1
            MySqlCommand coInsert = new MySqlCommand();
            //satisid``tarih``fisno``urunid``mwst``satisfiyat``adet``toplamtutar``birimkar``kasano`
            //MessageBox.Show(InsertSQL);
            if (this._barkod == null) this._barkod = "0";
            coInsert.Parameters.AddWithValue("@tarih", this._tarih);
            coInsert.Parameters.AddWithValue("@fisno", this._fisno);
            coInsert.Parameters.AddWithValue("@grupid", this._grubid);
            coInsert.Parameters.AddWithValue("@urunid", this._urunId);
            coInsert.Parameters.AddWithValue("@mwst", this._mwst);
            coInsert.Parameters.AddWithValue("@satisfiyat", this._satisfiyat);
            coInsert.Parameters.AddWithValue("@adet", this.IsKolli == false ? this._adet : KolliKistaInhalt);
            coInsert.Parameters.AddWithValue("@toplamtutar", this._toplamtutar);
            coInsert.Parameters.AddWithValue("@birimkar", this._birimkar);
            coInsert.Parameters.AddWithValue("@kasano", this._kasaNo);
            coInsert.Parameters.AddWithValue("@kasiyerno", Program.bedID);
            coInsert.Parameters.AddWithValue("@urunad", this._urunAd);
            coInsert.Parameters.AddWithValue("@storno", this._stornodurum);
            coInsert.Parameters.AddWithValue("@stornoilgi", this._stornoIlgi);
            coInsert.Parameters.AddWithValue("@barkod", this._barkod);
            coInsert.Parameters.AddWithValue("@birimid", this.Birimid);
            coInsert.Parameters.AddWithValue("@nettosatisfiyat", this.Nettosatisfiyat);
            coInsert.Parameters.AddWithValue("@gv_typ_id", this.Gv_typ_id);
            coInsert.Parameters.AddWithValue("@imhaus", this.Imhaus);
            coInsert.Parameters.AddWithValue("@usid_id", this.Ustid_id);
            coInsert.Parameters.AddWithValue("@internBarcode", this.InternBarcode == null ? "0" : this.InternBarcode);
            coInsert.Parameters.AddWithValue("@bestellID", this.kioskBestellID);
            coInsert.Parameters.AddWithValue("@menuPLU", this.kioskBestellungMenuPLU);
            //SELECT `satisid`, `tarih`, `fisno`, `bestellID`, `menuPLU`, `urunid`, `mwst`, `satisfiyat`, `adet`, `toplamtutar`, `birimkar`, `kasano`, `grupid`, `urunad`, `birimfiyat`,
            //`kasiyerno`, `storno`, `stornoilgi`, `barkod`, `birimid`, `nettovkpreis`, `gv_typ_id`, `ust_id`, `znr`, `gutschein_nr`, `imhaus`, `isPfand`, `isPfandRucknahme`, `internbarcode`, `kioskName` FROM `kioskbestellung` WHERE 1
            InsertSQL = "INSERT INTO kioskbestellung SET tarih=@tarih,fisno= @fisno, grupid=@grupid, urunid=@urunid, mwst=@mwst" +
              ", satisfiyat=@satisfiyat, adet=@adet, toplamtutar=@toplamtutar,birimkar=@birimkar ,kasano=@kasano, kasiyerno=@kasiyerno," +
              "urunAd=@urunad, storno=@storno, stornoilgi=@stornoilgi, barkod=@barkod,birimid=@birimid, nettovkpreis=@nettosatisfiyat," +
              " gv_typ_id=@gv_typ_id, ust_id=@usid_id, imhaus=@imhaus, internbarcode=@internBarcode ,  `bestellID`=@bestellID, `menuPLU`=@menuPLU";
            coInsert.CommandText = InsertSQL;
            coInsert.Connection = conn1;

            try
            {
                if (coInsert.ExecuteNonQuery() > 0)
                {
                    // MessageBox.Show("Hersey Yolunda!");
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show("SQL:urun ID:" + this._urunId + ",\n" + this._urunAd + "\n" + InsertSQL + "\n" + ex.Message + " " + ex.Number);
                new Log().AddtoLogFile("SQL:" + InsertSQL +
                    "Urunad:" + this.UrunAd + "\n" +
                    "Urunid:" + this.UrunId + "\n" +
                    "fisno:" + this.Fisno + "\n" +
                    "grupid:" + this.Grubid + "\n" +
                    "mwst:" + this.Mwst + "\n" +
                    "satisfiyat:" + this.Satisfiyat + "\n" +
                    "adet:" + this.Adet + "\n" +
                    "toplamtutar:" + this.Toplamtutar + "\n" +
                    "birimkar:" + this.Birimkar + "\n" +
                    "kasano:" + this.KasaNo + "\n" +
                    "kasiyerno:" + Program.bedID + "\n" +
                    "storno:" + this.Stornodurum + "\n" +
                    "stornoilgi:" + this.StornoIlgi + "\n" +
                    "barkod:" + this.Barkod + "\n" +
                    "birimid:" + this.Birimid + "\n" +
                    "nettosatisfiyat:" + this.Nettosatisfiyat + "\n" +
                    ex.Message + " " +
                    ex.Number, "Satisyap-295");

            }

            conn1.Close();
        }
        public void dbMasaIslem(FisOlustur MasaBilgisi, SatisYap SatisBilgisi)
        {
            db baglan = new db();
            //long masaDbid = 0;
            using (conn1 = baglan.myconn())
            {
                if (MasaBilgisi.MasaDBid > 0)
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1.Open();
                    }

                    MySqlCommand coDetail = new MySqlCommand();
                    coDetail.Parameters.AddWithValue("@fiyat", SatisBilgisi.Satisfiyat);
                    coDetail.Parameters.AddWithValue("@adet", SatisBilgisi.Adet);
                    coDetail.Parameters.AddWithValue("@toplamtutar", SatisBilgisi.Toplamtutar);
                    string SQLtext = "INSERT INTO masadetail SET tarih=" + MasaBilgisi.tarih + ",masadbid= " + MasaBilgisi.MasaDBid + ", grupid=" + SatisBilgisi.Grubid + ", urunid=" + SatisBilgisi.UrunId + ", mwst=" +
              SatisBilgisi.Mwst + ", satisfiyat=@fiyat, adet=@adet, toplamtutar=@toplamtutar, kasano=" + MasaBilgisi.KasaNo + ", kasiyerno=" + MasaBilgisi.kasiyerno + ", urunAd= '" + SatisBilgisi.UrunAd + "'";
                    coDetail.CommandText = SQLtext;
                    coDetail.Connection = conn1;
                    coDetail.ExecuteNonQuery();
                    MasaDetailId = coDetail.LastInsertedId;

                }

            }
        }
        public void dbMasaIslemUpdate(FisOlustur MasaBilgisi, SatisYap SatisBilgisi)
        {
            db baglan = new db();
            //long masaDbid = 0;
            using (conn1 = baglan.myconn())
            {
                if (MasaBilgisi.MasaDBid > 0)
                {
                    if (conn1.State == ConnectionState.Closed)
                    {
                        conn1.Open();
                    }

                    MySqlCommand coDetail = new MySqlCommand();
                    coDetail.Parameters.AddWithValue("@fiyat", SatisBilgisi.Satisfiyat);
                    coDetail.Parameters.AddWithValue("@adet", SatisBilgisi.Adet);
                    coDetail.Parameters.AddWithValue("@toplamtutar", SatisBilgisi.Toplamtutar);
                    string SQLtext = "UPDATE masadetail SET masadbid= " + MasaBilgisi.MasaDBid +" WHERE satisid="+SatisBilgisi.MasaDetailId;
                    coDetail.CommandText = SQLtext;
                    coDetail.Connection = conn1;
                    coDetail.ExecuteNonQuery();
                    MasaDetailId = coDetail.LastInsertedId;

                }

            }
        }

    }
}
