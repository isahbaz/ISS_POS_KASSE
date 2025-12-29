using Conn;
using EasyZVTDLL;
using iss_Rea;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
//using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using iss_Logger;


namespace IS_KASSE
{
    public partial class F_EConay : Form
    {
        public bool odemesonuc = false;
        public double toptutar = 0;
        EasyZVTDLL.EasyZVT objEasyZVT;
        long Ergebnis;
        public isstoRea Rea;
        string line = "", alteline = "";
        bool pr = true;
        Thread ECoku;
        List<string> dizi = new List<string>();
        Thread ReaTXTRead = null;
        Thread ReaVerlauf = null;
        public FisOlustur aktuelFis;
        public double eskiPuanToplami = 0, kazanilanPuan = 0, harcananPuan = 0, harcananPuanKarsiligiHarcananPara = 0, kazanilanIndirim = 0, angebotsuztoplamtutar = 0, kredit = 0;
        public int indirimturu = 0;
        public double rabatOran = 0;
        Musteri musteri;
        public int TeilZahlung = 0;
        public string cardTyp = "";
        public int cardpaymentTyp = 0;
        Tarih tarih = new Tarih();
        iss_Logger.Logger log = new iss_Logger.Logger("LOG\\EC");
        public F_EConay()
        {
            /* Process p = new Process();
             p.StartInfo.FileName = ("caspol.exe"); ;
             p.StartInfo.Arguments = ("machine addfulltrust EasyZVT.exe");
             p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
             p.StartInfo.RedirectStandardOutput = true;
             p.StartInfo.RedirectStandardError = true;
             p.StartInfo.UseShellExecute = false;
             if (!String.IsNullOrEmpty(currentDirectory))
                 p.StartInfo.WorkingDirectory = currentDirectory;
             p.StartInfo.CreateNoWindow = false;

             p.Start();
             ExecuteProcess("caspol -machine -addfulltrust MyPerm.exe");*/
            // AppDomain.CreateDomain( string friendlyName, Evidence securityInfo, AppDomainSetup info,PermissionSet grantSet, params StrongName[] fullTrustAssemblies);
            InitializeComponent();
        }

        private void F_EConay_Load(object sender, EventArgs e)
        {
            try
            {
                if (aktuelFis.Musterino != 0 && TeilZahlung == 0)
                {

                    if (aktuelFis.Musteri.Method == 2)
                    {
                        if (aktuelFis.Musteri.OzelOran == 0)
                        {
                            rabatOran = Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                        }
                        else
                        {
                            rabatOran = aktuelFis.Musteri.OzelOran;

                        }

                        /*F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["94"]+rabatOran+"%";
                        frmerror.ShowDialog();*/
                        MusteriIndirimiUygula();
                    }
                    else if (aktuelFis.Musteri.Method == 3)
                    {
                        if (aktuelFis.toplamtutar > 0)
                        {
                            using (F_PunkteOderRabat frmfrage = new F_PunkteOderRabat())
                            {

                                frmfrage.ShowDialog();
                                if (frmfrage.sonuc == 2)
                                {
                                    if (aktuelFis.Musteri.OzelOran == 0)
                                    {
                                        rabatOran = Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                                    }
                                    else
                                    {
                                        rabatOran = aktuelFis.Musteri.OzelOran;
                                    }
                                    aktuelFis.Indirimturu = -2;
                                    MusteriIndirimiUygula();
                                }
                                else if (frmfrage.sonuc == 1)
                                {

                                    if (aktuelFis.AngebotsuzToplamTutar > 0)
                                    {
                                        aktuelFis.KazanilanPuan = Math.Round(aktuelFis.AngebotsuzToplamTutar * (Convert.ToDouble(Program.IsletmeAyarlar["kartstandartpuan"])));
                                        aktuelFis.Indirimturu = -1;
                                        aktuelFis.EskiPuanToplamı = aktuelFis.Musteri.KullanilabilirPuan;
                                    }
                                }
                            }

                        }
                    }
                }
                label3.Text = "zu Zahlen:" + toptutar.ToString("C");
                log.Log("zu Zahlen:" + toptutar.ToString("C"));
            }
            catch (Exception hh)
            {
                MessageBox.Show("LOAD:\n" + hh.Message + "\n\n" + hh.StackTrace);
            }
        }
        private void MusteriIndirimiUygula()
        {
            kazanilanIndirim = rabatOran;
            aktuelFis.toplamtutar = toptutar = Math.Round(toptutar - ((aktuelFis.AngebotsuzToplamTutar) * rabatOran / 100), 2);
            label3.Text = "zu Zahlen : " + (toptutar).ToString("C");
            indirimturu = -2;
        }
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((Program.zvt == "ReaRetail") || (Program.zvt == "Rea"))
                {
                    log.Log("Payment throught REA CARD");
                    label2.Text = "Bitte warten...!";
                    try
                    {
                        log.Log("REA CARD-> Load DLL!");
                        Rea = Program.Rea;
                        File.Delete("REAZVT.txt");
                        kryptonButton1.Enabled = false;
                        kryptonButton2.Enabled = false;
                        //Rea = new isstoRea();

                        CheckForIllegalCrossThreadCalls = false;
                        log.Log("REA CARD-> Betrag is sending!");
                        Rea.Betrag = BetragFormat(toptutar);
                        log.Log("REA CARD-> Betrag is writing into REAZVT.ini!"+ Rea.Betrag);
                        Rea.WriteInDatei();
                        log.Log("REA CARD-> Betrag wrote into REAZVT.ini!" + Rea.Betrag);
                        //Rea.ECZahlung();
                        //timer1Tick();
                        ReaVerlauf = new Thread(new ThreadStart(ReaProc));
                        ReaVerlauf.Start();

                        ReaTXTRead = new Thread(new ThreadStart(timer1Tick));
                        ReaTXTRead.Start();


                        //timer2.Enabled = true;
                        //timer2.Start();



                    }
                    catch (Exception ee)
                    {
                        label2.Text = ee.Message;
                        kryptonButton1.Enabled = true;
                        kryptonButton2.Enabled = true;
                    }

                }
                else if (Program.zvt == "Easy1")
                {
                    label3.Visible = true;
                    label3.Text = "von der Kasse :" + toptutar.ToString("N");
                    MailMethode();

                }
                else if (Program.zvt == "Easy2")
                {
                    label3.Visible = true;
                    label3.Text = "von der Kasse :" + toptutar.ToString("N");
                    objEasyZVT = new EasyZVT();
                    EasyDLLMethode();

                }
                else
                {
                    odemesonuc = true;
                    this.Close();
                }
            }
            catch (Exception ss)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = ss.Message;
                frmerror.ShowDialog();
            }
        }


        private void ReaProc()
        {
            log.Log("REA CARD-> ReaProc START!");
            string erg = "";
            List<string> OutList = new List<string>();
            try
            {
                CheckForIllegalCrossThreadCalls = false;

                Application.DoEvents();
                erg = Rea.ECZahlung(log);
                log.Log("REA CARD-> Result!"+erg);
                if (erg== "Nochmal Versuchen!")
                {
                    Rea = new isstoRea();
                    int retcode = Rea.isstoRea1("REA CARD DLL Start!");
                    if (retcode != 1)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "EC KARTE GERÄTE IST NICHT BEREIT!";
                        frmerror.ShowDialog();

                        //return;

                    }
                    Program.Rea=Rea;    
                    erg = Rea.ECZahlung(log);

                }
                label2.Text = erg;
                if (erg != "")
                {

                    pr = true;
                    try
                    {
                        if (erg == "OK")
                        {
                            if (File.Exists((Application.StartupPath + "\\REAZVT.out")))
                            {
                                log.Log("REA CARD-> REAZVT.out is OK");
                                dbConn dbConn = new dbConn();
                                MySqlConnection myConn = dbConn.myconn();
                                string cardid = "", cardname = "";
                                if (myConn.State == ConnectionState.Closed)
                                {
                                    myConn.Open();
                                }
                                try
                                {

                                    System.IO.StreamReader file = new System.IO.StreamReader("REAZVT.out");
                                    while ((line = file.ReadLine()) != null)
                                    {
                                        //file = new System.IO.StreamReader("REAZVT.txt"); 
                                        if (line != null)
                                        {
                                            
                                           
                                            if (myConn.State == ConnectionState.Closed)
                                            {
                                                myConn.Open();
                                            }
                                            string[] inf = line.Split('=');
                                            if (inf.Length > 0)
                                            {
                                                if (inf[0] == "CARDID")
                                                {
                                                    cardid = inf[1];
                                                }
                                                if (inf[0] == "CARDNAME")
                                                {
                                                    cardname = inf[1];
                                                }
                                                if (inf[0] == "" && inf[1] == "")
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    file.Close();
                                    if (cardid != "" && cardid != "")
                                    {
                                        cardTyp = cardid;
                                        string cardTySQL = "SELECT * FROM `cardtyp` WHERE `cardid`= '" + cardid + "'";
                                        MySqlDataAdapter mydaCAardTyp = new MySqlDataAdapter(cardTySQL, myConn);
                                        DataTable dt = new DataTable();
                                        mydaCAardTyp.Fill(dt);
                                        if (dt.Rows.Count == 0)
                                        {
                                           string cmdInsertCardTySQL = "";
                                            MySqlCommand cmdInsertCardTyp = new MySqlCommand();
                                            cmdInsertCardTyp.Parameters.AddWithValue("@cardname", cardname);
                                            cmdInsertCardTyp.Parameters.AddWithValue("@cardid", cardid);
                                            cmdInsertCardTyp.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                            cmdInsertCardTySQL = "INSERT INTO `cardtyp`( `cardname`, `datum`, cardid) VALUES (@cardname,@datum, @cardid)";
                                            cmdInsertCardTyp.Connection = myConn;
                                            cmdInsertCardTyp.CommandText = cmdInsertCardTySQL;
                                            if (cmdInsertCardTyp.ExecuteNonQuery() > 0)
                                            {
                                                //cardTyp = Convert.ToInt32(cmdInsertCardTyp.LastInsertedId);

                                            }

                                        }
                                    }

                                }
                                catch (Exception dd)
                                {

                                }
                            }



                            //////////////////////
                            if (Program.zvt == "ReaRetail")
                            {

                                while (!File.Exists((Application.StartupPath + "\\REAZVT.tck")))
                                {
                                }
                            }
                            odemesonuc = true;
                            if (Program.zvt == "ReaRetail")
                            {
                                log.Log("REA CARD-> TICKET in Progress!");
                                BelegVorberaiten();
                            }
                            //Rea.exit();
                            kryptonButton1.Enabled = true;
                            kryptonButton2.Enabled = true;
                            //backgroundWorker1.CancelAsync();
                            //ReaTXTRead.Abort();


                            // ReaVerlauf.Suspend;

                            this.Close();

                        }
                        else
                        {

                            kryptonButton1.Enabled = true;
                            kryptonButton2.Enabled = true;
                            label2.Text = erg;
                            //Rea.exit();
                            //ReaTXTRead.Abort();

                            //ReaVerlauf.Abort();
                        }
                        /*  if (ReaTXTRead.IsAlive)
                          {

                              ReaTXTRead.Abort();

                              ReaVerlauf.Abort();
                          }*/

                    }
                    catch (Exception ss)
                    {
                        MessageBox.Show(ss.Message);
                    }
                }
                kryptonButton1.Enabled = true;
                kryptonButton2.Enabled = true;
            }
            catch
            {
            }




        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            //Thread.Sleep(500);
            timer1Tick();
        }

        private void timer1Tick()
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(300);

            while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
            {
                Thread.Sleep(300);
                if (File.Exists((Application.StartupPath + "\\REAZVT.txt")))
                {
                    try
                    {

                        System.IO.StreamReader file = new System.IO.StreamReader("REAZVT.txt");
                        while ((line = file.ReadLine()) != null)
                        {
                            //file = new System.IO.StreamReader("REAZVT.txt"); 
                            if (line != null)
                            {

                                // dizi.Add(line);

                                if (dizi.Contains(line))
                                {
                                }
                                else
                                {
                                    // CheckForIllegalCrossThreadCalls = false;
                                    dizi.Add(line);
                                    label2.Text = (dizi[dizi.Count - 1]);
                                }

                                //counter++;*/
                            }
                            else
                            {
                                label2.Text = (dizi[dizi.Count - 1]);
                            }
                            //file.Close();
                        }

                        file.Close();


                    }
                    catch (Exception dd)
                    {
                        // F_GenericError frmerror = new F_GenericError();
                        // frmerror.lblMesaj.Text = dd.Message;
                        // frmerror.ShowDialog();
                        continue;
                        //timer1.Start();
                        //timer1Tick();
                    }
                }


           }

            Thread.Sleep(500);


        }



        private void BelegVorberaiten()
        {
            try
            {
                List<string> ticket = new List<string>();
                int j = 0;
                dbConn dbConn = new dbConn();
                MySqlConnection myConn = dbConn.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                if (Program.zvt == "ReaRetail")
                {

                    while (!File.Exists((Application.StartupPath + "\\REAZVT.tck")))
                    {
                        j++;
                        Thread.Sleep(200);
                        if (j == 5)
                        {
                            log.Log("REA CARD-> TICKET ERROR!");
                            return;
                        }
                    }
                }
                Thread.Sleep(500);
                if (File.Exists("REAZVT.tck"))
                {
                    ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "", ticketProc = "";
                    int i = 0, a = 0;
                    foreach (string line in ticket)
                    {

                        string[] inf = line.Split(';');
                        if (inf[1] == "")
                        {
                            continue;
                        }
                        else
                        {

                            if ((inf[0] == "C:") || (inf[0] == "C:C"))
                            {
                                islem = "kunde";
                            }
                            else if ((inf[0] == "M:") || (inf[0] == "M:C"))
                            {
                                islem = "handler";
                            }
                            if (islem == "kunde")
                            {
                                Program.kundenbeleg.Add(inf[1]);
                            }
                            else if (islem == "handler")
                            {
                                Program.handlerbeleg.Add(inf[1]);
                                /*if (Program.ReaGerateTyp == "INGENICO")
                                {
                                    if (inf[1] == "Bezahlung" || a != 0)
                                    {

                                        if (a == 0) out dosyasindan okuma sonrasi buna gerek kalmadi
                                        {
                                            a = i;
                                        }

                                        if (i == a + 2) //Card Name ex. VISA
                                        {
                                            string cardTySQL = "SELECT * FROM `cardtyp` WHERE `cardname`= '" + inf[1] + "'";
                                            MySqlDataAdapter mydaCAardTyp = new MySqlDataAdapter(cardTySQL, myConn);
                                            DataTable dt = new DataTable();
                                            mydaCAardTyp.Fill(dt);
                                            if (dt.Rows.Count > 0)
                                            {
                                                cardTyp = Convert.ToInt16(dt.Rows[0].ItemArray[0]);
                                            }
                                            else
                                            {

                                                string cmdInsertCardTySQL = "";
                                                MySqlCommand cmdInsertCardTyp = new MySqlCommand();
                                                cmdInsertCardTyp.Parameters.AddWithValue("@cardname", inf[1]);
                                                cmdInsertCardTyp.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmdInsertCardTySQL = "INSERT INTO `cardtyp`( `cardname`, `datum`) VALUES (@cardname,@datum)";
                                                cmdInsertCardTyp.Connection = myConn;
                                                cmdInsertCardTyp.CommandText = cmdInsertCardTySQL;
                                                if (cmdInsertCardTyp.ExecuteNonQuery() > 0)
                                                {
                                                    cardTyp = Convert.ToInt32(cmdInsertCardTyp.LastInsertedId);

                                                }

                                            }
                                            a = 0;
                                        }
                                        if (i == a + 1) //PaymentTyp ex. Contacless
                                        {
                                            string cardTySQL1 = "SELECT * FROM `cardpaymenttyp` WHERE `zahlungstyp`= '" + inf[1] + "'";
                                            MySqlDataAdapter mydaCAardTyp1 = new MySqlDataAdapter(cardTySQL1, myConn);
                                            DataTable dt1 = new DataTable();
                                            mydaCAardTyp1.Fill(dt1);
                                            if (dt1.Rows.Count > 0)
                                            {
                                                cardpaymentTyp = Convert.ToInt16(dt1.Rows[0].ItemArray[0]);
                                            }
                                            else
                                            {

                                                string cmdInsertCardTySQL = "";
                                                MySqlCommand cmdInsertCardTyp = new MySqlCommand();
                                                cmdInsertCardTyp.Parameters.AddWithValue("@paymenttyp", inf[1]);
                                                cmdInsertCardTyp.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmdInsertCardTySQL = "INSERT INTO `cardpaymenttyp`( `zahlungstyp`, `datum`) VALUES (@paymenttyp,@datum)";
                                                cmdInsertCardTyp.Connection = myConn;
                                                cmdInsertCardTyp.CommandText = cmdInsertCardTySQL;
                                                if (cmdInsertCardTyp.ExecuteNonQuery() > 0)
                                                {
                                                    cardpaymentTyp = Convert.ToInt32(cmdInsertCardTyp.LastInsertedId);

                                                }

                                            }
                                        }
                                       
                                    }
                                } */


                            }


                        }

                        i++;
                    }
                    log.Log("REA CARD-> TICKET OK!");
                }
            }
            catch
            {
            }
        }


        public void EasyDLLMethode()
        {

            try
            {
                RegistryKey SoftwareKey = Registry.CurrentUser.OpenSubKey("Software", true);
                RegistryKey GUBKey = SoftwareKey.CreateSubKey("GUB");
                RegistryKey ZVTKey = GUBKey.CreateSubKey("ZVT");

                // Parameter die in der Anwendung gespeichert sein sollten
                /*ZVTKey.SetValue("KasseNr", 1, RegistryValueKind.DWord);
                ZVTKey.SetValue("IP", "192.168.101.222", RegistryValueKind.String)
                ZVTKey.SetValue("Port", 22000, RegistryValueKind.DWord)
                ZVTKey.SetValue("Lizenz", "", RegistryValueKind.String)*/
                // Zur Demo mit oder ohne Dialog
                /*If CheckBoxDialog.Checked() Then
                    ZVTKey.SetValue("Dialog", 1, RegistryValueKind.DWord) ' Mit kleinem Dialog
                Else
                    ZVTKey.SetValue("Dialog", 3, RegistryValueKind.DWord) ' ohne Dialog
                End If
    */
                //'Dynamischer Parameter, normalerweise der zu kassierende Betrag
                string pString = toptutar.ToString("N");
                string pTemizlenmis = "";
                long lPreis = 0;
                if (pString.Contains(','))
                {
                    int vPoz = pString.IndexOf(',');

                    pTemizlenmis = pString.Substring(0, vPoz) + (pString.Substring(vPoz + 1, pString.Length - (vPoz + 1)));

                }
                else
                {
                    pTemizlenmis = pString;
                }
                int ss = Convert.ToInt32(pTemizlenmis);
                ZVTKey.SetValue("Betrag", ss, RegistryValueKind.DWord);
                //ZVTKey.SetValue("Betrag", Convert.ToInt32(TextBoxBetrag.Text), RegistryValueKind.DWord);

                ZVTKey.Close();
                GUBKey.Close();
                SoftwareKey.Close();

                //'Ergebnis initialisieren
                //TextBoxErgebnisText.Text = "";
                //TextBoxErgebnisZahl.Text = "";
                //Button1.Enabled = False	;' Prozess darf nicht unterbrochen werden, daher Buttons deaktivieren
                //Button2.Enabled = False

                //' Instanz von EasyZVT erzeugen
                objEasyZVT = new EasyZVTDLL.EasyZVT();
                // EasyZVT starten, EasyZVT startet asynchron, d.h. die Anwendung erhält die Steuerung soforrt zurück
                objEasyZVT.Start();

                //Laufe ich noch ... oder bin ich fertig
                //Zeit = 500
                timer1.Interval = 500;
                timer1.Start();
            }

            catch (Exception ex)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = ex.Message;
                frmerror.ShowDialog();
            }
        }
        private string BetragFormat(double toptutar)
        {
            string pString = toptutar.ToString("N");
            string pTemizlenmis = "";
            long lPreis = 0;
            if (pString.Contains('.'))
            {
                pString = pString.Replace(".", "");
            }
            if (pString.Contains(','))
            {
                int vPoz = pString.IndexOf(',');

                pTemizlenmis = pString.Substring(0, vPoz) + (pString.Substring(vPoz + 1, pString.Length - (vPoz + 1)));
                return pTemizlenmis;
            }
            else
            {
                pTemizlenmis = pString;
                return pTemizlenmis;
            }
        }
        public void MailMethode()
        {
            try
            {
                kryptonButton1.Enabled = false;
                RegistryKey SoftwareKey = Registry.CurrentUser.OpenSubKey("Software", true);

                RegistryKey GUBKey = SoftwareKey.CreateSubKey("GUB");

                RegistryKey ZVTKey = GUBKey.CreateSubKey("ZVT");



                // ' Parameter die in der Anwendung gespeichert sein sollten

                /*   ZVTKey.SetValue("KasseNr", 1, RegistryValueKind.DWord);

                   ZVTKey.SetValue("COM", "10", RegistryValueKind.String);

                   //ZVTKey.SetValue("Port", 5577, RegistryValueKind.DWord);

                   //ZVTKey.SetValue("Lizenz", "", RegistryValueKind.String);
                 * 
    */
                string pString = toptutar.ToString("#0.00");
                string pTemizlenmis = "";
                long lPreis = 0;
                if (pString.Contains(','))
                {
                    int vPoz = pString.IndexOf(',');

                    pTemizlenmis = pString.Substring(0, vPoz) + (pString.Substring(vPoz + 1, pString.Length - (vPoz + 1)));

                }
                else
                {
                    pTemizlenmis = pString;
                }
                //label3.Text += "\nzur EC Kart Geräte: "+pTemizlenmis;
                double ptemizTOzuruck = Convert.ToDouble(pTemizlenmis.Insert(pTemizlenmis.Length - 2, ","));
                label3.Text += "\nzur EC Kart Geräte: " + ptemizTOzuruck;
                if (toptutar != ptemizTOzuruck)
                {
                    kryptonButton1.Enabled = true;
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "BETRAG NICHT EINSTIMMEN!\nPROGRAM STOP!";
                    frmerror.ShowDialog();
                    return;
                }
                ZVTKey.SetValue("Betrag", Convert.ToInt32(pTemizlenmis), RegistryValueKind.DWord);



                string Start = (string)ZVTKey.GetValue("START_UPDATE", "");

                ZVTKey.SetValue("Ergebnis", 999, RegistryValueKind.DWord);

                ZVTKey.SetValue("ErgebnisText", "Programmstart fehlgeschlagen", RegistryValueKind.String);



                if ((Start.Length) > 1)
                {

                    ZVTKey.SetValue("Aktiv", 1, RegistryValueKind.DWord);

                    System.Diagnostics.Process Proc = System.Diagnostics.Process.Start(Start);
                    Proc.StartInfo.CreateNoWindow = false;
                    Proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;


                    //System.Threading.Thread.Sleep(5000);

                    while (Convert.ToInt32(ZVTKey.GetValue("Aktiv")) == 1)
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        System.Threading.Thread.Sleep(100);
                        label2.Text = (string)ZVTKey.GetValue("Zwischenstatus");

                        //label2.Text = ErgebnisText;
                        // Application.DoEvents();

                    }

                }

                Ergebnis = Convert.ToInt32(ZVTKey.GetValue("Ergebnis"));
                if (Ergebnis == 0)
                {
                    odemesonuc = true;
                    this.Close();
                }
                else
                {
                    if (Ergebnis != 999)
                    {
                        F_GenericSoru frmerror = new F_GenericSoru();
                        frmerror.lblMesaj.Text = "Ergebniss:" + Ergebnis;
                        frmerror.ShowDialog();
                    }
                    kryptonButton1.Enabled = true;
                }

                // ErgebnisText = (string)ZVTKey.GetValue("Zwischenstatus");
                //label2.Text = ErgebnisText;


                ZVTKey.Close();

                GUBKey.Close();

                SoftwareKey.Close();
            }
            catch (Exception sd)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = sd.Message;
                frmerror.ShowDialog();
            }
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            odemesonuc = false;
            kazanilanIndirim = 0;
            toptutar = (aktuelFis.AngebotsuzToplamTutar);
            //label3.Text = "zu Zahlen : " + (toptutar).ToString("C");
            indirimturu = 0;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (objEasyZVT == null)
                objEasyZVT = new EasyZVT();
            label2.Text = objEasyZVT.getStatusText();
            //TextBoxZeit.Text = Zeit.ToString();
            //Zeit += 500

            if (objEasyZVT.getAktiv() == 0)
            {
                timer1.Stop();

                label2.Text = "Fertig";
                objEasyZVT = null;

                RegistryKey SoftwareKey = Registry.CurrentUser.OpenSubKey("Software", true);
                RegistryKey GUBKey = SoftwareKey.CreateSubKey("GUB");
                RegistryKey ZVTKey = GUBKey.CreateSubKey("ZVT");
                label2.Text = ZVTKey.GetValue("Ergebnis").ToString() + "->" + ZVTKey.GetValue("ErgebnisText").ToString();
                if (ZVTKey.GetValue("Ergebnis").ToString() == "0")
                {
                    odemesonuc = true;
                    this.Close();
                }

                //TextBoxErgebnisText.Text = ZVTKey.GetValue("ErgebnisText").ToString();
                ZVTKey.Close();
                GUBKey.Close();
                SoftwareKey.Close();
                //Button1.Enabled = True
                //Button2.Enabled = True

            }
        }



    }
}
