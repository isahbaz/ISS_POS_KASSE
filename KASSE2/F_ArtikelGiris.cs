using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using System.IO;
using POS.Devices;
using wg;
using System.Globalization;
using System.Diagnostics;


namespace IS_KASSE
{
    public partial class F_ArtikelGiris : Form
    {
        MySqlConnection myConn;
        db baglanti = new db();
        public int grupID = -2;
        VirgulAyikla vA = new VirgulAyikla();
        int barkodgeldi = 1;
        public string barkodno = "";
        string urunad = "";
        string resimyol = "";
        bool hesapislem = false;
        long eskistok = -1;
        OPOSScanner scanner =null;
        OPOSScanner scanner2 = null;
        string stokSQL = "";
        double stok = 0;
        Dictionary<int, string> srvLang = new Dictionary<int, string>();
        public F_ArtikelGiris()
        {
            InitializeComponent();
        }

        private void F_ArtikelGiris_Load(object sender, EventArgs e)
        {
            srvLang.Clear();
            try
            {
                if (Program.oposscanner != null)
                {
                    scanner = Program.oposscanner;
                }
                else if (Program.oposscanner2 != null)
                {
                    scanner2 = Program.oposscanner2;
                }
                else if (serialPort1.IsOpen == false)
                {
                    serialPort1.PortName = Program.SPORT;

                    serialPort1.Open();

                }
            }
            catch (Exception gg)
            {
                MessageBox.Show(gg.Message);
            }
            
            btnResimKaydet.Enabled = false;
            dgvUrun.Columns.Add("bnarkod", "Barcode");
            dgvUrun.Columns.Add("ad", "Artikelname");
            dgvUrun.Columns.Add("AlisFiyat", "EK-Preis");
            dgvUrun.Columns.Add("satisfiyat", "VK-Preis");
            label13.Text = "";
            this.ActiveControl = txtBarkod;
            txtBarkod.Focus();
            cbbleriDoldur();
            
            if((scanner!=null) || (scanner2!=null))
            {
                
                if (scanner != null)
                {
                    scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);
                }
                if (scanner2 != null)
                {
                    scanner2.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
                }
                else if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                }
            }
            try
            {
                if (Program.SPORT != "")
                {
                    serialPort1.PortName = Program.SPORT;

                    if (serialPort1.IsOpen == false)
                    {

                        serialPort1.Open();
                        label13.Text = "";
                    }
                }
            }
            catch
            {
                label13.Text = "SCANNERPORT IST GESCHLOßEN!";
            }
            if (grupID != -2)
            {
                barkodgeldi = 1;
            }
            else
            {
                barkodgeldi = 0;
            }
            this.cbbGrup.SelectedIndexChanged += new System.EventHandler(this.cbbGrup_SelectedIndexChanged);
        }
        void scanner_DataEvent(int Status)
        {
            try
            {
                byte[] bytes = new byte[scanner.ScanData.Length * sizeof(char)];
                System.Buffer.BlockCopy(scanner.ScanData.ToCharArray(), 0, bytes, 0, bytes.Length);
                ASCIIEncoding enco = new ASCIIEncoding();

                //MessageBox.Show("datageldi" + scanner.ScanDataLabel + "scandata: " + scanner.ScanData + "status: " + Status + "decode :" + enco.GetString(bytes));

                scanner.DataEventEnabled = true;
                barkodno = scanner.ScanDataLabel + "";
                if (scanner.ScanDataType == 0)// barcode type unkonown 
                {
                    barkodno = barkodno.Remove(0, 1);
                    barkodno = barkodno.Replace("\r", "");

                }
                else if (scanner.ScanDataType == 107)
                {
                    barkodno = barkodno.Substring(1, barkodno.Length - 1);
                }
                else if (scanner.ScanDataType == 501)
                {
                    barkodno = scanner.ScanData;
                }
                txtLeriBosalt();
                aramayap("barcode",barkodno);
            }
            catch
            {
            }
        }
        void scanner2_DataEvent(int Status)
        {
            if (Program.IsletmeAyarlar["kod"] == "90")
            {
                Console.Beep(1500, 200);
            }
            try
            {
                if (scanner2.ScanData.Length > 0)
                {
                    byte[] bytes = new byte[scanner2.ScanData.Length * sizeof(char)];
                    System.Buffer.BlockCopy(scanner2.ScanData.ToCharArray(), 0, bytes, 0, bytes.Length);
                    ASCIIEncoding enco = new ASCIIEncoding();
                    // MessageBox.Show("datageldi" + scanner2.ScanDataLabel + "scandata: " + scanner2.ScanData + "status: " + Status + "decode :" + enco.GetString(bytes)+" type:"+scanner2.ScanDataType);
                    //MessageBox.Show(scanner2.ScanDataType.ToString());

                    barkodno = scanner2.ScanData + "";
                    if (scanner2.ScanDataType == 0)// barcode type unkonown 
                    {
                        //barkodno = barkodno.Remove(0, 1);
                        barkodno = barkodno.Replace("\r", "");

                    }
                    else if (scanner2.ScanDataType == 107)
                    {
                        barkodno = barkodno.Substring(1, barkodno.Length - 1);
                    }
                    else if (scanner2.ScanDataType == 104)
                    {
                        barkodno = barkodno.Substring(1, barkodno.Length - 1);
                    }

                    //MessageBox.Show(barkodno);
                    //barkodluUrunEkle();
                    if( Program.scannerSO2 == "voyager")
                    {
                    barkodno =barkodno.Substring(4,(barkodno.Length-4));
                    }
                    aramayap("barcode", barkodno);
                }
            }
            catch
            {
            }
            finally
            {
                scanner2.DataEventEnabled = true;
            }
        }
        private void cbbleriDoldur()
        {
            myConn = new MySqlConnection();

            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            MySqlDataAdapter daGrup = new MySqlDataAdapter("SELECT * from artikelgrup WHERE aktif=1", myConn);
            DataTable dtGrup = new DataTable("artikelgrup");
            dtGrup.Rows.Clear();
            daGrup.Fill(dtGrup);
            BindingSource bsGrup = new BindingSource();
            bsGrup.DataSource = dtGrup;
            cbbGrup.DataSource = bsGrup;
            cbbGrup.DisplayMember = "grupad";
            cbbGrup.ValueMember = "grupid";

            MySqlDataAdapter daMarka = new MySqlDataAdapter("SELECT * from firma", myConn);
            DataTable dtMarka = new DataTable("firma");
            dtMarka.Rows.Clear();
            daMarka.Fill(dtMarka);
            //cbbMarka.DataSource = null;
            //cbbMarka.Items.Clear();
            BindingSource bsMarka = new BindingSource();
            bsMarka.DataSource = dtMarka;
            cbbMarka.DataSource = bsMarka;


            MySqlDataAdapter daLieferant = new MySqlDataAdapter("SELECT * from lieferant", myConn);
            DataTable dtLieferant = new DataTable("lieferant");
            dtLieferant.Rows.Clear();
            daLieferant.Fill(dtLieferant);
            BindingSource bsLieferant = new BindingSource();
            bsLieferant.DataSource = dtLieferant;
            cbbLieferant.DataSource = bsLieferant;


            MySqlDataAdapter daBirimler = new MySqlDataAdapter("SELECT * from birimler", myConn);
            DataTable dtBirimler = new DataTable("Birimler");
            dtBirimler.Rows.Clear();
            daBirimler.Fill(dtBirimler);
            BindingSource bsBirimler = new BindingSource();
            bsBirimler.DataSource = dtBirimler;
            cbbBirim.DataSource = bsBirimler;

            MySqlDataAdapter daPfand = new MySqlDataAdapter("SELECT * from pfand ORDER BY artikelno", myConn);
            DataTable dtPfand = new DataTable("pfand");
            dtPfand.Rows.Clear();
            daPfand.Fill(dtPfand);
            //cbbMarka.DataSource = null;
            //cbbMarka.Items.Clear();
            BindingSource bsPfand = new BindingSource();
            bsPfand.DataSource = dtPfand;
            cbbPfand.DataSource = bsPfand;

            if (grupID != -2)
            {
                MySqlDataAdapter daUpdate = new MySqlDataAdapter("SELECT * from artikel WHERE artikelid=" + grupID, myConn);
                DataTable dtUpdate = new DataTable("artikel");
                dtUpdate.Rows.Clear();
                daUpdate.Fill(dtUpdate);
                BindingSource bsUpdate = new BindingSource();
                bsUpdate.DataSource = dtUpdate;
                txtAd.DataBindings.Add("text", bsUpdate, "artikelad");
                txtAd2.DataBindings.Add("text", bsUpdate, "artikelad2");
                txtBarkod.DataBindings.Add("text", bsUpdate, "barkod");
                //MessageBox.Show(new ArtikelGrup((int)dtUpdate.Rows[0].ItemArray[3]).Grupno.ToString());
                cbbGrup.SelectedValue = new ArtikelGrup((int)dtUpdate.Rows[0].ItemArray[3]).Grupno;
                cbbMarka.SelectedValue = (int)dtUpdate.Rows[0].ItemArray[5];
                cbbLieferant.SelectedValue = (int)dtUpdate.Rows[0].ItemArray[4];
                txtAlisfiyat.DataBindings.Add("text", bsUpdate, "alisfiyat");
                txtSatisfiyat.DataBindings.Add("text", bsUpdate, "satisfiyat");
                txtKarE.DataBindings.Add("text", bsUpdate, "karmiktari");
                txtKar.Text = (Convert.ToDouble(txtKarE.Text) / Convert.ToDouble(txtAlisfiyat.Text)).ToString();
                cbbBirim.SelectedValue = new Birimler((int)dtUpdate.Rows[0].ItemArray[9]).Birimid;
                txtStok.DataBindings.Add("text", bsUpdate, "toplamstok");
                txtAgirlik.DataBindings.Add("text", bsUpdate, "agirlik");
                cbbRaf.SelectedItem = dtUpdate.Rows[0].ItemArray[25].ToString();
                cbbSira.SelectedItem = dtUpdate.Rows[0].ItemArray[26].ToString();
                cbbPfand.SelectedItem = dtUpdate.Rows[0].ItemArray[12].ToString();

                if ((int)dtUpdate.Rows[0].ItemArray[12] == 1)
                {
                    cbDurum.Checked = true;
                }


                if (dtUpdate.Rows[0].ItemArray[23].ToString() != "")
                {
                    try
                    {
                        pbBarkod.BackgroundImage = Image.FromFile(dtUpdate.Rows[0].ItemArray[23].ToString());
                    }
                    catch { }
                }
                if (dtUpdate.Rows[0].ItemArray[19].ToString() != "")
                {
                    try
                    {
                        pbResim.BackgroundImage = Image.FromFile(dtUpdate.Rows[0].ItemArray[19].ToString());
                    }
                    catch { }
                }
                //if((int)dtUpdate.Rows[0].ItemArray[];
            }
        }
        private void txtSatisfiyat_Enter(object sender, EventArgs e)
        {
            txtKar.Enabled = false;
            txtKarE.Enabled = false;
            cbbFiyatDuz.Enabled = false;
            lblFiyatDuz.Enabled = false;
        }

        private void txtSatisfiyat_Leave(object sender, EventArgs e)
        {
            if (txtSatisfiyat.Text == "")
            {
                txtKar.Enabled = true;
                txtKarE.Enabled = true;
                cbbFiyatDuz.Enabled = true;
                lblFiyatDuz.Enabled = true;
            }
            else
            {
                KarHesapla();
            }
        }

        private void KarHesapla()
        {
            if (hesapislem == false)
            {
                double alisfiyat;
                double satisfiyat;
                if (double.TryParse(txtAlisfiyat.Text, out alisfiyat))
                {
                    if (double.TryParse(txtSatisfiyat.Text, out satisfiyat))
                    {
                        double mwst = new ArtikelGrup((int)cbbGrup.SelectedValue).Mwst;
                        double kar =Math.Round((Convert.ToDouble(txtSatisfiyat.Text) ) - (alisfiyat * (1 + mwst / 100)),2);
                        txtKarE.Text = kar.ToString();
                        txtKar.Text = Math.Round((kar / (alisfiyat * (1 + mwst / 100))) * 100).ToString();
                    }
                }
                else
                {
                    txtKar.Text = "";
                    txtKarE.Text = "";

                }
            }
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (grupID != -2)
            {
                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();

                }

                string DelSql = "DELETE FROM artikel Where artikelid=" + grupID;
                MySqlCommand coDel = new MySqlCommand(DelSql, myConn);
                if (coDel.ExecuteNonQuery() > 0)
                {
                    if(Program.IsletmeAyarlar["land"]=="de")
                    label13.Text ="Artikel wurde gelöscht!";
                    else
                        label13.Text = "Product Deleted!";
                    grupID = -2;
                    txtLeriBosalt();
                }
                else
                {
                  label13.Text ="FEHLER!";
                }

            }
            myConn.Close();
        }

        private void cbbGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (new ArtikelGrup((int)cbbGrup.SelectedValue).GrupTur == 3)
            {
                txtTara.Enabled = true;
            }
            else
            {
                txtTara.Enabled = false;
                txtTara.Text = "0";
            }
            KarHesapla();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            this.Close();
        }

        private void txtSatisfiyat_TextChanged(object sender, EventArgs e)
        {
            double satisfiyat;
            if (double.TryParse(txtSatisfiyat.Text, out satisfiyat))
            {
                KarHesapla();
            }
        }

        private void txtAlisfiyat_TextChanged(object sender, EventArgs e)
        {
            KarHesapla();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            double birimfiyat = 0;
            int fand = 0;
            int fand2 = 0;
            int fand3 = 0;
            int durum = 0;
            int sira = 0;
            int raf = 0;
         
            double tara;

            
            myConn = baglanti.myconn();
            if (txtAd2.Text == "") txtAd2.Text = "-";
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            double satisfiyat;
            double agirlik;
            double  alisfiyat;
            if (double.TryParse(txtAlisfiyat.Text, out alisfiyat))
            {

            }
            else
            {
                satisfiyat = 0;
                satisfiyat = 0;
            }
            if (txtArtikelID.Text == "")
            {
                txtArtikelID.Text = "0";
            }
            if (double.TryParse(txtSatisfiyat.Text, out satisfiyat))
            {
                if (double.TryParse(txtAgirlik.Text, out agirlik))
                {
                    if (agirlik != 0)
                    {
                        if (agirlik > 100)
                        {


                            birimfiyat = satisfiyat * 1000 / agirlik;
                        }
                        else
                        {
                            birimfiyat = satisfiyat * 100 / agirlik;
                        }
                        label13.Text = "";
                    }
                }
                else
                {
                    label13.Text =Program.lang["24"]+"\n ERROR:WEIGHT";
                    return;
                }

                if (cbDurum.Checked == true)
                {
                    durum = 1;
                }
                /*FAND2(0,15)*/

                if (cbbSira.SelectedIndex != -1)
                {
                    sira = Convert.ToInt16(cbbRaf.SelectedItem);
                }
                if (cbbRaf.SelectedIndex != -1)
                {
                    raf = Convert.ToInt16(cbbRaf.SelectedItem);
                }

            }
            else
            {

            }
           if (stokSQL == "")
            {
                stokSQL = txtStok.Text;
            }
             /*else
            {
               stokSQL = "toplamstok+" + vA.virgulayikla(stok);
            } */
            
            if (double.TryParse(txtTara.Text, out tara))
            {
                if (tara < 0)
                {
                    tara = 0;
                }

            }
            else
            {
                tara = 0;
            }
            if ((grupID != -2) && (KayitKontrol() == true))
            {
                string comTXT = "";
                double sonstok = 0;
                
                double mwst = new ArtikelGrup((int)cbbGrup.SelectedValue).Mwst;
                //`artikelid``barkod``artikelad``grupid``liferantid``firmaid``mwst``alisfiyat``satisfiyat``birimid``karmiktari`
                //`toplamstok``artikeltur``angebotdurum``angebotbaslamatarih``angebotbitistarih``angebotfiyat`
                string UpdateSql = "UPDATE artikel SET barkod='" + txtBarkod.Text + "', artikelad='" + txtAd.Text + "', artikelad2='" + txtAd2.Text + "', grupid=" + cbbGrup.SelectedValue + ", liferantid=" + cbbLieferant.SelectedValue + ",firmaid=" + cbbMarka.SelectedValue +
                ", mwst=" + mwst + ", alisfiyat=" + vA.virgulayikla(Convert.ToDouble(txtAlisfiyat.Text)) + ", satisfiyat=" + vA.virgulayikla(Convert.ToDouble(txtSatisfiyat.Text)) + ", artikeltur=" + durum + ", birimid= " + cbbBirim.SelectedValue +
                ", karmiktari=" + vA.virgulayikla(Convert.ToDouble(txtKarE.Text)) + ", toplamstok= " + stokSQL + ", agirlik= " + txtAgirlik.Text + ", birimfiyat=" + vA.virgulayikla(birimfiyat) + ",tara="+tara+", fand=" + cbbPfand.SelectedValue + ",regal=" + raf + ",sira=" + sira + ", fand2=" + fand2 +
                " WHERE artikelid=" + grupID;
                MySqlCommand coUpdate = new MySqlCommand(UpdateSql, myConn);
                try
                {
                    if (coUpdate.ExecuteNonQuery() > 0)
                    {
                        if (Program.IsletmeAyarlar["land"] == "de")
                        label13.Text ="Aktualisierung OK!";
                        else
                            label13.Text = "UPDATE OK!";
                        if (rbInventur.Checked == true) //Inventur mu?? evet
                        {
                            sonstok = stok;

                            comTXT = "CALL SiparisHareket(" + grupID + "," + sonstok + ", @satisfiyat, @alisfiyat," + 0 + ",1)";
                            MySqlCommand coSayim = new MySqlCommand();
                            coSayim.Parameters.AddWithValue("@alisfiyat", alisfiyat);
                            coSayim.Parameters.AddWithValue("@satisfiyat", satisfiyat);
                            coSayim.Parameters.AddWithValue("@stk", stok);
                            coSayim.CommandText = "CALL SayimHareket(" + grupID + ",@stk, @satisfiyat, @alisfiyat," + cbbLieferant.SelectedValue + ")";
                            coSayim.Connection = myConn;
                            coSayim.ExecuteNonQuery();

                        }
                        else //Inventur mu?? hayır
                        {
                            if (eskistok == 0)
                            {
                                sonstok = stok;
                                comTXT = "CALL SiparisHareket(" + grupID + "," + vA.virgulayikla(sonstok) + ", @satisfiyat, @alisfiyat," + cbbLieferant.SelectedValue + ",1)";
                            }
                            else if (eskistok != -1 && eskistok != stok)
                            {
                                sonstok = stok;
                                comTXT = "CALL SiparisHareket(" + grupID + "," + vA.virgulayikla(sonstok) + ", @satisfiyat, @alisfiyat," + cbbLieferant.SelectedValue + ",0)";
                            }

                        }
                        if (comTXT != "")
                        {
                            try
                            {
                                MySqlCommand coSiparisTakip = new MySqlCommand();
                                coSiparisTakip.Parameters.AddWithValue("@alisfiyat", alisfiyat);
                                coSiparisTakip.Parameters.AddWithValue("@satisfiyat", satisfiyat);
                                coSiparisTakip.CommandText = comTXT;
                                coSiparisTakip.Connection = myConn;
                                coSiparisTakip.ExecuteNonQuery();
                            }
                            catch (Exception ee)
                            {
                                label13.Text = ee.Message;
                            }

                        }
                        grupID = -2;
                        barkodno = "";
                        txtLeriBosalt();

                    }
                    //label13.Text = "OK!";
                }
                catch(Exception ss)
                {
                    label13.Text ="ERROR! "+ss.Message;
                }

            }
            else if ((grupID == -2) && (KayitKontrol() == true))
            {
                double mwst = new ArtikelGrup((int)cbbGrup.SelectedValue).Mwst;
                //`artikelid``barkod``artikelad``grupid``liferantid``firmaid``mwst``alisfiyat``satisfiyat``birimid``karmiktari`
                //`toplamstok``artikeltur``angebotdurum``angebotbaslamatarih``angebotbitistarih``angebotfiyat`
                string InsertSql = "INSERT INTO artikel SET barkod=" + txtBarkod.Text + ", artikelad='" + txtAd.Text + "', artikelad2='" + txtAd2.Text + "', grupid=" + cbbGrup.SelectedValue + ", liferantid=" + cbbLieferant.SelectedValue + ",firmaid=" + cbbMarka.SelectedValue +
                ", mwst=" + mwst + ", alisfiyat=" + vA.virgulayikla(Convert.ToDouble(txtAlisfiyat.Text)) + ", satisfiyat=" + vA.virgulayikla(Convert.ToDouble(txtSatisfiyat.Text)) + ", artikeltur=" + durum + ", birimid= " + cbbBirim.SelectedValue + ", karmiktari=" + vA.virgulayikla(Convert.ToDouble(txtKarE.Text)) +
                ", toplamstok= " + stokSQL + ", agirlik= " + txtAgirlik.Text + ", birimfiyat=" + vA.virgulayikla(birimfiyat) + ", fand=" + cbbPfand.SelectedValue + ",tara=" + tara + ",regal=" + raf + ",sira=" + sira + ", fand2=" + fand2;

                MySqlCommand coUpdate = new MySqlCommand(InsertSql, myConn);
                try
                {
                    if (coUpdate.ExecuteNonQuery() > 0)
                    {

                        long id = coUpdate.LastInsertedId;
                        grupID =Convert.ToInt32(id);
                        if (Program.IsletmeAyarlar["land"] == "de")
                            label13.Text = "Produkt wurde hinzugefügt!";
                        else
                        label13.Text ="Product ist added!";
                        MySqlCommand coReyonetiket = new MySqlCommand();
                        coReyonetiket.Parameters.AddWithValue("@alisfiyat", alisfiyat);
                        coReyonetiket.Parameters.AddWithValue("@satisfiyat", satisfiyat);
                        coReyonetiket.CommandText = "CALL SayimHareket(" + id + "," + stok + ", @satisfiyat, @alisfiyat," + cbbLieferant.SelectedValue + ")";
                        coReyonetiket.Connection = myConn;
                        coReyonetiket.ExecuteNonQuery();

                        /**Siparis hareketleri*/
                        MySqlCommand coSiparisTakip = new MySqlCommand();
                        coSiparisTakip.Parameters.AddWithValue("@alisfiyat", alisfiyat);
                        coSiparisTakip.Parameters.AddWithValue("@satisfiyat", satisfiyat);
                        coSiparisTakip.CommandText = "CALL SiparisHareket(" + id + "," + stok + ", @satisfiyat, @alisfiyat," + cbbLieferant.SelectedValue + ",1)";
                        coSiparisTakip.Connection = myConn;
                        coSiparisTakip.ExecuteNonQuery();
                        txtLeriBosalt();
                        //label13.Text = "";
                    }
                    barkodno = "";
                    grupID = -2;  
                }
                catch(Exception ww)
                {
                   label13.Text="ERROR! "+ww.Message;
                }
               
            }
            else
            {
                if (Program.IsletmeAyarlar["land"] == "de")
                    label13.Text = "Bitte prüfen Sie Ihre Angabe noch einmal!";
                else
                    label13.Text = "Please, Check your Entry!";
            }
            
        }
        private bool KayitKontrol()
        {
            bool sonuc = false;
            foreach (Control cnt in panel1.Controls)
            {
                if (cnt is TextBox)
                {
                    if (cnt.Text == "")
                    {
                        sonuc = false;
                        break;

                    }
                    else
                    {
                        sonuc = true;
                    }
                }
                else if (cnt is ComboBox)
                {
                    if (cnt.Text == "" )
                    {
                        sonuc = false;
                        break;
                    }
                }
                else if (cnt is ComponentFactory.Krypton.Toolkit.KryptonComboBox)
                {
                    if (cnt.Text == "" )
                    {
                        sonuc = false;
                        break;
                    }
                }

            }
            return sonuc;
        }
        private void txtLeriBosalt()
        {
            foreach (Control cnt in panel1.Controls)
            {
                if (cnt is TextBox)
                {
                    cnt.Text = "";

                }

            }
            cbbPfand.SelectedIndex = 0;
            txtTara.Text = "0";
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            /*  F_Firma frmFirma = new F_Firma();
              frmFirma.ShowDialog();
              txtBarkod.Focus();
              cbbDoldur();*/
        }

        private void cbbDoldur()
        {
            myConn = new MySqlConnection();

            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            MySqlDataAdapter daGrup = new MySqlDataAdapter("SELECT * from artikelgrup", myConn);
            DataTable dtGrup = new DataTable("artikelgrup");
            dtGrup.Rows.Clear();
            daGrup.Fill(dtGrup);
            BindingSource bsGrup = new BindingSource();
            bsGrup.DataSource = dtGrup;
            cbbGrup.DataSource = bsGrup;
            cbbGrup.DisplayMember = "grupad";
            cbbGrup.ValueMember = "grupid";

            MySqlDataAdapter daMarka = new MySqlDataAdapter("SELECT * from firma", myConn);
            DataTable dtMarka = new DataTable("firma");
            dtMarka.Rows.Clear();
            daMarka.Fill(dtMarka);
            //cbbMarka.DataSource = null;
            //cbbMarka.Items.Clear();
            BindingSource bsMarka = new BindingSource();
            bsMarka.DataSource = dtMarka;
            cbbMarka.DataSource = bsMarka;


            MySqlDataAdapter daLieferant = new MySqlDataAdapter("SELECT * from lieferant", myConn);
            DataTable dtLieferant = new DataTable("lieferant");
            dtLieferant.Rows.Clear();
            daLieferant.Fill(dtLieferant);
            BindingSource bsLieferant = new BindingSource();
            bsLieferant.DataSource = dtLieferant;
            cbbLieferant.DataSource = bsLieferant;


            MySqlDataAdapter daBirimler = new MySqlDataAdapter("SELECT * from birimler", myConn);
            DataTable dtBirimler = new DataTable("Birimler");
            dtBirimler.Rows.Clear();
            daBirimler.Fill(dtBirimler);
            BindingSource bsBirimler = new BindingSource();
            bsBirimler.DataSource = dtBirimler;
            cbbBirim.DataSource = bsBirimler;

            MySqlDataAdapter daPfand = new MySqlDataAdapter("SELECT * from pfand ORDER BY pfandid", myConn);
            DataTable dtPfand = new DataTable("pfand");
            dtPfand.Rows.Clear();
            daPfand.Fill(dtPfand);
            //cbbMarka.DataSource = null;
            //cbbMarka.Items.Clear();
            BindingSource bsPfand = new BindingSource();
            bsPfand.DataSource = dtPfand;
            cbbPfand.DataSource = bsPfand;


        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            /*  F_Lieferant frmLief = new F_Lieferant();
              frmLief.ShowDialog();
              txtBarkod.Focus();
              cbbDoldur();*/
        }

        private void F_YeniArtikel_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void kryptonButton4_KeyDown(object sender, KeyEventArgs e)
        {
            /* if (e.KeyValue == 16 && e.KeyValue == 17 && e.KeyValue == 20)
             {
                 MessageBox.Show(e.KeyValue.ToString());
             }*/
            if ((e.Control & e.Shift))
            {
                /* FAyar frmAyar = new FAyar();
                 frmAyar.ShowDialog();*/
            }
        }

        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                barkodno = txtBarkod.Text;
                aramayap("barcode", barkodno); ; //GridiDoldur(); // Gridi doldurmak için yazdığın metod

                // 
            }
        }

        private void aramayap(string alan, string value)
        {
            stok = 0;
            stokSQL = "";
            label13.Text = "";
            //txtLeriBosalt();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
            string searchSql="";
            if (alan == "barcode")
            {
                searchSql = "SELECT * from artikel where barkod = " + value ;
            }
            else
            {
                searchSql = "SELECT * from artikel where artikelid = " + value ;
            }
            MySqlDataAdapter myDaGrup = new MySqlDataAdapter(searchSql, myConn);
            DataTable dtGrup = new DataTable("lieferant");
            dtGrup.Clear();
            myDaGrup.Fill(dtGrup);
            int kaySay = dtGrup.Rows.Count;
            if (kaySay > 0)
            {
                //MessageBox.Show("Bu Ürün Zaten Daha Önceden Girilmiş!\nÜrün Adı:" + dtGrup.Rows[0].ItemArray[2].ToString());
                //txtBarkod.Text = "";
                
                grupID = Convert.ToInt32(dtGrup.Rows[0].ItemArray[0]);
                if (alan == "barcode")
                {
                    txtBarkod.Text = value;
                }
                else
                {
                    txtBarkod.Text = dtGrup.Rows[0].ItemArray[1].ToString();
                }
                txtArtikelID.Text = dtGrup.Rows[0].ItemArray[0].ToString();
                eskistok = Convert.ToInt32(dtGrup.Rows[0].ItemArray[11]);
                txtAd.Text = dtGrup.Rows[0].ItemArray[2].ToString();
                txtAd2.Text = dtGrup.Rows[0].ItemArray[29].ToString();
                cbbGrup.SelectedValue = (int)dtGrup.Rows[0].ItemArray[3];
                cbbMarka.SelectedValue = (int)dtGrup.Rows[0].ItemArray[5];
                cbbLieferant.SelectedValue = (int)dtGrup.Rows[0].ItemArray[4];
                txtAlisfiyat.Text = dtGrup.Rows[0].ItemArray[7].ToString();
                txtSatisfiyat.Text = dtGrup.Rows[0].ItemArray[8].ToString();
                txtKarE.Text = dtGrup.Rows[0].ItemArray[10].ToString();
                txtKar.Text = Math.Round((((double)dtGrup.Rows[0].ItemArray[10] / (double)dtGrup.Rows[0].ItemArray[7]) * 100), 2).ToString();
                cbbBirim.SelectedValue = (int)dtGrup.Rows[0].ItemArray[9];
                txtStok.Text = dtGrup.Rows[0].ItemArray[11].ToString();
                txtAgirlik.Text = dtGrup.Rows[0].ItemArray[17].ToString();
                cbbRaf.SelectedItem = dtGrup.Rows[0].ItemArray[25].ToString();
                cbbSira.SelectedItem = dtGrup.Rows[0].ItemArray[26].ToString();
                cbbPfand.SelectedValue = (int)dtGrup.Rows[0].ItemArray[22];
                txtTara.Text = dtGrup.Rows[0].ItemArray[31].ToString();
                if ((int)dtGrup.Rows[0].ItemArray[12] == 1)
                {
                    cbDurum.Checked = true;

                }
                else
                {
                    cbDurum.Checked = false;

                }
                txtBarkod.Focus();

            }
            else
            {
                txtLeriBosalt();
                if(Program.IsletmeAyarlar["land"]=="de")
               label13.Text="Artikel konnte nicht gefunden werden!";
                else
                    label13.Text = "Product not found!";
                txtBarkod.Text = barkodno;
                grupID = -2;
                barkodno = "";
            }



            myConn.Close();
            barkodgeldi = 0;
        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            urunad = txtAd.Text;
            if (txtAd.Text != "")
            {
                if (barkodgeldi != 1)
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    try
                    {
                        MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikel where artikelad like '" + urunad + "%'", myConn);
                        DataTable dtGrup = new DataTable("lieferant");
                        dtGrup.Clear();
                        dgvUrun.Rows.Clear();
                        myDaGrup.Fill(dtGrup);
                        dgvUrun.Columns[1].Width = 200;
                        int kaySay = dtGrup.Rows.Count;
                        if (kaySay > 0)
                        {
                            //MessageBox.Show("Bu Ürün Zaten Daha Önceden Girilmiş!\nÜrün Adı:" + dtGrup.Rows[0].ItemArray[2].ToString());
                            //txtBarkod.Text = "";

                            //grupID = Convert.ToInt16(dtGrup.Rows[0].ItemArray[0]);
                            for (int i = 0; i < kaySay; i++)
                            {
                                dgvUrun.Rows.Add();
                                dgvUrun.Rows[i].Tag = dtGrup.Rows[i].ItemArray[0];
                                dgvUrun.Rows[i].Cells[0].Value = dtGrup.Rows[i].ItemArray[1].ToString();
                                dgvUrun.Rows[i].Cells[1].Value = dtGrup.Rows[i].ItemArray[2].ToString();
                                //cbbGrup.SelectedValue = (int)dtGrup.Rows[0].ItemArray[3];
                                //cbbMarka.SelectedValue = (int)dtGrup.Rows[0].ItemArray[5];
                                //cbbLieferant.SelectedValue = (int)dtGrup.Rows[0].ItemArray[4];
                                dgvUrun.Rows[i].Cells[2].Value = dtGrup.Rows[i].ItemArray[7].ToString();
                                dgvUrun.Rows[i].Cells[3].Value = dtGrup.Rows[i].ItemArray[8].ToString();
                                // txtKarE.Text = dtGrup.Rows[0].ItemArray[10].ToString();
                                //txtKar.Text = (((double)dtGrup.Rows[0].ItemArray[10] / (double)dtGrup.Rows[0].ItemArray[7]) * 100).ToString();
                                //cbbBirim.SelectedValue = (int)dtGrup.Rows[0].ItemArray[9];
                                //txtStok.Text = dtGrup.Rows[0].ItemArray[11].ToString();
                                //txtAgirlik.Text = dtGrup.Rows[0].ItemArray[17].ToString();
                            }
                            //txtBarkod.Focus();
                        }
                    }
                    catch (Exception dd)
                    {
                        MessageBox.Show(dd.Message);
                    }

                    myConn.Close();
                }
            }
        }

        private void dgvUrun_DoubleClick(object sender, EventArgs e)
        {
            barkodgeldi = 0;
            urunad = dgvUrun.CurrentRow.Cells[1].Value.ToString();
            txtBarkod.Text = dgvUrun.CurrentRow.Cells[0].Value.ToString();
            barkodno = dgvUrun.CurrentRow.Cells[0].Value.ToString();
            // SendKeys.Send("{ENTER}");
            aramayap("barcode", barkodno); 

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            barkodgeldi = 1;
            CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(100);
            label13.Text = "";
            //MessageBox.Show("veri geldi: " + gelenBarkod);
            int byteCount = serialPort1.BytesToRead;
            byte[] dataBuffer = new byte[byteCount];
            serialPort1.Read(dataBuffer, 0, byteCount);
            /***  gelenBarkod = serialPort1.ReadExisting();***/
            /* MessageBox.Show(Encoding.ASCII.GetString(dataBuffer));*/
            List<int> test = new List<int>();
            // bool brkd = false;
            for (int a = 0; a < dataBuffer.Count<byte>(); a++)
            {
                if ((dataBuffer[a] == 83) && (dataBuffer.Count<byte>() != 17))
                {
                    a = 6;
                }
                else if ((dataBuffer[a] == 83) && (dataBuffer.Count<byte>() == 17))
                {
                    a = 3;
                }
                else
                {
                    if (dataBuffer[a] > 47 && dataBuffer[a] < 58)
                    {
                        test.Add(dataBuffer[a]);
                    }
                }
            }
            byte[] temizlenmis = new byte[test.Count];
            int i = 0;
            foreach (int item in test)
            {

                temizlenmis[i] = Convert.ToByte(item);
                i++;
            }

            barkodno = Encoding.ASCII.GetString(temizlenmis);
            //barkodno = barkodno.Substring(0, barkodno.Length - 1);
            txtLeriBosalt();
            aramayap("barcode", barkodno); ;
           /* if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }

            MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikel where barkod like '" + barkodno + "%'", myConn);
            DataTable dtGrup = new DataTable("lieferant");
            dtGrup.Clear();
            myDaGrup.Fill(dtGrup);
            int kaySay = dtGrup.Rows.Count;
            if (kaySay > 0)
            {
                //MessageBox.Show("Bu Ürün Zaten Daha Önceden Girilmiş!\nÜrün Adı:" + dtGrup.Rows[0].ItemArray[2].ToString());
                //txtBarkod.Text = "";
                txtBarkod.Focus();
                grupID = Convert.ToInt16(dtGrup.Rows[0].ItemArray[0]);

                txtBarkod.Text = barkodno;
                txtAd.Text = dtGrup.Rows[0].ItemArray[2].ToString();
                cbbGrup.SelectedValue = (int)dtGrup.Rows[0].ItemArray[3];
                cbbMarka.SelectedValue = (int)dtGrup.Rows[0].ItemArray[5];
                cbbLieferant.SelectedValue = (int)dtGrup.Rows[0].ItemArray[4];
                txtAlisfiyat.Text = dtGrup.Rows[0].ItemArray[7].ToString();
                txtSatisfiyat.Text = dtGrup.Rows[0].ItemArray[8].ToString();
                txtKarE.Text = dtGrup.Rows[0].ItemArray[10].ToString();
                txtKar.Text = Math.Round((((double)dtGrup.Rows[0].ItemArray[10] / (double)dtGrup.Rows[0].ItemArray[7]) * 100), 2).ToString();
                cbbBirim.SelectedValue = (int)dtGrup.Rows[0].ItemArray[9];
                txtStok.Text = dtGrup.Rows[0].ItemArray[11].ToString();
                txtAgirlik.Text = dtGrup.Rows[0].ItemArray[17].ToString();
                cbbRaf.SelectedItem = dtGrup.Rows[0].ItemArray[25].ToString();
                cbbSira.SelectedItem = dtGrup.Rows[0].ItemArray[26].ToString();
                cbbPfand.SelectedValue = (int)dtGrup.Rows[0].ItemArray[22];



            }
            else
            {
                grupID = -2;
                txtBarkod.Text = barkodno;
                label13.Text ="Artikel konnte nicht gefunden werden!";
                //txtLeriBosalt();
            }

            */
            myConn.Close();
            barkodgeldi = 0;


            //txtBarkod.Text = serialPort1.ReadExisting();
            //aramayap();

        }



        public void brkoku()
        {
            barkodgeldi = 1;
            txtBarkod.Text = serialPort1.ReadExisting();
            /*
            //aramayap();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }

            MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikel where barkod like '" + txtBarkod.Text + "%'", myConn);
            DataTable dtGrup = new DataTable("lieferant");
            dtGrup.Clear();
            myDaGrup.Fill(dtGrup);
            int kaySay = dtGrup.Rows.Count;
            if (kaySay > 0)
            {
                //MessageBox.Show("Bu Ürün Zaten Daha Önceden Girilmiş!\nÜrün Adı:" + dtGrup.Rows[0].ItemArray[2].ToString());
                //txtBarkod.Text = "";
                txtBarkod.Focus();
                grupID = Convert.ToInt16(dtGrup.Rows[0].ItemArray[0]);


                txtAd.Text = dtGrup.Rows[0].ItemArray[2].ToString();
                cbbGrup.SelectedValue = (int)dtGrup.Rows[0].ItemArray[3];
                cbbMarka.SelectedValue = (int)dtGrup.Rows[0].ItemArray[5];
                cbbLieferant.SelectedValue = (int)dtGrup.Rows[0].ItemArray[4];
                txtAlisfiyat.Text = dtGrup.Rows[0].ItemArray[7].ToString();
                txtSatisfiyat.Text = dtGrup.Rows[0].ItemArray[8].ToString();
                txtKarE.Text = dtGrup.Rows[0].ItemArray[10].ToString();
                txtKar.Text = (((double)dtGrup.Rows[0].ItemArray[10] / (double)dtGrup.Rows[0].ItemArray[7]) * 100).ToString();
                cbbBirim.SelectedValue = (int)dtGrup.Rows[0].ItemArray[9];
                txtStok.Text = dtGrup.Rows[0].ItemArray[11].ToString();
                txtAgirlik.Text = dtGrup.Rows[0].ItemArray[17].ToString();

                if ((int)dtGrup.Rows[0].ItemArray[22] == 1)
                {
                    CBfand.Checked = true;

                }
                else
                {
                    CBfand.Checked = false;

                }

            }


            barkodgeldi = 0;
            myConn.Close();
            */
        }

        private void F_YeniArtikel_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (scanner != null)
                {
                    scanner.DataEvent -= new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);
                    scanner = null;
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
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            grupID = -2;
            //txtBarkod.Text = barkodno;
            //MessageBox.Show("URUN BULUNAMADI");
            txtLeriBosalt();
            barkodno = "";
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = ".jpg|*.jpg|.png|*.png|.bmp|*.bmp|.gif|*.gif";
            openFileDialog2.Title = "Urun resmi seçiniz";
            openFileDialog2.FileName = "";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                pbResim.Image = Image.FromFile(openFileDialog2.FileName);
                btnResimKaydet.Enabled = true;
                resimyol = "images/" + openFileDialog2.SafeFileName;
            }
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            string apppath = Application.StartupPath;
            if (Directory.Exists("images") == false)
            {
                Directory.CreateDirectory(apppath + "//images");

            }
            if (resimyol != "" && grupID != -2)
            {
                Image resim = pbResim.Image;
                // brkd.Save("barcodes//" + barkod + ".JPG");
                resim.Save(apppath + "//images//" + grupID.ToString() + ".JPG");
                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();

                }
                string UpdateSQL = "UPDATE artikel SET resimyol='" + "images/" + grupID.ToString() + ".JPG" + "' WHERE artikelid=" + grupID;
                MySqlCommand myCoUpdate = new MySqlCommand(UpdateSQL, myConn);
                try
                {
                    if (myCoUpdate.ExecuteNonQuery() > 0)
                    {
                        // MessageBox.Show("OK");
                        myConn.Close();
                        label13.Text = "";
                    }
                }
                catch (Exception ee)
                {
                    label13.Text ="FEHLER!\nERROR CODE:" + ee.Message;
                }

                resimyol = "";
            }

        }

        private void txtAd_Enter(object sender, EventArgs e)
        {
            // MessageBox.Show(barkodno);
            if (barkodno == "")
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }

                MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikel where barkod = '" + txtBarkod.Text + "'", myConn);
                DataTable dtGrup = new DataTable("lieferant");
                dtGrup.Clear();
                myDaGrup.Fill(dtGrup);
                int kaySay = dtGrup.Rows.Count;
                if (kaySay > 0)
                {
                    //MessageBox.Show("Bu Ürün Zaten Daha Önceden Girilmiş!\nÜrün Adı:" + dtGrup.Rows[0].ItemArray[2].ToString());
                    //txtBarkod.Text = "";
                    txtBarkod.Focus();
                    grupID = Convert.ToInt16(dtGrup.Rows[0].ItemArray[0]);
                    if (barkodno != "")
                    {
                        txtBarkod.Text = barkodno;
                    }
                    else
                    {
                    }
                    txtAd.Text = dtGrup.Rows[0].ItemArray[2].ToString();
                    cbbGrup.SelectedValue = (int)dtGrup.Rows[0].ItemArray[3];
                    cbbMarka.SelectedValue = (int)dtGrup.Rows[0].ItemArray[5];
                    cbbLieferant.SelectedValue = (int)dtGrup.Rows[0].ItemArray[4];
                    txtAlisfiyat.Text = dtGrup.Rows[0].ItemArray[7].ToString();
                    txtSatisfiyat.Text = dtGrup.Rows[0].ItemArray[8].ToString();
                    txtKarE.Text = dtGrup.Rows[0].ItemArray[10].ToString();
                    txtKar.Text = Math.Round((((double)dtGrup.Rows[0].ItemArray[10] / (double)dtGrup.Rows[0].ItemArray[7]) * 100), 2).ToString();
                    cbbBirim.SelectedValue = (int)dtGrup.Rows[0].ItemArray[9];
                    txtStok.Text = dtGrup.Rows[0].ItemArray[11].ToString();
                    txtAgirlik.Text = dtGrup.Rows[0].ItemArray[17].ToString();
                    cbbRaf.SelectedItem = dtGrup.Rows[0].ItemArray[25].ToString();
                    cbbSira.SelectedItem = dtGrup.Rows[0].ItemArray[26].ToString();
                    cbbPfand.SelectedValue = (int)dtGrup.Rows[0].ItemArray[22];

                    if ((int)dtGrup.Rows[0].ItemArray[12] == 1)
                    {
                        cbDurum.Checked = true;

                    }
                    else
                    {
                        cbDurum.Checked = false;

                    }

                    barkodno = "";
                }
                else
                {
                    barkodno = txtBarkod.Text;
                    txtLeriBosalt();
                    txtBarkod.Text = barkodno;
                    barkodno = "";
                }
            }
        }

        private void cbbSira_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(" item" + cbbSira.SelectedText + " item" + cbbSira.SelectedItem + " value" + cbbSira.SelectedValue);
            txtLeriBosalt();

        }

        private void txtKar_Leave(object sender, EventArgs e)
        {
            double karorani = 0;
            if (txtKar.Text != "")
            {
                if (double.TryParse(txtKar.Text, out karorani))
                {
                    KarOranlaHesapla(karorani);
                }
            }
        }

        private void KarOranlaHesapla(double karorani)
        {
            if (hesapislem == false)
            {
                hesapislem = true;
                double alisfiyat;
                double satisfiyat;
                if (double.TryParse(txtAlisfiyat.Text, out alisfiyat))
                {
                    if (double.TryParse(txtAlisfiyat.Text, out alisfiyat))
                    {
                        double mwst = new ArtikelGrup((int)cbbGrup.SelectedValue).Mwst;

                        if (cbbFiyatDuz.Checked == true)
                        {
                            satisfiyat = (alisfiyat + ((alisfiyat * mwst) / 100)) * (1 + karorani / 100);
                            satisfiyat = Math.Round(satisfiyat, 2);
                            txtSatisfiyat.Text = Math.Round(satisfiyat, 2).ToString();
                            int virgulyeri = satisfiyat.ToString().IndexOf(',');
                            if (virgulyeri == -1)
                            {
                            }
                            else
                            {
                                string virgullukisim = satisfiyat.ToString().Substring(satisfiyat.ToString().Length - 1, 1);
                                txtSatisfiyat.Text = FiyatFormat(virgullukisim, satisfiyat);
                            }
                            double kar = (Convert.ToDouble(txtSatisfiyat.Text) ) - (alisfiyat * (1 + mwst / 100));
                            txtKarE.Text = kar.ToString();
                            //txtSatisfiyat.Text = Math.Round(satisfiyat,2).ToString();
                            txtSatisfiyat.Enabled = false;
                            txtKarE.Enabled = false;
                        }
                        else
                        {
                            satisfiyat = (alisfiyat + ((alisfiyat * mwst) / 100)) * (1 + karorani / 100);
                            txtSatisfiyat.Text = Math.Round(satisfiyat, 2).ToString();
                            double kar = (Convert.ToDouble(txtSatisfiyat.Text) ) - (alisfiyat * (1 + mwst / 100));
                            txtKarE.Text = kar.ToString();
                            //txtSatisfiyat.Text = Math.Round(satisfiyat,2).ToString();
                            txtSatisfiyat.Enabled = false;
                            txtKarE.Enabled = false;
                            label13.Text = "";
                        }
                    }
                    else
                    {
                        if (Program.IsletmeAyarlar["land"] == "de")
                            label13.Text = "Falsche EK Preisformat!";
                        else
                            label13.Text = "False Price Format!"; 
                    }
                }
                else
                {
                    txtKar.Text = "";
                    txtKarE.Text = "";

                }
                hesapislem = false;
            }
        }

        private string FiyatFormat(string virgullukisim, double satisfiyat)
        {
            decimal satfiyat = Convert.ToDecimal(satisfiyat);
            if (Convert.ToInt16(virgullukisim) >= 4)
            {
                int fark1 = 9 - Convert.ToInt16(virgullukisim);
                decimal oran = fark1 / 100m;
                decimal fark = Math.Round(oran, 3);

                satfiyat = satfiyat + fark;
                return satfiyat.ToString();
            }
            else
            {
                decimal fark = (Convert.ToInt16(virgullukisim) + 1) / 100m;
                satfiyat -= fark;
                return satfiyat.ToString();
            }
        }



        private void txtKar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                double karorani = 0;
                if (txtKar.Text != "")
                {
                    if (double.TryParse(txtKar.Text, out karorani))
                    {
                        KarOranlaHesapla(karorani);
                    }
                }

                // 
            }
        }

        private void txtKar_Enter(object sender, EventArgs e)
        {
            txtSatisfiyat.Enabled = false;
        }

        private void txtSatisfiyat_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtSatisfiyat.Enabled = true;
        }

        private void txtAlisfiyat_DoubleClick(object sender, EventArgs e)
        {
            txtSatisfiyat.Enabled = true;
        }

        private void txtBarkod_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                barkodno = txtBarkod.Text;
                aramayap("barcode", barkodno); ; //GridiDoldur(); // Gridi doldurmak için yazdığın metod

                // 
            }
        }

    

        private void kryptonButton5_Click_1(object sender, EventArgs e)
        {
            try
            {
                string Windir = "", OskDir="";
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
            catch(Exception xx)
            {
                System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
               // MessageBox.Show(xx.Message);
            }
        }

        private void txtStok_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (double.TryParse(txtStok.Text, out stok))
            {
                if (rbInventur.Checked == true)
                {
                    stokSQL = "" + vA.virgulayikla(stok);
                }
                else
                {
                    if (eskistok != -1)
                    {
                        stokSQL = "toplamstok+" + vA.virgulayikla(stok);
                    }
                    else
                    {
                        stokSQL = "" + vA.virgulayikla(stok);
                    }
                }
            }
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            SrvLangYukle();
            string cltname = Thread.CurrentThread.CurrentUICulture.Name;
            Culturname.culturname = cltname;
            //string cltname = Thread.CurrentThread.CurrentUICulture.Name;
            wg_main frmUrunGrup1 = new wg_main();
            frmUrunGrup1.culname = cltname;
            frmUrunGrup1.myConn = myConn;
            //Dictionary<Int32, string> dString = Program.lang.ToDictionary(k => Convert.ToInt32(k.Key), k => k.Value.ToString());
            frmUrunGrup1.SrvLang = srvLang;
            Form grupform = frmUrunGrup1.formgetir();
            //grupform.MdiParent = this;
            grupform.Text = kryptonButton6.Text;
            //kryptonSplitContainer1.Panel2.Controls.Add(grupform);

            grupform.Show();
            srvLang.Clear();
        }
        private void SrvLangYukle()
        {
            using (myConn = baglanti.myconn())
            {
                int langcode = -1;// Convert.ToInt16(Program.IsletmeAyarlar["dil"]);
                string cltname = Thread.CurrentThread.CurrentUICulture.Name;
                if (cltname == "tr-TR")
                {
                    langcode = 0;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("tr-TR");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
                }
                else if (cltname == "de-DE")
                {
                    langcode = 1;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                }
                else if (cltname == "en-GB")
                {
                    langcode = 2;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }
                langcode += 1;

                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string sql = "SELECT * FROM langsrv ORDER BY ID";
                MySqlCommand cmdSrvLang = new MySqlCommand(sql, myConn);
                MySqlDataReader dr = cmdSrvLang.ExecuteReader();
                while (dr.Read())
                {
                    srvLang.Add(dr.GetInt16(0), dr.GetString(langcode));
                }

            }

        }

        private void F_ArtikelGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scanner != null)
            {
                scanner.DataEvent -= new _IOPOSScannerEvents_DataEventEventHandler(scanner_DataEvent);
            }
            if (scanner2 != null)
            {
                scanner2.DataEvent -= new _IOPOSScannerEvents_DataEventEventHandler(scanner2_DataEvent);
            }
        }

        private void txtArtikelID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                if (txtArtikelID.TextLength > 0)
                {
                    aramayap("artikelid", txtArtikelID.Text);
                }
            }
        }
       




    }
}

