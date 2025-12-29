using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using IS_KASSE;
using System.Data;

namespace IS_KASSE
{
    class Birimler
    {
        private int _birimid;

        public int Birimid
        {
            get { return _birimid; }
            set { _birimid = value; }
        }
        private string _birimad;

        public string Birimad
        {
            get { return _birimad; }
            set { _birimad = value; }
        }
        public Birimler(int birimid)
        {
            MySqlConnection myConn = new MySqlConnection();
            db baglanti = new db();
            myConn = baglanti.myconn();
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();

            }
            MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from birimler where birimid=" + birimid, myConn);
            DataTable dtGrup = new DataTable("artikelgrup");
            dtGrup.Clear();
            myDaGrup.Fill(dtGrup);
            int kaysay = dtGrup.Rows.Count;
            if (kaysay > 0)
            {
                this._birimad = dtGrup.Rows[0].ItemArray[1].ToString();
                this._birimid = (int)dtGrup.Rows[0].ItemArray[0];
                myConn.Close();

            }
            else
            {
                this._birimad = "Tanımsız";
                this._birimid = birimid;
                myConn.Close();
            }
        }

    }
}
