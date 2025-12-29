using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using iss_Rabat;
using iss_tse_v2;
using System.Diagnostics;
using iss_Kunden;
using tar;
using System.Threading;


namespace IS_KASSE
{
    public class FisOlustur
    {
        Tarih suantarih = new Tarih();
        MySqlConnection myConn1 = new MySqlConnection();
        private long satisAnaId = 0, kioskBestellID=0;
        Thread myTSEThread;

        public double paraustu = 0, verilenpara = 0, toplamtutar = 0, toplammwst = 0, mwst7Uygulanantutar = 0, mwst7miktar = 0, mwst19Uygulanantutar = 0, mwst19miktar = 0;
        public double mwst0Uygulanantutar = 0, tarih = 0, angebotsuztoplamtutar, brutto = 0, brutto7 = 0, brutto19 = 0;
        public int odemesekli = -1, kasiyerno = -1;
        private long _stornoIlgi;
        private double _rabat;
        private double aktuelgun = 0;

        VirgulAyikla vA = new VirgulAyikla();
        private List<SatisYap> _satisKalem = new List<SatisYap>();
        private double _rabattutar;

        public delegate void lblParaUstuYaz(string info);
        public event lblParaUstuYaz Infoevent;

        string StWaTSEMessage = "", StWaBonKopfMessage = "";

        int selfTestStarted = 0;
        int TSESearchCounter = 0;

        public double Rabattutar
        {
            get { return _rabattutar; }
            set { _rabattutar = value; }
        }
        private List<RabattMain> _rabatList = new List<RabattMain>();

        public List<RabattMain> RabatList
        {
            get { return _rabatList; }
            set { _rabatList = value; }
        }
        public double Rabat
        {
            get { return _rabat; }
            set { _rabat = value; }
        }
        public long StornoIlgi
        {
            get { return _stornoIlgi; }
            set { _stornoIlgi = value; }
        }
        public List<SatisYap> SatisKalem
        {
            get { return _satisKalem; }
            set
            {

                _satisKalem = value;
            }
        }
        public long SatisAnaId
        {
            get { return satisAnaId; }
            set { satisAnaId = value; }
        }
        private long _musterino;

        public long Musterino
        {
            get { return _musterino; }
            set { _musterino = value; }
        }

        private double _kazanilanPuan;

        public double KazanilanPuan
        {
            get { return _kazanilanPuan; }
            set { _kazanilanPuan = value; }
        }

        private double _harcananPuan;

        public double HarcananPuan
        {
            get { return _harcananPuan; }
            set { _harcananPuan = value; }
        }

        private double _harcananPuanKarsiligiPara;

        public double HarcananPuanKarsiligiPara
        {
            get { return _harcananPuanKarsiligiPara; }
            set { _harcananPuanKarsiligiPara = value; }
        }

        private double _eskiPuanToplamı;

        public double EskiPuanToplamı
        {
            get { return _eskiPuanToplamı; }
            set { _eskiPuanToplamı = value; }
        }

        private double _kredit;

        public double Kredit
        {
            get { return _kredit; }
            set { _kredit = value; }
        }

        private double _angebotsuzToplamTutar;

        public double AngebotsuzToplamTutar
        {
            get { return _angebotsuzToplamTutar; }
            set { _angebotsuzToplamTutar = value; }
        }

        private iss_Kunden.Musteri _musteri;

        public iss_Kunden.Musteri Musteri
        {
            get { return _musteri; }
            set { _musteri = value; }
        }

        private double bruttoToplam;

        public double BruttoToplam
        {
            get { return bruttoToplam; }
            set { bruttoToplam = value; }
        }

        private int _indirimturu;

        public int Indirimturu
        {
            get { return _indirimturu; }
            set { _indirimturu = value; }
        }
        private double _PuanRabat;

        public double PuanRabat
        {
            get { return _PuanRabat; }
            set { _PuanRabat = value; }
        }
        private int _bewirtung;

        public int Bewirtung
        {
            get { return _bewirtung; }
            set { _bewirtung = value; }
        }

        private double _teilEC;

        public double TeilEC
        {
            get { return _teilEC; }
            set { _teilEC = value; }
        }

        private double _teilCheck;

        public double TeilCheck
        {
            get { return _teilCheck; }
            set { _teilCheck = value; }
        }

        private double _toplamBar;

        public double ToplamBar
        {
            get { return _toplamBar; }
            set { _toplamBar = value; }
        }
        private double _toplamEc;

        public double ToplamEc
        {
            get { return _toplamEc; }
            set { _toplamEc = value; }
        }
        private double _toplamStorno;

        public double ToplamStorno
        {
            get { return _toplamStorno; }
            set { _toplamStorno = value; }
        }
        private double _toplamScheck;

        public double ToplamScheck
        {
            get { return _toplamScheck; }
            set { _toplamScheck = value; }
        }

        private double _gutschein;

        public double Gutschein
        {
            get { return _gutschein; }
            set { _gutschein = value; }
        }

        private List<long> _aktivierteGutschId;
        public List<long> AktivierteGutschId = new List<long>();
        private int _parknumber;

        public int Parknumber
        {
            get { return _parknumber; }
            set { _parknumber = value; }
        }

        private bool _dublikat;

        public bool Dublikat
        {
            get { return _dublikat; }
            set { _dublikat = value; }
        }

        private GeraboAccountInfo _geraboObject;

        public GeraboAccountInfo GeraboObject
        {
            get { return _geraboObject; }
            set { _geraboObject = value; }
        }

        private int _masaID;

        public Int32 MasaID
        {
            get { return _masaID; }
            set { _masaID = value; }
        }

        private long _masaDBid;

        public long MasaDBid
        {
            get { return _masaDBid; }
            set { _masaDBid = value; }
        }
        private string _masano;

        public string Masano
        {
            get { return _masano; }
            set { _masano = value; }
        }

        private int _kasaNo;

        public int KasaNo
        {
            get { return _kasaNo; }
            set { _kasaNo = value; }
        }
        private int _bon_typ_id;

        public int Bon_typ_id
        {
            get { return _bon_typ_id; }
            set { _bon_typ_id = value; }
        }
        private string _bon_typ;

        public string Bon_typ
        {
            get { return _bon_typ; }
            set { _bon_typ = value; }
        }
        private string _bon_name;

        public string Bon_name
        {
            get { return _bon_name; }
            set { _bon_name = value; }
        }

        private long _datum_start;

        public long Datum_start
        {
            get { return _datum_start; }
            set { _datum_start = value; }
        }
        private ulong _tsestartedTransaction;

        public ulong TseStartedTransaction
        {
            get { return _tsestartedTransaction; }
            set { _tsestartedTransaction = value; }
        }

        private ulong _tseSignaturzahler;

        public ulong TseSignaturzahler
        {
            get { return _tseSignaturzahler; }
            set { _tseSignaturzahler = value; }
        }

        private ulong _tseLogtime;

        public ulong TseLogtime
        {
            get { return _tseLogtime; }
            set { _tseLogtime = value; }
        }

        private string _tseFinishSignatur;

        public string TseFinishSignatur
        {
            get { return _tseFinishSignatur; }
            set { _tseFinishSignatur = value; }
        }

        private string _tseProcessData;

        public string TseProcessData
        {
            get { return _tseProcessData; }
            set { _tseProcessData = value; }
        }

        private ulong _transactionsnummer;

        public ulong Transactionsnummer
        {
            get { return _transactionsnummer; }
            set { _transactionsnummer = value; }
        }

        private ulong _tseLogTimeStart;

        public ulong TseLogTimeStart
        {
            get { return _tseLogTimeStart; }
            set { _tseLogTimeStart = value; }
        }

        private string _tseseriennr;

        public string Tseseriennr
        {
            get { return _tseseriennr; }
            set { _tseseriennr = value; }
        }

        private string _tsePublicKey;

        public string TsePublicKey
        {
            get { return _tsePublicKey; }
            set { _tsePublicKey = value; }
        }
        private string _sigAlg;

        public string SigAlg
        {
            get { return _sigAlg; }
            set { _sigAlg = value; }
        }

        private List<SellPinList> _pinlistSell = new List<SellPinList>();

        public List<SellPinList> PinlistSell
        {
            get { return _pinlistSell; }
            set { _pinlistSell = value; }
        }
        private List<SoldPinlist> _pinlistSold = new List<SoldPinlist>();

        public List<SoldPinlist> PinlistSold
        {
            get { return _pinlistSold; }
            set { _pinlistSold = value; }
        }


        private int _syncServer;

        public int SyncServer
        {
            get { return _syncServer; }
            set { _syncServer = value; }
        }
        Log log = new Log();

        private long _localbonid;

        public long Localbonid
        {
            get { return _localbonid; }
            set { _localbonid = value; }
        }
        private string _tseLastErrorNo;

        public string TseLastErrorNo
        {
            get { return _tseLastErrorNo; }
            set { _tseLastErrorNo = value; }
        }
        private string _tseLastErrorMessage;

        public string TseLastErrorMessage
        {
            get { return _tseLastErrorMessage; }
            set { _tseLastErrorMessage = value; }
        }

        public int isSelbstKioskBestellung {get; set;}
        public int isSelbstCheckoutBon { get; set; }

        public long kioskSelbstBestellID { get; set; }

        public Dictionary<string, int> angebotKarar = new Dictionary<string, int>();
        public  string CardTypID {  get; set; }   
        public int CardPaymentTypID { get; set; }
        public int tseID { get; set; }
        public FisOlustur()
        {

        }
        public FisOlustur ShallowCopy()
        {
            return (FisOlustur)this.MemberwiseClone();
        }
        public void FisYarat(int MusteriNo)
        {
            RabatList = new List<RabattMain>();
            if (Program.IsletmeAyarlar["festrabatt"] != "0")
            {
                RabattMain rabattMain = new RabattMain();
                rabattMain.RabatName = "Fix-Rabatt";
                rabattMain.RabattAlani = 0; //alggemein (nicht pos)
                rabattMain.RabattTyp = 0; //prozent
                rabattMain.RabatArt = 0; // ra´batt turu, genel 
                rabattMain.RabatMenge = Convert.ToInt32(Program.IsletmeAyarlar["festrabatt"]);
                rabattMain.Grupid = 7;
                this.RabatList.Add(rabattMain);

            }
            this.Bon_typ_id = (int)Bontype.Beleg;
            this.Bon_typ = "Beleg";
            this.Datum_start = suantarih.unixdate(DateTime.Now);
            this.MasaID = 0;

            
            if (Program.TSE == "1")
            {
                Program.TSELastUseDatetime = suantarih.unixdate(DateTime.Now);
            initmain:
                try
                {
                    WormTransactionResponse response;
                    //start transc (0,1,2)
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    WormReturnClass wormreturn =  new WormReturnClass();
                    int returnedCode;


                    int a = 0;
                init: wormreturn = Program.TSEdll.doTransactionDLL(0, this.Bon_typ, "", Program.ClientID, 0);
                    returnedCode = wormreturn.errorCode;
                    Program.TSELastError = wormreturn.errorCode.ToString();
                    Program.TseLastErrorMessage = wormreturn.errorMessage;
                    while (a <= 5)
                    {
                        a++;
                        if (a >= 5)
                        {

                            db baglan = new db();

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
                            //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                            string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`, kassenr, tseClienID) VALUES ('" + wormreturn.errorMessage + "'," + satisAnaId + ", " + tar.unixdate(DateTime.Now) + "," + Program.kasano + ", '" + Program.ClientID + "')";
                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                            cmdTseErrorCode.ExecuteNonQuery();
                            Program.TSE = "0";
                            TseLastErrorNo = Program.TSELastError;
                            TseLastErrorMessage = WormErrors.Wormerror(Convert.ToInt32(Program.TSELastError));
                            myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 5));
                            myTSEThread.Start();
                            
                            break;

                        }


                        if (returnedCode == 0)
                        {

                            response = Program.TSEdll.responseDLL;
                            this.TseStartedTransaction = response.transactionNumber();
                            string s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                            + "\nTransaction Number: " + response.transactionNumber()
                            + "\nLog Time: " + response.logTime()
                            + "\nSignature Counter: " + response.signatureCounter()
                            + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                            + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                           // MessageBox.Show(s);
                            Program.TSELastError = "0";
                            TseLastErrorMessage = "";

                            Program.TSE = "1";
                            
                            this.TseLogTimeStart = response.logTime();
                            break;
                        }
                        else
                        {
                            try
                            {
                                db baglan = new db();

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
                                string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + wormreturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'BonCreate')";
                                MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                cmdTseErrorCode.ExecuteNonQuery();
                            }
                            catch (Exception dd)
                            {
                                log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 444");

                            }
                            wormreturn.errorMessage.Trim();
                            string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                            // MessageBox.Show("Array Uzunlugu:"+ErrorMeldungArray.Length);
                            //MessageBox.Show(ErrorMeldungArray[0]+"->"+ErrorMeldungArray[0]);
                            if (wormreturn.errorCode != 0)
                            {
                                Program.TSELastError = ErrorMeldungArray[1]= wormreturn.errorCode.ToString();
                                    if (Convert.ToInt32(ErrorMeldungArray[1]) == 4104) //no started Transaction
                                    {
                                        wormreturn=Program.TSEdll.doTransactionDLL(0, this.Bon_typ, "", Program.ClientID, 0);
                                        if (wormreturn.errorCode == 0)
                                        {

                                            response = Program.TSEdll.responseDLL;
                                            this.TseStartedTransaction = response.transactionNumber();
                                        }
                                        else
                                        {
                                            goto init;
                                        }
                                    }
                                    else if (Convert.ToInt32(ErrorMeldungArray[1]) == 4180 || Convert.ToInt32(ErrorMeldungArray[1]) == 4181) //need Selt Test 
                                    {
                                        if (selfTestStarted == 0)
                                        {
                                            myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 1));
                                            myTSEThread.Start();
                                            break;
                                          
                                        }
                                            
                                      

                                    }
                                    else if (Convert.ToInt32(ErrorMeldungArray[1]) == 4098) //WORM_ERROR_NO_TIME_SET
                                    {
                                        myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 2));
                                        myTSEThread.Start();
                                        break;
                                        /*
                                        Program.TSEdll.ValidTimeCheck();
                                        if (Program.TSEdll.returnErrorCode == 0)
                                        {
                                            this.Infoevent("");
                                            goto init;

                                        }
                                        else
                                        {
                                            this.Infoevent("TSE Time Admin!, Bitte Warten! ");
                                            Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                            this.Infoevent("");
                                            goto init;
                                        }
                                        */
                                    }
                                }

                                else
                                {
                                    Program.TSELastError = "0";
                                    TseLastErrorMessage = "";

                                    Program.TSE = "1";  


                                }
                            }
                            




                            goto init;
                        
                    }
                }

                catch (Exception rr)
                {
                    Tarih tar = new Tarih();
                    myTSEThread = new Thread(() => TSE_Notfall_Thread("3", 3));
                    myTSEThread.Start();
                    
                    //INSERT INTO `tseerror`(`id`, `herstelelererrorcode`, `datum`) VALUES ([value-1],[value-2],[value-3])
                    // ADD `kassenr` INT NOT NULL , ADD `TSEClientID` VARCHAR(100) NOT NULL , ADD `level` VARCHAR(100) NOT NULL ;
                    db baglan = new db();
                    myConn1 = baglan.myconn();
                    if (myConn1.State == ConnectionState.Closed)
                        myConn1.Open();
                    string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + rr.Message + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'Allg. Catch  (636) " + rr.Message + "')";
                    MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                    cmdTseErrorCode.ExecuteNonQuery();
                }
            }
            //this.Masano = 0;

        }
        public void FisiKapat()
        {

            toplamtutar = 0; toplammwst = 0; mwst7Uygulanantutar = 0; mwst7miktar = 0; mwst19Uygulanantutar = 0; mwst19miktar = 0; mwst0Uygulanantutar = 0;
            brutto = 0; brutto19 = 0; brutto7 = 0; angebotsuztoplamtutar = 0; Rabattutar = 0;
            double RRabat = 0, totalRabat = 0;
            /* list<> ve class yardımı ile fiş hesaplama*/
            if (this._satisKalem.Count > 0)
            {
                if (Program.IsletmeAyarlar["land"] == "de" || Program.IsletmeAyarlar["land"] == "")
                {
                    foreach (SatisYap satilanlar in this._satisKalem)
                    {
                        if (satilanlar.Grubid != (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && satilanlar.Grubid != 43 && satilanlar.Grubid != 6)
                        {
                            brutto += Math.Round(satilanlar.Toplamtutar, 2);
                        }
                        else
                        {
                            RRabat += Math.Round(satilanlar.Toplamtutar, 2);
                        }
                        if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                        {
                            if (satilanlar.Angebotvarmi != 0)
                            {
                                angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            }
                            mwst7Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            mwst7miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / 1 + (Program.MwStList.Count > 0 ? Program.MwStList[1] / 100 : 7 / 100), 2);
                            brutto7 += Math.Round((double)satilanlar.Toplamtutar, 2);

                        }
                        else if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                        {
                            mwst19Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            mwst19miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / 1 + (Program.MwStList.Count > 0 ? Program.MwStList[2] / 100 : 19 / 100), 2);
                            if (satilanlar.Angebotvarmi != 0)
                            {
                                angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            }
                            brutto19 += Math.Round((double)satilanlar.Toplamtutar, 2);
                        }
                        else if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                        {
                            if (satilanlar.Grubid != 7 && satilanlar.Grubid != 43 && satilanlar.Grubid != 6 && satilanlar.Grubid != 999)
                            {
                                mwst0Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                                if (satilanlar.Angebotvarmi != 0)
                                {
                                    //angebotsuztoplamtutar += (double)satilanlar.Toplamtutar;
                                }
                            }
                        }
                    }
                    this.toplammwst = Math.Round(mwst7miktar + mwst19miktar, 2);
                    this.toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                    this.AngebotsuzToplamTutar = Math.Round(toplamtutar - angebotsuztoplamtutar, 2);
                    this.BruttoToplam = Math.Round(brutto, 2);

                    if (this.RabatList != null && this.RabatList.Count > 0)
                    {
                        //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                        //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                        foreach (RabattMain rbt in RabatList)
                        {
                            if (rbt.RabatArt == 0 && rbt.RabattTyp == 0) // Fisalti Indirim Procent indirimi seklinde
                            {
                                //1: € indirim
                                //0:% indirim


                                Rabattutar = Math.Round( (AngebotsuzToplamTutar) * rbt.RabatMenge / 100,2);
                                rbt.TotalRabattMenge = -Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                totalRabat += Rabattutar;
                                Rabat = rbt.RabatMenge;
                                AngebotsuzToplamTutar -= Rabattutar;
                            }
                            else if (rbt.RabatArt == 0 && rbt.RabattTyp == 1) // Fisalti Indiirm ama direk miktar indirimi
                            {
                                Rabattutar = rbt.RabatMenge;
                                rbt.TotalRabattMenge = -Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                totalRabat += Rabattutar;
                                if (Rabat != 0)
                                    Rabat = -1; // Rabat daha önce hesaplanmis ve degeri 0 dan farkli ise, birden fazla rabat vardir, o yuzden extra hesaplanmasi lazim
                            }
                            else if (rbt.RabatArt == 2)//punkte einlosen
                            {
                                //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                                // if (this.Musterino != 0 && Indirimturu == -1) //punkte einlosen
                                Rabattutar = rbt.RabatMenge;
                                rbt.TotalRabattMenge = -Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                totalRabat += Rabattutar;
                                if (Rabat != 0)
                                    Rabat = -1;
                            }
                            else if (rbt.RabatArt == 3) //Direkt Rabat
                            {
                                Rabattutar = (AngebotsuzToplamTutar) * rbt.RabatMenge / 100;

                                rbt.TotalRabattMenge = Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                AngebotsuzToplamTutar -= Rabattutar;
                                totalRabat += Rabattutar;
                                if (Rabat != 0)
                                    Rabat = -1;
                            }
                            else if (rbt.RabatArt == 4) //Gerabo Rabat
                            {
                                Rabattutar = rbt.TotalRabattMenge;

                                rbt.TotalRabattMenge = Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                totalRabat += Rabattutar;
                                if (Rabat != 0)
                                    Rabat = -1;
                            }
                            else if (rbt.RabatArt == 6) //RabattCoupon
                            {
                                Rabattutar = rbt.RabatMenge;

                                rbt.TotalRabattMenge = -Rabattutar;
                                double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                totalRabat += Rabattutar;
                                if (Rabat != 0)
                                    Rabat = -1;
                            }

                        }

                    }
                    if (Gutschein > 0)
                    {

                        if (this.Musterino != 0 && Indirimturu == -1) //punkte einlosen
                        {
                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1 + (Program.MwStList.Count > 0 ? Convert.ToDouble(Program.MwStList[1] / 100) : 7 / 100);

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1 + (Program.MwStList.Count > 0 ? Convert.ToDouble(Program.MwStList[2] / 100) : 19 / 100);

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein);

                        }

                        if (this.Musterino != 0 && Indirimturu == -2) //Direkt Rabat
                        {
                            Rabattutar = toplamtutar * PuanRabat / 100;

                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / (1 + Convert.ToDouble(Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100));

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / (1 + Convert.ToDouble(Program.MwStList.Count > 0 ? Convert.ToDouble(Program.MwStList[2]) / 100 : (double)19 / 100));

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein);
                        }
                        else  //
                        {
                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein);
                            mwst7miktar = mwst7Uygulanantutar - (mwst7Uygulanantutar / Convert.ToDouble(1 + (Program.MwStList.Count > 0 ? Convert.ToDouble(Program.MwStList[1]) / 100 : (double)7 / 100)));

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein);
                            mwst19miktar = mwst19Uygulanantutar - (mwst19Uygulanantutar / Convert.ToDouble(1 + (Program.MwStList.Count > 0 ? Convert.ToDouble(Program.MwStList[2]) / 100 : (double)19 / 100)));

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein);

                        }

                    }
                    this.toplammwst = mwst7miktar + mwst19miktar;
                    this.toplamtutar = mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar;
                    this.AngebotsuzToplamTutar = this.toplamtutar - angebotsuztoplamtutar;
                }
                else if (Program.IsletmeAyarlar["land"] == "nl")
                {
                    foreach (SatisYap satilanlar in this._satisKalem)
                    {
                        if (satilanlar.Grubid != 7 && satilanlar.Grubid != 43)
                        {
                            brutto += Math.Round(satilanlar.Toplamtutar, 2);
                        }
                        if (satilanlar.Mwst == 6)
                        {
                            if (satilanlar.Angebotvarmi != 0)
                            {
                                angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            }
                            mwst7Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            mwst7miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / 1.06, 2);
                            brutto7 += Math.Round((double)satilanlar.Toplamtutar, 2);

                        }
                        if (satilanlar.Mwst == 21)
                        {
                            mwst19Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            mwst19miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / 1.21, 2);
                            if (satilanlar.Angebotvarmi != 0)
                            {
                                angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            }
                            brutto19 += Math.Round((double)satilanlar.Toplamtutar, 2);
                        }
                        else if (satilanlar.Mwst == 0)
                        {
                            mwst0Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                            if (satilanlar.Angebotvarmi != 0)
                            {
                                //angebotsuztoplamtutar += (double)satilanlar.Toplamtutar;
                            }
                        }
                    }
                    this.toplammwst = Math.Round(mwst7miktar + mwst19miktar, 2);
                    this.toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                    this.AngebotsuzToplamTutar = Math.Round(toplamtutar - angebotsuztoplamtutar, 2);
                    this.BruttoToplam = Math.Round(brutto, 2);

                    if (Rabat > 0)
                    {
                        //0: € indirim
                        //1:% indirim
                        if (this.Musterino == 0 && Program.GlobalAyarlar["RABAT"] == 1)
                        {

                            Rabattutar = AngebotsuzToplamTutar * Rabat / 100;

                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar); ;
                        }
                        else if (this.Musterino == 0 && Program.GlobalAyarlar["RABAT"] == 0)
                        {
                            Rabattutar = Rabat;
                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar); ;
                        }
                        this.toplammwst = mwst7miktar + mwst19miktar;
                        this.toplamtutar = mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar;
                        this.AngebotsuzToplamTutar = this.toplamtutar - angebotsuztoplamtutar;
                    }

                    if (this.Musterino != 0 && Rabat > 0)
                    {
                        if (Program.GlobalAyarlar["RABAT"] == 1)
                        {

                            Rabattutar = toplamtutar * Rabat / 100;

                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar); ;
                        }
                        else if (Program.GlobalAyarlar["RABAT"] == 0)
                        {
                            Rabattutar = Rabat;
                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar); ;
                        }
                        this.toplammwst = mwst7miktar + mwst19miktar;
                        this.toplamtutar = mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar;
                        this.AngebotsuzToplamTutar = this.toplamtutar - angebotsuztoplamtutar;
                    }


                    if (PuanRabat > 0)
                    {

                        if (this.Musterino != 0 && Indirimturu == -1) //punkte einlosen
                        {
                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * PuanRabat);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * PuanRabat);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * PuanRabat);

                        }
                        if (this.Musterino != 0 && Indirimturu == -2) //Direkt Rabat
                        {
                            Rabattutar = toplamtutar * PuanRabat / 100;

                            mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * PuanRabat);
                            mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1.06;

                            mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar);
                            mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1.21;

                            mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar);
                        }
                        this.toplammwst = mwst7miktar + mwst19miktar;
                        this.toplamtutar = mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar;
                        this.AngebotsuzToplamTutar = this.toplamtutar - angebotsuztoplamtutar;
                    }
                    this.toplammwst = mwst7miktar + mwst19miktar;
                    this.toplamtutar = mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar;
                    this.AngebotsuzToplamTutar = this.toplamtutar - angebotsuztoplamtutar;
                }
            }





        }
        public bool FisiSonlandır(int odemetur, List<SatisYap> Pozisyonlar)
        {
            db baglan = new db();
            
            myConn1 = baglan.myconn();
            if (myConn1.State == ConnectionState.Closed)
            {
                baglan.openConnection();
                if (myConn1.State == ConnectionState.Closed)
                {
                    myConn1.Open();
                }

            }
            using (myConn1)
            {

                //MySqlCommand coNo = new MySqlCommand("INSERT INTO satisana SET tarih=" + suantarih.unixdate(DateTime.Now) + ", kasano=" + Program.kasano + ", subeno=" + Program.subeno + ", kasiyerno=" + Program.bedID + ", musterino=" + 0, myConn1);


                // if (coNo.ExecuteNonQuery() > 0)
                //{
                //satisAnaId = coNo.LastInsertedId;
                /* if (Program.GlobalAyarlar["TaglichZ"] == 1)
                 {
                     if (Program.ZberichtNo == false || Program.aktuelgun != suantarih.bugunBaslangic())
                     {
                         string VarmiSQL = "SELECT * FROM Zbericht WHERE tarih =" + suantarih.bugunBaslangic();
                         MySqlDataAdapter daVarmi = new MySqlDataAdapter(VarmiSQL, myConn1);
                         DataTable dtVarmi = new DataTable();
                         dtVarmi.Rows.Clear();
                         daVarmi.Fill(dtVarmi);
                         Program.aktuelgun = suantarih.bugunBaslangic();
                         if (dtVarmi.Rows.Count < 1)
                         {
                             MySqlCommand coZ = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + suantarih.bugunBaslangic() + " FROM zbericht ", myConn1);
                             coZ.ExecuteNonQuery();
                             //Program.ZberichtNo = true;
                         }
                         Program.ZberichtNo = true;
                     }
                 }
                 */



                toplamtutar = 0; toplammwst = 0; mwst7Uygulanantutar = 0; mwst7miktar = 0; mwst19Uygulanantutar = 0; mwst19miktar = 0; mwst0Uygulanantutar = 0;
                brutto = 0; brutto19 = 0; brutto7 = 0; angebotsuztoplamtutar = 0; Rabattutar = 0;
                double RRabat = 0, KontrolSumme = 0, totalRabat = 0;
                /* list<> ve class yardımı ile fiş hesaplama*/
                for (int a = 0; a < Pozisyonlar.Count; a++)
                {
                    KontrolSumme += Pozisyonlar[a].Toplamtutar;
                }

                if (Pozisyonlar.Count > 0)
                {
                    if (Program.IsletmeAyarlar["land"] == "de" || Program.IsletmeAyarlar["land"] == "")
                    {
                        try
                        {
                            foreach (SatisYap satilanlar in Pozisyonlar)
                            {
                                if (satilanlar.Grubid != 7 && satilanlar.Grubid != 43 && satilanlar.Grubid != 6 && satilanlar.Grubid != 999)
                                {
                                    brutto += Math.Round(satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    RRabat += Math.Round(satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                }
                                if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                {
                                    if (satilanlar.Angebotvarmi != 0)
                                    {
                                        angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                    }
                                    mwst7Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);
                                    brutto7 += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);

                                }
                                else if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                {
                                    mwst19Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar += Math.Round((double)satilanlar.Toplamtutar - (double)satilanlar.Toplamtutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);
                                    if (satilanlar.Angebotvarmi != 0)
                                    {
                                        angebotsuztoplamtutar += Math.Round((double)satilanlar.Toplamtutar, 2);
                                    }
                                    brutto19 += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (satilanlar.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                {
                                    if (satilanlar.Grubid != 7 && satilanlar.Grubid != 43 && satilanlar.Grubid != 6 && satilanlar.Grubid != 999)
                                    {
                                        mwst0Uygulanantutar += Math.Round((double)satilanlar.Toplamtutar, 2, MidpointRounding.AwayFromZero);
                                        if (satilanlar.Angebotvarmi != 0)
                                        {
                                            //angebotsuztoplamtutar += (double)satilanlar.Toplamtutar;
                                        }
                                    }
                                }
                                else
                                {

                                    F_GenericSoru frmerror = new F_GenericSoru();
                                    frmerror.lblMesaj.Text = "ERROR! Folgende Produkt hat keine eindeuitige MWST-Satz\n Produktname:" + satilanlar.UrunAd + "\n ProduktBarcode:" + satilanlar.Barkod + "\n V-K Preis:" + satilanlar.Satisfiyat + "\n  Pos.Summe:" + satilanlar.Toplamtutar + "\nBITTE Informierene Ihre Geschäftsleiter!";
                                    frmerror.ShowDialog();
                                    // txtGiris.Text = "";
                                    //return false;
                                }

                            }
                            this.toplammwst = Math.Round(mwst7miktar + mwst19miktar, 2, MidpointRounding.AwayFromZero);
                            this.toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2, MidpointRounding.AwayFromZero);
                            this.AngebotsuzToplamTutar = Math.Round(brutto - angebotsuztoplamtutar, 2, MidpointRounding.AwayFromZero);
                            this.BruttoToplam = Math.Round(brutto, 2, MidpointRounding.AwayFromZero);
                        }
                        catch (Exception ff)
                        {
                            F_GenericSoru frmerror = new F_GenericSoru();
                            frmerror.lblMesaj.Text = "ERROR! " + ff.Message + " BITTE Informierene Ihre Geschäftsleiter!";
                            frmerror.ShowDialog();
                            // txtGiris.Text = "";
                            //return false;
                        }
                        if (Math.Round(KontrolSumme, 2, MidpointRounding.AwayFromZero) != Math.Round(brutto + RRabat, 2, MidpointRounding.AwayFromZero))
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "ERROR!\n KONTROLSUMME= " + Math.Round(KontrolSumme, 2, MidpointRounding.AwayFromZero) + "\n BONPOZSUMME=  " + Math.Round(brutto, 2, MidpointRounding.AwayFromZero) + "\n BITTE Informierene Ihre Geschäftsleiter!";
                            frmerror.ShowDialog();
                            // txtGiris.Text = "";
                            // return false;
                        }
                        if (this.RabatList != null && this.RabatList.Count > 0)
                        {
                            //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                            //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                            foreach (RabattMain rbt in RabatList)
                            {
                                if (rbt.RabatArt == 0 && rbt.RabattTyp == 0) // Fisalti Indirim Procent indirimi seklinde
                                {
                                    //1: € indirim
                                    //0:% indirim


                                    Rabattutar = Math.Round((AngebotsuzToplamTutar) * rbt.RabatMenge / 100,2);
                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    totalRabat += Rabattutar;
                                    Rabat = rbt.RabatMenge;
                                    AngebotsuzToplamTutar -= Rabattutar;
                                }
                                else if (rbt.RabatArt == 0 && rbt.RabattTyp == 1) // Fisalti Indiirm ama direk miktar indirimi
                                {
                                    Rabattutar = rbt.RabatMenge;
                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar - totalRabat) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    totalRabat += Rabattutar;
                                    if (Rabat != 0)
                                        Rabat = -1; // Rabat daha önce hesaplanmis ve degeri 0 dan farkli ise, birden fazla rabat vardir, o yuzden extra hesaplanmasi lazim
                                }
                                else if (rbt.RabatArt == 2)//punkte einlosen
                                {
                                    //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                                    // if (this.Musterino != 0 && Indirimturu == -1) //punkte einlosen
                                    Rabattutar = rbt.RabatMenge;
                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    totalRabat += Rabattutar;
                                    if (Rabat != 0)
                                        Rabat = -1;
                                }
                                else if (rbt.RabatArt == 3) //Direkt Rabat
                                {
                                    Rabattutar =Math.Round((AngebotsuzToplamTutar) * rbt.RabatMenge / 100,2);

                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / (toplamtutar) * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    AngebotsuzToplamTutar -= Rabattutar;
                                    totalRabat += Rabattutar;
                                    if (Rabat != 0)
                                        Rabat = -1;
                                }
                                else if (rbt.RabatArt == 4) //Gerabo Rabat
                                {
                                    Rabattutar = rbt.TotalRabattMenge;

                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    totalRabat += Rabattutar;
                                    if (Rabat != 0)
                                        Rabat = -1;
                                }
                                else if (rbt.RabatArt == 6) //RabattCoupon
                                {
                                    Rabattutar = rbt.RabatMenge;

                                    rbt.TotalRabattMenge = -Rabattutar;
                                    double rabatBetragFurMwst7 = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst7 = Math.Round((mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))) - (rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7Betrag = -Math.Round(mwst7Uygulanantutar - rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst7 = -Math.Round(rabatMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7Uygulanantutar = Math.Round(rabatBetragFurMwst7, 2, MidpointRounding.AwayFromZero);
                                    mwst7miktar = Math.Round(rabatBetragFurMwst7 - rabatBetragFurMwst7 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero);

                                    double rabatBetragFurMwst19 = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    double rabatMwst19 = Math.Round((mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))) - (rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100))), 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19Betrag = -Math.Round(mwst19Uygulanantutar - rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    rbt.Mwst19 = -Math.Round(rabatMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19Uygulanantutar = Math.Round(rabatBetragFurMwst19, 2, MidpointRounding.AwayFromZero);
                                    mwst19miktar = Math.Round(rabatBetragFurMwst19 - rabatBetragFurMwst19 / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);


                                    double rabatBetragFurMwst0 = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Rabattutar), 2, MidpointRounding.AwayFromZero);
                                    mwst0Uygulanantutar = Math.Round(rabatBetragFurMwst0, 2, MidpointRounding.AwayFromZero);
                                    //rbt.TotalRabattMenge = rbt.Mwst7Betrag + rbt.Mwst19Betrag + rabatBetragFurMwst0;
                                    toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2);
                                    totalRabat += Rabattutar;
                                    if (Rabat != 0)
                                        Rabat = -1;
                                }

                            }

                        }
                        if (Gutschein > 0)
                        {

                            if (this.Musterino != 0 && Indirimturu == -1) //punkte einlosen
                            {
                                mwst7Uygulanantutar = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                mwst19Uygulanantutar = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);

                                mwst0Uygulanantutar = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);

                            }
                            if (this.Musterino != 0 && Indirimturu == -2) //Direkt Rabat
                            {
                                Rabattutar = Math.Round(toplamtutar * PuanRabat / 100, 2, MidpointRounding.AwayFromZero);

                                mwst7Uygulanantutar = Math.Round(mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);
                                mwst7miktar = Math.Round(mwst7Uygulanantutar - mwst7Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : (double)7 / 100)), 2, MidpointRounding.AwayFromZero);

                                mwst19Uygulanantutar = Math.Round(mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);
                                mwst19miktar = Math.Round(mwst19Uygulanantutar - mwst19Uygulanantutar / ((double)1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : (double)19 / 100)), 2, MidpointRounding.AwayFromZero);

                                mwst0Uygulanantutar = Math.Round(mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {

                                mwst7Uygulanantutar = mwst7Uygulanantutar - (mwst7Uygulanantutar / toplamtutar * Gutschein);
                                mwst7miktar = mwst7Uygulanantutar - mwst7Uygulanantutar / 1 + (Program.MwStList.Count > 0 ? Program.MwStList[1] / 100 : 7 / 100);

                                mwst19Uygulanantutar = mwst19Uygulanantutar - (mwst19Uygulanantutar / toplamtutar * Gutschein);
                                mwst19miktar = mwst19Uygulanantutar - mwst19Uygulanantutar / 1 + (Program.MwStList.Count > 0 ? Program.MwStList[2] / 100 : 19 / 100);

                                mwst0Uygulanantutar = mwst0Uygulanantutar - (mwst0Uygulanantutar / toplamtutar * Gutschein);



                            }
                            this.toplammwst = Math.Round(mwst7miktar + mwst19miktar, 2, MidpointRounding.AwayFromZero);
                            this.toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2, MidpointRounding.AwayFromZero);
                            this.AngebotsuzToplamTutar = Math.Round(this.toplamtutar - angebotsuztoplamtutar, 2, MidpointRounding.AwayFromZero);
                        }
                        this.toplammwst = Math.Round(mwst7miktar + mwst19miktar, 2, MidpointRounding.AwayFromZero);
                        this.toplamtutar = Math.Round(mwst7Uygulanantutar + mwst19Uygulanantutar + mwst0Uygulanantutar, 2, MidpointRounding.AwayFromZero);
                        this.AngebotsuzToplamTutar = Math.Round(this.toplamtutar - angebotsuztoplamtutar, 2, MidpointRounding.AwayFromZero);
                    }
                    //}
                }
               
                try//TSE
                {

                    if (Program.TSE == "1")
                    {
                        Stopwatch TSEFinishTrans = new Stopwatch();
                        if (Program.StopWatch == 1)
                        {
                            
                            TSEFinishTrans.Start();
                        }

                        Program.TSELastUseDatetime = suantarih.unixdate(DateTime.Now);
                        WormTransactionResponse response;
                        //start transc (0,1,2)
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        // processType: Kassenbeleg-V1
                        //processData: <Transaktionstyp>^<Brutto-Steuerumsätze>^<Zahlungen>
                        //Beleg^75.33_7.99_0.00_0.00_0.00^ 10.00:Bar_5.00:Bar:CHF_5.00:Bar:USD_64.30:Unbar
                        string processData = "";
                        if (this.Bon_typ == "Beleg")
                        {
                            processData = this.Bon_typ + "^" + String.Format("{0:0.00}", this.mwst19Uygulanantutar).Replace(',', '.') + "_" + String.Format("{0:0.00}", this.mwst7Uygulanantutar).Replace(',', '.') + "_0.00_0.00_" + String.Format("{0:0.00}", this.mwst0Uygulanantutar).Replace(',', '.') + "^" + ((this.ToplamBar + this.ToplamStorno) != 0 ? (String.Format("{0:0.00}", this.ToplamBar + this.ToplamStorno).Replace(',', '.')) + ":Bar" : "") + (this.ToplamEc != 0 ? (this.ToplamBar + this.ToplamStorno) != 0 ? "_" : "" + String.Format("{0:0.00}", this.ToplamEc + this.ToplamScheck).Replace(',', '.') + ":Unbar" : "");
                        }
                        else if (this.Bon_typ == "AVBelegstorno")
                        {
                            processData = this.Bon_typ + "^" + String.Format("{0:0.00}", this.mwst19Uygulanantutar).Replace(',', '.') + "_" + String.Format("{0:0.00}", this.mwst7Uygulanantutar).Replace(',', '.') + "_0.00_0.00_" + String.Format("{0:0.00}", this.mwst0Uygulanantutar).Replace(',', '.') + "^" + ((this.ToplamBar + this.ToplamStorno) != 0 ? (String.Format("{0:0.00}", (this.ToplamBar + this.ToplamStorno)).Replace(',', '.')) + ":Bar" : "") + (this.ToplamEc != 0 ? (this.ToplamBar + this.ToplamStorno) != 0 ? "_" : "" + String.Format("{0:0.00}", (this.ToplamEc + this.ToplamScheck)).Replace(',', '.') + ":Unbar" : "");
                        }
                        else
                        {
                            processData = this.Bon_typ + "^" + String.Format("{0:0.00}", this.mwst19Uygulanantutar).Replace(',', '.') + "_" + String.Format("{0:0.00}", this.mwst7Uygulanantutar).Replace(',', '.') + "_0.00_0.00_" + String.Format("{0:0.00}", this.mwst0Uygulanantutar).Replace(',', '.') + "^" + ((this.ToplamBar + this.ToplamStorno) != 0 ? (String.Format("{0:0.00}", this.ToplamBar + this.ToplamStorno).Replace(',', '.')) + ":Bar" : "") + (this.ToplamEc != 0 ? (this.ToplamBar + this.ToplamStorno) != 0 ? "_" : "" + String.Format("{0:0.00}", this.ToplamEc + this.ToplamScheck).Replace(',', '.') + ":Unbar" : "");
                        }
                        this.TseProcessData = processData;
                        int returnedDLLCode = -1;
                        int a = 0;
                    Init2: WormReturnClass wormreturn = Program.TSEdll.doTransactionDLL(2, "Kassenbeleg-V1", this.TseProcessData, Program.ClientID, this.TseStartedTransaction);
                        returnedDLLCode = wormreturn.errorCode;
                        while (a <= 5)
                        {
                            a++;
                            if (a >= 5)
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
                                //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                                string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`, kassenr, tseClienID) VALUES ('" + wormreturn.errorMessage + "'," + satisAnaId + ", " + tar.unixdate(DateTime.Now) + "," + Program.kasano + ", '" + Program.ClientID + "')";
                                MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                cmdTseErrorCode.ExecuteNonQuery();
                                Program.TSE = "0";
                                TseLastErrorNo = Program.TSELastError;
                                Program.TseLastErrorMessage = WormErrors.Wormerror(Convert.ToInt32(Program.TSELastError));
                                myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 5));
                                myTSEThread.Start();

                                break;

                            }


                            if (returnedDLLCode == 0)
                            {
                                Program.TSELastError = "0";
                                this.TseLastErrorNo = Program.TSELastError;

                                this.TseLastErrorMessage = "";
                                response = Program.TSEdll.responseDLL;
                                TseStartedTransaction = response.transactionNumber();
                                string s = "";
                                s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                                + "\nTransaction Number: " + response.transactionNumber()
                                + "\nLog Time: " + response.logTime()
                                + "\nSignature Counter: " + response.signatureCounter()
                                + "\nSignature: " + System.Convert.ToBase64String(response.signature())
                                + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                                //byte[] FSig= new byte[256];
                                //FSig = response.signature();
                                //MessageBox.Show(response.signature()+"");
                               // MessageBox.Show(s);
                                if (Program.PublicKey == "")
                                {
                                    try
                                    {
                                        Program.PublicKey = System.Convert.ToBase64String(Program.TSEdll.myWorm.info().tsePublicKey());
                                    }
                                    catch (Exception ff)
                                    {
                                        MessageBox.Show(ff.Message);
                                    }
                                }
                                //MessageBox.Show(Program.PublicKey+"\n"+Program.PublicKey.Length);

                                this.TseSignaturzahler = response.signatureCounter(); //Convert.ToBase64String(response.signature(),Base64FormattingOptions.None)
                                this.TseLogtime = response.logTime();
                                this.TseFinishSignatur = System.Convert.ToBase64String(response.signature()); //System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(BitConverter.ToString(response.signature()).Replace("-", ""))); //BitConverter.ToString(response.Signature()).Replace("-", ""); //System.Convert.ToBase64String(response.Signature());//Convert.ToBase64String
                                this.Transactionsnummer = response.transactionNumber();
                                this.Tseseriennr = BitConverter.ToString(Program.TSEdll.myWorm.info().tseSerialNumber()).Replace("-", "");
                                /* List<string> SlipList = new List<string>();
                                foreach(string Satir in Program.kundenbeleg)
                                {
                                    var encodeSatir = System.Text.Encoding.UTF8.GetBytes(Satir);
                                SlipList.Add(System.Convert.ToBase64String(encodeSatir));
                                }*/
                                this.TsePublicKey = System.Convert.ToBase64String(Program.TSEdll.myWorm.info().tsePublicKey()).Replace("-", "");
                                this.SigAlg = Program.TSEdll.logTimeFormat;
                                if (Program.TSEID == 0)
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
                                    string SelectSQLTSEID = "";

                                  /*  
                                   *  Ayar.cs'nin icine tasidik
                                   *  
                                   *  SelectSQLTSEID = "SELECT tse_id FROM tse  WHERE tse_serial= '" + this.Tseseriennr + "'";
                                    MySqlDataAdapter myDAtseID = new MySqlDataAdapter(SelectSQLTSEID, myConn1);
                                    DataTable dttseID = new DataTable();
                                    myDAtseID.Fill(dttseID);
                                    if (dttseID.Rows.Count > 0)
                                    {
                                        Program.TSEID = Convert.ToInt16(dttseID.Rows[0].ItemArray[0]);
                                    }*/
                                }
                                Tarih tar = new Tarih();
                                //INSERT INTO `tseerror`(`id`, `herstelelererrorcode`, `datum`) VALUES ([value-1],[value-2],[value-3])
                                // ADD `kassenr` INT NOT NULL , ADD `TSEClientID` VARCHAR(100) NOT NULL , ADD `level` VARCHAR(100) NOT NULL ;

                                //fur DSFINK


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
                                    log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 1611");

                                }

                                string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                                // MessageBox.Show("Array Uzunlugu:"+ErrorMeldungArray.Length);
                                //MessageBox.Show(ErrorMeldungArray[0]+"->"+ErrorMeldungArray[0]);
                                if (ErrorMeldungArray.Length == 2)
                                {
                                    Program.TSELastError = ErrorMeldungArray[1] = wormreturn.errorCode.ToString();
                                    if (Convert.ToInt32(ErrorMeldungArray[1]) == 4104) //no started Transaction
                                    {
                                        wormreturn = Program.TSEdll.doTransactionDLL(0, this.Bon_typ, "", Program.ClientID, 0);
                                        if (wormreturn.errorCode == 0)
                                        {

                                            response = Program.TSEdll.responseDLL;
                                            this.TseStartedTransaction = response.transactionNumber();
                                        }
                                        else
                                        {
                                            goto Init2;
                                        }
                                    }
                                    else if (Convert.ToInt32(ErrorMeldungArray[1]) == 4180 || Convert.ToInt32(ErrorMeldungArray[1]) == 4181) //need Selt Test 
                                    {                                      
                                        if (selfTestStarted == 0)
                                        {
                                            myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 1));
                                            myTSEThread.Start();
                                            break;
                                          
                                        }
                                            
                                      

                                    }
                                    else if (Convert.ToInt32(ErrorMeldungArray[1]) == 4098) //WORM_ERROR_NO_TIME_SET
                                    {
                                        myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 2));
                                        myTSEThread.Start();
                                        break;
                                       
                                    }
                                }

                                else
                                {
                                    goto Init2;
                                   /* try
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
                                    }
                                    catch
                                    {
                                    }

                                    */

                                }




                                goto Init2;
                            }
                        }
                        if (Program.StopWatch == 1)
                        {
                             
                            TSEFinishTrans.Stop();
                            TimeSpan etts = TSEFinishTrans.Elapsed;
                           StWaTSEMessage = "TSE Finish Process: Dauer:" + etts.ToString(@"hh\:mm\:ss\:fff");
                        }
                       ///end if
                    }
                    else
                    {
                        Program.TseLastErrorMessage = WormErrors.Wormerror(Convert.ToInt32(Program.TSELastError));
                        this.TseLastErrorMessage = Program.TseLastErrorMessage;
                    }
                }
                catch(Exception gg)
                {
                    myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 3));
                    myTSEThread.Start();
                }
                
                //TSE ENDE
                if (Program.programMode == "service")
                {
                    this.SyncServer = 0;
                }
                else
                {
                    this.SyncServer = 1;
                }
                this.tarih = suantarih.unixdate(DateTime.Now);
                string SonSQL = "", SonSQLLocal="";
                /*if (Program.ServerIp == "localhost" || Program.ServerIp == "127.0.0.1")
                {
                    SonSQL = "INSERT INTO  satisana SET localbonid=(select max(localbonid)+1 from satisana satana), toplamtutar=" + vA.virgulayikla(this.brutto) + ",toplammwst=" + vA.virgulayikla(this.toplammwst) + ", toplammwst7miktar=" + vA.virgulayikla(this.mwst7miktar) +
                       ", toplammwst7tutar=" + vA.virgulayikla(this.mwst7Uygulanantutar) + ", toplammwst19miktar=" + vA.virgulayikla(this.mwst19miktar) + ", toplammwst19tutar=" + vA.virgulayikla(this.mwst19Uygulanantutar) +
                       ", toplammwst0tutar=" + vA.virgulayikla(mwst0Uygulanantutar) + ", odemeturu=" + odemetur + ",verilenpara=" + vA.virgulayikla(this.verilenpara) + ", paraustu=" + vA.virgulayikla(this.paraustu) + ", stornoilgi=" + this._stornoIlgi + ", rabat=" + vA.virgulayikla(this.Rabat) +
                       ", rabattutar=" + vA.virgulayikla(totalRabat) + ", musterino=" + this.Musterino + ", kazanilanpuan=" + vA.virgulayikla(this.KazanilanPuan) + ", harcananpuan=" + vA.virgulayikla(this.HarcananPuan) + ", kredit=" + vA.virgulayikla(this.Kredit) +
                       ", toplamBar=" + vA.virgulayikla(this.ToplamBar) + ", toplamEc=" + vA.virgulayikla(this.ToplamEc) + ", toplamScheck=" + vA.virgulayikla(this.ToplamScheck) + ", toplamStorno=" + vA.virgulayikla(this.ToplamStorno) + ", tarih=" + this.tarih +
                       ", kasano=" + Program.kasano + ", subeno=" + Program.subeno + ", kasiyerno=" + Program.bedID + ", tischnr=" + this.MasaID + ", tarih_start=" + this.Datum_start + ", bon_typ_id=" + this.Bon_typ_id + ", bon_name='" + this.Bon_typ + "', abrechnungskreis=" + this.MasaID +
                       ",`signaturzahler`=" + this.TseSignaturzahler + ", `finishsignatur`='" + this.TseFinishSignatur + "', `logtime`=" + this.TseLogtime + ", `trsansactionnr`=" + this.Transactionsnummer + ", `processdata`='" + this.TseProcessData + "', syncserver=" + this.SyncServer;
                    //MessageBox.Show(SonSQL);
                }
                else
                {
                    try
                    {
                        if (Program.localConnection.State == ConnectionState.Closed)
                        {
                            Program.localConnection.Open();
                        }

                        SonSQLLocal = "INSERT INTO  satisana SET localbonid=(select case WHEN (Count(*)=0) THEN 1 ELSE max(localbonid)+1 END from satisana satana), toplamtutar=" + vA.virgulayikla(this.brutto) + ",toplammwst=" + vA.virgulayikla(this.toplammwst) + ", toplammwst7miktar=" + vA.virgulayikla(this.mwst7miktar) +
                          ", toplammwst7tutar=" + vA.virgulayikla(this.mwst7Uygulanantutar) + ", toplammwst19miktar=" + vA.virgulayikla(this.mwst19miktar) + ", toplammwst19tutar=" + vA.virgulayikla(this.mwst19Uygulanantutar) +
                          ", toplammwst0tutar=" + vA.virgulayikla(mwst0Uygulanantutar) + ", odemeturu=" + odemetur + ",verilenpara=" + vA.virgulayikla(this.verilenpara) + ", paraustu=" + vA.virgulayikla(this.paraustu) + ", stornoilgi=" + this._stornoIlgi + ", rabat=" + vA.virgulayikla(this.Rabat) +
                          ", rabattutar=" + vA.virgulayikla(totalRabat) + ", musterino=" + this.Musterino + ", kazanilanpuan=" + vA.virgulayikla(this.KazanilanPuan) + ", harcananpuan=" + vA.virgulayikla(this.HarcananPuan) + ", kredit=" + vA.virgulayikla(this.Kredit) +
                          ", toplamBar=" + vA.virgulayikla(this.ToplamBar) + ", toplamEc=" + vA.virgulayikla(this.ToplamEc) + ", toplamScheck=" + vA.virgulayikla(this.ToplamScheck) + ", toplamStorno=" + vA.virgulayikla(this.ToplamStorno) + ", tarih=" + this.tarih +
                          ", kasano=" + Program.kasano + ", subeno=" + Program.subeno + ", kasiyerno=" + Program.bedID + ", tischnr=" + this.MasaID + ", tarih_start=" + this.Datum_start + ", bon_typ_id=" + this.Bon_typ_id + ", bon_name='" + this.Bon_typ + "', abrechnungskreis=" + this.MasaID +
                          ",`signaturzahler`=" + this.TseSignaturzahler + ", `finishsignatur`='" + this.TseFinishSignatur + "', `logtime`=" + this.TseLogtime + ", `trsansactionnr`=" + this.Transactionsnummer + ", `processdata`='" + this.TseProcessData + "', syncserver=" + this.SyncServer;
                        MySqlCommand cmdLocalIns = new MySqlCommand(SonSQLLocal, Program.localConnection);
                        cmdLocalIns.ExecuteNonQuery();
                        MySqlCommand cmdlocal = new MySqlCommand();
                        cmdlocal.Connection = Program.localConnection;
                        cmdlocal.CommandType = CommandType.StoredProcedure;
                        cmdlocal.CommandText = "getLocalId";
                        cmdlocal.Parameters.AddWithValue("@lastid", cmdLocalIns.LastInsertedId);
                        cmdlocal.Parameters["@lastid"].Direction = ParameterDirection.Input;

                        cmdlocal.Parameters.Add("@localid", MySqlDbType.Int64);
                        cmdlocal.Parameters["@localid"].Direction = ParameterDirection.Output;

                        cmdlocal.ExecuteNonQuery();
                        Localbonid = Convert.ToInt64(cmdlocal.Parameters["@localid"].Value);
                        SonSQL = "INSERT INTO  satisana SET localbonid="+Localbonid+", toplamtutar=" + vA.virgulayikla(this.brutto) + ",toplammwst=" + vA.virgulayikla(this.toplammwst) + ", toplammwst7miktar=" + vA.virgulayikla(this.mwst7miktar) +
                      ", toplammwst7tutar=" + vA.virgulayikla(this.mwst7Uygulanantutar) + ", toplammwst19miktar=" + vA.virgulayikla(this.mwst19miktar) + ", toplammwst19tutar=" + vA.virgulayikla(this.mwst19Uygulanantutar) +
                      ", toplammwst0tutar=" + vA.virgulayikla(mwst0Uygulanantutar) + ", odemeturu=" + odemetur + ",verilenpara=" + vA.virgulayikla(this.verilenpara) + ", paraustu=" + vA.virgulayikla(this.paraustu) + ", stornoilgi=" + this._stornoIlgi + ", rabat=" + vA.virgulayikla(this.Rabat) +
                      ", rabattutar=" + vA.virgulayikla(totalRabat) + ", musterino=" + this.Musterino + ", kazanilanpuan=" + vA.virgulayikla(this.KazanilanPuan) + ", harcananpuan=" + vA.virgulayikla(this.HarcananPuan) + ", kredit=" + vA.virgulayikla(this.Kredit) +
                      ", toplamBar=" + vA.virgulayikla(this.ToplamBar) + ", toplamEc=" + vA.virgulayikla(this.ToplamEc) + ", toplamScheck=" + vA.virgulayikla(this.ToplamScheck) + ", toplamStorno=" + vA.virgulayikla(this.ToplamStorno) + ", tarih=" + this.tarih +
                      ", kasano=" + Program.kasano + ", subeno=" + Program.subeno + ", kasiyerno=" + Program.bedID + ", tischnr=" + this.MasaID + ", tarih_start=" + this.Datum_start + ", bon_typ_id=" + this.Bon_typ_id + ", bon_name='" + this.Bon_typ + "', abrechnungskreis=" + this.MasaID +
                      ",`signaturzahler`=" + this.TseSignaturzahler + ", `finishsignatur`='" + this.TseFinishSignatur + "', `logtime`=" + this.TseLogtime + ", `trsansactionnr`=" + this.Transactionsnummer + ", `processdata`='" + this.TseProcessData + "', syncserver=" + this.SyncServer;
                    }
                    catch(Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }

                 SELECT 1+coalesce((SELECT MAX(localbonid) FROM satisana WHERE kasano=2),0)
                }*/

                // Stop watch Bon Save
                 Stopwatch StWaBonSave = new Stopwatch();
                 Stopwatch StWaBonSaveKunden = new Stopwatch();
                if(Program.StopWatch==1)
                {
               
                StWaBonSave.Start();
                }
                SonSQL = "INSERT INTO  satisana (localbonid,toplamtutar,toplammwst,toplammwst7miktar,toplammwst7tutar,toplammwst19miktar,toplammwst19tutar,toplammwst0tutar, odemeturu,verilenpara, paraustu, stornoilgi, rabat,"+
                    "rabattutar, musterino, kazanilanpuan,harcananpuan, kredit,toplamBar,toplamEc,toplamScheck,toplamStorno,tarih, kasano,subeno,kasiyerno,tischnr,tarih_start,bon_typ_id,bon_name,abrechnungskreis,`signaturzahler`,"+
                    "`finishsignatur`,`logtime`,`trsansactionnr`, `processdata`,syncserver,`tse_errorno`, `tse_errormessage`, `tseid`, `cardtypid`, `cardpaymentid`) SELECT 1+coalesce((SELECT MAX(localbonid) FROM satisana WHERE kasano=" + Program.kasano + "),0) , " + vA.virgulayikla(this.brutto) + "," + vA.virgulayikla(this.toplammwst) + ", " + vA.virgulayikla(this.mwst7miktar) +
                      ", " + vA.virgulayikla(this.mwst7Uygulanantutar) + ", " + vA.virgulayikla(this.mwst19miktar) + ", " + vA.virgulayikla(this.mwst19Uygulanantutar) +
                      ", " + vA.virgulayikla(mwst0Uygulanantutar) + "," + odemetur + "," + vA.virgulayikla(this.verilenpara) + "," + vA.virgulayikla(this.paraustu) + "," + this._stornoIlgi + "," + vA.virgulayikla(this.Rabat) +
                      ", " + vA.virgulayikla(totalRabat) + "," + this.Musterino + ", " + vA.virgulayikla(this.KazanilanPuan) + ", " + vA.virgulayikla(this.HarcananPuan) + ", " + vA.virgulayikla(this.Kredit) +
                      ", " + vA.virgulayikla(this.ToplamBar) + ", " + vA.virgulayikla(this.ToplamEc) + ", " + vA.virgulayikla(this.ToplamScheck) + ", " + vA.virgulayikla(this.ToplamStorno) + ", " + this.tarih +
                      "," + Program.kasano + ", " + Program.subeno + ", " + Program.bedID + ", " + this.MasaID + ", " + this.Datum_start + ", " + this.Bon_typ_id + ", '" + this.Bon_typ + "', " + this.MasaID +
                      "," + this.TseSignaturzahler + ", '" + this.TseFinishSignatur + "', " + this.TseLogtime + ", " + this.Transactionsnummer + ", '" + this.TseProcessData + "', " + this.SyncServer+
                      ",'"+Program.TSELastError+"','"+Program.TseLastErrorMessage+"',"+Program.TSEID+",'"+this.CardTypID+"',"+this.CardPaymentTypID;
                MySqlCommand coSonlandir = new MySqlCommand(SonSQL, myConn1);
                try
                {
                    
                    if (coSonlandir.ExecuteNonQuery() > 0)
                    {
                        satisAnaId = coSonlandir.LastInsertedId;
                        try
                        {

                            MySqlCommand cmdlocal = new MySqlCommand();
                            cmdlocal.Connection = myConn1;
                            cmdlocal.CommandText = "getLocalId";
                            cmdlocal.CommandType = CommandType.StoredProcedure;

                            cmdlocal.Parameters.AddWithValue("@lastid", satisAnaId);
                            cmdlocal.Parameters["@lastid"].Direction = ParameterDirection.Input;

                            cmdlocal.Parameters.Add("@localid", MySqlDbType.Int64);
                            cmdlocal.Parameters["@localid"].Direction = ParameterDirection.Output;

                            cmdlocal.ExecuteNonQuery();
                            Localbonid = Convert.ToInt64(cmdlocal.Parameters["@localid"].Value);

                        }
                        catch (MySqlException dd)
                        {
                            MessageBox.Show(dd.Message);
                        }
                        StWaBonSave.Stop();
                        TimeSpan etts2 = StWaBonSave.Elapsed;
                        StWaBonKopfMessage = " Bon Kopf Process Dauer:" + etts2.ToString(@"hh\:mm\:ss\:fff");
                        //DS_FinV-K
                        if (Program.StopWatch == 1)
                        {

                            StWaBonSaveKunden.Start();
                        }
                        if (Program.TSEID != 0)
                        {
                            if (Program.TSE != "1")
                            {
                                string tse_dsfink = " INSERT INTO `transactions_tse`( `kasseid`,  `bonid`, `tseid`, `transnummer`, `transstart`, `transende`, `vorgangart`, `sigzahler`, `sig`, `fehler`, `vorgangdaten`) VALUES" +
                                           "(" + Program.kasano + "," + this.Localbonid + "," + Program.TSEID + "," + this.Transactionsnummer + "," + this.Datum_start + "," + this.TseLogtime + ",'Beleg'," + this.TseSignaturzahler + ",'" + this.TseFinishSignatur + "'," +
                                           "'0','" + this.TseProcessData + "')";
                                MySqlCommand cmdTseErrorCode = new MySqlCommand(tse_dsfink, myConn1);
                                cmdTseErrorCode.ExecuteNonQuery();
                            }
                        }
                       
                        

                        if (this.Musterino != 0 && this.Musteri != null)
                        {
                            if (this.Musteri.Method == 1 && Program.GlobalAyarlar["BARKOD"] != 1)
                            {
                                string SQLKundeAkt = "UPDATE kunden SET toplamtutar=toplamtutar+" + vA.virgulayikla(this.toplamtutar) + ", toplamindirim=toplamindirim+" + vA.virgulayikla(this.Rabattutar) + ", toplampuan=toplampuan+" + vA.virgulayikla(this.KazanilanPuan) +
                                    ", harcananpuan=harcananpuan+" + vA.virgulayikla(this.HarcananPuan) + ", harcanantutar=harcanantutar+" + vA.virgulayikla(this.Rabattutar) + ", kredit=kredit+" + this.Kredit + " WHERE barkode=" + this.Musteri.Barkod;
                                try
                                {
                                    MySqlCommand coKundenAkt = new MySqlCommand(SQLKundeAkt, myConn1);
                                    coKundenAkt.ExecuteNonQuery();
                                }
                                catch
                                {
                                }
                            }
                            else if (this.Indirimturu == -2)
                            {
                                string SQLKundeAkt = "UPDATE kunden SET toplamtutar=toplamtutar+" + vA.virgulayikla(this.toplamtutar) + ", toplamindirim=toplamindirim+" + vA.virgulayikla(this.Rabattutar) + ", kredit=kredit+" + vA.virgulayikla(this.Kredit) + " WHERE kundenid=" + this.Musterino;
                                try
                                {
                                    MySqlCommand coKundenAkt = new MySqlCommand(SQLKundeAkt, myConn1);
                                    coKundenAkt.ExecuteNonQuery();
                                }
                                catch
                                {
                                }
                            }

                            if (this.Musteri.Kredit != 0)
                            {
                                string SQLKundeAkt = "UPDATE kunden SET kredit=kredit+" + vA.virgulayikla(this.Musteri.Kredit) + " WHERE kundenid=" + this.Musterino;
                                try
                                {
                                    MySqlCommand coKundenAkt = new MySqlCommand(SQLKundeAkt, myConn1);
                                    if (coKundenAkt.ExecuteNonQuery() > 0)
                                    {

                                        string kreditSQL = "INSERT INTO kredit (kundenid, datum, betrag, kassiyerid) VALUES (" + this.Musterino + "," + suantarih.unixdate(DateTime.Now) + "," + vA.virgulayikla(this.Musteri.Kredit) + "," + this.kasiyerno + ")";
                                        MySqlCommand cmdKredit = new MySqlCommand(kreditSQL, myConn1);
                                        cmdKredit.ExecuteNonQuery();
                                    }
                                }
                                catch
                                {
                                }
                            }


                        }
                        //Rabat Save
                        if (this.RabatList != null && this.RabatList.Count > 0)
                        {
                            foreach (RabattMain rbt in this.RabatList)
                            {
                                MySqlCommand cmdRabat = new MySqlCommand();
                                // INSERT INTO `rabatt`(`id`, `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`) 
                                // VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13])
                                cmdRabat.Parameters.AddWithValue("@bonnr", satisAnaId);
                                cmdRabat.Parameters.AddWithValue("@name", rbt.RabatName);
                                cmdRabat.Parameters.AddWithValue("@rabattyp", rbt.RabattTyp);
                                cmdRabat.Parameters.AddWithValue("@rabatalani", rbt.RabattAlani);
                                cmdRabat.Parameters.AddWithValue("@rabattotal", rbt.TotalRabattMenge);
                                cmdRabat.Parameters.AddWithValue("@mwst7", rbt.Mwst7);
                                cmdRabat.Parameters.AddWithValue("@mwst7betrag", rbt.Mwst7Betrag);
                                cmdRabat.Parameters.AddWithValue("@mwst19", rbt.Mwst19);
                                cmdRabat.Parameters.AddWithValue("@mwst19betrag", rbt.Mwst19Betrag);
                                cmdRabat.Parameters.AddWithValue("@mwst0", rbt.Mwst0Betrag);
                                cmdRabat.Parameters.AddWithValue("@rabatmenge", rbt.RabatMenge);
                                cmdRabat.Parameters.AddWithValue("@rabatart", rbt.RabatArt);
                                cmdRabat.Parameters.AddWithValue("@grupid", rbt.Grupid);
                                string SQL = "INSERT INTO `rabatt`( `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`, grupid, kassenr) " +
                               "VALUES (@bonnr,@name,@rabattyp,@rabatalani,@rabattotal,@mwst7,@mwst7betrag,@mwst19,@mwst19betrag,@mwst0,@rabatmenge,@rabatart, @grupid," + Program.kasano + ")";
                                cmdRabat.CommandText = SQL;
                                cmdRabat.Connection = myConn1;
                                cmdRabat.ExecuteNonQuery();
                            }
                        }
                        //abrechnungskreis
                        if (this.MasaID != 0)
                        {
                            string sqlAbrKreis = "INSERT INTO `abrechnungskreis`( `bonid`, `kassenr`, `datum`, `abrechnungskreis`, `zusatzinfo`) VALUES " +
                                "(" + satisAnaId + "," + this.KasaNo + "," + suantarih.unixdate(DateTime.Now) + "," + this.MasaID + ",'')";
                            MySqlCommand cmdAbrKreis = new MySqlCommand(sqlAbrKreis, myConn1);
                            cmdAbrKreis.ExecuteNonQuery();

                        }
                        // Bezahlte Bestellung (`bestellid`, `satisdetayid`, `bonid`, `localbonid`, `tarih`, `toplamtutar`,
                        MySqlCommand cmdBesttMain = new MySqlCommand();
                        cmdBesttMain.Parameters.AddWithValue("@bonid", satisAnaId);
                        cmdBesttMain.Parameters.AddWithValue("@localbonid", Localbonid);
                        cmdBesttMain.Parameters.AddWithValue("@tarih", this.tarih);
                        cmdBesttMain.Parameters.AddWithValue("@kasano", KasaNo);
                        cmdBesttMain.Parameters.AddWithValue("@demeturu", odemetur);
                        cmdBesttMain.Parameters.AddWithValue("@toplam", toplamtutar);
                        cmdBesttMain.Connection = myConn1;
                        string SQLBest = "INSERT INTO kioskbestellungmain (`bonid`, `localbonid`, `tarih`, 	kasano, odemeturu,	toplamtutar) VALUES(@bonid,@localbonid,@tarih,@kasano,@demeturu,@toplam)";
                        cmdBesttMain.CommandText = SQLBest;
                        if(cmdBesttMain.ExecuteNonQuery()>0)
                        {
                            kioskBestellID = cmdBesttMain.LastInsertedId;
                            this.kioskSelbstBestellID = kioskBestellID;
                        }
                        


                        myConn1.Close();
                        //rabatCoupon = 0;
                        if (Program.StopWatch == 1)
                        {
                            StWaBonSaveKunden.Stop();
                            TimeSpan etts3 = StWaBonSaveKunden.Elapsed;
                            StWaBonKopfMessage += "\n Kundenkart + Rabatt Process Dauer:" + etts3.ToString(@"hh\:mm\:ss\:fff");
                            MessageBox.Show(StWaTSEMessage + "\n" + StWaBonKopfMessage);
                        }
                        return true;


                    }
                    else
                    {
                        //rabatCoupon = 0;
                        return false;
                    }

                }
                catch (Exception eexx)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = eexx.Message;
                    frmerror.ShowDialog();
                    return false;
                }
            }
        }
        private void TSE_Notfall_Thread(string errorCode, int p_2)
        {
            if (p_2 == 5)
            {
                while (Program.TSE == "0")
                {
                    if (Program.TSEEmailSend == 0)
                    {
                        TSE_Notfall tseNotfall = new TSE_Notfall();
                        tseNotfall.ErrorCode = errorCode;
                        tseNotfall.RecoveryError();
                        if (Program.TSE == "1")
                        {
                            myTSEThread.Suspend();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (p_2 == 1)//selfTEST
            {
                WormReturnClass wormReturn = new WormReturnClass();
                wormReturn = Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);
                while (wormReturn.errorCode != 0)
                {

                }
                if (wormReturn.errorCode == 4098)
                {
                    Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                }
                                           
            }
            else if (p_2 == 2)//TimeAdmin
            {
                Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                
                
            }
            else if (p_2 == 3)
            {
                if (Program.MyWorm == null)
                {
                    TSESearchCounter++;
                    Program.TSEdll = new F_TSEMain();
                    Program.MyWorm = Program.TSEdll.myWorm;
                    WormReturnClass wormReturn = new WormReturnClass();
                    int TSEDLL = -1, Result = -1;
                     Result= Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin).errorCode;
                     if (Result !=0)
                     {
                         TSE_Notfall tseNotfall = new TSE_Notfall();
                        
                         tseNotfall.ErrorCode = errorCode;
                         tseNotfall.RecoveryError();
                     }
                }
            }
        }
        
        public bool FisCagir(int fisno)
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
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from satisana where satisanaid=" + fisno, conn);
                DataTable dtArtikel = new DataTable("satisana");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {
                   
                    this.mwst19miktar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                    this.mwst19Uygulanantutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[9]);
                    this.mwst7miktar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[6]);
                    this.mwst7Uygulanantutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                    this.paraustu = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                    this.tarih = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[3]);
                    this.toplammwst = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[5]);
                    this.toplamtutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[4]);// Convert.ToDouble(dtArtikel.Rows[0].ItemArray[9]) + Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]) + Convert.ToDouble(dtArtikel.Rows[0].ItemArray[23]) - Convert.ToDouble(dtArtikel.Rows[0].ItemArray[19]);
                    this.verilenpara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[13]);
                    this.odemesekli = Convert.ToInt16(dtArtikel.Rows[0].ItemArray[12]);
                    this.kasiyerno = Convert.ToInt16(dtArtikel.Rows[0].ItemArray[16]);
                    this.StornoIlgi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[17]);
                    this.SatisAnaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                    this.Localbonid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[2]);
                    this.Rabat = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[18]);
                    this.Rabattutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[19]);
                    this.Dublikat = true;
                    this.Musterino = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[15]);
                    if (this.Musterino != 0)
                    {
                        this.Musterino = getMusteriNo(Musterino);
                    }
                    this.ToplamBar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[24]);
                    this.ToplamScheck = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[26]);
                    this.ToplamEc = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[25]);
                    this.RabatList = new List<RabattMain>();
                    //SatisYap satiskalem = new SatisYap();
                    MySqlDataAdapter daFisDetay = new MySqlDataAdapter("SELECT * from satisdetay where fisno=" + fisno, conn);
                    DataTable dtFisDetay = new DataTable("satisana");
                    dtFisDetay.Clear();
                    daFisDetay.Fill(dtFisDetay);
                    int rowCount1 = dtFisDetay.Rows.Count;
                    for (int i = 0; i < rowCount1; i++)
                    {
                        SatisYap satiskalem = new SatisYap();
                        satiskalem.Satisid = Convert.ToInt64(dtFisDetay.Rows[i].ItemArray[0]);
                        satiskalem.Tarih = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[1]);
                        satiskalem.Fisno = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[2]);
                        satiskalem.UrunId = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[3]);
                        satiskalem.UrunAd = dtFisDetay.Rows[i].ItemArray[11].ToString();
                        satiskalem.Mwst = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[4]);
                        satiskalem.Satisfiyat = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[5]);
                        satiskalem.Adet = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[6]);
                        satiskalem.Toplamtutar = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[7]);
                        satiskalem.KasiyerId = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[13]);
                        satiskalem.KasaNo = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[9]);
                        satiskalem.Stornodurum = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[14]);
                        satiskalem.StornoIlgi = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[15]);
                        satiskalem.Grubid = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[10]);
                        satiskalem.Gruptur = new ArtikelGrup(satiskalem.Grubid).GrupTur;
                        satiskalem.Ustid_id = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[20]);
                        satiskalem.Imhaus = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[23]);
                        satiskalem.Gv_typ_id = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[19]);
                        satiskalem.Znr = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[21]);

                        this._satisKalem.Add(satiskalem);

                    }

                    conn.Close();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }
        public bool FisCagir1(int fisno)
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
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from satisana where satisanaid=" + fisno, conn);
                DataTable dtArtikel = new DataTable("satisana");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {

                    this.mwst19miktar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[8]);
                    this.mwst19Uygulanantutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[9]);
                    this.mwst7miktar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[6]);
                    this.mwst7Uygulanantutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]);
                    this.paraustu = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[14]);
                    this.tarih = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[3]);
                    this.toplammwst = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[5]);
                    this.toplamtutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[4]) - Convert.ToDouble(dtArtikel.Rows[0].ItemArray[19]);// Convert.ToDouble(dtArtikel.Rows[0].ItemArray[9]) + Convert.ToDouble(dtArtikel.Rows[0].ItemArray[7]) + Convert.ToDouble(dtArtikel.Rows[0].ItemArray[23]) - Convert.ToDouble(dtArtikel.Rows[0].ItemArray[19]);
                    this.verilenpara = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[13]);
                    this.odemesekli = Convert.ToInt16(dtArtikel.Rows[0].ItemArray[12]);
                    this.kasiyerno = Convert.ToInt16(dtArtikel.Rows[0].ItemArray[16]);
                    this.StornoIlgi = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[17]);
                    this.SatisAnaId = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                    this.Localbonid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[2]);
                    this.Rabat = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[18]);
                    this.Rabattutar = Convert.ToDouble(dtArtikel.Rows[0].ItemArray[19]);
                    this.Dublikat = false;
                    this.Musterino = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[15]);
                    if (this.Musterino != 0)
                    {
                        this.Musterino = getMusteriNo(Musterino);
                    }
                    this.ToplamBar = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[24]);
                    this.ToplamScheck = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[26]);
                    this.ToplamEc = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[25]);
                    this.RabatList = new List<RabattMain>();
                    //SatisYap satiskalem = new SatisYap();
                    MySqlDataAdapter daFisDetay = new MySqlDataAdapter("SELECT * from satisdetay where fisno=" + fisno, conn);
                    DataTable dtFisDetay = new DataTable("satisana");
                    dtFisDetay.Clear();
                    daFisDetay.Fill(dtFisDetay);
                    int rowCount1 = dtFisDetay.Rows.Count;
                    for (int i = 0; i < rowCount1; i++)
                    {
                        SatisYap satiskalem = new SatisYap();
                        satiskalem.Satisid = Convert.ToInt64(dtFisDetay.Rows[i].ItemArray[0]);
                        satiskalem.Tarih = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[1]);
                        satiskalem.Fisno = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[2]);
                        satiskalem.UrunId = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[3]);
                        satiskalem.UrunAd = dtFisDetay.Rows[i].ItemArray[11].ToString();
                        satiskalem.Mwst = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[4]);
                        satiskalem.Satisfiyat = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[5]);
                        satiskalem.Adet = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[6]);
                        satiskalem.Toplamtutar = Convert.ToDouble(dtFisDetay.Rows[i].ItemArray[7]);
                        satiskalem.KasiyerId = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[13]);
                        satiskalem.KasaNo = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[9]);
                        satiskalem.Stornodurum = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[14]);
                        satiskalem.StornoIlgi = Convert.ToInt32(dtFisDetay.Rows[i].ItemArray[15]);
                        satiskalem.Grubid = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[10]);
                        satiskalem.Gruptur = new ArtikelGrup(satiskalem.Grubid).GrupTur;
                        satiskalem.Ustid_id = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[20]);
                        satiskalem.Imhaus = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[23]);
                        satiskalem.Gv_typ_id = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[19]);
                        satiskalem.Znr = Convert.ToInt16(dtFisDetay.Rows[i].ItemArray[21]);

                        this._satisKalem.Add(satiskalem);

                    }

                    conn.Close();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }
        private long getMusteriNo(long barkod)
        {

            try
            {
                iss_Kunden.Musteri mus = new iss_Kunden.Musteri();
                mus.MusteriBul(barkod);
                this.Musteri = mus;
                return Convert.ToInt32(mus.MusteriId);

            }
            catch
            {
                return -1;
            }



            // }
        }
        public void StornoFisYarat(int MusteriNo)
        {
            this.Bon_typ_id = (int)Bontype.AVBelegstorno;
            this.Bon_typ = "AVBelegstorno";

            this.Datum_start = suantarih.unixdate(DateTime.Now);
            if (Program.TSE == "1")
            {
                Program.TSELastUseDatetime = suantarih.unixdate(DateTime.Now);
                try
                {
                    WormTransactionResponse response;
                    //start transc (0,1,2)
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    WormReturnClass wormreturn = new WormReturnClass();
                    int returnedCode;


                    int a = 0;
                init: wormreturn = Program.TSEdll.doTransactionDLL(0, this.Bon_typ, "", Program.ClientID, 0);
                    returnedCode = wormreturn.errorCode;
                    // MessageBox.Show(returnedCode.ToString());
                    while (a < 5)
                    {
                        a++;
                        if (a >= 5)
                        {

                            db baglan = new db();

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
                            //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                            string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`) VALUES ('" + wormreturn.errorMessage + "'," + satisAnaId + ", " + tar.unixdate(DateTime.Now) + ")";
                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                            cmdTseErrorCode.ExecuteNonQuery();
                            Program.TSE = "0";
                            break;

                        }


                        if (returnedCode == 0)
                        {

                            response = Program.TSEdll.responseDLL;
                            TseStartedTransaction = response.transactionNumber();
                            string s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                            + "\nTransaction Number: " + response.transactionNumber()
                            + "\nLog Time: " + response.logTime()
                            + "\nSignature Counter: " + response.signatureCounter()
                            + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                            + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                            //MessageBox.Show(s);
                            this.TseLogTimeStart = response.logTime();
                            break;
                        }
                        else
                        {
                            try
                            {
                                db baglan = new db();

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
                                string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`) VALUES ('" + wormreturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + ")";
                                MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn1);
                                cmdTseErrorCode.ExecuteNonQuery();
                            }
                            catch (Exception dd)
                            {
                                log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 444");

                            }
                            /*  MessageBox.Show("2");
                              if (Program.TSEdll == null)
                              {
                                  MessageBox.Show("Program.TSEdll return ist null");
                              }
                              else
                              {
                                  MessageBox.Show("Program.TSEdll return ist nicht null");
                              }
                              if (wormreturn == null)
                              {
                                  MessageBox.Show("Worn return ist null");
                              }
                              else
                              {
                                  MessageBox.Show("Worn return ist nicht null");
                              }
                              MessageBox.Show(wormreturn.errorMessage);*/
                            string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                            // MessageBox.Show("Array Uzunlugu:"+ErrorMeldungArray.Length);
                            //MessageBox.Show(ErrorMeldungArray[0]+"->"+ErrorMeldungArray[0]);
                            if (ErrorMeldungArray.Length == 1)
                            {
                                this.Infoevent("TSE Init!, Bitte Warten! ");
                                WormReturnClass wormReturn = new WormReturnClass();
                                int TSEDLL = -1;
                                wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                TSEDLL = wormReturn.errorCode;
                                goto init;
                            }
                            if ((ErrorMeldungArray[1] == " 0x1054") || (ErrorMeldungArray[1] == " 0x1055"))
                            {
                                WormReturnClass newWormReturn = new WormReturnClass();
                                this.Infoevent("TSE Self TEST, Bitte Warten! ");
                                newWormReturn = Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);


                                if (newWormReturn.errorCode == 0)
                                {
                                    Program.TSEdll.ValidTimeCheck();
                                    if (Program.TSEdll.returnErrorCode == 0)
                                    {
                                        goto init;
                                    }
                                    else
                                    {
                                        this.Infoevent("TSE Time Admin!, Bitte Warten! ");
                                        Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                        goto init;
                                    }


                                }
                                else
                                {
                                    string[] SelfTestReturnClassErrorArray = newWormReturn.errorMessage.Split('=');
                                    if (SelfTestReturnClassErrorArray[1] == " 0x1055")
                                    {
                                        MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Kassenhersteller!");
                                        log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 505");
                                        return;
                                    }
                                    if (SelfTestReturnClassErrorArray[1] == " 0x1011")
                                    {
                                        MessageBox.Show(SelfTestReturnClassErrorArray[0] + "\nBitte dringend Informieren Sie Ihre Kassenhersteller!");
                                        log.AddtoLogFile(SelfTestReturnClassErrorArray[0], "TSE START TRANSACTION, 511");
                                        return;
                                    }

                                }
                            }
                            else if (ErrorMeldungArray[1] == " 0x1002")
                            {
                                this.Infoevent("TSE Self TEST, Bitte Warten! ");
                                Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);


                            }
                            else if (ErrorMeldungArray[1] == " 3")
                            {
                                this.Infoevent("TSE Init!, Bitte Warten! ");
                                WormReturnClass wormReturn = new WormReturnClass();
                                int TSEDLL = -1;
                                wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                TSEDLL = wormReturn.errorCode;
                            }
                            else
                            {
                                this.Infoevent("TSE Init!, Bitte Warten! ");
                                WormReturnClass wormReturn = new WormReturnClass();
                                int TSEDLL = -1;
                                wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                TSEDLL = wormReturn.errorCode;
                            }




                            goto init;
                        }
                    }
                }

                catch (Exception rr)
                {
                    MessageBox.Show(rr.Message);
                }
            }
        }
        public long dbMasaYarat(FisOlustur MasaBilgisi)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglan = new db();
            long masaDbid = 0;
            using (conn = baglan.myconn())
            {
                conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                MySqlCommand coNo = new MySqlCommand("INSERT INTO masamaster SET tarih=" + MasaBilgisi.tarih + ", kasano=" + MasaBilgisi.KasaNo + ", subeno=" + Program.subeno + ", kasiyerno=" + MasaBilgisi.kasiyerno + ", masano=" + MasaBilgisi.Masano + ", masaid=" + MasaBilgisi.MasaID, conn);
                if (coNo.ExecuteNonQuery() > 0)
                {
                    masaDbid = coNo.LastInsertedId;
                    return masaDbid;

                }
                else
                {
                    return 0;
                }


            }
        }
        public void BestellungSave()
        {
            try
            {

                db baglan = new db();

                myConn1 = baglan.myconn();
                if (myConn1.State == ConnectionState.Closed)
                {

                    if (myConn1.State == ConnectionState.Closed)
                    {
                        myConn1.Open();
                    }

                }
                MySqlCommand cmdBesttMain = new MySqlCommand();
                cmdBesttMain.Parameters.AddWithValue("@bonid", satisAnaId);
                cmdBesttMain.Parameters.AddWithValue("@localbonid", Localbonid);
                cmdBesttMain.Parameters.AddWithValue("@tarih", this.tarih);
                cmdBesttMain.Parameters.AddWithValue("@kasano", KasaNo);
                cmdBesttMain.Parameters.AddWithValue("@demeturu", this.odemesekli);
                cmdBesttMain.Parameters.AddWithValue("@toplam", toplamtutar);
                cmdBesttMain.Connection = myConn1;
                string SQLBest = "INSERT INTO kioskbestellungmain (`bonid`, `localbonid`, `tarih`,	kasano, odemeturu,	toplamtutar) VALUES(@bonid,@localbonid,@tarih,@kasano,@demeturu,@toplam)";
                cmdBesttMain.CommandText = SQLBest;
                if (cmdBesttMain.ExecuteNonQuery() > 0)
                {
                    kioskBestellID = cmdBesttMain.LastInsertedId;
                    this.kioskSelbstBestellID = kioskBestellID;
                }



                myConn1.Close();
            }
            catch(Exception dd)
            {
                MessageBox.Show(dd.Message);
            }
        }


    }
}
