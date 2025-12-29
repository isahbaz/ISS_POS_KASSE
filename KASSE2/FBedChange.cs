using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iss_KassenZahler;
using System.Runtime.InteropServices;
using System.Media;

namespace IS_KASSE
{
    public partial class FBedChange : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool MessageBeep(int type);
        TextBox aktifnesne = null;
        public Int32 lastid = -1;

        public FBedChange()
        {
            InitializeComponent();
            // MessageBeep(10);
            //Windows Ton für Information
            // SystemSounds.Asterisk.Play();
            //Windows Ton für Hinweise
            // SystemSounds.Beep.Play();beep
            //Windows Ton für Warnungen
            //SystemSounds.Exclamation.Play();soru
            //Windows Ton für Fehler
            // SystemSounds.Hand.Play(); hata
            //Windows Ton für Fragen
            // SystemSounds.Question.Play(); uyari
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                MySqlConnection conn = new MySqlConnection();
                db baglanti = new db();
                conn = baglanti.myconn();
                // conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();


                }
                MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * from user where kod=" + textBox1.Text, conn);
                DataTable dtArtikel = new DataTable("user");
                dtArtikel.Clear();
                daArtikel.Fill(dtArtikel);
                int rowCount = dtArtikel.Rows.Count;
                if (rowCount > 0)
                {
                    if (dtArtikel.Rows[0].ItemArray[10].ToString() == "1")
                    {
                        MySqlDataAdapter daLang = new MySqlDataAdapter("SELECT id, de FROM lang ", conn);
                        DataTable dtLang = new DataTable("lang");
                        dtLang.Rows.Clear();
                        daLang.Fill(dtLang);
                        if (dtLang.Rows.Count > 0)
                        {
                            if (Program.lang != null)
                            {
                                Program.lang.Clear();
                            }

                            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                            for (int a = 0; a < dtLang.Rows.Count; a++)
                            {
                                Program.lang.Add(dtLang.Rows[a].ItemArray[0].ToString(), dtLang.Rows[a].ItemArray[1].ToString());

                            }
                        }
                    }
                    else if (dtArtikel.Rows[0].ItemArray[10].ToString() == "0")
                    {
                        MySqlDataAdapter daLang = new MySqlDataAdapter("SELECT id, tr FROM lang ", conn);
                        DataTable dtLang = new DataTable("lang");
                        dtLang.Rows.Clear();
                        daLang.Fill(dtLang);
                        if (dtLang.Rows.Count > 0)
                        {
                            if (Program.lang != null)
                            {
                                Program.lang.Clear();
                            }

                            System.Threading.Thread.CurrentThread.CurrentUICulture = Program.oldUICulture;
                            for (int a = 0; a < dtLang.Rows.Count; a++)
                            {
                                Program.lang.Add(dtLang.Rows[a].ItemArray[0].ToString(), dtLang.Rows[a].ItemArray[1].ToString());

                            }
                        }
                    }
                    else if (dtArtikel.Rows[0].ItemArray[10].ToString() == "2")
                    {
                        MySqlDataAdapter daLang = new MySqlDataAdapter("SELECT id, en FROM lang ", conn);
                        DataTable dtLang = new DataTable("lang");
                        dtLang.Rows.Clear();
                        daLang.Fill(dtLang);
                        if (dtLang.Rows.Count > 0)
                        {
                            if (Program.lang != null)
                            {
                                Program.lang.Clear();
                            }

                            System.Threading.Thread.CurrentThread.CurrentUICulture = Program.oldUICulture;
                            for (int a = 0; a < dtLang.Rows.Count; a++)
                            {
                                Program.lang.Add(dtLang.Rows[a].ItemArray[0].ToString(), dtLang.Rows[a].ItemArray[1].ToString());

                            }
                        }
                    }
                    Tarih tarih = new Tarih();

                    if ((int)dtArtikel.Rows[0].ItemArray[11] != 1)
                    {
                        MySqlDataAdapter daUserControl = new MySqlDataAdapter("SELECT * from usertakip where userid=" + dtArtikel.Rows[0].ItemArray[0] + " and  tarih=" + tarih.bugunBaslangic() + " and onlinezaman<>0 and offlinezaman=0", conn);
                        DataTable dtUserControl = new DataTable("usertakip");
                        dtUserControl.Clear();
                        daUserControl.Fill(dtUserControl);
                        int rowCountUser = dtUserControl.Rows.Count;
                        if (rowCountUser > 0)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["26"];
                            frmerror.ShowDialog();
                            conn.Close();
                            textBox1.Text = "";
                        }
                        else
                        {
                            if (lastid != -1)
                            {
                                if (lastid == (int)dtArtikel.Rows[0].ItemArray[0])
                                {
                                    if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                    {
                                        string AuswahlSQL = "";
                                        if (Program.GlobalAyarlar["TaglichZ"] == 1)
                                        {
                                            AuswahlSQL = " AND znr=0 ";
                                        }
                                        else
                                        {
                                            AuswahlSQL = " AND znr=0 ";
                                        }
                                        string ABSQLcheck = "SELECT * FROM anfangbestand WHERE bedienerid=" + dtArtikel.Rows[0].ItemArray[0] + " AND kasseid=" + Program.kasano + " AND datum>=" + tarih.bugunBaslangic() + " AND datum<=" + tarih.bugunBitis() + " AND betrag>0 " + AuswahlSQL;
                                        MySqlDataAdapter daABTakip = new MySqlDataAdapter(ABSQLcheck, conn);
                                        DataTable dtAB = new DataTable();
                                        dtAB.Rows.Clear();
                                        daABTakip.Fill(dtAB);
                                        if (dtAB.Rows.Count < 1) // Bugun AB girmemis
                                        {
                                            //AB Form
                                            double ab = 0;
                                            //AB Form
                                            F_AnfangBestand anfangbestand = new F_AnfangBestand();
                                            anfangbestand.ShowDialog();
                                            ab = anfangbestand.verilenpara;
                                            MySqlTransaction myTrans;
                                            myTrans = conn.BeginTransaction();
                                            try
                                            {
                                                MySqlCommand cmdAB = new MySqlCommand();
                                                // INSERT INTO `rabatt`(`id`, `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`) 
                                                // VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13])
                                                cmdAB.Parameters.AddWithValue("@betrag", ab);
                                                string ABInputSQL = "INSERT INTO `anfangbestand` ( `kasseid`, `bedienerid`, `datum`, `betrag`) VALUES ( " + Program.kasano + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + tarih.unixdate(DateTime.Now) + ",@betrag)";
                                                cmdAB.CommandText = ABInputSQL;
                                                cmdAB.Connection = conn;
                                                cmdAB.Transaction = myTrans;
                                                cmdAB.ExecuteNonQuery();
                                                MySqlCommand cmdInsert = new MySqlCommand();
                                                cmdInsert.Parameters.AddWithValue("@betrag", ab);
                                                cmdInsert.Parameters.AddWithValue("@kaynak", "Anfangsbestand(Kleingeld) für Schublade Kasse-" + Program.kasano);
                                                cmdInsert.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmdInsert.Parameters.AddWithValue("@type", 1);
                                                cmdInsert.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                                cmdInsert.Parameters.AddWithValue("@hedef", 1);
                                                cmdInsert.Parameters.AddWithValue("@medium", Program.kasaAd);
                                                cmdInsert.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                                cmdInsert.Parameters.AddWithValue("@kassenr", Program.kasano);

                                                cmdInsert.Parameters.AddWithValue("@Anfangbestand", 1);
                                                cmdInsert.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`,kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                                cmdInsert.Connection = conn;
                                                cmdInsert.Transaction = myTrans;
                                                cmdInsert.ExecuteNonQuery();
                                                MySqlCommand cmdInsert1 = new MySqlCommand();
                                                cmdInsert1.Parameters.AddWithValue("@betrag", ab);
                                                cmdInsert1.Parameters.AddWithValue("@kaynak", "Geld Transit");
                                                cmdInsert1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                                cmdInsert1.Parameters.AddWithValue("@type", 2);
                                                cmdInsert1.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                                cmdInsert1.Parameters.AddWithValue("@hedef", 1);
                                                cmdInsert1.Parameters.AddWithValue("@medium", Program.kasaAd);
                                                cmdInsert1.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                                cmdInsert1.Parameters.AddWithValue("@kassenr", Program.kasano);

                                                cmdInsert1.Parameters.AddWithValue("@Anfangbestand", 1);
                                                cmdInsert1.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`,kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                                cmdInsert1.Connection = conn;
                                                cmdInsert1.Transaction = myTrans;
                                                cmdInsert1.ExecuteNonQuery();
                                                myTrans.Commit();
                                            }
                                            catch (Exception gg)
                                            {
                                                myTrans.Rollback();
                                                F_GenericError frmerror = new F_GenericError();
                                                frmerror.lblMesaj.Text = gg.Message;
                                                frmerror.ShowDialog();
                                                conn.Close();
                                            }
                                        }

                                    }
                                    string userTakipSql = "INSERT INTO usertakip (tarih, onlinezaman, userid, kasaid, subeid) VALUES (" + tarih.bugunBaslangic() + "," + tarih.unixdate(DateTime.Now) + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + Program.kasano + "," + Program.subeno + ")";
                                    MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                                    if (coUserTakip.ExecuteNonQuery() > 0)
                                    {
                                        Program.bedno = (int)dtArtikel.Rows[0].ItemArray[3];
                                        Program.bedAdSoyad = dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
                                        Program.bedID = (int)dtArtikel.Rows[0].ItemArray[0];
                                        Program.yonetici = 0;
                                        Program.BedSitzungId = coUserTakip.LastInsertedId;
                                        conn.Close();
                                        this.Close();
                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = Program.lang["27"];
                                        frmerror.ShowDialog();
                                        conn.Close();

                                    }
                                }
                                else
                                {
                                    SystemSounds.Exclamation.Play();
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "Beim Kasse darf nur Bediener : " + Program.bedAdSoyad + " oder ein Administrator anmelden!!!";
                                    frmerror.ShowDialog();
                                    conn.Close();
                                }
                            }
                            else
                            {
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    string AuswahlSQL = "";
                                    if (Program.GlobalAyarlar["TaglichZ"] == 1)
                                    {
                                        AuswahlSQL = " AND znr=0 ";
                                    }
                                    else
                                    {
                                        AuswahlSQL = " AND znr=0 ";
                                    }
                                    string ABSQLcheck = "SELECT * FROM anfangbestand WHERE bedienerid=" + dtArtikel.Rows[0].ItemArray[0] + " AND kasseid=" + Program.kasano + " AND datum>=" + tarih.bugunBaslangic() + " AND datum<=" + tarih.bugunBitis() + " AND betrag>0 " + AuswahlSQL;
                                    MySqlDataAdapter daABTakip = new MySqlDataAdapter(ABSQLcheck, conn);
                                    DataTable dtAB = new DataTable();
                                    dtAB.Rows.Clear();
                                    daABTakip.Fill(dtAB);
                                    if (dtAB.Rows.Count < 1) // Bugun AB girmemis
                                    {
                                        //AB Form
                                        double ab = 0;
                                        //AB Form
                                        F_AnfangBestand anfangbestand = new F_AnfangBestand();
                                        anfangbestand.ShowDialog();
                                        ab = anfangbestand.verilenpara;
                                        MySqlTransaction myTrans;
                                        myTrans = conn.BeginTransaction();
                                        try
                                        {
                                            MySqlCommand cmdAB = new MySqlCommand();
                                            // INSERT INTO `rabatt`(`id`, `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`) 
                                            // VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13])
                                            cmdAB.Parameters.AddWithValue("@betrag", ab);
                                            string ABInputSQL = "INSERT INTO `anfangbestand` ( `kasseid`, `bedienerid`, `datum`, `betrag`) VALUES ( " + Program.kasano + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + tarih.unixdate(DateTime.Now) + ",@betrag)";
                                            cmdAB.CommandText = ABInputSQL;
                                            cmdAB.Connection = conn;
                                            cmdAB.Transaction = myTrans;
                                            cmdAB.ExecuteNonQuery();

                                            MySqlCommand cmdInsert = new MySqlCommand();
                                            cmdInsert.Parameters.AddWithValue("@betrag", ab);
                                            cmdInsert.Parameters.AddWithValue("@kaynak", "Anfangsbestand(Kleingeld) für Schublade Kasse-" + Program.kasano);
                                            cmdInsert.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                            cmdInsert.Parameters.AddWithValue("@type", 1);
                                            cmdInsert.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                            cmdInsert.Parameters.AddWithValue("@hedef", 1);
                                            cmdInsert.Parameters.AddWithValue("@medium", Program.kasaAd);
                                            cmdInsert.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                            cmdInsert.Parameters.AddWithValue("@kassenr", Program.kasano);
                    
                                            cmdInsert.Parameters.AddWithValue("@Anfangbestand", 1);
                                            cmdInsert.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`,kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                            cmdInsert.Connection = conn;
                                            cmdInsert.Transaction = myTrans;
                                            cmdInsert.ExecuteNonQuery();

                                            MySqlCommand cmdInsert1 = new MySqlCommand();
                                            cmdInsert1.Parameters.AddWithValue("@betrag", ab);
                                            cmdInsert1.Parameters.AddWithValue("@kaynak", "Geld Transit");
                                            cmdInsert1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                            cmdInsert1.Parameters.AddWithValue("@type", 2);
                                            cmdInsert1.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                            cmdInsert1.Parameters.AddWithValue("@hedef", 1);
                                            cmdInsert1.Parameters.AddWithValue("@medium", Program.kasaAd);
                                            cmdInsert1.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                            cmdInsert1.Parameters.AddWithValue("@kassenr", Program.kasano);
                                            cmdInsert1.Parameters.AddWithValue("@Anfangbestand", 1);
                                            cmdInsert1.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`, kassenr, anfangbestand ) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                            cmdInsert1.Connection = conn;
                                            cmdInsert1.Transaction = myTrans;
                                            cmdInsert1.ExecuteNonQuery();
                                            myTrans.Commit();
                                        }
                                        catch (Exception gg)
                                        {
                                            myTrans.Rollback();
                                            F_GenericError frmerror = new F_GenericError();
                                            frmerror.lblMesaj.Text = gg.Message;
                                            frmerror.ShowDialog();
                                            conn.Close();
                                        }
                                    }

                                }
                                string userTakipSql = "INSERT INTO usertakip (tarih, onlinezaman, userid, kasaid, subeid) VALUES (" + tarih.bugunBaslangic() + "," + tarih.unixdate(DateTime.Now) + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + Program.kasano + "," + Program.subeno + ")";
                                MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                                if (coUserTakip.ExecuteNonQuery() > 0)
                                {
                                    Program.bedno = (int)dtArtikel.Rows[0].ItemArray[3];
                                    Program.bedAdSoyad = dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
                                    Program.bedID = (int)dtArtikel.Rows[0].ItemArray[0];
                                    Program.yonetici = 0;
                                    Program.BedSitzungId = coUserTakip.LastInsertedId;
                                    conn.Close();
                                    this.Close();
                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = Program.lang["27"];
                                    frmerror.ShowDialog();
                                    conn.Close();

                                }
                            }
                        }
                    }
                    else if ((int)dtArtikel.Rows[0].ItemArray[11] == 1)
                    {
                        if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                        {
                            string AuswahlSQL = "";
                            if (Program.GlobalAyarlar["TaglichZ"] == 1)
                            {
                                AuswahlSQL = " AND znr=0 ";
                            }
                            else
                            {
                                AuswahlSQL = " AND znr=0 ";
                            }
                            string ABSQLcheck = "SELECT * FROM anfangbestand WHERE bedienerid=" + dtArtikel.Rows[0].ItemArray[0] + " AND kasseid=" + Program.kasano + " AND datum>=" + tarih.bugunBaslangic() + " AND datum<=" + tarih.bugunBitis() + " AND betrag>0 " + AuswahlSQL;
                            MySqlDataAdapter daABTakip = new MySqlDataAdapter(ABSQLcheck, conn);
                            DataTable dtAB = new DataTable();
                            dtAB.Rows.Clear();
                            daABTakip.Fill(dtAB);
                            if (dtAB.Rows.Count < 1) // Bugun AB girmemis
                            {
                                //AB Form
                                double ab = 0;
                                //AB Form
                                F_AnfangBestand anfangbestand = new F_AnfangBestand();
                                anfangbestand.ShowDialog();
                                ab = anfangbestand.verilenpara;
                                MySqlTransaction myTrans;
                                myTrans = conn.BeginTransaction();
                                try
                                {

                                    MySqlCommand cmdAB = new MySqlCommand();
                                    // INSERT INTO `rabatt`(`id`, `bonnr`, `rabatname`, `rabattyp`, `rabatalani`, `rabatotalmenge`, `mwst7`, `mwst7betrag`, `mwst19`, `mwst19betrag`, `mwst0betrag`, `rabatmenge`, `rabatart`) 
                                    // VALUES ([value-1],[value-2],[value-3],[value-4],[value-5],[value-6],[value-7],[value-8],[value-9],[value-10],[value-11],[value-12],[value-13])
                                    cmdAB.Parameters.AddWithValue("@betrag", ab);
                                    string ABInputSQL = "INSERT INTO `anfangbestand` ( `kasseid`, `bedienerid`, `datum`, `betrag`) VALUES ( " + Program.kasano + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + tarih.unixdate(DateTime.Now) + ",@betrag)";
                                    cmdAB.CommandText = ABInputSQL;
                                    cmdAB.Connection = conn;
                                    cmdAB.Transaction = myTrans;
                                    cmdAB.ExecuteNonQuery();

                                    MySqlCommand cmdInsert = new MySqlCommand();
                                    cmdInsert.Parameters.AddWithValue("@betrag", ab);
                                    cmdInsert.Parameters.AddWithValue("@kaynak", "Anfangbestand(Kleingeld) für Schublade Kasse-"+Program.kasano);
                                    cmdInsert.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmdInsert.Parameters.AddWithValue("@type", 1);
                                    cmdInsert.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                    cmdInsert.Parameters.AddWithValue("@hedef", 1);
                                    cmdInsert.Parameters.AddWithValue("@medium", Program.kasaAd);
                                    cmdInsert.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                    cmdInsert.Parameters.AddWithValue("@kassenr", Program.kasano);
                                    cmdInsert.Parameters.AddWithValue("@Anfangbestand", 1);
                                    cmdInsert.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`,kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                    cmdInsert.Connection = conn;
                                    cmdInsert.Transaction = myTrans;
                                    cmdInsert.ExecuteNonQuery();

                                    MySqlCommand cmdInsert1 = new MySqlCommand();
                                    cmdInsert1.Parameters.AddWithValue("@betrag", ab);
                                    cmdInsert1.Parameters.AddWithValue("@kaynak", "Geld Transit");
                                    cmdInsert1.Parameters.AddWithValue("@datum", tarih.unixdate(DateTime.Now));
                                    cmdInsert1.Parameters.AddWithValue("@type", 2);
                                    cmdInsert1.Parameters.AddWithValue("@aciklama", "Bargeld Einlage in die Kasse");
                                    cmdInsert1.Parameters.AddWithValue("@hedef", 1);
                                    cmdInsert1.Parameters.AddWithValue("@medium", Program.kasaAd);
                                    cmdInsert1.Parameters.AddWithValue("@user", Program.bedAdSoyad);
                                    cmdInsert1.Parameters.AddWithValue("@kassenr", Program.kasano);

                                    cmdInsert1.Parameters.AddWithValue("@Anfangbestand", 1);
                                    cmdInsert1.CommandText = "INSERT INTO `kassenbuch`( `datum`, `type`,  `tutar`, `kaynak`, `aciklama`, `hedef`, `medium`, `username`,kassenr, anfangbestand) VALUES (@datum, @type, @betrag, @kaynak, @aciklama, @hedef, @medium,@user, @kassenr,@Anfangbestand)";
                                    cmdInsert1.Connection = conn;
                                    cmdInsert1.Transaction = myTrans;
                                    cmdInsert1.ExecuteNonQuery();
                                    myTrans.Commit();
                                }

                                catch (Exception gg)
                                {
                                    myTrans.Rollback();
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = gg.Message;
                                    frmerror.ShowDialog();
                                    conn.Close();
                                }
                            }

                        }
                        string userTakipSql = "INSERT INTO usertakip (tarih, onlinezaman, userid, kasaid, subeid) VALUES (" + tarih.bugunBaslangic() + "," + tarih.unixdate(DateTime.Now) + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + Program.kasano + "," + Program.subeno + ")";
                        MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                        if (coUserTakip.ExecuteNonQuery() > 0)
                        {
                            Program.bedno = (int)dtArtikel.Rows[0].ItemArray[3];
                            Program.bedID = (int)dtArtikel.Rows[0].ItemArray[0];
                            Program.bedAdSoyad = "[Admin]" + dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
                            Program.yonetici = 1;
                            Program.BedSitzungId = coUserTakip.LastInsertedId;
                            conn.Close();
                            this.Close();
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = Program.lang["27"];
                            frmerror.ShowDialog();
                            conn.Close();

                        }
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["28"];
                    frmerror.ShowDialog();
                    textBox1.Text = "";
                }
            }

            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["29"]; ;
                frmerror.ShowDialog();
            }
        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            if (aktifnesne != null)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btn = sender as ComponentFactory.Krypton.Toolkit.KryptonButton;

                int sayi;
                if (int.TryParse(btn.Text, out sayi))
                {
                    aktifnesne.Text += sayi.ToString();
                }
            }
        }

        private void FBedChange_Load(object sender, EventArgs e)
        {
            if (lastid == -1)
            {
                this.panel2.BackgroundImage = global::IS_KASSE.Properties.Resources.login_256;
            }
            else
            {
                this.panel2.BackgroundImage = global::IS_KASSE.Properties.Resources.coffee_256;
                label1.Text += "\nBitte Anmelden :\n" + Program.bedAdSoyad;
            }

            textBox1.Text = "";
            this.ActiveControl = textBox1;
            textBox1.Focus();
            aktifnesne = textBox1;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            F_XBericht frmxBericht = new F_XBericht();
            frmxBericht.ShowDialog();
        }

        private void btnKassenzahler_Click(object sender, EventArgs e)
        {
            if (Program.bedID != -1)
            {
                Dictionary<string, int> ParaAdetleri = new Dictionary<string, int>();

                KassenZahler kz = new KassenZahler();
                ParaAdetleri = kz.KassenZahler1();
                if (ParaAdetleri.Count > 0)
                {
                    FisBarkodlu fisclass = new FisBarkodlu();
                    fisclass.KassenZahlerDruck(ParaAdetleri);
                    this.Close();
                }
            }


        }


    }
}
