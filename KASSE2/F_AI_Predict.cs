using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iss_Artikel;
using iss_ebon;
using  MySql.Data.MySqlClient;
using iss_Ronsson;

namespace IS_KASSE
{
    public partial class F_AI_Predict : Form
    {
        public string SQLARtikel = "";
        public String code = "";
        public PredictionResult result;
        public F_AI_Predict()
        {
            InitializeComponent();
        }

        private void F_AI_Predict_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection();
            db baglanti = new db();
            conn = baglanti.myconn();
            conn.Open();
            string SQL = "SELECT * FROM artikel WHERE barkod IN (" + SQLARtikel + ")";
            MySqlDataAdapter myDaArt= new MySqlDataAdapter(SQL, conn);
            DataTable dtArtikel = new DataTable();
            myDaArt.Fill(dtArtikel);
            if (dtArtikel.Rows.Count > 0)
            {
                this.Size =new Size(dtArtikel.Rows.Count * 260,400) ;
                for (int i = 0; i < dtArtikel.Rows.Count; i++)
                {
                    var result1 = result.Scores.FirstOrDefault(x => x.Code == dtArtikel.Rows[i].ItemArray[1].ToString());
                    ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                    
                        BtnArtikel1.StateNormal.Back.Color1 = System.Drawing.Color.White;

                        BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Cascadia Mono", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    BtnArtikel1.Size = new System.Drawing.Size(240, 144);
                  

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
                    BtnArtikel1.Values.Text = result1.Value+"%"+ dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + string.Format("{0:f}", (dtArtikel.Rows[i].ItemArray[8])) + "EUR/St\r " + " [" + dtArtikel.Rows[i].ItemArray[1] + "]\r";
                    // BtnArtikel1.Values.Text = angebotSembol+dtArtikel.Rows[i].ItemArray[2].ToString() + "\n" + dtArtikel.Rows[i].ItemArray[8].ToString() + "[" + dtArtikel.Rows[i].ItemArray[1] + "]"; 
                    BtnArtikel1.Tag = dtArtikel.Rows[i].ItemArray[1];
                    BtnArtikel1.Click += new EventHandler(GenericButton);
                    flp.Controls.Add(BtnArtikel1);

                }
            }
            


        }
        private void GenericButton(object sender, EventArgs e)
        {
            //adet = yeniadet;
            //
            try
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
                code = (string)btnsecilen.Tag;
                    this.Close();

                
            }
            catch (Exception ff)
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
