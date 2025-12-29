using Conn;
using iss_ebon;
using iss_tse_v2;
using MySql.Data.MySqlClient;
using RC_V1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace IS_KASSE
{
    class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
    }
    class Ayar
    {
        /* Ayar Id:
         * 1:Fiş Türü Barkodlu / Barkodsuz
         * 6:Indırım Türü % / €
         * 7:Fiş Turu logolu /Logosuz
         */

        MySqlConnection myConn = new MySqlConnection();
        dbConn baglanti = new dbConn();
        BackgroundWorker RemoteRegister = new BackgroundWorker();
        Tarih tarih = new Tarih();
        Log logEntry = new Log();
        Logger log = new Logger("LOG\\SYSTEMSTART\\SETUPs");

        // List<KeyValuePair<string, int>>();
        public Ayar()
        {

            //MessageBox.Show("Ayar Start!");
            RemoteRegister.DoWork += new System.ComponentModel.DoWorkEventHandler(RemoteRegister_DoWork);
            //System.Windows.Forms.MessageBox.Show("AYAR GIRIS");
            Program.IsletmeAyarlar = new Dictionary<string, string>();
            Program.lang = new Dictionary<string, string>();
            Program.GlobalAyarlar = new Dictionary<string, int>();
            Program.BonText = new List<string>();
            myConn = baglanti.myconn();
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    // System.Windows.Forms.MessageBox.Show("AYAR myconn kapalı");

                    try
                    {

                        baglanti.openConnection();
                        Program.DBUsername = baglanti.UserName;
                        Program.DBPass = baglanti.Pass;
                        Program.ServerIp = baglanti.ServerIp1;
                        myConn = baglanti.myconn();
                        if (myConn.State == ConnectionState.Open)
                        {
                            Program.connection = true;
                            log.Log("MYSQL Connection OK!");
                        }


                    }
                    catch (MySqlException hata)
                    {
                        // System.Windows.Forms.MessageBox.Show("Hata Mesajı:" + hata.Message + "\nHata Kodu:" + hata.Number);
                        if (hata.Number == 1042)
                        {
                            //F_ServerOff frmServerOff = new F_ServerOff();
                            //frmServerOff.ShowDialog();
                            /* if (frmServerOff.sonuc == false)
                             {
                                
                                 MessageBox.Show("Datenbank Error!");
                                 return;
                             }*/

                        }
                        return;
                    }
                    if (myConn.State == ConnectionState.Closed)
                    {
                        baglanti.openConnection();
                        myConn = baglanti.myconn();

                    }
                }
                //COrona Mwst Änderung
                CoronaCheck();
                //Mwst List

                try
                {
                    Program.MwStList = new List<int>();
                    MySqlDataAdapter daMwst = new MySqlDataAdapter("SELECT * FROM mwst ", myConn);
                    DataTable dtMwst = new DataTable("user");
                    dtMwst.Rows.Clear();
                    daMwst.Fill(dtMwst);
                    //System.Windows.Forms.MessageBox.Show("AYAR ayaralar kay say:"+ dtUserBul.Rows.Count);
                    if (dtMwst.Rows.Count > 0)
                    {
                        for (int b = 0; b < dtMwst.Rows.Count; b++)
                        {
                            Program.MwStList.Add((int)dtMwst.Rows[b].ItemArray[1]);
                        }
                        log.Log("MwST Table is OK, READ is OK!");
                    }
                    else
                    {
                        MessageBox.Show("MWST-Tabelle ist leer!!!! Bitte prüfen Sie Mwst-Tabelle!");
                        log.Log("MwST Table is Empty");
                    }
                }
                catch
                {
                    log.Log("MwST List Read ERROR!");
                }
                MySqlDataAdapter daIsletmeBul = new MySqlDataAdapter("SELECT * FROM isletme ", myConn);
                DataTable dtIsletmeBul = new DataTable("user");
                dtIsletmeBul.Rows.Clear();
                daIsletmeBul.Fill(dtIsletmeBul);
                //System.Windows.Forms.MessageBox.Show("AYAR ayaralar kay say:"+ dtUserBul.Rows.Count);
                if (dtIsletmeBul.Rows.Count > 0)
                {
                    Program.IsletmeAyarlar.Add("isletme", dtIsletmeBul.Rows[0].ItemArray[1].ToString());
                    Program.IsletmeAyarlar.Add("baslangic", dtIsletmeBul.Rows[0].ItemArray[2].ToString());
                    Program.IsletmeAyarlar.Add("strase", dtIsletmeBul.Rows[0].ItemArray[3].ToString());
                    Program.IsletmeAyarlar.Add("plz", dtIsletmeBul.Rows[0].ItemArray[4].ToString());
                    Program.IsletmeAyarlar.Add("stadt", dtIsletmeBul.Rows[0].ItemArray[5].ToString());
                    Program.IsletmeAyarlar.Add("usid", dtIsletmeBul.Rows[0].ItemArray[9].ToString());
                    Program.IsletmeAyarlar.Add("tel1", dtIsletmeBul.Rows[0].ItemArray[6].ToString());
                    Program.IsletmeAyarlar.Add("tel2", dtIsletmeBul.Rows[0].ItemArray[7].ToString());
                    Program.IsletmeAyarlar.Add("fax", dtIsletmeBul.Rows[0].ItemArray[8].ToString());
                    Program.IsletmeAyarlar.Add("inhaber", dtIsletmeBul.Rows[0].ItemArray[10].ToString());
                    Program.IsletmeAyarlar.Add("kod", dtIsletmeBul.Rows[0].ItemArray[11].ToString());
                    Program.IsletmeAyarlar.Add("kartrabat", dtIsletmeBul.Rows[0].ItemArray[12].ToString());
                    Program.IsletmeAyarlar.Add("licence", dtIsletmeBul.Rows[0].ItemArray[13].ToString());
                    Program.IsletmeAyarlar.Add("indirimbarkoduorani", dtIsletmeBul.Rows[0].ItemArray[14].ToString());
                    Program.IsletmeAyarlar.Add("indirimbarkoduhedefi", dtIsletmeBul.Rows[0].ItemArray[15].ToString());
                    Program.IsletmeAyarlar.Add("indirimbarkodualtsinir", dtIsletmeBul.Rows[0].ItemArray[16].ToString());
                    Program.IsletmeAyarlar.Add("land", dtIsletmeBul.Rows[0].ItemArray[20].ToString());

                    Program.IsletmeAyarlar.Add("steuernummer", dtIsletmeBul.Rows[0].ItemArray[21].ToString());
                    Program.IsletmeAyarlar.Add("filhauptid", dtIsletmeBul.Rows[0].ItemArray[24].ToString());
                    Program.IsletmeAyarlar.Add("filname", dtIsletmeBul.Rows[0].ItemArray[26].ToString());
                    Program.IsletmeAyarlar.Add("mail", dtIsletmeBul.Rows[0].ItemArray[25].ToString());
                    Program.IsletmeAyarlar.Add("grosshandel", dtIsletmeBul.Rows[0].ItemArray[28].ToString());
                    Program.IsletmeAyarlar.Add("markt", Program.markt == null ? dtIsletmeBul.Rows[0].ItemArray[29].ToString() : Program.markt);
                    Program.IsletmeAyarlar.Add("festrabatt", dtIsletmeBul.Rows[0].ItemArray[30].ToString());
                    Program.IsletmeAyarlar.Add("token", dtIsletmeBul.Rows[0].ItemArray[31].ToString());
                    Program.IsletmeAyarlar.Add("companyID", dtIsletmeBul.Rows[0].ItemArray[32].ToString());
                    Program.IsletmeAyarlar.Add("HandyAufladeUsername", dtIsletmeBul.Rows[0].ItemArray[33].ToString());
                    Program.IsletmeAyarlar.Add("HandyAufladePassword", dtIsletmeBul.Rows[0].ItemArray[34].ToString());
                    Program.IsletmeAyarlar.Add("HandyAufladeAPI", dtIsletmeBul.Rows[0].ItemArray[44].ToString());
                    Program.IsletmeAyarlar.Add("datevberaterno", dtIsletmeBul.Rows[0].ItemArray[35].ToString());
                    Program.IsletmeAyarlar.Add("datevusername", dtIsletmeBul.Rows[0].ItemArray[36].ToString());
                    Program.IsletmeAyarlar.Add("datevpassword", dtIsletmeBul.Rows[0].ItemArray[37].ToString());
                    Program.IsletmeAyarlar.Add("webshopusername", dtIsletmeBul.Rows[0].ItemArray[38].ToString());
                    Program.IsletmeAyarlar.Add("webshoppassword", dtIsletmeBul.Rows[0].ItemArray[39].ToString());
                    Program.IsletmeAyarlar.Add("webshopstoreid", dtIsletmeBul.Rows[0].ItemArray[40].ToString());
                    Program.IsletmeAyarlar.Add("eBonBearer", dtIsletmeBul.Rows[0].ItemArray[45].ToString());
                    Program.IsletmeAyarlar.Add("AufladeFirma", dtIsletmeBul.Rows[0].ItemArray[46].ToString());
                    Program.IsletmeAyarlar.Add("wv", dtIsletmeBul.Rows[0].ItemArray[48].ToString());
                }
                log.Log("Company Table(Isletme) is OK!");
                Program.oldUICulture = new System.Globalization.CultureInfo("de-DE");//Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                /*lang Default*/
                if (Program.IsletmeAyarlar["land"] == "de")
                {

                    MySqlDataAdapter daLang = new MySqlDataAdapter("SELECT id, de FROM lang ", myConn);
                    DataTable dtLang = new DataTable("lang");
                    dtLang.Rows.Clear();
                    daLang.Fill(dtLang);
                    if (dtLang.Rows.Count > 0)
                    {
                        if (Program.lang != null)
                        {
                            Program.lang.Clear();
                        }

                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                        for (int a = 0; a < dtLang.Rows.Count; a++)
                        {
                            Program.lang.Add(dtLang.Rows[a].ItemArray[0].ToString(), dtLang.Rows[a].ItemArray[1].ToString());

                        }
                    }
                }
                else
                {
                    MySqlDataAdapter daLang = new MySqlDataAdapter("SELECT id, en FROM lang ", myConn);
                    DataTable dtLang = new DataTable("lang");
                    dtLang.Rows.Clear();
                    daLang.Fill(dtLang);
                    if (dtLang.Rows.Count > 0)
                    {
                        if (Program.lang != null)
                        {
                            Program.lang.Clear();
                        }

                        // System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                        for (int a = 0; a < dtLang.Rows.Count; a++)
                        {
                            Program.lang.Add(dtLang.Rows[a].ItemArray[0].ToString(), dtLang.Rows[a].ItemArray[1].ToString());

                        }
                    }
                }


                log.Log("Remote Register Start!-BackGround Worker");
                RemoteRegister.RunWorkerAsync();
                log.Log("Register Kasse START!");
                RegisterKasa();

                if(myConn.IsDisposed)
                {
                    myConn = baglanti.myconn();
                }
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string kundenKartSql = "SELECT * FROM puanrabat";
                MySqlCommand drKunden = new MySqlCommand(kundenKartSql, myConn);
                MySqlDataReader dr = drKunden.ExecuteReader();
                while (dr.Read())
                {
                    Program.IsletmeAyarlar.Add("kartstandartpuan", dr.GetDouble(1).ToString());
                    Program.IsletmeAyarlar.Add("kartstandartrabat", dr.GetDouble(2).ToString());
                    Program.IsletmeAyarlar.Add("kusuratlipuan", dr.GetDouble(3).ToString());
                    Program.IsletmeAyarlar.Add("harcamapuan", dr.GetDouble(4).ToString());
                    Program.IsletmeAyarlar.Add("enazpuan", dr.GetDouble(5).ToString());

                }
                dr.Close();

                string bontextSQL = "SELECT * FROM bontext ORDER BY id ASC";
                MySqlCommand cmdBonText = new MySqlCommand(bontextSQL, myConn);
                MySqlDataReader drBonText = cmdBonText.ExecuteReader();
                while (drBonText.Read())
                {
                    Program.BonText.Add(drBonText.GetString(1));

                }
                drBonText.Close();


                /*if (Program.IsletmeAyarlar["licence"] != "")
                            {
                                Tarih tar = new Tarih();
                                long gerideger = 0, bugun = 0, gunSayisi = 0, hesaplananSonucTarih = 0, bugunTarih = 0, bugunBaslangic = 0;
                                string[] degerler = Program.IsletmeAyarlar["licence"].Split('-');

                                gerideger = long.Parse(degerler[0], System.Globalization.NumberStyles.HexNumber);
                                // lblRbugun.Text = long.Parse(lblhex.Text, System.Globalization.NumberStyles.HexNumber).ToString();
                                bugun = gerideger - 234556611;
                                //lblRbugun.Text = bugun.ToString()+" --"+tarih.tarih(bugun);
                                bugunBaslangic = (long)tar.bugunBaslangic();


                                gunSayisi = long.Parse(degerler[1], System.Globalization.NumberStyles.HexNumber);
                                //if(gunSayisi>99
                                gunSayisi = 99123456 - gunSayisi;
                                //eski limit=15552000
                                if (gunSayisi <= 31536000) //max lisan 1 yillik verilebilir
                                {
                                    //lblVerilegün.Text = gunSayisi.ToString();

                                    if ((bugun + gunSayisi) < tar.bugunBaslangic())
                                    {
                                        Program.licence = false;
                                        F_Licence lis = new F_Licence();
                                        lis.ShowDialog();
                                        Application.Exit();
                                        return;

                                    }
                                    if (bugun + gunSayisi > bugunBaslangic)
                                    {
                                    }
                                    else
                                    {
                                        Program.licence = false;
                                        F_Licence lis = new F_Licence();
                                        lis.ShowDialog();
                                        Application.Exit();
                                        return;
                                    }
                                }
                                else
                                {
                                    Program.licence = false;
                                    F_Licence lis = new F_Licence();
                                    lis.ShowDialog();
                                    Application.Exit();
                                    return;

                                }




                }
            }
            // Check Aktivation
*/

                //System.Windows.Forms.MessageBox.Show("AYAR BAGLANTI ACIK");
                Program.GlobalAyarlar.Clear();
                /*CONCAT_WS : sorgu içinde alanları birleştirir)*/
                MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT ayarlardetay.* FROM ayarlardetay where ayartur= 0 OR ayartur=2 order by ayarid ASC", myConn);
                DataTable dtUserBul = new DataTable("user");
                dtUserBul.Rows.Clear();
                daUserBul.Fill(dtUserBul);
                //System.Windows.Forms.MessageBox.Show("AYAR ayaralar kay say:"+ dtUserBul.Rows.Count);
                if (dtUserBul.Rows.Count > 0)
                {
                    for (int i = 0; i < dtUserBul.Rows.Count; i++)
                    {
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 1)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("BARKOD", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("BARKOD", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 6)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1) //%indirim
                            {
                                Program.GlobalAyarlar.Add("RABAT", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1) //€ indirim
                            {
                                Program.GlobalAyarlar.Add("RABAT", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 7)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("LOGO", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("LOGO", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 9)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("KOMMA", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("KOMMA", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 10)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("BKOMMA", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("BKOMMA", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 11)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("INFOBARCODE", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("INFOBARCODE", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 12)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("BONADRESBLOK", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("BONADRESBLOK", 0);
                            }
                        }
                        if ((int)dtUserBul.Rows[i].ItemArray[1] == 15)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("ANFANGBESTAND", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("ANFANGBESTAND", 0);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 16)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("AutoBackup", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("AutoBackup", 0);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 17)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("KasseSper", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("KasseSperr", 0);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 18)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("DruckerMode", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("DruckerMode", 0);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 19)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("TaglichZ", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("TaglichZ", 0);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 20)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("PreisBarcodeTyp", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("PreisBarcodeTyp", 2);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[6] == 1)
                            {
                                Program.GlobalAyarlar.Add("PreisBarcodeTyp", 3);
                            }
                        }
                        else if ((int)dtUserBul.Rows[i].ItemArray[1] == 21)
                        {
                            if ((int)dtUserBul.Rows[i].ItemArray[4] == 1)
                            {
                                Program.GlobalAyarlar.Add("Video", 1);
                            }
                            else if ((int)dtUserBul.Rows[i].ItemArray[5] == 1)
                            {
                                Program.GlobalAyarlar.Add("Video", 0);
                            }

                        }

                    }
                }
                Program.GlobalAyarlar.Add("WAAGE", 1);
                Program.GlobalAyarlar.Add("ECKARTE", 1);
                /* string secenek = "";
                 if (Program.kasano == 1)
                 {
                     secenek = "birincisecenekdeger";
                 }
                 else if (Program.kasano == 2)
                 {
                     secenek = "ikincisecenekdeger";
                 }
                 else if (Program.kasano == 3)
                 {
                     secenek = "ucuncusecenekdeger";
                 }
                 else if (Program.kasano == 4)
                 {
                     secenek = "dorduncusecenekdeger";
                 }
                 string sqlOzel = "SELECT ayarid," + secenek + " FROM ayarlar where ayartur= 1  order by ayarid ASC";
                 //System.Windows.Forms.MessageBox.Show("AYAR ayaralar kay say:" + sqlOzel);
                 MySqlDataAdapter daKasaOzBul = new MySqlDataAdapter(sqlOzel, myConn);
                 DataTable dtKasaOzBul = new DataTable("user");
                 dtKasaOzBul.Rows.Clear();
                 daKasaOzBul.Fill(dtKasaOzBul);
                 // System.Windows.Forms.MessageBox.Show("AYAR ayaralar2 kay say:" + dtKasaOzBul.Rows.Count);
                 if (dtKasaOzBul.Rows.Count > 0)
                 {
                     for (int b = 0; b < dtKasaOzBul.Rows.Count; b++)
                     {
                         if ((int)dtKasaOzBul.Rows[b].ItemArray[0] == 4)
                         {
                             if ((int)dtKasaOzBul.Rows[b].ItemArray[1] == 1)
                             {
                                 Program.GlobalAyarlar.Add("WAAGE", 1);
                             }
                             else if ((int)dtKasaOzBul.Rows[b].ItemArray[1] == 0)
                             {
                                 Program.GlobalAyarlar.Add("WAAGE", 0);
                             }
                         }
                         if ((int)dtKasaOzBul.Rows[b].ItemArray[0] == 5)
                         {
                             if ((int)dtKasaOzBul.Rows[b].ItemArray[1] == 1)
                             {
                                 Program.GlobalAyarlar.Add("ECKARTE", 1);
                             }
                             else if ((int)dtKasaOzBul.Rows[b].ItemArray[1] == 0)
                             {
                                 Program.GlobalAyarlar.Add("ECKARTE", 0);
                             }
                         }

                     }
                 }
                 */
                //System.Windows.Forms.MessageBox.Show("AYARLAR SONU ");

                /*ANGEBOT CHECK*/
                log.Log("AngebotList START!");
                AngebotList angebot = new AngebotList(0);
                angebot.AngebotCheck();

                //Preis Barcode Info Loading
                log.Log("PreisBarcodeInfo START!");
                PreisBarcodeInfo();

                //Kundenkarte Barcode Info load
                log.Log("KundenBarcodeInfo START!");
                KundenBarcodeInfo();

                //Digi Waage List
                log.Log("DigiWaageList START!");
                DigiWaageList();

                log.Log("SystemWaageCheck START!");
                SystemWaageCheck();

                log.Log("ButtonEinstellungen START!");
                ButtonEinstellungen();

                log.Log("Program.ProgramAyarlar[\"zvt\"] == ReaRetail START!");
                if (Program.ProgramAyarlar["zvt"] == "ReaRetail")
                {
                    //MessageBox.Show("Program.zvt=" + Program.ProgramAyarlar["zvt"]);
                    EcKassenSchnittTermTyp();
                }


            }

        }

        private void EcKassenSchnittTermTyp()
        {
            log.Log("EcKassenSchnittTermTyp() START!");
            try
            {

                iss_Crypto.RWIniFiles iniFile = new iss_Crypto.RWIniFiles("REAZVT.ini");// RWIniFiles();
                Program.ReaGerateTyp = iniFile.ReadIni("Attributes", "TERMTYPE");
                //MessageBox.Show("iniFile.ReadIni(\"Attributes\", \"TERMTYP\"=" + Program.ReaGerateTyp);
                log.Log("EcKassenSchnittTermTyp() OK!, Program.ReaGerateTyp:"+ Program.ReaGerateTyp);
            }
            catch (Exception dd)
            {
                log.Log("EcKassenSchnittTermTyp() ERROR!");
            }
        }

        public static string GetLocalIPAddress()
        {
           
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        private string getHDDNumber()
        {
            log.Log("getHDDNumber() START!");
            ArrayList hdCollection = new ArrayList();

            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.Type = wmi_HD["InterfaceType"].ToString();

                hdCollection.Add(hd);
            }

            searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            int i = 0;
            string Nr = "";
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                // get the hard drive from collection
                // using index
                HardDrive hd = (HardDrive)hdCollection[i];

                // get the hardware serial no.
                if (hd.Type != "USB")
                {
                    if (wmi_HD["SerialNumber"] == null)
                    {
                        hd.SerialNo = "None";
                    }
                    else
                    {
                        Nr = hd.SerialNo = wmi_HD["SerialNumber"].ToString();
                        break;
                    }
                }
                ++i;
            }
            return Nr;
        }

        private void ButtonEinstellungen()
        {
            try
            {
                string BtnUntenSQL = "SELECT * FROM button WHERE 'default'=0";
                MySqlDataAdapter cmdUpdate = new MySqlDataAdapter(BtnUntenSQL, myConn);
                DataTable dtButton = new DataTable();
                cmdUpdate.Fill(dtButton);
                if (dtButton.Rows.Count > 0)
                {
                    //`buttonid`, `width`, `height`, `color`, `default`, `fontname`, `fontsize` 
                    for (int a = 0; a < dtButton.Rows.Count; a++)
                    {
                        Dictionary<string, string> Btn = new Dictionary<string, string>();
                        Btn.Add("width", dtButton.Rows[a].ItemArray[2].ToString());
                        Btn.Add("height", dtButton.Rows[a].ItemArray[3].ToString());
                        Btn.Add("color", dtButton.Rows[a].ItemArray[4].ToString());
                        Btn.Add("fontname", dtButton.Rows[a].ItemArray[6].ToString());
                        Btn.Add("fontsize", dtButton.Rows[a].ItemArray[7].ToString());

                        Program.ButtonEigenschaften.Add(Convert.ToInt32(dtButton.Rows[a].ItemArray[1]), Btn);
                    }
                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
            //throw new NotImplementedException();
        }

        private void SystemWaageCheck()
        {
            try
            {

                MySqlDataAdapter daSystem = new MySqlDataAdapter("SELECT scale FROM system ", myConn);
                DataTable dtSystem = new DataTable("user");
                dtSystem.Rows.Clear();
                daSystem.Fill(dtSystem);
                //System.Windows.Forms.MessageBox.Show("AYAR ayaralar kay say:"+ dtUserBul.Rows.Count);
                if (dtSystem.Rows.Count > 0)
                {
                    Program.ScaleName = dtSystem.Rows[0].ItemArray[0].ToString();

                }
            }
            catch (Exception gg)
            {

            }
        }

        private void DigiWaageList()
        {
            try
            {
                Program.DigiWaageList = new Dictionary<string, List<string>>();
                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string SelWaage = "SELECT * FROM digiwaage ";
                MySqlDataAdapter myDaSelWaage = new MySqlDataAdapter(SelWaage, myConn);
                DataTable dtSelWaage = new DataTable();
                myDaSelWaage.Fill(dtSelWaage);
                if (dtSelWaage.Rows.Count > 0)
                {
                    Program.DigiWaageList.Clear();
                    for (int i = 0; i < dtSelWaage.Rows.Count; i++)
                    {

                        List<string> WaageInfo = new List<string>();
                        WaageInfo.Add(dtSelWaage.Rows[i].ItemArray[6].ToString()); //Serverip
                        WaageInfo.Add(dtSelWaage.Rows[i].ItemArray[1].ToString());//Waage Name
                        WaageInfo.Add(dtSelWaage.Rows[i].ItemArray[5].ToString());//ZielGrup
                        Program.DigiWaageList.Add(dtSelWaage.Rows[i].ItemArray[4].ToString(), WaageInfo);

                    }
                }
            }
            catch
            {
            }
        }

        private void CoronaCheck()
        {
            //&& AND , || OR
            try
            {
                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                Tarih tarr = new Tarih();
                double temm1 = 0, ocak1 = 1, bugunbaslangic = 0;
                temm1 = tarr.gunBaslangic(1, 7, 2020);
                ocak1 = tarr.gunBaslangic(1, 1, 2021);
                bugunbaslangic = tarr.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

                if ((bugunbaslangic >= temm1) && (bugunbaslangic < ocak1))
                {
                    string mwstSql = "SELECT * FROM mwst  ";
                    MySqlDataAdapter cmdPartnerProdukt = new MySqlDataAdapter(mwstSql, myConn);
                    DataTable dtGew = new DataTable();
                    dtGew.Rows.Clear();
                    cmdPartnerProdukt.Fill(dtGew);
                    if (dtGew.Rows.Count > 2)
                    {
                        if (Convert.ToInt16(dtGew.Rows[1].ItemArray[1]) == 7)
                        {
                            MySqlCommand cmdGrup1 = new MySqlCommand("UPDATE artikelgrup SET mwst=5 WHERE mwst =7", myConn);
                            MySqlCommand cmdGrup2 = new MySqlCommand("UPDATE artikelgrup SET mwst=16 WHERE mwst =19", myConn);
                            MySqlCommand cmdArtikel1 = new MySqlCommand("UPDATE artikel SET mwst=5 WHERE mwst =7 ", myConn);
                            MySqlCommand cmdArtikel2 = new MySqlCommand("UPDATE artikel SET mwst=16 WHERE mwst =19 ", myConn);
                            MySqlCommand cmdMwst1 = new MySqlCommand("UPDATE mwst SET mwst=5 WHERE mwst =7", myConn);
                            MySqlCommand cmdMwst2 = new MySqlCommand("UPDATE mwst SET mwst=16 WHERE mwst =19", myConn);
                            //MySqlCommand cmdMwst3 = new MySqlCommand("UPDATE artikel SET mwst=16 WHERE mwst =19", myConn);
                            cmdGrup1.ExecuteNonQuery();
                            cmdGrup2.ExecuteNonQuery();
                            cmdArtikel1.ExecuteNonQuery();
                            cmdArtikel2.ExecuteNonQuery();
                            cmdMwst1.ExecuteNonQuery();
                            cmdMwst2.ExecuteNonQuery();




                        }
                    }

                }
                else
                {
                    string mwstSql = "SELECT * FROM mwst  ";
                    MySqlDataAdapter cmdPartnerProdukt = new MySqlDataAdapter(mwstSql, myConn);
                    DataTable dtGew = new DataTable();
                    dtGew.Rows.Clear();
                    cmdPartnerProdukt.Fill(dtGew);
                    if (dtGew.Rows.Count > 2)
                    {
                        if (Convert.ToInt16(dtGew.Rows[1].ItemArray[1]) == 5)
                        {
                            MySqlCommand cmdGrup1 = new MySqlCommand("UPDATE artikelgrup SET mwst=7 WHERE mwst =5", myConn);
                            MySqlCommand cmdGrup2 = new MySqlCommand("UPDATE artikelgrup SET mwst=19 WHERE mwst =16", myConn);
                            MySqlCommand cmdArtikel1 = new MySqlCommand("UPDATE artikel SET mwst=7 WHERE mwst =5", myConn);
                            MySqlCommand cmdArtikel2 = new MySqlCommand("UPDATE artikel SET mwst=19 WHERE mwst =16", myConn);
                            MySqlCommand cmdMwst1 = new MySqlCommand("UPDATE mwst SET mwst=7 WHERE mwst =5", myConn);
                            MySqlCommand cmdMwst2 = new MySqlCommand("UPDATE mwst SET mwst=19 WHERE mwst =16", myConn);
                            //MySqlCommand cmdMwst3 = new MySqlCommand("UPDATE artikel SET mwst=16 WHERE mwst =19", myConn);
                            cmdGrup1.ExecuteNonQuery();
                            cmdGrup2.ExecuteNonQuery();
                            cmdArtikel1.ExecuteNonQuery();
                            cmdArtikel2.ExecuteNonQuery();
                            cmdMwst1.ExecuteNonQuery();
                            cmdMwst2.ExecuteNonQuery();




                        }
                    }

                }



            }
            catch (Exception ff)
            {
            }

        }

        private void PreisBarcodeInfo()
        {
            try
            {
               
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string partnerProdukt = "SELECT * FROM preisbarcode  ";
                    MySqlDataAdapter cmdPartnerProdukt = new MySqlDataAdapter(partnerProdukt, myConn);
                    DataTable dtGew = new DataTable();
                    dtGew.Rows.Clear();
                    Program.PreisBarcodeInfo.Clear();
                    cmdPartnerProdukt.Fill(dtGew);
                    if (dtGew.Rows.Count > 0)
                    {
                        List<string> PreisbarcodeItem = null;
                        for (int i = 0; i < dtGew.Rows.Count; i++)
                        {
                            PreisbarcodeItem = new List<string>();
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[0].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[1].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[3].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[2].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[4].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[6].ToString());
                            PreisbarcodeItem.Add(dtGew.Rows[i].ItemArray[5].ToString());
                            Program.PreisBarcodeInfo.Add(PreisbarcodeItem);
                        }

                    }

                
            }
            catch (Exception ddd)
            {
            }
        }
        /*
         * Sadece Sistemi Bizim Servera Kaydeder, oradan gerekli bilgileri alir
         */
        private void KundenBarcodeInfo()
        {
            try
            {
               
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string partnerProdukt = "SELECT * FROM kundenkarte  ";
                    MySqlDataAdapter cmdPartnerProdukt = new MySqlDataAdapter(partnerProdukt, myConn);
                    DataTable dtGew = new DataTable();
                    dtGew.Rows.Clear();
                    Program.KundenBarcodeInfo.Clear();
                    cmdPartnerProdukt.Fill(dtGew);
                    if (dtGew.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtGew.Rows.Count; i++)
                        {

                            Program.KundenBarcodeInfo.Add(dtGew.Rows[i].ItemArray[1].ToString());

                        }

                    }

                
            }
            catch (Exception ddd)
            {
            }
        }
        private void RemoteRegister_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Log("RemoteRegister_DoWork->START!");
            try
            {
                // MessageBox.Show("Do register");
                Dictionary<string, string> donenDegerler = new Dictionary<string, string>();
                MainRC remoteIslem = new MainRC();
                if (Program.issServer.Count > 0)
                {
                    remoteIslem.url = Program.issServer[0];
                }
                else
                {
                    MessageBox.Show("Remote Server wurden nicht identifiziert!");
                    return;
                }
                Program.IPV4 = GetExternalIP();
                remoteIslem.myConn = myConn;
                remoteIslem.dbName = Program.dbName;
                remoteIslem.ip = Program.ServerIp;
                remoteIslem.Kod = Program.IsletmeAyarlar["kod"];
                remoteIslem.Isletme = Program.IsletmeAyarlar["isletme"];
                remoteIslem.Stadt = Program.IsletmeAyarlar["stadt"];
                remoteIslem.Tel1 = Program.IsletmeAyarlar["tel1"];

                remoteIslem.Strase = Program.IsletmeAyarlar["strase"];
                remoteIslem.plz = Program.IsletmeAyarlar["plz"];
                remoteIslem.mail = Program.IsletmeAyarlar["mail"];
                remoteIslem.filhauptid = Program.IsletmeAyarlar["filhauptid"];
                donenDegerler = remoteIslem.IslemYap();
                if (donenDegerler["InternetDurum"] == "1")
                {
                    Program.InternetDurum = 1;
                }
                if (donenDegerler["Programstop"] == "1")
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "LICENCE ERROR! BITTE WENDEN SIE ISS-POS SERVICE AN!";
                    frmerror.ShowDialog();
                    Application.Exit();
                }
            }
            catch (Exception ff)
            {
                // MessageBox.Show(ff.StackTrace);
            }
            log.Log("RemoteRegister Register END, eBON START!");
            //Ebon Bearer
            if (Program.ebon != "" && Program.ebon != null && Program.ebon != "0")
            {
                //if (Program.IsletmeAyarlar["eBonBearer"] == "")
                //{
                try
                {
                    ebon_Main ebonMain = new ebon_Main();

                    LoginRequest eBonLoginResponse = new LoginRequest();
                    string Bearer = "";
                    eBonLoginResponse = ebonMain.Login(Program.IsletmeAyarlar["companyID"], Program.IsletmeAyarlar["token"]);
                    if (eBonLoginResponse.result == true)
                    {
                        Program.eBonCompanyStatus = eBonLoginResponse;
                        Program.IsletmeAyarlar["eBonBearer"] = eBonLoginResponse.access_token;
                        //Update Isletme Table
                        string UpdateSQL = "UPDATE isletme SET eBonBearer='" + eBonLoginResponse.access_token + "'";
                        MySqlCommand cmdBearerUpdate = new MySqlCommand(UpdateSQL, myConn);
                        cmdBearerUpdate.ExecuteNonQuery();
                    }
                }
                catch
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Bitte überprüfen Company eBon Einstellungen!";
                    frmerror.ShowDialog();
                }
                //}
            }
            log.Log("RemoteRegister Register END, eBON END!");

            //MessageBox.Show("TSE Check");

        }
        private void RegisterKasa()
        {
            string MakimaAd = System.Environment.MachineName;
            
            log.Log("Register Kasse Funktion START. Computer Name:"+MakimaAd);
            if (MakimaAd != "SAHBAZ-SAMSUNG")
            {
                myConn = baglanti.myconn();
                //using (myConn = baglanti.myconn())
                //{
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                /*Kasa acılısında o kasadakıı oturumları sonlandırır*/
                MySqlCommand coKasaCheck2 = new MySqlCommand("UPDATE usertakip SET  offlinezaman=1 WHERE offlinezaman=0 AND kasaid=" + Program.kasano, myConn);
                coKasaCheck2.ExecuteNonQuery();
                /*son**/

                dbConn.IsletmeAyarlar = Program.IsletmeAyarlar;
                baglanti.HerstellerSeriennummer = Program.HerstellerKasseID;
                baglanti.HDD_ID = getHDDNumber();
                baglanti.SoftwareTyp = Program.SoftwareTyp;

                KasseInfo LocalKasseInfo = new KasseInfo();
            Read: //string MakimaAd = System.Environment.MachineName;
                MySqlDataAdapter daKasaCheck = new MySqlDataAdapter("SELECT * FROM kasa WHERE makinaad= '" + MakimaAd + "'", myConn);
                DataTable dtKasaCheck = new DataTable("kasa");
                dtKasaCheck.Rows.Clear();
                daKasaCheck.Fill(dtKasaCheck);
                string ip = GetComputer_LanIP();
                log.Log("Register Kasse->Lan IP:"+ip);
                if (dtKasaCheck.Rows.Count < 1)
                {
                    log.Log("Register Kasse->This Kasse not found in DB!, There is INSERT Now");
                    CheckKasaNo();
                    MySqlCommand coKasaCheck = new MySqlCommand("INSERT INTO kasa(kasano, kasaad, makinaad, makinaip) values (" + Program.kasano + ", ' KASSE " + Program.kasano + "', '" + MakimaAd + "', '" + ip + "')", myConn);
                    coKasaCheck.ExecuteNonQuery();
                    baglanti.CheckLicence("");
                    goto Read;
                }
                else
                {
                    log.Log("Register Kasse->This Kasse found in DB!");
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                    // label3.Text = " Version: " + String.Format("{0}", fvi.FileVersion);
                    //label7.Text = " PRODUKT NAME: " + fvi.CompanyName + "/" + String.Format("{0}", fvi.ProductName);
                    
                    if (ip != "-")
                    {
                        //INSERT INTO `kasa`(`id`, `kasano`, `kasaad`, `makinaad`, `makinaip`, `KasseHerstNr`, `brand`, `model`, `sw_brand`, `sw_version`, `basis_waehrung`, `keine_ust`) VALUES
                        MySqlCommand coKasaCheck = new MySqlCommand("UPDATE kasa SET makinaip= '" + GetComputer_LanIP() + "',kasano=" + Program.kasano + ", kasaad='KASSE " + Program.kasano + "', KasseHerstNr='" + Program.HerstellerKasseID + "',brand='ISS POS Kassensysteme',model='ISS KASSE',sw_brand='ISS POS',sw_version='" + fvi.FileVersion + "',basis_waehrung='EUR' WHERE makinaad='" + MakimaAd + "'", myConn);
                        coKasaCheck.ExecuteNonQuery();
                        log.Log("Register Kasse->Licence CHECK Start!");
                        if (baglanti.CheckLicence(dtKasaCheck.Rows[0].ItemArray[14].ToString()) == 1)
                        {
                            log.Log("Register Kasse->License Expired: Program STOP!");
                            Environment.FailFast("License Expired!");
                            return;
                        }

                    }
                    LocalKasseInfo.kasano = Program.kasano.ToString();
                    LocalKasseInfo.kasaad = "KASSE " + Program.kasano;
                    LocalKasseInfo.brand = dtKasaCheck.Rows[0].ItemArray[6].ToString();
                    LocalKasseInfo.basis_waehrung = dtKasaCheck.Rows[0].ItemArray[10].ToString();
                    LocalKasseInfo.KasseHerstNr = dtKasaCheck.Rows[0].ItemArray[5].ToString();
                    LocalKasseInfo.keine_ust = dtKasaCheck.Rows[0].ItemArray[11].ToString();
                    LocalKasseInfo.model = dtKasaCheck.Rows[0].ItemArray[7].ToString();
                    LocalKasseInfo.sw_brand = dtKasaCheck.Rows[0].ItemArray[8].ToString();
                    LocalKasseInfo.sw_version = dtKasaCheck.Rows[0].ItemArray[9].ToString();
                    LocalKasseInfo.makinaad = MakimaAd;
                    LocalKasseInfo.makinaip = ip;
                    log.Log("Register Kasse->LocalKasse INfo loaded!");
                    
                }
            }
            if (Program.TSE == "1")
            {
                this.log.Log("Register Kasse->TSE=1!");
                Logger log = new Logger("LOG\\TSE");
                WormStore myWorm = null;
                Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                Program.TSEdll = new F_TSEMain();
                Program.MyWorm = myWorm = Program.TSEdll.myWorm;
                try
                {
                init:
                    WormReturnClass wormReturn = new WormReturnClass();
                    int TSEDLL = -1;
                    wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                    myWorm = Program.TSEdll.myWorm;
                    TSEDLL = wormReturn.errorCode;
                    log.Log("1.TSE DLL Returned:" + TSEDLL);
                    Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                    if (TSEDLL == 0)
                    {


                        Program.TSEready = 1;
                        wormReturn = Program.TSEdll.ValidTimeCheck();
                        string[] ErrorMeldungArray = wormReturn.errorMessage.Split('=');
                        if (wormReturn.errorCode != 0)
                        {
                            Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                            WormReturnClass newWormReturn = new WormReturnClass();
                            Program.TSELastError = "0";
                            Program.TseLastErrorMessage = "";
                            //this.Infoevent("TSE Self TEST, Bitte Warten! ");
                            newWormReturn = Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);
                            log.Log("    10.TSE DLL=0-if Returned<>0, Selftest Result Message=" + newWormReturn.errorMessage + "->Selftest Result Code=" + newWormReturn.errorCode);
                            //Buraya Notfall Thread eklenecek

                            if (newWormReturn.errorCode == 0)
                            {
                                log.Log("    10-1-2.TSE if DLL=0 if (wormReturn.errorCode == 0)  if (newWormReturn.errorCode == 0) Returned-OK :" + newWormReturn.errorCode);
                                wormReturn = Program.TSEdll.ValidTimeCheck();
                                if (Program.TSEdll.returnErrorCode == 0)
                                {
                                    log.Log("   10-1-2-1 2.TSE DLL=0 Selftest Result was OK (0), TimeAdmin Result : Error Code" + wormReturn.errorCode + "- Error Message:" + wormReturn.errorMessage);
                                    goto init;
                                }
                                else
                                {
                                    log.Log("    10-1-2-2 2.TSE if DLL=0 if (wormReturn.errorCode == 0)  if (newWormReturn.errorCode != 0) Returned-OK - Make TIMEADMIN");
                                    //this.Infoevent("TSE Time Admin!, Bitte Warten! ");

                                    wormReturn = Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                    log.Log("    10-1-2-2 2.TSE if DLL=0 - Check TIMEADMIN RESULT->Code:" + wormReturn.errorCode + " -Message:" + wormReturn.errorMessage);

                                    if (wormReturn.errorCode != 0)
                                    {
                                        log.Log("    10-1-2-2-1 TSE STOP!!!");
                                        return;
                                    }
                                    else
                                    {
                                        log.Log("           10-1-2-2-2 TIMEADMIN Result Code:" + wormReturn.errorCode + " Message:" + wormReturn.errorMessage + " Go To INIT!");
                                        goto init;
                                    }
                                }

                                //Program.PublicKey = Program.TSEdll.p pub;

                            }
                            else
                            {
                                string[] SelfTestReturnClassErrorArray = newWormReturn.errorMessage.Split('=');
                                // log.Log("    2.TSE DLL Returned-ERROR:" + newWormReturn.errorCode+"->"+SelfTestReturnClassErrorArray[1]);
                                log.Log("   10-2  2.TSE if DLL=0 if (wormReturn.errorCode != 0)  " + newWormReturn.errorCode + "->" + newWormReturn.errorCode + "->" + SelfTestReturnClassErrorArray[1]);
                                if (SelfTestReturnClassErrorArray[1] == " 0x1055")
                                {
                                    MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Geschäftsleitung und Kassenhersteller!");
                                    //log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 505");
                                    return;
                                }
                                if (SelfTestReturnClassErrorArray[1] == " 0x1011")
                                {
                                    MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Geschäftsleitung und Kassenhersteller!");
                                    //log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 511");
                                    return;
                                }


                            }
                        }
                        else // TSE have no ERROR-> TAKE TSE ID
                        {
                            log.Log("20 1.TSE DLL Returned <OK>" + TSEDLL);
                            //myWorm = Program.TSEdll.myWorm;
                            int menge = 0, leng = 0;
                            string zertifikat = "", zertifikat1 = "", tseLastID = "", zertifikat2 = "", zertifikat3 = "", zertifikat4 = "", tseSerial = "", tsePublicKey = "";
                            zertifikat = System.Convert.ToBase64String(myWorm.getLogMessageCertificate());
                            leng = zertifikat.Length;
                            menge = zertifikat.Length % 100;
                            //MessageBox.Show(zertifikat);
                            //MessageBox.Show(zertifikat.Length.ToString());
                            zertifikat1 = System.Convert.ToBase64String(myWorm.getLogMessageCertificate()).Substring(0, 1000);
                            zertifikat2 = System.Convert.ToBase64String(myWorm.getLogMessageCertificate()).Substring(1000, 1000);
                            tseSerial = BitConverter.ToString(myWorm.info().tseSerialNumber()).Replace("-", "");//BitConverter.ToString(myWorm.Info().TseSerialNumber()).Replace("-", "");
                            tsePublicKey = System.Convert.ToBase64String(Program.TSEdll.myWorm.info().tsePublicKey());
                            
                                if (myConn.State == ConnectionState.Closed)
                                    myConn.Open();
                                Tarih tar = new Tarih();
                                //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                                MySqlDataAdapter daTSECheck = new MySqlDataAdapter("SELECT * FROM tse WHERE kassenr= " + Program.kasano + " AND clientid='" + Program.ClientID + "'", myConn);
                                DataTable dtTSECheck = new DataTable("kasa");
                                dtTSECheck.Rows.Clear();
                                daTSECheck.Fill(dtTSECheck);
                                if (dtTSECheck.Rows.Count == 1)
                                {
                                    Program.TSEID = Convert.ToInt16(dtTSECheck.Rows[0].ItemArray[1]);
                                    string aktivSQL = "UPDATE tse SET aktiv=1 WHERE tse_id=" + Program.TSEID;
                                    MySqlCommand cmdAktivTse = new MySqlCommand(aktivSQL, myConn);
                                    cmdAktivTse.ExecuteNonQuery();

                                }
                                else if (dtTSECheck.Rows.Count > 1)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "Sehr wichtig.\r\nIn Ihrem System sind mehrere aktive TSE-Geräte für diese Kasse vorhanden.\r\nBitte setzen Sie sich dringend mit dem technischen Support-Team in Verbindung!\r\n02202 7059900->Anruf, WhatsApp, Facebook, Telegramm, Instagram";
                                    frmerror.ShowDialog();
                                }
                                else if (dtTSECheck.Rows.Count < 1)
                                {
                                    string InsertSQL = "";

                                    MySqlCommand cmdIns = new MySqlCommand();
                                    cmdIns.Parameters.AddWithValue("@serial", tseSerial);
                                    cmdIns.Parameters.AddWithValue("@clientid", Program.ClientID);
                                    cmdIns.Parameters.AddWithValue("@sigalgo", "ecdsa-plain-SHA384");
                                    cmdIns.Parameters.AddWithValue("@zeitformat", "generalizedTime");
                                    cmdIns.Parameters.AddWithValue("@pdencoding", "UTF-8");
                                    cmdIns.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    if (leng < 1000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", "");
                                        cmdIns.Parameters.AddWithValue("@zer3", "");
                                        cmdIns.Parameters.AddWithValue("@zer4", "");
                                        cmdIns.Parameters.AddWithValue("@zer5", "");
                                        cmdIns.Parameters.AddWithValue("@zer6", "");
                                    }
                                    else if (leng < 2000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 2000 - leng - 1));
                                        cmdIns.Parameters.AddWithValue("@zer3", "");
                                        cmdIns.Parameters.AddWithValue("@zer4", "");
                                        cmdIns.Parameters.AddWithValue("@zer5", "");
                                        cmdIns.Parameters.AddWithValue("@zer6", "");
                                    }
                                    else if (leng < 3000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 3000 - leng - 1));
                                        cmdIns.Parameters.AddWithValue("@zer4", "");
                                        cmdIns.Parameters.AddWithValue("@zer5", "");
                                        cmdIns.Parameters.AddWithValue("@zer6", "");
                                    }
                                    else if (leng < 4000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 4000 - leng - 1));
                                        cmdIns.Parameters.AddWithValue("@zer5", "");
                                        cmdIns.Parameters.AddWithValue("@zer6", "");
                                    }
                                    else if (leng < 5000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, leng - 4000));
                                        cmdIns.Parameters.AddWithValue("@zer6", "");
                                    }
                                    else if (leng < 6000)
                                    {
                                        cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, 1000));
                                        cmdIns.Parameters.AddWithValue("@zer6", zertifikat.Substring(5000, 6000 - leng - 1));
                                    }
                                    // cmdIns.Parameters.AddWithValue("@zer1", txtZertifikat1.Text);
                                    // cmdIns.Parameters.AddWithValue("@zer2", txtZertifikat2.Text);
                                    cmdIns.Parameters.AddWithValue("@pubKey", Program.PublicKey);
                                    InsertSQL = "INSERT INTO `tse`(  `tse_serial`, `sig_algo`, `zeit_format`, `pd_encoding`,  `zertifikat_i`, `zertifikat_ii`, `datum`, public_key, `zertifikat_3`, `zertifikat_4`, `zertifikat_5`, `zertifikat_6`, clientid, kassenr,aktiv) VALUES (" +
                                            "@serial,@sigalgo, @zeitformat,@pdencoding,@zer1, @zer2," + tarih.unixdate(DateTime.Now) + ", @pubKey, @zer3, @zer4,@zer5,@zer6,@clientid,@kassenr,1)";
                                    cmdIns.CommandText = InsertSQL;
                                    cmdIns.Connection = myConn;
                                    if (cmdIns.ExecuteNonQuery() > 0)
                                    {
                                        long lastid = cmdIns.LastInsertedId;
                                        tseLastID = lastid.ToString();
                                        string UpdateIdSql = "UPDATE tse SET tse_id=" + lastid + " WHERE id=" + lastid;
                                        Program.TSEID = (int)lastid;
                                        MySqlCommand cmdUpdateID = new MySqlCommand(UpdateIdSql, myConn);
                                        cmdUpdateID.ExecuteNonQuery();
                                        //btnTSESpeichern.Visible = false;
                                    }
                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "Sehr wichtig!\r\nFür diese Kasse wurde kein aktives TSE-Gerät erkannt.\r\nBitte setzen Sie sich umgehend mit Ihrem Programmanbieter in Verbindung!";
                                    frmerror.ShowDialog();

                                }
                            

                        }

                        //  Program.PublicKey = Program.TSEdll.PublicKey();

                    }
                    else
                    {
                        log.Log("1.TSE DLL Returned:" + TSEDLL);
                        string aa = wormReturn.errorMessage;
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "TSE wurde nicht gefunden!\nHersteller Fehlercode:" + aa + "\nBitte dringend Informieren Sie Ihre Geschäftsleitung!";
                        frmerror.ShowDialog();
                        log.Log(" 1.TSE DLL Returned:" + TSEDLL + "-> ERROR:" + aa);
                       
                            if (myConn.State == ConnectionState.Closed)
                                myConn.Open();
                            Tarih tar = new Tarih();
                            //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                            string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`, kassenr, tseClienID) VALUES (' Program Init-> TSE wurde nicht gefunden!  '," + 0 + ", " + tar.unixdate(DateTime.Now) + "," + Program.kasano + ", '" + Program.ClientID + "')";
                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn);
                            cmdTseErrorCode.ExecuteNonQuery();
                        
                    //MessageBox.Show(aa);
                        logEntry.AddtoLogFile(aa + DateTime.Now.ToLongTimeString(), "Einstellungen");
                    }
                }
                catch (Exception rr)
                {
                    MessageBox.Show(rr.Message);
                    logEntry.AddtoLogFile("TSE INIT Fehlgeschlagen!" + DateTime.Now.ToLongTimeString(), "Einstellungen");
                }
                log.Log("Register Kasse->TSE=1 ENDE!");
            }



        }

        private void CheckKasaNo()
        {
            MySqlCommand coKasaCheck = new MySqlCommand("DELETE FROM kasa WHERE kasano =" + Program.kasano, myConn);
            coKasaCheck.ExecuteNonQuery();

        }
        private string GetComputer_LanIP()
        {
            string strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipAddress.ToString();
                }
            }

            return "-";
        }
        public static string GetExternalIP()
        {
            string externalIP;
            try
            {
                externalIP = (new System.Net.WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new System.Text.RegularExpressions.Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();

                return externalIP;
            }
            catch
            {

                return null;
            }

        }

    }
}
