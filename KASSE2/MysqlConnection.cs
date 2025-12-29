using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;
using System.IO;
using MySql.Data;
using iss_Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Digests;


namespace IS_KASSE
{
    public class db
    {

        public MySqlConnection conn;
        private string server;
        Log logEntry = new Log();

        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        public String userName;
        public String pass;
        public string dbName;
        F_GenericError frmerror1;
        string connString = "";
        public MySqlConnection localConn;

        public db()
        {
            baglan();
        }

        private void baglan()
        {
            string ip = "";
            string INIdb = "";
            //MessageBox.Show(Program.ServerIp);
            var decrypt = new Encryptor<TwofishEngine, Sha1Digest>(Encoding.UTF8);
            RWIniFiles iniFile = new RWIniFiles("program.ini");
            INIdb = iniFile.ReadIni("SERVER", "DB");

            this.server = Program.ServerIp;
            this.userName = decrypt.EncryptreturnUsername();
            this.pass = decrypt.EncryptreturnPassword();
             ip = decrypt.EncryptreturnIP();
             Program.aktuelJahr = -1;
             if (Program.aktuelJahr != -1)
             {
                 if (Program.aktuelJahr == DateTime.Now.Year)
                 {
                     this.dbName = "is_kasa";
                 }
                 else
                 {
                     this.dbName = "is_kasa" + "_" + Program.aktuelJahr;
                     //this.dbName = "is_kasa";
                 }
             }
             else
             {
                 this.dbName = "is_kasa";
             }
             if (INIdb != "")
                 this.dbName = INIdb;
             Program.dbName = INIdb;
            //this.dbName = "is_kasa";
            connString = "SERVER=" + this.server + ";" +
                      "DATABASE=" + this.dbName + ";" +
                      "UID=" + this.userName + ";" +
                      "PASSWORD=" + this.pass + ";" +
                      "Charset=utf8" + ";" +
                      "Connection Timeout=5";
            conn = new MySqlConnection();
            //coConnectionTimeout = 5;
            conn.ConnectionString = connString;
            Program.dbName = this.dbName;
            //&& AND
            //|| OR
            if (Program.ServerIp != "localhost" && Program.ServerIp != "127.0.0.1")
            {
                setLocalConnetion();
            }


        }

        private void setLocalConnetion()
        {
            try
            {
                connString = "SERVER=" + "localhost" + ";" +
                          "DATABASE=" + this.dbName + ";" +
                          "UID=" + this.userName + ";" +
                          "PASSWORD=" + this.pass + ";" +
                          "Charset=utf8" + ";" +
                          "Connection Timeout=5";
                localConn = new MySqlConnection();
                //coConnectionTimeout = 5;
                localConn.ConnectionString = connString;
                Program.localConnection = localConn;
            }
            catch
            {
            }
            
        }
        public void openConnection()
        {
            baglan();
            try
            {
                conn.Open();
                if (Program.boot == 0)
                {
                    //Program acılısında yedek alma
                    //Thread artikelyedek = new Thread(new ThreadStart(ArtikelGuncelle));
                    //artikelyedek.Start();
                    //ArtikelGuncelle();
                    Program.boot = 1;
                }
                Program.connection = true;

            }
            catch (MySqlException aa)
            {
                logEntry.AddtoLogFile(aa.Message, this.ToString() + "");
                if (Program.connectionAttempt == 0 && Program.boot == 0)
                {
                    if (Program.ServerIp == "localhost")
                    {
                        while (Program.connectionAttempt<10)
                        {
                            Thread.Sleep(500);
                            
                            if (conn.State == ConnectionState.Closed)
                            {
                                try
                                {
                                    conn.Open();
                                }
                                catch
                                {
                                    Program.connectionAttempt++;
                                    continue;
                                }
                                

                            }
                            else
                            {
                                
                                return;
                            }
                        }
                    }
                    if (aa.Number == 1042)
                    {
                        F_ServerOff frmServerOff = new F_ServerOff();
                        Program.connectionAttempt = 1;
                        //Program.boot = 1;
                        frmServerOff.ShowDialog();

                        if (frmServerOff.sonuc == false)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Datenbank Error!";
                            frmerror.ShowDialog();

                            // MessageBox.Show("Datenbank Error!");
                            return;
                        }
                    }

                }
                else
                {

                    ServiceController controller = new ServiceController();

                    controller.MachineName = ".";
                    controller.ServiceName = "wampmysqld";

                    // Start the service
                    try
                    {
                        if (Program.programMode != "service")
                        {
                            Thread serviceMode = new Thread(ShowLocalMode);
                            serviceMode.Start();
                            string mcname = ".";
                            Process[] processes = null;
                            try
                            {
                                processes = Process.GetProcesses(mcname);
                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, this.ToString() + " Service List Exception");

                            }
                            int threadscount = 0;
                            foreach (Process p in processes)
                            {
                                try
                                {
                                    if (p.ProcessName == "mysqld")
                                    {
                                        //StartPortService();
                                        threadscount = 1;
                                        break;
                                    }
                                }
                                catch { }
                            }

                            if (threadscount == 0)
                            {
                                controller.Start();
                                //Thread.Sleep(1000);
                                while (controller.Status != ServiceControllerStatus.Running)
                                {
                                    controller.Refresh();
                                }
                                Program.programMode = "service";
                                Program.ServerIp = "localhost";
                                this.server = "localhost";
                                this.conn = null;
                                this.conn = new MySqlConnection();
                                this.connString = "";
                                this.connString = "SERVER=localhost;" +
                                       "DATABASE=is_kasa;" +
                                       "UID=root;" +
                                       "PASSWORD=sahbaz";
                                this.conn.ConnectionString = connString;
                                this.conn.Open();
                                serviceMode.Abort();
                            }
                            else
                            {

                                Program.programMode = "service";
                                Program.ServerIp = "localhost";
                                this.server = "localhost";
                                this.conn = null;
                                this.conn = new MySqlConnection();
                                this.connString = "";
                                this.connString = "SERVER=localhost;" +
                                       "DATABASE=is_kasa;" +
                                       "UID=root;" +
                                       "PASSWORD=sahbaz";
                                this.conn.ConnectionString = connString;
                                this.conn.Open();
                                serviceMode.Abort();
                            }
                        }
                        else
                        {
                            string mcname = ".";
                            Process[] processes = null;
                            try
                            {
                                processes = Process.GetProcesses(mcname);
                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, this.ToString() + " Service List Exception");

                            }
                            int threadscount = 0;
                            foreach (Process p in processes)
                            {
                                try
                                {
                                    if (p.ProcessName == "mysqld")
                                    {
                                        //StartPortService();
                                        threadscount = 1;
                                        break;
                                    }
                                }
                                catch { }
                            }
                            Thread serviceMode = new Thread(ShowLocalMode);
                            if (threadscount == 0)
                            {

                                serviceMode.Start();
                                ServiceController controller1 = new ServiceController();

                                controller1.MachineName = ".";
                                controller1.ServiceName = "wampmysqld";

                                // Start the service
                                try
                                {

                                    controller1.Start();
                                    //Thread.Sleep(1000);
                                    while (controller1.Status != ServiceControllerStatus.Running)
                                    {
                                        controller1.Refresh();
                                    }

                                    Program.programMode = "service";
                                    Program.ServerIp = "localhost";
                                    this.server = "localhost";
                                    this.conn = null;
                                    this.conn = new MySqlConnection();
                                    this.connString = "";
                                    this.connString = "SERVER=localhost;" +
                                           "DATABASE=is_kasa;" +
                                           "UID=root;" +
                                           "PASSWORD=sahbaz";
                                    this.conn.ConnectionString = connString;
                                    this.conn.Open();
                                    serviceMode.Abort();
                                }
                                catch (Exception ee)
                                {
                                    logEntry.AddtoLogFile(ee.Message, this.ToString() + " Service  Vorhanden Exception");
                                }
                            }
                            //Thread serviceMode = new Thread(ShowLocalMode);
                            //serviceMode.Start();
                            Program.programMode = "service";
                            Program.ServerIp = "localhost";
                            this.server = "localhost";
                            this.conn = null;
                            this.conn = new MySqlConnection();
                            this.connString = "";
                            this.connString = "SERVER=localhost;" +
                             "DATABASE=is_kasa;" +
                             "UID=root;" +
                             "PASSWORD=sahbaz";
                            this.conn.ConnectionString = connString;
                            this.conn.Open();
                            serviceMode.Abort();
                        }


                    }
                    catch (Exception ee)
                    {
                        string ss = ee.Message;
                    }
                }

            }
            finally
            {
                this.myconn();
            }

        }
        private void ArtikelGuncelle()
        {
            ServiceController controller = new ServiceController();
            string mcname = ".";
            Process[] processes = null;
            try
            {
                processes = Process.GetProcesses(mcname);
            }
            catch (Exception ex)
            {
                logEntry.AddtoLogFile(ex.Message, this.ToString() + " Service List Exception");

            }
            int threadscount = 0;
            Process p1 = new Process();
            foreach (Process p in processes)
            {
                try
                {
                    if (p.ProcessName == "mysqld")
                    {
                        //StartPortService();

                        threadscount = 1;
                        p1 = p;
                        break;

                    }
                }
                catch { }
            }

            if (threadscount == 0)
            {
                controller.MachineName = ".";
                controller.ServiceName = "wampmysqld";
                controller.Start();
                while (controller.Status != ServiceControllerStatus.Running)
                {
                    controller.Refresh();
                }
            }



            MySqlConnection localConn = new MySqlConnection();
            string connString = "SERVER=localhost;" +
                 "DATABASE=is_kasa;" +
                 "UID=root;" +
                 "PASSWORD=sahbaz; Connection Timeout=120; default command timeout=600";
            localConn.ConnectionString = connString;
            try
            {
                localConn.Open();
            }
            catch (Exception ee)
            {
                logEntry.AddtoLogFile(ee.Message, this.ToString() + " LocalConnection Catch!");
                return;
            }
            string file = Application.StartupPath + "\\BACKUP\\backupartikel.sql";
            using (localConn)
            {
                //string databaseList = "";

                //FileStream ff = new FileStream();
                MemoryStream ms = new MemoryStream();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    
                    cmd.CommandTimeout = 120;
                    /*using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        try
                        {
                            cmd.Connection = this.conn;
                            if (conn.State == ConnectionState.Closed)
                                conn.Open();
                            //mb.ExportInfo.
                            //mb.ExportInfo.TablesToBeExportedList.Clear();
                            List<string> tableAdi = new List<string>();
                            tableAdi.Add("artikel");
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add("artikel", "SELECT * FROM `artikel` WHERE `satisfiyat` > 0;");
                            mb.ExportInfo.TablesToBeExportedDic = dic;
                            mb.ExportInfo.AddCreateDatabase = false;
                            mb.ExportInfo.ExportTableStructure = false;
                            mb.ExportInfo.ExportRows = true;
                            mb.ExportInfo.ExportProcedures = false;
                            mb.ExportInfo.ExportTriggers = false;

                            mb.ExportToFile(file);



                        }
                        catch (Exception ee)
                        {
                            logEntry.AddtoLogFile(ee.Message, this.ToString() + "Artikel Backup Export!!-MysqlConnection.cs");
                            return;
                        }
                    }
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {

                        try
                        {

                            string leerTable = "TRUNCATE TABLE artikel";
                            MySqlCommand cmdLeer = new MySqlCommand(leerTable, localConn);
                            cmdLeer.ExecuteNonQuery();
                            localConn.Close();
                            localConn.Open();
                            cmd.Connection = localConn;
                            if (localConn.State == ConnectionState.Closed)
                                localConn.Open();
                            //mb.ImportInfo.TargetDatabase = "is_kasa";
                            mb.ImportInfo.DatabaseDefaultCharSet = "utf8";
                            mb.ImportFromFile(file);
                        }
                        catch (Exception ede)
                        {
                            logEntry.AddtoLogFile(ede.Message, this.ToString() + "Artikel Backup IMPORT Error!!");
                            return;
                        }
                    }*/
                }
            }
            File.Delete(file);
            localConn.Close();
            if (controller.ServiceName != "")
            {
                controller.Stop();
            }
            else
            {
                p1.Kill();
            }



        }
        private void ShowLocalMode()
        {
            frmerror1 = new F_GenericError();
            frmerror1.BringToFront();
            frmerror1.lblMesaj.Text = "SERVICE MODE! BITTE WARTEN!";
            frmerror1.ShowDialog();
            // openConnection();


        }

        public void baglantiguncelle(string SrvIp)
        {
            Program.ServerIp = SrvIp;
            this.server = SrvIp;
            this.conn = null;
            this.conn = new MySqlConnection();
            this.connString = "";
            this.connString = "SERVER=" + SrvIp + ";" +
                   "DATABASE=is_kasa;" +
                   "UID=root;" +
                   "PASSWORD=sahbaz";
            this.conn.ConnectionString = connString;
            this.conn.Open();
            if (this.conn.State == ConnectionState.Open)
            {
                Program.programMode = "server";
            }

        }
        public void closeConnection()
        {

            conn.Close();

        }

        public MySqlConnection myconn()
        {
            baglan();
           return this.conn;
        }
        private void ArtikelAktar()
        {
            VirgulAyikla vA = new VirgulAyikla();
            string gelenData = "";
            string gelenSatisDetay = "";
            long lastid = 0;
            MySqlTransaction myTrans = null;
            //myTrans = myConn.BeginTransaction();
            MySqlConnection localConn = new MySqlConnection();
            string LocalconnString = "SERVER= localhost;" +
                               "DATABASE=is_kasa;" +
                               "UID=" + Program.DBUsername + ";" +
                               "PASSWORD=" + Program.DBPass + ";";
            localConn.ConnectionString = LocalconnString;
            try
            {
                localConn.Open();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042)
                {
                    MessageBox.Show("Local Server ist zu!");
                    //label1.Text = "Local Server Kapalı";
                }
                else
                {
                    //label1.Text = ex.Message;
                }
            }
            if (localConn.State == ConnectionState.Open)
            {
                MySqlDataAdapter daArtikelLocal = new MySqlDataAdapter("SELECT * from satisana WHERE ServerSync=0", localConn);
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

                    //label1.Text = "Localde " + rowCount + " adet Kayıt Var";
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
                            string SyncResult = "UPDATE satisana WHERE ServerSync=1 WHERE satisanaid=" + localsatianaId;
                            MySqlCommand cmdSyncResult = new MySqlCommand(SyncResult, localConn);
                            cmdSyncResult.ExecuteNonQuery();
                        }
                        catch (Exception dd)
                        {
                            myTrans.Rollback();
                        }
                    }

                }
            }
        }
        public string grupname(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select grupad from musterigrup where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public string liferantname(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select ticariad from tedarikci where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public string einheitgrupname(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select birimad from birimler where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public string malgrupname(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select grupad from malgruplari where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public string tarih(Int64 tarih)
        {
            DateTime convertedDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tarih);
            return convertedDateTime.ToShortDateString();

        }
        public string malkdv(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select kdv from malgruplari where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public int unixdate()
        {
            DateTime date1 = new DateTime(1970, 1, 1);  //Refernzdatum (festgelegt)
            DateTime date2 = DateTime.Now;              //jetztiges Datum / Uhrzeit
            TimeSpan ts = new TimeSpan(date2.Ticks - date1.Ticks);  // das Delta ermitteln
            // Das Delta als gesammtzahl der sekunden ist der Timestamp
            return (Convert.ToInt32(ts.TotalSeconds));
        }
        public string kolliInhalt(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select kolliinhalt from artikel where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return dsgrup.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public string getKdvOran(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select grupid from artikel where barkod=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                string kdvOran = this.malkdv(Convert.ToInt64(dsgrup.Tables[0].Rows[0].ItemArray[0]));
                return kdvOran;
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public int getDefIndOran(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select faturaaltiindirim from musteri where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                int defOran = Convert.ToInt16(dsgrup.Tables[0].Rows[0].ItemArray[0]);
                return defOran;
            }
            catch
            {
                return 0;
            }

        }
        public int getKayitSayisi(string tabload, string alan, string kriter)
        {
            MySqlDataAdapter daGrup;
            if (kriter != "0")
            {
                daGrup = new MySqlDataAdapter("select count(*) from " + tabload + " where " + alan + "=" + kriter, conn);
            }
            else
            {
                daGrup = new MySqlDataAdapter("select count(*) from " + tabload, conn);
            }
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                int kaysay = Convert.ToInt16(dsgrup.Tables[0].Rows[0].ItemArray[0]);
                return kaysay;
            }
            catch
            {
                return 0;
            }

        }
        public string getKdvOranDurchID(Int64 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select grupid from artikel where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                string kdvOran = this.malkdv(Convert.ToInt64(dsgrup.Tables[0].Rows[0].ItemArray[0]));
                return kdvOran;
            }
            catch
            {
                return "Tanımsız";
            }

        }
        public int faturaTempKontrol(Int64 fatno)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select count(*) from faturatemp where fatno=" + fatno, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                int sayi = Convert.ToInt16(dsgrup.Tables[0].Rows[0].ItemArray[0]);
                return sayi;
            }
            catch
            {
                return 0;
            }

        }
        public string getProduktKod(Int32 id)
        {
            MySqlDataAdapter daGrup = new MySqlDataAdapter("select kod from artikel where id=" + id, conn);
            DataSet dsgrup = new DataSet();
            daGrup.Fill(dsgrup);
            try
            {
                return Convert.ToString(dsgrup.Tables[0].Rows[0].ItemArray[0]);

            }
            catch
            {
                return "";
            }

        }



    }
}
