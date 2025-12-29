using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.ServiceProcess;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;


namespace IS_KASSE
{
    class DBCheck
    {
        MySqlConnection myConn;
        db baglanti = new db();
        Log logEntry = new Log();
        List<string> tables = new List<string>();
        string connString = "";
        public void Check()
        {
            ServiceController controller = new ServiceController();
            if (Program.programMode != "service")
            {
                myConn = new MySqlConnection();

                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();

                }
                using (myConn)
                {
                    string SQL = "SELECT * FROM watchdb WHERE durum=0";
                    MySqlCommand daWatch = new MySqlCommand(SQL, myConn);
                    MySqlDataReader dr = daWatch.ExecuteReader();
                    MySqlConnection localConn;
                    if (dr.HasRows)
                    {
                        tables.Clear();
                        while (dr.Read())
                        {
                            tables.Add(dr.GetString(1));
                        }
                        dr.Close();
                        if (tables.Count > 0)
                        {
                            string mcname = ".";
                            Process[] processes = null;
                            try
                            {
                                processes = Process.GetProcesses(mcname);
                            }
                            catch (Exception ex)
                            {
                                logEntry.AddtoLogFile(ex.Message, "Process List Read!");
                            }
                            int threadscount = 0;
                            foreach (Process p in processes)
                            {
                                try
                                {
                                    if (p.ProcessName == "mysqld")
                                    {
                                        threadscount = 1;
                                        break;
                                    }
                                    /* string[] prcdetails = new string[] { p.ProcessName, p.Id.ToString(), p.StartTime.ToShortTimeString(), p.TotalProcessorTime.Duration().Hours.ToString() + ":" + p.TotalProcessorTime.Duration().Minutes.ToString() + ":" + p.TotalProcessorTime.Duration().Seconds.ToString(), (p.WorkingSet / 1024).ToString() + "k", (p.PeakWorkingSet / 1024).ToString() + "k", p.HandleCount.ToString(), p.Threads.Count.ToString() };
                                     ListViewItem proc = new ListViewItem(prcdetails);
                                     lvprocesslist.Items.Add(proc);
                                     threadscount += p.Threads.Count;*/
                                }
                                catch (Exception eexxw)
                                {
                                    logEntry.AddtoLogFile(eexxw.Message, "Process List Read von der List!");
                                }
                            }
                            if (threadscount == 0)
                            {

                                try
                                {


                                    controller.MachineName = ".";
                                    controller.ServiceName = "wampmysqld";
                                    controller.Start();
                                    while (controller.Status != ServiceControllerStatus.Running)
                                    {
                                        controller.Refresh();
                                    }
                                }
                                catch (Exception eexx)
                                {
                                    logEntry.AddtoLogFile(eexx.Message, this.ToString() + " ServiceControl Catch!");
                                    return;
                                }
                                if (controller.Status == ServiceControllerStatus.Running)
                                {
                                    localConn = new MySqlConnection();
                                    connString = "SERVER=localhost;" +
                                    "DATABASE=is_kasa;" +
                                    "UID=root;" +
                                    "PASSWORD=sahbaz";
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
                                    string file = Application.StartupPath + "\\BACKUP\\backup.sql";
                                    /*using (localConn)
                                    {
                                        //string databaseList = "";

                                        //FileStream ff = new FileStream();
                                        using (MySqlCommand cmd = new MySqlCommand())
                                        {
                                            using (MySqlBackup mb = new MySqlBackup(cmd))
                                            {
                                                try
                                                {
                                                    cmd.Connection = myConn;
                                                    if (myConn.State == ConnectionState.Closed)
                                                        myConn.Open();
                                                    mb.ExportInfo.TablesToBeExportedList = tables;
                                                    mb.ExportInfo.AddCreateDatabase = false;
                                                    mb.ExportInfo.ExportTableStructure = false;
                                                    mb.ExportInfo.ExportRows = true;
                                                    mb.ExportInfo.ExportProcedures = false;
                                                    mb.ExportInfo.ExportTriggers = false;
                                                    mb.ExportToFile(file);
                                                    foreach (string tablename in tables)
                                                    {
                                                        string leerTable = "TRUNCATE TABLE " + tablename;
                                                        MySqlCommand cmdLeer = new MySqlCommand(leerTable, localConn);
                                                        cmdLeer.ExecuteNonQuery();

                                                        string leerTableEntry = "DELETE FROM watchdb WHERE tablename='" + tablename + "'";
                                                        MySqlCommand cmdLeerEntry = new MySqlCommand(leerTableEntry, myConn);
                                                        cmdLeerEntry.ExecuteNonQuery();


                                                    }

                                                }
                                                catch (Exception ee)
                                                {
                                                    logEntry.AddtoLogFile(ee.Message, this.ToString() + " Backup Export!!");
                                                    return;
                                                }

                                            }

                                            using (MySqlBackup mb = new MySqlBackup(cmd))
                                            {
                                                try
                                                {
                                                    cmd.Connection = localConn;
                                                    if (localConn.State == ConnectionState.Closed)
                                                        localConn.Open();
                                                    mb.ImportInfo.TargetDatabase = "is_kasa";
                                                    mb.ImportInfo.DatabaseDefaultCharSet = "utf8";
                                                    mb.ImportFromFile(file);
                                                }
                                                catch (Exception ede)
                                                {
                                                    logEntry.AddtoLogFile(ede.Message, this.ToString() + " Backup IMPORT!!");
                                                    return;
                                                }
                                            }
                                        }


                                    }*/
                                    File.Delete(file);
                                    localConn.Close();
                                    controller.Stop();

                                }


                            }


                        }

                    }
                    dr.Close();
                    if (Program.programMode != "service")
                    {
                        /*Localden Servere Aktarım*/
                        localConn = new MySqlConnection();
                        connString = "SERVER=localhost;" +
                            "DATABASE=is_kasa;" +
                            "UID=root;" +
                            "PASSWORD=sahbaz";
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

                        using (localConn)
                        {
                            if (Program.programMode == "server")
                            {
                                if (localConn.State == ConnectionState.Closed)
                                {
                                    localConn.Open();
                                }

                                MySqlTransaction toServerTrans = null;
                                //toServerTrans = localConn.BeginTransaction();
                                string CheckSQL = "SELECT * FROM satisana WHERE SyncServer=0";
                                // MySqlCommand cmdCheck = new MySqlCommand(CheckSQL, localConn);
                                MySqlDataAdapter daCheck = new MySqlDataAdapter(CheckSQL, localConn);
                                DataTable dtCheck = new DataTable();
                                dtCheck.Rows.Clear();
                                daCheck.Fill(dtCheck);

                                if (dtCheck.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCheck.Rows.Count; i++)
                                    {
                                        toServerTrans = localConn.BeginTransaction();
                                        MySqlCommand cmdInsServ = new MySqlCommand();
                                        cmdInsServ.Parameters.AddWithValue("@4", dtCheck.Rows[i].ItemArray[4]);
                                        cmdInsServ.Parameters.AddWithValue("@5", dtCheck.Rows[i].ItemArray[5]);
                                        cmdInsServ.Parameters.AddWithValue("@6", dtCheck.Rows[i].ItemArray[6]);
                                        cmdInsServ.Parameters.AddWithValue("@7", dtCheck.Rows[i].ItemArray[7]);
                                        cmdInsServ.Parameters.AddWithValue("@8", dtCheck.Rows[i].ItemArray[8]);
                                        cmdInsServ.Parameters.AddWithValue("@9", dtCheck.Rows[i].ItemArray[9]);
                                        cmdInsServ.Parameters.AddWithValue("@10", dtCheck.Rows[i].ItemArray[10]);
                                        cmdInsServ.Parameters.AddWithValue("@11", dtCheck.Rows[i].ItemArray[11]);
                                        cmdInsServ.Parameters.AddWithValue("@12", dtCheck.Rows[i].ItemArray[12]);
                                        cmdInsServ.Parameters.AddWithValue("@13", dtCheck.Rows[i].ItemArray[13]);
                                        cmdInsServ.Parameters.AddWithValue("@14", dtCheck.Rows[i].ItemArray[14]);
                                        cmdInsServ.Parameters.AddWithValue("@15", dtCheck.Rows[i].ItemArray[15]);
                                        cmdInsServ.Parameters.AddWithValue("@16", dtCheck.Rows[i].ItemArray[16]);
                                        cmdInsServ.Parameters.AddWithValue("@17", dtCheck.Rows[i].ItemArray[17]);
                                        cmdInsServ.Parameters.AddWithValue("@18", dtCheck.Rows[i].ItemArray[18]);
                                        cmdInsServ.Parameters.AddWithValue("@19", dtCheck.Rows[i].ItemArray[19]);
                                        cmdInsServ.Parameters.AddWithValue("@20", dtCheck.Rows[i].ItemArray[20]);
                                        cmdInsServ.Parameters.AddWithValue("@21", dtCheck.Rows[i].ItemArray[21]);
                                        cmdInsServ.Parameters.AddWithValue("@22", dtCheck.Rows[i].ItemArray[22]);
                                        cmdInsServ.Parameters.AddWithValue("@23", dtCheck.Rows[i].ItemArray[23]);
                                        cmdInsServ.Parameters.AddWithValue("@24", dtCheck.Rows[i].ItemArray[24]);
                                        cmdInsServ.Parameters.AddWithValue("@25", dtCheck.Rows[i].ItemArray[25]);
                                        cmdInsServ.Parameters.AddWithValue("@26", dtCheck.Rows[i].ItemArray[26]);
                                        cmdInsServ.Parameters.AddWithValue("@27", dtCheck.Rows[i].ItemArray[27]);
                                        cmdInsServ.Parameters.AddWithValue("@28", dtCheck.Rows[i].ItemArray[28]);
                                        cmdInsServ.Parameters.AddWithValue("@29", dtCheck.Rows[i].ItemArray[29]);
                                        cmdInsServ.Parameters.AddWithValue("@30", dtCheck.Rows[i].ItemArray[30]);
                                        cmdInsServ.Parameters.AddWithValue("@31", dtCheck.Rows[i].ItemArray[31]);
                                        cmdInsServ.Parameters.AddWithValue("@32", dtCheck.Rows[i].ItemArray[32]);
                                        cmdInsServ.Parameters.AddWithValue("@33", dtCheck.Rows[i].ItemArray[33]);
                                        cmdInsServ.Parameters.AddWithValue("@34", dtCheck.Rows[i].ItemArray[34]);
                                        cmdInsServ.Parameters.AddWithValue("@35", dtCheck.Rows[i].ItemArray[35]);
                                        cmdInsServ.Parameters.AddWithValue("@36", dtCheck.Rows[i].ItemArray[36]);
                                        cmdInsServ.Parameters.AddWithValue("@37", dtCheck.Rows[i].ItemArray[37]);
                                        cmdInsServ.Parameters.AddWithValue("@38", dtCheck.Rows[i].ItemArray[38]);
                                        cmdInsServ.Parameters.AddWithValue("@39", dtCheck.Rows[i].ItemArray[39]);
                                        cmdInsServ.Parameters.AddWithValue("@40", dtCheck.Rows[i].ItemArray[40]);
                                        cmdInsServ.Parameters.AddWithValue("@41", dtCheck.Rows[i].ItemArray[41]);
                                        cmdInsServ.Parameters.AddWithValue("@42", 1);
                                        cmdInsServ.Parameters.AddWithValue("@43", 0);
                                        //cmdInsServ.Parameters.AddWithValue("@40", dtCheck.Rows[i].ItemArray[21]);
                                        // cmdInsServ.Parameters.AddWithValue("@41", dtCheck.Rows[i].ItemArray[22]);

                                        string insSQL = "INSERT INTO satisana VALUES(null,0," + dtCheck.Rows[i].ItemArray[0] + "," + dtCheck.Rows[i].ItemArray[3] + ",@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25,@26,@27,@28,@29,@30,@31,@32,@33,@34,@35,@36,@37,@38,@39,@40,@41,@42,@43 )";
                                        cmdInsServ.CommandText = insSQL;
                                        cmdInsServ.Connection = myConn;
                                        cmdInsServ.Transaction = toServerTrans;
                                        if (cmdInsServ.ExecuteNonQuery() > 0)
                                        {
                                            long lastid = cmdInsServ.LastInsertedId;
                                            string selectLocalSQL = "SELECT * FROM satisdetay WHERE fisno =" + dtCheck.Rows[i].ItemArray[0];

                                            //string scr = "INSERT INTO satisdetay WHERE fisno =" + dtCheck.Rows[i].ItemArray[0];
                                            MySqlDataAdapter daSelDetay = new MySqlDataAdapter(selectLocalSQL, localConn);
                                            DataTable dtSeldetay = new DataTable();
                                            dtSeldetay.Clear();
                                            daSelDetay.Fill(dtSeldetay);
                                            if (dtSeldetay.Rows.Count > 0)
                                            {
                                                for (int a = 0; a < dtSeldetay.Rows.Count; a++)
                                                {
                                                    MySqlCommand cmdInsDetay = new MySqlCommand();
                                                    //cmdInsDetay.Parameters.AddWithValue("@4", dtSeldetay.Rows[i].ItemArray[4]);
                                                    cmdInsDetay.Parameters.AddWithValue("@1", dtSeldetay.Rows[a].ItemArray[1]);
                                                    cmdInsDetay.Parameters.AddWithValue("@3", dtSeldetay.Rows[a].ItemArray[3]);
                                                    cmdInsDetay.Parameters.AddWithValue("@4", dtSeldetay.Rows[a].ItemArray[4]);
                                                    cmdInsDetay.Parameters.AddWithValue("@5", dtSeldetay.Rows[a].ItemArray[5]);
                                                    cmdInsDetay.Parameters.AddWithValue("@6", dtSeldetay.Rows[a].ItemArray[6]);
                                                    cmdInsDetay.Parameters.AddWithValue("@7", dtSeldetay.Rows[a].ItemArray[7]);
                                                    cmdInsDetay.Parameters.AddWithValue("@8", dtSeldetay.Rows[a].ItemArray[8]);
                                                    cmdInsDetay.Parameters.AddWithValue("@9", dtSeldetay.Rows[a].ItemArray[9]);
                                                    cmdInsDetay.Parameters.AddWithValue("@10", dtSeldetay.Rows[a].ItemArray[10]);
                                                    cmdInsDetay.Parameters.AddWithValue("@11", dtSeldetay.Rows[a].ItemArray[11]);
                                                    cmdInsDetay.Parameters.AddWithValue("@12", dtSeldetay.Rows[a].ItemArray[12]);
                                                    cmdInsDetay.Parameters.AddWithValue("@13", dtSeldetay.Rows[a].ItemArray[13]);
                                                    cmdInsDetay.Parameters.AddWithValue("@14", dtSeldetay.Rows[a].ItemArray[14]);
                                                    cmdInsDetay.Parameters.AddWithValue("@15", dtSeldetay.Rows[a].ItemArray[15]);
                                                    cmdInsDetay.Parameters.AddWithValue("@16", dtSeldetay.Rows[a].ItemArray[16]);
                                                    cmdInsDetay.Parameters.AddWithValue("@17", dtSeldetay.Rows[a].ItemArray[17]);
                                                    cmdInsDetay.Parameters.AddWithValue("@18", dtSeldetay.Rows[a].ItemArray[18]);
                                                    cmdInsDetay.Parameters.AddWithValue("@19", dtSeldetay.Rows[a].ItemArray[19]);
                                                    cmdInsDetay.Parameters.AddWithValue("@20", dtSeldetay.Rows[a].ItemArray[20]);
                                                    cmdInsDetay.Parameters.AddWithValue("@21", dtSeldetay.Rows[a].ItemArray[21]);
                                                    cmdInsDetay.Parameters.AddWithValue("@22", dtSeldetay.Rows[a].ItemArray[22]);
                                                    cmdInsDetay.Parameters.AddWithValue("@23", dtSeldetay.Rows[a].ItemArray[23]);


                                                    string insSQLDet = "INSERT INTO satisdetay VALUES(null,@1," + lastid + ",@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23)";
                                                    cmdInsDetay.CommandText = insSQLDet;
                                                    cmdInsDetay.Connection = myConn;
                                                    cmdInsDetay.Transaction = toServerTrans;
                                                    if (cmdInsDetay.ExecuteNonQuery() > 0)
                                                    {
                                                       


                                                    }
                                                    else
                                                    {
                                                        toServerTrans.Rollback();
                                                        return;
                                                    }
                                                }
                                                string delLocal = "UPDATE satisana SET SyncServer=1 WHERE satisanaid=" + dtCheck.Rows[i].ItemArray[0];
                                                MySqlCommand delLocalAna = new MySqlCommand(delLocal, localConn);
                                                if (delLocalAna.ExecuteNonQuery() > 0)
                                                {


                                                }
                                                else
                                                {
                                                    toServerTrans.Rollback();

                                                }
                                                toServerTrans.Commit();

                                            }


                                        }
                                        else
                                        {
                                            toServerTrans.Rollback();
                                            return;
                                        }

                                    }

                                    //toServerTrans = localConn.BeginTransaction();
                                  /*  string leerTable = "TRUNCATE TABLE satisana; TRUNCATE TABLE satisdetay;";
                                    MySqlCommand cmdLeer = new MySqlCommand(leerTable, localConn);
                                    if (cmdLeer.ExecuteNonQuery() > 0)
                                    {
                                        toServerTrans.Commit();
                                    }
                                    else
                                    {
                                        toServerTrans.Rollback();
                                    }*/
                                }
                            }

                        }
                    }
                    /**/

                }

            }
        }

    }
}
