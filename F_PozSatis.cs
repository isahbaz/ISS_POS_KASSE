using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Threading;
using Microsoft.PointOfService;
using MySql.Data.MySqlClient;
using iss_Rabat;

namespace IS_KASSE
{
    public partial class F_PozSatis : Form
    {
        public FisOlustur fis = null;
        double toplamtutar = 0;
        public bool TeilOdemeVarmi = false;
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();

        public delegate void KundenDisplayDelagate(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verielnPara, double paraUstu, int odemeTur);
        public event KundenDisplayDelagate KundenDisplayFisInhaltEvent;

        public F_PozSatis()
        {
            InitializeComponent();
        }

        private void dgvPoz_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(dgvPoz.Rows[e.RowIndex].Cells[0].Value) == false)
            {
                dgvPoz.Rows[e.RowIndex].Cells[0].Value = true;
                toplamtutar += Convert.ToDouble(dgvPoz.Rows[e.RowIndex].Cells[2].Value);
                this.KundenDisplayFisInhaltEvent(dgvPoz.Rows[e.RowIndex].Cells[1].Value.ToString(), "1", dgvPoz.Rows[e.RowIndex].Cells[2].Value.ToString(), toplamtutar.ToString(), "TOTAL :" + toplamtutar.ToString(), 0,0,0,0);


            }
            else
            {
                dgvPoz.Rows[e.RowIndex].Cells[0].Value = false;
                toplamtutar -= Convert.ToDouble(dgvPoz.Rows[e.RowIndex].Cells[2].Value);
                this.KundenDisplayFisInhaltEvent(dgvPoz.Rows[e.RowIndex].Cells[1].Value.ToString(), "1", dgvPoz.Rows[e.RowIndex].Cells[2].Value.ToString(), (-toplamtutar).ToString(), "TOTAL :" + toplamtutar.ToString(), 1,0,0,0);
            }
            label1.Text = "TOTAL=" + toplamtutar.ToString("#0.00");
            

        }

        private void F_PozSatis_Load(object sender, EventArgs e)
        {
            this.KundenDisplayFisInhaltEvent("", "", "", "", "", 9,0,0,0);
            KryptonDataGridViewCheckBoxColumn cbCol = new KryptonDataGridViewCheckBoxColumn();
            cbCol.Width = 50;
            //cbCol.HeaderText = "SEÇ";
            dgvPoz.Columns.Add(cbCol);
            cbCol.ThreeState = false;
            dgvPoz.Columns.Add("artikel", "ARTIKEL NAME");
            dgvPoz.Columns[1].Width = 400;
            dgvPoz.Columns[1].ReadOnly = true;
            dgvPoz.Columns.Add("preis", "PREIS");
            dgvPoz.Columns[2].Width = 100;
            dgvPoz.Columns[2].ReadOnly = true;
            SatisKalemyukle();
            // dgvPoz.RowsDefaultCellStyle.h
          /* for (int i = 0; i < fis.SatisKalem.Count; i++)
            {
                dgvPoz.Rows.Add();
                dgvPoz.Rows[i].Cells[1].Value = fis.SatisKalem[i].UrunAd;
                dgvPoz.Rows[i].Cells[2].Value = fis.SatisKalem[i].Toplamtutar.ToString("#0.00");
                dgvPoz.Rows[i].Height = 40;
            }*/

        }
        private void SatisKalemyukle()
        {
             for (int i = 0; i < fis.SatisKalem.Count; i++)
            {
                dgvPoz.Rows.Add();
                dgvPoz.Rows[i].Cells[1].Value = fis.SatisKalem[i].UrunAd;
                dgvPoz.Rows[i].Cells[2].Value = fis.SatisKalem[i].Toplamtutar.ToString("#0.00");
                dgvPoz.Rows[i].Height = 40;
            }
        }
        private void btnBar_Click(object sender, EventArgs e)
        {
            Tarih tarih = new Tarih();
            SatisYap satisYap = null;
            FisOlustur TeilFis = new FisOlustur();

            TeilFis.FisYarat(0);
            TeilFis.MasaID = fis.MasaID;
            TeilFis.Masano = fis.Masano;
            TeilFis.Bewirtung = fis.Bewirtung;
            List<int> silinenIDler = new List<int>();
            List<SatisYap> KalanSatislar = new List<SatisYap>();
            for (int i = 0; i < dgvPoz.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvPoz.Rows[i].Cells[0].Value) == true)
                {
                    SatisYap TeilPoz = new SatisYap();
                    TeilPoz = fis.SatisKalem[i];
                    TeilFis.SatisKalem.Add(TeilPoz);
                    silinenIDler.Add(i);
                    //fis.SatisKalem.RemoveAt(i);
                    //dgvPoz.Rows.RemoveAt(i);
                }
                else
                {
                    KalanSatislar.Add(fis.SatisKalem[i]);
                }
            }
            TeilFis.FisiKapat();
            
            
            using (BarVerkauf barVk = new BarVerkauf())
            {

                barVk.toplamtutar = Math.Round(TeilFis.toplamtutar, 2, MidpointRounding.AwayFromZero);
                barVk.toplammwst = TeilFis.toplammwst;
                barVk.mwst19miktar = TeilFis.mwst19miktar;
                barVk.mwst7miktar = TeilFis.mwst7miktar;
                barVk.musteriNo = TeilFis.Musterino;
                barVk.angebotsuztoplamtutar = TeilFis.AngebotsuzToplamTutar;
                barVk.aktuelFis = TeilFis;

                //serialPortKD2.Close();
                barVk.ShowDialog();
                if (barVk.odemeSonuc == true)
                {
                    TeilFis.odemesekli = barVk.odemeturu;


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
                                else if (Program.printerType != "NCR" || Program.printerSO != "T-3II")
                                {
                                    //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
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
                        //else
                        // Program.cashDrawer.OpenDrawer();
                        //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)25) + ((char)250));
                    }
                    catch (Exception gg)
                    {
                        //logEntry.AddtoLogFile(gg.Message, " [1802]");
                    }

                    try
                    {
                        TeilFis.ToplamScheck = barVk.teilCek;
                        TeilFis.ToplamEc = barVk.teilEC;
                        TeilFis.ToplamBar = barVk.teilBar;
                        //serialPortKD2.Open();
                        //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                        //art->TeilFisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                        if (TeilFis.Musterino != 0 || barVk.harcananPuanKarsiligiHarcananPara != 0)
                        {
                            TeilFis.KazanilanPuan = barVk.kazanilanPuan;
                            if (barVk.indirimturu == -1) //-1: Punkte einlösen
                            {
                                TeilFis.EskiPuanToplamı = barVk.eskiPuanToplami;
                                TeilFis.HarcananPuan = barVk.harcananPuan;
                                TeilFis.Indirimturu = -1;
                                RabattMain rbt = new RabattMain();
                                rbt.RabattAlani = 0;
                                rbt.RabatArt = 2;
                                rbt.RabatName = "Punkte Einlösung";
                                rbt.RabattTyp = 1;
                                rbt.TotalRabattMenge = barVk.kazanilanIndirim;
                                rbt.RabatMenge = barVk.kazanilanIndirim;
                                rbt.Grupid = 7;
                                TeilFis.RabatList.Add(rbt);
                                TeilFis.FisiKapat();
                                TeilFis.Musteri.Kredit = barVk.kredit;
                            }
                            else if (barVk.indirimturu == -2) //-2: Direk kunden rabatt, alternative von Puntesammeln
                            {


                                TeilFis.Indirimturu = -2;
                                //TeilFis.Rabat = barVk.rabatOran;
                                //TeilFis.Rabattutar = barVk.kazanilanIndirim;

                                RabattMain rbt = new RabattMain();
                                rbt.RabattAlani = 0;
                                rbt.RabatArt = 3;
                                rbt.RabatName = "Kundenrabatt";
                                rbt.RabattTyp = 0;
                                rbt.Grupid = 7;
                                rbt.RabatMenge = barVk.rabatOran;
                                rbt.TotalRabattMenge = barVk.kazanilanIndirim;
                                TeilFis.RabatList.Add(rbt);
                                TeilFis.FisiKapat();
                                TeilFis.Musteri.Kredit = barVk.kredit;
                                //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                                //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                            }
                            else if (barVk.indirimturu == -3)
                            {


                                TeilFis.Indirimturu = -3;
                                //TeilFis.Rabat = barVk.rabatOran;
                                //TeilFis.Rabattutar = barVk.kazanilanIndirim;

                                RabattMain rbt = new RabattMain();
                                rbt.RabattAlani = 0;
                                rbt.RabatArt = 4;
                                rbt.RabatName = "Gerabo-KK Rabatt";
                                rbt.RabattTyp = 1;
                                rbt.Grupid = 7;
                                rbt.RabatMenge = barVk.kazanilanIndirim;
                                rbt.TotalRabattMenge = barVk.harcananPuanKarsiligiHarcananPara;
                                TeilFis.RabatList.Add(rbt);
                                TeilFis.FisiKapat();
                            }


                        }
                    }
                    catch (Exception eec)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = eec.Message;
                        frmerror.ShowDialog();
                    }

                    TeilFis.verilenpara = barVk.verilenpara;
                    TeilFis.paraustu = barVk.paraustu;
                    //knddsply.VerkaufInfo("BAR :\n" + fis.verilenpara.ToString("C") + "\nRÜCKGELD :\n" + fis.paraustu.ToString("C"), "");
                    if (Program.displaySO == "TVS")
                    {

                        Application.DoEvents();

                        //Thread.Sleep(100);
                        if (Program.cashDrawer != null)
                            Program.cashDrawer.OpenDrawer();

                    }


                    try
                    {
                        if (TeilFis.FisiSonlandır(TeilFis.odemesekli, TeilFis.SatisKalem) == true)
                        {
                            //lblBonNo.Text = TeilFis.SatisAnaId.ToString();
                            foreach (RabattMain rbt in TeilFis.RabatList)
                            {
                                if (rbt.Grupid != 999)
                                {
                                    if (rbt.RabatArt != 6)
                                    {
                                        satisYap = new SatisYap();
                                        satisYap.Adet = 1;
                                        satisYap.Fisno = TeilFis.SatisAnaId;
                                        satisYap.KasaNo = Program.kasano;
                                        satisYap.Mwst = 0;
                                        satisYap.Satisfiyat = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                        satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                        satisYap.Toplamtutar = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                        satisYap.UrunId = 0;
                                        satisYap.Birimkar = 0;
                                        satisYap.UrunAd = rbt.RabatName;
                                        satisYap.Grubid = rbt.Grupid;
                                        TeilFis.SatisKalem.Add(satisYap);
                                    }
                                }
                            }

                            double satisicerikKontrol = 0;
                            foreach (SatisYap satisIcerik in TeilFis.SatisKalem)
                            {
                                try
                                {
                                    if (satisIcerik.Barkod == null) satisIcerik.Barkod = "0";
                                    satisIcerik.Fisno = TeilFis.SatisAnaId;
                                    satisicerikKontrol += satisIcerik.Toplamtutar;
                                    satisIcerik.Kaydet();
                                }
                                catch (Exception rr)
                                {
                                    //logEntry.AddtoLogFile(satisIcerik.ToString(), "3117");
                                }
                                using (myConn = baglanti.myconn())
                                {
                                    if (myConn.State == ConnectionState.Closed)
                                    {
                                        myConn.Open();
                                    }

                                    MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE satisid= " + satisIcerik.MasaDetailId, myConn);
                                    co1.ExecuteNonQuery();

                                }
                            }

                            
                            using (myConn = baglanti.myconn())
                            {
                                if (myConn.State == ConnectionState.Closed)
                                {
                                    myConn.Open();
                                }
                                
                                    MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE masadbid= " + TeilFis.MasaDBid, myConn);
                                    co1.ExecuteNonQuery();
                                
                            }
                            //MessageBox.Show(lblParaUstu.Text);
                            if (Program.bonDruck == true)
                            {
                                try
                                {
                                    //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));

                                    Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                                    Program.BonBeleg.OdemeTur = TeilFis.odemesekli;
                                    Program.BonBeleg.basilacakFis = TeilFis;
                                    is1.Start();
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                //Program.bonDruck = true;
                                // kryptonButton19.StateNormal.Back.Image = Properties.Resources.printer_green;
                                try
                                {
                                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                }
                                catch
                                {
                                }

                            }


                            // MessageBox.Show(lblParaUstu.Text);

                            if (TeilFis.odemesekli == 0)

                                TeilFis = null;
                            satisYap = null;


                            if (Program.cashDrawer != null)
                            {
                                if ((Program.cashDrawer.DrawerOpened == true))
                                    Program.cashDrawer.WaitForDrawerClose(10000, 2000, 100, 1000);
                                /*  while (Program.cashDrawer.DrawerOpened == true)
                                  {
                                      System.Threading.Thread.Sleep(100);
                                  }*/

                                //When the drawer is not closed in ten seconds after opening, beep until it is closed.
                                //If  that method is executed, the value is not returned until the drawer is closed.

                            }
                            TeilFis = null;
                            satisYap = null;
                            this.fis.SatisKalem = KalanSatislar;
                            fis.FisiKapat();
                            dgvPoz.Rows.Clear();
                            SatisKalemyukle();
                            this.Close();
                            //MessageBox.Show(lblParaUstu.Text);
                            //KundenDisplay();
                        }
                    }
                    catch (Exception ee)
                    {
                        //logEntry.AddtoLogFile(ee.Message, " [2143]");
                    }


                    //yazıcı->gonder;
                }
                else
                {

                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["25"];
                    frmerror.ShowDialog();



                }
            }
                        TeilFis = null;
                        satisYap = null;
                        this.fis.SatisKalem = KalanSatislar;
                        fis.FisiKapat();
                        dgvPoz.Rows.Clear();
                        SatisKalemyukle();
                        this.Close();
        }
        public void fisyaz()
        {
            if (Program.PrinterLib == ".NET")
            {
                FisBarkodlu barkodluFis = new FisBarkodlu();
                barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                barkodluFis.FisYaz();
                //yeniFis.SatisKalem.Clear();
                /*  if (Program.GlobalAyarlar["LOGO"] == 1)
                  {
                FisBarkodlu barkodluFis = new FisBarkodlu();
                barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                barkodluFis.FisYaz();

                  }
                  else
                  {
                Program.BonBeleg.FisYazdir();
                  }*/
            }
            else
            {
                /*  FisOPOS barkodluFis = new FisOPOS();
                  // MessageBox.Show("barkodluFis");
                  barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                  barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                  barkodluFis.FisYaz();*/
                FisBarkodlu barkodluFis = new FisBarkodlu();
                barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                barkodluFis.FisYaz();
                // yeniFis.SatisKalem.Clear();
            }
            //KundenDisplay();
            //knddsply.VerkaufInfo("Herzlich Willkommen!", Program.IsletmeAyarlar["isletme"]);
            // knddsply.VerkaufInfo("BAR :\n" + yeniFis.verilenpara.ToString("C") + "\nRÜCKGELD \n:" + yeniFis.paraustu.ToString("C"), "");

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
