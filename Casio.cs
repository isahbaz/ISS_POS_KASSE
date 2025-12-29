using AForge.Video.DirectShow;
using ComponentFactory.Krypton.Toolkit;
using Conn;
using Digi_ISS;
using ept_extended;
using IS_Gutschein;
using IS_KASSE.Properties;
using iss_ebon;
using iss_HandyAuflade;
//using iss_HandyAuflade_V1;
using iss_HandyMopin;
using iss_nullbon;
using iss_Rabat;
using iss_Rea;
using iss_RechnungPrintClass;
using iss_RechnungsCreate;
using iss_Ronsson;
using iss_Satiskalem;
using iss_tse_v2;
using Microsoft.PointOfService;
using MySql.BackUp;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using POS.Devices;
using RC_V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace IS_KASSE
{
    public partial class Casio : Form
    {
        private string gelenBarkod = "";
        private Tarih tarih = new Tarih();
        private int seciliItem = -1;
        private int odemeTur = -1;
        private string gewicht = "";
        private byte[] buf = new byte[2];
        private string ag = "";
        private string fi = "";
        private string so = "";
        private string agirlik = "";
        private string angebotSembol = "";
        private DBCheck dbControl = new DBCheck();
        private List<string> barkodlarList = new List<string>();
        private Dictionary<List<string>, double> iliski = new Dictionary<List<string>, double>();
        private List<Dictionary<List<string>, double>> iliskiler = new List<Dictionary<List<string>, double>>();
        private Log logEntry = new Log();
        private WebReq webreq = new WebReq();
        private string stringTara = "";
        private F_KundenDisplay_Top knddsply = new F_KundenDisplay_Top();
        private string gutscheinbrkd = "";
        private List<FisOlustur> MultiParkBons = new List<FisOlustur>();
        private bool ScannerInUse = true;

        private SatisYap satisYap;
        private FisOlustur yeniFis;
        private FisOlustur parkedilenFis;
        private FisOlustur basilacakFis;
        private int position;
        private long gelenBarkodInt;
        private YetkiCheck yetkiCheck;
        private double SatilanAdet;
        private double SatisFiyat;
        private double WaageTutar;
        private Musteri musteri;
        private bool timeout;
        private bool globalTimeOut;
        private bool start;
        private int counter;
        private int counter_waage;
        private int step1;
        private string[] gidendata;
        private MySqlConnection newServer;
        private bool gewichtneuladen;
        private double PLUSatisFiyat;
        private double PLUSatilanAdet;
        private KryptonButton btnTik;
        private OPOSLineDisplay dsp;
        private PosExplorer explorer;
        private Thread checkDB;
        private Thread checkLiveConnection;
        private int dbCheckCounter;
        public OPOSScanner scanner;
        public OPOSScanner scanner2;
        private int brw_reload;
        protected double Tara;
        private F_KundenDisplay_V1 kndDispV1;
        private F_KundenDisplay_V2 kndDispV2;
        private int pfandid;
        private FaturaOlustur fatura;
        private int RuckGeldCounter;
        private isstoRea Rea;
        private int LastBedienerId;
        private int parkEdilmisFisSayisi;
        private Gerabo geraboKundenKarteClass;
        private List<string> GeraboInfo;
        private GeraboAccountInfo geraboAccuntInfo;
        private bool ReadGeraboKart;
        private Thread dssped;
        private long nr;
        private long nrLif;
        private GeraboAccountInfo AccountInfo;

        private Dictionary<long, FisOlustur> verkauferList = null;
        private List<User> UserList = null;


        public event Casio.KundenDisplayForm KundenDisplayText;

        //iss_HandyAuflade_Main Handy;
        int bonStornoFlag = 0;

        private SatisYap LastPos;
        private FisOlustur tekrarFis;

        bool dayBackup = false;

        Digi Digi_Waage = new Digi();

        private ept_extended.etp_extended_main ept = Program.ept;
        private string eBonVerfiedCode = "";

        string StWaPosSaveMessage = "", StWaDruckerMessage = "";
        int scaleid = -1;
        string ScaleName = "";
        bool PreisCheck = false;
        bool KoliKiste = false;
        int limit = 0;
        iss_Logger.Logger Logger = new iss_Logger.Logger("LOG\\SYSTEM");
        FisOlustur yazilacakBon;
        dbConn baglanti = new dbConn();
        MySqlConnection myConn;
        VideoCaptureDevice videoSource;
        VideoCaptureDevice videoSource2;
        private Bitmap _lastFrame;
        private readonly object _frameLock = new object();
        public Casio()
        {
            myConn = baglanti.myconn();
            Logger.Log("Class Constructor Start");
            Program.bonDruck = true;
            InitializeComponent();
            Logger.Log("Class Constructor END END");
            try
            {
                if (Screen.AllScreens.Length > 1)
                {
                    Logger.Log("Screen.AllScreens.Length > 1");
                    if (Program.ProgramAyarlar["DispLayout"] != "" && Program.ProgramAyarlar["DispLayout"] != null)
                    {
                        if (Program.ProgramAyarlar["DispLayout"] == "V1")
                        {
                            Logger.Log("Program.ProgramAyarlar[\"DispLayout\"] == \"V1\"");
                            kndDispV1 = new F_KundenDisplay_V1();
                            kndDispV1.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                        }
                        else if (Program.ProgramAyarlar["DispLayout"] == "V2")
                        {
                            Logger.Log("Program.ProgramAyarlar[\"DispLayout\"] == \"V2\"");
                            kndDispV2 = new F_KundenDisplay_V2();
                            kndDispV2.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                        }
                    }
                    Location = Screen.PrimaryScreen.WorkingArea.Location;
                    Logger.Log("Screen.AllScreens.Length > 1->Screen Location:X:" + Location.X + "Y:" + Location.Y);
                    /* if (((Size)Location).Width >= 1920)
                     {
                         limit = 47;
                     }
                     else
                     {
                         limit = 40;
                     }*/
                }
                // Location = Screen.PrimaryScreen.WorkingArea.Location;
                if (Location.IsEmpty)
                {




                    this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                    //MessageBox.Show(this.Location.X + "-" + this.Location.Y + "-" + ((Size)Location).Width + "-" + ((Size)Location).Height+"Bound X:"+ this.DesktopBounds.X+ "Bound Y:"+ this.DesktopBounds.Y+" Bound With:"+this.DesktopBounds.Width+ " Bound Height:"+this.DesktopBounds.Height);

                }
                //2025 - 07 - 27 12:42:29 - Screen.AllScreens.Length<> 1->Screen Location: X: 0 - Y:0Location.IsEmpty ?: True - Screen.PrimaryScreen.WorkingArea.With:1024 - Screen.PrimaryScreen.WorkingArea.Height:728 - Screen.AllScreens.Length:1

                Logger.Log("Screen.AllScreens.Length <> 1->Screen Location:X:" + Location.X + "-Y:" + Location.Y + "Location.IsEmpty ?:" + Location.IsEmpty + "-Screen.PrimaryScreen.WorkingArea.With:" + Screen.PrimaryScreen.WorkingArea.Width + "-Screen.PrimaryScreen.WorkingArea.Height:" + Screen.PrimaryScreen.WorkingArea.Height + "- Screen.AllScreens.Length:" + Screen.AllScreens.Length);
                if (((Size)Location).Width >= 1920)
                {
                    limit = 47;
                }
                else
                {
                    limit = 40;
                }

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

                this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                if (Program.ProgramAyarlar["bForm"] == "Diagonal")
                {
                    Logger.Log("Program.ProgramAyarlar[\"bForm\"] == \"Diagonal");
                    panelFav.Location = new System.Drawing.Point(1020, 0);
                    panelFav.Size = new Size(Screen.FromControl(this).Bounds.Width - 1024, 740);
                    panelSetup.Location = new System.Drawing.Point(0, 741);
                    panelSetup.Size = new Size(Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height - 22 - 740);
                    LoadFavProduct();

                }
                else
                {
                    Logger.Log("Program.ProgramAyarlar[\"bForm\"] != \"Diagonal");
                    panelFav.Visible = false;
                    panelSetup.Visible = false;
                }

                if (Program.IsletmeAyarlar["markt"] == "1") // Market
                {
                    Logger.Log("Program.IsletmeAyarlar[\"markt\"] == \"1\"");
                    btnPLU.Values.Text = "PLU";
                    btnPLU.StateNormal.Border.Rounding = 10;
                    btnPLU.StateNormal.Border.Width = 5;
                    btnPLU.StateNormal.Content.ShortText.Color1 = Color.Red;
                    btnPLU.StateNormal.Content.ShortText.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnZweiteEbene.Size = new Size(90, 62);
                    btnZweiteEbene.StateCommon.Content.ShortText.Image = (Image)Resources.rstart;
                    btnZweiteEbene.StateNormal.Back.Image = (Image)Resources.rstart;
                    btnZweiteEbene.StateNormal.Back.ImageStyle = PaletteImageStyle.Stretch;
                    btnZweiteEbene.StateNormal.Border.Color1 = Color.Green;
                    btnZweiteEbene.StateNormal.Border.DrawBorders = PaletteDrawBorders.All;
                    btnZweiteEbene.StateNormal.Border.Rounding = 10;
                    btnZweiteEbene.StateNormal.Border.Width = 5;
                    btnZweiteEbene.StateNormal.Content.ShortText.Color1 = Color.Red;
                    btnZweiteEbene.StateNormal.Content.ShortText.Font = new Font("Tahoma", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnZweiteEbene.StateNormal.Content.ShortText.Image = (Image)Resources.rstart;
                    btnZweiteEbene.TabIndex = 38;
                    btnZweiteEbene.Values.Text = "2. EBENE";
                }
                else if (Program.IsletmeAyarlar["markt"] == "0")// restaurant Imbiss
                {
                    Logger.Log("Program.IsletmeAyarlar[\"markt\"] != \"1\"");
                    btnPLU.Values.Text = "IMHAUS";
                    btnPLU.StateNormal.Border.Rounding = 10;
                    btnPLU.StateNormal.Border.Width = 5;
                    btnPLU.StateNormal.Content.ShortText.Color1 = Color.Red;
                    btnPLU.StateNormal.Content.ShortText.Font = new Font("Tahoma", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnZweiteEbene.Size = new Size(90, 62);
                    btnZweiteEbene.StateCommon.Content.ShortText.Image = (Image)Resources.collaboration;
                    btnZweiteEbene.StateNormal.Back.Image = (Image)Resources.collaboration;
                    btnZweiteEbene.StateNormal.Back.ImageStyle = PaletteImageStyle.Stretch;
                    btnZweiteEbene.StateNormal.Border.Color1 = Color.Green;
                    btnZweiteEbene.StateNormal.Border.DrawBorders = PaletteDrawBorders.All;
                    btnZweiteEbene.StateNormal.Border.Rounding = 10;
                    btnZweiteEbene.StateNormal.Border.Width = 5;
                    btnZweiteEbene.StateNormal.Content.ShortText.Color1 = Color.Red;
                    btnZweiteEbene.StateNormal.Content.ShortText.Font = new Font("Tahoma", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnZweiteEbene.StateNormal.Content.ShortText.Image = (Image)Resources.collaboration;
                    btnZweiteEbene.TabIndex = 38;
                    btnZweiteEbene.Values.Text = "";
                }
                else if (Program.IsletmeAyarlar["markt"] == "2") // Feinkost
                {
                    Logger.Log("Program.IsletmeAyarlar[\"markt\"] == \"2\"");
                    FisOlustur VerkauferFisi = new FisOlustur();
                    verkauferList = new Dictionary<long, FisOlustur>();
                    VerkauferListOlustur(ref VerkauferFisi);


                }
                if (Program.GlobalAyarlar["KOMMA"] == 1)
                    panel3.Visible = false;
                checkDB = new Thread(new ThreadStart(CheckDB));
                checkLiveConnection = new Thread(new ThreadStart(CheckServer));
                if (Program.lineDsp != null)
                    dsp = Program.lineDsp;
                if (Program.ScannerLib == "OPOS")
                {
                    Logger.Log("Program.ScannerLib == \"OPOS\"");
                    scanner = new OPOSScanner();
                    scanner.Open(Program.scannerSO);
                    try
                    {
                        Logger.Log("Try: scanner.ClaimDevice(1000)");
                        scanner.ClaimDevice(1000);
                    }
                    catch (Exception rr)
                    {
                        Logger.Log("Catch: scanner.ClaimDevice(1000) ERROR Message :" + rr.Message);
                        F_GenericError frmerr = new F_GenericError();
                        frmerr.lblMesaj.Text = rr.Message + " \nCODE:SC100 SCANNER CLAIM ERROR!";
                        frmerr.ShowDialog();
                        //MessageBox.Show(rr.Message);
                    }

                    if (scanner.Claimed == true)
                    {
                        // MessageBox.Show("scanner claimed");
                        try
                        {
                            Logger.Log("Try: scanner.Claimed == true");
                            scanner.DeviceEnabled = true;
                            //Program.scan = scanner;
                            scanner.DataEventEnabled = true;
                            scanner.DecodeData = true;
                            scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);
                            Program.oposscanner = scanner;

                        }
                        catch (Exception eess)
                        {
                            Logger.Log("Catch: scanner.Claimed == true, ERROR Message:" + eess.Message);
                            F_GenericError frmerr = new F_GenericError();
                            frmerr.lblMesaj.Text = eess.Message + "\nCODE:SC101 SCANNER ENABLED AND DATA_EVENT ERROR!";
                            frmerr.ShowDialog();
                            //MessageBox.Show("POS SCanner, Satir:180");
                        }

                    }
                    else
                    {
                        //MessageBox.Show("scanner dont claimed");
                        //MessageBox.Show(scanner.ToString());
                    }
                }
                if (Program.ScannerLib2 != "")
                {
                    Logger.Log("if: Program.ScannerLib2 != \"\"");
                    scanner2 = new OPOSScanner();
                    scanner2.Open(Program.scannerSO2);
                    try
                    {
                        Logger.Log("Try: Program.ScannerLib2 != \"\", Claim");
                        scanner2.ClaimDevice(1000);
                    }
                    catch (Exception rr)
                    {
                        Logger.Log("Catch: Program.ScannerLib2 != \"\" ERROR Message:" + rr.Message);
                        F_GenericError frmerr = new F_GenericError();
                        frmerr.lblMesaj.Text = rr.Message + "\nCODE:SC2-100 SCANNER 2 CLAIM ERROR!";
                        frmerr.ShowDialog();
                    }

                    if (scanner2.Claimed == true)
                    {
                        //MessageBox.Show("scanner claimed");
                        try
                        {
                            Logger.Log("Try: scanner2.Claimed == true");
                            scanner2.DeviceEnabled = true;
                            //Program.scan = scanner;
                            scanner2.DataEventEnabled = true;
                            //scanner2.DecodeData = true;
                            scanner2.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
                            Program.oposscanner2 = scanner2;

                        }
                        catch (Exception eess)
                        {
                            Logger.Log("Catch: scanner2.Claimed == true ERROR Message:" + eess.Message);
                            F_GenericError frmerr = new F_GenericError();
                            frmerr.lblMesaj.Text = eess.Message + "\nCODE:SC2-101 SCANNER 2 DEVICE_ENABLED AND DATA_EVENT ERROR!";
                            frmerr.ShowDialog();
                            //MessageBox.Show("POS Scanner 2, Satir:180");
                        }

                    }
                    else
                    {
                        //MessageBox.Show("scanner dont claimed");
                        //MessageBox.Show(scanner.ToString());
                    }
                }
            }
            catch (Exception ddd)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = ddd.Message;
                frmerr.ShowDialog();
            }
            Logger.Log("Class Constructor END");
        }

        private void LoadFavProduct()
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM artikelfav WHERE kassenr=" + Program.kasano + " OR kassenr=0 ORDER BY `sort` ", connection);
                DataTable dataTable = new DataTable("artikelgrup");
                dataTable.Clear();
                mySqlDataAdapter.Fill(dataTable);
                int count = dataTable.Rows.Count;
                if (count > 0)
                {
                    flpFav.Controls.Clear();
                    //`id`, `barcode`, `artikeid`, `artikelname`, `sort`, `kassenr`,image
                    //0         1           2           3            4          5       6
                    for (int index = 0; index < count; ++index)
                    {
                        KryptonButton btnFav = new KryptonButton();
                        btnFav.Location = new System.Drawing.Point(3, 3);
                        btnFav.Name = dataTable.Rows[index].ItemArray[1].ToString();
                        btnFav.Size = new System.Drawing.Size(135, 100);
                        btnFav.StateCommon.Back.Color1 = System.Drawing.Color.White;
                        btnFav.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        btnFav.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                        btnFav.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        btnFav.StateCommon.Border.Rounding = 2;
                        btnFav.StateCommon.Border.Width = 1;
                        btnFav.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Black;
                        btnFav.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnFav.TabIndex = 11;
                        btnFav.Values.Text = dataTable.Rows[index].ItemArray[3].ToString();
                        if (dataTable.Rows[index].ItemArray[6].ToString() != "")
                        {
                            try
                            {
                                btnFav.StateCommon.Content.Padding = new Padding(-1, btnFav.Size.Height - 40, -1, -1);
                                btnFav.StateNormal.Back.ImageAlign = PaletteRectangleAlign.Control;
                                btnFav.StateNormal.Back.ImageStyle = PaletteImageStyle.TopMiddle;
                                btnFav.StateNormal.Back.Image = Image.FromFile(Application.StartupPath + "\\" + dataTable.Rows[index].ItemArray[6].ToString());
                                btnFav.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                            }
                            catch (Exception ex)
                            {
                                //int num = (int)MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                        }
                        btnFav.Click += new EventHandler(btnFav_Click);
                        flpFav.Controls.Add((Control)btnFav);

                    }
                }
                else
                    flpFav.Controls.Clear();
            }
            catch (Exception dd)
            {
                MessageBox.Show("LoadFavProduct" + dd.Message);
            }
        }
        private void btnFav_Click(object sender, EventArgs e)
        {
            KryptonButton btnTiklanan = sender as KryptonButton;
            gelenBarkod = btnTiklanan.Name;
            barkodluUrunEkle();
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
        private void VerkauferListOlustur(ref FisOlustur VerkauferFisi)
        {
            User userList = new User();
            UserList = userList.GetUserList();
            if (UserList.Count > 0)
            {
                for (int i = 0; i < UserList.Count; i++)
                {
                    VerkauferFisi = new FisOlustur();
                    verkauferList.Add(UserList[i].Userid, null);
                }
            }

        }
        private FisOlustur YeniVerkauferFisiOlustur(ref FisOlustur VerkauferFisi, long SecilenElaman)
        {
            if (verkauferList[SecilenElaman] != null)
            {
                VerkauferFisi = verkauferList[SecilenElaman];
            }
            else
            {
                VerkauferFisi.FisYarat(0);
            }

            position = 1;
            listView1.Items.Clear();
            //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
            lblParaUstu.Text = "";
            Program.bedID = Convert.ToInt32(SecilenElaman);
            return VerkauferFisi;
        }
        private void explorer_DeviceAddedEvent(object sender, DeviceChangedEventArgs e)
        {
        }

        private void scanner_DataEvent(int Status)
        {
            SatilanAdet = 0;
            PLUSatilanAdet = 0;
            gelenBarkod = "";
            if (Program.IsletmeAyarlar["kod"] == "90")
                Console.Beep(1500, 200);
            try
            {
                byte[] numArray = new byte[scanner.ScanData.Length * 2];
                Buffer.BlockCopy((Array)scanner.ScanData.ToCharArray(), 0, (Array)numArray, 0, numArray.Length);
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                scanner.DataEventEnabled = true;
                gelenBarkod = (scanner.ScanDataLabel ?? "").Trim();
                if (scanner.ScanDataType == 0)
                {
                    gelenBarkod = gelenBarkod.Remove(0, 1);
                    gelenBarkod = gelenBarkod.Replace("\r", "");
                }
                else if (scanner.ScanDataType == 107)
                    gelenBarkod = gelenBarkod.Substring(1, gelenBarkod.Length - 1);
                else if (scanner.ScanDataType == 501)
                    gelenBarkod = scanner.ScanData;
                if (!ScannerInUse)
                    return;
                gelenBarkod = gelenBarkod.Replace("\r", "");
                gelenBarkod = gelenBarkod.Replace("\n", "");
                if (gelenBarkod.Length == 25)
                {
                    DigiWaageRead(gelenBarkod);
                }
                else
                {
                    barkodDegerlendir(gelenBarkod);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DigiWaageRead(string gelenBarkod)
        {
            string DigiWaageNr = "", DigiWaageBonnr = "", WaageIp = "";
            DigiWaageNr = (Convert.ToInt16(gelenBarkod.Substring(9, 2))).ToString();

            List<string> WaageInfo = new List<string>();
            if (Program.DigiWaageList.ContainsKey(DigiWaageNr))
            {
                WaageInfo = Program.DigiWaageList[DigiWaageNr];
                //WaageInfo.Where(x => Program.DigiWaageList.Keys.Any(d => d.Contains(DigiWaageNr))).ToList();
                //if(Program.DigiWaageList.Keys.FirstOrDefault(DigiWaageNr))
                /// WaageInfo = Program.DigiWaageList[DigiWaageNr][0];
                /// 
            }
            if (WaageInfo.Count == 0)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = "Digi Waage Communication Error->" + "Waage Nicht Gefunden!" + " Waage-Nr:" + DigiWaageNr;
                fGenericError.ShowDialog();
                return;
            }
            WaageIp = WaageInfo[0];
            DigiWaageBonnr = Convert.ToInt32(gelenBarkod.Substring(11, 7)).ToString();
            Receipt Umsatz = null;
            Digi_Waage.IP = WaageIp;
            Umsatz = Digi_Waage.getOneTransaction(DigiWaageNr, "S", DigiWaageBonnr);
            if (Umsatz == null)
            {
                UrunYok();
                return;
            }
            if (Umsatz != null && Umsatz.transactionNo > 0)
            {
                if (Program.IsletmeAyarlar["markt"] == "2")
                {
                    if (yeniFis != null)
                    {
                        verkauferList[Program.bedID] = yeniFis;
                    }
                }
                else if (yeniFis == null)
                {
                    yeniFis = new FisOlustur();
                    yeniFis.Infoevent += new FisOlustur.lblParaUstuYaz(ParaUstuLabelaYaz);
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                    //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                    lblParaUstu.Text = "";
                }

                List<ReceiptItem> posList = Umsatz.items;
                foreach (ReceiptItem pos in posList)
                {
                    if (pos.correction == false)
                    {
                        SatisYap DigiPos = new SatisYap();
                        DigiPos.Adet = pos.weight == 0 ? 1 : (double)pos.weight / 1000;
                        DigiPos.Alisfiyat = (double)pos.unitprdsc;
                        DigiPos.UrunId = pos.pluNo;
                        DigiPos.Grubid = Convert.ToInt32(WaageInfo[2]);
                        DigiPos.Gruptur = pos.weight == 0 ? 1 : 3;
                        DigiPos.Gv_typ_id = (int)GVTypEnum.Umsatz;
                        DigiPos.isHandyAuflade = 0;
                        DigiPos.KasaNo = Program.kasano;
                        DigiPos.KasiyerId = Program.bedno;
                        DigiPos.Mwst = Program.MwStList[1];
                        DigiPos.Satisfiyat = (double)pos.unitpraftdsc / 100;
                        DigiPos.Tarih = (double)tarih.unixdate(DateTime.Now);
                        DigiPos.Toplamtutar = Math.Round((double)pos.value / 100, 2);
                        DigiPos.Barkod = pos.barCode;
                        DigiPos.InternBarcode = DigiWaageBonnr;
                        if (DigiPos.Toplamtutar <= 0.0)
                        {
                            Console.Beep(400, 200);
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["67"];
                            int num3 = (int)fGenericError.ShowDialog();
                            return;
                        }
                        if (DigiPos.Toplamtutar >= 50.0)
                        {
                            F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                            summeBesteatigung.summe = DigiPos.Toplamtutar;
                            int num3 = (int)summeBesteatigung.ShowDialog();
                            if (summeBesteatigung.bestatigung != 1)
                            {
                                satisYap = (SatisYap)null;
                                return;
                            }
                        }
                        //satisYap.UrunId = (Decimal)artikel1.ArtikelId;
                        DigiPos.UrunAd = angebotSembol + pos.pluName != null ? Regex.Replace(pos.pluName, @"[^0-9 a-z A-Z]+", "") : "";
                        DigiPos.Birimkar = pos.unitprdsc != 0 ? (DigiPos.Satisfiyat - pos.unitprdsc / 100) : 0;
                        DigiPos.Ustid_id = (int)UstIdEnum.mwst7;
                        yeniFis.SatisKalem.Add(DigiPos);
                        int count1 = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count1].SubItems.Add(angebotSembol + DigiPos.UrunAd.ToString() + "(" + (object)DigiPos.Adet + (pos.weight == 0 ? " Stk. x" : " kg. x") + DigiPos.Satisfiyat.ToString("C") + (pos.weight == 0 ? "/Stk.)" : " /kg)"));
                        listView1.Items[count1].SubItems.Add(DigiPos.Toplamtutar.ToString("C"));
                        listView1.Items[count1].Tag = DigiPos.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        angebotSembol = "";
                        DSPINFO(DigiPos.UrunAd, DigiPos.Adet.ToString() + (pos.weight == 0 ? "Stk. " : " kg. "), DigiPos.Satisfiyat.ToString("C") + (pos.weight == 0 ? "/Stk. " : " /kg. "), DigiPos.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                    }

                }
            }

        }

        private void scanner2_DataEvent(int Status)
        {
            SatilanAdet = 0;
            PLUSatilanAdet = 0;
            gelenBarkod = "";
            if (Program.IsletmeAyarlar["kod"] == "90")
                Console.Beep(1500, 200);
            try
            {
                if (scanner2.ScanData.Length <= 0)
                    return;
                byte[] numArray = new byte[scanner2.ScanData.Length * 2];
                Buffer.BlockCopy((Array)scanner2.ScanData.ToCharArray(), 0, (Array)numArray, 0, numArray.Length);
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                gelenBarkod = scanner2.ScanData ?? "";
                gelenBarkod = gelenBarkod.Replace("\r", "");
                gelenBarkod = gelenBarkod.Replace("\n", "");
                if (scanner2.ScanDataType == 0)
                    gelenBarkod = gelenBarkod.Replace("\r", "");
                else if (scanner2.ScanDataType == 107)
                    gelenBarkod = gelenBarkod.Substring(1, gelenBarkod.Length - 1);
                else if (scanner2.ScanDataType == 104)
                    gelenBarkod = gelenBarkod.Substring(1, gelenBarkod.Length - 1);
                if (!ScannerInUse)
                    return;
                barkodDegerlendir(Program.scannerSO2 != "voyager" ? gelenBarkod : gelenBarkod.Substring(4, gelenBarkod.Length - 4));
            }
            catch
            {
            }
            finally
            {
                scanner2.DataEventEnabled = true;
            }
        }

        private void CheckDB()
        {
            if (checkDB.IsAlive)
                dbControl.Check();
            checkDB.Abort();
        }

        private void CheckServer()
        {
            string SrvIp = "";
            ModifyRegistry modifyRegistry = new ModifyRegistry();
            try
            {
                SrvIp = modifyRegistry.Read("Server Ip");
            }
            catch
            {
            }
            MySqlConnection mySqlConnection = new MySqlConnection();
            string str = "SERVER=" + SrvIp + ";DATABASE=is_kasa;UID=root;PASSWORD=sahbaz";
            mySqlConnection.ConnectionString = str;
            try
            {
                mySqlConnection.Open();
            }
            catch
            {
            }
            if (mySqlConnection.State == ConnectionState.Open)
            {
                new db().baglantiguncelle(SrvIp);
                if (!checkDB.IsAlive)
                {
                    try
                    {
                        checkDB.Start();
                    }
                    catch (Exception dd)
                    {
                        MessageBox.Show("CHECK SERVER ERROR!(447)" + dd.Message);
                    }
                }
            }
            if (!checkLiveConnection.IsAlive)
                return;
            checkLiveConnection.Abort();
        }

        private void kryptonCheckSet1_CheckedButtonChanged(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("20 dk sonrasi: " + tarih.tarih(tarih.unixdate(DateTime.Now.AddMinutes(20))) + " simdi:" + tarih.tarih(tarih.unixdate(DateTime.Now)));
            if (Program.TSE == "1")
            {
                // MessageBox.Show("TSE now-20 :" + tarih.tarih(tarih.unixdate(DateTime.Now.AddMinutes(-20))).ToString() + "\nAktueldate:" + Program.TSELastUseDatetime);
                if (tarih.unixdate(DateTime.Now.AddMinutes(-20)) > Program.TSELastUseDatetime)
                {
                    try
                    {
                        WormReturnClass newWormReturn = new WormReturnClass();
                        //ParaUstuLabelaYaz("TSE Self TEST, Bitte Warten! ");
                        Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                        Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                    }
                    catch (Exception dd)
                    {
                        /* F_GenericError fGenericError = new F_GenericError();
                         fGenericError.lblMesaj.Text = "TSE Sleepmode Error:"+dd.StackTrace + "\nMessage:" + dd.Message;
                         fGenericError.Location = Screen.PrimaryScreen.WorkingArea.Location; ;
                         fGenericError.ShowDialog();*/
                    }
                }
            }
            lblTarih.Text = DateTime.Now.ToShortDateString() + " / " + DateTime.Now.ToLongTimeString() + "    W:" + (object)counter;
            if (Program.programMode == "service")
            {
                if (lblParaUstu.Text == "SERVICE MODE!!")
                    lblParaUstu.Text = "";
                else
                    lblParaUstu.Text = "SERVICE MODE!!";
            }
            if (start)
            {
                pWaage.BackColor = Color.Fuchsia;
                ++counter;
                if (counter == 30)
                    timeout = true;
            }
            int num = Program.programMode != "service" ? 1 : 0;
            if (!(Program.programMode == "service"))
                return;
            ++dbCheckCounter;
            if (dbCheckCounter % 30 != 0)
                return;
            if (checkLiveConnection.IsAlive)
                checkLiveConnection.Abort();
            checkLiveConnection = new Thread(new ThreadStart(CheckServer));
            checkLiveConnection.Start();
        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "1";
        }

        private void btnIki_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "2";
        }

        private void btnUc_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "3";
        }

        private void btnDort_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "4";
        }

        private void btnBes_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "5";
        }

        private void btnAlti_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "6";
        }

        private void btnYedi_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "7";
        }

        private void btnSekiz_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "8";
        }

        private void btnDokuz_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "9";
        }

        private void btnSifir_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "0";
        }

        private void btnCiftSifir_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "00";
        }

        private void btnNokta_Click(object sender, EventArgs e)
        {
            txtGiris.Text += ",";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (txtGiris.Text.Length > 0)
                txtGiris.Text = txtGiris.Text.Substring(0, txtGiris.Text.Length - 1);
            if (seciliItem == -1)
                return;
            seciliItem = -1;
        }

        private void Casio_Load(object sender, EventArgs e)
        {
            Logger.Log("Load START");
            try
            {
                if (Program.IsletmeAyarlar["grosshandel"] == "1")
                {
                    Logger.Log("Load():if:Program.IsletmeAyarlar[\"grosshandel\"] == \"1\"");
                    btnBonPark.StateCommon.Content.LongText.Image = (Image)null;
                    btnBonPark.StateNormal.Back.Image = (Image)null;
                    btnBonPark.Location = new System.Drawing.Point(528, 592);
                    btnBonPark.Margin = new Padding(0);
                    btnBonPark.Name = "btnRechnung";
                    btnBonPark.OverrideDefault.Border.Color1 = Color.Fuchsia;
                    btnBonPark.OverrideDefault.Border.DrawBorders = PaletteDrawBorders.All;
                    btnBonPark.PaletteMode = PaletteMode.Office2007Silver;
                    btnBonPark.Size = new Size(90, 74);
                    btnBonPark.StateCommon.Content.ShortText.Color1 = Color.Green;
                    btnBonPark.StateCommon.Content.ShortText.Color2 = Color.Green;
                    btnBonPark.StateNormal.Back.ImageStyle = PaletteImageStyle.Stretch;
                    btnBonPark.StateNormal.Border.Color1 = Color.Green;
                    btnBonPark.StateNormal.Border.DrawBorders = PaletteDrawBorders.All;
                    btnBonPark.StateNormal.Border.Rounding = 10;
                    btnBonPark.StateNormal.Border.Width = 5;
                    btnBonPark.StateNormal.Content.ShortText.Color1 = Color.DarkGreen;
                    btnBonPark.StateNormal.Content.ShortText.Color2 = Color.DarkGreen;
                    btnBonPark.StateNormal.Content.ShortText.Font = new Font("Tahoma", 30f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnBonPark.TabIndex = 60;
                    btnBonPark.Values.Text = "R";
                }
                if (Screen.AllScreens.Length > 1)
                {
                    Logger.Log("Load():if:Screen.AllScreens.Length > 1");
                    //MessageBox.Show("Display Count>1");
                    // MessageBox.Show("Program.Display:" + Program.display + " ProgramAyaralarlayout:" + Program.ProgramAyarlar["DispLayout"]);
                    if (Program.display == "TVS" && Program.ProgramAyarlar["DispLayout"] != "")
                    {
                        Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\"");
                        if (Program.ProgramAyarlar["DispLayout"] != null)
                        {
                            Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\":Program.ProgramAyarlar[\"DispLayout\"] != null");
                            //MessageBox.Show("Display LAyuot!= null");
                            try
                            {
                                Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\":Program.ProgramAyarlar[\"DispLayout\"] != null:try");
                                if (Program.ProgramAyarlar["DispLayout"] == "V1")
                                {
                                    Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\":Program.ProgramAyarlar[\"DispLayout\"] != null :Program.ProgramAyarlar[\"DispLayout\"] == \"V1\"");
                                    //MessageBox.Show("Display Layout=v1");
                                    kndDispV1 = new F_KundenDisplay_V1();
                                    kndDispV1.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                                    //MessageBox.Show(Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Width+ " Height: "+Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Width );
                                    kndDispV1.Show();
                                }
                                else if (Program.ProgramAyarlar["DispLayout"] == "V2")
                                {
                                    Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\":Program.ProgramAyarlar[\"DispLayout\"] != null :Program.ProgramAyarlar[\"DispLayout\"] == \"V2\"");
                                    kndDispV2 = new F_KundenDisplay_V2();
                                    kndDispV2.panelLeftBild.Width = Convert.ToInt16(kndDispV2.LogoWidth);
                                    kndDispV2.listView1.Width = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Width - kndDispV2.panelLeftBild.Width;

                                    kndDispV2.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                                    kndDispV2.Show();

                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Log("Load():if:Screen.AllScreens.Length > 1:Program.display == \"TVS\" && Program.ProgramAyarlar[\"DispLayout\"] != \"\":Program.ProgramAyarlar[\"DispLayout\"] != null :Catch Error Message:" + ex.Message);
                                int num = (int)MessageBox.Show(":main:636" + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    Logger.Log("Load():if:Screen.AllScreens.Length > 1:else");
                    pWaage.BackColor = Color.Chartreuse;
                    if (dsp != null)
                    {
                        // ISSUE: reference to a compiler-generated method
                        dsp.DisplayTextAt(1, 0, Program.IsletmeAyarlar["isletme"], 0);
                    }
                    else if (Program.display != "TVS")
                    {
                        try
                        {
                            serialPortKD2.PortName = Program.KPORT;
                            if (Program.kasano == 3 && Program.IsletmeAyarlar["kod"] == "20" && Program.display == "torex")
                                serialPortKD2.Parity = Parity.None;
                            if (!serialPortKD2.IsOpen)
                                serialPortKD2.Open();
                            if (Program.display != "torex")
                            {
                                if (Program.kasano == 3 && Program.IsletmeAyarlar["kod"] == "20")
                                {
                                    serialPortKD2.Write(27.ToString() + (object)'@');
                                    serialPortKD2.Write(string.Concat((object)'\v'));
                                    serialPortKD2.Write("ACADEMICA KASSENSYS");
                                    serialPortKD2.Write(11.ToString() + (object)'\n');
                                    serialPortKD2.Write(Program.IsletmeAyarlar["isletme"]);
                                }
                                else
                                {
                                    serialPortKD2.Write(27.ToString() + (object)'R' + (object)'\x0002');
                                    serialPortKD2.Write(27.ToString() + (object)'[' + (object)'1' + (object)';' + (object)'1' + (object)'H');
                                    serialPortKD2.Write("ACADEMICA KASSENSYS");
                                    serialPortKD2.Write(27.ToString() + (object)'[' + (object)'2' + (object)';' + (object)'1' + (object)'H');
                                    serialPortKD2.Write("Herzlich Willkommen!");
                                }
                            }
                            else
                            {
                                serialPortKD2.Parity = Parity.None;
                                serialPortKD2.Write(string.Concat((object)'\f'));
                                serialPortKD2.Write(27.ToString() + (object)'Q' + "Herzlich Willkommen!" + (object)'\r');
                            }
                        }
                        catch (Exception ex)
                        {

                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = "688->" + Program.lang["61"] + "\nMessage:" + ex.Message;
                            fGenericError.Location = Screen.PrimaryScreen.WorkingArea.Location; ;
                            int num = (int)fGenericError.ShowDialog();
                        }
                    }
                }

                Location = Screen.PrimaryScreen.WorkingArea.Location;
                Logger.Log("Load():Location = Screen.PrimaryScreen.WorkingArea.Location->X:" + Location.X + "-Y:" + Location.Y);
                //if (Program.kasano != 3 && Program.IsletmeAyarlar["kod"] == "15")
                //{
                serialPort1.PortName = Program.SPORT;
                //serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = true;
                Logger.Log("Load():serialPort1.PortName = Program.SPORT:" + Program.SPORT);
                if (Program.ScannerLib != "OPOS" || scanner.DeviceEnabled == false)
                {
                    Logger.Log("Load():Program.ScannerLib != \"OPOS\" || scanner.DeviceEnabled == false");
                    try
                    {
                        if (serialPort1.IsOpen == false)
                        {
                            Logger.Log("Load():Program.ScannerLib != \"OPOS\" || scanner.DeviceEnabled == false:try:serialPort1.IsOpen == false");
                            serialPort1.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        Logger.Log("Load():Program.ScannerLib != \"OPOS\" || scanner.DeviceEnabled == false:Catch:serialPort1.IsOpen == false: Error Message:" + eee.Message);
                        this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "714->" + Program.lang["18"] + " " + eee.Message;
                        fGenericError.Location = Screen.PrimaryScreen.WorkingArea.Location;
                        //MessageBox.Show(Program.lang["18"] + " " + eee.Message);
                    }
                }
                /* if ((Program.WPORT != "") || (Program.WPORT == null))
                 {
                     if (sp.IsOpen == true)
                     {
                         sp.Close();
                     }
                     try
                     {
                         sp.PortName = Program.WPORT;
                         sp.BaudRate = 9600;
                         sp.Parity = Parity.Odd;
                         sp.DataBits = 7;
                         sp.StopBits = StopBits.One;
                         sp.DtrEnable = true;
                         sp.RtsEnable = true;
                         sp.Open();
                     }
                     catch (Exception ex)
                     {
                         F_GenericError fGenericError = new F_GenericError();
                         fGenericError.lblMesaj.Text = "739->" + Program.lang["62"] + " Message:" + ex.Message;
                         fGenericError.Location = Screen.PrimaryScreen.WorkingArea.Location;
                         int num = (int)fGenericError.ShowDialog();
                     }
                 }*/
                Logger.Log("RunWorkerAsync() START!");
                backgroundWorker1.RunWorkerAsync();
                Logger.Log("RunWorkerAsync() END!");
                lblBediener.Text = "BED :" + Program.bedID.ToString();
                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + Program.bedAdSoyad;
                toolStripStatusLabel3.Text = Program.TSE == "1" ? "TSE ON" : "TSE OFF";
                toolStripStatusLabel5.Text = "SERVER IP:" + Program.ServerIp;
                toolStripStatusLabel7.Text = "LOCAL IP:" + GetComputer_LanIP();

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                toolStripStatusLabel9.Text = "Version:" + fvi.FileVersion;
                toolStripStatusLabel11.Text = "Tel:+49 2202 7059900 (WhatsApp & Anruf)";
                toolStripStatusLabel13.Text = "Kasse-Nr:" + Program.kasano;
                ButtonOlustur();
                if ((Program.zvt != ""))
                {
                    if ((Program.zvt == "Rea") || (Program.zvt == "ReaRetail"))
                    {
                        Rea = new isstoRea();
                        string ECMeldung = "REA CARD DLL Start!";
                        int retcode = Rea.isstoRea1(ECMeldung);
                        if (retcode != 1)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "EC KARTE GERÄTE IST NICHT BEREIT!";
                            frmerror.ShowDialog();

                            //return;

                        }
                        Program.Rea = Rea;
                    }

                }
                try
                {
                    if ((Program.cashdrawerSO != "") && (Program.cashDrawer == null))
                    {
                        try
                        {
                            OPOSCashDrawer cashDrawer = new OPOSCashDrawer();

                            cashDrawer.Open(Program.cashdrawerSO); //printer.CharacterSet = 858;
                            cashDrawer.ClaimDevice(4000);
                            if (cashDrawer.Claimed == true)
                            {
                                cashDrawer.DeviceEnabled = true;
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerr = new F_GenericError();
                            frmerr.lblMesaj.Text = ff.Message + "\nCODE:CD101";
                            frmerr.Location = Screen.PrimaryScreen.WorkingArea.Location; ;
                            frmerr.ShowDialog();
                            //MessageBox.Show(ff.Message);
                        }

                    }
                    //MessageBox.Show(Program.cashdrawerSO + " Cashdrawername:" + Program.cashDrawer.DeviceName);
                }
                catch (Exception ss)
                {
                    MessageBox.Show("C-Load-Cashdrawer" + ss.Message);
                }
                KundenDisplay();
                if (Program.ProgramAyarlar["KartTyp"] == "gerabo")
                {
                    geraboKundenKarteClass = new Gerabo();
                    GeraboInfo = new List<string>();
                    GeraboInfo.Add(geraboKundenKarteClass.GeraboKey);
                    //GeraboInfo.Add(geraboKundenKarteClass.GeraboCode);
                    GeraboInfo.Add(geraboKundenKarteClass.Url);

                }
            }
            catch (Exception ttt)
            {


            }
            if ((Program.IsletmeAyarlar["HandyAufladeUsername"] != "") && (Program.IsletmeAyarlar["HandyAufladePassword"] != ""))
            {
                new Thread((ThreadStart)(() => HandyCardListeLoad())).Start();
            }
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if ((Program.ProgramAyarlar["Cam1"] != "") && (Program.ProgramAyarlar["Cam1"] != null))
            {
                //FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                /*for (int i = 0; i < videosources.Count; i++)
                {
                    cbbKameras.Items.Add(videosources[i].Name);
                }*/
                if (videosources.Count == 1)
                {
                    videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                    //label2.Visible = false;

                    videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame1);
                    videoSource.Start();

                }
            }
            if ((Program.ProgramAyarlar["Cam2"] != "") && (Program.ProgramAyarlar["Cam2"] != null))
            {

                videoSource2 = new VideoCaptureDevice(videosources[1].MonikerString);

                videoSource2.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame2);

                videoSource2.Start();
            }

        }
        void videoSource_NewFrame1(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            lock (_frameLock)
            {
                _lastFrame?.Dispose();
                _lastFrame = (Bitmap)eventArgs.Frame.Clone();
            }
            // pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        void videoSource_NewFrame2(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            lock (_frameLock)
            {
                _lastFrame?.Dispose();
                _lastFrame = (Bitmap)eventArgs.Frame.Clone();
            }
        }
        private void HandyCardListeLoad()
        {
            /* Handy = new iss_HandyAuflade_Main();
             Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
             Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
             List<Card> Result = Handy.CardList();*/
            try
            {
                iss_HandyMopin.iss_HandyAuflade_Main_Mopin Handy = new iss_HandyAuflade_Main_Mopin();



                Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                Handy.url = Program.IsletmeAyarlar["HandyAufladeAPI"];

                iss_HandyMopin.LoginInfo LogInfo = Handy.Login();
                if (LogInfo != null)
                {
                    //Token = LogInfo.token;
                    //RefreshToken = LogInfo.refresh;

                    F_HandyAufladeCreditHinweis guthabenHinweis = new F_HandyAufladeCreditHinweis();
                    guthabenHinweis.guthaben = LogInfo.balance.ToString("C");
                    guthabenHinweis.ShowDialog();
                    if (LogInfo.balance < 100)
                    {
                        string html = "";
                        html = "<body>" +
                        "<p></p>" +
                        "<p>ISS POS Kassensystem Guthabenhinweis:</p>" +
                        "<table width=\"741\" border=\"0\">" +
                         " <tbody>" +
                          "  <tr>" +
                           "   <td width=\"154\">&nbsp;Kundenname:</td>" +
                            "  <td width=\"571\">&nbsp;" + Program.IsletmeAyarlar["isletme"] + "</td>" +

                           " </tr>" +
                            "<tr>" +
                             " <td>&nbsp;Adresse:</td>" +
                              "<td>&nbsp;" + Program.IsletmeAyarlar["strase"] + " " + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "</td>" +

                            "</tr>" +
                            "<tr>" +
                             " <td>&nbsp;Tel:</td>" +
                              "<td>&nbsp;Tel-1:" + Program.IsletmeAyarlar["tel1"] + " Tel-2: " + Program.IsletmeAyarlar["tel2"] + "</td>" +

                            "</tr>" +
                            "<tr>" +
                             " <td>&nbsp;E-Mail:</td>" +
                              "<td>&nbsp;" + Program.IsletmeAyarlar["mail"] + "</td>" +

                            "</tr>" +
                            "<tr>" +
                             " <td>&nbsp;Aktuelles Guthaben:</td>" +
                              "<td>&nbsp;" + LogInfo.balance.ToString("C") + "</td>" +

                            "</tr>" +
                            "<tr>" +
                             " <td>&nbsp;ISS POS Kunden-Code:</td>" +
                              "<td>&nbsp;" + Program.IsletmeAyarlar["kod"] + "</td>" +

                            "</tr>" +
                          "</tbody>" +
                        "</table>" +
                        "<p></p>" +

                        "</body>";
                        if (AnadoluMail.MopinGuthabenEmail(html) == true)
                        {


                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception dd)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = "ERROR HANDYAUFLADE\n:" + dd.Message + "\nCODE:CD102";
                frmerr.Location = Screen.PrimaryScreen.WorkingArea.Location; ;
                frmerr.ShowDialog();
            }
        }

        private void btnDiv7_Click(object sender, EventArgs e)
        {
            // BGW fire
            int grupTur = 0;
            if (Program.IsletmeAyarlar["kod"] != "33")
            {
                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();
            }
            if (Program.cashdrawerSO != "")
            {
                if (CashDrawerProcessPruf() == false)
                    return;
            }
            RuckGeldCounter = 10;
            lblParaUstu.Text = "";
            seciliItem = -1;
            string urunad = "";
            double karmiktari = 0;
            long artikelid = 0;
            angebotSembol = "";
            SatilanAdet = 0;
            SatisFiyat = 0;
            WaageTutar = 0;
            PLUSatisFiyat = 0;
            PLUSatilanAdet = 0;
            KryptonButton btnTiklanan = sender as KryptonButton;

            //txtGirisBosalt();
            if (Program.IsletmeAyarlar["markt"] == "2")
            {
                if (yeniFis != null)
                {
                    verkauferList[Program.bedID] = yeniFis;
                }
                else
                {
                    yeniFis = new FisOlustur();
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                    lblParaUstu.Text = "";
                    verkauferList[Program.bedID] = yeniFis;
                }





            }
            else if (yeniFis == null)
            {
                yeniFis = new FisOlustur();
                yeniFis.Infoevent += new FisOlustur.lblParaUstuYaz(ParaUstuLabelaYaz);
                yeniFis.FisYarat(0);
                position = 1;
                listView1.Items.Clear();
                //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                lblParaUstu.Text = "";
            }
            if (btnTiklanan.TabIndex == 3)//agirlik
            {
                /* MessageBox.Show(btnTiklanan.Tag.ToString());*/
                if (Program.GlobalAyarlar["WAAGE"] == 0) //manuel agırlık Girme
                {
                    using (F_ObstGemuseManual frmobstGemuse = new F_ObstGemuseManual())
                    {
                        frmobstGemuse.grupid = Convert.ToInt16(btnTiklanan.Name);
                        frmobstGemuse.limit = limit;
                        frmobstGemuse.ShowDialog();

                        if (frmobstGemuse.sonuc == true)
                        {
                            SatilanAdet = frmobstGemuse.adet;
                            SatisFiyat = frmobstGemuse.fiyat;
                            urunad = frmobstGemuse.UrunAd;
                            karmiktari = frmobstGemuse.karmiktari;
                            artikelid = frmobstGemuse.artikelid;
                            WaageTutar = Math.Round(SatisFiyat * SatilanAdet, 2);
                            txtGiris.Text = SatilanAdet + "x" + SatisFiyat;
                            frmobstGemuse.Dispose();
                            counter_waage = 1;
                            grupTur = 3;
                        }
                        else
                        {
                            frmobstGemuse.Dispose();
                            return;
                        }
                    }
                }
                else if (Program.GlobalAyarlar["WAAGE"] == 1) //Teraziden Okuma
                {
                    Bitmap snap = null;
                    if ((Program.ProgramAyarlar["Cam1"] != "") && (Program.ProgramAyarlar["Cam1"] != null))
                    {
                        lock (_frameLock)
                        {
                            if (_lastFrame != null)
                                snap = (Bitmap)_lastFrame.Clone();
                        }
                    }
                    try
                    {
                        Dictionary<object, object> Info = new Dictionary<object, object>();
                        using (F_Waage frmWaage = new F_Waage())
                        {
                            frmWaage.grupid = Convert.ToInt16(btnTiklanan.Name);
                            frmWaage.limit = limit;
                            frmWaage.ShowDialog();
                            if ((frmWaage.Info != null) && (frmWaage.Info.Count > 0))
                            {
                                Info = frmWaage.Info;
                            }
                            else
                            {
                                return;
                            }

                        }
                        iss_Artikel.Artikel arananArtikel = new iss_Artikel.Artikel();
                        if ((Info != null) || (Info.Count > 0))
                        {
                            arananArtikel.ArtikelBul(Info["barcode"].ToString());

                            if (arananArtikel.urunvarmi == true)
                            {
                                if (snap != null)
                                {
                                    try
                                    {
                                        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AI\\image\\" + tarih.unixdate(DateTime.Now) + ".jpg");
                                        snap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        CheckForIllegalCrossThreadCalls = false;
                                        Thread ObstGemStkAI = new Thread(() => this.GenericRegisterImage(path, gelenBarkod));
                                        ObstGemStkAI.Start();

                                    }
                                    finally
                                    {
                                        snap.Dispose();
                                    }
                                }
                                Info.Clear();
                                if (arananArtikel.VkPreis <= 0)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "DER ARTIKELPREIS IST 0 (NULL)!";
                                    Console.Beep(1000, 1000);
                                    frmerror.ShowDialog();

                                    return;
                                }
                                double KundenPreis = 0, NormalPreis = 0;
                                if (yeniFis.Musterino != 0)
                                {
                                    if (yeniFis.Musteri.Method == 5)
                                    {
                                        NormalPreis = arananArtikel.VkPreis;
                                        KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, arananArtikel.BarkodNo);
                                        if (KundenPreis != 0)
                                        {

                                            arananArtikel.VkPreis = KundenPreis;
                                            angebotSembol = "#";
                                        }
                                        else
                                        {
                                            angebotSembol += "";
                                            satisYap.Satisfiyat = arananArtikel.VkPreis;
                                        }
                                    }
                                }

                                ept.ProgramAyarlar = Program.ProgramAyarlar;
                                ept.WaagePortName = Program.ProgramAyarlar["WPORT"];
                                // ept.BenimEventim += new etp_extended_main.DisplayDelegate(DSPINFO);
                                if (Program.IsletmeAyarlar["markt"] == "2")
                                {
                                    // if (artikel1.Gruptur == 1)
                                    // {
                                    if (yeniFis != null)
                                    {
                                        verkauferList[Program.bedID] = yeniFis;
                                    }
                                    else
                                    {
                                        yeniFis = new FisOlustur();
                                        yeniFis.FisYarat(0);
                                        position = 1;
                                        listView1.Items.Clear();
                                        lblParaUstu.Text = "";
                                        verkauferList[Program.bedID] = yeniFis;
                                    }



                                    // }

                                }
                                else if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                    lblParaUstu.Text = "";
                                }
                                satisYap = new SatisYap();
                                ept_extended.SatisYap_ept WaageResultPos = new SatisYap_ept();
                                WaageResultPos = ept.WaageMitPLU(arananArtikel);
                                if (WaageResultPos.errorMeldung == "")
                                {
                                    satisYap.Adet = WaageResultPos.Adet;
                                    if (yeniFis.Musterino != 0)
                                    {
                                        if (yeniFis.Musteri.Method == 5)
                                        {
                                            satisYap.ProzisyonRabatBetrag = (NormalPreis - KundenPreis) * satisYap.Adet;
                                        }
                                    }

                                    //satisYap.Fisno = yeniFis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = WaageResultPos.Mwst;
                                    //satisYap.Ustid_id = WaageResultPos.Ustid_id;
                                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Gruptur = WaageResultPos.Gruptur;
                                    satisYap.Alisfiyat = WaageResultPos.Alisfiyat;
                                    satisYap.Satisfiyat = WaageResultPos.Satisfiyat;
                                    satisYap.Gv_typ_id = WaageResultPos.Gv_typ_id;
                                    satisYap.Tarih = WaageResultPos.Tarih;
                                    satisYap.Einheit = WaageResultPos.Einheit;
                                    satisYap.Toplamtutar = Math.Round(WaageResultPos.Toplamtutar, 2);
                                    satisYap.Angebotvarmi = WaageResultPos.Angebotvarmi;
                                    if (satisYap.Toplamtutar >= 50)
                                    {

                                        F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                                        frmSumme.summe = satisYap.Toplamtutar;
                                        frmSumme.ShowDialog();
                                        if (frmSumme.bestatigung != 1)
                                        {
                                            satisYap = null;
                                            return;
                                        }

                                    }

                                    satisYap.UrunId = WaageResultPos.UrunId;
                                    satisYap.UrunAd = angebotSembol + WaageResultPos.UrunAd.Replace("'", "");
                                    satisYap.Birimkar = (WaageResultPos.Birimkar) * adet;
                                    satisYap.Grubid = WaageResultPos.Grubid;
                                    satisYap.Fand = WaageResultPos.Fand;
                                    satisYap.Birimid = WaageResultPos.Birimid;
                                    satisYap.Barkod = WaageResultPos.Barkod;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.KasiyerId = Program.bedID;
                                    if (Program.IsletmeAyarlar["markt"] == "2")
                                    {

                                        F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                                        FUserAuswahl.satilanPozition = satisYap;
                                        FUserAuswahl.UserList = UserList;
                                        FUserAuswahl.ShowDialog();
                                        if (FUserAuswahl.SecilenUser != -1)
                                        {
                                            FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                                            yeniFis = null;
                                            if (ElemanFisi == null)
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                                                KundenDisplay();
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                            }
                                            else
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis = ElemanFisi;
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                                listView1.Items.Clear();
                                                KundenDisplay();
                                                int num2 = 1;
                                                int count = listView1.Items.Count;
                                                foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                                                {

                                                    listView1.Items.Add(num2.ToString());
                                                    listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                                    listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                                    DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                    ++num2;
                                                    count++;
                                                }
                                                position = num2;
                                                yeniFis.FisiKapat();


                                            }

                                            yeniFis.SatisKalem.Add(satisYap);
                                            verkauferList[Program.bedID] = yeniFis;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else //Markt =1 ise
                                    {

                                        //satisYap.IsKolli = true;
                                        satisYap.Fisno = yeniFis.SatisAnaId;
                                        yeniFis.SatisKalem.Add(satisYap);
                                        LastPos = satisYap;
                                    }

                                    //yeniFis.SatisKalem.Add(satisYap);
                                    int count1 = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    listView1.Items[count1].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet.ToString("#0.000") + " kg. x " + satisYap.Satisfiyat.ToString("C") + "/kg.)");
                                    listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[count1].Tag = satisYap.Barkod;
                                    if (listView1.Items.Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    txtGiris.Text = "";
                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    ++position;
                                    angebotSembol = "";
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    satisYap = null;
                                    Console.Beep(800, 100);
                                    Console.Beep(1000, 100);
                                    return;
                                }
                                else
                                {

                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = WaageResultPos.errorMeldung;
                                    Console.Beep(1000, 1000);
                                    frmerror.ShowDialog();

                                    return;
                                }
                            }
                        }

                    }
                    catch (Exception dd)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = dd.Message;
                        Console.Beep(1000, 1000);
                        frmerror.ShowDialog();

                        return;
                    }
                }
            }
            else if (btnTiklanan.TabIndex == 2)
            {
                Bitmap snap = null;
                if ((Program.ProgramAyarlar["Cam1"] != "") && (Program.ProgramAyarlar["Cam1"] != null))
                {
                    lock (_frameLock)
                    {
                        if (_lastFrame != null)
                            snap = (Bitmap)_lastFrame.Clone();
                    }
                }


                using (F_ObstGemuseStuck frmobstGemuseStuck = new F_ObstGemuseStuck())
                {
                    if (txtGiris.Text != "")
                    {
                        if (txtGiris.Text.IndexOf('X') != -1)
                        {
                            int length = txtGiris.Text.IndexOf('X');
                            if (double.TryParse(txtGiris.Text.Substring(0, length), out PLUSatilanAdet))
                            {
                            }
                            else
                            {
                                PLUSatilanAdet = 1.0;
                            }


                        }

                        else if (double.TryParse(txtGiris.Text, out PLUSatilanAdet))
                        {
                        }
                        else
                        {
                            PLUSatilanAdet = 1;
                        }

                    }

                    frmobstGemuseStuck.grupid = Convert.ToInt16(btnTiklanan.Name);
                    frmobstGemuseStuck.limit = limit;
                    frmobstGemuseStuck.BarcodeSatisEvent += new F_ObstGemuseStuck.BarkodSatisDelagate(ObstGemDelegate);
                    frmobstGemuseStuck.ShowDialog();

                    if (frmobstGemuseStuck.sonuc == true)
                    {
                        gelenBarkod = frmobstGemuseStuck.Barcode;
                        if (snap != null)
                        {
                            try
                            {
                                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AI\\image\\" + tarih.unixdate(DateTime.Now) + ".jpg");
                                snap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                CheckForIllegalCrossThreadCalls = false;
                                Thread ObstGemStkAI = new Thread(() => this.GenericRegisterImage(path, gelenBarkod));
                                ObstGemStkAI.Start();

                            }
                            finally
                            {
                                snap.Dispose();
                            }
                        }
                        barkodluUrunEkle();
                        Console.Beep(800, 100);
                        Console.Beep(1500, 100);
                        return;

                    }
                    else
                    {
                        frmobstGemuseStuck.Dispose();
                        return;
                    }
                }
            }
            else if (btnTiklanan.TabIndex == 4)// generic div7% ve div19% verkauf
            {
                grupTur = 0;
            }
            else if (btnTiklanan.TabIndex == 6)// PFAND SATISI
            {
                if (btnTiklanan.Name == "42")
                {
                    if (txtGiris.Text != "")
                    {
                        if (double.TryParse(txtGiris.Text, out SatilanAdet))
                        {
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["20"];
                            frmerror.ShowDialog();
                            SatilanAdet = 1;
                            txtGiris.Text = "";
                        }

                    }
                    else
                    {
                        SatilanAdet = 1;
                    }
                    txtGiris.Text = SatilanAdet + "x" + SatisFiyat;
                    karmiktari = SatilanAdet * SatisFiyat;

                    Artikel urun = new Artikel();
                    urun = new Artikel();
                    urun.ArtikelBul("88887");
                    artikelid = urun.ArtikelId;
                    karmiktari = urun.KarMiktari * SatilanAdet;
                    SatisFiyat = urun.VkPreis;
                    urunad = urun.ArtikelAd;


                }
                else if (btnTiklanan.Name == "32")
                {
                    if (txtGiris.Text != "")
                    {
                        if (double.TryParse(txtGiris.Text, out SatilanAdet))
                        {
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["20"];
                            frmerror.ShowDialog();
                            SatilanAdet = 1;
                            txtGiris.Text = "";
                        }

                    }
                    else
                    {
                        SatilanAdet = 1;
                    }
                    txtGiris.Text = SatilanAdet + "x" + SatisFiyat;
                    karmiktari = SatilanAdet * SatisFiyat;

                    Artikel urun = new Artikel();
                    urun = new Artikel();
                    urun.ArtikelBul("88888");
                    artikelid = urun.ArtikelId;
                    karmiktari = urun.KarMiktari * SatilanAdet;
                    SatisFiyat = urun.VkPreis;
                    urunad = urun.ArtikelAd;



                }
                else if (btnTiklanan.Name == "70")
                {
                    if (txtGiris.Text != "")
                    {
                        if (double.TryParse(txtGiris.Text, out SatilanAdet))
                        {
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["20"];
                            frmerror.ShowDialog();
                            SatilanAdet = 1;
                        }

                    }
                    else
                    {
                        SatilanAdet = 1;
                    }
                    txtGiris.Text = SatilanAdet + "x" + SatisFiyat;
                    karmiktari = SatilanAdet * SatisFiyat;

                    Artikel urun = new Artikel();
                    urun = new Artikel();
                    urun.ArtikelBul("88886");
                    artikelid = urun.ArtikelId;
                    karmiktari = urun.KarMiktari * SatilanAdet;
                    SatisFiyat = urun.VkPreis;
                    urunad = urun.ArtikelAd;


                }
                else
                {
                    VkPreis();

                    if (SatilanAdet == 0) return;
                    if (SatisFiyat == 0) return;
                    artikelid = 0;
                    karmiktari = 0;
                    urunad = btnTiklanan.Text;
                }

            }
            else if (btnTiklanan.TabIndex == 7) //Obst Gemüse PLU (Stuck oder Waage)
            {
                if (txtGiris.Text != "")
                {
                    string satilanBarkod = "";
                    double PLUsatilanPlu = 0;
                    if (txtGiris.Text.IndexOf('X') != -1)
                    {


                        int Xyer = txtGiris.Text.IndexOf('X');
                        if (double.TryParse(txtGiris.Text.Substring(Xyer + 1, txtGiris.Text.Length - (Xyer + 1)), out PLUsatilanPlu))
                        {
                            satilanBarkod = PLUsatilanPlu.ToString();
                            if (double.TryParse(txtGiris.Text.Substring(0, Xyer), out PLUSatilanAdet))
                            {


                            }
                            else
                            {
                                PLUSatilanAdet = 1;
                            }

                        }

                    }
                    else
                    {
                        satilanBarkod = txtGiris.Text;


                    }
                    ObstGemusePLU(satilanBarkod);
                }

            }
            else if (btnTiklanan.TabIndex == 8)
            {
                try
                {

                    PLUSatilanAdet = 0;
                    //string strFiyat = "";// arananArtikel.VkPreis.ToString();
                    VkPreis();
                    if (SatisFiyat > 999.99)
                    {
                        lblParaUstu.Text = "PREIS ERROR!";
                        Console.Beep(100, 100);
                        Console.Beep(200, 200);
                        return;
                    }
                    else if (SatisFiyat != 0)
                    {
                        iss_Artikel.Artikel WaageArtikel = new iss_Artikel.Artikel();


                        WaageArtikel.ArtikelId = 0;
                        WaageArtikel.ArtikelAd = btnTiklanan.Text;
                        WaageArtikel.EkPreis = 0;
                        WaageArtikel.VkPreis = SatisFiyat;
                        WaageArtikel.Grubid = Convert.ToInt32(btnTiklanan.Name);
                        WaageArtikel.Mwst = new Artikel_Grup.ArtikelGrup(WaageArtikel.Grubid).Mwst; //ArtikelGrup(WaageArtikel.Grubid).
                        WaageArtikel.Gruptur = 3;
                        WaageArtikel.Fand = 0;


                        ept.ProgramAyarlar = Program.ProgramAyarlar;
                        ept.WaagePortName = Program.ProgramAyarlar["WPORT"];
                        // ept.BenimEventim += new etp_extended_main.DisplayDelegate(DSPINFO);
                        satisYap = new SatisYap();
                        ept_extended.SatisYap_ept WaageResultPos = new SatisYap_ept();
                        WaageResultPos = ept.WaageMitPLU(WaageArtikel);
                        if (WaageResultPos.errorMeldung == "")
                        {
                            satisYap.Adet = WaageResultPos.Adet;
                            //satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = WaageResultPos.Mwst;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Gruptur = WaageResultPos.Gruptur;
                            satisYap.Alisfiyat = WaageResultPos.Alisfiyat;
                            satisYap.Satisfiyat = WaageResultPos.Satisfiyat;
                            satisYap.Gv_typ_id = WaageResultPos.Gv_typ_id;
                            satisYap.Tarih = WaageResultPos.Tarih;
                            satisYap.Einheit = WaageResultPos.Einheit;
                            satisYap.Toplamtutar = Math.Round(WaageResultPos.Toplamtutar, 2);
                            satisYap.Angebotvarmi = WaageResultPos.Angebotvarmi;
                            if (satisYap.Toplamtutar >= 50)
                            {

                                F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                                frmSumme.summe = satisYap.Toplamtutar;
                                frmSumme.ShowDialog();
                                if (frmSumme.bestatigung != 1)
                                {
                                    satisYap = null;
                                    return;
                                }

                            }

                            satisYap.UrunId = WaageResultPos.UrunId;
                            satisYap.UrunAd = angebotSembol + WaageResultPos.UrunAd.Replace("'", "");
                            satisYap.Birimkar = (WaageResultPos.Birimkar) * adet;
                            satisYap.Grubid = WaageResultPos.Grubid;
                            satisYap.Fand = WaageResultPos.Fand;
                            satisYap.Birimid = WaageResultPos.Birimid;
                            satisYap.Barkod = WaageResultPos.Barkod;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.KasiyerId = Program.bedID;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count1 = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count1].SubItems.Add(angebotSembol + satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet.ToString("#0.000") + " kg. x " + satisYap.Satisfiyat.ToString("C") + "/kg.)");
                            listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count1].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            angebotSembol = "";
                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                            satisYap = null;
                            Console.Beep(800, 100);
                            Console.Beep(1000, 100);
                            return;
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = WaageResultPos.errorMeldung;
                            Console.Beep(1000, 1000);
                            frmerror.ShowDialog();

                            return;

                        }

                    }
                }
                catch (Exception dd)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = dd.Message;
                    Console.Beep(1000, 1000);
                    frmerror.ShowDialog();

                    return;
                }
            }
            else if (btnTiklanan.TabIndex == 9)
            {
                try
                {
                    if ((Program.IsletmeAyarlar["HandyAufladeUsername"] != "") && (Program.IsletmeAyarlar["HandyAufladePassword"] != ""))
                    {
                        using (F_HandyAuflade fHandyAuflade = new F_HandyAuflade())
                        {
                            fHandyAuflade.grupid = Convert.ToInt16(btnTiklanan.Name);
                            // fHandyAuflade.Handy = Handy;
                            fHandyAuflade.ShowDialog();


                            if (fHandyAuflade.sonuc == true)
                            {

                                SatisFiyat = Convert.ToDouble(fHandyAuflade.Info["preis"]);
                                grupTur = 0;
                                urunad = fHandyAuflade.Info["artikelname"].ToString();
                                if (txtGiris.Text != "")
                                {
                                    if (double.TryParse(txtGiris.Text, out SatilanAdet))
                                    {
                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = Program.lang["63"];
                                        frmerror.ShowDialog();
                                        SatilanAdet = 1;
                                    }

                                }
                                else
                                {
                                    SatilanAdet = 1;
                                }
                                if (SatilanAdet == 0)
                                {
                                    SatilanAdet = 1;//adet;
                                }
                                if (SatilanAdet > 0)
                                {
                                    if (SatilanAdet == 0) return;
                                    if (SatisFiyat == 0) return;
                                    Tarih tarih = new Tarih();
                                    satisYap = new SatisYap();
                                    if (fHandyAuflade.Info["cardtyp"].ToString() == "DR")
                                    {
                                        iss_HandyMopin.LoginInfo Loginresult;
                                        iss_HandyMopin.iss_HandyAuflade_Main_Mopin Handy = new iss_HandyMopin.iss_HandyAuflade_Main_Mopin();
                                        Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                                        Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                                        Handy.url = Program.IsletmeAyarlar["HandyAufladeAPI"];
                                        Loginresult = Handy.Login();
                                        if (Loginresult.token != "")
                                        {
                                            F_HandyAufladeDR handyDR = new F_HandyAufladeDR();
                                            // frmobstGemuseStuck.BarcodeSatisEvent += new F_ObstGemuseStuck.BarkodSatisDelagate(ObstGemDelegate);
                                            handyDR.TelNrEvent += new F_HandyAufladeDR.TelNrEingabeDelegate(DirekAuflade);
                                            handyDR.ShowDialog();
                                            if (handyDR.code != "")
                                            {
                                                DR_transaction HandyVerkaufResult = new DR_transaction();
                                                HandyVerkaufResult = Handy.DR_Transaction(Loginresult.token, handyDR.code, fHandyAuflade.Info["artikelid"].ToString());

                                                if (HandyVerkaufResult.message_type == 1)
                                                {

                                                    MySqlConnection conn = new MySqlConnection();
                                                    db baglanti = new db();
                                                    conn = baglanti.myconn();
                                                    if (conn.State == ConnectionState.Closed)
                                                    {
                                                        conn.Open();
                                                    }
                                                    string SQL = "";
                                                    //INSERT INTO `handyaufladelog`(`id`, `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES 
                                                    //([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13],[value-14])



                                                    SQL = "INSERT INTO `handyaufladelog`( `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES" +
                                                        "(" + tarih.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + HandyVerkaufResult.transaction_id + "','" + fHandyAuflade.Info["artikelid"] + "','" + "','" + "" + "','" + HandyVerkaufResult.transaction_id + "','" + HandyVerkaufResult.product + "','" + HandyVerkaufResult.product_instructions + "'," +
                                                         HandyVerkaufResult.total_amount.ToString().Replace(',', '.') + "," + Program.bedID + ",'" + HandyVerkaufResult.message + "','" + HandyVerkaufResult.message_type + "')";
                                                    MySqlCommand cmdInsert = new MySqlCommand(SQL, conn);
                                                    if (cmdInsert.ExecuteNonQuery() > 0)
                                                    {
                                                        //VerkauftePin.batchnumber = Verkauf.Pinliste[0].batchnumber;
                                                        //      VerkauftePin.cardid = artikelid.ToString();
                                                        // VerkauftePin.expirydate = Verkauf.Pinliste[0].cardid;
                                                        // VerkauftePin.pinnumber = Verkauf.Pinliste[0].cardid;
                                                        //VerkauftePin.transactionid = Verkauf.Pinliste[0].cardid;
                                                        //VerkauftePin.CardName = Verkauf.CardInfo.cardname;
                                                        // VerkauftePin.Insroduction = Verkauf.CardInfo.instruction;
                                                        SoldPinlist soldPin = new SoldPinlist();
                                                        soldPin.batchnumber = HandyVerkaufResult.transaction_id.ToString();
                                                        soldPin.CardName = HandyVerkaufResult.product;
                                                        //soldPin.expirydate = HandyVerkaufResult..expirydate;
                                                        soldPin.Instruction = HandyVerkaufResult.product_instructions;
                                                        soldPin.pinnumber = HandyVerkaufResult.transaction_id.ToString();
                                                        soldPin.purchaseprice = HandyVerkaufResult.total_amount.ToString().Replace(',', '.');
                                                        soldPin.transactionid = HandyVerkaufResult.transaction_id.ToString();
                                                        //soldPin.rate = HandyVerkaufResult.data.cardinfo.rate.ToString().Replace(',', '.');
                                                        soldPin.cardid = fHandyAuflade.Info["artikelid"].ToString();
                                                        //soldPin.customercare=HandyVerkaufResult.cus
                                                        yeniFis.PinlistSold.Add(soldPin);

                                                    }
                                                    else
                                                    {
                                                        F_GenericError frmerror = new F_GenericError();
                                                        frmerror.lblMesaj.Text = "Fehler beim Verkauf Direct Recharge: " + HandyVerkaufResult.message + "\nEntweder nochmal versuchen oder entfernen diese Produkt(e) von der Verkauflist!!";
                                                        frmerror.ShowDialog();
                                                    }

                                                }
                                                else
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Fehler beim Verkauf Direct Recharge: " + HandyVerkaufResult.message + "\nEntweder nochmal versuchen oder entfernen diese Produkt(e) von der Verkauflist!!";
                                                    frmerror.ShowDialog();

                                                }

                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = "Fehler beim Verkauf Direct Recharge Anmeldung\nBitte informieren Sie Ihren Verkäufer!!";
                                            frmerror.ShowDialog();
                                        }

                                    }
                                    else
                                    {
                                        for (int a = 0; a < SatilanAdet; a++)
                                        {
                                            SellPinList SellList = new SellPinList();
                                            SellList.cardid = fHandyAuflade.Info["artikelid"].ToString();
                                            SellList.Menge = "1";
                                            // VerkauftePin.CardName = fHandyAuflade.Info["artikelname"].ToString();
                                            //  VerkauftePin.purchaseprice = fHandyAuflade.Info["preis"].ToString();
                                            // VerkauftePin.pinnumber = Verkauf.Pinliste[0].cardid;
                                            //VerkauftePin.transactionid = Verkauf.Pinliste[0].cardid;
                                            //VerkauftePin.CardName = Verkauf.CardInfo.cardname;
                                            // VerkauftePin.Insroduction = Verkauf.CardInfo.instruction;
                                            //satisYap.PinlistVerkauf.Add(VerkauftePin);  


                                            yeniFis.PinlistSell.Add(SellList);
                                        }
                                    }
                                    satisYap.Barkod = fHandyAuflade.Barcode;
                                    satisYap.Adet = SatilanAdet;//
                                    satisYap.isHandyAuflade = 1;
                                    if (fHandyAuflade.Info["cardtyp"].ToString() == "DR")
                                    {
                                    }
                                    else
                                    {
                                        satisYap.Cardid = Convert.ToInt32(fHandyAuflade.Info["artikelid"]);
                                    }
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = Convert.ToInt16(btnTiklanan.Tag);
                                    satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Satisfiyat = SatisFiyat;//
                                    satisYap.Birimid = 1;
                                    satisYap.UrunAd = angebotSembol + fHandyAuflade.Info["artikelname"];//
                                    if (satisYap.UrunAd.Substring(0, 1) == "*")
                                    {
                                        satisYap.Angebotvarmi = 1;

                                    }
                                    else
                                    {
                                        if (new ArtikelGrup(Convert.ToInt16(btnTiklanan.Name)).Rabatpunkte != 1)
                                        {
                                            satisYap.Angebotvarmi = 1;
                                            angebotSembol = "*";
                                        }
                                        else
                                        {
                                            satisYap.Angebotvarmi = 0;
                                            angebotSembol = "";
                                        }
                                    }
                                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                    if (counter_waage == 1)
                                    {
                                        satisYap.Toplamtutar = WaageTutar;
                                    }
                                    else
                                    {
                                        satisYap.Toplamtutar = Math.Round(SatisFiyat * SatilanAdet, 2);
                                    }
                                    if (satisYap.Toplamtutar >= 50)
                                    {

                                        F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                                        frmSumme.summe = satisYap.Toplamtutar;
                                        frmSumme.ShowDialog();
                                        if (frmSumme.bestatigung != 1)
                                        {
                                            satisYap = null;
                                            return;
                                        }

                                    }

                                    satisYap.UrunId = artikelid;//
                                    satisYap.Birimkar = karmiktari;//
                                    satisYap.Grubid = Convert.ToInt16(btnTiklanan.Name);

                                    satisYap.Gruptur = grupTur;

                                    //
                                    if (Program.IsletmeAyarlar["markt"] == "2")
                                    {

                                        F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                                        FUserAuswahl.satilanPozition = satisYap;
                                        FUserAuswahl.UserList = UserList;
                                        FUserAuswahl.ShowDialog();
                                        if (FUserAuswahl.SecilenUser != -1)
                                        {
                                            FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                                            yeniFis = null;
                                            if (ElemanFisi == null)
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                                                KundenDisplay();
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                            }
                                            else
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis = ElemanFisi;
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                                listView1.Items.Clear();
                                                KundenDisplay();
                                                int num2 = 1;
                                                int count = listView1.Items.Count;
                                                foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                                                {

                                                    listView1.Items.Add(num2.ToString());
                                                    listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                                    listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                                    DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                    ++num2;
                                                    count++;
                                                }
                                                position = num2;
                                                yeniFis.FisiKapat();


                                            }
                                            yeniFis.SatisKalem.Add(satisYap);
                                            //yeniFis.PinlistVerkauf.Add(VerkauftePin);
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        satisYap.Fisno = yeniFis.SatisAnaId;
                                        yeniFis.SatisKalem.Add(satisYap);

                                    }

                                    //
                                    int listViewElaman = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    if (Program.Waage == 2)
                                    {
                                        listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString("C") + "x" + SatisFiyat.ToString("C") + ")");
                                    }
                                    else
                                    {
                                        if (counter_waage == 1)
                                        {
                                            listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString("#0.000") + "kg x" + SatisFiyat.ToString("C") + "/kg)");
                                        }
                                        else
                                        {
                                            listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString() + "St. x" + SatisFiyat.ToString("C") + "/St.)");
                                        }

                                    }

                                    listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[listViewElaman].Tag = satisYap.Barkod;
                                    if (listView1.Items.Count > 0)
                                    {
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    }

                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    position++;
                                    ///

                                    txtGiris.Text = "";




                                    pfandid = 0;
                                    //KDbirinciSatiraYaz(urunad, String.Format("{0:n}", SatilanAdet) + "x" + SatisFiyat.ToString());
                                    if (dsp != null)
                                    {
                                        if (Program.displayType != "IBM")
                                        {
                                            if (counter_waage == 1)
                                            {
                                                // KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "Kg. x" + SatisFiyat.ToString() + "€/Kg");
                                                //KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString()+"€");
                                                KDbirinciSatiraYaz(urunad, "");
                                                KDikinciSatiraYaz(SatilanAdet.ToString("#0.000") + "kg x " + SatisFiyat.ToString("C") + "/kg");
                                                // knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString("#0.000") + "kg x " + satisYap.Satisfiyat.ToString("C") + "/kg", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                                                DSPINFO(urunad, satisYap.Adet.ToString() + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, -1);
                                            }
                                            else
                                            {
                                                KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St");
                                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                                                //knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString() + "St.x" + satisYap.Satisfiyat.ToString("C") + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                                            }
                                        }
                                        else
                                        {
                                            if (Program.displayType == "TVS")
                                            {
                                                CheckForIllegalCrossThreadCalls = false;
                                                Thread dssp = new Thread(() => this.DSPINFO(urunad, SatilanAdet + " Stk.", SatisFiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                                                dssp.Start();

                                            }
                                            else
                                            {
                                                KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString() + ((char)213) + "/St");
                                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + ((char)213));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Program.displayType == "TVS")
                                        {
                                            CheckForIllegalCrossThreadCalls = false;
                                            Thread dssp = new Thread(() => this.DSPINFO(urunad, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                                            dssp.Start();
                                            // knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString() + "St.x" + satisYap.Satisfiyat.ToString("C")  + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C") );
                                        }
                                        else if (serialPortKD2.IsOpen == true)
                                        {


                                            KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "x" + SatisFiyat.ToString("#0.00"));
                                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                            // knddsply.VerkaufInfo(urunad + "\n" + SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                                        }
                                    }
                                    //knddsply.VerkaufInfo(urunad + "\n" + SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St" + (satisYap.Adet>1?"\n" +"Summe: "+ satisYap.Toplamtutar.ToString():"") ,"TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                                    SatilanAdet = 0;
                                    SatisFiyat = 0;
                                    counter_waage = 0;
                                    Console.Beep(800, 100);
                                    Console.Beep(1500, 100);
                                    return;
                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = Program.lang["21"];
                                    frmerror.ShowDialog();
                                }
                                // txtGiris.Text = SatilanAdet + "x" + SatisFiyat;
                                //karmiktari = SatilanAdet * karmiktari;


                            }
                            else
                            {
                                return;
                            }
                        }



                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "IHR KONTO IST NOCH NICHT AKTIV!\nBitte kontaktieren Sie mit dem folgenden Horline-nummer:\n+49 211 54080656 \n +49 157 85072232";
                        frmerror.ShowDialog();
                    }
                }
                catch (Exception ff)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = ff.Message;
                    frmerror.ShowDialog();
                }


            }
            else
            {
                VkPreis();

                if (SatilanAdet == 0) return;
                if (SatisFiyat == 0) return;
                artikelid = 0;
                karmiktari = 0;
                urunad = btnTiklanan.Text;


            }
            if (txtGiris.Text != "")
            {
                if (SatilanAdet == 0)
                {
                    SatilanAdet = adet;
                }
                if (SatilanAdet == 0) return;
                if (SatisFiyat == 0) return;
                Tarih tarih = new Tarih();
                satisYap = new SatisYap();
                satisYap.Adet = SatilanAdet;//

                satisYap.KasaNo = Program.kasano;
                satisYap.Mwst = Convert.ToInt16(btnTiklanan.Tag);
                satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                satisYap.Satisfiyat = SatisFiyat;//
                satisYap.Birimid = 1;
                if (urunad.Substring(0, 1) == "*")
                {
                    satisYap.Angebotvarmi = 1;

                }
                else
                {
                    if (new ArtikelGrup(Convert.ToInt16(btnTiklanan.Name)).Rabatpunkte != 1)
                    {
                        satisYap.Angebotvarmi = 1;
                        angebotSembol = "*";
                    }
                    else
                    {
                        satisYap.Angebotvarmi = 0;
                        angebotSembol = "";
                    }
                }
                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                if (counter_waage == 1)
                {
                    satisYap.Toplamtutar = WaageTutar;
                }
                else
                {
                    satisYap.Toplamtutar = Math.Round(SatisFiyat * SatilanAdet, 2);
                }
                if (satisYap.Toplamtutar >= 50)
                {

                    F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                    frmSumme.summe = satisYap.Toplamtutar;
                    frmSumme.ShowDialog();
                    if (frmSumme.bestatigung != 1)
                    {
                        satisYap = null;
                        return;
                    }

                }

                satisYap.UrunId = artikelid;//
                satisYap.Birimkar = karmiktari;//
                satisYap.Grubid = Convert.ToInt16(btnTiklanan.Name);
                satisYap.UrunAd = angebotSembol + urunad;//
                satisYap.Gruptur = grupTur;
                //
                if (Program.IsletmeAyarlar["markt"] == "2")
                {

                    F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                    FUserAuswahl.satilanPozition = satisYap;
                    FUserAuswahl.UserList = UserList;
                    FUserAuswahl.ShowDialog();
                    if (FUserAuswahl.SecilenUser != -1)
                    {
                        FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                        toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                        yeniFis = null;
                        if (ElemanFisi == null)
                        {
                            FisOlustur yeniFis1 = new FisOlustur();
                            yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                            yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                            KundenDisplay();
                            satisYap.Fisno = yeniFis.SatisAnaId;
                        }
                        else
                        {
                            FisOlustur yeniFis1 = new FisOlustur();
                            yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                            yeniFis = ElemanFisi;
                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            listView1.Items.Clear();
                            KundenDisplay();
                            int num2 = 1;
                            int count = listView1.Items.Count;
                            foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                            {

                                listView1.Items.Add(num2.ToString());
                                listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                ++num2;
                                count++;
                            }
                            position = num2;
                            yeniFis.FisiKapat();



                        }
                        yeniFis.SatisKalem.Add(satisYap);
                        verkauferList[Program.bedID] = yeniFis;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    yeniFis.SatisKalem.Add(satisYap);
                    LastPos = satisYap;
                }

                //

                int listViewElaman = listView1.Items.Count;
                listView1.Items.Add(position.ToString());
                if (Program.Waage == 2)
                {
                    listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString("C") + "x" + SatisFiyat.ToString("C") + ")");
                }
                else
                {
                    if (counter_waage == 1)
                    {
                        listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString("#0.000") + "kg x" + SatisFiyat.ToString("C") + "/kg)");
                    }
                    else
                    {
                        listView1.Items[listViewElaman].SubItems.Add(angebotSembol + urunad + " (" + SatilanAdet.ToString() + "St. x" + SatisFiyat.ToString("C") + "/St.)");
                    }

                }

                listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                listView1.Items[listViewElaman].Tag = satisYap.Barkod == null ? "0" : satisYap.Barkod;
                if (listView1.Items.Count > 0)
                {
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }

                yeniFis.FisiKapat();
                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                position++;
                ///
                if (pfandid != 0)
                {
                    Artikel furun = new Artikel();
                    furun.ArtikelBul(pfandid.ToString());
                    if (furun.urunvarmi == true)
                    {
                        satisYap = new SatisYap();
                        satisYap.Adet = SatilanAdet;
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.Barkod = furun.BarkodNo;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = furun.Mwst;
                        satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Satisfiyat = furun.VkPreis;
                        satisYap.Tarih = tarih.unixdate(DateTime.Now);
                        satisYap.Toplamtutar = Math.Round(furun.VkPreis * SatilanAdet, 2);
                        satisYap.UrunId = furun.ArtikelId;
                        satisYap.UrunAd = furun.ArtikelAd;
                        satisYap.Birimkar = (furun.VkPreis - furun.EkPreis) * SatilanAdet;
                        satisYap.Grubid = furun.Grubid;
                        satisYap.Fand = 1;
                        satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                        satisYap.Birimid = 1;
                        yeniFis.SatisKalem.Add(satisYap);
                        listViewElaman = listView1.Items.Count;
                        listView1.Items.Add("");
                        listView1.Items[listViewElaman].SubItems.Add(furun.ArtikelAd.ToString() + "(" + SatilanAdet + "x" + furun.VkPreis.ToString("C") + ")");
                        listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        //listView1.SelectedItems[listView1.Items.Count - 1].Focused = true;
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        position++;
                    }
                }
                txtGiris.Text = "";




                pfandid = 0;
                //KDbirinciSatiraYaz(urunad, String.Format("{0:n}", SatilanAdet) + "x" + SatisFiyat.ToString());
                if (dsp != null)
                {
                    if (Program.displayType != "IBM")
                    {
                        if (counter_waage == 1)
                        {
                            // KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "Kg. x" + SatisFiyat.ToString() + "€/Kg");
                            //KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString()+"€");
                            KDbirinciSatiraYaz(urunad, "");
                            KDikinciSatiraYaz(SatilanAdet.ToString("#0.000") + "kg x " + SatisFiyat.ToString("C") + "/kg");
                            // knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString("#0.000") + "kg x " + satisYap.Satisfiyat.ToString("C") + "/kg", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                            DSPINFO(urunad, satisYap.Adet.ToString() + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, -1);
                        }
                        else
                        {
                            KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St");
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                            //knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString() + "St.x" + satisYap.Satisfiyat.ToString("C") + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                        }
                    }
                    else
                    {
                        if (Program.displayType == "TVS")
                        {
                            CheckForIllegalCrossThreadCalls = false;
                            Thread dssp = new Thread(() => this.DSPINFO(urunad, SatilanAdet + " Stk.", SatisFiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                            dssp.Start();

                        }
                        else if (counter_waage == 1)
                        {

                            // KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "Kg. x" + SatisFiyat.ToString() + "€/Kg");
                            //KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString()+"€");
                            KDbirinciSatiraYaz(urunad, "");
                            KDikinciSatiraYaz(SatilanAdet.ToString("#0.000") + "kg x " + SatisFiyat.ToString() + ((char)213) + "/kg");


                        }
                        else
                        {
                            KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString() + ((char)213) + "/St");
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + ((char)213));
                        }
                    }
                }
                else
                {
                    if (counter_waage == 1)
                    {
                        if (Program.displayType == "TVS")
                        {
                            DSPINFO(urunad, satisYap.Adet.ToString() + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        }
                        else
                        {
                            KDbirinciSatiraYaz(urunad, "");
                            KDikinciSatiraYaz(SatilanAdet.ToString("#0.000") + "kg x " + SatisFiyat.ToString() + ((char)213) + "/kg");
                        }
                    }
                    else if (Program.displayType == "TVS")
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        Thread dssp = new Thread(() => this.DSPINFO(urunad, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                        dssp.Start();
                        // knddsply.VerkaufInfo(urunad + "\n" + satisYap.Adet.ToString() + "St.x" + satisYap.Satisfiyat.ToString("C")  + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C") );
                    }
                    else if (serialPortKD2.IsOpen == true)
                    {


                        KDbirinciSatiraYaz(urunad, SatilanAdet.ToString() + "x" + SatisFiyat.ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        // knddsply.VerkaufInfo(urunad + "\n" + SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St", "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                    }
                }
                //knddsply.VerkaufInfo(urunad + "\n" + SatilanAdet.ToString() + "St.x" + SatisFiyat.ToString("C") + "/St" + (satisYap.Adet>1?"\n" +"Summe: "+ satisYap.Toplamtutar.ToString():"") ,"TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                SatilanAdet = 0;
                SatisFiyat = 0;
                counter_waage = 0;

            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["21"];
                frmerror.ShowDialog();
            }


        }
        public void ObstGemusePLU(string brkdOG)
        {
            Bitmap snap = null;
            if ((Program.ProgramAyarlar["Cam1"] != "") && (Program.ProgramAyarlar["Cam1"] != null))
            {
                lock (_frameLock)
                {
                    if (_lastFrame != null)
                        snap = (Bitmap)_lastFrame.Clone();
                }
            }


            iss_Artikel.Artikel arananArtikel = new iss_Artikel.Artikel();
            arananArtikel.ArtikelBul(brkdOG);
            if (arananArtikel.urunvarmi == true)
            {
                if (snap != null)
                {
                    try
                    {
                        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AI\\image\\" + tarih.unixdate(DateTime.Now) + ".jpg");
                        snap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        CheckForIllegalCrossThreadCalls = false;
                        Thread ObstGemStkAI = new Thread(() => this.GenericRegisterImage(path, gelenBarkod));
                        ObstGemStkAI.Start();

                    }
                    finally
                    {
                        snap.Dispose();
                    }
                }
                if (arananArtikel.VkPreis <= 0)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "DER ARTIKELPREIS IST 0 (NULL)!";
                    Console.Beep(1000, 1000);
                    frmerror.ShowDialog();

                    return;
                }

                int grupNo = arananArtikel.Grubid;
                Artikel_Grup.ArtikelGrup grupBul = new Artikel_Grup.ArtikelGrup(grupNo);
                SatisFiyat = arananArtikel.VkPreis;
                string urunad = arananArtikel.ArtikelAd;
                long artikelid = arananArtikel.ArtikelId;
                if (grupBul.GrupTur == 2 || grupBul.GrupTur == 1) //Satıs
                {
                    gelenBarkod = brkdOG;
                    barkodluUrunEkle();
                    Console.Beep(800, 100);
                    Console.Beep(1500, 100);
                    return;
                }
                else if (grupBul.GrupTur == 3) //waage
                {
                    ept.ProgramAyarlar = Program.ProgramAyarlar;
                    ept.WaagePortName = Program.ProgramAyarlar["WPORT"];
                    //ept.BenimEventim += new etp_extended_main.DisplayDelegate(DSPINFO);
                    double KundenPreis = 0, NormalPreis = 0;
                    if (yeniFis.Musterino != 0)
                    {
                        if (yeniFis.Musteri.Method == 5)
                        {
                            NormalPreis = arananArtikel.VkPreis;
                            KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, arananArtikel.BarkodNo);
                            if (KundenPreis != 0)
                            {

                                arananArtikel.VkPreis = KundenPreis;
                                angebotSembol = "#";
                            }
                            else
                            {
                                angebotSembol += "";
                                satisYap.Satisfiyat = arananArtikel.VkPreis;
                            }
                        }
                    }
                    satisYap = new SatisYap();
                    ept_extended.SatisYap_ept WaageResultPos = new SatisYap_ept();
                    WaageResultPos = ept.WaageMitPLU(arananArtikel);
                    if (WaageResultPos.errorMeldung == "")
                    {
                        satisYap.Adet = WaageResultPos.Adet;
                        if (yeniFis.Musterino != 0)
                        {
                            if (yeniFis.Musteri.Method == 5)
                            {
                                satisYap.ProzisyonRabatBetrag = (NormalPreis - KundenPreis) * satisYap.Adet;
                            }
                        }
                        //satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = WaageResultPos.Mwst;
                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Gruptur = WaageResultPos.Gruptur;
                        satisYap.Alisfiyat = WaageResultPos.Alisfiyat;
                        satisYap.Satisfiyat = WaageResultPos.Satisfiyat;
                        satisYap.Gv_typ_id = WaageResultPos.Gv_typ_id;
                        satisYap.Tarih = WaageResultPos.Tarih;
                        satisYap.Einheit = WaageResultPos.Einheit;
                        satisYap.Toplamtutar = Math.Round(WaageResultPos.Toplamtutar, 2);
                        satisYap.Angebotvarmi = WaageResultPos.Angebotvarmi;

                        if (satisYap.Toplamtutar >= 50)
                        {

                            F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                            frmSumme.summe = satisYap.Toplamtutar;
                            frmSumme.ShowDialog();
                            if (frmSumme.bestatigung != 1)
                            {
                                satisYap = null;
                                return;
                            }

                        }

                        satisYap.UrunId = WaageResultPos.UrunId;
                        satisYap.UrunAd = angebotSembol + WaageResultPos.UrunAd.Replace("'", "");
                        satisYap.Birimkar = (WaageResultPos.Birimkar) * adet;
                        satisYap.Grubid = WaageResultPos.Grubid;
                        satisYap.Fand = WaageResultPos.Fand;
                        satisYap.Birimid = WaageResultPos.Birimid;
                        satisYap.Barkod = WaageResultPos.Barkod;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.KasiyerId = Program.bedID;
                        if (Program.IsletmeAyarlar["markt"] == "2")
                        {

                            F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                            FUserAuswahl.satilanPozition = satisYap;
                            FUserAuswahl.UserList = UserList;
                            FUserAuswahl.ShowDialog();
                            if (FUserAuswahl.SecilenUser != -1)
                            {
                                FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                                yeniFis = null;
                                if (ElemanFisi == null)
                                {
                                    FisOlustur yeniFis1 = new FisOlustur();
                                    yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                    yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                                    toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                                    KundenDisplay();
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                }
                                else
                                {
                                    FisOlustur yeniFis1 = new FisOlustur();
                                    yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                    yeniFis = ElemanFisi;
                                    toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    listView1.Items.Clear();
                                    KundenDisplay();
                                    int num2 = 1;
                                    int count = listView1.Items.Count;
                                    foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                                    {

                                        listView1.Items.Add(num2.ToString());
                                        listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                        listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                        DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        ++num2;
                                        count++;
                                    }
                                    position = num2;
                                    yeniFis.FisiKapat();



                                }
                                yeniFis.SatisKalem.Add(satisYap);
                                verkauferList[Program.bedID] = yeniFis;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            yeniFis.SatisKalem.Add(satisYap);
                            LastPos = satisYap;
                        }
                        //yeniFis.SatisKalem.Add(satisYap);
                        int count1 = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count1].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet.ToString("#0.000") + " kg. x " + satisYap.Satisfiyat.ToString("C") + "/kg.)");
                        listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count1].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        angebotSembol = "";
                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        satisYap = null;
                        Console.Beep(800, 100);
                        Console.Beep(1000, 100);
                        return;
                    }
                    else
                    {

                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = WaageResultPos.errorMeldung;
                        Console.Beep(1000, 1000);
                        frmerror.ShowDialog();

                        return;
                    }


                }


            }
            else
            {
                UrunYok();
                return;
            }


        }
        public void GenericRegisterImage(string path, string Name)
        {
            iss_smart_AI iss_Smart_AI = new iss_smart_AI();
            iss_Smart_AI.register_image(path, 0, Name);

        }
        private void DSPENDE(string mesaj)
        {
            Application.DoEvents();
            knddsply.VerkaufInfo(mesaj, "");
            if (!dssped.IsAlive)
                return;
            dssped.Suspend();
        }
        /* 0: Normal Satis
         * 1: Position St0rno
         * 2: Zahlung
         * 3:Verkaufslist Clear
         * 4: Pause
         * 5. Abmelden
         * 
         * 
         * 
         * 
         */
        string CamTextList = "";

        private void DSPINFO(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verilenPara, double paraustu, int odemetur)
        {
            try
            {

                if (Program.ProgramAyarlar["CamKas"] != "")
                {

                    if (Program.ProgramAyarlar["CamKas"] != null)
                    {
                        if (Program.ProgramAyarlar["CamKas"] != "0")
                        {
                            string Info = "";
                            string[] SatisfiyatInfo, posTutarInfo, topTutarInfo;
                            Font fnt = new Font("Impact", 20, FontStyle.Bold);
                            Font fnt2 = new Font("Impact", 20, FontStyle.Strikeout);
                            Label lblTest = new Label();
                            switch (islemtur)
                            {
                                case 0:

                                    SatisfiyatInfo = satisfiyat.Split(' ');
                                    posTutarInfo = postoplam.Split(' ');
                                    topTutarInfo = toplamtutar.Split(' ');
                                    lblTest.Font = fnt;

                                    lblTest.Text = "VERKAUF \n" + new String('-', 20) + "\n" + urunad + "\n" + adet + " x " + (SatisfiyatInfo.Length > 0 ? SatisfiyatInfo[0] : "") + " = " + (posTutarInfo.Length > 0 ? posTutarInfo[0] : "") + "\n" + new String('-', 20) + "\nBON SUMME" + (topTutarInfo.Length > 0 ? topTutarInfo[1] : "") + "\n" + new String('-', 20);
                                    Info = lblTest.Text;
                                    CamTextList += Info + "\n";
                                    break;
                                case 1:
                                    //string[] SatisfiyatInfo, posTutarInfo, topTutarInfo;
                                    SatisfiyatInfo = satisfiyat.Split(' ');
                                    posTutarInfo = postoplam.Split(' ');
                                    topTutarInfo = toplamtutar.Split(' ');
                                    lblTest.Font = fnt2;
                                    lblTest.Text = "POS. STORNO\n" + new String('-', 20) + "\n" + urunad + "\n" + adet + " x " + (SatisfiyatInfo.Length > 0 ? SatisfiyatInfo[0] : "") + " \n" + new String('-', 20) + "\nPOS. SUMME: " + (posTutarInfo.Length > 0 ? posTutarInfo[0] : "") + " \n" + new String('-', 20) + "\nBON-SUMME" + (topTutarInfo.Length > 0 ? topTutarInfo[1] : "") + "";
                                    Info = lblTest.Text;
                                    CamTextList += Info + "\n";
                                    break;
                                case 2:
                                    if (odemetur == 1)
                                    {
                                        topTutarInfo = toplamtutar.Split(' ');
                                        Info = "BAR ZAHLUNG\n" + new String('-', 20) + "\nBON _SUMME: " + (topTutarInfo.Length > 0 ? topTutarInfo[0] : "") + "\n" + new String('-', 20) + "\nGEGEBEN: " + verilenPara.ToString("C") + "\n" + new String('-', 20) + "\nRUCKGELD: " + paraustu.ToString("C") + "";
                                        break;
                                    }
                                    else if (odemetur == 0)
                                    {
                                        topTutarInfo = toplamtutar.Split(' ');
                                        Info = "EC/CARD ZAHLUNG\n" + new String('-', 20) + "\nBON_SUMME :" + (topTutarInfo.Length > 0 ? topTutarInfo[1] : "") + " \n" + new String('-', 20) + "\nGEGEBEN: " + verilenPara.ToString("C") + "\n" + new String('-', 20) + "\nRUCKGELD :" + paraustu.ToString("C") + "";
                                        break;
                                    }
                                    else
                                    {
                                        topTutarInfo = toplamtutar.Split(' ');
                                        Info = "ZAHLUNG\n" + new String('-', 20) + "\nBON_SUMME :" + (topTutarInfo.Length > 0 ? topTutarInfo[1] : "") + "\n" + new String('-', 20) + "\nGEGEBEN: " + verilenPara.ToString("C") + " \n" + new String('-', 20) + "\nRUCKGELD :" + paraustu.ToString("C") + "";
                                        break;
                                    }
                                case 3:
                                    if (bonStornoFlag != 0)
                                        Info = "!!!! BON STORNO !!!! ";
                                    bonStornoFlag = 0;
                                    break;
                                /* case 5:
                                      {
                                          if (bonStornoFlag != 0)
                                          {
                                              lblTest.Text = "!!!! BON STORNO !!!! ";
                                              break;
                                          }
                                      }
                                
                                 case 6:
                                     kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                     return;
                                 case 7:
                                     kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                     return;
                                 case 8:
                                     kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                     return;
                                 case 9:
                                     kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                     return;*/
                                default:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                            }
                            byte[] bytes = new byte[1024];
                            Socket sender1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            sender1.Connect(IPAddress.Parse(Program.ProgramAyarlar["CamIP"]), Convert.ToInt32(Program.ProgramAyarlar["CamPORT"]));


                            // Encode the data string into a byte array.   

                            byte[] msg = Encoding.ASCII.GetBytes(Info.ToUpper());

                            // Send the data through the socket.    
                            int bytesSent = sender1.Send(msg);

                            // Receive the response from the remote device.    
                            // int bytesRec = sender1.Receive(bytes);
                            // Console.WriteLine("Echoed test = {0}",
                            // Encoding.ASCII.GetString(bytes, 0, bytesRec));

                            // Release the socket.    
                            sender1.Shutdown(SocketShutdown.Both);
                            sender1.Close();
                        }
                    }

                }
            }

            catch (Exception rr)
            {
                AddtoLogFile(rr + " Cam Kontrol - DSPINFO", "2018");
                Program.ProgramAyarlar["CamKas"] = "0";

            }
            if (Program.ProgramAyarlar["DispLayout"] != "")
            {
                if (Program.ProgramAyarlar["DispLayout"] != null)
                {
                    try
                    {
                        if (Program.ProgramAyarlar["DispLayout"] == "V1")
                        {
                            if (kndDispV1 == null)
                                return;
                            switch (islemtur)
                            {
                                case 0:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 2:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 4:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 5:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 6:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 7:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 8:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 9:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 11:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                default:
                                    kndDispV1.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                            }
                        }
                        else
                        {
                            if (!(Program.ProgramAyarlar["DispLayout"] == "V2") || kndDispV2 == null)
                                return;
                            switch (islemtur)
                            {
                                case 0:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 2:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 4:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 5:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 6:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 7:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 8:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                case 9:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                                default:
                                    kndDispV2.VerkaufInfo(urunad, adet, satisfiyat, postoplam, toplamtutar, islemtur, verilenPara, paraustu, odemetur);
                                    return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show("DISPLAY INFO Meldung:" + ex.Message);
                        return;
                    }
                }
            }
            if (islemtur != 3)
                knddsply.VerkaufInfo(urunad + "\n" + adet + " x " + satisfiyat, toplamtutar);
            else
                knddsply.VerkaufInfo(toplamtutar, "");
        }

        public void AddtoLogFile(string Message, string WebPage)
        {
            string path = Application.StartupPath.ToString() + ("Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
            if (System.IO.File.Exists(path))
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine("-------------------START-------------" + (object)DateTime.Now);
                    streamWriter.WriteLine("Source :" + Name);
                    streamWriter.WriteLine(Message);
                    streamWriter.WriteLine("-------------------END-------------" + (object)DateTime.Now);
                }
            }
            else
            {
                StreamWriter text = System.IO.File.CreateText(path);
                text.WriteLine("-------------------START-------------" + (object)DateTime.Now);
                text.WriteLine("Source :" + Name);
                text.WriteLine(Message);
                text.WriteLine("-------------------END-------------" + (object)DateTime.Now);
                text.Close();
            }
        }

        /*private void pluWaagePrecess(ISS_Artikel.Artikel arananArtikel)
        {
            btnTik.Enabled = false;
            while (PLUSatilanAdet == 0.0)
            {
                start = true;
                if (gewichtneuladen)
                {
                    btnTik.Enabled = true;
                    gewichtneuladen = false;
                    break;
                }
                if (timeout)
                {
                    if (sp.IsOpen)
                    {
                        sp.WriteLine(4.ToString() + (object)'\x0002' + "81" + (object)'\x0003');
                        lblParaUstu.Text = "WAAGE TIMEOUT!";
                        Console.Beep(100, 100);
                        Console.Beep(200, 100);
                        sp.WriteLine(4.ToString() + (object)'\x0002' + "81" + (object)'\x0003');
                        pWaage.BackColor = Color.Chartreuse;
                        globalTimeOut = true;
                        timeout = false;
                        start = false;
                        btnTik.Enabled = true;
                        return;
                    }
                    pWaage.BackColor = Color.Chartreuse;
                    globalTimeOut = true;
                    timeout = false;
                    start = false;
                    btnTik.Enabled = true;
                    return;
                }
            }
            if (PLUSatilanAdet == 0.0)
            {
                btnTik.Enabled = true;
            }
            else
            {
                if (yeniFis == null)
                {
                    yeniFis = new FisOlustur();
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                }
                Tarih tarih = new Tarih();
                SatisYap satisYap = new SatisYap();
                satisYap.Gruptur = arananArtikel.Gruptur;
                satisYap.Adet = PLUSatilanAdet;
                satisYap.Fisno = yeniFis.SatisAnaId;
                satisYap.KasaNo = Program.kasano;
                satisYap.Mwst = arananArtikel.Mwst;
                satisYap.Ustid_id = arananArtikel.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (arananArtikel.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                satisYap.Birimid = 3.0;
                satisYap.Barkod = arananArtikel.BarkodNo;
                if (arananArtikel.AngebotVarmi != 0)
                {
                    if (arananArtikel.AngebotBaslamaTarihi <= (double)tarih.unixdate(DateTime.Now) && arananArtikel.AngebotBitistarihi >= (double)tarih.unixdate(DateTime.Now))
                    {
                        satisYap.Satisfiyat = arananArtikel.AngebotFiyati;
                        angebotSembol = "";
                    }
                    else
                        satisYap.Satisfiyat = arananArtikel.VkPreis;
                }
                else
                {
                    satisYap.Satisfiyat = arananArtikel.VkPreis;
                    if (new ArtikelGrup(arananArtikel.Grubid).Rabatpunkte != 1)
                    {
                        satisYap.Angebotvarmi = 1;
                        angebotSembol = "*";
                    }
                }
                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                satisYap.Toplamtutar = WaageTutar;
                satisYap.UrunId = (Decimal)arananArtikel.ArtikelId;
                satisYap.UrunAd = angebotSembol + arananArtikel.ArtikelAd;
                satisYap.Birimkar = (satisYap.Satisfiyat - arananArtikel.EkPreis) * PLUSatilanAdet;
                satisYap.Grubid = arananArtikel.Grubid;
                satisYap.Fand = arananArtikel.Fand;
                satisYap.Fand2 = arananArtikel.Fand2;
                //
                if (Program.IsletmeAyarlar["markt"] == "2")
                {

                    F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                    FUserAuswahl.satilanPozition = satisYap;
                    FUserAuswahl.UserList = UserList;
                    FUserAuswahl.ShowDialog();
                    if (FUserAuswahl.SecilenUser != -1)
                    {
                        FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                        toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                        yeniFis = null;
                        if (ElemanFisi == null)
                        {
                            FisOlustur yeniFis1 = new FisOlustur();
                            yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                            yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                            KundenDisplay();
                            satisYap.Fisno = yeniFis.SatisAnaId;
                        }
                        else
                        {
                            FisOlustur yeniFis1 = new FisOlustur();
                            yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                            yeniFis = ElemanFisi;
                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            listView1.Items.Clear();
                            KundenDisplay();
                            int num2 = 1;
                            int count1 = listView1.Items.Count;
                            foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                            {

                                listView1.Items.Add(num2.ToString());
                                listView1.Items[count1].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                listView1.Items[count1].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                ++num2;
                                count1++;
                            }
                            position = num2;
                            yeniFis.FisiKapat();


                        }
                        yeniFis.SatisKalem.Add(satisYap);
                        verkauferList[Program.bedID] = yeniFis;
                    }
                    else
                    {
                        btnTik.Enabled = true;
                        return;
                    }
                }
                else
                {
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    yeniFis.SatisKalem.Add(satisYap);
                }

                //
                int count = listView1.Items.Count;
                listView1.Items.Add(position.ToString());
                listView1.Items[count].SubItems.Add(angebotSembol + arananArtikel.ArtikelAd.ToString() + "(" + PLUSatilanAdet.ToString("#0.000") + "kg x" + satisYap.Satisfiyat.ToString("C") + "/kg)");
                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                if (listView1.Items.Count > 0)
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                txtGiris.Text = "";
                yeniFis.FisiKapat();
                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                ++position;
                sp.Close();
                if (dsp != null)
                {
                    if (Program.displayType != "IBM")
                    {
                        KDbirinciSatiraYaz(satisYap.UrunAd, PLUSatilanAdet.ToString("#0.000") + "kg x" + PLUSatisFiyat.ToString("C"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                    }
                    else
                    {
                        KDbirinciSatiraYaz(satisYap.UrunAd, PLUSatilanAdet.ToString("#0.000") + "kg x" + PLUSatisFiyat.ToString() + (object)'Õ');
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                    }
                }
                else if (Program.displayType == "TVS")
                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                else if (serialPortKD2.IsOpen)
                {
                    KDbirinciSatiraYaz(satisYap.UrunAd, PLUSatilanAdet.ToString("#0.000") + "kg x" + PLUSatisFiyat.ToString("#0.00") + "€/kg");
                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00") + "€");
                }
                PLUSatilanAdet = 0.0;
                PLUSatisFiyat = 0.0;
                SatisFiyat = 0.0;
                start = false;
                timeout = false;
                counter = 0;
                pWaage.BackColor = Color.Chartreuse;
                globalTimeOut = true;
                sp.Close();
                Console.Beep(800, 100);
                Console.Beep(1000, 100);
                foreach (KryptonButton control in (ArrangedElementCollection)flowPanel1.Controls)
                {
                    if (control.TabIndex == 3 || control.TabIndex == 7 || control.TabIndex == 8)
                        control.Enabled = true;
                }
                btnTik.Enabled = true;
            }
        }*/

        private void txtGirisBosalt()
        {
            throw new NotImplementedException();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker3.IsBusy)
                return;
            if (Program.InternetDurum == 1)
            {
                int millisecondsTimeout = 900000; //90000
                while (!backgroundWorker1.CancellationPending)
                {
                    try
                    {
                        AngebotList angebot = new AngebotList(0);
                        angebot.AngebotCheck();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("BacgroundWorker1->AngebotCheck" + ex.Message);
                    }
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    MySqlConnection conn = new db().conn;
                    try
                    {

                        MainRC mainRc = new MainRC();
                        if (Program.issServer.Count > 0)
                        {
                            mainRc.url = Program.issServer[0];
                            mainRc.myConn = conn;
                            mainRc.ip = Program.ServerIp;
                            mainRc.Kod = Program.IsletmeAyarlar["kod"];
                            mainRc.Isletme = Program.IsletmeAyarlar["isletme"];
                            mainRc.Stadt = Program.IsletmeAyarlar["stadt"];
                            mainRc.Tel1 = Program.IsletmeAyarlar["tel1"];
                            mainRc.dbName = Program.dbName;
                            mainRc.IPV4 = Program.IPV4;
                            mainRc.mail = Program.IsletmeAyarlar["mail"];
                            mainRc.Strase = Program.IsletmeAyarlar["strase"];
                            mainRc.plz = Program.IsletmeAyarlar["plz"];
                            mainRc.filhauptid = Program.IsletmeAyarlar["filhauptid"];
                            mainRc.LivePrufung();
                            mainRc.FilAnmelden(Program.IsletmeAyarlar["kod"], Program.IsletmeAyarlar["filname"], "");

                            if (conn.State == ConnectionState.Closed)
                                conn.Open();
                            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM system ", conn);
                            DataTable dataTable = new DataTable();
                            dataTable.Rows.Clear();
                            mySqlDataAdapter.Fill(dataTable);
                            if (Convert.ToDouble(dataTable.Rows[0].ItemArray[4]) < tarih.bugunBaslangic())
                            {
                                if (!backgroundWorker3.IsBusy)
                                    backgroundWorker3.RunWorkerAsync();
                            }
                            else if (Convert.ToInt16(dataTable.Rows[0].ItemArray[7]) != (short)1)
                            {
                                if (!backgroundWorker3.IsBusy)
                                    backgroundWorker3.RunWorkerAsync();
                            }

                            try //Handyauflade Check
                            {
                                if (Program.IsletmeAyarlar["HandyAufladeUsername"] != "" && Program.IsletmeAyarlar["HandyAufladePassword"] != "")
                                {
                                    /* if (Program.IsletmeAyarlar["AufladeFirma"] == "Debeka")
                                     {

                                         iss_HandyAuflade_Manegement.Construktur constr = new iss_HandyAuflade_Manegement.Construktur();
                                         constr.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                                         constr.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                                         //constr.Wurl = Program.IsletmeAyarlar["HandyAufladeAPI"];
                                         constr.myConn = conn;
                                         constr.Update();
                                     }
                                     else
                                     {
                                         iss_HandyAuflade_Manegement_Mopin.Construktur constr = new iss_HandyAuflade_Manegement_Mopin.Construktur();
                                         constr.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                                         constr.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                                         constr.URL = Program.IsletmeAyarlar["HandyAufladeAPI"];
                                         constr.myConn = conn;
                                         constr.Update();
                                     }*/
                                }
                            }
                            catch (Exception ff)
                            {
                            }


                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = "Remote Server wurden nicht identifiziert!";
                            int num = (int)fGenericError.ShowDialog();

                            return;
                        }
                    }
                    catch
                    {
                    }
                    if (Program.ProgramAyarlar["localBackup"] == "1")
                    {
                        if (Program.DailyBackup == false)
                        {
                            Thread makeLocalBackupTread = new Thread(MakeLocalBackUP);
                            makeLocalBackupTread.Start();
                        }

                    }
                    Thread.Sleep(millisecondsTimeout);
                }
                e.Cancel = true;

            }
            else
            {
                if (!CheckInternet())
                    return;
                Program.InternetDurum = 1;
            }
        }

        private void InputControl()
        {
            if (txtGiris.Text.Length > 0)
                btnSil.Enabled = false;
            else
                btnSil.Enabled = true;
        }

        public double adet
        {
            get
            {
                int length = txtGiris.Text.IndexOf('X');
                if (length == -1)
                    return 1.0;
                double result;
                if (double.TryParse(txtGiris.Text.Substring(0, length), out result))
                {
                    if (result.ToString().IndexOf(',') == -1)
                        return result;
                    if (double.TryParse(txtGiris.Text.Substring(length + 1, txtGiris.Text.Length - (length + 1)), out result))
                    {
                        if (result.ToString().IndexOf(',') == -1)
                            return result;
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["23"];
                        int num = (int)fGenericError.ShowDialog();
                        return 0.0;
                    }
                    F_GenericError fGenericError1 = new F_GenericError();
                    fGenericError1.lblMesaj.Text = Program.lang["23"];
                    int num1 = (int)fGenericError1.ShowDialog();
                    return 0.0;
                }
                if (double.TryParse(txtGiris.Text.Substring(length + 1, txtGiris.Text.Length - (length + 1)), out result))
                {
                    result.ToString().IndexOf(',');
                    if (result.ToString().IndexOf(',') == -1)
                        return result;
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["23"];
                    int num = (int)fGenericError.ShowDialog();
                    return 0.0;
                }
                F_GenericError fGenericError2 = new F_GenericError();
                fGenericError2.lblMesaj.Text = Program.lang["23"];
                int num2 = (int)fGenericError2.ShowDialog();
                return 0.0;
            }
        }

        public void VkPreis()
        {
            try
            {
                int length = txtGiris.Text.IndexOf('X');
                if (length == -1)
                {
                    double result;
                    if (double.TryParse(txtGiris.Text, out result))
                    {
                        SatilanAdet = 1.0;
                        if (Program.GlobalAyarlar["KOMMA"] == 0)
                            SatisFiyat = result / 100.0;
                        else
                            SatisFiyat = result;
                    }
                    else
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["24"];
                        int num = (int)fGenericError.ShowDialog();
                        SatilanAdet = 0.0;
                        SatisFiyat = 0.0;
                    }
                }
                else
                {
                    double result1;
                    if (double.TryParse(txtGiris.Text.Substring(0, length), out result1))
                    {
                        SatilanAdet = result1;
                        double result2;
                        if (double.TryParse(txtGiris.Text.Substring(length + 1, txtGiris.Text.Length - (length + 1)), out result2))
                        {
                            if (Program.GlobalAyarlar["KOMMA"] == 0)
                                SatisFiyat = result2 / 100.0;
                            else
                                SatisFiyat = result2;
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["24"];
                            int num = (int)fGenericError.ShowDialog();
                            SatilanAdet = 0.0;
                            SatisFiyat = 0.0;
                        }
                    }
                    else
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["24"];
                        int num = (int)fGenericError.ShowDialog();
                        SatilanAdet = 0.0;
                        SatisFiyat = 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message + ":VKPreis");
            }
        }

        private void btnCarpi_Click(object sender, EventArgs e)
        {
            txtGiris.Text += "X";
        }

        private void btnZws_Click(object sender, EventArgs e)
        {
            if (yeniFis == null)
                return;
            yeniFis.FisiKapat();
            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
        }

        private void btnZws_Leave(object sender, EventArgs e)
        {
            txtGiris.Text = "";
        }

        private void CashDrawerProcess()
        {
            try
            {
                if (Program.cashDrawer == null)
                    return;
                while (!Program.cashDrawer.DrawerOpened)
                    Thread.Sleep(100);
                // ISSUE: reference to a compiler-generated method
                Program.cashDrawer.WaitForDrawerClose(10000, 2000, 100, 1000);
            }
            catch
            {
            }
        }

        private void CashDrawerManualOpen()
        {
            try
            {
                if (Program.cashDrawer == null)
                    return;
                // ISSUE: reference to a compiler-generated method
                Program.cashDrawer.OpenDrawer();
                if (!Program.cashDrawer.DrawerOpened)
                    return;
                // ISSUE: reference to a compiler-generated method
                Program.cashDrawer.WaitForDrawerClose(10000, 2000, 100, 1000);
            }
            catch
            {
            }
        }

        private bool CashDrawerProcessPruf()
        {
            try
            {
                if (!Program.cashDrawer.DrawerOpened)
                    return true;
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = "BITTE SCHLIßEN ZUERST SCHUBLADE/KASSETTE!";
                int num = (int)fGenericError.ShowDialog();
                return false;
            }
            catch (Exception ex)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = "BITTE SCHLIßEN ZUERST SCHUBLADE/KASSETTE!";
                int num = (int)fGenericError.ShowDialog();
                return false;
            }
        }

        private void btnBar_Click(object sender, EventArgs e)
        {
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            BarVerkaufProcess();
        }

        private void BarVerkaufProcess()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            if (yeniFis != null && yeniFis.toplamtutar != 0.0)
            {
                if ((txtGiris.Text != "") && (yeniFis.Musterino == 0 || yeniFis.Musteri.Method == 2)) // ana ekrandan para girildiginde
                {

                    try
                    {
                        if (double.TryParse(txtGiris.Text, out yeniFis.verilenpara))
                        {
                            if (Program.GlobalAyarlar["BKOMMA"] == 0)
                                yeniFis.verilenpara /= 100.0;
                            double num1 = Math.Round(yeniFis.toplamtutar, 2);
                            if (yeniFis.verilenpara < num1)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["64"];
                                int num2 = (int)fGenericError.ShowDialog();
                                txtGiris.Text = "";
                                return;
                            }
                            try
                            {
                                if (Program.cashDrawer == null)
                                {
                                    if (Program.printerType != "star")
                                    {

                                        if (Program.printerSO == "T-3II")
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                        }
                                        else if (Program.printerType != "NCR")
                                        {
                                            if (Program.printerSO != "T-3II")
                                            {
                                                if (Program.printerType != "bixolon")
                                                {
                                                    // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                                }
                                            }
                                            if (Program.printerType == "bixolon")
                                            {
                                                //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)25) + ((char)255));
                                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            }

                                            //((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121)
                                        }
                                        else
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            try
                                            {                                                          // 0x1D 0x28 0x4C 0x04 0x00 0x30 0x42 27,112,0,25,255
                                                //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)0x1D) + ((char)0x28) + ((char)0x4C) + ((char)0x04) + ((char)0x00) + ((char)0x30) + ((char)0x42) + ((char)0x20) + ((char)0x20));
                                            }
                                            catch (Exception ff)
                                            {
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Program.cashDrawer.OpenDrawer();
                                }
                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, " [1802]");
                            }
                            lblParaUstu.Text = (yeniFis.verilenpara - num1).ToString("C");
                            yeniFis.paraustu = yeniFis.verilenpara - num1;
                            yeniFis.ToplamScheck = 0.0;
                            yeniFis.ToplamEc = 0.0;
                            yeniFis.ToplamBar = num1;
                            if (Program.displaySO == "TVS")
                            {
                                /* Control.CheckForIllegalCrossThreadCalls = false;
                                    
                                 dssped = new Thread((ThreadStart)(() => DSPINFO("", "", "", "", "BAR :" + yeniFis.verilenpara.ToString("C") + "   RÜCKGELD :" + yeniFis.paraustu.ToString("C"), 2,yeniFis.verilenpara,yeniFis.paraustu, 1)));
                                 dssped.Start();
                                 Application.DoEvents();
                                 Thread.Sleep(100);*/
                                DSPINFO("", "", "", "", yeniFis.toplamtutar.ToString("C"), 2, yeniFis.verilenpara, yeniFis.paraustu, 1);
                                if (Program.cashdrawerSO != null && Program.cashDrawer != null)
                                {
                                    // ISSUE: reference to a compiler-generated method
                                    Program.cashDrawer.OpenDrawer();
                                }
                            }
                            else if (dsp != null)
                            {
                                if (Program.displayType != "IBM")
                                {
                                    KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString("C"));
                                    KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString("C"));
                                    txtGiris.Text = "";
                                }
                                else
                                {
                                    KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString() + (object)'Õ');
                                    KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString() + (object)'Õ');
                                    txtGiris.Text = "";
                                }
                            }
                            else if (serialPortKD2.IsOpen)
                            {
                                KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString("C"));
                                KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString("C"));
                                txtGiris.Text = "";
                            }
                            txtGiris.Text = "";
                            try
                            {
                                if (yeniFis.PinlistSell.Count > 0)
                                {
                                    if (HandyAufladeSell(yeniFis.PinlistSell) == false)
                                        return;
                                }
                                if (yeniFis.FisiSonlandır(1, yeniFis.SatisKalem))
                                {
                                    Stopwatch StWaBonPosSave = new Stopwatch();
                                    if (Program.StopWatch == 1)
                                    {
                                        StWaBonPosSave.Start();
                                    }
                                    lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                                    if (ReadGeraboKart)
                                        GeraboNeueVerkauf();
                                    foreach (RabattMain rabat in yeniFis.RabatList)
                                    {
                                        if (rabat.Grupid != 999 && rabat.RabatArt != 6)
                                        {
                                            if (rabat.TotalRabattMenge != 0)
                                            {
                                                satisYap = new SatisYap();
                                                satisYap.Adet = 1.0;
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                                satisYap.KasaNo = Program.kasano;
                                                satisYap.Mwst = 0;
                                                satisYap.Gv_typ_id = (int)GVTypEnum.Rabatt;
                                                satisYap.Ustid_id = 0;
                                                satisYap.Satisfiyat = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                                satisYap.Toplamtutar = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                                satisYap.UrunId = new Decimal(0);
                                                satisYap.Birimkar = 0.0;
                                                satisYap.UrunAd = rabat.RabatName;
                                                satisYap.Grubid = rabat.Grupid;
                                                yeniFis.SatisKalem.Add(satisYap);
                                            }
                                        }
                                    }
                                    double num2 = 0.0;
                                    this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                                    foreach (SatisYap satisYap in yeniFis.SatisKalem)
                                    {
                                        satisYap.Fisno = yeniFis.SatisAnaId;
                                        if (satisYap.Barkod == null)
                                            satisYap.Barkod = "0";
                                        num2 += satisYap.Toplamtutar;
                                        satisYap.Kaydet();
                                    }
                                    if (Math.Round(num2, 2) != Math.Round(yeniFis.toplamtutar, 2))
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = "ERROR!\n KONTROLSUMME= " + (object)Math.Round(yeniFis.toplamtutar, 2) + "\n BONPOZSUMME=  " + (object)Math.Round(num2, 2) + "\n BITTE Informieren Sie Ihre Geschäftsleiter!";
                                        int num3 = (int)fGenericError.ShowDialog();
                                        //return;
                                    }
                                    if (Program.StopWatch == 1)
                                    {
                                        StWaBonPosSave.Stop();
                                        TimeSpan etts1 = StWaBonPosSave.Elapsed;

                                        StWaPosSaveMessage = "\nBon Position Save Process Dauer:" + etts1.ToString(@"hh\:mm\:ss\:fff");
                                    }
                                    if (Program.bonDruck)
                                    {
                                        yazilacakBon = yeniFis.ShallowCopy();
                                        yeniFis = null;
                                        listView1.Items.Clear();
                                        txtGiris.Text = "";
                                        txtToplam.Text = "";
                                        ScannerInUse = true;
                                        try
                                        {
                                            Control.CheckForIllegalCrossThreadCalls = false;
                                            Thread thread = new Thread(new ThreadStart(fisyaz));
                                            Program.BonBeleg.OdemeTur = 1;
                                            Program.BonBeleg.basilacakFis = yazilacakBon;
                                            thread.Start();
                                        }
                                        catch (Exception ex)
                                        {
                                            logEntry.AddtoLogFile(ex.Message, " [1895]");
                                            F_GenericError fGenericError = new F_GenericError();
                                            fGenericError.lblMesaj.Text = "ERROR! " + ex.Message;
                                            int num3 = (int)fGenericError.ShowDialog();
                                            txtGiris.Text = "";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, 27.ToString() + (object)'p' + (object)char.MinValue + (object)'2' + (object)'ú');
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    if (verkauferList != null)
                                    {
                                        verkauferList[Program.bedID] = null;
                                    }
                                    ReadGeraboKart = false;
                                    geraboAccuntInfo = (GeraboAccountInfo)null;
                                    listView1.Items.Clear();
                                    txtGiris.Text = "";
                                    txtToplam.Text = "";
                                    ScannerInUse = true;

                                    if (Program.cashDrawer != null)
                                    {
                                        if (Program.cashDrawer.DrawerOpened)
                                        {
                                            // ISSUE: reference to a compiler-generated method
                                            Program.cashDrawer.WaitForDrawerClose(10000, 2000, 100, 1000);

                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, " [1934]");
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = "ERROR! " + ex.Message;
                                int num2 = (int)fGenericError.ShowDialog();
                                txtGiris.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["23"];
                            int num = (int)fGenericError.ShowDialog();

                        }
                    }
                    catch (Exception ex)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = ex.Message;
                        int num = (int)fGenericError.ShowDialog();

                    }

                }

                // Ana Ekran PAra Girme Sonu
                else
                {
                    ScannerInUse = false;
                    try
                    {
                        if (dsp != null)
                        {
                            // ISSUE: reference to a compiler-generated method
                            dsp.ClearText();
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                        }
                    }
                    catch (Exception ex)
                    {
                        logEntry.AddtoLogFile(ex.Message, " [2413]");
                    }
                    using (BarVerkauf barVerkauf = new BarVerkauf())
                    {
                        if (ReadGeraboKart)
                        {
                            barVerkauf.ReadGeraboKart = true;
                            barVerkauf.gerabOInfo = AccountInfo;
                        }
                        txtGiris.Text = "";
                        barVerkauf.toplamtutar = Math.Round(yeniFis.toplamtutar, 2, MidpointRounding.AwayFromZero);
                        barVerkauf.toplammwst = yeniFis.toplammwst;
                        barVerkauf.mwst19miktar = yeniFis.mwst19miktar;
                        barVerkauf.mwst7miktar = yeniFis.mwst7miktar;
                        barVerkauf.musteriNo = yeniFis.Musterino;
                        barVerkauf.angebotsuztoplamtutar = yeniFis.AngebotsuzToplamTutar;
                        barVerkauf.aktuelFis = yeniFis;
                        barVerkauf.BenimEventim += new BarVerkauf.BenimDelegem(TFTYazBarVerkauf);
                        int num1 = (int)barVerkauf.ShowDialog();
                        if (barVerkauf.odemeSonuc)
                        {
                            eBonVerfiedCode = barVerkauf.eBonVerifiedCode;
                            yeniFis.odemesekli = barVerkauf.odemeturu;
                            if (ReadGeraboKart)
                                GeraboNeueVerkauf();
                            try
                            {
                                if (Program.cashDrawer == null)
                                {
                                    if (Program.printerType != "star")
                                    {

                                        if (Program.printerSO == "T-3II")
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                        }
                                        else if (Program.printerType != "NCR")
                                        {
                                            if (Program.printerSO != "T-3II")
                                            {
                                                // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            }
                                            if (Program.printerType == "bixolon")
                                            {
                                                //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)25) + ((char)255));
                                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            }
                                            //((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121)
                                        }
                                        else
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            try
                                            {                                                          // 0x1D 0x28 0x4C 0x04 0x00 0x30 0x42
                                                //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)0x1D) + ((char)0x28) + ((char)0x4C) + ((char)0x04) + ((char)0x00) + ((char)0x30) + ((char)0x42) + ((char)0x20) + ((char)0x20));
                                            }
                                            catch (Exception ff)
                                            {
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Program.cashDrawer.OpenDrawer();
                                }

                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, " [1802]");
                            }
                            try
                            {
                                yeniFis.ToplamScheck = barVerkauf.teilCek;
                                yeniFis.ToplamEc = barVerkauf.teilEC;
                                yeniFis.ToplamBar = barVerkauf.teilBar;
                                if (yeniFis.Musterino == 0L)
                                {
                                    if (barVerkauf.harcananPuanKarsiligiHarcananPara == 0.0)
                                        goto label_83;
                                }
                                yeniFis.KazanilanPuan = barVerkauf.kazanilanPuan;
                                if (barVerkauf.indirimturu == -1)
                                {
                                    yeniFis.EskiPuanToplamı = barVerkauf.eskiPuanToplami;
                                    yeniFis.HarcananPuan = barVerkauf.harcananPuan;
                                    yeniFis.Indirimturu = -1;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 2,
                                        RabatName = "Punkte Einlösung",
                                        RabattTyp = 1,
                                        //TotalRabattMenge = barVerkauf.kazanilanIndirim,
                                        RabatMenge = barVerkauf.kazanilanIndirim,
                                        Grupid = 7
                                    });
                                    yeniFis.FisiKapat();
                                    yeniFis.Musteri.Kredit = barVerkauf.kredit;
                                }
                                /*else if (barVerkauf.indirimturu == -2)
                                {
                                    yeniFis.Indirimturu = -2;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 3,
                                        RabatName = "Kundenrabatt",
                                        RabattTyp = 0,
                                        Grupid = 7,
                                        RabatMenge = barVerkauf.rabatOran,
                                        TotalRabattMenge = barVerkauf.kazanilanIndirim
                                    });
                                    yeniFis.FisiKapat();
                                    yeniFis.Musteri.Kredit = barVerkauf.kredit;
                                }*/
                                else if (barVerkauf.indirimturu == -3)
                                {
                                    yeniFis.Indirimturu = -3;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 4,
                                        RabatName = "Gerabo-KK Rabatt",
                                        RabattTyp = 1,
                                        Grupid = 7,
                                        RabatMenge = barVerkauf.kazanilanIndirim,
                                        TotalRabattMenge = barVerkauf.harcananPuanKarsiligiHarcananPara
                                    });
                                    yeniFis.FisiKapat();
                                }
                            }
                            catch (Exception ex)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = "4394\n" + ex.Message;
                                int num2 = (int)fGenericError.ShowDialog();
                            }
                        label_83:
                            lblParaUstu.Text = barVerkauf.paraustu.ToString("C");
                            yeniFis.verilenpara = barVerkauf.verilenpara;
                            yeniFis.paraustu = barVerkauf.paraustu;
                            if (Program.displaySO == "TVS")
                            {
                                DSPINFO("", "", "", "", yeniFis.toplamtutar.ToString("C"), 2, yeniFis.verilenpara, yeniFis.paraustu, 1);
                                /*Control.CheckForIllegalCrossThreadCalls = false;
                                dssped = new Thread((ThreadStart)(() => DSPINFO("", "", "", "", yeniFis.toplamtutar.ToString("C"), 2, yeniFis.verilenpara,yeniFis.paraustu,1)));
                                dssped.Start();
                                Application.DoEvents();*/
                                if (Program.cashDrawer != null)
                                {
                                    // ISSUE: reference to a compiler-generated method
                                    Program.cashDrawer.OpenDrawer();
                                }
                            }
                            else if (dsp != null)
                            {
                                if (Program.displayType != "IBM")
                                {
                                    KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString("C"));
                                    KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString("C"));
                                    txtGiris.Text = "";
                                }
                                else
                                {
                                    KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString() + (object)'Õ');
                                    KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString() + (object)'Õ');
                                    txtGiris.Text = "";
                                }
                            }
                            else if (serialPortKD2.IsOpen)
                            {
                                KDbirinciSatiraYaz("BAR :", yeniFis.verilenpara.ToString("C"));
                                KDikinciSatiraYaz("RUECKGELD : " + yeniFis.paraustu.ToString("C"));
                                txtGiris.Text = "";
                            }
                            try
                            {
                                if (yeniFis.PinlistSell.Count > 0)
                                {
                                    if (HandyAufladeSell(yeniFis.PinlistSell) == false)
                                        return;
                                }
                                if (yeniFis.FisiSonlandır(yeniFis.odemesekli, yeniFis.SatisKalem))
                                {
                                    lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                                    foreach (RabattMain rabat in yeniFis.RabatList)
                                    {
                                        if (rabat.Grupid != 999 && rabat.RabatArt != 6)
                                        {
                                            satisYap = new SatisYap();
                                            satisYap.Adet = 1.0;
                                            satisYap.Fisno = yeniFis.SatisAnaId;
                                            satisYap.KasaNo = Program.kasano;
                                            satisYap.Mwst = 0;
                                            satisYap.Gv_typ_id = (int)GVTypEnum.Rabatt;
                                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                            satisYap.Satisfiyat = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                            satisYap.Toplamtutar = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                            satisYap.UrunId = new Decimal(0);
                                            satisYap.Birimkar = 0.0;
                                            satisYap.UrunAd = rabat.RabatName;
                                            satisYap.Grubid = rabat.Grupid;
                                            satisYap.Gv_typ_id = (int)GVTypEnum.Rabatt;
                                            yeniFis.SatisKalem.Add(satisYap);
                                        }
                                    }
                                    double num2 = 0.0;
                                    foreach (SatisYap satisYap in yeniFis.SatisKalem)
                                    {
                                        try
                                        {
                                            if (satisYap.Barkod == null)
                                                satisYap.Barkod = "0";
                                            satisYap.Fisno = yeniFis.SatisAnaId;
                                            num2 += satisYap.Toplamtutar;
                                            satisYap.Kaydet();
                                        }
                                        catch (Exception ex)
                                        {
                                            logEntry.AddtoLogFile(satisYap.ToString(), "3117");
                                        }
                                    }
                                    if (Math.Round(num2, 2, MidpointRounding.AwayFromZero) != Math.Round(yeniFis.toplamtutar, 2, MidpointRounding.AwayFromZero))
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = "ERROR!\n KONTROLSUMME= " + (object)Math.Round(yeniFis.toplamtutar, 2) + "\n BONPOZSUMME=  " + (object)Math.Round(num2, 2) + "\n BITTE Informierene Ihre Geschäftsleiter!";
                                        int num3 = (int)fGenericError.ShowDialog();
                                    }
                                    if (Program.bonDruck)
                                    {
                                        yazilacakBon = yeniFis.ShallowCopy();
                                        yeniFis = null;
                                        listView1.Items.Clear();
                                        txtGiris.Text = "";
                                        txtToplam.Text = "";
                                        ScannerInUse = true;
                                        try
                                        {
                                            Control.CheckForIllegalCrossThreadCalls = false;
                                            Thread thread = new Thread(new ThreadStart(fisyaz));
                                            Program.BonBeleg.OdemeTur = yazilacakBon.odemesekli;
                                            Program.BonBeleg.basilacakFis = yazilacakBon;
                                            thread.Start();
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Program.printer.PrintNormal(PrinterStation.Receipt, 27.ToString() + (object)'p' + (object)char.MinValue + (object)'2' + (object)'ú');
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    if (yeniFis.odemesekli == 0)
                                        lblParaUstu.Text = "";
                                    yeniFis = null;
                                    if (Program.IsletmeAyarlar["markt"] == "2")
                                    {
                                        if (verkauferList != null)
                                        {
                                            verkauferList[Program.bedID] = null;
                                        }
                                    }


                                    //satisYap = null;
                                    ReadGeraboKart = false;
                                    geraboAccuntInfo = (GeraboAccountInfo)null;
                                    if (Program.cashDrawer != null && Program.cashDrawer.DrawerOpened)
                                    {
                                        // ISSUE: reference to a compiler-generated method
                                        Program.cashDrawer.WaitForDrawerClose(10000, 2000, 100, 1000);
                                    }
                                    listView1.Items.Clear();
                                    txtToplam.Text = "";
                                }
                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, " [2143]");
                            }
                        }
                        else if (!serialPortKD2.IsOpen)
                        {
                            try
                            {
                                serialPortKD2.Open();
                            }
                            catch
                            {
                            }
                        }
                    }
                    ScannerInUse = true;
                }

            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["25"];
                int num = (int)fGenericError.ShowDialog();
                ScannerInUse = true;
            }
        }
        private string TransID()
        {
            //transid=unixtimestamp+kod(4 stell)+kasaNo(1 stell)
            string Unixdate = tarih.unixdate(DateTime.Now).ToString();
            string Kod = Program.IsletmeAyarlar["kod"];
            string Kasano = Program.kasano.ToString();
            while (Kod.Length < 4)
            {
                Kod = "0" + Kod;
            }
            return Unixdate + Kod + Kasano;
        }
        private bool HandyAufladeSell(List<SellPinList> list)
        {
            //LoginInfo Loginresult = new LoginInfo();
            if (Program.IsletmeAyarlar["AufladeFirma"] == "Debeka")
            {
                string Loginresult = "";
                iss_HandyAuflade.iss_HandyAuflade_Main Handy = new iss_HandyAuflade.iss_HandyAuflade_Main();
                Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                //Handy.url = Program.IsletmeAyarlar["HandyAufladeAPI"];
                Loginresult = Handy.Login();
                if (Loginresult == "OK")
                {
                    foreach (SellPinList VerkaufteCardID in list)
                    {

                        Result HandyVerkaufResult = new Result();
                        HandyVerkaufResult = Handy.RequestCardPin(VerkaufteCardID.cardid, tarih.unixdate(DateTime.Now).ToString());

                        if (HandyVerkaufResult.error.errorCode == "0")
                        {
                            if (HandyVerkaufResult.data.pinslist.Count > 0)
                            {
                                MySqlConnection conn = new MySqlConnection();
                                db baglanti = new db();
                                conn = baglanti.myconn();
                                if (conn.State == ConnectionState.Closed)
                                {
                                    conn.Open();
                                }
                                string SQL = "";
                                //INSERT INTO `handyaufladelog`(`id`, `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES 
                                //([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13],[value-14])


                                foreach (PinsList row in HandyVerkaufResult.data.pinslist)
                                {
                                    SQL = "INSERT INTO `handyaufladelog`( `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES" +
                                        "(" + tarih.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + row.batchnumber + "','" + HandyVerkaufResult.data.pinslist[0].cardid + "','" + "','" + row.pinnumber + "','" + HandyVerkaufResult.data.pinslist[0].transactionid + "','" + HandyVerkaufResult.data.cardinfo.cardname + "','" + HandyVerkaufResult.data.cardinfo.instruction + "'," +
                                         HandyVerkaufResult.data.cardinfo.rate.ToString().Replace(',', '.') + "," + Program.bedID + ",'" + HandyVerkaufResult.error.errorCode + "','" + HandyVerkaufResult.error.errorString + "')";
                                    MySqlCommand cmdInsert = new MySqlCommand(SQL, conn);
                                    if (cmdInsert.ExecuteNonQuery() > 0)
                                    {
                                        //VerkauftePin.batchnumber = Verkauf.Pinliste[0].batchnumber;
                                        //      VerkauftePin.cardid = artikelid.ToString();
                                        // VerkauftePin.expirydate = Verkauf.Pinliste[0].cardid;
                                        // VerkauftePin.pinnumber = Verkauf.Pinliste[0].cardid;
                                        //VerkauftePin.transactionid = Verkauf.Pinliste[0].cardid;
                                        //VerkauftePin.CardName = Verkauf.CardInfo.cardname;
                                        // VerkauftePin.Insroduction = Verkauf.CardInfo.instruction;
                                        SoldPinlist soldPin = new SoldPinlist();
                                        soldPin.batchnumber = row.batchnumber;
                                        soldPin.CardName = HandyVerkaufResult.data.cardinfo.cardname;
                                        soldPin.expirydate = row.expirydate;
                                        soldPin.Instruction = HandyVerkaufResult.data.cardinfo.instruction;
                                        soldPin.pinnumber = row.pinnumber;
                                        soldPin.purchaseprice = HandyVerkaufResult.data.cardinfo.rate.ToString().Replace(',', '.');
                                        soldPin.transactionid = HandyVerkaufResult.data.pinslist[0].transactionid;
                                        soldPin.rate = HandyVerkaufResult.data.cardinfo.rate.ToString().Replace(',', '.');
                                        soldPin.cardid = HandyVerkaufResult.data.pinslist[0].cardid;
                                        yeniFis.PinlistSold.Add(soldPin);

                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }

                                return true;

                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Fehler beim Verkauf Handyauflade-Karte: " + HandyVerkaufResult.error.errorString + "\nEntweder nochmal versuchen oder entfernen diese Produkt(e) von der Verkauflist!!";
                            frmerror.ShowDialog();
                            return false;
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // MOPIN
            {
                iss_HandyMopin.LoginInfo Loginresult;
                iss_HandyMopin.iss_HandyAuflade_Main_Mopin Handy = new iss_HandyMopin.iss_HandyAuflade_Main_Mopin();
                Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                Handy.url = Program.IsletmeAyarlar["HandyAufladeAPI"];
                Loginresult = Handy.Login();
                if (Loginresult.token != "")
                {
                    foreach (SellPinList VerkaufteCardID in list)
                    {

                        iss_HandyMopin.transaction HandyVerkaufResult = new iss_HandyMopin.transaction();
                        HandyVerkaufResult = Handy.RequestCardPin(VerkaufteCardID.cardid, 1, Loginresult.token);

                        if (HandyVerkaufResult.message_type == 1)
                        {
                            if (HandyVerkaufResult.pins_list.Count > 0)
                            {
                                MySqlConnection conn = new MySqlConnection();
                                db baglanti = new db();
                                conn = baglanti.myconn();
                                if (conn.State == ConnectionState.Closed)
                                {
                                    conn.Open();
                                }
                                string SQL = "";
                                //INSERT INTO `handyaufladelog`(`id`, `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES 
                                //([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13],[value-14])


                                foreach (iss_HandyMopin.Pins_List row in HandyVerkaufResult.pins_list)
                                {
                                    SQL = "INSERT INTO `handyaufladelog`( `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`, `errorcode`, `errormeldung`) VALUES" +
                                        "(" + tarih.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + row.batch + "','" + HandyVerkaufResult.product_id + "','" + "','" + row.pin + "','" + HandyVerkaufResult.transaction_id + "','" + HandyVerkaufResult.product + "','" + HandyVerkaufResult.product_instructions + "'," +
                                         HandyVerkaufResult.total_amount.ToString().Replace(',', '.') + "," + Program.bedID + ",'" + HandyVerkaufResult.message + "','" + HandyVerkaufResult.message_type + "')";
                                    MySqlCommand cmdInsert = new MySqlCommand(SQL, conn);
                                    if (cmdInsert.ExecuteNonQuery() > 0)
                                    {
                                        //VerkauftePin.batchnumber = Verkauf.Pinliste[0].batchnumber;
                                        //      VerkauftePin.cardid = artikelid.ToString();
                                        // VerkauftePin.expirydate = Verkauf.Pinliste[0].cardid;
                                        // VerkauftePin.pinnumber = Verkauf.Pinliste[0].cardid;
                                        //VerkauftePin.transactionid = Verkauf.Pinliste[0].cardid;
                                        //VerkauftePin.CardName = Verkauf.CardInfo.cardname;
                                        // VerkauftePin.Insroduction = Verkauf.CardInfo.instruction;
                                        SoldPinlist soldPin = new SoldPinlist();
                                        soldPin.batchnumber = row.batch;
                                        soldPin.CardName = HandyVerkaufResult.product;
                                        //soldPin.expirydate = HandyVerkaufResult..expirydate;
                                        soldPin.Instruction = HandyVerkaufResult.product_instructions;
                                        soldPin.pinnumber = row.pin;
                                        soldPin.purchaseprice = HandyVerkaufResult.total_amount.ToString().Replace(',', '.');
                                        soldPin.transactionid = HandyVerkaufResult.transaction_id.ToString();
                                        //soldPin.rate = HandyVerkaufResult.data.cardinfo.rate.ToString().Replace(',', '.');
                                        soldPin.cardid = HandyVerkaufResult.product_id.ToString();
                                        //soldPin.customercare=HandyVerkaufResult.cus
                                        yeniFis.PinlistSold.Add(soldPin);

                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }

                                return true;

                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Fehler beim Verkauf Handyauflade-Karte: " + HandyVerkaufResult.message + "\nEntweder nochmal versuchen oder entfernen diese Produkt(e) von der Verkauflist!!";
                            frmerror.ShowDialog();
                            return false;
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private void btnEC_Click(object sender, EventArgs e)
        {
            try
            {
                ScannerInUse = false;
                if (yeniFis != null && yeniFis.toplamtutar != 0.0)
                {
                    using (F_EConay fEconay = new F_EConay())
                    {
                        yeniFis.FisiKapat();
                        fEconay.toptutar = Math.Round(yeniFis.toplamtutar, 2);
                        fEconay.aktuelFis = yeniFis;
                        fEconay.Rea = Rea;
                        int num1 = (int)fEconay.ShowDialog();
                        if (fEconay.odemesonuc)
                        {
                            yeniFis.verilenpara = 0.0;
                            yeniFis.paraustu = 0.0;
                            yeniFis.ToplamScheck = 0.0;
                            yeniFis.ToplamEc = yeniFis.toplamtutar;
                            yeniFis.ToplamBar = 0.0;
                            yeniFis.odemesekli = 0;
                            yeniFis.CardTypID = fEconay.cardTyp;
                            yeniFis.CardPaymentTypID = fEconay.cardpaymentTyp;

                            try
                            {
                                yeniFis.ToplamScheck = 0.0;
                                yeniFis.ToplamEc = fEconay.aktuelFis.toplamtutar;
                                yeniFis.ToplamBar = 0.0;
                                knddsply.VerkaufInfo("EC-KARTE :\n" + yeniFis.verilenpara.ToString("C"), "");
                                DSPINFO("", "", "", "", yeniFis.toplamtutar.ToString("C"), 2, yeniFis.verilenpara, 0, 0);
                                if (yeniFis.Musteri != null && yeniFis.Musteri.Method == 1)
                                    getKundeProcess();
                                if (dsp != null)
                                {
                                    if (Program.displayType != "IBM")
                                    {
                                        KDbirinciSatiraYaz("TOTAL : ", yeniFis.toplamtutar.ToString("C"));
                                        KDikinciSatiraYaz("ECKARTE");
                                    }
                                    else
                                    {
                                        KDbirinciSatiraYaz("TOTAL : ", yeniFis.toplamtutar.ToString() + (object)'Õ');
                                        KDikinciSatiraYaz("ECKARTE");
                                    }
                                }
                                else if (!(Program.displayType == "TVS"))
                                {
                                    if (!serialPortKD2.IsOpen)
                                    {
                                        try
                                        {
                                            serialPortKD2.Open();
                                            KDbirinciSatiraYaz("TOTAL : ", yeniFis.toplamtutar.ToString("C"));
                                            KDikinciSatiraYaz("ECKARTE");
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        KDbirinciSatiraYaz("TOTAL : ", yeniFis.toplamtutar.ToString("C"));
                                        KDikinciSatiraYaz("ECKARTE");
                                    }
                                }
                                if (yeniFis.Musterino == 0L)
                                {
                                    if (fEconay.harcananPuanKarsiligiHarcananPara == 0.0)
                                        goto label_24;
                                }
                                if (fEconay.indirimturu == -1)
                                {
                                    yeniFis.EskiPuanToplamı = fEconay.eskiPuanToplami;
                                    yeniFis.HarcananPuan = fEconay.harcananPuan;
                                    yeniFis.Indirimturu = -1;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 2,
                                        RabatName = "Punkte Einlösung",
                                        RabattTyp = 1,
                                        TotalRabattMenge = fEconay.kazanilanIndirim,
                                        RabatMenge = fEconay.kazanilanIndirim,
                                        Grupid = 7
                                    });
                                    yeniFis.FisiKapat();
                                    yeniFis.Musteri.Kredit = fEconay.kredit;
                                }
                                else if (fEconay.indirimturu == -2)
                                {
                                    yeniFis.Indirimturu = -2;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 3,
                                        RabatName = "Kundenrabatt",
                                        RabattTyp = 0,
                                        Grupid = 7,
                                        RabatMenge = fEconay.rabatOran,
                                        TotalRabattMenge = fEconay.kazanilanIndirim
                                    });
                                    yeniFis.FisiKapat();
                                }
                                else if (fEconay.indirimturu == -3)
                                {
                                    yeniFis.Indirimturu = -3;
                                    yeniFis.RabatList.Add(new RabattMain()
                                    {
                                        RabattAlani = 0,
                                        RabatArt = 4,
                                        RabatName = "Gerabo-KK Rabatt",
                                        RabattTyp = 1,
                                        Grupid = 7,
                                        RabatMenge = fEconay.kazanilanIndirim,
                                        TotalRabattMenge = fEconay.harcananPuanKarsiligiHarcananPara
                                    });
                                    yeniFis.FisiKapat();
                                }
                            }
                            catch (Exception ex)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = ex.Message;
                                int num2 = (int)fGenericError.ShowDialog();
                            }
                        label_24:
                            if (yeniFis.PinlistSell.Count > 0)
                            {
                                if (HandyAufladeSell(yeniFis.PinlistSell) == false)
                                    return;
                            }
                            if (yeniFis.FisiSonlandır(yeniFis.odemesekli, yeniFis.SatisKalem))
                            {
                                lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                                if (ReadGeraboKart)
                                    GeraboNeueVerkauf();
                                foreach (RabattMain rabat in yeniFis.RabatList)
                                {
                                    if (rabat.Grupid != 999 && rabat.RabatArt != 6)
                                    {
                                        satisYap = new SatisYap();
                                        satisYap.Adet = 1.0;
                                        satisYap.Fisno = yeniFis.SatisAnaId;
                                        satisYap.KasaNo = Program.kasano;
                                        satisYap.Mwst = 0;
                                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                        satisYap.Satisfiyat = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                        satisYap.Toplamtutar = Math.Round(rabat.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                        satisYap.UrunId = new Decimal(0);
                                        satisYap.Birimkar = 0.0;
                                        satisYap.UrunAd = rabat.RabatName;
                                        satisYap.Grubid = rabat.Grupid;
                                        yeniFis.SatisKalem.Add(satisYap);
                                    }
                                }
                                foreach (SatisYap satisYap in yeniFis.SatisKalem)
                                {
                                    if (satisYap.Barkod == null)
                                        satisYap.Barkod = "0";
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    satisYap.Kaydet();
                                }
                            }
                            int num3 = Program.bonDruck ? 1 : 0;
                            try
                            {
                                Control.CheckForIllegalCrossThreadCalls = false;
                                Thread thread = new Thread(new ThreadStart(fisyaz));
                                Program.BonBeleg.OdemeTur = 0;
                                Program.BonBeleg.basilacakFis = yeniFis;
                                thread.Start();
                                try
                                {
                                    if (Program.cashDrawer == null)
                                    {
                                        if (Program.printerType != "star")
                                        {

                                            if (Program.printerSO == "T-3II")
                                            {
                                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                            }
                                            else if (Program.printerType != "NCR")
                                            {
                                                if (Program.printerSO != "T-3II")
                                                {
                                                    if (Program.printerType != "bixolon")
                                                    {
                                                        // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                                    }
                                                }
                                                if (Program.printerType == "bixolon")
                                                {
                                                    //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)25) + ((char)255));
                                                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                                }
                                                //((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121)
                                            }
                                            else
                                            {
                                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                                try
                                                {                                                          // 0x1D 0x28 0x4C 0x04 0x00 0x30 0x42
                                                    //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)0x1D) + ((char)0x28) + ((char)0x4C) + ((char)0x04) + ((char)0x00) + ((char)0x30) + ((char)0x42) + ((char)0x20) + ((char)0x20));
                                                }
                                                catch (Exception ff)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Program.cashDrawer.OpenDrawer();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logEntry.AddtoLogFile(ex.Message, " [1802]");
                                }
                            }
                            catch
                            {
                            }
                            yeniFis = null;
                            if (Program.IsletmeAyarlar["markt"] == "2")
                            {
                                if (verkauferList != null)
                                {
                                    verkauferList[Program.bedID] = null;
                                }
                            }
                            ReadGeraboKart = false;
                            geraboAccuntInfo = (GeraboAccountInfo)null;
                            lblParaUstu.Text = "";
                            listView1.Items.Clear();
                            txtGiris.Text = "";
                            txtToplam.Text = "";
                        }
                    }
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["25"];
                    int num = (int)fGenericError.ShowDialog();
                }
                ScannerInUse = true;
            }
            catch (Exception ff)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = ff.Message + "\n" + ff.StackTrace;
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void MusteriIndirimiUygula(double rabatOran)
        {
            yeniFis.Indirimturu = -2;
            yeniFis.RabatList.Add(new RabattMain()
            {
                RabattAlani = 0,
                RabatArt = 3,
                RabatName = "Kundenrabatt",
                RabattTyp = 0,
                Grupid = 7,
                RabatMenge = rabatOran
            });
            yeniFis.FisiKapat();
        }

        private void getKundeProcess()
        {
            double num1 = 0.0;
            if (yeniFis.Musteri.Method == 2)
            {
                if (yeniFis.Musteri.OzelOran == 0.0)
                    MusteriIndirimiUygula(Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]));
                else
                    MusteriIndirimiUygula(yeniFis.Musteri.OzelOran);
            }
            else if (yeniFis.Musteri.Method == 1)
            {
                if (yeniFis.AngebotsuzToplamTutar <= 0.0)
                    return;
                yeniFis.KazanilanPuan = Math.Round(yeniFis.AngebotsuzToplamTutar * Convert.ToDouble(Program.IsletmeAyarlar["kartstandartpuan"]));
                yeniFis.Indirimturu = -1;
                yeniFis.EskiPuanToplamı = yeniFis.Musteri.KullanilabilirPuan;
            }
            else
            {
                if (yeniFis.Musteri.Method != 3 || yeniFis.toplamtutar <= 0.0)
                    return;
                using (F_PunkteOderRabat fPunkteOderRabat = new F_PunkteOderRabat())
                {
                    int num2 = (int)fPunkteOderRabat.ShowDialog();
                    if (fPunkteOderRabat.sonuc == 2)
                    {
                        num1 = yeniFis.Musteri.OzelOran != 0.0 ? yeniFis.Musteri.OzelOran : Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                        yeniFis.Indirimturu = -2;
                    }
                    else
                    {
                        if (fPunkteOderRabat.sonuc != 1 || yeniFis.AngebotsuzToplamTutar <= 0.0)
                            return;
                        yeniFis.KazanilanPuan = Math.Round(yeniFis.AngebotsuzToplamTutar * Convert.ToDouble(Program.IsletmeAyarlar["kartstandartpuan"]));
                        yeniFis.Indirimturu = -1;
                        yeniFis.EskiPuanToplamı = yeniFis.Musteri.KullanilabilirPuan;
                    }
                }
            }
        }
        private string getBarkodKunden()
        {
            Ean13 barcode = new Ean13();
            barcode.CountryCode = "24" + Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + "03";
            barcode.ManufacturerCode = "";
            barcode.ProductCode = getMaxIDKunden(barcode.CountryCode.Length);
            return barcode.ToString();
        }

        private string getMaxIDKunden(int length)
        {

            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
            string maxIdSQL = "SELECT max(kundenid) from kunden ";
            MySqlDataAdapter daMaxID = new MySqlDataAdapter(maxIdSQL, myConn);
            DataTable dtMaxID = new DataTable();
            dtMaxID.Rows.Clear();
            daMaxID.Fill(dtMaxID);
            string maxID = "0";
            if (dtMaxID.Rows[0].ItemArray[0] is DBNull)
                maxID = "1";
            else
            {
                maxID = (Convert.ToInt32(dtMaxID.Rows[0].ItemArray[0]) + 1).ToString();
            }
            //int uz = maxID.Length;
            while (maxID.Length + length < 12)
            {
                maxID = "0" + maxID;
            }
            return maxID;


        }
        public void fisyaz()
        {
            Stopwatch StWaBonDruck = new Stopwatch();
            if (Program.StopWatch == 1)
            {
                StWaBonDruck.Start();
            }
            int error = 0;
        b:
            if (Program.ebon != "" && Program.ebon != null && Program.ebon != "0" && error < 2)
            {
                if (yeniFis == null)
                {
                    yeniFis = Program.BonBeleg.basilacakFis;
                }
            a:
                //
                this.Enabled = false;
                string code = "";
                if (eBonVerfiedCode == "")
                {
                    F_ebonCodeEingabe fEbonCodeEingabe = new F_ebonCodeEingabe();
                    int num1 = (int)fEbonCodeEingabe.ShowDialog();
                    code = fEbonCodeEingabe.code;
                }
                else
                {
                    code = eBonVerfiedCode;
                }
                string slipText = "";
                double num2 = 0.0;
                if (code != "")
                {
                    ebon_Main ebonMain = new ebon_Main();
                    //loyalty card check
                    VerifiedCodeRes res = new VerifiedCodeRes();
                    res = ebonMain.VerifiedCode(code, Program.IsletmeAyarlar["companyID"], Program.IsletmeAyarlar["eBonBearer"]);
                    if (res != null)
                    {
                        if (res.result == true)
                        {
                            long musteriNo = 0;
                            if (Program.eBonCompanyStatus.permission.cp_can_loyalty == true)
                            {
                                if (res.data.cards.Count > 0)
                                {

                                    if (Program.BonBeleg.basilacakFis == null)
                                    {
                                        /* yeniFis = new FisOlustur();
                                         yeniFis.FisYarat(0);
                                         position = 1;
                                         listView1.Items.Clear();*/
                                        return;
                                    }
                                    musteriNo = getMusteriNo(res.data.cards[0].cardNumber);
                                    if (musteriNo != -1L)
                                    {
                                        getKundeProcess();
                                        Program.BonBeleg.basilacakFis.Musterino = musteriNo;
                                        DSPINFO("", "", "", "", "ebon Kundencard :" + res.data.cards[0].cardNumber, 0, 0, 0, 0);
                                        if (Program.BonBeleg.basilacakFis.EskiPuanToplamı != res.data.cards[0].balance)
                                        {
                                            Program.BonBeleg.basilacakFis.EskiPuanToplamı = res.data.cards[0].balance;
                                        }
                                        gelenBarkod = "";

                                    }
                                    else
                                    {
                                        iss_Kunden.Musteri musteri = new iss_Kunden.Musteri();
                                        musteri.Abholmu = 0;
                                        musteri.Aciklama = "e-Bonn Loyalty App Auto Generation!-" + res.data.userId;
                                        musteri.Activ = 1;
                                        musteri.AdSoyad = "";
                                        musteri.Anrede = 0;
                                        musteri.Barcodepath = "";
                                        musteri.Barkod = res.data.cards[0].cardNumber;
                                        musteri.Baslangicpuani = 0;
                                        musteri.eBonCustomer = true;
                                        musteri.Fax = "";
                                        musteri.Firmaadi = "";
                                        musteri.Gsm = "";
                                        musteri.Harcananpuan = 0;
                                        musteri.Harcanantutar = 0;
                                        musteri.Inhaber = "";
                                        musteri.Kontotyp = 0;
                                        musteri.Kredit = 0;
                                        musteri.KullanilabilirKredi = 0;
                                        musteri.KullanilabilirPuan = res.data.cards[0].balance;
                                        musteri.KundenSoyad = "";
                                        musteri.Land = "";
                                        musteri.Letzteeinkauf = 0;
                                        musteri.Mail = "";
                                        musteri.Method = 1;
                                        musteri.MuseteriAsilAdresId = 0;
                                        musteri.Muskod = res.data.cards[0].cardNumber;
                                        musteri.MusteriAd = "";
                                        musteri.MusteriBindOran = 0;
                                        musteri.MusteriGrup = 0;
                                        musteri.MusteriId = 0;
                                        musteri.MusteriIndOran = 0;
                                        musteri.MusteriKod = res.data.cards[0].cardNumber;
                                        musteri.MusteriOrtVade = 0;
                                        musteri.Nachname = "";
                                        musteri.Ozeloran = 0;
                                        musteri.OzelOran = 0;
                                        musteri.Plz = Program.IsletmeAyarlar["plz"];
                                        musteri.Print = 0;
                                        musteri.Resimpath = "";
                                        musteri.Sorumlu = "";
                                        musteri.Stad = Program.IsletmeAyarlar["stadt"];
                                        musteri.Stnr = "";

                                        //musteri. = (new db()).myconn();
                                        if (musteri.Kaydet() == true)
                                        {
                                            musteriNo = getMusteriNo(res.data.cards[0].cardNumber);
                                        }

                                    }
                                }
                                else
                                {
                                    iss_Kunden.Musteri musteri = new iss_Kunden.Musteri();
                                    string KundenBarcode = getBarkodKunden();
                                    musteri.Abholmu = 0;
                                    musteri.Aciklama = "e-Bonn Loyalty App Auto Generation!-" + res.data.userId;
                                    musteri.Activ = 1;
                                    musteri.AdSoyad = KundenBarcode;
                                    musteri.Anrede = 0;
                                    musteri.Barcodepath = "";
                                    musteri.Barkod = KundenBarcode;
                                    musteri.Baslangicpuani = 0;
                                    musteri.eBonCustomer = true;
                                    musteri.Fax = "";
                                    musteri.Firmaadi = "";
                                    musteri.Gsm = "";
                                    musteri.Harcananpuan = 0;
                                    musteri.Harcanantutar = 0;
                                    musteri.Inhaber = "";
                                    musteri.Kontotyp = 0;
                                    musteri.Kredit = 0;
                                    musteri.KullanilabilirKredi = 0;
                                    musteri.KullanilabilirPuan = 0;
                                    musteri.KundenSoyad = "";
                                    musteri.Land = "";
                                    musteri.Letzteeinkauf = 0;
                                    musteri.Mail = "";
                                    musteri.Method = 1;
                                    musteri.MuseteriAsilAdresId = 0;
                                    musteri.Muskod = KundenBarcode;
                                    musteri.MusteriAd = KundenBarcode;
                                    musteri.MusteriBindOran = 0;
                                    musteri.MusteriGrup = 0;
                                    musteri.MusteriId = 0;
                                    musteri.MusteriIndOran = 0;
                                    musteri.MusteriKod = KundenBarcode;
                                    musteri.MusteriOrtVade = 0;
                                    musteri.Nachname = "";
                                    musteri.Ozeloran = 0;
                                    musteri.OzelOran = 0;
                                    musteri.Plz = Program.IsletmeAyarlar["plz"];
                                    musteri.Print = 0;
                                    musteri.Resimpath = "";
                                    musteri.Sorumlu = "";
                                    musteri.Stad = Program.IsletmeAyarlar["stadt"];
                                    musteri.Stnr = "";
                                    // musteri.db = (new db()).myconn();
                                    if (musteri.Kaydet() == true)
                                    {
                                        getMusteriNo(KundenBarcode);
                                        getKundeProcess();
                                        Program.BonBeleg.basilacakFis.Musterino = musteriNo;
                                        DSPINFO("", "", "", "", "ebon Kundencard :" + KundenBarcode, 0, 0, 0, 0);
                                        gelenBarkod = "";
                                        ebonMain.loyaltyCardCreate(res.data.userId, Program.IsletmeAyarlar["eBonBearer"], 0, musteri.Barkod);
                                    }

                                }
                            }
                        }
                        else
                        {
                            error++;
                            goto b;

                        }

                    }
                    else
                    {
                        error++;
                        goto a;
                    }
                    Tarih tarih = new Tarih();
                    DateTime dateTime = Convert.ToDateTime(Program.BonBeleg.basilacakFis.tarih == 0.0 ? DateTime.Now : tarih.KisatarihDateTime((long)Convert.ToInt32(Program.BonBeleg.basilacakFis.tarih)));

                    Info firmenInfo = new Info();
                    firmenInfo.cassierName = Program.bedAdSoyad;
                    firmenInfo.cassierNo = Program.bedID.ToString();
                    firmenInfo.city = Program.IsletmeAyarlar["stadt"];
                    firmenInfo.companyName = Program.IsletmeAyarlar["isletme"];
                    firmenInfo.companyNo = Program.IsletmeAyarlar["kod"];
                    firmenInfo.email = Program.IsletmeAyarlar["mail"];
                    firmenInfo.faxPhone = Program.IsletmeAyarlar["fax"];
                    firmenInfo.freeAddress = Program.IsletmeAyarlar["strase"] + " " + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"];
                    firmenInfo.phone = Program.IsletmeAyarlar["tel1"];
                    firmenInfo.posNo = Program.kasano.ToString();
                    firmenInfo.posName = "KASSE " + (object)Program.kasano;
                    firmenInfo.postalCode = Program.IsletmeAyarlar["plz"];
                    firmenInfo.receiptNo = Program.BonBeleg.basilacakFis.SatisAnaId.ToString();
                    firmenInfo.receiptTime = dateTime.ToString("HH:mm");
                    firmenInfo.recieptDate = dateTime.ToString("dd.MMMM.yyyy");
                    firmenInfo.street = Program.IsletmeAyarlar["strase"];
                    firmenInfo.taxNumber = Program.IsletmeAyarlar["usid"] != "" ? Program.IsletmeAyarlar["usid"] : Program.IsletmeAyarlar["steuernummer"];
                    List<Item> items = new List<Item>();
                    foreach (SatisYap satisYap in Program.BonBeleg.basilacakFis.SatisKalem)
                    {
                        Item obj = new Item();
                        obj.barcode = satisYap.Barkod;
                        obj.currency = "€";
                        obj.price = satisYap.Toplamtutar.ToString("#0.00");
                        obj.tax = (double)satisYap.Mwst;
                        obj.taxCost = Math.Round(satisYap.Toplamtutar - satisYap.Toplamtutar / (double)((100 + satisYap.Mwst) / 100), 2);
                        obj.taxSymbol = satisYap.Mwst == Program.MwStList[1] ? "A" : (satisYap.Mwst == Program.MwStList[2] ? "B" : ((satisYap.Mwst == 0 && satisYap.Grubid != 7 && satisYap.Grubid != 6 && satisYap.Grubid != 43) ? "C" : ""));
                        obj.title = satisYap.UrunAd;

                        if (satisYap.Gruptur == 3 || satisYap.Gruptur == 8 || satisYap.Gruptur == 7)
                        {

                            obj.unitId = 3;
                            obj.unit = satisYap.Adet.ToString("#0.000");
                        }
                        else
                        {

                            obj.unitId = 1;
                            obj.unit = satisYap.Adet.ToString();
                        }

                        obj.unitPrice = satisYap.Satisfiyat.ToString("#0.00");
                        items.Add(obj);
                    }
                    List<TaxType> taxInfo = new List<TaxType>();
                    if (Program.BonBeleg.basilacakFis.mwst7Uygulanantutar > 0.0)
                    {
                        TaxType taxType = new TaxType();
                        taxType.tax = 7.0;
                        taxType.taxCost = Math.Round(Math.Round(Program.BonBeleg.basilacakFis.mwst7Uygulanantutar, 2, MidpointRounding.AwayFromZero) - Math.Round(Math.Round(Program.BonBeleg.basilacakFis.mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero), 2);
                        taxType.taxBtax = Math.Round(Program.BonBeleg.basilacakFis.mwst7Uygulanantutar, 2, MidpointRounding.AwayFromZero);
                        taxType.taxAtax = Math.Round(Program.BonBeleg.basilacakFis.mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);
                        taxType.taxSymbol = "A:MwSt " + Program.MwStList[1] + "%";
                        num2 += taxType.taxCost;
                        taxInfo.Add(taxType);
                    }
                    if (Program.BonBeleg.basilacakFis.mwst19Uygulanantutar > 0.0)
                    {
                        TaxType taxType = new TaxType();
                        taxType.tax = 19.0;
                        taxType.taxBtax = Math.Round(Program.BonBeleg.basilacakFis.mwst19Uygulanantutar, 2);
                        taxType.taxAtax = Math.Round(Program.BonBeleg.basilacakFis.mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);
                        taxType.taxCost = Math.Round(Program.BonBeleg.basilacakFis.mwst19Uygulanantutar - Math.Round(Program.BonBeleg.basilacakFis.mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero), 2);
                        taxType.taxSymbol = "B:MwSt " + Program.MwStList[2] + "%";
                        num2 += taxType.taxCost;
                        taxInfo.Add(taxType);
                    }
                    if (Program.BonBeleg.basilacakFis.mwst0Uygulanantutar > 0.0)
                    {
                        TaxType taxType = new TaxType();
                        taxType.tax = 0.0;
                        taxType.taxBtax = Math.Round(Program.BonBeleg.basilacakFis.mwst0Uygulanantutar, 2);
                        taxType.taxAtax = Math.Round(Program.BonBeleg.basilacakFis.mwst0Uygulanantutar, 2, MidpointRounding.AwayFromZero);
                        taxType.taxCost = 0.0;
                        taxType.taxSymbol = "C:MwSt " + Program.MwStList[0] + "%";
                        num2 += taxType.taxCost;
                        taxInfo.Add(taxType);
                    }
                    //qrcodeData = "V0;" + Program.HerstellerKasseID + ";Kassenbeleg-V1;" + basilacakFis.TseProcessData + ";" + basilacakFis.Transactionsnummer + ";" + basilacakFis.TseSignaturzahler + ";" + tarih.tarih(basilacakFis.Datum_start) +
                    // ";" + tarih.tarih(Convert.ToInt32(basilacakFis.TseLogtime)) + ";ecdsa-plain-SHA384;unixTime;" + basilacakFis.TseFinishSignatur + ";" + Program.PublicKey;
                    //  m_Printer.PrintNormal(PrinterStation.Receipt, qrcodeData + "\n");

                    qr qrCode = new qr();
                    if (Program.TSE == "1")
                    {
                        qrCode.bqr_kassen_seriennummer = Program.HerstellerKasseID;
                        qrCode.bqr_log_time = tarih.TSEtarih(Convert.ToInt32(Program.BonBeleg.basilacakFis.TseLogtime)); //tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogtime))
                        qrCode.bqr_log_time_format = "unixTime";
                        qrCode.bqr_process_data = Program.BonBeleg.basilacakFis.TseProcessData;
                        qrCode.bqr_process_type = "Kassenbeleg-V1";
                        qrCode.bqr_public_key = Program.PublicKey;
                        qrCode.bqr_sig_alg = "ecdsa-plain-SHA384";
                        qrCode.bqr_signatur = Program.BonBeleg.basilacakFis.TseFinishSignatur;
                        qrCode.bqr_signatur_zaehler = Convert.ToInt32(Program.BonBeleg.basilacakFis.TseSignaturzahler);
                        qrCode.bqr_start_zeit = tarih.TSEtarih(Convert.ToInt32(Program.BonBeleg.basilacakFis.TseLogTimeStart));// tarih.tarih(Program.BonBeleg.basilacakFis.Datum_start); 
                        qrCode.bqr_transaktions_nummer = Convert.ToInt32(Program.BonBeleg.basilacakFis.Transactionsnummer);
                        qrCode.bqr_version = "V0";
                    }
                    Loyalty kundencard = null;
                    if (Program.eBonCompanyStatus.permission.cp_can_loyalty == true)
                    {
                        kundencard = new Loyalty();
                        if (res.data.cards.Count == 0)
                        {
                            kundencard = null;
                        }
                        else
                        {
                            kundencard.cardNumber = Program.BonBeleg.basilacakFis.Musteri.Barkod;
                            kundencard.currentBalance = (Program.BonBeleg.basilacakFis.EskiPuanToplamı + Program.BonBeleg.basilacakFis.KazanilanPuan + Program.BonBeleg.basilacakFis.HarcananPuan);
                            kundencard.earnedBalance = Program.BonBeleg.basilacakFis.KazanilanPuan;
                            kundencard.previousBalance = Program.BonBeleg.basilacakFis.EskiPuanToplamı;
                            kundencard.spentBalance = Program.BonBeleg.basilacakFis.HarcananPuan;
                            kundencard.cardHolderName = Program.BonBeleg.basilacakFis.Musteri.AdSoyad;
                        }
                    }
                    if (Program.zvt == "ReaRetail")
                    {
                        if (Program.kundenbeleg.Count > 0)
                        {
                            string encodeSatir = "";
                            /* m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                             for (int c = 0; c < Program.kundenbeleg.Count; c++)
                             {
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.kundenbeleg[c] + "\n");
                             }*/
                            /* List<string> SlipList = new List<string>();
                              foreach(string Satir in Program.kundenbeleg)
                              {
                                  var encodeSatir = System.Text.Encoding.UTF8.GetBytes(Satir);
                              SlipList.Add(System.Convert.ToBase64String(encodeSatir));
                              }*/
                            foreach (string Satir in Program.kundenbeleg)
                            {
                                encodeSatir += Satir + "\n";

                            }
                            System.Text.Encoding.UTF8.GetBytes(encodeSatir);
                            slipText = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(encodeSatir));
                        }
                    }


                    string bon = ebonMain.createBon(code, Program.IsletmeAyarlar["companyID"], qrCode, items, firmenInfo, new ReceipInfo()
                    {
                        cash = Program.BonBeleg.basilacakFis.verilenpara,
                        changeTotal = Program.BonBeleg.basilacakFis.paraustu,
                        discountTotal = -Program.BonBeleg.basilacakFis.RabatList.Sum<RabattMain>((Func<RabattMain, double>)(x => x.TotalRabattMenge)),
                        paymentType = Program.BonBeleg.OdemeTur == 0 ? 2 : 1,
                        taxType = taxInfo,
                        totalCost = Program.BonBeleg.basilacakFis.toplamtutar,
                        totalTax = num2,
                        companyId = Program.IsletmeAyarlar["companyID"],
                        companyToken = Program.IsletmeAyarlar["token"],
                        Items = items,
                        verifiedCode = code
                    }, taxInfo, kundencard, Program.IsletmeAyarlar["eBonBearer"], slipText);
                    CheckForIllegalCrossThreadCalls = false;
                    float size = lblParaUstu.Font.Size;
                    Font lbfon = lblParaUstu.Font;
                    // lblParaUstu.Font = new Font(lblParaUstu.Font.Name, 5f);
                    // lblParaUstu.TextAlign = ContentAlignment.MiddleLeft;

                    if (bon == "OK")
                    {
                        lblParaUstu.Text = "e-Bon ist OK!";
                    }
                    else
                    {
                        error++;
                        if (error < 2)
                        {
                            goto a;
                        }
                        else
                        {
                            goto b;
                        }
                    }
                    lblParaUstu.Font = lbfon;
                    //lblParaUstu.TextAlign = ContentAlignment.MiddleRight;

                }
                else if (Program.PrinterLib == ".NET")
                {
                    if (Program.BonBeleg.basilacakFis != null)
                        new FisBarkodlu()
                        {
                            basilacakFis = Program.BonBeleg.basilacakFis,
                            OdemeTur = Program.BonBeleg.OdemeTur
                        }.FisYaz();
                    else if (Program.BonBeleg.basilacakFatura != null)
                    {
                        F_PrinterAuswahl fPrinterAuswahl = new F_PrinterAuswahl();
                        int num3 = (int)fPrinterAuswahl.ShowDialog();
                        if (fPrinterAuswahl.sonuc == 0)
                            new FisBarkodlu()
                            {
                                basilacakFatura = Program.BonBeleg.basilacakFatura,
                                OdemeTur = Program.BonBeleg.OdemeTur
                            }.FisYaz();
                        else
                            new RechnungPrintClass().print(Program.BonBeleg.basilacakFatura, 1);
                    }
                }
                else
                    new FisBarkodlu()
                    {
                        basilacakFis = Program.BonBeleg.basilacakFis,
                        OdemeTur = Program.BonBeleg.OdemeTur
                    }.FisYaz();
                KundenDisplay();
                yeniFis = null;
                Program.BonBeleg.basilacakFis = null;
                this.Enabled = true;
            }
            else
            {

                if (Program.PrinterLib == ".NET")
                {
                    if (Program.BonBeleg.basilacakFis != null)
                        new FisBarkodlu()
                        {
                            basilacakFis = Program.BonBeleg.basilacakFis,
                            OdemeTur = Program.BonBeleg.OdemeTur
                        }.FisYaz();
                    else if (Program.BonBeleg.basilacakFatura != null)
                    {
                        F_PrinterAuswahl fPrinterAuswahl = new F_PrinterAuswahl();
                        int num = (int)fPrinterAuswahl.ShowDialog();
                        if (fPrinterAuswahl.sonuc == 0)
                            new FisBarkodlu()
                            {
                                basilacakFatura = Program.BonBeleg.basilacakFatura,
                                OdemeTur = Program.BonBeleg.OdemeTur
                            }.FisYaz();
                        else
                            new RechnungPrintClass().print(Program.BonBeleg.basilacakFatura, 1);
                    }
                    yazilacakBon = null;
                }
                else
                {
                    if (Program.BonBeleg.basilacakFis.Musteri != null)
                    {
                        if (Program.BonBeleg.basilacakFis.Musteri.Kontotyp == 1)
                        {
                            new RechnungPrintClass().printBonAlsRechnung(Program.BonBeleg.basilacakFatura, 1);
                        }
                        else
                        {
                            new FisBarkodlu()
                            {
                                basilacakFis = Program.BonBeleg.basilacakFis,
                                OdemeTur = Program.BonBeleg.OdemeTur
                            }.FisYaz();
                            KundenDisplay();
                            yazilacakBon = null;
                            Program.BonBeleg.basilacakFis = null;
                        }
                    }
                    else
                    {
                        new FisBarkodlu()
                        {
                            basilacakFis = Program.BonBeleg.basilacakFis,
                            OdemeTur = Program.BonBeleg.OdemeTur
                        }.FisYaz();
                        KundenDisplay();
                        yazilacakBon = null;
                        Program.BonBeleg.basilacakFis = null;
                    }

                }
            }
            if (Program.StopWatch == 1)
            {
                StWaBonDruck.Stop();
                TimeSpan etts2 = StWaBonDruck.Elapsed;

                StWaPosSaveMessage += "\n Druck Process Dauer:" + etts2.ToString(@"hh\:mm\:ss\:fff");
                MessageBox.Show(StWaPosSaveMessage);
            }
            this.Enabled = true;
        }

        private void timerRuckgeldStart()
        {
            timerRuckgeld.Enabled = true;
            timerRuckgeld.Start();
        }

        public static byte[] GetByteArrayFromIntArray(int[] intArray)
        {
            byte[] numArray = new byte[intArray.Length * 4];
            for (int index = 0; index < intArray.Length; ++index)
                Array.Copy((Array)BitConverter.GetBytes(intArray[index]), 0, (Array)numArray, index * 4, 4);
            return numArray;
        }
        string appendChecksum(string code)
        {
            var sum = 0;

            for (var i = code.Length; i >= 1; i--)
            {
                var d = Convert.ToInt32(code.Substring(i - 1, 1));
                var f = i % 2 == 0 ? 3 : 1;
                sum += d * f;
            }
            var checksum = (10 - (sum % 10)) % 10;

            return code + checksum;
        }
        private string GetGutscheinInfo(string kod)
        {
            while (kod.Length < 4)
            {
                kod = "0" + kod;
                if (kod.Length == 4)
                {
                    break;
                }

            }
            return kod;
        }
        private void barkodDegerlendir(string brkd)
        {
            if (txtGiris.Text != "")
            {
                if (txtGiris.Text.IndexOf('X') != -1)
                {
                    int length = txtGiris.Text.IndexOf('X');
                    if (double.TryParse(txtGiris.Text.Substring(0, length), out PLUSatilanAdet))
                    {
                    }
                    else
                    {
                        PLUSatilanAdet = 1.0;
                    }


                }

                else if (double.TryParse(txtGiris.Text, out PLUSatilanAdet))
                {
                }
                else
                {
                    PLUSatilanAdet = 1;
                }

            }
            RuckGeldCounter = 10;
            if (!(brkd != ""))
                return;

            if (brkd.Length == 13)
            {
                brkd = Regex.Replace(brkd, "[^0-9]+", string.Empty);
                if (brkd.Length < 13)
                {

                    gelenBarkod = appendChecksum(brkd.Substring(0, 12));
                }
                else
                {
                    gelenBarkod = brkd;

                }
            }
            //gelenBarkod = brkd;
            // MessageBox.Show(gelenBarkod);
            if (Program.ScaleName == "Digi")
            {
                DigiWaageRead(gelenBarkod);
                return;
            }
            else if (gelenBarkod.Substring(0, 4) == "WBON")//Mettler QR Code
            {
                string[] FleischProdukte;
                FleischProdukte = gelenBarkod.Split(';');
                for (int a = 0; a < FleischProdukte.Length; a++)
                {
                    gelenBarkod = FleischProdukte[a].Replace(";", "");
                    if (gelenBarkod.Substring(0, 2) == "QR")
                    {
                        gelenBarkod = gelenBarkod.Substring(2, gelenBarkod.Length - 2);
                    }

                    string[] MTBonInfo;
                    int FPlu = 0;
                    double FPreis = 0, FVK = 0, FSumme = 0;
                    MTBonInfo = gelenBarkod.Split('|');
                    if (MTBonInfo.Length > 0)
                    {
                        if (int.TryParse(MTBonInfo[0].Substring(4, MTBonInfo[0].Length - 4), out FPlu))
                        {
                            try
                            {
                                double result = 0.0;

                                Artikel artikel = new Artikel();
                                artikel.ArtikelBulGewicht(FPlu.ToString());
                                double num;
                                num = Math.Round(Convert.ToDouble(MTBonInfo[2]) / 100, 2);
                                if (artikel.urunvarmi)
                                {
                                    if (yeniFis == null)
                                    {
                                        yeniFis = new FisOlustur();
                                        yeniFis.FisYarat(0);
                                        position = 1;
                                        listView1.Items.Clear();
                                    }
                                    satisYap = new SatisYap();
                                    satisYap.Adet = (Convert.ToDouble(MTBonInfo[1]) == 1 ? 1 : Math.Round(Convert.ToDouble(MTBonInfo[1]) / 1000, 3));
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = artikel.Mwst;
                                    satisYap.Satisfiyat = num;
                                    satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                    satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                    satisYap.Birimkar = artikel.KarMiktari;
                                    satisYap.Grubid = artikel.Grubid;
                                    satisYap.Barkod = FPlu.ToString();
                                    satisYap.Toplamtutar = Math.Round(Convert.ToDouble(MTBonInfo[3]) / 100, 2);
                                    if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                                    {
                                        satisYap.Angebotvarmi = 1;
                                        angebotSembol = "*";
                                    }
                                    else
                                    {
                                        satisYap.Angebotvarmi = 0;
                                        angebotSembol = "";
                                    }
                                    satisYap.UrunAd = artikel.ArtikelAd;
                                    yeniFis.SatisKalem.Add(satisYap);
                                    int count = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    listView1.Items[count].SubItems.Add(artikel.ArtikelAd + " (" + (satisYap.Adet != 1 ? (satisYap.Adet.ToString("#0.000") + "gr x") : (satisYap.Adet.ToString("#0.00") + "Stk. x")) + num.ToString("C") + ")");
                                    listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[count].Tag = satisYap.Barkod;
                                    if (listView1.Items.Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    txtGiris.Text = "";
                                    if (satisYap.Toplamtutar <= 0)
                                        Console.Beep(1000, 1000);
                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    ++position;
                                    if (dsp != null)
                                    {
                                        KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    }
                                    else if (Program.displayType == "TVS")
                                    {
                                        if (Program.IsletmeAyarlar["kod"] == "383")
                                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C"), satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        else
                                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    }
                                    else if (serialPortKD2.IsOpen)
                                    {
                                        KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    }
                                }
                                else
                                {
                                    UrunYok();
                                }

                                gelenBarkod = "";

                            }
                            catch (Exception ex)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = ex.Message;
                                int num = (int)fGenericError.ShowDialog();
                                return;
                            }
                        }
                    }
                }
                return;
            }
            else if (Program.ScaleName == "Bizerba")
            {
                if ((gelenBarkod.Substring(0, 2) == "24" || gelenBarkod.Substring(0, 2) == "22") && gelenBarkod.Length == 13)
                {
                    Logger log = new Logger("LOG\\BIZERBA_WAAGE");
                    try
                    {
                        int BizerbaBonNr = 0;
                        if (int.TryParse(gelenBarkod.Substring(2, 5), out BizerbaBonNr))
                        {
                            log.Log("Org. Barcode(Read):" + gelenBarkod);
                            log.Log("Biterba Bon nach  Barcode(Read):" + BizerbaBonNr);
                            if (BizerbaBonNr != 0)
                            {

                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.Infoevent += new FisOlustur.lblParaUstuYaz(ParaUstuLabelaYaz);
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                    //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                                    lblParaUstu.Text = "";
                                }
                                //find BizerbaBon
                                MySqlConnection mySqlConnection = new MySqlConnection();
                                MySqlConnection connection = new db().myconn();

                                if (connection.State == ConnectionState.Closed)
                                    connection.Open();
                                string BizSQL = "SELECT  biz_umsatz_pos.*, ABEZ,EK, EAN1, biz_umsatz_kopf.gelesen, sysWGNR FROM biz_umsatz_pos LEFT JOIN `biz_artikel` ON PLNR=artnr INNER JOIN biz_umsatz_kopf ON biz_umsatz_kopf.id=biz_umsatz_pos.kopf_id WHERE biz_umsatz_pos.bonnr=" + BizerbaBonNr + " ORDER by kopf_id DESC";
                                MySqlDataAdapter myDaBiz = new MySqlDataAdapter(BizSQL, connection);
                                DataTable dtBiz = new DataTable();
                                myDaBiz.Fill(dtBiz);
                                log.Log("Find Bizerba Bon SQL:" + BizSQL);
                                if (dtBiz.Rows.Count > 0)
                                {
                                    log.Log("Found Bizerba Bon Pos. Count:" + dtBiz.Rows.Count);
                                    //Read Check
                                    string bizCheck = "";
                                    bizCheck = "SELECT * FROM biz_umsatz_kopf WHERE id=" + dtBiz.Rows[0].ItemArray[1] + "";
                                    MySqlDataAdapter myDaCheck = new MySqlDataAdapter(bizCheck, connection);
                                    DataTable dtCheck = new DataTable();
                                    myDaCheck.Fill(dtCheck);
                                    log.Log("Find Bizerba Bon first Bon:" + "SELECT * FROM biz_umsatz_kopf WHERE id = " + dtBiz.Rows[0].ItemArray[1]);
                                    if (dtCheck.Rows.Count > 0)
                                    {
                                        log.Log("Find Bizerba Bon first Bon Count:" + "SELECT * FROM biz_umsatz_kopf WHERE id = " + dtBiz.Rows[0].ItemArray[1] + " Count:" + dtCheck.Rows.Count);
                                        if (Convert.ToInt16(dtCheck.Rows[0].ItemArray[10]) == 1)
                                        {
                                            log.Log("Bizerba was read-Meldung an der Bediener");
                                            F_GenericError fGenericError = new F_GenericError();
                                            fGenericError.lblMesaj.Text = "Dieser Bon wurde schon gelesen!\nLese-Datum:" + tarih.tarih(Convert.ToInt32(dtCheck.Rows[0].ItemArray[11]));
                                            int num = (int)fGenericError.ShowDialog();
                                            return;
                                        }
                                        else
                                        {
                                            string bizCheckPos = "";                //12
                                            bizCheckPos = "SELECT biz_umsatz_pos.*, ABEZ,EK, EAN1,sysWGNR FROM biz_umsatz_pos LEFT JOIN biz_artikel ON PLNR=artnr WHERE kopf_id=" + dtBiz.Rows[0].ItemArray[1] + "";
                                            MySqlDataAdapter myDaCheckPos = new MySqlDataAdapter(bizCheckPos, connection);
                                            DataTable dtCheckPos = new DataTable();
                                            myDaCheckPos.Fill(dtCheckPos);
                                            for (int q = 0; q < dtCheckPos.Rows.Count; q++)
                                            {
                                                log.Log("Produt was found- Count:" + (q + 1) + "/" + dtCheckPos.Rows.Count);
                                                satisYap = new SatisYap();
                                                satisYap.Adet = Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[8]);
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                                satisYap.Barkod = dtCheckPos.Rows[q].ItemArray[14].ToString();
                                                satisYap.KasaNo = Program.kasano;
                                                satisYap.Mwst = Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[6]);
                                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                                satisYap.Alisfiyat = dtCheckPos.Rows[q].ItemArray[13] is DBNull ? 0 : Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[13]);
                                                satisYap.Satisfiyat = Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[7]);
                                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                                satisYap.UrunId = Convert.ToDecimal(dtCheckPos.Rows[q].ItemArray[14]);
                                                satisYap.Birimkar = Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[7]) - (dtCheckPos.Rows[q].ItemArray[13] is DBNull ? 0 : Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[13]));
                                                if (Convert.ToInt32(dtCheckPos.Rows[q].ItemArray[15]) == 0)
                                                {
                                                    ArtikelGrup artGrp = new ArtikelGrup(1);
                                                    artGrp.ArtikelGrupInfo(" WHERE isFleischtheke=1");
                                                    satisYap.Grubid = artGrp.Grupno;
                                                    if (artGrp.Rabatpunkte != 1)
                                                    {
                                                        satisYap.Angebotvarmi = 1;
                                                        angebotSembol = "*";
                                                    }
                                                    else
                                                    {
                                                        satisYap.Angebotvarmi = 0;
                                                        angebotSembol = "";
                                                    }
                                                }
                                                else
                                                {
                                                    satisYap.Grubid = Convert.ToInt32(dtCheckPos.Rows[q].ItemArray[15]);
                                                    ArtikelGrup artGrp = new ArtikelGrup(1);
                                                    artGrp.ArtikelGrupInfo(" WHERE grupid=" + Convert.ToInt32(dtCheckPos.Rows[q].ItemArray[15]));
                                                    if (artGrp.Rabatpunkte != 1)
                                                    {
                                                        satisYap.Angebotvarmi = 1;
                                                        angebotSembol = "*";
                                                    }
                                                    else
                                                    {
                                                        satisYap.Angebotvarmi = 0;
                                                        angebotSembol = "";
                                                    }
                                                }
                                                log.Log("Produt was found- Count:" + (q + 1) + "/" + dtCheckPos.Rows.Count + " Groupname:" + satisYap.Grubid);
                                                satisYap.Gruptur = 3;
                                                satisYap.Toplamtutar = Convert.ToDouble(dtCheckPos.Rows[q].ItemArray[11]);

                                                satisYap.UrunAd = dtCheckPos.Rows[q].ItemArray[12] is DBNull ? "Fleisch-Produkte" : dtCheckPos.Rows[q].ItemArray[12].ToString();
                                                yeniFis.SatisKalem.Add(satisYap);
                                                log.Log("Produt was found- Count:" + (q + 1) + "/" + dtCheckPos.Rows.Count + " Name:" + satisYap.UrunAd);
                                                int count = listView1.Items.Count;
                                                listView1.Items.Add(position.ToString());
                                                listView1.Items[count].SubItems.Add(satisYap.UrunAd + " (" + satisYap.Adet.ToString() + "kg. x" + satisYap.Satisfiyat.ToString("C") + ")");
                                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                                listView1.Items[count].Tag = satisYap.Barkod;
                                                if (listView1.Items.Count > 0)
                                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                                txtGiris.Text = "";
                                                if (satisYap.Toplamtutar <= 0)
                                                    Console.Beep(1000, 1000);
                                                yeniFis.FisiKapat();
                                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                                ++position;
                                                if (dsp != null)
                                                {
                                                    KDbirinciSatiraYaz(satisYap.UrunAd, satisYap.Toplamtutar.ToString("#0.00"));
                                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                                }
                                                else if (Program.displayType == "TVS")
                                                {
                                                    //if (Program.IsletmeAyarlar["kod"] == "383")
                                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                    //else
                                                    // DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + "kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0);
                                                }
                                                else if (serialPortKD2.IsOpen)
                                                {
                                                    KDbirinciSatiraYaz(satisYap.UrunAd, satisYap.Toplamtutar.ToString("#0.00"));
                                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                                }
                                            }

                                            //ReadTime Update
                                            string BizUpdSQL = "UPDATE biz_umsatz_kopf SET gelesen=1, leseDatum=" + tarih.unixdate(DateTime.Now) + " WHERE bonnr=" + BizerbaBonNr;
                                            MySqlCommand cmdBizUpd = new MySqlCommand(BizUpdSQL, connection);
                                            cmdBizUpd.ExecuteNonQuery();
                                            gelenBarkod = "";
                                            return;
                                        }
                                    }
                                }
                                else
                                {

                                }




                            }
                        }
                    }
                    catch (Exception gg)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = gg.Message;
                        int num = (int)fGenericError.ShowDialog();
                    }
                }

            }
            if (Program.ProgramAyarlar["KartTyp"] == "gerabo" && gelenBarkod.IndexOf('@') != -1)
            {
                gerabo(gelenBarkod);
                ReadGeraboKart = true;
            }
            else if (gelenBarkod.Substring(0, 2) == "23" && (GetGutscheinInfo(gelenBarkod.Substring(2, 4)) + gelenBarkod.Substring(6, 2) == GetGutscheinInfo(Program.IsletmeAyarlar["kod"]) + "02"))
            {
                int result;
                if (!int.TryParse(gelenBarkod.Substring(6, 6), out result))
                    return;
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM gutschein WHERE barkode='" + gelenBarkod + "' AND aktif=1", connection);
                DataTable dataTable = new DataTable("rabatkupon");
                mySqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count == 0)
                {
                    try
                    {
                        wg.ArtikelGrupList artgrp = new wg.ArtikelGrupList();
                        artgrp.myConn = connection;
                        wg.ArtGrupInterface intface = artgrp.ArtikelGrupNachTyp("isGutschverkauf");
                        if (intface.IsGutscheinKauf == 0)
                        {
                            F_GenericError fGenericError1 = new F_GenericError();
                            fGenericError1.lblMesaj.Text = "Bitte wählen Sie im Warengruppen-Menü eine Warengruppe mit dem Typ „Mehrzweckgutschein-Verkauf“ aus.\n" +
                                "Falls noch keine passende Warengruppe existiert, erstellen Sie eine neue und legen Sie den Typ auf „Mehrzweckgutschein - Verkauf“ fest.";
                            int num1 = (int)fGenericError1.ShowDialog();
                            return;
                        }

                        Gutschein gutsch = new Gutschein();
                        gutsch.CallForm();
                        if (gutsch.GutscheinBetrag != 0)
                        { //INSERT INTO `gutschein`(`id`, `fisno`, `erstelldatum`, `ablaufdatum`, `barkode`, `erstelltmiktar`, `bedienerid`, `bewertungdate`, `aktif`, `restmiktar`) VALUES 
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();
                            MySqlCommand cmdNeu = new MySqlCommand();
                            cmdNeu.Parameters.AddWithValue("@barcode", gelenBarkod);
                            cmdNeu.Parameters.AddWithValue("@erstDatum", tarih.unixdate(DateTime.Now));
                            cmdNeu.Parameters.AddWithValue("@erstelltmiktar", gutsch.GutscheinBetrag);
                            cmdNeu.Parameters.AddWithValue("@bedid", Program.bedID);
                            cmdNeu.Parameters.AddWithValue("@aktif", 1);
                            cmdNeu.Parameters.AddWithValue("@restmiktar", gutsch.GutscheinBetrag);
                            cmdNeu.Connection = connection;
                            cmdNeu.CommandText = "INSERT INTO `gutschein`( `erstelldatum`, `ablaufdatum`, `barkode`, `erstelltmiktar`, `bedienerid`, `aktif`, `restmiktar`)" +
                                "VALUES(@erstDatum, 0, @barcode,@erstelltmiktar, @bedid,@aktif,@restmiktar)";
                            if (cmdNeu.ExecuteNonQuery() > 0)
                            {
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                yeniFis.AktivierteGutschId.Add(cmdNeu.LastInsertedId);
                                Tarih tarih = new Tarih();
                                satisYap = new SatisYap();
                                satisYap.Adet = 1.0;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = 0;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = Convert.ToDouble(gutsch.GutscheinBetrag);
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Convert.ToDouble(gutsch.GutscheinBetrag);
                                satisYap.UrunId = new Decimal(0);
                                satisYap.Birimkar = 0.0;
                                satisYap.UrunAd = "Warengutschein-Kauf";
                                satisYap.Grubid = intface.Grupno;
                                satisYap.Gv_typ_id = (int)GVTypEnum.MehrzweckgutscheinKauf;
                                yeniFis.Gutschein += Convert.ToDouble(gutsch.GutscheinBetrag);
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add("Warengutschein-Kauf (" + (object)1 + "Stk. x" + (Convert.ToDouble(gutsch.GutscheinBetrag)).ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);

                                    //KDbirinciSatiraYaz("Warengutschein-Kauf", (Convert.ToDouble(gutsch.GutscheinBetrag)).ToString("#0.00"));
                                    //  KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                                else
                                {
                                    if (!serialPortKD2.IsOpen)
                                        return;
                                    KDbirinciSatiraYaz("Warengutschein-Kauf", (Convert.ToDouble(gutsch.GutscheinBetrag)).ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                            }

                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception gg)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = gg.Message;
                        int num = (int)fGenericError.ShowDialog();
                    }
                }
                else if (Convert.ToInt16(dataTable.Rows[0].ItemArray[8]) == (short)1)
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["58"] + "\n " + tarih.tarih(Convert.ToInt64(dataTable.Rows[0].ItemArray[7]));
                    int num = (int)fGenericError.ShowDialog();
                }
                else
                {
                    if (yeniFis == null)
                    {
                        yeniFis = new FisOlustur();
                        yeniFis.FisYarat(0);
                        position = 1;
                        listView1.Items.Clear();
                    }
                    Tarih tarih = new Tarih();
                    satisYap = new SatisYap();
                    satisYap.Adet = 1.0;
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.Mwst = 0;
                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                    satisYap.Satisfiyat = -Convert.ToDouble(dataTable.Rows[0].ItemArray[9]);
                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                    satisYap.Toplamtutar = -Convert.ToDouble(dataTable.Rows[0].ItemArray[9]);
                    satisYap.UrunId = new Decimal(0);
                    satisYap.Birimkar = 0.0;
                    satisYap.UrunAd = "Warengutschein";
                    satisYap.Grubid = 6;
                    satisYap.Gv_typ_id = (int)GVTypEnum.MehrzweckgutscheinEinloesung;
                    yeniFis.Gutschein += Convert.ToDouble(dataTable.Rows[0].ItemArray[9]);
                    yeniFis.SatisKalem.Add(satisYap);
                    int count = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[count].SubItems.Add("Warengutschein (" + (object)1 + "x" + (-Convert.ToDouble(dataTable.Rows[0].ItemArray[9])).ToString("C") + ")");
                    listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                    listView1.Items[count].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    ++position;
                    if (dsp != null)
                    {
                        KDbirinciSatiraYaz("Warengutschein", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[9])).ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                    else
                    {
                        if (!serialPortKD2.IsOpen)
                            return;
                        KDbirinciSatiraYaz("Warengutschein", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[9])).ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                }

            }
            else
            {
                if (GewichtsBarcodeCheck(gelenBarkod) || PreisBarcodeCheck(gelenBarkod) || KundenBarcodeCheck(gelenBarkod))
                {
                    return;
                }
                else if (gelenBarkod.Substring(0, 2) == "22")
                {
                    if (Program.IsletmeAyarlar["kod"] == "83")
                    {
                        try
                        {
                            int result1 = 0;
                            double result2 = 0.0;
                            if (!int.TryParse(gelenBarkod.Substring(2, 4), out result1) || !double.TryParse(gelenBarkod.Substring(7, 2) + "," + gelenBarkod.Substring(9, 3), out result2))
                                return;
                            Artikel artikel = new Artikel();
                            artikel.ArtikelBul(result1.ToString());
                            if (!artikel.urunvarmi)
                                return;
                            double num1 = Math.Round(artikel.VkPreis, 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            Tarih tarih = new Tarih();
                            satisYap = new SatisYap();
                            satisYap.Adet = result2;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = artikel.Mwst;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = num1;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.UrunId = (Decimal)artikel.ArtikelId;
                            satisYap.Birimkar = artikel.KarMiktari;
                            satisYap.Grubid = 9;
                            satisYap.Toplamtutar = Math.Round(num1 * result2, 2);
                            if (new ArtikelGrup(9).Rabatpunkte != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Angebotvarmi = 0;
                                angebotSembol = "";
                            }
                            satisYap.UrunAd = angebotSembol + " " + artikel.ArtikelAd;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add(angebotSembol + artikel.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num1.ToString("C") + ")");
                            ListViewItem.ListViewSubItemCollection subItems = listView1.Items[count].SubItems;
                            double num2 = satisYap.Toplamtutar;
                            string text = num2.ToString("C");
                            subItems.Add(text);
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            if (Program.displayType == "TVS")
                            {
                                string urunAd = satisYap.UrunAd;
                                num2 = satisYap.Adet;
                                string adet = num2.ToString("#0.000") + "gr.";
                                num2 = satisYap.Satisfiyat;
                                string satisfiyat = num2.ToString("C") + "/kg";
                                string postoplam;
                                if (satisYap.Adet <= 0.0)
                                {
                                    postoplam = "";
                                }
                                else
                                {
                                    num2 = satisYap.Toplamtutar;
                                    postoplam = "\n" + num2.ToString("C");
                                }
                                string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                                DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                                return;
                            }
                            if (!serialPortKD2.IsOpen)
                                return;
                            KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            return;
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                }
                else if (gelenBarkod.Substring(0, 2) == "28")
                {
                    if (Program.IsletmeAyarlar["kod"] == "205")
                    {
                        try
                        {
                            int result1 = 0;
                            double result2 = 0.0;
                            if (!int.TryParse(gelenBarkod.Substring(2, 4), out result1) || !double.TryParse(gelenBarkod.Substring(7, 2) + "," + gelenBarkod.Substring(9, 3), out result2))
                                return;
                            Artikel artikel = new Artikel();
                            artikel.ArtikelBul(result1.ToString());
                            if (!artikel.urunvarmi)
                                return;
                            if (result2 == 0.0)
                            {
                                Math.Round(artikel.VkPreis, 2);
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                satisYap = new SatisYap();
                                satisYap.Adet = 1.0;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = artikel.VkPreis;
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                satisYap.Birimkar = artikel.KarMiktari;
                                satisYap.Grubid = artikel.Grubid;
                                satisYap.Gruptur = 1;
                                satisYap.Toplamtutar = Math.Round(artikel.VkPreis, 2);
                                if (new ArtikelGrup(artikel.Grubid).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = angebotSembol + " " + artikel.ArtikelAd;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add(angebotSembol + artikel.ArtikelAd + " (" + satisYap.Adet.ToString("#0.000") + "Stk x" + artikel.VkPreis.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, artikel.VkPreis.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    return;
                                }
                                if (Program.displayType == "TVS")
                                {
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    return;
                                }
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz(artikel.ArtikelAd, artikel.VkPreis.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            double num1 = Math.Round(artikel.VkPreis, 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            satisYap = new SatisYap();
                            satisYap.Adet = result2;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = artikel.Mwst;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = num1;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.UrunId = (Decimal)artikel.ArtikelId;
                            satisYap.Birimkar = artikel.KarMiktari;
                            satisYap.Grubid = artikel.Grubid;
                            satisYap.Gruptur = 3;
                            satisYap.Toplamtutar = Math.Round(num1 * result2, 2);
                            if (new ArtikelGrup(artikel.Grubid).Rabatpunkte != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Angebotvarmi = 0;
                                angebotSembol = "";
                            }
                            satisYap.UrunAd = angebotSembol + " " + artikel.ArtikelAd;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count1 = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count1].SubItems.Add(angebotSembol + artikel.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num1.ToString("C") + ")");
                            ListViewItem.ListViewSubItemCollection subItems = listView1.Items[count1].SubItems;
                            double num2 = satisYap.Toplamtutar;
                            string text = num2.ToString("C");
                            subItems.Add(text);
                            listView1.Items[count1].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            if (Program.displayType == "TVS")
                            {
                                string urunAd = satisYap.UrunAd;
                                num2 = satisYap.Adet;
                                string adet = num2.ToString("#0.000") + "gr.";
                                num2 = satisYap.Satisfiyat;
                                string satisfiyat = num2.ToString("C") + "/kg";
                                string postoplam;
                                if (satisYap.Adet <= 0.0)
                                {
                                    postoplam = "";
                                }
                                else
                                {
                                    num2 = satisYap.Toplamtutar;
                                    postoplam = "\n" + num2.ToString("C");
                                }
                                string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                                DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                                return;
                            }
                            if (!serialPortKD2.IsOpen)
                                return;
                            KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            return;
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                }
                else if (gelenBarkod.Substring(0, 5) == "24005")
                {
                    if (Program.IsletmeAyarlar["kod"] == "176")
                    {
                        try
                        {
                            int result1 = 0;
                            double result2 = 0.0;
                            if (!int.TryParse(gelenBarkod.Substring(4, 3), out result1) || !double.TryParse(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2), out result2))
                                return;
                            Artikel artikel = new Artikel();
                            artikel.ArtikelBul(result1.ToString());
                            if (!artikel.urunvarmi)
                                return;
                            double num = Math.Round(result2, 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            Tarih tarih = new Tarih();
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = artikel.Mwst;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = num;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.UrunId = (Decimal)artikel.ArtikelId;
                            satisYap.Birimkar = artikel.KarMiktari;
                            satisYap.Grubid = artikel.Grubid;
                            satisYap.Toplamtutar = Math.Round(num, 2);
                            if (new ArtikelGrup(artikel.Grubid).Rabatpunkte != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Angebotvarmi = 0;
                                angebotSembol = "";
                            }
                            satisYap.UrunAd = angebotSembol + " " + artikel.ArtikelAd;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add(angebotSembol + artikel.ArtikelAd + " (" + (object)1 + "Stk x" + num.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            if (Program.displayType == "TVS")
                            {
                                DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                return;
                            }
                            if (!serialPortKD2.IsOpen)
                                return;
                            KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            return;
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                }
                else if (gelenBarkod.Substring(0, 2) == "98" && Program.IsletmeAyarlar["kod"] == "83")
                {
                    int int32 = Convert.ToInt32(gelenBarkod.Substring(2, 5));
                    double text = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                    Tarih tarih = new Tarih();
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    MySqlConnection connection = new db().myconn();

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlDataAdapter mySqlDataAdapter1 = new MySqlDataAdapter("SELECT * FROM pfandboninfo WHERE automatid=" + (object)int32 + " AND datum >" + (object)tarih.unixdate(DateTime.Now.AddDays(-90.0)), connection);
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Rows.Clear();
                    mySqlDataAdapter1.Fill(dataTable1);
                    if (dataTable1.Rows.Count > 0)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Dieser Pfandautomat-Bon wurde am " + tarih.tarih(Convert.ToInt64(dataTable1.Rows[0].ItemArray[3])) + " entwertet!/nDer Bon darf nur einmal entwertet werden!";
                        int num = (int)fGenericError.ShowDialog();
                    }
                    else
                    {
                        try
                        {
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = Program.MwStList[2];
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = -text;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.Grubid = 44;
                            satisYap.Toplamtutar = Math.Round(-text, 2);
                            satisYap.UrunAd = angebotSembol + "PfandRückgabe";
                            satisYap.Gv_typ_id = (int)GVTypEnum.PfandRueckzahlung;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add(angebotSembol + "PfandRückgabe (" + (object)1 + "St x" + satisYap.Satisfiyat.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            double num;
                            if (dsp != null)
                            {
                                num = satisYap.Satisfiyat;
                                KDbirinciSatiraYaz("PfandRückgabe", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else if (serialPortKD2.IsOpen)
                            {
                                num = satisYap.Satisfiyat;
                                KDbirinciSatiraYaz("Feinkost", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            if (!(Program.displayType == "TVS"))
                                return;
                            string urunAd = satisYap.UrunAd;
                            num = satisYap.Adet;
                            string adet = num.ToString() + "Stk.";
                            num = satisYap.Satisfiyat;
                            string satisfiyat = num.ToString("C") + "/Stk.";
                            num = satisYap.Toplamtutar;
                            string postoplam = num.ToString("C");
                            string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                            DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                            SatilanAdet = 0.0;
                        }
                        catch
                        {
                        }
                        finally
                        {
                            VirgulAyikla virgulAyikla = new VirgulAyikla();
                            if (new MySqlCommand("INSERT INTO `pfandboninfo`( `automatid`, `betrag`, `datum`) VALUES (" + (object)int32 + "," + virgulAyikla.virgulayikla(text) + "," + (object)tarih.unixdate(DateTime.Now) + ")", connection).ExecuteNonQuery() > 0)
                            {
                                MySqlDataAdapter mySqlDataAdapter2 = new MySqlDataAdapter("SELECT * FROM `pfandboninfo` WHERE automatid=" + (object)int32 + " AND datum<" + (object)tarih.unixdate(DateTime.Now.AddDays(-90.0)), connection);
                                DataTable dataTable2 = new DataTable();
                                dataTable2.Rows.Clear();
                                mySqlDataAdapter2.Fill(dataTable2);
                                if (dataTable2.Rows.Count > 0)
                                    new MySqlCommand("DELETE FROM `pfandboninfo` WHERE id=" + dataTable2.Rows[0].ItemArray[0], connection).ExecuteNonQuery();
                            }
                        }
                    }

                }
                else if (gelenBarkod.Substring(0, 5) == "27100" && Program.IsletmeAyarlar["kod"] == "238")
                {
                    int int32 = Convert.ToInt32(gelenBarkod.Substring(2, 5));
                    double text = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                    Tarih tarih = new Tarih();
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    MySqlConnection connection = new db().myconn();
                    /*
                         if (connection.State == ConnectionState.Closed)
                             connection.Open();
                         MySqlDataAdapter mySqlDataAdapter1 = new MySqlDataAdapter("SELECT * FROM pfandboninfo WHERE automatid=" + (object)int32 + " AND datum >" + (object)tarih.unixdate(DateTime.Now.AddDays(-90.0)), connection);
                         DataTable dataTable1 = new DataTable();
                         dataTable1.Rows.Clear();
                         mySqlDataAdapter1.Fill(dataTable1);
                         if (dataTable1.Rows.Count > 0)
                         {
                             F_GenericError fGenericError = new F_GenericError();
                             fGenericError.lblMesaj.Text = "Dieser Pfandautomat-Bon wurde am " + tarih.tarih(Convert.ToInt64(dataTable1.Rows[0].ItemArray[3])) + " entwertet!/nDer Bon dar nur einmal entwertet werden!";
                             int num = (int)fGenericError.ShowDialog();
                         }
                         else
                         {*/
                    try
                    {
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        satisYap = new SatisYap();
                        satisYap.Adet = 1.0;
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = Program.MwStList[2];
                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Satisfiyat = -text;
                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                        satisYap.UrunId = new Decimal(0);
                        satisYap.Birimkar = 0.0;
                        satisYap.Grubid = 44;
                        satisYap.Toplamtutar = Math.Round(-text, 2);
                        satisYap.UrunAd = angebotSembol + "PfandRückgabe";
                        satisYap.Gv_typ_id = (int)GVTypEnum.PfandRueckzahlung;
                        yeniFis.SatisKalem.Add(satisYap);
                        int count = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count].SubItems.Add(angebotSembol + "PfandRückgabe (" + (object)1 + "St x" + satisYap.Satisfiyat.ToString("C") + ")");
                        listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        double num;
                        if (dsp != null)
                        {
                            num = satisYap.Satisfiyat;
                            KDbirinciSatiraYaz("PfandRückgabe", num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                        else if (serialPortKD2.IsOpen)
                        {
                            num = satisYap.Satisfiyat;
                            KDbirinciSatiraYaz("Feinkost", num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                        if (!(Program.displayType == "TVS"))
                            return;
                        string urunAd = satisYap.UrunAd;
                        num = satisYap.Adet;
                        string adet = num.ToString() + "Stk.";
                        num = satisYap.Satisfiyat;
                        string satisfiyat = num.ToString("C") + "/Stk.";
                        num = satisYap.Toplamtutar;
                        string postoplam = num.ToString("C");
                        string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                        DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                        SatilanAdet = 0.0;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        /* VirgulAyikla virgulAyikla = new VirgulAyikla();
                         if (new MySqlCommand("INSERT INTO `pfandboninfo`( `automatid`, `betrag`, `datum`) VALUES (" + (object)int32 + "," + virgulAyikla.virgulayikla(text) + "," + (object)tarih.unixdate(DateTime.Now) + ")", connection).ExecuteNonQuery() > 0)
                         {
                             MySqlDataAdapter mySqlDataAdapter2 = new MySqlDataAdapter("SELECT * FROM `pfandboninfo` WHERE automatid=" + (object)int32 + " AND datum<" + (object)tarih.unixdate(DateTime.Now.AddDays(-90.0)), connection);
                             DataTable dataTable2 = new DataTable();
                             dataTable2.Rows.Clear();
                             mySqlDataAdapter2.Fill(dataTable2);
                             if (dataTable2.Rows.Count > 0)
                                 new MySqlCommand("DELETE FROM `pfandboninfo` WHERE id=" + dataTable2.Rows[0].ItemArray[0], connection).ExecuteNonQuery();
                         } */
                    }
                    // }

                }
                else if (gelenBarkod.Substring(0, 3) == "244" && Program.IsletmeAyarlar["kod"] == "84")
                {
                    double num1 = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                    if (yeniFis == null)
                    {
                        yeniFis = new FisOlustur();
                        yeniFis.FisYarat(0);
                        position = 1;
                        listView1.Items.Clear();
                    }
                    Tarih tarih = new Tarih();
                    satisYap = new SatisYap();
                    satisYap.Adet = 1.0;
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.Mwst = Program.MwStList[1];
                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                    satisYap.Satisfiyat = num1;
                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                    satisYap.UrunId = new Decimal(0);
                    satisYap.Birimkar = 0.0;
                    satisYap.Grubid = 388;
                    satisYap.Toplamtutar = Math.Round(num1, 2);
                    if (new ArtikelGrup(388).Rabatpunkte != 1)
                    {
                        satisYap.Angebotvarmi = 1;
                        angebotSembol = "*";
                    }
                    else
                    {
                        satisYap.Angebotvarmi = 0;
                        angebotSembol = "";
                    }
                    satisYap.UrunAd = angebotSembol + "Bedienertheke_Fisch_Fleisch";
                    yeniFis.SatisKalem.Add(satisYap);
                    int count = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[count].SubItems.Add(angebotSembol + "Bedienertheke_Fisch_Fleisch (" + (object)1 + "St x" + num1.ToString("C") + ")");
                    ListViewItem.ListViewSubItemCollection subItems = listView1.Items[count].SubItems;
                    double num2 = satisYap.Toplamtutar;
                    string text = num2.ToString("C");
                    subItems.Add(text);
                    listView1.Items[count].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    ++position;
                    if (dsp != null)
                    {
                        KDbirinciSatiraYaz("Bedienertheke_Fisch_Fleisch", num1.ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                    else if (Program.displayType == "TVS")
                    {
                        string urunAd = satisYap.UrunAd;
                        num2 = satisYap.Adet;
                        string adet = num2.ToString("#0.000") + "gr.";
                        num2 = satisYap.Satisfiyat;
                        string satisfiyat = num2.ToString("C") + "/kg.";
                        num2 = satisYap.Toplamtutar;
                        string postoplam = num2.ToString("C");
                        string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                        DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                    }
                    else
                    {
                        if (!serialPortKD2.IsOpen)
                            return;
                        KDbirinciSatiraYaz("Bedienertheke_Fisch_Fleisch", num1.ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                }
                else
                {
                    if (gelenBarkod.Substring(0, 3) == "240")
                    {
                        if (Program.IsletmeAyarlar["kod"] == "84")
                        {
                            try
                            {
                                int result1 = 0;
                                double result2 = 0.0;
                                if (!int.TryParse(gelenBarkod.Substring(2, 4), out result1) || !double.TryParse(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2), out result2))
                                    return;
                                Artikel artikel = new Artikel();
                                artikel.ArtikelBul(result1.ToString());
                                if (!artikel.urunvarmi)
                                    return;
                                double num1 = Math.Round(result2, 2);
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                Tarih tarih = new Tarih();
                                satisYap = new SatisYap();
                                satisYap.Adet = result2;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = num1;
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                satisYap.Birimkar = artikel.KarMiktari;
                                satisYap.Grubid = 406;
                                satisYap.Toplamtutar = Math.Round(num1, 2);
                                if (new ArtikelGrup(9).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = angebotSembol + " " + artikel.ArtikelAd;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add(angebotSembol + artikel.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num1.ToString("C") + ")");
                                ListViewItem.ListViewSubItemCollection subItems = listView1.Items[count].SubItems;
                                double num2 = satisYap.Toplamtutar;
                                string text = num2.ToString("C");
                                subItems.Add(text);
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    return;
                                }
                                if (Program.displayType == "TVS")
                                {
                                    string urunAd = satisYap.UrunAd;
                                    num2 = satisYap.Adet;
                                    string adet = num2.ToString("#0.000") + "gr.";
                                    num2 = satisYap.Satisfiyat;
                                    string satisfiyat = num2.ToString("C") + "/kg.";
                                    num2 = satisYap.Toplamtutar;
                                    string postoplam = num2.ToString("C");
                                    string toplamtutar = "TOTAL :" + yeniFis.toplamtutar.ToString("C");
                                    DSPINFO(urunAd, adet, satisfiyat, postoplam, toplamtutar, 0, 0, 0, 0);
                                    return;
                                }
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz(artikel.ArtikelAd, num1.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                        }
                    }
                    else if (gelenBarkod.Substring(0, 2) == "24" && Program.IsletmeAyarlar["kod"] == "34")
                    {
                        double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        Tarih tarih = new Tarih();
                        satisYap = new SatisYap();
                        satisYap.Adet = 1.0;
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = Program.MwStList[1];
                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Satisfiyat = num;
                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                        satisYap.UrunId = new Decimal(0);
                        satisYap.Birimkar = 0.0;
                        satisYap.Grubid = 9;
                        satisYap.Toplamtutar = Math.Round(num, 2);
                        if (new ArtikelGrup(9).Rabatpunkte != 1)
                        {
                            satisYap.Angebotvarmi = 1;
                            angebotSembol = "*";
                        }
                        else
                        {
                            satisYap.Angebotvarmi = 0;
                            angebotSembol = "";
                        }
                        satisYap.UrunAd = angebotSembol + "Fleisch";
                        yeniFis.SatisKalem.Add(satisYap);
                        int count = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count].SubItems.Add(angebotSembol + "Fleisch (" + (object)1 + "St x" + num.ToString("C") + ")");
                        listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        if (dsp != null)
                        {
                            KDbirinciSatiraYaz("Fleisch", num.ToString("C"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                        }
                        else
                        {
                            if (!serialPortKD2.IsOpen)
                                return;
                            KDbirinciSatiraYaz("Fleisch", num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                    }
                    else if (gelenBarkod.Substring(0, 2) == "24" && gelenBarkod.Substring(2, Program.IsletmeAyarlar["kod"].Length) == Program.IsletmeAyarlar["kod"] && gelenBarkod.Substring(2 + Program.IsletmeAyarlar["kod"].Length, 2) == "02" && Program.IsletmeAyarlar["kod"] != "37")
                    {
                        MySqlConnection mySqlConnection = new MySqlConnection();
                        MySqlConnection connection = new db().myconn();

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM rabatkupon WHERE barkode='" + gelenBarkod + "'", connection);
                        DataTable dataTable = new DataTable("rabatkupon");
                        dataTable.Rows.Clear();
                        mySqlDataAdapter.Fill(dataTable);
                        if (dataTable.Rows.Count == 0)
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["57"];
                            int num = (int)fGenericError.ShowDialog();
                        }
                        else if (Convert.ToInt16(dataTable.Rows[0].ItemArray[8]) == (short)1)
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["58"] + "\n " + tarih.tarih(Convert.ToInt64(dataTable.Rows[0].ItemArray[7]));
                            int num = (int)fGenericError.ShowDialog();
                        }
                        else
                        {
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            Tarih tarih = new Tarih();
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = 0;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = -Convert.ToDouble(dataTable.Rows[0].ItemArray[6]);
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = -Convert.ToDouble(dataTable.Rows[0].ItemArray[6]);
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.UrunAd = "Rabatt Coupon";
                            satisYap.Grubid = 43;
                            satisYap.Gv_typ_id = (int)GVTypEnum.Rabatt;
                            yeniFis.SatisKalem.Add(satisYap);
                            yeniFis.RabatList.Add(new RabattMain()
                            {
                                RabattAlani = 0,
                                Grupid = 43,
                                RabattTyp = 1,
                                RabatMenge = Convert.ToDouble(dataTable.Rows[0].ItemArray[6]),
                                RabatArt = 6,
                                RabatName = "RabattCoupon"
                            });
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add("Rabat Coupon (" + (object)1 + "x" + (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            DSPINFO("RABATTCOUPON " + (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("C"), "", "", "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz("Rabat Coupon", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else if (serialPortKD2.IsOpen)
                            {
                                KDbirinciSatiraYaz("Rabat Coupon", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            if (new MySqlCommand("UPDATE rabatkupon SET bewertungdate=" + (object)tarih.unixdate(DateTime.Now) + ", aktif=1 WHERE id=" + dataTable.Rows[0].ItemArray[0], connection).ExecuteNonQuery() <= 0)
                                return;
                            connection.Close();
                        }

                    }
                    else if (gelenBarkod.Substring(0, 7) == ((int)Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + 2).ToString() + "00000")
                    {
                        /* double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                         if (yeniFis == null)
                         {
                             yeniFis = new FisOlustur();
                             yeniFis.FisYarat(0);
                             position = 1;
                             listView1.Items.Clear();
                         }
                         Tarih tarih = new Tarih();
                         satisYap = new SatisYap();
                         satisYap.Adet = 1.0;
                         satisYap.Fisno = yeniFis.SatisAnaId;
                         satisYap.KasaNo = Program.kasano;
                         satisYap.Mwst = 19;
                         satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                         satisYap.Satisfiyat = -Convert.ToDouble(num);
                         satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                         satisYap.Toplamtutar = -Convert.ToDouble(num);
                         satisYap.UrunId = new Decimal(0);
                         satisYap.Birimkar = 0.0;
                         satisYap.UrunAd = "PfandRuckgabe";
                         satisYap.Gv_typ_id = (int)GVTypEnum.PfandRueckzahlung;
                         satisYap.Grubid = 64;
                         yeniFis.SatisKalem.Add(satisYap);
                         int count = listView1.Items.Count;
                         listView1.Items.Add(position.ToString());
                         listView1.Items[count].SubItems.Add("PfandRuckgabe (" + (object)1 + "x" + num.ToString("C") + ")");
                         listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                         listView1.Items[count].Tag = satisYap.Barkod;
                         if (listView1.Items.Count > 0)
                             listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                         txtGiris.Text = "";
                         yeniFis.FisiKapat();
                         txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                         ++position;
                         if (dsp != null)
                         {
                             KDbirinciSatiraYaz("Pfand Ruckgabe", num.ToString("#0.00"));
                             KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                         }
                         else
                         {
                             if (!serialPortKD2.IsOpen)
                                 return;
                             KDbirinciSatiraYaz("Pfand Ruckgabe", num.ToString("#0.00"));
                             KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                         }*/
                    }
                    else if (gelenBarkod.Substring(0, 7) == Convert.ToInt16(Program.IsletmeAyarlar["kod"]).ToString() + "00000")
                    {
                        Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        if (getMusteriNo(gelenBarkod) == -1L)
                            return;
                        yeniFis.Musterino = getMusteriNo(gelenBarkod);
                    }
                    else if ((gelenBarkod.Substring(0, 7) == "0000000" || gelenBarkod.Substring(0, 7) == "0100000" || gelenBarkod.Substring(0, 7) == "2408732") && (Program.IsletmeAyarlar["kod"] == "70" || Program.IsletmeAyarlar["kod"] == "71" || Program.IsletmeAyarlar["kod"] == "25"))
                    {
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        if (getMusteriNo(gelenBarkod) == -1L)
                            return;
                        yeniFis.Musterino = getMusteriNo(gelenBarkod);
                    }
                    else if (gelenBarkod.Substring(0, 7) == "1000000" && Program.IsletmeAyarlar["kod"] == "45")
                    {
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        if (getMusteriNo(gelenBarkod) == -1L)
                            return;
                        yeniFis.Musterino = getMusteriNo(gelenBarkod);
                    }
                    else
                    {
                        barkodluUrunEkle();
                    }
                }
            }
        }
        private bool KundenBarcodeCheck(string gelenBarkod)
        {
            if (gelenBarkod.Length >= 8)
            {
                try
                {
                    string gelenBa = gelenBarkod.Substring(0, 8);
                    /*Buda olur
                     * List<string> list = Program.KundenBarcodeInfo.Where<string>((Func<string, bool>)(i => gelenBa.Contains(i[1]))).ToList<string>();
                     int found = 0;

                     if (list.Count <= 0)
                     {
                         return false;
                     }
                     else
                     {
                         foreach(string brk in list)
                         {

                             if (gelenBa.Substring(0, brk.Length) == brk)
                             {
                                 found = 1;
                                 break;
                             }
                         }
                         if (found == 0)
                         {
                             return false;
                         }
                     }*/
                    var match = Program.KundenBarcodeInfo.FirstOrDefault(stringToCheck => stringToCheck.Contains(gelenBa.Substring(0, stringToCheck.Length)));
                    if (match == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        long musteriNo = getMusteriNo(gelenBarkod);
                        if (musteriNo != -1L)
                        {
                            yeniFis.Musterino = musteriNo;
                            DSPINFO("", "", "", "", "Kunde :" + yeniFis.Musteri.AdSoyad, 10, 0, 0, 0);
                            gelenBarkod = "";
                            if (yeniFis.Musteri.Method == 5)
                            {
                                if (yeniFis.SatisKalem.Count > 0)
                                {
                                    CheckKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musteri.MusteriId);
                                    yeniFis.FisiKapat();
                                }
                                return true;
                            }
                            if (yeniFis.Musteri.Method == 2)
                            {
                                if (yeniFis.Musteri.Ozeloran != 0)
                                {
                                    if (yeniFis == null)
                                        return false;
                                    RabattMain rabattMain = new RabattMain();
                                    rabattMain.RabatName = "Allgemeinrabatt";
                                    rabattMain.RabattAlani = 0;
                                    rabattMain.RabatArt = 3;
                                    rabattMain.RabatMenge = yeniFis.Musteri.OzelOran; //!= 0 ? yeniFis.Musteri.OzelOran : yeniFis.Musteri.GrupIndirimOrani != 0 ? yeniFis.Musteri.GrupIndirimOrani : Prog;
                                    yeniFis.RabatList.Add(rabattMain);
                                    rabattMain.Grupid = 7;
                                    yeniFis.FisiKapat();
                                    //DSPINFO("ALLG. RABATT " + result.ToString("F") + "%", "", "", "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    if (dsp != null)
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    else if (serialPortKD2.IsOpen)
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));

                                    txtGiris.Text = "";
                                    return true;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }





                        }

                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace + ":Kundenbarcode");
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private void CheckKundenPreis(int MusteriGrupId, long MusteriID)
        {
            try
            {
                foreach (SatisYap satkalem in yeniFis.SatisKalem)
                {
                    double KundenPr = 0;
                    KundenPr = GetKundenPreis(yeniFis.Musteri.MusteriId, satkalem.Barkod);
                    if (KundenPr != 0)
                    {
                        satkalem.ProzisyonRabatBetrag += (satkalem.Satisfiyat - KundenPr) * satkalem.Adet;
                        satkalem.Satisfiyat = KundenPr;
                        satkalem.Toplamtutar = KundenPr * satkalem.Adet;
                        satkalem.UrunAd = "#" + satkalem.UrunAd;



                    }
                    else
                    {
                        double KundenGrPr = 0;
                        KundenGrPr = GetKundenGrupPreis(yeniFis.Musteri.MusteriGrup, satkalem.Barkod);
                        if (KundenGrPr != 0)
                        {
                            satkalem.ProzisyonRabatBetrag += (satkalem.Satisfiyat - KundenGrPr) * satkalem.Adet;
                            satkalem.Satisfiyat = KundenGrPr;
                            satkalem.Toplamtutar = KundenGrPr * satkalem.Adet;
                            satkalem.UrunAd = "#" + satkalem.UrunAd;
                        }
                    }


                }
                KundenDisplay();
                listView1.Items.Clear();
                //listView1.Clear();
                position = 1;
                foreach (SatisYap satkalem in yeniFis.SatisKalem)
                {
                    int count1 = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    if (satkalem.Gruptur == 3 || satkalem.Gruptur == 8 || satkalem.Gruptur == 7 || satkalem.Gruptur == 9)
                    {
                        listView1.Items[count1].SubItems.Add(satkalem.UrunAd.ToString() + "(" + (object)satkalem.Adet.ToString("#0.000") + " kg. x " + satkalem.Satisfiyat.ToString("C") + "/kg.)");
                    }
                    else
                    {
                        listView1.Items[count1].SubItems.Add(satkalem.UrunAd.ToString() + "(" + (object)satkalem.Adet + "Stk. x" + satkalem.Satisfiyat.ToString("C") + ")");
                    }
                    listView1.Items[count1].SubItems.Add(satkalem.Toplamtutar.ToString("C"));
                    listView1.Items[count1].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    ++position;

                    DSPINFO(satkalem.UrunAd, satkalem.Adet.ToString() + " Stk.", satkalem.Satisfiyat.ToString("C") + "/Stk.", satkalem.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                }

            }
            catch (Exception ff)
            {
            }

        }
        private double GetSellKundenPreis(int MusteriGrupId, long musterino, string ProdBarcode)
        {
            try
            {

                double KundenPr = 0;
                KundenPr = GetKundenPreis(musterino, ProdBarcode);
                if (KundenPr != 0)
                {
                    return KundenPr;
                }
                else
                {
                    double KundenGrPr = 0;
                    KundenGrPr = GetKundenGrupPreis(MusteriGrupId, ProdBarcode);
                    if (KundenGrPr != 0)
                    {
                        return KundenGrPr;
                    }
                    else
                    {
                        return 0;
                    }
                }



            }
            catch (Exception ff)
            {
                return 0;
            }

        }
        private double GetKundenGrupPreis(long KundenGrupId, string ProdBarcode)
        {
            try
            {
                MySqlConnection connection = new db().myconn();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT preis FROM kundengruppreis WHERE barcode='" + ProdBarcode + "' AND grupid=" + KundenGrupId + " AND ( DATE(NOW())>=DATE(FROM_unixtime(von)) AND DATE(NOW())<=DATE(FROM_unixtime(bis) ) OR bis=0)", connection);
                DataTable dataTable = new DataTable("kundenpreis");
                dataTable.Rows.Clear();
                mySqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return Convert.ToDouble(dataTable.Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception gg)
            {
                return 0;
            }


        }
        private double GetKundenPreis(long kundenid, string Barcode)
        {
            try
            {
                MySqlConnection connection = new db().myconn();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT preis FROM kundenpreis WHERE barkode='" + Barcode + "' AND kundenid=" + kundenid, connection);
                DataTable dataTable = new DataTable("kundenpreis");
                dataTable.Rows.Clear();
                mySqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return Convert.ToDouble(dataTable.Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception gg)
            {
                return 0;
            }


        }
        private bool PreisBarcodeCheck(string gelenBarkod)
        {
            try
            {
                string gelenBa = "";
                List<List<string>> list;

                gelenBa = gelenBarkod.Substring(0, 7);
                list = Program.PreisBarcodeInfo.Where<List<string>>((Func<List<string>, bool>)(i => gelenBarkod.Contains(i[6]) && i[1] == gelenBarkod.Substring(0, i[1].Length))).ToList<List<string>>();

                if (list.Count > 0)
                {
                    if (Program.GlobalAyarlar["PreisBarcodeTyp"] == 3)
                    {
                        list.Clear();
                        list = Program.PreisBarcodeInfo.Where<List<string>>((Func<List<string>, bool>)(i => gelenBarkod.Contains(i[6]) && i[4] != "0" && i[6] != "")).ToList<List<string>>();
                        if (!(gelenBa.Substring(0, list[0][1].Length) == list[0][1]))
                            return false;
                        Artikel artikel = new Artikel();
                        if (Program.IsletmeAyarlar["kod"] == "243")
                        {
                            artikel.ArtikelBulFleich(gelenBarkod.Substring(0, (list[0][1].Length + Convert.ToInt16(list[0][4]))));
                        }
                        else
                        {
                            artikel.ArtikelBulFleich(list[0][6]);
                        }
                        if (artikel.urunvarmi)
                        {

                            /*if (Program.IsletmeAyarlar["kod"] == "237") //TUTAK Özel
                            {
                                artikel = new Artikel();
                                artikel.ArtikelBulGewichtOnlyPLU(gelenBarkod.Substring(list[0][1].Length, (Convert.ToInt16(list[0][4]))));
                                double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                                if (artikel.urunvarmi)
                                {
                                    //tar.Tarih tarih = new tar.Tarih();
                                    if (yeniFis == null)
                                    {
                                        yeniFis = new FisOlustur();
                                        yeniFis.FisYarat(0);
                                        position = 1;
                                        listView1.Items.Clear();
                                    }
                                    satisYap = new SatisYap();
                                    satisYap.Adet = Math.Round(num / (artikel.VkPreis), 3);
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    satisYap.Barkod = gelenBarkod;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = artikel.Mwst;
                                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Satisfiyat = artikel.VkPreis;
                                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                    satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                    satisYap.Birimkar = artikel.KarMiktari;
                                    satisYap.Grubid = artikel.Grubid;
                                    satisYap.Gruptur = 3;
                                    satisYap.Toplamtutar = Math.Round(num * 1.0, 2);
                                    if (satisYap.Toplamtutar >= 100.0)
                                    {
                                        F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                                        summeBesteatigung.summe = satisYap.Toplamtutar;
                                        int num3 = (int)summeBesteatigung.ShowDialog();
                                        if (summeBesteatigung.bestatigung != 1)
                                        {
                                            satisYap = (SatisYap)null;
                                            return false;
                                        }
                                    }
                                    if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                                    {
                                        satisYap.Angebotvarmi = 1;
                                        angebotSembol = "*";
                                    }
                                    else
                                    {
                                        satisYap.Angebotvarmi = 0;
                                        angebotSembol = "";
                                    }
                                    satisYap.UrunAd = artikel.ArtikelAd;
                                    yeniFis.SatisKalem.Add(satisYap);
                                    int count = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    listView1.Items[count].SubItems.Add(artikel.ArtikelAd + " (" + satisYap.Adet.ToString() + "kg. x" + satisYap.Satisfiyat.ToString("C") + ")");
                                    listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[count].Tag = satisYap.Barkod;
                                    if (listView1.Items.Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    txtGiris.Text = "";
                                    if (satisYap.Toplamtutar <= 0)
                                        Console.Beep(1000, 1000);
                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    ++position;
                                    if (dsp != null)
                                    {
                                        KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    }
                                    else if (Program.displayType == "TVS")
                                    {
                                        //if (Program.IsletmeAyarlar["kod"] == "383")
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        //else
                                        // DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + "kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0);
                                    }
                                    else if (serialPortKD2.IsOpen)
                                    {
                                        KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    }
                                    gelenBarkod = "";
                                    return true;
                                }
                            }
                            else*/
                            //{
                            // double etFiyat = Math.Round(Convert.ToDouble(gelenBarkod.Substring((list[0][1].Length + Convert.ToInt16(list[0][4])), (10 - ((list[0][1].Length + Convert.ToInt16(list[0][4]))))) + "," + gelenBarkod.Substring(10, 2)), 2);

                            double etFiyat = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            //Tarih tarih = new Tarih();
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = Program.MwStList[1];
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = etFiyat;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.Grubid = Convert.ToInt32(list[0][3]);
                            satisYap.Toplamtutar = Math.Round(etFiyat, 2);
                            if (satisYap.Toplamtutar >= 100.0)
                            {
                                F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                                summeBesteatigung.summe = satisYap.Toplamtutar;
                                int num3 = (int)summeBesteatigung.ShowDialog();
                                if (summeBesteatigung.bestatigung != 1)
                                {
                                    satisYap = (SatisYap)null;
                                    return false;
                                }
                            }
                            satisYap.Barkod = gelenBarkod;
                            if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Angebotvarmi = 0;
                                angebotSembol = "";
                            }
                            satisYap.UrunAd = angebotSembol + (list[0][5] != "" ? list[0][5] : list[0][2]);
                            yeniFis.SatisKalem.Add(satisYap);
                            int count1 = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count1].SubItems.Add(angebotSembol + (list[0][5] != "" ? list[0][5] : list[0][2]) + " (" + (object)1 + "St x" + etFiyat.ToString("C") + ")");
                            listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count1].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            if (satisYap.Toplamtutar <= 0)
                                Console.Beep(1000, 1000);
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz(list[0][2], etFiyat.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else if (Program.displayType == "TVS")
                                DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", etFiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);

                            gelenBarkod = "";
                            return true;
                            // }
                        }
                        else
                        {
                            double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring((list[0][1].Length + Convert.ToInt16(list[0][4])), (10 - ((list[0][1].Length + Convert.ToInt16(list[0][4]))))) + "," + gelenBarkod.Substring(10, 2)), 2);
                            if (artikel.urunvarmi)
                            {
                                //tar.Tarih tarih = new tar.Tarih();
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                satisYap = new SatisYap();
                                if (artikel.Gruptur == 3)
                                {
                                    satisYap.Adet = Math.Round(num / (artikel.VkPreis), 3);
                                }
                                else
                                {
                                    satisYap.Adet = 1;
                                }
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = artikel.VkPreis;
                                satisYap.Alisfiyat = artikel.EkPreis;
                                satisYap.Birimkar = Math.Round(satisYap.Adet * artikel.KarMiktari, 2);
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                satisYap.Birimkar = artikel.KarMiktari;
                                satisYap.Grubid = artikel.Grubid;
                                satisYap.Gruptur = artikel.Gruptur;
                                satisYap.Barkod = gelenBarkod;
                                satisYap.Toplamtutar = Math.Round(num * 1.0, 2);
                                if (satisYap.Toplamtutar >= 100.0)
                                {
                                    F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                                    summeBesteatigung.summe = satisYap.Toplamtutar;
                                    int num3 = (int)summeBesteatigung.ShowDialog();
                                    if (summeBesteatigung.bestatigung != 1)
                                    {
                                        satisYap = (SatisYap)null;
                                        return false;
                                    }
                                }
                                if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = artikel.ArtikelAd;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                if (artikel.Gruptur != 3)
                                {
                                    listView1.Items[count].SubItems.Add(artikel.ArtikelAd + " (" + satisYap.Adet.ToString() + "Stk. x" + satisYap.Satisfiyat.ToString("C") + ")");
                                }
                                else
                                {
                                    listView1.Items[count].SubItems.Add(artikel.ArtikelAd + " (" + satisYap.Adet.ToString() + "kg. x" + satisYap.Satisfiyat.ToString("C") + ")");
                                }
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                if (satisYap.Toplamtutar <= 0)
                                    Console.Beep(1000, 1000);
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                                else if (Program.displayType == "TVS")
                                {
                                    //if (Program.IsletmeAyarlar["kod"] == "383")
                                    if (satisYap.Gruptur == 3)
                                    {
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    }
                                    else
                                    {
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " Stk.", num.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);

                                    }
                                    //else
                                    // DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + "kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0);
                                }
                                else if (serialPortKD2.IsOpen)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                            }
                            return true;
                        }

                    }
                    else
                    {
                        //list.Clear();
                        //gelenBa = gelenBarkod.Substring(0, 7);
                        //list = Program.PreisBarcodeInfo.Where<List<string>>((Func<List<string>, bool>)(i => gelenBarkod.Contains(i[1]))).ToList<List<string>>();
                        if (list.Count <= 0)
                            return false;
                        if (!(gelenBa.Substring(0, list[0][1].Length) == list[0][1]))
                            return false;
                        double num1 = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        //Tarih tarih = new Tarih();
                        satisYap = new SatisYap();
                        satisYap.Adet = 1.0;
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = Program.MwStList[1];
                        satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Satisfiyat = num1;
                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                        satisYap.UrunId = new Decimal(0);
                        satisYap.Birimkar = 0.0;
                        satisYap.Grubid = Convert.ToInt32(list[0][3]);
                        satisYap.Toplamtutar = Math.Round(num1, 2);
                        if (satisYap.Toplamtutar >= 100.0)
                        {
                            F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                            summeBesteatigung.summe = satisYap.Toplamtutar;
                            int num3 = (int)summeBesteatigung.ShowDialog();
                            if (summeBesteatigung.bestatigung != 1)
                            {
                                satisYap = (SatisYap)null;
                                return false;
                            }
                        }
                        satisYap.Barkod = gelenBarkod;
                        if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                        {
                            satisYap.Angebotvarmi = 1;
                            angebotSembol = "*";
                        }
                        else
                        {
                            satisYap.Angebotvarmi = 0;
                            angebotSembol = "";
                        }
                        satisYap.UrunAd = angebotSembol + list[0][2];
                        yeniFis.SatisKalem.Add(satisYap);
                        int count12 = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count12].SubItems.Add(angebotSembol + list[0][2] + " (" + (object)1 + "St x" + num1.ToString("C") + ")");
                        listView1.Items[count12].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count12].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        if (satisYap.Toplamtutar <= 0)
                            Console.Beep(1000, 1000);
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        if (dsp != null)
                        {
                            KDbirinciSatiraYaz(list[0][2], num1.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                        else if (Program.displayType == "TVS")
                        {
                            //DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                            CheckForIllegalCrossThreadCalls = false;
                            Thread dssp = new Thread(() => this.DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                            dssp.Start();
                        }
                        else if (serialPortKD2.IsOpen)
                        {
                            KDbirinciSatiraYaz(list[0][2], num1.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                        gelenBarkod = "";
                        gelenBa = "";
                        list = null;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }



        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lblParaUstu.Text = "";
            seciliItem = -1;
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(200);
            if (Program.IsletmeAyarlar["kod"] == "90")
                Console.Beep(1500, 200);
            angebotSembol = "";
            int bytesToRead = serialPort1.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            serialPort1.Read(buffer, 0, bytesToRead);
            List<int> intList = new List<int>();
            for (int index = 0; index < ((IEnumerable<byte>)buffer).Count<byte>(); ++index)
            {
                if (buffer[index] == (byte)83 && ((IEnumerable<byte>)buffer).Count<byte>() != 17)
                    index = 6;
                else if (buffer[index] == (byte)83 && ((IEnumerable<byte>)buffer).Count<byte>() == 17)
                    index = 3;
                else if (buffer[index] > (byte)47 && buffer[index] < (byte)58)
                    intList.Add((int)buffer[index]);
            }
            byte[] bytes = new byte[intList.Count];
            int index1 = 0;
            foreach (int num in intList)
            {
                bytes[index1] = Convert.ToByte(num);
                ++index1;
            }
            gelenBarkod = Encoding.ASCII.GetString(bytes);
            if (gelenBarkod.Length == 12 && gelenBarkod.Substring(0, 1) == "0")
                gelenBarkod = "0" + gelenBarkod;
            if (gelenBarkod.Length < 8)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["65"];
                int num = (int)fGenericError.ShowDialog();
            }
            else
                barkodDegerlendir(gelenBarkod);
        }

        private long getMusteriNo(string barkod)
        {
            try
            {
                iss_Kunden.Musteri musteri = new iss_Kunden.Musteri();
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();
                //musteri.db = connection;
                musteri.IsletmeAyarlar = Program.IsletmeAyarlar;
                musteri.MusteriBarkod(barkod);
                if (musteri.MusteriId != 0L)
                {
                    KDbirinciSatiraYaz("Kunde", "");
                    KDikinciSatiraYaz(musteri.MusteriAd + " " + musteri.KundenSoyad);
                    lblParaUstu.Text = musteri.MusteriAd + " " + musteri.KundenSoyad;
                    iss_kundengrup.KundenGrup KundenGrup = new iss_kundengrup.KundenGrup();
                    KundenGrup.MyConn = connection;
                    musteri.GrupIndirimOrani = KundenGrup.GetKundenGrupIndOrani(musteri.MusteriGrup);
                    yeniFis.Musteri = musteri;

                    if (musteri.Method == 2)
                    {
                        double num1 = musteri.OzelOran != 0.0 ? musteri.OzelOran : musteri.GrupIndirimOrani != 0 ? musteri.GrupIndirimOrani : Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["94"] + (object)num1 + "%";
                        int num2 = (int)fGenericError.ShowDialog();
                        yeniFis.Musteri.OzelOran = num1;
                        yeniFis.Musteri.Ozeloran = num1;
                    }
                    txtGiris.Text = "";
                    return (long)Convert.ToInt32(musteri.MusteriId);
                }
                if (!(gelenBarkod.Substring(0, 2) == "01"))
                    return -1;
                F_GenericError fGenericError1 = new F_GenericError();
                fGenericError1.lblMesaj.Text = Program.lang["66"];
                int num = (int)fGenericError1.ShowDialog();
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        private string karakterCevir(string gelenMesaj)
        {
            string str = gelenMesaj;
            char[] chArray1 = new char[14]
      {
        'ö',
        'Ö',
        'ü',
        'Ü',
        'ç',
        'Ç',
        'İ',
        'ı',
        'Ğ',
        'ğ',
        'Ş',
        'ş',
        'ä',
        'Ä'
      };
            char[] chArray2 = new char[14]
      {
        'o',
        'O',
        'u',
        'U',
        'c',
        'C',
        'I',
        'i',
        'G',
        'g',
        'S',
        's',
        'a',
        'A'
      };
            for (int index = 0; index < chArray1.Length; ++index)
                str = str.Replace(chArray1[index], chArray2[index]);
            return str;
        }

        public void KDbirinciSatiraYaz(string p, string p_2)
        {
            string str = "";
            int num1 = 20 - p_2.Length;
            p = karakterCevir(p);
            int length = p.Length;
            try
            {
                if (length > num1)
                {
                    p = p.Substring(0, num1 - 2);
                    p = p + ".:" + p_2;
                }
                else
                {
                    int num2 = num1 - length - 1;
                    for (int index = 0; index < num2; ++index)
                        str += " ";
                    p = p + ":" + str + p_2;
                }
            }
            catch (Exception ex)
            {
                logEntry.AddtoLogFile(ex.Message, "Kasse-3896");
            }
            try
            {
                if (dsp != null)
                {
                    // ISSUE: reference to a compiler-generated method
                    dsp.ClearText();
                    // ISSUE: reference to a compiler-generated method
                    dsp.DisplayTextAt(0, 0, p, 0);
                }
                else
                {
                    if (!serialPortKD2.IsOpen)
                        return;
                    if (Program.display != "torex")
                    {
                        if (Program.kasano == 3 && Program.IsletmeAyarlar["kod"] == "20")
                        {
                            serialPortKD2.Write(27.ToString() + (object)'@');
                            serialPortKD2.Write(string.Concat((object)'\v'));
                            serialPortKD2.Write(p);
                        }
                        else
                        {
                            serialPortKD2.Write(27.ToString() + (object)'[' + (object)'2' + (object)'J');
                            serialPortKD2.Write(27.ToString() + (object)'[' + (object)'1' + (object)';' + (object)'1' + (object)'H');
                            serialPortKD2.Write(p);
                        }
                    }
                    else
                    {
                        serialPortKD2.Write(string.Concat((object)'\f'));
                        serialPortKD2.Write(27.ToString() + (object)'Q' + p + (object)'\r');
                    }
                }
            }
            catch (Exception ex)
            {
                logEntry.AddtoLogFile(ex.Message, "Kasse-3938");
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = ex.Message;
                int num2 = (int)fGenericError.ShowDialog();
            }
        }

        public void KDikinciSatiraYaz(string p)
        {
            try
            {
                if (dsp != null)
                {
                    // ISSUE: reference to a compiler-generated method
                    dsp.DisplayTextAt(1, 0, p, 0);
                }
                else
                {
                    if (!serialPortKD2.IsOpen)
                        return;
                    if (Program.display != "torex")
                    {
                        if (Program.kasano == 3 && Program.IsletmeAyarlar["kod"] == "20")
                        {
                            serialPortKD2.Write(11.ToString() + (object)'\n');
                            serialPortKD2.Write(p);
                        }
                        else
                        {
                            serialPortKD2.Write(27.ToString() + (object)'[' + (object)'2' + (object)';' + (object)'1' + (object)'H');
                            serialPortKD2.Write(p);
                        }
                    }
                    else
                        serialPortKD2.Write(27.ToString() + (object)'Q' + p + (object)'\r');
                }
            }
            catch (Exception ex)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = ex.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }
        private void ObstGemDelegate(string plu)
        {
            gelenBarkod = plu;
            barkodluUrunEkle();
        }
        private void DirekAuflade(string Tel, int sira)
        {
            DSPINFO(Tel, "", "", "", "", 11, 0, 0, sira);
        }
        private void barkodluUrunEkle()
        {
            bool flag = false;
            int result = 0, KolliPfand = 0;
            string Kolibarcode = "", Kolliartikelname = "";
            double KolliPreis = 0, Kolliinhaltmenge = 0;
            /*if (txtGiris.Text != "")
            {
                
                    if (txtGiris.Text.IndexOf('X') != -1)
                    {
                        txtGiris.Text.IndexOf('X');
                        if (!int.TryParse(txtGiris.Text.Replace("X", ""), out result))
                            result = 1;
                    }
                    else
                    {
                        result = 1;
                    }
                
                SatilanAdet = (double)result;
            }
            else
                SatilanAdet = 1.0;*/
            if (gelenBarkod != "")
            {
                if (PLUSatilanAdet != 0)
                {
                    SatilanAdet = PLUSatilanAdet;
                }
                else if (gelenBarkod.IndexOf('X') != -1)
                {
                    int length = gelenBarkod.IndexOf('X');
                    if (double.TryParse(gelenBarkod.Substring(0, length), out SatilanAdet))
                        gelenBarkod = gelenBarkod.Substring(length + 1, gelenBarkod.Length - (length + 1));
                    else
                        SatilanAdet = 1.0;
                }
                else if (SatilanAdet == 0)
                {
                    SatilanAdet = 1;
                }
            }
            else
                result = 1;
            A:
            iss_Artikel.Artikel artikel1 = new iss_Artikel.Artikel();
            artikel1.ArtikelBul(gelenBarkod);
            if (artikel1.urunvarmi)
            {
                if (PreisCheck == true)
                {
                    F_PreisCheck prsChk = new F_PreisCheck();
                    prsChk.Preis = artikel1.VkPreis.ToString("C");
                    prsChk.ArtikelName = artikel1.ArtikelAd;
                    prsChk.ShowDialog();
                    PreisCheck = false;
                    return;
                }
                if (KoliKiste == true)//Kolli KIste secilmisse
                {

                    MySqlConnection mySqlConnection = new MySqlConnection();
                    MySqlConnection connection = new db().myconn();
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    //SELECT `id`, `artikelbarcode`, `artikelname`, `preis`, `pfandid`, `pfandname`, `inhaltmenge` FROM `kistekoli` WHERE 1
                    string SQL = "SELECT * FROM `kistekoli`  WHERE artikelbarcode='" + artikel1.BarkodNo + "'";
                    MySqlDataAdapter daKolli = new MySqlDataAdapter(SQL, connection);
                    DataTable dtKoli = new DataTable();
                    dtKoli.Rows.Clear();
                    daKolli.Fill(dtKoli);
                    if (dtKoli.Rows.Count > 1)
                    {
                        try
                        {
                            /* _id = id;
                             _artikelbarcode = artikelbarcode;
                             _pfandName = pfandName;
                             _preis = preis;
                             _kolliINhalt = kolliINhalt;
                             _pfandID = pfandID;
                             _name = name;*/
                            List<KolliKiste> KistenList = new List<KolliKiste>();
                            for (int a = 0; a < dtKoli.Rows.Count; a++)
                            {
                                KolliKiste kolliKisteClass = new KolliKiste();
                                kolliKisteClass.Id = Convert.ToInt32(dtKoli.Rows[a].ItemArray[0]);
                                kolliKisteClass.Artikelbarcode = dtKoli.Rows[a].ItemArray[1].ToString();
                                kolliKisteClass.PfandName = dtKoli.Rows[a].ItemArray[5].ToString();
                                kolliKisteClass.Preis = Convert.ToDouble(dtKoli.Rows[a].ItemArray[3]);
                                kolliKisteClass.KolliINhalt = Convert.ToDouble(dtKoli.Rows[a].ItemArray[6]);
                                kolliKisteClass.PfandID = Convert.ToInt32(dtKoli.Rows[a].ItemArray[4]);
                                kolliKisteClass.Name = dtKoli.Rows[a].ItemArray[2].ToString();
                                KistenList.Add(kolliKisteClass);
                            }
                            F_KolliAuswahl koliForm = new F_KolliAuswahl();
                            koliForm.kolliList = KistenList;
                            koliForm.ShowDialog();
                            if (koliForm.secilenkiste != null)
                            {
                                Kolibarcode = koliForm.secilenkiste.Artikelbarcode;
                                KolliPreis = koliForm.secilenkiste.Preis;
                                Kolliartikelname = koliForm.secilenkiste.Name;
                                Kolliinhaltmenge = koliForm.secilenkiste.KolliINhalt;
                                KolliPfand = koliForm.secilenkiste.PfandID;
                                if (KolliPfand != 0)
                                {
                                    artikel1.Fand = KolliPfand;
                                }
                            }
                            else
                            {
                                KoliKiste = false;
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = "Sie haben keinen Preis für dieses Produkt!";
                                int num = (int)fGenericError.ShowDialog();
                                return;
                            }
                        }

                        catch (Exception ff)
                        {

                            MessageBox.Show("barkodluUrunEkle()" + ff.Message);
                        }
                    }
                    else if (dtKoli.Rows.Count == 1)
                    {
                        Kolibarcode = artikel1.BarkodNo;
                        KolliPreis = Convert.ToDouble(dtKoli.Rows[0].ItemArray[3]);
                        Kolliartikelname = dtKoli.Rows[0].ItemArray[2].ToString();
                        Kolliinhaltmenge = Convert.ToDouble(dtKoli.Rows[0].ItemArray[6]);
                        KolliPfand = Convert.ToInt32(dtKoli.Rows[0].ItemArray[4]);
                        if (KolliPfand != 0)
                        {
                            artikel1.Fand = KolliPfand;
                        }


                    }
                    else
                    {
                        KoliKiste = false;
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Sie haben keinen Preis für dieses Produkt!";
                        int num = (int)fGenericError.ShowDialog();
                        return;

                    }





                }
            }
            if (Program.IsletmeAyarlar["markt"] == "2")
            {
                // if (artikel1.Gruptur == 1)
                // {
                if (yeniFis != null)
                {
                    verkauferList[Program.bedID] = yeniFis;
                }
                else
                {
                    yeniFis = new FisOlustur();
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                    lblParaUstu.Text = "";
                    verkauferList[Program.bedID] = yeniFis;
                }



                // }

            }
            else if (yeniFis == null)
            {
                yeniFis = new FisOlustur();
                yeniFis.FisYarat(0);
                position = 1;
                listView1.Items.Clear();
                lblParaUstu.Text = "";
            }
            Tarih tarih = new Tarih();

            if (artikel1.urunvarmi)
            {
                if (artikel1.Gruptur == 3)
                {
                    WaagePLUTaste(artikel1);
                    return;
                }

                if ((artikel1.Grubid == 31 || artikel1.Grubid == 55 || artikel1.Grubid == 62) && Program.IsletmeAyarlar["kod"] == "20")
                {
                    int num1 = (int)new F_AchtungAlkohol().ShowDialog();
                }
                if ((artikel1.Grubid == 82 || artikel1.Grubid == 75 || (artikel1.Grubid == 87 || artikel1.Grubid == 88) || artikel1.Grubid == 72) && Program.IsletmeAyarlar["kod"] == "21")
                {
                    int num2 = (int)new F_AchtungAlkohol().ShowDialog();
                }
                if (!flag)
                {
                    bool baskaUrunVar = false;
                    if (artikel1.StafellpreisFlag == 1)
                    {
                        try
                        {

                            var matchingvalues = yeniFis.SatisKalem.FirstOrDefault(s => (s.Barkod == artikel1.BarkodNo) && (s.staffelOK == 0));
                            if (matchingvalues != null)
                            {
                                bool angebotuAl = false;

                                if (artikel1.StafelInfo.Count > 1) //birden fazla angebot varsa
                                {
                                B:
                                    if (yeniFis.angebotKarar.Keys.Contains(artikel1.BarkodNo))
                                    {
                                        if (matchingvalues.Adet + SatilanAdet >= artikel1.StafelInfo[0].Menge)
                                        {
                                            angebotuAl = true;
                                        }
                                    }
                                    else
                                    {
                                        F_MultiStafelSelect multiStafel = new F_MultiStafelSelect();
                                        multiStafel.StafelInfo = artikel1.StafelInfo;
                                        multiStafel.ShowDialog();
                                        if (multiStafel.AngeboiID != 0)
                                        {

                                            var arananAngebot = artikel1.StafelInfo.FirstOrDefault(s => (s.AngebotId == multiStafel.AngeboiID));
                                            if (arananAngebot != null)
                                            {
                                                yeniFis.angebotKarar[artikel1.BarkodNo] = arananAngebot.Menge;
                                                goto B;
                                            }
                                        }
                                    }

                                }
                                if (angebotuAl == false)
                                {

                                    if (matchingvalues.Adet + SatilanAdet >= artikel1.StafelInfo[0].Menge)
                                    {
                                        matchingvalues.Adet = matchingvalues.Adet + SatilanAdet;
                                        matchingvalues.Toplamtutar = Math.Round(matchingvalues.Satisfiyat * matchingvalues.Adet, 2);
                                        //yeniFis.SatisKalem.Remove(matchingvalues);
                                        int bolum = 0, kalan = 0;
                                        kalan = Convert.ToInt16(matchingvalues.Adet) % artikel1.StafelInfo[0].Menge;
                                        bolum = (Convert.ToInt16(matchingvalues.Adet) - kalan) / artikel1.StafelInfo[0].Menge;


                                        // var item = this.listView1.Items.Cast<ListViewItem>()
                                        //.Where(x => (x.Tag.ToString() == matchingvalues.Barkod))
                                        // .FirstOrDefault();
                                        var item = listView1.Items.Cast<ListViewItem>().LastOrDefault(x => (x.Tag.ToString() == matchingvalues.Barkod));
                                        item.SubItems[1].Text = (artikel1.StafelInfo[0].AngebotName.ToString() + "(" + (object)bolum + "Stk. x" + artikel1.StafelInfo[0].Preis.ToString("C") + ")");
                                        item.SubItems[2].Text = (artikel1.StafelInfo[0].Preis * bolum).ToString("C");

                                        matchingvalues.UrunAd = artikel1.StafelInfo[0].AngebotName.ToString();
                                        matchingvalues.Adet = bolum;
                                        matchingvalues.Toplamtutar = Math.Round((artikel1.StafelInfo[0].Preis * bolum), 2);
                                        matchingvalues.Satisfiyat = artikel1.StafelInfo[0].Preis;
                                        matchingvalues.Barkod = matchingvalues.Barkod;
                                        matchingvalues.staffelOK = 1;

                                        item.Tag = "0";
                                        DSPINFO(matchingvalues.UrunAd, SatilanAdet.ToString() + " Stk.", matchingvalues.Satisfiyat.ToString("C") + "/Stk.", Math.Round(matchingvalues.Satisfiyat * SatilanAdet, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 3, 0, 0, 0);
                                        baskaUrunVar = true;

                                        txtGiris.Text = "";

                                        yeniFis.FisiKapat();
                                        foreach (SatisYap satis in yeniFis.SatisKalem)
                                        {
                                            if (satis.Gruptur == 3)
                                            {
                                                DSPINFO(satis.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satis.Satisfiyat.ToString("C") + "/kg.", satis.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                            }
                                            else
                                            {
                                                DSPINFO(satis.UrunAd, SatilanAdet.ToString() + " Stk.", satis.Satisfiyat.ToString("C") + "/Stk.", Math.Round(satis.Toplamtutar, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                            }
                                        }
                                        if (listView1.Items.Count > 0)
                                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                        //DSPINFO(matchingvalues.UrunAd, SatilanAdet.ToString() + " Stk.", matchingvalues.Satisfiyat.ToString("C") + "/Stk.", Math.Round(matchingvalues.Satisfiyat * SatilanAdet, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        baskaUrunVar = true;
                                        if (kalan > 0)
                                        {
                                            SatilanAdet = kalan;
                                            goto A;

                                        }
                                    }
                                    else //sayi yeterli degilse
                                    {
                                        matchingvalues.Adet = matchingvalues.Adet + SatilanAdet;
                                        matchingvalues.Toplamtutar = Math.Round(matchingvalues.Satisfiyat * matchingvalues.Adet, 2);
                                        //yeniFis.SatisKalem.Remove(matchingvalues);

                                        var item = this.listView1.Items.Cast<ListViewItem>()
                            .Where(x => (x.Tag.ToString() == matchingvalues.Barkod))
                            .LastOrDefault();
                                        item.SubItems[1].Text = (matchingvalues.UrunAd.ToString() + "(" + (object)matchingvalues.Adet + "Stk. x" + matchingvalues.Satisfiyat.ToString("C") + ")");
                                        item.SubItems[2].Text = matchingvalues.Toplamtutar.ToString("C");
                                        item.Tag = matchingvalues.Barkod;
                                        if (listView1.Items.Count > 0)
                                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                        txtGiris.Text = "";
                                        yeniFis.FisiKapat();
                                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                        DSPINFO(matchingvalues.UrunAd, SatilanAdet.ToString() + " Stk.", matchingvalues.Satisfiyat.ToString("C") + "/Stk.", Math.Round(matchingvalues.Satisfiyat * SatilanAdet, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        baskaUrunVar = true;
                                    }
                                }
                                else //angebotu al 
                                {
                                    try
                                    {
                                        if (matchingvalues.Adet + SatilanAdet >= yeniFis.angebotKarar[artikel1.BarkodNo])
                                        {
                                            matchingvalues.Adet = matchingvalues.Adet + SatilanAdet;
                                            matchingvalues.Toplamtutar = Math.Round(matchingvalues.Satisfiyat * matchingvalues.Adet, 2);
                                            //yeniFis.SatisKalem.Remove(matchingvalues);
                                            int bolum = 0, kalan = 0;
                                            kalan = Convert.ToInt16(matchingvalues.Adet) % yeniFis.angebotKarar[artikel1.BarkodNo];
                                            bolum = (Convert.ToInt16(matchingvalues.Adet) - kalan) / yeniFis.angebotKarar[artikel1.BarkodNo];


                                            // var item = this.listView1.Items.Cast<ListViewItem>()
                                            //.Where(x => (x.Tag.ToString() == matchingvalues.Barkod))
                                            // .FirstOrDefault();
                                            var arananAngebot = artikel1.StafelInfo.FirstOrDefault(s => (s.StammBarcode == artikel1.BarkodNo && s.Menge == yeniFis.angebotKarar[artikel1.BarkodNo]));
                                            var item = listView1.Items.Cast<ListViewItem>().FirstOrDefault(x => (x.Tag.ToString() == matchingvalues.Barkod));
                                            item.SubItems[1].Text = (arananAngebot.AngebotName.ToString() + "(" + (object)bolum + "Stk. x" + arananAngebot.Preis.ToString("C") + ")");
                                            item.SubItems[2].Text = (arananAngebot.Preis * bolum).ToString("C");

                                            matchingvalues.UrunAd = arananAngebot.AngebotName.ToString();
                                            matchingvalues.Adet = bolum;
                                            matchingvalues.Toplamtutar = Math.Round((arananAngebot.Preis * bolum), 2);
                                            matchingvalues.Satisfiyat = arananAngebot.Preis;
                                            //matchingvalues.Barkod = "0";
                                            matchingvalues.staffelOK = 1;

                                            item.Tag = "0";

                                            txtGiris.Text = "";

                                            yeniFis.FisiKapat();
                                            foreach (SatisYap satis in yeniFis.SatisKalem)
                                            {
                                                if (satis.Gruptur == 3)
                                                {
                                                    DSPINFO(satis.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satis.Satisfiyat.ToString("C") + "/kg.", satis.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                }
                                                else
                                                {
                                                    DSPINFO(satis.UrunAd, SatilanAdet.ToString() + " Stk.", satis.Satisfiyat.ToString("C") + "/Stk.", Math.Round(satis.Toplamtutar, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                }
                                            }
                                            if (listView1.Items.Count > 0)
                                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                            // DSPINFO(matchingvalues.UrunAd, SatilanAdet.ToString() + " Stk.", matchingvalues.Satisfiyat.ToString("C") + "/Stk.", Math.Round(matchingvalues.Satisfiyat * SatilanAdet, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                            baskaUrunVar = true;
                                            if (kalan > 0)
                                            {
                                                SatilanAdet = kalan;
                                                goto A;

                                            }
                                        }
                                        else //sayi yeterli degilse
                                        {
                                            matchingvalues.Adet = matchingvalues.Adet + SatilanAdet;
                                            matchingvalues.Toplamtutar = Math.Round(matchingvalues.Satisfiyat * matchingvalues.Adet, 2);
                                            //yeniFis.SatisKalem.Remove(matchingvalues);

                                            var item = this.listView1.Items.Cast<ListViewItem>()
                                .Where(x => (x.Tag.ToString() == matchingvalues.Barkod))
                                .FirstOrDefault();
                                            item.SubItems[1].Text = (matchingvalues.UrunAd.ToString() + "(" + (object)matchingvalues.Adet + "Stk. x" + matchingvalues.Satisfiyat.ToString("C") + ")");
                                            item.SubItems[2].Text = matchingvalues.Toplamtutar.ToString("C");
                                            item.Tag = matchingvalues.Barkod;
                                            if (listView1.Items.Count > 0)
                                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                            txtGiris.Text = "";
                                            yeniFis.FisiKapat();
                                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                            DSPINFO(matchingvalues.UrunAd, SatilanAdet.ToString() + " Stk.", matchingvalues.Satisfiyat.ToString("C") + "/Stk.", Math.Round(matchingvalues.Satisfiyat * SatilanAdet, 2).ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                            baskaUrunVar = true;
                                        }
                                    }
                                    catch (Exception gg)
                                    {
                                        MessageBox.Show("barkodluUrunEkle()" + gg.Message);
                                    }
                                }

                            }
                            else //baska urun yok ama urunde satfelpreis var
                            {
                                int index = -1;
                                if (artikel1.StafelInfo.Count > 1) //birden fazla angebot varsa
                                {
                                B:
                                    if (yeniFis.angebotKarar.Keys.Contains(artikel1.BarkodNo))
                                    {

                                        var arananAngebot = artikel1.StafelInfo.FirstOrDefault(s => (s.StammBarcode == artikel1.BarkodNo && s.Menge == yeniFis.angebotKarar[artikel1.BarkodNo]));
                                        if (arananAngebot != null)
                                        {

                                            index = artikel1.StafelInfo.IndexOf(arananAngebot);

                                        }

                                    }
                                    else
                                    {
                                        F_MultiStafelSelect multiStafel = new F_MultiStafelSelect();
                                        multiStafel.StafelInfo = artikel1.StafelInfo;
                                        multiStafel.ShowDialog();
                                        if (multiStafel.AngeboiID != 0)
                                        {

                                            var arananAngebot = artikel1.StafelInfo.FirstOrDefault(s => (s.AngebotId == multiStafel.AngeboiID));
                                            if (arananAngebot != null)
                                            {
                                                yeniFis.angebotKarar[artikel1.BarkodNo] = arananAngebot.Menge;
                                                index = artikel1.StafelInfo.IndexOf(arananAngebot);

                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    index = 0;
                                }

                                if (SatilanAdet >= artikel1.StafelInfo[index].Menge)
                                {

                                    //yeniFis.SatisKalem.Remove(matchingvalues);
                                    int bolum = 0, kalan = 0;
                                    kalan = Convert.ToInt16(SatilanAdet) % artikel1.StafelInfo[index].Menge;
                                    bolum = (Convert.ToInt16(SatilanAdet) - kalan) / artikel1.StafelInfo[index].Menge;


                                    // var item = this.listView1.Items.Cast<ListViewItem>()
                                    //.Where(x => (x.Tag.ToString() == matchingvalues.Barkod))
                                    // .FirstOrDefault();
                                    //var item = listView1.Items.Cast<ListViewItem>().FirstOrDefault(x => (x.Tag.ToString() == matchingvalues.Barkod));
                                    //item.SubItems[1].Text = (artikel1.StafelInfo[0].AngebotName.ToString() + "(" + (object)bolum + "Stk. x" + artikel1.StafelInfo[0].Preis.ToString("C") + ")");
                                    //item.SubItems[2].Text = (artikel1.StafelInfo[0].Preis * bolum).ToString("C");
                                    //+++ Satis Pozisyonu ekle
                                    satisYap = new SatisYap();
                                    satisYap.staffelOK = 1;
                                    satisYap.Adet = bolum;
                                    //satisYap.Fisno = yeniFis.SatisAnaId;

                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = artikel1.Mwst;
                                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Barkod = "0";
                                    if (yeniFis.Musterino != 0)
                                    {
                                        if (yeniFis.Musteri.Method == 5)
                                        {
                                            double KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, artikel1.BarkodNo);
                                            if (KundenPreis != 0)
                                            {
                                                satisYap.ProzisyonRabatBetrag = (artikel1.VkPreis - KundenPreis) * satisYap.Adet;
                                                satisYap.Satisfiyat = KundenPreis;
                                                angebotSembol = "#";
                                            }
                                            else
                                            {
                                                angebotSembol += "";
                                                satisYap.Satisfiyat = artikel1.VkPreis;
                                            }

                                        }
                                        else
                                        {
                                            angebotSembol += "";
                                            satisYap.Satisfiyat = artikel1.StafelInfo[index].Preis;
                                        }
                                    }
                                    else
                                    {
                                        angebotSembol += "";
                                        satisYap.Satisfiyat = artikel1.StafelInfo[index].Preis;
                                    }






                                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                    satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * bolum, 2);
                                    if (satisYap.Toplamtutar == 0.0)
                                    {
                                        Console.Beep(400, 200);
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = Program.lang["67"];
                                        int num3 = (int)fGenericError.ShowDialog();
                                        return;
                                    }
                                    if (satisYap.Toplamtutar >= 50.0)
                                    {
                                        F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                                        summeBesteatigung.summe = satisYap.Toplamtutar;
                                        int num3 = (int)summeBesteatigung.ShowDialog();
                                        if (summeBesteatigung.bestatigung != 1)
                                        {
                                            satisYap = (SatisYap)null;
                                            return;
                                        }
                                    }
                                    satisYap.UrunId = 0;
                                    satisYap.UrunAd = angebotSembol + artikel1.StafelInfo[index].AngebotName.ToString();
                                    satisYap.Birimkar = (satisYap.Satisfiyat - artikel1.EkPreis) * SatilanAdet;
                                    satisYap.Grubid = artikel1.Grubid;
                                    satisYap.Fand = artikel1.Fand;
                                    satisYap.Fand2 = artikel1.Fand2;
                                    satisYap.Angebotvarmi = artikel1.AngebotVarmi;
                                    satisYap.Angebotfiyat = artikel1.AngebotFiyati;
                                    satisYap.Gruptur = artikel1.Gruptur;
                                    if (artikel1.AngebotVarmi == 0 && artikel1.Punkterabatdurum != 1)
                                    {
                                        satisYap.Angebotvarmi = 1;
                                        angebotSembol = "*";
                                    }
                                    //
                                    if (Program.IsletmeAyarlar["markt"] == "2")
                                    {

                                        F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                                        FUserAuswahl.satilanPozition = satisYap;
                                        FUserAuswahl.UserList = UserList;
                                        FUserAuswahl.ShowDialog();
                                        if (FUserAuswahl.SecilenUser != -1)
                                        {
                                            FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                                            toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                                            yeniFis = null;
                                            if (ElemanFisi == null)
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                                                KundenDisplay();
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                            }
                                            else
                                            {
                                                FisOlustur yeniFis1 = new FisOlustur();
                                                yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                                yeniFis = ElemanFisi;
                                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                                                satisYap.Fisno = yeniFis.SatisAnaId;
                                                listView1.Items.Clear();
                                                KundenDisplay();
                                                int num2 = 1;
                                                int count = listView1.Items.Count;
                                                foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                                                {

                                                    listView1.Items.Add(num2.ToString());
                                                    listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                                    listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                                    DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                                    ++num2;
                                                    count++;
                                                }
                                                position = num2;
                                                yeniFis.FisiKapat();


                                            }
                                            if (yeniFis.Musterino != 0)
                                            {
                                                if (artikel1.KundenPreisFlag != 0)
                                                {
                                                    double KundenPreis = yeniFis.Musteri.GetKundenPreis((Int32)artikel1.ArtikelId, yeniFis.Musteri.MusteriId);
                                                    if (KundenPreis > 0)
                                                    {
                                                        angebotSembol += "#";
                                                        satisYap.Satisfiyat = KundenPreis;
                                                    }
                                                    else
                                                    {
                                                        satisYap.Satisfiyat = artikel1.StafelInfo[index].Preis;
                                                    }

                                                }
                                                else
                                                {
                                                    angebotSembol = "";
                                                    satisYap.Satisfiyat = artikel1.StafelInfo[index].Preis;
                                                }
                                            }
                                            else if (artikel1.VkPreis2 > 0)
                                            {
                                                F_Preisauswahl preislist = new F_Preisauswahl();
                                                preislist.Preis1 = Convert.ToDouble(artikel1.VkPreis);
                                                preislist.Preis2 = Convert.ToDouble(artikel1.VkPreis2);
                                                preislist.Preis3 = Convert.ToDouble(artikel1.VkPreis3);

                                                preislist.ShowDialog();
                                                satisYap.Satisfiyat = preislist.seilenPreis;
                                            }
                                            else
                                            {
                                                angebotSembol += "";
                                                satisYap.Satisfiyat = artikel1.VkPreis;
                                            }
                                            yeniFis.SatisKalem.Add(satisYap);
                                            verkauferList[Program.bedID] = yeniFis;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else //Markt =1 ise
                                    {
                                        if (yeniFis.Musterino != 0)
                                        {

                                            if (yeniFis.Musteri.Method == 5)
                                            {
                                                double KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, artikel1.BarkodNo);
                                                if (KundenPreis != 0)
                                                {
                                                    satisYap.ProzisyonRabatBetrag = (artikel1.StafelInfo[index].Preis - KundenPreis) * satisYap.Adet;
                                                    satisYap.Satisfiyat = KundenPreis;
                                                    angebotSembol = "#";
                                                }
                                            }
                                            else
                                            {
                                                angebotSembol = "";
                                                satisYap.Satisfiyat = artikel1.VkPreis;
                                            }
                                        }
                                        satisYap.Fisno = yeniFis.SatisAnaId;
                                        yeniFis.SatisKalem.Add(satisYap);
                                        LastPos = satisYap;
                                    }
                                    int count1 = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    listView1.Items[count1].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)bolum + "Stk. x" + satisYap.Satisfiyat.ToString("C") + ")");
                                    listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[count1].Tag = "0";
                                    if (listView1.Items.Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    txtGiris.Text = "";
                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    ++position;
                                    angebotSembol = "";
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    if (artikel1.Fand != 0)
                                    {
                                        Artikel artikel2 = new Artikel();
                                        artikel2.ArtikelBul(artikel1.Fand.ToString());
                                        if (artikel2.urunvarmi)
                                        {
                                            satisYap = new SatisYap();
                                            satisYap.Adet = SatilanAdet;
                                            satisYap.Fisno = yeniFis.SatisAnaId;
                                            satisYap.Barkod = artikel2.BarkodNo;
                                            satisYap.KasaNo = Program.kasano;
                                            satisYap.Mwst = artikel2.Mwst;
                                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                            satisYap.Satisfiyat = artikel2.VkPreis;
                                            if (artikel2.VkPreis == 0)
                                            {
                                                F_GenericError fGenericError = new F_GenericError();
                                                fGenericError.lblMesaj.Text = "VORSICHT!!!!\nPfandwert ist NULL!!";
                                                fGenericError.ShowDialog();
                                                return;
                                            }
                                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                            satisYap.Toplamtutar = Math.Round(artikel2.VkPreis * SatilanAdet, 2);
                                            satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                            satisYap.UrunAd = artikel2.ArtikelAd;
                                            satisYap.Birimkar = (artikel2.VkPreis - artikel2.EkPreis) * SatilanAdet;
                                            satisYap.Grubid = artikel2.Grubid;
                                            satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                                            satisYap.Fand = 1;
                                            yeniFis.SatisKalem.Add(satisYap);
                                            int count2 = listView1.Items.Count;
                                            listView1.Items.Add("");
                                            listView1.Items[count2].SubItems.Add(artikel2.ArtikelAd.ToString() + "(" + (object)SatilanAdet + "x" + artikel2.VkPreis.ToString("C") + ")");
                                            listView1.Items[count2].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                            txtGiris.Invoke((Action)delegate () { txtGiris.Text = ""; });
                                            txtGiris.Text = "";
                                            yeniFis.FisiKapat();
                                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                        }
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    }

                                    //++
                                    baskaUrunVar = true;
                                    if (kalan > 0)
                                    {
                                        SatilanAdet = kalan;
                                        goto A;

                                    }
                                }
                                else
                                {
                                    baskaUrunVar = false;
                                }
                            }
                        }
                        catch (Exception ff)
                        {
                            MessageBox.Show("barkodluUrunEkle()" + ff.Message);

                        }
                    }
                    if (baskaUrunVar == false)
                    {
                        satisYap = new SatisYap();
                        satisYap.Adet = SatilanAdet;
                        //satisYap.Fisno = yeniFis.SatisAnaId;

                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = artikel1.Mwst;
                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Barkod = artikel1.BarkodNo;
                        if (artikel1.AngebotVarmi != 0)
                        {
                            if (artikel1.Punkterabatdurum != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                                satisYap.Satisfiyat = artikel1.VkPreis;
                            }
                            if (artikel1.AngebotBaslamaTarihi <= (double)tarih.unixdate(DateTime.Now) && artikel1.AngebotBitistarihi >= (double)tarih.unixdate(DateTime.Now))
                            {
                                satisYap.Satisfiyat = artikel1.AngebotFiyati;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Satisfiyat = artikel1.VkPreis;
                                angebotSembol = "";
                            }
                        }
                        else if (artikel1.VkPreis2 > 0)
                        {

                            F_Preisauswahl preislist = new F_Preisauswahl();
                            preislist.Preis1 = Convert.ToDouble(artikel1.VkPreis);
                            preislist.Preis2 = Convert.ToDouble(artikel1.VkPreis2);
                            preislist.Preis3 = Convert.ToDouble(artikel1.VkPreis3);

                            preislist.ShowDialog();
                            satisYap.Satisfiyat = preislist.seilenPreis;
                        }
                        else if (yeniFis.Musterino != 0)
                        {
                            if (yeniFis.Musteri.Method == 5)
                            {
                                double KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, artikel1.BarkodNo);
                                if (KundenPreis != 0)
                                {
                                    satisYap.ProzisyonRabatBetrag = (artikel1.VkPreis - KundenPreis) * satisYap.Adet;
                                    satisYap.Satisfiyat = KundenPreis;
                                    angebotSembol = "#";
                                }
                                else
                                {
                                    angebotSembol += "";
                                    satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis); ;
                                }

                            }
                            else
                            {
                                angebotSembol += "";
                                satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis); ;
                            }
                        }
                        else
                        {
                            angebotSembol += "";
                            satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis); ;
                        }






                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                        satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * SatilanAdet, 2);
                        if (satisYap.Toplamtutar == 0.0)
                        {
                            Console.Beep(400, 200);
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["67"];
                            int num3 = (int)fGenericError.ShowDialog();
                            return;
                        }
                        if (satisYap.Toplamtutar >= 50.0)
                        {
                            F_GrossSummeBesteatigung summeBesteatigung = new F_GrossSummeBesteatigung();
                            summeBesteatigung.summe = satisYap.Toplamtutar;
                            int num3 = (int)summeBesteatigung.ShowDialog();
                            if (summeBesteatigung.bestatigung != 1)
                            {
                                satisYap = (SatisYap)null;
                                return;
                            }
                        }
                        satisYap.IsKolli = (KoliKiste == true ? true : false);
                        satisYap.KolliKistaInhalt = (KoliKiste == true ? Kolliinhaltmenge : 0);
                        satisYap.UrunId = (Decimal)artikel1.ArtikelId;
                        satisYap.UrunAd = (KoliKiste == true ? Kolliartikelname : (angebotSembol + artikel1.ArtikelAd));
                        satisYap.Birimkar = (satisYap.Satisfiyat - artikel1.EkPreis) * SatilanAdet;
                        satisYap.Grubid = artikel1.Grubid;
                        satisYap.Fand = artikel1.Fand;
                        satisYap.Fand2 = artikel1.Fand2;
                        satisYap.Angebotvarmi = artikel1.AngebotVarmi;
                        satisYap.Angebotfiyat = artikel1.AngebotFiyati;
                        satisYap.Gruptur = artikel1.Gruptur;
                        if (artikel1.AngebotVarmi == 0 && artikel1.Punkterabatdurum != 1)
                        {
                            satisYap.Angebotvarmi = 1;
                            angebotSembol = "*";
                        }
                        //
                        if (Program.IsletmeAyarlar["markt"] == "2")
                        {

                            F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                            FUserAuswahl.satilanPozition = satisYap;
                            FUserAuswahl.UserList = UserList;
                            FUserAuswahl.ShowDialog();
                            if (FUserAuswahl.SecilenUser != -1)
                            {
                                FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];
                                toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser];
                                yeniFis = null;
                                if (ElemanFisi == null)
                                {
                                    FisOlustur yeniFis1 = new FisOlustur();
                                    yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                    yeniFis.kasiyerno = Convert.ToInt16(FUserAuswahl.SecilenUser);
                                    toolStripStatusLabel1.Text = Program.lang["13"] + " :" + yeniFis.kasiyerno;
                                    KundenDisplay();
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                }
                                else
                                {
                                    FisOlustur yeniFis1 = new FisOlustur();
                                    yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                                    yeniFis = ElemanFisi;
                                    toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    listView1.Items.Clear();
                                    KundenDisplay();
                                    int num2 = 1;
                                    int count = listView1.Items.Count;
                                    foreach (SatisYap satisYapEski in yeniFis.SatisKalem)
                                    {

                                        listView1.Items.Add(num2.ToString());
                                        listView1.Items[count].SubItems.Add(satisYapEski.UrunAd.ToString() + "(" + (satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.") + "x" + (satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.") + ")");
                                        listView1.Items[count].SubItems.Add(satisYapEski.Toplamtutar.ToString("C"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                        DSPINFO(satisYapEski.UrunAd, satisYapEski.Gruptur != 3 ? satisYapEski.Adet.ToString() + " Stk." : satisYapEski.Adet.ToString() + "kg.", satisYapEski.Gruptur != 3 ? satisYapEski.Satisfiyat.ToString("C") + "/Stk." : satisYapEski.Satisfiyat.ToString("C") + "/kg.", satisYapEski.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        ++num2;
                                        count++;
                                    }
                                    position = num2;
                                    yeniFis.FisiKapat();


                                }
                                if (yeniFis.Musterino != 0)
                                {
                                    if (artikel1.KundenPreisFlag != 0)
                                    {
                                        double KundenPreis = yeniFis.Musteri.GetKundenPreis((Int32)artikel1.ArtikelId, yeniFis.Musteri.MusteriId);
                                        if (KundenPreis > 0)
                                        {
                                            angebotSembol += "#";
                                            satisYap.Satisfiyat = (KoliKiste != true ? KundenPreis : KolliPreis);
                                        }
                                        else
                                        {
                                            satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis);
                                        }

                                    }
                                    else
                                    {
                                        angebotSembol = "";
                                        satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis);
                                    }
                                }
                                else if (artikel1.VkPreis2 > 0)
                                {
                                    F_Preisauswahl preislist = new F_Preisauswahl();
                                    preislist.Preis1 = Convert.ToDouble(artikel1.VkPreis);
                                    preislist.Preis2 = Convert.ToDouble(artikel1.VkPreis2);
                                    preislist.Preis3 = Convert.ToDouble(artikel1.VkPreis3);

                                    preislist.ShowDialog();
                                    satisYap.Satisfiyat = preislist.seilenPreis;
                                }
                                else
                                {
                                    angebotSembol += "";
                                    satisYap.Satisfiyat = artikel1.VkPreis;
                                }
                                yeniFis.SatisKalem.Add(satisYap);
                                verkauferList[Program.bedID] = yeniFis;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else //Markt =1 ise
                        {
                            if (yeniFis.Musterino != 0)
                            {

                                if (yeniFis.Musteri.Method == 5)
                                {
                                    double KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, artikel1.BarkodNo);
                                    if (KundenPreis != 0)
                                    {
                                        satisYap.ProzisyonRabatBetrag = (artikel1.VkPreis - KundenPreis) * satisYap.Adet;
                                        satisYap.Satisfiyat = KundenPreis;
                                        angebotSembol = "#";
                                        satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * SatilanAdet, 2);
                                    }
                                }
                                else
                                {
                                    angebotSembol = "";
                                    satisYap.Satisfiyat = (KoliKiste != true ? artikel1.VkPreis : KolliPreis);
                                    satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * SatilanAdet, 2);
                                }
                            }
                            //satisYap.IsKolli = true;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            yeniFis.SatisKalem.Add(satisYap);
                            LastPos = satisYap;
                        }

                        //
                        /*if (artikel1.Partnerschaft == 1)
                        {
                            iliskiler = artikel1.UrunIliskileri(gelenBarkod, iliskiler);
                            if (yeniFis.SatisKalem.Count > 0)
                            {
                                foreach (Dictionary<List<string>, double> dictionary1 in iliskiler)
                                {
                                    foreach (List<string> key1 in dictionary1.Keys)
                                    {
                                        int count = key1.Count;
                                        int num3 = 0;
                                        Dictionary<string, double> source = new Dictionary<string, double>();
                                        if (key1.Contains(gelenBarkod))
                                        {
                                            foreach (string key2 in key1)
                                            {
                                                Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
                                                foreach (SatisYap satisYap1 in yeniFis.SatisKalem)
                                                {
                                                    if (satisYap1.Barkod == key2 && satisYap1.Iliskilendirme != 1)
                                                    {
                                                        List<KeyValuePair<string, double>> keyValuePairList = new List<KeyValuePair<string, double>>((IEnumerable<KeyValuePair<string, double>>)source);
                                                        if (keyValuePairList.Count > 0)
                                                        {
                                                            foreach (KeyValuePair<string, double> keyValuePair in keyValuePairList)
                                                            {
                                                                if (keyValuePair.Key == satisYap1.Barkod)
                                                                    source[keyValuePair.Key] = source[satisYap1.Barkod] + satisYap1.Adet;
                                                                else
                                                                    source.Add(key2, satisYap1.Adet);
                                                            }
                                                        }
                                                        else
                                                            source.Add(satisYap1.Barkod, satisYap1.Adet);
                                                        ++num3;
                                                        if (num3 >= count)
                                                        {
                                                            double num4 = source.Aggregate<KeyValuePair<string, double>>((Func<KeyValuePair<string, double>, KeyValuePair<string, double>, KeyValuePair<string, double>>)((l, r) =>
                                                            {
                                                                if (l.Value >= r.Value)
                                                                    return r;
                                                                return l;
                                                            })).Value;
                                                            double num5 = new List<KeyValuePair<List<string>, double>>((IEnumerable<KeyValuePair<List<string>, double>>)dictionary1)[0].Value;
                                                            int int16 = (int)Convert.ToInt16(num4);
                                                            double num6 = num5 / (double)num3;
                                                            int num7 = 0;
                                                            while (num7 < num3)
                                                            {
                                                                foreach (string str in key1)
                                                                {
                                                                    foreach (SatisYap satisYap2 in yeniFis.SatisKalem)
                                                                    {
                                                                        if (satisYap2.Barkod == str && satisYap2.Iliskilendirme != 1)
                                                                        {
                                                                            satisYap2.Iliskilendirme = 1;
                                                                            satisYap2.UrunAd = "A-" + satisYap2.UrunAd;
                                                                            double num8 = satisYap2.Adet - num4;
                                                                            double num9 = num7 != num3 - 1 ? num4 * num6 + num8 * satisYap2.Satisfiyat : num4 * (num5 - num6) + num8 * satisYap2.Satisfiyat;
                                                                            double num10 = num9 / satisYap2.Adet;
                                                                            satisYap2.Satisfiyat = num10;
                                                                            satisYap2.Toplamtutar = num9;
                                                                            ++num7;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            yeniFis.FisiKapat();
                                                            break;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }*/
                        int count1 = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count1].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)SatilanAdet + "Stk. x" + satisYap.Satisfiyat.ToString("C") + ")");
                        listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count1].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        angebotSembol = "";
                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        if (artikel1.Fand != 0)
                        {
                            Artikel artikel2 = new Artikel();
                            artikel2.ArtikelBul(artikel1.Fand.ToString());
                            if (artikel2.urunvarmi)
                            {
                                satisYap = new SatisYap();
                                satisYap.Adet = SatilanAdet;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.Barkod = artikel2.BarkodNo;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel2.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = artikel2.VkPreis;
                                if (artikel2.VkPreis == 0)
                                {
                                    F_GenericError fGenericError = new F_GenericError();
                                    fGenericError.lblMesaj.Text = "VORSICHT!!!!\nPfandwert ist NULL!!";
                                    fGenericError.ShowDialog();
                                    return;
                                }
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(artikel2.VkPreis * SatilanAdet, 2);
                                satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                satisYap.UrunAd = artikel2.ArtikelAd;
                                satisYap.Birimkar = (artikel2.VkPreis - artikel2.EkPreis) * SatilanAdet;
                                satisYap.Grubid = artikel2.Grubid;
                                satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                                satisYap.Fand = 1;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count2 = listView1.Items.Count;
                                listView1.Items.Add("");
                                listView1.Items[count2].SubItems.Add(artikel2.ArtikelAd.ToString() + "(" + (object)SatilanAdet + "x" + artikel2.VkPreis.ToString("C") + ")");
                                listView1.Items[count2].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                txtGiris.Invoke((Action)delegate () { txtGiris.Text = ""; });
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            }
                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        }
                    }
                }
                if (dsp != null)
                {
                    if (Program.displayType != "IBM")
                    {
                        KDbirinciSatiraYaz(artikel1.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString("C"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                    }
                    else
                    {
                        KDbirinciSatiraYaz(artikel1.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString() + (object)'Õ');
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                    }
                }
                else if (serialPortKD2.IsOpen)
                {
                    KDbirinciSatiraYaz(artikel1.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString("#0.00"));
                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                }
                SatilanAdet = 0.0;
                PLUSatilanAdet = 0;
                gelenBarkod = "";
                KoliKiste = false;
                PreisCheck = false;
            }
            else
            {
                UrunYok();
            }

        }

        private void listBoxBosalt()
        {
            listView1.Items.Clear();
        }

        private void btnYukari_Click(object sender, EventArgs e)
        {
            int count = listView1.Items.Count;
            seciliItem = -1;
            if (count <= 0)
                return;
            for (int index = 0; index < count; ++index)
            {
                if (listView1.Items[index].Selected)
                {
                    seciliItem = index;
                    break;
                }
            }
            if (seciliItem > 0)
            {
                listView1.Items[seciliItem - 1].Selected = true;
                --seciliItem;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
            else
            {
                listView1.Items[count - 1].Selected = true;
                seciliItem = count - 1;
                listView1.FullRowSelect = true;
                listView1.Focus();
            }
        }

        private void btnAsagi_Click(object sender, EventArgs e)
        {
            int count = listView1.Items.Count;
            seciliItem = -1;
            if (count <= 0)
                return;
            for (int index = 0; index < count; ++index)
            {
                if (listView1.Items[index].Selected)
                {
                    seciliItem = index;
                    break;
                }
            }
            if (seciliItem < count - 1)
            {
                listView1.Items[seciliItem + 1].Selected = true;
                ++seciliItem;
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
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (seciliItem == -1)
            {
                if (LastPos != null)
                {
                    if (LastPos.Gruptur == 3)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Sie dürfen nicht gewogene Artikelmenge manuel ändern! Bitte nochmal wiegen!";
                        int num = (int)fGenericError.ShowDialog();
                        return;
                    }
                    if (yeniFis != null)
                    {
                        yeniFis.SatisKalem.Add(LastPos);

                        int listViewElaman = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[listViewElaman].SubItems.Add(angebotSembol + LastPos.UrunAd.ToString() + "(" + LastPos.Adet + "x" + LastPos.Satisfiyat.ToString("C") + ")");
                        listView1.Items[listViewElaman].SubItems.Add(LastPos.Toplamtutar.ToString("C"));
                        listView1.Items[listViewElaman].Tag = LastPos.Barkod;
                        if (listView1.Items.Count > 0)
                        {
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        }
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        position++;
                        DSPINFO(LastPos.UrunAd, 1.ToString() + " Stk.", LastPos.Satisfiyat.ToString("C") + "/Stk.", LastPos.Satisfiyat.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        if (LastPos.Fand != 0)
                        {
                            Artikel artikel2 = new Artikel();
                            artikel2.ArtikelBul(LastPos.Fand.ToString());
                            if (artikel2.urunvarmi)
                            {
                                satisYap = new SatisYap();
                                satisYap.Adet = SatilanAdet;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.Barkod = artikel2.BarkodNo;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel2.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = artikel2.VkPreis;
                                if (artikel2.VkPreis == 0)
                                {
                                    F_GenericError fGenericError = new F_GenericError();
                                    fGenericError.lblMesaj.Text = "VORSICHT!!!!\nPfandwert ist NULL!!";
                                    fGenericError.ShowDialog();
                                    return;
                                }
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(artikel2.VkPreis * LastPos.Adet, 2);
                                satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                satisYap.UrunAd = artikel2.ArtikelAd;
                                satisYap.Birimkar = (artikel2.VkPreis - artikel2.EkPreis) * SatilanAdet;
                                satisYap.Grubid = artikel2.Grubid;
                                satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                                satisYap.Fand = 1;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count2 = listView1.Items.Count;
                                listView1.Items.Add("");
                                listView1.Items[count2].SubItems.Add(artikel2.ArtikelAd.ToString() + "(" + (object)SatilanAdet + "x" + artikel2.VkPreis.ToString("C") + ")");
                                listView1.Items[count2].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                txtGiris.Invoke((Action)delegate () { txtGiris.Text = ""; });
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            }
                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        }
                    }
                }
            }
            else
            {

                string text = listView1.Items[seciliItem].Text;
                if (text != "")
                {
                    if (yeniFis.SatisKalem[seciliItem].Gruptur == 3)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Sie dürfen nicht gewogene Artikelmenge manuel ändern! Bitte nochmal wiegen!";
                        fGenericError.ShowDialog();
                        return;
                    }
                    double adet = yeniFis.SatisKalem[seciliItem].Adet;
                    double num = yeniFis.SatisKalem[seciliItem].Birimkar / yeniFis.SatisKalem[seciliItem].Adet;
                    ++yeniFis.SatisKalem[seciliItem].Adet;
                    if (yeniFis.SatisKalem[seciliItem].Fand != 0)
                    {
                        ++yeniFis.SatisKalem[seciliItem + 1].Adet;
                        yeniFis.SatisKalem[seciliItem + 1].Toplamtutar = yeniFis.SatisKalem[seciliItem + 1].Adet * yeniFis.SatisKalem[seciliItem + 1].Satisfiyat;
                    }
                    else if (yeniFis.SatisKalem[seciliItem].Fand2 == 1)
                    {
                        ++yeniFis.SatisKalem[seciliItem + 1].Adet;
                        yeniFis.SatisKalem[seciliItem + 1].Toplamtutar = yeniFis.SatisKalem[seciliItem + 1].Adet * 0.15;
                    }
                    yeniFis.SatisKalem[seciliItem].Birimkar = num * yeniFis.SatisKalem[seciliItem].Adet;
                    yeniFis.SatisKalem[seciliItem].Toplamtutar = Math.Round(yeniFis.SatisKalem[seciliItem].Toplamtutar / adet * yeniFis.SatisKalem[seciliItem].Adet, 2);
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    listView1.Items[seciliItem].SubItems[0].Text = text;
                    listView1.Items[seciliItem].SubItems[1].Text = yeniFis.SatisKalem[seciliItem].UrunAd.ToString() + "(" + (object)yeniFis.SatisKalem[seciliItem].Adet + "x" + (object)yeniFis.SatisKalem[seciliItem].Satisfiyat + ")";
                    listView1.Items[seciliItem].SubItems[2].Text = yeniFis.SatisKalem[seciliItem].Toplamtutar.ToString("C");
                    if (yeniFis.SatisKalem[seciliItem].Fand != 0 || yeniFis.SatisKalem[seciliItem].Fand2 != 0)
                    {
                        listView1.Items[seciliItem + 1].SubItems[0].Text = "";
                        listView1.Items[seciliItem + 1].SubItems[1].Text = yeniFis.SatisKalem[seciliItem + 1].UrunAd.ToString() + "(" + (object)yeniFis.SatisKalem[seciliItem + 1].Adet + "x" + (object)yeniFis.SatisKalem[seciliItem + 1].Satisfiyat + ")";
                        listView1.Items[seciliItem + 1].SubItems[2].Text = yeniFis.SatisKalem[seciliItem + 1].Toplamtutar.ToString("C");
                    }
                    if (dsp != null)
                    {
                        if (Program.displayType != "IBM")
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                        else
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                    }
                    else
                    {
                        if (!(Program.displayType == "TVS"))
                            return;
                        DSPINFO(yeniFis.SatisKalem[seciliItem].UrunAd, 1.ToString() + " Stk.", yeniFis.SatisKalem[seciliItem].Satisfiyat.ToString("C") + "/Stk.", yeniFis.SatisKalem[seciliItem].Satisfiyat.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                    }
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["68"] + "\n" + Program.lang["69"];
                    int num = (int)fGenericError.ShowDialog();
                }
            }
        }

        private void Casio_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (scanner2 != null)
                {
                    serialPort1.Close();
                    // ISSUE: reference to a compiler-generated method
                    scanner.ReleaseDevice();
                    // ISSUE: reference to a compiler-generated method
                    scanner2.ReleaseDevice();
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                }

            }
            catch
            {
            }
            KundenDisplay();
            try
            {
                if (Rea != null)
                    Rea.exit();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message + ":FormClosed");
            }
            Application.Exit();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (seciliItem == -1 || yeniFis == null)
                return;
            string text = listView1.Items[seciliItem].Text;
            if (text != "")
            {
                if (yeniFis.SatisKalem[seciliItem].Gruptur == 3)
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = "Sie dürfen nicht gewogene Artikelmenge manuel ändern!!! Bitte nochmal wiegen!";
                    fGenericError.ShowDialog();
                    return;
                }
                double adet = yeniFis.SatisKalem[seciliItem].Adet;
                if (adet > 1.0)
                {

                    double num = yeniFis.SatisKalem[seciliItem].Birimkar / yeniFis.SatisKalem[seciliItem].Adet;
                    --yeniFis.SatisKalem[seciliItem].Adet;
                    if (yeniFis.SatisKalem[seciliItem].Fand != 0)
                    {
                        --yeniFis.SatisKalem[seciliItem + 1].Adet;
                        yeniFis.SatisKalem[seciliItem + 1].Toplamtutar = yeniFis.SatisKalem[seciliItem + 1].Adet * yeniFis.SatisKalem[seciliItem + 1].Satisfiyat;
                    }
                    else if (yeniFis.SatisKalem[seciliItem].Fand2 != 0)
                    {
                        --yeniFis.SatisKalem[seciliItem + 1].Adet;
                        yeniFis.SatisKalem[seciliItem + 1].Toplamtutar = yeniFis.SatisKalem[seciliItem + 1].Adet * 0.15;
                    }
                    yeniFis.SatisKalem[seciliItem].Birimkar = num * yeniFis.SatisKalem[seciliItem].Adet;
                    yeniFis.SatisKalem[seciliItem].Toplamtutar = Math.Round(yeniFis.SatisKalem[seciliItem].Toplamtutar / adet * yeniFis.SatisKalem[seciliItem].Adet, 2);
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    listView1.Items[seciliItem].SubItems[0].Text = text;
                    listView1.Items[seciliItem].SubItems[1].Text = yeniFis.SatisKalem[seciliItem].UrunAd.ToString() + "(" + (object)yeniFis.SatisKalem[seciliItem].Adet + "x" + (object)yeniFis.SatisKalem[seciliItem].Satisfiyat + ")";
                    listView1.Items[seciliItem].SubItems[2].Text = yeniFis.SatisKalem[seciliItem].Toplamtutar.ToString("C");
                    if (yeniFis.SatisKalem[seciliItem].Fand != 0 || yeniFis.SatisKalem[seciliItem].Fand2 != 0)
                    {
                        listView1.Items[seciliItem + 1].SubItems[0].Text = "";
                        listView1.Items[seciliItem + 1].SubItems[1].Text = yeniFis.SatisKalem[seciliItem + 1].UrunAd.ToString() + "(" + (object)yeniFis.SatisKalem[seciliItem].Adet + "x" + (object)yeniFis.SatisKalem[seciliItem + 1].Satisfiyat + ")";
                        listView1.Items[seciliItem + 1].SubItems[2].Text = yeniFis.SatisKalem[seciliItem + 1].Toplamtutar.ToString("C");
                    }

                    if (dsp != null)
                    {
                        if (Program.displayType != "IBM")
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                        else
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                    else
                    {
                        if (!(Program.displayType == "TVS"))
                            return;
                        //DSPINFO(yeniFis.SatisKalem[seciliItem].UrunAd, satisYap.Adet.ToString() + " Stk.", yeniFis.SatisKalem[seciliItem].Satisfiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0,0,0,0);
                        // DSPINFO(yeniFis.SatisKalem[seciliItem].UrunAd, adet.ToString() + (yeniFis.SatisKalem[seciliItem].Gruptur != 3 ? (object)" Stk." : (object)"kg."), satisYap.Toplamtutar.ToString("C") + (yeniFis.SatisKalem[seciliItem].Gruptur != 3 ? "/Stk." : "kg"), toplamtutar2.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 1, 0, 0, 0);
                        DSPINFO(yeniFis.SatisKalem[seciliItem].UrunAd, 1.ToString() + " Stk.", yeniFis.SatisKalem[seciliItem].Satisfiyat.ToString("C") + "/Stk.", yeniFis.SatisKalem[seciliItem].Satisfiyat.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 1, 0, 0, 0);
                    }
                }
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["68"] + "\n" + Program.lang["69"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                if (parkedilenFis != null)
                {
                    foreach (SatisYap pos in parkedilenFis.SatisKalem)
                    {
                        if (pos.Gruptur == 3)
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = "Sie haben in geparkter Kassenbon gewogene Pozitionen! Bitte ausdrucken Sie zuerst diese Kassenbon oder stornieren! ";
                            int num = (int)fGenericError.ShowDialog();
                            return;
                        }
                    }
                }
                F_GenericSoru fGenericSoru1 = new F_GenericSoru();
                fGenericSoru1.lblMesaj.Text = Program.lang["5"];
                int num1 = (int)fGenericSoru1.ShowDialog();
                if (!fGenericSoru1.sonuc)
                    return;
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                new MySqlCommand("UPDATE usertakip SET offlinezaman=" + (object)tarih.unixdate(DateTime.Now) + " WHERE userid=" + (object)Program.bedID + " and tarih=" + (object)tarih.bugunBaslangic() + " and  onlinezaman<>0 and offlinezaman=0 and kasaid=" + (object)Program.kasano, connection).ExecuteNonQuery();
                if (Program.GlobalAyarlar["AutoBackup"] != 0)
                {
                    int num2 = Program.GlobalAyarlar["AutoBackup"];
                    F_GenericSoru fGenericSoru2 = new F_GenericSoru();
                    fGenericSoru2.lblMesaj.Text = Program.lang["96"];
                    int num3 = (int)fGenericSoru2.ShowDialog();
                    if (fGenericSoru2.sonuc)
                        MakeLocalBackUP();
                }
                Process.Start("shutdown", "-s -f -t 0");
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void MakeLocalBackUP()
        {
            if (Program.ProgramAyarlar["localBackup"] == "1")
            {
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Monday) || (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) || (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) || (DateTime.Now.DayOfWeek == DayOfWeek.Thursday))
                {
                    try
                    {
                        if (dayBackup == false)
                        {
                            string filePath = Application.StartupPath + "\\BACKUP\\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".sql";
                            Directory.CreateDirectory(Application.StartupPath + "\\BACKUP\\");
                            string[] files = Directory.GetFiles(Application.StartupPath + "\\BACKUP\\");

                            foreach (string file in files)
                            {
                                FileInfo fi = new FileInfo(file);
                                if (fi.LastAccessTime < DateTime.Now.AddDays(-10))
                                    fi.Delete();
                                if (fi.LastAccessTime.Day == DateTime.Now.Day)
                                {
                                    dayBackup = true;
                                    break;
                                }

                            }

                            MySqlConnection mySqlConnection1 = new MySqlConnection();
                            MySqlConnection mySqlConnection2 = new db().myconn();
                            if (mySqlConnection2.State == ConnectionState.Closed)
                                mySqlConnection2.Open();

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                using (MySqlBackup mySqlBackup = new MySqlBackup(cmd))
                                {
                                    cmd.Connection = mySqlConnection2;
                                    mySqlBackup.ExportToFile(filePath);
                                    mySqlConnection2.Close();

                                }
                            }

                            Program.DailyBackup = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message + ":LocalBackup");
                    }
                }
            }
            else
            {
                Program.DailyBackup = true;
            }
        }

        private void KundenDisplay()
        {
            if (Program.displayType == "TVS")
                DSPINFO("", "", "", "", "Herzlich Willkommen!", 3, 0, 0, 0);
            else if (dsp != null)
            {
                // ISSUE: reference to a compiler-generated method
                dsp.ClearText();
                if (Program.IsletmeAyarlar["land"] == "de")
                {
                    // ISSUE: reference to a compiler-generated method
                    dsp.DisplayTextAt(0, 0, "Herzlich Willkommen!", 0);
                }
                else
                {
                    // ISSUE: reference to a compiler-generated method
                    dsp.DisplayTextAt(0, 0, "Van Harte Welkom!", 0);
                }
                // ISSUE: reference to a compiler-generated method
                dsp.DisplayTextAt(1, 0, Program.IsletmeAyarlar["isletme"], 0);
            }
            else
            {
                if (Program.displayType == "TVS")
                    knddsply.VerkaufInfo("Herzlich Willkommen!", Program.IsletmeAyarlar["isletme"]);
                if (!serialPortKD2.IsOpen)
                    return;
                if (Program.display != "torex")
                {
                    if (Program.kasano == 3 && Program.IsletmeAyarlar["kod"] == "20")
                    {
                        serialPortKD2.Write(27.ToString() + (object)'@');
                        serialPortKD2.Write(string.Concat((object)'\v'));
                        serialPortKD2.Write("Herzlich Willkommen!");
                        serialPortKD2.Write(11.ToString() + (object)'\n');
                        serialPortKD2.Write(Program.IsletmeAyarlar["isletme"]);
                    }
                    else
                    {
                        serialPortKD2.Write(27.ToString() + (object)'[' + (object)'2' + (object)'J');
                        serialPortKD2.Write(27.ToString() + (object)'[' + (object)'1' + (object)';' + (object)'1' + (object)'H');
                        serialPortKD2.Write("Herzlich Willkommen!");
                        serialPortKD2.Write(27.ToString() + (object)'[' + (object)'2' + (object)';' + (object)'1' + (object)'H');
                        serialPortKD2.Write(Program.IsletmeAyarlar["isletme"]);
                    }
                }
                else
                {
                    serialPortKD2.Write(string.Concat((object)'\f'));
                    serialPortKD2.Write(27.ToString() + (object)'Q' + "Herzlich Willkommen!" + (object)'\r');
                }
            }
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                if (seciliItem != -1)
                {
                    if (scaleid == -1)
                    {
                        try
                        {
                            MySqlConnection mySqlConnection = new MySqlConnection();
                            MySqlConnection connection = new db().myconn();

                            if (connection.State == ConnectionState.Closed)
                                connection.Open();
                            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM system", connection);
                            DataTable dataTable = new DataTable("system");
                            dataTable.Rows.Clear();
                            mySqlDataAdapter.Fill(dataTable);
                            if (dataTable.Rows.Count > 0)
                            {
                                scaleid = Convert.ToInt16(dataTable.Rows[0].ItemArray[12]);
                            }

                        }
                        catch
                        {
                        }

                    }
                    try
                    {
                        if (listView1.Items[seciliItem].Text != "")
                        {
                            string urunAd = yeniFis.SatisKalem[seciliItem].UrunAd;
                            double toplamtutar1 = yeniFis.SatisKalem[seciliItem].Toplamtutar;
                            double adet = yeniFis.SatisKalem[seciliItem].Adet;
                            double toplamtutar2 = yeniFis.SatisKalem[seciliItem].Toplamtutar;
                            int gruptur = yeniFis.SatisKalem[seciliItem].Gruptur;
                            if (yeniFis.SatisKalem[seciliItem].isHandyAuflade == 1)
                            {
                                /* if (yeniFis.SatisKalem[seciliItem].Adet == 1)
                                 {
                                     var itemToRemove = yeniFis.PinlistVerkauf.SingleOrDefault(r => r.cardid == yeniFis.SatisKalem[seciliItem].UrunId.ToString());
                                     yeniFis.PinlistVerkauf.Remove(itemToRemove);
                                 }
                                 else
                                 {*/
                                //var DelCardList = yeniFis.PinlistVerkauf.Where(x => x.cardid == yeniFis.SatisKalem[seciliItem].Cardid.ToString()).ToList();
                                //yeniFis.PinlistVerkauf.RemoveRange(
                                for (int b = 0; b < yeniFis.SatisKalem[seciliItem].Adet; b++)
                                {
                                    //for (int c = 0; c < yeniFis.PinlistVerkauf.Count; c++) 
                                    //{
                                    var value = yeniFis.PinlistSell.First(item => item.cardid == yeniFis.SatisKalem[seciliItem].Cardid.ToString());
                                    yeniFis.PinlistSell.Remove(value);

                                    //}
                                }
                                // }
                            }
                            if (yeniFis.SatisKalem[seciliItem].Fand != 0 || yeniFis.SatisKalem[seciliItem].Fand2 != 0)
                            {
                                if (scaleid == 0)
                                {
                                    AddSofortStorno(yeniFis.SatisKalem[seciliItem].UrunAd, yeniFis.SatisKalem[seciliItem].UrunId, yeniFis.SatisKalem[seciliItem].Barkod, yeniFis.SatisKalem[seciliItem].Satisfiyat, yeniFis.SatisKalem[seciliItem].Toplamtutar, yeniFis.SatisKalem[seciliItem].Adet, yeniFis.SatisKalem[seciliItem].Grubid, "", "", "", "", "");

                                }
                                yeniFis.SatisKalem.RemoveAt(seciliItem + 1);
                                yeniFis.SatisKalem.RemoveAt(seciliItem);
                            }
                            /*if (yeniFis.SatisKalem[seciliItem].Grubid == 43)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = "Sie Dürfen nicht Rabatt-Coupon löschen, da es schon entwertet worden! Bitte versuchen Sie komplette Bon  ";
                                fGenericError.lblMesaj.Font = new Font("Tahoma", 11f);
                                fGenericError.lblMesaj.Text += Program.lang["8"];
                                int num = (int)fGenericError.ShowDialog();
                                seciliItem = -1;
                            }*/
                            else
                            {
                                if (scaleid == 0)
                                {
                                    AddSofortStorno(yeniFis.SatisKalem[seciliItem].UrunAd, yeniFis.SatisKalem[seciliItem].UrunId, yeniFis.SatisKalem[seciliItem].Barkod, yeniFis.SatisKalem[seciliItem].Satisfiyat, yeniFis.SatisKalem[seciliItem].Toplamtutar, yeniFis.SatisKalem[seciliItem].Adet, yeniFis.SatisKalem[seciliItem].Grubid, "", "", "", "", "");
                                }
                                yeniFis.SatisKalem.RemoveAt(seciliItem);
                            }
                            int num = 1;
                            listView1.Items.Clear();
                            foreach (SatisYap satisYap in yeniFis.SatisKalem)
                            {
                                int count = listView1.Items.Count;
                                listView1.Items.Add(num.ToString());
                                listView1.Items[count].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet + "x" + satisYap.Satisfiyat.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                if (satisYap.staffelOK == 1)
                                {
                                    listView1.Items[count].Tag = "0";

                                }
                                else
                                {
                                    listView1.Items[count].Tag = satisYap.Barkod == null ? "0" : satisYap.Barkod;
                                }
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                ++num;
                                //matchingvalues.staffelOK = 1;
                            }
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            position = listView1.Items.Count + 1;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            if (dsp != null)
                            {
                                if (Program.displayType != "IBM")
                                {
                                    KDbirinciSatiraYaz(urunAd, "-" + toplamtutar1.ToString("C"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                                }
                                else
                                {
                                    KDbirinciSatiraYaz(urunAd, "-" + toplamtutar1.ToString() + (object)'Õ');
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                                }
                            }
                            else
                            {
                                if (Program.displayType == "TVS")
                                    DSPINFO(urunAd, adet.ToString() + (gruptur != 3 ? (object)" Stk." : (object)"kg."), toplamtutar1.ToString("C") + (gruptur != 3 ? "/Stk." : "kg"), toplamtutar2.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 1, 0, 0, 0);
                                if (serialPortKD2.IsOpen)
                                {
                                    KDbirinciSatiraYaz(urunAd, "-" + toplamtutar1.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                            }
                            seciliItem = -1;
                        }
                        else if (yeniFis.SatisKalem[seciliItem].Grubid == 999)
                        {
                            yeniFis.SatisKalem.RemoveAt(seciliItem);
                            yeniFis.FisiKapat();
                            listView1.Items.RemoveAt(seciliItem);
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            seciliItem = -1;
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["7"];
                            fGenericError.lblMesaj.Font = new Font("Tahoma", 11f);
                            fGenericError.lblMesaj.Text += Program.lang["8"];
                            int num = (int)fGenericError.ShowDialog();
                            seciliItem = -1;
                        }
                    }
                    catch (Exception dd)
                    {

                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = dd.Message;
                        fGenericError.lblMesaj.Font = new Font("Tahoma", 11f);

                        int num = (int)fGenericError.ShowDialog();
                    }
                }
                else
                {
                    if (yeniFis == null || yeniFis.SatisKalem.Count <= 0)
                        return;
                    F_GenericSoru fGenericSoru = new F_GenericSoru();
                    fGenericSoru.lblMesaj.Text = Program.lang["9"];
                    int num = (int)fGenericSoru.ShowDialog();
                    if (!fGenericSoru.sonuc)
                        return;
                    bonStornoFlag = 1;

                    // 25.04.2021 TSE Update Beim Verkaufabbruch
                    try
                    {

                        if (Program.TSE == "1")
                        {

                            Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                            WormTransactionResponse response;
                            //start transc (0,1,2)
                            Stopwatch stopwatch = Stopwatch.StartNew();
                            // processType: Kassenbeleg-V1
                            //processData: <Transaktionstyp>^<Brutto-Steuerumsätze>^<Zahlungen>
                            //Beleg^75.33_7.99_0.00_0.00_0.00^ 10.00:Bar_5.00:Bar:CHF_5.00:Bar:USD_64.30:Unbar

                            string processData = "AVBelegabbruch^" + String.Format("{0:0.00}", yeniFis.mwst19Uygulanantutar).Replace(',', '.') + "_" + String.Format("{0:0.00}", yeniFis.mwst7Uygulanantutar).Replace(',', '.') + "_" + String.Format("{0:0.00}", yeniFis.mwst0Uygulanantutar).Replace(',', '.') + "_0.00_0.00^" + (String.Format("{0:0.00}", yeniFis.ToplamBar + yeniFis.ToplamStorno).Replace(',', '.')) + ":Bar_" + String.Format("{0:0.00}", yeniFis.ToplamEc + yeniFis.ToplamScheck).Replace(',', '.') + ":UnBar";
                            yeniFis.TseProcessData = processData;
                            int returnedDLLCode = -1;
                            int a = 0;
                        Init2: WormReturnClass wormreturn = Program.TSEdll.doTransactionDLL(2, "Kassenbeleg-V1", yeniFis.TseProcessData, Program.ClientID, yeniFis.TseStartedTransaction);
                            returnedDLLCode = wormreturn.errorCode;
                            db baglan = new db();
                            MySqlConnection myConn1 = baglan.myconn();

                            while (a <= 5)
                            {
                                a++;
                                if (a >= 5)
                                {




                                    if (myConn1.State == ConnectionState.Closed)
                                    {
                                        baglan.openConnection();
                                        if (myConn1.State == ConnectionState.Closed)
                                        {
                                            myConn1.Open();
                                        }

                                    }
                                    Tarih tar = new Tarih();
                                    //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                                    string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`) VALUES ('" + wormreturn.errorMessage + "'," + 0 + ", " + tar.unixdate(DateTime.Now) + ")";
                                    MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                    cmdTseErrorCode.ExecuteNonQuery();
                                    Program.TSE = "0";
                                    break;

                                }


                                if (returnedDLLCode == 0)
                                {

                                    response = Program.TSEdll.responseDLL;
                                    yeniFis.TseStartedTransaction = response.transactionNumber();
                                    string s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                                    + "\nTransaction Number: " + response.transactionNumber()
                                    + "\nLog Time: " + response.logTime()
                                    + "\nSignature Counter: " + response.signatureCounter()
                                    + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                                    + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "")
                                    + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                                   + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                                    //MessageBox.Show(s);
                                    if (Program.PublicKey == "")
                                    {
                                        try
                                        {
                                            Program.PublicKey = BitConverter.ToString(Program.TSEdll.myWorm.info().tsePublicKey()).Replace("-", "");
                                        }
                                        catch (Exception ff)
                                        {
                                            MessageBox.Show(ff.Message + ":KryptonButton_3");
                                        }
                                    }
                                    yeniFis.TseSignaturzahler = response.signatureCounter();
                                    yeniFis.TseLogtime = response.logTime();
                                    yeniFis.TseFinishSignatur = System.Convert.ToBase64String(response.signature());
                                    yeniFis.Transactionsnummer = response.transactionNumber();
                                    yeniFis.Tseseriennr = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(BitConverter.ToString(response.serialNumber()).Replace("-", "")));

                                    break;
                                }
                                else
                                {

                                    try
                                    {

                                        myConn1 = baglan.myconn();
                                        if (myConn1.State == ConnectionState.Closed)
                                        {
                                            baglan.openConnection();
                                            if (myConn1.State == ConnectionState.Closed)
                                            {
                                                myConn1.Open();
                                            }

                                        }
                                        Tarih tar = new Tarih();
                                        //INSERT INTO `tseerror`(`id`, `herstelelererrorcode`, `datum`) VALUES ([value-1],[value-2],[value-3])
                                        // ADD `kassenr` INT NOT NULL , ADD `TSEClientID` VARCHAR(100) NOT NULL , ADD `level` VARCHAR(100) NOT NULL ;
                                        string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + wormreturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'BonFinished')";
                                        MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                        cmdTseErrorCode.ExecuteNonQuery();
                                    }
                                    catch (Exception dd)
                                    {
                                        //log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 1460");

                                    }

                                    string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                                    // MessageBox.Show("Array Uzunlugu:"+ErrorMeldungArray.Length);
                                    //MessageBox.Show(ErrorMeldungArray[0]+"->"+ErrorMeldungArray[0]);
                                    if (ErrorMeldungArray.Length == 1)
                                    {
                                        ParaUstuLabelaYaz("TSE Init!, Bitte Warten! ");
                                        WormReturnClass wormReturn = new WormReturnClass();
                                        int TSEDLL = -1;
                                        wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                        TSEDLL = wormReturn.errorCode;
                                        goto Init2;
                                    }
                                    if ((ErrorMeldungArray[1] == " 0x1054") || (ErrorMeldungArray[1] == " 0x1055"))
                                    {
                                        WormReturnClass newWormReturn = new WormReturnClass();
                                        ParaUstuLabelaYaz("TSE Self TEST, Bitte Warten! ");
                                        newWormReturn = Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);


                                        if (newWormReturn.errorCode == 0)
                                        {
                                            Program.TSEdll.ValidTimeCheck();
                                            if (Program.TSEdll.returnErrorCode == 0)
                                            {
                                                ParaUstuLabelaYaz("");
                                                goto Init2;

                                            }
                                            else
                                            {
                                                ParaUstuLabelaYaz("TSE Time Admin!, Bitte Warten! ");
                                                Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                                ParaUstuLabelaYaz("");
                                                goto Init2;
                                            }


                                        }
                                        else
                                        {
                                            string[] SelfTestReturnClassErrorArray = newWormReturn.errorMessage.Split('=');
                                            if (SelfTestReturnClassErrorArray[1] == " 0x1055")
                                            {
                                                MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Kassenhersteller!");
                                                //log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 505");
                                                return;
                                            }
                                            if (SelfTestReturnClassErrorArray[1] == " 0x1011")
                                            {
                                                MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Kassenhersteller!");
                                                //log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 511");
                                                return;
                                            }

                                        }
                                    }
                                    else if (ErrorMeldungArray[1] == " 0x1002")
                                    {
                                        ParaUstuLabelaYaz("TSE TimeAdmin!Warten.. ");
                                        Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                        ParaUstuLabelaYaz("");


                                    }
                                    else if (ErrorMeldungArray[1] == " 3")
                                    {
                                        ParaUstuLabelaYaz("TSE Init!,Warten! ");
                                        WormReturnClass wormReturn = new WormReturnClass();
                                        int TSEDLL = -1;
                                        wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                        TSEDLL = wormReturn.errorCode;
                                        ParaUstuLabelaYaz("");
                                    }
                                    else
                                    {
                                        try
                                        {


                                            myConn1 = baglan.myconn();
                                            if (myConn1.State == ConnectionState.Closed)
                                            {
                                                baglan.openConnection();
                                                if (myConn1.State == ConnectionState.Closed)
                                                {
                                                    myConn1.Open();
                                                }

                                            }
                                            Tarih tar = new Tarih();
                                            //INSERT INTO `tseerror`(`id`, `herstelelererrorcode`, `datum`) VALUES ([value-1],[value-2],[value-3])
                                            // ADD `kassenr` INT NOT NULL , ADD `TSEClientID` VARCHAR(100) NOT NULL , ADD `level` VARCHAR(100) NOT NULL ;
                                            string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + (ErrorMeldungArray.Length > 1 ? ErrorMeldungArray[0] + "->" + ErrorMeldungArray[1] : ErrorMeldungArray[0]) + "->else1'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'BonFinished')";
                                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                            cmdTseErrorCode.ExecuteNonQuery();


                                            ParaUstuLabelaYaz("TSE Init!, Bitte Warten! ");
                                            WormReturnClass wormReturn = new WormReturnClass();
                                            int TSEDLL = -1;
                                            wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                            TSEDLL = wormReturn.errorCode;


                                            if (TSEDLL == 0)
                                                goto Init2;

                                            if (myConn1.State == ConnectionState.Closed)
                                            {
                                                baglan.openConnection();
                                                if (myConn1.State == ConnectionState.Closed)
                                                {
                                                    myConn1.Open();
                                                }

                                            }

                                            //INSERT INTO `tseerror`(`id`, `herstelelererrorcode`, `datum`) VALUES ([value-1],[value-2],[value-3])
                                            // ADD `kassenr` INT NOT NULL , ADD `TSEClientID` VARCHAR(100) NOT NULL , ADD `level` VARCHAR(100) NOT NULL ;
                                            string tseError2 = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + wormReturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'BonFinished')";
                                            MySqlCommand cmdTseErrorCode2 = new MySqlCommand(tseError2, myConn1);
                                            cmdTseErrorCode2.ExecuteNonQuery();
                                        }
                                        catch (Exception dd)
                                        {
                                            //log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 590");

                                        }
                                    }




                                    goto Init2;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    // ende

                    foreach (SatisYap satisYap in yeniFis.SatisKalem)
                        AddSofortStorno(satisYap.UrunAd, satisYap.UrunId, satisYap.Barkod, satisYap.Satisfiyat, satisYap.Toplamtutar, satisYap.Adet, satisYap.Grubid, yeniFis.TseSignaturzahler.ToString(), yeniFis.TseFinishSignatur, yeniFis.TseLogtime.ToString(), yeniFis.Transactionsnummer.ToString(), yeniFis.TseProcessData);
                    yeniFis.SatisKalem.Clear();
                    yeniFis.Rabat = 0.0;
                    yeniFis.Rabattutar = 0.0;
                    yeniFis.AngebotsuzToplamTutar = 0.0;
                    yeniFis.BruttoToplam = 0.0;
                    yeniFis.EskiPuanToplamı = 0.0;
                    yeniFis.HarcananPuan = 0.0;
                    yeniFis.HarcananPuanKarsiligiPara = 0.0;
                    yeniFis.KazanilanPuan = 0.0;
                    yeniFis.Kredit = 0.0;
                    yeniFis.Musteri = (iss_Kunden.Musteri)null;
                    yeniFis.Musterino = 0L;
                    yeniFis.StornoIlgi = 0L;
                    listView1.Items.Clear();
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    if (dsp != null)
                        KundenDisplay();
                    else if (Program.displayType == "TVS")
                        KundenDisplay();
                    else if (serialPortKD2.IsOpen)
                        KundenDisplay();
                    position = 1;
                    yeniFis = (FisOlustur)null;
                }
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void AddSofortStorno(
          string UrunAd,
          Decimal UrunId,
          string Barkod,
          double Satisfiyat,
          double Toplamtutar,
          double Adet,
          int grupID, string tseSigZahler, string TseFinisSig, string TseLogtime, string TseTransNr, string TseProcessData)
        {
            if (TseFinisSig == null)
                TseFinisSig = "-";
            if (TseProcessData == null)
                TseProcessData = "-";
            if (tseSigZahler == null)
                tseSigZahler = "-";
            if (TseLogtime == null)
                TseLogtime = "-";
            if (TseTransNr == null)
                TseTransNr = "-";
            // yeniFis.TseSignaturzahler,yeniFis.TseFinishSignatur,yeniFis.TseLogtime,yeniFis.Transactionsnummer,yeniFis.TseProcessData);
            try
            {
                NullBon nullBon = new NullBon();
                DbParameter Parametre = new DbParameter();
                Parametre.DbName = Program.dbName;
                Parametre.Host = Program.ServerIp;
                if (grupID == 43)
                {
                    yeniFis.RabatList.Remove(yeniFis.RabatList.Single<RabattMain>((Func<RabattMain, bool>)(r => r.TotalRabattMenge == -Satisfiyat)));
                    yeniFis.FisiKapat();
                }
                else
                    nullBon.SofortStornoAdd(Program.bedID, Parametre, Program.BedSitzungId, UrunAd, UrunId, Barkod, Satisfiyat, Toplamtutar, Adet, tseSigZahler, TseFinisSig, TseLogtime, TseTransNr, TseProcessData);
            }
            catch (Exception dd)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = dd.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void btnPLU_Click(object sender, EventArgs e)
        {
            lblParaUstu.Text = "";
            if (Program.IsletmeAyarlar["markt"] == "1")
            {
                if (txtGiris.Text.Length > 7)
                {
                    gelenBarkod = txtGiris.Text;
                    txtGiris.Text = "";
                    barkodDegerlendir(gelenBarkod);
                }
                else
                {
                    if (txtGiris.Text.Length > 0)
                        PLUUrunEkle();
                }
            }
            else
            {
                if (!(Program.IsletmeAyarlar["markt"] == "0") || yeniFis == null || yeniFis.SatisKalem.Count <= 0)
                    return;
                /*** am 29.10.2024 wieder dekativiert*******/
                foreach (SatisYap satisYap in yeniFis.SatisKalem) //Wegen Deutschland Steuersenkung wurde deaktiviert!
                    satisYap.Mwst = Program.MwStList[2];

                yeniFis.FisiKapat();
                BarVerkaufProcess();
            }
        }

        private void PLUUrunEkle()
        {
            lblParaUstu.Text = "";
            SatilanAdet = adet;
            if (txtGiris.Text.IndexOf('X') != -1)
            {
                int length = txtGiris.Text.IndexOf('X');
                if (double.TryParse(txtGiris.Text.Substring(0, length), out PLUSatilanAdet))
                {
                    gelenBarkod = txtGiris.Text.Substring(length + 1, txtGiris.Text.Length - (length + 1));
                    SatilanAdet = PLUSatilanAdet;
                }
                else
                    SatilanAdet = 1.0;
            }
            else
            {
                gelenBarkod = txtGiris.Text;
                SatilanAdet = 1.0;
            }
            txtGiris.Clear();
            Artikel artikel1 = new Artikel();
            if (yeniFis == null)
            {
                yeniFis = new FisOlustur();
                yeniFis.FisYarat(0);
                position = 1;
                listView1.Items.Clear();
            }
            Tarih tarih = new Tarih();
            if (gelenBarkod.Length >= 7)
            {
                if (gelenBarkod.Substring(0, 2) == "24" && gelenBarkod.Substring(2, 2) == Program.IsletmeAyarlar["kod"] && gelenBarkod.Substring(4, 2) == "02")
                    barkodDegerlendir(gelenBarkod);
                else if (gelenBarkod.Substring(0, 6) == "240001" || gelenBarkod.Substring(0, 7) == "2200040")
                {
                    double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                    if (yeniFis == null)
                    {
                        yeniFis = new FisOlustur();
                        yeniFis.FisYarat(0);
                        position = 1;
                        listView1.Items.Clear();
                    }
                    satisYap = new SatisYap();
                    satisYap.Adet = 1.0;
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.Mwst = Program.MwStList[1];
                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                    satisYap.Satisfiyat = num;
                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                    satisYap.Toplamtutar = Math.Round(num, 2);
                    satisYap.UrunId = new Decimal(0);
                    satisYap.Birimkar = 0.0;
                    satisYap.UrunAd = "Fleisch";
                    satisYap.Grubid = 9;
                    yeniFis.SatisKalem.Add(satisYap);
                    int count = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[count].SubItems.Add("Fleisch (" + (object)1 + "St. x" + num.ToString("C") + ")");
                    listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                    listView1.Items[count].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    ++position;
                    if (dsp != null)
                    {
                        KDbirinciSatiraYaz("Fleisch", num.ToString("C"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                    }
                    else
                    {
                        if (!serialPortKD2.IsOpen)
                            return;
                        KDbirinciSatiraYaz("Fleisch", num.ToString("#0.00"));
                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                    }
                }
                else
                {
                    if (gelenBarkod.Substring(0, 2) == "22")
                    {
                        if (Program.IsletmeAyarlar["kod"] == "83")
                        {
                            try
                            {
                                int result1 = 0;
                                double result2 = 0.0;
                                if (!int.TryParse(gelenBarkod.Substring(2, 4), out result1) || !double.TryParse(gelenBarkod.Substring(7, 2) + "," + gelenBarkod.Substring(9, 3), out result2))
                                    return;
                                Artikel artikel2 = new Artikel();
                                artikel2.ArtikelBul(result1.ToString());
                                if (!artikel2.urunvarmi)
                                    return;
                                double num = Math.Round(artikel2.VkPreis, 2);
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                satisYap = new SatisYap();
                                satisYap.Adet = result2;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel2.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = num;
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                satisYap.Birimkar = artikel2.KarMiktari;
                                satisYap.Grubid = 9;
                                satisYap.Toplamtutar = Math.Round(num * result2, 2);
                                if (new ArtikelGrup(9).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = angebotSembol + " " + artikel2.ArtikelAd;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add(angebotSembol + artikel2.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    return;
                                }
                                if (Program.displayType == "TVS")
                                {
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg", satisYap.Adet > 1.0 ? "\n" + satisYap.Toplamtutar.ToString("C") : "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    return;
                                }
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                        }
                    }
                    if (gelenBarkod.Substring(0, 2) == "28")
                    {
                        if (Program.IsletmeAyarlar["kod"] == "205")
                        {
                            try
                            {
                                int result1 = 0;
                                double result2 = 0.0;
                                if (!int.TryParse(gelenBarkod.Substring(2, 5), out result1) || !double.TryParse(gelenBarkod.Substring(7, 2) + "," + gelenBarkod.Substring(9, 3), out result2))
                                    return;
                                Artikel artikel2 = new Artikel();
                                artikel2.ArtikelBul(result1.ToString());
                                if (!artikel2.urunvarmi)
                                    return;
                                double num = Math.Round(artikel2.VkPreis, 2);
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                satisYap = new SatisYap();
                                satisYap.Adet = result2;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel2.Mwst;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = num;
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                satisYap.Birimkar = artikel2.KarMiktari;
                                satisYap.Grubid = artikel2.Grubid;
                                satisYap.Toplamtutar = Math.Round(num * result2, 2);
                                if (new ArtikelGrup(artikel2.Grubid).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = angebotSembol + " " + artikel2.ArtikelAd;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add(angebotSembol + artikel2.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    return;
                                }
                                if (Program.displayType == "TVS")
                                {
                                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg", satisYap.Adet > 1.0 ? "\n" + satisYap.Toplamtutar.ToString("C") : "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    return;
                                }
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                return;
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                        }
                    }
                    if (gelenBarkod.Substring(0, 3) == "244" && Program.IsletmeAyarlar["kod"] == "84")
                    {
                        double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                        }
                        satisYap = new SatisYap();
                        satisYap.Adet = 1.0;
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.KasaNo = Program.kasano;
                        satisYap.Mwst = Program.MwStList[1];
                        satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                        satisYap.Satisfiyat = num;
                        satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                        satisYap.UrunId = new Decimal(0);
                        satisYap.Birimkar = 0.0;
                        satisYap.Grubid = 388;
                        satisYap.Toplamtutar = Math.Round(num, 2);
                        if (new ArtikelGrup(9).Rabatpunkte != 1)
                        {
                            satisYap.Angebotvarmi = 1;
                            angebotSembol = "*";
                        }
                        else
                        {
                            satisYap.Angebotvarmi = 0;
                            angebotSembol = "";
                        }
                        satisYap.UrunAd = angebotSembol + "Bedienertheke_Fisch_Fleisch";
                        yeniFis.SatisKalem.Add(satisYap);
                        int count = listView1.Items.Count;
                        listView1.Items.Add(position.ToString());
                        listView1.Items[count].SubItems.Add(angebotSembol + "Bedienertheke_Fisch_Fleisch (" + (object)1 + "St x" + num.ToString("C") + ")");
                        listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                        listView1.Items[count].Tag = satisYap.Barkod;
                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        txtGiris.Text = "";
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        ++position;
                        if (dsp != null)
                        {
                            KDbirinciSatiraYaz("Bedienertheke_Fisch_Fleisch", num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                        else if (Program.displayType == "TVS")
                        {
                            DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg", satisYap.Adet > 1.0 ? "\n" + satisYap.Toplamtutar.ToString("C") : "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                        }
                        else
                        {
                            if (!serialPortKD2.IsOpen)
                                return;
                            KDbirinciSatiraYaz("Bedienertheke_Fisch_Fleisch", num.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                    }
                    else
                    {
                        if (gelenBarkod.Substring(0, 3) == "240")
                        {
                            if (Program.IsletmeAyarlar["kod"] == "84")
                            {
                                try
                                {
                                    int result1 = 0;
                                    double result2 = 0.0;
                                    if (!int.TryParse(gelenBarkod.Substring(2, 4), out result1) || !double.TryParse(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2), out result2))
                                        return;
                                    Artikel artikel2 = new Artikel();
                                    artikel2.ArtikelBul(result1.ToString());
                                    if (!artikel2.urunvarmi)
                                        return;
                                    double num = result2;
                                    if (yeniFis == null)
                                    {
                                        yeniFis = new FisOlustur();
                                        yeniFis.FisYarat(0);
                                        position = 1;
                                        listView1.Items.Clear();
                                    }
                                    satisYap = new SatisYap();
                                    satisYap.Adet = 1.0;
                                    satisYap.Fisno = yeniFis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = artikel2.Mwst;
                                    satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                    satisYap.Satisfiyat = num;
                                    satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                    satisYap.UrunId = (Decimal)artikel2.ArtikelId;
                                    satisYap.Birimkar = artikel2.KarMiktari;
                                    satisYap.Grubid = 406;
                                    satisYap.Toplamtutar = Math.Round(num, 2);
                                    if (new ArtikelGrup(9).Rabatpunkte != 1)
                                    {
                                        satisYap.Angebotvarmi = 1;
                                        angebotSembol = "*";
                                    }
                                    else
                                    {
                                        satisYap.Angebotvarmi = 0;
                                        angebotSembol = "";
                                    }
                                    satisYap.UrunAd = angebotSembol + " " + artikel2.ArtikelAd;
                                    yeniFis.SatisKalem.Add(satisYap);
                                    int count = listView1.Items.Count;
                                    listView1.Items.Add(position.ToString());
                                    listView1.Items[count].SubItems.Add(angebotSembol + artikel2.ArtikelAd + " (" + result2.ToString("#0.000") + "gr x" + num.ToString("C") + ")");
                                    listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                    listView1.Items[count].Tag = satisYap.Barkod;
                                    if (listView1.Items.Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                    txtGiris.Text = "";
                                    yeniFis.FisiKapat();
                                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                    ++position;
                                    if (dsp != null)
                                    {
                                        KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                        return;
                                    }
                                    if (Program.displayType == "TVS")
                                    {
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg", satisYap.Adet > 1.0 ? "\n" + satisYap.Toplamtutar.ToString("C") : "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                        return;
                                    }
                                    if (!serialPortKD2.IsOpen)
                                        return;
                                    KDbirinciSatiraYaz(artikel2.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    return;
                                }
                            }
                        }
                        if (gelenBarkod.Substring(0, 4) == "2401" && Program.IsletmeAyarlar["kod"] == "80")
                        {
                            double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = Program.MwStList[1];
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = num;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = Math.Round(num, 2);
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.Grubid = 9;
                            if (new ArtikelGrup(9).Rabatpunkte != 1)
                            {
                                satisYap.Angebotvarmi = 1;
                                angebotSembol = "*";
                            }
                            else
                            {
                                satisYap.Angebotvarmi = 0;
                                angebotSembol = "";
                            }
                            satisYap.UrunAd = angebotSembol + "Fleisch";
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add(angebotSembol + "Fleisch (" + (object)1 + "St. x" + num.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz("Fleisch", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else
                            {
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz("Fleisch", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                        }
                        else if (gelenBarkod.Substring(0, 6) == "240002" || gelenBarkod.Substring(0, 7) == "2200040")
                        {
                            double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = Program.MwStList[1];
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = num;
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = num;
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.UrunAd = "Bäckerei";
                            satisYap.Grubid = 40;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add("Bäckerei (" + (object)1 + "St. x" + num.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz("Backerei", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else
                            {
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz("Backerei", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                        }
                        else if (gelenBarkod.Substring(0, 7) == ((int)Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + 1).ToString() + "00000")
                        {
                            MySqlConnection mySqlConnection = new MySqlConnection();
                            MySqlConnection connection = new db().myconn();

                            if (connection.State == ConnectionState.Closed)
                                connection.Open();
                            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM rabatkupon WHERE barkode='" + gelenBarkod + "'", connection);
                            DataTable dataTable = new DataTable("rabatkupon");
                            dataTable.Rows.Clear();
                            mySqlDataAdapter.Fill(dataTable);
                            if (dataTable.Rows.Count == 0)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["57"];
                                int num = (int)fGenericError.ShowDialog();
                            }
                            else if (Convert.ToInt16(dataTable.Rows[0].ItemArray[8]) == (short)1)
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["57"] + "\n :" + tarih.tarih(Convert.ToInt64(dataTable.Rows[0].ItemArray[7]));
                                int num = (int)fGenericError.ShowDialog();
                            }
                            else
                            {
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                satisYap = new SatisYap();
                                satisYap.Adet = 1.0;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = 0;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Satisfiyat = -Convert.ToDouble(dataTable.Rows[0].ItemArray[6]);
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = -Convert.ToDouble(dataTable.Rows[0].ItemArray[6]);
                                satisYap.UrunId = new Decimal(0);
                                satisYap.Birimkar = 0.0;
                                satisYap.UrunAd = "Rabat Kupon";
                                satisYap.Grubid = 43;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add("Rabat Coupon (" + (object)1 + "x" + (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz("Rabat Coupon", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                                else
                                {
                                    if (!serialPortKD2.IsOpen)
                                        return;
                                    KDbirinciSatiraYaz("Rabat Coupon", (-Convert.ToDouble(dataTable.Rows[0].ItemArray[6])).ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                            }

                        }
                        else if (gelenBarkod.Substring(0, 7) == ((int)Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + 2).ToString() + "00000")
                        {
                            double num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            satisYap = new SatisYap();
                            satisYap.Adet = 1.0;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = Program.MwStList[2];
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = -Convert.ToDouble(num);
                            satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = -Convert.ToDouble(num);
                            satisYap.UrunId = new Decimal(0);
                            satisYap.Birimkar = 0.0;
                            satisYap.UrunAd = "PfandRuckgabe";
                            satisYap.Gv_typ_id = (int)GVTypEnum.PfandRueckzahlung;
                            satisYap.Grubid = 64;
                            yeniFis.SatisKalem.Add(satisYap);
                            int count = listView1.Items.Count;
                            listView1.Items.Add(position.ToString());
                            listView1.Items[count].SubItems.Add("PfandRuckgabe (" + (object)1 + "x" + num.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            listView1.Items[count].Tag = satisYap.Barkod;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            ++position;
                            if (dsp != null)
                            {
                                KDbirinciSatiraYaz("Pfand Ruckgabe", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                            else
                            {
                                if (!serialPortKD2.IsOpen)
                                    return;
                                KDbirinciSatiraYaz("Pfand Ruckgabe", num.ToString("#0.00"));
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            }
                        }
                        else if ((gelenBarkod.Substring(0, 7) == "0000000" || gelenBarkod.Substring(0, 7) == "0100000") && Program.IsletmeAyarlar["kod"] == "70")
                        {
                            if (yeniFis == null)
                            {
                                yeniFis = new FisOlustur();
                                yeniFis.FisYarat(0);
                                position = 1;
                                listView1.Items.Clear();
                            }
                            if (getMusteriNo(gelenBarkod) == -1L)
                                return;
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                        }
                        else if (gelenBarkod.Length > 7)
                        {
                            if (gelenBarkod.Substring(0, 8) == Convert.ToInt16(Program.IsletmeAyarlar["kod"]).ToString() + "000000")
                            {
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }
                                if (getMusteriNo(gelenBarkod) == -1L)
                                    return;
                                yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            }
                        }
                        else
                            barkodluUrunEkle();
                    }
                }
            }
            else if (gelenBarkod.Length == 5)
            {
                if (Program.IsletmeAyarlar["kod"] == "70")
                {
                    string brkd = gelenBarkod;
                    for (int a = 6; a <= 13; a++)
                    {
                        gelenBarkod = "0" + gelenBarkod;
                    }
                    if ((gelenBarkod.Substring(0, 7) == "0000000") && Program.IsletmeAyarlar["kod"] == "70")//alem
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            //musteri = new Musteri(musNo);
                        }
                        else
                        {
                            string sifir = new string('0', 6);
                            brkd = "01" + sifir + brkd;
                            gelenBarkod = brkd;

                            if ((gelenBarkod.Substring(0, 7) == "0100000") && Program.IsletmeAyarlar["kod"] == "70")//alem
                            {

                                //MessageBox.Show(etfiyat.ToString());
                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                    //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                                }

                                musNo = getMusteriNo(gelenBarkod);
                                if (musNo != -1)
                                {
                                    yeniFis.Musterino = getMusteriNo(gelenBarkod);
                                    //musteri = new Musteri(musNo);
                                }
                            }
                        }
                    }

                }
                else if (Program.IsletmeAyarlar["kod"] == "27")
                {
                    string brkd = gelenBarkod;
                    gelenBarkod = "27000000" + gelenBarkod;
                    if (gelenBarkod.Substring(0, 7) == "2700000") //riwa
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            //musteri = new Musteri(musNo);
                        }

                    }

                }
                else if (Program.IsletmeAyarlar["kod"] == "30")
                {
                    gelenBarkod = "30000000" + gelenBarkod;
                    if ((gelenBarkod.Substring(0, 7) == "3000000") && Program.IsletmeAyarlar["kod"] == "30")//huzur
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            // lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            //musteri = new Musteri(musNo);
                        }
                    }
                }
                else if (Program.IsletmeAyarlar["kod"] == "45") //ersin
                {
                    gelenBarkod = "10000000" + gelenBarkod;
                    if ((gelenBarkod.Substring(0, 7) == "1000000") && Program.IsletmeAyarlar["kod"] == "45")//huzur
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            // lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = musNo;
                            //musteri = new Musteri(musNo);
                        }
                    }
                }
                else if (Program.IsletmeAyarlar["kod"] == "191") //ecagri
                {
                    gelenBarkod = "20160600" + gelenBarkod;
                    if (Program.IsletmeAyarlar["kod"] == "191")//
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            // lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            //musteri = new Musteri(musNo);
                        }
                    }
                }
                else  //allgemein
                {
                    gelenBarkod = "10000000" + gelenBarkod;
                    if ((gelenBarkod.Substring(0, 7) == "1000000") && Program.IsletmeAyarlar["kod"] == "45")//huzur
                    {

                        //MessageBox.Show(etfiyat.ToString());
                        if (yeniFis == null)
                        {
                            yeniFis = new FisOlustur();
                            yeniFis.FisYarat(0);
                            position = 1;
                            listView1.Items.Clear();
                            // lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                        }

                        long musNo = getMusteriNo(gelenBarkod);
                        if (musNo != -1)
                        {
                            yeniFis.Musterino = getMusteriNo(gelenBarkod);
                            //musteri = new Musteri(musNo);
                        }
                    }
                }
            }
            else if (gelenBarkod.Length == 4 && Convert.ToInt16(gelenBarkod) >= (short)1000 && Convert.ToInt16(gelenBarkod) <= (short)1020)
            {
                if (gelenBarkod.Length == 4 && gelenBarkod == "1000")
                {
                    if (yeniFis == null)
                        return;
                    yeniFis.Musterino = 0L;
                    yeniFis.Musteri = (iss_Kunden.Musteri)null;
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["59"];
                    int num = (int)fGenericError.ShowDialog();
                    txtGiris.Text = "";
                    lblParaUstu.Text = "";
                    ReadGeraboKart = false;
                }
                else if (gelenBarkod.Length == 4 && gelenBarkod == "1010")
                {
                    if (yeniFis == null)
                        return;
                    yeniFis.Rabat = 0.0;
                    yeniFis.FisiKapat();
                    txtGiris.Text = "";
                    lblParaUstu.Text = "";
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                }
                else if (gelenBarkod.Length == 4 && gelenBarkod == "1006")
                {
                    if (yeniFis == null)
                        return;
                    KoliKiste = true;
                    lblParaUstu.Text = "Kolli-Kiste";

                }
                else if (gelenBarkod.Length == 4 && gelenBarkod == "1001")
                {
                    if (yeniFis == null)
                        return;
                    PreisCheck = true;
                    lblParaUstu.Text = "Preis-Check";

                }
                else
                {
                    if (gelenBarkod.Length != 4 || !(gelenBarkod == "1005"))
                        return;
                    MySqlConnection mySqlConnection1 = new MySqlConnection();
                    db db = new db();
                    VirgulAyikla virgulAyikla = new VirgulAyikla();
                    MySqlConnection mySqlConnection2 = db.myconn();
                    if (mySqlConnection2.State == ConnectionState.Closed)
                        mySqlConnection2.Open();
                    Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
                    Gutschein gutschein = new Gutschein();
                    gutschein.Virgulayari = Program.GlobalAyarlar["BKOMMA"];
                    gutschein.m_Printer = Program.printer;
                    gutschein.IsletmeAdi = Program.IsletmeAyarlar["isletme"];
                    gutschein.Bediener = Program.bedAdSoyad;
                    string str = !(gutscheinbrkd == "") ? gutscheinbrkd : getBarkod();
                    gutschein.Brkd = str;
                    Dictionary<string, string> dictionary2 = gutschein.GutscheinDruck();
                    if (dictionary2.Count <= 0)
                        return;
                    MySqlConnection connection;
                    connection = db.myconn();

                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        if (new MySqlCommand("INSERT INTO gutschein VALUES('','', " + dictionary2["erstellt"] + ", " + dictionary2["ablauf"] + ",'" + str + "'," + virgulAyikla.virgulayikla(Convert.ToDouble(dictionary2["Betrag"])) + "," + (object)Program.bedID + ", 0, 0," + virgulAyikla.virgulayikla(Convert.ToDouble(dictionary2["Betrag"])) + ")", connection).ExecuteNonQuery() > 0)
                            return;
                        int num = (int)MessageBox.Show("S:332-FIsl" + Program.lang["776"]);
                    }
                    catch
                    {
                    }

                }
            }
            else if (gelenBarkod.Length < 4)
            {

                iss_Artikel.Artikel urun = new iss_Artikel.Artikel();

                if (yeniFis == null)
                {
                    yeniFis = new FisOlustur();
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                    lblParaUstu.Text = "";
                    // lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                    //Thread thread2 = new Thread(new ThreadStart(listBoxBosalt));
                    //thread2.Start();


                }

                //  MessageBox.Show("fisno=" + yeniFis.satisAnaId.ToString());
                /*ilişki araştırması
                 List<string> barkodlarList = new List<string>();
                Dictionary<List<string>, double> iliski = new Dictionary<List<string>, double>();
                List<Dictionary<List<string>, double>> iliskiler = new List<Dictionary<List<string>, double>>();
                 */

                urun.ArtikelBulPLU(gelenBarkod);

                if (urun.urunvarmi == true)
                {
                    if (urun.Gruptur == 3)
                    {
                        WaagePLUTaste(urun);
                        return;
                    }
                    //GECICI Alkol Akkauf Uyarı

                    if (((urun.Grubid == 31) || (urun.Grubid == 55) || (urun.Grubid == 62)) && (Program.IsletmeAyarlar["kod"] == "20"))
                    {
                        F_AchtungAlkohol frmAlkohol = new F_AchtungAlkohol();
                        frmAlkohol.ShowDialog();
                    }
                    if (((urun.Grubid == 82) || (urun.Grubid == 75) || (urun.Grubid == 87) || (urun.Grubid == 88) || (urun.Grubid == 72)) && (Program.IsletmeAyarlar["kod"] == "21"))
                    {
                        F_AchtungAlkohol frmAlkohol = new F_AchtungAlkohol();
                        frmAlkohol.ShowDialog();
                    }
                    int elemanNo = 0;

                    satisYap = new SatisYap();
                    satisYap.Adet = adet;
                    satisYap.Fisno = yeniFis.SatisAnaId;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.Mwst = urun.Mwst;
                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                    satisYap.Barkod = urun.BarkodNo;
                    if (urun.AngebotVarmi != 0)
                    {
                        if (urun.AngebotBaslamaTarihi <= tarih.unixdate(DateTime.Now) && urun.AngebotBitistarihi >= tarih.unixdate(DateTime.Now))
                        {
                            satisYap.Satisfiyat = urun.AngebotFiyati;
                            angebotSembol = "*";
                        }
                        else
                        {
                            satisYap.Satisfiyat = urun.VkPreis;
                            angebotSembol = "";
                        }
                    }
                    else if (urun.Punkterabatdurum != 1)
                    {
                        satisYap.Angebotvarmi = 1;
                        angebotSembol = "*";
                        satisYap.Satisfiyat = urun.VkPreis;

                    }
                    else
                    {
                        angebotSembol = "";
                        satisYap.Satisfiyat = urun.VkPreis;
                    }



                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                    satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * adet, 2);
                    if (satisYap.Toplamtutar == 0)
                    {
                        Console.Beep(400, 200);
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["67"];
                        frmerror.ShowDialog();
                        return;
                    }
                    satisYap.UrunId = urun.ArtikelId;
                    satisYap.UrunAd = angebotSembol + urun.ArtikelAd;
                    satisYap.Birimkar = (satisYap.Satisfiyat - urun.EkPreis) * adet;
                    satisYap.Grubid = urun.Grubid;
                    satisYap.Fand = urun.Fand;
                    satisYap.Fand2 = urun.Fand2;
                    satisYap.Angebotvarmi = urun.AngebotVarmi;
                    satisYap.Angebotfiyat = urun.AngebotFiyati;
                    satisYap.Gruptur = urun.Gruptur;
                    if (urun.AngebotVarmi == 0 && urun.Punkterabatdurum != 1)
                    {
                        satisYap.Angebotvarmi = 1;
                        angebotSembol = "*";

                    }
                    yeniFis.SatisKalem.Add(satisYap);
                    LastPos = satisYap;
                    int listViewElaman = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[listViewElaman].SubItems.Add(angebotSembol + satisYap.UrunAd.ToString() + "(" + adet + "Stk. x" + satisYap.Satisfiyat.ToString("C") + "/Stk.)");
                    listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                    listView1.Items[listViewElaman].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    }
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    position++;
                    if (urun.Fand != 0)
                    {
                        Artikel furun = new Artikel();
                        furun.ArtikelBul(urun.Fand.ToString());
                        if (furun.urunvarmi == true)
                        {
                            satisYap = new SatisYap();
                            satisYap.Adet = adet;
                            satisYap.Fisno = yeniFis.SatisAnaId;
                            satisYap.Barkod = furun.BarkodNo;
                            satisYap.KasaNo = Program.kasano;
                            satisYap.Mwst = furun.Mwst;
                            satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                            satisYap.Satisfiyat = furun.VkPreis;
                            satisYap.Tarih = tarih.unixdate(DateTime.Now);
                            satisYap.Toplamtutar = Math.Round(furun.VkPreis * adet, 2);
                            satisYap.UrunId = furun.ArtikelId;
                            satisYap.UrunAd = furun.ArtikelAd;
                            satisYap.Birimkar = (furun.VkPreis - furun.EkPreis) * adet;
                            satisYap.Grubid = furun.Grubid;
                            satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                            satisYap.Fand = 1;
                            yeniFis.SatisKalem.Add(satisYap);
                            listViewElaman = listView1.Items.Count;
                            listView1.Items.Add("");
                            listView1.Items[listViewElaman].SubItems.Add(furun.ArtikelAd.ToString() + "(" + adet + "Stk. x" + furun.VkPreis.ToString("C") + "/Stk.)");
                            listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            //listView1.SelectedItems[listView1.Items.Count - 1].Focused = true;
                            txtGiris.Text = "";
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        }
                    }



                    if (dsp != null)
                    {
                        if (Program.displayType != "IBM")
                        {
                            KDbirinciSatiraYaz(urun.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString("C"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                        }
                        else
                        {

                            KDbirinciSatiraYaz(urun.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString() + ((char)213));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + ((char)213));
                        }
                    }
                    else
                    {
                        if (Program.displayType == "TVS")
                        {
                            //knddsply.VerkaufInfo(satisYap.UrunAd + "\n" + satisYap.Adet + "Stk. X" + satisYap.Satisfiyat.ToString("C") + "/Stk." + (satisYap.Adet > 1 ? "\n" + satisYap.Toplamtutar.ToString("C") : ""), "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                            DSPINFO(satisYap.UrunAd, satisYap.Adet + " Stk.", satisYap.Satisfiyat.ToString("C") + "/Stk.", (satisYap.Adet > 1 ? "\n" + satisYap.Toplamtutar.ToString("C") : ""), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);

                        }
                        else if (serialPortKD2.IsOpen == true)
                        {
                            // serialPortKD2.Write("" + ((char)27) + ((char)91) + ((char)50) + ((char)74));
                            KDbirinciSatiraYaz(urun.ArtikelAd.ToString(), satisYap.Satisfiyat.ToString("#0.00"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                        }
                    }
                }
                else
                {
                    GewichtsBarcodeCheck(gelenBarkod);
                    UrunYok();

                }


            }
            else
                barkodluUrunEkle();
        }
        private void WaagePLUTaste(iss_Artikel.Artikel arananArtikel)
        {
            if (arananArtikel.urunvarmi == true)
            {
                if (yeniFis == null)
                {
                    yeniFis = new FisOlustur();
                    yeniFis.Infoevent += new FisOlustur.lblParaUstuYaz(ParaUstuLabelaYaz);
                    yeniFis.FisYarat(0);
                    position = 1;
                    listView1.Items.Clear();
                    //lblBonNo.Text = yeniFis.SatisAnaId.ToString();
                    lblParaUstu.Text = "";
                }

                if (arananArtikel.VkPreis <= 0)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "DER ARTIKELPREIS IST 0 (NULL)!";
                    Console.Beep(1000, 1000);
                    frmerror.ShowDialog();

                    return;
                }
                double KundenPreis = 0, NormalPreis = 0;
                if (yeniFis.Musterino != 0)
                {
                    if (yeniFis.Musteri.Method == 5)
                    {
                        NormalPreis = arananArtikel.VkPreis;
                        KundenPreis = GetSellKundenPreis(yeniFis.Musteri.MusteriGrup, yeniFis.Musterino, arananArtikel.BarkodNo);
                        if (KundenPreis != 0)
                        {

                            arananArtikel.VkPreis = KundenPreis;
                            angebotSembol = "#";
                        }
                        else
                        {
                            angebotSembol += "";
                            satisYap.Satisfiyat = arananArtikel.VkPreis;
                        }
                    }
                }

                ept.ProgramAyarlar = Program.ProgramAyarlar;
                ept.WaagePortName = Program.ProgramAyarlar["WPORT"];
                // ept.BenimEventim += new etp_extended_main.DisplayDelegate(DSPINFO);
                satisYap = new SatisYap();
                ept_extended.SatisYap_ept WaageResultPos = new SatisYap_ept();
                WaageResultPos = ept.WaageMitPLU(arananArtikel);
                if (WaageResultPos.errorMeldung == "")
                {
                    satisYap.Adet = WaageResultPos.Adet;
                    if (yeniFis.Musterino != 0)
                    {
                        if (yeniFis.Musteri.Method == 5)
                        {
                            satisYap.ProzisyonRabatBetrag = (NormalPreis - KundenPreis) * satisYap.Adet;
                        }
                    }

                    //satisYap.Fisno = yeniFis.SatisAnaId;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.Mwst = WaageResultPos.Mwst;
                    satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                    satisYap.Gruptur = WaageResultPos.Gruptur;
                    satisYap.Alisfiyat = WaageResultPos.Alisfiyat;
                    satisYap.Satisfiyat = WaageResultPos.Satisfiyat;
                    satisYap.Gv_typ_id = WaageResultPos.Gv_typ_id;
                    satisYap.Tarih = WaageResultPos.Tarih;
                    satisYap.Einheit = WaageResultPos.Einheit;
                    satisYap.Toplamtutar = Math.Round(WaageResultPos.Toplamtutar, 2);
                    satisYap.Angebotvarmi = WaageResultPos.Angebotvarmi;
                    if (satisYap.Toplamtutar >= 50)
                    {

                        F_GrossSummeBesteatigung frmSumme = new F_GrossSummeBesteatigung();
                        frmSumme.summe = satisYap.Toplamtutar;
                        frmSumme.ShowDialog();
                        if (frmSumme.bestatigung != 1)
                        {
                            satisYap = null;
                            return;
                        }

                    }

                    satisYap.UrunId = WaageResultPos.UrunId;
                    satisYap.UrunAd = angebotSembol + WaageResultPos.UrunAd.Replace("'", "");
                    satisYap.Birimkar = (WaageResultPos.Birimkar) * adet;
                    satisYap.Grubid = WaageResultPos.Grubid;
                    satisYap.Fand = WaageResultPos.Fand;
                    satisYap.Birimid = WaageResultPos.Birimid;
                    satisYap.Barkod = WaageResultPos.Barkod;
                    satisYap.KasaNo = Program.kasano;
                    satisYap.KasiyerId = Program.bedID;
                    yeniFis.SatisKalem.Add(satisYap);
                    int count1 = listView1.Items.Count;
                    listView1.Items.Add(position.ToString());
                    listView1.Items[count1].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet.ToString("#0.000") + " kg. x " + satisYap.Satisfiyat.ToString("C") + "/kg.)");
                    listView1.Items[count1].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                    listView1.Items[count1].Tag = satisYap.Barkod;
                    if (listView1.Items.Count > 0)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    txtGiris.Text = "";
                    yeniFis.FisiKapat();
                    txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                    ++position;
                    angebotSembol = "";
                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                    satisYap = null;
                    Console.Beep(800, 100);
                    Console.Beep(1000, 100);
                    return;
                }
                else
                {

                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = WaageResultPos.errorMeldung;
                    Console.Beep(1000, 1000);
                    frmerror.ShowDialog();

                    return;
                }
            }
        }
        private string getMaxID(string Brkd)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection connection;
            connection = new db().myconn();
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT max(id) as Maxid from gutschein ", connection);
                DataTable dataTable = new DataTable();
                dataTable.Rows.Clear();
                mySqlDataAdapter.Fill(dataTable);
                string str = !(dataTable.Rows[0].ItemArray[0] is DBNull) ? (Convert.ToInt32(dataTable.Rows[0].ItemArray[0]) + 1).ToString() : 1.ToString();
                while (str.Length < 12 - Brkd.Length)
                    str = "0" + str;
                return str;
            }
        }

        private string getBarkod()
        {
            Ean13 ean13 = new Ean13()
            {
                CountryCode = "24" + (object)Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + "02",
                ManufacturerCode = "0"
            };
            ean13.ProductCode = getMaxID(ean13.CountryCode + ean13.ManufacturerCode);
            return ean13.ToString();
        }

        private bool GewichtsBarcodeCheck(string gelenBarkod)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection connection = new db().myconn();
            try
            {

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM `gewichtsbarcode` WHERE SUBSTRING(" + gelenBarkod + ",1,(`firmenIDlange`+`firmenArtIDlange`)) LIKE `gewinfo`", connection);
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                mySqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count <= 0)
                    return false;
                try
                {
                    double result = 0.0;
                    if (double.TryParse(gelenBarkod.Substring(8, 1) + "," + gelenBarkod.Substring(9, 3), out result))
                    {
                        Artikel artikel = new Artikel();
                        artikel.ArtikelBulGewicht(dataTable.Rows[0].ItemArray[5].ToString());
                        double num = 0;
                        if (Program.IsletmeAyarlar["kod"] == "383")
                        {
                            num = Math.Round(Convert.ToDouble(gelenBarkod.Substring(7, 3) + "," + gelenBarkod.Substring(10, 2)), 2);
                            result = 1.0;
                        }
                        else
                        {
                            if (artikel.urunvarmi)
                            {
                                satisYap = new SatisYap();
                                if (artikel.AngebotVarmi != 0)
                                {

                                    if (artikel.Punkterabatdurum != 1)
                                    {
                                        satisYap.Angebotvarmi = 1;
                                        angebotSembol = "*";
                                        num = satisYap.Satisfiyat = artikel.VkPreis;
                                    }
                                    if (artikel.AngebotBaslamaTarihi <= (double)tarih.unixdate(DateTime.Now) && artikel.AngebotBitistarihi >= (double)tarih.unixdate(DateTime.Now))
                                    {
                                        num = satisYap.Satisfiyat = artikel.AngebotFiyati;
                                        angebotSembol = "*";
                                    }
                                    else
                                    {
                                        num = satisYap.Satisfiyat = artikel.VkPreis;
                                        angebotSembol = "";
                                    }
                                }
                                else
                                {
                                    num = Math.Round(artikel.VkPreis, 2);
                                }



                                if (yeniFis == null)
                                {
                                    yeniFis = new FisOlustur();
                                    yeniFis.FisYarat(0);
                                    position = 1;
                                    listView1.Items.Clear();
                                }

                                satisYap.Adet = result;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = artikel.Mwst;
                                satisYap.Satisfiyat = num;
                                satisYap.Ustid_id = satisYap.Mwst == 7 ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == 19 ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.UrunId = (Decimal)artikel.ArtikelId;
                                satisYap.Birimkar = artikel.KarMiktari;
                                satisYap.Grubid = artikel.Grubid;
                                satisYap.Toplamtutar = Math.Round(num * result, 2);
                                if (new ArtikelGrup(satisYap.Grubid).Rabatpunkte != 1)
                                {
                                    satisYap.Angebotvarmi = 1;
                                    angebotSembol = "*";
                                }
                                else
                                {
                                    satisYap.Angebotvarmi = 0;
                                    angebotSembol = "";
                                }
                                satisYap.UrunAd = artikel.ArtikelAd;
                                satisYap.Barkod = artikel.BarkodNo;
                                yeniFis.SatisKalem.Add(satisYap);
                                int count = listView1.Items.Count;
                                listView1.Items.Add(position.ToString());
                                listView1.Items[count].SubItems.Add(artikel.ArtikelAd + " (" + result.ToString("#0.000") + "gr x" + num.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                if (listView1.Items.Count > 0)
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                txtGiris.Text = "";
                                if (satisYap.Toplamtutar <= 0)
                                    Console.Beep(1000, 1000);
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                ++position;
                                if (dsp != null)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                                else if (Program.displayType == "TVS")
                                {
                                    if (Program.IsletmeAyarlar["kod"] == "383")
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString() + " Stk.", satisYap.Satisfiyat.ToString("C"), satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                    else
                                        DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + " kg.", satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                }
                                else if (serialPortKD2.IsOpen)
                                {
                                    KDbirinciSatiraYaz(artikel.ArtikelAd, num.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        private void UrunYok()
        {
            int num = (int)new F_UrunYok().ShowDialog();
        }

        private void btnStorno_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                int result;
                if (int.TryParse(txtGiris.Text, out result))
                {
                    int num = (int)new F_Storno() { fisno = result }.ShowDialog();
                    txtGiris.Text = "";
                }
                else
                {

                    if (tekrarFis != null)
                    {
                        tekrarFis = null;
                    }
                    if (tekrarFis == null)
                    {
                        tekrarFis = new FisOlustur();
                    }
                    Int32 tekrarFisNo = 0;
                    if (Int32.TryParse(lblBonNo.Text, out tekrarFisNo))
                    {
                        if (tekrarFisNo != 0)
                        {
                            if (tekrarFis.FisCagir(tekrarFisNo) == true)
                            {
                                tekrarFis.Bewirtung = 0;
                                CheckForIllegalCrossThreadCalls = false;
                                FisBarkodlu barkodluFis = new FisBarkodlu();
                                barkodluFis.basilacakFis = tekrarFis;

                                barkodluFis.OdemeTur = tekrarFis.odemesekli;
                                barkodluFis.basilacakFis.toplamtutar = tekrarFis.toplamtutar - tekrarFis.Rabattutar - tekrarFis.PuanRabat;
                                barkodluFis.FisYaz();
                            }
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["11"];
                            int num = (int)fGenericError.ShowDialog();
                        }
                    }

                }
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void ButtonOlustur()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection connection = new db().myconn();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM artikelgrup WHERE (gruptur>1 AND grupid NOT IN (select btnID FROM kassenbutton WHERE kasano<>" + Program.kasano + ")) OR (gruptur>1 AND grupid IN (select btnID FROM kassenbutton WHERE kasano=" + Program.kasano + ")) ORDER BY `grupad` ", connection);
            DataTable dataTable = new DataTable("artikelgrup");
            dataTable.Clear();
            mySqlDataAdapter.Fill(dataTable);
            int count = dataTable.Rows.Count;
            if (count > 0)
            {
                flowPanel1.Controls.Clear();
                if (Program.ProgramAyarlar["wprotokoll"] == "Dialog06ext")
                {
                    Panel pnlVCO = new Panel();
                    pnlVCO.Width = 490;
                    pnlVCO.Height = 50;
                    flowPanel1.Controls.Add(pnlVCO);
                }

                for (int index = 0; index < count; ++index)
                {
                    KryptonButton kryptonButton = new KryptonButton();
                    kryptonButton.Location = new System.Drawing.Point(3, 3);
                    kryptonButton.Name = dataTable.Rows[index].ItemArray[0].ToString();
                    kryptonButton.OverrideDefault.Border.Color1 = Color.Fuchsia;
                    kryptonButton.OverrideDefault.Border.DrawBorders = PaletteDrawBorders.All;
                    kryptonButton.PaletteMode = PaletteMode.Office2007Silver;
                    if (Program.ProgramAyarlar["wprotokoll"] == "Dialog06ext")
                    {
                        kryptonButton.Size = new Size(155, 90);
                    }
                    else
                    {
                        kryptonButton.Size = new Size(155, 100);
                    }

                    kryptonButton.StateNormal.Border.Color1 = Color.Navy;///.ActiveCaption;
                    kryptonButton.StateNormal.Border.DrawBorders = PaletteDrawBorders.All;
                    kryptonButton.StateNormal.Border.Rounding = 4;
                    kryptonButton.StateNormal.Border.Width = 2;
                    kryptonButton.StateNormal.Content.ShortText.Color1 = Color.Navy;
                    kryptonButton.StateNormal.Content.ShortText.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    kryptonButton.StateTracking.Content.ShortText.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    kryptonButton.TabIndex = (int)Convert.ToInt16(dataTable.Rows[index].ItemArray[3]);
                    kryptonButton.Values.Text = dataTable.Rows[index].ItemArray[1].ToString();
                    kryptonButton.Tag = dataTable.Rows[index].ItemArray[2];
                    if (dataTable.Rows[index].ItemArray[8].ToString() != "")
                    {
                        try
                        {
                            kryptonButton.StateCommon.Content.Padding = new Padding(-1, kryptonButton.Size.Height - 40, -1, -1);
                            kryptonButton.StateNormal.Back.ImageAlign = PaletteRectangleAlign.Control;
                            kryptonButton.StateNormal.Back.ImageStyle = PaletteImageStyle.TopMiddle;
                            kryptonButton.StateNormal.Back.Image = Image.FromFile(Application.StartupPath + "\\" + dataTable.Rows[index].ItemArray[8].ToString());
                            kryptonButton.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                        }
                        catch (Exception ex)
                        {
                            //int num = (int)MessageBox.Show(ex.Message);
                        }
                    }
                    kryptonButton.Click += new EventHandler(btnDiv7_Click);
                    flowPanel1.Controls.Add((Control)kryptonButton);
                }
            }
            else
                flowPanel1.Controls.Clear();
        }

        private void TFTYazBarVerkauf(string Detay, string Total)
        {
            DSPINFO("", "", "", Detay, Total, 8, 0, 0, 0);
        }

        private void TFTYaz(string Detay, string Total)
        {
            DSPINFO("", "", "", "", Detay + Total, 7, 0, 0, 0);
        }
        private void ParaUstuLabelaYaz(string Info)
        {
            lblParaUstu.Text = Info;
        }
        private void btnAyar_Click(object sender, EventArgs e)
        {
            //barkodDegerlendir(txtGiris.Text);
            try
            {
                KryptonButton kryptonButton = sender as KryptonButton;
                yetkiCheck = new YetkiCheck();
                yetkiCheck.YetkiKontrol(kryptonButton.Name);
                if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
                {
                    FIslemler fislemler = new FIslemler();
                    fislemler.geparkteBonList = MultiParkBons;
                    fislemler.BenimEventim += new FIslemler.BenimDelegem(TFTYaz);
                    fislemler.Rea = Rea;
                    fislemler.yetkiCheck = yetkiCheck;
                    int num = (int)fislemler.ShowDialog();
                    if (Convert.ToInt16(Program.ProgramAyarlar["mp"]) > (short)0)
                    {
                        btnBonPark.StateNormal.Back.Image = (Image)Resources.parking;
                        btnBonPark.StateNormal.Back.ImageStyle = PaletteImageStyle.CenterLeft;
                        btnBonPark.Values.Text = MultiParkBons.Count.ToString();
                        btnBonPark.StateNormal.Content.ShortText.TextH = PaletteRelativeAlign.Far;
                    }
                    if (yeniFis == null)
                        KundenDisplay();
                    kryptonButton19.StateNormal.Back.Image = Program.DruckerMode != 1 ? (Program.DruckerMode != 1 ? (Image)Resources.printer_yellow : (Image)Resources.printer_red) : (Image)Resources.printer_green;
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["6"];
                    int num = (int)fGenericError.ShowDialog();
                }
                DSPINFO("", "", "", "", "", 6, 0, 0, 0);
            }
            catch (Exception gg)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = gg.Message + "\n\n" + gg.StackTrace;
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void btnZweiteEbene_Click(object sender, EventArgs e)
        {
            if (Program.IsletmeAyarlar["markt"] == "0")
            {
                DSPINFO("", "", "", "", "", 9, 0, 0, 0);
                F_Tisch fTisch = new F_Tisch();
                fTisch.KundenDisplayEvent += new F_Tisch.KundenDisplayDelagate(DSPINFO);

                int num = (int)fTisch.ShowDialog();
                DSPINFO("", "", "", "", "", 9, 0, 0, 0);
            }
            else
            {
                try
                {
                    KryptonButton kryptonButton = sender as KryptonButton;
                    yetkiCheck = new YetkiCheck();
                    yetkiCheck.YetkiKontrol(kryptonButton.Name);
                    if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
                    {
                        if (parkedilenFis != null)
                        {
                            foreach (SatisYap pos in parkedilenFis.SatisKalem)
                            {
                                if (pos.Gruptur == 3)
                                {
                                    F_GenericError fGenericError = new F_GenericError();
                                    fGenericError.lblMesaj.Text = "Sie haben in geparkter Kassenbon gewogene Pozitionen! Bitte ausdrucken Sie zuerst diese Kassenbon oder stornieren! ";
                                    int num = (int)fGenericError.ShowDialog();
                                    return;
                                    /*
                                    F_GenericSoru FWaageCheck = new F_GenericSoru();
                                    FWaageCheck.lblMesaj.Text = "Sie haben in geparkter Kassenbon gewogene Pozitionen! Bitte ausdrucken Sie zuerst diese Kassenbon oder stornieren! ";
                                    int num3 = (int)FWaageCheck.ShowDialog();
                                    return;*/
                                }
                            }
                        }
                        int num1 = (int)new F_Neuste().ShowDialog();
                    }
                    else
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["6"];
                        int num2 = (int)fGenericError.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = ex.Message;
                    int num = (int)fGenericError.ShowDialog();
                }
            }
        }

        private void Casio_FormClosing(object sender, FormClosingEventArgs e)
        {
            KundenDisplay();
            try
            {
                if (Program.printer != null)
                {
                    Program.printer.Release();
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("FormClosing" + ex.Message);
            }
            try
            {
                if (dsp != null)
                {
                    // ISSUE: reference to a compiler-generated method
                    dsp.ReleaseDevice();
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message + ":FormClosing");
            }
            if (scanner != null)
            {
                try
                {
                    // ISSUE: reference to a compiler-generated method
                    scanner.ReleaseDevice();
                }
                catch (Exception ex)
                {
                    int num = (int)MessageBox.Show(ex.Message + ":FormClosing2");
                }
            }
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection connection = new db().myconn();

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            if (new MySqlCommand("UPDATE usertakip SET offlinezaman=" + (object)tarih.unixdate(DateTime.Now) + " WHERE id=" + (object)Program.bedID + " and tarih=" + (object)tarih.bugunBaslangic() + " and onlinezaman<>0 and offlinezaman=0", connection).ExecuteNonQuery() <= 0)
                return;
            connection.Close();

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (Program.IsletmeAyarlar["markt"] == "2")
            {
                F_FeinKostBedinerAuswahl FUserAuswahl = new F_FeinKostBedinerAuswahl();
                FUserAuswahl.UserList = UserList;
                FUserAuswahl.ShowDialog();
                if (FUserAuswahl.SecilenUser != -1)
                {
                    FisOlustur ElemanFisi = verkauferList[FUserAuswahl.SecilenUser];

                    yeniFis = null;
                    if (ElemanFisi == null)
                    {

                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Es gibt kein Verkauf für diese Verkäfer gefunden!";
                        int num = (int)fGenericError.ShowDialog();

                    }
                    else
                    {
                        FisOlustur yeniFis1 = new FisOlustur();
                        yeniFis = YeniVerkauferFisiOlustur(ref yeniFis1, FUserAuswahl.SecilenUser);
                        yeniFis = ElemanFisi;
                        listView1.Items.Clear();
                        toolStripStatusLabel1.Text = Program.lang["13"] + " :" + verkauferList[FUserAuswahl.SecilenUser].kasiyerno;
                        KundenDisplay();
                        int num2 = 1;
                        int count = listView1.Items.Count;
                        foreach (SatisYap satisYap in yeniFis.SatisKalem)
                        {

                            listView1.Items.Add(num2.ToString());
                            listView1.Items[count].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet + "x" + satisYap.Satisfiyat.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                            DSPINFO(satisYap.UrunAd, satisYap.Gruptur != 3 ? satisYap.Adet.ToString() + " Stk." : satisYap.Adet.ToString() + "kg.", satisYap.Gruptur != 3 ? satisYap.Satisfiyat.ToString("C") + "/Stk." : satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                            ++num2;
                            count++;
                        }
                        position = num2;
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");

                    }
                }



            }
            else
            {
                if (dsp != null)
                    KundenDisplay();
                else if (serialPortKD2.IsOpen)
                    KundenDisplay();
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                LastBedienerId = Program.bedID;
                MySqlCommand mySqlCommand = new MySqlCommand("UPDATE usertakip SET offlinezaman=" + (object)tarih.unixdate(DateTime.Now) + " WHERE userid=" + (object)Program.bedID + " and tarih=" + (object)tarih.bugunBaslangic() + " and  onlinezaman<>0 and offlinezaman=0 and kasaid=" + (object)Program.kasano, connection);
                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    long lastInsertedId = mySqlCommand.LastInsertedId;
                    connection.Close();
                }
                F_AbmeldungNeue fAbmeldungNeue = new F_AbmeldungNeue();
                fAbmeldungNeue.lasBedId = LastBedienerId;
                int num1 = (int)fAbmeldungNeue.ShowDialog();
                if (fAbmeldungNeue.lasBedId == -1)
                {
                    DSPINFO("", "", "", "", "", 5, 0, 0, 0);
                    Program.bedno = -1;
                    Program.bedAdSoyad = "";
                    Program.bedID = -1;
                }
                else
                    DSPINFO("", "", "", "", "", 4, 0, 0, 0);
                if (fAbmeldungNeue.lasBedId != 0)
                {
                    int num2 = (int)new FBedChange()
                    {
                        lastid = fAbmeldungNeue.lasBedId
                    }.ShowDialog();
                }
                lblBediener.Text = "BED :" + Program.bedID.ToString();
                toolStripStatusLabel1.Text = "BEDIENER :" + Program.bedAdSoyad;
                DSPINFO("", "", "", "", "", 6, 0, 0, 0);
            }
        }

        private void btnBonPark_Click(object sender, EventArgs e)
        {
            if (Program.IsletmeAyarlar["grosshandel"] == "1")
            {
                MySqlConnection mySqlConnection = new MySqlConnection();
                MySqlConnection connection = new db().myconn();

                iss_Kunden.Musteri musteri = new iss_Kunden.Musteri();
                //musteri.db = connection;
                if (yeniFis == null)
                    return;
                fatura = new FaturaOlustur();
                fatura.myConn = connection;
                List<SatisKalem> satisKalemList = new List<SatisKalem>();
                foreach (SatisYap satisYap in yeniFis.SatisKalem)
                {
                    SatisKalem satisKalem = new SatisKalem()
                    {
                        ArtikelName = satisYap.UrunAd,
                        Barkod = satisYap.Barkod,
                        Faturaid = fatura.FaturaId,
                        GrupId = satisYap.Grubid,
                        Einheit = Convert.ToInt32(satisYap.Einheit),
                        Pfand = (long)satisYap.Fand,
                        Menge = satisYap.Adet,
                        Mwst = (double)satisYap.Mwst,
                        Stokid = (long)satisYap.UrunId,
                        UrunId = (long)satisYap.UrunId,
                        Vk = satisYap.Mwst != Program.MwStList[1] ? satisYap.Satisfiyat : satisYap.Satisfiyat,
                        Ek = satisYap.Alisfiyat
                    };
                    satisKalem.Toplamtutar = satisKalem.Menge * satisKalem.Vk;
                    if (satisYap.Fand == 1)
                        satisKalem.Pfandmi = 1;
                    satisYap.Gv_typ_id = (int)GVTypEnum.Pfand;
                    satisKalemList.Add(satisKalem);
                }
                fatura.Cihazid = "1";
                fatura.Doktype = 1;
                fatura.Musterino = yeniFis.Musterino;
                fatura.MusteriInfo = musteri.MusteriBul(yeniFis.Musterino);
                fatura.Mwst0tutar = yeniFis.mwst0Uygulanantutar;
                fatura.Mwst19 = yeniFis.mwst19miktar;
                fatura.Mwst19tutar = yeniFis.mwst19Uygulanantutar;
                fatura.Mwst7 = yeniFis.mwst7miktar;
                fatura.Mwst7tutar = yeniFis.mwst7Uygulanantutar;
                fatura.Odemesekli = yeniFis.odemesekli;
                fatura.Rabat = yeniFis.Rabat;
                fatura.Rabattutar = yeniFis.Rabattutar;
                fatura.SatisKalem = satisKalemList;
                fatura.StornoIlgi = yeniFis.StornoIlgi;
                fatura.Tarih = (double)tarih.unixdate(DateTime.Now);
                fatura.IslemTarih = (double)tarih.unixdate(DateTime.Now);
                fatura.LieferTarih = (double)tarih.unixdate(DateTime.Now);
                fatura.ToplamMwst = yeniFis.toplammwst;
                fatura.Userid = Program.bedID;
                fatura.Toplamtutar = yeniFis.toplamtutar;
                if (fatura.Musterino == 0L)
                {
                    if (fatura.Toplamtutar < 150.0)
                    {
                        fatura.Musterino = 3L;
                        fatura.Odemesekli = 0;
                    }
                    else
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "LÜTFEN MÜSTERI BARKODU OKUTUNUZ VEYA MÜSTERI KODUNU MANUEL SISTEME GIRIP TEKRAR DENEYINIZ!";
                        int num = (int)fGenericError.ShowDialog();
                        return;
                    }
                }
                fatura.Userid = yeniFis.kasiyerno;
                RechnungsNummer();
                if (!fatura.FaturaYarat(fatura.FaturaId))
                    return;
                yeniFis = (FisOlustur)null;
                if (Program.bonDruck)
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    Thread thread = new Thread(new ThreadStart(fisyaz));
                    Program.BonBeleg.OdemeTur = 1;
                    Program.BonBeleg.basilacakFatura = fatura;
                    thread.Start();
                }
                VirgulAyikla virgulAyikla = new VirgulAyikla();
                foreach (SatisKalem satisKalem in fatura.SatisKalem)
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * from artikel WHERE artikelid= " + (object)satisKalem.UrunId, connection);
                    DataTable dataTable = new DataTable();
                    dataTable.Rows.Clear();
                    mySqlDataAdapter.Fill(dataTable);
                    double num1 = 0.0;
                    double menge1 = satisKalem.Menge;
                    double menge2 = satisKalem.Menge;
                    try
                    {
                        new MySqlCommand("UPDATE artikel SET toplamstok=toplamstok-" + virgulAyikla.virgulayikla(menge2) + " WHERE artikelid=" + (object)satisKalem.UrunId, connection).ExecuteNonQuery();
                        num1 = 0.0;
                    }
                    catch (Exception ex)
                    {
                        int num2 = (int)MessageBox.Show("Fatura Olusturuldu Ancak Stok düzenlemesi sirasinda hata olustu! " + ex.Message);
                    }
                    if (nr != 0L && new MySqlCommand("UPDATE rechnungslayout SET start=" + (object)nr, connection).ExecuteNonQuery() > 0)
                    {
                        nr = 0L;
                        yeniFis = (FisOlustur)null;
                        satisYap = (SatisYap)null;
                        listView1.Items.Clear();
                        txtGiris.Text = "";
                        txtToplam.Text = "";
                        fatura = (FaturaOlustur)null;
                        musteri = null;
                    }
                }

            }
            else
            {
                KryptonButton kryptonButton = sender as KryptonButton;
                yetkiCheck = new YetkiCheck();
                yetkiCheck.YetkiKontrol(kryptonButton.Name);
                if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
                {
                    if (Program.mp == null || Program.mp == "0" || Program.mp == "")
                    {
                        if (parkedilenFis == null)
                        {
                            if (yeniFis != null && yeniFis.SatisKalem.Count > 0)
                            {
                                parkedilenFis = new FisOlustur();
                                parkedilenFis = yeniFis;
                                yeniFis = (FisOlustur)null;
                                btnBonPark.StateNormal.Back.Image = (Image)Resources.parkyasak;
                                btnBonPark.Values.Text = "GeparkteBon";
                                listView1.Items.Clear();
                                txtToplam.Text = "";
                                if (Program.displayType == "TVS")
                                {
                                    KundenDisplay();
                                }
                                else
                                {
                                    if (!serialPortKD2.IsOpen)
                                        return;
                                    KundenDisplay();
                                }
                            }
                            else
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["14"];
                                int num = (int)fGenericError.ShowDialog();
                            }
                        }
                        else if (yeniFis == null)
                        {
                            F_GenericSoru fGenericSoru = new F_GenericSoru();
                            fGenericSoru.lblMesaj.Text = Program.lang["15"];
                            int num1 = (int)fGenericSoru.ShowDialog();
                            if (!fGenericSoru.sonuc)
                                return;
                            yeniFis = parkedilenFis;
                            parkedilenFis = (FisOlustur)null;
                            int num2 = 1;
                            listView1.Items.Clear();
                            foreach (SatisYap satisYap in yeniFis.SatisKalem)
                            {
                                int count = listView1.Items.Count;
                                listView1.Items.Add(num2.ToString());
                                listView1.Items[count].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet + "x" + satisYap.Satisfiyat.ToString("C") + ")");
                                listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                                listView1.Items[count].Tag = satisYap.Barkod;
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                                DSPINFO(satisYap.UrunAd, satisYap.Gruptur != 3 ? satisYap.Adet.ToString() + " Stk." : satisYap.Adet.ToString() + "kg.", satisYap.Gruptur != 3 ? satisYap.Satisfiyat.ToString("C") + "/Stk." : satisYap.Satisfiyat.ToString("C") + "/kg.", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                ++num2;
                            }
                            yeniFis.FisiKapat();
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");

                            position = listView1.Items.Count + 1;
                            if (listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            btnBonPark.StateNormal.Back.Image = (Image)Resources.park;
                            btnBonPark.Values.Text = "BonPARKEN";
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["16"];
                            int num = (int)fGenericError.ShowDialog();
                        }
                    }
                    else if (yeniFis != null && yeniFis.SatisKalem.Count > 0)
                    {
                        yeniFis.Parknumber = new Random().Next(0, 10000);
                        yeniFis.tarih = (double)tarih.unixdate(DateTime.Now);
                        MultiParkBons.Add(yeniFis);
                        yeniFis = (FisOlustur)null;
                        btnBonPark.StateNormal.Back.Image = (Image)Resources.parking;
                        btnBonPark.StateNormal.Back.ImageStyle = PaletteImageStyle.CenterLeft;
                        btnBonPark.Values.Text = MultiParkBons.Count.ToString();
                        btnBonPark.StateNormal.Content.ShortText.TextH = PaletteRelativeAlign.Far;
                        listView1.Items.Clear();
                        txtToplam.Text = "";
                    }
                    else
                    {
                        F_Multiparking fMultiparking = new F_Multiparking();
                        fMultiparking.geparkteBonList = MultiParkBons;
                        int num1 = (int)fMultiparking.ShowDialog();
                        if (fMultiparking.geParkteBon == null)
                            return;
                        yeniFis = fMultiparking.geParkteBon;
                        parkedilenFis = (FisOlustur)null;
                        int num2 = 1;
                        listView1.Items.Clear();
                        foreach (SatisYap satisYap in yeniFis.SatisKalem)
                        {
                            int count = listView1.Items.Count;
                            listView1.Items.Add(num2.ToString());
                            listView1.Items[count].SubItems.Add(satisYap.UrunAd.ToString() + "(" + (object)satisYap.Adet + "x" + satisYap.Satisfiyat.ToString("C") + ")");
                            listView1.Items[count].SubItems.Add(satisYap.Toplamtutar.ToString("C"));
                            KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString());
                            listView1.Items[count].Tag = satisYap.Barkod;
                            ++num2;
                        }
                        yeniFis.FisiKapat();
                        txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                        position = listView1.Items.Count + 1;

                        if (listView1.Items.Count > 0)
                            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        MultiParkBons.RemoveAll((Predicate<FisOlustur>)(D => D.Parknumber == yeniFis.Parknumber));
                        btnBonPark.StateNormal.Back.Image = (Image)Resources.parking;
                        btnBonPark.StateNormal.Back.ImageStyle = PaletteImageStyle.CenterLeft;
                        btnBonPark.Values.Text = MultiParkBons.Count.ToString();
                        btnBonPark.StateNormal.Content.ShortText.TextH = PaletteRelativeAlign.Far;
                    }
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["6"];
                    int num = (int)fGenericError.ShowDialog();
                }
            }
        }

        private void RechnungsNummer()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection conn = new db().conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            int num = 0;
            string str1 = "";
            string str2 = "";

            MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT max(start)+1, lange, prefix, suffix FROM layoutrechnung", conn).ExecuteReader();
            if (mySqlDataReader.HasRows)
            {
                mySqlDataReader.Read();
                nr = (long)mySqlDataReader.GetInt32(0);
                num = (int)mySqlDataReader.GetInt16(1);
                str1 = mySqlDataReader.GetString(2);
                str2 = mySqlDataReader.GetString(3);
            }
            mySqlDataReader.Close();
            string str3 = "";
            int length = nr.ToString().Length;
            if (length < num)
                str3 = new string('0', num - length);
            fatura.FaturaNo = str1 + str3 + (object)nr + str2;

        }

        private void LieferscheinNummer()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            MySqlConnection conn = new db().conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            int num = 0;
            string str1 = "";
            string str2 = "";

            MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT max(start)+1, lange, prefix, suffix FROM lieferscheinlayout", conn).ExecuteReader();
            if (mySqlDataReader.HasRows)
            {
                mySqlDataReader.Read();
                nrLif = (long)mySqlDataReader.GetInt32(0);
                num = (int)mySqlDataReader.GetInt16(1);
                str1 = mySqlDataReader.GetString(2);
                str2 = mySqlDataReader.GetString(3);
            }
            mySqlDataReader.Close();
            string str3 = "";
            int length = nrLif.ToString().Length;
            if (length < num)
                str3 = new string('0', num - length);
            fatura.FaturaNo = str1 + str3 + (object)nrLif + str2;

        }

        private void btnRabat_Click(object sender, EventArgs e)
        {
            if (yeniFis == null)
                return;
            if (yeniFis.SatisKalem.Count > 0)
            {
                double result = 0.0;
                KryptonButton kryptonButton = sender as KryptonButton;
                yetkiCheck = new YetkiCheck();
                yetkiCheck.YetkiKontrol(kryptonButton.Name);
                if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
                {
                    if (double.TryParse(txtGiris.Text, out result))
                    {
                        if (seciliItem != -1)
                        {
                            RabattMain rabattMain = new RabattMain();
                            rabattMain.RabattAlani = 1;
                            rabattMain.Grupid = 999;
                            if (listView1.Items[seciliItem].Text != "")
                            {
                                double adet = yeniFis.SatisKalem[seciliItem].Adet;
                                double num1 = yeniFis.SatisKalem[seciliItem].Birimkar / yeniFis.SatisKalem[seciliItem].Adet;
                                double num2;
                                if (Program.GlobalAyarlar["RABAT"] == 1)
                                {
                                    if (result > 100.0)
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = Program.lang["11"];
                                        int num3 = (int)fGenericError.ShowDialog();
                                        return;
                                    }
                                    num2 = -yeniFis.SatisKalem[seciliItem].Toplamtutar * (result / 100.0);
                                    rabattMain.RabattTyp = 0;
                                    rabattMain.TotalRabattMenge = num2;
                                }
                                else
                                {
                                    if (result > yeniFis.SatisKalem[seciliItem].Toplamtutar)
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = Program.lang["11"];
                                        int num3 = (int)fGenericError.ShowDialog();
                                        return;
                                    }
                                    num2 = -(yeniFis.SatisKalem[seciliItem].Toplamtutar - result);
                                    rabattMain.RabattTyp = 1;
                                    rabattMain.TotalRabattMenge = num2;
                                }
                                yeniFis.SatisKalem[seciliItem].ProzisyonRabatBetrag = num2;
                                Tarih tarih = new Tarih();
                                satisYap = new SatisYap();
                                satisYap.Adet = 1.0;
                                satisYap.Fisno = yeniFis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = yeniFis.SatisKalem[seciliItem].Mwst;
                                satisYap.Gruptur = yeniFis.SatisKalem[seciliItem].Gruptur;
                                satisYap.Satisfiyat = num2;
                                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                                satisYap.Angebotvarmi = 0;
                                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(num2, 2);
                                satisYap.UrunId = new Decimal(0);
                                satisYap.UrunAd = yeniFis.SatisKalem[seciliItem].UrunAd + " Pos. Rabatt";
                                satisYap.Birimkar = 0.0;
                                satisYap.Grubid = 999;
                                satisYap.Fand = 0;
                                satisYap.Fand2 = 0;
                                satisYap.Birimid = 1.0;
                                satisYap.Gv_typ_id = (int)GVTypEnum.Rabatt;
                                satisYap.PozRabatMainId = seciliItem;
                                yeniFis.SatisKalem.Add(satisYap);
                                rabattMain.RabatArt = 1;
                                rabattMain.RabatMenge = result;
                                rabattMain.RabatName = satisYap.UrunAd;
                                rabattMain.TotalRabattMenge = num2;
                                if (satisYap.Mwst == Program.MwStList[1])
                                {
                                    rabattMain.Mwst7Betrag = num2;
                                    rabattMain.Mwst7 = num2 - num2 / 1.07;
                                }
                                else if (satisYap.Mwst == Program.MwStList[2])
                                {
                                    rabattMain.Mwst19Betrag = num2;
                                    rabattMain.Mwst19 = num2 - num2 / 1.19;
                                }
                                else if (satisYap.Mwst == Program.MwStList[0])
                                    rabattMain.Mwst0Betrag = num2;
                                yeniFis.RabatList.Add(rabattMain);
                                int count = listView1.Items.Count;
                                listView1.Items.Add("");
                                listView1.Items[count].SubItems.Add(yeniFis.SatisKalem[seciliItem].UrunAd + " Pos. Rabatt");
                                listView1.Items[count].SubItems.Add(num2.ToString("C"));
                                yeniFis.FisiKapat();
                                txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                                txtGiris.Text = "";
                                if (dsp != null)
                                {
                                    if (Program.displayType != "IBM")
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("C"));
                                    else
                                        KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString() + (object)'Õ');
                                }
                                else if (Program.displayType == "TVS")
                                    DSPINFO(satisYap.UrunAd, 1.ToString(), "", satisYap.Toplamtutar.ToString("C"), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                                seciliItem = -1;
                            }
                            else
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["68"] + "\n" + Program.lang["69"];
                                int num = (int)fGenericError.ShowDialog();
                            }
                        }
                        else
                        {
                            //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                            //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                            if (yeniFis == null)
                                return;
                            RabattMain rabattMain = new RabattMain();
                            rabattMain.RabatName = "Allgemeinrabatt";
                            rabattMain.RabattAlani = 0;
                            if (Program.GlobalAyarlar["RABAT"] == 1)
                            {
                                if (Program.GlobalAyarlar["KOMMA"] == 0)
                                {
                                    // result = result / 100;


                                }
                                if (result > 100.0)
                                {
                                    F_GenericError fGenericError = new F_GenericError();
                                    fGenericError.lblMesaj.Text = Program.lang["11"];
                                    int num3 = (int)fGenericError.ShowDialog();
                                    return;
                                }
                                KDbirinciSatiraYaz(" % RABAT!", result.ToString("F"));
                                rabattMain.RabattTyp = 0;
                            }
                            else
                            {
                                if (Program.GlobalAyarlar["KOMMA"] == 0)
                                    result = result / 100;
                                if (result > yeniFis.toplamtutar)
                                {
                                    F_GenericError fGenericError = new F_GenericError();
                                    fGenericError.lblMesaj.Text = Program.lang["11"];
                                    int num3 = (int)fGenericError.ShowDialog();
                                    return;
                                }
                                KDbirinciSatiraYaz("  RABAT! ", result.ToString("F"));
                                rabattMain.RabattTyp = 1;
                            }
                            rabattMain.RabatArt = 0;
                            rabattMain.RabatMenge = result;
                            yeniFis.RabatList.Add(rabattMain);
                            rabattMain.Grupid = 7;
                            yeniFis.FisiKapat();
                            DSPINFO("ALLG. RABATT " + result.ToString("F") + "%", "", "", "", "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0);
                            txtToplam.Text = "TOTAL : " + yeniFis.toplamtutar.ToString("C");
                            if (dsp != null)
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            else if (serialPortKD2.IsOpen)
                                KDikinciSatiraYaz("TOTAL : " + yeniFis.toplamtutar.ToString("#0.00"));
                            txtGiris.Text = "";
                        }
                    }
                    else
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = Program.lang["11"];
                        int num = (int)fGenericError.ShowDialog();
                    }
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = Program.lang["6"];
                    int num = (int)fGenericError.ShowDialog();
                }
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["17"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void btnOrnek_Click(object sender, EventArgs e)
        {
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
        }

        private void kryptonButton18_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi)
            {
                if (yetkiCheck.Durum == 1L)
                    goto label_3;
            }
            if (yetkiCheck.YetkiTanimlimi)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
                return;
            }
        label_3:
            try
            {
                if (Program.cashdrawerSO != "")
                    CashDrawerManualOpen();
                else if (Program.printerType == "citizen")
                {
                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                }
                else
                {
                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                }
                new NullBon().NullBonAdd(Program.bedID, new DbParameter()
                {
                    DbName = Program.dbName,
                    Host = Program.ServerIp
                }, Program.BedSitzungId);
            }
            catch
            {
            }
        }

        private void kryptonButton19_Click(object sender, EventArgs e)
        {
            /* KryptonButton kryptonButton = sender as KryptonButton;
             yetkiCheck = new YetkiCheck();
             yetkiCheck.YetkiKontrol(kryptonButton.Name);
             if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
             {
                 switch (Program.DruckerMode)
                 {
                     case 1:
                         if (Program.bonDruck)
                         {
                             Program.bonDruck = false;
                             kryptonButton19.StateNormal.Back.Image = (Image)Resources.printer_red;
                             break;
                         }
                         Program.bonDruck = true;
                         kryptonButton19.StateNormal.Back.Image = (Image)Resources.printer_green;
                         break;
                     case 3:
                         FisOlustur fisOlustur = new FisOlustur();
                         if (!fisOlustur.FisCagir1(Convert.ToInt32(lblBonNo.Text)))
                             break;
                         Program.BonBeleg.basilacakFis = fisOlustur;
                         Program.BonBeleg.OdemeTur = fisOlustur.odemesekli;
                         Program.BonBeleg.basilacakFis.odemesekli = fisOlustur.odemesekli;
                         fisyaz();
                         break;
                 }
             }
             else
             {
                 F_GenericError fGenericError = new F_GenericError();
                 fGenericError.lblMesaj.Text = Program.lang["6"];
                 int num = (int)fGenericError.ShowDialog();
             }*/
        }



        private void btnNeuProdukt_Click(object sender, EventArgs e)
        {
            KryptonButton btnTik = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(btnTik.Name);//
            if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
            {
                try
                {
                    if (Program.oposscanner != null)
                    {
                        scanner.DataEvent -= new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);

                    }
                    if (Program.oposscanner2 != null)
                    {
                        scanner2.DataEvent -= new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
                    }
                    else if (serialPort1.IsOpen == true)
                    {
                        serialPort1.Close();
                        //serialPort1.Dispose();
                    }
                }
                catch
                {
                }
                finally
                {

                }
                if ((Program.IsletmeAyarlar["markt"] == "1") || (Program.IsletmeAyarlar["markt"] == "2"))
                {
                    try
                    {
                        F_ArtikelGiris frmyeniArtikel = new F_ArtikelGiris();
                        frmyeniArtikel.ShowDialog();
                        ButtonOlustur();
                        if (Program.oposscanner != null)
                        {
                            scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);


                        }
                        if (Program.oposscanner2 != null)
                        {
                            scanner2.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
                            scanner2.DataEventEnabled = true;
                        }
                        else if (serialPort1.IsOpen == false)
                        {
                            serialPort1.Open();
                            //  serialPort1.Dispose();
                        }
                    }
                    catch
                    {
                    }
                }
                else if ((Program.IsletmeAyarlar["markt"] == "0"))
                {
                    try
                    {
                        F_ArtikelGirisRest frmyeniArtikel = new F_ArtikelGirisRest();
                        frmyeniArtikel.ShowDialog();
                        ButtonOlustur();
                        if (Program.oposscanner != null)
                        {
                            scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);


                        }
                        if (Program.oposscanner2 != null)
                        {
                            scanner2.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
                            scanner2.DataEventEnabled = true;
                        }
                        else if (serialPort1.IsOpen == false)
                        {
                            serialPort1.Open();
                            //  serialPort1.Dispose();
                        }
                    }
                    catch
                    {
                    }
                }

            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["6"];
                frmerror.ShowDialog();
            }
            LoadFavProduct();
            /* double ss = 0;
             if (double.TryParse(txtGiris.Text, out ss))
             {
             }
             else
             {
                 ss = 1;
             }
             gelenBarkod = txtGiris.Text;
             barkodluUrunEkle();*/
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (!CheckInternet())
                return;
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
            connectionStringBuilder.UserID = "iss_server";
            connectionStringBuilder.Password = "Ab3420351?";
            connectionStringBuilder.Server = "46.163.69.230";
            connectionStringBuilder.Database = "admin_iss";
            newServer = new MySqlConnection();
            newServer.ConnectionString = connectionStringBuilder.ToString();
            try
            {
                newServer.Open();
            }
            catch (Exception ex)
            {
            }
            new Thread((ThreadStart)(() => ServerIpYaz())).Start();
        }

        private bool CheckInternet()
        {
            try
            {
                return new Ping().Send("www.google.com", 1000).Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private void ServerIpYaz()
        {
            bool flag = false;
            string externalIp = Casio.GetExternalIP();
            if (externalIp == null)
                return;
            try
            {
                MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT * FROM kunden WHERE kod= " + Program.IsletmeAyarlar["kod"], newServer).ExecuteReader();
                if (mySqlDataReader.HasRows)
                    flag = true;
                mySqlDataReader.Close();
                if (flag)
                {
                    MySqlCommand mySqlCommand = new MySqlCommand();
                    mySqlCommand.Parameters.AddWithValue("@tutar", (object)0);
                    string str = "UPDATE kunden SET ip='" + externalIp + "', datum=" + (object)tarih.unixdate(DateTime.Now) + ", kasatoplam=@tutar WHERE kod=" + Program.IsletmeAyarlar["kod"];
                    mySqlCommand.Connection = newServer;
                    mySqlCommand.CommandText = str;
                    if (mySqlCommand.ExecuteNonQuery() <= 0)
                        ;
                }
                else if (new MySqlCommand("INSERT INTO kunden SET ip='" + externalIp + "', datum=" + (object)tarih.unixdate(DateTime.Now) + ", kod=" + Program.IsletmeAyarlar["kod"] + ", kasatoplam=" + (object)0, newServer).ExecuteNonQuery() > 0)
                {
                    int num1 = (int)MessageBox.Show("OK!");
                }
                else
                {
                    int num2 = (int)MessageBox.Show("ERROR!");
                }
            }
            catch
            {
            }
        }

        public static string GetExternalIP()
        {
            try
            {
                return new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}").Matches(new WebClient().DownloadString("http://checkip.dyndns.org/"))[0].ToString();
            }
            catch
            {
                return (string)null;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (brw_reload != 0)
                return;
            Enabled = false;
            WindowState = FormWindowState.Minimized;
            brw_reload = 1;
            db db = new db();
            F_LicenceError fLicenceError = new F_LicenceError();
            fLicenceError.BringToFront();
            fLicenceError.myCon = db.myconn();
            fLicenceError.lblMesaj.Text = "SYSTEM ERROR! Error Code:S_1\nProgram werde beenden! Bitte wenden Sie ISS POS Service team an!\n+49 2202 7059900";
            int num = (int)fLicenceError.ShowDialog();
            brw_reload = 0;
            Enabled = true;
            WindowState = FormWindowState.Maximized;
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
        }

        private void ZVTIslem(double p)
        {
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
        }

        private void Casio_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void Barkodbul(string p)
        {
            if (!(p != ""))
                return;
            Artikel artikel = new Artikel();
            artikel.ArtikelBulID((Decimal)Convert.ToInt32(p));
            if (!artikel.urunvarmi)
                return;
            barkodDegerlendir(artikel.BarkodNo);
        }

        private void txtGiris_KeyDown(object sender, KeyEventArgs e)
        {
            string text = txtGiris.Text;
            try
            {
                if (e.KeyCode == Keys.F12)
                {
                    if (!(txtGiris.Text != ""))
                        return;
                    txtGiris.Text = "";
                    barkodDegerlendir(text);
                }
                else if (e.KeyCode == Keys.F11)
                {
                    txtGiris.Text = "";
                    Barkodbul(text);
                }
                else
                {
                    if (e.KeyCode != Keys.Return)
                        return;
                    txtGiris.Text = "";
                    barkodDegerlendir(text.Split('/')[4].Remove(4, 3).Insert(4, "@"));
                    txtGiris.Focus();
                }
            }
            catch
            {
            }
        }

        private void timerRuckgeld_Tick(object sender, EventArgs e)
        {
            ++RuckGeldCounter;
        }

        public void geraboRedeem(string Kartcode)
        {
            GeraboPunkteeinlosung geraboPunkteeinlosung = new GeraboPunkteeinlosung();
            geraboKundenKarteClass.GeraboRedeemPremium(geraboKundenKarteClass.Url, Kartcode, "", "", "");
        }

        public void gerabo(string Kartcode)
        {
            AccountInfo = geraboKundenKarteClass.GetKundeInfo(geraboKundenKarteClass.Url, Kartcode);
            if (AccountInfo.meta.code != 3)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = AccountInfo.meta.message + "\n" + Kartcode;
                int num = (int)fGenericError.ShowDialog();
                txtGiris.Text = "";
            }
            float size = lblParaUstu.Font.Size;
            lblParaUstu.Font = new Font(lblParaUstu.Font.Name, 10f);
            lblParaUstu.Text = "Guthaben:" + (object)(Convert.ToDouble(AccountInfo.account.Credits) / 100.0) + " Punkte:" + (object)AccountInfo.account.Points;
            lblParaUstu.Font = new Font(lblParaUstu.Font.Name, size);
            TFTYazBarVerkauf("Kundenkarte:Guthaben:" + (Convert.ToDouble(AccountInfo.account.Credits) / 100.0).ToString("C"), " Punkte:" + (object)AccountInfo.account.Points);
        }

        public void GeraboNeueVerkauf()
        {
            GeraboRoot geraboRoot1 = new GeraboRoot();
            GeraboRoot geraboRoot2 = geraboKundenKarteClass.CollectPoints(geraboKundenKarteClass.Url, AccountInfo.code, yeniFis.AngebotsuzToplamTutar.ToString("N").Remove(yeniFis.AngebotsuzToplamTutar.ToString("N").Length - 3, 1));
            AccountInfo = geraboKundenKarteClass.GetKundeInfo(geraboKundenKarteClass.Url, AccountInfo.code);
            if (yeniFis == null)
                return;
            yeniFis.GeraboObject = AccountInfo;
            yeniFis.KazanilanPuan = (double)geraboRoot2.valuediff;
            yeniFis.EskiPuanToplamı = (double)(geraboRoot2.value - geraboRoot2.valuediff);
        }

        private void geraboSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = geraboSerialPort.BytesToRead;
            byte[] numArray = new byte[bytesToRead];
            geraboSerialPort.Read(numArray, 0, bytesToRead);
            int num1 = (int)MessageBox.Show("geraboSerialPort" + Encoding.ASCII.GetString(numArray));
            List<int> intList = new List<int>();
            for (int index = 0; index < ((IEnumerable<byte>)numArray).Count<byte>(); ++index)
            {
                if (numArray[index] == (byte)83 && ((IEnumerable<byte>)numArray).Count<byte>() != 17)
                    index = 6;
                else if (numArray[index] == (byte)83 && ((IEnumerable<byte>)numArray).Count<byte>() == 17)
                    index = 3;
                else if (numArray[index] > (byte)47 && numArray[index] < (byte)58)
                    intList.Add((int)numArray[index]);
            }
            byte[] bytes = new byte[intList.Count];
            int index1 = 0;
            foreach (int num2 in intList)
            {
                bytes[index1] = Convert.ToByte(num2);
                ++index1;
            }
            string Kartcode = Encoding.ASCII.GetString(bytes);
            ReadGeraboKart = true;
            gerabo(Kartcode);
        }

        private void txtGiris_TextChanged(object sender, EventArgs e)
        {
        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {
        }



        public delegate void KundenDisplayForm(string Sonsatilan, string Total);

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            try
            {
                string Windir = "", OskDir = "";
                Windir = Environment.GetEnvironmentVariable("windir");
                Process.Start(@"c:\Windows\Sysnative\cmd.exe", "/c osk.exe");
                Thread.Sleep(1000);
                foreach (var process in Process.GetProcessesByName("cmd"))
                {
                    process.Kill();
                }

                /*
                 //System.Diagnostics.Process.Start("osk.exe");
                 OskDir = Windir + @"\System32\osk.exe";
                 System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                 string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                 string keyboardPath = Path.Combine(progFiles, "TabTip.exe");

                  Process.Start(keyboardPath);*/
            }
            catch (Exception xx)
            {
                System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                // MessageBox.Show(xx.Message);
            }
        }

        private void btnKiste_Click(object sender, EventArgs e)
        {
            KoliKiste = true;
            lblParaUstu.Text = "Kolli-Kiste";
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnKameraCheck_Click(object sender, EventArgs e)
        {
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videosources != null)
            {
                F_CameraWagenCheck frmWagenCheck = new F_CameraWagenCheck();
                frmWagenCheck.ShowDialog();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = "Hinweis: Für diese Kasse ist derzeit keine Kamera verbunden! ";
                int num = (int)fGenericError.ShowDialog();
            }

        }

        private void btnAIimage_Click(object sender, EventArgs e)
        {
            if (Program.ProgramAyarlar["AILibImage"] != null)
            {
                if (Program.ProgramAyarlar["AILibImage"] != "")
                {
                    Bitmap snap = null;
                    if ((Program.ProgramAyarlar["Cam1"] != "") && (Program.ProgramAyarlar["Cam1"] != null))
                    {
                        lock (_frameLock)
                        {
                            if (_lastFrame != null)
                                snap = (Bitmap)_lastFrame.Clone();
                        }
                    }


                }
            }
        }
        public void GenericPredict(string path)
        {
            if (Program.ProgramAyarlar["AILibImage"] == "Rx") //Ronson
            {
                iss_smart_AI iss_Smart_AI = new iss_smart_AI();
                if (File.Exists(path))
                {
                    string Result = iss_Smart_AI.processing_image_from_path(path);
                    JObject obj = JObject.Parse(Result);

                    PredictionResult result = new PredictionResult
                    {
                        Predicted_Label = (string)obj["predicted_label"],
                        Scores = obj["scores"].Select(s => new Score
                        {
                            Code = (string)s[0],
                            Value = (string)s[1]
                        })
                            .ToList()
                    };

                    if (result.Predicted_Label != null)
                    {
                        if (result.Predicted_Label != "")
                        {
                            if (Convert.ToDouble(result.Scores[0].Value) > 0.75)
                            {
                                ObstGemusePLU(result.Scores[0].Code);
                            }
                            //richTextBox1.AppendText(result.Predicted_Label + "(" + result.Scores[0].Value.ToString() + ")\n");
                        }
                        else
                        {
                            string INSql = "";
                            for(int i=0; i<result.Scores.Count;i++)
                            {
                                if (Convert.ToDouble(result.Scores[i].Value)>0.40)
                                {
                                    insql 
                                }
                            }

                        }
                    }
                }
            }
        }
        private void kryptonButton47_Click(object sender, EventArgs e)
        {
            PreisCheck = true;
            lblParaUstu.Text = "Preis-Check";
        }


    }
}
