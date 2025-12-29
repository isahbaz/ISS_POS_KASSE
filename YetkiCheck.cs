using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using ComponentFactory.Krypton.Toolkit;

namespace IS_KASSE
{
    public class YetkiCheck
    {
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        private long _durum;

        public long Durum
        {
            get { return _durum; }
            set { _durum = value; }
        }
        private string _yetkiAdi;

        public string YetkiAdi
        {
            get { return _yetkiAdi; }
            set { _yetkiAdi = value; }
        }
        private bool _yetkiTanimlimi;

        public bool YetkiTanimlimi
        {
            get { return _yetkiTanimlimi; }
            set { _yetkiTanimlimi = value; }
        }
        public void YetkiKontrol(string btn)
        {
            using (myConn = new MySqlConnection())
            {

                myConn = baglanti.myconn();
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();

                }
                //SELECT yetkiler.yetkiid, useryetki.userid, useryetki.durum FROM yetkiler LEFT join useryetki on yetkiler.yetkiid=useryetki.yetkiid and useryetki.userid=12
                //SELECT useryetkiid, userid, yetkiid, durum FROM useryetki WHERE userid=
                string yetkiSQL="SELECT yetkiler.yetkiad, useryetki.durum FROM useryetki INNER JOIN yetkiler ON useryetki.yetkiid=yetkiler.yetkiid AND useryetki.userid=" + Program.bedID + " and yetkiler.element='" + btn + "'";
                MySqlDataAdapter daGrup1 = new MySqlDataAdapter(yetkiSQL, myConn);
                DataTable dtGrup1 = new DataTable();
                dtGrup1.Rows.Clear();
                daGrup1.Fill(dtGrup1);
                int kaySay = dtGrup1.Rows.Count;
                if (kaySay > 0)
                {
                    YetkiTanimlimi = true;
                    YetkiAdi = dtGrup1.Rows[0].ItemArray[0].ToString();
                    Durum = (int)dtGrup1.Rows[0].ItemArray[1];
                }
                else
                {
                    YetkiTanimlimi = false;
                }
            }
        
        }
    }
}
