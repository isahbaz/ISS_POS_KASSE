using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;
using POS.Devices;
using Microsoft.PointOfService;
using System.Threading;
using iss_Rea;
using iss_RechnungsCreate;
using iss_ebon;
using System.Diagnostics;
using iss_CashDro;

namespace IS_KASSE
{
    public partial class F_Kiosk : Form
    {
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
        MySqlConnection mySqlConnection = new MySqlConnection();
        MySqlConnection connection = new db().myconn();
        tar.Tarih tarih = new tar.Tarih();
        iss_CashDro_Main main;
        public F_Kiosk()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

        }
      
        private void F_Kiosk_Load(object sender, EventArgs e)
        {
            main = new iss_CashDro_Main();
            main.KundenDisplayText += new iss_CashDro_Main.DisplayForm(DisplayFormShow);
            panelLogoPlatz.BackColor = Color.FromArgb(0x6f2795);
            btnBestellen.Enabled = false;
            btnBestellen.Text = "BESTELLUNG BESTÄTIGEN";
            lblToplam.Text = 0.ToString("C");
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("SELECT * FROM artikel INNER JOIN artikelgrup ON artikel.grupid = artikelgrup.grupid AND artikelgrup.gruptur = 10 ", connection);
            DataTable dataTableGrup = new DataTable("artikelgrup");
            dataTableGrup.Clear();
            mySqlDataAdapter.Fill(dataTableGrup);
            int count = dataTableGrup.Rows.Count;
            if (count > 0)
            {
                flpLeftMenu.Controls.Clear();


                for (int index = 0; index < count; ++index)
                {
                    try
                    {
                        ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1 = new KryptonGroup();
                        System.Windows.Forms.Panel panel1 = new Panel();
                        System.Windows.Forms.Label label1 = new Label();
                        kryptonGroup1.Location = new System.Drawing.Point(10, 25);
                        kryptonGroup1.Margin = new System.Windows.Forms.Padding(5, 20, 5, 20);
                        kryptonGroup1.Name = "kryptonGroup1";
                        kryptonGroup1.Tag = dataTableGrup.Rows[index].ItemArray[1].ToString();
                        kryptonGroup1.Click += new EventHandler(GenericButton);
                        //  ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                        // kryptonGroup1.Panel
                        // 

                        kryptonGroup1.Panel.Controls.Add(label1);
                        kryptonGroup1.Panel.Controls.Add(panel1);
                        kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(5);
                        kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
                        kryptonGroup1.Size = new System.Drawing.Size(206, 181);
                        kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        kryptonGroup1.StateCommon.Border.Rounding = 3;
                        kryptonGroup1.StateNormal.Border.Color1 = System.Drawing.Color.Chocolate;
                        kryptonGroup1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        kryptonGroup1.StateNormal.Border.Rounding = 5;
                        kryptonGroup1.StateNormal.Border.Width = 1;
                        kryptonGroup1.Panel.Tag = dataTableGrup.Rows[index].ItemArray[1].ToString();
                        kryptonGroup1.Panel.Click += new EventHandler(GenericButton);
                        kryptonGroup1.TabIndex = 3;

                        kryptonGroup1.Panel.Click += new System.EventHandler(this.GenericButton);
                        // 
                        // label1
                        // 

                        label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        label1.Font = new System.Drawing.Font("Copperplate Gothic Light", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        label1.Location = new System.Drawing.Point(3, 120);
                        label1.Name = "label1";
                        label1.Size = new System.Drawing.Size(191, 52);
                        label1.TabIndex = 1;
                        label1.Text = dataTableGrup.Rows[index].ItemArray[2].ToString();
                        label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        label1.Tag = dataTableGrup.Rows[index].ItemArray[1].ToString();
                        label1.Click += new System.EventHandler(this.GenericButton);
                        // 
                        // panel1
                        // 


                        panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        if (dataTableGrup.Rows[index].ItemArray[19].ToString() == "")
                        {
                            panel1.BackgroundImage = global::IS_KASSE.Properties.Resources.NoImage;
                        }
                        else
                        {
                            try
                            {
                                panel1.BackgroundImage = Image.FromFile(dataTableGrup.Rows[index].ItemArray[19].ToString());
                            }
                            catch
                            {
                                panel1.BackgroundImage = global::IS_KASSE.Properties.Resources.NoImage;
                            }
                        }
                        panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        panel1.Location = new System.Drawing.Point(33, 6);
                        panel1.Name = "panel1";
                        panel1.Size = new System.Drawing.Size(144, 111);
                        panel1.TabIndex = 0;
                        panel1.Tag = dataTableGrup.Rows[index].ItemArray[1].ToString();
                        panel1.Click += new System.EventHandler(this.GenericButton);
                        flpLeftMenu.Controls.Add(kryptonGroup1);
                    }
                    catch (Exception ee)
                    {
                        continue;

                    }

                }

                //flowPanelGrup.Size = new Size(133 * count, 127);
                //flowPanelGrup.AutoScrollMinSize = new System.Drawing.Size(500, 0);
            }


        }

        private void Panel1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProductLists(object sender, EventArgs e)
        {
            try
            {
                MySqlDataAdapter daArtikel = null;
                DataTable dtArtikel = null;
                Tarih tarih = new Tarih();
                string angebotSembol = "";
                Panel btnsecilen = sender as Panel;
                int grupid = Convert.ToInt32(btnsecilen.Tag);
                string apppath = Application.StartupPath;
                flpTop.Controls.Clear();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (connection)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 AND satisfiyat<>0 ORDER BY sno ASC, artikelad ASC", connection);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);




                    if (dtArtikel.Rows.Count > 0)
                    {


                        for (int i = 0; i < dtArtikel.Rows.Count; i++)
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
                                    // angebotSembol = "*";
                                }
                            }
                            else
                            {
                                Info.Add("preis", dtArtikel.Rows[i].ItemArray[8]);
                                //angebotSembol = "";
                            }
                            ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                            BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                            BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[10].ToString();
                            BtnArtikel1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                            BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                            BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                            BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                            BtnArtikel1.Size = new System.Drawing.Size(162, 98);
                            BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
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
                            BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                            BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                            BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                            BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                            BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                            BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                            BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                            BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                            BtnArtikel1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                            /* if (Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]) < 0)
                             {
                                 BtnArtikel1.TabIndex = 0;
                             }
                             else
                             {
                                 BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                             }*/
                            //BtnArtikel1.Values.Text
                            BtnArtikel1.Values.Text = angebotSembol + dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + string.Format("{0:f}", (dtArtikel.Rows[i].ItemArray[8])) + "EUR/St\r " + " [" + dtArtikel.Rows[i].ItemArray[1] + "]\r";
                            // BtnArtikel1.Values.Text = angebotSembol+dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + dtArtikel.Rows[i].ItemArray[8].ToString() + "[" + dtArtikel.Rows[i].ItemArray[1] + "]"; 
                            BtnArtikel1.Tag = Info;//dtArtikel.Rows[i].ItemArray[8];
                            BtnArtikel1.Click += new EventHandler(GenericButton);

                            flpTop.Controls.Add(BtnArtikel1);

                        }

                    }
                }
            }
            catch
            {
            }
        }
        private void GenericButton(object sender, EventArgs e)
        {
            btnBestellen.Enabled = true;
            List<FlowLayoutPanel> panelList = new List<FlowLayoutPanel>();
            for (int i = 0; i < 10; i++)
            {
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.AutoSize = true;
                panelList.Add(panel);
            }
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string menuPLU = "";
            Control cont = (Control)sender;
            //MessageBox.Show(cont.GetType().ToString());
            Control btnsecilen;
            if (cont.GetType().ToString() == "ComponentFactory.Krypton.Toolkit.KryptonGroup")
            {
                btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonGroup;
                menuPLU = btnsecilen.Tag.ToString();
            }
            else if (cont.GetType().ToString() == "System.Windows.Forms.Panel")
            {
                btnsecilen = sender as Panel;
                menuPLU = btnsecilen.Tag.ToString();
            }
            else if (cont.GetType().ToString() == "System.Windows.Forms.Label")
            {
                btnsecilen = sender as Label;
                menuPLU = btnsecilen.Tag.ToString();
            }
            else if (cont.GetType().ToString() == "ComponentFactory.Krypton.Toolkit.KryptonPanel")
            {
                btnsecilen = sender as Label;
                menuPLU = btnsecilen.Tag.ToString();
            }
            else
            {
                return;
            }



            string menuSQL = "SELECT artikelmenu.*, (SELECT count(distinct sort) from artikelmenu WHERE menuPLU='" + menuPLU + "') as PanelCount   FROM artikelmenu WHERE menuPLU='" + menuPLU + "' ORDER BY sort";
            MySqlDataAdapter myDaMenu = new MySqlDataAdapter(menuSQL, connection);
            DataTable dtMenuPlu = new DataTable();
            myDaMenu.Fill(dtMenuPlu);
            int sortList = -1;
            int panelCount = 0;
            if (dtMenuPlu.Rows.Count > 0)
            {
                flpTop.Controls.Clear();

                FlowLayoutPanel aktifPanel = null;
                for (int a = 0; a < dtMenuPlu.Rows.Count; a++)
                {
                    ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup6 = new KryptonGroup();
                    ComponentFactory.Krypton.Toolkit.KryptonButton btnPreis = new KryptonButton();
                    ComponentFactory.Krypton.Toolkit.KryptonButton btnPreisPlus = new KryptonButton();
                    ComponentFactory.Krypton.Toolkit.KryptonButton btnPreisMinus = new KryptonButton();
                    System.Windows.Forms.Panel pnlInfo = new Panel();

                    System.Windows.Forms.Label lblInfo = new Label();
                    lblInfo.Text = "";
                    Label label6 = new Label();
                    Panel panel8 = new Panel();

                    if (Convert.ToInt16(dtMenuPlu.Rows[a].ItemArray[7]) != sortList)
                    {
                        if (aktifPanel != null)
                        {
                            flpTop.Controls.Add(aktifPanel);
                        }
                        sortList = Convert.ToInt16(dtMenuPlu.Rows[a].ItemArray[7]);
                        aktifPanel = panelList[panelCount];
                        aktifPanel.BackColor = Color.FromArgb(255, 255, 250 );
                        aktifPanel.WrapContents = true;
                        aktifPanel.AutoSize = true;
                        aktifPanel.Width = flpTop.Width;
                        aktifPanel.Controls.Add(pnlInfo);
                        //aktifPanel.MaximumSize = new Size(Screen.AllScreens[1].WorkingArea.Width, 350);
                        panelCount++;
                    }
                    //PLU Data
                    string SQLPlu = "SELECT * FROM artikel WHERE barkod='" + dtMenuPlu.Rows[a].ItemArray[1] + "'";
                    MySqlDataAdapter myDaPLU = new MySqlDataAdapter(SQLPlu, connection);
                    DataTable dtPLU = new DataTable();
                    myDaPLU.Fill(dtPLU);
                    Artikel artikel = new Artikel();
                    artikel.ArtikelBul(dtMenuPlu.Rows[a].ItemArray[1].ToString());
                    if (dtPLU.Rows.Count > 0)
                    {
                        pnlInfo.Controls.Add(lblInfo);
                        pnlInfo.Location = new System.Drawing.Point(3, 3);
                        pnlInfo.Name = "pnlInfo";
                        pnlInfo.Size = new System.Drawing.Size(667, 36);
                        pnlInfo.TabIndex = 7;
                        // 
                        // lblInfo
                        // 
                        lblInfo.AutoSize = true;
                        lblInfo.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        lblInfo.Location = new System.Drawing.Point(15, 4);
                        lblInfo.Name = "lblInfo";
                        lblInfo.Size = new System.Drawing.Size(248, 22);
                        lblInfo.TabIndex = 0;
                        lblInfo.Text = dtMenuPlu.Rows[a].ItemArray[10] + " Auswahl " + panelCount + "/" + dtMenuPlu.Rows[a].ItemArray[11];


                        // 
                        // kryptonGroup6
                        // 

                        kryptonGroup6.Location = new System.Drawing.Point(30, 30);
                        kryptonGroup6.Margin = new System.Windows.Forms.Padding(15);
                        kryptonGroup6.Name = "kryptonGroup6";
                        // 
                        // kryptonGroup6.Panel
                        // 
                        kryptonGroup6.Panel.Controls.Add(btnPreisMinus);
                        kryptonGroup6.Panel.Controls.Add(btnPreisPlus);
                        kryptonGroup6.Panel.Controls.Add(btnPreis);
                        kryptonGroup6.Panel.Controls.Add(label6);
                        kryptonGroup6.Panel.Controls.Add(panel8);
                        kryptonGroup6.Panel.Margin = new System.Windows.Forms.Padding(5);
                        kryptonGroup6.Panel.Padding = new System.Windows.Forms.Padding(3);
                        kryptonGroup6.Size = new System.Drawing.Size(305, 212);
                        kryptonGroup6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        kryptonGroup6.StateCommon.Border.Rounding = 3;
                        if (Convert.ToInt16(dtMenuPlu.Rows[a].ItemArray[6]) == 1)
                        {
                            kryptonGroup6.StateNormal.Border.Color1 = System.Drawing.Color.Green;
                        }
                        else
                        {
                            kryptonGroup6.StateNormal.Border.Color1 = System.Drawing.Color.Navy;
                        }
                        kryptonGroup6.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        kryptonGroup6.StateNormal.Border.Rounding = 5;
                        kryptonGroup6.StateNormal.Border.Width = 2;
                        kryptonGroup6.TabIndex = 4;
                        // 
                        // btnPreis
                        // 

                        btnPreis.Location = new System.Drawing.Point(232, 6);
                        btnPreis.Name = "btnPreis";
                        btnPreis.Size = new System.Drawing.Size(61, 29);
                        btnPreis.StateCommon.Back.Color1 = System.Drawing.Color.White;
                        btnPreis.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        btnPreis.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                        btnPreis.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        btnPreis.StateCommon.Border.Rounding = 4;
                        btnPreis.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnPreis.TabIndex = 4;
                        btnPreis.Values.Text = Convert.ToDouble(dtMenuPlu.Rows[a].ItemArray[4]).ToString("C");


                        // 
                        // btnPreisPlus
                        // 
                        Dictionary<string, Artikel> pluInfos = new Dictionary<string, Artikel>();
                        pluInfos.Add(menuPLU, artikel);
                        btnPreisPlus.Location = new System.Drawing.Point(7, 6);
                        btnPreisPlus.Name = "btnPreisPlus";
                        btnPreisPlus.Size = new System.Drawing.Size(74, 40);
                        btnPreisPlus.StateCommon.Back.Color1 = System.Drawing.Color.YellowGreen;
                        btnPreisPlus.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        btnPreisPlus.StateCommon.Border.Color1 = System.Drawing.Color.DarkGreen;
                        btnPreisPlus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        btnPreisPlus.StateCommon.Border.Rounding = 4;
                        btnPreisPlus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.YellowGreen;
                        btnPreisPlus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnPreisPlus.TabIndex = 5;
                        btnPreisPlus.Tag = pluInfos;
                        btnPreisPlus.Values.Text = "+";
                        btnPreisPlus.Click += new System.EventHandler(this.SatisEkle);
                        // 
                        // btnPreisMinus
                        // 

                        btnPreisMinus.Location = new System.Drawing.Point(7, 76);
                        btnPreisMinus.Name = "btnPreisMinus";
                        btnPreisMinus.Size = new System.Drawing.Size(74, 41);
                        btnPreisMinus.StateCommon.Back.Color1 = System.Drawing.Color.Orange;
                        btnPreisMinus.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                        btnPreisMinus.StateCommon.Border.Color1 = System.Drawing.Color.Crimson;
                        btnPreisMinus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        btnPreisMinus.StateCommon.Border.Rounding = 4;
                        btnPreisMinus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
                        btnPreisMinus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnPreisMinus.TabIndex = 6;
                        btnPreisMinus.Values.Text = "-";
                        // 
                        // label6
                        // 

                        label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        label6.Font = new System.Drawing.Font("Constantia", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        label6.Location = new System.Drawing.Point(9, 135);
                        label6.Name = "label6";
                        label6.Size = new System.Drawing.Size(287, 43);
                        label6.TabIndex = 1;
                        label6.Text = dtMenuPlu.Rows[a].ItemArray[9].ToString();
                        label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        // 
                        // panel8
                        // 

                        panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        if (dtPLU.Rows[0].ItemArray[19].ToString() == "")
                        {
                            panel8.BackgroundImage = global::IS_KASSE.Properties.Resources.NoImage;
                        }
                        else
                        {
                            try
                            {
                                panel8.BackgroundImage = Image.FromFile(dtPLU.Rows[0].ItemArray[19].ToString());
                            }
                            catch (Exception)
                            {
                                // continue;
                            }
                        }

                        panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        panel8.Location = new System.Drawing.Point(87, 6);
                        panel8.Name = "panel8";
                        panel8.Size = new System.Drawing.Size(139, 111);
                        //this.panel8.TabIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Artikel wurde nicht gefunden!");
                    }
                    aktifPanel.Height = kryptonGroup6.Height;
                    aktifPanel.Controls.Add(kryptonGroup6);
                }
                if (aktifPanel != null)
                {
                    flpTop.Controls.Add(aktifPanel);
                }

            }


        }
        private void SatisEkle(object sender, EventArgs e)
        {
            try
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                btnsecilen.StateNormal.Border.Width = 5;
                Dictionary<string, Artikel> pluInfos = new Dictionary<string, Artikel>();
                pluInfos = (Dictionary<string, Artikel>)btnsecilen.Tag;
                string menuPLU = pluInfos.Keys.ElementAt(0);
                Artikel artikel1 = pluInfos.Values.ElementAt(0);
                if (yeniFis == null)
                {
                    yeniFis = new FisOlustur();
                    yeniFis.FisYarat(0);
                    position = 1;
                    yeniFis.isSelbstKioskBestellung = 1;

                }
                satisYap = new SatisYap();
                satisYap.kioskBestellungMenuPLU = menuPLU;
                satisYap.Adet = 1;
                //satisYap.Fisno = yeniFis.SatisAnaId;

                satisYap.KasaNo = Program.kasano;
                satisYap.Mwst = artikel1.Mwst;
                satisYap.Ustid_id = satisYap.Mwst == Program.MwStList[1] ? ((int)UstIdEnum.mwst7) : (satisYap.Mwst == Program.MwStList[2] ? (int)UstIdEnum.mwst19 : (int)UstIdEnum.mwst0);
                satisYap.Barkod = artikel1.BarkodNo;
                satisYap.Satisfiyat = artikel1.VkPreis;
                satisYap.Tarih = (double)tarih.unixdate(DateTime.Now);
                satisYap.Toplamtutar = Math.Round(satisYap.Satisfiyat * satisYap.Adet, 2);
                satisYap.UrunId = (Decimal)artikel1.ArtikelId;
                satisYap.UrunAd = artikel1.ArtikelAd;
                satisYap.Birimkar = (satisYap.Satisfiyat - artikel1.EkPreis) * SatilanAdet;
                satisYap.Grubid = artikel1.Grubid;
                satisYap.Fand = artikel1.Fand;
                satisYap.Fand2 = artikel1.Fand2;
                satisYap.Angebotvarmi = artikel1.AngebotVarmi;
                satisYap.Angebotfiyat = artikel1.AngebotFiyati;
                satisYap.Gruptur = artikel1.Gruptur;
                satisYap.Fisno = yeniFis.SatisAnaId;
                yeniFis.SatisKalem.Add(satisYap);


                yeniFis.FisiKapat();
                btnBestellen.Text = "BESTELLUNG BESTÄTIGEN\n" + yeniFis.SatisKalem.Count + " Items -" + yeniFis.toplamtutar.ToString("C");
                lblToplam.Text = yeniFis.toplamtutar.ToString("C");

            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }

        }
        private void kryptonButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void kryptonGroup6_Panel_Click(object sender, EventArgs e)
        {
            MetroFramework.MetroMessageBox.Show(this, "Test");
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBearbeiten_Click(object sender, EventArgs e)
        {
            F_Kiosk_Bearbeiten bearbeitenForm = new F_Kiosk_Bearbeiten();
            bearbeitenForm.Location = Screen.AllScreens[1].WorkingArea.Location;
            bearbeitenForm.Width = 1080;
            bearbeitenForm.Height = 1920;
            bearbeitenForm.summe = yeniFis.toplamtutar;
            bearbeitenForm.yeniFis = yeniFis;

            bearbeitenForm.ShowDialog();
            yeniFis = bearbeitenForm.yeniFis;
            btnBestellen.Text = "BESTELLUNG BESTÄTIGEN\n3 Items -" + yeniFis.toplamtutar.ToString("C");
            lblToplam.Text = yeniFis.toplamtutar.ToString("C");

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (yeniFis != null)
            {
                yeniFis = null;
                btnBestellen.Enabled = false;
                btnBestellen.Text = "BESTELLUNG BESTÄTIGEN";
                lblToplam.Text = 0.ToString("C");

            }
        }
        private void DisplayFormShow(string Mesaj1, string Mesaj2)
        {
            if (Mesaj2 == "Web")
            {
                //webBrowser1.DocumentText = Mesaj1;
                //webBrowser1.Refresh();
            }
            else
            {
                CheckForIllegalCrossThreadCalls = false;
                if(Mesaj2=="F")
                {
                    cashdroOK = "OK";
                }
                //listBox1.Items.Add(Mesaj2 + "-->" + Mesaj1);
                // listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
            }
        }
        string cashdroOK = "";
        private void btnBestellen_Click(object sender, EventArgs e)
        {
            int imhaus = 0, odemeSekliBarEc = 0; //imhaus-> 1:auserhaus, 2: imhaus| odemeSekli 1:BAr 2 :Ec
            F_Kiosk_Zahlung zahlungsForm = new F_Kiosk_Zahlung();
            zahlungsForm.ShowDialog();
            imhaus = zahlungsForm.odemeSekli;
            if(imhaus==0)
            {
                return;
            }
            F_Kiosk_Bar_EC odemeForm = new F_Kiosk_Bar_EC();
            odemeForm.ShowDialog();
            odemeSekliBarEc = odemeForm.odemeSekliBarEc;
            btnBearbeiten.Enabled = true;
            if (imhaus != 0)
            {
                if (imhaus == 1)
                {
                    foreach (SatisYap sat in yeniFis.SatisKalem)
                    {
                        sat.Mwst = Program.MwStList[2];
                    }
                }
                if (odemeSekliBarEc == 1)
                {
                    string CashDroAntwort = "";
                    try
                    {
                        string betrag = yeniFis.toplamtutar.ToString("#0.00").Replace(",", "");
                        CashDroAntwort = main.CallSaleTransaction(betrag, "admin", "1", "Ali", "1");
                        while(cashdroOK!="OK")
                        {

                        }
                        return;

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
                                    try
                                    {
                                        yeniFis.ToplamScheck = 0.0;
                                        yeniFis.ToplamEc = fEconay.aktuelFis.toplamtutar;
                                        yeniFis.ToplamBar = 0.0;


                                        if (yeniFis.Musterino == 0L)
                                        {
                                            if (fEconay.harcananPuanKarsiligiHarcananPara == 0.0)
                                                goto label_24;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = ex.Message;
                                        int num2 = (int)fGenericError.ShowDialog();
                                    }
                                label_24:

                                    if (yeniFis.FisiSonlandır(1, yeniFis.SatisKalem))
                                    {


                                        foreach (SatisYap satisYap in yeniFis.SatisKalem)
                                        {
                                            if (satisYap.Barkod == null)
                                                satisYap.Barkod = "0";
                                            satisYap.Fisno = yeniFis.SatisAnaId;
                                            satisYap.Kaydet();
                                            satisYap.kioskBestellID = yeniFis.kioskSelbstBestellID;
                                            satisYap.BestellungKaydet();
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
                                            // logEntry.AddtoLogFile(ex.Message, " [1802]");
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

                                }
                            }
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["25"];
                            int num = (int)fGenericError.ShowDialog();
                        }
                    }
                    catch (Exception dd)
                    {
                        MessageBox.Show(dd.Message);
                    }

                }
                else if (odemeSekliBarEc == 2) //ec
                {
                    try
                    {

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
                                    try
                                    {
                                        yeniFis.ToplamScheck = 0.0;
                                        yeniFis.ToplamEc = fEconay.aktuelFis.toplamtutar;
                                        yeniFis.ToplamBar = 0.0;


                                        if (yeniFis.Musterino == 0L)
                                        {
                                            if (fEconay.harcananPuanKarsiligiHarcananPara == 0.0)
                                                goto label_24;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        F_GenericError fGenericError = new F_GenericError();
                                        fGenericError.lblMesaj.Text = ex.Message;
                                        int num2 = (int)fGenericError.ShowDialog();
                                    }
                                label_24:

                                    if (yeniFis.FisiSonlandır(0, yeniFis.SatisKalem))
                                    {


                                        foreach (SatisYap satisYap in yeniFis.SatisKalem)
                                        {
                                            if (satisYap.Barkod == null)
                                                satisYap.Barkod = "0";
                                            satisYap.Fisno = yeniFis.SatisAnaId;
                                            satisYap.Kaydet();
                                            satisYap.kioskBestellID = yeniFis.kioskSelbstBestellID;
                                            satisYap.BestellungKaydet();
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
                                            // logEntry.AddtoLogFile(ex.Message, " [1802]");
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

                                }
                            }
                        }
                        else
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = Program.lang["25"];
                            int num = (int)fGenericError.ShowDialog();
                        }

                    }
                    catch (Exception ff)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = ff.Message + "\n" + ff.StackTrace;
                        int num = (int)fGenericError.ShowDialog();
                    }
                }
                else if (odemeSekliBarEc == 3)//Bestellung
                {
                    yeniFis.BestellungSave();
                    foreach (SatisYap satisYap in yeniFis.SatisKalem)
                    {
                        if (satisYap.Barkod == null)
                            satisYap.Barkod = "0";
                        satisYap.Fisno = yeniFis.SatisAnaId;
                        satisYap.Kaydet();
                        satisYap.kioskBestellID = yeniFis.kioskSelbstBestellID;
                        satisYap.BestellungKaydet();
                    }
                    FisBarkodlu druck = new FisBarkodlu();
                    druck.basilacakFis = yeniFis;
                    druck.BestellungDruck();



                }
                Program.BonBeleg.basilacakFis = null;
                yeniFis = null;
                btnBestellen.Enabled = false;
                btnBestellen.Text = "BESTELLUNG BESTÄTIGEN";
                lblToplam.Text = 0.ToString("C");
            }




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
                /* if (eBonVerfiedCode == "")
                 {
                     F_ebonCodeEingabe fEbonCodeEingabe = new F_ebonCodeEingabe();
                     int num1 = (int)fEbonCodeEingabe.ShowDialog();
                     code = fEbonCodeEingabe.code;
                 }
                 else
                 {
                     code = eBonVerfiedCode;
                 }*
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
                                          listView1.Items.Clear();*9/
                                         return;
                                     }

                                     if (musteriNo != -1L)
                                     {

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

                                         musteri.db = (new db()).myconn();
                                         if (musteri.Kaydet() == true)
                                         {

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
                                     musteri.db = (new db()).myconn();


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
                  }*9/
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
        //float size = lblParaUstu.Font.Size;
        //Font lbfon = lblParaUstu.Font;
        // lblParaUstu.Font = new Font(lblParaUstu.Font.Name, 5f);
        // lblParaUstu.TextAlign = ContentAlignment.MiddleLeft;

        if (bon == "OK")
        {
            //lblParaUstu.Text = "e-Bon ist OK!";
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

        //lblParaUstu.TextAlign = ContentAlignment.MiddleRight;

    }9 */
                if (Program.PrinterLib == ".NET")
                {
                    if (Program.BonBeleg.basilacakFis != null)
                        new FisBarkodlu()
                        {
                            basilacakFis = Program.BonBeleg.basilacakFis,
                            OdemeTur = Program.BonBeleg.OdemeTur
                        }.FisYaz();
                }
                else
                    new FisBarkodlu()
                    {
                        basilacakFis = Program.BonBeleg.basilacakFis,
                        OdemeTur = Program.BonBeleg.OdemeTur
                    }.FisYaz();

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
                        //else
                        // new RechnungPrintClass().print(Program.BonBeleg.basilacakFatura, 1);
                    }
                    //yazilacakBon = null;
                }
                else
                {
                    if (Program.BonBeleg.basilacakFis.Musteri != null)
                    {
                        if (Program.BonBeleg.basilacakFis.Musteri.Kontotyp == 1)
                        {
                            //new RechnungPrintClass().printBonAlsRechnung(Program.BonBeleg.basilacakFatura, 1);
                        }
                        else
                        {
                            new FisBarkodlu()
                            {
                                basilacakFis = Program.BonBeleg.basilacakFis,
                                OdemeTur = Program.BonBeleg.OdemeTur
                            }.FisYaz();

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

                        Program.BonBeleg.basilacakFis = null;
                    }

                }
            }
            if (Program.StopWatch == 1)
            {
                StWaBonDruck.Stop();
                TimeSpan etts2 = StWaBonDruck.Elapsed;


            }
            this.Enabled = true;
        }
    }
}
