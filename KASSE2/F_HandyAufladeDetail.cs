using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using System.Web;
using iss_HandyAuflade_V1;
using System.IO;
using System.Drawing.Imaging;



namespace IS_KASSE
{
    public partial class F_HandyAufladeDetail : Form
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
        public int limit = 40;
        public int count = 0;
        int rowCount = 0;
        public string Barcode = "0";
        public int ToplamKayitSay;
        MySqlDataAdapter daArtikel = null;
        DataTable dtArtikel = null;
        Tarih tarih = new Tarih();
        string angebotSembol = "";
        public Int32 pfandid = 0;
        public int i = 0;
        public int start = 0;
        public delegate void BarkodSatisDelagate(string brkd);
        public event BarkodSatisDelagate BarcodeSatisEvent;

        public Dictionary<object, object> Info = new Dictionary<object, object>();
        //public iss_HandyAuflade_Main Handy = new iss_HandyAuflade_Main();
        //public Result HandyVerkaufResult = new Result();
        public Int32 CardID = 0;
        string Token;
        string RefToken;

        public F_HandyAufladeDetail()
        {
            InitializeComponent();
        }
        private Image LoadImage(string url)
        {
            try
            {
                url = System.Web.HttpUtility.UrlDecode(url);
                System.Net.WebRequest request =
                    System.Net.WebRequest.Create(url);

                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream =
                    response.GetResponseStream();

                Bitmap bmp = new Bitmap(@responseStream);
                Bitmap bmp2 = new Bitmap(bmp, new Size(150, 65));

                responseStream.Dispose();

                return bmp2;
            }
            catch
            {
                return null;
            }
        }
        private Bitmap ImageLoadFromLocal(string cardid, string url)
        {
            string AppPath = Application.StartupPath;
            if (cardid != "")
            {
                if (File.Exists(@AppPath + "\\HandyImage\\" + cardid + ".JPEG"))
                {
                    return new Bitmap(@AppPath + "\\HandyImage\\" + cardid + ".JPEG");

                }
                else
                {
                    Image image = LoadImage(url);
                    image.Save(@AppPath + "\\HandyImage\\" + cardid + ".JPEG", ImageFormat.Jpeg);
                    Bitmap bmp2 = new Bitmap(image, new Size(150, 65));
                    return bmp2;
                }
            }
            else
            {
                return null;
            }
        }
        private void F_HandyAuflade_Load(object sender, EventArgs e)
        {
            string apppath = Application.StartupPath;
            // waageYarat();

            //iss_HandyAuflade_Main Handy = new iss_HandyAuflade_Main();
            LoginInfo Loginresult = new LoginInfo();
            //iss_HandyAuflade_Main HandyAuflade = new iss_HandyAuflade_Main();

            if (Program.IsletmeAyarlar["HandyAufladeUsername"] != "" && Program.IsletmeAyarlar["HandyAufladePassword"] != "")
            {
                iss_HandyAuflade_Main constr = new iss_HandyAuflade_Main();
                constr.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                constr.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                constr.url = Program.IsletmeAyarlar["HandyAufladeAPI"];
                Loginresult = constr.Login();
                if (Loginresult.message == "approved")
                {

                    Token = @Loginresult.token;
                    RefToken = @Loginresult.refresh;
                    Products Cardlist = new Products();

                    Cardlist = constr.CardList("cash card", UrunAd, Token);
                    int listenenSayi = 0;
                    if (Cardlist.products.Count < 1)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Es gibt kein Produkt! Bitte kontaktieren mit Ihrem Verkaufer!";
                        frmerror.ShowDialog();
                        this.Close();
                        return;
                    }
                    ToplamKayitSay = Cardlist.products[0].product_denominations.Count;
                    //string Cardlist = "";
                    for (i = start; i < (Cardlist.products[0].product_denominations.Count < ToplamKayitSay ? (limit + start) : ToplamKayitSay); i++)
                    {
                        //Bitmap bild = new Bitmap();
                        listenenSayi++;
                        //Cardlist += Result[i].cardid + " - " + "cardname:" + Result[i].cardname + " - " + "denomination:" + Result[i].denomination + " - " + "pprice:" + Result[i].pprice + "\n";
                        Button button4 = new Button();
                        //ImageList images = new ImageList();
                        try
                        {
                            //images.Images.Add(Result[i].cardid,
                            // LoadImage(Result[i].cardimage));
                        }
                        catch
                        {
                        }



                        Dictionary<object, object> Info = new Dictionary<object, object>();
                        // Info.Add("karmiktari", dtArtikel.Result[i]s[i].ItemArray[10]);
                        Info.Add("artikelid", Cardlist.products[0].product_id);
                        Info.Add("artikelname", Cardlist.products[0].product_name);
                        Info.Add("preis", Cardlist.products[0].product_denominations[i].value);
                        Info.Add("denominationid", Cardlist.products[0].product_denominations[i].denomination_id);
                        //Info.Add("PLU", dtHandy.Rows[i].ItemArray[3] != "" ? dtHandy.Rows[i].ItemArray[3] : "0");

                        ComponentFactory.Krypton.Toolkit.KryptonButton BtnArtikel1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                        BtnArtikel1.Location = new System.Drawing.Point(2, 2);
                        BtnArtikel1.Name = Cardlist.products[0].product_denominations[i].value.ToString();
                        BtnArtikel1.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
                        BtnArtikel1.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                                    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                        BtnArtikel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
                        BtnArtikel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                        BtnArtikel1.Size = new System.Drawing.Size(162, 98);
                        BtnArtikel1.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;

                        try
                        {
                            BtnArtikel1.StateNormal.Back.Image = ImageLoadFromLocal(Cardlist.products[0].product_id.ToString(), "https://sandbox-api.pjtelesoftgmbh.de" + Cardlist.products[0].product_image.ToString());
                            BtnArtikel1.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                        }
                        catch (Exception ee)
                        {
                            string msj = ee.Message;

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
                        BtnArtikel1.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        BtnArtikel1.StateNormal.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
                        BtnArtikel1.StateNormal.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.TopMiddle;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
                        BtnArtikel1.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
                        BtnArtikel1.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.None;
                        BtnArtikel1.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
                        BtnArtikel1.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
                        BtnArtikel1.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                        /* if (Convert.ToInt16(dtArtikel.Result[i]s[i].ItemArray[10]) < 0)
                         {
                             BtnArtikel1.TabIndex = 0;
                         }
                         else
                         {
                             BtnArtikel1.TabIndex = Convert.ToInt16(dtArtikel.Result[i]s[i].ItemArray[10]);
                         }*/
                        //BtnArtikel1.Values.Text
                        BtnArtikel1.Values.Text = angebotSembol + Cardlist.products[0].product_name + "-" + Cardlist.products[0].product_denominations[i].value.ToString("C");// +"\n" + string.Format("{0:f}", (dtHandy.Rows[i].ItemArray[5])) + "EUR/St\r " + " [" + dtHandy.Rows[i].ItemArray[1] + "]\r";
                        // BtnArtikel1.Values.Text = angebotSembol+dtArtikel.Result[i]s[i].ItemArray[2].ToString() + "\n" + dtArtikel.Result[i]s[i].ItemArray[8].ToString() + "[" + dtArtikel.Result[i]s[i].ItemArray[1] + "]"; 
                        BtnArtikel1.Tag = Info;//dtArtikel.Result[i]s[i].ItemArray[8];
                        BtnArtikel1.Click += new EventHandler(GenericButton);

                        flp.Controls.Add(BtnArtikel1);



                    }
                        start += listenenSayi;
                        if (ToplamKayitSay > start)
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
                            flp.Controls.Add(btnEbeneNext);

                        }


                        this.kryptonButton4.Location = new System.Drawing.Point(2, 2);
                        this.kryptonButton4.Name = "kryptonButton4";
                        this.kryptonButton4.Size = new System.Drawing.Size(162, 98);
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
                    
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Geschäfts Accountinfo sind Falsch oder nicht Korrekt!Prüf Companyinfo Tabelle!";
                    frmerror.ShowDialog();
                }
           
            }
            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = "Geschäfts Accountinfo werden nicht gefunden! Prüf Companyinfo Tabelle!";
                frmerror.ShowDialog();
            }




           
        }
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            sonuc = false;
            this.Close();
        }
        private string TransID()
        {
            //transid=unixtimestamp+kod(4 stell)+kasaNo(1 stell)
            string Unixdate = tarih.unixdate(DateTime.Now).ToString();
            string Kod = Program.IsletmeAyarlar["kod"];
            string Kasano = Program.kasano.ToString();
            while (Kod.Length < 4)
            {
                Kod = "0" + Kod;
            }
            return Unixdate + Kod + Kasano;
        }
        private void GenericButton(object sender, EventArgs e)
        {
            //adet = yeniadet;
            //
            ComponentFactory.Krypton.Toolkit.KryptonButton btnsecilen = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;
            Info = (Dictionary<object, object>)btnsecilen.Tag;

            
            //fiyat = Convert.ToDouble(Info["preis"]);
            //fiyat = Convert.ToDouble(btnsecilen.Tag);
            //CardID = Convert.ToInt32(Info["artikelid"]);
            //INSERT INTO `handyaufladelog`(`id`, `datum`, `kasano`, `batchnummer`, `cardid`, `expirydate`, `pinnummer`, `transid`, `cardname`, `instruction`, `preis`, `bedienerid`) VALUES
            //([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12])
            if (Info["denominationid"].ToString() !="")
            {
                
                
                sonuc = true;
                this.Close();
               
              
            }
        }
        private string ErrorCode(string errorCode)
        {
            switch (errorCode)
            {
                case "100": return errorCode + "->client transid is already exist";
                case "101": return errorCode + "->Invalid Login Credentials pattern";
                case "102": return errorCode + "->Invalid Username/Password";
                case "103": return errorCode + "->User Not Active";
                case "104": return errorCode + "->Customer Account is not Active";
                //case "105": return "Invalid Login Credentials pattern";
                case "106": return errorCode + "->There is no records found";
                case "109": return errorCode + "->No Cards found for the card ID given!!";
                case "112": return errorCode + "->Sales User is not found";
                case "119": return errorCode + "->Input parameter is not valid!!";
                case "121": return errorCode + "->Password is not changed";
                case "122": return errorCode + "->New password is Invalid Credentials pattern";
                default: return errorCode + "->not identifiziert!Unbekannt!";
            }
        }
        private void btnEbeneNext_Click(object sender, EventArgs e)
        {
            i = start;
            F_HandyAuflade frmobstGemuseStuck = new F_HandyAuflade();
            ebene++;
            count = count + limit;
            frmobstGemuseStuck.i = start;
            frmobstGemuseStuck.start = start;
            frmobstGemuseStuck.ebene = ebene;
            frmobstGemuseStuck.grupid = grupid;
            frmobstGemuseStuck.count = count;
            frmobstGemuseStuck.ToplamKayitSay = ToplamKayitSay;
            // frmobstGemuseStuck.BarcodeSatisEvent += new BarkodSatisDelagate(ObstGemFromForm);
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
                this.Info = frmobstGemuseStuck.Info;
                //this.HandyVerkaufResult = frmobstGemuseStuck.HandyVerkaufResult;
                fiyat = frmobstGemuseStuck.fiyat;
                artikelid = frmobstGemuseStuck.artikelid;
                karmiktari = frmobstGemuseStuck.karmiktari;
                UrunAd = frmobstGemuseStuck.UrunAd;
                Barcode = Info["PLU"].ToString();
                sonuc = true;
                this.Close();
            }
        }
    }
}
