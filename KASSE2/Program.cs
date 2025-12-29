using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.PointOfService;
using System.Globalization;
using MySql.Data.MySqlClient;
using POS.Devices;
using System.Reflection;
using System.Runtime.InteropServices;
using iss_Rea;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
//using iss_tse;
using iss_tse_v2;
using ept_extended;
using iss_ebon;


namespace IS_KASSE
{
    static class Program
    {

        public static int kasano = 1;
        public static string kasaAd = "";
        public static int subeno = 1;
        public static int bedno = -1;
        public static string bedAdSoyad = "";
        public static string ServerIp = "";
        public static string WPORT = "";
        public static string SPORT = "";
        public static string KPORT = "";
        public static int Waage = -1;
        public static int bedID;
        public static Dictionary<string, int> GlobalAyarlar = null;
        public static Dictionary<string, string> IsletmeAyarlar = null;
        public static Dictionary<string, string> lang = null;
        public static Dictionary<string, string> ProgramAyarlar = null;
        public static List<string> BonText = null;
        public static Fis BonBeleg = null;
        public static int yonetici = 0;
        public static bool connection = false;
        public static bool ZberichtNo = false;
        public static string cashdrawerSO = "";
        public static string printerSO = "NCRPOSPrinter.1"; //TM-T88III, CT-S310II_1, Star TSP100 Cutter (TSP143)_1, NCRPOSPrinter.1
        public static string printerType = "T20"; //citizen, , T20, TM88, star                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        public static string PrinterLib = "OPOS"; //OPOS, .NET
        public static PosPrinter printer = null;
        //public static OPOSPOSPrinter printer2 = null;
        public static bool bSetBitmapSuccess = false;
        public static string ScannerLib = "OPOS";
        public static CultureInfo oldUICulture = null;
        public static bool bonDruck = true;
        public static MySqlConnection PerConn = new MySqlConnection();
        public static bool licence = true;
        public static long PTB_Preis = 0, PTB_Port = 0, PTB_Encoding = 0, PTB_Bondrucker = 0, PTB_KundenDisplay = 0, PTB_BonSumme = 0, PTB_VerkaufsArtikel = 0, PTB_CRC16 = 0, PTB_CRCKermit = 0,
 PTB_F_Einstellungen = 0, PTB_WaageLog = 0;
        public static string display = "";
        public static string displayType = "NCR"; //OPOS için : WN, epson, star, IBM, NCR,TVS
        public static string displaySO = "";
        public static OPOSLineDisplay lineDsp = null;
        public static OPOSCashDrawer cashDrawer = null;
        public static LD dsp = null;
        public static int aktuelJahr = 0;
        public static int connectionAttempt = 0;
        public static string programMode = "server";
        public static int boot = 0;
        public static string scannerSO = "RS232Scanner"; //SYMBOL_SCANNER, RS232Scanner
        public static string ScannerLib2 = "";
        public static string scannerSO2 = "";
        public static double aktuelgun = 0;
        public static int InternetDurum = 0;
        public static OPOSScanner oposscanner = null;
        public static OPOSScanner oposscanner2 = null;
        public static string dbName = "is_kasa";
        public static string zvt = "manuel";
        public static long BedSitzungId = 0;
        public static List<string> handlerbeleg = new List<string>();
        public static List<string> kundenbeleg = new List<string>();
        public static List<string> reticket = new List<string>();
        public static List<string> kassenschnittticket = new List<string>();
        public static List<string> stornoticketKunde = new List<string>();
        public static List<string> stornoticketHandler = new List<string>();
        public static List<string> diagTicket = new List<string>();
        public static string MacID = "";
        public static int RechPapier = 1;
        public static int schubladeauffurstorno = 0;
        public static string IPV4 = "";
        public static string RemoteServerIP = "";
        public static List<List<string>> PreisBarcodeInfo = new List<List<string>>();
        public static List<string> KundenBarcodeInfo = new List<string>();
        public static isstoRea Rea = null;
        public static string mp = "";//multi bon
        public static string kb = ""; //kundenbarcode
        public static string Layout = "";//TVS Layout
        public static string KartTyp = "";
        public static string ebon = "";
        public static List<string> issServer = new List<string>();
       // public static string localBackup = "";
        public static bool DailyBackup = false;
        public static string localBackup = "0";
        public static int DruckerMode = 1; //1 : immer 2:nicht 3: letzter

        public static PosPrinter LanPrinter = null;
        public static string LanPrinterSO = ""; //TM-T20IIE
        public static OPOSPOSPrinter printer2 = null;
        public static string LanPrinterSO1 = ""; //TM-T20IIE
        public static PosPrinter LanPrinter2 = null;
        public static string LanPrinterSO2 = "";
        public static PosPrinter LanPrinter3 = null;
        public static string LanPrinterSO3 = "";
        public static PosPrinter LanPrinter4 = null;
        public static string LanPrinterSO4 = "";

        public static int multibon = 0;
        public static string anrufmonutor = "0";
        public static string festrabat = "0";
        public static string voucar = "0";
        public static string multiserver = "0";
        public static string remoteserver = "0";
        public static int festrabatprozent = 0;

        public static int DispNr = 0;
        public static int DispInfoNr = 0;

        //TSE
        public static int TSEready=-1;
        public static string TSE;
        public static string TSEDrive;
        public static string TSEPin;
        public static string TSEPuk;
        public static string TSETimeAdmin;
        public static string ClientID;
        public static Int32 TSELastUseDatetime = 0;
        public static string TSELastError = "0";
        public static string TseLastErrorMessage = "";
        public static int TSEEmailSend = 0;
        
        //TSE DLL
        public static F_TSEMain TSEdll = null;
        public static string HerstellerKasseID;
        public static string PublicKey = "";
        public static WormStore MyWorm ;
        //public static string TseLastErrorNo = "0";
        //
        public static int TSEHealty = 0;   

        public static string companyToken = "";
        public static string companyID = "";

        public static string WaitTime="";

        public static List<int> MwStList = null;

        public static string CamKas = "";
        public static string CamPORT = "";
        public static string CamIP = "";

        public static string HandyAufladeUsername = "";
        public static string HandyAufladePassword = "";

        public static KasseInfo LocalKasseInfo = null;

        public static bool TaglichAngebotCheck = false;

        public static string DBUsername = "";
        public static string DBPass = "";

        public static int TSEID = 0;

        public static Dictionary<string, List<string>> DigiWaageList = null;

        public static MySqlConnection localConnection;

        public static etp_extended_main ept;

        public static string wprotokoll;

        public static string bForm = "";

        public static string Modus = "Kiosk";

        public static LoginRequest eBonCompanyStatus = null;

        public static int StopWatch = 0;

        public static string ScaleName = "";

        public static Dictionary<int, Dictionary<string, string>> ButtonEigenschaften = new Dictionary<int, Dictionary<string, string>>();

        public static bool DatevConnection = false;
        public static string markt = "";
        public static string SoftwareTyp = "Kasse";
        public static string ReaGerateTyp = "";
        static Logger ProgLog = new Logger("LOG\\SYSTEMSTART\\PROGRAM");
        public static User userClass = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
           /* ept.ProgramAyarlar = Program.ProgramAyarlar;
            ept.WaagePortName = Program.ProgramAyarlar["WPORT"];
            ept.BenimEventim += new etp_extended_main.DisplayDelegate(DSPINFO);
            satisYap = new SatisYap();
            ept.WaageMitPLU(arananArtikel);
            return;*/

            try
            {
                ept = new etp_extended_main();
                ProgramAyarlar = new Dictionary<string, string>();
                issServer.Add("http://52.58.67.112/iss-api/v1/");
                issServer.Add("http://www.academicasoft.de/issapi/v1/");
                
                ModifyRegistry reg = new ModifyRegistry();
                try
                {

                    ServerIp = reg.Read("Server Ip");
                    ProgramAyarlar.Add("ServerIp", ServerIp);

                    WPORT = reg.Read("WPORT");
                    ProgramAyarlar.Add("WPORT", WPORT);

                    SPORT = reg.Read("SPORT");
                    ProgramAyarlar.Add("SPORT", SPORT);

                    KPORT = reg.Read("KPORT");
                    ProgramAyarlar.Add("KPORT", KPORT);

                    kasano = Convert.ToInt16(reg.Read("Kasse Nr"));
                    ProgramAyarlar.Add("kasano", kasano.ToString());
                    kasaAd = "KASSE "+kasano;

                    PrinterLib = reg.Read("printerLib");
                    ProgramAyarlar.Add("PrinterLib", PrinterLib);

                    printerType = reg.Read("printerType");
                    ProgramAyarlar.Add("printerType", printerType);

                    printerSO = reg.Read("printerSO");
                    ProgramAyarlar.Add("printerSO", printerSO);

                    ScannerLib = reg.Read("scannerLib");
                    ProgramAyarlar.Add("ScannerLib", ScannerLib);

                    scannerSO = reg.Read("scannerSO");
                    ProgramAyarlar.Add("scannerSO", scannerSO);

                    displayType = reg.Read("displayType");
                    ProgramAyarlar.Add("displayType", displayType);

                    displaySO = reg.Read("displaySO");
                    ProgramAyarlar.Add("displaySO", displaySO);

                    display = reg.Read("displayType");
                    ProgramAyarlar.Add("display", display);

                    ScannerLib2 = reg.Read("scannerLib2");
                    ProgramAyarlar.Add("ScannerLib2", ScannerLib2);

                    scannerSO2 = reg.Read("scannerSO2");
                    ProgramAyarlar.Add("scannerSO2", scannerSO2);

                    zvt = reg.Read("ZVT");
                    ProgramAyarlar.Add("zvt", zvt);

                    MacID = reg.Read("MACID");
                    ProgramAyarlar.Add("MacID", MacID);

                    cashdrawerSO = reg.Read("cashDrawer");
                    ProgramAyarlar.Add("cashdrawerSO", cashdrawerSO);

                    mp = reg.Read("mp"); //multi parking
                    ProgramAyarlar.Add("mp", mp);

                   // kb = reg.Read("kb");
                   // KundenBarcodeInfo.Add(kb);


                    KartTyp = reg.Read("karttyp");
                    ProgramAyarlar.Add("KartTyp", KartTyp);

                    Layout = reg.Read("DispLayout");
                    ProgramAyarlar.Add("DispLayout", Layout);

                    ebon = reg.Read("ebon");
                    ProgramAyarlar.Add("econ", ebon);

                    
                    LanPrinterSO1 = reg.Read("printer1SO");
                    ProgramAyarlar.Add("printer1SO", LanPrinterSO1);

                    LanPrinterSO2 = reg.Read("printer2SO");
                    ProgramAyarlar.Add("printer2SO", LanPrinterSO2);

                    LanPrinterSO3 = reg.Read("printer3SO");
                    ProgramAyarlar.Add("printer3SO", LanPrinterSO3);

                    LanPrinterSO4 = reg.Read("printer4SO");
                    ProgramAyarlar.Add("printer4SO", LanPrinterSO4);

                    //ProgramAyarlar.Add("kb", mp);
                    try
                    {
                        multibon = Convert.ToInt16(reg.Read("multibon"));
                    }
                    catch
                    {
                        multibon = 0;
                    }
                    ProgramAyarlar.Add("multibon", multibon.ToString());

                    anrufmonutor = reg.Read("am");
                    ProgramAyarlar.Add("am", anrufmonutor);

                    multiserver = reg.Read("multiserver");
                    ProgramAyarlar.Add("multiserver", multiserver);

                    voucar = reg.Read("vh");
                    ProgramAyarlar.Add("vh", voucar);

                    festrabat = reg.Read("fr");
                    ProgramAyarlar.Add("fr", festrabat);

                    remoteserver = reg.Read("rs");
                    ProgramAyarlar.Add("rs", remoteserver);

                    festrabatprozent = Convert.ToInt16(reg.Read("fsp"));
                    ProgramAyarlar.Add("fsp", festrabatprozent.ToString());


                    DispNr = Convert.ToInt16(reg.Read("DispNr"));
                    ProgramAyarlar.Add("DispNr", DispNr.ToString());

                    ProgramAyarlar.Add("TSEReady", "0");

                    TSE = reg.Read("TSE");
                    ProgramAyarlar.Add("TSE", TSE);

                    TSEPin = reg.Read("TSEPin");
                    ProgramAyarlar.Add("TSEPin", TSEPin);

                    TSEPuk = reg.Read("TSEPuk");
                    ProgramAyarlar.Add("TSEPuk", TSEPuk);

                    TSEDrive = reg.Read("TSEDrive");
                    ProgramAyarlar.Add("TSEDrive", TSEDrive);

                    TSETimeAdmin = reg.Read("TSETimeAdmin");
                    ProgramAyarlar.Add("TSETimeAdmin",TSETimeAdmin);

                    ClientID = reg.Read("TSEClientID");
                    ProgramAyarlar.Add("ClientID", ClientID);

                    HerstellerKasseID = reg.Read("HerstellerKasseNr");
                    ProgramAyarlar.Add("HerstellerKasseNr", HerstellerKasseID);

                    WaitTime = reg.Read("WaitTime");
                    ProgramAyarlar.Add("WaitTime",WaitTime);
                    //Camera und Kasse Integration
                    CamKas = reg.Read("CamKas");
                    ProgramAyarlar.Add("CamKas",CamKas);
                    CamPORT = reg.Read("CamPORT");
                    ProgramAyarlar.Add("CamPORT", CamPORT);
                    CamIP = reg.Read("CamIP");
                    ProgramAyarlar.Add("CamIP", CamIP);

                    localBackup = reg.Read("localBackup");
                    ProgramAyarlar.Add("localBackup", localBackup);

                    wprotokoll = reg.Read("wprotokoll");
                    ProgramAyarlar.Add("wprotokoll", wprotokoll);

                    bForm = reg.Read("bForm");
                    ProgramAyarlar.Add("bForm", bForm);

                    markt = reg.Read("markt");
                    ProgramAyarlar.Add("markt", markt);

                    Modus = reg.Read("Modus");
                   // Modus = "Kiosk";
                    ProgramAyarlar.Add("Modus", Modus);
                    //MessageBox.Show(WPORT + SPORT + KPORT + ServerIp);

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Register Error!\n" + ee.Message);
                    Application.Exit();
                }
                
                int systemCounter=0;
                while (systemCounter <= Convert.ToInt16(WaitTime))
                {
                    System.Threading.Thread.Sleep(1000);
                    systemCounter++;
                }
                if (CheckRunState() != true)
                {
                    MessageBox.Show("Bereits läuft ein Program !");


                    Application.Exit();
                }
                //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
                if (ServerIp != "localhost")
                {
                    string locIP = GetLocalIPAddress();
                    while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                    {

                        int count = 0;
                        while (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                        {
                            System.Threading.Thread.Sleep(1000);
                            count++;
                            //MessageBox.Show(GetLocalIPAddress());
                            if (count == 20)
                            {

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "BITTE PRÜFEN IHRE NETWORK CONNECTION!";
                                frmerror.ShowDialog();

                            }
                        }
                    }
                }
                 
                db baglanti = new db();
                PerConn = baglanti.myconn();
                //DBCheck check = new DBCheck();
                //check.Check();
                if (ServerIp == null)
                {
                    reg.Write("Server Ip", "localhost");
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //2.Monutor Bos Kalmasin diye



                Native scrollbar = new Native();
                try
                {
                    Ayar ayar = new Ayar();
                    Waage = GlobalAyarlar["WAAGE"];
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Einstellung Error!\n" + ee.Message+"\n Bitte Company Tabelle vergleischen!!!");
                    return;
                }

                if (Screen.AllScreens.Length > 1)
                {
                   
                    /*if (Program.IsletmeAyarlar["kod"] == "83")
                    {
                          F_KundenDisplay knddsply =  new F_KundenDisplay();
                        knddsply.KundenDisplayText += new F_KundenDisplay.KundenDisplayFormDelegate(knddsply.VerkaufInfo);
                        knddsply.Location = Screen.AllScreens[1].WorkingArea.Location;
                        //Screen.AllScreens[1].WorkingArea.Location.
                        //Screen.AllScreens[1].Primary = false;
                        knddsply.Show();
                        knddsply.Location = Screen.AllScreens[1].WorkingArea.Location;
                    }*/
                    if ((Modus == "") || (Modus == null))
                    {
                        if (display == "TVS")
                        //(Program.IsletmeAyarlar["kod"] == "84") || (Program.IsletmeAyarlar["kod"] == "138") || (Program.IsletmeAyarlar["kod"] == "140") || (Program.IsletmeAyarlar["kod"] == "88") || (Program.IsletmeAyarlar["kod"] == "137") || (Program.IsletmeAyarlar["kod"] == "136") || (Program.IsletmeAyarlar["kod"] == "26") || (Program.IsletmeAyarlar["kod"] == "52") || (Program.IsletmeAyarlar["kod"] == "58") || (Program.IsletmeAyarlar["kod"] == "25") || (Program.IsletmeAyarlar["kod"] == "83") || (Program.IsletmeAyarlar["kod"] == "130") || (Program.IsletmeAyarlar["kod"] == "131") || (Program.IsletmeAyarlar["kod"] == "132"))
                        {
                            //MessageBox.Show(Screen.PrimaryScreen.DeviceName + "\n Birinci Display adi:" + Screen.AllScreens[0].DeviceName.ToString() + "Ikinci Display Adi: " + Screen.AllScreens[1].DeviceName.ToString());
                            //Screen.AllScreens[1].Primary = true;
                            try
                            {
                                if (Screen.AllScreens.Length > 1)
                                {
                                    if (!Screen.AllScreens[1].Primary)
                                    {
                                        //MessageBox.Show("if Kismi");
                                        Screen.AllScreens[0].Primary.ToString();
                                        F_KundenDisplay_Top knddsply = new F_KundenDisplay_Top();
                                        //knddsply.KundenDisplayText += new F_KundenDisplay_Top.KundenDisplayFormDelegate(knddsply.VerkaufInfo);
                                        knddsply.Location = Screen.AllScreens[Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                                        //Screen.AllScreens[1].WorkingArea.Location.
                                        //Screen.AllScreens[1].Primary = false;
                                        knddsply.Show();
                                        //knddsply.Location = Screen.AllScreens[1].WorkingArea.Location;
                                    }
                                    else
                                    {
                                        // Screen.AllScreens[0].Primary.ToString();
                                        //MessageBox.Show("Else Kismi");
                                        F_KundenDisplay_Top knddsply = new F_KundenDisplay_Top();
                                        //knddsply.KundenDisplayText += new F_KundenDisplay_Top.KundenDisplayFormDelegate(knddsply.VerkaufInfo);
                                        knddsply.Location = Screen.AllScreens[Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                                        //Screen.AllScreens[1].WorkingArea.Location.
                                        //Screen.AllScreens[1].Primary = false;
                                        knddsply.Show();
                                        // knddsply.Location = Screen.AllScreens[1].WorkingArea.Location;
                                    }
                                }


                            }
                            catch (Exception ee)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "BITTE PRÜFEN IHRES KUNDENDISPLAY ORDNUNG!\n" + ee.Message;
                                frmerror.ShowDialog();
                                // MessageBox.Show(ee.Message);
                            }
                        }
                    }
                    else
                    {
                        if (Screen.AllScreens.Length > 1)
                        {
                            Program.BonBeleg = new Fis();
                            F_Kiosk kiosk = new F_Kiosk();
                            kiosk.Location = Screen.AllScreens[1].WorkingArea.Location;
                            kiosk.Width = 1080;
                            kiosk.Height = 1920;

                            kiosk.ShowDialog();
                        }
                        else
                        {
                            Program.BonBeleg = new Fis();
                            F_Kiosk kiosk = new F_Kiosk();
                            //kiosk.Location = Screen.AllScreens[1].WorkingArea.Location;
                            kiosk.Width = 1080;
                            kiosk.Height = 1920;

                            kiosk.ShowDialog();
                        }

                    }
                    
                }
                else if(Modus == "Kiosk")
                {
                    Program.BonBeleg = new Fis();
                    F_Kiosk kiosk = new F_Kiosk();
                    //kiosk.Location = Screen.AllScreens[1].WorkingArea.Location;
                    kiosk.Width = 1080;
                    kiosk.Height = 1920;

                    kiosk.ShowDialog();
                    return;
                }
                else if (Modus == "SelfCheckout")
                {
                    Program.BonBeleg = new Fis();
                    //Application.Run(new F_SelfCheckoutSplash());
                    //kiosk.Location = Screen.AllScreens[1].WorkingArea.Location;
                    Application.Run(new F_SelfCheckout());
                    return;
                    
                }


                Program.BonBeleg = new Fis();
                //System.Windows.Forms.MessageBox.Show("AYAR CIKIS");
                if (licence == true)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    //The following line (part of the original answer) is misleading.
                    //**Do not** use it unless you want to return the System.Reflection.Assembly type's GUID.
                    string AppGUID = assembly.GetType().GUID.ToString();


                    // The following is the correct code.
                    // var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                    //var id = attribute.Value;
                    // MessageBox.Show(attribute + " " + id);
                    bool result;
                    var mutex = new System.Threading.Mutex(true, AppGUID, out result);

                    if (!result)
                    {
                        // MessageBox.Show("Another instance is already running.");
                        return;
                    }

                    //Application.Run(new Form1());


                    ProgLog.Log("Bed is OK! Bed ID:" + bedno);
                    Application.Run(new F_Bediener());
                    if (bedno != -1)
                    {
                        //MessageBox.Show(bedno.ToString());
                        ProgLog.Log("RUN main FORM!");
                        Application.Run(new Casio());

                    }

                }
                else
                {
                    MessageBox.Show("License Error!");
                    Application.Exit();
                }
            }
            catch (Exception cc)
            {
                MessageBox.Show("Error!"+ cc.Message);
            }

        }

       

        private static bool CheckRunState()
        {
            try
            {
                
                string mcname = ".";
                Process[] processes = null;
                
                processes = Process.GetProcesses(mcname);
                
                int threadscount = 0;
                //MessageBox.Show(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                foreach (Process p in processes)
                {
                    try
                    {
                        //MessageBox.Show(p.ProcessName);
                        if (p.ProcessName == System.Diagnostics.Process.GetCurrentProcess().ProcessName)
                        {
                            //StartPortService();
                            threadscount ++;
                            if (threadscount > 1)
                            {
                                MessageBox.Show("Bereits läuft ein Program !");
                                p.Kill();

                                
                            }
                        }
                    }
                    catch(Exception hh)
                    {
                        return false;
                    }
                }

                if (threadscount > 1)
                {
                    return false;
                }

                return true;

            }
            catch (Exception ee)
            {
                string ss = ee.Message;
                return false;
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
    }
}
