using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iss_tse_v2;

namespace IS_KASSE
{
    public class TSE_Notfall
    {
         
        private string _errorCode;
        int EmailSend = 0;
        int EmailErrorCode = 0;

        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }
        public void RecoveryError()
        {
            start:
            WormStore Worm = Program.TSEdll.myWorm;
            WormReturnClass wormReturn = new WormReturnClass();
            if (Convert.ToInt32(ErrorCode) == 4104) //no started Transaction
            {
                Program.TSE = "1";
                Program.TSELastError = "0";
            }
            else if (Convert.ToInt32(ErrorCode) == 4180 || Convert.ToInt32(ErrorCode) == 4181) //need Selt Test 
            {
                //this.Infoevent("TSE Self TEST, Bitte Warten! ");
                wormReturn= Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);
                if (wormReturn.errorCode == 0)
                {
                     Program.TSE = "1";
                Program.TSELastError = "0";
                }
                else
                {
                    if ( (Convert.ToInt32(ErrorCode) == 4098)) //WORM_ERROR_NO_TIME_SET
                    {
                    init1:
                        Program.TSEdll.ValidTimeCheck();
                        if (Program.TSEdll.returnErrorCode == 0)
                        {
                            Program.TSE = "1";
                            Program.TSELastError = "0";

                        }
                        else
                        {

                            Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);

                            goto init1;
                        }
                    }
                }
               

            }
            else if ((Convert.ToInt32(ErrorCode) == 4198) || (Convert.ToInt32(ErrorCode) == 4098)) //WORM_ERROR_NO_TIME_SET
            {
            init2:
                Program.TSEdll.ValidTimeCheck();
                if (Program.TSEdll.returnErrorCode == 0)
                {
                    Program.TSE = "1";
                    Program.TSELastError = "0";

                }
                else
                {

                    Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);

                    goto init2;
                }

            }
            else if (Convert.ToInt32(ErrorCode) == 4113) //WORM_ERROR_CLIENT_NOT_REGISTERED
            {
                wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                Program.TSEdll.returnErrorCode = wormReturn.errorCode;
                if (Program.TSEdll.returnErrorCode == 0)
                {
                    Program.TSE = "1";
                    Program.TSELastError = "0";
                }

            }
            else if (Convert.ToInt32(ErrorCode) == 4105) //WORM_ERROR_MAX_PARALLEL_TRANSACTIONS
            {
                uint startedTrans1;
                startedTrans1 = Worm.info().startedTransactions();
               
                if (startedTrans1 > 0)
                {
                    List<UInt64> startedTrans = new List<ulong>();
                    startedTrans = Worm.transaction_listStartedTransactions().ToList<UInt64>();
                    byte[] processData = Encoding.ASCII.GetBytes("Started Transactions wurden von ISS POS Supportteam manuell geschlossen!");
                    for (int i = 0; i < startedTrans.Count; i++)
                    {
                        ulong transID = Convert.ToUInt64(startedTrans[i]);
                        Worm.transaction_finish(Program.ClientID, transID, processData, "AVBelegabbruch");
                    }
                }
                Program.TSE = "1";
                Program.TSELastError = "0";

            }
            else if (Convert.ToInt32(ErrorCode) ==3 ) //TSE Not Found
            {
                  Tarih tarih = new Tarih();
                //Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                Program.TSEdll = new F_TSEMain();
                Program.MyWorm = Program.TSEdll.myWorm;
                wormReturn = new WormReturnClass();
                    int TSEDLL = -1;
                    wormReturn = Program.TSEdll.doInitDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                try
                {
                    
                    TSEDLL = wormReturn.errorCode;
                    Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                    if (TSEDLL == 0)
                    {

                        Program.TSEready = 1;
                        wormReturn = Program.TSEdll.ValidTimeCheck();
                        string[] ErrorMeldungArray = wormReturn.errorMessage.Split('=');
                        if (wormReturn.errorCode != 0)
                        {
                            Program.TSELastUseDatetime = tarih.unixdate(DateTime.Now);
                            WormReturnClass newWormReturn = new WormReturnClass();
                            Program.TSELastError = "0";
                            Program.TseLastErrorMessage = "";
                            //this.Infoevent("TSE Self TEST, Bitte Warten! ");
                            newWormReturn = Program.TSEdll.SelfTestDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin, Program.ClientID);

                            //Buraya Notfall Thread eklenecek

                            if (newWormReturn.errorCode == 0)
                            {
                                Program.TSEdll.ValidTimeCheck();
                                if (Program.TSEdll.returnErrorCode == 0)
                                {
                                    Program.TSE = "1";
                                }
                                else
                                {
                                    //this.Infoevent("TSE Time Admin!, Bitte Warten! ");
                                    Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                    Program.TSE = "1";
                                }

                                //Program.PublicKey = Program.TSEdll.p pub;

                            }

                        }
                        else
                        {

                            Program.TSEdll.ValidTimeCheck();
                            if (Program.TSEdll.returnErrorCode == 0)
                            {
                                Program.TSELastError = "0";
                                Program.TseLastErrorMessage = "";
                                Program.TSE = "1";
                            }
                            else
                            {

                                Program.TSEdll.TimeSetupDLL(Program.TSEDrive, Program.TSEPin, Program.TSEPuk, Program.TSETimeAdmin);
                                Program.TSELastError = "0";
                                Program.TseLastErrorMessage = "";
                                Program.TSE = "1";
                            }

                            //Program.PublicKey = Program.TSEdll.p pub;


                        }


                        //  Program.PublicKey = Program.TSEdll.PublicKey();

                    }
                    else
                    {
                        Program.TSE = "0";
                        Program.TSELastError = "3";
                        Program.TseLastErrorMessage = "TSE not Found!";
                        if (Program.TSEEmailSend == 0)
                        {
                            if (Program.TSELastError != "0" && Program.TSELastError != "")
                            {
                                string html = "";
                                html = "Datum:" + DateTime.Now.ToShortDateString() + "<br>Uhr-Zeit:" + DateTime.Now.ToLongTimeString() + "<br>" +
                                    "Kundenname:" + Program.IsletmeAyarlar["isletme"] + "<br>Adresse:" + Program.IsletmeAyarlar["strase"] + " " + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "<br>" +
                                    "Tel :" + Program.IsletmeAyarlar["tel1"] + "<br>" + "Kundencode:" + Program.IsletmeAyarlar["kod"] + "<br>" +
                                    "<table border=\"1\" bordercolor=\"#000000\">" +
                                    "<tr>" +
                                    "<td><div align=\"center\">Error Code:</div></td>" +
                                    "<td><div align=\"center\">Error Message:</div></td>" +
                                    "<td><div align=\"center\"> Kasse-Nr:</div></td>" +
                                    "<td><div align=\"center\">Bedinername:</div></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "<td>" + this.ErrorCode + "</td>" +
                                    "<td>" + WormErrors.Wormerror(Convert.ToInt32(this.ErrorCode)) + "</td>" +
                                    "<td>" + Program.kasano + "</td>" +
                                    "<td>" + Program.bedAdSoyad + "</td>" +
                                    "</tr>";

                                html += "</table>";

                                if (AnadoluMail.TSE_ErrorMail(html) == true)
                                {
                                    Program.TSEEmailSend = 1;

                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            else if (Convert.ToInt32(ErrorCode) ==0 ) //NO Error
            {
                Program.TSELastError = "0";
                Program.TseLastErrorMessage = "";
                Program.TSE = "1";
            }
            else
            {

                if (Program.TSEEmailSend == 0)
                {
                    if (Program.TSELastError != "0" && Program.TSELastError != "")
                    {
                        string html = "";
                        html = "Datum:" + DateTime.Now.ToShortDateString() + "<br>Uhr-Zeit:" + DateTime.Now.ToLongTimeString() + "<br>" +
                            "Kundenname:" + Program.IsletmeAyarlar["isletme"] + "<br>Adresse:" + Program.IsletmeAyarlar["strase"] + " " + Program.IsletmeAyarlar["plz"] + " " + Program.IsletmeAyarlar["stadt"] + "<br>" +
                            "Tel :" + Program.IsletmeAyarlar["tel1"] + "<br>" + "Kundencode:" + Program.IsletmeAyarlar["kod"] + "<br>" +
                            "<table border=\"1\" bordercolor=\"#000000\">" +
                            "<tr>" +
                            "<td><div align=\"center\">Error Code:</div></td>" +
                            "<td><div align=\"center\">Error Message:</div></td>" +
                            "<td><div align=\"center\"> Kasse-Nr:</div></td>" +
                            "<td><div align=\"center\">Bedinername:</div></td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td>" + this.ErrorCode + "</td>" +
                            "<td>" + WormErrors.Wormerror(Convert.ToInt32(this.ErrorCode)) + "</td>" +
                            "<td>" + Program.kasano + "</td>" +
                            "<td>" + Program.bedAdSoyad + "</td>" +
                            "</tr>";

                        html += "</table>";

                        if (AnadoluMail.TSE_ErrorMail(html) == true)
                        {
                            Program.TSEEmailSend = 1;

                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
