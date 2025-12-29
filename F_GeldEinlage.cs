using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Conn;
using MySql.Data.MySqlClient;
using tar;
using iss_tse_v2;

namespace IS_KASSE
{
    public partial class F_GeldEinlage : Form
    {
        MySqlConnection myConn = new MySqlConnection();
        Conn.dbConn Db = new dbConn();
        tar.Tarih tarih = new tar.Tarih();
        int Islem = 0; ///siehe Kassenbuch tabelle für weitere Informationen und andere Optionen
        string gv_Type = "";
        Thread myTSEThread;
        public F_GeldEinlage()
        {
            InitializeComponent();
            myConn = Db.myconn();
            if (myConn.State == ConnectionState.Closed)
                myConn.Open();
        }

      

        private void textBox2_Enter(object sender, EventArgs e)
        {
            
        }

        private void F_GeldEinlage_Load(object sender, EventArgs e)
        {
            try
            {
                if (myConn.State == ConnectionState.Closed)
                    myConn.Open();
                string EinlageListSQL = "SELECT * FROM einnahmetype";
                MySqlDataAdapter myDA = new MySqlDataAdapter(EinlageListSQL, myConn);
                DataTable dt = new DataTable();
                dt.Rows.Clear();
                myDA.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Button buton1 = new Button();
                        buton1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                        buton1.Dock = System.Windows.Forms.DockStyle.Top;
                        buton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        buton1.Location = new System.Drawing.Point(0, 0);
                        buton1.Name = "buton1";
                        buton1.Size = new System.Drawing.Size(821, 42);
                        buton1.TabIndex = 5;
                        buton1.Text = dt.Rows[i].ItemArray[1].ToString();
                        buton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                        buton1.UseVisualStyleBackColor = false;
                        buton1.Click += new System.EventHandler(this.GenericButton);
                        flowLayoutPanel1.Controls.Add(buton1);
                    }
                }
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.Message);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Islem != 0)
            {
                if (myConn.State == ConnectionState.Closed)
                    myConn.Open();
                MySqlTransaction myTrans;
                myTrans = myConn.BeginTransaction();
                try
                {
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {


                        //INSERT INTO `kassenbuch`(`id`, `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`) VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10])
                        MySqlCommand cmdInsert = new MySqlCommand();
                        cmdInsert.Parameters.AddWithValue("@betrag", textBox1.Text);
                        cmdInsert.Parameters.AddWithValue("@kaynak", textBox2.Text);
                        cmdInsert.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                        cmdInsert.Parameters.AddWithValue("@type", 1);
                        cmdInsert.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse:"+gv_Type);
                        cmdInsert.Parameters.AddWithValue("@hedef", 1);
                        cmdInsert.Parameters.AddWithValue("@medium", Program.kasaAd);
                        cmdInsert.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                        cmdInsert.Parameters.AddWithValue("@kassenr", Program.kasano);
                        cmdInsert.Parameters.AddWithValue("@anfangbestand", Islem);
                        cmdInsert.Parameters.AddWithValue("@gv_type", gv_Type);

                        cmdInsert.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`, kassenr, anfangbestand, gv_type ) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand,@gv_type)";
                        cmdInsert.Connection = myConn;
                        cmdInsert.Transaction = myTrans;
                        cmdInsert.ExecuteNonQuery();
                        TSE_Protokoll_Start(Islem, 0, Convert.ToDouble(textBox1.Text));
                           /* MySqlCommand cmdInsert1 = new MySqlCommand();
                            cmdInsert1.Parameters.AddWithValue("@betrag", textBox1.Text);
                            cmdInsert1.Parameters.AddWithValue("@kaynak", "Geld Transit");
                            cmdInsert1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                            cmdInsert1.Parameters.AddWithValue("@type", 2);
                            cmdInsert1.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                            cmdInsert1.Parameters.AddWithValue("@hedef", 1);
                            cmdInsert1.Parameters.AddWithValue("@medium", Program.kasaAd);
                            cmdInsert1.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                            cmdInsert1.Parameters.AddWithValue("@kassenr", Program.kasano);
                            cmdInsert1.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`, kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user,@kassenr,1)";
                            cmdInsert1.Connection = myConn;
                            cmdInsert1.Transaction = myTrans;
                            cmdInsert1.ExecuteNonQuery();*/

                            //MySqlCommand cmdAB = new MySqlCommand();
                            // INSERT INTO `rabatt`(`id`, `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`) 
                            // VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13])
                           /* cmdAB.Parameters.AddWithValue("@betrag", textBox1.Text);
                            string ABInputSQL = "INSERT INTO `anfangbestand` ( `kasseid`, `bedienerid`, `datum`, `betrag`) VALUES ( " + Program.kasano + "," + Program.bedID + "," + tarih.unixdate(DateTime.Now) + ",@betrag)";
                            cmdAB.CommandText = ABInputSQL;
                            cmdAB.Connection = myConn;
                            cmdAB.Transaction = myTrans;
                            cmdAB.ExecuteNonQuery();*/
                        
                        myTrans.Commit();
                        Islem = 0;
                        this.Close();
                    }

                }
                catch (Exception dd)
                {
                    myTrans.Rollback();

                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie eine Geschäftsvorfälle aus!?");
            }

        }
        private void TSE_Protokoll_Start(int Islem, int transTyp, double Betrag)
        {
            if (Program.TSE == "1")
            {
                ulong TseTransNr = 0;
                ulong TseStartLogDatum = 0;
                Tarih tar = new Tarih();
                Program.TSELastUseDatetime = tar.unixdate(DateTime.Now);
                try
                {
                    WormTransactionResponse response;
                    //start transc (0,1,2)
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    WormReturnClass wormreturn = new WormReturnClass();
                    int returnedCode;


                    int a = 0;
                init: wormreturn = Program.TSEdll.doTransactionDLL(0, "Beleg", "", Program.ClientID, 0);
                    returnedCode = wormreturn.errorCode;
                    // MessageBox.Show(returnedCode.ToString());
                    while (a <= 5)
                    {
                        a++;
                        if (a >= 5)
                        {



                            if (myConn.State == ConnectionState.Closed)
                                myConn.Open();

                            //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                            string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`, kassenr, tseClienID) VALUES ('" + wormreturn.errorMessage + "'," + 0 + ", " + tar.unixdate(DateTime.Now) + "," + Program.kasano + ", '" + Program.ClientID + "')";
                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn);
                            cmdTseErrorCode.ExecuteNonQuery();
                            Program.TSE = "0";
                            myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 5));
                            myTSEThread.Start();
                            //Notfallconcept->
                            break;

                        }


                        if (returnedCode == 0)
                        {

                            response = Program.TSEdll.responseDLL;
                            TseTransNr = response.transactionNumber();
                            string s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                            + "\nTransaction Number: " + response.transactionNumber()
                            + "\nLog Time: " + response.logTime()
                            + "\nSignature Counter: " + response.signatureCounter()
                            + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                            + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                            //MessageBox.Show(s);
                            TseStartLogDatum = response.logTime();
                            TSE_Protokoll_Ende(Islem, TseTransNr, Betrag, response.logTime());
                            break;
                        }
                        else
                        {
                            string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                            try
                            {

                                string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level,`errorcode`, `tse_errormessage` ) VALUES ('" + wormreturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'GeldEinlage'" + ErrorMeldungArray[0] + "," + WormErrors.Wormerror(Convert.ToInt32(ErrorMeldungArray[0])) + " )";
                                MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn);
                                cmdTseErrorCode.ExecuteNonQuery();
                            }
                            catch (Exception dd)
                            {
                                //log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 444");

                            }


                            if (ErrorMeldungArray.Length == 1)
                            {
                                Program.TSELastError = ErrorMeldungArray[0];
                                if (Convert.ToInt32(ErrorMeldungArray[0]) == 4104) //no started Transaction
                                {
                                    Program.TSEdll.doTransactionDLL(0, "Beleg", "", Program.ClientID, 0);
                                    goto init;
                                }
                                else if (Convert.ToInt32(ErrorMeldungArray[0]) == 4180 || Convert.ToInt32(ErrorMeldungArray[0]) == 4181) //need Selt Test 
                                {
                                    //this.Infoevent("TSE Self TEST, Bitte Warten! ");
                                    Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);
                                    Program.TSEdll.doTransactionDLL(0, "Beleg", "", Program.ClientID, 0);
                                    //this.Infoevent("");
                                    goto init;

                                }
                                else if ((Convert.ToInt32(ErrorMeldungArray[0]) == 4198) || (Convert.ToInt32(ErrorMeldungArray[0]) == 4098)) //WORM_ERROR_NO_TIME_SET
                                {
                                    Program.TSEdll.ValidTimeCheck();
                                    if (Program.TSEdll.returnErrorCode == 0)
                                    {
                                        //this.Infoevent("");
                                        goto init;

                                    }
                                    else
                                    {

                                        Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);

                                        goto init;
                                    }

                                }
                                else if (Convert.ToInt32(ErrorMeldungArray[0]) == 4198) //WORM_ERROR_CLIENT_NOT_REGISTERED
                                {
                                    wormreturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                    Program.TSEdll.returnErrorCode = wormreturn.errorCode;
                                    if (Program.TSEdll.returnErrorCode == 0)
                                        goto init;
                                }

                            }

                            goto init;
                        }
                    }


                }

                catch (Exception rr)
                {

                }
            }
        }

        private void TSE_Notfall_Thread(string errorCode, int p_2)
        {
            TSE_Notfall tseNotfall = new TSE_Notfall();
            tseNotfall.ErrorCode = errorCode;
            tseNotfall.RecoveryError();
            if (Program.TSE == "1")
            {
                myTSEThread.Suspend();
            }
        }

        private void TSE_Protokoll_Ende(int Islem, ulong TseTransNr, double Betrag, ulong StartTime)
        {
            Tarih tar = new Tarih();
            if (Program.TSE == "1")
            {
                Program.TSELastUseDatetime = tar.unixdate(DateTime.Now);
                WormTransactionResponse response;
                //start transc (0,1,2)
                Stopwatch stopwatch = Stopwatch.StartNew();
                // processType: Kassenbeleg-V1
                //processData: <Transaktionstyp>^<Brutto-Steuerumsätze>^<Zahlungen>
                //Beleg^75.33_7.99_0.00_0.00_0.00^ 10.00:Bar_5.00:Bar:CHF_5.00:Bar:USD_64.30:Unbar
                //Beleg^0.00_0.00_0.00_0.00_-100.00^-100.00:Bar
                string processData = "Beleg^0.00_0.00_0.00_0.00_" + String.Format("{0:0.00}", Betrag).Replace(',', '.') + "^" + String.Format("{0:0.00}", Betrag).Replace(',', '.') + ":Bar";
                //this.TseProcessData = processData;
                int returnedDLLCode = -1;
                int a = 0;
            init: WormReturnClass wormreturn = Program.TSEdll.doTransactionDLL(2, "Kassenbeleg-V1", processData, Program.ClientID, TseTransNr);
                returnedDLLCode = wormreturn.errorCode;
                while (a <= 5)
                {
                    a++;
                    if (a >= 5)
                    {
                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        //INSERT INTO `tseerrorprotokoll`(`id`, `herstellererrorcode`, `lastbonid`, `datum`) VALUES ([value-1],[value-2],[value-3],[value-4])
                        string tseError = "INSERT INTO `tseerrorprotokoll`( `herstellererrorcode`, `lastbonid`, `datum`, kassenr, tseClienID) VALUES ('" + wormreturn.errorMessage + "'," + 0 + ", " + tar.unixdate(DateTime.Now) + "," + Program.kasano + ", '" + Program.ClientID + "')";
                        MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn);
                        cmdTseErrorCode.ExecuteNonQuery();
                        Program.TSE = "0";
                        myTSEThread = new Thread(() => TSE_Notfall_Thread(Program.TSELastError, 5));
                        myTSEThread.Start();
                        break;

                    }


                    if (returnedDLLCode == 0)
                    {
                       
                        response = Program.TSEdll.responseDLL;
                        //TseStartedTransaction = response.transactionNumber();
                        string s = "";
                        s = "Transaction time " + stopwatch.ElapsedMilliseconds + " ms!"
                        + "\nTransaction Number: " + response.transactionNumber()
                        + "\nLog Time: " + response.logTime()
                        + "\nSignature Counter: " + response.signatureCounter()
                        + "\nSignature: " + BitConverter.ToString(response.signature()).Replace("-", "")
                        + "\nSerial Number: " + BitConverter.ToString(response.serialNumber()).Replace("-", "");
                        //MessageBox.Show(s);
                        if (Program.PublicKey == "")
                        {
                            try
                            {
                                Program.PublicKey = BitConverter.ToString(Program.TSEdll.myWorm.info().tsePublicKey()).Replace("-", "");
                            }
                            catch (Exception ff)
                            {
                                MessageBox.Show(ff.Message);
                            }
                        }

                        if (Program.TSEID == 0)
                        {
                            string SelectSQLTSEID = "";

                            SelectSQLTSEID = "SELECT tse_id FROM tse  WHERE tse_serial= '" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(BitConverter.ToString(Program.TSEdll.myWorm.info().tseSerialNumber()).Replace("-", ""))) + "'";
                            MySqlDataAdapter myDAtseID = new MySqlDataAdapter(SelectSQLTSEID, myConn);
                            DataTable dttseID = new DataTable();
                            myDAtseID.Fill(dttseID);
                            if (dttseID.Rows.Count > 0)
                            {
                                Program.TSEID = Convert.ToInt16(dttseID.Rows[0].ItemArray[0]);
                            }
                        }

                        string tse_dsfink = " INSERT INTO `transactions_tse`( `kasseid`,  `bonid`, `tseid`, `transnummer`, `transstart`, `transende`, `vorgangart`, `sigzahler`, `sig`, `fehler`, `vorgangdaten`) VALUES" +
                            "(" + Program.kasano + "," + 0 + "," + Program.TSEID + "," + TseTransNr + "," + StartTime + "," + response.logTime() + ",'Beleg'," + response.signatureCounter() + ",'" + BitConverter.ToString(response.signature()).Replace("-", "") + "'," +
                            "'0','" + processData + "')";
                        MySqlCommand cmdTseErrorCode = new MySqlCommand(tse_dsfink, myConn);
                        cmdTseErrorCode.ExecuteNonQuery();

                        break;
                    }
                    else
                    {
                        try
                        {



                            string tseError = "INSERT INTO `tseerror`( `herstellererrorcode`, `datum`, Kassenr, TSEClientID, level) VALUES ('" + wormreturn.errorMessage + "'," + tar.unixdate(DateTime.Now) + "," + Program.kasano + ",'" + Program.ClientID + "', 'GeldEinlage-FinishTransaction')";
                            MySqlCommand cmdTseErrorCode = new MySqlCommand(tseError, myConn);
                            cmdTseErrorCode.ExecuteNonQuery();
                        }
                        catch (Exception dd)
                        {
                            //log.AddtoLogFile(dd.Message, "TSE START TRANSACTION, 1611");

                        }

                        string[] ErrorMeldungArray = wormreturn.errorMessage.Split('=');
                        if (ErrorMeldungArray.Length == 1)
                        {
                            Program.TSELastError = ErrorMeldungArray[0];
                            if (Convert.ToInt32(ErrorMeldungArray[0]) == 4104) //no started Transaction
                            {
                                Program.TSEdll.doTransactionDLL(0, "Beleg", "", Program.ClientID, 0);
                                goto init;
                            }
                            else if (Convert.ToInt32(ErrorMeldungArray[0]) == 4180 || Convert.ToInt32(ErrorMeldungArray[0]) == 4181) //need Selt Test 
                            {
                                //this.Infoevent("TSE Self TEST, Bitte Warten! ");
                                Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);
                                Program.TSEdll.doTransactionDLL(0, "Beleg", "", Program.ClientID, 0);
                                //this.Infoevent("");
                                goto init;

                            }
                            else if ((Convert.ToInt32(ErrorMeldungArray[0]) == 4198) || (Convert.ToInt32(ErrorMeldungArray[0]) == 4098)) //WORM_ERROR_NO_TIME_SET
                            {
                                Program.TSEdll.ValidTimeCheck();
                                if (Program.TSEdll.returnErrorCode == 0)
                                {
                                    //this.Infoevent("");
                                    goto init;

                                }
                                else
                                {

                                    Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);

                                    goto init;
                                }

                            }
                            else if (Convert.ToInt32(ErrorMeldungArray[0]) == 4198) //WORM_ERROR_CLIENT_NOT_REGISTERED
                            {
                                wormreturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                Program.TSEdll.returnErrorCode = wormreturn.errorCode;
                                if (Program.TSEdll.returnErrorCode == 0)
                                    goto init;
                            }

                        }


                    }
                }
            }
        }
        private void GenericButton(object sender, EventArgs e)
        {
            Button btnClick = sender as Button;
            textBox2.Text = "";
            textBox2.Text = btnClick.Text;
            /*if (btnClick.Name == "button2")
            {
                geldTransit = 1;
            }
            else
            {
                geldTransit = 0;
            }*/
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string Windir = "", OskDir = "";
                Windir = Environment.GetEnvironmentVariable("windir");
                Process.Start(@"c:\Windows\Sysnative\cmd.exe", "/c osk.exe");
                Thread.Sleep(1000);
                foreach (var process in Process.GetProcessesByName("cmd"))
                {
                    process.Kill();
                }

                /*
                 //System.Diagnostics.Process.Start("osk.exe");
                 OskDir = Windir + @"\System32\osk.exe";
                 System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                 string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                 string keyboardPath = Path.Combine(progFiles, "TabTip.exe");

                  Process.Start(keyboardPath);*/
            }
            catch (Exception xx)
            {
                System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                // MessageBox.Show(xx.Message);
            }
        }

        private void btngeldtransit_Click(object sender, EventArgs e)
        {
            Button btnClick = sender as Button;
            if (btnClick.Name == "btngeldtransit")
            {
                Islem = 18;
                gv_Type = "Geldtransit";
            }
            else if (btnClick.Name == "btnEinzahlung")
            {
                Islem = 23;
                gv_Type = "Einzahlung";
            }
            else if (btnClick.Name == "btnPrivateinlage")
            {
                Islem = 20;
                gv_Type = "Privateinlage";
            }
        }
    }
}
