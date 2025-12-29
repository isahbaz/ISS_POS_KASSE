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

namespace IS_KASSE
{

    public partial class F_ObstGemuseManual : Form
    {
        GroupBox groupBox1 = new System.Windows.Forms.GroupBox();
        KryptonButton btnKaydet = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnSil = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonPanel kryptonPanel4 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
        KryptonButton btnNokta = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnDokuz = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnAlti = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnUc = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnCiftSifir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnSekiz = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnBes = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnIki = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnSifir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnYedi = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnDort = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        KryptonButton btnBir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        TextBox textBox1 = new System.Windows.Forms.TextBox();
        Panel panel1 = new System.Windows.Forms.Panel();

        TextBox aktifnesne = null;
        public int count = 0;
        public int limit = 32;
        public int grupid = 0;
        public double adet = 0;
        public double fiyat = 0;
        public double karmiktari = 0;
        public int artikelid = 0;
        public string UrunAd = "";
        public bool sonuc = false;
        DataTable dtArtikel = null;
        MySqlDataAdapter daArtikel = null;
        int rowCount = 0;
        public int ToplamKayitSay;
        public int ebene = 0;

        public F_ObstGemuseManual()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_ObstGemuse_Load(object sender, EventArgs e)
        {
            string apppath = Application.StartupPath;
            waageYarat();
            panel2.Controls.Add(this.groupBox1);
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
            aktifnesne = textBox1;

            //conn.Open();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();


            }
            /* if(
             MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 ORDER BY artikelad", conn);
             DataTable dtArtikel = new DataTable("artikelgrup");
             dtArtikel.Clear();
             daArtikel.Fill(dtArtikel);
             int rowCount = dtArtikel.Rows.Count;
             */
            if (count == 0)
            {
                using (conn)
                {
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 ORDER BY artikelad", conn);
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
                    daArtikel = new MySqlDataAdapter("SELECT * from artikel where grupid=" + grupid + " AND artikeltur<>1 ORDER BY artikelad LIMIT " + count + "," + limit, conn);
                    dtArtikel = new DataTable("artikelgrup");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    rowCount = dtArtikel.Rows.Count;
                }
            }
            if (rowCount > 0)
            {

                for (int i = 0; i < 16; i++)
                {

                    //ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                    /* BtnGrup.Location = new System.Drawing.Point(3, 3);
                     BtnGrup.Name = dtArtikel.Rows[i].ItemArray[0].ToString();
                     BtnGrup.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                     BtnGrup.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                 | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                 | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                     BtnGrup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                     BtnGrup.Size = new System.Drawing.Size(155, 100);
                     BtnGrup.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                     BtnGrup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                 | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                 | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                     BtnGrup.StateNormal.Border.Rounding = 6;
                     BtnGrup.StateNormal.Border.Width = 3;
                     BtnGrup.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Navy;
                     BtnGrup.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                     BtnGrup.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                     BtnGrup.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[3]);
                     BtnGrup.Values.Text = dtArtikel.Rows[i].ItemArray[1].ToString();
                     BtnGrup.Tag = dtArtikel.Rows[i].ItemArray[2];
                     /* */
                    try
                    {
                        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                        BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                        BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[0].ToString();
                        BtnArtikel1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                        BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                        BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                        BtnArtikel1.Size = new System.Drawing.Size(140, 98);
                        BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        if (dtArtikel.Rows[i].ItemArray[i].ToString() != "")
                        {
                            try
                            {
                                BtnArtikel1.StateNormal.Back.Image = Image.FromFile(dtArtikel.Rows[i].ItemArray[19].ToString());
                            }
                            catch { }
                        }
                        // BtnArtikel1.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.mayd1;
                        BtnArtikel1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                        BtnArtikel1.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                        BtnArtikel1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.StateNormal.Border.Rounding = 6;
                        BtnArtikel1.StateNormal.Border.Width = 3;
                        BtnArtikel1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                        BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                        BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                        BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                        BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                        BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                        BtnArtikel1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        if (Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]) < 0)
                        {
                            BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                        }
                        else
                        {
                            BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                        }
                        BtnArtikel1.Values.Text = dtArtikel.Rows[i].ItemArray[2].ToString();
                        BtnArtikel1.Tag = dtArtikel.Rows[i].ItemArray[8];
                        BtnArtikel1.Click += new EventHandler(GenericButton);

                        flp.Controls.Add(BtnArtikel1);
                    }
                    catch
                    {
                    }

                }
                for (int i = 16; i < limit; i++)
                {
                    try
                    {
                        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                        BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                        BtnArtikel1.Name = dtArtikel.Rows[i].ItemArray[0].ToString();
                        BtnArtikel1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                        BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                        BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                        BtnArtikel1.Size = new System.Drawing.Size(155, 100);
                        BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        if (dtArtikel.Rows[i].ItemArray[19].ToString() != "")
                        {
                            try
                            {
                                BtnArtikel1.StateNormal.Back.Image = Image.FromFile(dtArtikel.Rows[i].ItemArray[19].ToString());
                            }
                            catch { }
                        }
                        BtnArtikel1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
                        BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                        BtnArtikel1.StateNormal.Border.Color1 = System.Drawing.SystemColors.ActiveCaption;
                        BtnArtikel1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.StateNormal.Border.Rounding = 6;
                        BtnArtikel1.StateNormal.Border.Width = 3;
                        BtnArtikel1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Black;
                        BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                        BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                        BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                        BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                        BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                        BtnArtikel1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Rows[i].ItemArray[10]);
                        BtnArtikel1.Values.Text = dtArtikel.Rows[i].ItemArray[2].ToString();
                        BtnArtikel1.Tag = dtArtikel.Rows[i].ItemArray[8];
                        BtnArtikel1.Click += new EventHandler(GenericButton);

                        flp2.Controls.Add(BtnArtikel1);
                    }
                    catch
                    {
                    }
                }
                if (ToplamKayitSay > limit + count)
                {
                    
                
                    this.btnEbeneNext.Location = new System.Drawing.Point(2, 2);
                    this.btnEbeneNext.Name = "btnEbeneNext";
                    this.btnEbeneNext.Size = new System.Drawing.Size(162, 98);
                    this.btnEbeneNext.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    this.btnEbeneNext.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                    this.btnEbeneNext.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                    this.btnEbeneNext.TabIndex = 51;
                    this.btnEbeneNext.Values.Text = "WEITER";
                    this.btnEbeneNext.Click += new System.EventHandler(this.btnEbeneNext_Click);
                    flp2.Controls.Add(btnEbeneNext);

                }
            }
            this.kryptonButton4.Location = new System.Drawing.Point(2, 2);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Size = new System.Drawing.Size(164, 98);
            this.kryptonButton4.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton4.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton4.TabIndex = 51;
            this.kryptonButton4.Values.Text = "ABBRUCH";
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
            flp2.Controls.Add(kryptonButton4);



        }

        private void waageYarat()
        {

            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).BeginInit();
            this.kryptonPanel4.SuspendLayout();
            this.SuspendLayout();

            this.btnNokta.Location = new System.Drawing.Point(252, 186);
            this.btnNokta.Margin = new System.Windows.Forms.Padding(0);
            this.btnNokta.Name = "btnNokta";
            this.btnNokta.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnNokta.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnNokta.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnNokta.Size = new System.Drawing.Size(106, 66);
            this.btnNokta.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnNokta.StateNormal.Border.Rounding = 10;
            this.btnNokta.StateNormal.Border.Width = 5;
            this.btnNokta.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnNokta.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnNokta.TabIndex = 32;
            this.btnNokta.Values.Text = "C";
            this.btnNokta.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnDokuz
            // 
            this.btnDokuz.Location = new System.Drawing.Point(252, 127);
            this.btnDokuz.Name = "btnDokuz";
            this.btnDokuz.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnDokuz.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDokuz.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnDokuz.Size = new System.Drawing.Size(106, 57);
            this.btnDokuz.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDokuz.StateNormal.Border.Rounding = 10;
            this.btnDokuz.StateNormal.Border.Width = 5;
            this.btnDokuz.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnDokuz.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDokuz.TabIndex = 31;
            this.btnDokuz.Values.Text = "9";
            this.btnDokuz.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnAlti
            // 
            this.btnAlti.Location = new System.Drawing.Point(252, 67);
            this.btnAlti.Name = "btnAlti";
            this.btnAlti.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnAlti.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAlti.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnAlti.Size = new System.Drawing.Size(104, 57);
            this.btnAlti.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAlti.StateNormal.Border.Rounding = 10;
            this.btnAlti.StateNormal.Border.Width = 5;
            this.btnAlti.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnAlti.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAlti.TabIndex = 30;
            this.btnAlti.Values.Text = "6";
            this.btnAlti.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnUc
            // 
            this.btnUc.Location = new System.Drawing.Point(252, 7);
            this.btnUc.Name = "btnUc";
            this.btnUc.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnUc.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUc.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnUc.Size = new System.Drawing.Size(104, 57);
            this.btnUc.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUc.StateNormal.Border.Rounding = 10;
            this.btnUc.StateNormal.Border.Width = 5;
            this.btnUc.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnUc.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnUc.TabIndex = 29;
            this.btnUc.Values.Text = "3";
            this.btnUc.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnKaydet);
            this.groupBox1.Controls.Add(this.kryptonButton4);
            this.groupBox1.Controls.Add(this.kryptonPanel4);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 419);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = Program.lang["44"];
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(227, 12);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(138, 73);
            this.btnKaydet.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnKaydet.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.TabIndex = 57;
            this.btnKaydet.Values.Text = Program.lang["55"];
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // kryptonButton4
            // 
            this.kryptonButton4.Location = new System.Drawing.Point(227, 91);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Size = new System.Drawing.Size(138, 58);
            this.kryptonButton4.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton4.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton4.TabIndex = 56;
            this.kryptonButton4.Values.Text = Program.lang["56"];
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
            // 
            // kryptonPanel4
            // 
            this.kryptonPanel4.Controls.Add(this.btnNokta);
            this.kryptonPanel4.Controls.Add(this.btnDokuz);
            this.kryptonPanel4.Controls.Add(this.btnAlti);
            this.kryptonPanel4.Controls.Add(this.btnUc);
            this.kryptonPanel4.Controls.Add(this.btnCiftSifir);
            this.kryptonPanel4.Controls.Add(this.btnSekiz);
            this.kryptonPanel4.Controls.Add(this.btnBes);
            this.kryptonPanel4.Controls.Add(this.btnIki);
            this.kryptonPanel4.Controls.Add(this.btnSifir);
            this.kryptonPanel4.Controls.Add(this.btnYedi);
            this.kryptonPanel4.Controls.Add(this.btnDort);
            this.kryptonPanel4.Controls.Add(this.btnBir);
            this.kryptonPanel4.Location = new System.Drawing.Point(5, 155);
            this.kryptonPanel4.Name = "kryptonPanel4";
            this.kryptonPanel4.Size = new System.Drawing.Size(360, 258);
            this.kryptonPanel4.TabIndex = 47;
            // 
            // btnCiftSifir
            // 
            this.btnCiftSifir.Location = new System.Drawing.Point(134, 187);
            this.btnCiftSifir.Name = "btnCiftSifir";
            this.btnCiftSifir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnCiftSifir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCiftSifir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnCiftSifir.Size = new System.Drawing.Size(113, 66);
            this.btnCiftSifir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCiftSifir.StateNormal.Border.Rounding = 10;
            this.btnCiftSifir.StateNormal.Border.Width = 5;
            this.btnCiftSifir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnCiftSifir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCiftSifir.TabIndex = 28;
            this.btnCiftSifir.Values.Text = "00";
            this.btnCiftSifir.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnSekiz
            // 
            this.btnSekiz.Location = new System.Drawing.Point(134, 127);
            this.btnSekiz.Name = "btnSekiz";
            this.btnSekiz.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnSekiz.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSekiz.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnSekiz.Size = new System.Drawing.Size(114, 57);
            this.btnSekiz.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSekiz.StateNormal.Border.Rounding = 10;
            this.btnSekiz.StateNormal.Border.Width = 5;
            this.btnSekiz.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnSekiz.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSekiz.TabIndex = 27;
            this.btnSekiz.Values.Text = "8";
            this.btnSekiz.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnBes
            // 
            this.btnBes.Location = new System.Drawing.Point(134, 67);
            this.btnBes.Name = "btnBes";
            this.btnBes.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnBes.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBes.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnBes.Size = new System.Drawing.Size(114, 57);
            this.btnBes.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBes.StateNormal.Border.Rounding = 10;
            this.btnBes.StateNormal.Border.Width = 5;
            this.btnBes.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnBes.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBes.TabIndex = 26;
            this.btnBes.Values.Text = "5";
            this.btnBes.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnIki
            // 
            this.btnIki.Location = new System.Drawing.Point(134, 7);
            this.btnIki.Margin = new System.Windows.Forms.Padding(0);
            this.btnIki.Name = "btnIki";
            this.btnIki.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnIki.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnIki.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnIki.Size = new System.Drawing.Size(113, 57);
            this.btnIki.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnIki.StateNormal.Border.Rounding = 10;
            this.btnIki.StateNormal.Border.Width = 5;
            this.btnIki.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnIki.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIki.TabIndex = 25;
            this.btnIki.Values.Text = "2";
            this.btnIki.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnSifir
            // 
            this.btnSifir.Location = new System.Drawing.Point(3, 187);
            this.btnSifir.Name = "btnSifir";
            this.btnSifir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnSifir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSifir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnSifir.Size = new System.Drawing.Size(124, 66);
            this.btnSifir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSifir.StateNormal.Border.Rounding = 10;
            this.btnSifir.StateNormal.Border.Width = 5;
            this.btnSifir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnSifir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSifir.TabIndex = 24;
            this.btnSifir.Values.Text = "0";
            this.btnSifir.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnYedi
            // 
            this.btnYedi.Location = new System.Drawing.Point(4, 127);
            this.btnYedi.Name = "btnYedi";
            this.btnYedi.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnYedi.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYedi.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnYedi.Size = new System.Drawing.Size(124, 57);
            this.btnYedi.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYedi.StateNormal.Border.Rounding = 10;
            this.btnYedi.StateNormal.Border.Width = 5;
            this.btnYedi.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnYedi.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYedi.TabIndex = 23;
            this.btnYedi.Values.Text = "7";
            this.btnYedi.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnDort
            // 
            this.btnDort.Location = new System.Drawing.Point(4, 67);
            this.btnDort.Name = "btnDort";
            this.btnDort.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnDort.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDort.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnDort.Size = new System.Drawing.Size(124, 57);
            this.btnDort.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDort.StateNormal.Border.Rounding = 10;
            this.btnDort.StateNormal.Border.Width = 5;
            this.btnDort.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnDort.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDort.TabIndex = 22;
            this.btnDort.Values.Text = "4";
            this.btnDort.Click += new System.EventHandler(this.btnUc_Click);
            // 
            // btnBir
            // 
            this.btnBir.Location = new System.Drawing.Point(4, 7);
            this.btnBir.Name = "btnBir";
            this.btnBir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnBir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnBir.Size = new System.Drawing.Size(124, 57);
            this.btnBir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBir.StateNormal.Border.Rounding = 10;
            this.btnBir.StateNormal.Border.Width = 5;
            this.btnBir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnBir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBir.TabIndex = 21;
            this.btnBir.Values.Text = "1";
            this.btnBir.Click += new System.EventHandler(this.btnUc_Click);

            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.Location = new System.Drawing.Point(5, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 105);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
        }
        private void GenericButton(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                double yeniadet = 0;

                if (double.TryParse(textBox1.Text, out adet))
                {
                    int uzu = textBox1.Text.Length;
                    if (uzu < 3)
                    {
                        while (textBox1.Text.Length < 4)
                        {
                            textBox1.Text = "0" + textBox1.Text;
                        }
                    }
                    string virgullu = textBox1.Text.Substring(textBox1.Text.Length - 3, 3);
                    string tam = textBox1.Text.Substring(0, textBox1.Text.Length - 3);
                    string fiyatvirgullu = tam + "," + virgullu;
                    if (double.TryParse(fiyatvirgullu, out yeniadet))
                    {
                        adet = yeniadet;
                        ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                        fiyat = Convert.ToDouble(btnsecilen.Tag);
                        artikelid = Convert.ToInt32(btnsecilen.Name);
                        karmiktari = Math.Round(Convert.ToDouble(btnsecilen.TabIndex) * adet, 2);
                        UrunAd = btnsecilen.Values.Text;
                        sonuc = true;
                        this.Close();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["40"];
                    frmerror.ShowDialog();
                }

            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["39"];
                frmerror.ShowDialog();
            }
            /* if (adet != 0)
             {
                 ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                 fiyat = Convert.ToDouble(btnsecilen.Tag);
                 artikelid = Convert.ToInt32(btnsecilen.Name);
                 karmiktari = Convert.ToDouble(btnsecilen.TabIndex);
                 UrunAd = btnsecilen.Values.Text;




                 sonuc = true;
                 this.Close();
             }
             else
             {
                 F_GenericError frmerror = new F_GenericError();
                 frmerror.lblMesaj.Text = "AGIRLIK ALANINA HATALI BİLGİ GİRDİNİZ!";
                 frmerror.ShowDialog();
             }*/
        }
        private void btnSil_Click(object sender, EventArgs e)
        {

            this.textBox1.Text = "";

        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
            aktifnesne = textBox1;
        }
        private void btnUc_Click(object sender, EventArgs e)
        {
            if (aktifnesne != null)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btn = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;

                int sayi;
                if (int.TryParse(btn.Text, out sayi))
                {
                    aktifnesne.Text += sayi.ToString();
                }
            }
        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            sonuc = false;
            this.Close();
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                double yeniadet = 0;

                if (double.TryParse(textBox1.Text, out adet))
                {
                    int uzu = textBox1.Text.Length;
                    if (uzu < 3)
                    {
                        while (textBox1.Text.Length < 4)
                        {
                            textBox1.Text = "0" + textBox1.Text;
                        }
                    }
                    string virgullu = textBox1.Text.Substring(textBox1.Text.Length - 3, 3);
                    string tam = textBox1.Text.Substring(0, textBox1.Text.Length - 3);
                    string fiyatvirgullu = tam + "," + virgullu;
                    if (double.TryParse(fiyatvirgullu, out yeniadet))
                    {
                        adet = yeniadet;
                        sonuc = true;
                        this.Close();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["40"];
                    frmerror.ShowDialog();
                }

            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["42"];
                frmerror.ShowDialog();
            }


        }

        private void btnEbeneNext_Click(object sender, EventArgs e)
        {

            F_ObstGemuseManual frmobstGemuseStuck = new F_ObstGemuseManual();
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
            if (frmobstGemuseStuck.sonuc == true)
            {
                fiyat = frmobstGemuseStuck.fiyat;
                adet = frmobstGemuseStuck.adet;
                //WaageTutar = frmobstGemuseStuck.WaageTutar;
                artikelid = frmobstGemuseStuck.artikelid;
                karmiktari = frmobstGemuseStuck.karmiktari;
                UrunAd = frmobstGemuseStuck.UrunAd;
                sonuc = true;
                this.Close();
            }
        }

    }
}
