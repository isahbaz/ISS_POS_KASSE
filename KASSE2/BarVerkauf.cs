using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using iss_Kunden;
using iss_ebon;
namespace IS_KASSE
{
    public partial class BarVerkauf : Form
    {
        public double paraustu=0, verilenpara = 0, toplamtutar = 0, toplammwst = 0, mwst7tutar = 0, mwst7miktar = 0, mwst19tutar = 0, mwst19miktar = 0, mwst0tutar = 0;
        public double eskiPuanToplami=0, kazanilanPuan=0, harcananPuan=0, harcananPuanKarsiligiHarcananPara=0, kazanilanIndirim=0, angebotsuztoplamtutar=0, kredit=0;
        public double teilEC = 0, teilCek = 0, teilBar=0, odenenTeilBetrag=0, genericBetrag=0;
        public long musteriNo = 0;
        public int indirimturu = 0, odemeturu=1;
        public double rabatOran = 0;
        public FisOlustur aktuelFis;
        public Boolean odemeSonuc = false;
        public int EcOdeme = 0;
        public bool ReadGeraboKart=false;
        iss_Kunden.Musteri musteri;

        public GeraboAccountInfo gerabOInfo = new GeraboAccountInfo();
        Gerabo GeraboIslem = null;
        public delegate void BenimDelegem(string info, string Info2);
        public event BenimDelegem BenimEventim;
        public string eBonVerifiedCode="";
        public BarVerkauf()
        {
            InitializeComponent();
            if (EcOdeme == 1)
                btnZws.Enabled = false;
            else
                btnZws.Enabled = true;
        }

        private void BarVerkauf_Load(object sender, EventArgs e)
        {
            //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
            //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
            if (Program.ebon != "" && Program.ebon != null && Program.ebon != "0" )
            {
                btnMobilPay.Visible = true;
            }
            lblTutar.Text = "TOTAL : "+toplamtutar.ToString("C");
           // -1 : Punkte einlösung, nachlass(typ=0), algemein(rabattalani=0). 
            if (musteriNo == 0)
            {
                lblGutHaben.Visible = false;
                btnEinlosen.Enabled = false;
                btnKredit.Enabled = false;
            }
            else
            {
                musteri = aktuelFis.Musteri;
                eskiPuanToplami = musteri.KullanilabilirPuan;
                if (musteri.Method == 1) //punktesammel
                {
                    //eskiPuanToplami = musteri.KullanilabilirPuan;
                    //indirimturu = -1;
                    lblGutHaben.Text = "Pnkt:" + (musteri.KullanilabilirPuan.ToString()) + "<--> € :" + musteri.KullanilabilirKredi.ToString("C");
                    string test = lblGutHaben.Text;
                }
                else if (musteri.Method == 2) //direkt rabatt
                {
                    lblGutHaben.Visible = false;
                    //indirimturu = -2;
                    if (musteri.OzelOran == 0)
                    {
                        rabatOran = Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                    }
                    else
                    {
                        rabatOran = musteri.OzelOran;
                        
                    }
                    /*F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["94"]+rabatOran+"%";
                    frmerror.ShowDialog();*/
                    MusteriIndirimiUygula();
                }
                else if (musteri.Method ==3 )
                {
                    //eskiPuanToplami = musteri.KullanilabilirPuan;
                    //indirimturu = -1;
                    if (musteri.KullanilabilirPuan > 0)
                    {
                        lblGutHaben.Text = "Pnkt:" + (musteri.KullanilabilirPuan.ToString()) + "<--> € :" + musteri.KullanilabilirKredi.ToString("C");
                    }
                    else
                    {
                        label1.Visible = false;
                    }
                }

                if (musteri.ToplamKredit > 0)
                {
                    F_KreditRuckzahlung fkredit = new F_KreditRuckzahlung();
                    fkredit.musteri = musteri;
                    fkredit.musterino = musteri.MusteriId;
                    fkredit.ShowDialog();
                }
            }
           // rtbInfo.Font = new Font("Verdana", 10, FontStyle.Bold);
            //rtbInfo.Text += "TOTAL MwST : " + toplammwst.ToString("C") + "\n";

            if (ReadGeraboKart == true)
            {
               
                btnGeraboPuntzuGeld.Visible = true;
                lblGutHaben.Visible = true;
                lblGutHaben.Text = "Pnkt:" + (gerabOInfo.account.Points.ToString()) + "\n€ :" + (Convert.ToDouble(gerabOInfo.account.Credits) / 100).ToString("C");
                btnEinlosen.Enabled = true;
                btnKredit.Enabled = false;
                GeraboIslem = new Gerabo();
                GeraboKundeDetay();
            }
            else
                btnGeraboPuntzuGeld.Visible = false;

           
        }

        private void MusteriIndirimiUygula()
        {
           /* kazanilanIndirim = rabatOran;
            toplamtutar = Math.Round(toplamtutar - ((angebotsuztoplamtutar) * rabatOran / 100),2);
            lblTutar.Text = "TOTAL : " + (toplamtutar).ToString("C");
            indirimturu = -2;*/
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           


        }
        double odenenpara = 0;
        private void ParaEkle(object sender, EventArgs e)
        {

            PictureBox paraResmi = sender as PictureBox;
            double tiklanapara;//,odenenpara;
            if (!double.TryParse(paraResmi.Tag.ToString(), out tiklanapara))
            {
                tiklanapara = 0;
            }
            else
            {
                if ((Program.GlobalAyarlar["BKOMMA"] == 0))
                {
                    tiklanapara = tiklanapara * 100;
                }
                else
                {

                }
            }
                odenenpara+=tiklanapara;
         /*   if (!double.TryParse(txtOdenen.Text, out odenenpara))
            {
                odenenpara = 0;

            }
            else
            {
                odenenpara += tiklanapara;
            }
            */

                if ((Program.GlobalAyarlar["BKOMMA"] == 0))
                {
                    verilenpara = odenenpara / 100;
                }
                else
                {
                    verilenpara = (odenenpara);// +tiklanapara;
                }

            txtOdenen.Text = verilenpara.ToString("N");
           // txtOdenen.Text = verilenpara.ToString();
           
            paraustu = verilenpara - toplamtutar;
            lblParaUstu.Text = (verilenpara - toplamtutar).ToString("C");

        }
        private void btnZws_Click(object sender, EventArgs e)
        {
            if (txtOdenen.Text == "0" || txtOdenen.Text == "")
            {
                if (odemeturu == 4)
                {

                    if (Math.Round(toplamtutar,2,MidpointRounding.AwayFromZero) > 0)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["64"];
                        frmerror.ShowDialog();
                    }
                    return;
                }
                else
                {
                    odemeturu = 1;
                    teilBar = toplamtutar;
                    odemeSonuc = true;
                    if (musteriNo != 0)
                    {
                        getKundeProcess();
                    }
                    //serialPortKD2.Close();
                    this.Close();
                    verilenpara = toplamtutar;
                    paraustu = 0;
                }


            }
            else if (verilenpara >= Math.Round(toplamtutar,2))
            {
                odemeSonuc = true;
                if (musteriNo != 0)
                {
                    getKundeProcess();
                }

                teilBar = toplamtutar;
                this.Close();
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["64"]; 
                frmerror.ShowDialog();
            }

        }

        private void getKundeProcess()
        {
            /*1:Rabat
                 * 2:Punkte 
                 * 3:Sor*/
           
            if (musteri.Method == 2)//rabat
            {
                if (musteri.OzelOran == 0)
                {
                    rabatOran = Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                }
                else
                {
                    rabatOran = musteri.OzelOran;
                }

            }
            else if (musteri.Method == 1)//punkte
            {
                angebotsuztoplamtutar = angebotsuztoplamtutar - kazanilanIndirim;
                if (angebotsuztoplamtutar > 0)
                {
                    kazanilanPuan = Math.Round(angebotsuztoplamtutar * (Convert.ToDouble(Program.IsletmeAyarlar["kartstandartpuan"])));
                    //indirimturu = -1;
                }
            }
            else if (musteri.Method == 3)
            {
                if (toplamtutar > 0)
                {
                    using (F_PunkteOderRabat frmfrage = new F_PunkteOderRabat())
                    {

                        frmfrage.ShowDialog();
                        if (frmfrage.sonuc == 2)
                        {
                            if (musteri.OzelOran == 0)
                            {
                                rabatOran = Convert.ToDouble(Program.IsletmeAyarlar["kartstandartrabat"]);
                            }
                            else
                            {
                                rabatOran = musteri.OzelOran;
                            }
                            indirimturu = -2;
                        }
                        else if (frmfrage.sonuc == 1)
                        {
                            angebotsuztoplamtutar = angebotsuztoplamtutar - kazanilanIndirim;
                            if (angebotsuztoplamtutar > 0)
                            {
                                kazanilanPuan = Math.Round(angebotsuztoplamtutar * (Convert.ToDouble(Program.IsletmeAyarlar["kartstandartpuan"])));
                               // indirimturu = -1;
                            }
                        }
                    }

                }
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            //serialPortKD2.Close();
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            txtOdenen.Text = "0";
            lblParaUstu.Text = "";
            odenenpara = 0;
        }

        private void btnIki_Click(object sender, EventArgs e)
        {
            double girilenPara;
            verilenpara=0;
            if(txtOdenen.Text=="0")
            {
                txtOdenen.Text="";
            }
            KryptonButton btn = sender as KryptonButton;
            txtOdenen.Text += btn.Text;

            if(double.TryParse(txtOdenen.Text, out girilenPara))
            {
                if (((Program.GlobalAyarlar["BKOMMA"] == 0)))
                {
                    
                    verilenpara = (girilenPara / 100);
                    odenenpara = verilenpara;
                    lblParaUstu.Text = ( verilenpara- toplamtutar).ToString("C");
                    paraustu = (verilenpara) - toplamtutar;
                }
                else
                {
                    verilenpara = girilenPara;
                    odenenpara = girilenPara;
                    lblParaUstu.Text = (odenenpara - toplamtutar).ToString("C");
                    paraustu = verilenpara - toplamtutar;
                    
                }
                
            }
        }
        private void ParaGir()
        {
            
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            
            

        }

        private void btnKredit_Click(object sender, EventArgs e)
        {
            if (txtOdenen.Text == "0" || txtOdenen.Text == "")
            {
                kredit = toplamtutar;
                toplamtutar = 0;
                lblParaUstu.Text = "KREDIT :" + kredit.ToString("C");

            }
            else
            {
                double pp = 0;
                if (double.TryParse(txtOdenen.Text, out pp))
                {
                    if (ReadGeraboKart == true)
                    {
                        F_GenericSoru frmerror = new F_GenericSoru();
                        frmerror.lblMesaj.Text = Program.lang["93"] ;
                        frmerror.ShowDialog();
                        if (frmerror.sonuc == true)
                        {
                            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
                            RetunInfo = GeraboIslem.CollectCredit(GeraboIslem.Url, gerabOInfo.code, pp.ToString("N").IndexOf(',') != -1 ? pp.ToString("N").Remove(pp.ToString("N").Length - 3, 1) : pp.ToString("N"));//ToString("N").Remove(yeniFis.AngebotsuzToplamTutar.ToString("N").Length-3,1
                            lblGutHaben.Visible = true;
                            lblGutHaben.Text = "Pnkt:" + (RetunInfo.value.ToString()) + "\n€ :" + (Convert.ToDouble(RetunInfo.value) / 100).ToString("C");
                            //RetunInfo.value;
                        }

                        
                    }
                    else
                    {
                        kredit = pp;
                        toplamtutar = 0;
                        txtOdenen.Text = "0";
                        lblParaUstu.Text = "KREDIT :" + kredit.ToString("C");
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["24"];
                    frmerror.ShowDialog();
                }
            }
            
        }

        private void btnEinlosen_Click(object sender, EventArgs e)
        {
            double girilenPara = 0;
            if (ReadGeraboKart == true)
            {
                if (txtOdenen.Text != "0" || txtOdenen.Text != "")
                {
                    double pp=0;
                    if (double.TryParse(txtOdenen.Text, out pp))
                    {
                        GeraboPunkteeinlosung ergebnis= new GeraboPunkteeinlosung();
                        ergebnis=GeraboIslem.GeraboEinlosung(GeraboIslem.Url, gerabOInfo.code, pp.ToString("N").IndexOf(',') != -1 ? pp.ToString("N").Remove(pp.ToString("N").Length - 3, 1) : pp.ToString("N"));
                        if (ergebnis.meta.code == 3)
                        {
                            try
                            {
                                kazanilanIndirim = pp;
                                toplamtutar = toplamtutar - kazanilanIndirim;
                                lblTutar.Text = "TOTAL : " + (toplamtutar).ToString("C");
                                GeraboKundeDetay();
                                kryptonButton1.Enabled = false;
                                indirimturu = -3;
                                //this.aktuelFis.Rabattutar =this.aktuelFis.Rabattutar+ pp;
                                harcananPuanKarsiligiHarcananPara = pp;
                                txtOdenen.Text = "0";
                                odenenpara = 0;
                                
                            }
                            catch (Exception rr)
                            {
                                MessageBox.Show(rr.Message);
                            }

                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["95"] +"\n"+ ergebnis.meta.message;
                            frmerror.ShowDialog();
                        }

                    }
                    
                }
            }
            else //diger Kartlar
            {
                if (musteri.KullanilabilirPuan >= Convert.ToInt32(Program.IsletmeAyarlar["enazpuan"]))
                {
                    lblParaUstu.Text = "";
                    if (double.TryParse(txtOdenen.Text, out girilenPara))
                    {
                        /*if (((Program.GlobalAyarlar["BKOMMA"] == 0)))
                        {
                            //odenenpara = verilenpara;
                            verilenpara = (girilenPara / 100);
                            lblParaUstu.Text = (verilenpara - toplamtutar).ToString("C");
                            paraustu = (verilenpara) - toplamtutar;
                            
                        }
                        else
                        {
                            lblParaUstu.Text = (verilenpara - toplamtutar).ToString("C");
                            paraustu = verilenpara - toplamtutar;
                            odenenpara = verilenpara;
                        }*/
                        harcananPuanKarsiligiHarcananPara=girilenPara;
                        if (harcananPuanKarsiligiHarcananPara > musteri.KullanilabilirKredi)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["91"] + musteri.KullanilabilirKredi.ToString("C");
                            frmerror.ShowDialog();
                        }
                        else
                        {
                            //lblParaUstu.Text = (verilenpara - toplamtutar).ToString("C");
                            //paraustu = verilenpara - toplamtutar;
                            double yenipuan = 0;
                            double yenikullanilabilirkredi = 0;
                            if (harcananPuanKarsiligiHarcananPara >= toplamtutar)
                            {
                                kazanilanIndirim = toplamtutar;
                                toplamtutar = 0;
                                lblTutar.Text = "TOTAL : 0";
                                harcananPuan = kazanilanIndirim * Convert.ToDouble(Program.IsletmeAyarlar["harcamapuan"]);
                                yenipuan = musteri.KullanilabilirPuan - harcananPuan;
                                yenikullanilabilirkredi = musteri.KullanilabilirKredi - kazanilanIndirim;
                                lblGutHaben.Text = "Pnkt:" + (yenipuan.ToString()) + "<--> € :" + yenikullanilabilirkredi.ToString("C");
                                indirimturu = -1;
                                /*if (serialPortKD2.IsOpen == true)
                                {
                                    //serialPortKD2.Write("" + ((char)27) + ((char)82) + ((char)02));
                                    KDbirinciSatiraYaz("RABAT:", "-" + kazanilanIndirim.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + toplamtutar.ToString("#0.00"));

                                }
                                else
                                {
                                    serialPortKD2.Open();
                                    KDbirinciSatiraYaz("RABAT:", "-" + kazanilanIndirim.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + toplamtutar.ToString("#0.00"));
                                }*/
                                teilBar = toplamtutar;
                            }
                            else
                            {
                                kazanilanIndirim = harcananPuanKarsiligiHarcananPara;
                                toplamtutar = toplamtutar - kazanilanIndirim;
                                lblTutar.Text = "TOTAL : " + (toplamtutar).ToString("C");
                                harcananPuan = kazanilanIndirim * Convert.ToDouble(Program.IsletmeAyarlar["harcamapuan"]);
                                yenipuan = musteri.KullanilabilirPuan - harcananPuan;
                                yenikullanilabilirkredi = musteri.KullanilabilirKredi - kazanilanIndirim;
                                lblGutHaben.Text = "Pnkt:" + (yenipuan.ToString()) + "<--> € :" + yenikullanilabilirkredi.ToString("C");
                                indirimturu = -1;
                                /*if (serialPortKD2.IsOpen == true)
                                {
                                    //serialPortKD2.Write("" + ((char)27) + ((char)82) + ((char)02));
                                    KDbirinciSatiraYaz("RABAT:", "-" + kazanilanIndirim.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + toplamtutar.ToString("#0.00"));

                                }
                                else
                                {
                                    serialPortKD2.Open();
                                    KDbirinciSatiraYaz("RABAT:", "-" + kazanilanIndirim.ToString("#0.00"));
                                    KDikinciSatiraYaz("TOTAL : " + toplamtutar.ToString("#0.00"));
                                }*/

                            }

                        }
                        txtOdenen.Text = "";
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["23"];
                        frmerror.ShowDialog();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["92"] + Program.IsletmeAyarlar["enazpuan"];
                    frmerror.ShowDialog();
                }
                odenenpara = 0;
            }
        }
        public void KDbirinciSatiraYaz(string p, string p_2)
        {
            string bosluk = "";
            int fiyatuzunluk = p_2.Length;
            int kalanuzunluk = 20 - fiyatuzunluk;
            p = karakterCevir(p);
            int isimuzunluk = p.Length;
            if (isimuzunluk > kalanuzunluk)
            {
                p = p.Substring(0, kalanuzunluk - 2);
                p = p + ".:" + p_2;
            }
            else
            {
                int fark = kalanuzunluk - isimuzunluk - 1;
                for (int i = 0; i < fark; i++)
                {
                    bosluk += " ";
                }
                p = p + ":" + bosluk + p_2;
            }

            if (serialPortKD2.IsOpen == true)
            {

                if (Program.kasano == 3)
                {
                    serialPortKD2.Write("" + ((char)27) + ((char)64));
                    serialPortKD2.Write("" + ((char)11));
                    serialPortKD2.Write(p);
                }
                else
                {
                    serialPortKD2.Write("" + ((char)27) + ((char)91) + ((char)50) + ((char)74));
                    serialPortKD2.Write("" + ((char)27) + ((char)91) + ((char)49) + ((char)59) + ((char)49) + ((char)72));
                    serialPortKD2.Write(p);
                }
            }
        }
        public void KDikinciSatiraYaz(string p)
        {

            if (serialPortKD2.IsOpen == true)
            {
                if (Program.kasano == 3)
                {
                    serialPortKD2.Write("" + ((char)11) + ((char)10));
                    serialPortKD2.Write(p);
                }
                else
                {
                    serialPortKD2.Write("" + ((char)27) + ((char)91) + ((char)50) + ((char)59) + ((char)49) + ((char)72));
                    serialPortKD2.Write(p);
                }
            }

        }
        private string karakterCevir(string gelenMesaj)
        {
            string mesaj = gelenMesaj;
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş', 'ä', 'Ä' };
            char[] newValue = new char[] { 'o', 'O', 'u', 'U', 'c', 'C', 'I', 'i', 'G', 'g', 'S', 's', 'a', 'A' };
            for (int sayac = 0; sayac < oldValue.Length; sayac++)
            {
                mesaj = mesaj.Replace(oldValue[sayac], newValue[sayac]);
            }
            return mesaj;
        }

      
       

        private void btnSheck_Click(object sender, EventArgs e)
        {
            F_scheckBestatigung best = new F_scheckBestatigung();
            best.ShowDialog();
            if (best.onay == 1)
            {
                if (double.TryParse(txtOdenen.Text, out genericBetrag))
                {
                    if ((Program.GlobalAyarlar["BKOMMA"] == 0))
                    {
                        if (txtOdenen.Text.IndexOf(',') == -1)
                        {
                            genericBetrag = genericBetrag / 100;
                        }
                    }
                    if (genericBetrag > toplamtutar)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "SCHECK BETRAG DARF NICHT GROßER ALS EINKAUFSSUMME!\nBitte den Kunde informieren!!!";
                        frmerror.ShowDialog();
                        return;
                    }
                    teilCek += genericBetrag;

                    if (teilCek > 0)
                    {
                        if (genericBetrag >= toplamtutar)
                        {
                            if (odemeturu != 4)
                            {
                                odemeturu = 3;
                                verilenpara = 0;
                            }
                            else
                            {
                                if (teilBar == 0)
                                    //odemeturu = 3;
                                verilenpara = 0;

                            }
                            //teilCek = toplamtutar;
                            odemeSonuc = true;
                            
                            if (musteriNo != 0)
                            {
                                getKundeProcess();
                            }

                            //serialPortKD2.Close();
                            this.Close();


                        }
                        else
                        {
                            odenenTeilBetrag += genericBetrag;
                            toplamtutar = toplamtutar - genericBetrag;
                            lblTutar.Text = "TOTAL : " + (toplamtutar).ToString("C");
                            lblGutHaben.Visible = true;
                            odemeturu = 4;
                            if (lblGutHaben.Text.Length > 0)
                            {
                                lblGutHaben.Text = lblGutHaben.Text + "\n" + Program.lang["83"] + genericBetrag.ToString("C");
                            }
                            else
                            {
                                lblGutHaben.Text = Program.lang["83"] + genericBetrag.ToString("C");
                            }
                            genericBetrag = 0;
                            odenenpara = 0;
                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["81"];
                        frmerror.ShowDialog();
                    }
                    txtOdenen.Text = "0";
                }
            }
        }

        private void btnTeilZahlung_Click(object sender, EventArgs e)
        {

            try
            {
                bool ecSonuc = false;
               


                //ecSonuc = true;
              //  if (ecSonuc == true)
                //{
                if (verilenpara>0)
                {
                    genericBetrag = verilenpara;
                    /*if ((Program.GlobalAyarlar["BKOMMA"] == 0))
                    {
                        genericBetrag = genericBetrag / 100;
                    }
                    if (genericBetrag >= toplamtutar)
                    {
                        genericBetrag = toplamtutar;
                    }*/
                    //EC PROCESS Sonuc true ise:

                    using (F_EConay frmEcOnay = new F_EConay())
                    {
                        if (genericBetrag <= 0)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "BETRAG IST ZU KLEIN!!!";
                            frmerror.ShowDialog();
                            return;
                        }
                        frmEcOnay.TeilZahlung = 1;
                        frmEcOnay.aktuelFis = aktuelFis;
                        frmEcOnay.toptutar = genericBetrag;

                        frmEcOnay.ShowDialog();
                        if (frmEcOnay.odemesonuc == true)
                        {
                            kryptonButton1.Enabled = false;

                          /*  ecSonuc = true;
                            if (ecSonuc == true)
                            {*/

                            teilEC += frmEcOnay.toptutar;

                            if (genericBetrag >= toplamtutar)
                            {
                                if (odemeturu != 4)
                                {

                                    odemeturu = 0;
                                }
                                odemeSonuc = true;
                                if (musteriNo != 0)
                                {
                                    getKundeProcess();
                                }

                                //serialPortKD2.Close();
                                this.Close();


                            }
                            else
                            {
                                odenenTeilBetrag += genericBetrag;
                                toplamtutar = toplamtutar - genericBetrag;
                                if (toplamtutar <= 0)
                                {
                                    odemeturu = 4;
                                    odemeSonuc = true;
                                    if (musteriNo != 0)
                                    {
                                        getKundeProcess();
                                    }

                                    //serialPortKD2.Close();
                                    this.Close();
                                }
                                else
                                {
                                    odemeturu = 4;
                                    lblTutar.Text = "TOTAL : " + (toplamtutar).ToString("C");
                                    lblGutHaben.Visible = true;
                                    if (lblGutHaben.Text.Length > 0)
                                    {
                                        lblGutHaben.Text = lblGutHaben.Text + "\n" + Program.lang["82"] + genericBetrag.ToString("C");
                                    }
                                    else
                                    {
                                        lblGutHaben.Text = Program.lang["82"] + genericBetrag.ToString("C");
                                    }
                                }
                                genericBetrag = 0;
                            }
                            txtOdenen.Text = "0";
                            odenenpara = 0;
                        }

                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["81"];
                            frmerror.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception er)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = er.Message;
                frmerror.ShowDialog();
            }
        }

        private void btnGeraboPuntzuGeld_Click(object sender, EventArgs e)
        {
            int punkteMenge = 0;
            if (txtOdenen.Text == "0" || txtOdenen.Text == "")
            {
                punkteMenge=0;
            }
            else
            {
                if(Int32.TryParse(txtOdenen.Text,out punkteMenge))
                {
                }
            }
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            if(punkteMenge>0)
            {
                RetunInfo=GeraboIslem.GeraboCashBack(GeraboIslem.Url,gerabOInfo.code,punkteMenge.ToString());
            }
            else
            {
                RetunInfo=GeraboIslem.GeraboCashBack(GeraboIslem.Url,gerabOInfo.code,punkteMenge.ToString());
            }
            if (RetunInfo.meta.code == 3)
            {
                GeraboKundeDetay();
                this.BenimEventim("Pnkt:" + (RetunInfo.points.ToString()), "\n€ :" + (Convert.ToDouble(RetunInfo.credits) / 100).ToString("C"));
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = RetunInfo.meta.message;
                frmerror.ShowDialog();
            }
        }
        public void GeraboKundeDetay()
        {
            gerabOInfo = GeraboIslem.GetKundeInfo(GeraboIslem.Url, gerabOInfo.code);
            //MessageBox.Show("Guthaben:"+Convert.ToDouble(AccountInfo.account.Credits/100)+" Punkte:"+AccountInfo.account.Points);
            btnGeraboPuntzuGeld.Visible = true;
            lblGutHaben.Visible = true;
            lblGutHaben.Text = "Pnkt:" + (gerabOInfo.account.Points.ToString()) + "\n€ :" + (Convert.ToDouble(gerabOInfo.account.Credits) / 100).ToString("C");
            btnEinlosen.Enabled = true;
        }

        private void kryptonButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                a:
                F_ebonCodeEingabe fEbonCodeEingabe = new F_ebonCodeEingabe();
                int num1 = (int)fEbonCodeEingabe.ShowDialog();
                string code = fEbonCodeEingabe.code;
                double num2 = 0.0;
                if (code != "")
                {
                    eBonVerifiedCode = code;
                    ebon_Main ebonMain = new ebon_Main();
                    VerifiedCodeRes res = new VerifiedCodeRes();
                    res = ebonMain.VerifiedCode(code, Program.IsletmeAyarlar["companyID"], Program.IsletmeAyarlar["eBonBearer"]);
                    if (res != null)
                    {
                        if (res.result == true)
                        {
                            if (res.data.paymentType != "1")
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Bitte anhacken Sie Auf dem eBon App : \"Pay On Mobile\" ";
                                frmerror.ShowDialog();
                                goto a;
                            }
                            else
                            {
                                PaymentCreate payCreate = new PaymentCreate();
                                payCreate = ebonMain.CreatePayment(code, toplamtutar.ToString().Replace(',', '.'), Program.IsletmeAyarlar["eBonBearer"]);

                                if (payCreate.result == true)
                                {
                                    //start Payment Form
                                    F_eBonPaymentInfo paymentInfo = new F_eBonPaymentInfo();
                                    paymentInfo.verifiedCode = code;
                                    paymentInfo.payCreate = payCreate;
                                    paymentInfo.ShowDialog();
                                    if (paymentInfo.Zahlungsresult == 1)
                                    {
                                        odemeSonuc = true;
                                        odemeturu = 0;
                                        this.Close();

                                    }

                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "Bitte anhacken Sie Auf dem eBon App : \"Pay On Mobile\" ";
                                    frmerror.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = res.message;
                            frmerror.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ff)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = ff.Message;
                frmerror.ShowDialog();
            }
        }

    }
}
