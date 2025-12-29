using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IS_KASSE
{
    public class AngebotList
    {
        db baglanti = new db();
        private string _angebotAd;

        public string AngebotAd
        {
            get { return _angebotAd; }
            set { _angebotAd = value; }
        }
        private double _baszaman;

        public double Baszaman
        {
            get { return _baszaman; }
            set { _baszaman = value; }
        }
        private double _bitzaman;

        public double Bitzaman
        {
            get { return _bitzaman; }
            set { _bitzaman = value; }
        }
        private string _aciklama;

        public string Aciklama
        {
            get { return _aciklama; }
            set { _aciklama = value; }
        }
        public AngebotList(int angebotId)
        {
            MySqlConnection myConn = new MySqlConnection();
            db baglanti = new db();
            using (myConn = baglanti.myconn())
            {
                if (myConn.State == System.Data.ConnectionState.Closed)
                {
                    myConn.Open();

                }
                MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from angebot where angebotid=" + angebotId, myConn);
                DataTable dtGrup = new DataTable("artikelgrup");
                dtGrup.Clear();
                myDaGrup.Fill(dtGrup);
                int kaysay = dtGrup.Rows.Count;
                if (kaysay > 0)
                {

                    this._angebotAd = dtGrup.Rows[0].ItemArray[1].ToString();
                    this._baszaman = (double)dtGrup.Rows[0].ItemArray[2];
                    this._bitzaman = (double)dtGrup.Rows[0].ItemArray[3];
                    this._aciklama = dtGrup.Rows[0].ItemArray[4].ToString();

                }
            }
        
        }


        internal void AngebotCheck()
        {
            VirgulAyikla  vA= new VirgulAyikla();
            MySqlConnection myConn = new MySqlConnection();
            db baglanti = new db();
            Tarih tarih = new Tarih();
            using (myConn = baglanti.myconn())
            {
                if (myConn.State == System.Data.ConnectionState.Closed)
                {
                    myConn.Open();

                }
                string durumSQL="SELECT * FROM angebot WHERE bitzaman < " + tarih.bugunBaslangic()+" AND aktif=1";
                MySqlDataAdapter myDaGrup = new MySqlDataAdapter(durumSQL, myConn);
                DataTable dtGrup = new DataTable("angebot");
                dtGrup.Clear();
                myDaGrup.Fill(dtGrup);
                int kaysay = dtGrup.Rows.Count;
                if (kaysay > 0)
                {
                    for (int i = 0; i < kaysay; i++)
                    {
                        string UpdateSQL = "UPDATE angebot SET aktif=0 WHERE angebotid=" + dtGrup.Rows[i].ItemArray[0];
                        MySqlCommand coUpdateAngebot = new MySqlCommand(UpdateSQL, myConn);
                        if (coUpdateAngebot.ExecuteNonQuery() > 0)
                        {
                            string UpdateArtikelSQL = "UPDATE artikel SET angebotdurum=0, angebotbaslamatarih=0, angebotbitistarih=0, angebotfiyat=0 WHERE angebotdurum=" + dtGrup.Rows[i].ItemArray[0];
                            MySqlCommand coUpdateArtikelAngebot = new MySqlCommand(UpdateArtikelSQL, myConn);
                            coUpdateArtikelAngebot.ExecuteNonQuery();
                        }
                    }

                }
                // Bekleyen Angebot
                string durumSQL2 = "SELECT * FROM angebot WHERE baszaman >= " + tarih.bugunBaslangic() + " AND baszaman<"+tarih.bugunBitis();
                MySqlDataAdapter myDaGrup2 = new MySqlDataAdapter(durumSQL2, myConn);
                DataTable dtGrup2 = new DataTable("angebot");
                dtGrup2.Clear();
                myDaGrup2.Fill(dtGrup2);
                int kaysay2 = dtGrup2.Rows.Count;
                if (kaysay2 > 0)
                {
                    for (int i = 0; i < kaysay2; i++)
                    {
                        string UpdateSQL = "UPDATE angebot SET aktif=1 WHERE angebotid=" + dtGrup2.Rows[i].ItemArray[0];
                        MySqlCommand coUpdateAngebot = new MySqlCommand(UpdateSQL, myConn);
                        if (coUpdateAngebot.ExecuteNonQuery() > 0)
                        {
                            //SELECT `icerikid`, `artikelid`, `artikelad`, `angebotid`, `angebotfiyat`, `fiyat`, `barcode` FROM `angeboticerik` WHERE 1
                            string SQLAngebotdetay = "SELECT * from angeboticerik WHERE angebotid=" + dtGrup2.Rows[i].ItemArray[0];
                            MySqlDataAdapter myDAList = new MySqlDataAdapter(SQLAngebotdetay, myConn);
                            DataTable dtAngebotListe = new DataTable();
                            dtAngebotListe.Rows.Clear();
                            myDAList.Fill(dtAngebotListe);
                            if (dtAngebotListe.Rows.Count > 0)
                            {
                                for(int a=0; a<dtAngebotListe.Rows.Count; a++)
                                {
                                    string UpdateArtikelSQL = "UPDATE artikel SET angebotdurum=" + dtGrup2.Rows[i].ItemArray[0] + ", angebotbaslamatarih=" + dtGrup2.Rows[i].ItemArray[2] + ", angebotbitistarih=" + dtGrup2.Rows[i].ItemArray[3] + ", angebotfiyat=" + vA.virgulayikla(Convert.ToDouble(dtAngebotListe.Rows[a].ItemArray[4])) + " WHERE artikelid=" + dtAngebotListe.Rows[a].ItemArray[1];
                                MySqlCommand coUpdateArtikelAngebot = new MySqlCommand(UpdateArtikelSQL, myConn);
                                coUpdateArtikelAngebot.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
