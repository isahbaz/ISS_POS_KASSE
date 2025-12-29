using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IS_KASSE
{
    public class Musteri
    {
        db Baglanti = new db();
        MySqlConnection myConn;

        private long _musteriNo;

        public long MusteriNo
        {
            get { return _musteriNo; }
            set { _musteriNo = value; }
        }
        private double _kullanilabilirKredi;

        public double KullanilabilirKredi
        {
            get { return _kullanilabilirKredi; }
            set { _kullanilabilirKredi = value; }
        }
        
        private double _yapilanHarcama;

        public double YapilanHarcama
        {
            get { return _yapilanHarcama; }
            set { _yapilanHarcama = value; }
        }
        private double _kazanilanPuan;

        public double KazanilanPuan
        {
            get { return _kazanilanPuan; }
            set { _kazanilanPuan = value; }
        }
        private double _grupIndirimOrani;

        public double GrupIndirimOrani
        {
            get { return _grupIndirimOrani; }
            set { _grupIndirimOrani = value; }
        }
        private double _bireyIndirimOrani;

        public double BireyIndirimOrani
        {
            get { return _bireyIndirimOrani; }
            set { _bireyIndirimOrani = value; }
        }
        private int _grupID;

        public int GrupID
        {
            get { return _grupID; }
            set { _grupID = value; }
        }
        private string _kundenName;

        public string KundenAd
        {
            get { return _kundenName; }
            set { _kundenName = value; }
        }
        private string _kundenSoyad;

        public string KundenSoyad
        {
            get { return _kundenSoyad; }
            set { _kundenSoyad = value; }
        }
        private double _kullanilabilirPuan;

        public double KullanilabilirPuan
        {
            get { return _kullanilabilirPuan; }
            set { _kullanilabilirPuan = value; }
        }
        private double _kredit;

        public double Kredit
        {
            get { return _kredit; }
            set { _kredit = value; }
        }

        private double _toplamKredit;

        public double ToplamKredit
        {
            get { return _toplamKredit; }
            set { _toplamKredit = value; }
        }

        private int _methode;

        public int Methode
        {
            get { return _methode; }
            set { _methode = value; }
        }

        private double _ozelOran;

        public double OzelOran
        {
            get { return _ozelOran; }
            set { _ozelOran = value; }
        }

        private string _adSoyad;

        public string AdSoyad
        {
            get { return _adSoyad; }
            set { _adSoyad = value; }
        }
        private string _barkod;

        public string Barkod
        {
            get { return _barkod; }
            set { _barkod = value; }
        }


        public void MusteriMusteriNO(long MusteriNo)
        {
            string SelectSQL = "";
            //myConn = Baglanti.myconn();
            using (myConn = Baglanti.myconn())
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
               
                    SelectSQL = "SELECT * FROM kunden WHERE kundenid=" + MusteriNo;
                
                MySqlCommand cmdKunde = new MySqlCommand(SelectSQL, myConn);
                MySqlDataReader drKunde = cmdKunde.ExecuteReader();
                
                while(drKunde.Read())
                {
                    this.MusteriNo = MusteriNo;
                    this.GrupID = (int)drKunde.GetInt16(9);
                    this.BireyIndirimOrani = (int)drKunde.GetInt16(21);
                    this.KullanilabilirPuan = drKunde.GetDouble(18) - drKunde.GetDouble(19);
                    this.KullanilabilirKredi =Math.Round(this.KullanilabilirPuan/Convert.ToDouble(Program.IsletmeAyarlar["harcamapuan"]),2);
                    this.Barkod = drKunde.GetString(11);
                    this.KundenAd = drKunde.GetString(1);
                    this.KundenSoyad = drKunde.GetString(2);
                    this.Methode = drKunde.GetInt16(10);
                    this.OzelOran = drKunde.GetDouble(23);
                    this.ToplamKredit = drKunde.GetDouble(22);
                    this.AdSoyad = this.KundenAd + " " + this.KundenSoyad;
                }
                drKunde.Close();
               /* MySqlDataAdapter daKunde = new MySqlDataAdapter(SelectSQL, myConn);
                DataTable dtKunde = new DataTable();
                dtKunde.Rows.Clear();
                daKunde.Fill(dtKunde);
                if (dtKunde.Rows.Count > 0)
                {
                    this.GrupID = (int)dtKunde.Rows[0].ItemArray[9];
                    this.BireyIndirimOrani = (int)dtKunde.Rows[0].ItemArray[21];
                    this.KullanilabilirKredi = Convert.ToDouble(dtKunde.Rows[0].ItemArray[17]) - Convert.ToDouble(dtKunde.Rows[0].ItemArray[20]);
                    this.KullanilabilirPuan = Convert.ToDouble(dtKunde.Rows[0].ItemArray[18]) - Convert.ToDouble(dtKunde.Rows[0].ItemArray[19]);
                    this.KundenAd = dtKunde.Rows[0].ItemArray[1].ToString();
                    this.KundenSoyad = dtKunde.Rows[0].ItemArray[2].ToString();
                    this.Methode = (int)dtKunde.Rows[0].ItemArray[10];
                    this.OzelOran = Convert.ToDouble(dtKunde.Rows[0].ItemArray[23]);
                    

                }*/
                
            }
        }
        public void MusteriBarkod(string Barkod)
        {
            string SelectSQL = "";
            //myConn = Baglanti.myconn();
            using (myConn = Baglanti.myconn())
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                
                    SelectSQL = "SELECT * FROM kunden WHERE barkode LIKE '%" + Barkod+"%'";
                
                MySqlCommand cmdKunde = new MySqlCommand(SelectSQL, myConn);
                MySqlDataReader drKunde = cmdKunde.ExecuteReader();
                while(drKunde.Read())
                {
                    this.MusteriNo = (long)drKunde.GetInt16(0);
                    this.GrupID = (int)drKunde.GetInt16(9);
                    this.BireyIndirimOrani = (int)drKunde.GetInt16(21);
                    this.KullanilabilirPuan = drKunde.GetDouble(18) - drKunde.GetDouble(19);
                    this.KullanilabilirKredi =Math.Round(this.KullanilabilirPuan/Convert.ToDouble(Program.IsletmeAyarlar["harcamapuan"]),2);
                    this.Barkod = drKunde.GetString(11);
                    this.KundenAd = drKunde.GetString(1);
                    this.KundenSoyad = drKunde.GetString(2);
                    this.Methode = drKunde.GetInt16(10);
                    this.OzelOran = drKunde.GetDouble(23);
                    this.ToplamKredit= drKunde.GetDouble(22);
                    this.AdSoyad = this.KundenAd + " " + this.KundenSoyad;
                }
                drKunde.Close();
               /* MySqlDataAdapter daKunde = new MySqlDataAdapter(SelectSQL, myConn);
                DataTable dtKunde = new DataTable();
                dtKunde.Rows.Clear();
                daKunde.Fill(dtKunde);
                if (dtKunde.Rows.Count > 0)
                {
                    this.GrupID = (int)dtKunde.Rows[0].ItemArray[9];
                    this.BireyIndirimOrani = (int)dtKunde.Rows[0].ItemArray[21];
                    this.KullanilabilirKredi = Convert.ToDouble(dtKunde.Rows[0].ItemArray[17]) - Convert.ToDouble(dtKunde.Rows[0].ItemArray[20]);
                    this.KullanilabilirPuan = Convert.ToDouble(dtKunde.Rows[0].ItemArray[18]) - Convert.ToDouble(dtKunde.Rows[0].ItemArray[19]);
                    this.KundenAd = dtKunde.Rows[0].ItemArray[1].ToString();
                    this.KundenSoyad = dtKunde.Rows[0].ItemArray[2].ToString();
                    this.Methode = (int)dtKunde.Rows[0].ItemArray[10];
                    this.OzelOran = Convert.ToDouble(dtKunde.Rows[0].ItemArray[23]);
                    

                }*/
                
            }
        }

    }
}
