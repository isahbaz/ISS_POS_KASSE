using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;
using POS.Devices;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Threading;

namespace IS_KASSE
{
    public class FisBarkodlu2
    {
        public OPOSPOSPrinter m_Printer = Program.printer2;
        public OPOSPOSPrinter m_Printer2 = null;
        public FisOlustur basilacakFis = null;
        public int OdemeTur = -1;
        Tarih tarih = new Tarih();
        Ean13 barcode = new Ean13();
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        VirgulAyikla vA = new VirgulAyikla();
        const int MAX_LINE_WIDTHS = 2;
        bool bSetBitmapSuccess = Program.bSetBitmapSuccess;
        string strCurDir = "";

        public FisBarkodlu2()
        {
	  m_Printer = Program.printer2;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            
            strCurDir = Directory.GetCurrentDirectory();

            // strFilePath = strCurDir.Substring(2, strCurDir.LastIndexOf("Step5") + "Step5\\".Length);

            string strFilePath = strCurDir + "\\Logo.bmp";
            if (File.Exists(strFilePath))
            {

            }
            else
            {
                //default resim kullan
                strFilePath = "defaultlogo.bmp";
            }
            if (m_Printer2 != null)
            {
                if (Program.GlobalAyarlar["LOGO"] == 1)
                {
                    m_Printer2.RecLetterQuality = true;

                    if (m_Printer2.CapRecBitmap == true)
                    {

                        if (Program.bSetBitmapSuccess == false)
                        {
                            for (int iRetryCount = 0; iRetryCount < 5; iRetryCount++)
                            {
                                try
                                {
                                    //<<<step5>>>--Start
                                    //Register a bitmap
                                    m_Printer2.SetBitmap(0, 1, strFilePath, m_Printer2.RecLineWidth / 2, (int)OPOSPOSPrinterConstants.PTR_BM_CENTER);
                                    //<<<step5>>>--End
                                    bSetBitmapSuccess = true;
                                    m_Printer2.SetBitmap(1, 1,
                                        "Makas.bmp", m_Printer2.RecLineWidth / 2,
                                        (int)OPOSPOSPrinterConstants.PTR_BM_CENTER);
                                    Program.bSetBitmapSuccess = true;
                                    break;
                                }
                                catch (PosControlException pce)
                                {
                                    if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0 && pce.Message == "It is not initialized.")
                                    {
                                        System.Threading.Thread.Sleep(2000);
                                    }
                                }
                            }
                        }
                        if (!bSetBitmapSuccess)
                        {
                            MessageBox.Show("Logo Bild Fehler!", "Printer_IS_KASSE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                //<<<step3>>>--End

                //<<<step5>>>--Start
                // Even if using any printers, 0.01mm unit makes it possible to print neatly.
                m_Printer2.MapMode = (int)OPOSPOSPrinterConstants.PTR_MM_METRIC;
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = "PRINTER ILE ILGILI SORUN VAR!FIS BASILAMAZ!";
                frmerror.ShowDialog();
                return;
            }

        }
        public void FisYaz()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            CultureInfo culture = new CultureInfo("de-DE");
            DateTime nowDate = DateTime.Now;							//System date
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();	//Date Format
            dateFormat.MonthDayPattern = "MMMM";
            string strDate = nowDate.ToString("dd.MMMM.yyyy  HH:mm", culture);
            int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            long lRecLineCharsCount;


            //MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (m_Printer2 != null)
            {
                m_Printer2.PrintNormal(2, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                m_Printer2.TransactionPrint(2, (int)OPOSPOSPrinterConstants.PTR_TP_TRANSACTION);
                try
                {
                    ArrayList items = new ArrayList();
                    try
                    {
                        string[] fontlar = m_Printer2.FontTypefaceList.Split(',');
                        string ff = fontlar[0];
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("Printer Error!\n Orginal Message:" + hata.Message);
                        return;
                    }
                    //m_Printer2.PrintNormal(2, 
                    //printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                    //<<<step3>>>--Start

                    if (m_Printer2.CapRecBitmap == true)
                    {
                        m_Printer2.PrintNormal(2, "\x1b|0B");
                        m_Printer2.PrintNormal(2, "\x1b|1B");
                        m_Printer2.PrintNormal(2, "\x1b|100uF");
                    }



                    //<<<step3>>>--End
                    // m_Printer2.PrintNormal(2, "\x1b|1B");
                    // m_Printer2.PrintBitmap(2, strFilePath, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);

                    m_Printer2.PrintNormal(2, "\x1b|cA" + "\x1b|bC" + Program.IsletmeAyarlar["isletme"] + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|cA" + Program.IsletmeAyarlar["strase"] + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|cA" + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|cA" + "Tel :" + Program.IsletmeAyarlar["tel1"] + " Fax:" + Program.IsletmeAyarlar["fax"] + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|cA" + " UsId :" + Program.IsletmeAyarlar["usid"] + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|cA" + " <BED." + Program.bedID + "> " + "<BON:" + this.basilacakFis.SatisAnaId + ">" + "\n");
                    
                    //<<<step5>>--Start
                    //Make 2mm speces
                    //ESC|#uF = Line Feed
                    //9999m_Printer2.PrintNormal(2, "\x1b|100uF");
                    //<<<step5>>>-End

                    lRecLineCharsCount = GetRecLineChars(ref RecLineChars);
                    if (lRecLineCharsCount >= 2)
                    {
                        m_Printer2.RecLineChars = RecLineChars[1];
                        m_Printer2.PrintNormal(2, "\x1b|cA" + strDate + "\n");
                        m_Printer2.RecLineChars = RecLineChars[0];
                    }
                    else
                    {
                        m_Printer2.PrintNormal(2, "\x1b|cA" + strDate + "\n");
                    }
                    m_Printer2.RecLineChars = RecLineChars[0];
                    //
                    string cizgi = MakePrintString(m_Printer2.RecLineChars, "", "");
                    m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");
                    m_Printer2.PrintNormal(2, "\x1b|50uF");
                    //m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");

                    //<<<step5>>>--Start
                    //Make 5mm speces
                    //999 m_Printer2.PrintNormal(2, "\x1b|200uF");

                    string strPrintData = "";
                    //Print buying goods
                    /* double total = 0.0;
                
                     for (int i = 0; i < astritem.Length; i++)
                     {
                         strPrintData = MakePrintString(m_Printer2.RecLineChars, astritem[i], "$"
                             + astrprice[i]);

                         m_Printer2.PrintNormal(2, strPrintData + "\n");

                         total += Convert.ToDouble(astrprice[i]);

                     }*/
                    foreach (SatisYap satiskalem in basilacakFis.SatisKalem)
                    {
                        // PrintLineItem(printer, satiskalem.UrunAd, satiskalem.Adet, satiskalem.Satisfiyat, satiskalem.Toplamtutar);
                        if (new ArtikelGrup(satiskalem.Grubid).GrupTur == 3)
                        {
                            m_Printer2.PrintNormal(2, (MakePrintString(m_Printer2.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString("#0.000") + "Kg" + "*" + satiskalem.Satisfiyat.ToString("c") + "/Kg}",
                               satiskalem.Toplamtutar.ToString("c") + "-A")));
                        }
                        else if (satiskalem.Grubid == 43) //Rabatt Coupon veya Rabatt
                        {
                            m_Printer2.PrintNormal(2, (MakePrintString(m_Printer2.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString() + "*" + satiskalem.Satisfiyat.ToString("c") + "}",
                              satiskalem.Toplamtutar.ToString("c") + "  ")));
                        }

                        else
                        {
                            if (satiskalem.Grubid != 7)
                            {
                                if (satiskalem.Mwst == 19)
                                {
                                    m_Printer2.PrintNormal(2, (MakePrintString(m_Printer2.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/ST}",
                                      satiskalem.Toplamtutar.ToString("c") + "-B")));
                                }
                                else
                                {
                                    m_Printer2.PrintNormal(2, (MakePrintString(m_Printer2.RecLineChars, satiskalem.UrunAd + "{" + satiskalem.Adet.ToString() + "St *" + satiskalem.Satisfiyat.ToString("c") + "/ST}",
                                      satiskalem.Toplamtutar.ToString("c") + "-A")));
                                }
                            }

                        }
                        //m_Printer2.PrintNormal(2, strPrintData + "\n");

                    }
                    m_Printer2.PrintNormal(2, "\x1b|50uF");
                    //items.Clear();
                    m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");
                    m_Printer2.RecLineChars = RecLineChars[1];

                    m_Printer2.PrintNormal(2, "\x1b|cA" + "\x1b|iC" + "Typ          Netto              Mwst             Brutto" + "\n");
                    if (basilacakFis.mwst19Uygulanantutar > 0)
                    {
                        m_Printer2.PrintNormal(2, getMwstInfo("B:19%", (this.basilacakFis.mwst19Uygulanantutar / 1.19).ToString("c"), this.basilacakFis.mwst19miktar.ToString("c"), this.basilacakFis.mwst19Uygulanantutar.ToString("c")) + "\n");
                    }
                    if (basilacakFis.mwst7Uygulanantutar > 0)
                    {
                        m_Printer2.PrintNormal(2, getMwstInfo("A: 7%", (this.basilacakFis.mwst7Uygulanantutar / 1.07).ToString("c"), this.basilacakFis.mwst7miktar.ToString("c"), this.basilacakFis.mwst7Uygulanantutar.ToString("c")) + "\n");
                    }
                    m_Printer2.RecLineChars = RecLineChars[0];
                    m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");
                    if (OdemeTur == 0) //ec Karte
                    {
                        strPrintData = MakePrintString(m_Printer2.RecLineChars / 2, "ZU BEZAHLEN:", basilacakFis.toplamtutar.ToString("c"));
                        m_Printer2.PrintNormal(2, "\x1b|bC" + "\x1b|2C" + strPrintData + "\n");
                        if (basilacakFis.Rabattutar != 0 || basilacakFis.PuanRabat != 0)
                        {
                            /*eski
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABAT", "-" + this.basilacakFis.Rabattutar.ToString("C"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "ZWS", (this.basilacakFis.toplamtutar - this.basilacakFis.Rabattutar).ToString("C"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");*/
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "TOTAL", (this.basilacakFis.Rabattutar + this.basilacakFis.toplamtutar + this.basilacakFis.PuanRabat).ToString("c"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");
                            if (basilacakFis.PuanRabat > 0)
                            {

                                strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABATT(" + this.basilacakFis.HarcananPuan + " Pnkt.)", "-" + this.basilacakFis.PuanRabat.ToString("c"));
                                m_Printer2.PrintNormal(2, strPrintData + "\n");
                            }
                            if (basilacakFis.Rabattutar > 0)
                            {
                                if (Program.GlobalAyarlar["RABAT"] == 1)
                                {
                                    strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABATT " + this.basilacakFis.Rabat.ToString() + "%", "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                    m_Printer2.PrintNormal(2, strPrintData + "\n");
                                }
                                else
                                {
                                    strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABAT " + this.basilacakFis.Rabat.ToString("C"), "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                    m_Printer2.PrintNormal(2, strPrintData + "\n");
                                }
                            }
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "ZWS", (this.basilacakFis.toplamtutar).ToString("c"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");
                        }
                        strPrintData = MakePrintString(m_Printer2.RecLineChars, "EC-KARTE :", (this.basilacakFis.toplamtutar).ToString("c"));
                        m_Printer2.PrintNormal(2, strPrintData + "\n");
                    }
                    else if (OdemeTur == 1) //BAR
                    {
                        strPrintData = MakePrintString(m_Printer2.RecLineChars / 2, "ZU BEZAHLEN :", basilacakFis.toplamtutar.ToString("c"));
                        m_Printer2.PrintNormal(2, "\x1b|bC" + "\x1b|2C" + strPrintData + "\n");
                        if (basilacakFis.Rabattutar != 0 || basilacakFis.PuanRabat != 0)
                        {
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "TOTAL", (this.basilacakFis.Rabattutar + this.basilacakFis.toplamtutar + basilacakFis.PuanRabat).ToString("c"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");

                            if (basilacakFis.PuanRabat > 0)
                            {

                                strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABATT(" + this.basilacakFis.HarcananPuan + " Pnkt.)", "-" + this.basilacakFis.PuanRabat.ToString("c"));
                                m_Printer2.PrintNormal(2, strPrintData + "\n");
                            }
                            if (basilacakFis.Rabattutar > 0)
                            {
                                if (Program.GlobalAyarlar["RABAT"] == 1)
                                {
                                    strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABATT " + this.basilacakFis.Rabat.ToString() + "%", "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                    m_Printer2.PrintNormal(2, strPrintData + "\n");
                                }
                                else
                                {
                                    strPrintData = MakePrintString(m_Printer2.RecLineChars, "RABAT " + this.basilacakFis.Rabat.ToString("C"), "-" + this.basilacakFis.Rabattutar.ToString("c"));
                                    m_Printer2.PrintNormal(2, strPrintData + "\n");
                                }
                            }
                            strPrintData = MakePrintString(m_Printer2.RecLineChars, "ZWS", (this.basilacakFis.toplamtutar).ToString("c"));
                            m_Printer2.PrintNormal(2, strPrintData + "\n");
                        }
                        strPrintData = MakePrintString(m_Printer2.RecLineChars, "BAR :", basilacakFis.verilenpara.ToString("c"));
                        m_Printer2.PrintNormal(2, strPrintData + "\n");
                        strPrintData = MakePrintString(m_Printer2.RecLineChars, "RÜCKGELD :", basilacakFis.paraustu.ToString("c"));
                        m_Printer2.PrintNormal(2, strPrintData + "\n");
                    }
                    else if (OdemeTur == 2) //STORNO
                    {
                        strPrintData = MakePrintString(m_Printer2.RecLineChars / 2, "TOTAL   :", "-" + basilacakFis.toplamtutar.ToString("c"));
                        m_Printer2.PrintNormal(2, "\x1b|bC" + "\x1b|2C" + strPrintData + "\n");
                        strPrintData = MakePrintString(m_Printer2.RecLineChars, "BAR :", "-" + basilacakFis.toplamtutar.ToString("c"));
                        m_Printer2.PrintNormal(2, strPrintData + "\n");
                        m_Printer2.PrintNormal(2, "\x1b|cA" + "< STORNO >< STORNO >< STORNO >" + "\n");
                    }


                    if (Program.GlobalAyarlar["BARKOD"] == 1)
                    {
                        if ((Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "0" || (Program.IsletmeAyarlar["indirimbarkoduhedefi"] == "1" && this.basilacakFis.Musterino != 0)) && basilacakFis.AngebotsuzToplamTutar >= Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkodualtsinir"])) //Tüm müşterilere ind barkodu ver
                        {
                            double ind = Math.Round(basilacakFis.AngebotsuzToplamTutar * Convert.ToDouble(Program.IsletmeAyarlar["indirimbarkoduorani"]) / 100, 2);
                            string indOran = ind.ToString("C");
                            // string strFilePath2 = strCurDir + "\\Makas.bmp";
                            m_Printer2.PrintNormal(2, "\x1b|300uF");
                            m_Printer2.PrintNormal(2, "\x1b|2B");
                            // m_Printer2.PrintBitmap(2, strFilePath2, PosPrinter.PrinterBitmapAsIs, PosPrinter.PrinterBitmapCenter);
                            m_Printer2.PrintNormal(2, "\x1b|200uF");
                            m_Printer2.PrintNormal(2, "\x1b|cA" + "\x1b|rvC" + "Unser Dankeschön für Ihren Einkauf!" + "\n");
                            m_Printer2.PrintNormal(2, "\x1b|cA" + "\x1b|4C" + "\x1b|bC" + indOran + " RABATT\n");
                            m_Printer2.PrintNormal(2, "\x1b|cA" + strDate + "\n");
                            //<<<step4>>>--Start
                            string brkd = "";
                            if (m_Printer2.CapRecBarCode == true)
                            {
                                brkd = getBarkod();
                                //Barcode printing
                                m_Printer2.PrintBarCode(2, brkd,(int)OPOSPOSPrinterConstants.PTR_BCS_EAN13, 1000,
                                    m_Printer2.RecLineWidth * 2 / 3,(int)OPOSPOSPrinterConstants.PTR_BC_CENTER,(int)OPOSPOSPrinterConstants.PTR_BC_TEXT_BELOW);
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
                                    MessageBox.Show("Indirim Oranı Sisteme İşlenemedi!");
                                }


                            }
                            m_Printer2.PrintNormal(2, "\x1b|200uF");
                            m_Printer2.PrintNormal(2, "\x1b|cA" + "\x1b|rvC" + "Erhalten Sie bei Ihrem nächsten Einkauf!" + "\n");
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
                                m_Printer2.PrintNormal(2, "\x1b|200uF");
                                m_Printer2.RecLineChars = RecLineChars[1];
                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Punkte INFO: KN:" + Convert.ToInt64(this.basilacakFis.Musteri.Barkod) + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");
                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Alte Zustand:" + this.basilacakFis.EskiPuanToplamı.ToString() + "Pnkt.\n");
                                if (this.basilacakFis.HarcananPuan > 0)
                                {
                                    m_Printer2.PrintNormal(2, "\x1b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                }
                                if (this.basilacakFis.KazanilanPuan > 0)
                                {
                                    m_Printer2.PrintNormal(2, "\x1b|iC" + "Gewinn:" + this.basilacakFis.KazanilanPuan.ToString() + "Pnkt.\n");
                                }

                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Neue Zustand:" + (this.basilacakFis.EskiPuanToplamı + this.basilacakFis.KazanilanPuan - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                            }
                            else // Indrim Barkodu verildiğinde auynı zamanda puan da vermez ama puanlar yinede hesaplanmıştır. Hesaplanan bu puanların fiş de yeralmaması sadece eski puanın yer alması 
                            {
                                m_Printer2.PrintNormal(2, "\x1b|200uF");
                                m_Printer2.RecLineChars = RecLineChars[1];
                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Punkte INFO: KN:" + this.basilacakFis.Musterino + "," + this.basilacakFis.Musteri.AdSoyad + "\n");
                                m_Printer2.PrintNormal(2, "\x1b|uC" + cizgi + "\n");
                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Alte Zustand:" + this.basilacakFis.EskiPuanToplamı.ToString() + "Pnkt.\n");
                                if (this.basilacakFis.HarcananPuan > 0)
                                {
                                    m_Printer2.PrintNormal(2, "\x1b|iC" + "Ausgeben:" + this.basilacakFis.HarcananPuan.ToString() + "Pnkt.\n");
                                }
                                if (this.basilacakFis.KazanilanPuan > 0)
                                {
                                    m_Printer2.PrintNormal(2, "\x1b|iC" + "Gewinn:0 Pnkt.\n");
                                }

                                m_Printer2.PrintNormal(2, "\x1b|iC" + "Neue Zustand:" + (this.basilacakFis.EskiPuanToplamı - this.basilacakFis.HarcananPuan).ToString() + "Pnkt.\n");
                            }
                        }
                    }
                    m_Printer2.RecLineChars = RecLineChars[0];
                    if (Program.BonText.Count > 0 && Program.BonText[0] != "")
                    {
                        foreach (string text in Program.BonText)
                        {
                            m_Printer2.PrintNormal(2, "\x1b|cA" + text + "\n");
                        }

                    }
                    else
                    {
                        m_Printer2.PrintNormal(2, "\x1b|cA" + "Ziyaretiniz Icin Tesekkur Ederiz!" + "\n");
                        m_Printer2.PrintNormal(2, "\x1b|cA" + "Vielen Dank fuer Ihren Besuch!" + "\n");

                    }
                    m_Printer2.PrintNormal(2, "\x1b|fP");
                    //m_Printer2.CutPaper(200);
                    m_Printer2.TransactionPrint(2, (int)OPOSPOSPrinterConstants.PTR_TP_NORMAL);

                    //<<<step2>>>--End
                }
                catch (PosControlException)
                {
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

            /*EPSON T20
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
            */
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
        private string getBarkod()
        {
            barcode.CountryCode = (Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + 1).ToString();
            barcode.ManufacturerCode = "00000";
            barcode.ProductCode = getMaxID();
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
                    string[] ad = strBuf.Split('{');

                    strBuf = ad[0].Substring(2, ad[0].Length + iSpaces - 4);
                    strBuf = strBuf + "..{" + ad[1];
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
            lCount = m_Printer2.RecLineCharsList.Length;

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
                    RecLineChars[i] = m_Printer2.RecLineCharsList[i];
                }

                lRecLineChars = lCount;
            }

            return lRecLineChars;
        }

    }
}
