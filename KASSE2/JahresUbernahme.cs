using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace IS_KASSE
{
    class JahresUbernahme
    {
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        public int JahresUbernahme_yap()
        {
            long lastInsertId = 0;
            using (myConn = baglanti.myconn())
            {
                if (myConn.State == ConnectionState.Closed)
                {
                    myConn.Open();
                }
                string aktuelJahrSQL = "SELECT MAX(jahr) FROM aktueljahr";
                MySqlDataAdapter daAktueljahr = new MySqlDataAdapter(aktuelJahrSQL, myConn);
                DataTable dtAktuelJahr = new DataTable();
                dtAktuelJahr.Clear();
                daAktueljahr.Fill(dtAktuelJahr);
                if (dtAktuelJahr.Rows.Count == 0)
                {
                    string aktueljahrInsertSql = "INSERT INTO aktueljahr (jahr) VALUES (" + DateTime.Now.Year + ")";
                    MySqlCommand cmdAktuelJahrInsert = new MySqlCommand(aktueljahrInsertSql, myConn);
                    cmdAktuelJahrInsert.ExecuteNonQuery();
                    return 0;
                }
                else
                {
                    int aktuelJahr = (int)dtAktuelJahr.Rows[0].ItemArray[0];
                    string arsivYili = dtAktuelJahr.Rows[0].ItemArray[0].ToString();
                    int jahr = DateTime.Now.Year;
                    if (jahr > aktuelJahr) //
                    {
                        //string arsivYili = dtAktuelJahr.Rows[0].ItemArray[0].ToString();
                        string dbName = "is_kasa_" + arsivYili;
                        //Yeni YIL AKTARIMLARINI YAP
                        //eger baska bir kasa veya terminal işe başlamışsa bunun kontrolu
                        string aktarmadurum = "SELECT * FROM jahresubernahme WHERE altesjahr=" + jahr;
                        MySqlCommand cmdDurumCheck = new MySqlCommand(aktarmadurum, myConn);
                        MySqlDataReader drDurum = cmdDurumCheck.ExecuteReader();
                        if (drDurum.HasRows)
                        {
                            drDurum.Read();
                            if (drDurum.GetInt16(4) == 1)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = "YILLIK AKTARIM ILE ILGILI BASLAMIS BIR ISLEM VAR! LUTFEN BEKLEYINIZ!\n SISTEM YENIDEN BASLATILACAKTIR!  "; //Program.lang["6"];
                                frmerror.ShowDialog();
                                return 1;

                            }
                            if (drDurum.GetInt16(4) == 2)
                            {
                                return 2;
                            }
                            drDurum.Close();
                        }
                        else
                        {
                            drDurum.Close();
                            MySqlTransaction myTrans = null;
                            myTrans = myConn.BeginTransaction();

                            Tarih date = new Tarih();
                            string aktarmadurumekle = "INSERT INTO jahresubernahme(altesjahr, neuesjahr, status, beginzeit) VALUES (" + aktuelJahr + "," + jahr + ",1," + date.unixdate(DateTime.Now) + ")";
                            MySqlCommand aktarmacmdDurum = new MySqlCommand(aktarmadurumekle, myConn);
                            aktarmacmdDurum.Transaction = myTrans;
                            aktarmacmdDurum.ExecuteNonQuery();
                            lastInsertId = aktarmacmdDurum.LastInsertedId;


                            string sqlCreateTable = "";

                            List<string> tablesName = new List<string>();
                            string tableSQL = "SHOW tables ";
                            MySqlCommand cmdTables = new MySqlCommand(tableSQL, myConn);
                            MySqlDataReader dr = cmdTables.ExecuteReader();
                            //cmdTables.Transaction = myTrans;
                            if (dr.HasRows)
                            {

                                tablesName.Clear();

                                while (dr.Read())
                                {
                                    tablesName.Add(dr.GetString(0));
                                }
                                dr.Close();

                                if (tablesName.Count > 0)
                                {
                                    try
                                    {
                                        string sqlCreateDB = "DROP DATABASE IF EXISTS  `" + dbName + "`; CREATE DATABASE `" + dbName + "` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;";
                                        MySqlCommand cmdCreateDB = new MySqlCommand(sqlCreateDB, myConn);
                                        cmdCreateDB.Transaction = myTrans;
                                        if (cmdCreateDB.ExecuteNonQuery() > 0)
                                        {
                                            foreach (string tblName in tablesName)
                                            {
                                                try
                                                {
                                                    sqlCreateTable = "CREATE TABLE " + dbName + "." + tblName + " LIKE " + tblName;
                                                    MySqlCommand cmdCreateTable = new MySqlCommand(sqlCreateTable, myConn);
                                                    cmdCreateTable.Transaction = myTrans;
                                                    cmdCreateTable.ExecuteNonQuery();

                                                    string sqlInsert = "INSERT INTO " + dbName + "." + tblName + " SELECT * FROM " + tblName;
                                                    MySqlCommand cmdInsert = new MySqlCommand(sqlInsert, myConn);
                                                    cmdInsert.Transaction = myTrans;
                                                    cmdInsert.ExecuteNonQuery();
                                                }
                                                catch (Exception eed)
                                                {
                                                    string mesaj = eed.Message;
                                                }




                                            }
                                            string aktarmadurumguncelle = "UPDATE jahresubernahme SET status=2, endezeit=" + date.unixdate(DateTime.Now) + " WHERE id=" + lastInsertId;
                                            MySqlCommand aktarmacmdDurumGuncelle = new MySqlCommand(aktarmadurumguncelle, myConn);
                                            aktarmacmdDurumGuncelle.Transaction = myTrans;
                                            aktarmacmdDurumGuncelle.ExecuteNonQuery();

                                            string aktueljahrInsertSql = "INSERT INTO aktueljahr (jahr) VALUES (" + DateTime.Now.Year + ")";
                                            MySqlCommand cmdAktuelJahrInsert1 = new MySqlCommand(aktueljahrInsertSql, myConn);
                                            cmdAktuelJahrInsert1.Transaction = myTrans;
                                            cmdAktuelJahrInsert1.ExecuteNonQuery();
                                            Program.aktuelJahr = DateTime.Now.Year;


                                            string leerTable = "TRUNCATE TABLE satisana; TRUNCATE TABLE satisdetay; TRUNCATE TABLE zbericht;";
                                            MySqlCommand cmdLeer = new MySqlCommand(leerTable, myConn);
                                            cmdLeer.ExecuteNonQuery();
                                            cmdLeer.Transaction = myTrans;
                                            myTrans.Commit();
                                            return 0;

                                        }
                                    }
                                    catch (Exception ee)
                                    {
                                        string eeE = ee.Message;
                                        myTrans.Rollback();
                                        return 1;

                                    }
                                    return 3;

                                }
                            }
                            else
                            {
                            }


                        }

                    }
                    else
                    {
                        Program.aktuelJahr = (int)dtAktuelJahr.Rows[0].ItemArray[0];
                    }
                    return 0;

                }
            }
        }
    }
}
