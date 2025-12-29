using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_Multiparking : Form
    {
        public List<FisOlustur> geparkteBonList = new List<FisOlustur>();
        public FisOlustur geParkteBon = null;
        Tarih tar = new Tarih();
        ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        ComponentFactory.Krypton.Toolkit.KryptonButton btnEbeneNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();
        public bool BonParken(FisOlustur Bon)
        {
            this.btnBon = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBon.Location = new System.Drawing.Point(3, 3);
            this.btnBon.Name = "btnBon";
            this.btnBon.Size = new System.Drawing.Size(164, 98);
            this.btnBon.StateCommon.Back.Color1 = System.Drawing.Color.Gainsboro;
            this.btnBon.StateCommon.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnBon.StateCommon.Back.ColorAngle = 0F;
            this.btnBon.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnBon.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopLeft;
            this.btnBon.StateCommon.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnBon.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBon.StateCommon.Border.Rounding = 3;
            this.btnBon.StateCommon.Border.Width = 3;
            this.btnBon.StateCommon.Content.LongText.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnBon.StateCommon.Content.LongText.Font = new System.Drawing.Font("Eurostile", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBon.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnBon.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnBon.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBon.TabIndex = 0;
            this.btnBon.Tag = Bon;
            this.btnBon.Values.Text = "Bon Summe : " + Bon.toplamtutar + "€\r\nUhrZeit :" + tar.tarih(Convert.ToInt32(Bon.tarih)) + "\r\nBon-Nr:" + 1234 + "\r\n\r\n";
            this.btnBon.Click += new EventHandler(ButtonAdd);
            // 
            flp1.Controls.Add(this.btnBon);
            return true;
        }
        public F_Multiparking()
        {
            InitializeComponent();
        }

        private void F_Multiparking_Load(object sender, EventArgs e)
        {
            foreach (FisOlustur Bon in geparkteBonList)
            {
                BonParken(Bon);
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
            flp1.Controls.Add(kryptonButton4);
            if (Program.bedID == 129)
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
                this.btnEbeneNext.Values.Text = "Löschen!!!";
                this.btnEbeneNext.Click += new System.EventHandler(this.btnEbeneNext_Click);
                flp1.Controls.Add(btnEbeneNext);
            }
        }
        private void ButtonAdd(object sender, EventArgs e)
        {
            ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
            geParkteBon = (FisOlustur)btnsecilen.Tag;
            this.Close();

        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            geParkteBon = null;
            this.Close();
        }
        private void btnEbeneNext_Click(object sender, EventArgs e)
        {
            FisOlustur SecilenFis = new FisOlustur();
            ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
            SecilenFis = (FisOlustur)btnsecilen.Tag;
            geparkteBonList.RemoveAll(D => D.Parknumber == SecilenFis.Parknumber);
            flp1.Controls.Clear();
            foreach (FisOlustur Bon in geparkteBonList)
            {
                BonParken(Bon);
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
            flp1.Controls.Add(kryptonButton4);
            if (Program.bedID == 129)
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
                this.btnEbeneNext.Values.Text = "Löschen!!!";
                this.btnEbeneNext.Click += new System.EventHandler(this.btnEbeneNext_Click);
                flp1.Controls.Add(btnEbeneNext);
            }
        }
    }
}
