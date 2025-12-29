using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Devices;

using Microsoft.PointOfService;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using iss_RechnungsCreate;


namespace IS_KASSE
{

    public class Fis
    {
        private PosPrinter printer = Program.printer;
        // public OPOSPOSPrinter printer = null;
        public FaturaOlustur basilacakFatura = null;
        public FisOlustur basilacakFis = null;
        public int OdemeTur = -1;
        Tarih tarih = new Tarih();
        //CashDrawer m_Drawer = null;
        const int MAX_LINE_WIDTHS = 2;
        int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
        long lRecLineCharsCount;

        private void PrintReceipt()
        {
            if (printer.State == ControlState.Closed)
            {
                //printer = GetReceiptPrinter();

                ConnectToPrinter(printer);
            }

            try
            {
                printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                ;                //CHR$(27) & CHR$(112) & CHR$(48) & CHR$(40) & CHR$(40)
                //PrintTextLine(printer, "" + ((char)27) + ((char)105));
                // m_Drawer.OpenDrawer();
                //PrintTextLine(printer, "" + ((char)91) + ((char)91));27,112,0,25,250
                /*cozum 1
                PrintTextLine(printer, "" + ((char)27) + ((char)105));// 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
                PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                */
                //PrintTextLine(printer, "" + ((char)27) + ((char)105));// 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
                //PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                // PrintXRapHeader(printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"], online, offline);
                PrintReceiptHeader(printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"]);
                PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                foreach (SatisYap satiskalem in basilacakFis.SatisKalem)
                {
                    PrintLineItem(printer, satiskalem.UrunAd, satiskalem.Adet, satiskalem.Satisfiyat, satiskalem.Toplamtutar);
                    // PrintLineItem(printer, "Item 2", 101, 0.00);
                    //PrintLineItem(printer, "Item 3", 9, 0.1);
                    //PrintLineItem(printer, "Item 4", 1000, 1);

                }

                PrintReceiptFooter(printer, 1, 0.1, 0.1, "VIELEN DANK!");
                printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");

            }
            catch
            {

            }
        }

        private void DisconnectFromPrinter(PosPrinter printer)
        {
            printer.Release();
            printer.Close();
        }

        private void ConnectToPrinter(PosPrinter printer)
        {
            try
            {
                printer.Open();
                printer.Claim(5000);
                int i = 0;
                while (printer.DeviceEnabled != true)
                {
                    i++;
                    //Thread.Sleep(5000);
                    printer.Claim(500);
                    if (printer.Claimed == true)
                    {

                        try
                        {
                            printer.DeviceEnabled = true;
                        }
                        catch { }

                        //Thread.Sleep(5000);

                    }
                    else
                    {
                        printer.DeviceEnabled = false;
                    }
                    if (i > 19)
                        break;

                }
                if (printer.DeviceEnabled == false)
                {
                    F_GenericError frmerr = new F_GenericError();

                    frmerr.BringToFront();
                    frmerr.Height = 400;
                    frmerr.Width = 600;
                    frmerr.lblMesaj.Text = Program.lang["3"] + "[4]" + "DEVICE ENABLED = FALSE " + printer.DeviceName;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
                    frmerr.ShowDialog();
                    printer.DeviceEnabled = true;
                }
                else
                {
                    /*F_GenericError frmerr = new F_GenericError();

                    frmerr.BringToFront();
                    frmerr.Height = 400;
                    frmerr.Width = 600;
                    frmerr.lblMesaj.Text = "DEVICE ENABLED = TRUE" + printer.DeviceName;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
                    frmerr.ShowDialog();
                    printer.DeviceEnabled = true;*/
                }

            }
            catch (Exception eex)
            {
                F_GenericError frmerr = new F_GenericError();

                frmerr.BringToFront();
                frmerr.Height = 400;
                frmerr.Width = 600;
                frmerr.lblMesaj.Text = Program.lang["3"] + "[2]" + eex.Message;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
                frmerr.ShowDialog();
                //return;
                try
                {
                    printer.DeviceEnabled = true;
                }
                catch
                {

                }
                finally
                {
                    F_GenericError frmerr1 = new F_GenericError();

                    frmerr1.BringToFront();
                    frmerr1.Height = 400;
                    frmerr1.Width = 600;
                    frmerr1.lblMesaj.Text = "DEVICE ENABLED = " + printer.DeviceEnabled + "<-> " + printer.DeviceName;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
                    frmerr1.ShowDialog();
                    Program.printer = printer;
                }
                /* if (printer.DeviceEnabled == false)
                 {
               F_GenericError frmerr1 = new F_GenericError();

               frmerr1.BringToFront();
               frmerr1.Height = 400;
               frmerr1.Width = 600;
               frmerr1.lblMesaj.Text = Program.lang["3"] + "[5]" + "DEVICE ENABLED = FALSE" + printer.DeviceName;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
               frmerr1.ShowDialog();
               try
               {
                   printer.DeviceEnabled = true;
               }
               catch
               {
               }
               finally
               {
                   Application.Restart();
               }
                 }
                 else
                 {
               /*F_GenericError frmerr1 = new F_GenericError();

               frmerr1.BringToFront();
               frmerr1.Height = 400;
               frmerr1.Width = 600;
               frmerr1.lblMesaj.Text = "DEVICE ENABLED = TRUE[6]" + printer.DeviceName;//PRINTER ILE ILGILI SORUN VAR!\nHATA MESAJI:"
               frmerr1.ShowDialog();
               printer.DeviceEnabled = true;*9/
                 }*/

            }
            finally
            {

            }


        }

        private void GetReceiptPrinter()
        {
            /*PosExplorer posExplorer = new PosExplorer(this);
            DeviceInfo receiptPrinterDevice = posExplorer.GetDevice("PosPrinter", "ReceiptPrinter"); //May need to change this if you don't use a logicial name or use a different one.
            return (PosPrinter)posExplorer.CreateInstance(receiptPrinterDevice);
            */
            string myMsrName = "PosPrinter";
            //PosPrinter msr = null;
            PosExplorer explorer = new PosExplorer();
            DeviceInfo deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
            if (deviceInfo == null)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo=null";
                frmerr.ShowDialog();
                return;
            }
            else
            {
                printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                /* F_GenericError frmerr = new F_GenericError();
                 frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                 frmerr.ShowDialog();*/

                //return msr;
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
        private void PrintReceiptFooter(PosPrinter printer, int subTotal, double tax, double discount, string footerText)
        {

            string offSetString = new string(' ', printer.RecLineChars / 2);


            lRecLineCharsCount = GetRecLineChars(ref RecLineChars);

            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            /* printer.RecLineChars = RecLineChars[1];
             printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + "\u001b|iC" + "Typ           Netto            Mwst          Brutto" + "\n");
             if (basilacakFis.mwst19Uygulanantutar > 0)
             {
                 printer.PrintNormal(PrinterStation.Receipt, getMwstInfo("B:19%", (this.basilacakFis.mwst19Uygulanantutar - this.basilacakFis.mwst19miktar).ToString("C"), this.basilacakFis.mwst19miktar.ToString("C"), this.basilacakFis.mwst19Uygulanantutar.ToString("C")) + "\n");
             }
             if (basilacakFis.mwst7Uygulanantutar > 0)
             {
                 printer.PrintNormal(PrinterStation.Receipt, getMwstInfo("A: 7%", (this.basilacakFis.mwst7Uygulanantutar - this.basilacakFis.mwst7miktar).ToString("C"), this.basilacakFis.mwst7miktar.ToString("C"), this.basilacakFis.mwst7Uygulanantutar.ToString("C")) + "\n");
             }
             PrintTextLine(printer, new string('-', (printer.RecLineChars)));
             printer.RecLineChars = RecLineChars[0];*/
            if (this.OdemeTur == 1)
            {
                PrintTextLine(printer, offSetString + String.Format("TOTAL           {0}", this.basilacakFis.toplamtutar.ToString("C")));
                if (basilacakFis.Rabat != 0)
                {

                    PrintTextLine(printer, offSetString + String.Format("RABAT          -{0}", this.basilacakFis.Rabattutar.ToString("C")));
                    PrintTextLine(printer, offSetString + String.Format("ZWS             {0}", (this.basilacakFis.toplamtutar - this.basilacakFis.Rabattutar).ToString("C")));
                }
                PrintTextLine(printer, offSetString + String.Format("BAR             {0}", this.basilacakFis.verilenpara.ToString("C")));
                PrintTextLine(printer, offSetString + String.Format("RÜCKGELD        {0}", this.basilacakFis.paraustu.ToString("C").PadRight(1)));
                //PrintTextLine(printer, offSetString + new string('-', (printer.RecLineChars / 3)));
                //PrintTextLine(printer, offSetString + String.Format("TOTAL         {0}", (subTotal - (tax + discount)).ToString("#0.00")));
            }
            else if (this.OdemeTur == 0)
            {
                PrintTextLine(printer, offSetString + String.Format("TOTAL           {0}", this.basilacakFis.toplamtutar.ToString("C")));
                if (basilacakFis.Rabat != 0)
                {

                    PrintTextLine(printer, offSetString + String.Format("RABAT          -{0}", this.basilacakFis.Rabattutar.ToString("C")));
                    PrintTextLine(printer, offSetString + String.Format("ZWS             {0}", (this.basilacakFis.toplamtutar - this.basilacakFis.Rabattutar).ToString("C")));
                }
                PrintTextLine(printer, offSetString + String.Format("EC-KARTE        {0}", (this.basilacakFis.toplamtutar - this.basilacakFis.Rabattutar).ToString("C")));

            }
            if (this.OdemeTur == 2)
            {
                PrintTextLine(printer, offSetString + String.Format("TOTAL         {0}", this.basilacakFis.toplamtutar.ToString("C")));
                PrintTextLine(printer, offSetString + String.Format("BAR           {0}", this.basilacakFis.verilenpara.ToString("C")));
                PrintTextLine(printer, offSetString + "< STORNO! >");
                //PrintTextLine(printer, offSetString + new string('-', (printer.RecLineChars / 3)));
                //PrintTextLine(printer, offSetString + String.Format("TOTAL         {0}", (subTotal - (tax + discount)).ToString("#0.00")));
            }
            /* if (basilacakFis.Rabat != 0)
             {

                 PrintTextLine(printer, offSetString + String.Format("RABAT            {0}", this.basilacakFis.Rabattutar.ToString("#0.00")));
                 PrintTextLine(printer, offSetString + String.Format("ZWS              {0}", (this.basilacakFis.toplamtutar-this.basilacakFis.Rabattutar).ToString("#0.00")));
             }*/
            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            PrintTextLine(printer, String.Empty);

            //Embed 'center' alignment tag on front of string below to have it printed in the center of the receipt.
            if (basilacakFis.Bewirtung == 0)
                PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' }) + footerText);
            else
            {

                PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' }) + "Bewertungsaufwand-Angaben");
                PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' }) + "(Par. 4 §5 Ziff. 2 EStG)");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "Bewirtete Personen:");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "Anlaß der Bewirtung:");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "Höhe der Aufwendungen:");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));

                PrintTextLine(printer, "Bei Bewertung im Restaurant");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "in anderen Fallen");
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "Ort:                                Datum:");
                PrintTextLine(printer, new string(' ', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, "Unterschreiben");
                PrintTextLine(printer, new string(' ', (printer.RecLineChars)));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));



            }
            //Added in these blank lines because RecLinesToCut seems to be wrong on my printer and
            //these extra blank lines ensure the cut is after the footer ends.
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            //PrintTextLine(printer, String.Empty);
            //27,112,0,25,250
            //Print 'advance and cut' escape command.

            PrintTextLine(printer, "" + ((char)27) + ((char)109));// 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
            PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
        }

        private long GetRecLineChars(ref int[] RecLineChars)
        {
            long lRecLineChars = 0;
            long lCount;
            int i;

            // Calculate the element count.
            lCount = printer.RecLineCharsList.GetLength(0);

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
                    RecLineChars[i] = printer.RecLineCharsList[i];
                }

                lRecLineChars = lCount;
            }

            return lRecLineChars;
        }
        private void PrintLineItem(PosPrinter printer, string itemCode, double quantity, double unitPrice, double total)
        {
            string ek = "";
            string toptutar = total.ToString("C");
            int toptutaruzunluk = toptutar.Length;
            int kalanuzunluk = printer.RecLineChars - toptutaruzunluk;
            string bilgi = "";
            if (Program.Waage == 2)
            {
                bilgi = itemCode + "  {" + quantity.ToString("#0.000") + "x" + unitPrice.ToString("C") + "}";
            }
            else
            {
                bilgi = itemCode + "  {" + quantity.ToString() + "x" + unitPrice.ToString("C") + "}";
            }
            int bilgiuzunluk = bilgi.Length;
            if (bilgiuzunluk >= kalanuzunluk)
            {
                int fark = bilgiuzunluk - kalanuzunluk + 2;
                bilgi = itemCode.Substring(0, itemCode.Length - fark) + "..{" + quantity.ToString() + "x" + unitPrice.ToString("C") + "}";

            }
            else
            {
                int poz = kalanuzunluk - bilgiuzunluk;
                for (int i = 0; i < poz; i++)
                {
                    ek += " ";
                }
                bilgi = bilgi + ek;
            }

            PrintTextLine(printer, bilgi + toptutar);
            //PrintText(printer, toptutar.PadLeft(15));
            /*  PrintText(printer, TruncateAt(unitPrice.ToString("#0.00").PadLeft(10), 10));
             PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString("#0.00").PadLeft(10), 10));*/
        }

        private void PrintReceiptHeader(PosPrinter printer, string companyName, string addressLine1, string addressLine2, string taxNumber, DateTime dateTime, string usid)
        {
            PrintTextLine(printer, companyName.PadRight(3));
            PrintTextLine(printer, addressLine1);
            PrintTextLine(printer, addressLine2);
            PrintTextLine(printer, taxNumber);
            PrintTextLine(printer, "UsID: " + usid);
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            if (basilacakFis.Bewirtung == 0)
            {
                PrintTextLine(printer, "KOPIE--KOPIE--KOPIE--KOPIE--KOPIE--KOPIE");
                PrintTextLine(printer, new string('-', printer.RecLineChars));
            }
            PrintTextLine(printer, dateTime.ToShortDateString() + "/" + dateTime.ToLongTimeString() + " <BED." + Program.bedID + "> " + "<BON:" + this.basilacakFis.SatisAnaId + ">");
            PrintTextLine(printer, "Artikel        {Menge X Preis }         Total");
            //PrintText(printer, "Menge       ");
            //PrintText(printer, "Unit Price ");
            // PrintTextLine(printer, "Total ");
            PrintTextLine(printer, new string('=', printer.RecLineChars));
            //PrintTextLine(printer, String.Empty);

        }

        private void PrintText(PosPrinter printer, string text)
        {
            if (text.Length <= printer.RecLineChars)
                printer.PrintNormal(PrinterStation.Receipt, text); //Print text
            else if (text.Length > printer.RecLineChars)
                printer.PrintNormal(PrinterStation.Receipt, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest.
        }

        private void PrintTextLine(PosPrinter printer, string text)
        {
            if (text.Length < printer.RecLineChars)
                printer.PrintNormal(PrinterStation.Receipt, text + Environment.NewLine); //Print text, then a new line character.
            else if (text.Length > printer.RecLineChars)
                printer.PrintNormal(PrinterStation.Receipt, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest, no new line character (printer will probably auto-feed for us)
            else if (text.Length == printer.RecLineChars)
                printer.PrintNormal(PrinterStation.Receipt, text + Environment.NewLine); //Print text, no new line character, printer will probably auto-feed for us.
        }

        private string TruncateAt(string text, int maxWidth)
        {
            string retVal = text;
            if (text.Length > maxWidth)
                retVal = text.Substring(0, maxWidth);

            return retVal;
        }

        public Fis()
        {
            
            if (printer == null)
            {
                if (Program.PrinterLib == ".NET")
                {
                   // MessageBox.Show("Fis start .NET"); 
                    string myMsrName = "";
                    if (Program.printerType == "TM88" || Program.printerType == "T20" || Program.printerType == "citizen")
                    {
                        if (Program.printerType == "TM88" || Program.printerType == "T20")
                        {
                            myMsrName = "PosPrinter";
                        }
                        else if (Program.printerType == "citizen")
                        {

                            myMsrName = "CT-S310II_1";
                        }

                        //PosPrinter msr = null;
                        PosExplorer explorer = new PosExplorer();
                        DeviceInfo deviceInfo = null;

                        try
                        {
                            Thread.Sleep(5000);
                            deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
                        }
                        catch (Exception ee)
                        {
                            printer = null;
                            F_GenericError frmerr = new F_GenericError();
                            frmerr.lblMesaj.Text = "DRUCKER ERROR ->getreceiptprinter->deviceinfo=null " + ee.Message;
                            frmerr.ShowDialog();
                        }
                       // MessageBox.Show(deviceInfo.ToString());
                       if (deviceInfo == null)
                        {
                            printer = null;
                            F_GenericError frmerr = new F_GenericError();
                            frmerr.lblMesaj.Text = "DRUCKER ERROR!->getreceiptprinter->deviceinfo=null[3] "+deviceInfo.ToString();
                            frmerr.ShowDialog();
                        }
                        else
                        {
                            printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                            //m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
                            ConnectToPrinter(printer);
                            Program.printer = printer;
                            if (Program.cashdrawerSO != "")
                            {
                                CashDrawer m_Drawer = null;
                                PosExplorer posExplorer = new PosExplorer();

                                DeviceInfo deviceInfo2 = null;

                                try
                                {
                                    deviceInfo2 = posExplorer.GetDevice(DeviceType.CashDrawer, Program.cashdrawerSO);
                                    m_Drawer = (CashDrawer)posExplorer.CreateInstance(deviceInfo2);
                                }
                                catch (Exception)
                                {
                                    //Nothing can be used.
                                    //ChangeButtonStatus();
                                    return;
                                }
                                //Open the device
                                //Use a Logical Device Name which has been set on the SetupPOS.
                                m_Drawer.Open();

                                //Get the exclusive control right for the opened device.
                                //Then the device is disable from other application.
                                m_Drawer.Claim(1000);

                                //Enable the device.
                                m_Drawer.DeviceEnabled = true;
                                if (m_Drawer.DeviceEnabled == true)
                                {
                                    //Program.cashDrawer =(CashDrawer) m_Drawer;
                                }
                            }
                            /* F_GenericError frmerr = new F_GenericError();
                              frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                              frmerr.ShowDialog();*/
                            //return msr;
                        }
                    }
                  // MessageBox.Show("Fis Ende");
                }
                else
                {

                    try
                    {
                       // MessageBox.Show("OPOS DRUCKER!");
                        PosExplorer posExplorer = new PosExplorer();
                        DeviceCollection device1 = posExplorer.GetDevices();

                        string deviceler = "";
                       // MessageBox.Show(device1.ToString());
                        foreach (DeviceInfo device in device1)
                        {
                            if (device.Type == DeviceType.PosPrinter)
                            {
                                try
                                {
                                    if (device.ServiceObjectName == Program.printerSO)
                                    {
                                        printer = (PosPrinter)posExplorer.CreateInstance(device);
                                       // printer.CharacterSet = 858;
                                        System.Reflection.PropertyInfo devicePathAccess; 
                                        devicePathAccess = printer.GetType().GetProperty( "DevicePath", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                                        //MessageBox.Show(printer.ToString());

                                        if (devicePathAccess != null)
                                        {

                                            string devicePath = (string)devicePathAccess.GetValue(printer, null);

                                           //MessageBox.Show(devicePath);

                                        }

                                        else
                                        {

                                            //MessageBox.Show("Unable to access protected method.  Verify that it is not public");

                                        }
                                        //printer.CharacterSet = 1252;
                                        if (Program.printerType == "star")
                                        {
                                            
                                        }
                                        try
                                        {
                                            printer.Open();
                                        }
                                        catch (Exception ff)
                                        {
                                            //MessageBox.Show("Open()-Error:"+ff.Message);
                                        }
                                        
                                        try
                                        {
                                        printer.Claim(1000);
                                         }
                                        catch (Exception ff)
                                        {
                                            //MessageBox.Show("Claim()-Error:" + ff.Message);
                                        }
                                        try
                                        {
                                            printer.DeviceEnabled = true;
                                        }
                                        catch (Exception ff)
                                        {
                                            //MessageBox.Show("DeviceEnable()-Error:" + ff.Message);
                                        }
                                       // MessageBox.Show("Open-Claim-Enable OK");

                                        if (Program.printerType != "citizen" )
                                        {
                                            if (Program.printerType != "NCR")
                                            {
                                                if (Program.printerType == "star")
                                                {
                                                    printer.CharacterSet = 1252;
                                                    printer.MapCharacterSet = true;

                                                }
                                                else if (Program.printerSO == "T-3II")
                                                {
                                                    printer.CharacterSet = 1252;
                                                    printer.MapCharacterSet = true;

                                                }
                                                else if (Program.printerType == "bixolon")
                                                {
                                                    printer.CharacterSet = 1252;
                                                    printer.MapCharacterSet = true;

                                                }
                                                else if (Program.printerType == "bixolon")
                                                {
                                                    printer.CharacterSet = 1252;
                                                   // printer.MapCharacterSet = true;

                                                }
                                                else if (Program.printerType == "colormetrics")
                                                {
                                                    printer.CharacterSet = 1252;
                                                    // printer.MapCharacterSet = true;

                                                }
                                                else
                                                {

                                                    printer.CharacterSet = 1252;
                                                    printer.MapCharacterSet = true;
                                                   // printer.CharacterSet = 1252;
                                                    //MessageBox.Show("Else -734"+printer.CapCharacterSet.ToString());

                                                    //printer.MapCharacterSet = true;
                                                }
                                                //printer.CharacterSet = 1252;
                                            }
                                        }
                                       //MessageBox.Show("List:" + printer.CharacterSetList.ToString() + "charset  Def:" + printer.CharacterSet + " cap:" + printer.CapCharacterSet);
                                        if (printer.DeviceEnabled == true)
                                        {
                                           Program.printer = printer;
                                           // printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
                                            //printer.PrintNormal(PrinterStation.Receipt, "Drucker Status: OK!");
                                            //printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                                            if (Program.cashdrawerSO != "")
                                            {
                                                OPOSCashDrawer cashdriver = new OPOSCashDrawer();
                                                //opospos OPOSPOSPrinter Oprinter = new OPOSPOSPrinter();
                                                cashdriver.Open(Program.cashdrawerSO); //printer.CharacterSet = 858;
                                                cashdriver.ClaimDevice(4000);
                                                try
                                                {
                                                    cashdriver.DeviceEnabled = true;
                                                }
                                                catch (Exception dd)
                                                {
                                                    F_GenericError frmerr = new F_GenericError();
                                                    frmerr.lblMesaj.Text = dd.Message+"\nCODE:CD103";
                                                    frmerr.ShowDialog();
                                                }
                                                if (cashdriver.DeviceEnabled == true)
                                                {
                                                    //cashdriver.OpenDrawer();
                                                    Program.cashDrawer = cashdriver;

                                                }
                                                else
                                                {
                                                    F_GenericError frmerr = new F_GenericError();
                                                    frmerr.lblMesaj.Text = "CASH DRAWER ENABLED FALSE! \nCODE:CD104";
                                                    frmerr.ShowDialog();
                                                }
                                            }

                                            // MessageBox.Show("List:" + printer.CharacterSetList.ToString() + "charset  Def:" + printer.CharacterSet + " cap:" + printer.CapCharacterSet);
                                            break;
                                        }
                                    }
                                    //deviceler += device.ServiceObjectName + "\n";
                                }
                                catch (Exception ee)
                                {
                                    F_GenericError frmerr = new F_GenericError();
                                    frmerr.lblMesaj.Text =  ee.Message+"\nOPOS DRUCKER ERROR! CODE:P101"+ee.InnerException.Message ;
                                    frmerr.ShowDialog();
                                }
                            }
                        }
                       // MessageBox.Show(printer.CharacterSet.ToString());
                        //MessageBox.Show(printer.MapCharacterSet.ToString());
                    }

                    catch (Exception ee)
                    {
                        printer = null;
                        F_GenericError frmerr = new F_GenericError();
                        frmerr.lblMesaj.Text = "OPOS Drucker Error! " + ee.Message;
                        frmerr.ShowDialog();
                    }
                }

            }




        }
        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
        public void DisconnectPrinter()
        {
            try
            {
                if (printer != null && printer.DeviceEnabled == true)
                {
                    printer.Release();
                    printer.Close();
                }
            }
            catch { }
        }
        public void FisYazdir()
        {
            if (printer != null)
            {
                if (OdemeTur == 2)
                {
                    try
                    {
                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        for (int i = 0; i < 2; i++)
                        {
                            PrintReceipt();

                        }
                        basilacakFis = null;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    PrintReceipt();
                    basilacakFis = null;
                }
                //DisconnectFromPrinter(printer);
            }
            else
            {
                string myMsrName = "PosPrinter";
                //PosPrinter msr = null;
                PosExplorer explorer = new PosExplorer();
                DeviceInfo deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
                if (deviceInfo == null)
                {
                    printer = null;
                    F_GenericError frmerr = new F_GenericError();
                    frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo=null";
                    frmerr.ShowDialog();
                }
                else
                {
                    printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                    ConnectToPrinter(printer);
                    /* F_GenericError frmerr = new F_GenericError();
                      frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                      frmerr.ShowDialog();*/
                    //return msr;
                }
                if (OdemeTur == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        PrintReceipt();

                    }
                    basilacakFis = null;
                }
                else
                {
                    PrintReceipt();
                    basilacakFis = null;
                }
                //DisconnectFromPrinter(printer);
            }
        }
        public void BasitXrapor(DataTable dtOturum, DataTable dt, Int64 online, Int64 offline)
        {
            if (printer != null)
            {

                PrintBasitXRapor(dtOturum, dt, online, offline);
                dt.Rows.Clear();
                //DisconnectFromPrinter(printer);

            }
            else
            {
                string myMsrName = "PosPrinter";
                //PosPrinter msr = null;
                PosExplorer explorer = new PosExplorer();
                DeviceInfo deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
                if (deviceInfo == null)
                {
                    printer = null;
                    F_GenericError frmerr = new F_GenericError();
                    frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo=null";
                    frmerr.ShowDialog();
                }
                else
                {
                    printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                    ConnectToPrinter(printer);
                    /* F_GenericError frmerr = new F_GenericError();
                      frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                      frmerr.ShowDialog();*/
                    //return msr;
                }
                PrintBasitXRapor(dtOturum, dt, online, offline);
                dt.Rows.Clear();
                dtOturum.Rows.Clear();
                // DisconnectFromPrinter(printer);
            }
        }
        public void DetayliXrapor(DataTable dt, Int64 online, Int64 offline, DataTable dtozet)
        {
            if (printer != null)
            {

                PrintDetayliXRapor(dt, online, offline, dtozet);
                dt.Rows.Clear();
                printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                // DisconnectFromPrinter(printer);

            }
            else
            {
                string myMsrName = "PosPrinter";
                //PosPrinter msr = null;
                PosExplorer explorer = new PosExplorer();
                DeviceInfo deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
                if (deviceInfo == null)
                {
                    printer = null;
                    F_GenericError frmerr = new F_GenericError();
                    frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo=null";
                    frmerr.ShowDialog();
                }
                else
                {
                    printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                    ConnectToPrinter(printer);
                    /* F_GenericError frmerr = new F_GenericError();
                      frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                      frmerr.ShowDialog();*/
                    //return msr;
                }
                PrintDetayliXRapor(dt, online, offline, dtozet);
                printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                dt.Rows.Clear();
                //DisconnectFromPrinter(printer);
            }
        }
        private void PrintBasitXRapor(DataTable dtOturum, DataTable dt, Int64 online, Int64 offline)
        {
            if (printer == null)
            {
                //printer = GetReceiptPrinter();

                ConnectToPrinter(printer);
            }

            try
            {
                if (dtOturum == null)//tek oturum
                {
                    PrintXRapHeader(printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"], online, offline);
                    PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    double totalBar = 0, totalEC = 0, totalStorno = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PrintXRapLineItem(printer, Convert.ToString(dt.Rows[i].ItemArray[1]), (double)dt.Rows[i].ItemArray[0]);
                        if ((int)dt.Rows[i].ItemArray[1] == 0)
                        {
                            totalEC += (double)dt.Rows[i].ItemArray[0];
                        }
                        else if ((int)dt.Rows[i].ItemArray[1] == 1)
                        {
                            totalBar += (double)dt.Rows[i].ItemArray[0];
                        }
                        else
                        {
                            totalStorno += (double)dt.Rows[i].ItemArray[0];
                        }

                    }
                    PrintXRapFooter(printer, 1, totalBar, totalStorno, "VIELEN DANK");
                    printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                    //PrintXRapFooter(printer, 1, 0.1, 0.1, "VIELEN DANK");
                }
                else //Tum Oturumlar için Rapor
                {
                    int kaysay = dtOturum.Rows.Count;
                    online = Convert.ToInt32(dtOturum.Rows[0].ItemArray[0]);
                    offline = Convert.ToInt32(dtOturum.Rows[kaysay - 1].ItemArray[1]);
                    PrintXRapHeader(printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"], online, offline);
                    PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    double totalBar = 0, totalEC = 0, totalStorno = 0;
                    for (int i = 0; i < dtOturum.Rows.Count; i++)
                    {
                        PrintTextLine(printer, (i + 1) + ".SITZUNG [" + tarih.saat((long)dtOturum.Rows[i].ItemArray[0]) + "<->" + tarih.saat((long)dtOturum.Rows[i].ItemArray[1]) + "]");
                        PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                        PrintXRapLineItem(printer, "1", (double)dt.Rows[i].ItemArray[2]);
                        totalBar += (double)dt.Rows[i].ItemArray[2];
                        PrintXRapLineItem(printer, "0", (double)dt.Rows[i].ItemArray[3]);
                        totalEC += (double)dt.Rows[i].ItemArray[3];
                        PrintXRapLineItem(printer, "2", (double)dt.Rows[i].ItemArray[4]);
                        totalStorno += (double)dt.Rows[i].ItemArray[4];
                        PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                    }
                    PrintXRapFooter(printer, 1, totalBar, totalStorno, "VIELEN DANK");
                    printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
                   // m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);

                }
            }
            catch (Exception eexxe)
            {
                //throw new Exception(eexxe.Message);
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = eexxe.Message;
                frmerr.ShowDialog();
            }
        }
        private void PrintXRapHeader(PosPrinter printer, string companyName, string addressLine1, string addressLine2, string taxNumber, DateTime dateTime, string cashierName, Int64 online, Int64 offline)
        {
            printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
            PrintTextLine(printer, "BEDIENER X BERICHT");
            PrintTextLine(printer, companyName.PadRight(3));
            PrintTextLine(printer, addressLine1);
            PrintTextLine(printer, addressLine2);
            PrintTextLine(printer, taxNumber);
            PrintTextLine(printer, new string('-', printer.RecLineChars / 2));
            PrintTextLine(printer, dateTime.ToShortDateString() + "/" + dateTime.ToLongTimeString());
            PrintTextLine(printer, new string('-', printer.RecLineChars / 2));
            PrintTextLine(printer, " ");
            PrintTextLine(printer, "X BERICHT [KASSENBESTAND]");
            PrintTextLine(printer, "BED:" + Program.bedAdSoyad);
            PrintTextLine(printer, "[" + tarih.tarih(online) + "<->" + tarih.tarih(offline) + "]");
            PrintTextLine(printer, new string('=', printer.RecLineChars));
            //PrintTextLine(printer, String.Empty);

        }
        private void PrintXRapLineItem(PosPrinter printer, string itemCode, object total)
        {
            string ek = "";
            string toptutar = String.Format("{0:n}", total);
            int toptutaruzunluk = toptutar.Length;
            int kalanuzunluk = printer.RecLineChars - toptutaruzunluk;
            string bilgi = "";

            bilgi = Enum.GetName(typeof(OdemeTuru), Convert.ToInt16(itemCode));
            //(string)Enum.Parse(typeof(OdemeTuru),Enum.GetName(typeof(OdemeTuru), itemCode)) ;
            int bilgiuzunluk = bilgi.Length;

            int poz = kalanuzunluk - bilgiuzunluk;
            for (int i = 0; i < poz; i++)
            {
                ek += ".";
            }
            bilgi = bilgi + ek;


            PrintTextLine(printer, bilgi + toptutar);
            //PrintText(printer, toptutar.PadLeft(15));
            /*  PrintText(printer, TruncateAt(unitPrice.ToString("#0.00").PadLeft(10), 10));
             PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString("#0.00").PadLeft(10), 10));*/
        }
        private void PrintXRapFooter(PosPrinter printer, int subTotal, double total, double discount, string footerText)
        {
            string sonnnn = (total - discount).ToString("#0.00");
            PrintTextLine(printer, new string('=', (printer.RecLineChars)));
            PrintXRapDetayLineItem(printer, "Kasenbestand:", string.Format("{0:n}", (total + discount)));
            PrintTextLine(printer, new string('=', (printer.RecLineChars)));
            PrintTextLine(printer, String.Empty);

            //Embed 'center' alignment tag on front of string below to have it printed in the center of the receipt.
            PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' }));

            //Added in these blank lines because RecLinesToCut seems to be wrong on my printer and
            //these extra blank lines ensure the cut is after the footer ends.
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            //PrintTextLine(printer, String.Empty);
            //27,112,0,25,250
            //Print 'advance and cut' escape command.
            PrintTextLine(printer, "" + ((char)27) + ((char)109));// 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
           // PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
        }
        private void PrintDetayliXRapor(DataTable dt, Int64 online, Int64 offline, DataTable dtOzet)
        {
            if (printer == null)
            {
                //printer = GetReceiptPrinter();

                ConnectToPrinter(printer);
            }

            try
            {
                PrintXRapHeader(printer, Program.IsletmeAyarlar["isletme"], Program.IsletmeAyarlar["strase"], Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"], "T: " + Program.IsletmeAyarlar["tel1"] + " F: " + Program.IsletmeAyarlar["fax"], DateTime.Now, Program.IsletmeAyarlar["usid"], online, offline);
                //PrintXRapHeader(printer, "AKKAUF SUPERMARKT.", "xxxx Str.21", "xxxxx, Koblenz", "T:0212 64235713 F:0212 64235714 ", DateTime.Now, "ABCDEF",online,offline);
                PrintTextLine(printer, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                int mwst = 0;
                double toplam = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((int)dt.Rows[i].ItemArray[5] != mwst)
                    {
                        if (toplam != 0)
                        {
                            PrintXRapDetayLineItem(printer, "MwST " + mwst + "% TOTAL:", string.Format("{0:n}", toplam));
                            toplam = 0;
                        }
                        if (mwst == 0)
                        {
                            mwst = (int)dt.Rows[i].ItemArray[5];
                        }
                        mwst = (int)dt.Rows[i].ItemArray[5];
                        PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                        PrintXRapDetayLineItem(printer, "MwST " + mwst + "%", " TOTAL");
                    }
                    PrintXRapDetayLineItem(printer, Convert.ToString(dt.Rows[i].ItemArray[17]), (double)dt.Rows[i].ItemArray[0]);
                    toplam += (double)dt.Rows[i].ItemArray[0];
                    // PrintLineItem(printer, "Item 2", 101, 0.00);
                    //PrintLineItem(printer, "Item 3", 9, 0.1);
                    //PrintLineItem(printer, "Item 4", 1000, 1);

                }
                if (toplam != 0)
                {
                    PrintXRapDetayLineItem(printer, "MwST " + mwst + "% TOTAL:", toplam);
                    toplam = 0;
                }
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                double totalBar = 0, totalEC = 0, totalStorno = 0;
                for (int b = 0; b < dtOzet.Rows.Count; b++)
                {
                    PrintXRapLineItem(printer, Convert.ToString(dtOzet.Rows[b].ItemArray[1]), (double)dtOzet.Rows[b].ItemArray[0]);
                    if ((int)dtOzet.Rows[b].ItemArray[1] == 0)
                    {
                        totalEC += (double)dtOzet.Rows[b].ItemArray[0];
                    }
                    else if ((int)dtOzet.Rows[b].ItemArray[1] == 1)
                    {
                        totalBar += (double)dtOzet.Rows[b].ItemArray[0];
                    }
                    else
                    {
                        totalStorno += (double)dtOzet.Rows[b].ItemArray[0];
                    }

                }
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));

                PrintXRapFooter(printer, 1, totalBar, totalStorno, "VIELEN DANK");
            }
            catch (Exception eexxe)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = eexxe.Message;
                frmerr.ShowDialog();
            }
        }
        private void PrintXRapDetayLineItem(PosPrinter printer, string itemCode, object total)
        {
            string ek = "";
            string toptutar = string.Format("{0:n}", total);
            int toptutaruzunluk = toptutar.Length;
            int kalanuzunluk = printer.RecLineChars - toptutaruzunluk;
            string bilgi = "";

            bilgi = itemCode;
            //(string)Enum.Parse(typeof(OdemeTuru),Enum.GetName(typeof(OdemeTuru), itemCode)) ;
            int bilgiuzunluk = bilgi.Length;

            int poz = kalanuzunluk - bilgiuzunluk;
            for (int i = 0; i < poz; i++)
            {
                ek += ".";
            }
            bilgi = bilgi + ek;


            PrintTextLine(printer, bilgi + toptutar);
            //PrintText(printer, toptutar.PadLeft(15));
            /*  PrintText(printer, TruncateAt(unitPrice.ToString("#0.00").PadLeft(10), 10));
             PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString("#0.00").PadLeft(10), 10));*/
        }
    }
}
