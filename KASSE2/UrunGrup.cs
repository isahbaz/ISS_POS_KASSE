using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IS_KASSE
{
    class UrunGrup
    {
        private double _mwst;

        public double Mwst
        {
            get { return _mwst; }
            set { _mwst = value; }
        }

        private int _grupno;
        
        public int Grupno
        {
            get { return _grupno; }
            set { _grupno = value; }
        }
        private string _grupad;

        public string Grupad
        {
            get { return _grupad; }
            set {
                value = _grupno.ToString();
                _grupad = value; 
            }
        }
        public UrunGrup(int grupid)
        {
            MySqlConnection myConn = new MySqlConnection();
            db baglanti = new db();
            myConn = baglanti.myconn();
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();

            }
            MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikelgrup where grupid="+grupid, myConn);
            DataTable dtGrup = new DataTable("artikelgrup");
            dtGrup.Clear();
            myDaGrup.Fill(dtGrup);
            int kaysay = dtGrup.Rows.Count;
            if (kaysay > 0)
            {
                this._grupad = dtGrup.Rows[0].ItemArray[1].ToString();
                this._grupno = (int)dtGrup.Rows[0].ItemArray[0];
                this._mwst = (double)dtGrup.Rows[0].ItemArray[2];
                
                myConn.Close();
            }
            else
            {
                this._grupad = "Tanımsız";
                this._grupno = 11;
                this._mwst = 0;
                myConn.Close();
            }

        }
    }
}
