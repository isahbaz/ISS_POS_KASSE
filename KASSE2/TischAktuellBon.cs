using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Globalization;
using Microsoft.PointOfService;
using MySql.Data.MySqlClient;
using iss_Rabat;

namespace IS_KASSE
{
    class TischAktuellBon
    {

        public List<Panel> panel = new List<Panel>();
        public FisOlustur fis = null;
        int seciliItem = -1;
        int position = 1;
        public double verilenPara = 0;
        SatisYap satisYap;
        Tarih tarih = new Tarih();
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        public delegate void KundenDisplayDelagateFisInhalt(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verilenPara, double paraUstu, int odemetur);
        public event KundenDisplayDelagateFisInhalt KundenDisplayFisInhaltEvent;
        public Dictionary<int, FisOlustur> MevcutMasalar = new Dictionary<int, FisOlustur>();

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnYukari;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAsagi;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBonNo;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtToplam;
        private System.Windows.Forms.Panel panelTotal1;
        public System.Windows.Forms.Label lblTutar;
        private System.Windows.Forms.Panel panelOdemetip;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBar;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEC;
        public System.Windows.Forms.Label lblRuckgeld;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel bewirtungPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox cbBewirtung;
        private ComponentFactory.Krypton.Toolkit.KryptonButton pozOdeme;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUmbuchung;

        private List<Panel> BonGoruntu()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Test));
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "",
            ""}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Info, new System.Drawing.Font("Verdana", 9F));
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "",
            ""}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Info, new System.Drawing.Font("Verdana", 9F));
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Info, new System.Drawing.Font("Tahoma", 9.75F));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.kryptonButton3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnYukari = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAsagi = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblBonNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtToplam = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.panelTotal1 = new System.Windows.Forms.Panel();
            this.lblTutar = new System.Windows.Forms.Label();
            this.panelOdemetip = new System.Windows.Forms.Panel();
            this.btnBar = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEC = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblRuckgeld = new System.Windows.Forms.Label();
            this.bewirtungPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.cbBewirtung = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.pozOdeme = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnUmbuchung = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelTotal1.SuspendLayout();
            this.panelOdemetip.SuspendLayout();

            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.panel1.Controls.Add(this.btnUmbuchung);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panelTotal1);
            this.panel1.Controls.Add(this.panelOdemetip);
            this.panel1.Controls.Add(this.bewirtungPanel);
            this.panel1.Controls.Add(this.pozOdeme);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1010, 400);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.kryptonButton3);
            this.panel2.Controls.Add(this.kryptonButton2);
            this.panel2.Controls.Add(this.kryptonButton1);
            this.panel2.Controls.Add(this.btnYukari);
            this.panel2.Controls.Add(this.btnAsagi);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(550, 398);
            this.panel2.TabIndex = 5;
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton3.Location = new System.Drawing.Point(474, 161);
            this.kryptonButton3.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonButton3.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton3.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton3.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton3.Size = new System.Drawing.Size(70, 55);
            this.kryptonButton3.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
            this.kryptonButton3.StateCommon.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.BottomLeft;
            this.kryptonButton3.StateCommon.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton3.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton3.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton3.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisCharacter;
            this.kryptonButton3.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton3.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton3.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton3.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton3.StateNormal.Back.Image")));
            this.kryptonButton3.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton3.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton3.StateNormal.Border.Color2 = System.Drawing.Color.White;
            this.kryptonButton3.StateNormal.Border.ColorAngle = 0F;
            this.kryptonButton3.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.SolidInside;
            this.kryptonButton3.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.StateNormal.Border.Rounding = 5;
            this.kryptonButton3.StateNormal.Border.Width = 1;
            this.kryptonButton3.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton3.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopRight;
            this.kryptonButton3.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton3.StateNormal.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton3.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton3.StateTracking.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton3.StateTracking.Back.Image")));
            this.kryptonButton3.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton3.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton3.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.StateTracking.Border.Rounding = 2;
            this.kryptonButton3.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton3.TabIndex = 52;
            this.kryptonButton3.Values.Text = "";
            this.kryptonButton3.Click += new EventHandler(kryptonButton3_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton2.Location = new System.Drawing.Point(475, 85);
            this.kryptonButton2.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonButton2.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton2.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton2.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton2.Size = new System.Drawing.Size(69, 55);
            this.kryptonButton2.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
            this.kryptonButton2.StateCommon.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton2.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.BottomLeft;
            this.kryptonButton2.StateCommon.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisCharacter;
            this.kryptonButton2.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton2.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton2.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton2.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton2.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton2.StateNormal.Back.Image")));
            this.kryptonButton2.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton2.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton2.StateNormal.Border.Color1 = System.Drawing.Color.White;
            this.kryptonButton2.StateNormal.Border.Color2 = System.Drawing.Color.MediumBlue;
            this.kryptonButton2.StateNormal.Border.ColorAngle = 0F;
            this.kryptonButton2.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.SolidInside;
            this.kryptonButton2.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateNormal.Border.Rounding = 5;
            this.kryptonButton2.StateNormal.Border.Width = 1;
            this.kryptonButton2.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton2.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopRight;
            this.kryptonButton2.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton2.StateNormal.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton2.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton2.StateTracking.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton2.StateTracking.Back.Image")));
            this.kryptonButton2.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton2.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton2.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateTracking.Border.Rounding = 2;
            this.kryptonButton2.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.TabIndex = 51;
            this.kryptonButton2.Values.Text = "";
            this.kryptonButton2.Click += new EventHandler(kryptonButton2_Click);
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton1.Location = new System.Drawing.Point(475, 26);
            this.kryptonButton1.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonButton1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton1.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton1.Size = new System.Drawing.Size(69, 55);
            this.kryptonButton1.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
            this.kryptonButton1.StateCommon.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton1.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.BottomLeft;
            this.kryptonButton1.StateCommon.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisCharacter;
            this.kryptonButton1.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton1.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton1.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton1.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton1.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton1.StateNormal.Back.Image")));
            this.kryptonButton1.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton1.StateNormal.Border.Color1 = System.Drawing.Color.White;
            this.kryptonButton1.StateNormal.Border.Color2 = System.Drawing.Color.DarkBlue;
            this.kryptonButton1.StateNormal.Border.ColorAngle = 0F;
            this.kryptonButton1.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.SolidRightLine;
            this.kryptonButton1.StateNormal.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateNormal.Border.Rounding = 5;
            this.kryptonButton1.StateNormal.Border.Width = 1;
            this.kryptonButton1.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton1.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton1.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton1.StateNormal.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopRight;
            this.kryptonButton1.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton1.StateNormal.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton1.StateTracking.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton1.StateTracking.Back.Image")));
            this.kryptonButton1.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton1.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton1.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateTracking.Border.Rounding = 2;
            this.kryptonButton1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton1.TabIndex = 50;
            this.kryptonButton1.Values.Text = "";
            this.kryptonButton1.Click += new EventHandler(kryptonButton1_Click);
            // 
            // btnYukari
            // 
            this.btnYukari.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnYukari.Location = new System.Drawing.Point(475, 245);
            this.btnYukari.Margin = new System.Windows.Forms.Padding(0);
            this.btnYukari.Name = "btnYukari";
            this.btnYukari.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.btnYukari.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnYukari.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYukari.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnYukari.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYukari.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnYukari.Size = new System.Drawing.Size(69, 55);
            this.btnYukari.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
            this.btnYukari.StateCommon.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnYukari.StateCommon.Content.ShortText.Image = ((System.Drawing.Image)(resources.GetObject("btnYukari.StateCommon.Content.ShortText.Image")));
            this.btnYukari.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.BottomLeft;
            this.btnYukari.StateCommon.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisCharacter;
            this.btnYukari.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnYukari.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnYukari.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnYukari.StateNormal.Back.ColorAngle = 50F;
            this.btnYukari.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnYukari.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("btnYukari.StateNormal.Back.Image")));
            this.btnYukari.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnYukari.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnYukari.StateNormal.Border.Color1 = System.Drawing.Color.White;
            this.btnYukari.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnYukari.StateNormal.Border.ColorAngle = 0F;
            this.btnYukari.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.GlassFade;
            this.btnYukari.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYukari.StateNormal.Border.Rounding = 5;
            this.btnYukari.StateNormal.Border.Width = 1;
            this.btnYukari.StateNormal.Content.AdjacentGap = 0;
            this.btnYukari.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnYukari.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnYukari.StateNormal.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopRight;
            this.btnYukari.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnYukari.StateNormal.Content.ShortText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnYukari.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYukari.StateNormal.Content.ShortText.Image = ((System.Drawing.Image)(resources.GetObject("btnYukari.StateNormal.Content.ShortText.Image")));
            this.btnYukari.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnYukari.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnYukari.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
            this.btnYukari.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnYukari.StateNormal.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.EllipsisCharacter;
            this.btnYukari.StateTracking.Back.Image = ((System.Drawing.Image)(resources.GetObject("btnYukari.StateTracking.Back.Image")));
            this.btnYukari.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnYukari.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnYukari.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYukari.StateTracking.Border.Rounding = 2;
            this.btnYukari.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYukari.TabIndex = 49;
            this.btnYukari.Values.Text = "";
            this.btnYukari.Click += new EventHandler(btnYukari_Click);
            // 
            // btnAsagi
            // 
            this.btnAsagi.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnAsagi.Location = new System.Drawing.Point(474, 305);
            this.btnAsagi.Margin = new System.Windows.Forms.Padding(0);
            this.btnAsagi.Name = "btnAsagi";
            this.btnAsagi.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnAsagi.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAsagi.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnAsagi.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAsagi.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnAsagi.Size = new System.Drawing.Size(70, 55);
            this.btnAsagi.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAsagi.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnAsagi.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnAsagi.StateNormal.Back.ColorAngle = 50F;
            this.btnAsagi.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnAsagi.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("btnAsagi.StateNormal.Back.Image")));
            this.btnAsagi.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnAsagi.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnAsagi.StateNormal.Border.Color1 = System.Drawing.Color.White;
            this.btnAsagi.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnAsagi.StateNormal.Border.ColorAngle = 0F;
            this.btnAsagi.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.SolidInside;
            this.btnAsagi.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAsagi.StateNormal.Border.Rounding = 5;
            this.btnAsagi.StateNormal.Border.Width = 1;
            this.btnAsagi.StateNormal.Content.AdjacentGap = 0;
            this.btnAsagi.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnAsagi.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnAsagi.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnAsagi.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAsagi.StateTracking.Back.Image = ((System.Drawing.Image)(resources.GetObject("btnAsagi.StateTracking.Back.Image")));
            this.btnAsagi.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnAsagi.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnAsagi.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAsagi.StateTracking.Border.Rounding = 2;
            this.btnAsagi.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAsagi.TabIndex = 48;
            this.btnAsagi.Values.Text = "";
            this.btnAsagi.Click += new EventHandler(btnAsagi_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Controls.Add(this.lblBonNo);
            this.panel3.Controls.Add(this.txtToplam);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(471, 398);
            this.panel3.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listView1.AutoArrange = false;
            this.listView1.BackColor = System.Drawing.SystemColors.Info;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem10,
            listViewItem11,
            listViewItem12});
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(471, 350);
            this.listView1.TabIndex = 10;
            this.listView1.TileSize = new System.Drawing.Size(228, 70);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SNO";
            this.columnHeader1.Width = 25;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Artikel Ad";
            this.columnHeader2.Width = 285;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Fiyat";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 75;
            // 
            // lblBonNo
            // 
            this.lblBonNo.Location = new System.Drawing.Point(363, 51);
            this.lblBonNo.Name = "lblBonNo";
            this.lblBonNo.Size = new System.Drawing.Size(65, 18);
            this.lblBonNo.StateNormal.ShortText.Color1 = System.Drawing.Color.White;
            this.lblBonNo.StateNormal.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblBonNo.TabIndex = 2;
            this.lblBonNo.Values.Text = "00000000";

            // 
            // panelTotal1
            // 
            this.panelTotal1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelTotal1.Controls.Add(this.lblTutar);
            this.panelTotal1.Controls.Add(this.lblRuckgeld);
            this.panelTotal1.Location = new System.Drawing.Point(609, 16);
            this.panelTotal1.Name = "panelTotal1";
            this.panelTotal1.Size = new System.Drawing.Size(386, 102);
            this.panelTotal1.TabIndex = 7;
            // 
            // lblTutar
            // 
            this.lblTutar.BackColor = System.Drawing.Color.LightBlue;
            this.lblTutar.Font = new System.Drawing.Font("Verdana", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTutar.Location = new System.Drawing.Point(3, 0);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(380, 54);
            this.lblTutar.TabIndex = 3;
            this.lblTutar.Text = "label1";
            this.lblTutar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelOdemetip
            // 
            this.panelOdemetip.Controls.Add(this.btnBar);
            this.panelOdemetip.Controls.Add(this.btnEC);
            this.panelOdemetip.Location = new System.Drawing.Point(603, 128);
            this.panelOdemetip.Name = "panelOdemetip";
            this.panelOdemetip.Size = new System.Drawing.Size(393, 100);
            this.panelOdemetip.TabIndex = 6;
            // 
            // btnBar
            // 
            this.btnBar.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnBar.Location = new System.Drawing.Point(0, 0);
            this.btnBar.Margin = new System.Windows.Forms.Padding(0);
            this.btnBar.Name = "btnBar";
            this.btnBar.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnBar.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBar.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnBar.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBar.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnBar.Size = new System.Drawing.Size(224, 100);
            this.btnBar.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnBar.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnBar.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnBar.StateNormal.Back.ColorAngle = 50F;
            this.btnBar.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnBar.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnBar.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnBar.StateNormal.Border.Color1 = System.Drawing.Color.Brown;
            this.btnBar.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnBar.StateNormal.Border.ColorAngle = 5F;
            this.btnBar.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnBar.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBar.StateNormal.Border.Rounding = 5;
            this.btnBar.StateNormal.Border.Width = 5;
            this.btnBar.StateNormal.Content.AdjacentGap = 0;
            this.btnBar.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnBar.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnBar.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnBar.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBar.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnBar.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnBar.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBar.StateTracking.Border.Rounding = 2;
            this.btnBar.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBar.TabIndex = 57;
            this.btnBar.Values.Text = "BAR";
            this.btnBar.Click += new EventHandler(btnBar_Click);
            // 
            // btnEC
            // 
            this.btnEC.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnEC.Location = new System.Drawing.Point(238, 0);
            this.btnEC.Margin = new System.Windows.Forms.Padding(0);
            this.btnEC.Name = "btnEC";
            this.btnEC.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnEC.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEC.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnEC.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEC.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnEC.Size = new System.Drawing.Size(155, 100);
            this.btnEC.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnEC.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnEC.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnEC.StateNormal.Back.ColorAngle = 50F;
            this.btnEC.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnEC.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnEC.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnEC.StateNormal.Border.Color1 = System.Drawing.Color.Brown;
            this.btnEC.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnEC.StateNormal.Border.ColorAngle = 5F;
            this.btnEC.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnEC.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEC.StateNormal.Border.Rounding = 5;
            this.btnEC.StateNormal.Border.Width = 5;
            this.btnEC.StateNormal.Content.AdjacentGap = 0;
            this.btnEC.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnEC.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnEC.StateNormal.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEC.StateNormal.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEC.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnEC.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEC.StateNormal.Content.ShortText.Image = ((System.Drawing.Image)(resources.GetObject("btnEC.StateNormal.Content.ShortText.Image")));
            this.btnEC.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnEC.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnEC.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnEC.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEC.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEC.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnEC.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnEC.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEC.StateTracking.Border.Rounding = 2;
            this.btnEC.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEC.TabIndex = 53;
            this.btnEC.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnEC.Values.Image")));
            this.btnEC.Values.Text = "";
            this.btnEC.Click += new EventHandler(btnEC_Click);

            this.lblRuckgeld.BackColor = System.Drawing.Color.LightBlue;
            this.lblRuckgeld.Font = new System.Drawing.Font("Verdana", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRuckgeld.Location = new System.Drawing.Point(102, 58);
            this.lblRuckgeld.Name = "lblRuckgeld";
            this.lblRuckgeld.Size = new System.Drawing.Size(170, 37);
            this.lblRuckgeld.TabIndex = 4;
            this.lblRuckgeld.Text = "label1";
            this.lblRuckgeld.Visible = false;
            this.lblRuckgeld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

           

            this.bewirtungPanel.Controls.Add(this.cbBewirtung);
            this.bewirtungPanel.Location = new System.Drawing.Point(668, 324);
            this.bewirtungPanel.Name = "bewirtungPanel";
            this.bewirtungPanel.Size = new System.Drawing.Size(225, 59);
            this.bewirtungPanel.TabIndex = 8;
            // 
            // cbBewirtung
            // 
           // this.cbBewirtung.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            this.cbBewirtung.Location = new System.Drawing.Point(2, 9);
            this.cbBewirtung.Name = "cbBewirtung";
            this.cbBewirtung.Size = new System.Drawing.Size(220, 41);
            this.cbBewirtung.StateNormal.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBewirtung.TabIndex = 0;
            this.cbBewirtung.Text = "BEWIRTUNG";
            this.cbBewirtung.Values.Text = "BEWIRTUNG";
            this.cbBewirtung.CheckedChanged += new System.EventHandler(this.kryptonCheckBox1_CheckedChanged);

            this.pozOdeme.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.pozOdeme.Location = new System.Drawing.Point(643, 231);
            this.pozOdeme.Margin = new System.Windows.Forms.Padding(0);
            this.pozOdeme.Name = "pozOdeme";
            this.pozOdeme.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.pozOdeme.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.pozOdeme.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.pozOdeme.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.pozOdeme.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.pozOdeme.Size = new System.Drawing.Size(132, 86);
            this.pozOdeme.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pozOdeme.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.pozOdeme.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.pozOdeme.StateNormal.Back.ColorAngle = 50F;
            this.pozOdeme.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.pozOdeme.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.pozOdeme.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.pozOdeme.StateNormal.Border.Color1 = System.Drawing.Color.OliveDrab;
            this.pozOdeme.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.pozOdeme.StateNormal.Border.ColorAngle = 5F;
            this.pozOdeme.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.pozOdeme.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.pozOdeme.StateNormal.Border.Rounding = 5;
            this.pozOdeme.StateNormal.Border.Width = 5;
            this.pozOdeme.StateNormal.Content.AdjacentGap = 0;
            this.pozOdeme.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.pozOdeme.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.pozOdeme.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.pozOdeme.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.pozOdeme.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.pozOdeme.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.pozOdeme.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.pozOdeme.StateTracking.Border.Rounding = 2;
            this.pozOdeme.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.pozOdeme.TabIndex = 58;
            this.pozOdeme.Values.Text = "POS. BEZAHLEN";
            this.pozOdeme.Click += new EventHandler(pozOdeme_Click);
            // 
            // btnUmbuchung
            // 
            this.btnUmbuchung.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnUmbuchung.Location = new System.Drawing.Point(856, 231);
            this.btnUmbuchung.Margin = new System.Windows.Forms.Padding(0);
            this.btnUmbuchung.Name = "btnUmbuchung";
            this.btnUmbuchung.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnUmbuchung.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUmbuchung.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnUmbuchung.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUmbuchung.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnUmbuchung.Size = new System.Drawing.Size(119, 86);
            this.btnUmbuchung.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnUmbuchung.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnUmbuchung.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnUmbuchung.StateNormal.Back.ColorAngle = 50F;
            this.btnUmbuchung.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnUmbuchung.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnUmbuchung.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnUmbuchung.StateNormal.Border.Color1 = System.Drawing.Color.OliveDrab;
            this.btnUmbuchung.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnUmbuchung.StateNormal.Border.ColorAngle = 5F;
            this.btnUmbuchung.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnUmbuchung.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUmbuchung.StateNormal.Border.Rounding = 5;
            this.btnUmbuchung.StateNormal.Border.Width = 5;
            this.btnUmbuchung.StateNormal.Content.AdjacentGap = 0;
            this.btnUmbuchung.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnUmbuchung.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnUmbuchung.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnUmbuchung.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnUmbuchung.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnUmbuchung.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnUmbuchung.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUmbuchung.StateTracking.Border.Rounding = 2;
            this.btnUmbuchung.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnUmbuchung.TabIndex = 59;
            this.btnUmbuchung.Values.Text = "POS. Umbuchen";
            this.btnUmbuchung.Click += new System.EventHandler(this.btnUmbuchung_Click);

            panel.Add(panel1);
            listeYukle();
            return panel;
            //   
        }
        public List<Panel> panelGetir()
        {
            BonGoruntu();

            return panel;
        }
        private void btnUmbuchung_Click(object sender, EventArgs e)
        {
            try
            {
                if (fis.SatisKalem.Count > 0)
                {
                    F_PozUmbuchen pozSat = new F_PozUmbuchen();
                    pozSat.fis = fis;
                    pozSat.KundenDisplayFisInhaltEvent+=new F_PozUmbuchen.KundenDisplayDelagate(KundenDisplayFisInhaltEvent);
                    pozSat.MevcutMasalar = MevcutMasalar;
                    if (cbBewirtung.Checked == true)
                        pozSat.fis.Bewirtung = 1;
                    pozSat.ShowDialog();
                    
                        listeYukle();
                    
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["105"];
                    frmerror.ShowDialog();

                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        private void pozOdeme_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (fis.SatisKalem.Count > 1)
                {
                    F_PozSatis pozSat = new F_PozSatis();
                    pozSat.fis = fis;
                    pozSat.KundenDisplayFisInhaltEvent+=new F_PozSatis.KundenDisplayDelagate(KundenDisplayFisInhaltEvent);
                    if (cbBewirtung.Checked == true)
                        pozSat.fis.Bewirtung = 1;
                    pozSat.ShowDialog();
                    if (pozSat.TeilOdemeVarmi == true)
                    {
                        listeYukle();
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["104"];
                    frmerror.ShowDialog();

                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void btnBar_Click(object sender, EventArgs e)
        {
            //lblParaUstu.Font = new Font(lblParaUstu.Font.FontFamily.Name, 28);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            if (fis != null && fis.toplamtutar != 0)
            {

                if (verilenPara > 0 && fis.Musterino == 0)
                {

                    double tut = Math.Round(fis.toplamtutar, 2);
                    if (fis.verilenpara < tut)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["64"];
                        frmerror.ShowDialog();

                        return;

                    }
                    else
                    {

                        lblRuckgeld.Text = (fis.verilenpara - tut).ToString("C");//barVk.paraustu.ToString("C");
                        //fis.verilenpara = barVk.verilenpara;
                        fis.paraustu = (fis.verilenpara - tut);
                        //MessageBox.Show("Odeme OK!");


                        if (Program.bonDruck == true)
                        {
                            // CheckForIllegalCrossThreadCalls = false;
                            Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                            Program.BonBeleg.OdemeTur = 1;
                            Program.BonBeleg.basilacakFis = fis;
                            is1.Start();
                        }
                        else
                        {
                            //Program.bonDruck = true;
                            //kryptonButton19.StateNormal.Back.Image = Properties.Resources.printer_green;
                            try
                            {
                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                            }
                            catch
                            {
                            }

                        }

                        if (fis.FisiSonlandır(1, fis.SatisKalem) == true)
                        {
                            lblBonNo.Text = fis.SatisAnaId.ToString();
                            if (fis.Rabattutar != 0)
                            {
                                satisYap = new SatisYap();
                                satisYap.Adet = 1;
                                satisYap.Fisno = fis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = 0;
                                satisYap.Satisfiyat = -fis.Rabattutar;
                                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(-fis.Rabattutar, 2);
                                satisYap.UrunId = 0;
                                satisYap.Birimkar = 0;
                                satisYap.UrunAd = "Rabatt";
                                satisYap.Grubid = 7;
                                fis.SatisKalem.Add(satisYap);
                            }
                            if (fis.PuanRabat != 0)
                            {
                                satisYap = new SatisYap();
                                satisYap.Adet = 1;
                                satisYap.Fisno = fis.SatisAnaId;
                                satisYap.KasaNo = Program.kasano;
                                satisYap.Mwst = 0;
                                satisYap.Satisfiyat = -fis.PuanRabat;
                                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                satisYap.Toplamtutar = Math.Round(-fis.PuanRabat, 2);
                                satisYap.UrunId = 0;
                                satisYap.Birimkar = 0;
                                satisYap.UrunAd = "Rabatt";
                                satisYap.Grubid = 7;
                                fis.SatisKalem.Add(satisYap);
                            }
                            foreach (SatisYap satisIcerik in fis.SatisKalem)
                            {
                                satisIcerik.Fisno = fis.SatisAnaId;
                                satisIcerik.Kaydet();
                            }


                            try
                            {
                                using (myConn = baglanti.myconn())
                                {
                                    if (myConn.State == ConnectionState.Closed)
                                    {
                                        myConn.Open();
                                    }
                                    MySqlCommand co = new MySqlCommand("DELETE FROM masamaster WHERE id= " + fis.MasaDBid, myConn);
                                    if (co.ExecuteNonQuery() > 0)
                                    {
                                        MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE masadbid= " + fis.MasaDBid, myConn);
                                        co1.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception eexx)
                            {
                                MessageBox.Show(eexx.Message);
                            }
                            fis = null;
                            satisYap = null;
                            listView1.Items.Clear();

                            txtToplam.Text = "";
                            lblTutar.Text = "";
                            lblRuckgeld.Text = "";


                        }
                    }


                }
                else // ana ekrandan para girilmediğinde ve/veya Müşteri kartı okutulmuşsa
                {

                    using (BarVerkauf barVk = new BarVerkauf())
                    {
                        barVk.toplamtutar = Math.Round(fis.toplamtutar, 2, MidpointRounding.AwayFromZero);
                        barVk.toplammwst = fis.toplammwst;
                        barVk.mwst19miktar = fis.mwst19miktar;
                        barVk.mwst7miktar = fis.mwst7miktar;
                        barVk.musteriNo = fis.Musterino;
                        barVk.angebotsuztoplamtutar = fis.AngebotsuzToplamTutar;
                        barVk.aktuelFis = fis;

                        //serialPortKD2.Close();
                        barVk.ShowDialog();
                        if (barVk.odemeSonuc == true)
                        {
                            fis.odemesekli = barVk.odemeturu;


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
                                fis.ToplamScheck = barVk.teilCek;
                                fis.ToplamEc = barVk.teilEC;
                                fis.ToplamBar = barVk.teilBar;
                                //serialPortKD2.Open();
                                //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                                //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                                if (fis.Musterino != 0 || barVk.harcananPuanKarsiligiHarcananPara != 0)
                                {
                                    fis.KazanilanPuan = barVk.kazanilanPuan;
                                    if (barVk.indirimturu == -1) //-1: Punkte einlösen
                                    {
                                        fis.EskiPuanToplamı = barVk.eskiPuanToplami;
                                        fis.HarcananPuan = barVk.harcananPuan;
                                        fis.Indirimturu = -1;
                                        RabattMain rbt = new RabattMain();
                                        rbt.RabattAlani = 0;
                                        rbt.RabatArt = 2;
                                        rbt.RabatName = "Punkte Einlösung";
                                        rbt.RabattTyp = 1;
                                        rbt.TotalRabattMenge = barVk.kazanilanIndirim;
                                        rbt.RabatMenge = barVk.kazanilanIndirim;
                                        rbt.Grupid = 7;
                                        fis.RabatList.Add(rbt);
                                        fis.FisiKapat();
                                        fis.Musteri.Kredit = barVk.kredit;
                                    }
                                    else if (barVk.indirimturu == -2) //-2: Direk kunden rabatt, alternative von Puntesammeln
                                    {


                                        fis.Indirimturu = -2;
                                        //fis.Rabat = barVk.rabatOran;
                                        //fis.Rabattutar = barVk.kazanilanIndirim;

                                        RabattMain rbt = new RabattMain();
                                        rbt.RabattAlani = 0;
                                        rbt.RabatArt = 3;
                                        rbt.RabatName = "Kundenrabatt";
                                        rbt.RabattTyp = 0;
                                        rbt.Grupid = 7;
                                        rbt.RabatMenge = barVk.rabatOran;
                                        rbt.TotalRabattMenge = barVk.kazanilanIndirim;
                                        fis.RabatList.Add(rbt);
                                        fis.FisiKapat();
                                        fis.Musteri.Kredit = barVk.kredit;
                                        //RabattAlani->allgemain oder Position (0: tumbonn,  1:position) / Typ-> procent oder menge (0: procent 1:nachlass)
                                        //art->fisalti indirim:0 , positionrabat:1, Kundenkart-normalpunkte:2 ,Kundenkart-normaldirekt=3, kundenkartgerabo:4 kundenkart-iss:5
                                    }
                                    else if (barVk.indirimturu == -3)
                                    {


                                        fis.Indirimturu = -3;
                                        //fis.Rabat = barVk.rabatOran;
                                        //fis.Rabattutar = barVk.kazanilanIndirim;

                                        RabattMain rbt = new RabattMain();
                                        rbt.RabattAlani = 0;
                                        rbt.RabatArt = 4;
                                        rbt.RabatName = "Gerabo-KK Rabatt";
                                        rbt.RabattTyp = 1;
                                        rbt.Grupid = 7;
                                        rbt.RabatMenge = barVk.kazanilanIndirim;
                                        rbt.TotalRabattMenge = barVk.harcananPuanKarsiligiHarcananPara;
                                        fis.RabatList.Add(rbt);
                                        fis.FisiKapat();
                                    }


                                }
                            }
                            catch (Exception eec)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = eec.Message;
                                frmerror.ShowDialog();
                            }

                            fis.verilenpara = barVk.verilenpara;
                            fis.paraustu = barVk.paraustu;
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
                                if (fis.FisiSonlandır(fis.odemesekli, fis.SatisKalem) == true)
                                {
                                    lblBonNo.Text = fis.SatisAnaId.ToString();
                                    foreach (RabattMain rbt in fis.RabatList)
                                    {
                                        if (rbt.Grupid != 999)
                                        {
                                            if (rbt.RabatArt != 6)
                                            {
                                                satisYap = new SatisYap();
                                                satisYap.Adet = 1;
                                                satisYap.Fisno = fis.SatisAnaId;
                                                satisYap.KasaNo = Program.kasano;
                                                satisYap.Mwst = 0;
                                                satisYap.Satisfiyat = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                                satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                                satisYap.Toplamtutar = Math.Round(rbt.TotalRabattMenge, 2, MidpointRounding.ToEven);
                                                satisYap.UrunId = 0;
                                                satisYap.Birimkar = 0;
                                                satisYap.UrunAd = rbt.RabatName;
                                                satisYap.Grubid = rbt.Grupid;
                                                fis.SatisKalem.Add(satisYap);
                                            }
                                        }
                                    }

                                    double satisicerikKontrol = 0;
                                    foreach (SatisYap satisIcerik in fis.SatisKalem)
                                    {
                                        try
                                        {
                                            if (satisIcerik.Barkod == null) satisIcerik.Barkod = "0";
                                            satisIcerik.Fisno = fis.SatisAnaId;
                                            satisicerikKontrol += satisIcerik.Toplamtutar;
                                            satisIcerik.Kaydet();
                                        }
                                        catch (Exception rr)
                                        {
                                            //logEntry.AddtoLogFile(satisIcerik.ToString(), "3117");
                                        }

                                    }

                                    if (Math.Round(satisicerikKontrol, 2, MidpointRounding.AwayFromZero) != Math.Round(fis.toplamtutar, 2, MidpointRounding.AwayFromZero))
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "ERROR!\n KONTROLSUMME= " + Math.Round(fis.toplamtutar, 2) + "\n BONPOZSUMME=  " + Math.Round(satisicerikKontrol, 2) + "\n BITTE Informierene Ihre Geschäftsleiter!";
                                        frmerror.ShowDialog();
                                        // txtGiris.Text = "";
                                        //return false;
                                        //return;
                                    }
                                    using (myConn = baglanti.myconn())
                                    {
                                        if (myConn.State == ConnectionState.Closed)
                                        {
                                            myConn.Open();
                                        }
                                        MySqlCommand co = new MySqlCommand("DELETE FROM masamaster WHERE id= " + fis.MasaDBid, myConn);
                                        if (co.ExecuteNonQuery() > 0)
                                        {
                                            MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE masadbid= " + fis.MasaDBid, myConn);
                                            co1.ExecuteNonQuery();
                                        }
                                    }
                                    if (Program.bonDruck == true)
                                    {
                                        try
                                        {
                                            //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));

                                            Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                                            Program.BonBeleg.OdemeTur = fis.odemesekli;
                                            Program.BonBeleg.basilacakFis = fis;
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

                                    if (fis.odemesekli == 0)

                                        fis = null;
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
                                    listView1.Items.Clear();
                                    txtToplam.Text = "";
                                    fis = null;
                                    satisYap = null;
                                    listView1.Items.Clear();

                                    txtToplam.Text = "";
                                    lblTutar.Text = "";
                                    lblRuckgeld.Text = "";
                                    fis = null;
                                    satisYap = null;
                                    //System.GC.SuppressFinalize(fis);

                                    //MessageBox.Show("İŞLEM TAMAM , FİİŞ YOK EDİLDİ");
                                    listView1.Items.Clear();
                                    txtToplam.Text = "";
                                    listeYukle();
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
                }
            }
                            

                       /* barVk.toplamtutar = Math.Round(fis.toplamtutar, 2);
                        barVk.toplammwst = fis.toplammwst;
                        barVk.mwst19miktar = fis.mwst19miktar;
                        barVk.mwst7miktar = fis.mwst7miktar;
                        barVk.musteriNo = fis.Musterino;
                        barVk.angebotsuztoplamtutar = fis.AngebotsuzToplamTutar;
                        barVk.aktuelFis = fis;
                        //serialPortKD2.Close();
                        barVk.ShowDialog();
                        if (barVk.odemeSonuc == true)
                        {
                            try
                            {
                                //serialPortKD2.Open();
                                if (fis.Musterino != 0)
                                {
                                    fis.KazanilanPuan = barVk.kazanilanPuan;
                                    if (barVk.indirimturu == -1)
                                    {
                                        fis.EskiPuanToplamı = barVk.eskiPuanToplami;
                                        fis.HarcananPuan = barVk.harcananPuan;


                                        fis.Indirimturu = -1;
                                        fis.PuanRabat = barVk.kazanilanIndirim;
                                        fis.FisiKapat();
                                    }
                                    else if (barVk.indirimturu == -2)
                                    {


                                        fis.Indirimturu = -2;
                                        fis.Rabat = barVk.rabatOran;
                                        fis.Rabattutar = barVk.kazanilanIndirim;
                                        fis.FisiKapat();
                                    }
                                    fis.Musteri.Kredit = barVk.kredit;

                                }
                            }
                            catch (Exception eec)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = eec.Message;
                                frmerror.ShowDialog();
                            }

                            lblRuckgeld.Text = barVk.paraustu.ToString("C");
                            fis.verilenpara = barVk.verilenpara;
                            fis.paraustu = barVk.paraustu;
                            if (fis.FisiSonlandır(1,fis.SatisKalem) == true)
                            {
                                lblBonNo.Text = fis.SatisAnaId.ToString();
                                if (fis.Rabattutar != 0)
                                {
                                    satisYap = new SatisYap();
                                    satisYap.Adet = 1;
                                    satisYap.Fisno = fis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = 0;
                                    satisYap.Satisfiyat = Math.Round(-fis.Rabattutar, 2);
                                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                    satisYap.Toplamtutar = Math.Round(-fis.Rabattutar, 2);
                                    satisYap.UrunId = 0;
                                    satisYap.Birimkar = 0;
                                    satisYap.UrunAd = "Rabatt";
                                    satisYap.Grubid = 7;
                                    fis.SatisKalem.Add(satisYap);
                                }
                                if (fis.PuanRabat != 0)
                                {
                                    satisYap = new SatisYap();
                                    satisYap.Adet = 1;
                                    satisYap.Fisno = fis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = 0;
                                    satisYap.Satisfiyat = -fis.PuanRabat;
                                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                    satisYap.Toplamtutar = Math.Round(-fis.PuanRabat, 2);
                                    satisYap.UrunId = 0;
                                    satisYap.Birimkar = 0;
                                    satisYap.UrunAd = "Rabatt";
                                    satisYap.Grubid = 7;
                                    fis.SatisKalem.Add(satisYap);
                                }
                                foreach (SatisYap satisIcerik in fis.SatisKalem)
                                {
                                    satisIcerik.Fisno = fis.SatisAnaId;
                                    satisIcerik.Kaydet();
                                }
                                try
                                {
                                    using (myConn = baglanti.myconn())
                                    {
                                        if (myConn.State == ConnectionState.Closed)
                                        {
                                            myConn.Open();
                                        }
                                        MySqlCommand co = new MySqlCommand("DELETE FROM masamaster WHERE id= " + fis.MasaDBid, myConn);
                                        if (co.ExecuteNonQuery() > 0)
                                        {
                                            MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE masadbid= " + fis.MasaDBid, myConn);
                                            co1.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception eexx)
                                {
                                    MessageBox.Show(eexx.Message);
                                }

                            if (Program.bonDruck == true)
                            {
                                try
                                {
                                    Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                                    //CheckForIllegalCrossThreadCalls = false;
                                    Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                                    Program.BonBeleg.OdemeTur = 1;
                                    Program.BonBeleg.basilacakFis = fis;
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

                            
                                fis = null;
                                satisYap = null;
                                listView1.Items.Clear();

                                txtToplam.Text = "";
                                lblTutar.Text = "";
                                lblRuckgeld.Text = "";
                                fis = null;
                                satisYap = null;
                                //System.GC.SuppressFinalize(fis);

                                //MessageBox.Show("İŞLEM TAMAM , FİİŞ YOK EDİLDİ");
                                listView1.Items.Clear();
                                txtToplam.Text = "";
                                
                            }

                            //yazıcı->gonder;
                        }
                        */
                    
                
            
                    
        }
        public void fisyaz()
        {
            if (Program.PrinterLib == ".NET")
            {
                FisBarkodlu barkodluFis = new FisBarkodlu();
                barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                barkodluFis.FisYaz();
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
                FisBarkodlu barkodluFis = new FisBarkodlu();
                barkodluFis.basilacakFis = Program.BonBeleg.basilacakFis;
                barkodluFis.OdemeTur = Program.BonBeleg.OdemeTur;
                barkodluFis.FisYaz();
            }


        }
        private void btnEC_Click(object sender, EventArgs e)
        {
            try
            {
                if (fis != null && fis.toplamtutar != 0)
                {

                    using (F_EConay frmEcOnay = new F_EConay())
                    {
                        frmEcOnay.aktuelFis = fis;
                        frmEcOnay.toptutar = fis.toplamtutar;
                        frmEcOnay.ShowDialog();
                        if (frmEcOnay.odemesonuc == true)
                        {
                            fis.verilenpara = 0;
                            fis.paraustu = 0;
                            //MessageBox.Show("Odeme OK!");

                            if (Program.bonDruck == true)
                            {
                                //CheckForIllegalCrossThreadCalls = false;
                                Thread is1 = new Thread(new ThreadStart(this.fisyaz));
                                Program.BonBeleg.OdemeTur = 0;
                                Program.BonBeleg.basilacakFis = fis;
                                is1.Start();
                            }
                            try
                            {
                                Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                            }
                            catch
                            {
                            }

                            if (fis.FisiSonlandır(0, fis.SatisKalem) == true)
                            {
                                lblBonNo.Text = fis.SatisAnaId.ToString();
                                if (fis.Rabattutar != 0)
                                {
                                    satisYap = new SatisYap();
                                    satisYap.Adet = 1;
                                    satisYap.Fisno = fis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = 0;
                                    satisYap.Satisfiyat = -fis.Rabattutar;
                                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                    satisYap.Toplamtutar = Math.Round(-fis.Rabattutar, 2);
                                    satisYap.UrunId = 0;
                                    satisYap.Birimkar = 0;
                                    satisYap.UrunAd = "Rabatt";
                                    satisYap.Grubid = 7;
                                    fis.SatisKalem.Add(satisYap);
                                }
                                if (fis.PuanRabat != 0)
                                {
                                    satisYap = new SatisYap();
                                    satisYap.Adet = 1;
                                    satisYap.Fisno = fis.SatisAnaId;
                                    satisYap.KasaNo = Program.kasano;
                                    satisYap.Mwst = 0;
                                    satisYap.Satisfiyat = -fis.PuanRabat;
                                    satisYap.Tarih = tarih.unixdate(DateTime.Now);
                                    satisYap.Toplamtutar = Math.Round(-fis.PuanRabat, 2);
                                    satisYap.UrunId = 0;
                                    satisYap.Birimkar = 0;
                                    satisYap.UrunAd = "Rabatt";
                                    satisYap.Grubid = 7;
                                    fis.SatisKalem.Add(satisYap);
                                }
                                foreach (SatisYap satisIcerik in fis.SatisKalem)
                                {
                                    satisIcerik.Fisno = fis.SatisAnaId;
                                    satisIcerik.Kaydet();
                                }
                                try
                                {
                                    using (myConn = baglanti.myconn())
                                    {
                                        if (myConn.State == ConnectionState.Closed)
                                        {
                                            myConn.Open();
                                        }
                                        MySqlCommand co = new MySqlCommand("DELETE FROM masamaster WHERE id= " + fis.MasaDBid, myConn);
                                        if (co.ExecuteNonQuery() > 0)
                                        {
                                            MySqlCommand co1 = new MySqlCommand("DELETE FROM masadetail WHERE masadbid= " + fis.MasaDBid, myConn);
                                            co1.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception eexx)
                                {
                                    MessageBox.Show(eexx.Message);
                                }
                                fis = null;
                                satisYap = null;
                                listView1.Items.Clear();

                                txtToplam.Text = "";
                                lblTutar.Text = "";
                                lblRuckgeld.Text = "";
                                fis = null;
                                satisYap = null;
                                //System.GC.SuppressFinalize(fis);

                                //MessageBox.Show("İŞLEM TAMAM , FİİŞ YOK EDİLDİ");
                                listView1.Items.Clear();
                                txtToplam.Text = "";
                            }
                        }
                    }
                }
            }
            catch(Exception dd)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text =dd.Message;
                frmerror.ShowDialog();
            }
        }
        public void RuckGeldHesapla(double verilenPara)
        {
            if (fis != null)
            {
                double tutar = 0;

                if (fis.toplamtutar > 0)
                {
                    if (verilenPara > 0)
                    {
                        lblRuckgeld.Visible = true;
                        lblRuckgeld.Text = (verilenPara - fis.toplamtutar).ToString("C");
                    }
                    else
                        lblRuckgeld.Visible = false;
                }
            }
        }
        private void listeYukle()
        {
            this.KundenDisplayFisInhaltEvent("", "", "", "", "", 9,0,0,0);
            this.listView1.Items.Clear();

            foreach (SatisYap satislar in fis.SatisKalem)
            {
                string urunad = satislar.UrunAd.ToString().IndexOf('\n') != -1 ? satislar.UrunAd.Substring(0, satislar.UrunAd.ToString().IndexOf('\n')) : satislar.UrunAd.ToString();
                int listViewElaman = listView1.Items.Count;
                listView1.Items.Add(position.ToString());
                listView1.Items[listViewElaman].SubItems.Add(urunad.ToString() + "{" + satislar.Adet + "x" + satislar.Satisfiyat.ToString("C") + "}");
                listView1.Items[listViewElaman].SubItems.Add(satislar.Toplamtutar.ToString("C"));
                listView1.Items[listViewElaman].Tag = satislar.MasaDetailId;
                if (satislar.Stornodurum == 1)
                {
                    listView1.Items[listViewElaman].SubItems.Add("*");
                }
                else
                {
                    listView1.Items[listViewElaman].SubItems.Add("-");

                }

                if (satislar.UrunId != 99999)
                {
                    position++;
                }

                this.KundenDisplayFisInhaltEvent(satislar.UrunAd, satislar.Adet.ToString("#0.00") , satislar.Satisfiyat.ToString("C") , satislar.Toplamtutar.ToString("C") , "TOTAL :" + fis.toplamtutar.ToString("C"), 0,0,0,0);
                /*if (Program.displayType == "TVS")
                {
                    //knddsply.VerkaufInfo(satisYap.UrunAd + "\n" + satisYap.Adet.ToString("#0.000") + "gr. x" + satisYap.Satisfiyat.ToString("C") + "/kg" + (satisYap.Adet > 1 ? "\n" + satisYap.Toplamtutar.ToString("C") : ""), "TOTAL :" + yeniFis.toplamtutar.ToString("C"));
                    DSPINFO(satisYap.UrunAd, satisYap.Adet.ToString("#0.000") + "gr.", satisYap.Satisfiyat.ToString("C") + "/kg", (satisYap.Adet > 1 ? "\n" + satisYap.Toplamtutar.ToString("C") : ""), "TOTAL :" + yeniFis.toplamtutar.ToString("C"), 0);

                }*/
            }
            txtToplam.Text = "TOTAL : " + fis.toplamtutar.ToString("C");
            lblTutar.Text = fis.toplamtutar.ToString("C");
        }
        private void btnYukari_Click(object sender, EventArgs e)
        {

            int sayi = listView1.Items.Count;
            seciliItem = -1;
            ListViewItem eleman;
            if (sayi > 0)
            {
                //listView1.Enabled = true;
                for (int i = 0; i < sayi; i++)
                {
                    eleman = listView1.Items[i];
                    if (eleman.Selected)
                    {
                        seciliItem = i;
                        break;

                    }

                }
                if (seciliItem > 0)
                {
                    listView1.Items[seciliItem - 1].Selected = true;
                    seciliItem = seciliItem - 1;
                    listView1.FullRowSelect = true;
                    listView1.Focus();
                }
                else
                {
                    listView1.Items[sayi - 1].Selected = true;
                    seciliItem = sayi - 1;
                    listView1.FullRowSelect = true;
                    listView1.Focus();
                }
            }
            //MessageBox.Show(seciliItem.ToString());
        }

        private void btnAsagi_Click(object sender, EventArgs e)
        {

            int sayi = listView1.Items.Count;
            seciliItem = -1;
            ListViewItem eleman;
            if (sayi > 0)
            {
                //listView1.Enabled = true;
                for (int i = 0; i < sayi; i++)
                {
                    eleman = listView1.Items[i];
                    if (eleman.Selected)
                    {

                        seciliItem = i;
                        break;

                    }

                }
                if (seciliItem < sayi - 1)
                {
                    listView1.Items[seciliItem + 1].Selected = true;
                    seciliItem = seciliItem + 1;
                    listView1.FullRowSelect = true;
                    listView1.Focus();
                }
                else
                {
                    listView1.Items[0].Selected = true;
                    seciliItem = 0;
                    listView1.FullRowSelect = true;
                    listView1.Focus();
                }
                // MessageBox.Show(seciliItem.ToString());
            }
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (seciliItem != -1)
            {
                string secilitext = listView1.Items[seciliItem].Text;
                //MessageBox.Show(listView1.Items[seciliItem].Text);
                // MessageBox.Show("Ekleme Öncesi adet:" + fis.SatisKalem[seciliItem].Adet.ToString());
                if (secilitext != "")
                {
                    double ilkadet = fis.SatisKalem[seciliItem].Adet;
                    double birimkar = fis.SatisKalem[seciliItem].Birimkar / fis.SatisKalem[seciliItem].Adet;

                    fis.SatisKalem[seciliItem].Adet += 1;
                    if (fis.SatisKalem[seciliItem].Fand == 1)
                    {
                        fis.SatisKalem[seciliItem + 1].Adet += 1;
                        fis.SatisKalem[seciliItem + 1].Toplamtutar = fis.SatisKalem[seciliItem + 1].Adet * 0.25;
                    }
                    else if (fis.SatisKalem[seciliItem].Fand2 == 1)
                    {
                        fis.SatisKalem[seciliItem + 1].Adet += 1;
                        fis.SatisKalem[seciliItem + 1].Toplamtutar = fis.SatisKalem[seciliItem + 1].Adet * 0.15;
                    }

                    fis.SatisKalem[seciliItem].Birimkar = birimkar * fis.SatisKalem[seciliItem].Adet;

                    fis.SatisKalem[seciliItem].Toplamtutar = Math.Round((fis.SatisKalem[seciliItem].Toplamtutar / ilkadet) * fis.SatisKalem[seciliItem].Adet, 2);

                    //MessageBox.Show("Ekleme Sonrasi adet:" + fis.SatisKalem[seciliItem].Adet.ToString());
                    SatisiGuncelle();
                    fis.FisiKapat();
                    lblTutar.Text = fis.toplamtutar.ToString("C");
                    //listView1.Items.RemoveAt(seciliItem); //.Text = seciliItem.ToString();
                    //listView1.Items[seciliItem].SubItems.RemoveAt(0);
                    //listView1.Items[seciliItem].SubItems.RemoveAt(1);
                    listView1.Items[seciliItem].SubItems[0].Text = secilitext;
                    listView1.Items[seciliItem].SubItems[1].Text = fis.SatisKalem[seciliItem].UrunAd.ToString() + "(" + fis.SatisKalem[seciliItem].Adet + "x" + fis.SatisKalem[seciliItem].Satisfiyat + ")";
                    listView1.Items[seciliItem].SubItems[2].Text = fis.SatisKalem[seciliItem].Toplamtutar.ToString("C");
                    if (fis.SatisKalem[seciliItem].Fand != 0 || fis.SatisKalem[seciliItem].Fand2 != 0)
                    {
                        listView1.Items[seciliItem + 1].SubItems[0].Text = "";
                        listView1.Items[seciliItem + 1].SubItems[1].Text = fis.SatisKalem[seciliItem + 1].UrunAd.ToString() + "(" + fis.SatisKalem[seciliItem].Adet + "x" + fis.SatisKalem[seciliItem].Satisfiyat + ")";
                        listView1.Items[seciliItem + 1].SubItems[2].Text = fis.SatisKalem[seciliItem + 1].Toplamtutar.ToString("C");
                    }

                    // KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString("#0.00"));
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["68"] + "\n" + Program.lang["69"];
                    frmerror.ShowDialog();
                }
            }
        }



        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (seciliItem != -1 && fis != null)
            {
                string secilitext = listView1.Items[seciliItem].Text;
                //MessageBox.Show(listView1.Items[seciliItem].Text);
                // MessageBox.Show("Ekleme Öncesi adet:" + fis.SatisKalem[seciliItem].Adet.ToString());
                if (secilitext != "")
                {
                    double ilkadet = fis.SatisKalem[seciliItem].Adet;
                    if (ilkadet > 1)
                    {
                        double birimkar = fis.SatisKalem[seciliItem].Birimkar / fis.SatisKalem[seciliItem].Adet;

                        fis.SatisKalem[seciliItem].Adet -= 1;
                        if (fis.SatisKalem[seciliItem].Fand == 1)
                        {
                            fis.SatisKalem[seciliItem + 1].Adet -= 1;
                            fis.SatisKalem[seciliItem + 1].Toplamtutar = fis.SatisKalem[seciliItem + 1].Adet * 0.25;
                        }
                        else if (fis.SatisKalem[seciliItem].Fand2 == 1)
                        {
                            fis.SatisKalem[seciliItem + 1].Adet -= 1;
                            fis.SatisKalem[seciliItem + 1].Toplamtutar = fis.SatisKalem[seciliItem + 1].Adet * 0.15;
                        }

                        fis.SatisKalem[seciliItem].Birimkar = birimkar * fis.SatisKalem[seciliItem].Adet;

                        fis.SatisKalem[seciliItem].Toplamtutar = Math.Round((fis.SatisKalem[seciliItem].Toplamtutar / ilkadet) * fis.SatisKalem[seciliItem].Adet, 2);

                        SatisiGuncelle();
                        fis.FisiKapat();
                        lblTutar.Text = fis.toplamtutar.ToString("C");
                        //listView1.Items.RemoveAt(seciliItem); //.Text = seciliItem.ToString();
                        //listView1.Items[seciliItem].SubItems.RemoveAt(0);
                        //listView1.Items[seciliItem].SubItems.RemoveAt(1);
                        listView1.Items[seciliItem].SubItems[0].Text = secilitext;
                        listView1.Items[seciliItem].SubItems[1].Text = fis.SatisKalem[seciliItem].UrunAd.ToString() + "(" + fis.SatisKalem[seciliItem].Adet + "x" + fis.SatisKalem[seciliItem].Satisfiyat + ")";
                        listView1.Items[seciliItem].SubItems[2].Text = fis.SatisKalem[seciliItem].Toplamtutar.ToString("C");
                        if (fis.SatisKalem[seciliItem].Fand != 0 || fis.SatisKalem[seciliItem].Fand2 != 0)
                        {
                            listView1.Items[seciliItem + 1].SubItems[0].Text = "";
                            listView1.Items[seciliItem + 1].SubItems[1].Text = fis.SatisKalem[seciliItem + 1].UrunAd.ToString() + "(" + fis.SatisKalem[seciliItem].Adet + "x" + fis.SatisKalem[seciliItem].Satisfiyat + ")";
                            listView1.Items[seciliItem + 1].SubItems[2].Text = fis.SatisKalem[seciliItem + 1].Toplamtutar.ToString("C");
                        }
                        // KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString("#0.00"));
                    }
                    
                    // KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString("#0.00"));
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["68"] + "\n" + Program.lang["69"];
                    frmerror.ShowDialog();
                }

            }
        }

        private void SatisiGuncelle()
        {
            db baglan = new db();
            //long masaDbid = 0;
            using (myConn = baglan.myconn())
            {
                if (fis.MasaDBid > 0)
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    //Delete Fis Icerigi
                    MySqlCommand cmdIcerikSil = new MySqlCommand("DELETE FROM masadetail WHERE masadbid=" + fis.MasaDBid, myConn);
                    cmdIcerikSil.ExecuteNonQuery();
                    foreach (SatisYap satisItem in fis.SatisKalem)
                    {
                        MySqlCommand coDetail = new MySqlCommand();
                        coDetail.Parameters.AddWithValue("@fiyat", satisItem.Satisfiyat);
                        coDetail.Parameters.AddWithValue("@adet", satisItem.Adet);
                        coDetail.Parameters.AddWithValue("@toplamtutar", satisItem.Toplamtutar);
                        string SQLtext = "INSERT INTO masadetail SET tarih=" + fis.tarih + ",masadbid= " + fis.MasaDBid + ", grupid=" + satisItem.Grubid + ", urunid=" + satisItem.UrunId + ", mwst=" +
                  satisItem.Mwst + ", satisfiyat=@fiyat, adet=@adet, toplamtutar=@toplamtutar, kasano=" + fis.KasaNo + ", kasiyerno=" + fis.kasiyerno + ", urunAd= '" + satisItem.UrunAd + "'";
                        coDetail.CommandText = SQLtext;
                        coDetail.Connection = myConn;
                        coDetail.ExecuteNonQuery();
                    }
                    if (fis.SatisKalem.Count < 1)
                    {
                        MySqlCommand cmdMasaSil = new MySqlCommand("DELETE FROM masamaster WHERE id=" + fis.MasaDBid, myConn);
                        cmdMasaSil.ExecuteNonQuery();
                    }
                }

            }
        }
        private void kryptonButton3_Click(object sender, EventArgs e)
        {


            if (seciliItem != -1)
            {
                //MessageBox.Show(seciliItem.ToString());


                //MessageBox.Show(fis.SatisKalem[seciliItem].UrunAd);
                string secilitext = listView1.Items[seciliItem].Text;
                if (secilitext != "")
                {
                    string silinenad = fis.SatisKalem[seciliItem].UrunAd;
                    double silinenfiyat = fis.SatisKalem[seciliItem].Toplamtutar;

                    if (fis.SatisKalem[seciliItem].Fand != 0 || fis.SatisKalem[seciliItem].Fand2 != 0)
                    {
                        fis.SatisKalem.RemoveAt(seciliItem + 1);
                        fis.SatisKalem.RemoveAt(seciliItem);
                    }
                    else
                    {
                        fis.SatisKalem.RemoveAt(seciliItem);
                    }
                    int i = 1;
                    listView1.Items.Clear();

                    foreach (SatisYap kalanlar in fis.SatisKalem)
                    {
                        int listViewElaman = listView1.Items.Count;
                        listView1.Items.Add(i.ToString());
                        listView1.Items[listViewElaman].SubItems.Add(kalanlar.UrunAd.ToString() + "(" + kalanlar.Adet + "x" + kalanlar.Satisfiyat + ")");
                        listView1.Items[listViewElaman].SubItems.Add(kalanlar.Toplamtutar.ToString("C"));


                        /* int listViewElaman = listView1.Items.Count;
                         listView1.Items.Add(position.ToString());
                         listView1.Items[listViewElaman].SubItems.Add(urun.ArtikelAd.ToString() + "(" + urun.VkPreis + "x" + adet + "}");
                         listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString("C"));*/
                        // KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString());
                        i++;
                    }
                    SatisiGuncelle();
                    fis.FisiKapat();
                    lblTutar.Text = fis.toplamtutar.ToString("C");
                    position = listView1.Items.Count + 1;
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    }
                    /* if (dsp != null)
                     {
                         KDbirinciSatiraYaz(silinenad, "-" + silinenfiyat.ToString("#0.00"));
                         KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString("#0.00"));
                     }
                     else
                     {
                         if (serialPortKD2.IsOpen == true)
                         {
                             //serialPortKD2.Write("" + ((char)27) + ((char)91) + ((char)50) + ((char)74));
                             KDbirinciSatiraYaz(silinenad, "-" + silinenfiyat.ToString("#0.00"));
                             KDikinciSatiraYaz("TOTAL : " + fis.toplamtutar.ToString("#0.00"));
                         }
                     }*/
                    seciliItem = -1;
                    //listView1.Enabled = false;
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["7"];
                    frmerror.lblMesaj.Font = new System.Drawing.Font("Tahoma", 11);
                    frmerror.lblMesaj.Text += Program.lang["8"];
                    frmerror.ShowDialog();
                    seciliItem = -1;
                }
            }
            else
            {
                if (fis != null && fis.SatisKalem.Count > 0)
                {
                    F_GenericSoru frmSilmeOnay = new F_GenericSoru();
                    frmSilmeOnay.lblMesaj.Text = Program.lang["9"];
                    frmSilmeOnay.ShowDialog();
                    if (frmSilmeOnay.sonuc == true)
                    {
                        fis.SatisKalem.Clear();
                        fis.Rabat = 0;
                        fis.Rabattutar = 0;
                        fis.AngebotsuzToplamTutar = 0;
                        fis.BruttoToplam = 0;
                        fis.EskiPuanToplamı = 0;
                        fis.HarcananPuan = 0;
                        fis.HarcananPuanKarsiligiPara = 0;
                        fis.KazanilanPuan = 0;
                        fis.Kredit = 0;
                        fis.Musteri = null;
                        fis.Musterino = 0;
                        fis.StornoIlgi = 0;

                        listView1.Items.Clear();
                        SatisiGuncelle();
                        fis.FisiKapat();

                        lblTutar.Text = fis.toplamtutar.ToString("C");
                        /*   if (dsp != null)
                           {
                               KundenDisplay();
                           }
                           else
                           {
                               if (serialPortKD2.IsOpen == true)
                               {
                                   KundenDisplay();
                               }
                           }*/
                        position = 1;
                        //listView1.Enabled = false;
                        fis = null;

                    }

                }
            }


        }
        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBewirtung.Checked == true)
                    fis.Bewirtung = 1;
                else
                    fis.Bewirtung = 0;
            }
            catch
            {
            }

        }
        

    }
}
