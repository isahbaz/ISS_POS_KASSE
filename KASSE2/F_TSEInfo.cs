using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iss_tse_v2;
using System.IO;
using iss_tse_Meldepflicht;
using System.Reflection;
using Microsoft.VisualBasic.Logging;
using System.Management;
using iss_Datev;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using tar;

namespace IS_KASSE
{
    public partial class F_TSEInfo : Form
    {
        tar.Tarih tarih = new tar.Tarih();
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        WormReturnClass wormReturn = new WormReturnClass();
        TSELocalInfo locInfo = new TSELocalInfo();
        //TSEClasses tseInfo = new TSEClasses();
        TSEClasses tseHerstInfo = new TSEClasses();
        WormStore myWorm;
        string ilkDBConnectionString = "", enEskiDBName="";
        int enEskiYil = 0;
        int bulunanYil, arsivEneskiYil = 0;
        MeldenDaten tSEMeldeDaten;
        public F_TSEInfo()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void F_TSEInfo_Load(object sender, EventArgs e)
        {
             lb1.Visible = false;
            btnTSEAnmeldeDruck.Visible = false;
            txttseclientid.Text = Program.ClientID;
            txtTsedrivename.Text = Program.TSEDrive;
            txttsepin.Text = Program.TSEPin;
            txttsepuk.Text = Program.TSEPuk;
            txttsetimeadminpuk.Text = Program.TSETimeAdmin;
            
            locInfo.clienID = Program.ClientID;
            locInfo.drive = Program.TSEDrive;
            locInfo.pin = Program.TSEPin;
            locInfo.puk = Program.TSEPuk;
            locInfo.timeadmin = Program.TSETimeAdmin;
            locInfo.publicKey = Program.PublicKey;
            TSEInfoLoad();
        }

        private void TSEInfoLoad()
        {
            myConn = baglanti.myconn();     
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            try
            {
                


                wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                myWorm = Program.TSEdll.myWorm;

                string zertifikat = "";
                if (myWorm != null)
                {
                    int menge = 0, leng=0;
                    try
                    {
                        //str.Split(new[] { "is Marco and" }, StringSplitOptions.None);
                        string[] sertifika = System.Text.Encoding.UTF8.GetString(myWorm.getLogMessageCertificate()).Split(new[] { "-----BEGIN CERTIFICATE-----" },StringSplitOptions.None);
                        for (int i = 0; i < sertifika.Length; i++)
                        {
                            zertifikat += sertifika[i].Replace("-----END CERTIFICATE-----","");   // System.Convert.ToBase64String(myWorm.getLogMessageCertificate());
                        }
                        leng = zertifikat.Length;
                        menge = zertifikat.Length % 1000;
                        //MessageBox.Show(zertifikat);
                        //MessageBox.Show(zertifikat.Length.ToString());
                            txtZertifikat1.Text = System.Convert.ToBase64String(myWorm.getLogMessageCertificate()).Substring(0,1000);
                            txtZertifikat2.Text = System.Convert.ToBase64String(myWorm.getLogMessageCertificate()).Substring(1000, 1000);
                            txttseserial.Text =BitConverter.ToString(myWorm.info().tseSerialNumber()).Replace("-", "");//BitConverter.ToString(myWorm.Info().TseSerialNumber()).Replace("-", "");
                        txtPublicKey.Text= System.Convert.ToBase64String(Program.TSEdll.myWorm.info().tsePublicKey());

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(WormErrors.Wormerror(Convert.ToInt16(ex.Message)));

                        return;
                    }
                   
                        if (myConn.State == ConnectionState.Closed)
                        {
                            myConn.Open();
                        }
                        string SelectSQL = "";
                        long lastid = 0;
                        SelectSQL = "SELECT * FROM tse  WHERE clientid= @serial";
                        MySqlCommand myDatse = new MySqlCommand(SelectSQL);
                        myDatse.Parameters.AddWithValue("@serial", Program.ClientID);
                        
                        myDatse.Connection = myConn;
                        MySqlDataReader dr = myDatse.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();

                            btnTSESpeichern.Visible = true;
                            //INSERT INTO `tse`(`id`, `tse_id`, `tse_serial`, `sig_algo`, `zeit_format`, `pd_encoding`, `public_key`, `zertifikat_i`, `zertifikat_ii`, `datum`, `tsehersteller`) VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11])
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();
                            }
                            string InsertSQL = "";

                            MySqlCommand cmdIns = new MySqlCommand();
                            cmdIns.Parameters.AddWithValue("@serial", txttseserial.Text);
                            cmdIns.Parameters.AddWithValue("@clientid", Program.ClientID);
                            cmdIns.Parameters.AddWithValue("@sigalgo", txtxTseSigAlgo.Text);
                            cmdIns.Parameters.AddWithValue("@zeitformat", txtTSEZeitformat.Text);
                            cmdIns.Parameters.AddWithValue("@pdencoding", txtPDEncoding.Text);
                            cmdIns.Parameters.AddWithValue("@kassenr", Program.kasano);
                            if (leng<1000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0,1000));
                                cmdIns.Parameters.AddWithValue("@zer2", "");
                                cmdIns.Parameters.AddWithValue("@zer3","");
                                cmdIns.Parameters.AddWithValue("@zer4", "");
                                cmdIns.Parameters.AddWithValue("@zer5", "");
                                cmdIns.Parameters.AddWithValue("@zer6", "");
                            }
                            else if(leng<2000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, leng-1000));
                                cmdIns.Parameters.AddWithValue("@zer3", "");
                                cmdIns.Parameters.AddWithValue("@zer4", "");
                                cmdIns.Parameters.AddWithValue("@zer5", "");
                                cmdIns.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 3000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, leng-2000));
                                cmdIns.Parameters.AddWithValue("@zer4", "");
                                cmdIns.Parameters.AddWithValue("@zer5", "");
                                cmdIns.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 4000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, leng - 3000));
                                cmdIns.Parameters.AddWithValue("@zer5", "");
                                cmdIns.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 5000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, leng-4000));
                                cmdIns.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 6000)
                            {
                                cmdIns.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdIns.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, 1000));
                                cmdIns.Parameters.AddWithValue("@zer6", zertifikat.Substring(5000, leng - 5000));
                            }
                           // cmdIns.Parameters.AddWithValue("@zer1", txtZertifikat1.Text);
                           // cmdIns.Parameters.AddWithValue("@zer2", txtZertifikat2.Text);
                            cmdIns.Parameters.AddWithValue("@pubKey", Program.PublicKey);
                            InsertSQL = "INSERT INTO `tse`(  `tse_serial`, `sig_algo`, `zeit_format`, `pd_encoding`,  `zertifikat_i`, `zertifikat_ii`, `datum`, public_key, `zertifikat_3`, `zertifikat_4`, `zertifikat_5`, `zertifikat_6`, clientid, kassenr) VALUES (" +
                                    "@serial,@sigalgo, @zeitformat,@pdencoding,@zer1, @zer2," + tarih.unixdate(DateTime.Now) + ", @pubKey, @zer3, @zer4,@zer5,@zer6,@clientid,@kassenr)";
                            cmdIns.CommandText = InsertSQL;
                            cmdIns.Connection = myConn;
                            if (cmdIns.ExecuteNonQuery() > 0)
                            {
                                lastid = cmdIns.LastInsertedId;
                                txtTseId.Text = lastid.ToString();
                                string UpdateIdSql = "UPDATE tse SET tse_id=" + lastid + " WHERE id=" + lastid;
                                Program.TSEID = (int)lastid;
                                MySqlCommand cmdUpdateID = new MySqlCommand(UpdateIdSql, myConn);
                                cmdUpdateID.ExecuteNonQuery();
                                btnTSESpeichern.Visible = false;
                            }

                        }
                        else
                        {
                            dr.Read();
                            txtTseId.Text = dr.GetInt16(1).ToString();
                            Program.TSEID = dr.GetInt16(1);
                            MySqlCommand cmdUpdateTSE = new MySqlCommand();
                            cmdUpdateTSE.Parameters.AddWithValue("@serial", txttseserial.Text);
                            cmdUpdateTSE.Parameters.AddWithValue("@clientid", Program.ClientID);
                            cmdUpdateTSE.Parameters.AddWithValue("@sigalgo", txtxTseSigAlgo.Text);
                            cmdUpdateTSE.Parameters.AddWithValue("@zeitformat", txtTSEZeitformat.Text);
                            cmdUpdateTSE.Parameters.AddWithValue("@pdencoding", txtPDEncoding.Text);
                            cmdUpdateTSE.Parameters.AddWithValue("@kassenr", Program.kasano);
                            if (leng < 1000)
                            {
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 2000)
                            {
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000,leng-1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 3000)
                            {
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, leng-2000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 4000)
                            {
                                string t1 = "", t2, t3, t4;
                                t1 = zertifikat.Substring(0, 1000);
                                t2 = zertifikat.Substring(1000, 1000);
                                t3 = zertifikat.Substring(2000, 1000);
                                // t4= zertifikat.Substring(3000, 1000);
                                //MessageBox.Show(t1.Length + " " + t2.Length + " " + t3.Length + " ");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, leng-3000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", "");
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 5000)
                            {
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, leng - 4000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", "");
                            }
                            else if (leng < 6000)
                            {
                                cmdUpdateTSE.Parameters.AddWithValue("@zer1", zertifikat.Substring(0, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer2", zertifikat.Substring(1000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer3", zertifikat.Substring(2000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer4", zertifikat.Substring(3000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer5", zertifikat.Substring(4000, 1000));
                                cmdUpdateTSE.Parameters.AddWithValue("@zer6", zertifikat.Substring(5000, leng - 5000));
                            }
                            dr.Close();
                            if (myConn.State == ConnectionState.Closed)
                                myConn.Open();
                            String UpdateTSeInfo = "";
                            UpdateTSeInfo = "UPDATE tse SET `pin`='" + locInfo.pin + "',`puk`='" + locInfo.puk + "',`timeadmin`='" + locInfo.timeadmin + "', kassenr=" + Program.kasano +
                                ", public_key='"+txtPublicKey.Text+"', tse_serial= '" + txttseserial.Text + "', zertifikat_i=@zer1, zertifikat_ii=@zer2,`zertifikat_3`=@zer3, `zertifikat_4`=@zer4, `zertifikat_5`=@zer5, `zertifikat_6`=@zer6 WHERE `clientid`='" + locInfo.clienID+"'" ;
                            cmdUpdateTSE.CommandText = UpdateTSeInfo;
                            cmdUpdateTSE.Connection = myConn;
                            cmdUpdateTSE.ExecuteNonQuery();
                        }
                   
                    
                    tseHerstInfo.tseID =Convert.ToInt16(txtTseId.Text);
                    tseHerstInfo.tsePDEncoding = txtPDEncoding.Text;
                    tseHerstInfo.tseSerial = txttseserial.Text;
                    tseHerstInfo.tseSigAlgo = txtxTseSigAlgo.Text;
                    tseHerstInfo.tseZeitFormat = txtTSEZeitformat.Text;
                    tseHerstInfo.tseZertifikat1 = txtZertifikat1.Text;
                    tseHerstInfo.tseZertifikat2 = txtZertifikat2.Text;

                    //§ 146a Abs. 4 AO
                   
                }
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WormStore myWorm = Program.TSEdll.myWorm;
            if (myWorm != null)
            {
                try
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.FileName = "tse_cert.pem";
                    saveFile.Filter = "PEM file (*.pem)|*.pem|All files (*.*)|*.*";
                    if (saveFile.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                   using( FileStream stream = (System.IO.FileStream)saveFile.OpenFile())
                    {
                        byte[] bytes = myWorm.getLogMessageCertificate();
                        stream.Write(bytes, 0, bytes.Length);

                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                txttseserial.Text = BitConverter.ToString(myWorm.info().tseSerialNumber()).Replace("-", "");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FisBarkodlu fisTSe = new FisBarkodlu();
            fisTSe.TSEInfoDruck(tseHerstInfo, locInfo);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WormStore myWorm = Program.TSEdll.myWorm;
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = "tse_cert.pem";
                saveFile.Filter = "PEM file (*.pem)|*.pem|All files (*.*)|*.*";
                if (saveFile.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                using (FileStream stream = (System.IO.FileStream)saveFile.OpenFile())
                {
                    byte[] bytes = myWorm.getLogMessageCertificate();
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                //tbResult.Text = ex.Message;
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.TSEID != 0)
                {
                    lb1.Items.Clear();
                    Logger Log = new Logger("LOG\\TSEMELDEPFLICTH_LOG");
                    if (Program.TSEID != 0)
                        FindFirstDB();
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                   
                    tSEMeldeDaten.anschaffungDesEas = FindFirstReceipDate(Program.kasano, Log);
                    tSEMeldeDaten.anzahlDerEas = FindCountTSE(Log);
                    tSEMeldeDaten.artBauFormDerTSE = FindTseHardWareTyp(Program.TSEDrive, Log);
                    tSEMeldeDaten.artDesEas = "Computergestützte/PC-Kassensysteme";
                    tSEMeldeDaten.ausserbetribnahmeBetriebstaette = "";
                    tSEMeldeDaten.ausserBetriebnahmeDesEas = "";
                    tSEMeldeDaten.bemerkungZumEas = "";
                    tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas = "";
                    tSEMeldeDaten.bezeichnungBetriebStaette = Program.IsletmeAyarlar["isletme"]; //Datei "Stamm_Orte“ (location.csv) -Feld "LOC_NAME"
                    tSEMeldeDaten.bemerkungenBetriebStaette = "";
                    tSEMeldeDaten.BSIZertifierungsID = "0362-2019";//BSI - K - TR - 0362 - 2019 //BSI - K - TR - NNNN - YYYY
                    tSEMeldeDaten.herstellerDesEas = "Windsoft Tech GmbH"; //BSI - K - TR - NNNN - YYYY
                    tSEMeldeDaten.inbetriebnahmeDerTSE = FindFirstTseTX(Program.kasano, Log);
                    tSEMeldeDaten.inbetriebnahmeDesEas = FindFirstReceipDate(Program.kasano, Log);
                    tSEMeldeDaten.modelDesEas = "ISS KASSE";///Stamm_Modell“ (cashregister.csv) -Feld "KASSE_MODELL"
                    tSEMeldeDaten.seriennummerDerTSE = BitConverter.ToString(myWorm.info().tseSerialNumber()).Replace("-", "");
                    tSEMeldeDaten.seriennummerDesEas = Program.ClientID;
                    tSEMeldeDaten.softwareDesEas = "ISS POS Kassensysteme"; //KASSE_SW_BRAND
                    tSEMeldeDaten.softwareVersionDesEas = fvi.FileVersion;//2.1.4.16
                                                                          //UPDATE `tse` SET `id`=[value-1],`tse_id`=[value-2],`tse_serial`=[value-3],`sig_algo`=[value-4],`zeit_format`=[value-5],`pd_encoding`=[value-6],`public_key`=[value-7],`zertifikat_i`=[value-8],`zertifikat_ii`=[value-9],`datum`=[value-10],
                                                                          //`tsehersteller`=[value-11],`pin`=[value-12],`puk`=[value-13],`clientid`=[value-14],`timeadmin`=[value-15],`kassenr`=[value-16],`zertifikat_3`=[value-17],`zertifikat_4`=[value-18],`zertifikat_5`=[value-19],`zertifikat_6`=[value-20],`zertifikat_7`=[value-21],
                                                                          //`zertifikat_8`=[value-22],`zertifikat_9`=[value-23],`BSIZertifierungsID`=[value-24],`inbetriebnahmeDerTSE`=[value-25],`artBauFormDerTSE`=[value-26],`bezeichnungBetriebStaette`=[value-27],`ausserbetribnahmeBetriebstaette`=[value-28],`inbetriebnahmeDesEas`=[value-29],
                                                                          //`ausserBetriebnahmeDesEas`=[value-30],`grundDerAusserbetriebnahmeDesEas`=[value-31],`anmeldedatenOK`=[value-32],`gesendet`=[value-33] WHERE 1
                    MySqlCommand cmdInsDaten = new MySqlCommand();
                    cmdInsDaten.Parameters.AddWithValue("@BSIZertifierungsID", tSEMeldeDaten.BSIZertifierungsID);
                    cmdInsDaten.Parameters.AddWithValue("@artBauFormDerTSE", tSEMeldeDaten.artBauFormDerTSE);
                    cmdInsDaten.Parameters.AddWithValue("@inbetriebnahmeDerTSE", tSEMeldeDaten.inbetriebnahmeDerTSE);
                    cmdInsDaten.Parameters.AddWithValue("@bezeichnungBetriebStaette", tSEMeldeDaten.bezeichnungBetriebStaette);
                    cmdInsDaten.Parameters.AddWithValue("@ausserbetribnahmeBetriebstaette", tSEMeldeDaten.ausserbetribnahmeBetriebstaette);
                    cmdInsDaten.Parameters.AddWithValue("@inbetriebnahmeDesEas", tSEMeldeDaten.inbetriebnahmeDesEas);
                    cmdInsDaten.Parameters.AddWithValue("@ausserBetriebnahmeDesEas", tSEMeldeDaten.ausserBetriebnahmeDesEas);
                    cmdInsDaten.Parameters.AddWithValue("@grundDerAusserbetriebnahmeDesEas", tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas);
                    cmdInsDaten.Parameters.AddWithValue("@anschaffungDesEas", tSEMeldeDaten.anschaffungDesEas);
                    cmdInsDaten.Parameters.AddWithValue("@softwareVersionDesEas", tSEMeldeDaten.softwareVersionDesEas);
                    cmdInsDaten.Parameters.AddWithValue("@anmeldedatenOK", 1);
                    cmdInsDaten.Parameters.AddWithValue("@gesendet", 0);
                    cmdInsDaten.Parameters.AddWithValue("@aktiv", 1);
                    cmdInsDaten.CommandText = "UPDATE `tse` SET softwareVersionDesEas=@softwareVersionDesEas, anschaffungDesEas=@anschaffungDesEas, `BSIZertifierungsID`=@BSIZertifierungsID,`inbetriebnahmeDerTSE`=@inbetriebnahmeDerTSE,`artBauFormDerTSE`=@artBauFormDerTSE,`bezeichnungBetriebStaette`=@bezeichnungBetriebStaette,`ausserbetribnahmeBetriebstaette`=@ausserbetribnahmeBetriebstaette,`inbetriebnahmeDesEas`=@inbetriebnahmeDesEas," +
                    "`ausserBetriebnahmeDesEas`=@ausserBetriebnahmeDesEas,`grundDerAusserbetriebnahmeDesEas`=@grundDerAusserbetriebnahmeDesEas,`anmeldedatenOK`=@anmeldedatenOK,`gesendet`=@gesendet, aktiv=@aktiv WHERE tse_id=" + Program.TSEID;
                    cmdInsDaten.Connection = myConn;
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    if (cmdInsDaten.ExecuteNonQuery() > 0)
                    {
                        lb1.Items.Add("TSE Anmeldedaten:");
                        lb1.Items.Add("1.Art des eAS:" + tSEMeldeDaten.artDesEas.ToString());
                        lb1.Items.Add("2.Software des eAS:" + tSEMeldeDaten.softwareDesEas.ToString());
                        lb1.Items.Add("3.Software-Version des eAS:" + tSEMeldeDaten.softwareVersionDesEas.ToString());
                        lb1.Items.Add("4.Seriennumer des eAS/Software App:" + tSEMeldeDaten.seriennummerDesEas.ToString());
                        lb1.Items.Add("5.Hersteller des eAS:" + tSEMeldeDaten.herstellerDesEas.ToString());
                        lb1.Items.Add("6.Model des eAS:" + tSEMeldeDaten.modelDesEas.ToString());
                        lb1.Items.Add("7.Anschaffungsdatum des eAS:" + tSEMeldeDaten.anschaffungDesEas.ToString());
                        lb1.Items.Add("8.Inbetriebnahme des eAS:" + tSEMeldeDaten.inbetriebnahmeDesEas.ToString());
                        lb1.Items.Add("9. Ausserbetriebnahme des eAS:" + tSEMeldeDaten.ausserBetriebnahmeDesEas.ToString());
                        lb1.Items.Add("10.Grund der Ausserbetrieb. des eAS:" + tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas.ToString());
                        lb1.Items.Add("11.Bemerkungen zum eAS:" + tSEMeldeDaten.bemerkungZumEas.ToString());
                        lb1.Items.Add("12.Seriennumer der TSE:" + tSEMeldeDaten.seriennummerDerTSE.ToString());
                        lb1.Items.Add("13.BSI Zertifizierungs-ID:" + tSEMeldeDaten.BSIZertifierungsID.ToString());
                        lb1.Items.Add("14.Inbetriebnahme/Aktivierung der TSE:" + tSEMeldeDaten.inbetriebnahmeDerTSE.ToString());
                        lb1.Items.Add("15.Art/Bauform der TSE:" + tSEMeldeDaten.artBauFormDerTSE.ToString());
                        //lb1.Items.Add("Inbetriebnahme des eAS:" + tSEMeldeDaten.artDesEas.ToString());
                        lb1.Visible = true;
                        btnTSEAnmeldeDruck.Visible = true;

                    }
                }




            }
            catch (Exception ex)
            {
                F_GenericError fGenericError = new F_GenericError();
                fGenericError.lblMesaj.Text = ex.Message;
                int num = (int)fGenericError.ShowDialog();
            }
        }
        private void FindFirstDB()
        { //TSE Icin ilk dbyi bulalim
            try
            {
                
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string showDBSQL = "USE INFORMATION_SCHEMA;\r\nSELECT `SCHEMA_NAME` from `SCHEMATA` WHERE `SCHEMA_NAME` LIKE 'is_kasa%'; ";
                    MySqlDataAdapter myShowDB = new MySqlDataAdapter(showDBSQL,myConn);
                    DataTable dtShowDB = new DataTable();
                    myShowDB.Fill(dtShowDB);
                    if(dtShowDB.Rows.Count > 0)
                    {
                       
                        for (int i = 0; i < dtShowDB.Rows.Count; i++)
                        {
                            string[] DbInfo=dtShowDB.Rows[i].ItemArray[0].ToString().Split('_');
                            if(DbInfo.Length > 2)
                            {
                                if(int.TryParse(DbInfo[2], out bulunanYil))
                                {
                                    if (bulunanYil > 2010 && bulunanYil<=2025)
                                    {
                                        if (enEskiYil == 0)
                                        {
                                            enEskiYil = bulunanYil;
                                            enEskiDBName = dtShowDB.Rows[i].ItemArray[0].ToString();
                                            arsivEneskiYil = enEskiYil;
                                        }
                                        else
                                        {
                                            if (bulunanYil < enEskiYil)
                                            {
                                                enEskiYil = bulunanYil;
                                                enEskiDBName = dtShowDB.Rows[i].ItemArray[0].ToString();
                                                arsivEneskiYil = enEskiYil;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        if(enEskiYil == 0)//gecmiste yil yok ve bu yil kurulum yapilmis veya data yillara bölunmemis, yillara ayrilmis db yok örn:is_kasa_2021 gibi birsey yok 
                        {
                            //mevcut connectiontring yeterli

                        }
                        else
                        {
                            //yeni bir connectionstring olustur


                        }
                    }
                
            }
            catch (Exception e)
            {
            }
        }
        private string FindFirstTseTX(int kasano, Logger log)
        {
            enEskiYil = arsivEneskiYil;
            if (enEskiYil == 0)
            {

                log.Log("FIND FIRST TSE TX DATE->FirstYear=0, Aktuelle Year is using");
               
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    try
                    {
                        string firstBillDate = "SELECT tarih FROM satisana WHERE kasano=" + kasano+" AND trsansactionnr>= 1 ORDER by tarih";
                        MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, myConn);
                        DataTable dtFirstBill = new DataTable();
                        myFirstBill.Fill(dtFirstBill);
                        if (dtFirstBill.Rows.Count > 0)
                        {
                            return Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])==0?"0":tarih.tarihDateTime(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])).ToShortDateString();
                        }
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                
            }
            else
            {
            A:
                while (enEskiYil <= DateTime.Now.Year)
                {
                    log.Log("FIND FIRST TSE TX DATE->FirstYear=" + enEskiYil);
                    try
                    {
                        MySqlConnection mySS = new MySqlConnection();
                        if (enEskiYil == DateTime.Now.Year)
                        {
                            ilkDBConnectionString = "SERVER=" + Program.ServerIp + ";" +
                                                    "DATABASE=is_kasa" + ";" +
                                                    "UID=" + Program.DBUsername + ";" +
                                                    "PASSWORD=" + Program.DBPass + ";" +
                                                    "Connection Timeout=1000";
                            mySS.ConnectionString = ilkDBConnectionString;
                        }
                        else
                        {

                            ilkDBConnectionString = "SERVER=" + Program.ServerIp + ";" +
                                                     "DATABASE=is_kasa_" + enEskiYil + ";" +
                                                     "UID=" + Program.DBUsername + ";" +
                                                     "PASSWORD=" + Program.DBPass + ";" +
                                                     "Connection Timeout=1000";
                            mySS.ConnectionString = ilkDBConnectionString;
                        }
                        mySS.Open();
                        if (mySS.State == ConnectionState.Open)
                        {
                            try
                            {
                                string firstBillDate = "SELECT tarih FROM satisana WHERE kasano=" + kasano+ " AND trsansactionnr>=1 ORDER by tarih ";
                                MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, mySS);
                                DataTable dtFirstBill = new DataTable();
                                myFirstBill.Fill(dtFirstBill);
                                if (dtFirstBill.Rows.Count == 0)
                                {  
                                    enEskiYil++;
                                    goto A;                                                           ;
                                                                       
                                }
                                else
                                {
                                    log.Log("First TSE TX Date=" + tarih.tarih(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])));
                                    enEskiYil = bulunanYil;
                                    return Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0]) == 0 ? "0" : tarih.tarihDateTime(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])).ToShortDateString();
                                }
                            }
                            catch (Exception ff)
                            {
                                enEskiYil++;
                                continue;
                            }

                        }
                        else
                        {
                            log.Log("First TSE TX Date  Error. MySS Open Fehler! ConnString=" + ilkDBConnectionString);
                            enEskiYil = bulunanYil;
                            return "";
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Log("First TSE TX Date Error. Error Message=" + ex.Message);
                        enEskiYil++;
                        continue;
                    }
                }

            }
            log.Log("First TSE TX Date Error. Unbekannt");
            enEskiYil = bulunanYil;
            return "";

        }

        private string FindFirstReceipDate(int kasano, Logger log)
        {
            enEskiYil = arsivEneskiYil;
            if (enEskiYil == 0)
            {

                log.Log("FIND FIRST RECEIPT DATE->FirstYear=0, Aktuelle Year is using");
                
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    try
                    {
                        string firstBillDate = "SELECT tarih FROM satisana  WHERE kasano=" + kasano;
                        MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, myConn);
                        DataTable dtFirstBill = new DataTable();
                        myFirstBill.Fill(dtFirstBill);
                        if (dtFirstBill.Rows.Count > 0)
                        {
                            return tarih.tarihDateTime(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])).ToShortDateString();
                        }
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                
            }
            else
            {
            A:
                while (enEskiYil <= DateTime.Now.Year)
                {

                    log.Log("FIND FIRST RECEIPT DATE->FirstYear=" + enEskiYil);
                    try
                    {
                        MySqlConnection mySS = new MySqlConnection();
                        if (enEskiYil == DateTime.Now.Year)
                        {
                            ilkDBConnectionString = "SERVER=" + Program.ServerIp + ";" +
                                                    "DATABASE=is_kasa" + ";" +
                                                    "UID=" + Program.DBUsername + ";" +
                                                    "PASSWORD=" + Program.DBPass + ";" +
                                                    "Connection Timeout=1000";
                            mySS.ConnectionString = ilkDBConnectionString;
                        }
                        else
                        {
                            ilkDBConnectionString = "SERVER=" + Program.ServerIp + ";" +
                                                 "DATABASE=is_kasa_" + enEskiYil + ";" +
                                                 "UID=" + Program.DBUsername + ";" +
                                                 "PASSWORD=" + Program.DBPass + ";" +
                                                 "Connection Timeout=1000";
                            mySS.ConnectionString = ilkDBConnectionString;
                        }
                        mySS.Open();
                        if (mySS.State == ConnectionState.Open)
                        {
                            string firstBillDate = "SELECT tarih FROM satisana WHERE kasano=" + kasano+ " ORDER BY  tarih ASC";
                            MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, mySS);
                            DataTable dtFirstBill = new DataTable();
                            myFirstBill.Fill(dtFirstBill);
                            if (dtFirstBill.Rows.Count > 0)
                            {
                                log.Log("First Receipt Date=" + tarih.tarih(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])));
                                return tarih.tarihDateTime(Convert.ToInt64(dtFirstBill.Rows[0].ItemArray[0])).ToShortDateString();
                            }
                            else
                            {
                                enEskiYil++;
                                log.Log("First Receipt Date Error. dtFirstBill.Rows.Count = 0, ConnString=" + ilkDBConnectionString);
                                goto A;
                                
                                //return "";
                            }

                        }
                        else
                        {
                            log.Log("First Receipt Date  Error. MySS Open Fehler! ConnString=" + ilkDBConnectionString);
                            return "";
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Log("First Receipt Date Error. Error Message=" + ex.Message);
                        enEskiYil++;
                        continue;
                    }
                }

            }
            log.Log("First Receipt Date Error. Unbekannt");
            return "";
        }
        private string FindTseHardWareTyp(string Drive, Logger log)
        {
            return SwissbitTseDetector.DetectSwissbitTseDevice(Drive);
        }

        private string FindCountTSE(Logger log)
        {
            

                log.Log("TSE COUNT");
               
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    try
                    {
                        string firstBillDate = "SELECT SUM(TotalTSE) totalT FROM ( SELECT count(*) TotalTSE FROM tse WHERE `clientid`<>'' group by `kassenr`)src";
                        MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, myConn);
                        DataTable dtFirstBill = new DataTable();
                        myFirstBill.Fill(dtFirstBill);
                        if (dtFirstBill.Rows.Count > 0)
                        {
                            return dtFirstBill.Rows[0].ItemArray[0].ToString(); 
                        }
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                
           
            
            log.Log("First TSE TX Date Error. Unbekannt");
            return "";
        }

        private void btnTSEAnmeldeDruck_Click(object sender, EventArgs e)
        {
            FisBarkodlu fisBarkod=new FisBarkodlu();
            fisBarkod.TseMeldeDatenDruck(tSEMeldeDaten);

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (tabControl1.SelectedIndex == 4)
            {
                tSEMeldeDaten = new MeldenDaten();
                Logger Log = new Logger("LOG\\TSEMELDEPFLICTH_LOG");
                if (Program.TSEID != 0)
                    FindFirstDB();
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
               
                    string firstBillDate = "SELECT * FROM tse WHERE tse_id=" + Program.TSEID;
                    MySqlDataAdapter myFirstBill = new MySqlDataAdapter(firstBillDate, myConn);
                    DataTable dtFirstBill = new DataTable();
                    myFirstBill.Fill(dtFirstBill);
                    if (dtFirstBill.Rows.Count > 0)
                    {
                        lb1.Items.Clear();
                        //`id`=[value-1],`tse_id`=[value-2],`tse_serial`=[value-3],`sig_algo`=[value-4],`zeit_format`=[value-5],`pd_encoding`=[value-6],`public_key`=[value-7],`zertifikat_i`=[value-8],
                        //`zertifikat_ii`=[value-9],`datum`=[value-10],`tsehersteller`=[value-11],`pin`=[value-12],`puk`=[value-13],`clientid`=[value-14],`timeadmin`=[value-15],`kassenr`=[value-16],
                        //`zertifikat_3`=[value-17],`zertifikat_4`=[value-18],`zertifikat_5`=[value-19],`zertifikat_6`=[value-20],`zertifikat_7`=[value-21],`zertifikat_8`=[value-22],`zertifikat_9`=[value-23],
                        //`aktiv`=[value-24],`BSIZertifierungsID`=[value-25],`inbetriebnahmeDerTSE`=[value-26],`artBauFormDerTSE`=[value-27],`bezeichnungBetriebStaette`=[value-28],
                        //`ausserbetribnahmeBetriebstaette`=[value-29],`inbetriebnahmeDesEas`=[value-30],`ausserBetriebnahmeDesEas`=[value-31],`grundDerAusserbetriebnahmeDesEas`=[value-32],
                        //`anmeldedatenOK`=[value-33],`gesendet`=[value-34] WHERE 1
                        if (Convert.ToInt16(dtFirstBill.Rows[0].ItemArray[23])==1)
                        {
                            label1.Visible = true;
                            lb1.Visible = true;
                            btnTSEAnmeldeDruck.Visible = true;
                            tSEMeldeDaten.anschaffungDesEas = dtFirstBill.Rows[0].ItemArray[32].ToString();
                            tSEMeldeDaten.anzahlDerEas = FindCountTSE(Log);
                            tSEMeldeDaten.artBauFormDerTSE = dtFirstBill.Rows[0].ItemArray[26].ToString();
                            tSEMeldeDaten.artDesEas = "Computergestützte/PC-Kassensysteme";
                            tSEMeldeDaten.ausserbetribnahmeBetriebstaette = dtFirstBill.Rows[0].ItemArray[28].ToString(); ;
                            tSEMeldeDaten.ausserBetriebnahmeDesEas = dtFirstBill.Rows[0].ItemArray[30].ToString();
                            tSEMeldeDaten.bemerkungZumEas = dtFirstBill.Rows[0].ItemArray[33].ToString();
                            tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas = dtFirstBill.Rows[0].ItemArray[31].ToString();
                            tSEMeldeDaten.bezeichnungBetriebStaette = Program.IsletmeAyarlar["isletme"] + "\n\r"+ Program.IsletmeAyarlar["strase"] + "','" + Program.IsletmeAyarlar["plz"] + "','" + Program.IsletmeAyarlar["stadt"]; //Datei "Stamm_Orte“ (location.csv) -Feld "LOC_NAME"
                            tSEMeldeDaten.bemerkungenBetriebStaette = dtFirstBill.Rows[0].ItemArray[34].ToString();
                            tSEMeldeDaten.BSIZertifierungsID = "0362-2019";//BSI - K - TR - 0362 - 2019 //BSI - K - TR - NNNN - YYYY
                            tSEMeldeDaten.herstellerDesEas = "Windsoft Tech GmbH"; //BSI - K - TR - NNNN - YYYY
                            tSEMeldeDaten.inbetriebnahmeDerTSE = dtFirstBill.Rows[0].ItemArray[25].ToString();
                            tSEMeldeDaten.inbetriebnahmeDesEas = dtFirstBill.Rows[0].ItemArray[29].ToString();
                            tSEMeldeDaten.modelDesEas = "ISS KASSE";///Stamm_Modell“ (cashregister.csv) -Feld "KASSE_MODELL"
                            tSEMeldeDaten.seriennummerDerTSE = dtFirstBill.Rows[0].ItemArray[2].ToString(); //BitConverter.ToString(myWorm.info().tseSerialNumber()).Replace("-", "");
                            tSEMeldeDaten.seriennummerDesEas = dtFirstBill.Rows[0].ItemArray[13].ToString(); 
                            tSEMeldeDaten.softwareDesEas = "ISS POS Kassensysteme"; //KASSE_SW_BRAND
                            tSEMeldeDaten.softwareVersionDesEas = dtFirstBill.Rows[0].ItemArray[35].ToString();//2.1.4.16
                                                                                  //UPDATE `tse` SET `id`=[value-1],`tse_id`=[value-2],`tse_serial`=[value-3],`sig_algo`=[value-4],`zeit_format`=[value-5],`pd_encoding`=[value-6],`public_key`=[value-7],`zertifikat_i`=[value-8],`zertifikat_ii`=[value-9],`datum`=[value-10],
                                                                                  //`tsehersteller`=[value-11],`pin`=[value-12],`puk`=[value-13],`clientid`=[value-14],`timeadmin`=[value-15],`kassenr`=[value-16],`zertifikat_3`=[value-17],`zertifikat_4`=[value-18],`zertifikat_5`=[value-19],`zertifikat_6`=[value-20],`zertifikat_7`=[value-21],
                                                                                  //`zertifikat_8`=[value-22],`zertifikat_9`=[value-23],`BSIZertifierungsID`=[value-24],`inbetriebnahmeDerTSE`=[value-25],`artBauFormDerTSE`=[value-26],`bezeichnungBetriebStaette`=[value-27],`ausserbetribnahmeBetriebstaette`=[value-28],`inbetriebnahmeDesEas`=[value-29],
                                                                                  //`ausserBetriebnahmeDesEas`=[value-30],`grundDerAusserbetriebnahmeDesEas`=[value-31],`anmeldedatenOK`=[value-32],`gesendet`=[value-33] WHERE 1
                            
                                lb1.Items.Add("TSE Anmeldedaten:");
                                lb1.Items.Add("1.Art des eAS:" + tSEMeldeDaten.artDesEas.ToString());
                                lb1.Items.Add("2.Software des eAS:" + tSEMeldeDaten.softwareDesEas.ToString());
                                lb1.Items.Add("3.Software-Version des eAS:" + tSEMeldeDaten.softwareVersionDesEas.ToString());
                                lb1.Items.Add("4.Seriennumer des eAS/Software App:" + tSEMeldeDaten.seriennummerDesEas.ToString());
                                lb1.Items.Add("5.Hersteller des eAS:" + tSEMeldeDaten.herstellerDesEas.ToString());
                                lb1.Items.Add("6.Model des eAS:" + tSEMeldeDaten.modelDesEas.ToString());
                                lb1.Items.Add("7.Anschaffungsdatum des eAS:" + tSEMeldeDaten.anschaffungDesEas.ToString());
                                lb1.Items.Add("8.Inbetriebnahme des eAS:" + tSEMeldeDaten.inbetriebnahmeDesEas.ToString());
                                lb1.Items.Add("9. Ausserbetriebnahme des eAS:" + tSEMeldeDaten.ausserBetriebnahmeDesEas.ToString());
                                lb1.Items.Add("10.Grund der Ausserbetrieb. des eAS:" + tSEMeldeDaten.grundDerAusserbetriebnahmeDesEas.ToString());
                                lb1.Items.Add("11.Bemerkungen zum eAS:" + tSEMeldeDaten.bemerkungZumEas.ToString());
                                lb1.Items.Add("12.Seriennumer der TSE:" + tSEMeldeDaten.seriennummerDerTSE.ToString());
                                lb1.Items.Add("13.BSI Zertifizierungs-ID:" + tSEMeldeDaten.BSIZertifierungsID.ToString());
                                lb1.Items.Add("14.Inbetriebnahme/Aktivierung der TSE:" + tSEMeldeDaten.inbetriebnahmeDerTSE.ToString());
                                lb1.Items.Add("15.Art/Bauform der TSE:" + tSEMeldeDaten.artBauFormDerTSE.ToString());
                                //lb1.Items.Add("Inbetriebnahme des eAS:" + tSEMeldeDaten.artDesEas.ToString());
                                lb1.Visible = true;
                                btnTSEAnmeldeDruck.Visible = true;

                            

                        }
                        
                    }
                

            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        
    }
    public class Logger
    {
        private readonly string logDirectory;
        private readonly string logFilePath;

        public Logger(string subDirectory, string fileName = "log.txt")
        {
            logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subDirectory);
            logFilePath = Path.Combine(logDirectory, fileName);

            // Alt klasör yoksa oluştur
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public void Log(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log yazılamadı: " + ex.Message);
            }
        }
    }
    public class SwissbitTseDetector
    {
        public static string DetectSwissbitTseDevice(string driveLetter)
        {
            Logger log = new Logger("LOG\\TSEMELDEPFLICTH_LOG");
            if (string.IsNullOrWhiteSpace(driveLetter))
                return "No drive letter provided.";

            string targetDriveLetter = driveLetter;

            foreach (ManagementObject drive in new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get())
            {
                string model = (drive["Model"] ?? "").ToString();
                string interfaceType = (drive["InterfaceType"] ?? "").ToString();
                string pnpId = (drive["PNPDeviceID"] ?? "").ToString();
                log.Log("Model="+model+ "->Interface Type="+ interfaceType+"->pnpID="+pnpId);
                foreach (ManagementObject partition in drive.GetRelated("Win32_DiskPartition"))
                {
                    foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                    {
                        string diskLetter = (logicalDisk["DeviceID"] ?? "").ToString();

                        if (diskLetter.Equals(targetDriveLetter, StringComparison.OrdinalIgnoreCase))
                        {
                            //|| model.ToLower().Contains("SDHC")
                            if ( model.ToLower().Contains("tse") )
                            {
                                string format = interfaceType.ToLower() == "usb"? "USB-Stick": (model.ToLower().Contains("sd") ? "SD-Karte" : "Unknown");
                                return format;
                                
                                //Console.WriteLine($"Swissbit TSE device found on {diskLetter}\nModel: {model}\nInterfaceType: {interfaceType}\nDevice Type: {format}");
                            }
                            else if(model.ToLower().Contains("sdhc"))
                            {
                                string format = "SD-Karte";
                                return format;
                            }
                            else
                            {
                                log.Log($"Drive {diskLetter} is not a Swissbit TSE device.");
                                Console.WriteLine($"Drive {diskLetter} is not a Swissbit TSE device.");
                                return "Unknown";
                            }
                        }
                    }
                }
            }

            return $"Drive {targetDriveLetter} not found or not associated with any known device.";
        }
    }
}
