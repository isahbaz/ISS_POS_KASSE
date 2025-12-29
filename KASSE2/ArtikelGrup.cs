using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IS_KASSE
{
    public class ArtikelGrup
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
            set
            {
                value = _grupno.ToString();
                _grupad = value;
            }
        }

        private int _grupTur;

        public int GrupTur
        {
            get { return _grupTur; }
            set { _grupTur = value; }
        }

        private int _rabatpunkte;

        public int Rabatpunkte
        {
            get { return _rabatpunkte; }
            set { _rabatpunkte = value; }
        }
        private int _isPfand;

        public int IsPfand
        {
            get { return _isPfand; }
            set { _isPfand = value; }
        }
        private int _isPfandRuckgabe;

        public int IsPfandRuckgabe
        {
            get { return _isPfandRuckgabe; }
            set { _isPfandRuckgabe = value; }
        }

        private int _isFleischTheke;

        public int IsFleischTheke
        {
            get { return _isFleischTheke; }
            set { _isFleischTheke = value; }
        }
        public ArtikelGrup(int grupid)
        {
            try
            {
                MySqlConnection myConn = new MySqlConnection();
                db baglanti = new db();
                myConn = baglanti.myconn();
                if (myConn.State == System.Data.ConnectionState.Closed)
                {
                    baglanti.openConnection();
                    if (myConn.State == System.Data.ConnectionState.Closed)
                    {
                        baglanti.openConnection();
                        myConn = baglanti.myconn();
                    }

                }

                MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikelgrup where grupid=" + grupid, myConn);
                DataTable dtGrup = new DataTable("artikelgrup");
                dtGrup.Clear();
                myDaGrup.Fill(dtGrup);
                int kaysay = dtGrup.Rows.Count;
                if (kaysay > 0)
                {
                    this._grupad = dtGrup.Rows[0].ItemArray[1].ToString();
                    this._grupno = (int)dtGrup.Rows[0].ItemArray[0];
                    this._mwst = (double)dtGrup.Rows[0].ItemArray[2];
                    this._grupTur = (int)dtGrup.Rows[0].ItemArray[3];
                    this._rabatpunkte = (int)dtGrup.Rows[0].ItemArray[5];
                    
                    myConn.Close();
                }
                else
                {
                    this._grupad = "nA";
                    this._grupno = 11;
                    this._mwst = 0;
                    this._grupTur = 0;
                    this._rabatpunkte = 0;
                    myConn.Close();
                }

            }
            catch
            {
            }
        }
        public ArtikelGrup ArtikelGrupInfo(string WHERE)
        {
            try
            {
                MySqlConnection myConn = new MySqlConnection();
                db baglanti = new db();
                myConn = baglanti.myconn();
                if (myConn.State == System.Data.ConnectionState.Closed)
                {
                    baglanti.openConnection();
                    if (myConn.State == System.Data.ConnectionState.Closed)
                    {
                        baglanti.openConnection();
                        myConn = baglanti.myconn();
                    }

                }

                MySqlDataAdapter myDaGrup = new MySqlDataAdapter("SELECT * from artikelgrup " + WHERE, myConn);
                DataTable dtGrup = new DataTable("artikelgrup");
                dtGrup.Clear();
                myDaGrup.Fill(dtGrup);
                int kaysay = dtGrup.Rows.Count;
                if (kaysay > 0)
                {
                    this._grupad = dtGrup.Rows[0].ItemArray[1].ToString();
                    this._grupno = (int)dtGrup.Rows[0].ItemArray[0];
                    this._mwst = (double)dtGrup.Rows[0].ItemArray[2];
                    this._grupTur = (int)dtGrup.Rows[0].ItemArray[3];
                    this._rabatpunkte = (int)dtGrup.Rows[0].ItemArray[5];
                    this.IsPfand = (int)dtGrup.Rows[0].ItemArray[11];
                    this.IsPfandRuckgabe = (int)dtGrup.Rows[0].ItemArray[12];
                    this._isFleischTheke = (int)dtGrup.Rows[0].ItemArray[13];

                    myConn.Close();
                    return this;
                }
                else
                {
                    this._grupad = "nA";
                    this._grupno = 11;
                    this._mwst = 0;
                    this._grupTur = 0;
                    this._rabatpunkte = 0;
                    this.IsPfand = 0;
                    this.IsPfandRuckgabe = 0;
                    this._isFleischTheke = 0;
                    myConn.Close();
                    return this;
                }

            }
            catch
            {
                return this;
            }
        }

        
    }
}
