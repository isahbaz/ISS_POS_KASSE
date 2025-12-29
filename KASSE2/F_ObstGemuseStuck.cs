using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace IS_KASSE
{
    public partial class F_ObstGemuseStuck : Form
    {
        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();

        public int grupid = 0;
        public double fiyat = 0;
        public double karmiktari = 0;
        public int artikelid = 0;
        public string UrunAd = "";
        public bool sonuc = false;
        public int ebene = 0;
        public int limit = 47; //40
        public int count = 0;
        int rowCount = 0;
        public string Barcode = "0";
        public int ToplamKayitSay;
        MySqlDataAdapter daArtikel=null;
        DataTable dtArtikel =null;
        Tarih tarih = new Tarih();
        string angebotSembol = "";
        public Int32 pfandid=0;

        public delegate void BarkodSatisDelagate(string brkd);
        public event BarkodSatisDelagate BarcodeSatisEvent;

        Dictionary<object, object> Info = new Dictionary<object, object>();

        public F_ObstGemuseStuck()
        {
            InitializeComponent();
        }

        private void F_ObstGemuseStuck_Load(object sender, EventArgs e)
        {
            try
            {
                string apppath = Application.StartupPath;
                // waageYarat();

                MySqlConnection conn = new MySqlConnection();
                db baglanti = new db();
                conn = baglanti.myconn();

                if (Program.ProgramAyarlar["bForm"] == "Diagonal")
                {
                   // limit = 70;
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                if (count == 0)
                {
                    using (conn)
                    {
                        daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 AND satisfiyat<>0  ORDER BY  sno ASC, artikelad ASC", conn);
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
                        daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 AND satisfiyat<>0 ORDER BY sno ASC, artikelad ASC LIMIT " + count + "," + limit, conn);
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
                        Dictionary<object, object> Info = new Dictionary<object, object>();
                        Info.Add("karmiktari", dtArtikel.Rows[i].ItemArray[10]);
                        Info.Add("artikelid", dtArtikel.Rows[i].ItemArray[0]);
                        Info.Add("pfandid", dtArtikel.Rows[i].ItemArray[22]);
                        Info.Add("preis2", dtArtikel.Rows[i].ItemArray[42]);
                        Info.Add("preis3", dtArtikel.Rows[i].ItemArray[43]);
                        Info.Add("PLU", dtArtikel.Rows[i].ItemArray[1].ToString());
                        if ((int)dtArtikel.Rows[i].ItemArray[13] != 0)
                        {
                            if (Convert.ToInt32(dtArtikel.Rows[i].ItemArray[14]) <= tarih.unixdate(DateTime.Now) && Convert.ToInt32(dtArtikel.Rows[i].ItemArray[15]) >= tarih.unixdate(DateTime.Now))
                            {
                                Info.Add("preis", dtArtikel.Rows[i].ItemArray[16]);
                                angebotSembol = "*";
                            }
                            else
                            {
                                Info.Add("preis", dtArtikel.Rows[i].ItemArray[8]);
                                angebotSembol = "";
                            }
                        }
                        else
                        {
                            Info.Add("preis", dtArtikel.Rows[i].ItemArray[8]);
                            angebotSembol = "";
                        }
                        //var item=Program.ButtonEigenschaften.FirstOrDefault(a=> a.Key == grupid);
                        Dictionary<string, string> BtnEinz = null;
                        if (Program.ButtonEigenschaften.ContainsKey(grupid))
                        {
                            BtnEinz = new Dictionary<string, string>();
                            BtnEinz = Program.ButtonEigenschaften[grupid];
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
                        BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[10].ToString();
                        if (BtnEinz!=null)
                        {
                            BtnArtikel1.StateNormal.Back.Color1 = Color.FromName(BtnEinz["color"]);
                            BtnArtikel1.Size = ( new System.Drawing.Size(Convert.ToInt16(BtnEinz["width"]), Convert.ToInt16(BtnEinz["height"])));
                            BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font(BtnEinz["fontname"],Convert.ToSingle(BtnEinz["fontsize"]));
                        }
                        else
                        {
                            BtnArtikel1.StateNormal.Back.Color1 = System.Drawing.Color.White;
                           
                            BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Cascadia Mono", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        }
                        
                        BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
                        //BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                       
                        BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        BtnArtikel1.StateNormal.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        BtnArtikel1.StateNormal.Back.Color1 = Color.White;
                        if (dtArtikel.Rows[i].ItemArray[19].ToString() != "")
                        {
                            try
                            {
                               
                                    BtnArtikel1.StateNormal.Back.Image = Image.FromFile(dtArtikel.Rows[i].ItemArray[19].ToString());
                                    BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                                
                            }
                            catch (Exception ee)
                            {
                                //string msj = ee.Message;

                            }
                        }
                        // BtnArtikel1.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.mayd1;
                        BtnArtikel1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                        BtnArtikel1.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                        BtnArtikel1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.StateNormal.Border.Rounding = 6;
                        BtnArtikel1.StateNormal.Border.Width = 3;
                        BtnArtikel1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                        
                        BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                        BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                        BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                        BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                        BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                        BtnArtikel1.StateNormal.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisPath;
                        BtnArtikel1.StateNormal.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        
                        BtnArtikel1.Values.Text = angebotSembol + dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + string.Format("{0:f}", (dtArtikel.Rows[i].ItemArray[8])) + "EUR/St\r " + " [" + dtArtikel.Rows[i].ItemArray[1] + "]\r";
                        // BtnArtikel1.Values.Text = angebotSembol+dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + dtArtikel.Rows[i].ItemArray[8].ToString() + "[" + dtArtikel.Rows[i].ItemArray[1] + "]"; 
                        BtnArtikel1.Tag = Info;//dtArtikel.Rows[i].ItemArray[8];
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
                this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                this.kryptonButton4.TabIndex = 51;
                this.kryptonButton4.Values.Text = Program.lang["56"];
                this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
                flp.Controls.Add(kryptonButton4);
                // }
            }
            catch (Exception ee)
            {
                AddtoLogFile(ee.Message, "OG_Stk_Load");
            }
        }
        public void AddtoLogFile(string Message, string WebPage)
        {
            string path = Application.StartupPath.ToString() + ("Log_OG_STK" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
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
        private void GenericButton(object sender, EventArgs e)
        {
            //adet = yeniadet;
            //
            try
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                Info = (Dictionary<object, object>)btnsecilen.Tag;


                //fiyat = Convert.ToDouble(Info["preis"]);
                //fiyat = Convert.ToDouble(btnsecilen.Tag);
                artikelid = Convert.ToInt32(Info["artikelid"]);
                if (Program.IsletmeAyarlar["markt"] == "1" || Program.IsletmeAyarlar["markt"] == null)
                {
                    if (Convert.ToDouble(Info["preis2"]) != 0)
                    {
                        F_Preisauswahl preislist = new F_Preisauswahl();
                        preislist.Preis1 = Convert.ToDouble(Info["preis"]);
                        preislist.Preis2 = Convert.ToDouble(Info["preis2"]);
                        preislist.Preis3 = Convert.ToDouble(Info["preis3"]);

                        preislist.ShowDialog();
                        fiyat = preislist.seilenPreis;

                    }
                    else
                    {

                        fiyat = Convert.ToDouble(Info["preis"]);
                    }
                    karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2); //Convert.ToDouble(btnsecilen.Name);
                    UrunAd = btnsecilen.Values.Text;
                    pfandid = Convert.ToInt32(Info["pfandid"]);
                    Barcode = Info["PLU"].ToString();
                    sonuc = true;
                    this.Close();
                }
                else if (Program.IsletmeAyarlar["markt"] == "0")
                {
                    this.BarcodeSatisEvent(Info["PLU"].ToString());
                    /* karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2); //Convert.ToDouble(btnsecilen.Name);
                     UrunAd = btnsecilen.Values.Text;
                     pfandid = Convert.ToInt32(Info["pfandid"]);*/

                }
                else if (Program.IsletmeAyarlar["markt"] == "2")
                {
                    this.BarcodeSatisEvent(Info["PLU"].ToString());
                    /* karmiktari = Math.Round(Convert.ToDouble(Info["karmiktari"]), 2); //Convert.ToDouble(btnsecilen.Name);
                     UrunAd = btnsecilen.Values.Text;
                     pfandid = Convert.ToInt32(Info["pfandid"]);*/
                    this.Close();

                }
            }
            catch (Exception ff)
            {
                AddtoLogFile(ff.Message, "OG_Stk_GenericButton");
            }
        }
        public void ObstGemFromForm(string PLU)
        {
            this.BarcodeSatisEvent(PLU);
        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            sonuc = false;
            this.Close();
        }
        private void btnEbeneNext_Click(object sender, EventArgs e)
        {
            try
            {
                F_ObstGemuseStuck frmobstGemuseStuck = new F_ObstGemuseStuck();
                ebene++;
                count = count + limit;
                frmobstGemuseStuck.ebene = ebene;
                frmobstGemuseStuck.grupid = grupid;
                frmobstGemuseStuck.count = count;
                frmobstGemuseStuck.ToplamKayitSay = ToplamKayitSay;
                frmobstGemuseStuck.BarcodeSatisEvent += new BarkodSatisDelagate(ObstGemFromForm);
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
                if (frmobstGemuseStuck.sonuc == true)
                {
                    Info = frmobstGemuseStuck.Info;
                    fiyat = frmobstGemuseStuck.fiyat;
                    artikelid = frmobstGemuseStuck.artikelid;
                    karmiktari = frmobstGemuseStuck.karmiktari;
                    UrunAd = frmobstGemuseStuck.UrunAd;
                    Barcode = Info["PLU"].ToString();
                    sonuc = true;
                    this.Close();
                }
            }
            catch (Exception dd)
            {
                AddtoLogFile(dd.Message, "OG_Stk_NextButton");
            }
        }
        
        
    }
}
