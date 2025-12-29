using iss_Rabat;
using iss_RechnungsCreate;
using iss_tse_Meldepflicht;
using Microsoft.PointOfService;
using MySql.Data.MySqlClient;
using POS.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace IS_KASSE
{
    public class FisBarkodlu
    {
        public PosPrinter m_Printer = null;
        public OPOSPOSPrinter m_Printer2 = null;
        public FisOlustur
            basilacakFis = null;
        public FaturaOlustur basilacakFatura = null;
        public int OdemeTur = -1;

        tar.Tarih tarih = new tar.Tarih();
        Ean13 barcode = new Ean13();
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        VirgulAyikla vA = new VirgulAyikla();
        const int MAX_LINE_WIDTHS = 2;
        bool bSetBitmapSuccess = Program.bSetBitmapSuccess;
        string strCurDir = "";
        string strFilePath = "";
        int kundenAnzahl = 0;
        public FisBarkodlu()
        {
            if (Program.IsletmeAyarlar["land"] == "de")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            }
            else if (Program.IsletmeAyarlar["land"] == "nl")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
            /*  if (Program.printerType == "citizen")
              {
                  m_Printer2 = Program.printer2;
                  m_Printer = null;

              }
              else
              {
                
              }*/
            m_Printer = Program.printer;
            strCurDir = Directory.GetCurrentDirectory();

            // strFilePath = strCurDir.Substring(0, strCurDir.LastIndexOf("Step5") + "Step5\\".Length);

            strFilePath = @strCurDir + "\\Logo.bmp";
            //m_Printer.SetBitmap(SetLogo(PrinterLogoLocation.Bottom, "TEST");
            //m_Printer.PrintMemoryBitmap(
            if (File.Exists(strFilePath))
            {

            }
            else
            {
                //default resim kullan
                strFilePath = "defaultlogo.bmp";
            }

            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {
                if (Program.GlobalAyarlar["LOGO"] == 1)
                {
                    m_Printer.RecLetterQuality = true;

                    if (m_Printer.CapRecBitmap == true)
                    {

                        if (Program.bSetBitmapSuccess == false)
                        {
                            for (int iRetryCount = 0; iRetryCount < 5; iRetryCount++)
                            {
                                try
                                {
                                    //<<<step5>>>--Start
                                    //Register a bitmap

                                    m_Printer.SetBitmap(1, PrinterStation.Receipt,
                                  strFilePath, PosPrinter.PrinterBitmapAsIs,
                                  PosPrinter.PrinterBitmapCenter);
                                    //<<<step5>>>--End
                                    bSetBitmapSuccess = true;
                                    m_Printer.SetBitmap(2, PrinterStation.Receipt,
                                  "Makas.bmp", PosPrinter.PrinterBitmapAsIs,
                                  PosPrinter.PrinterBitmapCenter);
                                    Program.bSetBitmapSuccess = true;
                                    if (File.Exists(@strCurDir + "\\Yayla.bmp"))
                                    {
                                        m_Printer.SetBitmap(3, PrinterStation.Receipt,
                                 "Yayla.bmp", 400,
                                 PosPrinter.PrinterBitmapCenter);
                                        Program.bSetBitmapSuccess = true;
                                    }
                                    break;
                                }
                                catch (PosControlException pce)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = pce.ErrorCodeExtended.ToString();
                                    frmerror.ShowDialog();
                                    // MessageBox.Show(pce.ErrorCodeExtended.ToString());
                                    if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0 && pce.Message == "It is not initialized.")
                                    {
                                        System.Threading.Thread.Sleep(1000);
                                        //MessageBox.Show(pce.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Set Bitmap succes True!", "Printer_IS_KASSE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (!bSetBitmapSuccess)
                        {
                            try
                            {

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Logo Bild Fehler!" + "Printer_IS_KASSE";
                                frmerror.ShowDialog();
                            }
                            catch
                            {

                            }
                            finally
                            {
                                try
                                {
                                    Program.printer.Claim(1000);
                                    Program.printer.DeviceEnabled = true;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }

                //<<<step3>>>--End

                //<<<step5>>>--Start
                // Even if using any printers, 0.01mm unit makes it possible to print neatly.
                m_Printer.MapMode = MapMode.Metric;
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["3"];
                frmerror.ShowDialog();
                return;
            }

        }
        // List<SatisYap> ozetKalem1 ;
        List<SatisYap> ozetKalem = new List<SatisYap>();
        List<SatisYap> OrginalFisKalemleri = null;
        /* private List<SatisYap> OzetKalemCons(ref List<SatisYap> refListe)
         {
             ozetKalem1 =  refListe;
             ozetKalem1.Clear();
             return refListe;
         } */
        private void OzetKalemList(SatisYap Pos)
        {
            SatisYap PosCopy = Pos;
            SatisYap results = ozetKalem.Where(d => d.Barkod == PosCopy.Barkod).FirstOrDefault();
            if (results == null)
            {
                ozetKalem.Add(PosCopy);

            }
            else
            {
                results.Adet += PosCopy.Adet;
                results.Toplamtutar += PosCopy.Toplamtutar;
            }
            /* if (ozetKalem.Count == 0)
            {
                ozetKalem.Add(PosCopy);
            }
            else
            {
                foreach (SatisYap sts in ozetKalem)
                {
                    if (sts.Barkod == PosCopy.Barkod)
                    {
                        sts.Adet = PosCopy.Adet + sts.Adet;
                        break;
                    }
                    else
                    {
                        ozetKalem.Add(PosCopy);
                        break;
                    }
                }
            }
            */
        }
        public void FisYaz()
        {
            CultureInfo culture = null;
            if (Program.IsletmeAyarlar["land"] == "de")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                culture = new CultureInfo("de-DE");
            }
            else if (Program.IsletmeAyarlar["land"] == "nl")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl-NL");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
                culture = new CultureInfo("nl-NL");
            }
            /* System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
             Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");*/

            DateTime nowDate = Convert.ToDateTime((basilacakFis.tarih == 0 ? (DateTime.Now) : tarih.KisatarihDateTime(Convert.ToInt32(basilacakFis.tarih))));							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm");
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;


            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer != null)
            {
                try
                {
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                }
                catch
                {
                    return;
                }
                //PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                try
                {
                    //if(Program.printerType=="star")
                    //Program.cashDrawer.OpenDrawer();
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                    lRecLineCharsCount = GetRecLineChars(ref RecLineChars);
                    ArrayList items = new ArrayList();
                    //string fontAdlari = "";
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;

                    }
                    catch (Exception hata)
                    {

                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();
                        return;
                    }
                    //MessageBox.Show(fontAdlari);
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start
                    // m_Printer.CapCharacterSet = 858;
                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }

                    if (Program.IsletmeAyarlar["land"] == "" || Program.IsletmeAyarlar["land"] == "de")
                    {
                        //<<<step3>>>--End
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                        // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                        string masano = "";
                        if (basilacakFis.Masano != null)
                        {
                            masano = "<Tisch:" + basilacakFis.Masano + ">";
                        }
                        else
                        {
                            if (basilacakFis.Bewirtung == 1)
                            {
                                masano = "<Tisch:" + 1 + ">";

                            }
                            else
                            {
                                masano = "";
                            }
                        }
                        // m_Printer.RecLineChars = RecLineChars[1];
                        if (basilacakFis.Dublikat == true)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "\u001b|1C <DUBLIKAT><DUBLIKAT><DUBLIKAT><DUBLIKAT>" + "\n");
                        }
                        if (Program.GlobalAyarlar["BONADRESBLOK"] != 0)
                        {
                            if (lRecLineCharsCount >= 2 && Program.printerType != "star" && Program.printerType != "Hy")
                            {
                                m_Printer.RecLineChars = RecLineChars[0];
                                //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C \u001b|cA" + strDate + "\n");


                            }
                            else
                            {
                                m_Printer.RecLineChars = RecLineChars[1];
                                //m_Printer.PrintNormal(PrinterStation.Receipt, " \u001b|1C \u001b|cA" + strDate + "\n");
                            }
                            if (Program.IsletmeAyarlar["kod"] != "83")
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|bC" + "\u001b|cA" + Program.IsletmeAyarlar["isletme"] + "\n");
                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + (Program.IsletmeAyarlar["kod"] == "83" ? "" : " Fax:" + Program.IsletmeAyarlar["fax"]) + "\n");
                            if (Program.IsletmeAyarlar["usid"] != "")
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                            }
                            else
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "\u001b|cA" + "<BED." + Program.bedID + "> " + "<BON:" + (this.basilacakFis.Localbonid != 0 ? this.basilacakFis.Localbonid : this.basilacakFis.SatisAnaId) + "-" + Program.kasano + ">" + (this.basilacakFis.Masano != null ? ("<Tisch:" + this.basilacakFis.Masano + ">") : ("")) + (this.basilacakFis.Localbonid != 0 ? "<#" + this.basilacakFis.SatisAnaId + ">" : "") + "\n");
                            //if (this.basilacakFis.Localbonid != 0)
                            // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA"+"#" + this.basilacakFis.SatisAnaId + "\n");

                        }
                        //Burada ok
                        //m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");

                        //<<<step5>>--Start
                        //Make 2mm speces
                        //ESC|#uF = Line Feed
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C Test String");
                        //<<<step5>>>-End
                        //string Msq = "Artikelbezeichnung     Betrag(€) Ust\n";

                        // MessageBox.Show(RecLineChars.Length.ToString() + "\n" + RecLineChars[0] + "\n" + RecLineChars[1]);
                        /* foreach (string ad in RecLineChars.)
                         {
                             fontAdlari += ad + "\n";
                         }*/
                        if (lRecLineCharsCount >= 2 && Program.printerType != "star" && Program.printerType != "Hy")
                        {
                            m_Printer.RecLineChars = RecLineChars[0];
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");


                        }
                        else
                        {
                            m_Printer.RecLineChars = RecLineChars[1];
                            m_Printer.PrintNormal(PrinterStation.Receipt, " \u001b|1C \u001b|cA" + strDate + "\n");
                        }
                        string cizgi = "";
                        cizgi = MakePrintString(m_Printer.RecLineChars, "", "");
                        // m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        // m_Printer.RecLineChars = RecLineChars[0];
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC \u001b|1C" + cizgi + "\n");
                        m_Printer.RecLineChars = RecLineChars[1];  // "\u001b|cA" + "\u001b|iC" + "Typ          Netto              Mwst             Brutto" + "\n"
                        m_Printer.PrintNormal(PrinterStation.Receipt, MakePrintString(RecLineChars[1], "\u001b|1C Artikelbezeichnung(Menge*Preis)", "Betrag() Ust" + "\n"));
                        if (lRecLineCharsCount >= 2 && Program.printerType != "star" && Program.printerType != "Hy")
                        {
                            m_Printer.RecLineChars = RecLineChars[0];

                        }


                        if (Program.printerType == "star")
                        {
                            // m_Printer.RecLineChars = RecLineChars[0];

                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC \u001b|1C" + cizgi + "\n");
                        }
                        else
                        {
                            // m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        }
                        //  if (Program.printerType != "star")
                        //   m_Printer.RecLineChars = RecLineChars[0];
                        //cizgi = MakePrintString(m_Printer.RecLineChars, "", "");
                        //m_Printer.RecLineChars = RecLineChars[0];


                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|20uF");
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");

                        //<<<step5>>>--Start
                        //Make 5mm speces
                        //999 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                        string strPrintData = "";
                        //Print buying goods
                        /* double total = 0.0;
                
                         for (int i = 0; i < astritem.Length; i++)
                         {
                             strPrintData = MakePrintString(m_Printer.RecLineChars, astritem[i], "$"
                                 + astrprice[i]);

                             m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                             total += Convert.ToDouble(astrprice[i]);

                         }*/

                        List<SatisYap> OrginalFisKalemleri = basilacakFis.SatisKalem;

                        for (int a = 0; a < OrginalFisKalemleri.Count; a++)
                        {
                            SatisYap satiskalem1 = new SatisYap();
                            satiskalem1 = (basilacakFis.SatisKalem[a]).ShallowCopy();
                            if ((satiskalem1.Gruptur == 3 || satiskalem1.Gruptur == 8 || satiskalem1.Gruptur == 7 || satiskalem1.Gruptur == 9))
                            {
                                ozetKalem.Add(satiskalem1);
                            }
                            else
                            {
                                if ((satiskalem1.Barkod != "") && (satiskalem1.Barkod != "0") && (satiskalem1.Barkod != null) && satiskalem1.IsKolli == false)
                                {
                                    //(s=>myString.Contains(s));
                                    //var SameNames = ozetKalem.All((y => y.Barkod.Equals(satiskalem1.Barkod)));
                                    //listOfStrings.Any(s=>myString.Contains(s));
                                    OzetKalemList(satiskalem1);

                                    /* var results = ozetKalem.Where(d => d.Barkod == satiskalem1.Barkod).FirstOrDefault();
                                     // if (dog != null) { dog.Name = "some value"; }
                                     //List<SatisYap> results = ozetKalem.FindAll(x => x.Barkod == satiskalem1.Barkod);
                                     if (results == null)
                                     {
                                         ozetKalem.Add(satiskalem1);

                                     }
                                     else
                                     {
                                         //private void ChangeList(ref List<int> myList) {...}

                                         //ChangeList(ref myList);
                                         OzetKalemList(ref ozetKalem, satiskalem1);
                                         // results.Adet += satiskalem1.Adet;
                                         // results.Toplamtutar += satiskalem1.Toplamtutar;

                                     }*/

                                }
                                else
                                {
                                    ozetKalem.Add(satiskalem1);
                                }
                            }
                        }
                        int wiegeArtCount = 0, totalArtikelCount = 0;
                        foreach (SatisYap satiskalem in ozetKalem)
                        {

                            // PrintLineItem(printer, satiskalem.UrunAd, satiskalem.Adet, satiskalem.Satisfiyat, satiskalem.Toplamtutar);
                            if (satiskalem.Gruptur == 3 || satiskalem.Gruptur == 8 || satiskalem.Gruptur == 7 || satiskalem.Gruptur == 9)
                            {
                                totalArtikelCount++;
                                /* eski sadece burası
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString("#0.000") + "Kg" + "*" + satiskalem.Satisfiyat.ToString("c") + "/Kg}",
                             satiskalem.Toplamtutar.ToString("c") + "-A")));
                                 */
                                wiegeArtCount++;
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd, "")) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, " " + satiskalem.Adet.ToString("#0.000") + " kg" + " * " + satiskalem.Satisfiyat.ToString("c") + "/kg",
                             satiskalem.Toplamtutar.ToString("c") + (satiskalem.Mwst == Program.MwStList[1] ? " A" : satiskalem.Mwst == Program.MwStList[2] ? " B" : " C"))) + "\n");

                            }
                            else if (satiskalem.Grubid == 43 || satiskalem.Grubid == 7 || satiskalem.Grubid == 6) //Rabatt Coupon veya Rabatt
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "*" + satiskalem.Satisfiyat.ToString("c") + ")",
                            satiskalem.Toplamtutar.ToString("c") + "  ")) + "\n");
                            }
                            else
                            {
                                if (satiskalem.Fand != 1)
                                {
                                    totalArtikelCount = totalArtikelCount + Convert.ToInt32(satiskalem.Adet);
                                }
                                if (satiskalem.Grubid != 7)
                                {
                                    if (satiskalem.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                    {
                                        if (satiskalem.Adet > 1)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/St)",
                                              satiskalem.Toplamtutar.ToString("c") + " B")) + "\n");
                                        }
                                        else
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd, satiskalem.Toplamtutar.ToString("c") + " B")) + "\n");
                                        }
                                    }
                                    else if (satiskalem.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                    {
                                        if (satiskalem.Adet > 1)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/St)",
                                              satiskalem.Toplamtutar.ToString("c") + " A")) + "\n");
                                        }
                                        else
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd,
                                             satiskalem.Toplamtutar.ToString("c") + " A")) + "\n");
                                        }
                                    }
                                    else if (satiskalem.Mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                    {
                                        if (satiskalem.Adet > 1)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/St)",
                                             satiskalem.Toplamtutar.ToString("c") + " C")) + "\n");
                                        }
                                        else
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd,
                                            satiskalem.Toplamtutar.ToString("c") + " C")) + "\n");
                                        }
                                    }
                                }

                            }
                            //m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                        }
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        //items.Clear();
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");

                        if (Program.printerType != "star")
                            m_Printer.RecLineChars = RecLineChars[1];
                        if (Program.printerType != "star" && Program.printerType != "star" && Program.printerType != "Hy")
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + "Typ          Netto              Mwst             Brutto" + "\n");
                        else
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + "  Typ        Netto       Mwst         Brutto" + "\n");



                        if (basilacakFis.mwst7Uygulanantutar != 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + getMwstInfo("A: " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "%", (Math.Round((double)this.basilacakFis.mwst7Uygulanantutar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))), 2, MidpointRounding.AwayFromZero)).ToString("c"), (Math.Round(this.basilacakFis.mwst7Uygulanantutar, 2, MidpointRounding.AwayFromZero) - Math.Round((Math.Round(this.basilacakFis.mwst7Uygulanantutar / (1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)), 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)).ToString("c"), Math.Round(this.basilacakFis.mwst7Uygulanantutar, 2, MidpointRounding.AwayFromZero).ToString("c")) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        }
                        if (basilacakFis.mwst19Uygulanantutar != 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + getMwstInfo("B: " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "%", (Math.Round(this.basilacakFis.mwst19Uygulanantutar / (1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)), 2, MidpointRounding.AwayFromZero)).ToString("c"), (this.basilacakFis.mwst19Uygulanantutar - (Math.Round(this.basilacakFis.mwst19Uygulanantutar / (1 + (Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)), 2, MidpointRounding.AwayFromZero))).ToString("c"), this.basilacakFis.mwst19Uygulanantutar.ToString("c")) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        }
                        if (basilacakFis.mwst0Uygulanantutar != 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, getMwstInfo("C: " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "%", (Math.Round(this.basilacakFis.mwst0Uygulanantutar, 2, MidpointRounding.AwayFromZero)).ToString("c"), 0.ToString("c"), (this.basilacakFis.mwst0Uygulanantutar.ToString("c")) + "\n"));
                        }
                        if (Program.printerType != "star" && Program.printerType != "Hy")
                            m_Printer.RecLineChars = RecLineChars[0];
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        if (OdemeTur == 0) //ec Karte
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "ZU BEZAHLEN:", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.RabatList.Count > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "TOTAL:", (-this.basilacakFis.RabatList.Sum(x => x.TotalRabattMenge) + this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                foreach (RabattMain rbt in basilacakFis.RabatList)
                                {

                                    if (rbt.Grupid != 999)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, rbt.RabatName + " " + rbt.RabatMenge.ToString("F") + (rbt.RabattTyp == 0 ? "%" : "€"), (rbt.TotalRabattMenge).ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                                    }

                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "ZWS:", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (basilacakFis.Gutschein > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "Gutschein: ", "-" + this.basilacakFis.Gutschein.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "EC-KARTE :", (this.basilacakFis.toplamtutar).ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        else if (OdemeTur == 1) //BAR
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "ZU BEZAHLEN :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.RabatList.Count > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "TOTAL:", (-this.basilacakFis.RabatList.Sum(x => x.TotalRabattMenge) + this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                foreach (RabattMain rbt in basilacakFis.RabatList)
                                {
                                    if (rbt.Grupid != 999)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, rbt.RabatName + " " + (rbt.RabattTyp == 0 ? rbt.RabatMenge.ToString("F") : rbt.TotalRabattMenge.ToString("0.00")) + (rbt.RabattTyp == 0 ? "%" : "€"), (rbt.TotalRabattMenge).ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                                    }

                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "ZWS:", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (basilacakFis.Gutschein > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "Gutschein: ", "-" + this.basilacakFis.Gutschein.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "BAR GEGEBEN :", basilacakFis.verilenpara.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "RÜCKGELD :", basilacakFis.paraustu.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        else if (OdemeTur == 2) //STORNO
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "\u001b|1C" + "TOTAL   :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.RabatList.Count > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "TOTAL:", (-this.basilacakFis.RabatList.Sum(x => x.TotalRabattMenge) + this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                foreach (RabattMain rbt in basilacakFis.RabatList)
                                {
                                    if (rbt.Grupid != 999)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, rbt.RabatName + " " + (rbt.RabattTyp == 0 ? rbt.RabatMenge.ToString("F") : rbt.TotalRabattMenge.ToString()) + (rbt.RabattTyp == 0 ? "%" : "€"), (rbt.TotalRabattMenge).ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + strPrintData + "\n");

                                    }

                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "ZWS:", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "ZURÜCKGEZAHLT :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + "< STORNO >< STORNO >< STORNO >" + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|uC" + cizgi + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|2uC" + "\u001b|cA" + "[STORNO-INFO]" + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "Kundenname:", ""));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|uC" + cizgi + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, MakePrintString(m_Printer.RecLineChars, "Kundenadresse:", ""));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|uC" + cizgi + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|uC" + cizgi + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "Kundenunterschrift:", ""));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|uC" + cizgi + "\n");
                        }
                        else if (OdemeTur == 3) //SCHECK
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "ZU BEZAHLEN :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.RabatList.Count > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "TOTAL:", (-this.basilacakFis.RabatList.Sum(x => x.TotalRabattMenge) + this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                foreach (RabattMain rbt in basilacakFis.RabatList)
                                {
                                    if (rbt.Grupid != 999)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, rbt.RabatName + " " + (rbt.RabattTyp == 0 ? rbt.RabatMenge.ToString("F") : rbt.TotalRabattMenge.ToString()) + (rbt.RabattTyp == 0 ? "%" : "€"), (rbt.TotalRabattMenge).ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + strPrintData + "\n");

                                    }

                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "ZWS:", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + strPrintData + "\n");
                            }
                            if (basilacakFis.Gutschein > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "Gutschein: ", "-" + this.basilacakFis.Gutschein.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "SCHECK :", basilacakFis.ToplamScheck.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "RÜCKGELD :", basilacakFis.paraustu.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        else if (OdemeTur == 4) //KOMBI
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "ZU BEZAHLEN :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.RabatList.Count > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "TOTAL:", (this.basilacakFis.RabatList.Sum(x => x.TotalRabattMenge) + this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                foreach (RabattMain rbt in basilacakFis.RabatList)
                                {
                                    if (rbt.Grupid != 999)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, rbt.RabatName + " " + (rbt.RabattTyp == 0 ? rbt.RabatMenge.ToString("F") : rbt.TotalRabattMenge.ToString()) + (rbt.RabattTyp == 0 ? "%" : "€"), (-rbt.TotalRabattMenge).ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + strPrintData + "\n");

                                    }

                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "ZWS:", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (this.basilacakFis.ToplamBar > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "BAR :", basilacakFis.ToplamBar.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (this.basilacakFis.ToplamEc > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "EC-KARTE :", basilacakFis.ToplamEc.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (this.basilacakFis.ToplamScheck > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "SCHECK :", basilacakFis.ToplamScheck.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            if (basilacakFis.Gutschein > 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "Gutschein: ", "-" + this.basilacakFis.Gutschein.ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "BAR GEGEBEN :", basilacakFis.verilenpara.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "\u001b|1C" + "RÜCKGELD :", basilacakFis.paraustu.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        string qrcodeData = "";
                        if (Program.TSE == "1")
                        {
                            if (basilacakFis.Dublikat == false)
                            {
                                //<qr-code-version>;<kassen-seriennummer>;<processType>;<processData>;<transaktions-nummer>;<signatur-zaehler>;<start-zeit>;<log-time>;<sig-alg>;<log-time-format>;<signatur>;<public-key>
                                m_Printer.RecLineChars = RecLineChars.Max();
                                m_Printer.PrintNormal(PrinterStation.Receipt, "TSE INFO:" + "\n");
                                string qrcodeData1 = "QR-Code-Vers:V0;Kasse-Seriennr:" + Program.ClientID + ";ProcessTyp:Kassenbeleg-V1;\nProcessData:" + basilacakFis.TseProcessData + "\nTrans-Nr:" + basilacakFis.Transactionsnummer + ";Signatur-Zaehler:" + basilacakFis.TseSignaturzahler + ";\nStart-Zeit:" + tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogTimeStart)) +
                                   ";\nEnde-Zeit:" + tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogtime)) + ";\nSig-Alg.:ecdsa-plain-SHA384;Log-Time-Format:unixTime;\nSignatur:" + basilacakFis.TseFinishSignatur + ";\nTSE-Public-Key:" + basilacakFis.TsePublicKey;

                                qrcodeData = "V0;" + Program.ClientID + ";Kassenbeleg-V1;" + basilacakFis.TseProcessData + ";" + basilacakFis.Transactionsnummer + ";" + basilacakFis.TseSignaturzahler + ";" + tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogTimeStart)) +
                                    ";" + tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogtime)) + ";ecdsa-plain-SHA384;unixTime;" + basilacakFis.TseFinishSignatur + ";" + basilacakFis.TsePublicKey;//.ToString();
                                //MessageBox.Show(qrcodeData);
                                /*  V0;AMA-2642;Kassenbeleg-V1;Beleg^4.05_3.00_0.00_0.00_0.00^7.05:Bar;13;44131;2019-
  11-22T11:29:48.000Z;2019-11-22T11:29:49.000Z;ecdsa-plain-
  SHA384;unixTime;K8zsZ6NjsBzo/Yd3Hba84aH3oT+c4Og5VcfJ7s6Dxz7UmAwtcKmzW16OkS/lm8pDE/
  37JHoRlYofUTLNF+9bY5Jv7C2P4nuEaHwlVarbiJs3bYlgQmIXQDZnf+
  8FhfBm;BBXNYQErM4d9sk9Iy+0T6A4sdTocijml5X78Gq/At2uXTcs/3bZNNpyu
  Hd+dpmY59yLh0xZcr9osHhkDAxsQumgjmtb3d9GIVnTaPCslEki84P1iHPiHKHfcszeQajPk3A==*/
                                //tarih.TSEtarih(Convert.ToInt32(basilacakFis.TseLogtime))
                                if (Program.printerType == "colormetrics")
                                    qrcodeData = qrcodeData.Substring(0, 255);
                                if (Program.printerType == "bixolon")
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + qrcodeData1 + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|1C" + "System:ISS-POS" + "\n");
                                    m_Printer.RecLineChars = RecLineChars[0];
                                    m_Printer.PrintBarCode(PrinterStation.Receipt, qrcodeData,
                                                    BarCodeSymbology.QRCode, 1,
                                                   1, PosPrinter.PrinterBarCodeCenter,
                                                    BarCodeTextPosition.None);
                                }
                                else
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + qrcodeData1 + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|1C" + "System:ISS-POS" + "\n");
                                    m_Printer.RecLineChars = RecLineChars[0];
                                    m_Printer.PrintBarCode(PrinterStation.Receipt, qrcodeData,
                                                    BarCodeSymbology.QRCode, 2500,
                                                   2500, PosPrinter.PrinterBarCodeCenter,
                                                    BarCodeTextPosition.None);
                                }
                            }



                            //m_Printer.PrintNormal(PrinterStation.Receipt, qrcodeData + "\n");

                        }
                        else
                        {
                            if (basilacakFis.TseLastErrorMessage != "" && basilacakFis.TseLastErrorMessage != null && basilacakFis.TseLastErrorMessage != "WORM_ERROR_NOERROR = 0")
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|150uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + "TSE Fehler!!! Fehler-Message:" + basilacakFis.TseLastErrorMessage + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA" + "\u001b|1C" + "System:ISS-POS" + "\n");
                            }
                        }
                        if (Program.printerType != "star")
                        {
                            m_Printer.RecLineChars = RecLineChars.Max();
                        }
                        m_Printer.RecLineChars = RecLineChars[1];
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|uC" + cizgi + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|iC" + "\u001b|1C" + "INFO: Insgesamt :" + totalArtikelCount + " Artikel" + (wiegeArtCount > 0 ? ", davon " + wiegeArtCount + " Wiegeartikel!" : "") + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                        if (Program.printerType != "star")
                        {
                            m_Printer.RecLineChars = RecLineChars[0];
                        }
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|2B"); image örnekleri 2 nolu image
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        if (Program.GlobalAyarlar["BARKOD"] == 1)
                        {
                            if ((Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "0" || (Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "1" && this.basilacakFis.Musterino != 0)) && basilacakFis.AngebotsuzToplamTutar >= Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkodualtsinir"])) //Tüm müşterilere ind barkodu ver
                            {
                                double ind = Math.Round(basilacakFis.AngebotsuzToplamTutar * Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkoduorani"]) / 100, 2);
                                string indOran = ind.ToString("C");
                                // string strFilePath2 = strCurDir + "\\Makas.bmp";
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|2B");
                                // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath2, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|rvC" + "Unser Dankeschön für Ihren Einkauf!" + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|4C" + "\u001b|bC" + indOran + " RABATT\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + strDate + "\n");
                                //<<<step4>>>--Start
                                string brkd = "";
                                if (m_Printer.CapRecBarCode == true)
                                {
                                    brkd = getBarkod();
                                    //Barcode printing
                                    m_Printer.PrintBarCode(PrinterStation.Receipt, brkd,
                                        BarCodeSymbology.EanJan13, 1000,
                                        m_Printer.RecLineWidth * 2 / 3, PosPrinter.PrinterBarCodeCenter,
                                        BarCodeTextPosition.Below);
                                }
                                using (myConn = baglanti.myconn())
                                {
                                    if (myConn.State == ConnectionState.Closed)
                                    {
                                        myConn.Open();
                                    }
                                    string insertSQL = "INSERT INTO rabatkupon VALUES('', " + basilacakFis.SatisAnaId + ",3, " + tarih.unixdate(DateTime.Now) + ",'" + brkd + "'," + vA.virgulayikla(basilacakFis.toplamtutar) + "," + vA.virgulayikla(ind) + ", 0 ,0)";
                                    MySqlCommand coKupon = new MySqlCommand(insertSQL, myConn);
                                    if (coKupon.ExecuteNonQuery() > 0)
                                    {

                                    }
                                    else
                                    {

                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "S:476-FB" + Program.lang["776"];
                                        frmerror.ShowDialog();
                                    }


                                }
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|rvC" + "Erhalten Sie bei Ihrem nächsten Einkauf!" + "\n");
                                //<<<step4>>>--End
                                //<<<step5>>>--End
                            }
                        }
                        if (this.basilacakFis.Musterino != 0)
                        {
                            if (basilacakFis.Musteri.Method == 1)
                            {
                                if (Program.GlobalAyarlar["BARKOD"] != 1)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    if (Program.printerType != "star")
                                        m_Printer.RecLineChars = RecLineChars[1];
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "\u001b|1C" + "Punkte INFO: KN:" + Convert.ToInt64(this.basilacakFis.Musteri.Barkod) + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Alte Zustand:" + this.basilacakFis.Musteri.KullanilabilirPuan.ToString() + "Pnkt.\n");
                                    if (this.basilacakFis.HarcananPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                    }
                                    if (this.basilacakFis.KazanilanPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Gewinn:" + this.basilacakFis.KazanilanPuan.ToString() + "Pnkt.\n");
                                    }

                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Neue Zustand:" + (this.basilacakFis.Musteri.KullanilabilirPuan + this.basilacakFis.KazanilanPuan - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                                }
                                else // Indrim Barkodu verildiğinde auynı zamanda puan da vermez ama puanlar yinede hesaplanmıştır. Hesaplanan bu puanların fiş de yeralmaması sadece eski puanın yer alması 
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    if (Program.printerType != "star")
                                        m_Printer.RecLineChars = RecLineChars[1];
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Punkte INFO: KN:" + this.basilacakFis.Musterino + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Alte Zustand:" + this.basilacakFis.Musteri.KullanilabilirPuan.ToString() + "Pnkt.\n");
                                    if (this.basilacakFis.HarcananPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                    }


                                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Neue Zustand:" + (this.basilacakFis.EskiPuanToplamı - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                                }
                            }
                            if (basilacakFis.Musteri.Method == 5)
                            {
                                if (Program.GlobalAyarlar["BARKOD"] != 1)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    if (Program.printerType != "star")
                                        m_Printer.RecLineChars = RecLineChars[1];
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Kunden-INFO: KN:" + Convert.ToInt64(this.basilacakFis.Musteri.Barkod) + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Sie haben durch Ihrer Kundenkarte " + this.basilacakFis.SatisKalem.Sum(item => item.ProzisyonRabatBetrag).ToString("C") + " gespart!" + "\n");

                                }


                            }

                        }
                        else if (basilacakFis.GeraboObject != null)
                        {

                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            if (Program.printerType != "star")
                                m_Printer.RecLineChars = RecLineChars[1];
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Punkte INFO:\n KN:" + this.basilacakFis.GeraboObject.code + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Alte Zustand:" + this.basilacakFis.EskiPuanToplamı.ToString() + "Pnkt.\n");
                            if (this.basilacakFis.HarcananPuanKarsiligiPara > 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuanKarsiligiPara.ToString("C") + " \n");
                            }
                            if (this.basilacakFis.KazanilanPuan > 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Gewinn:" + this.basilacakFis.KazanilanPuan + "Pnkt.\n");
                            }

                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Neue Zustand:Punkte:" + this.basilacakFis.GeraboObject.account.Points + "-Guthaben:" + (Convert.ToDouble(this.basilacakFis.GeraboObject.account.Credits) / 100).ToString("C") + "\n");

                        }
                        if (Program.printerType != "star")
                            m_Printer.RecLineChars = RecLineChars[0];
                        if (Program.BonText.Count > 0 && Program.BonText[0] != "")
                        {
                            foreach (string text in Program.BonText)
                            {
                                if (text != "")
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + text + "\n");
                                }
                            }

                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Ziyaretiniz Icin Tesekkur Ederiz!" + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Vielen Dank fuer Ihren Besuch!" + "\n");

                        }
                        //REA BELEG
                        if (Program.zvt == "ReaRetail")
                        {
                            if (Program.kundenbeleg.Count > 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                for (int c = 0; c < Program.kundenbeleg.Count; c++)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.kundenbeleg[c] + "\n");
                                }
                            }
                            if (Program.handlerbeleg.Count > 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                                for (int c = 0; c < Program.handlerbeleg.Count; c++)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.handlerbeleg[c] + "\n");
                                }
                            }
                            if (Program.printerType == "bixolon")
                            {
                                // m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            }
                        }
                        if (basilacakFis.isSelbstKioskBestellung == 1)
                        {
                            using (myConn = baglanti.myconn())
                            {
                                /* if (myConn.State == ConnectionState.Closed)
                                 {
                                     myConn.Open();
                                 }
                                 string SelSQL = "";
                                 SelSQL = "SELECT * FROM kioskbestellung WHERE bestellID=" + basilacakFis.kioskSelbstBestellID;
                                 MySqlDataAdapter cmdUpdate = new MySqlDataAdapter(SelSQL, myConn);
                                 DataTable dtBest = new DataTable();
                                 cmdUpdate.Fill(dtBest);
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                                 if (dtBest.Rows.Count > 0)
                                 {

                                     for(int i=0; i<dtBest.Rows.Count; i++)
                                     {
                                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1C" + "TSE Fehler!!! Fehler-Message:" + basilacakFis.TseLastErrorMessage + "\n");
                                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C" + basilacakFis.SatisAnaId + "\n");

                                     }

                                 }*/
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|4C" + "\u001b|bC Bestell-Nr: #" + basilacakFis.kioskSelbstBestellID + "\n");
                                for (int a = 0; a < basilacakFis.SatisKalem.Count; a++)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C" + basilacakFis.SatisKalem[a].Adet + "*" + basilacakFis.SatisKalem[a].UrunAd + "\n");
                                }
                            }

                        }
                        //BEWIRTUNG
                        if (basilacakFis.Bewirtung == 1)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Bewertungsaufwand-Angaben\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "(Par. 4 §5 Ziff. 2 EStG)\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Bewirtete Personen:\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            // m_Printer.PrintNormal(PrinterStation.Receipt, new string('-', (m_Printer.RecLineChars)));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Anlaß der Bewirtung:\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Höhe der Aufwendungen:\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Bei Bewertung im Restaurant\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "in anderen Fallen\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Ort:                        Datum:\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "Unterschreiben\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");


                        }
                        //
                        if (basilacakFis.PinlistSold.Count > 0)
                        {
                            foreach (SoldPinlist card in basilacakFis.PinlistSold)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "Card Name :" + card.CardName + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "Card ID :" + card.cardid + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "Batchnummer :" + card.batchnumber + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "PIN :" + card.pinnumber + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "Aktivierungshinweis :" + card.Instruction + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            }

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|3B");
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|150uF");
                        /* OPOSPOSPrinter prnt = new OPOSPOSPrinter();
                         int nRC;
                         // Open the printer.
                         nRC = prnt.Open("PRT650_01");
                         prnt.ClaimDevice(1000);
                         prnt.DeviceEnabled = true;
                         if (prnt.DeviceEnabled == true)
                         {
                             prnt.PrintBarCode(2,
                                            qrcodeData,
                                            204,
                                            200,
                                            200,
                                            -2,
                                            -13);
                         } */
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        try
                        {
                            using (myConn = baglanti.myconn())
                            {
                                if (myConn.State == ConnectionState.Closed)
                                {
                                    myConn.Open();
                                }
                                string UpdateSQL = "";
                                UpdateSQL = "UPDATE satisana SET papierbondruck=1 WHERE satisanaid=" + basilacakFis.SatisAnaId;
                                MySqlCommand cmdUpdate = new MySqlCommand(UpdateSQL, myConn);
                                cmdUpdate.ExecuteNonQuery();
                            }
                            /*  if (basilacakFis.SatisKalem.Count > 1)
                              {
                                  //MyClass result = list.Find(x => x.GetId() == "xy");

                                  var result = basilacakFis.SatisKalem.Where(i => i.Fand != 0).ToList();
                                  //if(basilacakFis.SatisKalem.
                                  if (!(result.Count > 0))
                                  {
                                      using (myConn = baglanti.myconn())
                                      {
                                          if (myConn.State == ConnectionState.Closed)
                                          {
                                              myConn.Open();
                                          }
                                          string UpdateSQL = "";
                                          UpdateSQL = "UPDATE satisana SET papierbondruck=1 WHERE satisanaid=" + basilacakFis.SatisAnaId;
                                          MySqlCommand cmdUpdate = new MySqlCommand(UpdateSQL, myConn);
                                          cmdUpdate.ExecuteNonQuery();
                                      }
                                  }
                              }*/
                        }
                        catch (Exception ee)
                        {
                        }
                        Program.BonBeleg.OdemeTur = -1;
                        Program.kundenbeleg.Clear();
                        Program.handlerbeleg.Clear();
                        //<<<step2>>>--End

                    }
                    else if (Program.IsletmeAyarlar["land"] == "nl")
                    {
                        //<<<step3>>>--End
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                        // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " BTW-nummer :" + Program.IsletmeAyarlar["usid"] + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " <BED." + Program.bedID + "> " + "<BON-nummer:" + this.basilacakFis.SatisAnaId + ">" + "\n");

                        //<<<step5>>--Start
                        //Make 2mm speces
                        //ESC|#uF = Line Feed
                        //9999m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                        //<<<step5>>>-End

                        lRecLineCharsCount = GetRecLineChars(ref RecLineChars);
                        if (lRecLineCharsCount >= 2)
                        {
                            m_Printer.RecLineChars = RecLineChars[1];
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        //
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Product Name       Aantal*Preijs ", "Bedrag(€) BTW")));


                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        m_Printer.RecLineChars = RecLineChars[0];

                        string cizgi = MakePrintString(m_Printer.RecLineChars, "", "");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");

                        //<<<step5>>>--Start
                        //Make 5mm speces
                        //999 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                        string strPrintData = "";
                        //Print buying goods
                        /* double total = 0.0;
                
                         for (int i = 0; i < astritem.Length; i++)
                         {
                             strPrintData = MakePrintString(m_Printer.RecLineChars, astritem[i], "$"
                                 + astrprice[i]);

                             m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                             total += Convert.ToDouble(astrprice[i]);

                         }*/
                        foreach (SatisYap satiskalem in basilacakFis.SatisKalem)
                        {
                            // PrintLineItem(printer, satiskalem.UrunAd, satiskalem.Adet, satiskalem.Satisfiyat, satiskalem.Toplamtutar);
                            if (new ArtikelGrup(satiskalem.Grubid).GrupTur == 3)
                            {
                                /* eski sadece burası
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString("#0.000") + "Kg" + "*" + satiskalem.Satisfiyat.ToString("c") + "/Kg}",
                             satiskalem.Toplamtutar.ToString("c") + "-A")));
                                 */
                                Program.PTB_Bondrucker = long.Parse("1A1A13", System.Globalization.NumberStyles.HexNumber);
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd, "")) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " " + satiskalem.Adet.ToString("#0.000") + "kg" + "*" + satiskalem.Satisfiyat.ToString("c") + "/kg",
                             satiskalem.Toplamtutar.ToString("c") + " A")) + "\n");

                            }
                            else if (satiskalem.Grubid == 43) //Rabatt Coupon veya Rabatt
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "*" + satiskalem.Satisfiyat.ToString("c") + ")",
                            satiskalem.Toplamtutar.ToString("c") + "  ")) + "\n");
                            }

                            else
                            {
                                if (satiskalem.Grubid != 7)
                                {
                                    if (satiskalem.Mwst == 21)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/St)",
                                          satiskalem.Toplamtutar.ToString("c") + " B")) + "\n");
                                    }
                                    else
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, satiskalem.UrunAd + "(" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/St)",
                                          satiskalem.Toplamtutar.ToString("c") + " A")) + "\n");
                                    }
                                }

                            }
                            //m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        //items.Clear();
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                        m_Printer.RecLineChars = RecLineChars[1];

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|iC" + "Type          Netto              BTW             Bruto" + "\n");
                        if (basilacakFis.mwst19Uygulanantutar > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, getMwstInfo("B:21%", (this.basilacakFis.mwst19Uygulanantutar / 1.21).ToString("c"), this.basilacakFis.mwst19miktar.ToString("c"), this.basilacakFis.mwst19Uygulanantutar.ToString("c")) + "\n");
                        }
                        if (basilacakFis.mwst7Uygulanantutar > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, getMwstInfo("A: 6%", (this.basilacakFis.mwst7Uygulanantutar / 1.06).ToString("c"), this.basilacakFis.mwst7miktar.ToString("c"), this.basilacakFis.mwst7Uygulanantutar.ToString("c")) + "\n");
                        }
                        m_Printer.RecLineChars = RecLineChars[0];
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                        if (OdemeTur == 0) //ec Karte
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "TOTAAL:", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.Rabattutar != 0 || basilacakFis.PuanRabat != 0)
                            {
                                /*eski
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "RABAT", "-" + this.basilacakFis.Rabattutar.ToString("C"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "ZWS", (this.basilacakFis.toplamtutar - this.basilacakFis.Rabattutar).ToString("C"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");*/
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "TOTAAL", (this.basilacakFis.Rabattutar + this.basilacakFis.toplamtutar + this.basilacakFis.PuanRabat).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                if (basilacakFis.PuanRabat > 0)
                                {

                                    strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING(" + this.basilacakFis.HarcananPuan + " Pnkt.)", "-" + this.basilacakFis.PuanRabat.ToString("c"));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                }
                                if (basilacakFis.Rabattutar > 0)
                                {
                                    if (Program.GlobalAyarlar["RABAT"] == 1)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING " + this.basilacakFis.Rabat.ToString() + "%", "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                    }
                                    else
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING " + this.basilacakFis.Rabat.ToString("C"), "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                    }
                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "SUBT.", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "PIN :", (this.basilacakFis.toplamtutar).ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        else if (OdemeTur == 1) //BAR
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "TOTAAL :", basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            if (basilacakFis.Rabattutar != 0 || basilacakFis.PuanRabat != 0)
                            {
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "TOTAAL", (this.basilacakFis.Rabattutar + this.basilacakFis.toplamtutar + basilacakFis.PuanRabat).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                                if (basilacakFis.PuanRabat > 0)
                                {

                                    strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING(" + this.basilacakFis.HarcananPuan + " Pnkt.)", "-" + this.basilacakFis.PuanRabat.ToString("c"));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                }
                                if (basilacakFis.Rabattutar > 0)
                                {
                                    if (Program.GlobalAyarlar["RABAT"] == 1)
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING " + this.basilacakFis.Rabat.ToString() + "%", "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                    }
                                    else
                                    {
                                        strPrintData = MakePrintString(m_Printer.RecLineChars, "KORTING " + this.basilacakFis.Rabat.ToString("C"), "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                        m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                                    }
                                }
                                strPrintData = MakePrintString(m_Printer.RecLineChars, "SUBT.", (this.basilacakFis.toplamtutar).ToString("c"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            }
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "KONTANT :", basilacakFis.verilenpara.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "WISSELGELD :", basilacakFis.paraustu.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                        }
                        else if (OdemeTur == 2) //STORNO
                        {
                            strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "TOTAAL   :", "-" + basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer.RecLineChars, "KONTANT :", "-" + basilacakFis.toplamtutar.ToString("c"));
                            m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "< ANNULERING >< ANNULERING >< ANNULERING >" + "\n");
                        }


                        if (Program.GlobalAyarlar["BARKOD"] == 1)
                        {
                            if ((Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "0" || (Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "1" && this.basilacakFis.Musterino != 0)) && basilacakFis.AngebotsuzToplamTutar >= Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkodualtsinir"])) //Tüm müşterilere ind barkodu ver
                            {
                                double ind = Math.Round(basilacakFis.AngebotsuzToplamTutar * Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkoduorani"]) / 100, 2);
                                string indOran = ind.ToString("C");
                                // string strFilePath2 = strCurDir + "\\Makas.bmp";
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|2B");
                                // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath2, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|rvC" + "Bedankt voor u bezoek!" + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|4C" + "\u001b|bC" + indOran + " KORTING\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                                //<<<step4>>>--Start
                                string brkd = "";
                                if (m_Printer.CapRecBarCode == true)
                                {
                                    brkd = getBarkod();
                                    //Barcode printing
                                    m_Printer.PrintBarCode(PrinterStation.Receipt, brkd,
                                        BarCodeSymbology.EanJan13, 1000,
                                        m_Printer.RecLineWidth * 2 / 3, PosPrinter.PrinterBarCodeCenter,
                                        BarCodeTextPosition.Below);
                                }
                                using (myConn = baglanti.myconn())
                                {
                                    if (myConn.State == ConnectionState.Closed)
                                    {
                                        myConn.Open();
                                    }
                                    string insertSQL = "INSERT INTO rabatkupon VALUES('', " + basilacakFis.SatisAnaId + ",3, " + tarih.unixdate(DateTime.Now) + ",'" + brkd + "'," + vA.virgulayikla(basilacakFis.toplamtutar) + "," + vA.virgulayikla(ind) + ", 0 ,0)";
                                    MySqlCommand coKupon = new MySqlCommand(insertSQL, myConn);
                                    if (coKupon.ExecuteNonQuery() > 0)
                                    {

                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "S:795-FB" + Program.lang["776"];
                                        frmerror.ShowDialog();

                                    }


                                }
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|rvC" + "Erhalten Sie bei Ihrem nächsten Einkauf!" + "\n");
                                //<<<step4>>>--End
                                //<<<step5>>>--End
                            }
                        }
                        if (this.basilacakFis.Musterino != 0)
                        {
                            if (basilacakFis.Indirimturu == -1)
                            {
                                if (Program.GlobalAyarlar["BARKOD"] != 1)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.RecLineChars = RecLineChars[1];
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Punkte INFO: KN:" + Convert.ToInt64(this.basilacakFis.Musteri.Barkod) + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Alte Zustand:" + this.basilacakFis.EskiPuanToplamı.ToString() + "Pnkt.\n");
                                    if (this.basilacakFis.HarcananPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                    }
                                    if (this.basilacakFis.KazanilanPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Gewinn:" + this.basilacakFis.KazanilanPuan.ToString() + "Pnkt.\n");
                                    }

                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Neue Zustand:" + (this.basilacakFis.EskiPuanToplamı + this.basilacakFis.KazanilanPuan - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                                }
                                else // Indrim Barkodu verildiğinde auynı zamanda puan da vermez ama puanlar yinede hesaplanmıştır. Hesaplanan bu puanların fiş de yeralmaması sadece eski puanın yer alması 
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.RecLineChars = RecLineChars[1];
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Punkte INFO: KN:" + this.basilacakFis.Musterino + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + cizgi + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Alte Zustand:" + this.basilacakFis.EskiPuanToplamı.ToString() + "Pnkt.\n");
                                    if (this.basilacakFis.HarcananPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                    }
                                    if (this.basilacakFis.KazanilanPuan > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Gewinn:0 Pnkt.\n");
                                    }

                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Neue Zustand:" + (this.basilacakFis.EskiPuanToplamı - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                                }
                            }
                        }
                        if (Program.printerType != "star")
                        {
                            m_Printer.RecLineChars = RecLineChars[0];
                        }
                        if (Program.BonText.Count > 0 && Program.BonText[0] != "")
                        {
                            foreach (string text in Program.BonText)
                            {
                                if (text != "")
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + text + "\n");
                                }
                            }

                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Ziyaretiniz Icin Tesekkur Ederiz!" + "\n");
                            //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Bedankt voor u bezoek!" + "\n");

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);

                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                        //<<<step2>>>--End
                    }
                    if (Program.printerType == "star")
                    {

                        // Program.cashDrawer.OpenDrawer();
                    }
                    else
                    {
                        if (Program.printerType == "bixolon")
                        {
                            //m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }
                        else
                        {
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                        }
                    }
                } //try sonu
                catch (PosControlException ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:S:875-FB" + ee.Message;
                    frmerror.ShowDialog();

                }
                finally
                {
                    if (Program.GlobalAyarlar["LOGO"] == 1)
                    {
                        try
                        {
                            if (Program.bSetBitmapSuccess == true)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");

                                /*F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Program.bSetBitmapSuccess == true";
                                frmerror.ShowDialog();*/
                            }
                            else
                            {
                                //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                                /* F_GenericError frmerror = new F_GenericError();
                                 frmerror.lblMesaj.Text = "Program.bSetBitmapSuccess == false";
                                 frmerror.ShowDialog();*/
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = "Drucker ERROR!";
                frmerror.ShowDialog();
            }

        }

        private string getMwstInfo(string tip, string netto, string mwstTutar, string brutto)
        {
            int t = 0, n = 0, m = 0, b = 0;

            /*EPSON T20*/
            if (Program.printerType == "T20")
            {
                while (t < 8)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 15)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 19)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 18)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "TM88") /*EPSON TM 88*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 13)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 18)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 17)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "bixolon") /*EPSON TM 88*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 13)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 18)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 17)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "NCR") /*NCR ama EPSON TM 88 ayarlarini kullandik*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 13)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 18)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 17)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "star")
            {
                while (t < 8)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 11)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 13)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 13)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "colormetrics") /*EPSON TM 88*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 10)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 15)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 15)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else if (Program.printerType == "Hy")            /*citizen*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 13)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 13)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 13)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
            else //if (Program.printerType == "citizen")            /*citizen*/
            {
                while (t < 7)
                {
                    tip = " " + tip;
                    t = tip.Length;
                }
                while (n < 17)
                {
                    netto = " " + netto;
                    n = netto.Length;
                }
                while (m < 18)
                {
                    mwstTutar = " " + mwstTutar;
                    m = mwstTutar.Length;
                }
                while (b < 17)
                {
                    brutto = " " + brutto;
                    b = brutto.Length;
                }
                return tip + netto + mwstTutar + brutto;
            }
        }
        private string getBarkod()
        {
            barcode.CountryCode = "24" + (Convert.ToInt16(Program.IsletmeAyarlar["kod"])) + "02";
            barcode.ProductCode = getMaxID();
            barcode.ManufacturerCode = new string('0', (12 - (Convert.ToInt16(barcode.CountryCode.Length) + Convert.ToInt16(barcode.ProductCode.Length))));   //"00000";

            return barcode.ToString();
            //picture1.Image = barcode.CreateBitmap();
        }
        private string getMaxID()
        {
            using (myConn = baglanti.myconn())
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string maxIdSQL = "SELECT max(id) as Maxid from rabatkupon ";
                MySqlDataAdapter daMaxID = new MySqlDataAdapter(maxIdSQL, myConn);
                DataTable dtMaxID = new DataTable();
                dtMaxID.Rows.Clear();
                daMaxID.Fill(dtMaxID);
                string maxID = "";
                if (dtMaxID.Rows[0].ItemArray[0] is DBNull)
                {
                    maxID = 1.ToString();
                }
                else
                {
                    maxID = (Convert.ToInt32(dtMaxID.Rows[0].ItemArray[0]) + 1).ToString();
                }
                //int uz = maxID.Length;
                while (maxID.Length < 5)
                {
                    maxID = "0" + maxID;
                }
                return maxID;

            }
        }

        public void Zdruck()
        {

            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "";
            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {
                if (Program.GlobalAyarlar["TaglichZ"] == 1)
                {
                    AuswahlSQL = " satisana.tarih >=" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND satisana.tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " ";
                }
                else
                {
                    AuswahlSQL = " satisana.znr=0 ";
                }

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();

                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    else
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                    using (myConn = baglanti.myconn())
                    {
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }

                        string sqltarih1 = "SELECT * FROM zbericht where tarih =" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                        MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn);
                        DataTable dttar1 = new DataTable();
                        dttar1.Rows.Clear();
                        datar1.Fill(dttar1);
                        long berNo;
                        if (Program.GlobalAyarlar["TaglichZ"] == 1)
                        {
                            if (dttar1.Rows.Count > 0)
                            {
                                berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                            }
                            else
                            {
                                MySqlCommand coZ = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht ", myConn);
                                coZ.ExecuteNonQuery();
                                berNo = coZ.LastInsertedId;
                            }
                        }
                        else
                        {
                            MySqlCommand coZ = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht ", myConn);
                            coZ.ExecuteNonQuery();
                            berNo = coZ.LastInsertedId;
                        }
                        double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                        double RabatTutar = 0;
                        double RabatCouponTutar = 0;
                        double Gutschein = 0;
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + "-" + Program.kasano + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                              "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                              "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay FROM satisana WHERE " +
                              AuswahlSQL + " AND kasano=" + Program.kasano + "  GROUP BY odemeturu ORDER BY odemeturu ASC";
                        MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                        DataTable dtSatisAna = new DataTable("satisana");
                        dtSatisAna.Rows.Clear();
                        daSatisAna.Fill(dtSatisAna);

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

                        int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                        string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 AND" + AuswahlSQL + " AND satisana.kasano=" + Program.kasano + "  GROUP BY satisdetay.mwst";
                        // eskisi: satisdetay.tarih >=" +tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND satisdetay.tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) 
                        MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn);
                        DataTable dtKombi = new DataTable();
                        daKombi.Fill(dtKombi);
                        string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay, SUM(CASE WHEN `toplamEc` > 0  THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND " + AuswahlSQL + " AND satisana.kasano=" + Program.kasano;
                        MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn);
                        DataTable dtKombiSumme = new DataTable();
                        daKombiSumme.Fill(dtKombiSumme);
                        if (dtKombi.Rows.Count > 0)
                        {
                            double KombiTotal = 0;
                            for (int c = 0; c < dtKombi.Rows.Count; c++)
                            {

                                KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                KombiTotal = KombiBar + KombiEC + KombiScheck;

                            }


                        }
                        for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                        {
                            if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))) + "\n")));
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                stornoSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);
                                stornoTutar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]));


                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR" + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[4]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[4] + " Kombi. Bar)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                barSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                barTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[5]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[5] + " Kombi. Ec Karte)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                ecSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                ecTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X SCHECK " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[6]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[6] + " Kombi. Scheck)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                scheckSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                scheckTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }


                        }
                        if ((barTutar == 0) && (KombiBar != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[4] + " X BAR " + ":", string.Format("{0:C}", (KombiBar)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                        }
                        if ((scheckTutar == 0) && (KombiScheck != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[6] + " X SCHECK " + ":", string.Format("{0:C}", (KombiScheck)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                        }
                        if ((ecTutar == 0) && (KombiEC != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[5] + " X EC KARTE " + ":", string.Format("{0:C}", (KombiEC)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        }
                        if (stornoSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X STORNO :", "0,00 €")));
                        }
                        if (ecSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X EC KARTE :", "0,00 € ")));
                        }
                        if (barSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X BAR :", "0,00 €")));
                        }
                        this.kundenAnzahl = stornoSay + ecSay + barSay;
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Kassenbarbestand: " + string.Format("{0:C}", (barTutar + stornoTutar + Gutschein)) + "]\n");
                        if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[ges. Anfangbestand: " + string.Format("{0:C}", AnfangBestandReturn()) + "]\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Soll Bestand: " + string.Format("{0:C}", AnfangBestandReturn() + barTutar + stornoTutar + Gutschein) + "]\n");

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: " + KasiyerNullBonReturn() + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                        /*string sqlSatisAna = "SELECT sum(`toplamtutar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                                "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                                "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay FROM satisana WHERE tarih >=" +
                                tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND kasano=" + Program.kasano + "  GROUP BY odemeturu ORDER BY odemeturu DESC";
                        MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                        DataTable dtSatisAna = new DataTable("satisana");
                        dtSatisAna.Rows.Clear();
                        daSatisAna.Fill(dtSatisAna);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                        for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                        {
                            if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[0])) + "\n"));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[0]:0)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[10]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[0]:0)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[10]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[0]:0)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[0]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

                            }
                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);*/
                        if (Program.printerType == "bixolon")
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }
                        if (Program.GlobalAyarlar["TaglichZ"] == 0)
                        {
                            ZNummerEkle(berNo);
                        }
                    }
                }
                catch
                {
                }

            }
        }


        public void XZDruck(int berNo)
        {
            double yuzde19lukmiktar = 0, yuzde7likmiktar = 0, yuzde0likmiktar = 0, toplamtutar = 0, toplamBar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, toplam0brut = 0, ec7tutar = 0, ec19tutar = 0, ec0tutar = 0,
                        storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0, storno0tutar = 0, scheckmiktar = 0, scheck7tutar = 0, scheck19tutar = 0, scheck0tutar = 0, KombiBar = 0, KombiEC = 0, KombiScheck = 0,
                        KombiBar0 = 0, KombiBar7 = 0, KombiBar19 = 0, KombiEc0 = 0, KombiEc7 = 0, KombiEc19 = 0, KombiCek0 = 0, KombiCek7 = 0, KombiCek19 = 0, toplam19Yazildi = 0, Gutschein = 0, RabatCouponTutar = 0, RabatTutar = 0,
                        totalAnfangBestand = 0, rabatt0Brutto = 0, rabatt7Brutto = 0, rabatt19Brutto = 0, rabattCoupon0Brutto = 0, rabattCoupon7Brutto = 0, rabattCoupon19Brutto = 0;
            int printDruck = 0, barAnzahl = 0, ecAnzahl = 0, stornoAnzahl = 0, scheckAnzahl = 0, bonAnzahl = 0, kombiBonAnzahl = 0, kombiBarAnzahl = 0, kombiECAnzahl = 0, kombiScheckAnzahl = 0;
            int mwst = -1, KombiSay = 0;
            //`barAnzahl`, `ecAnzahl`, `stornoAnzahl`, `scheckAnzahl`, `bonAnzahl`, `kombiBonAnzahl`, `kombiBarAnzahl`, `kombiECAnzahl`, `kombiScheckAnzahl`, `storno0tutar`;
            Int32 startBonNr = 0, endBonNr = 0;
            long bastarih = 0, bittarih = 0;
            List<Int32> ZberichtArsivePrintList = new List<int>();
            Int32 erstellDatum = 0;

            double totalGeldEntnahme = 0;
            double totalGeldEinlage = 0;
            double kassenbuchbarBetrag = 0;

            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;

            MySqlConnection myConn1 = new MySqlConnection();
            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());

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
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {


                MySqlTransaction mytrans = null;
                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();


                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    else
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                    using (myConn1)
                    {

                        if (myConn1.State == ConnectionState.Closed)
                        {
                            myConn1.Open();
                        }

                        /* string sqltarih1 = "SELECT * FROM zbericht WHERE erstelldatum=0 ";
                         MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn1);
                         DataTable dttar1 = new DataTable();
                         dttar1.Rows.Clear();
                         datar1.Fill(dttar1);
                       

                         if (dttar1.Rows.Count > 0)
                         {
                             berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                         }
                         else
                         {

                         */
                        string sqltarih1 = "";
                        sqltarih1 = "SELECT * FROM zberichtpos WHERE znr =" + berNo + " ORDER BY mwst";
                        MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn1);
                        DataTable dtZpos = new DataTable();
                        dtZpos.Rows.Clear();
                        datar1.Fill(dtZpos);
                        //long berNo=0;
                        if (dtZpos.Rows.Count > 0)
                        {
                            // berNo = ZberichtArsivePrintList[j];
                        }
                        else
                        {
                            MessageBox.Show("Für dieser Tag, leider auf dem System wurde keine Z-Positionen gefunden! Bitte kontaktieren Sie mit Service-Nr: 02202 7059900 für eine manuelle kurze Bericht! ");
                            return;
                        }
                        // Z Berihtskopf dateien
                        string ZBerichtSQL = "SELECT * FROM Zbericht WHERE Zberichtno=" + berNo;
                        MySqlDataAdapter myDaZ = new MySqlDataAdapter(ZBerichtSQL, myConn1);
                        DataTable myDtZ = new DataTable();
                        myDtZ.Rows.Clear();
                        myDaZ.Fill(myDtZ);

                        Int32 StartDatum = 0, EndDatum = 0;
                        int ZKasseNo = 0;
                        ZKasseNo = Convert.ToInt16(myDtZ.Rows[0].ItemArray[13]);
                        //localbonid and Vorgangid vergleich für Datum
                        string IdCheck = "SELECT * FROM system ";
                        MySqlDataAdapter myDaIdChek = new MySqlDataAdapter(IdCheck, myConn1);
                        DataTable dtIdCheck = new DataTable();
                        myDaIdChek.Fill(dtIdCheck);
                        string ZStartSQL = "";
                        if (Convert.ToInt32(dtIdCheck.Rows[0].ItemArray[10]) == 0)
                        {
                            ZStartSQL = "SELECT tarih FROM satisana WHERE satisanaid=" + myDtZ.Rows[0].ItemArray[15] + " OR satisanaid=" + myDtZ.Rows[0].ItemArray[16];
                        }
                        else if ((tarih.gunBaslangicUnix(Convert.ToInt32(myDtZ.Rows[0].ItemArray[1]))) < (tarih.gunBaslangicUnix(Convert.ToInt32(dtIdCheck.Rows[0].ItemArray[10]))))
                        {

                            ZStartSQL = "SELECT tarih FROM satisana WHERE satisanaid=" + myDtZ.Rows[0].ItemArray[15] + " OR satisanaid=" + myDtZ.Rows[0].ItemArray[16];
                        }
                        else
                        {
                            if (ZKasseNo != 0)
                            {
                                ZStartSQL = "SELECT tarih FROM satisana WHERE (localbonid=" + (Convert.ToInt32(myDtZ.Rows[0].ItemArray[15]) == 0 ? 1 : myDtZ.Rows[0].ItemArray[15]) + " OR localbonid=" + myDtZ.Rows[0].ItemArray[16] + ") AND kasano=" + myDtZ.Rows[0].ItemArray[13] + " ORDER by localbonid";
                            }
                            else
                            {
                                ZStartSQL = "SELECT tarih FROM satisana WHERE (satisanaid=" + myDtZ.Rows[0].ItemArray[15] + " OR satisanaid=" + myDtZ.Rows[0].ItemArray[16] + ")   ORDER by satisanaid";
                            }
                        }
                        MySqlDataAdapter myDaZStartBon = new MySqlDataAdapter(ZStartSQL, myConn1);
                        DataTable myDtZStartBon = new DataTable();
                        myDtZStartBon.Rows.Clear();
                        myDaZStartBon.Fill(myDtZStartBon);



                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Datum von: " + tarih.tarih(Convert.ToInt32(myDtZStartBon.Rows[0].ItemArray[0])) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Datum bis: " + (myDtZStartBon.Rows.Count > 1 ? (tarih.tarih(Convert.ToInt32(myDtZStartBon.Rows[1].ItemArray[0]))) : (tarih.tarih(Convert.ToInt32(myDtZStartBon.Rows[0].ItemArray[0])))) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /*GRUP BILGISI BASI */
                        if (dtZpos.Rows.Count > 0)
                        {


                            for (int a = 0; a < dtZpos.Rows.Count; a++)
                            {
                                if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) == 43)
                                {
                                    RabatCouponTutar += (double)dtZpos.Rows[a].ItemArray[3];
                                }
                                else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) == 7)
                                {
                                    RabatTutar += (double)dtZpos.Rows[a].ItemArray[3];
                                }
                                else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) == 6)
                                {
                                    Gutschein += (double)dtZpos.Rows[a].ItemArray[3];
                                }

                                else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) != mwst && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7)
                                {
                                    if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                    {
                                        if (yuzde0likmiktar != 0)
                                        {

                                            // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                            // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", 0))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                    {
                                        if (yuzde7likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                    {
                                        if (yuzde19lukmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }




                                    mwst = Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]);
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                    if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtZpos.Rows[a].ItemArray[7].ToString(), string.Format("{0:C}", dtZpos.Rows[a].ItemArray[3]))) + "\n");


                                }
                                else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == mwst && Convert.ToInt16(dtZpos.Rows[0].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                {
                                    if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    else if (Convert.ToInt16(dtZpos.Rows[a].ItemArray[6]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtZpos.Rows[a].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtZpos.Rows[a].ItemArray[3];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtZpos.Rows[a].ItemArray[7].ToString(), string.Format("{0:C}", dtZpos.Rows[a].ItemArray[3]))) + "\n");

                                }
                            }
                            if (yuzde19lukmiktar != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                //m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            }

                            //(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`, `mwst`, `grupname`)
                            if (RabatTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Rabatt :", string.Format("{0:C}", RabatTutar))) + "\n");
                                if (RabatTutar < 0)
                                {
                                    string RabattSql = "";
                                    RabattSql = "SELECT SUM(`mwst0betrag`), SUM(`mwst7betrag`), SUM(`mwst19betrag`) FROM `rabatt` WHERE grupid=7 AND znr =" + berNo;
                                    MySqlDataAdapter myDaRabat = new MySqlDataAdapter(RabattSql, myConn1);
                                    DataTable dtRabatt = new DataTable();
                                    myDaRabat.Fill(dtRabatt);
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[0]) != 0)
                                    {
                                        rabatt0Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[0]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 0%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[0]))) + "\n");
                                    }
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[1]) != 0)
                                    {
                                        rabatt7Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[1]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 7%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[1]))) + "\n");
                                    }
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[2]) != 0)
                                    {
                                        rabatt19Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[2]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 19%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[2]))) + "\n");
                                    }
                                }

                            }
                            if (RabatCouponTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "RabattCoupon :", string.Format("{0:C}", RabatCouponTutar))) + "\n");
                                if (RabatTutar < 0)
                                {
                                    string RabattSql = "";
                                    RabattSql = "SELECT SUM(`mwst0betrag`), SUM(`mwst7betrag`), SUM(`mwst19betrag`) FROM `rabatt` WHERE grupid=43 AND znr =" + berNo;
                                    MySqlDataAdapter myDaRabat = new MySqlDataAdapter(RabattSql, myConn1);
                                    DataTable dtRabatt = new DataTable();
                                    myDaRabat.Fill(dtRabatt);
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[0]) != 0)
                                    {
                                        rabattCoupon0Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[0]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 0%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[0]))) + "\n");
                                    }
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[1]) != 0)
                                    {
                                        rabattCoupon7Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[1]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 7%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[1]))) + "\n");
                                    }
                                    if (Convert.ToDouble(dtRabatt.Rows[0].ItemArray[2]) != 0)
                                    {
                                        rabattCoupon19Brutto = Convert.ToDouble(dtRabatt.Rows[0].ItemArray[2]);
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "", "davon Mwst 19%:" + string.Format("{0:C}", dtRabatt.Rows[0].ItemArray[2]))) + "\n");
                                    }
                                }

                            }
                            if (Gutschein < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Gutschein :", string.Format("{0:C}", Gutschein))) + "\n");

                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO (TOTAL) :", string.Format("{0:C}", yuzde19lukmiktar + yuzde7likmiktar + yuzde0likmiktar + Gutschein + RabatCouponTutar + RabatTutar))) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));


                        }

                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "ES GIBT KEIN NEUER UMSATZ BEI DER KASSE!! \n DER Z-BERICHT WURDE SCHON AUSGEDRUCK!";
                            frmerror.ShowDialog();
                            return;
                        }




                        /*GRUP BILGISI SONU */

                        KombiBar = Convert.ToDouble(myDtZ.Rows[0].ItemArray[34]);
                        KombiEC = Convert.ToDouble(myDtZ.Rows[0].ItemArray[35]);
                        KombiScheck = Convert.ToDouble(myDtZ.Rows[0].ItemArray[36]);
                        KombiSay = Convert.ToInt16(myDtZ.Rows[0].ItemArray[33]);
                        //KombiTotal = KombiBar + KombiEC + KombiScheck;


                        /* KombiBar0 = (combimwst0 / KombiTotal) * KombiBar;
                         KombiBar7 = (combimwst7 / KombiTotal) * KombiBar;
                         KombiBar19 = (combimwst19 / KombiTotal) * KombiBar;

                         KombiEc0 = (combimwst0 / KombiTotal) * KombiEC;
                         KombiEc7 = (combimwst7 / KombiTotal) * KombiEC;
                         KombiEc19 = (combimwst19 / KombiTotal) * KombiEC;

                         KombiCek0 = (combimwst0 / KombiTotal) * KombiScheck;
                         KombiCek7 = (combimwst7 / KombiTotal) * KombiScheck;
                         KombiCek19 = (combimwst19 / KombiTotal) * KombiScheck;*/




                        if (Convert.ToDouble(myDtZ.Rows[0].ItemArray[30]) != 0)
                        {


                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(myDtZ.Rows[0].ItemArray[30]) + " X STORNO :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[10]))) + "\n")));

                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[37]))))));

                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", myDtZ.Rows[0].ItemArray[11]))));

                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[11]) - Convert.ToDouble(myDtZ.Rows[0].ItemArray[11]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", myDtZ.Rows[0].ItemArray[12]))));

                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[12]) - Convert.ToDouble(myDtZ.Rows[0].ItemArray[12]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");



                        }
                        if (Convert.ToDouble(myDtZ.Rows[0].ItemArray[25]) > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(myDtZ.Rows[0].ItemArray[28]) + " X BAR" + ((!(myDtZ.Rows[0].ItemArray[34] is DBNull) && (Convert.ToInt16(myDtZ.Rows[0].ItemArray[34])) > 0) ? "(+" + myDtZ.Rows[0].ItemArray[34] + " Kombi. Bar)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[25]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            if (Convert.ToInt16(myDtZ.Rows[0].ItemArray[34]) == 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[39]))))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[40]))))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", ((Convert.ToDouble(myDtZ.Rows[0].ItemArray[40])) - (Convert.ToDouble(myDtZ.Rows[0].ItemArray[40]) + KombiBar7) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[41])))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[41])) - (Convert.ToDouble(myDtZ.Rows[0].ItemArray[41]) + KombiBar19) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                            }

                        }
                        if (Convert.ToDouble(myDtZ.Rows[0].ItemArray[7]) > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(myDtZ.Rows[0].ItemArray[29]) + " X EC KARTE " + ((!(myDtZ.Rows[0].ItemArray[35] is DBNull) && (Convert.ToInt16(myDtZ.Rows[0].ItemArray[35])) > 0) ? "(+" + myDtZ.Rows[0].ItemArray[35] + " Kombi. EC-Karte)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[7]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            if (Convert.ToInt16(myDtZ.Rows[0].ItemArray[35]) == 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC/CC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[22]))))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC/CC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[8])))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble((Convert.ToDouble(myDtZ.Rows[0].ItemArray[8])) - ((Convert.ToDouble(myDtZ.Rows[0].ItemArray[8]) + KombiEc7) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC/CC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[9])))));

                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[9])) - ((Convert.ToDouble(myDtZ.Rows[0].ItemArray[9]) + KombiEc19) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                            }

                        }
                        if (Convert.ToDouble(myDtZ.Rows[0].ItemArray[17]) > 0)
                        {
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(myDtZ.Rows[0].ItemArray[31]) + " X SCHECK " + ((!(myDtZ.Rows[0].ItemArray[36] is DBNull) && (Convert.ToInt16(myDtZ.Rows[0].ItemArray[36])) > 0) ? "(+" + myDtZ.Rows[0].ItemArray[36] + " Kombi. Scheck)" : ""), string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[17]))))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (Convert.ToInt32(myDtZ.Rows[0].ItemArray[33]) == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(Convert.ToDouble(myDtZ.Rows[0].ItemArray[20])))))));
                                    scheck0tutar = Convert.ToDouble(Convert.ToDouble(myDtZ.Rows[0].ItemArray[20]));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", myDtZ.Rows[0].ItemArray[18]))));
                                    scheck7tutar = Convert.ToDouble(myDtZ.Rows[0].ItemArray[18]);
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", ((Convert.ToDouble(myDtZ.Rows[0].ItemArray[18])) - (Convert.ToDouble(myDtZ.Rows[0].ItemArray[18]) + KombiCek7) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[19])))));
                                    scheck19tutar = Convert.ToDouble(Convert.ToDouble(myDtZ.Rows[0].ItemArray[19]));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[19]) + KombiCek19) - (Convert.ToDouble(myDtZ.Rows[0].ItemArray[19]) + KombiCek19) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }

                            }


                        }

                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "[Kassenbarbestand: ", string.Format("{0:C}", (Convert.ToDouble(myDtZ.Rows[0].ItemArray[25]) + Convert.ToDouble(myDtZ.Rows[0].ItemArray[10]) + Gutschein)) + "]\n")));

                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "[ges. Anfangsbestand(Geldtransit):", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[38])) + "]\n")));
                        if (kombiBonAnzahl > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "[Kombi-Bon Anzahl:", kombiBonAnzahl.ToString() + "]\n")));
                        }
                        totalGeldEinlage = Convert.ToDouble(myDtZ.Rows[0].ItemArray[42]);
                        totalGeldEntnahme = Convert.ToDouble(myDtZ.Rows[0].ItemArray[43]);
                        if (totalGeldEinlage > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "[Bargeld Einlage: ", string.Format("{0:C}", totalGeldEinlage) + "]\n")));
                        }
                        if (totalGeldEntnahme > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "[Bargeld Entnahme: ", string.Format("{0:C}", -totalGeldEntnahme) + "]\n")));
                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "[Soll Bestand: ", string.Format("{0:C}", Convert.ToDouble(myDtZ.Rows[0].ItemArray[25]) + Convert.ToDouble(myDtZ.Rows[0].ItemArray[10]) + Gutschein + Convert.ToDouble(myDtZ.Rows[0].ItemArray[38]) + totalGeldEinlage - totalGeldEntnahme) + "]\n")));
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + myDtZ.Rows[0].ItemArray[32] + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                        if (Program.printerType == "bixolon")
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }








                    }
                }
                catch (Exception ff)
                {
                    MessageBox.Show(ff.Message);
                }

            }
        }
        private void ZNummerEkle(long berNo)
        {

            try
            {
                //if (Program.GlobalAyarlar["TaglichZ"] != 1)
                // {
                using (myConn = baglanti.myconn())
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string znrSQL = "UPDATE satisana SET znr=" + berNo + " WHERE kasano=" + Program.kasano + " AND  znr=0";
                    MySqlCommand cmdZnr = new MySqlCommand(znrSQL, myConn);
                    if (cmdZnr.ExecuteNonQuery() > 0)
                    {
                        try
                        {

                            //Update Bon_pos
                            string bonPosSQL = "UPDATE satisdetay SET znr=" + berNo + " WHERE znr=0 AND kasano=" + Program.kasano;
                            MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                            cmdbonPosSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //update Rabatt
                        try
                        {
                            string RabattSQL = "UPDATE rabatt SET znr=" + berNo + " WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                            cmdRabattSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Update Anfangbestand
                        try
                        {
                            if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                            {
                                string ABSQL = "UPDATE anfangbestand SET znr=" + berNo + " WHERE znr=0 AND kasseid=" + Program.kasano;
                                MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                cmdAB.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        // Abrechnungskreis
                        try
                        {
                            string AbrKrSQL = "UPDATE abrechnungskreis SET znr=" + berNo + " WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                            cmdAbrKrSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Kassenbuch
                        try
                        {
                            string KasaKrSQL = "UPDATE kassenbuch SET znr=" + berNo + "  WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                            cmdKBSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                    }
                }

            }
            catch (Exception ss)
            {
            }
        }
        public void zArchiveDruck(DateTime date)
        {

            iss_gdpdu.F_DataExport exportDatev = new iss_gdpdu.F_DataExport();
            exportDatev.IsletmeAyarlar = Program.IsletmeAyarlar;
            exportDatev.TableExportDATEVDSFinK("", 2.ToString(), Program.kasano, 0, 0);
            /*
            double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
            double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, ec7tutar = 0, ec19tutar = 0, storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0;
            double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
            double RabatTutar = 0;
            double RabatCouponTutar = 0;
            double Gutschein = 0;
            Int64 maxBonnr = 0, minBonNr = 0, mwst=0;
            using (myConn = baglanti.myconn())
            {
                try
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT * FROM zbericht WHERE kasano=" + Program.kasano + " AND erstelldatum<>0 AND tarih= " + tarih.gunBaslangic(date.Day, date.Month, date.Year), myConn);
                    DataTable dtUserBul = new DataTable("zbericht");
                    dtUserBul.Rows.Clear();
                    daUserBul.Fill(dtUserBul);
                    if (dtUserBul.Rows.Count > 0)
                    {
                        MySqlDataAdapter daZbul = new MySqlDataAdapter("SELECT * FROM zberichtpos WHERE znr= " + dtUserBul.Rows[0].ItemArray[2], myConn);
                        DataTable dtZBul = new DataTable("zbericht");
                        dtZBul.Rows.Clear();
                        daZbul.Fill(dtZBul);
                        if (dtZBul.Rows.Count > 0)
                        {


                            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                            if (m_Printer != null && m_Printer.DeviceEnabled == true)
                            {
                                CultureInfo culture = new CultureInfo("de-DE");
                                DateTime nowDate = DateTime.Now;							//System date
                                DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
                                dateFormat.MonthDayPattern = "MMMM";
                                string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
                                int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
                                long lRecLineCharsCount;
                                double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
                                string AuswahlSQL = "";
                                long erstellDatum = 0;




                                m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                                ArrayList items = new ArrayList();
                                try
                                {
                                    string[] fontlar = m_Printer.FontTypefaceList;
                                }
                                catch (Exception hata)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                                    frmerror.ShowDialog();


                                    return;
                                }
                                //m_Printer.PrintNormal(PrinterStation.Receipt, 
                                //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                                //<<<step3>>>--Start

                                if (m_Printer.CapRecBitmap == true)
                                {
                                    //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                                }



                                //<<<step3>>>--End
                                // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                                // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + strDate + " \n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                                if (Program.IsletmeAyarlar["usid"] != "")
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                                }
                                else
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                                }
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                            }



                            for (int i = 0; i < dtZBul.Rows.Count; i++)
                            {




                                if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) == 43)
                                {
                                    RabatCouponTutar += (double)dtZBul.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) == 7)
                                {
                                    RabatTutar += (double)dtZBul.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) == 6)
                                {
                                    Gutschein += (double)dtZBul.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) != mwst && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                    {
                                        if (yuzde0likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", 0))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                    {
                                        if (yuzde7likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                    {
                                        if (yuzde19lukmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }




                                    mwst = Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]);
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                    if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtZBul.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtZBul.Rows[i].ItemArray[0]))) + "\n");
                                  

                                }
                                else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == mwst && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtZBul.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtZBul.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtZBul.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtZBul.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtZBul.Rows[i].ItemArray[0]))) + "\n");
                                }

                                if (yuzde19lukmiktar != 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                }

                                //(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`, `mwst`, `grupname`)
                                if (RabatTutar < 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Rabatt :", string.Format("{0:C}", RabatTutar))) + "\n");
                                   
                                }
                                if (RabatCouponTutar < 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "RabattCoupon :", string.Format("{0:C}", RabatCouponTutar))) + "\n");
                                 
                                }
                                if (Gutschein < 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Gutschein :", string.Format("{0:C}", Gutschein))) + "\n");
                                   
                                }
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO (TOTAL) :", string.Format("{0:C}", yuzde19lukmiktar + yuzde7likmiktar + yuzde0likmiktar + Gutschein + RabatCouponTutar + RabatTutar))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                               

                            }
                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "ES GIBT KEIN NEUER UMSATZ BEI DER KASSE!! \n DER Z-BERICHT WURDE SCHON AUSGEDRUCK!";
                        frmerror.ShowDialog();
                        return;
                    }




                    /*GRUP BILGISI SONU *9/



                    if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))) + "\n")));
                        if (dtKombi.Rows.Count == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        }
                        stornoSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);
                        stornoTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                    }
                    else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR" + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[4]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[4] + " Kombi. Bar)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        if (dtKombi.Rows.Count == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        }
                        barSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                        barTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                    }
                    else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[5]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[5] + " Kombi. Ec Karte)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        if (dtKombi.Rows.Count == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        }
                        ecSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                        ecTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                    }
                    else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]) != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]) + " X SCHECK " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[6]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[6] + " Kombi. Scheck)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        if (dtKombi.Rows.Count == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "%% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        }
                        scheckSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                        scheckTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                    }



                    if ((barTutar == 0) && (KombiBar != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[4] + " X BAR " + ":", string.Format("{0:C}", (KombiBar)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        barTutar += KombiBar;
                    }
                    if ((scheckTutar == 0) && (KombiScheck != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[6] + " X SCHECK " + ":", string.Format("{0:C}", (KombiScheck)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        scheckTutar = +KombiScheck;

                    }
                    if ((ecTutar == 0) && (KombiEC != 0))
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[5] + " X EC KARTE " + ":", string.Format("{0:C}", (KombiEC)))));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        ecTutar = +KombiEC;
                    }
                    if (stornoSay == 0)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X STORNO :", "0,00 €")));
                    }
                    if (ecSay == 0)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X EC KARTE :", "0,00 € ")));
                    }
                    if (barSay == 0)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X BAR :", "0,00 €")));
                    }
                    this.kundenAnzahl = stornoSay + ecSay + barSay + CombiBonAnzahl;
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Kassenbarbestand: " + string.Format("{0:C}", (barTutar + stornoTutar + Gutschein)) + "]\n");
                    if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[ges. Anfangbestand: " + string.Format("{0:C}", AnfangBestandReturn()) + "]\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Soll Bestand: " + string.Format("{0:C}", AnfangBestandReturn() + barTutar + stornoTutar + Gutschein) + "]\n");

                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: " + KasiyerNullBonReturn() + ">" + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*9/
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    if (Program.printerType == "bixolon")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    }
                }

                catch
                {
                }
            }
            */

        }
        public void TSEInfoDruck(TSEClasses tseInfo, TSELocalInfo tseLocInfo)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm:ss", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "";
            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();

                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }

                    m_Printer.PrintNormal(PrinterStation.Receipt, "*");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "TSE Informationen für Steuerberater und \n Finanzverwaltung (§ 146a Abs. 4 AO ) \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "*");

                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "FIRMEN INFORMATIONEN\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    else
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "TSE HERSTELLER INFORMATIONEN\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    if (tseInfo != null)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Hersteller: Swissbit\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE ID:" + tseInfo.tseID + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE SN:" + tseInfo.tseSerial + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Sig. Algo.:" + tseInfo.tseSigAlgo + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Time Format:" + tseInfo.tseZeitFormat + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Encoding:" + tseInfo.tsePDEncoding + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Zertifikat 1:" + tseInfo.tseZertifikat1 + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Zertifikat 2:" + tseInfo.tseZertifikat2 + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Encoding:" + tseInfo.tsePDEncoding + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    }
                    if (tseLocInfo != null)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "TSE LOCAL INFORMATIONEN" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE ClientID:" + tseLocInfo.clienID + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE PIN:" + tseLocInfo.pin + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE PUK:" + tseLocInfo.puk + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE TimeAdmin PUK:" + tseLocInfo.timeadmin + "\n");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "TSE Drive:" + tseLocInfo.drive + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    }
                    if (Program.LocalKasseInfo != null)
                    {
                        //INSERT INTO `kasa`(`id`, `kasano`, `kasaad`, `makinaad`, `makinaip`, `KasseHerstNr`, `, `model`, `sw_brand`, `sw_version`, `basis_waehrung`, `keine_ust`) VALUES
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "SOFTWARE INFORMATIONEN" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Brand:" + Program.LocalKasseInfo.brand + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Model:" + Program.LocalKasseInfo.model + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "KasseHerstellerNr:" + Program.LocalKasseInfo.KasseHerstNr + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "SW-Version:" + Program.LocalKasseInfo.sw_version + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "SW-Brand:" + Program.LocalKasseInfo.sw_brand + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Kassenterminal-Nr:" + Program.LocalKasseInfo.kasano + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Kassenrechner Name:" + Program.LocalKasseInfo.makinaad + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Kassen IP Addresse:" + Program.LocalKasseInfo.makinaip + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "DATUM:" + strDate + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Techniker:" + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|2500uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    //m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                }
                catch
                {
                }
            }

        }
        public void TSEMeldeInfoDruck(MeldenDaten meldeDaten)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm:ss", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "";
            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();

                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }

                    m_Printer.PrintNormal(PrinterStation.Receipt, "*");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "TSE Informationen für Steuerberater und \n Finanzverwaltung (§ 146a Abs. 4 AO ) \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "*");

                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "FIRMEN INFORMATIONEN\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    else
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "TSE INFORMATIONEN\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    if (meldeDaten != null)
                    {
                        /*lb1.Items.Add("1.Art des eAS:" + meldeDaten.artDesEas.ToString());
                        lb1.Items.Add("2.Software des eAS:" + meldeDaten.softwareDesEas.ToString());
                        lb1.Items.Add("3.Software-Version des eAS:" + meldeDaten.softwareVersionDesEas.ToString());
                        lb1.Items.Add("4.Seriennumer des eAS/Software App:" + meldeDaten.seriennummerDesEas.ToString());
                        lb1.Items.Add("5.Hersteller des eAS:" + meldeDaten.herstellerDesEas.ToString());
                        lb1.Items.Add("6.Model des eAS:" + meldeDaten.modelDesEas.ToString());
                        lb1.Items.Add("7.Anschaffungsdatum des eAS:" + meldeDaten.anschaffungDesEas.ToString());
                        lb1.Items.Add("8.Inbetriebnahme des eAS:" + meldeDaten.artDesEas.ToString());
                        lb1.Items.Add("9. Ausserbetriebnahme des eAS:" + meldeDaten.ausserBetriebnahmeDesEas.ToString());
                        lb1.Items.Add("10.Grund der Ausserbetrieb. des eAS:" + meldeDaten.grundDerAusserbetriebnahmeDesEas.ToString());
                        lb1.Items.Add("11.Bemerkungen zum eAS:" + meldeDaten.bemerkungZumEas.ToString());
                        lb1.Items.Add("12.Seriennumer der TSE:" + meldeDaten.seriennummerDerTSE.ToString());
                        lb1.Items.Add("13.BSI Zertifizierungs-ID:" + meldeDaten.BSIZertifierungsID.ToString());
                        lb1.Items.Add("14.Inbetriebnahme/Aktivierung der TSE:" + meldeDaten.inbetriebnahmeDerTSE.ToString());
                        lb1.Items.Add("15.Art/Bauform der TSE:" + meldeDaten.artBauFormDerTSE.ToString());*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "1.Art des eAS: " + meldeDaten.artDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "2.Software des eAS:" + meldeDaten.softwareDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "3.Software-Version des eAS:" + meldeDaten.softwareVersionDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "4.Seriennumer des eAS/Software App:" + meldeDaten.seriennummerDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "5.Hersteller des eAS:" + meldeDaten.herstellerDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "6.Model des eAS:" + meldeDaten.modelDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "7.Anschaffungsdatum des eAS:" + meldeDaten.anschaffungDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "8.Inbetriebnahme des eAS:" + meldeDaten.artDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "9. Ausserbetriebnahme des eAS:" + meldeDaten.ausserBetriebnahmeDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "10.Grund der Ausserbetrieb. des eAS:" + meldeDaten.grundDerAusserbetriebnahmeDesEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "11.Bemerkungen zum eAS:" + meldeDaten.bemerkungZumEas.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "12.Seriennumer der TSE:" + meldeDaten.seriennummerDerTSE.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "13.BSI Zertifizierungs-ID:" + meldeDaten.BSIZertifierungsID.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "14.Inbetriebnahme/Aktivierung der TSE:" + meldeDaten.inbetriebnahmeDerTSE.ToString() + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "15.Art/Bauform der TSE:" + meldeDaten.artBauFormDerTSE.ToString() + "\n");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    }


                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "DATUM:" + strDate + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|iC" + "Techniker:" + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|2500uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    //m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                }
                catch
                {
                }
            }

        }
        public void HandAufladeTicketDruck(int VerkaudID)
        {
            try
            {
                using (myConn = baglanti.myconn())
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string insertSQL = "SELECT * FROM handyaufladelog WHERE id=" + VerkaudID;
                    MySqlDataAdapter myDa = new MySqlDataAdapter(insertSQL, myConn);
                    DataTable dt = new DataTable();
                    dt.Rows.Clear();
                    myDa.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {


                        CultureInfo culture = new CultureInfo("de-DE");
                        DateTime nowDate = DateTime.Now;							//System date
                        DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
                        dateFormat.MonthDayPattern = "MMMM";
                        string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm:ss", culture);
                        int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
                        long lRecLineCharsCount;
                        double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
                        string AuswahlSQL = "";
                        //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                        if (m_Printer != null && m_Printer.DeviceEnabled == true)
                        {



                            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                            ArrayList items = new ArrayList();
                            try
                            {
                                string[] fontlar = m_Printer.FontTypefaceList;
                            }
                            catch (Exception hata)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                                frmerror.ShowDialog();

                                return;
                            }
                            //m_Printer.PrintNormal(PrinterStation.Receipt, 
                            //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                            //<<<step3>>>--Start

                            if (m_Printer.CapRecBitmap == true)
                            {
                                //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                            }
                            //SELECT `id`, `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`
                            m_Printer.PrintNormal(PrinterStation.Receipt, "****** WIEDERDRUCK ******\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "HandyAuflade-Karte Informationen \n");
                            //m_Printer.PrintNormal(PrinterStation.Receipt, "*");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            //<<<step3>>>--End
                            //m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "Verkaufsdatum:" + tarih.tarih(Convert.ToInt32(dt.Rows[0].ItemArray[1])) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|3C" + "\u001b|bC" + "PIN-NR:" + dt.Rows[0].ItemArray[6] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "CARD-NAME:" + dt.Rows[0].ItemArray[8] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "BATCH-NUMMER:" + dt.Rows[0].ItemArray[3] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1C" + "TRANSACTION-ID:" + dt.Rows[0].ItemArray[7] + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        }


                    }
                }
            }
            catch
            {
            }

        }
        public void XZ_Korrektur(Int32 Datum, int berNoSystem, Int32 LastBonDatum)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "";
            long erstellDatum = 0;

            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {



                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();


                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }

                    Random rndm = new Random();
                    erstellDatum = LastBonDatum + rndm.Next(0, 300);

                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Z BERICHT-" + tarih.tarih(erstellDatum) + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    else
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + tarih.tarih(erstellDatum) + "\n");
                    long berNo;
                    using (myConn = baglanti.myconn())
                    {

                        /*GRUP BILGISI BASI */
                        int mwst = -1;
                        double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                        double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, ec7tutar = 0, ec19tutar = 0, storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0;
                        double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                        double RabatTutar = 0;
                        double RabatCouponTutar = 0;
                        double Gutschein = 0;
                        Int64 maxBonnr = 0, minBonNr = 0;
                        // Zpos insert
                        string ZposSQL = "INSERT INTO `zberichtpos`(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`,`mwst`, `grupname`) VALUES ";
                        /*INNER İLE LEFTi değiştir UNUTMA*/
                        string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, satisdetay.mwst, max(fisno) as MaxBonNr, min(fisno) as MinBonNr FROM satisdetay" +
                        " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid  LEFT JOIN satisana ON satisana.satisanaid=satisdetay.fisno" +
                        " WHERE satisdetay.kasano=" + Program.kasano + " AND satisdetay.tarih>" + Datum + " AND satisdetay.tarih<=" + erstellDatum + " GROUP BY  satisdetay.mwst, artikelgrup.grupid ORDER BY satisdetay.mwst ASC, grupad ASC";
                        MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn);

                        DataTable dtSatisSayisi = new DataTable("satisdetay");
                        dtSatisSayisi.Rows.Clear();
                        daSatisSayisi.Fill(dtSatisSayisi);
                        if (dtSatisSayisi.Rows.Count > 0)
                        {
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();
                            }
                            string sqltarih1 = "SELECT * FROM zbericht WHERE tarih =" + Datum + " AND erstelldatum=0 AND kasano=" + Program.kasano;
                            MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn);
                            DataTable dttar1 = new DataTable();
                            dttar1.Rows.Clear();
                            datar1.Fill(dttar1);



                            if (dttar1.Rows.Count > 0)
                            {
                                berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                            }
                            else
                            {



                                MySqlCommand coZ = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano, erstelldatum ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + Datum + "," + Program.kasano + "," + erstellDatum + "  FROM zbericht ", myConn);
                                coZ.ExecuteNonQuery();
                                string sqltarih11 = "SELECT * FROM zbericht WHERE id=" + coZ.LastInsertedId;
                                MySqlDataAdapter datar11 = new MySqlDataAdapter(sqltarih11, myConn);
                                DataTable dttar11 = new DataTable();
                                dttar11.Rows.Clear();
                                datar11.Fill(dttar11);
                                berNo = Convert.ToInt16(dttar11.Rows[0].ItemArray[2]);
                            }



                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + ">" + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Far;

                            for (int i = 0; i < dtSatisSayisi.Rows.Count; i++)
                            {
                                maxBonnr = Convert.ToInt64(dtSatisSayisi.Rows[i].ItemArray[4]);
                                minBonNr = Convert.ToInt64(dtSatisSayisi.Rows[i].ItemArray[5]);

                                if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 43)
                                {
                                    RabatCouponTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 7)
                                {
                                    RabatTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 6)
                                {
                                    Gutschein += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                    {
                                        if (yuzde0likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", 0))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                    {
                                        if (yuzde7likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                    {
                                        if (yuzde19lukmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }




                                    mwst = Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]);
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");
                                    ZposSQL += "(NULL," + berNo + "," + Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) + "," + vA.virgulayikla(Convert.ToDouble(dtSatisSayisi.Rows[i].ItemArray[0])) + "," + Program.kasano + ",'" + Program.kasaAd + "'," + mwst + ",'" + dtSatisSayisi.Rows[i].ItemArray[1].ToString() + "'),";
                                    // (`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`) VALUES ";

                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");
                                    ZposSQL += "(NULL," + berNo + "," + Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) + "," + vA.virgulayikla(Convert.ToDouble(dtSatisSayisi.Rows[i].ItemArray[0])) + "," + Program.kasano + ",'" + Program.kasaAd + "'," + mwst + ",'" + dtSatisSayisi.Rows[i].ItemArray[1].ToString() + "'),";
                                }
                            }
                            if (yuzde19lukmiktar != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            }

                            //(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`, `mwst`, `grupname`)
                            if (RabatTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Rabatt :", string.Format("{0:C}", RabatTutar))) + "\n");
                                ZposSQL += "(NULL," + berNo + ",7," + vA.virgulayikla(RabatTutar) + "," + Program.kasano + ",'" + Program.kasaAd + "'," + mwst + ",'Rabatt'),";
                            }
                            if (RabatCouponTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "RabattCoupon :", string.Format("{0:C}", RabatCouponTutar))) + "\n");
                                ZposSQL += "(NULL," + berNo + ",43," + vA.virgulayikla(RabatCouponTutar) + "," + Program.kasano + ",'" + Program.kasaAd + "'," + mwst + ",'RabattCoupon'),";
                            }
                            if (Gutschein < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Gutschein :", string.Format("{0:C}", Gutschein))) + "\n");
                                ZposSQL += "(NULL," + berNo + ",6," + vA.virgulayikla(Gutschein) + "," + Program.kasano + ",'" + Program.kasaAd + "'," + mwst + ",'Gutschein'),";
                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO (TOTAL) :", string.Format("{0:C}", yuzde19lukmiktar + yuzde7likmiktar + yuzde0likmiktar + Gutschein + RabatCouponTutar + RabatTutar))) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            try
                            {
                                if (myConn.State == ConnectionState.Closed)
                                    myConn.Open();
                                ZposSQL = ZposSQL.Substring(0, ZposSQL.Length - 1);
                                MySqlCommand cmdZPos = new MySqlCommand(ZposSQL, myConn);
                                cmdZPos.ExecuteNonQuery();

                            }
                            catch (Exception rr)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = rr.Message + "\n\n" + rr.StackTrace;
                                frmerror.ShowDialog();

                            }

                        }

                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "ES GIBT KEIN NEUER UMSATZ BEI DER KASSE!! \n DER Z-BERICHT WURDE SCHON AUSGEDRUCK!";
                            frmerror.ShowDialog();
                            return;
                        }




                        /*GRUP BILGISI SONU */



                        string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                               "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                               "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay " +
                               ", SUM(`toplamEc`), SUM(toplamStorno), SUM(toplamScheck) FROM satisana WHERE " +
                                "  satisana.kasano=" + Program.kasano + " AND satisana.tarih>" + Datum + " AND satisana.tarih<=" + erstellDatum + "   GROUP BY odemeturu ORDER BY odemeturu ASC";
                        MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                        DataTable dtSatisAna = new DataTable("satisana");
                        dtSatisAna.Rows.Clear();
                        daSatisAna.Fill(dtSatisAna);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                        int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                        string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 " +
                            " AND satisana.kasano=" + Program.kasano + " AND satisdetay.tarih>" + Datum + " AND satisdetay.tarih<=" + erstellDatum + "  GROUP BY satisdetay.mwst";
                        MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn);
                        DataTable dtKombi = new DataTable();
                        daKombi.Fill(dtKombi);
                        int CombiBonAnzahl = dtKombi.Rows.Count;
                        string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay," +
                            " SUM(CASE WHEN `toplamEc` > 0 THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND " +
                            "   satisana.kasano=" + Program.kasano;
                        MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn);
                        DataTable dtKombiSumme = new DataTable();
                        daKombiSumme.Fill(dtKombiSumme);
                        if (dtKombi.Rows.Count > 0)
                        {
                            double KombiTotal = 0;
                            for (int c = 0; c < dtKombi.Rows.Count; c++)
                            {

                                KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                KombiTotal = KombiBar + KombiEC + KombiScheck;

                            }


                        }
                        for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                        {
                            if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))) + "\n")));
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                stornoSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);
                                stornoTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                            }
                            else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0))
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR" + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[4]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[4] + " Kombi. Bar)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                barSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                barTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }
                            else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0))
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[5]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[5] + " Kombi. Ec Karte)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                ecSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                ecTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }
                            else if ((Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]) != 0))
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]) + " X SCHECK " + (Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6] is DBNull ? 0 : dtKombiSumme.Rows[0].ItemArray[6]) > 0 ? "(+" + dtKombiSumme.Rows[0].ItemArray[6] + " Kombi. Scheck)" : "") + ":", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck)))));
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                if (dtKombi.Rows.Count == 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "%% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " SCHECK " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))));
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                                }
                                scheckSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                                scheckTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck;// +Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                            }


                        }
                        if ((barTutar == 0) && (KombiBar != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[4] + " X BAR " + ":", string.Format("{0:C}", (KombiBar)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            barTutar += KombiBar;
                        }
                        if ((scheckTutar == 0) && (KombiScheck != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[6] + " X SCHECK " + ":", string.Format("{0:C}", (KombiScheck)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            scheckTutar = +KombiScheck;

                        }
                        if ((ecTutar == 0) && (KombiEC != 0))
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtKombiSumme.Rows[0].ItemArray[5] + " X EC KARTE " + ":", string.Format("{0:C}", (KombiEC)))));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            ecTutar = +KombiEC;
                        }
                        if (stornoSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X STORNO :", "0,00 €")));
                        }
                        if (ecSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X EC KARTE :", "0,00 € ")));
                        }
                        if (barSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X BAR :", "0,00 €")));
                        }
                        this.kundenAnzahl = stornoSay + ecSay + barSay + CombiBonAnzahl;
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Kassenbarbestand: " + string.Format("{0:C}", (barTutar + stornoTutar + Gutschein)) + "]\n");
                        if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[ges. Anfangbestand: " + string.Format("{0:C}", AnfangBestandReturn()) + "]\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Soll Bestand: " + string.Format("{0:C}", AnfangBestandReturn() + barTutar + stornoTutar + Gutschein) + "]\n");

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: " + KasiyerNullBonReturn() + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                        if (Program.printerType == "bixolon")
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }

                        string SqlZ = "UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde19lukmiktar + yuzde7likmiktar + yuzde0likmiktar) + ", toplammwst=" + vA.virgulayikla((yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))) + (yuzde7likmiktar - yuzde0likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))) + ",toplam7tutar=" + vA.virgulayikla(yuzde7likmiktar) +
                           ", toplam19tutar=" + vA.virgulayikla(yuzde19lukmiktar) + ", toplamEC=" + vA.virgulayikla(ecTutar) + ", toplamScheck=" + vA.virgulayikla(scheckTutar) + ",toplamstorno=" + vA.virgulayikla(stornoTutar) + ", erstelldatum=" + erstellDatum + ", kasano=" + Program.kasano +
                            " ,toplamBar=" + vA.virgulayikla(barTutar) + ",KasseHerstNr='" + Program.HerstellerKasseID + "',kassename='" + Program.kasaAd + "', druckAnzahl=druckAnzahl+1, startBonID=" + minBonNr + ", endBonID=" + maxBonnr + " WHERE Zberichtno= " + berNo;
                        MySqlCommand coZInsert = new MySqlCommand(SqlZ, myConn);
                        if (coZInsert.ExecuteNonQuery() > 0)
                        {

                            string znrSQL = "UPDATE satisana SET znr=" + berNo + " WHERE kasano=" + Program.kasano + " AND  satisana.tarih>" + Datum + " AND satisana.tarih<=" + erstellDatum + "";
                            MySqlCommand cmdZnr = new MySqlCommand(znrSQL, myConn);
                            if (cmdZnr.ExecuteNonQuery() > 0)
                            {
                                //Update Bon_pos
                                string bonPosSQL = "UPDATE satisdetay SET znr=" + berNo + " WHERE  satisdetay.tarih>" + Datum + " AND satisdetay.tarih<=" + erstellDatum + " AND kasano=" + Program.kasano;
                                MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                                cmdbonPosSQL.ExecuteNonQuery();
                                //update Rabatt
                                string RabattSQL = "UPDATE rabatt SET znr=" + berNo + " WHERE tarih>" + Datum + " AND tarih<=" + erstellDatum + " AND kassenr=" + Program.kasano;
                                MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();
                                //Update Anfangbestand
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string ABSQL = "UPDATE anfangbestand SET znr=" + berNo + " WHERE datum>" + Datum + " AND datum<=" + erstellDatum + " AND kasseid=" + Program.kasano;
                                    MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                    cmdAB.ExecuteNonQuery();
                                }
                                // Abrechnungskreis
                                string AbrKrSQL = "UPDATE abrechnungskreis SET znr=" + berNo + " WHERE datum>" + Datum + " AND datum<=" + erstellDatum + "AND kassenr=" + Program.kasano;
                                MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();
                            }
                            //businesscases, Payment
                            DSFinK_Businesscases(berNo, yuzde19lukmiktar, yuzde7likmiktar, yuzde0likmiktar, Gutschein, RabatCouponTutar, RabatTutar, erstellDatum, barTutar, ecTutar, scheckTutar, stornoTutar);
                        }

                    }
                }
                catch (Exception ff)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = ff.Message + "\n";
                    frmerror.ShowDialog();

                }

            }
        }
        private void DSFinK_Businesscases(long znr, double yuzde19lukmiktar, double yuzde7likmiktar, double yuzde0likmiktar, double Gutschein, double RabatCouponTutar, double RabatTutar, long erstelldatum, double barTutar, double ecTutar, double scheckTutar, double stornoTutar)
        {
            using (myConn = baglanti.myconn())
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }


                try
                {
                    //bbusiness case
                    MySqlCommand cmdBusinesscases = new MySqlCommand();
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19lukmiktar", yuzde19lukmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19luknetto", (yuzde19lukmiktar / 119) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19lukmwst", yuzde19lukmiktar - (yuzde19lukmiktar / 119) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7likmiktar", yuzde7likmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7liknetto", (yuzde7likmiktar / 107) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7likmwst", yuzde7likmiktar - (yuzde7likmiktar / 107) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde0likmiktar", yuzde0likmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@Gutschein", Gutschein);
                    cmdBusinesscases.Parameters.AddWithValue("@RabatCouponTutar", RabatCouponTutar);
                    cmdBusinesscases.Parameters.AddWithValue("@RabatTutar", RabatTutar);
                    string sqlSatisAna = "INSERT INTO `businesscases`( `datum`, `znr`, `kassenr`, `gv_typ`, `gv_name`, `agentur_id`, `ustid_id`, `z_umsatz_brutto`, `z_umsatz_netto`, `z_umsatz_ust`) VALUES" +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 7 Proz.',0,2,@yuzde19lukmiktar,@yuzde19luknetto,@yuzde19lukmwst)," +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 19 Proz.',0,1,@yuzde7likmiktar,@yuzde7liknetto,@yuzde7likmwst)," +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 0 Proz.',0,6,@yuzde0likmiktar,@yuzde0likmiktar,0)";
                    //rabatt Suche
                    if (RabatTutar != 0 || RabatCouponTutar != 0)
                    {

                        string RabattSQL = "SELECT SUM(`rabatotalmenge`) as BruttoTotal, SUM(`mwst7`) AS Netto7, SUM(`mwst7betrag`) As Brutto7,SUM(`mwst19`) AS Netto19, SUM(`mwst19betrag`) As Brutto19, SUM(`mwst0betrag`)AS Netto0 FROM `rabatt` WHERE  znr=" + znr + "  AND kassenr=" + Program.kasano;
                        MySqlDataAdapter cmdRabattSQL = new MySqlDataAdapter(RabattSQL, myConn);
                        DataTable dtRabattSearch = new DataTable();
                        cmdRabattSQL.Fill(dtRabattSearch);
                        if (dtRabattSearch.Rows.Count > 0)
                        {
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7Brutto", dtRabattSearch.Rows[0].ItemArray[2]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7Netto", (Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[2]) - Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[1])));
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7mwst", dtRabattSearch.Rows[0].ItemArray[1]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19Brutto", dtRabattSearch.Rows[0].ItemArray[4]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19Netto", (Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[4]) - Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[3])));
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19mwst", dtRabattSearch.Rows[0].ItemArray[3]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar0Brutto", dtRabattSearch.Rows[0].ItemArray[5]);

                            sqlSatisAna += ",(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 7 Proz.',0,2,@RabatTutar7Brutto,@RabatTutar7Netto,@RabatTutar7mwst)," +
                   "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 19 Proz.',0,1,@RabatTutar19Brutto,@RabatTutar19Netto,@RabatTutar19mwst)," +
                  "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 0 Proz.',0,6,@RabatTutar0Brutto,@RabatTutar0Brutto,0)";


                        }


                    }
                    // Buraya Gutscheinda eklenebilir
                    cmdBusinesscases.CommandText = sqlSatisAna;
                    cmdBusinesscases.Connection = myConn;
                    cmdBusinesscases.ExecuteNonQuery();
                    // Payment
                    MySqlCommand cmdPayment = new MySqlCommand();
                    cmdPayment.Parameters.AddWithValue("@BarTutar", barTutar + stornoTutar);
                    cmdPayment.Parameters.AddWithValue("@ecTutar", ecTutar);
                    cmdPayment.Parameters.AddWithValue("@scheckTutar", scheckTutar);
                    cmdPayment.Parameters.AddWithValue("@stornoTutar", stornoTutar);
                    string sqlPayment = "INSERT INTO `payment`( `datum`, `znr`,`kassenr`, `zahlart_typ`, `zahlart_name`, `zahlart_betrag`) VALUES ";
                    if (barTutar != 0)
                    {
                        sqlPayment += " (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Bar','Bar',@BarTutar)";
                    }
                    if (ecTutar != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Unbar','Ec Karte',@ecTutar)";
                    }
                    if (scheckTutar != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Unbar','Scheck',@scheckTutar)";
                    }
                    cmdPayment.CommandText = sqlPayment;
                    cmdPayment.Connection = myConn;
                    try
                    {
                        cmdPayment.ExecuteNonQuery();
                    }
                    catch (Exception dd)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = dd.Message + "\n" + dd.StackTrace;
                        frmerror.ShowDialog();

                    }

                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();


                }
                //KasiyerSofortStorno();
            }

        }
        private int KasiyerNullBonReturn()
        {
            using (myConn = baglanti.myconn())
            {

                try
                {
                    string sqlSatisAna = "SELECT sum(`nullbon`) FROM usertakip WHERE tarih >=" +
                            tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) +
                            " AND  kasaid=" + Program.kasano;
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        return dtSatisAna.Rows[0].ItemArray[0] is DBNull ? 0 : Convert.ToInt16(dtSatisAna.Rows[0].ItemArray[0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            }

        }
        private double AnfangBestandReturn()
        {
            using (myConn = baglanti.myconn())
            {

                try
                {
                    string sqlSatisAna = "SELECT sum(`betrag`) FROM anfangbestand WHERE kasseid=" + Program.kasano + " AND znr=0";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Anfangsbetand Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            }

        }
        private double GeldEinlageReturn()
        {
            using (myConn = baglanti.myconn())
            {

                try
                {
                    string sqlSatisAna = "SELECT sum(`tutar`) FROM kassenbuch WHERE type=1 AND kassenr=" + Program.kasano + " AND znr=0 AND (anfangbestand=18 OR anfangbestand=20 or anfangbestand=23)";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Geldeinlage Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            }

        }
        private double GeldEntnahmeReturn()
        {
            using (myConn = baglanti.myconn())
            {

                try
                {
                    string sqlSatisAna = "SELECT sum(`tutar`) FROM kassenbuch WHERE type=2 AND kassenr=" + Program.kasano + " AND znr=0 AND (anfangbestand=19 OR anfangbestand=22 or anfangbestand=24)";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Geldentnahme Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            }

        }
        private double AnfangBestandReturnBedienr(Int32 BedinerId)
        {
            using (myConn = baglanti.myconn())
            {

                try
                {
                    string sqlSatisAna = "SELECT sum(`betrag`) FROM anfangbestand WHERE datum >=" +
                            tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND datum <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) +
                            " AND  kasseid=" + Program.kasano + " AND znr=0 AND bedienerid= " + BedinerId;
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            }

        }
        public void XDruck()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "", AuswahlSQL2 = "";

            AuswahlSQL = "INNER JOIN satisana ON satisdetay.fisno=satisanaid WHERE satisana.znr=0 ";
            AuswahlSQL2 = " satisana.znr=0 ";

            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {

                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();
                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "X BERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " St.-Nr :" + Program.IsletmeAyarlar["usid"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " KasseTerm-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                    using (myConn = baglanti.myconn())
                    {
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }

                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + "-" + Program.kasano + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /*GRUP BILGISI BASI */
                        int mwst = -1;
                        double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                        double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, ec7tutar = 0, ec19tutar = 0, storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0;
                        double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                        double RabatTutar = 0;
                        double RabatCouponTutar = 0;
                        double Gutschein = 0;
                        /*INNER İLE LEFTi değiştir UNUTMA*/
                        string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, satisdetay.mwst FROM satisdetay" +
                        " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid " +
                        AuswahlSQL + " AND satisdetay.kasano=" + Program.kasano +
                        " GROUP BY  satisdetay.mwst, artikelgrup.grupid ORDER BY satisdetay.mwst ASC, grupad ASC";
                        MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn);
                        DataTable dtSatisSayisi = new DataTable("satisdetay");
                        dtSatisSayisi.Rows.Clear();
                        daSatisSayisi.Fill(dtSatisSayisi);
                        if (dtSatisSayisi.Rows.Count > 0)
                        {
                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Far;

                            for (int i = 0; i < dtSatisSayisi.Rows.Count; i++)
                            {
                                if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 43)
                                {
                                    RabatCouponTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 7)
                                {
                                    RabatTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 6)
                                {
                                    Gutschein += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (mwst == 0)
                                    {
                                        if (yuzde0likmiktar != (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0))
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", 0))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) + "% :", string.Format("{0:C}", yuzde0likmiktar))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7))
                                    {
                                        if (yuzde7likmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) + "% :", string.Format("{0:C}", yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100)))))) + "\n");

                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }
                                    else if (mwst == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19))
                                    {
                                        if (yuzde19lukmiktar != 0)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        }
                                    }




                                    mwst = Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]);
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");


                                }
                                else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                {
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 6)
                                    {
                                        yuzde0likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");
                                }
                            }
                            if (yuzde19lukmiktar != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO " + (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) + "% :", string.Format("{0:C}", yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            }


                            if (RabatTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Rabatt :", string.Format("{0:C}", RabatTutar))) + "\n");
                            }
                            if (RabatCouponTutar < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "RabattCoupon :", string.Format("{0:C}", RabatCouponTutar))) + "\n");

                            }
                            if (Gutschein < 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Gutschein :", string.Format("{0:C}", Gutschein))) + "\n");

                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO (TOTAL) :", string.Format("{0:C}", yuzde19lukmiktar + yuzde7likmiktar + yuzde0likmiktar + Gutschein + RabatCouponTutar + RabatTutar))) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                        }






                        /*GRUP BILGISI SONU 
                        "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                            "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                            "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay, sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay  FROM
                        */
                        string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                              "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                              "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay FROM satisana WHERE" +
                              AuswahlSQL2 + " AND kasano=" + Program.kasano + "  GROUP BY odemeturu ORDER BY odemeturu DESC";
                        MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                        DataTable dtSatisAna = new DataTable("satisana");
                        dtSatisAna.Rows.Clear();
                        daSatisAna.Fill(dtSatisAna);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                        int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0, KombiBarSay = 0, KombiEcSay = 0, KombiScheckSay = 0;
                        string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 AND " +
                            " satisana.znr=0 AND satisana.kasano=" + Program.kasano + "  GROUP BY satisdetay.mwst";
                        MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn);
                        DataTable dtKombi = new DataTable();
                        daKombi.Fill(dtKombi);
                        string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay, SUM(CASE WHEN `toplamEc` > 0  THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND znr=0 AND satisana.kasano=" + Program.kasano;
                        MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn);
                        DataTable dtKombiSumme = new DataTable();
                        daKombiSumme.Fill(dtKombiSumme);

                        if (dtKombi.Rows.Count > 0)
                        {
                            double KombiTotal = 0;
                            for (int c = 0; c < dtKombi.Rows.Count; c++)
                            {

                                KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                KombiBarSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]);

                                KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                KombiScheckSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]);

                                KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                KombiScheckSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]);

                                KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                KombiTotal = KombiBar + KombiEC + KombiScheck;

                            }


                        }
                        for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                        {
                            if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                            {
                                /* m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[0])) + "\n"));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19)))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                stornoSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);
                                stornoTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0)
                            {
                                /* m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]))))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                barSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                barTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar;
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                /* m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]))))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                ecSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                ecTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC;
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]) != 0)
                            {
                                /* m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|300uF");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]))))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                scheckSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                                scheckTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck;
                            }
                        }

                        if (stornoSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X STORNO :", "0,00 €")));
                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, stornoSay + " X STORNO :", string.Format("{0:C}", stornoTutar))));
                        }
                        if (ecSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X EC KARTE " + (KombiEcSay == 0 ? "" : " +" + KombiEcSay) + ":", "0,00 € ")));
                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, ecSay + " X EC KARTE " + (KombiEcSay == 0 ? "" : " +" + KombiEcSay) + ":", string.Format("{0:C}", ecTutar))));
                        }
                        if (barSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X BAR " + (KombiBarSay == 0 ? "" : " +" + KombiBarSay) + ":", "0,00 €")));
                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, barSay + " X BAR " + (KombiBarSay == 0 ? "" : " +" + KombiBarSay) + ":", string.Format("{0:C}", barTutar))));
                        }
                        if (scheckSay == 0)
                        {

                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X SCHECK " + (KombiScheckSay == 0 ? "" : " +" + KombiScheckSay) + ":", string.Format("{0:C}", KombiScheck))));

                        }
                        else
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, scheckSay + " X SCHECK " + (KombiScheckSay == 0 ? "" : " +" + KombiScheckSay) + ":", string.Format("{0:C}", scheckTutar))));
                        }
                        this.kundenAnzahl = stornoSay + ecSay + barSay;
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Kassenbarbestand: " + string.Format("{0:C}", (barTutar + stornoTutar)) + "]\n");
                        if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[gesamt Anfangbestand: " + string.Format("{0:C}", AnfangBestandReturn()) + "]\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Soll Bestand: " + string.Format("{0:C}", AnfangBestandReturn() + barTutar + stornoTutar + Gutschein) + "]\n");

                        }
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade(nullBon):" + KasiyerNullBonReturn() + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        if (Program.printerType == "bixolon")
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    }
                }
                catch
                {
                }

            }

            /* CultureInfo culture = new CultureInfo("de-DE");
             DateTime nowDate = DateTime.Now;							//System date
             DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
             dateFormat.MonthDayPattern = "MMMM";
             string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
             int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
             long lRecLineCharsCount;


             //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
             if (m_Printer != null && m_Printer.DeviceEnabled == true)
             {

                 try
                 {

                     m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                     ArrayList items = new ArrayList();
                     try
                     {
                         string[] fontlar = m_Printer.FontTypefaceList;
                     }
                     catch (Exception hata)
                     {
                         MessageBox.Show("Printer Error!\n Orginal Message:" + hata.Message);
                         return;
                     }
                     //m_Printer.PrintNormal(PrinterStation.Receipt, 
                     //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                     //<<<step3>>>--Start

                     if (m_Printer.CapRecBitmap == true)
                     {
                         //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                     }



                     //<<<step3>>>--End
                     // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                     // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                     m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "X BERICHT-" + strDate + " \n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " St.-Nr :" + Program.IsletmeAyarlar["usid"] + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                     using (myConn = baglanti.myconn())
                     {
                         if (myConn.State == ConnectionState.Closed)
                         {
                             myConn.Open();
                         }
                         string sqltarih1 = "SELECT * FROM zbericht where tarih =" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                         MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn);
                         DataTable dttar1 = new DataTable();
                         dttar1.Rows.Clear();
                         datar1.Fill(dttar1);
                         long berNo;
                         if (dttar1.Rows.Count > 0)
                         {
                             berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                         }
                         else
                         {
                             MySqlCommand coZ = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht ", myConn);
                             coZ.ExecuteNonQuery();
                             berNo = coZ.LastInsertedId;
                         }
                         // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + ">" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                         /*GRUP BILGISI BASI *9/
                         int mwst = 0;
                         double yuzde7likmiktar = 0;
                         double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, ec7tutar = 0, ec19tutar = 0, storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0;

                         double RabatTutar = 0;
                         double RabatCouponTutar = 0;
                         /*INNER İLE LEFTi değiştir UNUTMA*9/
                         string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, artikelgrup.mwst FROM satisdetay" +
                         " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid " +
                         " WHERE tarih >=" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND kasano=" + Program.kasano + " GROUP BY  artikelgrup.grupid ORDER BY artikelgrup.mwst ASC, grupad ASC";
                         MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn);
                         DataTable dtSatisSayisi = new DataTable("satisdetay");
                         dtSatisSayisi.Rows.Clear();
                         daSatisSayisi.Fill(dtSatisSayisi);
                         if (dtSatisSayisi.Rows.Count > 0)
                         {
                             StringFormat format = new StringFormat();
                             format.Alignment = StringAlignment.Far;

                             for (int i = 0; i < dtSatisSayisi.Rows.Count; i++)
                             {
                                 if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 43)
                                 {
                                     RabatCouponTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                 }
                                 else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) == 7)
                                 {
                                     RabatTutar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                 }
                                 else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                 {
                                     if (mwst != 0)
                                     {
                                         if (yuzde7likmiktar != 0)
                                         {
                                             m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                             m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% :", string.Format("{0:C}", yuzde7likmiktar))) + "\n");
                                             m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% :", string.Format("{0:C}", yuzde7likmiktar - yuzde7likmiktar / 1.07))) + "\n");
                                             m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% :", string.Format("{0:C}", yuzde7likmiktar / 1.07))) + "\n");

                                             m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                         }
                                     }
                                     mwst = Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]);
                                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N" + "\u001b|iC" + "\u001b|1uC" + "MwST " + mwst + "% \n");
                                     m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N");
                                     if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                     {
                                         yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                     }
                                     else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == 19 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                     {
                                         yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                     }
                                     m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");


                                 }
                                 else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == mwst && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                 {
                                     if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                     {
                                         yuzde7likmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                     }
                                     else if (Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[3]) == 19 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[i].ItemArray[2]) != 43)
                                     {
                                         yuzde19lukmiktar += (double)dtSatisSayisi.Rows[i].ItemArray[0];
                                     }
                                     m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtSatisSayisi.Rows[i].ItemArray[1].ToString(), string.Format("{0:C}", dtSatisSayisi.Rows[i].ItemArray[0]))) + "\n");
                                 }
                             }

                             if (yuzde19lukmiktar != 0)
                             {
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% :", string.Format("{0:C}", yuzde19lukmiktar))) + "\n");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "ERHALTENE MwST "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% :", string.Format("{0:C}", yuzde19lukmiktar - yuzde19lukmiktar / 1.19))) + "\n");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "GESAMT NETTO "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% :", string.Format("{0:C}", yuzde19lukmiktar / 1.19))) + "\n");
                                 m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                             }

                             if (RabatTutar < 0)
                             {
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Rabatt :", string.Format("{0:C}", RabatTutar))) + "\n");
                             }
                             if (RabatCouponTutar < 0)
                             {
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "RabattCoupon :", string.Format("{0:C}", RabatCouponTutar))) + "\n");

                             }
                             m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                             m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + (MakePrintString(m_Printer.RecLineChars, "GESAMT BRUTTO (TOTAL) :", string.Format("{0:C}", yuzde19lukmiktar + yuzde7likmiktar + RabatCouponTutar + RabatTutar))) + "\n");
                             m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                         }
                         /*KASIYER TOPLAMLARI
                         string sqlSatisBilgisi = "SELECT tarih,sum(toplamtutar), kasano,subeno,odemeturu,rabat FROM satisana WHERE tarih>" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)+ ""
                        + " GROUP BY kasano,odemeturu ORDER BY satisana.kasano ASC";
                         MySqlDataAdapter daSatisBilgi = new MySqlDataAdapter(sqlSatisBilgisi, myConn);
                         DataTable dtSatisBilgi = new DataTable("satisana");
                         dtSatisBilgi.Rows.Clear();
                         daSatisBilgi.Fill(dtSatisBilgi);
                         int kaySaySatisbilgi = dtSatisBilgi.Rows.Count;
                         if (kaySaySatisbilgi > 0)
                         {
                             DialogResult printsonuc = printDialog1.ShowDialog();
                             if (printsonuc == DialogResult.OK)
                             {
                                 printDocument1.Print();
                             }
                         }*/
            /* KASIYER TOPLAMLARI SONU*9/

            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
            //m_Printer.CutPaper(100);
            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
        }
    }
    catch (Exception ss)
    {
    }
}
*/
        }
        /*
         * input: bediener Id, typ(0 : basic, 1: detailiert)
         * 
         * 
         */
        public void BestellungDruck()
        {
            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
            if (basilacakFis.isSelbstKioskBestellung == 1)
            {
                using (myConn = baglanti.myconn())
                {

                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|4C" + "\u001b|bC Bestell-Nr #" + basilacakFis.kioskSelbstBestellID + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C Datum:" + DateTime.Now + "\n");
                    //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C" + Program.IsletmeAyarlar["isletme"] + "\n");
                    for (int a = 0; a < basilacakFis.SatisKalem.Count; a++)
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C" + basilacakFis.SatisKalem[a].Adet + "*" + basilacakFis.SatisKalem[a].UrunAd + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|lA" + "\u001b|1C" + "Vielen Dank für Ihre Bestellung!" + "\n");
                }

            }
            //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
            //m_Printer.CutPaper(100);
            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
        }
        public void BedinerBerichtDruck(Int32 BedinerId, int typ, string BedAdSoyad)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;
            double barTutar = 0, ecTutar = 0, stornoTutar = 0, scheckTutar = 0;
            string AuswahlSQL = "", AuswahlSQL2 = "";


            AuswahlSQL = "INNER JOIN satisana ON satisdetay.fisno=satisanaid WHERE satisana.znr=0 ";
            AuswahlSQL2 = " satisana.znr=0 ";

            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {

                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();
                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "BEDIENERNAME: " + BedAdSoyad + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "BEDIENERBERICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " St.-Nr :" + Program.IsletmeAyarlar["usid"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");

                    using (myConn = baglanti.myconn())
                    {
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }

                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Z Berichtsnummer:" + berNo + "-" + Program.kasano + ">" + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /*GRUP BILGISI BASI */
                        int mwst = -1;
                        double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                        double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, ec7tutar = 0, ec19tutar = 0, storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0;
                        double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                        double RabatTutar = 0;
                        double RabatCouponTutar = 0;
                        double Gutschein = 0;
                        /*INNER İLE LEFTi değiştir UNUTMA*/







                        /*GRUP BILGISI SONU 
                        "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                            "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                            "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay, sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay  FROM
                        */
                        string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                              "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                              "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay FROM satisana WHERE" +
                              AuswahlSQL2 + " AND kasano=" + Program.kasano + " AND kasiyerno=" + BedinerId + "  GROUP BY odemeturu ORDER BY odemeturu DESC";
                        MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                        DataTable dtSatisAna = new DataTable("satisana");
                        dtSatisAna.Rows.Clear();
                        daSatisAna.Fill(dtSatisAna);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                        int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                        string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 AND satisdetay.tarih >=" +
                            tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND satisdetay.tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND satisana.kasano=" + Program.kasano + " AND satisdetay.kasiyerno=" + BedinerId + "  GROUP BY satisdetay.mwst";
                        MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn);
                        DataTable dtKombi = new DataTable();
                        daKombi.Fill(dtKombi);
                        string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay, SUM(CASE WHEN `toplamEc` > 0  THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND tarih >=" +
                            tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND tarih <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND satisana.kasano=" + Program.kasano + " AND kasiyerno=" + BedinerId;
                        MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn);
                        DataTable dtKombiSumme = new DataTable();
                        daKombiSumme.Fill(dtKombiSumme);
                        if (dtKombi.Rows.Count > 0)
                        {
                            double KombiTotal = 0;
                            for (int c = 0; c < dtKombi.Rows.Count; c++)
                            {

                                KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                KombiTotal = KombiBar + KombiEC + KombiScheck;

                            }


                        }
                        for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                        {
                            if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) + " X STORNO :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + "\n")));
                                /*m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " STORNO "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19)))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                stornoSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);
                                stornoTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);



                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) + " X BAR :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                /* m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]))))));
                                  m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07)))));
                                  m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                  m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                  m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                barSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                barTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar;
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                /*m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                ecSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                ecTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEC;
                            }
                            else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3 && Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) + " X EC KARTE :", string.Format("{0:C}", (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]))))));
                                /*m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " EC KARTE "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[3]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[1]:7)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) / 1.07))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " BAR "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% BRUTTO :", string.Format("{0:C}", dtSatisAna.Rows[b].ItemArray[5]))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, " Erhaltene "+(Program.MwStList.Count>0?Program.MwStList[2]:19)+"% MwSt :", string.Format("{0:C}", Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) - Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) / 1.19))));
                                 m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");*/
                                scheckSay = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                scheckTutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiScheck;
                            }
                        }

                        if (stornoSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X STORNO :", "0,00 €")));
                        }
                        if (ecSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X EC KARTE :", "0,00 € ")));
                        }
                        if (barSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X BAR :", "0,00 €")));
                        }
                        if (scheckSay == 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "0 X SCHECK :", "0,00 €")));
                        }
                        this.kundenAnzahl = stornoSay + ecSay + barSay;
                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Kassenbarbestand: " + string.Format("{0:C}", (barTutar + stornoTutar)) + "]\n");
                        if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[gesamt Anfangbestand: " + string.Format("{0:C}", AnfangBestandReturnBedienr(BedinerId)) + "]\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "[Soll Bestand: " + string.Format("{0:C}", AnfangBestandReturnBedienr(BedinerId) + barTutar + stornoTutar + Gutschein) + "]\n");

                        }
                        // m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                        // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade(nullBon):" + KasiyerNullBonReturn() + ">" + "\n");
                        // m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                        /* m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars)+"\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "<Kundenanzahl/Bonanzahl:" + this.kundenAnzahl + "- nur Lade: 0>" + "\n");
                         m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));*/
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");

                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                        //m_Printer.CutPaper(100);
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        if (Program.printerType == "bixolon")
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            if (Program.printerType == "bixolon")
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                                // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            }
                        }
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    }
                }
                catch
                {
                }

            }


        }

        private string CizgiCiz(int iLineChars)
        {
            int a = 0;
            string cizgi = "";
            while (a < iLineChars)
            {
                cizgi = cizgi + "-";
                a++;
            }
            return cizgi;
        }
        public String MakePrintString(int iLineChars, String strBuf, String strPrice)
        {
            int iSpaces = 0;
            String tab = "";
            try
            {

                iSpaces = iLineChars - (strBuf.Length + strPrice.Length);
                if (iSpaces >= 0)
                {
                    for (int j = 0; j < iSpaces; j++)
                    {
                        tab += " ";
                    }
                }
                else
                {
                    string[] ad = strBuf.Split('(');

                    strBuf = ad[0].Substring(0, ad[0].Length + iSpaces - 6);
                    if (ad.Length > 1)
                    {
                        strBuf = strBuf + "..(" + ad[1]; //strBuf + ""; //
                    }
                    iSpaces = iLineChars - (strBuf.Length + strPrice.Length);
                    for (int j = 0; j < iSpaces; j++)
                    {
                        tab += " ";
                    }
                }
            }
            catch (Exception)
            {
            }
            return strBuf + tab + strPrice;
        }
        private long GetRecLineChars(ref int[] RecLineChars)
        {
            long lRecLineChars = 0;
            long lCount;
            int i;

            // Calculate the element count.
            lCount = m_Printer.RecLineCharsList.GetLength(0);

            if (lCount == 0)
            {
                lRecLineChars = 0;
            }
            else
            {
                if (lCount > MAX_LINE_WIDTHS)
                {
                    lCount = MAX_LINE_WIDTHS;
                }

                for (i = 0; i < lCount; i++)
                {
                    RecLineChars[i] = m_Printer.RecLineCharsList[i];
                }

                lRecLineChars = lCount;
            }

            return lRecLineChars;
        }
        public void GutscheinDruck()
        {
        }
        public void ReticketDruck()
        {


            if (m_Printer != null && m_Printer.DeviceEnabled == true && Program.reticket.Count > 0)
            {

                try
                {
                    if (Program.zvt == "ReaRetail")
                    {
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        for (int c = 0; c < Program.reticket.Count; c++)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.reticket[c] + "\n");
                        }

                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    Program.reticket.Clear();
                }
                catch
                {
                }

            }
        }
        public void StornoticketDruck()
        {


            if (m_Printer != null && m_Printer.DeviceEnabled == true && Program.stornoticketHandler.Count > 0)
            {

                try
                {
                    if (Program.zvt == "ReaRetail")
                    {
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                        if (Program.stornoticketKunde.Count > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                            for (int c = 0; c < Program.stornoticketKunde.Count; c++)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.stornoticketKunde[c] + "\n");
                            }
                        }
                        if (Program.stornoticketHandler.Count > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                            for (int c = 0; c < Program.stornoticketHandler.Count; c++)
                            {
                                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.stornoticketHandler[c] + "\n");
                            }
                        }

                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    Program.stornoticketKunde.Clear();
                    Program.stornoticketHandler.Clear();
                }
                catch
                {
                }

            }
        }
        public void DiagDruck()
        {


            if (m_Printer != null && m_Printer.DeviceEnabled == true && Program.diagTicket.Count > 0)
            {

                try
                {
                    if (Program.zvt == "ReaRetail")
                    {
                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        for (int c = 0; c < Program.diagTicket.Count; c++)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.diagTicket[c] + "\n");
                        }

                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    Program.diagTicket.Clear();
                }
                catch
                {
                }

            }
        }
        public void KassenschnittDruck(List<string> Ticket, int berNo)
        {


            if (m_Printer != null && m_Printer.DeviceEnabled == true && Ticket.Count > 0)
            {

                try
                {
                    if (Program.zvt == "ReaRetail")
                    {

                        m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                        for (int c = 0; c < Ticket.Count; c++)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Ticket[c] + "\n");
                        }
                        if (Program.ProgramAyarlar["zvt"] == "ReaRetail" && Program.ReaGerateTyp == "INGENICO")
                        {
                            using (myConn = baglanti.myconn())
                            {
                                if (myConn.State == ConnectionState.Closed)
                                {
                                    myConn.Open();
                                }
                                string cardSQ = "SELECT sum(`toplamEc`),count(`cardtypid`), cardname FROM `satisana` LEFT JOIN cardtyp ON cardtyp.cardid=`cardtypid` WHERE znr=" + berNo + " AND odemeturu=0 group by `cardtypid` ";
                                MySqlDataAdapter adapter = new MySqlDataAdapter(cardSQ, myConn);
                                DataTable dtCard = new DataTable();
                                adapter.Fill(dtCard);
                                if (dtCard.Rows.Count > 0)
                                {
                                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                    for (int i = 0; i < dtCard.Rows.Count; i++)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtCard.Rows[i].ItemArray[2].ToString() + "(x" + dtCard.Rows[i].ItemArray[1] + ")", string.Format("{0:C}", dtCard.Rows[i].ItemArray[0]))) + "\n");
                                    }
                                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                                    String detay = "SELECT cardname, toplamEc  FROM `satisana` LEFT JOIN cardtyp ON cardtyp.cardid =`cardtypid` WHERE znr = " + berNo + " AND odemeturu = 0 order by tarih";
                                    MySqlDataAdapter adapterDetay = new MySqlDataAdapter(detay, myConn);
                                    DataTable dtCardDetay = new DataTable();
                                    adapterDetay.Fill(dtCardDetay);
                                    if (dtCardDetay.Rows.Count > 0)
                                    {
                                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                                        for (int b = 0; b < dtCardDetay.Rows.Count; b++)
                                        {
                                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtCardDetay.Rows[b].ItemArray[0].ToString(), string.Format("{0:C}", dtCardDetay.Rows[b].ItemArray[1]))) + "\n");
                                        }
                                        m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));

                                    }
                                }
                            }
                        }

                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    Ticket.Clear();
                }
                catch
                {
                }

            }
        }
        public void TseMeldeDatenDruck(MeldenDaten tSEMeldeDaten)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;

            MySqlConnection myConn1 = new MySqlConnection();
            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());

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
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {


                MySqlTransaction mytrans = null;
                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);

                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer.FontTypefaceList;
                    }
                    catch (Exception hata)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + hata.Message;
                        frmerror.ShowDialog();


                        return;
                    }
                    //m_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer.CapRecBitmap == true)
                    {
                        //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|1B");
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // m_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "MELDEPFLICHT-" + strDate + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    if (Program.IsletmeAyarlar["usid"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " USt-IdNr :" + Program.IsletmeAyarlar["usid"] + "\n"); //St.-Nr.
                    }
                    if (Program.IsletmeAyarlar["steuernummer"] != "")
                    {
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Steuer-Nr :" + Program.IsletmeAyarlar["steuernummer"] + "\n");
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "TSE Anmeldedaten:" + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "1.Art des eAS:" + tSEMeldeDaten.artDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "2.Software des eAS:" + tSEMeldeDaten.softwareDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "3.Software-Version des eAS:" + tSEMeldeDaten.softwareVersionDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "4.Seriennumer des eAS/Software App:" + tSEMeldeDaten.seriennummerDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "5.Hersteller des eAS:" + tSEMeldeDaten.herstellerDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "6.Model des eAS:" + tSEMeldeDaten.modelDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "7.Anschaffungsdatum des eAS:" + tSEMeldeDaten.anschaffungDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "8.Inbetriebnahme des eAS:" + tSEMeldeDaten.inbetriebnahmeDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "9. Ausserbetriebnahme des eAS:" + tSEMeldeDaten.ausserBetriebnahmeDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "10.Grund der Ausserbetrieb. des eAS:" + tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "11.Bemerkungen zum eAS:" + tSEMeldeDaten.bemerkungZumEas.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "12.Seriennumer der TSE:" + tSEMeldeDaten.seriennummerDerTSE.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "13.BSI Zertifizierungs-ID:" + tSEMeldeDaten.BSIZertifierungsID.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "14.Inbetriebnahme/Aktivierung der TSE:" + tSEMeldeDaten.inbetriebnahmeDerTSE.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "15.Art/Bauform der TSE:" + tSEMeldeDaten.artBauFormDerTSE.ToString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars) + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                }
                catch (Exception dd)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "" + dd.Message;
                    frmerror.ShowDialog();
                    return;
                }
            }
        }
        public void SiparisYaz(SatisYap basilacakItem)
        {
            if (Program.LanPrinter != null && Program.LanPrinter.DeviceEnabled == true)
            {
                PosPrinter L_Printer = Program.LanPrinter;

                try
                {

                    L_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);


                    //L_Printer.PrintNormal(PrinterStation.Receipt, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    User User = new IS_KASSE.User();


                    //<<<step3>>>--End
                    // L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
                    // L_Printer.PrintBitmap(PrinterStation.Receipt, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                    L_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(L_Printer.RecLineChars));
                    L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tarih/Saat:" + DateTime.Now + " \nKasiyer:" + User.UserBul(basilacakItem.KasiyerId) + " \n");
                    L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + basilacakItem.Adet + " X " + basilacakItem.UrunAd + "\n");


                    L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                    // L_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //L_Printer.CutPaper(100);
                    L_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                }
                catch
                {
                }

            }
        }
        public void KassenZahlerDruck(Dictionary<string, int> ParaAdetleri)
        {
            double gesamtTotal = 0;
            if (m_Printer != null && m_Printer.DeviceEnabled == true && ParaAdetleri.Count > 0)
            {
                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Kassenzählerprotokoll-" + DateTime.Now.ToShortDateString() + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["inhaber"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " St.-Nr :" + Program.IsletmeAyarlar["usid"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + " Kasse-Nr :" + Program.kasano + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "Bediener:" + Program.bedAdSoyad + "\n");
                    var list = ParaAdetleri.Keys.ToList();
                    list.Sort();

                    // Loop through keys.
                    /*foreach (var key in list)
                    {
                        Console.WriteLine("{0}: {1}", key, dictionary[key]); //KeyValuePair<string, int> item in ParaAdetleri
                    }*/
                    foreach (var key in list)
                    {

                        m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, key + "X" + ParaAdetleri[key], string.Format("{0:C}", Convert.ToDouble(key) * Convert.ToDouble(ParaAdetleri[key]))) + "\n"));
                        gesamtTotal += Convert.ToDouble(key) * Convert.ToDouble(ParaAdetleri[key]);
                    }
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C" + MakePrintString(m_Printer.RecLineChars, "Gesamt:", gesamtTotal.ToString("C") + "\n"));



                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                    //m_Printer.CutPaper(100);
                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    ParaAdetleri.Clear();
                }
                catch
                {
                }

            }
        }
        public void TagesjournalDruck()
        {
            double toptutar = 0;
            if (m_Printer != null && m_Printer.DeviceEnabled == true)
            {

                try
                {

                    m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|bC" + "Tagesjournal -" + DateTime.Now + " \n");
                    m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");


                    using (myConn = baglanti.myconn())
                    {
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }
                        //0,       1,      2,               3,                   4,                   5                   6                               7                               8
                        string istSQL = "SELECT urunid,artikelad,artikel.barkod, artikel.grupid, artikel.alisfiyat, artikel.`satisfiyat`, sum(`adet`) as satilanadet, sum(`toplamtutar`) as satistutari, sum(`birimkar`) as toplamkar, artikel.toplamstok" +
                               " FROM `satisdetay` INNER JOIN artikel on artikel.artikelid = satisdetay.urunid WHERE tarih>=" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year).ToString() + " AND tarih<=" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year).ToString() + " AND toplamtutar>0 AND stornoilgi=0 GROUP BY `urunid`   ORDER BY `satilanadet` DESC";
                        MySqlDataAdapter myDaGrup = new MySqlDataAdapter(istSQL, myConn);
                        DataTable dtGrup1 = new DataTable("");
                        dtGrup1.Clear();
                        myDaGrup.Fill(dtGrup1);
                        int kaySay = dtGrup1.Rows.Count;
                        if (kaySay > 0)
                        {
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Menge x Artikelname (VK-Preis)", " Summe  [Lager]")) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                            for (int i = 0; i < dtGrup1.Rows.Count; i++)
                            {
                                //                                                                      Ad              Satilan adet                satisfiyati       tutar
                                //m_Printer.PrintNormal(PrinterStation.Receipt, (getMwstInfo(dtGrup1.Rows[i].ItemArray[1].ToString(), dtGrup1.Rows[i].ItemArray[6].ToString(), string.Format("{0:C}", dtGrup1.Rows[i].ItemArray[5]), string.Format("{0:C}", dtGrup1.Rows[i].ItemArray[7]))) + "\n");
                                m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, dtGrup1.Rows[i].ItemArray[6].ToString() + "x" + dtGrup1.Rows[i].ItemArray[1].ToString() + "(" + string.Format("{0:C}", dtGrup1.Rows[i].ItemArray[5]) + ")", string.Format("{0:C}", dtGrup1.Rows[i].ItemArray[7]) + "[" + dtGrup1.Rows[i].ItemArray[9] + "]")) + "\n");

                                /* dgvGrup.Rows[i].Cells[0].Value = dtGrup1.Rows[i].ItemArray[1];//ad
                                 dgvGrup.Rows[i].Tag = dtGrup1.Rows[i].ItemArray[0];
                                 dgvGrup.Rows[i].Cells[1].Value = dtGrup1.Rows[i].ItemArray[2];//barkod
                                 dgvGrup.Rows[i].Cells[2].Value = new ArtikelGrup((int)dtGrup1.Rows[i].ItemArray[3]).Grupad;//grup
                                 dgvGrup.Rows[i].Cells[3].Value = dtGrup1.Rows[i].ItemArray[4];//alisfiyat
                                 dgvGrup.Rows[i].Cells[4].Value = dtGrup1.Rows[i].ItemArray[5];//satisfiyat
                                 //dgvGrup.Rows[i].Cells[5].Value = dtGrup1.Rows[i].ItemArray[9];// new Birimler((int)dtGrup.Rows[i].ItemArray[9]).Birimad;
                                 dgvGrup.Rows[i].Cells[5].Value = dtGrup1.Rows[i].ItemArray[6];//tadet
                                 dgvGrup.Rows[i].Cells[6].Value = dtGrup1.Rows[i].ItemArray[8];//ttutar
                                 dgvGrup.Rows[i].Cells[7].Value = dtGrup1.Rows[i].ItemArray[7];//tkar
                                 */
                                toptutar += Convert.ToDouble(dtGrup1.Rows[i].ItemArray[7]);
                            }
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, (MakePrintString(m_Printer.RecLineChars, "Gesamt Summe :", string.Format("{0:C}", toptutar))) + "\n");
                            m_Printer.PrintNormal(PrinterStation.Receipt, CizgiCiz(m_Printer.RecLineChars));
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|50uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|150uF");
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");
                            //m_Printer.CutPaper(100);
                            m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                        }
                    }
                }
                catch
                {
                }
            }


        }
    }
}
