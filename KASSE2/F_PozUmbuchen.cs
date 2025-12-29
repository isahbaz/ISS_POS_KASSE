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
using System.Threading;

namespace IS_KASSE
{
    public partial class F_PozUmbuchen : Form
    {
        public FisOlustur fis = null;
        double toplamtutar = 0;
        public bool TeilOdemeVarmi = false;
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        public Dictionary<int, FisOlustur> MevcutMasalar = new Dictionary<int, FisOlustur>();
        FisOlustur SecilenMasa = null;
        KryptonButton btnSecilenMasa = null;

        public delegate void KundenDisplayDelagate(string urunad, string adet, string satisfiyat, string postoplam, string toplamtutar, int islemtur, double verilenPara, double paraUstu, int odemeTur);
        public event KundenDisplayDelagate KundenDisplayFisInhaltEvent;

        public F_PozUmbuchen()
        {
            InitializeComponent();
        }

        private void dgvPoz_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(dgvPoz.Rows[e.RowIndex].Cells[0].Value) == false)
            {
                dgvPoz.Rows[e.RowIndex].Cells[0].Value = true;
                toplamtutar += Convert.ToDouble(dgvPoz.Rows[e.RowIndex].Cells[2].Value);


            }
            else
            {
                dgvPoz.Rows[e.RowIndex].Cells[0].Value = false;
                toplamtutar -= Convert.ToDouble(dgvPoz.Rows[e.RowIndex].Cells[2].Value);
            }
        }

        private void F_PozUmbuchen_Load(object sender, EventArgs e)
        {
            this.KundenDisplayFisInhaltEvent("", "", "", "", "", 9,0,0,0);
            KryptonDataGridViewCheckBoxColumn cbCol = new KryptonDataGridViewCheckBoxColumn();
            cbCol.Width = 50;
            //cbCol.HeaderText = "SEÇ";
            dgvPoz.Columns.Add(cbCol);
            cbCol.ThreeState = false;
            dgvPoz.Columns.Add("artikel", "ARTIKEL NAME");
            dgvPoz.Columns[1].Width = 400;
            dgvPoz.Columns[1].ReadOnly = true;
            dgvPoz.Columns.Add("preis", "PREIS");
            dgvPoz.Columns[2].Width = 100;
            dgvPoz.Columns[2].ReadOnly = true;
            Thread satisyukle = new Thread(new ThreadStart(SatisKalemyukle));
            satisyukle.Start();
            //Thread masayukle = new Thread(new ThreadStart(MasaYukle));
           // masayukle.Start();
            //SatisKalemyukle();
            MasaYukle();
        }
        private void SatisKalemyukle()
        {
            for (int i = 0; i < fis.SatisKalem.Count; i++)
            {
                dgvPoz.Rows.Add();
                dgvPoz.Rows[i].Cells[1].Value = fis.SatisKalem[i].UrunAd;
                dgvPoz.Rows[i].Cells[2].Value = fis.SatisKalem[i].Toplamtutar.ToString("#0.00");
                dgvPoz.Rows[i].Height = 40;
            }
        }

        private void MasaYukle()
        {
            foreach (FisOlustur masa in MevcutMasalar.Values)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnMasa = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                btnMasa.Location = new System.Drawing.Point(571, 319);
                btnMasa.Name = "btnMasa";
                btnMasa.Size = new System.Drawing.Size(91, 54);
                btnMasa.StateCommon.Back.Color1 = System.Drawing.Color.Beige;
                btnMasa.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
                btnMasa.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                btnMasa.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                btnMasa.StateCommon.Border.Rounding = 1;
                btnMasa.StateCommon.Border.Width = 2;
                btnMasa.TabIndex = 60;
                btnMasa.Values.Text = masa.Masano;
                btnMasa.Tag = masa;
                btnMasa.Click += new System.EventHandler(this.btnMasa_Click);
                flp1.Controls.Add(btnMasa);
            }
        }
        private void btnMasa_Click(object sender, EventArgs e)
        {
            this.KundenDisplayFisInhaltEvent("", "", "", "", "", 9,0,0,0);
            btnSecilenMasa = sender as KryptonButton;
            btnSecilenMasa.StateCommon.Border.Color1 = System.Drawing.Color.Green;
            btnSecilenMasa.StateNormal.Border.Color1 = System.Drawing.Color.Green;
            btnSecilenMasa.StatePressed.Border.Color1 = System.Drawing.Color.Green;

            SecilenMasa = (FisOlustur)btnSecilenMasa.Tag;
            foreach(KryptonButton btn in flp1.Controls)
            {
                if (btn.Tag != btnSecilenMasa.Tag)
                {
                    btnSecilenMasa.StateCommon.Border.Color1 = System.Drawing.Color.Red;
                    btnSecilenMasa.StateNormal.Border.Color1 = System.Drawing.Color.Red;
                }
            }
            foreach (SatisYap satislar in SecilenMasa.SatisKalem)
            {
                string urunad = satislar.UrunAd.ToString().IndexOf('\n') != -1 ? satislar.UrunAd.Substring(0, satislar.UrunAd.ToString().IndexOf('\n')) : satislar.UrunAd.ToString();
                
                this.KundenDisplayFisInhaltEvent(satislar.UrunAd, satislar.Adet.ToString("#0.00"), satislar.Satisfiyat.ToString("C"), satislar.Toplamtutar.ToString("C"), "TOTAL :" + fis.toplamtutar.ToString("C"), 0,0,0,0);
                
            }

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBar_Click(object sender, EventArgs e)
        {
            Tarih tarih = new Tarih();
            SatisYap satisYap = null;
            

            
            List<int> silinenIDler = new List<int>();
            List<SatisYap> KalanSatislar = new List<SatisYap>();
            for (int i = 0; i < dgvPoz.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvPoz.Rows[i].Cells[0].Value) == true)
                {
                    try
                    {
                        SatisYap TeilPoz = new SatisYap();
                        TeilPoz = fis.SatisKalem[i];

                        //SecilenMasa.SatisKalem.Add(satisYap);
                        TeilPoz.dbMasaIslemUpdate(SecilenMasa, TeilPoz);
                        this.KundenDisplayFisInhaltEvent(TeilPoz.UrunAd, TeilPoz.Adet.ToString("#0.00"), TeilPoz.Satisfiyat.ToString("C"), TeilPoz.Toplamtutar.ToString("C"), "TOTAL :" + fis.toplamtutar.ToString("C"), 0,0,0,0);

                        SecilenMasa.FisiKapat();
                        silinenIDler.Add(i);
                    }
                    catch (Exception ff)
                    {
                        F_GenericError frmerror = new F_GenericError();
                        frmerror.lblMesaj.Text = ff.Message;
                        frmerror.ShowDialog();
                    }
                    //fis.SatisKalem.RemoveAt(i);
                    //dgvPoz.Rows.RemoveAt(i);
                }
                else
                {
                    KalanSatislar.Add(fis.SatisKalem[i]);
                }
            }
            fis.SatisKalem = KalanSatislar;
            fis.FisiKapat();
            dgvPoz.Rows.Clear();
            SatisKalemyukle();
            this.Close();
            
        }
    }
}
