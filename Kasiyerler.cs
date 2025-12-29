using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace IS_KASSE
{
    public class Kasiyerler
    {
        MySqlConnection myConn;
        
        db baglanti = new db(); 
        private int _kasaid;

        public int Kasaid
        {
            get { return _kasaid; }
            set { _kasaid = value; }
        }
        private int _kasano;

        public int Kasano
        {
            get { return _kasano; }
            set { _kasano = value; }
        }
        private string _kasaad;

        public string Kasaad
        {
            get { return _kasaad; }
            set { _kasaad = value; }
        }
        public DataSet KasiyerList(double bastarih, double bittarih)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
            string SQL = "SELECT DISTINCT  userid,  CONCAT_WS(' ',ad,soyad) as isim FROM user INNER JOIN satisana ON user.userid=satisana.kasiyerno WHERE satisana.tarih>" + bastarih + " AND satisana.tarih<" + bittarih;
            MySqlDataAdapter daKasa = new MySqlDataAdapter(SQL, myConn);
            DataSet dtKasa = new DataSet();
            dtKasa.Clear();
            daKasa.Fill(dtKasa);
            if (dtKasa.Tables[0].Rows.Count > 0)
            {
                return dtKasa;
            }
            else
            {
                return null;
            }
        }
    }
}
