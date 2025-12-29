using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;


namespace IS_KASSE
{
    public partial class F_Bediener : Form
    {
        TextBox aktifnesne = null;
        Logger BedLog = new Logger("LOG\\SYSTEMSTART\\START-BED");
        public F_Bediener()
        {
           // MessageBox.Show(Application.ProductName);
            try
            {
                splashForm();
                ModifyRegistry reg = new ModifyRegistry();
                CheckForIllegalCrossThreadCalls = false;
                Thread t1 = new Thread(new ThreadStart(splashForm));
                t1.Start();
                //MessageBox.Show("Bonbeleg Basliyor");

                Thread.Sleep(3000);

                Program.dsp = new LD();

                InitializeComponent();
                try
                {
                    t1.Abort();
                }
                catch
                {
                    MessageBox.Show("Error 1");
                }
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message);
            }
            finally
            {
            }

        }

        public void splashForm()
        {
            try
            {
                Application.Run(new F_Splash());
            }
            catch(Exception dd)
            {
               //MessageBox.Show(dd.Message);
            }
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                BedLog.Log("Bediner Code:"+textBox1.Text);
                MySqlConnection conn = new MySqlConnection();
                db baglanti = new db();
                conn = baglanti.myconn();
                // conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    BedLog.Log("ConnectionState.Closed");
                    try
                    {
                        baglanti.openConnection();
                        BedLog.Log("ConnectionState Opened" );
                    }
                    catch (MySqlException hata)
                    {
                        BedLog.Log("ConnectionState catch Error Nr="+hata.Number);
                        // MessageBox.Show("Hata Mesajı:" + hata.Message + "\nHata Kodu:" + hata.Number);
                        if (hata.Number == 1042)
                        {
                            F_ServerOff frmServerOff = new F_ServerOff();
                            frmServerOff.ShowDialog();
                            if (frmServerOff.sonuc == false)
                            {
                                return;
                            }
                        }
                    }

                    baglanti = new db();
                    conn = baglanti.myconn();
                }
                if (conn.State == ConnectionState.Closed)
                {
                    try
                    {
                        baglanti.openConnection();
                        conn.Open();
                    }
                    catch
                    {
                    }
                }
                if (Program.programMode == "service")
                {
                    Thread.Sleep(1000);
                }
                try
                {
                    MySqlDataAdapter daArtikel = new MySqlDataAdapter("SELECT * FROM user WHERE kod=" + textBox1.Text, conn);
                    DataTable dtArtikel = new DataTable("user");
                    dtArtikel.Clear();
                    daArtikel.Fill(dtArtikel);
                    int rowCount = dtArtikel.Rows.Count;
                    if (rowCount > 0)
                    { 
                        User user = new User();
                       
                        user.Userid = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[0]);
                        user.UserAd = dtArtikel.Rows[0].ItemArray[1].ToString();
                        user.UserSoyad = dtArtikel.Rows[0].ItemArray[2].ToString();
                        user.AdSoyad = dtArtikel.Rows[0].ItemArray[1].ToString()+" "+ dtArtikel.Rows[0].ItemArray[2].ToString();
                        user.UserKod = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[3]);
                        user.ZYetki = Convert.ToInt32(dtArtikel.Rows[0].ItemArray[15]);
                        user.AppID = dtArtikel.Rows[0].ItemArray[14].ToString();
                        user.UserBarkod = dtArtikel.Rows[0].ItemArray[13].ToString();
                        Program.userClass = user;
                        Tarih tarih = new Tarih();
                        if ((int)dtArtikel.Rows[0].ItemArray[11] != 1)
                        {
                            MySqlDataAdapter daUserControl = new MySqlDataAdapter("SELECT * FROM usertakip WHERE userid=" + dtArtikel.Rows[0].ItemArray[0] + " and  tarih=" + tarih.bugunBaslangic() + " and onlinezaman<>0 and offlinezaman=0", conn);
                            DataTable dtUserControl = new DataTable("usertakip");
                            dtUserControl.Clear();
                            daUserControl.Fill(dtUserControl);
                            int rowCountUser = dtUserControl.Rows.Count;
                            if (rowCountUser > 0)
                            {

                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = Program.lang["26"];
                                frmerror.ShowDialog();
                                //conn.Close();
                                textBox1.Text = "";
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
                                        string ABSQLcheck = "SELECT * FROM anfangbestand WHERE bedienerid=" + dtArtikel.Rows[0].ItemArray[0] + " AND kasseid=" + Program.kasano + " AND datum>=" + tarih.bugunBaslangic() + " AND datum<=" + tarih.bugunBitis() + " AND betrag>0 "+AuswahlSQL;
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
                                                if (ab > 0)
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
                                            }
                                            catch (Exception ff)
                                            {
                                                myTrans.Rollback();
                                                F_GenericError frmerror = new F_GenericError();
                                                frmerror.lblMesaj.Text = ff.Message ;
                                                frmerror.ShowDialog();
                                            }
                                        }

                                    }
                                string userTakipSql = "INSERT INTO usertakip (tarih, onlinezaman, userid, kasaid, subeid) VALUES (" + tarih.bugunBaslangic() + "," + tarih.unixdate(DateTime.Now) + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + Program.kasano + "," + Program.subeno + ")";
                                MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                                if (coUserTakip.ExecuteNonQuery() > 0)
                                {
                                    Program.bedno = (int)dtArtikel.Rows[0].ItemArray[3];
                                    Program.bedID = (int)dtArtikel.Rows[0].ItemArray[0];
                                    Program.bedAdSoyad = dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
                                    Program.yonetici = 0;
                                    Program.BedSitzungId = coUserTakip.LastInsertedId;
                                    
                                    conn.Close();
                                    this.Close();
                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = Program.lang["27"]; ;
                                    frmerror.ShowDialog();
                                    //conn.Close();
                                }
                            }
                        }

                        //GECICI//
                        else if ((int)dtArtikel.Rows[0].ItemArray[11] == 1)
                        {
                            double ab = 0;
                            try
                            {
                                string AuswahlSQL = "";
                                if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                                {
                                    if (Program.GlobalAyarlar["TaglichZ"] == 1)
                                    {
                                        AuswahlSQL = " AND znr=0 AND datum>=" + tarih.bugunBaslangic() + " AND datum<=" + tarih.bugunBitis();
                                    }
                                    else
                                    {
                                        AuswahlSQL = " AND znr=0 ";
                                    }
                                    string ABSQLcheck = "SELECT * FROM anfangbestand WHERE bedienerid=" + dtArtikel.Rows[0].ItemArray[0] + " AND kasseid="+Program.kasano+" AND betrag>0 "+AuswahlSQL;
                                    MySqlDataAdapter daABTakip = new MySqlDataAdapter(ABSQLcheck, conn);
                                    DataTable dtAB = new DataTable();
                                    dtAB.Rows.Clear();
                                    daABTakip.Fill(dtAB);
                                    if (dtAB.Rows.Count < 1) // Bugun AB girmemis
                                    {
                                        
                                        //AB Form
                                        F_AnfangBestand anfangbestand = new F_AnfangBestand();
                                        anfangbestand.ShowDialog();
                                        ab = anfangbestand.verilenpara;
                                        if (ab > 0)
                                        {
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
                                            
                                            catch (Exception ff)
                                            {
                                                myTrans.Rollback();
                                                F_GenericError frmerror = new F_GenericError();
                                                frmerror.lblMesaj.Text = ff.Message ;
                                                frmerror.ShowDialog();
                                            }
                                        }


                                    }

                                }
                                string userTakipSql = "INSERT INTO usertakip (tarih, onlinezaman, userid, kasaid, subeid) VALUES (" + tarih.bugunBaslangic() + "," + tarih.unixdate(DateTime.Now) + "," + (int)dtArtikel.Rows[0].ItemArray[0] + "," + Program.kasano + "," + Program.subeno + ")";
                                MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                                if (coUserTakip.ExecuteNonQuery() > 0)
                                {
                                    Program.bedno = (int)dtArtikel.Rows[0].ItemArray[3];
                                    Program.bedID = (int)dtArtikel.Rows[0].ItemArray[0];
                                    Program.bedAdSoyad = "[Admin-]" + dtArtikel.Rows[0].ItemArray[1].ToString() + " " + dtArtikel.Rows[0].ItemArray[2].ToString();
                                    Program.yonetici = 1;
                                    Program.BedSitzungId = coUserTakip.LastInsertedId;
                                    //conn.Close();
                                    this.Close();
                                }
                                else
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = Program.lang["27"];
                                    frmerror.ShowDialog();
                                    //conn.Close();
                                }
                            }
                            catch (Exception ss)
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = ss.Message;
                                frmerror.ShowDialog();
                            }

                        }
                    }
                    else
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = Program.lang["28"]; ;
                        frmerror.ShowDialog();
                        textBox1.Text = "";
                    }
                }
                catch (Exception sxss)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = Program.lang["26"]; ;
                    frmerror.ShowDialog();
                }

            }


            else
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = Program.lang["29"]; ;
                frmerror.ShowDialog();
            }
            BedLog.Log("ENDE Button1.Click");
        }

        private void F_Bediener_Load(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length > 1)
            {

               // this.Location = Screen.AllScreens[0].WorkingArea.Location;
            }
            textBox1.Text = "";
            this.ActiveControl = textBox1;
            textBox1.Focus();
            aktifnesne = textBox1;
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

        private void btnNokta_Click(object sender, EventArgs e)
        {


        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void kryptonPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
