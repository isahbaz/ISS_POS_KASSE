using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IS_KASSE
{
    public class Gerabo
    {
        string _geraboKey;

        public string GeraboKey
        {
            get { return _geraboKey; }
            set { _geraboKey = value; }
        }

        
        double _rabatrate;

        public double Rabatrate
        {
            get { return _rabatrate; }
            set { _rabatrate = value; }
        }

        double _totalpunkte;

        public double Totalpunkte
        {
            get { return _totalpunkte; }
            set { _totalpunkte = value; }
        }
        double _kredit;

        public double Kredit
        {
            get { return _kredit; }
            set { _kredit = value; }
        }
        string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        string _geraboCode;

        public string GeraboCode
        {
            get { return _geraboCode; }
            set { _geraboCode = value; }
        }

        MySqlConnection conn1 = new MySqlConnection();
         
        public Gerabo()
        {
            string selectSQL = "";
            db baglan = new db();
            conn1 = baglan.myconn();
            if (conn1.State == ConnectionState.Closed)
            {
                conn1.Open();
            }
            using(conn1)
            {
                selectSQL = "SELECT * FROM geraboinfo";
                MySqlDataAdapter myDaSel= new MySqlDataAdapter(selectSQL,conn1);
                DataTable dtSel= new DataTable();
                dtSel.Rows.Clear();
                myDaSel.Fill(dtSel);
                if (dtSel.Rows.Count > 0)
                {
                    GeraboKey = dtSel.Rows[0].ItemArray[1].ToString();
                    Rabatrate = Convert.ToDouble(dtSel.Rows[0].ItemArray[2]);
                    Url = dtSel.Rows[0].ItemArray[3].ToString();
                    //GeraboCode = dtSel.Rows[0].ItemArray[4].ToString();
                   
                }
                else
                {
                    GeraboKey = "";
                    Rabatrate = 0;
                }

            }
            

        }
        public GeraboPunkteeinlosung GeraboRedeemPremium(string url, string gerCode, string credits, string preid, string preEAN)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            RetunInfo = webreg.RedeemPremium(this.GeraboKey, gerCode, credits,preid, preEAN);
            return RetunInfo;


        }
        public GeraboPunkteeinlosung GeraboEinlosung(string url, string gerCode, string credits)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            RetunInfo = webreg.CreditEinlosen(this.GeraboKey, gerCode, credits);
            return RetunInfo;


        }
        public GeraboPunkteeinlosung GeraboCashBack(string url, string gerCode, string Punkte)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            RetunInfo = webreg.GeraboCashback(this.GeraboKey, gerCode, Punkte);
            return RetunInfo;


        }
        public GeraboPunkteeinlosung CollectCredit(string url, string gerCode, string Credits)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            RetunInfo = webreg.GeraboCollectCredit(this.GeraboKey, gerCode, Credits);
            return RetunInfo;


        }
        public GeraboPunkteeinlosung PunkteReedem(string url, string gerCode)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboPunkteeinlosung RetunInfo = new GeraboPunkteeinlosung();
            RetunInfo = webreg.PunkteZumGeld(this.GeraboKey, gerCode);
            return RetunInfo;


        }

        public GeraboAccountInfo GetKundeInfo(string url, string gerCode)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboAccountInfo RetunInfo = new GeraboAccountInfo();
            RetunInfo = webreg.GeraboKundenInfo(this.GeraboKey,gerCode);
            return RetunInfo;
            
            
        }
        public GeraboRoot CollectPoints(string url, string gerCode, string Betrag)
        {
            GeraboWebReq webreg = new GeraboWebReq();
            webreg.Url = url;
            GeraboRoot RetunInfo= new GeraboRoot();
            RetunInfo = webreg.GeraboCollectPunte(this.GeraboKey, gerCode, Betrag);
            return RetunInfo;

        }
    }
}
