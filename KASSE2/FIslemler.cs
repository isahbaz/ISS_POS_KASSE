using ComponentFactory.Krypton.Toolkit;
using Conn;
using iss_Datev;
using iss_KassenZahler;
using iss_Rea;
using Microsoft.PointOfService;
using Microsoft.VisualBasic.Logging;
using MySql.BackUp;
using MySql.Data.MySqlClient;
using POS.Devices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
//using System.Diagnostics;

using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using MySqlBackup;
//using System.Threading;

namespace IS_KASSE
{
    public partial class FIslemler : Form
    {
        byte[] _ba = null;
        VirgulAyikla vA = new VirgulAyikla();
        Tarih tarih = new Tarih();
        MySqlConnection myConn = new MySqlConnection();
        dbConn baglanti = new dbConn();
        User user = new User();
        long sonuc = 0;
        public delegate void BenimDelegem(string info, string Info2);
        public event BenimDelegem BenimEventim;

        public isstoRea Rea;
        string line = "", alteline = "";
        bool pr = true;
        Thread ECoku;
        List<string> dizi = new List<string>();
        public string gutscheinbrkd = "";
        public List<FisOlustur> geparkteBonList = new List<FisOlustur>();
        MySqlConnection conn = new MySqlConnection();
        public YetkiCheck yetkiCheck;
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        string codeVerifier = "", codeChallenge = "", refreshTokenVar = "", AccessTokenVar = "";
        iss_Datev_Main datevMain = new iss_Datev_Main();
        int berNo = 0;
        Logger log = new Logger("LOG\\DATEV\\LOGIN");

        public FIslemler()
        {
            InitializeComponent();
        }

        private void FIslemler_Load(object sender, EventArgs e)
        {
            if ((Program.IsletmeAyarlar["HandyAufladeUsername"] != "") && (Program.IsletmeAyarlar["HandyAufladePassword"] != ""))
            {
                btnHandyAuflade.Visible = true;
            }
            else
            {
                btnHandyAuflade.Visible = false;
            }
            //exportProgress.Visible = false;
            if (Program.ProgramAyarlar["wprotokoll"] == "Dialog06")
            {
                btnHersSWID.Visible = false;
            }
            Conn.dbConn baglanti = new Conn.dbConn();
            conn = baglanti.myconn();
            if (Program.TSEready == 1)
            {

            }
            if (Program.zvt == "ReaRetail")
            {
                Rea = Program.Rea;
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
            }
            if ((Program.mp == null) && (Program.mp == "0")) // multi bon tanimli degilse veya multi bon sayisi 0 olarak verilmisse...
            {
                btnSystemParameter.Visible = false;
            }
            else
            {
                btnSystemParameter.Visible = true;
            }
            try
            {
                if (Program.printer != null)
                {
                    if (Program.printer.DeviceEnabled == true)
                    {
                        btnPrinter.Text = "Drucker Freilassen!";
                    }
                    else
                    {
                        btnPrinter.Text = "Drucker Zurücknehmen!";
                    }
                }
            }
            catch
            {
            }
            // Program.PTB_F_Einstellungen = long.Parse("1A1A19", System.Globalization.NumberStyles.HexNumber);
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            label3.Text = " Version: " + String.Format("{0}", fvi.FileVersion);
            label7.Text = " PRODUKT NAME: " + fvi.CompanyName + "/" + String.Format("{0}", fvi.ProductName);

            // label4.Text = " SW-MD5 KEY :" + String.Format("{0}", getMD5());
            // Drucker olmadan yapılan testler için 
            // Program.PTB_Bondrucker = long.Parse("1A1A13", System.Globalization.NumberStyles.HexNumber);

            sonuc = 0;
            label6.Text = Program.ept.ReadSWID();

            /*string pass = Microsoft.VisualBasic.Interaction.InputBox("Lütfen Yönetici Şifresini Giriniz", "YÖNETİMSEL AYARLAR", "");
            if (pass != "sahbaz3420351")
            {
                this.Close();
            }
            */
            myConn = baglanti.myconn();
            MySqlDataAdapter daUserControl = new MySqlDataAdapter("SELECT * from usertakip where  tarih=" + tarih.bugunBaslangic() + " and onlinezaman<>0 and offlinezaman=0", myConn);
            DataTable dtUserControl = new DataTable("usertakip");
            dtUserControl.Clear();
            daUserControl.Fill(dtUserControl);
            int rowCountUser = dtUserControl.Rows.Count;
            if (rowCountUser > 0)
            {
                for (int i = 0; i < rowCountUser; i++)
                {
                    ComponentFactory.Krypton.Toolkit.KryptonCheckBox cbb = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();

                    cbb.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl;
                    cbb.Location = new System.Drawing.Point(25, 40 + (i * 30));
                    cbb.Name = dtUserControl.Rows[i].ItemArray[0].ToString();
                    cbb.Size = new System.Drawing.Size(116, 50);
                    cbb.TabIndex = 2;
                    cbb.Checked = true;
                    cbb.Text = user.UserBul(Convert.ToInt64(dtUserControl.Rows[i].ItemArray[4]));
                    cbb.Values.Text = user.UserBul(Convert.ToInt64(dtUserControl.Rows[i].ItemArray[4]));
                    cbb.CheckedChanged += new System.EventHandler(this.cbb_CheckedChanged);
                    panel1.Controls.Add(cbb);

                    //cbb.Dispose();
                }

            }
            if (Program.IsletmeAyarlar["markt"] == "0")
            {
                btnZweiteEbene.Visible = true;
            }

            //Datev Check
            try
            {
                if (myConn.State == ConnectionState.Closed)
                    myConn.Open();
                string checkTenandIDSql = "";
                checkTenandIDSql = "SELECT DatevTenandID from isletme";
                MySqlDataAdapter myDaTenand = new MySqlDataAdapter(checkTenandIDSql, myConn);
                DataTable dtTenand = new DataTable();
                myDaTenand.Fill(dtTenand);
                if (dtTenand.Rows.Count > 0 && dtTenand.Rows[0].ItemArray[0].ToString() != "")
                {
                    label13.Text = "DATEV Tenand-ID:" + dtTenand.Rows[0].ItemArray[0].ToString();
                    string readRefTokenSql = "";
                    readRefTokenSql = "SELECT refreshtoken from datev_info order by id DESC";
                    MySqlDataAdapter myDaRef = new MySqlDataAdapter(readRefTokenSql, myConn);
                    DataTable dtRef = new DataTable();
                    myDaRef.Fill(dtRef);
                    if (dtRef.Rows.Count > 0 && dtRef.Rows[0].ItemArray[0].ToString() != "")
                    {

                        //refresh Token
                        iss_Datev_Main datevMain = new iss_Datev_Main();
                        // datevMain.Token("Localhost", frmConn.Code, codeVerifier, "");
                        DatevToken datevToken = new DatevToken();
                        datevToken = datevMain.RefreshToken(dtRef.Rows[0].ItemArray[0].ToString(), "refresh_token");
                        if (datevToken != null)
                        {
                            MySqlCommand cmd1 = new MySqlCommand();
                            cmd1.Parameters.AddWithValue("@json", "");
                            cmd1.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                            cmd1.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                            cmd1.Parameters.AddWithValue("@idToken", datevToken.id_token);
                            cmd1.Parameters.AddWithValue("@reqID", "");
                            cmd1.Parameters.AddWithValue("@tenandID", "");
                            cmd1.Parameters.AddWithValue("@filename", "");
                            cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                            cmd1.Parameters.AddWithValue("@error", "0");
                            cmd1.Parameters.AddWithValue("@errormessage", "");
                            cmd1.Parameters.AddWithValue("@level", "Token mit Refresh-Token");
                            cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                            cmd1.Parameters.AddWithValue("@log", "Die Authentifizierung ist erfolgreich. Access-Token durch Refresh-Token wurde erzeugt!");

                            cmd1.Connection = myConn;
                            cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr, datev_log ) VALUES" +
                                "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr, @log)";
                            cmd1.ExecuteNonQuery();
                            string standOrt = "";
                            standOrt = Program.IsletmeAyarlar["stadt"] + "-" + Program.IsletmeAyarlar["plz"] + "-" + Program.IsletmeAyarlar["strase"];
                            AccessTokenVar = datevToken.access_token;
                            refreshTokenVar = datevToken.refresh_token;
                            //Kassenorner check
                            string CheckOrdnerSql = "";
                            CheckOrdnerSql = "Select kassenr FROM datev_kassen WHERE kassenr=" + Program.kasano + " AND location='" + standOrt + "'";
                            MySqlDataAdapter cmOrnerCheck = new MySqlDataAdapter(CheckOrdnerSql, myConn);
                            DataTable dtCheck = new DataTable();
                            cmOrnerCheck.Fill(dtCheck);
                            if (dtCheck.Rows.Count > 0)
                            {
                                btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);
                                Program.DatevConnection = true;
                                label13.Visible = true;
                                label14.Visible = true;
                                label15.Visible = true;
                                txtZnr.Visible = true;
                                btnDatevNach.Visible = true;
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                                cmd.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                                cmd.Parameters.AddWithValue("@idToken", datevToken.id_token);
                                if (myConn.State == ConnectionState.Closed)
                                    myConn.Open();
                                cmd.Connection = myConn;
                                cmd.CommandText = "UPDATE `datev_info` SET  `accesstoken`=@accessToken, `refreshtoken`=@refrToken, `idtoken`=@idToken";
                                cmd.ExecuteNonQuery();
                                MySqlCommand cmd2 = new MySqlCommand();
                                cmd2.Parameters.AddWithValue("@json", "");
                                cmd2.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                                cmd2.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                                cmd2.Parameters.AddWithValue("@idToken", datevToken.id_token);
                                cmd2.Parameters.AddWithValue("@reqID", "");
                                cmd2.Parameters.AddWithValue("@tenandID", "");
                                cmd2.Parameters.AddWithValue("@filename", "");
                                cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                cmd2.Parameters.AddWithValue("@error", "0");
                                cmd2.Parameters.AddWithValue("@errormessage", "");
                                cmd2.Parameters.AddWithValue("@level", "Token mit Refresh-Token");
                                cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                                cmd2.Parameters.AddWithValue("@log", "Die Kassenterminal ist schon als verbundene-Kasse markiert!");

                                cmd2.Connection = myConn;
                                cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr, datev_log ) VALUES" +
                                    "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr, @log)";
                                cmd2.ExecuteNonQuery();
                            }
                            else
                            {
                                bool Result = CreateDatevOrdner(AccessTokenVar, refreshTokenVar, datevToken.id_token, dtTenand.Rows[0].ItemArray[0].ToString());
                                if (Result == true)
                                {
                                    btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                    btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);
                                    Program.DatevConnection = true;
                                    label13.Visible = true;
                                    label14.Visible = true;
                                    label15.Visible = true;
                                    txtZnr.Visible = true;
                                    btnDatevNach.Visible = true;
                                    MySqlCommand cmd = new MySqlCommand();
                                    cmd.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                                    cmd.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                                    cmd.Parameters.AddWithValue("@idToken", datevToken.id_token);
                                    if (myConn.State == ConnectionState.Closed)
                                        myConn.Open();
                                    cmd.Connection = myConn;
                                    cmd.CommandText = "UPDATE `datev_info` SET  `accesstoken`=@accessToken, `refreshtoken`=@refrToken, `idtoken`=@idToken";
                                    cmd.ExecuteNonQuery();

                                    MySqlCommand cmdOrdner = new MySqlCommand();
                                    cmdOrdner.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmdOrdner.Parameters.AddWithValue("@kassename", Program.kasaAd);
                                    cmdOrdner.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmdOrdner.Parameters.AddWithValue("@clientid", Program.ClientID);
                                    cmdOrdner.Parameters.AddWithValue("@location", standOrt);

                                    cmdOrdner.CommandText = "INSERT INTO `datev_kassen`(`kassenr`, `kassename`,  `datum`, `clientid`, location) VALUES (@kassenr,@kassename,@datum,@clientid,@location)";
                                    cmdOrdner.Connection = myConn;
                                    cmdOrdner.ExecuteNonQuery();
                                    MySqlCommand cmd2 = new MySqlCommand();
                                    cmd2.Parameters.AddWithValue("@json", "");
                                    cmd2.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                                    cmd2.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                                    cmd2.Parameters.AddWithValue("@idToken", datevToken.id_token);
                                    cmd2.Parameters.AddWithValue("@reqID", "");
                                    cmd2.Parameters.AddWithValue("@tenandID", "");
                                    cmd2.Parameters.AddWithValue("@filename", "");
                                    cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmd2.Parameters.AddWithValue("@error", "0");
                                    cmd2.Parameters.AddWithValue("@errormessage", "");
                                    cmd2.Parameters.AddWithValue("@level", "Token mit Refresh-Token");
                                    cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmd2.Parameters.AddWithValue("@log", "Die Authentifizierung ist erfolgreich. Access-Token durch Refresh-Token wurde erzeugt! Aber diese Kassenterminal ist nicht als verbundene-Kasse markiert!");

                                    cmd2.Connection = myConn;
                                    cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr, datev_log ) VALUES" +
                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr, @log)";
                                    cmd2.ExecuteNonQuery();
                                    MySqlCommand cmd3 = new MySqlCommand();
                                    cmd3.Parameters.AddWithValue("@json", "");
                                    cmd3.Parameters.AddWithValue("@accessToken", datevToken.access_token);
                                    cmd3.Parameters.AddWithValue("@refrToken", datevToken.refresh_token);
                                    cmd3.Parameters.AddWithValue("@idToken", datevToken.id_token);
                                    cmd3.Parameters.AddWithValue("@reqID", "");
                                    cmd3.Parameters.AddWithValue("@tenandID", "");
                                    cmd3.Parameters.AddWithValue("@filename", "");
                                    cmd3.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmd3.Parameters.AddWithValue("@error", 0);
                                    cmd3.Parameters.AddWithValue("@errormessage", "");
                                    cmd3.Parameters.AddWithValue("@level", "Verbundene-Kassen");
                                    cmd3.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmd3.Parameters.AddWithValue("@log", "(Intern-log)" + Program.kasaAd + " wurde als Verbund-Kasse markiert!");

                                    cmd3.Connection = myConn;
                                    cmd3.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr, datev_log ) VALUES" +
                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr, @log)";
                                    cmd3.ExecuteNonQuery();
                                }


                            }





                        }
                        else //Refresh Token ist Ungültig
                        {
                            F_GenericError fGenericError = new F_GenericError();
                            fGenericError.lblMesaj.Text = "Die Authentifizierung ist fehlgeschlagen.Bitte verbinden Sie Ihre Kasse erneut mit dem Kassenarchiv online.";
                            int num = (int)fGenericError.ShowDialog();
                            btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.DatevVerbinden;
                            // btnDatevVerbinden.Click += new System.EventHandler(this.kryptonButton1_Click_1);
                            Program.DatevConnection = true;
                            label13.Visible = false;
                            label14.Visible = false;
                            label15.Visible = false;
                            txtZnr.Visible = false;
                            btnDatevNach.Visible = false;
                            MySqlCommand cmd2 = new MySqlCommand();
                            cmd2.Parameters.AddWithValue("@json", "");
                            cmd2.Parameters.AddWithValue("@accessToken", "");
                            cmd2.Parameters.AddWithValue("@refrToken", dtRef.Rows[0].ItemArray[0].ToString());
                            cmd2.Parameters.AddWithValue("@idToken", "");
                            cmd2.Parameters.AddWithValue("@reqID", "");
                            cmd2.Parameters.AddWithValue("@tenandID", "");
                            cmd2.Parameters.AddWithValue("@filename", "");
                            cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                            cmd2.Parameters.AddWithValue("@error", "Unautorisiert");
                            cmd2.Parameters.AddWithValue("@errormessage", "");
                            cmd2.Parameters.AddWithValue("@level", "Token mit Refresh-Token");
                            cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                            cmd2.Parameters.AddWithValue("@log", "Die Authentifizierung ist fehlgeschlagen.Bitte verbinden Sie Ihre Kasse erneut mit dem Kassenarchiv online.");

                            cmd2.Connection = myConn;
                            cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr, datev_log ) VALUES" +
                                "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr, @log)";
                            cmd2.ExecuteNonQuery();


                        }

                    }

                }
                else
                {
                    label13.Text = "Datev Tenand-ID:";
                }
            }
            catch (Exception gg)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = "DATEV Refresh Token Error\n" + gg.Message;
                int num = (int)fGenericError.ShowDialog();
            }
            btnDatevNach.Visible = true;
            txtZnr.Visible = true;

        }
        private void kryptonButton1_Click_1(object sender, EventArgs e)
        {
            log.Log("Connect Button Click!");
            if (Program.IsletmeAyarlar["wv"] == "0")
            {
                log.Log("Wartungsvertrag ist nicht Aktive. Check Isletme  Tabelle!");
                MessageBox.Show("Um DATEV-Schnittstelle zu aktivieren, kontaktieren Sie mit Ihrem Verkäufer!");
            }
            else
            {
                log.Log("Datev_Connect_Check();");
                Datev_Connect_Check();
            }
            // FisBarkodlu fc = new FisBarkodlu();
            // fc.XZDruck(28);
        }
        private void Datev_Connect_Check()
        {
            bool lastLog = true;
        A:

            if (myConn.State == ConnectionState.Closed)
                myConn.Open();
            string checkTenandIDSql = "";
            checkTenandIDSql = "SELECT DatevTenandID from isletme";
            MySqlDataAdapter myDaTenand = new MySqlDataAdapter(checkTenandIDSql, myConn);
            DataTable dtTenand = new DataTable();
            myDaTenand.Fill(dtTenand);
            if (dtTenand.Rows.Count > 0 && dtTenand.Rows[0].ItemArray[0].ToString() != "" && lastLog == true)
            {
                log.Log("Datev_Connect_Check(): if (dtTenand.Rows.Count > 0 && dtTenand.Rows[0].ItemArray[0].ToString() != \"\" && lastLog == true)");
                string readRefTokenSql = "";
                readRefTokenSql = "SELECT refreshtoken from datev_info order by id DESC";
                MySqlDataAdapter myDaRef = new MySqlDataAdapter(readRefTokenSql, myConn);
                DataTable dtRef = new DataTable();
                myDaRef.Fill(dtRef);
                if (dtRef.Rows.Count > 0 && dtRef.Rows[0].ItemArray[0].ToString() != "")
                {
                    label13.Text = "DATEV Tenand-ID:" + dtTenand.Rows[0].ItemArray[0].ToString();
                    //refresh Token
                    iss_Datev_Main datevMain = new iss_Datev_Main();
                    // datevMain.Token("Localhost", frmConn.Code, codeVerifier, "");
                    DatevToken datevToken = new DatevToken();
                    datevToken = datevMain.RefreshToken(dtRef.Rows[0].ItemArray[0].ToString(), "refresh_token");
                    if (datevToken != null)
                    {

                        AccessTokenVar = datevToken.access_token;
                        refreshTokenVar = datevToken.refresh_token;
                        GetTenand(AccessTokenVar, refreshTokenVar, datevToken.id_token);

                    }
                    else
                    {
                        lastLog = false;
                        goto A;
                    }
                }
                else
                {
                    lastLog = false;
                    goto A;
                }
            }
            else
            {
            B:
                log.Log("if (dtTenand.Rows.Count > 0 && dtTenand.Rows[0].ItemArray[0].ToString() == \"\" or lastLog == false");
                log.Log("datevMain.DatevConnect();");
                datevMain.DatevConnect();
               
                codeVerifier = datevMain.codeVerifier;
                codeChallenge = datevMain.codeChallenge;
                log.Log("datevMain.DatevConnect():codeVerifier = "+ datevMain.codeVerifier+ " codeChallenge ="+ datevMain.codeChallenge);
                F_Datev_Login frmConn = new F_Datev_Login();
                frmConn.codeChallenge = datevMain.codeChallenge;
                frmConn.Uri_state = datevMain.RandomString(20);
                frmConn.UUID = datevMain.UUID();
                frmConn.ShowDialog();
                if (frmConn.Code != "")
                {


                    // datevMain.Token("Localhost", frmConn.Code, codeVerifier, "");
                    DatevToken datevToken = new DatevToken();
                    datevToken = datevMain.Token("http://localhost:8080", frmConn.Code, codeVerifier, "authorization_code");
                    if (datevToken.access_token != null)
                    {

                        AccessTokenVar = datevToken.access_token;

                        refreshTokenVar = datevToken.refresh_token;
                        btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                        GetTenand(AccessTokenVar, refreshTokenVar, datevToken.id_token);


                    }

                }
                else if (frmConn.error != "")
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = "DATEV Login: https://meinfiskal.de/kassenarchiv/login ERROR!";
                    int num = (int)fGenericError.ShowDialog();
                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = "Für das Kassenarchiv online wurde noch kein Unternehmen angelegt. Damit die Daten an" +
                                                   " das Kassenarchiv online gesendet werden können, führen Sie bitte die dazu notwendigen" +
                                                   " Schritte unter https://www.datev.de/meinfiskal/ durch.";
                    int num = (int)fGenericError.ShowDialog();
                }
            }
        }
        private bool CreateDatevOrdner(string AccesToken, string refreshToken, string idToken, string tenandName)
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string jsonText = "{\r\n  \"document_type\": \"CASH_REGISTER_MISC_DOCUMENT\",\r\n  \"note\": \"ISS POS Kassensystem-Datev Integration\",\r\n  \"extensions\": {\r\n    \"cash_register\": {\r\n      \"address\": {\r\n        \"street\": \"" + Program.IsletmeAyarlar["strase"] +
                    "\",\r\n        \"postal_code\": \"" + Program.IsletmeAyarlar["plz"] + "\",\r\n        \"city\": \"" + Program.IsletmeAyarlar["stadt"] + "\",\r\n        \"country_code\": \"DE\"\r\n      },\r\n      \"serial_number\": \"" + Program.HerstellerKasseID +
                    "\",\r\n      \"manufacturer\": \"Windsoft Tech GmbH\",\r\n      \"model_type\": \"ISS POS \",\r\n      \"name\": \"" + Program.HerstellerKasseID + "\",\r\n      \"description\": \"Windsoft Tech GmbH-ISS POS Kassensysteme \"\r\n    },\r\n    \"client_application\": {\r\n      \"client_application_name\": \"ISS POS Kassensystem\",\r\n      \"client_application_vendor\": \"Windsoft Tech GmbH\",\r\n      \"client_application_version\": \"" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion +
                    "\"\r\n    },\r\n    \"date_from\": \"1970-01-01\",\r\n    \"date_to\": \"1970-01-01\"\r\n  }\r\n}";

                string jsonInfo = "", RequestID = "", DatevDir = @Application.StartupPath + "\\DATEV\\";

                //jsonInfo = JsonConvert.SerializeObject(cash_register);
                RequestID = datevMain.RandomString(20);
                if (!Directory.Exists(DatevDir))
                {
                    Directory.CreateDirectory(DatevDir);
                    File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                    //File.WriteAllText(@DatevDir + "\\metadata.json", JsonConvert.SerializeObject(cash_register));
                    File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);

                }
                else
                {
                    if (!File.Exists(DatevDir + @"\\initialize-folder.init.conf"))
                    {
                        File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                    }
                    if (!File.Exists(DatevDir + @"\\metadata.json"))
                    {
                        File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);
                    }
                    else
                    {
                        File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);
                    }
                }
                Antwort ant = new Antwort();
                ant = datevMain.fileUpload2(AccessTokenVar, RequestID, tenandName, DatevDir + "metadata.json", DatevDir + "initialize-folder.init.conf", "initialize-folder.init.conf");
                if (ant.ErrorMessage != "")
                {
                    MySqlCommand cmd1 = new MySqlCommand();
                    cmd1.Parameters.AddWithValue("@json", jsonText);
                    cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                    cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                    cmd1.Parameters.AddWithValue("@idToken", idToken);
                    cmd1.Parameters.AddWithValue("@reqID", RequestID);
                    cmd1.Parameters.AddWithValue("@tenandID", tenandName);
                    cmd1.Parameters.AddWithValue("@filename", jsonInfo);
                    cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                    cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                    cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                    cmd1.Parameters.AddWithValue("@level", "Ordner-Create");
                    cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                    cmd1.Connection = myConn;
                    cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr ) VALUES" +
                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level, @kassenr)";
                    cmd1.ExecuteNonQuery();
                    label13.Visible = false;
                    label14.Visible = false;
                    label15.Visible = false;
                    txtZnr.Visible = false;
                    btnDatevNach.Visible = false;
                    btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.DatevVerbinden;
                    Program.DatevConnection = false;
                    btnDatevVerbinden.Click += new System.EventHandler(this.kryptonButton1_Click_1);

                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = "Für das Kassenarchiv online wurde noch kein Unternehmen angelegt. " +
                        "Damit die Daten an das Kassenarchiv online gesendet werden können, führen Sie bitte die dazu notwendigen Schritte unter https://www.datev.de/meinfiskal/ durch.";
                    int num = (int)fGenericError.ShowDialog();
                    return false;



                }
                else
                {
                    //Verbindung OK!
                    MySqlCommand cmd1 = new MySqlCommand();
                    cmd1.Parameters.AddWithValue("@json", jsonText);
                    cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                    cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                    cmd1.Parameters.AddWithValue("@idToken", idToken);
                    cmd1.Parameters.AddWithValue("@reqID", RequestID);
                    cmd1.Parameters.AddWithValue("@tenandID", tenandName);
                    cmd1.Parameters.AddWithValue("@filename", jsonInfo);
                    cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                    cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                    cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                    cmd1.Parameters.AddWithValue("@level", "Ordner-Create");
                    cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                    cmd1.Connection = myConn;
                    cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level, kassenr ) VALUES" +
                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr)";
                    cmd1.ExecuteNonQuery();

                    btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                    Program.DatevConnection = true;
                    btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);
                    label13.Visible = true;
                    label14.Visible = true;
                    label15.Visible = true;
                    txtZnr.Visible = true;
                    btnDatevNach.Visible = true;
                    return true;
                }


            }
            catch (Exception zz)
            {
                return false;
            }
        }
        private void GetTenand(string AccesToken, string refreshToken, string idToken)
        {
            try
            {
                iss_Datev_Main datevMain = new iss_Datev_Main();
                List<Tenant> tenList = new List<Tenant>();
                string requestIDD = datevMain.RandomString(20);
                tenList = datevMain.GetTenant(AccessTokenVar, requestIDD);
                if (tenList.Count > 0)
                {
                    Conn.dbConn baglanti = new Conn.dbConn();
                    MySqlConnection myConn = baglanti.myconn();
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    //INSERT INTO `datev_info`( `tenandid`, `tenandname`, `accesstoken`, `refreshtoken`, `idtoken`) VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6])
                    //check TenandID
                    string checkInfoSql = "SELECT * FROM datev_info";
                    MySqlDataAdapter myDaCheckInfo = new MySqlDataAdapter(checkInfoSql, myConn);
                    DataTable dtCehckInfoSql = new DataTable();
                    myDaCheckInfo.Fill(dtCehckInfoSql);
                    MySqlCommand cmd = new MySqlCommand();
                    if (dtCehckInfoSql.Rows.Count == 0)
                    {

                        cmd.Parameters.AddWithValue("@accessToken", AccesToken);
                        cmd.Parameters.AddWithValue("@refrToken", refreshToken);
                        cmd.Parameters.AddWithValue("@idToken", idToken);
                        cmd.Parameters.AddWithValue("@tenandName", tenList[0].name);
                        cmd.Parameters.AddWithValue("@tenandID", tenList[0].id);


                        cmd.Connection = myConn;
                        cmd.CommandText = "INSERT INTO `datev_info`( `tenandid`, `tenandname`, `accesstoken`, `refreshtoken`, `idtoken`) VALUES" +
                            "(@tenandID,@tenandName,@accessToken,@refrToken,@idToken)";

                    }
                    else
                    {

                        cmd.Parameters.AddWithValue("@accessToken", AccesToken);
                        cmd.Parameters.AddWithValue("@refrToken", refreshToken);
                        cmd.Parameters.AddWithValue("@idToken", idToken);
                        cmd.Parameters.AddWithValue("@tenandName", tenList[0].name);
                        cmd.Parameters.AddWithValue("@tenandID", tenList[0].id);


                        cmd.Connection = myConn;
                        cmd.CommandText = "UPDATE  `datev_info` SET `tenandid`=@tenandID, `tenandname`=@tenandName, `accesstoken`=@accessToken, `refreshtoken`=@refrToken, `idtoken`=@idToken ";

                    }
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        F_GenericError fGenericError = new F_GenericError();
                        fGenericError.lblMesaj.Text = "Die Kasse wurde erfolgreich mit dem Kassenarchiv online verbunden.\nTenantID:" + tenList[0].id;
                        int num = (int)fGenericError.ShowDialog();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.Parameters.AddWithValue("@json", "");
                        cmd2.Parameters.AddWithValue("@accessToken", AccesToken);
                        cmd2.Parameters.AddWithValue("@refrToken", refreshToken);
                        cmd2.Parameters.AddWithValue("@idToken", idToken);
                        cmd2.Parameters.AddWithValue("@reqID", requestIDD);
                        cmd2.Parameters.AddWithValue("@tenandID", tenList[0].id);
                        cmd2.Parameters.AddWithValue("@filename", "");
                        cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                        cmd2.Parameters.AddWithValue("@error", "0");
                        cmd2.Parameters.AddWithValue("@errormessage", "");
                        cmd2.Parameters.AddWithValue("@level", "Erst-Registrierung)");
                        cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                        cmd2.Parameters.AddWithValue("@log", "Die Kasse wurde erfolgreich mit dem Kassenarchiv online verbunden.\nTenantID:" + tenList[0].id);

                        cmd2.Connection = myConn;
                        cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                            "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                        cmd2.ExecuteNonQuery();

                        string UpdateTenandIDSql = "";
                        UpdateTenandIDSql = "UPDATE isletme SET DatevTenandID='" + tenList[0].id + "'";
                        MySqlCommand myDaTenand = new MySqlCommand(UpdateTenandIDSql, myConn);
                        if (myDaTenand.ExecuteNonQuery() > 0)
                        {
                            //erste Kontakt für Ordner-Create
                            Tarih tarih = new Tarih();
                            /* CASH_REGISTER_TRANSACTIONS cash_register = new CASH_REGISTER_TRANSACTIONS();
                             cash_register.document_type = "CASH_REGISTER_TRANSACTIONS";
                             cash_register.note = "ISS POS TEST PROGRAMM";
                             cash_register.extensions.date_from = "2023-07-13";
                             cash_register.extensions.date_to = "2023-07-14";
                             cash_register.extensions.cash_register.description = "Windsoft Tech GmbH ISS POS Test";
                             cash_register.extensions.cash_register.manufacturer = "Windsoft Tech GmbH";
                             cash_register.extensions.cash_register.model_type = "ISS POS ";
                             cash_register.extensions.cash_register.name = "ISS POS Kassensysteme";
                             cash_register.extensions.cash_register.serial_number = Program.ClientID;
                             cash_register.extensions.cash_register.address.city = Program.IsletmeAyarlar["stadt"];
                             cash_register.extensions.cash_register.address.country_code = "DE";
                             cash_register.extensions.cash_register.address.postal_code = Program.IsletmeAyarlar["plz"];
                             cash_register.extensions.cash_register.address.street = Program.IsletmeAyarlar["strase"];
                             cash_register.extensions.client_application.client_application_name = "ISS POS Kassensystem";
                             cash_register.extensions.client_application.client_application_vendor = "Windsoft Tech GmbH";

                             cash_register.extensions.client_application.client_application_version = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;*/
                            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                            string jsonText = "{\r\n  \"document_type\": \"CASH_REGISTER_MISC_DOCUMENT\",\r\n  \"note\": \"ISS POS Kassensystem-Datev Integration\",\r\n  \"extensions\": {\r\n    \"cash_register\": {\r\n      \"address\": {\r\n        \"street\": \"" + Program.IsletmeAyarlar["strase"] +
                                "\",\r\n        \"postal_code\": \"" + Program.IsletmeAyarlar["plz"] + "\",\r\n        \"city\": \"" + Program.IsletmeAyarlar["stadt"] + "\",\r\n        \"country_code\": \"DE\"\r\n      },\r\n      \"serial_number\": \"" + Program.HerstellerKasseID +
                                "\",\r\n      \"manufacturer\": \"Windsoft Tech GmbH\",\r\n      \"model_type\": \"ISS POS \",\r\n      \"name\": \"" + Program.HerstellerKasseID + "\",\r\n      \"description\": \"Windsoft Tech GmbH-ISS POS Kassensysteme \"\r\n    },\r\n    \"client_application\": {\r\n      \"client_application_name\": \"ISS POS Kassensystem\",\r\n      \"client_application_vendor\": \"Windsoft Tech GmbH\",\r\n      \"client_application_version\": \"" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion +
                                "\"\r\n    },\r\n    \"date_from\": \"1970-01-01\",\r\n    \"date_to\": \"1970-01-01\"\r\n  }\r\n}";

                            string jsonInfo = "", RequestID = "", DatevDir = @Application.StartupPath + "\\DATEV\\";

                            //jsonInfo = JsonConvert.SerializeObject(cash_register);
                            RequestID = datevMain.RandomString(20);
                            if (!Directory.Exists(DatevDir))
                            {
                                Directory.CreateDirectory(DatevDir);
                                File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                                //File.WriteAllText(@DatevDir + "\\metadata.json", JsonConvert.SerializeObject(cash_register));
                                File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);

                            }
                            else
                            {
                                if (!File.Exists(DatevDir + @"\\initialize-folder.init.conf"))
                                {
                                    File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                                }
                                if (!File.Exists(DatevDir + @"\\metadata.json"))
                                {
                                    File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);
                                }
                                else
                                {
                                    File.WriteAllText(@DatevDir + "\\metadata.json", jsonText);
                                }
                            }
                            Antwort ant = new Antwort();
                            ant = datevMain.fileUpload2(AccessTokenVar, RequestID, tenList[0].id, DatevDir + "metadata.json", DatevDir + "initialize-folder.init.conf", "initialize-folder.init.conf");
                            if (ant.ErrorMessage != "")
                            {

                                if (ant.ErrorNo == "Forbidden")
                                {
                                    F_GenericError fGenericError1 = new F_GenericError();
                                    fGenericError1.lblMesaj.Text = "Für das Kassenarchiv online wurde noch kein Vertrag angelegt. Damit die Daten an" +
                                                                                " das Kassenarchiv online gesendet werden können, führen Sie bitte die dazu notwendigen" +
                                                                                " Schritte unter https://www.meinfiskal.de durch.";
                                    int num1 = (int)fGenericError1.ShowDialog();

                                    MySqlCommand cmd1 = new MySqlCommand();
                                    cmd1.Parameters.AddWithValue("@json", jsonText);
                                    cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                                    cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                                    cmd1.Parameters.AddWithValue("@idToken", idToken);
                                    cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                    cmd1.Parameters.AddWithValue("@tenandID", tenList[0].id);
                                    cmd1.Parameters.AddWithValue("@filename", jsonInfo);
                                    cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                                    cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                    cmd1.Parameters.AddWithValue("@level", "Ordner-Create(initialize-folder.init)");
                                    cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmd1.Parameters.AddWithValue("@log", "Übertrag der Testdatei ist fehlgeschlagen!(initialize-folder.init). Es gibt kein Vertrag!");

                                    cmd1.Connection = myConn;
                                    cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr,@log)";
                                    cmd1.ExecuteNonQuery();
                                }
                                else
                                {
                                    MySqlCommand cmd1 = new MySqlCommand();
                                    cmd1.Parameters.AddWithValue("@json", jsonText);
                                    cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                                    cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                                    cmd1.Parameters.AddWithValue("@idToken", idToken);
                                    cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                    cmd1.Parameters.AddWithValue("@tenandID", tenList[0].id);
                                    cmd1.Parameters.AddWithValue("@filename", jsonInfo);
                                    cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                                    cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                    cmd1.Parameters.AddWithValue("@level", "Ordner-Create(initialize-folder.init)");
                                    cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmd1.Parameters.AddWithValue("@log", "Übertrag der Testdatei ist fehlgeschlagen!(initialize-folder.init)");

                                    cmd1.Connection = myConn;
                                    cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr,@log)";
                                    cmd1.ExecuteNonQuery();
                                }





                            }
                            else
                            {
                                //Verbindung OK!
                                MySqlCommand cmd1 = new MySqlCommand();
                                cmd1.Parameters.AddWithValue("@json", jsonText);
                                cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                                cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                                cmd1.Parameters.AddWithValue("@idToken", idToken);
                                cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                cmd1.Parameters.AddWithValue("@tenandID", tenList[0].id);
                                cmd1.Parameters.AddWithValue("@filename", jsonInfo);
                                cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                                cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                cmd1.Parameters.AddWithValue("@level", "Ordner-Create(Registrierung)");
                                cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                cmd1.Parameters.AddWithValue("@log", "Die Testdatei wurde erfolgreich übertragen!(initialize-folder.init)");

                                cmd1.Connection = myConn;
                                cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                    "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                cmd1.ExecuteNonQuery();

                                btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                Program.DatevConnection = true;
                                btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);
                                label13.Visible = true;
                                label14.Visible = true;
                                label15.Visible = true;
                                txtZnr.Visible = true;
                                btnDatevNach.Visible = true;
                                string standOrt = "";
                                standOrt = Program.IsletmeAyarlar["stadt"] + "-" + Program.IsletmeAyarlar["plz"] + "-" + Program.IsletmeAyarlar["strase"];
                                MySqlCommand cmdOrdner = new MySqlCommand();
                                cmdOrdner.Parameters.AddWithValue("@kassenr", Program.kasano);
                                cmdOrdner.Parameters.AddWithValue("@kassename", Program.kasaAd);
                                cmdOrdner.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                cmdOrdner.Parameters.AddWithValue("@clientid", Program.ClientID);
                                cmdOrdner.Parameters.AddWithValue("@location", standOrt);

                                cmdOrdner.CommandText = "INSERT INTO `datev_kassen`(`kassenr`, `kassename`,  `datum`, `clientid`, location) VALUES (@kassenr,@kassename,@datum,@clientid,@location)";
                                cmdOrdner.Connection = myConn;
                                cmdOrdner.ExecuteNonQuery();


                            }
                        }
                    }

                }
                else
                {
                    F_GenericError fGenericError = new F_GenericError();
                    fGenericError.lblMesaj.Text = "Für das Kassenarchiv online wurde noch kein Unternehmen angelegt. Damit die Daten an" +
                                                    " das Kassenarchiv online gesendet werden können, führen Sie bitte die dazu notwendigen" +
                                                    " Schritte unter https://www.datev.de/meinfiskal/ durch.";
                    int num = (int)fGenericError.ShowDialog();
                    MySqlCommand cmd1 = new MySqlCommand();
                    cmd1.Parameters.AddWithValue("@json", "");
                    cmd1.Parameters.AddWithValue("@accessToken", AccesToken);
                    cmd1.Parameters.AddWithValue("@refrToken", refreshToken);
                    cmd1.Parameters.AddWithValue("@idToken", idToken);
                    cmd1.Parameters.AddWithValue("@reqID", requestIDD);
                    cmd1.Parameters.AddWithValue("@tenandID", tenList[0].id);
                    cmd1.Parameters.AddWithValue("@filename", "");
                    cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                    cmd1.Parameters.AddWithValue("@error", "");
                    cmd1.Parameters.AddWithValue("@errormessage", "Tenant-List-Leer");
                    cmd1.Parameters.AddWithValue("@level", "Get Tenatlist");
                    cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                    cmd1.Connection = myConn;
                    cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr ) VALUES" +
                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr)";
                    cmd1.ExecuteNonQuery();
                }
            }
            catch (Exception ff)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = ff.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }
        private string getMD5()
        {
            string filename = Application.StartupPath + "\\iss_ept.dll";
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var buffer = md5.ComputeHash(File.ReadAllBytes(filename));
                var sb = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
                return sb.ToString();



            }
        }
        private void cbb_CheckedChanged(object sender, EventArgs e)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            KryptonCheckBox rb = sender as KryptonCheckBox;

            string id = rb.Name;
            string updateSQL = "UPDATE usertakip SET offlinezaman=" + tarih.unixdate(DateTime.Now) + " WHERE id=" + id;
            //updateSQL = "WHERE ayarid=" + rb.Tag;
            MySqlCommand coAyar = new MySqlCommand(updateSQL, myConn);
            if (coAyar.ExecuteNonQuery() < 1)
            {
                MessageBox.Show("Bir Hata Oluştu! Ayarlar Kaydedilemedi!");
            }




        }
        private void ArtikelAktar()
        {
            string gelenData = "";
            string gelenSatisDetay = "";
            long lastid = 0;
            MySqlTransaction myTrans = null;
            //myTrans = myConn.BeginTransaction();
            MySqlConnection localConn = new MySqlConnection();
            string LocalconnString = "SERVER= 192.168.178.27;" +
                               "DATABASE=is_kasa;" +
                               "UID=root;" +
                               "PASSWORD=sahbaz";
            localConn.ConnectionString = LocalconnString;
            try
            {
                localConn.Open();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042)
                {
                    MessageBox.Show("Local Server Kapalı");
                    label1.Text = "Local Server Kapalı";
                }
                else
                {
                    label1.Text = ex.Message;
                }
            }
            if (localConn.State == ConnectionState.Open)
            {
                MySqlDataAdapter daArtikelLocal = new MySqlDataAdapter("SELECT * from satisana", localConn);
                DataTable dtArtikelLocal = new DataTable("user");
                dtArtikelLocal.Clear();
                daArtikelLocal.Fill(dtArtikelLocal);
                int rowCount = dtArtikelLocal.Rows.Count;

                if (rowCount > 0)
                {


                    // conn.Open();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    label1.Text = "Localde " + rowCount + " adet Kayıt Var";
                    foreach (DataRow dr in dtArtikelLocal.Rows)
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        try
                        {

                            myTrans = conn.BeginTransaction();
                            for (int i = 1; i < dr.ItemArray.Count<object>(); i++)
                            {
                                gelenData += vA.virgulayikla(Convert.ToDouble(dr.ItemArray[i])).ToString() + ",";

                            }
                            //MessageBox.Show(gelenData);
                            string localsatianaId = dr.ItemArray[0].ToString();
                            //MessageBox.Show(localsatianaId);
                            gelenData = gelenData.Substring(0, gelenData.Length - 1);
                            string insertSql = "INSERT INTO satisana VALUES (''," + gelenData + ")";
                            MySqlCommand cmdInsert = new MySqlCommand(insertSql, conn);
                            cmdInsert.Transaction = myTrans;
                            if (cmdInsert.ExecuteNonQuery() > 0)
                            {
                                lastid = cmdInsert.LastInsertedId;
                                //MessageBox.Show(lastid.ToString());
                            }
                            MySqlDataAdapter daArtikelDetay = new MySqlDataAdapter("SELECT * from satisdetay WHERE fisno =" + localsatianaId, localConn);
                            DataTable dtArtikelDetay = new DataTable("satisdetay");
                            dtArtikelDetay.Clear();
                            daArtikelDetay.Fill(dtArtikelDetay);
                            int rowCountDetay = dtArtikelDetay.Rows.Count;

                            if (rowCountDetay > 0)
                            {
                                foreach (DataRow drDetay in dtArtikelDetay.Rows)
                                {
                                    for (int b = 1; b < drDetay.ItemArray.Count<object>(); b++)
                                    {
                                        if (b == 2)
                                        {
                                            gelenSatisDetay += lastid.ToString() + ",";
                                        }
                                        else if ((b == 11) || (b == 16))
                                        {
                                            gelenSatisDetay += "'" + drDetay.ItemArray[b].ToString() + "',";
                                        }
                                        else
                                        {
                                            gelenSatisDetay += vA.virgulayikla(Convert.ToDouble(drDetay.ItemArray[b])).ToString() + ",";
                                        }
                                    }
                                    gelenSatisDetay = gelenSatisDetay.Substring(0, gelenSatisDetay.Length - 1);
                                    string insertDetaySql = "INSERT INTO satisdetay VALUES (''," + gelenSatisDetay + ")";
                                    //MessageBox.Show(insertDetaySql);
                                    MySqlCommand coInsertDetay = new MySqlCommand(insertDetaySql, conn);
                                    coInsertDetay.Transaction = myTrans;
                                    if (coInsertDetay.ExecuteNonQuery() > 0)
                                    {
                                        // MessageBox.Show("OK");

                                    }
                                    else
                                    {

                                        // MessageBox.Show("SORUN VAR ");
                                    }
                                    gelenSatisDetay = "";


                                }
                            }
                            gelenData = "";
                            myTrans.Commit();
                        }
                        catch (Exception dd)
                        {
                            myTrans.Rollback();
                        }
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ArtikelAktar();


        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            F_Log log = new F_Log();
            log.ShowDialog();
        }

        private void FIslemler_Activated(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label6.Text = getMD5();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            KryptonButton btnTik = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(btnTik.Name);//
            if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
            {
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * FROM user WHERE userid=" + Program.bedID, conn);
                DataTable dtArtikel = new DataTable("user");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {

                    Tarih tarih = new Tarih();

                    if ((int)dtArtikel.Rows[0].ItemArray[11] != 1)
                    {
                        FisBarkodlu fisdruck = new FisBarkodlu();
                        fisdruck.BedinerBerichtDruck(Program.bedID, 0, Program.bedAdSoyad);
                    }
                    else if ((int)dtArtikel.Rows[0].ItemArray[11] == 1)
                    {
                        Kasiyerler kasiyerlist = new Kasiyerler();

                        DataTable dtKasiyerList = kasiyerlist.KasiyerList(tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year), tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)).Tables[0];
                        if (dtKasiyerList.Rows.Count > 1)
                        {
                            F_XBericht frmxBericht = new F_XBericht();
                            frmxBericht.dtUserControl = dtKasiyerList;
                            frmxBericht.ShowDialog();
                        }
                        else
                        {
                            FisBarkodlu fisdruck = new FisBarkodlu();
                            fisdruck.BedinerBerichtDruck(Convert.ToInt32(dtKasiyerList.Rows[0].ItemArray[0]), 0, Program.bedAdSoyad);
                        }
                    }
                }
                else
                {

                }
                //alt
                // F_XBericht frmxBericht = new F_XBericht();
                //frmxBericht.ShowDialog();
                // FisBarkodlu fisclass = new FisBarkodlu();
                // fisclass.Zdruck();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string SWID = Program.ept.ReadSWID();
                if (Program.displayType == "TVS")
                {
                    this.BenimEventim("SW-ID:", SWID);

                }
                else
                {
                    //this.BenimEventim(label6.Text, "");
                    OPOSLineDisplay dsp = Program.lineDsp;
                    label6.Text = SWID;
                    dsp.ClearText();
                    dsp.DisplayTextAt(0, 0, label6.Text, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                }
                /* F_GenericError frmerr = new F_GenericError();
                 frmerr.label1.Text = "SOFTWARE_ID:";
                 frmerr.lblMesaj.Text = "SW-ID:"+SWID;
                 frmerr.ShowDialog();*/

            }
            catch
            {
            }
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {

            KryptonButton btnTik = sender as KryptonButton;


            try
            {
                if (myConn.State == ConnectionState.Closed)
                    myConn.Open();

                //check DSFinK
                MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT * FROM ayarlardetay WHERE ayarid=24 ", myConn);
                DataTable dtUserBul = new DataTable("user");
                dtUserBul.Rows.Clear();
                daUserBul.Fill(dtUserBul);
                {
                    if (dtUserBul.Rows.Count == 0)
                    {
                        //`ayarid`, `birincisecenekad`, `ikincisecenekad`, `ucuncusecenekad`, `ayartur`, `kasaid`, `ayaradi`, `seceneksayisi`, `birincisecenekdeger`, `ikincisecenekdeger`, `ucuncusecenekdeger`,
                        //`dorduncusecenekad`, `besincisecenekad`, `autobestandpruf`, `liefrechverfolgung`, `ayaradi_de`, `ayaradi_en`, `birincisecenekad_de`, `ikincisecenekad_de`, `ucuncusecenekad_de`, `birincisecenekad_en`, `ikincisecenekad_en`, `ucuncusecenekad_en`
                        //INSERT AND Vorberetung
                        string cmdAyarSQL1 = "INSERT INTO ayarlar (ayarid, ayaradi, birincisecenekad, ikincisecenekad, ayaradi_de,ayaradi_en, birincisecenekad_de, ikincisecenekad_de, birincisecenekad_en, ikincisecenekad_en)VALUES" +
                           "(24,'DSFINK-V','EVET', 'HAYIR','DSFINK-V','DSFINK-V','Ja', 'Nein','Yes', 'No')";
                        MySqlCommand cmdAyar1 = new MySqlCommand(cmdAyarSQL1, myConn);
                        cmdAyar1.ExecuteNonQuery();

                        string cmdAyarSQL = "INSERT INTO ayarlardetay (ayarid, seceneksayisi, birinci,ikinci)VALUES(24,2,1,0)";
                        MySqlCommand cmdAyar = new MySqlCommand(cmdAyarSQL, myConn);
                        if (cmdAyar.ExecuteNonQuery() > 0)
                        {
                            string znrSQL = "UPDATE satisana SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                            MySqlCommand cmdZnr = new MySqlCommand(znrSQL, myConn);
                            if (cmdZnr.ExecuteNonQuery() > 0)
                            {
                                //Update Bon_pos
                                string bonPosSQL = "UPDATE satisdetay SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                                cmdbonPosSQL.ExecuteNonQuery();
                                //update Rabatt
                                string RabattSQL = "UPDATE rabatt SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();
                                //Update Anfangbestand
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string ABSQL = "UPDATE anfangbestand SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                    MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                    cmdAB.ExecuteNonQuery();
                                }
                                // Abrechnungskreis
                                string AbrKrSQL = "UPDATE abrechnungskreis SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();

                                string ZberKrSQL = "UPDATE zbericht SET erstelldatum=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                                MySqlCommand cmdZberKrSQL = new MySqlCommand(ZberKrSQL, myConn);
                                cmdZberKrSQL.ExecuteNonQuery();

                                // Kassenbuch
                                string KasaKrSQL = "UPDATE kassenbuch SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                                cmdKBSQL.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(dtUserBul.Rows[0].ItemArray[4]) == 0)
                        {
                            // Update Ayar
                            string cmdAyarSQL = "UPDATE ayarlardetay SET birinci=1, ikinci=0 WHERE ayarid=24";
                            MySqlCommand cmdAyarUpdate = new MySqlCommand(cmdAyarSQL, myConn);
                            if (cmdAyarUpdate.ExecuteNonQuery() > 0)
                            {
                                //Update Bon_kopf
                                string bonPosKopfSQL = "UPDATE satisdetay SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdbonKopfSQL = new MySqlCommand(bonPosKopfSQL, myConn);
                                cmdbonKopfSQL.ExecuteNonQuery();
                                //Update Bon_pos
                                string bonPosSQL = "UPDATE satisdetay SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                                cmdbonPosSQL.ExecuteNonQuery();
                                //update Rabatt
                                string RabattSQL = "UPDATE rabatt SET znr=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();
                                //Update Anfangbestand
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string ABSQL = "UPDATE anfangbestand SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                    MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                    cmdAB.ExecuteNonQuery();
                                }
                                // Abrechnungskreis
                                string AbrKrSQL = "UPDATE abrechnungskreis SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                                cmdRabattSQL.ExecuteNonQuery();

                                string ZberKrSQL = "UPDATE zbericht SET erstelldatum=-1 WHERE tarih<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                                MySqlCommand cmdZberKrSQL = new MySqlCommand(ZberKrSQL, myConn);
                                cmdZberKrSQL.ExecuteNonQuery();

                                //Kassenbuch
                                string KasaKrSQL = "UPDATE kassenbuch SET znr=-1 WHERE datum<" + tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND  znr=0";
                                MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                                cmdKBSQL.ExecuteNonQuery();
                            }
                        }

                    }
                }
                // garaanti check Z nr  
                //Zukünftig kann weg genommen werden

                tar.Tarih tar = new tar.Tarih();
                int a = 0;
                double yuzde19lukmiktar = 0, yuzde7likmiktar = 0, yuzde0likmiktar = 0, toplamtutar = 0, toplamBar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, toplam0brut = 0, ec7tutar = 0, ec19tutar = 0, ec0tutar = 0,
                  storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0, storno0tutar = 0, scheckmiktar = 0, scheck7tutar = 0, scheck19tutar = 0, scheck0tutar = 0, KombiBar = 0, KombiEC = 0, KombiScheck = 0,
                  KombiBar0 = 0, KombiBar7 = 0, KombiBar19 = 0, KombiEc0 = 0, KombiEc7 = 0, KombiEc19 = 0, KombiCek0 = 0, KombiCek7 = 0, KombiCek19 = 0, toplam19Yazildi = 0, Gutschein = 0, RabatCouponTutar = 0, RabatTutar = 0,
                  totalAnfangBestand = 0;
                int printDruck = 0, barAnzahl = 0, ecAnzahl = 0, stornoAnzahl = 0, scheckAnzahl = 0, bonAnzahl = 0, kombiBonAnzahl = 0, kombiBarAnzahl = 0, kombiECAnzahl = 0, kombiScheckAnzahl = 0;
                double totalGeldEntnahme = 0;
                double totalGeldEinlage = 0;
                double kassenbuchbarBetrag = 0;
                int kundenzahl = 0;
                int ZprintFlaglastInsertId = 0;
                Int64 maxBonnr = 0, minBonNr = 0;
                //Check Tagliche Verkauf
                string ZCHECK1 = "SELECT COUNT(*) FROM satisana WHERE znr=0 AND kasano=" + Program.kasano;
                MySqlDataAdapter daZCHECK1 = new MySqlDataAdapter(ZCHECK1, myConn);
                DataTable dtZCHECK1 = new DataTable();
                dtZCHECK1.Rows.Clear();
                daZCHECK1.Fill(dtZCHECK1);
                if (Convert.ToInt32(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                {
                    string checkZDruck = "SELECT * FROM zdruckprotokol WHERE flag=2 AND datum>" + tarih.bugunBaslangic();
                    MySqlDataAdapter myDaZdruckCheck = new MySqlDataAdapter(checkZDruck, myConn);
                    DataTable dtDruckCheck = new DataTable();
                    dtDruckCheck.Rows.Clear();
                    myDaZdruckCheck.Fill(dtDruckCheck);
                    if (dtDruckCheck.Rows.Count > 0)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "ES GIBT IM SYSTEM EINE ANDERE Z-DRUCK VERLAUF!! \n BITTE Z-BERICHT NACHEINANDER AUSDRUCKEN!  \n BITTE NOCHMAL VERSUCHEN!";
                        frmerror.ShowDialog();
                        return;
                    }
                    db baglan = new db();
                    MySqlConnection myConn1 = new MySqlConnection();
                    myConn1 = baglan.myconn();
                    if (myConn1.State == ConnectionState.Closed)
                    {
                        baglan.openConnection();
                        if (myConn1.State == ConnectionState.Closed)
                        {
                            myConn1.Open();
                        }

                    }
                    MySqlTransaction mytrans = null;
                    string KasseHerstNr = "", kassename = "";
                    int kasano = Program.kasano;
                    //KasseHerstNr = dtKasaNr.Rows[k].ItemArray[5].ToString();
                    kassename = Program.kasaAd;
                    string ZCHECK12 = "SELECT COUNT(*) FROM satisana WHERE znr=0 AND kasano=" + Program.kasano;
                    MySqlDataAdapter daZCHECK12 = new MySqlDataAdapter(ZCHECK12, myConn);
                    DataTable dtZCHECK12 = new DataTable();
                    dtZCHECK12.Rows.Clear();
                    daZCHECK12.Fill(dtZCHECK12);
                    if (Convert.ToInt16(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                    {
                        

                            if (myConn1.State == ConnectionState.Closed)
                            {
                                myConn1.Open();
                            }
                            mytrans = myConn1.BeginTransaction();
                            /* string sqltarih1 = "SELECT * FROM zbericht WHERE erstelldatum=0";
                             MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn1);
                             DataTable dttar1 = new DataTable();
                             dttar1.Rows.Clear();
                             datar1.Fill(dttar1);
                             //long berNo=0;
                             if (dttar1.Rows.Count > 0)
                             {
                                 berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                             }
                             else
                             {*/
                            MySqlCommand coZ1 = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht ", myConn1);
                            coZ1.Transaction = mytrans;
                            coZ1.ExecuteNonQuery();
                            string sqltarih11 = "SELECT * FROM zbericht WHERE id=" + coZ1.LastInsertedId;
                            MySqlDataAdapter datar11 = new MySqlDataAdapter(sqltarih11, myConn1);
                            DataTable dttar11 = new DataTable();
                            dttar11.Rows.Clear();
                            datar11.Fill(dttar11);
                            berNo = Convert.ToInt16(dttar11.Rows[0].ItemArray[2]);
                            // }

                            string ZPro = "INSERT INTO `zdruckprotokol`( `zberichtno`, `datum`, `flag`, medium, user) VALUES (" + berNo + "," + tarih.unixdate(DateTime.Now) + ",2,'KASSE " + Program.kasano + "','" + Program.bedAdSoyad + "')";
                            MySqlCommand cmdZpro = new MySqlCommand(ZPro, myConn1);
                            cmdZpro.Transaction = mytrans;
                            ZprintFlaglastInsertId = cmdZpro.ExecuteNonQuery();





                            int mwst = -1;
                            /*double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                            double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, storno = 0, stornomiktar = 0;
                            double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                            double RabatTutar = 0;
                            double RabatCouponTutar = 0;
                            double Gutschein = 0;*/

                            // Zpos insert
                            string ZposSQL = "INSERT INTO `zberichtpos`(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`,`mwst`, `grupname`) VALUES ";
                            /*INNER İLE LEFTi değiştir UNUTMA*/
                            string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, satisdetay.mwst, max(localbonid) as MaxBonNr, min(localbonid) as MinBonNr FROM satisdetay" +
                            " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid  LEFT JOIN satisana ON satisana.satisanaid=satisdetay.fisno" +
                            " WHERE satisana.znr=0" +
                            " AND satisdetay.kasano=" + Program.kasano + " AND satisdetay.znr=0 GROUP BY  satisdetay.mwst, artikelgrup.grupid ORDER BY satisdetay.mwst ASC, grupad ASC";
                            MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn1);

                            DataTable dtSatisSayisi = new DataTable("satisdetay");
                            dtSatisSayisi.Rows.Clear();
                            daSatisSayisi.Fill(dtSatisSayisi);
                            if (dtSatisSayisi.Rows.Count > 0)
                            {

                                StringFormat format = new StringFormat();
                                format.Alignment = StringAlignment.Far;
                                //max min bonnr
                                string MinMaxSQL = "";
                                string IdCheck = "SELECT * FROM system ";
                                MySqlDataAdapter myDaIdChek = new MySqlDataAdapter(IdCheck, myConn1);
                                DataTable dtIdCheck = new DataTable();
                                myDaIdChek.Fill(dtIdCheck);
                                string ZStartSQL = "";
                                if (Convert.ToInt32(dtIdCheck.Rows[0].ItemArray[10]) == 0)
                                {
                                    MinMaxSQL = "SELECT satisanaid FROM `satisana` WHERE znr=0  AND kasano=" + kasano + " ORDER BY satisanaid ASC";
                                }
                                else if (tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) < (tar.gunBaslangicUnix(Convert.ToInt32(dtIdCheck.Rows[0].ItemArray[10]))))
                                {
                                    MinMaxSQL = "SELECT satisanaid FROM `satisana` WHERE znr=0  AND kasano=" + kasano + " ORDER BY satisanaid ASC";
                                }
                                else
                                {
                                    MinMaxSQL = "SELECT localbonid FROM `satisana` WHERE znr=0  AND kasano=" + kasano + " ORDER BY satisanaid ASC";
                                }
                                MySqlDataAdapter myDaMinMax = new MySqlDataAdapter(MinMaxSQL, myConn1);
                                DataTable dtMinMax = new DataTable();
                                dtMinMax.Rows.Clear();
                                myDaMinMax.Fill(dtMinMax);
                                if (dtMinMax.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dtMinMax.Rows[0].ItemArray[0]) == 0)
                                    {
                                        maxBonnr = Convert.ToInt64(dtMinMax.Rows[dtMinMax.Rows.Count - 1].ItemArray[0]);
                                        minBonNr = Convert.ToInt64(dtMinMax.Rows[0].ItemArray[0]);
                                    }
                                    else
                                    {
                                        maxBonnr = Convert.ToInt64(dtMinMax.Rows[dtMinMax.Rows.Count - 1].ItemArray[0]);
                                        minBonNr = Convert.ToInt64(dtMinMax.Rows[0].ItemArray[0]);
                                    }
                                }


                                while (a < dtSatisSayisi.Rows.Count)
                                {


                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 43)
                                    {
                                        RabatCouponTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 7)
                                    {
                                        RabatTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 6)
                                    {
                                        Gutschein += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        mwst = Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]);

                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }





                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }

                                    }
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        ZposSQL += "(NULL," + berNo + "," + Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) + "," + vA.virgulayikla(Convert.ToDouble(dtSatisSayisi.Rows[a].ItemArray[0])) + "," + kasano + ",'" + "KASSE " + kasano + "'," + mwst + ",'" + dtSatisSayisi.Rows[a].ItemArray[1].ToString() + "'),";
                                    }
                                    a++;
                                }
                                if (RabatTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",7," + vA.virgulayikla(RabatTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Rabatt'),";
                                }
                                if (RabatCouponTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",43," + vA.virgulayikla(RabatCouponTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'RabattCoupon'),";
                                }
                                if (Gutschein < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",6," + vA.virgulayikla(Gutschein) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Gutschein'),";
                                }


                                try
                                {
                                    if (myConn1.State == ConnectionState.Closed)
                                        myConn1.Open();
                                    ZposSQL = ZposSQL.Substring(0, ZposSQL.Length - 1);
                                    MySqlCommand cmdZPos = new MySqlCommand(ZposSQL, myConn1);
                                    cmdZPos.Transaction = mytrans;
                                    cmdZPos.ExecuteNonQuery();

                                }
                                catch (Exception rr)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "ln:885" + rr.Message + "\n\n" + rr.StackTrace;
                                    frmerror.ShowDialog();
                                    mytrans.Rollback();
                                    return;

                                }

                                //GRUP BILGISI SONU

                                string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                                   "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                                   "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay " +
                                   ", SUM(`toplamEc`), SUM(toplamStorno), SUM(toplamScheck) FROM satisana WHERE " +
                                    "  satisana.kasano=" + kasano + " AND znr=0  GROUP BY odemeturu ORDER BY odemeturu ASC";
                                MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn1);
                                DataTable dtSatisAna = new DataTable("satisana");
                                dtSatisAna.Rows.Clear();
                                daSatisAna.Fill(dtSatisAna);

                                int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                                string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 " +
                                    " AND satisana.kasano=" + kasano + " AND satisana.znr=0 AND satisdetay.znr=0  GROUP BY satisdetay.mwst";
                                MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn1);
                                DataTable dtKombi = new DataTable();
                                daKombi.Fill(dtKombi);
                                int CombiBonAnzahl = dtKombi.Rows.Count;
                                string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay," +
                                    " SUM(CASE WHEN `toplamEc` > 0 THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND " +
                                    "  znr=0 AND satisana.kasano=" + kasano;
                                MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn1);
                                DataTable dtKombiSumme = new DataTable();
                                daKombiSumme.Fill(dtKombiSumme);
                                if (dtKombi.Rows.Count > 0)
                                {
                                    double KombiTotal = 0;
                                    for (int c = 0; c < dtKombiSumme.Rows.Count; c++)
                                    {

                                        KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                        KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                        KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                        KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                        kombiBarAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]);
                                        kombiECAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]);
                                        kombiScheckAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]);
                                        KombiTotal = KombiBar + KombiEC + KombiScheck;

                                    }


                                }

                                for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                                {
                                    if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        scheckmiktar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiScheck);
                                        scheckAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                                        kombiScheckAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[6] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]) : 0;

                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            scheck7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiCek7;
                                            scheck19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiCek19;
                                            scheck0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiCek0;

                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        stornomiktar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]));
                                        stornoAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);



                                        storno7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]);
                                        storno19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                                        storno0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0
                                    {
                                        toplamBar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiBar);
                                        barAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                        kombiBarAnzahl = ((!(dtKombiSumme.Rows[0].ItemArray[4] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]) : 0);

                                        if (dtKombi.Rows.Count == 0)
                                        {

                                            toplam7brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiBar7;
                                            toplam19brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiBar19;
                                            toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar0;
                                            //line += aralik;
                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0
                                    {
                                        toplamEC = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiEC);
                                        ecAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                        kombiECAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[5] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]) : 0;
                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            ec7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiEc7;
                                            ec19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiEc19;
                                            ec0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEc0;
                                        }
                                        //toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);
                                        //line += aralik;

                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 4)
                                    {
                                        if ((toplamEC == 0) && (KombiEC != 0))
                                        {

                                            toplamEC = KombiEC;
                                            ec7tutar = KombiEc7;
                                            ec19tutar = KombiEc19;
                                            ecAnzahl = kombiECAnzahl;

                                        }
                                        if ((toplamBar == 0) && (KombiBar != 0))
                                        {

                                            toplamtutar = KombiBar;

                                            yuzde7likmiktar = KombiBar7;

                                            yuzde19lukmiktar = KombiBar19;

                                            yuzde0likmiktar = KombiBar0;
                                            barAnzahl = kombiBarAnzahl;

                                        }
                                        if ((scheckmiktar == 0) && (KombiScheck != 0))
                                        {
                                            scheckmiktar = KombiScheck;
                                            scheck7tutar = KombiCek7;
                                            scheck19tutar = KombiCek19;
                                            scheckAnzahl = kombiScheckAnzahl;

                                        }
                                    }
                                }
                                // MessageBox.Show(berNo.ToString());



                                totalAnfangBestand = AnfangBestandReturn();
                                totalGeldEinlage = GeldEinlageReturn();
                                totalGeldEntnahme = GeldEntnahmeReturn();
                                kassenbuchbarBetrag = toplamBar + stornomiktar + Gutschein + totalAnfangBestand + totalGeldEinlage - totalGeldEntnahme;
                                if (KombiSay > 0)
                                {


                                    kombiBonAnzahl = KombiSay;
                                }
                                if (myConn1.State == ConnectionState.Closed)
                                {
                                    myConn1.Open();
                                }

                                // Int32 minBonNr = 0, maxBonnr = 0;
                                kundenzahl = stornoAnzahl + ecAnzahl + barAnzahl + KombiSay;
                                Int32 erstellDatum = tar.unixdate(DateTime.Now);
                                // `toplamScheck7Tutar`, `toplamScheck19Tutar`, `toplamScheck0Tutar`
                                string SqlZ = "";
                                /* SqlZ = "UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla(toplam19brut * 0.19 + toplam7brut * 0.07) + ",toplam7tutar=" + vA.virgulayikla(toplam7brut) +
                                   ", toplam19tutar=" + vA.virgulayikla(toplam19brut) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                                   ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                                   ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno19tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + startBonNr + ", endBonID=" + endBonNr +
                                   ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                                   ", anfangbestandsumme=" + totalAnfangBestand + " WHERE zberichtNo= " + berNo;*/
                                SqlZ = ("UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla((yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))) + (yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))) + ",toplam7tutar=" + vA.virgulayikla(yuzde7likmiktar) +
                              ", toplam19tutar=" + vA.virgulayikla(yuzde19lukmiktar) + ",toplam0Tutar=" + vA.virgulayikla(yuzde0likmiktar) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", bar7tutar=" + vA.virgulayikla(toplam7brut) + ", bar19tutar=" + vA.virgulayikla(toplam19brut) + ", bar0tutar=" + vA.virgulayikla(toplam0brut) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                              ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                              ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno0tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + minBonNr + ", endBonID=" + maxBonnr +
                              ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                              ", anfangbestandsumme=" + vA.virgulayikla(totalAnfangBestand) + ", totalgeldeinlage=" + vA.virgulayikla(totalGeldEinlage) + ", totalgeldentnahme=" + vA.virgulayikla(totalGeldEntnahme) + ", kassenbuchbarBetrag=" + vA.virgulayikla(kassenbuchbarBetrag) + " WHERE zberichtNo= " + berNo);

                                MySqlCommand coZ = new MySqlCommand(SqlZ, myConn1);
                                coZ.Transaction = mytrans;
                                if (coZ.ExecuteNonQuery() > 0)
                                {
                                    try
                                    {
                                        if (Program.ScaleName == "Bizerba")
                                        {
                                            string BizerbaUmsatzUpdate = "UPDATe biz_umsatz_kopf SET gelesen=2 WHERE gelesen=0"; ;
                                            MySqlCommand cmdBizUp = new MySqlCommand(BizerbaUmsatzUpdate, myConn1);
                                            cmdBizUp.Transaction = mytrans;
                                            cmdBizUp.ExecuteNonQuery();

                                        }
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1 WHERE zberichtno=" + berNo;
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.Transaction = mytrans;
                                        cmdZproUp.ExecuteNonQuery();
                                        if (coZ.ExecuteNonQuery() > 0)
                                        {
                                            ZNummerEkle(berNo, kasano);
                                            //businesscases, Payment
                                            DSFinK_Businesscases(berNo, yuzde19lukmiktar, yuzde7likmiktar, yuzde0likmiktar, Gutschein, RabatCouponTutar, RabatTutar, erstellDatum, toplamBar, toplamEC, scheckmiktar, stornomiktar, totalAnfangBestand, totalGeldEinlage, totalGeldEntnahme);

                                            if (myConn1.State == ConnectionState.Closed)
                                                myConn1.Open();
                                            string sorSQL = "SELECT id,datum FROM kassenbuch WHERE datum= " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND islemid=" + berNo;
                                            MySqlDataAdapter mydaSor = new MySqlDataAdapter(sorSQL, myConn1);
                                            DataTable dtSor = new DataTable();
                                            mydaSor.Fill(dtSor);
                                            if (dtSor.Rows.Count > 0)
                                            {
                                                //update
                                                string kassenbuchSQL = "UPDATE kassenbuch SET tutar=" + vA.virgulayikla(toplamBar + stornomiktar) + " WHERE id=" + dtSor.Rows[0].ItemArray[0]; ;
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                //insert
                                                string kassenbuchSQL = "INSERT INTO kassenbuch (datum, type, islemid, tutar, kaynak, aciklama, hedef, znr) VALUES (" + tar.unixdate(DateTime.Now) + ", 1 ," + berNo + "," + vA.virgulayikla(toplamBar + stornomiktar) + ", 'Z-Bericht', '" + "BERICHT-Nr:" + berNo.ToString() + "', 1," + berNo + ")";
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                        }
                                        mytrans.Commit();
                                    }
                                    catch (Exception dd)
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "ln:1103" + dd.Message + "\n\n" + dd.StackTrace;
                                        frmerror.ShowDialog();
                                        mytrans.Rollback();
                                        if (myConn1.State == ConnectionState.Closed)
                                            myConn1.Open();
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1";
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.ExecuteNonQuery();


                                        // MessageBox.Show("ln:1277\n"+dd.Message);
                                        //lblError.Text = "ln:1300" + dd.Message;
                                    }
                                }




                            }
                        
                        try
                        {
                            Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        }
                        catch (Exception dd)
                        {

                        }
                        yetkiCheck = new YetkiCheck();
                        yetkiCheck.YetkiKontrol(btnTik.Name);//
                        if (Program.DatevConnection == false)
                        {
                            if ((yetkiCheck.YetkiTanimlimi == false))
                            {
                                FisBarkodlu fisclass = new FisBarkodlu();
                                fisclass.XZDruck(berNo);

                            }
                            else if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1))
                            {


                                F_ZAuswahl zauswahl = new F_ZAuswahl();
                                zauswahl.ShowDialog();

                                if (zauswahl.islem == 2)
                                {
                                    FisBarkodlu fisclass = new FisBarkodlu();
                                    fisclass.XZDruck(berNo);
                                }

                            }
                            else
                            {
                                F_GenericError fGenericError = new F_GenericError();
                                fGenericError.lblMesaj.Text = Program.lang["6"];
                                int num = (int)fGenericError.ShowDialog();
                            }
                        }
                        else // datev Connected
                        {
                            // Z basmayi ayiriyoruz, yoksa uzun suruyor
                            if(Program.userClass.ZYetki==0)
                            {
                                //Thread dssp = new Thread(() => this.DSPINFO(urunad, SatilanAdet + " Stk.", SatisFiyat.ToString("C") + "/Stk.", satisYap.Toplamtutar.ToString("C"), yeniFis.toplamtutar.ToString("C"), 0, 0, 0, 0));

                               // dssp.Start();
                                Thread DatevZdruck = new Thread(()=>DatevMitZdruck(berNo));
                                DatevZdruck.Start();
                            }
                           
                            string DatevDir = @Application.StartupPath + "\\DATEV\\";
                            iss_gdpdu.F_DataExport frmDataExport = new iss_gdpdu.F_DataExport();
                            frmDataExport.myCon = myConn;
                            frmDataExport.Host = myConn.DataSource;
                            frmDataExport.IsletmeAyarlar = Program.IsletmeAyarlar;
                            if (!Directory.Exists(DatevDir))
                            {
                                Directory.CreateDirectory(DatevDir);

                            }
                            else
                            {
                                if (myConn1.State == ConnectionState.Closed)
                                    myConn1.Open();
                                string DatevDayDir = @Application.StartupPath + "\\DATEV\\" + DateTime.Now.ToString("yyyy-MM-dd");

                                if (DatevDirCheck(DatevDayDir) == true)
                                {
                                    string ZipFileName = frmDataExport.TableExportDATEVDSFinK(DatevDayDir, berNo.ToString(), Program.kasano, 0, 0);
                                    if (ZipFileName != "")
                                    {

                                        string ZStartSQL = "", Date_from = "", Date_to = "";
                                        ZStartSQL = "SELECT tarih FROM satisana WHERE (localbonid=" + minBonNr + " OR localbonid=" + maxBonnr + ") AND kasano=" + Program.kasano + " ORDER by localbonid";
                                        MySqlDataAdapter myDaZStartBon = new MySqlDataAdapter(ZStartSQL, myConn1);
                                        DataTable myDtZStartBon = new DataTable();
                                        myDtZStartBon.Rows.Clear();
                                        myDaZStartBon.Fill(myDtZStartBon);
                                        Date_from = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows[0].ItemArray[0])).ToString("yyyy-MM-dd");
                                        Date_to = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows.Count == 1 ? myDtZStartBon.Rows[0].ItemArray[0] : myDtZStartBon.Rows[1].ItemArray[0])).ToString("yyyy-MM-dd");

                                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                                        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                                        string jsonText = "{\r\n  \"document_type\": \"CASH_POINT_CLOSING\",\r\n  \"note\": \"ISS POS Kassensystem-Datev Integration\",\r\n  \"extensions\": {\r\n    \"cash_register\": {\r\n      \"address\": {\r\n        \"street\": \"" + Program.IsletmeAyarlar["strase"] +
                                            "\",\r\n        \"postal_code\": \"" + Program.IsletmeAyarlar["plz"] + "\",\r\n        \"city\": \"" + Program.IsletmeAyarlar["stadt"] + "\",\r\n        \"country_code\": \"DE\"\r\n      },\r\n      \"serial_number\": \"" + Program.HerstellerKasseID +
                                            "\",\r\n      \"manufacturer\": \"Windsoft Tech GmbH\",\r\n      \"model_type\": \"ISS POS \",\r\n      \"name\": \"" + Program.HerstellerKasseID + "\",\r\n      \"description\": \"Windsoft Tech GmbH-ISS POS Kassensysteme \"\r\n    },\r\n    \"client_application\": {\r\n      \"client_application_name\": \"ISS POS Kassensystem\",\r\n      \"client_application_vendor\": \"Windsoft Tech GmbH\",\r\n      \"client_application_version\": \"" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion +
                                            "\"\r\n    },\r\n    \"date_from\": \"" + Date_from + "\",\r\n    \"date_to\": \"" + Date_to + "\"\r\n  }\r\n}";

                                        string RequestID = "", tenandID = "", accessToken = "", refreshToken = "", idToken = "";

                                        //jsonInfo = JsonConvert.SerializeObject(cash_register);
                                        RequestID = datevMain.RandomString(20);
                                        if (!Directory.Exists(DatevDayDir))
                                        {
                                            Directory.CreateDirectory(DatevDayDir);
                                            //File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                                            //File.WriteAllText(@DatevDir + "\\metadata.json", JsonConvert.SerializeObject(cash_register));
                                            File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);

                                        }
                                        else
                                        {

                                            if (!File.Exists(DatevDayDir + @"\\metadata.json"))
                                            {

                                                File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                            }
                                            else
                                            {
                                                File.Delete(DatevDayDir + @"\\metadata.json");
                                                File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                            }
                                        }
                                        // TenandID und AcceToken
                                        string DatevInfo = "SELECT  `tenandid`,  `accesstoken`, `refreshtoken`, `idtoken` FROM `datev_info`";
                                        MySqlDataAdapter myDaDatevInfo = new MySqlDataAdapter(DatevInfo, myConn);
                                        DataTable dtINfo = new DataTable();
                                        myDaDatevInfo.Fill(dtINfo);
                                        if (dtINfo.Rows.Count > 0)
                                        {
                                            bool result = false;
                                            string DatevError = "";
                                            tenandID = dtINfo.Rows[0].ItemArray[0].ToString();
                                            accessToken = dtINfo.Rows[0].ItemArray[1].ToString();
                                            refreshToken = dtINfo.Rows[0].ItemArray[2].ToString();
                                            idToken = dtINfo.Rows[0].ItemArray[2].ToString();
                                            FileInfo fi = new FileInfo(@DatevDayDir + "\\" + ZipFileName);
                                            double FileSize = 0;
                                            FileSize = fi.Length / (1024 * 1024);
                                            if (FileSize > 200)
                                            {
                                                F_GenericError frmerror = new F_GenericError();
                                                frmerror.lblMesaj.Text = "Die Daten können nicht übertragen werden. Die maximale Dateigröße von 200 MB wurde überschritten." +
                                                    " Weitere Informationen entnehmen Sie bitte dem Logfile in Ihrem Kassensystem.";
                                                frmerror.ShowDialog();
                                                MySqlCommand cmd1 = new MySqlCommand();
                                                cmd1.Parameters.AddWithValue("@json", jsonText);
                                                cmd1.Parameters.AddWithValue("@accessToken", accessToken);
                                                cmd1.Parameters.AddWithValue("@refrToken", "");
                                                cmd1.Parameters.AddWithValue("@idToken", idToken);
                                                cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                                cmd1.Parameters.AddWithValue("@tenandID", tenandID);
                                                cmd1.Parameters.AddWithValue("@filename", ZipFileName);
                                                cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmd1.Parameters.AddWithValue("@error", "FILEIMPORT_BE_ARCHIVE_000026");
                                                cmd1.Parameters.AddWithValue("@errormessage", "Dateigroße ist größer als 200MB");
                                                cmd1.Parameters.AddWithValue("@level", "File-Übertrag");
                                                cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                cmd1.Parameters.AddWithValue("@log", "Die Daten können nicht übertragen werden.Die maximale Dateigröße von 200 MB wurde überschritten.");

                                                cmd1.Connection = myConn1;
                                                cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                    "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr,@log)";
                                                cmd1.ExecuteNonQuery();
                                            }
                                            Antwort ant = new Antwort();
                                            ant = datevMain.fileUpload2(accessToken, RequestID, tenandID, @DatevDayDir + "\\metadata.json", @DatevDayDir + "\\" + ZipFileName, ZipFileName);
                                            if (ant.ErrorMessage != "")
                                            {
                                                // z bericht Datev Field Update
                                                // Datev FileLog
                                                /*INSERT INTO `datev_dfu`(`id`, `znr`, `createdate`, `zdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`) VALUES 
                                                 * ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12])*/
                                                /*MySqlCommand cmd1 = new MySqlCommand();
                                                cmd1.Parameters.AddWithValue("@json", jsonText);
                                                cmd1.Parameters.AddWithValue("@accessToken", accessToken);
                                                cmd1.Parameters.AddWithValue("@refrToken", "");
                                                cmd1.Parameters.AddWithValue("@idToken", idToken);
                                                cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                                cmd1.Parameters.AddWithValue("@tenandID", tenandID);
                                                cmd1.Parameters.AddWithValue("@filename", ZipFileName);
                                                cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmd1.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                cmd1.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                cmd1.Parameters.AddWithValue("@level", "File-Übertrag"); 
                                                cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                cmd1.Parameters.AddWithValue("@log", "Beim Datenübertragen ist ein Fehler aufgetreten.(" + ZipFileName + " und metadata.json)");

                                                cmd1.Connection = myConn1;
                                                cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                    "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                cmd1.ExecuteNonQuery();
                                                if (cmd1.ExecuteNonQuery() > 0)
                                                {
                                                    string UpdateZdatev = "";
                                                    UpdateZdatev = "UPDATE zbericht SET datev='ERROR' WHERE Zberichtno="+berNo;
                                                    MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn1);
                                                    cmdDatevUpdate.ExecuteNonQuery();
                                                }
                                                */
                                                result = false;
                                                DatevError = ant.ErrorMessage;
                                                if (ant.ErrorNo == "403" || ant.ErrorNo == "Forbidden")
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Für das Kassenarchiv online wurde noch kein Vertrag angelegt. Damit die Daten an" +
                                                                                " das Kassenarchiv online gesendet werden können, führen Sie bitte die dazu notwendigen" +
                                                                                " Schritte unter https://www.meinfiskal.de durch.";
                                                    frmerror.ShowDialog();
                                                    MySqlCommand cmd2 = new MySqlCommand();
                                                    cmd2.Parameters.AddWithValue("@json", jsonText);
                                                    cmd2.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd2.Parameters.AddWithValue("@refrToken", "");
                                                    cmd2.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd2.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd2.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd2.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd2.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                    cmd2.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                    cmd2.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd2.Parameters.AddWithValue("@log", "Die Dateien(" + ZipFileName + " und metadata.json) wurden nicht übertragen, da für das Kassenarchiv online noch kein Vertrag angelegt  wurde., Fehler_Nr:403.");

                                                    cmd2.Connection = myConn1;
                                                    cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                    cmd2.ExecuteNonQuery();
                                                    return;
                                                }
                                                else if (ant.ErrorNo == "402")
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Die Kassendaten konnten nicht archiviert werden, da kein ausreichendes Kontingent an Kassenordner vorhanden ist." +
                                                        "Auf der Startseite können Sie unter „weitere Kassenordner bestellen“ ein zusätzliches Kontingent buchen. Bitte versuchen Sie die Archivierung im Anschluss erneut.";
                                                    frmerror.ShowDialog();
                                                    MySqlCommand cmd2 = new MySqlCommand();
                                                    cmd2.Parameters.AddWithValue("@json", jsonText);
                                                    cmd2.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd2.Parameters.AddWithValue("@refrToken", "");
                                                    cmd2.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd2.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd2.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd2.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd2.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                    cmd2.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                    cmd2.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd2.Parameters.AddWithValue("@log", "Die Dateien(" + ZipFileName + " und metadata.json) wurden nicht übertragen, da kein ausreichendes Kontingent an Kassenordner vorhanden ist., Fehler_Nr:402.");

                                                    cmd2.Connection = myConn1;
                                                    cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                    cmd2.ExecuteNonQuery();
                                                    return;
                                                }
                                                else if (ant.ErrorNo == "Unautorisiert")
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Die Authentifizierung ist fehlgeschlagen. Bitte verbinden Sie Ihre Kasse erneut mit dem Kassenarchiv online.";
                                                    frmerror.ShowDialog();
                                                    btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                                    Program.DatevConnection = true;
                                                    btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);
                                                    MySqlCommand cmd3 = new MySqlCommand();
                                                    cmd3.Parameters.AddWithValue("@json", jsonText);
                                                    cmd3.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd3.Parameters.AddWithValue("@refrToken", "");
                                                    cmd3.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd3.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd3.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd3.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd3.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd3.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                    cmd3.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                    cmd3.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd3.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd3.Parameters.AddWithValue("@log", "Wegen der Statusprüfung wurden die Dateien(" + ZipFileName + " und metadata.json)  nicht übertragen., Fehler_Nr:401.Die Kasse muss erneut mit dem Kassenarchiv verbindet werden.");

                                                    cmd3.Connection = myConn1;
                                                    cmd3.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                    cmd3.ExecuteNonQuery();
                                                    return;
                                                }
                                                else if (ant.ErrorNo == "423")
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Die Kassendaten konnten nicht archiviert werden, da der Kassenordner im Kassenarchiv online deaktiviert ist." +
                                                                               "Um den Kassenordner zu aktivieren, melden Sie sich unter https://www.meinfiskal.de an.";
                                                    frmerror.ShowDialog();
                                                    /*btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                                    Program.DatevConnection = true;
                                                    btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);*/
                                                    MySqlCommand cmd4 = new MySqlCommand();
                                                    cmd4.Parameters.AddWithValue("@json", jsonText);
                                                    cmd4.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd4.Parameters.AddWithValue("@refrToken", "");
                                                    cmd4.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd4.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd4.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd4.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd4.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd4.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                    cmd4.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                    cmd4.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd4.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd4.Parameters.AddWithValue("@log", "Wegen der Statusprüfung wurden die Dateien(" + ZipFileName + " und metadata.json)  nicht übertragen., Fehler_Nr:423.Kassenordner deaktiviert. Um den Kassenordner zu aktivieren, melden Sie sich unter https://www.meinfiskal.de an.");

                                                    cmd4.Connection = myConn1;
                                                    cmd4.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                    cmd4.ExecuteNonQuery();
                                                    return;
                                                }
                                                else if (ant.ErrorNo == "PaymentRequired")
                                                {
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Die Kassendaten konnten nicht archiviert werden, da kein ausreichendes Kontingent an Kassenordner vorhanden ist." +
                                                        "Auf der Startseite können Sie unter „weitere Kassenordner bestellen“ ein zusätzliches Kontingent buchen. Bitte versuchen Sie die Archivierung im Anschluss erneut.";
                                                    frmerror.ShowDialog();
                                                    /*btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                                    Program.DatevConnection = true;
                                                    btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);*/
                                                    MySqlCommand cmd4 = new MySqlCommand();
                                                    cmd4.Parameters.AddWithValue("@json", jsonText);
                                                    cmd4.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd4.Parameters.AddWithValue("@refrToken", "");
                                                    cmd4.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd4.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd4.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd4.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd4.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd4.Parameters.AddWithValue("@error", ant.ErrorNo);
                                                    cmd4.Parameters.AddWithValue("@errormessage", ant.ErrorMessage);
                                                    cmd4.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd4.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd4.Parameters.AddWithValue("@log", "Wegen der Statusprüfung wurden die Dateien(" + ZipFileName + " und metadata.json)  nicht übertragen., Fehler_Nr:402. Kein asreichendes Kontingent!");

                                                    cmd4.Connection = myConn1;
                                                    cmd4.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                                    cmd4.ExecuteNonQuery();
                                                    return;
                                                }


                                            }
                                            else
                                            {
                                                //Verbindung OK!
                                                result = true;
                                                MySqlCommand cmd1 = new MySqlCommand();
                                                cmd1.Parameters.AddWithValue("@znr", berNo);
                                                cmd1.Parameters.AddWithValue("@createdate", tarih.unixdate(DateTime.Now));
                                                //cmd1.Parameters.AddWithValue("@zdate", refreshToken);
                                                cmd1.Parameters.AddWithValue("@zfile", ZipFileName);
                                                cmd1.Parameters.AddWithValue("@daydir", DatevDayDir);
                                                cmd1.Parameters.AddWithValue("@jsonfile", DatevDayDir + "\\metadata.json");
                                                cmd1.Parameters.AddWithValue("@result", 1);
                                                cmd1.Parameters.AddWithValue("@fehlercode", ant.ErrorNo);
                                                cmd1.Parameters.AddWithValue("@fehlermessage", ant.ErrorMessage);
                                                cmd1.Parameters.AddWithValue("@requestID", RequestID);
                                                cmd1.Parameters.AddWithValue("@accesstoken", accessToken);
                                                cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                if (myConn.State == ConnectionState.Closed)
                                                    myConn.Open();
                                                cmd1.Connection = myConn1;
                                                cmd1.CommandText = "INSERT INTO `datev_dfu`( `znr`, `createdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`,kassenr) VALUES " +
                                                    "(@znr,@createdate,@zfile,@daydir,@jsonfile,@result,@fehlercode,@fehlermessage,@requestID,@accesstoken,@kassenr)";
                                                if (cmd1.ExecuteNonQuery() > 0)
                                                {
                                                    string UpdateZdatev = "";
                                                    UpdateZdatev = "UPDATE zbericht SET datev='OK' WHERE Zberichtno=" + berNo;
                                                    MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn1);
                                                    cmdDatevUpdate.ExecuteNonQuery();
                                                    MySqlCommand cmd2 = new MySqlCommand();
                                                    cmd2.Parameters.AddWithValue("@json", DatevDayDir + "\\metadata.json");
                                                    cmd2.Parameters.AddWithValue("@accessToken", accessToken);
                                                    cmd2.Parameters.AddWithValue("@refrToken", refreshToken);
                                                    cmd2.Parameters.AddWithValue("@idToken", idToken);
                                                    cmd2.Parameters.AddWithValue("@reqID", RequestID);
                                                    cmd2.Parameters.AddWithValue("@tenandID", tenandID);
                                                    cmd2.Parameters.AddWithValue("@filename", ZipFileName);
                                                    cmd2.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                    cmd2.Parameters.AddWithValue("@error", "0");
                                                    cmd2.Parameters.AddWithValue("@errormessage", "");
                                                    cmd2.Parameters.AddWithValue("@level", "File-Übertrag");
                                                    cmd2.Parameters.AddWithValue("@kassenr", Program.kasano);
                                                    cmd2.Parameters.AddWithValue("@log", "Die Dateien (" + ZipFileName + " und metadata.json) wurden erfolgreich übertragen");

                                                    cmd2.Connection = myConn1;
                                                    cmd2.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr, datev_log ) VALUES" +
                                                        "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr,@log)";
                                                    cmd2.ExecuteNonQuery();
                                                    F_GenericError frmerror = new F_GenericError();
                                                    frmerror.lblMesaj.Text = "Die Daten wurden erfolgreich an das Kassenarchiv online übertragen.";
                                                    frmerror.ShowDialog();
                                                    //return;
                                                }

                                            }
                                            F_ZAuswahl zauswahl = new F_ZAuswahl();
                                            zauswahl.datevResult = result;
                                            zauswahl.DatevError = DatevError;
                                            zauswahl.ShowDialog();
                                            if (Program.userClass.ZYetki != 0)
                                            {
                                                if (zauswahl.islem == 2)
                                                {
                                                    FisBarkodlu fisclass = new FisBarkodlu();
                                                    fisclass.XZDruck(berNo);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "Fehler bei der DSFinV-K Export!";
                                        frmerror.ShowDialog();
                                        return;
                                    }
                                }
                            }


                        }
                    }
                    else
                    {
                        
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();
                            }



                            //Update Anfangbestand
                            try
                            {
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string ABSQL = "UPDATE anfangbestand SET znr=" + -1 + " WHERE znr=0 AND kasseid=" + Program.kasano;
                                    MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                    cmdAB.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ff)
                            {
                                F_GenericError frmerror1 = new F_GenericError();
                                frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                                frmerror1.ShowDialog();

                            }
                            // Abrechnungskreis

                            //Kassenbuch
                            try
                            {
                                string KasaKrSQL = "UPDATE kassenbuch SET znr=" + -1 + "  WHERE znr=0 AND kassenr=" + Program.kasano;
                                MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                                cmdKBSQL.ExecuteNonQuery();
                            }
                            catch (Exception ff)
                            {
                                F_GenericError frmerror1 = new F_GenericError();
                                frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                                frmerror1.ShowDialog();

                            }

                        
                        
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();
                            }



                            //Update Anfangbestand
                            try
                            {
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string ABSQL = "UPDATE anfangbestand SET znr=" + -1 + " WHERE znr=0 AND kasseid=" + Program.kasano;
                                    MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                    cmdAB.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ff)
                            {
                                F_GenericError frmerror1 = new F_GenericError();
                                frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                                frmerror1.ShowDialog();

                            }
                            // Abrechnungskreis

                            //Kassenbuch
                            try
                            {
                                string KasaKrSQL = "UPDATE kassenbuch SET znr=" + -1 + "  WHERE znr=0 AND kassenr=" + Program.kasano;
                                MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                                cmdKBSQL.ExecuteNonQuery();
                            }
                            catch (Exception ff)
                            {
                                F_GenericError frmerror1 = new F_GenericError();
                                frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                                frmerror1.ShowDialog();

                            }

                        
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "ES GIBT KEIN NEUER UMSATZ BEI DER KASSE!! \n DER Z-BERICHT WURDE SCHON AUSGEDRUCK \nODER\n ES GIBT KEIN NEUER UMSATZ!";
                        frmerror.ShowDialog();
                        return;
                    }





                }
                else
                {
                    
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }



                        //Update Anfangbestand
                        try
                        {
                            if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                            {
                                string ABSQL = "UPDATE anfangbestand SET znr=" + -1 + " WHERE znr=0 AND kasseid=" + Program.kasano;
                                MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                cmdAB.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror1 = new F_GenericError();
                            frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror1.ShowDialog();

                        }
                        // Abrechnungskreis

                        //Kassenbuch
                        try
                        {
                            string KasaKrSQL = "UPDATE kassenbuch SET znr=" + -1 + "  WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                            cmdKBSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror1 = new F_GenericError();
                            frmerror1.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror1.ShowDialog();

                        }

                    
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "ES GIBT KEIN NEUER UMSATZ BEI DER KASSE!! \n DER Z-BERICHT WURDE SCHON AUSGEDRUCK \nODER\n ES GIBT KEIN NEUER UMSATZ!";
                    frmerror.ShowDialog();
                    return;
                }
            }
            catch (Exception gg)
            {
                MessageBox.Show(gg.Message + "\n\n" + gg.StackTrace);
            }

        }

        private void DatevMitZdruck(int berno)
        {
            try
            {
                FisBarkodlu fisclass = new FisBarkodlu();
                fisclass.XZDruck(berNo);
            }
            catch (Exception ff)
            {
                Logger log = new Logger("LOG\\SYSTEM");
                log.Log("Error bei DATEV Zbericht Druck, wit User Zdruck volle Berectigung!");
                log.Log(ff.Message);
            }
        }

        private bool DatevDirCheck(string DatevDir)
        {
            if (!Directory.Exists(DatevDir))
            {
                Directory.CreateDirectory(DatevDir);
                return true;
            }
            else
            {
                return true;
            }
        }

        private void DSFinK_Businesscases(long znr, double yuzde19lukmiktar, double yuzde7likmiktar, double yuzde0likmiktar, double Gutschein, double RabatCouponTutar, double RabatTutar, long erstelldatum, double barTutar, double ecTutar, double scheckTutar, double stornoTutar, double totalAnfangBestand, double totalGeldEinlage, double totalGeldEntnahme)
        {
            
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }


                try
                {
                    //bbusiness case
                    MySqlCommand cmdBusinesscases = new MySqlCommand();
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19lukmiktar", yuzde19lukmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19luknetto", (yuzde19lukmiktar / 119) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde19lukmwst", yuzde19lukmiktar - (yuzde19lukmiktar / 119) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7likmiktar", yuzde7likmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7liknetto", (yuzde7likmiktar / 107) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde7likmwst", yuzde7likmiktar - (yuzde7likmiktar / 107) * 100);
                    cmdBusinesscases.Parameters.AddWithValue("@yuzde0likmiktar", yuzde0likmiktar);
                    cmdBusinesscases.Parameters.AddWithValue("@Gutschein", Gutschein);
                    cmdBusinesscases.Parameters.AddWithValue("@RabatCouponTutar", RabatCouponTutar);
                    cmdBusinesscases.Parameters.AddWithValue("@RabatTutar", RabatTutar);
                    string sqlSatisAna = "INSERT INTO `businesscases`( `datum`, `znr`, `kassenr`, `gv_typ`, `gv_name`, `agentur_id`, `ustid_id`, `z_umsatz_brutto`, `z_umsatz_netto`, `z_umsatz_ust`) VALUES" +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 7 Proz.',0,2,@yuzde19lukmiktar,@yuzde19luknetto,@yuzde19lukmwst)," +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 19 Proz.',0,1,@yuzde7likmiktar,@yuzde7liknetto,@yuzde7likmwst)," +
                        "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Umsatz','Umsatz 0 Proz.',0,6,@yuzde0likmiktar,@yuzde0likmiktar,0)";
                    //rabatt Suche
                    if (RabatTutar != 0 || RabatCouponTutar != 0)
                    {

                        string RabattSQL = "SELECT SUM(`rabatotalmenge`) as BruttoTotal, SUM(`mwst7`) AS Netto7, SUM(`mwst7betrag`) As Brutto7,SUM(`mwst19`) AS Netto19, SUM(`mwst19betrag`) As Brutto19, SUM(`mwst0betrag`)AS Netto0 FROM `rabatt` WHERE  znr=" + znr + "  AND kassenr=" + Program.kasano;
                        MySqlDataAdapter cmdRabattSQL = new MySqlDataAdapter(RabattSQL, myConn);
                        DataTable dtRabattSearch = new DataTable();
                        cmdRabattSQL.Fill(dtRabattSearch);
                        if (dtRabattSearch.Rows.Count > 0)
                        {
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7Brutto", dtRabattSearch.Rows[0].ItemArray[2]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7Netto", (Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[2]) - Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[1])));
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar7mwst", dtRabattSearch.Rows[0].ItemArray[1]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19Brutto", dtRabattSearch.Rows[0].ItemArray[4]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19Netto", (Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[4]) - Convert.ToDouble(dtRabattSearch.Rows[0].ItemArray[3])));
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar19mwst", dtRabattSearch.Rows[0].ItemArray[3]);
                            cmdBusinesscases.Parameters.AddWithValue("@RabatTutar0Brutto", dtRabattSearch.Rows[0].ItemArray[5]);

                            sqlSatisAna += ",(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 7 Proz.',0,2,@RabatTutar7Brutto,@RabatTutar7Netto,@RabatTutar7mwst)," +
                   "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 19 Proz.',0,1,@RabatTutar19Brutto,@RabatTutar19Netto,@RabatTutar19mwst)," +
                  "(" + erstelldatum + "," + znr + "," + Program.kasano + ",'Rabatt','Rabatt 0 Proz.',0,6,@RabatTutar0Brutto,@RabatTutar0Brutto,0)";


                        }


                    }
                    // Buraya Gutscheinda eklenebilir
                    cmdBusinesscases.CommandText = sqlSatisAna;
                    cmdBusinesscases.Connection = myConn;
                    cmdBusinesscases.ExecuteNonQuery();
                    // Payment
                    MySqlCommand cmdPayment = new MySqlCommand();
                    cmdPayment.Parameters.AddWithValue("@BarTutar", barTutar + stornoTutar);
                    cmdPayment.Parameters.AddWithValue("@ecTutar", ecTutar);
                    cmdPayment.Parameters.AddWithValue("@scheckTutar", scheckTutar);
                    cmdPayment.Parameters.AddWithValue("@stornoTutar", stornoTutar);
                    cmdPayment.Parameters.AddWithValue("@AnfangBestand", totalAnfangBestand);
                    cmdPayment.Parameters.AddWithValue("@Geldeinlage", totalGeldEinlage);
                    cmdPayment.Parameters.AddWithValue("@Geldantnahme", totalGeldEntnahme);
                    string sqlPayment = "INSERT INTO `payment`( `datum`, `znr`,`kassenr`, `zahlart_typ`, `zahlart_name`, `zahlart_betrag`) VALUES ";
                    if (barTutar != 0)
                    {
                        sqlPayment += " (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Bar','Bar',@BarTutar)";
                    }
                    if (ecTutar != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Unbar','Ec Karte',@ecTutar)";
                    }
                    if (scheckTutar != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Unbar','Scheck',@scheckTutar)";
                    }
                    if (scheckTutar != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Unbar','Scheck',@scheckTutar)";
                    }
                    if (totalAnfangBestand != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Bar','Anfangbestand',@AnfangBestand)";
                    }
                    if (totalGeldEinlage != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Bar','Privateinlage',@Geldeinlage)";
                    }
                    if (totalGeldEntnahme != 0)
                    {
                        sqlPayment += ", (" + erstelldatum + "," + znr + "," + Program.kasano + ",'Bar','Privatentnahme',@Geldantnahme)";
                    }
                    cmdPayment.Connection = myConn;
                    cmdPayment.CommandText = sqlPayment;
                    try
                    {
                        cmdPayment.ExecuteNonQuery();

                    }
                    catch (Exception dd)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = dd.Message + "\n" + dd.StackTrace;
                        frmerror.ShowDialog();

                    }

                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();


                }
                //KasiyerSofortStorno();
            

        }
        private double AnfangBestandReturn()
        {
            

                try
                {
                    string sqlSatisAna = "SELECT sum(`betrag`) FROM anfangbestand WHERE kasseid=" + Program.kasano + " AND znr=0";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Anfangsbetand Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            

        }
        private void ZNummerEkle(long berNo, int kasano)
        {

            try
            {
                //if (Program.GlobalAyarlar["TaglichZ"] != 1)
                // {
                
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string znrSQL = "UPDATE satisana SET znr=" + berNo + " WHERE kasano=" + Program.kasano + " AND  znr=0";
                    MySqlCommand cmdZnr = new MySqlCommand(znrSQL, myConn);
                    if (cmdZnr.ExecuteNonQuery() > 0)
                    {
                        try
                        {

                            //Update Bon_pos
                            string bonPosSQL = "UPDATE satisdetay SET znr=" + berNo + " WHERE znr=0 AND kasano=" + Program.kasano;
                            MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                            cmdbonPosSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //update Rabatt
                        try
                        {
                            string RabattSQL = "UPDATE rabatt SET znr=" + berNo + " WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                            cmdRabattSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Update Anfangbestand
                        try
                        {
                            if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                            {
                                string ABSQL = "UPDATE anfangbestand SET znr=" + berNo + " WHERE znr=0 AND kasseid=" + Program.kasano;
                                MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                cmdAB.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        // Abrechnungskreis
                        try
                        {
                            string AbrKrSQL = "UPDATE abrechnungskreis SET znr=" + berNo + " WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                            cmdAbrKrSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Kassenbuch
                        try
                        {
                            string KasaKrSQL = "UPDATE kassenbuch SET znr=" + berNo + "  WHERE znr=0 AND kassenr=" + Program.kasano;
                            MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                            cmdKBSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                    }
                

            }
            catch (Exception ss)
            {
            }
        }
        private double GeldEinlageReturn()
        {
            

                try
                {
                    string sqlSatisAna = "SELECT sum(`tutar`) FROM kassenbuch WHERE type=1 AND kassenr=" + Program.kasano + " AND znr=0 AND (anfangbestand=18 OR anfangbestand=20 or anfangbestand=23)";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Geldeinlage Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            

        }
        private double GeldEntnahmeReturn()
        {
            

                try
                {
                    string sqlSatisAna = "SELECT sum(`tutar`) FROM kassenbuch WHERE type=2 AND kassenr=" + Program.kasano + " AND znr=0 AND (anfangbestand=19 OR anfangbestand=22 or anfangbestand=24)";
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Geldentnahme Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            

        }
        private double AnfangBestandReturnBedienr(Int32 BedinerId)
        {
            

                try
                {
                    string sqlSatisAna = "SELECT sum(`betrag`) FROM anfangbestand WHERE datum >=" +
                            tarih.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND datum <" + tarih.gunBitis(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) +
                            " AND  kasseid=" + Program.kasano + " AND znr=0 AND bedienerid= " + BedinerId;
                    MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn);
                    DataTable dtSatisAna = new DataTable("satisana");
                    dtSatisAna.Rows.Clear();
                    daSatisAna.Fill(dtSatisAna);
                    if (dtSatisAna.Rows.Count > 0)
                    {
                        if (dtSatisAna.Rows[0].ItemArray[0] is DBNull)
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(dtSatisAna.Rows[0].ItemArray[0]);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ee)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Printer Error!\n Orginal Message:" + ee.Message;
                    frmerror.ShowDialog();

                    return 0;
                }
                //KasiyerSofortStorno();
            

        }
        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                FisBarkodlu fisclass = new FisBarkodlu();
                fisclass.XDruck();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                Dictionary<string, int> ParaAdetleri = new Dictionary<string, int>();

                KassenZahler kz = new KassenZahler();
                ParaAdetleri = kz.KassenZahler1();
                if (ParaAdetleri.Count > 0)
                {
                    FisBarkodlu fisclass = new FisBarkodlu();
                    fisclass.KassenZahlerDruck(ParaAdetleri);
                    this.Close();
                }
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
            /* alte Gutschein
            try
            {
                Dictionary<string, string> gutschein = new Dictionary<string, string>();
                Gutschein gtschn = new Gutschein();
                gtschn.Virgulayari = Program.GlobalAyarlar["BKOMMA"];
                gtschn.m_Printer = Program.printer;
                gtschn.IsletmeAdi = Program.IsletmeAyarlar["isletme"];
                gtschn.Bediener = Program.bedAdSoyad;
                string brkd = "";
                if (gutscheinbrkd == "")
                {
                    brkd = getBarkod();

                }
                else
                {
                    brkd = gutscheinbrkd;

                }
                gtschn.Brkd = brkd;
                gutschein = gtschn.GutscheinDruck();
                if (gutschein.Count > 0)
                {
                    using (myConn = baglanti.myconn())
                    {
                        try
                        {
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();
                            }
                            //INSERT INTO `gutschein`(`id`, `fisno`, `erstelldatum`, `ablaufdatum`, `barkode`, `erstelltmiktar`, `bedienerid`, `bewertungdate`, `aktif`, `restmiktar`)
                            string insertSQL = "INSERT INTO gutschein VALUES('','', " + gutschein["erstellt"] + ", " + gutschein["ablauf"] + ",'" + brkd + "'," + vA.virgulayikla(Convert.ToDouble(gutschein["Betrag"])) + "," + Program.bedID + ", 0, 0," + vA.virgulayikla(Convert.ToDouble(gutschein["Betrag"])) + ")";
                            MySqlCommand coKupon = new MySqlCommand(insertSQL, myConn);
                            if (coKupon.ExecuteNonQuery() > 0)
                            {

                            }
                            else
                            {
                                MessageBox.Show("S:332-FIsl" + Program.lang["776"]);
                            }
                        }
                        catch (Exception ss)
                        {
                            MessageBox.Show(ss.Message + "\nss Exception");
                        }


                    }
                }
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.Message + "\ndd Exception");
            }
            */
        }
        private string getMaxID()
        {
            
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string maxIdSQL = "SELECT max(id) as Maxid from gutschein ";
                MySqlDataAdapter daMaxID = new MySqlDataAdapter(maxIdSQL, myConn);
                DataTable dtMaxID = new DataTable();
                dtMaxID.Rows.Clear();
                daMaxID.Fill(dtMaxID);
                string maxID = "";
                if (dtMaxID.Rows[0].ItemArray[0] is DBNull)
                {
                    maxID = 1.ToString();
                }
                else
                {
                    maxID = (Convert.ToInt32(dtMaxID.Rows[0].ItemArray[0]) + 1).ToString();
                }
                //int uz = maxID.Length;
                while (maxID.Length < (12 - (4 + Program.IsletmeAyarlar["kod"].Length)))
                {
                    maxID = "0" + maxID;
                }
                return maxID;

            
        }
        private string getBarkod()
        {
            Ean13 barcode = new Ean13();
            barcode.CountryCode = "24" + Convert.ToInt16(Program.IsletmeAyarlar["kod"]) + "04";
            barcode.ManufacturerCode = "";
            barcode.ProductCode = getMaxID();
            return barcode.ToString();
            //picture1.Image = barcode.CreateBitmap();
        }




        private void BelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        /*if (line.Substring(0, 1) == "C")
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.kundenbeleg.Add(inf[1]);
                            }
                        }
                        else
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.handlerbeleg.Add(inf[1]);
                            }
                        }*/
                        string[] inf = line.Split(';');
                        if (inf[1] == "")
                        {
                            continue;
                        }
                        else
                        {

                            if (inf[1].Contains("Kundenbeleg") == true)
                            {
                                islem = "kunde";
                            }
                            else if (inf[1].Contains("Händlerbeleg") == true)
                            {
                                islem = "handler";
                            }
                            if (islem == "kunde")
                            {
                                Program.kundenbeleg.Add(inf[1]);
                            }
                            else if (islem == "handler")
                            {
                                Program.handlerbeleg.Add(inf[1]);
                            }

                            /* if (((inf[1].Substring(0,1) == "*") && (inf[1].Substring(1,1) == " ") && (inf[1].Substring(2,1) == "*"))&&(islem==""))
                             {
                                 islem = "kunde";

                             }
                             else if (((inf[1].Substring(0, 1) == "*") && (inf[1].Substring(1, 1) == " ") && (inf[1].Substring(2, 1) == "*")) && (islem == ""))
                             {
                                 islem = "kunde";

                             }*/
                        }


                    }
                }
            }
            catch
            {
            }
        }
        private void timer1Tick()
        {
            while (pr == false)
            {
                if (!backgroundWorker1.IsBusy)
                {
                    try
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            Rea.Redruck();
            CheckForIllegalCrossThreadCalls = false;
            // label2.Text = "";
            // Rea.WriteInDatei();
            //timer1Tick();
            Thread.Sleep(200);
            while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
            {
            }
            string erg = "";
            erg = Rea.ZwischenInfo();
            label2.Text = erg;
            if (erg != "")
            {
                if (erg == "OK")
                {
                    // odemesonuc = true;
                    if (Program.zvt == "ReaRetail")
                    {
                        ReticketBelegVorberaiten();
                        if (Program.reticket.Count > 0)
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.ReticketDruck();
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Reticket-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                            frmerror.ShowDialog();
                        }
                    }

                    this.Close();

                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = erg;
                    frmerror.ShowDialog();

                }


            }
        }

        private void ReticketBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {


                        string[] inf = line.Split(';');
                        Program.reticket.Add(inf[1]);



                    }
                }
            }
            catch
            {
            }
        }

        private void kryptonButton11_Click(object sender, EventArgs e)
        {
            string Belegno = "";
            using (F_RakamForm frakam = new F_RakamForm())
            {
                frakam.ShowDialog();
                if (frakam.islem == true)
                {
                    if (frakam.BelegNo != "")
                    {
                        Rea.Storno(frakam.BelegNo);
                        CheckForIllegalCrossThreadCalls = false;
                        // label2.Text = "";
                        // Rea.WriteInDatei();
                        //timer1Tick();
                        while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
                        {
                        }
                        string erg = "";
                        erg = Rea.ZwischenInfo();
                        label2.Text = erg;
                        if (erg != "")
                        {
                            if (erg == "OK")
                            {
                                // odemesonuc = true;
                                if (Program.zvt == "ReaRetail")
                                {
                                    StornoticketBelegVorberaiten();
                                    if (Program.stornoticketKunde.Count > 0)
                                    {
                                        FisBarkodlu fisclass = new FisBarkodlu();
                                        fisclass.StornoticketDruck();
                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "StornoBeleg-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                                        frmerror.ShowDialog();
                                    }
                                }

                                this.Close();

                            }
                            else
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = erg;
                                frmerror.ShowDialog();

                            }

                        }
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Beleg-Nr Fehlt!";
                    frmerror.ShowDialog();

                }
            }
        }
        private void StornoticketBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "C")
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.stornoticketKunde.Add(inf[1]);
                            }
                        }
                        else
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.stornoticketHandler.Add(inf[1]);
                            }
                        }


                    }
                }
            }
            catch
            {
            }
        }

        private void kryptonButton9_Click(object sender, EventArgs e)
        {

            Rea.Diag();
            CheckForIllegalCrossThreadCalls = false;
            // label2.Text = "";
            // Rea.WriteInDatei();
            //timer1Tick();
            Thread.Sleep(200);
            while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
            {
            }
            string erg = "";
            erg = Rea.ZwischenInfo();
            label2.Text = erg;
            if (erg != "")
            {
                if (erg == "OK")
                {
                    // odemesonuc = true;
                    if (Program.zvt == "ReaRetail")
                    {
                        DiagBelegVorberaiten();
                        if (Program.diagTicket.Count > 0)
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.DiagDruck();
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Diag-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                            frmerror.ShowDialog();
                        }
                    }

                    this.Close();

                }


            }
        }

        private void DiagBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "A")
                        {

                            string[] inf = line.Split(';');
                            Program.diagTicket.Add(inf[1]);

                        }


                    }
                }
            }
            catch
            {
            }
        }
        private void kryptonButton10_Click(object sender, EventArgs e)
        {
            if (Program.ProgramAyarlar["zvt"] == "ReaRetail" && Program.ReaGerateTyp == "INGENICO")
            {
                if(berNo==0)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Bitte drucken Sie zuerst Z-Bericht aus!\n Dann nochmal versuchen!";
                    frmerror.ShowDialog();
                }
            }
            KryptonButton btnTik = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(btnTik.Name);//
            if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
            {
                this.Enabled = false;
                try
                {
                    string ergebnis = "";
                    ergebnis = Rea.Kassenschnitt();
                    if (ergebnis == "OK")
                    {
                        if (Program.zvt == "ReaRetail")
                        {
                            while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
                            {
                            }
                            Thread.Sleep(300);
                            Thread.Sleep(3000);
                            while (!File.Exists((Application.StartupPath + "\\REAZVT.tck")))
                            {
                            }
                            Thread.Sleep(500);
                            List<String> Ticket = KassenSchnittBelegVorberaiten();

                            if (Ticket.Count > 0)
                            {
                                FisBarkodlu fisclass = new FisBarkodlu();
                                fisclass.KassenschnittDruck(Ticket, berNo);
                                this.Close();
                            }
                            else
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Kassenschnitt-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                                frmerror.ShowDialog();
                            }

                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = ergebnis;
                        frmerror.ShowDialog();
                    }
                }
                catch (Exception gg)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = gg.Message + "\n InnerExcep:" + gg.InnerException.Message;
                    Log log = new Log();
                    log.AddtoLogFile(gg.Message + "\n InnerExcep:" + gg.InnerException.Message, "Fislemler-867");
                    frmerror.ShowDialog();
                    this.Enabled = true;
                }
                this.Enabled = true;
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void ReaProc()
        {
            Rea.dllInUse = true;
            Rea.Kassenschnitt();
        }
        private List<string> KassenSchnittBelegVorberaiten()
        {
            List<string> returnTicket = new List<string>();
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();

                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "A")
                        {

                            string[] inf = line.Split(';');
                            returnTicket.Add(inf[1]);

                        }



                    }
                   
                    return returnTicket;
                }
            }
            catch
            {
                return returnTicket;
            }
            return returnTicket;
        }

        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            Rea.Abmelden();
        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            iss_gdpdu.F_DataExport frmDataExport = new iss_gdpdu.F_DataExport();
            frmDataExport.myCon = myConn;
            frmDataExport.Host = myConn.DataSource;
            frmDataExport.IsletmeAyarlar = Program.IsletmeAyarlar;
            frmDataExport.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            string file = Application.StartupPath + "\\BACKUP\\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".sql";

           
                // MemoryStream ms = new MemoryStream();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = myConn;
                        myConn.Open();

                        //mb.ExportToMemoryStream(ms);

                        /*TextReader tsMemory = new StreamReader(ms,System.Text.Encoding.UTF8);

                        MessageBox.Show(tsMemory.ReadToEnd());
                        TextWriter tswriter = new StreamWriter(file);
                        mb.EncryptDumpFile(tsMemory, tswriter, "sahbaz");
                        */
                        //mb.Database.DefaultCharacterSet =
                        mb.ExportToFile(file);

                        myConn.Close();
                    }
                }

            
        }
        void LoadIntoMemory(byte[] ba)
        {
            if (ba == null || ba.Length == 0)
            {
                ClearMemory();
            }
            else
            {
                _ba = ba;
                //lbStatus.Text = "Loaded into memory.";
                //lbStatus.ForeColor = Color.DarkGreen;
                // btImport.Enabled = true;
            }
        }

        void ClearMemory()
        {
            _ba = null;
            // lbStatus.Text = "No dump content is loaded in memory.";
            // lbStatus.ForeColor = Color.Black;
            // btImport.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            geparkteBonList.Clear();
            F_SystemParameter ParamaterForm = new F_SystemParameter();
            ParamaterForm.ShowDialog();
        }

        private void kryptonButton14_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("TeamViewerQS.exe");
            }
            catch
            {
            }
        }

        private void btnZweiteEbene_Click(object sender, EventArgs e)
        {
            try
            {
                KryptonButton btnTik = sender as KryptonButton;
                YetkiCheck yetkiCheck = new YetkiCheck();
                yetkiCheck.YetkiKontrol(btnTik.Name);//
                if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
                {

                    F_Neuste frmNeustart = new F_Neuste();
                    frmNeustart.ShowDialog();
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["6"];
                    frmerror.ShowDialog();
                }
            }
            catch (Exception fff)
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = fff.Message;
                frmerror.ShowDialog();
            }

        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                FisBarkodlu fisclass = new FisBarkodlu();
                fisclass.TagesjournalDruck();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }

        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {

        }

        private void btnTSEon_Click(object sender, EventArgs e)
        {




        }

        private void btnTSEoff_Click(object sender, EventArgs e)
        {



        }

        private void kryptonButton15_Click(object sender, EventArgs e)
        {
            if (Program.printer != null)
            {
                if (Program.printer.DeviceEnabled == true)
                {
                    Program.printer.DeviceEnabled = false;
                    Program.printer.Release();
                    btnPrinter.Text = "Drucker Zurücknehmen";
                    //System.Diagnostics.Process.Start("http://google.com");
                }
                else
                {
                    Program.printer.Claim(1000);
                    Program.printer.DeviceEnabled = true;
                    btnPrinter.Text = "Drucker Freilassen";
                }
            }
            else
            {
                Program.printer.Claim(1000);
                Program.printer.DeviceEnabled = true;
                btnPrinter.Text = "Drucker Freilassen";
                //System.Diagnostics.Process.Start("http://google.com");
            }

        }

        private void kryptonButton15_Click_1(object sender, EventArgs e)
        {
            iss_gdpdu.F_DataExport frmDataExport = new iss_gdpdu.F_DataExport();
            frmDataExport.myCon = myConn;
            frmDataExport.Host = myConn.DataSource;
            frmDataExport.IsletmeAyarlar = Program.IsletmeAyarlar;
            frmDataExport.Show();
        }

        private void kryptonButton16_Click(object sender, EventArgs e)
        {

            try
            {
                F_TSEexport tseExport = new F_TSEexport();
                tseExport.ShowDialog();
                if (tseExport.sonuc == 0)
                {
                    return;
                }
                else if (tseExport.sonuc == 1)
                {
                    if (Program.TSE == "1")
                    {

                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.InitialDirectory = @Application.StartupPath + "\\TSEExport\\";
                        saveFile.FileName = "TSEExport-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".tar";
                        saveFile.Filter = "TAR archive (*.tar)|*.tar|All files (*.*)|*.*";
                        if (saveFile.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        forceGuiRefresh();
                        Program.TSEdll.exportTar(saveFile);
                        MessageBox.Show("TSE-Dateien wurden erfolgreich exportiert!");
                    }
                }
                else if (tseExport.sonuc == 2)
                {
                    if (Program.TSE == "1")
                    {
                        DateTime startDate;
                        DateTime endDate;
                        UInt64 startDateUnix = 0;
                        UInt64 endDateUnix = 0;

                        startDate = new DateTime(tseExport.startDate.Year, tseExport.startDate.Month, tseExport.startDate.Day);
                        endDate = new DateTime(tseExport.endDate.Year, tseExport.endDate.Month, tseExport.endDate.Day);
                        //startDateUnix = Program.TSEdll.DateTimeUnixTime(startDate);
                        //endDateUnix = Program.TSEdll.DateTimeUnixTime(endDate);
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.InitialDirectory = @Application.StartupPath + "\\TSEExport\\";
                        saveFile.FileName = "TSEExport-von-" + startDate.Day + "-" + startDate.Month + "-" + startDate.Year + "-bis-" + endDate.Day + "-" + endDate.Month + "-" + endDate.Year + ".tar";
                        saveFile.Filter = "TAR archive (*.tar)|*.tar|All files (*.*)|*.*";
                        FileStream stream = (System.IO.FileStream)saveFile.OpenFile();
                        if (saveFile.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        forceGuiRefresh();
                        //Program.TSEdll.exportTarDatum(startDateUnix, endDateUnix, saveFile, Program.ClientID);
                        MessageBox.Show("TSE-Dateien wurden erfolgreich exportiert!");
                    }
                }


            }
            catch (Exception ex)
            {

                return;
            }
            finally
            {

            }
        }

        private void forceGuiRefresh()
        {
            Application.DoEvents();
        }

        private void btnHandyAuflade_Click(object sender, EventArgs e)
        {
            //F_HandAufladeReport report = new F_HandAufladeReport();
            //report.ShowDialog();
            if (Program.IsletmeAyarlar["AufladeFirma"] == "Debeka")
            {

                /*iss_HandyAuflade_Manegement.Construktur constr = new iss_HandyAuflade_Manegement.Construktur();
                constr.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                constr.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                //constr.Wurl = Program.IsletmeAyarlar["HandyAufladeAPI"];
                constr.myConn = conn;
                constr.GetListe();*/

            }
            else
            {
                int VerkaufID = -1;
                try
                {

                    /* iss_HandyAuflade_Manegement_Mopin.Construktur constr = new iss_HandyAuflade_Manegement_Mopin.Construktur();
                     constr.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
                     constr.password = Program.IsletmeAyarlar["HandyAufladePassword"];
                     constr.URL = Program.IsletmeAyarlar["HandyAufladeAPI"];
                     constr.myConn = conn;
                     //MessageBox.Show(constr.username + "\n" + constr.password + "\n" + constr.URL);
                     constr.GetListe();
                     VerkaufID = constr.selectedVerkaufID;
                     if (VerkaufID != -1)
                     {
                         if (MessageBox.Show("Möchten Sie Aufladekarte-Informationen nochmal ausdrucken?", "Bitte Bestätigen!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                         {
                             FisBarkodlu fisDruck = new FisBarkodlu();
                             fisDruck.HandAufladeTicketDruck(VerkaufID);
                         }
                     }*/
                }
                catch (Exception dd)
                {
                    MessageBox.Show(dd.Message + "" + dd.StackTrace);
                }
            }
        }

        private void btnZarsive_Click(object sender, EventArgs e)
        {
            F_Zarchive arshive = new F_Zarchive();
            arshive.myConn = conn;
            arshive.ShowDialog();
            this.Close();
        }

        private void btnTSEInfoDruck_Click(object sender, EventArgs e)
        {
            /*FisBarkodlu fisclass = new FisBarkodlu();
             fisclass.TagesjournalDruck();*/
            F_TSEInfo tseinfo = new F_TSEInfo();
            tseinfo.ShowDialog();
        }

        private void FIslemler_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Control) && (e.KeyCode == Keys.Z))
            {
                // F_ZKorrektur zkorrek = new F_ZKorrektur();
                // zkorrek.ShowDialog();
            }
        }

        private void FIslemler_KeyPress(object sender, KeyPressEventArgs e)
        {
            //F_ZKorrektur zkorrek = new F_ZKorrektur();
            //zkorrek.ShowDialog();
        }

        private void FIslemler_DoubleClick(object sender, EventArgs e)
        {
            if (DateTime.Now.Year == 2025 || DateTime.Now.Month == 3 || DateTime.Now.Day == 2)
            {
                F_ZKorrektur zkorrek = new F_ZKorrektur();
                zkorrek.ShowDialog();
            }
        }

        private void btnDatevNach_Click(object sender, EventArgs e)
        {
            Int16 berNo = 0;
            try
            {
                if (Int16.TryParse(txtZnr.Text, out berNo))
                {
                    string ZinfoSql = "";
                    ZinfoSql = "SELECT startBonID, endBonID, datev, zfile FROM Zbericht LEFT JOIN datev_dfu ON Zberichtno=znr WHERE Zberichtno=" + berNo;
                    MySqlDataAdapter myDaZInfo = new MySqlDataAdapter(ZinfoSql, myConn);
                    DataTable dtZinfo = new DataTable();
                    myDaZInfo.Fill(dtZinfo);
                    if (dtZinfo.Rows.Count == 0)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Der Z-Bericht konnte nicht gefunden werden! ";
                        frmerror.ShowDialog();
                        return;
                    }
                    if (!(dtZinfo.Rows[0].ItemArray[2] is DBNull))
                    {
                        if (dtZinfo.Rows[0].ItemArray[2].ToString() == "OK")
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Die Kassendaten sind bereits in das Kassenarchiv online übertragen worden. " +
                                                      "Weitere Informationen entnehmen Sie bitte dem Logfile in Ihrem Kassensystem.\n" + "Dateiname: " + dtZinfo.Rows[0].ItemArray[3].ToString();
                            frmerror.ShowDialog();
                            return;


                        }
                    }

                    string DatevDir = @Application.StartupPath + "\\DATEV\\";
                    iss_gdpdu.F_DataExport frmDataExport = new iss_gdpdu.F_DataExport();
                    frmDataExport.myCon = myConn;
                    frmDataExport.Host = myConn.DataSource;
                    frmDataExport.IsletmeAyarlar = Program.IsletmeAyarlar;
                    if (!Directory.Exists(DatevDir))
                    {
                        Directory.CreateDirectory(DatevDir);

                    }
                    else
                    {
                        string DatevDayDir = @Application.StartupPath + "\\DATEV\\" + DateTime.Now.ToString("yyyy-MM-dd");

                        if (DatevDirCheck(DatevDayDir) == true)
                        {
                            string ZipFileName = frmDataExport.TableExportDATEVDSFinK(DatevDayDir, berNo.ToString(), Program.kasano,0, 0);
                            if (ZipFileName != "")
                            {
                                string ZStartSQL = "", Date_from = "", Date_to = "";
                                ZStartSQL = "SELECT tarih FROM satisana WHERE (localbonid=" + dtZinfo.Rows[0].ItemArray[0] + " OR localbonid=" + dtZinfo.Rows[0].ItemArray[1] + ") AND kasano=" + Program.kasano + " AND znr=" + berNo + " ORDER by localbonid";
                                MySqlDataAdapter myDaZStartBon = new MySqlDataAdapter(ZStartSQL, myConn);
                                DataTable myDtZStartBon = new DataTable();
                                myDtZStartBon.Rows.Clear();
                                myDaZStartBon.Fill(myDtZStartBon);
                                Date_from = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows[0].ItemArray[0])).ToString("yyyy-MM-dd");
                                Date_to = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows.Count == 1 ? myDtZStartBon.Rows[0].ItemArray[0] : myDtZStartBon.Rows[1].ItemArray[0])).ToString("yyyy-MM-dd");

                                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                                string jsonText = "{\r\n  \"document_type\": \"CASH_POINT_CLOSING\",\r\n  \"note\": \"ISS POS Kassensystem-Datev Integration\",\r\n  \"extensions\": {\r\n    \"cash_register\": {\r\n      \"address\": {\r\n        \"street\": \"" + Program.IsletmeAyarlar["strase"] +
                                    "\",\r\n        \"postal_code\": \"" + Program.IsletmeAyarlar["plz"] + "\",\r\n        \"city\": \"" + Program.IsletmeAyarlar["stadt"] + "\",\r\n        \"country_code\": \"DE\"\r\n      },\r\n      \"serial_number\": \"" + Program.HerstellerKasseID +
                                    "\",\r\n      \"manufacturer\": \"Windsoft Tech GmbH\",\r\n      \"model_type\": \"ISS POS \",\r\n      \"name\": \"" + Program.HerstellerKasseID + "\",\r\n      \"description\": \"Windsoft Tech GmbH-ISS POS Kassensysteme \"\r\n    },\r\n    \"client_application\": {\r\n      \"client_application_name\": \"ISS POS Kassensystem\",\r\n      \"client_application_vendor\": \"Windsoft Tech GmbH\",\r\n      \"client_application_version\": \"" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion +
                                    "\"\r\n    },\r\n    \"date_from\": \"" + Date_from + "\",\r\n    \"date_to\": \"" + Date_to + "\"\r\n  }\r\n}";

                                string RequestID = "", tenandID = "", accessToken = "";
                                try
                                {
                                    Directory.Delete(DatevDayDir);
                                }
                                catch (Exception dd)
                                {

                                }
                                //jsonInfo = JsonConvert.SerializeObject(cash_register);
                                RequestID = datevMain.RandomString(20);
                                if (!Directory.Exists(DatevDayDir))
                                {
                                    Directory.CreateDirectory(DatevDayDir);
                                    //File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                                    //File.WriteAllText(@DatevDir + "\\metadata.json", JsonConvert.SerializeObject(cash_register));
                                    File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);

                                }
                                else
                                {

                                    if (!File.Exists(DatevDayDir + @"\\metadata.json"))
                                    {

                                        File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                    }
                                    else
                                    {
                                        File.Delete(DatevDayDir + @"\\metadata.json");
                                        File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                    }
                                }
                                // TenandID und AcceToken
                                string idToken = "";
                                string DatevInfo = "SELECT  `tenandid`,  `accesstoken`, idtoken FROM `datev_info`";
                                MySqlDataAdapter myDaDatevInfo = new MySqlDataAdapter(DatevInfo, myConn);
                                DataTable dtINfo = new DataTable();
                                myDaDatevInfo.Fill(dtINfo);
                                if (dtINfo.Rows.Count > 0)
                                {
                                    bool result = false;
                                    string DatevError = "";
                                    tenandID = dtINfo.Rows[0].ItemArray[0].ToString();
                                    accessToken = dtINfo.Rows[0].ItemArray[1].ToString();

                                    idToken = dtINfo.Rows[0].ItemArray[2].ToString();
                                    FileInfo fi = new FileInfo(@DatevDayDir + "\\" + ZipFileName);
                                    double FileSize = 0;
                                    FileSize = fi.Length / (1024 * 1024);
                                    if (FileSize > 200)
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "Die Daten können nicht übertragen werden. Die maximale Dateigröße von 200 MB wurde überschritten." +
                                            " Weitere Informationen entnehmen Sie bitte dem Logfile in Ihrem Kassensystem.";
                                        frmerror.ShowDialog();
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@json", jsonText);
                                        cmd1.Parameters.AddWithValue("@accessToken", accessToken);
                                        cmd1.Parameters.AddWithValue("@refrToken", "");
                                        cmd1.Parameters.AddWithValue("@idToken", idToken);
                                        cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                        cmd1.Parameters.AddWithValue("@tenandID", tenandID);
                                        cmd1.Parameters.AddWithValue("@filename", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                        cmd1.Parameters.AddWithValue("@error", "FILEIMPORT_BE_ARCHIVE_000026");
                                        cmd1.Parameters.AddWithValue("@errormessage", "Dateigroße ist größer als 200MB");
                                        cmd1.Parameters.AddWithValue("@level", "File-Übertrag");
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                        cmd1.Parameters.AddWithValue("@log", "Die Daten können nicht übertragen werden. Die maximale Dateigröße von 200 MB wurde überschritten.");

                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr,datev_log ) VALUES" +
                                            "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr, @log)";
                                        cmd1.ExecuteNonQuery();
                                    }

                                    Antwort ant = new Antwort();
                                    ant = datevMain.fileUpload2(accessToken, RequestID, tenandID, @DatevDayDir + "\\metadata.json", @DatevDayDir + "\\" + ZipFileName, ZipFileName);
                                    if (ant.ErrorMessage != "")
                                    {
                                        // z bericht Datev Field Update
                                        // Datev FileLog
                                        /*INSERT INTO `datev_dfu`(`id`, `znr`, `createdate`, `zdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`) VALUES 
                                         * ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12])*/
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@znr", berNo);
                                        cmd1.Parameters.AddWithValue("@createdate", tarih.unixdate(DateTime.Now));
                                        //cmd1.Parameters.AddWithValue("@zdate", refreshToken);
                                        cmd1.Parameters.AddWithValue("@zfile", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@daydir", DatevDayDir);
                                        cmd1.Parameters.AddWithValue("@jsonfile", DatevDayDir + "\\metadata.json");
                                        cmd1.Parameters.AddWithValue("@result", 0);
                                        cmd1.Parameters.AddWithValue("@fehlercode", ant.ErrorNo);
                                        cmd1.Parameters.AddWithValue("@fehlermessage", ant.ErrorMessage);
                                        cmd1.Parameters.AddWithValue("@requestID", RequestID);
                                        cmd1.Parameters.AddWithValue("@accesstoken", accessToken);
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                                        if (myConn.State == ConnectionState.Closed)
                                            myConn.Open();
                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_dfu`( `znr`, `createdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`,kassenr) VALUES " +
                                            "(@znr,@createdate,@zfile,@daydir,@jsonfile,@result,@fehlercode,@fehlermessage,@requestID,@accesstoken,@kassenr)";
                                        if (cmd1.ExecuteNonQuery() > 0)
                                        {
                                            string UpdateZdatev = "";
                                            UpdateZdatev = "UPDATE zbericht SET datev='ERROR' WHERE Zberichtno=" + berNo;
                                            MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn);
                                            cmdDatevUpdate.ExecuteNonQuery();
                                        }

                                        result = false;
                                        DatevError = ant.ErrorMessage;
                                        if (ant.ErrorNo == "Unauthorized")
                                        {
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = "Die Authentifizierung ist fehlgeschlagen. Bitte verbinden Sie Ihre Kasse erneut mit dem Kassenarchiv online.";
                                            frmerror.ShowDialog();
                                            btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.DatevVerbinden;
                                            Program.DatevConnection = false;
                                            btnDatevVerbinden.Click += new System.EventHandler(this.kryptonButton1_Click_1);

                                            return;
                                        }
                                        else if (ant.ErrorNo == "423")
                                        {
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = "Die Kassendaten konnten nicht archiviert werden, da der Kassenordner im Kassenarchiv online deaktiviert ist." +
                                                                       "Um den Kassenordner zu aktivieren, melden Sie sich unter https://www.meinfiskal.de an.";
                                            frmerror.ShowDialog();
                                            /*btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                            Program.DatevConnection = true;
                                            btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);*/

                                            return;
                                        }



                                    }
                                    else
                                    {
                                        //Verbindung OK!
                                        result = true;
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@znr", berNo);
                                        cmd1.Parameters.AddWithValue("@createdate", tarih.unixdate(DateTime.Now));
                                        //cmd1.Parameters.AddWithValue("@zdate", refreshToken);
                                        cmd1.Parameters.AddWithValue("@zfile", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@daydir", DatevDayDir);
                                        cmd1.Parameters.AddWithValue("@jsonfile", DatevDayDir + "\\metadata.json");
                                        cmd1.Parameters.AddWithValue("@result", 1);
                                        cmd1.Parameters.AddWithValue("@fehlercode", ant.ErrorNo);
                                        cmd1.Parameters.AddWithValue("@fehlermessage", ant.ErrorMessage);
                                        cmd1.Parameters.AddWithValue("@requestID", RequestID);
                                        cmd1.Parameters.AddWithValue("@accesstoken", accessToken);
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                        if (myConn.State == ConnectionState.Closed)
                                            myConn.Open();
                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_dfu`( `znr`, `createdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`,kassenr) VALUES " +
                                            "(@znr,@createdate,@zfile,@daydir,@jsonfile,@result,@fehlercode,@fehlermessage,@requestID,@accesstoken,@kassenr)";
                                        if (cmd1.ExecuteNonQuery() > 0)
                                        {
                                            string UpdateZdatev = "";
                                            UpdateZdatev = "UPDATE zbericht SET datev='OK' WHERE Zberichtno=" + berNo;
                                            MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn);
                                            cmdDatevUpdate.ExecuteNonQuery();
                                        }
                                    }
                                    F_ZAuswahl zauswahl = new F_ZAuswahl();
                                    zauswahl.datevResult = result;
                                    zauswahl.DatevError = DatevError;
                                    zauswahl.ShowDialog();

                                    if (zauswahl.islem == 2)
                                    {
                                        FisBarkodlu fisclass = new FisBarkodlu();
                                        fisclass.XZDruck(berNo);
                                    }
                                }
                            }
                            else
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Fehler bei der DSFinV-K Export!";
                                frmerror.ShowDialog();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    //Zbericht hatali
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Bitte geben Sie eine gültige Z-Nr!";
                    frmerror.ShowDialog();
                    return;
                }
            }
            catch (Exception dd)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = dd.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void kryptonButton17_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                F_GeldEinlage geldEinlage = new F_GeldEinlage();
                if (Program.printerType == "bixolon")
                {
                    try
                    {
                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                        // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    }
                    catch
                    {

                    }

                }
                else
                {
                    try
                    {
                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                    }
                    catch
                    {

                    }
                }

                geldEinlage.ShowDialog();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }

        }

        private void kryptonButton1_Click_2(object sender, EventArgs e)
        {
            MessageBox.Show("in Bearbeitung");
            // TSEanDATEV();
        }

        private void kryptonButton5_Click_1(object sender, EventArgs e)
        {
            F_CameraWagenCheck cam= new F_CameraWagenCheck();
            cam.ShowDialog();

        }

        private void kryptonButton18_Click(object sender, EventArgs e)
        {
            KryptonButton kryptonButton = sender as KryptonButton;
            yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol(kryptonButton.Name);
            if (yetkiCheck.YetkiTanimlimi && yetkiCheck.Durum == 1L || !yetkiCheck.YetkiTanimlimi)
            {
                F_GeldEntnahme geldEntnahme = new F_GeldEntnahme();
                try
                {
                    if (Program.printerType == "bixolon")
                    {
                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                        //Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                        // Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)0) + ((char)50) + ((char)250));
                    }
                    else
                    {
                        Program.printer.PrintNormal(PrinterStation.Receipt, "" + ((char)27) + ((char)112) + ((char)48) + ((char)55) + ((char)121));
                    }
                }
                catch
                {

                }
                geldEntnahme.ShowDialog();
            }
            else
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = Program.lang["6"];
                int num = (int)fGenericError.ShowDialog();
            }
        }

        private void btnHersSWID_Click(object sender, EventArgs e)
        {
            try
            {
                Program.ept.ReadHerstellerSWID();

            }
            catch
            {
            }
        }

        private void kryptonButton11_MouseHover(object sender, EventArgs e)
        {

        }
        private void TSEanDATEV()
        {
            Int16 berNo = 0;
            try
            {
                if (Int16.TryParse(txtZnr.Text, out berNo))
                {
                    string ZinfoSql = "";
                    ZinfoSql = "SELECT startBonID, endBonID, datev, zfile FROM Zbericht LEFT JOIN datev_dfu ON Zberichtno=znr WHERE Zberichtno=" + berNo;
                    MySqlDataAdapter myDaZInfo = new MySqlDataAdapter(ZinfoSql, myConn);
                    DataTable dtZinfo = new DataTable();
                    myDaZInfo.Fill(dtZinfo);
                    if (dtZinfo.Rows.Count == 0)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = "Der Z-Bericht konnte nicht gefunden werden! ";
                        frmerror.ShowDialog();
                        return;
                    }
                    if (!(dtZinfo.Rows[0].ItemArray[2] is DBNull))
                    {
                        if (dtZinfo.Rows[0].ItemArray[2].ToString() == "OK")
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Die Kassendaten sind bereits in das Kassenarchiv online übertragen worden. " +
                                                      "Weitere Informationen entnehmen Sie bitte dem Logfile in Ihrem Kassensystem.\n" + "Dateiname: " + dtZinfo.Rows[0].ItemArray[3].ToString();
                            frmerror.ShowDialog();
                            return;


                        }
                    }

                    string DatevDir = @Application.StartupPath + "\\DATEV\\";
                    iss_gdpdu.F_DataExport frmDataExport = new iss_gdpdu.F_DataExport();
                    frmDataExport.myCon = myConn;
                    frmDataExport.Host = myConn.DataSource;
                    frmDataExport.IsletmeAyarlar = Program.IsletmeAyarlar;
                    if (!Directory.Exists(DatevDir))
                    {
                        Directory.CreateDirectory(DatevDir);

                    }
                    else
                    {
                        string DatevDayDir = @Application.StartupPath + "\\DATEV\\" + DateTime.Now.ToString("yyyy-MM-dd");

                        if (DatevDirCheck(DatevDayDir) == true)
                        {
                            string ZipFileName = frmDataExport.TableExportDATEVDSFinK(DatevDayDir, berNo.ToString(), Program.kasano, 0,0);
                            if (ZipFileName != "")
                            {
                                string ZStartSQL = "", Date_from = "", Date_to = "";
                                ZStartSQL = "SELECT tarih FROM satisana WHERE (localbonid=" + dtZinfo.Rows[0].ItemArray[0] + " OR localbonid=" + dtZinfo.Rows[0].ItemArray[1] + ") AND kasano=" + Program.kasano + " AND znr=" + berNo + " ORDER by localbonid";
                                MySqlDataAdapter myDaZStartBon = new MySqlDataAdapter(ZStartSQL, myConn);
                                DataTable myDtZStartBon = new DataTable();
                                myDtZStartBon.Rows.Clear();
                                myDaZStartBon.Fill(myDtZStartBon);
                                Date_from = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows[0].ItemArray[0])).ToString("yyyy-MM-dd");
                                Date_to = tarih.KisatarihDateTime(Convert.ToInt32(myDtZStartBon.Rows.Count == 1 ? myDtZStartBon.Rows[0].ItemArray[0] : myDtZStartBon.Rows[1].ItemArray[0])).ToString("yyyy-MM-dd");

                                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                                string jsonText = "{\r\n  \"document_type\": \"TSE\",\r\n  \"note\": \"ISS POS Kassensystem-Datev Integration\",\r\n  \"extensions\": {\r\n    \"tse_properties\": {\r\n      \"serial_number\": \"" + Program.IsletmeAyarlar["strase"] +
                                    "\",\r\n        \"postal_code\": \"" + Program.IsletmeAyarlar["plz"] + "\",\r\n        \"city\": \"" + Program.IsletmeAyarlar["stadt"] + "\",\r\n        \"country_code\": \"DE\"\r\n      },\r\n      \"serial_number\": \"" + Program.ClientID +
                                    "\",\r\n      \"manufacturer\": \"Windsoft Tech GmbH\",\r\n      \"model_type\": \"ISS POS \",\r\n      \"name\": \"" + Program.kasaAd + "\",\r\n      \"description\": \"Windsoft Tech GmbH-ISS POS Kassensysteme \"\r\n    },\r\n    \"client_application\": {\r\n      \"client_application_name\": \"ISS POS Kassensystem\",\r\n      \"client_application_vendor\": \"Windsoft Tech GmbH\",\r\n      \"client_application_version\": \"" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion +
                                    "\"\r\n    },\r\n    \"date_from\": \"" + Date_from + "\",\r\n    \"date_to\": \"" + Date_to + "\"\r\n  }\r\n}";

                                string RequestID = "", tenandID = "", accessToken = "";
                                try
                                {
                                    Directory.Delete(DatevDayDir);
                                }
                                catch (Exception dd)
                                {

                                }
                                //jsonInfo = JsonConvert.SerializeObject(cash_register);
                                RequestID = datevMain.RandomString(20);
                                if (!Directory.Exists(DatevDayDir))
                                {
                                    Directory.CreateDirectory(DatevDayDir);
                                    //File.Copy("initialize-folder.init.conf", DatevDir + @"\\initialize-folder.init.conf");
                                    //File.WriteAllText(@DatevDir + "\\metadata.json", JsonConvert.SerializeObject(cash_register));
                                    File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);

                                }
                                else
                                {

                                    if (!File.Exists(DatevDayDir + @"\\metadata.json"))
                                    {

                                        File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                    }
                                    else
                                    {
                                        File.Delete(DatevDayDir + @"\\metadata.json");
                                        File.WriteAllText(@DatevDayDir + "\\metadata.json", jsonText);
                                    }
                                }
                                // TenandID und AcceToken
                                string idToken = "";
                                string DatevInfo = "SELECT  `tenandid`,  `accesstoken`, idtoken FROM `datev_info`";
                                MySqlDataAdapter myDaDatevInfo = new MySqlDataAdapter(DatevInfo, myConn);
                                DataTable dtINfo = new DataTable();
                                myDaDatevInfo.Fill(dtINfo);
                                if (dtINfo.Rows.Count > 0)
                                {
                                    bool result = false;
                                    string DatevError = "";
                                    tenandID = dtINfo.Rows[0].ItemArray[0].ToString();
                                    accessToken = dtINfo.Rows[0].ItemArray[1].ToString();

                                    idToken = dtINfo.Rows[0].ItemArray[2].ToString();
                                    FileInfo fi = new FileInfo(@DatevDayDir + "\\" + ZipFileName);
                                    double FileSize = 0;
                                    FileSize = fi.Length / (1024 * 1024);
                                    if (FileSize > 200)
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "Die Daten können nicht übertragen werden. Die maximale Dateigröße von 200 MB wurde überschritten." +
                                            " Weitere Informationen entnehmen Sie bitte dem Logfile in Ihrem Kassensystem.";
                                        frmerror.ShowDialog();
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@json", jsonText);
                                        cmd1.Parameters.AddWithValue("@accessToken", accessToken);
                                        cmd1.Parameters.AddWithValue("@refrToken", "");
                                        cmd1.Parameters.AddWithValue("@idToken", idToken);
                                        cmd1.Parameters.AddWithValue("@reqID", RequestID);
                                        cmd1.Parameters.AddWithValue("@tenandID", tenandID);
                                        cmd1.Parameters.AddWithValue("@filename", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                        cmd1.Parameters.AddWithValue("@error", "FILEIMPORT_BE_ARCHIVE_000026");
                                        cmd1.Parameters.AddWithValue("@errormessage", "Dateigroße ist größer als 200MB");
                                        cmd1.Parameters.AddWithValue("@level", "File-Übertrag");
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_log`( `datum`, `accesstoken`, `refreshtoken`, `idtoken`,  `dateiname`,  requestID, tenandID, error,errormeldung,level,kassenr ) VALUES" +
                                            "(@datum,@accessToken,@refrToken,@idToken,@filename,@reqID,@tenandID,@error,@errormessage,@level,@kassenr)";
                                        cmd1.ExecuteNonQuery();
                                    }

                                    Antwort ant = new Antwort();
                                    ant = datevMain.fileUpload2(accessToken, RequestID, tenandID, @DatevDayDir + "\\metadata.json", @DatevDayDir + "\\" + ZipFileName, ZipFileName);
                                    if (ant.ErrorMessage != "")
                                    {
                                        // z bericht Datev Field Update
                                        // Datev FileLog
                                        /*INSERT INTO `datev_dfu`(`id`, `znr`, `createdate`, `zdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`) VALUES 
                                         * ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12])*/
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@znr", berNo);
                                        cmd1.Parameters.AddWithValue("@createdate", tarih.unixdate(DateTime.Now));
                                        //cmd1.Parameters.AddWithValue("@zdate", refreshToken);
                                        cmd1.Parameters.AddWithValue("@zfile", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@daydir", DatevDayDir);
                                        cmd1.Parameters.AddWithValue("@jsonfile", DatevDayDir + "\\metadata.json");
                                        cmd1.Parameters.AddWithValue("@result", 0);
                                        cmd1.Parameters.AddWithValue("@fehlercode", ant.ErrorNo);
                                        cmd1.Parameters.AddWithValue("@fehlermessage", ant.ErrorMessage);
                                        cmd1.Parameters.AddWithValue("@requestID", RequestID);
                                        cmd1.Parameters.AddWithValue("@accesstoken", accessToken);
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);

                                        if (myConn.State == ConnectionState.Closed)
                                            myConn.Open();
                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_dfu`( `znr`, `createdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`,kassenr) VALUES " +
                                            "(@znr,@createdate,@zfile,@daydir,@jsonfile,@result,@fehlercode,@fehlermessage,@requestID,@accesstoken,@kassenr)";
                                        if (cmd1.ExecuteNonQuery() > 0)
                                        {
                                            string UpdateZdatev = "";
                                            UpdateZdatev = "UPDATE zbericht SET datev='ERROR' WHERE Zberichtno=" + berNo;
                                            MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn);
                                            cmdDatevUpdate.ExecuteNonQuery();
                                        }

                                        result = false;
                                        DatevError = ant.ErrorMessage;
                                        if (ant.ErrorNo == "Unauthorized")
                                        {
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = "Die Authentifizierung ist fehlgeschlagen. Bitte verbinden Sie Ihre Kasse erneut mit dem Kassenarchiv online.";
                                            frmerror.ShowDialog();
                                            btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.DatevVerbinden;
                                            Program.DatevConnection = false;
                                            btnDatevVerbinden.Click += new System.EventHandler(this.kryptonButton1_Click_1);

                                            return;
                                        }
                                        else if (ant.ErrorNo == "423")
                                        {
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = "Die Kassendaten konnten nicht archiviert werden, da der Kassenordner im Kassenarchiv online deaktiviert ist." +
                                                                       "Um den Kassenordner zu aktivieren, melden Sie sich unter https://www.meinfiskal.de an.";
                                            frmerror.ShowDialog();
                                            /*btnDatevVerbinden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Datev_verbunden;
                                            Program.DatevConnection = true;
                                            btnDatevVerbinden.Click -= new System.EventHandler(this.kryptonButton1_Click_1);*/

                                            return;
                                        }



                                    }
                                    else
                                    {
                                        //Verbindung OK!
                                        result = true;
                                        MySqlCommand cmd1 = new MySqlCommand();
                                        cmd1.Parameters.AddWithValue("@znr", berNo);
                                        cmd1.Parameters.AddWithValue("@createdate", tarih.unixdate(DateTime.Now));
                                        //cmd1.Parameters.AddWithValue("@zdate", refreshToken);
                                        cmd1.Parameters.AddWithValue("@zfile", ZipFileName);
                                        cmd1.Parameters.AddWithValue("@daydir", DatevDayDir);
                                        cmd1.Parameters.AddWithValue("@jsonfile", DatevDayDir + "\\metadata.json");
                                        cmd1.Parameters.AddWithValue("@result", 1);
                                        cmd1.Parameters.AddWithValue("@fehlercode", ant.ErrorNo);
                                        cmd1.Parameters.AddWithValue("@fehlermessage", ant.ErrorMessage);
                                        cmd1.Parameters.AddWithValue("@requestID", RequestID);
                                        cmd1.Parameters.AddWithValue("@accesstoken", accessToken);
                                        cmd1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                        if (myConn.State == ConnectionState.Closed)
                                            myConn.Open();
                                        cmd1.Connection = myConn;
                                        cmd1.CommandText = "INSERT INTO `datev_dfu`( `znr`, `createdate`, `zfile`, `daydir`, `jsonfile`, `result`, `fehlercode`, `fehlermessage`, `requestID`, `accesstoken`,kassenr) VALUES " +
                                            "(@znr,@createdate,@zfile,@daydir,@jsonfile,@result,@fehlercode,@fehlermessage,@requestID,@accesstoken,@kassenr)";
                                        if (cmd1.ExecuteNonQuery() > 0)
                                        {
                                            string UpdateZdatev = "";
                                            UpdateZdatev = "UPDATE zbericht SET datev='OK' WHERE Zberichtno=" + berNo;
                                            MySqlCommand cmdDatevUpdate = new MySqlCommand(UpdateZdatev, myConn);
                                            cmdDatevUpdate.ExecuteNonQuery();
                                        }
                                    }
                                    F_ZAuswahl zauswahl = new F_ZAuswahl();
                                    zauswahl.datevResult = result;
                                    zauswahl.DatevError = DatevError;
                                    zauswahl.ShowDialog();

                                    if (zauswahl.islem == 2)
                                    {
                                        FisBarkodlu fisclass = new FisBarkodlu();
                                        fisclass.XZDruck(berNo);
                                    }
                                }
                            }
                            else
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "Fehler bei der DSFinV-K Export!";
                                frmerror.ShowDialog();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    //Zbericht hatali
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Bitte geben Sie eine gültige Z-Nr!";
                    frmerror.ShowDialog();
                    return;
                }
            }
            catch (Exception dd)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = dd.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }


    }
}
