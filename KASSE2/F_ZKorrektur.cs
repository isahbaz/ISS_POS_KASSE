using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iss_VA;


namespace IS_KASSE
{
    public partial class F_ZKorrektur : Form
    {
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        Tarih tarih = new Tarih();
        VirgulAyikla vA = new VirgulAyikla();
        public F_ZKorrektur()
        {
            InitializeComponent();
            myConn = baglanti.myconn();
        }

        private void F_ZKorrektur_Load(object sender, EventArgs e)
        {
            dgvUmsatz.Columns.Add("startBonDT", "START BON Datum");
            dgvUmsatz.Columns.Add("startBonNr", "START BON NR");
            dgvUmsatz.Columns.Add("endBonDT", "END BON DATUM");
            dgvUmsatz.Columns.Add("endBonNr", "END BON NR");

            dgvUmsatz.Columns.Add("bonAnzahl", "BON ANZAHL");
            dgvUmsatz.Columns.Add("kasano", "KASSENR");
            dgvUmsatz.Columns.Add("maxLocID", "MAXLOCID");
            dgvUmsatz.Columns.Add("minLocID", "MINLOCID");
            dgvUmsatz.Columns.Add("basUnix", "BASUNIX");
            dgvUmsatz.Columns.Add("bitUnix", "BITUNIX");


        }

        private void loadUmsatz()
        {
            try
            {
                using (myConn)
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();

                    }
                    string sql = "";                                    // 1                                       2                 3                       4                               5                       6                           7        8                                9                            10                                                      
                    sql = "SELECT FROM_UNIXTIME(MAX(`tarih`)) AS Day, FROM_UNIXTIME(MIN(`tarih`)) AS Daymin, count(*) AS total, MAX(satisanaid) AS maxBonNr, MIN(satisanaid) AS MinBonNr,  MIN(tarih) as Baslangic, MAX(tarih) AS endBonDatum, kasano, MAX(`localbonid`) As MAxLOCBonID, MIN(`localbonid`) As minLocBonID FROM satisana WHERE tarih>=" + tarih.gunBaslangic(dtp.Value.Day, dtp.Value.Month, dtp.Value.Year) + " AND tarih<=" + tarih.gunBitis(dtp2.Value.Day, dtp2.Value.Month, dtp2.Value.Year) + "  GROUP BY year(FROM_UNIXTIME(`tarih`)), month(FROM_UNIXTIME(`tarih`)),DAY(FROM_UNIXTIME(`tarih`)), kasano";
                    MySqlDataAdapter myDaSatis = new MySqlDataAdapter(sql, myConn);
                    DataTable dtSaatlik = new DataTable();
                    dtSaatlik.Rows.Clear();
                    myDaSatis.Fill(dtSaatlik);
                    dgvUmsatz.Rows.Clear();
                    if (dtSaatlik.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSaatlik.Rows.Count; i++)
                        {
                            dgvUmsatz.Rows.Add();
                            dgvUmsatz.Rows[i].Tag = dtSaatlik.Rows[i].ItemArray[5];
                            dgvUmsatz.Rows[i].Cells[0].Value = dtSaatlik.Rows[i].ItemArray[1]; //start bon tarih
                            dgvUmsatz.Rows[i].Cells[0].Tag = dtSaatlik.Rows[i].ItemArray[6];
                            dgvUmsatz.Rows[i].Cells[1].Value = dtSaatlik.Rows[i].ItemArray[4]; //start bon ID
                            dgvUmsatz.Rows[i].Cells[2].Value = dtSaatlik.Rows[i].ItemArray[0];//End Bon Datum
                            dgvUmsatz.Rows[i].Cells[3].Value = dtSaatlik.Rows[i].ItemArray[3];//END Bon ID
                            dgvUmsatz.Rows[i].Cells[4].Value = dtSaatlik.Rows[i].ItemArray[2];//Bon ANzahl
                            dgvUmsatz.Rows[i].Cells[5].Value = dtSaatlik.Rows[i].ItemArray[7];//Kasa no
                            dgvUmsatz.Rows[i].Cells[6].Value = dtSaatlik.Rows[i].ItemArray[8]; //max loc ID
                            dgvUmsatz.Rows[i].Cells[7].Value = dtSaatlik.Rows[i].ItemArray[9]; //Min loc ID
                            dgvUmsatz.Rows[i].Cells[8].Value = dtSaatlik.Rows[i].ItemArray[5]; //min bon date unix
                            dgvUmsatz.Rows[i].Cells[9].Value = dtSaatlik.Rows[i].ItemArray[6]; // max bon date unix
                          //  dgvUmsatz.Rows[i].Cells[10].Value = dtSaatlik.Rows[i].ItemArray[8]; //max sys bon ID
                           // dgvUmsatz.Rows[i].Cells[11].Value = dtSaatlik.Rows[i].ItemArray[9]; //Min sys bon ID
                        }
                    }

                }
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.Message);
            }
        }

        private void btnHandyAuflade_Click(object sender, EventArgs e)
        {
            ZBerichtReset();
        }

        private void ZBerichtReset()
        {
            int zBreichNo;
            try
            {
                if (txtLastZ.Text != "")
                {
                    if (int.TryParse(txtLastZ.Text, out zBreichNo))
                    {
                        using (myConn)
                        {
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();

                            }
                            // datum check
                            string SelSQL = "SELECT * FROM zbericht WHERE tarih>=" + tarih.gunBaslangic(dtp.Value.Day, dtp.Value.Month, dtp.Value.Year) + " AND tarih<=" + tarih.gunBaslangic(dtp2.Value.Day, dtp2.Value.Month, dtp2.Value.Year);
                            MySqlDataAdapter mydaSel = new MySqlDataAdapter(SelSQL, myConn);
                            DataTable dtSel = new DataTable();
                            dtSel.Rows.Clear();
                            mydaSel.Fill(dtSel);
                            if (dtSel.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtSel.Rows.Count; a++)
                                {
                                    string DelSQLpos = "DELETE FROM zberichtpos WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDelpos = new MySqlCommand(DelSQLpos, myConn);
                                    cmdDelpos.ExecuteNonQuery();

                                    string DelSQL = "DELETE FROM zbericht WHERE  zberichtno=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel = new MySqlCommand(DelSQL, myConn);
                                    cmdDel.ExecuteNonQuery();

                                    string DelSQL2 = "DELETE FROM businesscases WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel2 = new MySqlCommand(DelSQL2, myConn);
                                    cmdDel2.ExecuteNonQuery();

                                    string DelSQL3 = "DELETE FROM payment WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel3 = new MySqlCommand(DelSQL3, myConn);
                                    cmdDel3.ExecuteNonQuery();




                                }
                            }
                            foreach (DataGridViewRow row in dgvUmsatz.Rows)
                            {
                                DateTime dttime = tarih.KisatarihDateTime(Convert.ToInt32(row.Tag));
                                Double Datum = tarih.gunBaslangic(dttime.Day, dttime.Month, dttime.Year);
                                FisBarkodlu zKorrek = new FisBarkodlu();
                                zKorrek.XZ_Korrektur(Convert.ToInt32(Datum), zBreichNo, Convert.ToInt32(row.Cells[0].Tag));
                            }


                        }
                    }

                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }

        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            loadUmsatz();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FisBarkodlu zKorrek = new FisBarkodlu();
            zKorrek.zArchiveDruck(DateTime.Now);
        }
        private void ZBerichtReset2(int berNo)
        {
            int zBreichNo;
            try
            {
                if (txtLastZ.Text != "")
                {
                    if (int.TryParse(txtLastZ.Text, out zBreichNo))
                    {
                        using (myConn)
                        {
                            if (myConn.State == ConnectionState.Closed)
                            {
                                myConn.Open();

                            }
                            // datum check
                            string SelSQL = "SELECT * FROM zbericht WHERE zberichtno>" + berNo;
                            MySqlDataAdapter mydaSel = new MySqlDataAdapter(SelSQL, myConn);
                            DataTable dtSel = new DataTable();
                            dtSel.Rows.Clear();
                            mydaSel.Fill(dtSel);
                            if (dtSel.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtSel.Rows.Count; a++)
                                {
                                    string DelSQLpos = "DELETE FROM zberichtpos WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDelpos = new MySqlCommand(DelSQLpos, myConn);
                                    cmdDelpos.ExecuteNonQuery();

                                    string DelSQL = "DELETE FROM zbericht WHERE  zberichtno=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel = new MySqlCommand(DelSQL, myConn);
                                    cmdDel.ExecuteNonQuery();

                                    string DelSQL2 = "DELETE FROM businesscases WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel2 = new MySqlCommand(DelSQL2, myConn);
                                    cmdDel2.ExecuteNonQuery();

                                    string DelSQL3 = "DELETE FROM payment WHERE  znr=" + dtSel.Rows[a].ItemArray[2];
                                    MySqlCommand cmdDel3 = new MySqlCommand(DelSQL3, myConn);
                                    cmdDel3.ExecuteNonQuery();




                                }
                            }
                            


                        }
                    }

                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }

        }

        private void btnZErstellen_Click(object sender, EventArgs e)
        {
            string hataligunler = "";
            
            int berNo = 0;
            berNo = Convert.ToInt16(txtLastZ.Text);
            ZBerichtReset2(berNo);
            foreach (DataGridViewRow row in dgvUmsatz.Rows)
            {
                tar.Tarih tar = new tar.Tarih();
                DateTime dttime = tarih.KisatarihDateTime(Convert.ToInt32(row.Tag));
                Double Datumbas = tarih.gunBaslangic(dttime.Day, dttime.Month, dttime.Year);
                //DateTime sonAbschlussDatum; 
                    //int sonKasa = -1;

                Int32 gunBaslangic = 0, gunBitis = 0, ogunTarih;
                if (Int32.TryParse(row.Tag.ToString(), out ogunTarih))
                {
                    gunBaslangic = tar.gunBaslangicUnix(ogunTarih);
                    gunBitis = tar.gunBitisUnixTimeStamp(ogunTarih);

                }
                else
                {
                    hataligunler += row.Cells[0].ToString() + " KN:" + row.Cells[5] + "\n";
                    continue;
                }


                int kasano = Convert.ToInt32(row.Cells[5].Value.ToString());
                int a = 0;
                double yuzde19lukmiktar = 0, yuzde7likmiktar = 0, yuzde0likmiktar = 0, toplamtutar = 0, toplamBar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, toplam0brut = 0, ec7tutar = 0, ec19tutar = 0, ec0tutar = 0,
                  storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0, storno0tutar = 0, scheckmiktar = 0, scheck7tutar = 0, scheck19tutar = 0, scheck0tutar = 0, KombiBar = 0, KombiEC = 0, KombiScheck = 0,
                  KombiBar0 = 0, KombiBar7 = 0, KombiBar19 = 0, KombiEc0 = 0, KombiEc7 = 0, KombiEc19 = 0, KombiCek0 = 0, KombiCek7 = 0, KombiCek19 = 0, toplam19Yazildi = 0, Gutschein = 0, RabatCouponTutar = 0, RabatTutar = 0,
                  totalAnfangBestand = 0;
                int printDruck = 0, barAnzahl = 0, ecAnzahl = 0, stornoAnzahl = 0, scheckAnzahl = 0, bonAnzahl = 0, kombiBonAnzahl = 0, kombiBarAnzahl = 0, kombiECAnzahl = 0, kombiScheckAnzahl = 0;
                double totalGeldEntnahme = 0;
                double totalGeldEinlage = 0;
                double kassenbuchbarBetrag = 0;
                int kundenzahl = 0;
                int ZprintFlaglastInsertId = 0;
                Int64 maxBonnr = 0, minBonNr = 0;
                //Check Tagliche Verkauf
                string ZCHECK1 = "SELECT COUNT(*) FROM satisana WHERE  kasano=" + kasano + " AND tarih>" + gunBaslangic + " AND tarih<" + gunBitis;
                MySqlDataAdapter daZCHECK1 = new MySqlDataAdapter(ZCHECK1, myConn);
                DataTable dtZCHECK1 = new DataTable();
                dtZCHECK1.Rows.Clear();
                daZCHECK1.Fill(dtZCHECK1);
                if (Convert.ToInt16(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                {

                    db baglan = new db();
                    MySqlConnection myConn1 = new MySqlConnection();
                    myConn1 = baglan.myconn();
                    if (myConn1.State == ConnectionState.Closed)
                    {
                        baglan.openConnection();
                        if (myConn1.State == ConnectionState.Closed)
                        {
                            myConn1.Open();
                        }

                    }
                    MySqlTransaction mytrans = null;
                    string KasseHerstNr = "", kassename = "";

                    //KasseHerstNr = dtKasaNr.Rows[k].ItemArray[5].ToString();
                    kassename = "KASSE " + kasano ;
                    string ZCHECK12 = "SELECT COUNT(*) FROM satisana WHERE kasano=" + kasano + " AND tarih>" + gunBaslangic + " AND tarih<" + gunBitis; ;
                    MySqlDataAdapter daZCHECK12 = new MySqlDataAdapter(ZCHECK12, myConn);
                    DataTable dtZCHECK12 = new DataTable();
                    dtZCHECK12.Rows.Clear();
                    daZCHECK12.Fill(dtZCHECK12);
                    if (Convert.ToInt16(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                    {
                        using (myConn1 = baglanti.myconn())
                        {

                            if (myConn1.State == ConnectionState.Closed)
                            {
                                myConn1.Open();
                            }
                            mytrans = myConn1.BeginTransaction();
                            /* string sqltarih1 = "SELECT * FROM zbericht WHERE erstelldatum=0";
                             MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn1);
                             DataTable dttar1 = new DataTable();
                             dttar1.Rows.Clear();
                             datar1.Fill(dttar1);
                             //long berNo=0;
                             if (dttar1.Rows.Count > 0)
                             {
                                 berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                             }
                             else
                             {*/
                            berNo = berNo + 1;
                           // MySqlCommand coZ1 = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht
                            MySqlCommand coZ1 = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) VALUES("+berNo+"," + Convert.ToInt32(row.Tag) + "," + kasano + ")", myConn1);
                            coZ1.Transaction = mytrans;
                            coZ1.ExecuteNonQuery();
                            string sqltarih11 = "SELECT * FROM zbericht WHERE id=" + coZ1.LastInsertedId;
                            MySqlDataAdapter datar11 = new MySqlDataAdapter(sqltarih11, myConn1);
                            DataTable dttar11 = new DataTable();
                            dttar11.Rows.Clear();
                            datar11.Fill(dttar11);
                            berNo = Convert.ToInt16(dttar11.Rows[0].ItemArray[2]);
                            // }
                            Random rnd = new Random();
                            Int32 erstellDatum = Convert.ToInt32(row.Cells[9].Value.ToString()) + rnd.Next(0, 30) * 60;
                            string ZPro = "INSERT INTO `zdruckprotokol`( `zberichtno`, `datum`, `flag`, medium, user) VALUES (" + berNo + "," + erstellDatum + ",2,'KASSE " + kasano + "','" + Program.bedAdSoyad + "')";
                            MySqlCommand cmdZpro = new MySqlCommand(ZPro, myConn1);
                            cmdZpro.Transaction = mytrans;
                            ZprintFlaglastInsertId = cmdZpro.ExecuteNonQuery();





                            int mwst = -1;
                            /*double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                            double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, storno = 0, stornomiktar = 0;
                            double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                            double RabatTutar = 0;
                            double RabatCouponTutar = 0;
                            double Gutschein = 0;*/

                            // Zpos insert
                            string ZposSQL = "INSERT INTO `zberichtpos`(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`,`mwst`, `grupname`) VALUES ";
                            /*INNER İLE LEFTi değiştir UNUTMA*/
                            string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, satisdetay.mwst, max(localbonid) as MaxBonNr, min(localbonid) as MinBonNr FROM satisdetay" +
                            " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid  LEFT JOIN satisana ON satisana.satisanaid=satisdetay.fisno" +
                            " WHERE  satisdetay.tarih> " + gunBaslangic + " AND satisdetay.tarih<" + gunBitis +
                            " AND satisdetay.kasano=" + kasano + "  GROUP BY  satisdetay.mwst, artikelgrup.grupid ORDER BY satisdetay.mwst ASC, grupad ASC";
                            MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn1);

                            DataTable dtSatisSayisi = new DataTable("satisdetay");
                            dtSatisSayisi.Rows.Clear();
                            daSatisSayisi.Fill(dtSatisSayisi);
                            if (dtSatisSayisi.Rows.Count > 0)
                            {
                                maxBonnr = Convert.ToInt64(row.Cells[6].Value)!=0? Convert.ToInt64(row.Cells[6].Value): Convert.ToInt64(row.Cells[3].Value);
                                minBonNr = Convert.ToInt64(row.Cells[7].Value)!=0? Convert.ToInt64(row.Cells[7].Value): Convert.ToInt64(row.Cells[1].Value);




                                while (a < dtSatisSayisi.Rows.Count)
                                {


                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 43)
                                    {
                                        RabatCouponTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 7)
                                    {
                                        RabatTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 6)
                                    {
                                        Gutschein += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        mwst = Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]);

                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }





                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }

                                    }
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        ZposSQL += "(NULL," + berNo + "," + Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) + "," + vA.virgulayikla(Convert.ToDouble(dtSatisSayisi.Rows[a].ItemArray[0])) + "," + kasano + ",'" + "KASSE " + kasano + "'," + mwst + ",'" + dtSatisSayisi.Rows[a].ItemArray[1].ToString() + "'),";
                                    }
                                    a++;
                                }
                                if (RabatTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",7," + vA.virgulayikla(RabatTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Rabatt'),";
                                }
                                if (RabatCouponTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",43," + vA.virgulayikla(RabatCouponTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'RabattCoupon'),";
                                }
                                if (Gutschein < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",6," + vA.virgulayikla(Gutschein) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Gutschein'),";
                                }


                                try
                                {
                                    if (myConn1.State == ConnectionState.Closed)
                                        myConn1.Open();
                                    ZposSQL = ZposSQL.Substring(0, ZposSQL.Length - 1);
                                    MySqlCommand cmdZPos = new MySqlCommand(ZposSQL, myConn1);
                                    cmdZPos.Transaction = mytrans;
                                    cmdZPos.ExecuteNonQuery();

                                }
                                catch (Exception rr)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "ln:885" + rr.Message + "\n\n" + rr.StackTrace;
                                    frmerror.ShowDialog();
                                    mytrans.Rollback();
                                    return;

                                }

                                //GRUP BILGISI SONU

                                string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                                   "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                                   "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay " +
                                   ", SUM(`toplamEc`), SUM(toplamStorno), SUM(toplamScheck) FROM satisana WHERE " +
                                    "  satisana.kasano=" + kasano + " AND satisana.tarih> " + gunBaslangic + " AND satisana.tarih<" + gunBitis + " GROUP BY odemeturu ORDER BY odemeturu ASC";
                                MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn1);
                                DataTable dtSatisAna = new DataTable("satisana");
                                dtSatisAna.Rows.Clear();
                                daSatisAna.Fill(dtSatisAna);

                                int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                                string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 " +
                                    " AND satisana.kasano=" + kasano + " AND satisana.tarih> " + gunBaslangic + " AND satisana.tarih<" + gunBitis + "  GROUP BY satisdetay.mwst";
                                MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn1);
                                DataTable dtKombi = new DataTable();
                                daKombi.Fill(dtKombi);
                                int CombiBonAnzahl = dtKombi.Rows.Count;
                                string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay," +
                                    " SUM(CASE WHEN `toplamEc` > 0 THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND " +
                                    "  satisana.tarih> " + gunBaslangic + " AND satisana.tarih<" + gunBitis + " AND satisana.kasano=" + kasano;
                                MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn1);
                                DataTable dtKombiSumme = new DataTable();
                                daKombiSumme.Fill(dtKombiSumme);
                                if (dtKombi.Rows.Count > 0)
                                {
                                    double KombiTotal = 0;
                                    for (int c = 0; c < dtKombiSumme.Rows.Count; c++)
                                    {

                                        KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                        KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                        KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                        KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                        kombiBarAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]);
                                        kombiECAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]);
                                        kombiScheckAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]);
                                        KombiTotal = KombiBar + KombiEC + KombiScheck;

                                    }


                                }

                                for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                                {
                                    if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        scheckmiktar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiScheck);
                                        scheckAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                                        kombiScheckAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[6] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]) : 0;

                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            scheck7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiCek7;
                                            scheck19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiCek19;
                                            scheck0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiCek0;

                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        stornomiktar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]));
                                        stornoAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);



                                        storno7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]);
                                        storno19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                                        storno0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0
                                    {
                                        toplamBar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiBar);
                                        barAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                        kombiBarAnzahl = ((!(dtKombiSumme.Rows[0].ItemArray[4] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]) : 0);

                                        if (dtKombi.Rows.Count == 0)
                                        {

                                            toplam7brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiBar7;
                                            toplam19brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiBar19;
                                            toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar0;
                                            //line += aralik;
                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0
                                    {
                                        toplamEC = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiEC);
                                        ecAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                        kombiECAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[5] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]) : 0;
                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            ec7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiEc7;
                                            ec19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiEc19;
                                            ec0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEc0;
                                        }
                                        //toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);
                                        //line += aralik;

                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 4)
                                    {
                                        if ((toplamEC == 0) && (KombiEC != 0))
                                        {

                                            toplamEC = KombiEC;
                                            ec7tutar = KombiEc7;
                                            ec19tutar = KombiEc19;
                                            ecAnzahl = kombiECAnzahl;

                                        }
                                        if ((toplamBar == 0) && (KombiBar != 0))
                                        {

                                            toplamtutar = KombiBar;

                                            yuzde7likmiktar = KombiBar7;

                                            yuzde19lukmiktar = KombiBar19;

                                            yuzde0likmiktar = KombiBar0;
                                            barAnzahl = kombiBarAnzahl;

                                        }
                                        if ((scheckmiktar == 0) && (KombiScheck != 0))
                                        {
                                            scheckmiktar = KombiScheck;
                                            scheck7tutar = KombiCek7;
                                            scheck19tutar = KombiCek19;
                                            scheckAnzahl = kombiScheckAnzahl;

                                        }
                                    }
                                }
                                // MessageBox.Show(berNo.ToString());



                                /*totalAnfangBestand = AnfangBestandReturn();
                                totalGeldEinlage = GeldEinlageReturn();
                                totalGeldEntnahme = GeldEntnahmeReturn();*/
                                kassenbuchbarBetrag = toplamBar + stornomiktar + Gutschein + totalAnfangBestand + totalGeldEinlage - totalGeldEntnahme;
                                if (KombiSay > 0)
                                {


                                    kombiBonAnzahl = KombiSay;
                                }
                                if (myConn1.State == ConnectionState.Closed)
                                {
                                    myConn1.Open();
                                }

                                // Int32 minBonNr = 0, maxBonnr = 0;
                                kundenzahl = stornoAnzahl + ecAnzahl + barAnzahl + KombiSay;
                               
                                // `toplamScheck7Tutar`, `toplamScheck19Tutar`, `toplamScheck0Tutar`
                                string SqlZ = "";
                                /* SqlZ = "UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla(toplam19brut * 0.19 + toplam7brut * 0.07) + ",toplam7tutar=" + vA.virgulayikla(toplam7brut) +
                                   ", toplam19tutar=" + vA.virgulayikla(toplam19brut) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                                   ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                                   ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno19tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + startBonNr + ", endBonID=" + endBonNr +
                                   ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                                   ", anfangbestandsumme=" + totalAnfangBestand + " WHERE zberichtNo= " + berNo;*/
                                SqlZ = ("UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla((yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))) + (yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))) + ",toplam7tutar=" + vA.virgulayikla(yuzde7likmiktar) +
                              ", toplam19tutar=" + vA.virgulayikla(yuzde19lukmiktar) + ",toplam0Tutar=" + vA.virgulayikla(yuzde0likmiktar) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", bar7tutar=" + vA.virgulayikla(toplam7brut) + ", bar19tutar=" + vA.virgulayikla(toplam19brut) + ", bar0tutar=" + vA.virgulayikla(toplam0brut) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                              ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                              ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno0tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + minBonNr + ", endBonID=" + maxBonnr +
                              ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                              ", anfangbestandsumme=" + vA.virgulayikla(totalAnfangBestand) + ", totalgeldeinlage=" + vA.virgulayikla(totalGeldEinlage) + ", totalgeldentnahme=" + vA.virgulayikla(totalGeldEntnahme) + ", kassenbuchbarBetrag=" + vA.virgulayikla(kassenbuchbarBetrag) + " WHERE zberichtNo= " + berNo);

                                MySqlCommand coZ = new MySqlCommand(SqlZ, myConn1);
                                coZ.Transaction = mytrans;
                                if (coZ.ExecuteNonQuery() > 0)
                                {
                                    try
                                    {
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1 WHERE zberichtno=" + berNo;
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.Transaction = mytrans;
                                        cmdZproUp.ExecuteNonQuery();
                                        if (coZ.ExecuteNonQuery() > 0)
                                        {
                                            ZNummerEkle(berNo, kasano, gunBaslangic, gunBitis);
                                            //businesscases, Payment
                                            //DSFinK_Businesscases(berNo, yuzde19lukmiktar, yuzde7likmiktar, yuzde0likmiktar, Gutschein, RabatCouponTutar, RabatTutar, erstellDatum, toplamBar, toplamEC, scheckmiktar, stornomiktar, totalAnfangBestand, totalGeldEinlage, totalGeldEntnahme);

                                            if (myConn1.State == ConnectionState.Closed)
                                                myConn1.Open();
                                            string sorSQL = "SELECT id,datum FROM kassenbuch WHERE datum= " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND islemid=" + berNo;
                                            MySqlDataAdapter mydaSor = new MySqlDataAdapter(sorSQL, myConn1);
                                            DataTable dtSor = new DataTable();
                                            mydaSor.Fill(dtSor);
                                            if (dtSor.Rows.Count > 0)
                                            {
                                                //update
                                                string kassenbuchSQL = "UPDATE kassenbuch SET tutar=" + vA.virgulayikla(toplamBar + stornomiktar) + " WHERE id=" + dtSor.Rows[0].ItemArray[0]; ;
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                //insert
                                                string kassenbuchSQL = "INSERT INTO kassenbuch (datum, type, islemid, tutar, kaynak, aciklama, hedef, znr) VALUES (" + tar.unixdate(DateTime.Now) + ", 1 ," + berNo + "," + vA.virgulayikla(toplamBar + stornomiktar) + ", 'Z-Bericht', '" + "BERICHT-Nr:" + berNo.ToString() + "', 1," + berNo + ")";
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                        }
                                        mytrans.Commit();
                                    }
                                    catch (Exception dd)
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "ln:1103" + dd.Message + "\n\n" + dd.StackTrace;
                                        frmerror.ShowDialog();
                                        mytrans.Rollback();
                                        if (myConn1.State == ConnectionState.Closed)
                                            myConn1.Open();
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1";
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.ExecuteNonQuery();


                                        // MessageBox.Show("ln:1277\n"+dd.Message);
                                        //lblError.Text = "ln:1300" + dd.Message;
                                    }
                                }




                            }
                        }

                        if ((cbBondruck.Checked == true))
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.XZDruck(berNo);

                        }



                    }






                }
            }

        }
        private void ZNummerEkle(long berNo, int kasano, Int32 basTarih, Int32 bitTarih)
        {

            try
            {
                //if (Program.GlobalAyarlar["TaglichZ"] != 1)
                // {
                using (myConn = baglanti.myconn())
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    string znrSQL = "UPDATE satisana SET znr=" + berNo + " WHERE kasano=" + kasano + " AND tarih>="+basTarih +" AND tarih<="+bitTarih;
                    MySqlCommand cmdZnr = new MySqlCommand(znrSQL, myConn);
                    if (cmdZnr.ExecuteNonQuery() > 0)
                    {
                        try
                        {

                            //Update Bon_pos
                            string bonPosSQL = "UPDATE satisdetay SET znr=" + berNo + "  WHERE kasano=" + kasano + " AND tarih>="+basTarih +" AND tarih<="+bitTarih;
                            MySqlCommand cmdbonPosSQL = new MySqlCommand(bonPosSQL, myConn);
                            cmdbonPosSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //update Rabatt
                        try
                        {
                            string selectSQL = "SELECT * FROM rabatt INNER JOIN satisana ON rabatt.bonnr=satisanaid WHERE kassenr=" + kasano + " AND satisana.tarih>=" + basTarih + " AND satisana.tarih<=" + bitTarih;
                            MySqlDataAdapter myDASel = new MySqlDataAdapter(selectSQL, myConn);
                            DataTable dtSel = new DataTable();
                            dtSel.Rows.Clear();
                            myDASel.Fill(dtSel);
                            if (dtSel.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSel.Rows.Count; i++)
                                {
                                    string RabattSQL = "UPDATE rabatt SET znr=" + berNo + " WHERE bonnr=" + dtSel.Rows[i].ItemArray[1];
                                    MySqlCommand cmdRabattSQL = new MySqlCommand(RabattSQL, myConn);
                                    cmdRabattSQL.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Update Anfangbestand
                        try
                        {
                            if (Program.GlobalAyarlar["ANFANGBESTAND"] == 1)
                            {
                                string ABSQL = "UPDATE anfangbestand SET znr=" + berNo + "  WHERE kasseid=" + kasano + " AND datum>=" + basTarih + " AND datum<=" + bitTarih;
                                MySqlCommand cmdAB = new MySqlCommand(ABSQL, myConn);
                                cmdAB.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        // Abrechnungskreis
                        try
                        {
                            string AbrKrSQL = "UPDATE abrechnungskreis SET znr=" + berNo + "  WHERE kassenr=" + kasano + " AND datum>=" + basTarih + " AND datum<=" + bitTarih;
                            MySqlCommand cmdAbrKrSQL = new MySqlCommand(AbrKrSQL, myConn);
                            cmdAbrKrSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                        //Kassenbuch
                        try
                        {
                            string KasaKrSQL = "UPDATE kassenbuch SET znr=" + berNo + "   WHERE kassenr=" + kasano + " AND datum>=" + basTarih + " AND datum<=" + bitTarih;
                            MySqlCommand cmdKBSQL = new MySqlCommand(KasaKrSQL, myConn);
                            cmdKBSQL.ExecuteNonQuery();
                        }
                        catch (Exception ff)
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Bitte fotografieren Sie diese Fehlermeldung für eine Hersteller-Fehler-Analyse !\n" + ff.Message + "\n";
                            frmerror.ShowDialog();

                        }
                    }
                }

            }
            catch (Exception ss)
            {
            }
        }

        private void dtp2_ValueChanged(object sender, EventArgs e)
        {
            loadUmsatz();
        }

        private void btnZberichtNachNummer_Click(object sender, EventArgs e)
        {
            string hataligunler = "";

            int berNo = 0;
            berNo = Convert.ToInt16(txtLastZ.Text);
            //ZBerichtReset2(berNo);
            foreach (DataGridViewRow row in dgvUmsatz.Rows)
            {
                tar.Tarih tar = new tar.Tarih();
                DateTime dttime = tarih.KisatarihDateTime(Convert.ToInt32(row.Tag));
                Double Datumbas = tarih.gunBaslangic(dttime.Day, dttime.Month, dttime.Year);
                //DateTime sonAbschlussDatum; 
                //int sonKasa = -1;

                Int32 gunBaslangic = 0, gunBitis = 0, ogunTarih;
                if (Int32.TryParse(row.Tag.ToString(), out ogunTarih))
                {
                    gunBaslangic = tar.gunBaslangicUnix(ogunTarih);
                    gunBitis = tar.gunBitisUnixTimeStamp(ogunTarih);

                }
                else
                {
                    hataligunler += row.Cells[0].ToString() + " KN:" + row.Cells[5] + "\n";
                    continue;
                }


                int kasano = Convert.ToInt32(row.Cells[5].Value.ToString());
                int a = 0;
                double yuzde19lukmiktar = 0, yuzde7likmiktar = 0, yuzde0likmiktar = 0, toplamtutar = 0, toplamBar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, toplam0brut = 0, ec7tutar = 0, ec19tutar = 0, ec0tutar = 0,
                  storno = 0, stornomiktar = 0, storno7tutar = 0, storno19tutar = 0, storno0tutar = 0, scheckmiktar = 0, scheck7tutar = 0, scheck19tutar = 0, scheck0tutar = 0, KombiBar = 0, KombiEC = 0, KombiScheck = 0,
                  KombiBar0 = 0, KombiBar7 = 0, KombiBar19 = 0, KombiEc0 = 0, KombiEc7 = 0, KombiEc19 = 0, KombiCek0 = 0, KombiCek7 = 0, KombiCek19 = 0, toplam19Yazildi = 0, Gutschein = 0, RabatCouponTutar = 0, RabatTutar = 0,
                  totalAnfangBestand = 0;
                int printDruck = 0, barAnzahl = 0, ecAnzahl = 0, stornoAnzahl = 0, scheckAnzahl = 0, bonAnzahl = 0, kombiBonAnzahl = 0, kombiBarAnzahl = 0, kombiECAnzahl = 0, kombiScheckAnzahl = 0;
                double totalGeldEntnahme = 0;
                double totalGeldEinlage = 0;
                double kassenbuchbarBetrag = 0;
                int kundenzahl = 0;
                int ZprintFlaglastInsertId = 0;
                Int64 maxBonnr = 0, minBonNr = 0;
                //Check Tagliche Verkauf
                string ZCHECK1 = "SELECT COUNT(*) FROM satisana WHERE  znr="+berNo;
                MySqlDataAdapter daZCHECK1 = new MySqlDataAdapter(ZCHECK1, myConn);
                DataTable dtZCHECK1 = new DataTable();
                dtZCHECK1.Rows.Clear();
                daZCHECK1.Fill(dtZCHECK1);
                if (Convert.ToInt16(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                {

                    db baglan = new db();
                    MySqlConnection myConn1 = new MySqlConnection();
                    myConn1 = baglan.myconn();
                    if (myConn1.State == ConnectionState.Closed)
                    {
                        baglan.openConnection();
                        if (myConn1.State == ConnectionState.Closed)
                        {
                            myConn1.Open();
                        }

                    }
                    MySqlTransaction mytrans = null;
                    string KasseHerstNr = "", kassename = "";

                    //KasseHerstNr = dtKasaNr.Rows[k].ItemArray[5].ToString();
                    kassename = "KASSE " + kasano;
                    string ZCHECK12 = "SELECT COUNT(*) FROM satisana WHERE  znr=" + berNo;
                    MySqlDataAdapter daZCHECK12 = new MySqlDataAdapter(ZCHECK12, myConn);
                    DataTable dtZCHECK12 = new DataTable();
                    dtZCHECK12.Rows.Clear();
                    daZCHECK12.Fill(dtZCHECK12);
                    if (Convert.ToInt16(dtZCHECK1.Rows[0].ItemArray[0]) > 0)
                    {
                        using (myConn1 = baglanti.myconn())
                        {

                            if (myConn1.State == ConnectionState.Closed)
                            {
                                myConn1.Open();
                            }
                            mytrans = myConn1.BeginTransaction();
                            /* string sqltarih1 = "SELECT * FROM zbericht WHERE erstelldatum=0";
                             MySqlDataAdapter datar1 = new MySqlDataAdapter(sqltarih1, myConn1);
                             DataTable dttar1 = new DataTable();
                             dttar1.Rows.Clear();
                             datar1.Fill(dttar1);
                             //long berNo=0;
                             if (dttar1.Rows.Count > 0)
                             {
                                 berNo = Convert.ToInt16(dttar1.Rows[0].ItemArray[2]);
                             }
                             else
                             {*/
                            berNo = berNo + 1;
                            // MySqlCommand coZ1 = new MySqlCommand("INSERT into ZBericht (Zberichtno ,tarih, kasano ) SELECT COALESCE(MAX(Zberichtno),0)+1, " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + "," + Program.kasano + "  FROM zbericht
                           
                            // }
                            Random rnd = new Random();
                            Int32 erstellDatum = Convert.ToInt32(row.Cells[9].Value.ToString()) + rnd.Next(0, 30) * 60;
                            string ZPro = "INSERT INTO `zdruckprotokol`( `zberichtno`, `datum`, `flag`, medium, user) VALUES (" + berNo + "," + erstellDatum + ",2,'KASSE " + kasano + "','" + Program.bedAdSoyad + "')";
                            MySqlCommand cmdZpro = new MySqlCommand(ZPro, myConn1);
                            cmdZpro.Transaction = mytrans;
                            ZprintFlaglastInsertId = cmdZpro.ExecuteNonQuery();





                            int mwst = -1;
                            /*double yuzde7likmiktar = 0, yuzde0likmiktar = 0;
                            double yuzde19lukmiktar = 0, toplamtutar = 0, toplamEC = 0, toplam7brut = 0, toplam19brut = 0, storno = 0, stornomiktar = 0;
                            double KombiBar = 0, KombiEC = 0, KombiScheck = 0;
                            double RabatTutar = 0;
                            double RabatCouponTutar = 0;
                            double Gutschein = 0;*/

                            // Zpos insert
                            string ZposSQL = "INSERT INTO `zberichtpos`(`id`, `znr`, `grupid`, `betrag`, `kasseid`, `kassename`,`mwst`, `grupname`) VALUES ";
                            /*INNER İLE LEFTi değiştir UNUTMA*/
                            string gunSinirlari = "SELECT sum( satisdetay.toplamtutar ) , artikelgrup.grupad, satisdetay.grupid, satisdetay.mwst, max(localbonid) as MaxBonNr, min(localbonid) as MinBonNr FROM satisdetay" +
                            " LEFT JOIN artikelgrup ON satisdetay.grupid = artikelgrup.grupid  LEFT JOIN satisana ON satisana.satisanaid=satisdetay.fisno" +
                            " WHERE satisdetay.znr=" + berNo + "  GROUP BY  satisdetay.mwst, artikelgrup.grupid ORDER BY satisdetay.mwst ASC, grupad ASC";
                            MySqlDataAdapter daSatisSayisi = new MySqlDataAdapter(gunSinirlari, myConn1);

                            DataTable dtSatisSayisi = new DataTable("satisdetay");
                            dtSatisSayisi.Rows.Clear();
                            daSatisSayisi.Fill(dtSatisSayisi);
                            if (dtSatisSayisi.Rows.Count > 0)
                            {
                                maxBonnr = Convert.ToInt64(row.Cells[6].Value);
                                minBonNr = Convert.ToInt64(row.Cells[7].Value);




                                while (a < dtSatisSayisi.Rows.Count)
                                {


                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 43)
                                    {
                                        RabatCouponTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 7)
                                    {
                                        RabatTutar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) == 6)
                                    {
                                        Gutschein += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                    }
                                    else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) != mwst && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        mwst = Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]);

                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }





                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[1] : 7) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde7likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[2] : 19) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde19lukmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }
                                        else if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[3]) == (Program.MwStList.Count > 0 ? Program.MwStList[0] : 0) && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                        {
                                            yuzde0likmiktar += (double)dtSatisSayisi.Rows[a].ItemArray[0];
                                        }

                                    }
                                    if (Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 7 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 43 && Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) != 6)
                                    {

                                        ZposSQL += "(NULL," + berNo + "," + Convert.ToInt16(dtSatisSayisi.Rows[a].ItemArray[2]) + "," + vA.virgulayikla(Convert.ToDouble(dtSatisSayisi.Rows[a].ItemArray[0])) + "," + kasano + ",'" + "KASSE " + kasano + "'," + mwst + ",'" + dtSatisSayisi.Rows[a].ItemArray[1].ToString() + "'),";
                                    }
                                    a++;
                                }
                                if (RabatTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",7," + vA.virgulayikla(RabatTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Rabatt'),";
                                }
                                if (RabatCouponTutar < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",43," + vA.virgulayikla(RabatCouponTutar) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'RabattCoupon'),";
                                }
                                if (Gutschein < 0)
                                {

                                    ZposSQL += "(NULL," + berNo + ",6," + vA.virgulayikla(Gutschein) + "," + kasano + ",'KASSE " + kasano + "'," + mwst + ",'Gutschein'),";
                                }


                                try
                                {
                                    if (myConn1.State == ConnectionState.Closed)
                                        myConn1.Open();
                                    ZposSQL = ZposSQL.Substring(0, ZposSQL.Length - 1);
                                    MySqlCommand cmdZPos = new MySqlCommand(ZposSQL, myConn1);
                                    cmdZPos.Transaction = mytrans;
                                    cmdZPos.ExecuteNonQuery();

                                }
                                catch (Exception rr)
                                {
                                    F_GenericError frmerror = new F_GenericError();
                                    frmerror.lblMesaj.Text = "ln:885" + rr.Message + "\n\n" + rr.StackTrace;
                                    frmerror.ShowDialog();
                                    mytrans.Rollback();
                                    return;

                                }

                                //GRUP BILGISI SONU

                                string sqlSatisAna = "SELECT sum(`toplamBar`), sum(`toplammwst`),sum(`toplammwst7miktar`), sum(`toplammwst7tutar`), " +
                                   "SUM(`toplammwst19miktar`),sum(`toplammwst19tutar`), SUM(CASE WHEN `odemeturu` = 0  THEN 1 ELSE 0 END) AS KartliOdemeSay, " +
                                   "SUM(CASE WHEN `odemeturu` = 2  THEN 1 ELSE 0 END) AS stornoSay, odemeturu, SUM(CASE WHEN `odemeturu` = 1  THEN 1 ELSE 0 END) AS BarSay,sum(`toplammwst0tutar`),SUM(CASE WHEN `odemeturu` = 3 THEN 1 ELSE 0 END) AS CekSay " +
                                   ", SUM(`toplamEc`), SUM(toplamStorno), SUM(toplamScheck) FROM satisana WHERE  znr=" + berNo +
                                    "  GROUP BY odemeturu ORDER BY odemeturu ASC";
                                MySqlDataAdapter daSatisAna = new MySqlDataAdapter(sqlSatisAna, myConn1);
                                DataTable dtSatisAna = new DataTable("satisana");
                                dtSatisAna.Rows.Clear();
                                daSatisAna.Fill(dtSatisAna);

                                int barSay = 0, ecSay = 0, stornoSay = 0, scheckSay = 0, KombiSay = 0;
                                string sqlKombi = "SELECT satisdetay.mwst, SUM(satisdetay.toplamtutar), SUM(`toplamBar`) AS KBar,SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,count(*) FROM `satisana` INNER JOIN satisdetay ON satisana.`satisanaid`=satisdetay.fisno WHERE `odemeturu`=4 AND satisdetay.grupid<>6 AND satisdetay.grupid<>7 AND satisdetay.grupid<>43 " +
                                    "  znr=" + berNo +"  GROUP BY satisdetay.mwst";
                                MySqlDataAdapter daKombi = new MySqlDataAdapter(sqlKombi, myConn1);
                                DataTable dtKombi = new DataTable();
                                daKombi.Fill(dtKombi);
                                int CombiBonAnzahl = dtKombi.Rows.Count;
                                string SqlKombiSumme = "SELECT SUM(`toplamBar`) AS KBar, SUM(`toplamEc`)As KEC,SUM(`toplamScheck`) AS KCek,Count(*),SUM(CASE WHEN `toplamBar` > 0  THEN 1 ELSE 0 END) AS BarKombiSay," +
                                    " SUM(CASE WHEN `toplamEc` > 0 THEN 1 ELSE 0 END) AS BarEcSay, SUM(CASE WHEN `toplamScheck` > 0  THEN 1 ELSE 0 END) AS SheckKombiSay  FROM `satisana` WHERE `odemeturu`=4 AND " +
                                    " znr=" + berNo ;
                                MySqlDataAdapter daKombiSumme = new MySqlDataAdapter(SqlKombiSumme, myConn1);
                                DataTable dtKombiSumme = new DataTable();
                                daKombiSumme.Fill(dtKombiSumme);
                                if (dtKombi.Rows.Count > 0)
                                {
                                    double KombiTotal = 0;
                                    for (int c = 0; c < dtKombiSumme.Rows.Count; c++)
                                    {

                                        KombiBar = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[0]);
                                        KombiEC = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[1]);
                                        KombiScheck = Convert.ToDouble(dtKombiSumme.Rows[0].ItemArray[2]);
                                        KombiSay = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[3]);
                                        kombiBarAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]);
                                        kombiECAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]);
                                        kombiScheckAnzahl = Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]);
                                        KombiTotal = KombiBar + KombiEC + KombiScheck;

                                    }


                                }

                                for (int b = 0; b < dtSatisAna.Rows.Count; b++)
                                {
                                    if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 3) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        scheckmiktar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiScheck);
                                        scheckAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[11]);
                                        kombiScheckAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[6] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[6]) : 0;

                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            scheck7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiCek7;
                                            scheck19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiCek19;
                                            scheck0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiCek0;

                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 2) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]) != 0
                                    {
                                        stornomiktar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]));
                                        stornoAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[7]);



                                        storno7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]);
                                        storno19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]);
                                        storno0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);


                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 1) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]) != 0
                                    {
                                        toplamBar = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiBar);
                                        barAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[9]);
                                        kombiBarAnzahl = ((!(dtKombiSumme.Rows[0].ItemArray[4] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[4]) : 0);

                                        if (dtKombi.Rows.Count == 0)
                                        {

                                            toplam7brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiBar7;
                                            toplam19brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiBar19;
                                            toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiBar0;
                                            //line += aralik;
                                        }
                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 0) //&& Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]) != 0
                                    {
                                        toplamEC = (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + (Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10])) + KombiEC);
                                        ecAnzahl = Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[6]);
                                        kombiECAnzahl = (!(dtKombiSumme.Rows[0].ItemArray[5] is DBNull)) ? Convert.ToInt16(dtKombiSumme.Rows[0].ItemArray[5]) : 0;
                                        if (dtKombi.Rows.Count == 0)
                                        {
                                            ec7tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[3]) + KombiEc7;
                                            ec19tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[5]) + KombiEc19;
                                            ec0tutar = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]) + KombiEc0;
                                        }
                                        //toplam0brut = Convert.ToDouble(dtSatisAna.Rows[b].ItemArray[10]);
                                        //line += aralik;

                                    }
                                    else if (Convert.ToInt16(dtSatisAna.Rows[b].ItemArray[8]) == 4)
                                    {
                                        if ((toplamEC == 0) && (KombiEC != 0))
                                        {

                                            toplamEC = KombiEC;
                                            ec7tutar = KombiEc7;
                                            ec19tutar = KombiEc19;
                                            ecAnzahl = kombiECAnzahl;

                                        }
                                        if ((toplamBar == 0) && (KombiBar != 0))
                                        {

                                            toplamtutar = KombiBar;

                                            yuzde7likmiktar = KombiBar7;

                                            yuzde19lukmiktar = KombiBar19;

                                            yuzde0likmiktar = KombiBar0;
                                            barAnzahl = kombiBarAnzahl;

                                        }
                                        if ((scheckmiktar == 0) && (KombiScheck != 0))
                                        {
                                            scheckmiktar = KombiScheck;
                                            scheck7tutar = KombiCek7;
                                            scheck19tutar = KombiCek19;
                                            scheckAnzahl = kombiScheckAnzahl;

                                        }
                                    }
                                }
                                // MessageBox.Show(berNo.ToString());



                                /*totalAnfangBestand = AnfangBestandReturn();
                                totalGeldEinlage = GeldEinlageReturn();
                                totalGeldEntnahme = GeldEntnahmeReturn();*/
                                kassenbuchbarBetrag = toplamBar + stornomiktar + Gutschein + totalAnfangBestand + totalGeldEinlage - totalGeldEntnahme;
                                if (KombiSay > 0)
                                {


                                    kombiBonAnzahl = KombiSay;
                                }
                                if (myConn1.State == ConnectionState.Closed)
                                {
                                    myConn1.Open();
                                }

                                // Int32 minBonNr = 0, maxBonnr = 0;
                                kundenzahl = stornoAnzahl + ecAnzahl + barAnzahl + KombiSay;

                                // `toplamScheck7Tutar`, `toplamScheck19Tutar`, `toplamScheck0Tutar`
                                string SqlZ = "";
                                /* SqlZ = "UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla(toplam19brut * 0.19 + toplam7brut * 0.07) + ",toplam7tutar=" + vA.virgulayikla(toplam7brut) +
                                   ", toplam19tutar=" + vA.virgulayikla(toplam19brut) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                                   ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                                   ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno19tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + startBonNr + ", endBonID=" + endBonNr +
                                   ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                                   ", anfangbestandsumme=" + totalAnfangBestand + " WHERE zberichtNo= " + berNo;*/
                                SqlZ = ("UPDATE zbericht SET toplamtutar=" + vA.virgulayikla(yuzde7likmiktar + yuzde19lukmiktar + yuzde0likmiktar + RabatTutar + RabatCouponTutar + Gutschein) + ", toplammwst=" + vA.virgulayikla((yuzde19lukmiktar - yuzde19lukmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[2] / 100 : 19 / 100)))) + (yuzde7likmiktar - yuzde7likmiktar / ((double)1 + Convert.ToDouble((Program.MwStList.Count > 0 ? (double)Program.MwStList[1] / 100 : 7 / 100))))) + ",toplam7tutar=" + vA.virgulayikla(yuzde7likmiktar) +
                              ", toplam19tutar=" + vA.virgulayikla(yuzde19lukmiktar) + ",toplam0Tutar=" + vA.virgulayikla(yuzde0likmiktar) + ", toplamEC=" + vA.virgulayikla(toplamEC) + ", toplamBar=" + vA.virgulayikla(toplamBar) + ", bar7tutar=" + vA.virgulayikla(toplam7brut) + ", bar19tutar=" + vA.virgulayikla(toplam19brut) + ", bar0tutar=" + vA.virgulayikla(toplam0brut) + ", toplamScheck=" + vA.virgulayikla(scheckmiktar) + ", toplamScheck7Tutar=" + vA.virgulayikla(scheck7tutar) +
                              ", toplamScheck19Tutar=" + vA.virgulayikla(scheck19tutar) + ", toplamScheck0Tutar=" + vA.virgulayikla(scheck0tutar) + ", ec7tutar=" + vA.virgulayikla(ec7tutar) + ", ec19tutar=" + vA.virgulayikla(ec19tutar) + ", ec0tutar=" + vA.virgulayikla(ec0tutar) + ",toplamstorno=" + vA.virgulayikla(stornomiktar) +
                              ", storno7tutar=" + vA.virgulayikla(storno7tutar) + ", storno19tutar=" + vA.virgulayikla(storno19tutar) + " , `storno0tutar`=" + vA.virgulayikla(storno0tutar) + ", erstelldatum=" + erstellDatum + ", KasseHerstNr = '" + KasseHerstNr + "',kassename='" + kassename + "', druckAnzahl=druckAnzahl+1, startBonID=" + minBonNr + ", endBonID=" + maxBonnr +
                              ",`barAnzahl`=" + barAnzahl + ", `ecAnzahl`=" + ecAnzahl + ", `stornoAnzahl`=" + stornoAnzahl + ", `scheckAnzahl`=" + scheckAnzahl + ", `bonAnzahl`=" + kundenzahl + ", `kombiBonAnzahl`=" + kombiBonAnzahl + ", `kombiBarAnzahl`=" + kombiBarAnzahl + ", `kombiECAnzahl`=" + kombiECAnzahl + ", `kombiScheckAnzahl`=" + kombiScheckAnzahl +
                              ", anfangbestandsumme=" + vA.virgulayikla(totalAnfangBestand) + ", totalgeldeinlage=" + vA.virgulayikla(totalGeldEinlage) + ", totalgeldentnahme=" + vA.virgulayikla(totalGeldEntnahme) + ", kassenbuchbarBetrag=" + vA.virgulayikla(kassenbuchbarBetrag) + " WHERE zberichtNo= " + berNo);

                                MySqlCommand coZ = new MySqlCommand(SqlZ, myConn1);
                                coZ.Transaction = mytrans;
                                if (coZ.ExecuteNonQuery() > 0)
                                {
                                    try
                                    {
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1 WHERE zberichtno=" + berNo;
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.Transaction = mytrans;
                                        cmdZproUp.ExecuteNonQuery();
                                        if (coZ.ExecuteNonQuery() > 0)
                                        {
                                            ZNummerEkle(berNo, kasano, gunBaslangic, gunBitis);
                                            //businesscases, Payment
                                            //DSFinK_Businesscases(berNo, yuzde19lukmiktar, yuzde7likmiktar, yuzde0likmiktar, Gutschein, RabatCouponTutar, RabatTutar, erstellDatum, toplamBar, toplamEC, scheckmiktar, stornomiktar, totalAnfangBestand, totalGeldEinlage, totalGeldEntnahme);

                                            if (myConn1.State == ConnectionState.Closed)
                                                myConn1.Open();
                                            string sorSQL = "SELECT id,datum FROM kassenbuch WHERE datum= " + tar.gunBaslangic(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + " AND islemid=" + berNo;
                                            MySqlDataAdapter mydaSor = new MySqlDataAdapter(sorSQL, myConn1);
                                            DataTable dtSor = new DataTable();
                                            mydaSor.Fill(dtSor);
                                            if (dtSor.Rows.Count > 0)
                                            {
                                                //update
                                                string kassenbuchSQL = "UPDATE kassenbuch SET tutar=" + vA.virgulayikla(toplamBar + stornomiktar) + " WHERE id=" + dtSor.Rows[0].ItemArray[0]; ;
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                //insert
                                                string kassenbuchSQL = "INSERT INTO kassenbuch (datum, type, islemid, tutar, kaynak, aciklama, hedef, znr) VALUES (" + tar.unixdate(DateTime.Now) + ", 1 ," + berNo + "," + vA.virgulayikla(toplamBar + stornomiktar) + ", 'Z-Bericht', '" + "BERICHT-Nr:" + berNo.ToString() + "', 1," + berNo + ")";
                                                MySqlCommand cmdKassenbuch = new MySqlCommand(kassenbuchSQL, myConn1);
                                                cmdKassenbuch.Transaction = mytrans;
                                                cmdKassenbuch.ExecuteNonQuery();
                                            }
                                        }
                                        mytrans.Commit();
                                    }
                                    catch (Exception dd)
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "ln:1103" + dd.Message + "\n\n" + dd.StackTrace;
                                        frmerror.ShowDialog();
                                        mytrans.Rollback();
                                        if (myConn1.State == ConnectionState.Closed)
                                            myConn1.Open();
                                        string ZProUpdate = "UPDATE `zdruckprotokol` SET `flag`=1";
                                        MySqlCommand cmdZproUp = new MySqlCommand(ZProUpdate, myConn1);
                                        cmdZproUp.ExecuteNonQuery();


                                        // MessageBox.Show("ln:1277\n"+dd.Message);
                                        //lblError.Text = "ln:1300" + dd.Message;
                                    }
                                }




                            }
                        }

                        if ((cbBondruck.Checked == true))
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.XZDruck(berNo);

                        }



                    }






                }
            }
        }
    }
}
