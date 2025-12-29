using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using System.Threading;
using System.Drawing.Text;


namespace IS_KASSE
{
    public partial class F_Waage : Form
    {
        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        System.Windows.Forms.Timer timertest = new System.Windows.Forms.Timer();

        public int grupid = 0;
        public double adet = 0;
        public double fiyat = 0;
        public double WaageTutar = 0;
        public double karmiktari = 0;
        public int artikelid = 0;
        public string UrunAd = "";
        public bool sonuc = false;
        public int ebene = 0;
        public int limit = 47;
        public int limit2 = 0;
        public int count = 0;
        int rowCount = 0;
        public int ToplamKayitSay;
        MySqlDataAdapter daArtikel = null;
        DataTable dtArtikel = null;

        //F_WaageMsj frmmesaj = new F_WaageMsj();
        public int counter_waage = 0;
        Tarih tarih = new Tarih();
        string angebotSembol = "";

        double tara = 0;
        string stringTara = "";

        string gewicht = "";
        int step1 = 0;
        string[] gidendata = null;
        byte[] buf = new byte[2];
        string ag = "";
        string fi = "";
        string so = "";
        string agirlik = "";
        public Dictionary<object, object> Info = new Dictionary<object, object>();
        int widht = 0;
        public F_Waage()
        {
            InitializeComponent();
            if (Program.ProgramAyarlar["bForm"] == "Diagonal")
            {
                if (Screen.AllScreens[0].Primary)
                {
                    widht = Screen.AllScreens[0].WorkingArea.Width - 10;
                }
                else
                {
                    widht = Screen.AllScreens[1].WorkingArea.Width - 10;
                }
              

            }
        }

        private void F_Waage_Load(object sender, EventArgs e)
        {
           
            UrunAd = "";
            string apppath = Application.StartupPath;
            // waageYarat();

            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
           
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();


            }
            if (count == 0)
            {
                using (conn)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 AND satisfiyat<>0  ORDER BY  sno ASC, artikelad", conn);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    rowCount = dtArtikel.Rows.Count;
                    ToplamKayitSay = rowCount;
                    if (ToplamKayitSay < limit)
                        limit = ToplamKayitSay;
                }
            }
            else
            {
                using (conn)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 AND satisfiyat<>0  ORDER BY sno ASC, artikelad LIMIT " + count + "," + limit, conn);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    rowCount = dtArtikel.Rows.Count;
                }
            }
            if (rowCount > 0)
            {

                for (int i = 0; i < limit; i++)
                {
                    Info = new Dictionary<object, object>();
                    Info.Add("karmiktari", dtArtikel.Rows[i].ItemArray[10]);
                    Info.Add("tara", dtArtikel.Rows[i].ItemArray[31]);
                    Info.Add("barcode", dtArtikel.Rows[i].ItemArray[1]);
                    if ((int)dtArtikel.Rows[i].ItemArray[13] != 0)
                    {
                        if (Convert.ToInt32(dtArtikel.Rows[i].ItemArray[14]) <= tarih.unixdate(DateTime.Now) && Convert.ToInt32(dtArtikel.Rows[i].ItemArray[15]) >= tarih.unixdate(DateTime.Now))
                        {
                            Info.Add("preis", dtArtikel.Rows[i].ItemArray[16]);
                            angebotSembol = "*";
                        }
                    }
                    else
                    {
                        Info.Add("preis", dtArtikel.Rows[i].ItemArray[8]);
                        angebotSembol = "";
                    }
                    ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                    if (Program.ProgramAyarlar["bForm"] == "Diagonal")
                    {
                        BtnArtikel1.Size = new System.Drawing.Size(200, 120);
                        this.btnEbeneNext.Size = new System.Drawing.Size(200, 120);
                        this.kryptonButton4.Size = new System.Drawing.Size(200, 120);
                    }
                    else
                    {
                        BtnArtikel1.Size = new System.Drawing.Size(164, 98);
                        this.btnEbeneNext.Size = new System.Drawing.Size(164, 98);
                        this.kryptonButton4.Size = new System.Drawing.Size(164, 98);
                    }
                    BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                    BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[0].ToString();
                                    
                    BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                    
                    BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                    if (dtArtikel.Rows[i].ItemArray[19].ToString() != "")
                    {
                        try
                        {
                        BtnArtikel1.StateNormal.Back.Image= Image.FromFile(dtArtikel.Rows[i].ItemArray[19].ToString());
                        }
                        catch { }
                        finally
                        {
                            
                        }
                    }
                    // BtnArtikel1.StateNormal.Back.Image = global::ISS_Scale.Properties.Resources.mayd1;
                    BtnArtikel1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                    BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                    BtnArtikel1.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                    BtnArtikel1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    BtnArtikel1.StateNormal.Border.Rounding = 6;
                    BtnArtikel1.StateNormal.Border.Width = 3;
                    /*System.Drawing.Text.PrivateFontCollection privateFonts = new PrivateFontCollection();
                    privateFonts.AddFontFile(@"\Font\segoeui.ttf");
                    System.Drawing.Font font = new Font(privateFonts.Families[0], 12);*/
                    BtnArtikel1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                    BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Segoe UI Standard", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                    BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                    BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                    BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                    BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                    BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                    BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                    BtnArtikel1.StateNormal.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                    BtnArtikel1.StateNormal.Back.Color1 = Color.White;
                    // BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    //BtnArtikel1.TabIndex = ;
                    /*if (Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]) < 0)
                   {   
                   }
                   else
                   {
                       BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                   }*/
                    //"Mayddfgdfgdanoz\n1,99EUR/Kg\r[5] \r"
                    BtnArtikel1.Text = angebotSembol + dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + string.Format("{0:f}", (dtArtikel.Rows[i].ItemArray[8])) + "EUR/kg\r " + " [" + dtArtikel.Rows[i].ItemArray[1] + "]\r";
                    BtnArtikel1.Tag = Info; //dtArtikel.Rows[i].ItemArray[8];
                    BtnArtikel1.Click += new EventHandler(GenericButton);

                    flp.Controls.Add(BtnArtikel1);
                }
                if (ToplamKayitSay > limit + count)
                {
                    this.btnEbeneNext.Location = new System.Drawing.Point(2, 2);
                    this.btnEbeneNext.Name = "btnEbeneNext";
                   
                    this.btnEbeneNext.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    this.btnEbeneNext.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.btnEbeneNext.TabIndex = 51;
                    this.btnEbeneNext.Values.Text = "WEITER";
                    this.btnEbeneNext.Click += new System.EventHandler(this.btnEbeneNext_Click);
                    flp.Controls.Add(btnEbeneNext);

                }
            }
            this.kryptonButton4.Location = new System.Drawing.Point(2, 2);
            this.kryptonButton4.Name = "kryptonButton4";
            
            this.kryptonButton4.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton4.TabIndex = 51;
            this.kryptonButton4.Values.Text = "ABBRUCH";
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
            flp.Controls.Add(kryptonButton4);

          
        }


        private void GenericButton(object sender, EventArgs e)
        {
            try
            {
                
                // timer1.Enabled = true;
                

               
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                Info = (Dictionary<object, object>)btnsecilen.Tag;
                this.Close();
                /*fiyat = Convert.ToDouble(Info["preis"]);
                artikelid = Convert.ToInt32(btnsecilen.Name);
                karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2);
                UrunAd = btnsecilen.Values.Text;
                tara = Convert.ToDouble(Info["tara"]);
                if (sp.IsOpen == true)
                {
                    sp.Close();
                }
                try
                {

                    //CheckForIllegalCrossThreadCalls = false;
                    // Thread waage = new Thread(new ThreadStart(pluWaagePrecess)); //new Thread(() => pluWaagePrecess());

                    // waage.Start();
                    if (sp.IsOpen == false)
                    {
                        sp.PortName = Program.WPORT;
                        sp.BaudRate = 9600;
                        sp.Parity = Parity.Odd;
                        sp.DataBits = 7;
                        sp.RtsEnable = true;
                        sp.DtrEnable = true;
                        
                        sp.StopBits = StopBits.One;
                        //sp.Handshake = Handshake.RequestToSend;
                        sp.Open();
                    }
                    fiyatgonder();
                }
                catch (Exception ex)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["62"] + ex.Message;
                    frmerror.ShowDialog();
                    this.Close();

                } */
                
            }
            catch (Exception ee)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = "generic button error"+ ee.Message;
                frmerror.ShowDialog();
                this.Close();
            }




        }

        private void pluWaagePrecess()
        {
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += new EventHandler(timer1_Tick);
        }
       /* Thread is2 = null;
        private void fiyatgonder()
        {
            try
            {
                string strFiyat = fiyat.ToString("#0.00");
                int virgulyeri = strFiyat.LastIndexOf(',');
                //strFiyat = strFiyat.Remove(virgulyeri,1);
                step1 = 0;

                if (virgulyeri != -1)
                {
                    strFiyat = strFiyat.Remove(virgulyeri, 1);
                }
                else
                {
                    strFiyat = strFiyat + "00";
                }
                while (strFiyat.Length < 6)
                {
                    strFiyat = "0" + strFiyat;
                }
                if (tara <= 0)
                {
                    stringTara = "0000";
                }
                else
                {
                    stringTara = tara.ToString();
                    if (stringTara.IndexOf(',') != -1)
                    {
                        stringTara = stringTara.Remove(stringTara.IndexOf(','), 1);
                    }
                    int tarauz = stringTara.Length;
                    if (tarauz > 10)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.BringToFront();
                        frmerror.lblMesaj.Text = Program.lang["74"];
                        frmerror.ShowDialog();
                        this.Close();
                        return;
                    }
                    else if (tarauz > 4)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.BringToFront();
                        frmerror.lblMesaj.Text = Program.lang["74"];
                        frmerror.ShowDialog();
                        this.Close();
                        return;
                    }
                    else if (tarauz < 4)
                    {
                        while (stringTara.Length < 4)
                        {
                            stringTara = "0" + stringTara;
                        }
                    }




                }

                if (sp.IsOpen == true)
                {
                    sp.WriteLine("" + ((char)004) + ((char)002) + "03" + ((char)27) + strFiyat + ((char)027) + stringTara + ((char)003));
                }
                else
                {
                    sp.Open();
                    sp.WriteLine("" + ((char)004) + ((char)002) + "03" + ((char)27) + strFiyat + ((char)027) + stringTara + ((char)003));
                }

            }
            catch (Exception fff)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.BringToFront();
                frmerror.lblMesaj.Text = "preis Check Error:"+fff.Message;
                frmerror.ShowDialog();
                this.Close();
            }

        }*/
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            this.ebene--;
            this.count = ebene * count;
            Info.Clear();
            sonuc = false;
            
            
            this.Close();
        }
        bool sabir = false;
      /*  private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(200);
                if (sp.IsOpen == false)
                {
                    try
                    {
                        sp.Open();
                    }
                    catch
                    {
                        //MessageBox.Show("BIR HATA MEYDANA GELDI!\n");
                        step1 = 0;
                        return;
                    }
                }
                int byteCount = sp.BytesToRead;
                byte[] dataBuffer = new byte[byteCount];
                // dataBuffer = null;
                // MessageBox.Show("OKUNAN:" + comport.ReadExisting());


                sp.Read(dataBuffer, 0, byteCount);
                if (dataBuffer[0] != 0 || dataBuffer != null)
                {
                    if (dataBuffer.Length == 1 && dataBuffer[0] == 21)
                    {

                        step1 = 2;
                        sp.WriteLine("" + ((char)004) + ((char)002) + "08" + ((char)003));


                    }

                    else if (dataBuffer.Length == 1 && dataBuffer[0] == 6)
                    {
                        step1 = 1;
                        encode(dataBuffer);
                    }
                    else if (dataBuffer.Length == 2 && dataBuffer[0] == 6)
                    {
                        step1 = 1;
                        encode(dataBuffer);
                    }


                    //sp.Read(gidendata, 0, 2);
                    else if (step1 == 2)
                    {
                        if (dataBuffer.Length == 7)
                        {








                            /*****9/
                            if ((dataBuffer[4] == 50) && (dataBuffer[5] == 49))
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.BringToFront();
                                frmerror.lblMesaj.Text = Program.lang["60"];
                                frmerror.ShowDialog();
                                dataBuffer = null;
                                step1 = 3;
                                timertest.Enabled = false;
                                this.Enabled = true;
                                //sonuc = false;
                                //CheckForIllegalCrossThreadCalls = false;
                                //this.Close();
                            }
                            else if ((dataBuffer[4] == 48) && (dataBuffer[5] == 48))
                            {
                                step1 = 3;
                                sp.WriteLine("" + ((char)004) + ((char)005));
                                return;
                                //sonuc = false;
                                //CheckForIllegalCrossThreadCalls = false;
                                //this.Close();
                            }
                            else if ((dataBuffer[4] == 50) && (dataBuffer[5] == 50))
                            {
                                step1 = 3;
                                //MessageBox.Show("ARTIKEL BILGISI EKSIK");
                                //sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                                fiyatgonder();
                                // this.Enabled = true;
                                return;

                                //sonuc = false;
                                //CheckForIllegalCrossThreadCalls = false;
                                //this.Close();
                            }
                            else if ((dataBuffer[4] == 49) && (dataBuffer[5] == 49))
                            {
                                step1 = 0;
                                //MessageBox.Show("GECERSIZ FIYAT\nTEKRAR DENEYINIZ");
                                sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                                dataBuffer = null;
                                /*YENI EKLENEN 21.07.2013*9/
                                fiyatgonder();
                                //return;
                                //sonuc = false;
                                //CheckForIllegalCrossThreadCalls = false;
                                //this.Close();
                            }
                            else if ((dataBuffer[4] == 48) && (dataBuffer[5] == 50))
                            {
                                step1 = 0;
                                //MessageBox.Show("GECERSIZ FIYAT\nTEKRAR DENEYINIZ");
                                //sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                                dataBuffer = null;
                                /*YENI EKLENEN 21.07.2013*9/
                                fiyatgonder();
                                //return;
                                //sonuc = false;
                                //CheckForIllegalCrossThreadCalls = false;
                                //this.Close();
                            }
                            else if ((dataBuffer[4] == 50) && (dataBuffer[5] == 48))
                            {
                                step1 = 0;
                                //MessageBox.Show("GECERSIZ FIYAT\nTEKRAR DENEYINIZ");
                                //sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                                dataBuffer = null;
                                /*YENI EKLENEN 21.07.2013*9/
                                fiyatgonder();
                            }
                            else if ((dataBuffer[4] == 51) && (dataBuffer[5] == 48))
                            {
                                step1 = 0;

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.BringToFront();
                                frmerror.lblMesaj.Text = Program.lang["77"];
                                frmerror.ShowDialog();
                                counter_waage = 0;
                                timertest.Enabled = false;
                                dataBuffer = null;
                                this.Enabled = true;
                                return;
                            }
                            else if ((dataBuffer[4] == 51) && (dataBuffer[5] == 49))
                            {
                                step1 = 0;

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.BringToFront();
                                frmerror.lblMesaj.Text = Program.lang["78"];
                                frmerror.ShowDialog();

                                counter_waage = 0;
                                timertest.Enabled = false;
                                dataBuffer = null;
                                this.Enabled = true;
                                return;
                            }
                            else if ((dataBuffer[4] == 51) && (dataBuffer[5] == 50))
                            {
                                step1 = 0;

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.BringToFront();
                                frmerror.lblMesaj.Text = Program.lang["79"];
                                frmerror.ShowDialog();

                                counter_waage = 0;
                                timertest.Enabled = false;
                                dataBuffer = null;
                                this.Enabled = true;
                                return;
                            }
                            else if ((dataBuffer[4] == 51) && (dataBuffer[5] == 51))
                            {
                                step1 = 0;

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.BringToFront();
                                frmerror.lblMesaj.Text = Program.lang["80"];
                                frmerror.ShowDialog();

                                counter_waage = 0;
                                timertest.Enabled = false;
                                dataBuffer = null;

                                return;
                            }
                            else
                            {
                                step1 = 0;
                                dataBuffer = null;
                                fiyatgonder();
                            }

                        }

                    }
                    else
                    {

                        if (dataBuffer[0] == 21 && step1 == 0 && Program.connection == false)
                        {


                            fiyatgonder();


                        }
                        else if (dataBuffer[0] == 21 && step1 != 0)
                        {

                            step1 = 0;
                            dataBuffer = null;
                            fiyatgonder();


                        }

                        else
                        {
                            encode(dataBuffer);
                        }
                    }
                }
            }
            catch (Exception ggg)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.BringToFront();
                frmerror.lblMesaj.Text = "data receive error:"+ggg.Message;
                frmerror.ShowDialog();
                //return;
            }


        }

        private void encode(byte[] data)
        {
            if (step1 == 0)
            {
                string rCS = "";
                string rKW = "";
                try
                {
                    int i = 0, a = 0;
                    gidendata = new string[2];


                    foreach (byte bayt in data)
                    {
                        if (i == 5 || i == 6)
                        {
                            // aa =char(bayt);
                            //gidendata.Add(bayt);
                            buf[a] = bayt;
                            i++;
                            a++;
                        }
                        else
                        {
                            i++;
                        }


                    }
                    string csW = Encoding.ASCII.GetString(buf);
                    string CS = "2C3C";
                    ushort longCS = ushort.Parse("2C3C", System.Globalization.NumberStyles.HexNumber);
                    string KW = "FA07";
                    ushort longKW = ushort.Parse("FA07", System.Globalization.NumberStyles.HexNumber);

                    int sola = Int16.Parse(csW.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);

                    int saga = 0;
                    if (buf[1] < 10)
                    {
                        saga = buf[1];
                    }
                    else
                    {
                        saga = Int16.Parse(csW.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    }

                     rCS = RotateLeft(longCS, sola).ToString("X4");
                    rKW = RotateRight(longKW, saga).ToString("X4");


                    // MessageBox.Show(rCS);
                    // MessageBox.Show(rKW);
                    if (rCS.Length > 4)
                    {
                        rCS = rCS.Substring(1, 4);
                    }
                    if (rCS.Length < 4)
                    {
                        rCS = "0" + rCS;
                    }
                    if (rKW.Length > 4)
                    {
                        rKW = rKW.Substring(rKW.Length - 4, 4);
                    }
                    if (rKW.Length < 4)
                    {
                        rKW = "0" + rKW;
                    }
                    step1 = 1;
                    sp.WriteLine("" + ((char)004) + ((char)002) + "10" + ((char)27) + rCS + rKW + ((char)003));
                    return;
                }
                catch(Exception ss)
                {
                    //MessageBox.Show(ss.Message + " -1" + "\n"  + rCS + rKW +"");
                }

            }
            else if (step1 == 1)
            {


                //if (sp.BytesToRead == 0) dataReceived = true;
                try
                {
                    if (data[0] == 21)
                    {

                        step1 = 2;
                        sp.WriteLine("" + ((char)004) + ((char)002) + "08" + ((char)003));
                    }
                    else
                    {

                        step1 = 3;
                        sp.WriteLine("" + ((char)004) + ((char)005));
                        return;

                    }
                    gewicht = "";
                }
                catch (Exception ss)
                {
                    //MessageBox.Show(ss.Message + "-2");
                }

                //step1 = false;
            }
            else if (step1 == 2)
            {

                sp.WriteLine("" + ((char)004) + ((char)002) + "08" + ((char)003));
                //gewicht += Encoding.ASCII.GetString(data);
                //if (sp.BytesToRead == 0) dataReceived = true;
                //MessageBox.Show(gewicht);

                gewicht = "";

            }
            else if (step1 == 3)
            {
                try
                {
                    if (data[0] == 2 && data[1] == 49 && data[2] == 49 && data[3] == 27 && data[4] == 49)
                    {
                        // step1 = 4;
                        sp.WriteLine("" + ((char)004) + ((char)005));
                        return;
                    }
                    else if ((data[0] == 2 && data[1] == 49 && data[2] == 49 && data[3] == 27 && data[4] == 50))
                    {
                        int i = 0, a = 0;
                        foreach (byte bayt in data)
                        {
                            if (i == 5 || i == 6)
                            {
                                // aa =char(bayt);
                                //gidendata.Add(bayt);
                                buf[a] = bayt;
                                i++;
                                a++;
                            }
                            else
                            {
                                i++;
                            }


                        }
                        string csW = Encoding.ASCII.GetString(buf);
                        string CS = "2C3C";
                        ushort longCS = ushort.Parse("2C3C", System.Globalization.NumberStyles.HexNumber);
                        string KW = "FA07";
                        ushort longKW = ushort.Parse("FA07", System.Globalization.NumberStyles.HexNumber);

                        int sola = Int16.Parse(csW.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);

                        int saga = 0;
                        if (buf[1] < 10)
                        {
                            saga = buf[1];
                        }
                        else
                        {
                            saga = Int16.Parse(csW.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                        }

                        string rCS = RotateLeft(longCS, sola).ToString("X");
                        string rKW = RotateRight(longKW, saga).ToString("X");


                        // MessageBox.Show(rCS);
                        // MessageBox.Show(rKW);
                        if (rCS.Length > 4)
                        {
                            rCS = rCS.Substring(1, 4);
                        }
                        if (rCS.Length < 4)
                        {
                            rCS = "0" + rCS;
                        }
                        if (rKW.Length > 4)
                        {
                            rKW = rKW.Substring(rKW.Length - 4, 4);
                        }
                        if (rKW.Length < 4)
                        {
                            rKW = "0" + rKW;
                        }
                        step1 = 1;
                        sp.WriteLine("" + ((char)004) + ((char)002) + "10" + ((char)27) + rCS + rKW + ((char)003));
                        return;



                    }
                    else if (data[0] == 2 && data[1] == 49 && data[2] == 49 && data[3] == 27 && data[4] == 48)
                    {
                        step1 = 0;

                        fiyatgonder();
                    }
                    else if (data.Length == 26)
                    {
                        try
                        {
                            //Program.connection = true;
                            CheckForIllegalCrossThreadCalls = false;
                            Program.connection = true;
                            ag = "";
                            fi = "";
                            so = "";

                            byte[] agirlikDizi = new byte[5];
                            byte[] fiyatDizi = new byte[6];
                            byte[] sonucDizi = new byte[6];

                            Array.Copy(data, 6, agirlikDizi, 0, 5);
                            Array.Copy(data, 12, fiyatDizi, 0, 6);
                            Array.Copy(data, 19, sonucDizi, 0, 6);


                            ag += Encoding.ASCII.GetString(agirlikDizi);
                            fi += Encoding.ASCII.GetString(fiyatDizi);
                            so += Encoding.ASCII.GetString(sonucDizi);
                            this.agirlik = ag.Substring(0, 2) + "," + ag.Substring(2, 3);
                            this.adet = Convert.ToDouble(agirlik);
                            this.fiyat = Convert.ToDouble(fi.Substring(0, 4) + "," + fi.Substring(4, 2));
                            this.WaageTutar = Convert.ToDouble(so.Substring(0, 4) + "," + so.Substring(4, 2));
                            /* CheckForIllegalCrossThreadCalls = false;
                             Thread is1 = new Thread(new ThreadStart(is1tread));
                             is1.Start();*9/
                            this.sonuc = true;
                            sp.Close();
                            this.Close();


                            //gewicht += Encoding.ASCII.GetString(data);
                            //if (sp.BytesToRead == 0) dataReceived = true;
                            //MessageBox.Show(gewicht);
                            // sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                            gewicht = "";
                            step1 = 0;
                            counter_waage = 0;
                            timertest.Enabled = false;
                            tara = 0;
                            stringTara = "";
                        }
                        catch (Exception ee)
                        {
                            ///MessageBox.Show(ee.Message);
                        }

                    }
                }
                catch(Exception ss)
                {
                    //MessageBox.Show(ss.Message + " -3"); 
                }

            }


        }
        private void is1tread()
        {
            /*F_WaageMsj frmmesaj= new F_WaageMsj();
                        frmmesaj.ShowDialog();
                        sabir = true;*/
            //string fiyatTerazi = Convert.ToDouble(fi.Substring(0, 4) + "," + fi.Substring(4, 2)).ToString();
            //connection = true;
           /* agirlik = ag.Substring(0, 2) + "," + ag.Substring(2, 3);
            adet = Convert.ToDouble(agirlik);
            fiyat = Convert.ToDouble(fi.Substring(0, 4) + "," + fi.Substring(4, 2));
            sonuc = true;
            sp.Close();
            sabir = false;
            if (is2 != null)
            {
                is2.Abort();
                //frmmesaj.Close();
            }
            this.Close();
            //label2.Text = Convert.ToDouble(fi.Substring(0, 4) + "," + fi.Substring(4, 2)).ToString();
            //label3.Text = Convert.ToDouble(so.Substring(0, 4) + "," + so.Substring(4, 2)).ToString();
        }*/
        private void sabirgoster()
        {

           


        }
        public ulong CheckCSKW(ulong ulCSKW1)
        {
            string s = Convert.ToString(ulCSKW1);
            // ulong ulCSKW = Convert.ToUInt64("0x" + s);

            ulong ulCSKW = ulCSKW1;//0x47110000L;
            ulong islem = 0;

            int bIndex;
            ulong ulPolynome = 3276898304;
            ulong ulMask = 2147483648;// 0x80000000L;
            //ulong ulPolynome1=ulPolynome.Pa

            for (bIndex = 0; bIndex <= 15; bIndex++)
            {
                islem = ((ulong)(ulCSKW) & (ulong)(ulMask));
                if (islem != 0)
                {
                    ulCSKW ^= ulPolynome;
                }
                ulPolynome >>= 1;
                ulMask >>= 1;
            }
            return ulCSKW;

        }
        public ushort RotateLeft(ulong value, int count)
        {
            ushort val = (ushort)value;
            return (ushort)((val << count) | (val >> (16 - count)));
        }

        public ushort RotateRight(ulong value, int count)
        {
            ushort val = (ushort)value;
            return (ushort)((value >> count) | (value << (16 - count)));
        }

        private void F_Waage_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void btnEbeneNext_Click(object sender, EventArgs e)
        {
            try
            {
               // sp.Close();
            }
            finally
            {
                //sp.Close();
            }
            F_Waage frmobstGemuseStuck = new F_Waage();
            ebene++;
            count = count + limit;
            frmobstGemuseStuck.ebene = ebene;
            frmobstGemuseStuck.grupid = grupid;
            frmobstGemuseStuck.count = count;
            frmobstGemuseStuck.ToplamKayitSay = ToplamKayitSay;
            if ((ebene + 1) * limit < ToplamKayitSay)
            {
                frmobstGemuseStuck.limit = limit;
            }
            else
            {
                if (count > ToplamKayitSay)
                {
                    count = count - limit;
                    frmobstGemuseStuck.count = count;
                    // lim

                }
                frmobstGemuseStuck.limit = ToplamKayitSay - count;
            }
            frmobstGemuseStuck.ShowDialog();
            if (frmobstGemuseStuck.Info.Count > 0)
            {
                fiyat = frmobstGemuseStuck.fiyat;
                adet = frmobstGemuseStuck.adet;
                WaageTutar = frmobstGemuseStuck.WaageTutar;
                artikelid = frmobstGemuseStuck.artikelid;
                karmiktari = frmobstGemuseStuck.karmiktari;
                UrunAd = frmobstGemuseStuck.UrunAd;
                this.Info = frmobstGemuseStuck.Info;
                //sonuc = true;
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter_waage++;
            if (counter_waage == 30)
            {
                //counter_waage = 0;
                this.Close();
                timer1.Enabled = false;


            }
        }


        public void timertest_Tick(object sender, EventArgs e)
        {
            counter_waage++;
            if (counter_waage == 20)
            {
                //counter = 0;
                //sp.WriteLine("" + ((char)004) + ((char)002) + "81" + ((char)003));
                timertest.Stop();
                timertest.Enabled = false;
                
                //counter = 0;
                this.Close();
                //timer1.Enabled = false;


            }
        }
    }
}
