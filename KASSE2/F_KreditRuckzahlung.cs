using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;
using iss_Kunden;

namespace IS_KASSE
{
    public partial class F_KreditRuckzahlung : Form
    {
        Tarih tarih = new Tarih();
        MySqlConnection myConn = new MySqlConnection();
        double topBetrag = 0, verilenpara=0;
        VirgulAyikla vA = new VirgulAyikla();

        public long musterino = 0;
        public iss_Kunden.Musteri musteri;

        Dictionary<int, double> odenenler = new Dictionary<int, double>();
        Dictionary<int, double> TumKayitlar = new Dictionary<int, double>();
        db baglan = new db();
        public F_KreditRuckzahlung()
        {
            InitializeComponent();
        }

        private void F_KreditRuckzahlung_Load(object sender, EventArgs e)
        {
             
             this.flowLayoutPanel1.SuspendLayout();
            
             using (myConn = baglan.myconn())
             {
                 if (myConn.State == ConnectionState.Closed)
                 {
                     myConn.Open();
                 }
                 

                 string kreditListSQL = "SELECT * FROM kredit WHERE kundenid="+musterino+" AND durum=0 ORDER BY datum ASC LIMIT 0,5";
                 MySqlCommand cmdList = new MySqlCommand(kreditListSQL, myConn);
                 MySqlDataReader drList = cmdList.ExecuteReader();
                 while (drList.Read())
                 {
                     topBetrag += drList.GetDouble(3);
                     this.checkBox1 = new System.Windows.Forms.CheckBox();
                     
                     this.label1 = new System.Windows.Forms.Label();
                     this.panel1 = new System.Windows.Forms.Panel();
                    


                     this.checkBox1.AutoSize = true;
                     this.checkBox1.Font = new System.Drawing.Font("Verdana", 18.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                     this.checkBox1.Location = new System.Drawing.Point(15, 6);
                     this.checkBox1.Name = drList.GetInt16(0).ToString() ;
                     this.checkBox1.Size = new System.Drawing.Size(145, 27);
                     this.checkBox1.TabIndex = 0;
                     this.checkBox1.Text = tarih.tarih(drList.GetInt32(2));
                     this.checkBox1.UseVisualStyleBackColor = true;
                     this.checkBox1.Tag = drList.GetDouble(3);
                     this.checkBox1.CheckedChanged+=new EventHandler(checkBox1_CheckedChanged);
                     // 
                     // label1
                     // 
                    
                     this.label1.AutoSize = true;
                     this.label1.Font = new System.Drawing.Font("Verdana", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                     this.label1.Location = new System.Drawing.Point(12, 43);
                     this.label1.Name = "label1";
                     this.label1.Size = new System.Drawing.Size(84, 25);
                     this.label1.TabIndex = 1;
                     this.label1.Text = "BETRAG :"+drList.GetDouble(3).ToString("C");

                    
                     this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                     this.panel1.Controls.Add(this.label1);
                     this.panel1.Controls.Add(this.checkBox1);
                     this.panel1.Location = new System.Drawing.Point(3, 3);
                     this.panel1.Name = "panel1";
                     this.panel1.Size = new System.Drawing.Size(397, 91);
                     this.panel1.TabIndex = 0;

                     this.flowLayoutPanel1.Controls.Add(this.panel1);
                     TumKayitlar.Add(drList.GetInt16(0), drList.GetDouble(3));

                 }
                 this.checkBox1.ResumeLayout(false);
                 this.panel1.ResumeLayout(false);
                 this.panel1.PerformLayout();
                 this.flowLayoutPanel1.ResumeLayout(false);
                 lblTutar.Text = "TOTAL :" + topBetrag.ToString("C");
                 //drList.Close();
                 

             }
            
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox tiklanan= sender as CheckBox;
            if (tiklanan.Checked==true)
            {
                odenenler.Add(Convert.ToInt32(tiklanan.Name), Convert.ToDouble(tiklanan.Tag));
            }
            else
            {
                odenenler.Remove(Convert.ToInt32(tiklanan.Name));
            }

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnZws_Click(object sender, EventArgs e)
        {
            string odemeSQL = "", kundeSQL="";

            if (odenenler.Count > 0)
            {
                using (myConn = baglan.myconn())
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    foreach (KeyValuePair<Int32, double> aa in odenenler)
                    {
                        kundeSQL = "UPDATE kunden SET kredit=kredit-" + vA.virgulayikla(aa.Value) +" WHERE kundenid=" + musteri.MusteriId;
                        MySqlCommand cmdRuckZahl = new MySqlCommand(kundeSQL, myConn);
                        if (cmdRuckZahl.ExecuteNonQuery() > 0)
                        {
                            odemeSQL = "UPDATE kredit SET durum=1,  ruckzahlungdatum="+ tarih.unixdate(DateTime.Now) + " WHERE id=" + aa.Key;
                            MySqlCommand cmdOdeme = new MySqlCommand(odemeSQL, myConn);
                            cmdOdeme.ExecuteNonQuery();
                        }

                    }
                }
            }
            else
            {
                double kalan = 0;
                using (myConn = baglan.myconn())
                {
                    if (myConn.State == ConnectionState.Closed)
                    {
                        myConn.Open();
                    }
                    foreach (KeyValuePair<Int32, double> kayit in TumKayitlar)
                    {
                        if (verilenpara >= kayit.Value)
                        {


                            kundeSQL = "UPDATE kunden SET kredit=kredit-" + vA.virgulayikla(kayit.Value) + " WHERE kundenid=" + musteri.MusteriId;
                            MySqlCommand cmdRuckZahl = new MySqlCommand(kundeSQL, myConn);
                            if (cmdRuckZahl.ExecuteNonQuery() > 0)
                            {
                                odemeSQL = "UPDATE kredit SET durum=1, ruckzahlungdatum="+tarih.unixdate(DateTime.Now)+" WHERE id=" + kayit.Key;
                                MySqlCommand cmdOdeme = new MySqlCommand(odemeSQL, myConn);
                                cmdOdeme.ExecuteNonQuery();
                            }
                            kalan = verilenpara - kayit.Value;
                            if (kalan == 0)
                            {
                                break;
                            }
                            else
                            {
                                verilenpara = kalan;
                            }

                        }
                        else
                        {
                            kundeSQL = "UPDATE kunden SET kredit=kredit-" + vA.virgulayikla(verilenpara) + " WHERE kundenid=" + musteri.MusteriId;
                            MySqlCommand cmdRuckZahl = new MySqlCommand(kundeSQL, myConn);
                            if (cmdRuckZahl.ExecuteNonQuery() > 0)
                            {
                                odemeSQL = "UPDATE kredit SET betrag=betrag-" + vA.virgulayikla(verilenpara) + ", ruckzahlungdatum="+tarih.unixdate(DateTime.Now)+" WHERE id=" + kayit.Key;
                                MySqlCommand cmdOdeme = new MySqlCommand(odemeSQL, myConn);
                                cmdOdeme.ExecuteNonQuery();
                            }
                            break;

                        }

                    }

                }

            }
        }

        private void btnBir_Click(object sender, EventArgs e)
        {
            verilenpara = 0;
            if (txtOdenen.Text == "0")
            {
                txtOdenen.Text = "";
            }
            KryptonButton btn = sender as KryptonButton;
            txtOdenen.Text += btn.Text;

            if (!double.TryParse(txtOdenen.Text, out verilenpara))
            {
                F_GenericError frmerror = new F_GenericError();
                frmerror.lblMesaj.Text = "HATALI GIRIS YAPTINIZ!";
                frmerror.ShowDialog();
            }
        }
    }
}
