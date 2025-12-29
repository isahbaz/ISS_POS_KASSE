using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

namespace IS_KASSE
{
    public partial class MainForm : Form
    {
        MySqlConnection conn = new MySqlConnection();
        FisOlustur yeniFis;
        SatisYap satisYap;
        int position = 0;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int adet = 0;
            Artikel urun = new Artikel();
            if (txtBarkod.Text.Length<4)
            {
                int.TryParse(txtBarkod.Text, out adet);
            }

            if (yeniFis == null)
            {
               yeniFis = new FisOlustur();
                yeniFis.FisYarat(0);
               position = 1;
               listView1.Items.Clear();

            }
            Tarih tarih = new Tarih();
          //  MessageBox.Show("fisno=" + yeniFis.satisAnaId.ToString());
            urun.ArtikelBul(textBox1.Text);
            satisYap = new SatisYap();
            satisYap.Adet = adet;
            satisYap.Fisno = yeniFis.SatisAnaId;
            satisYap.KasaNo = Program.kasano;
            satisYap.Mwst = urun.Mwst;
            satisYap.Satisfiyat = urun.VkPreis;
            satisYap.Tarih = tarih.unixdate(DateTime.Now);
            satisYap.Toplamtutar = urun.VkPreis * adet;
            satisYap.UrunId = urun.ArtikelId;
            satisYap.Birimkar = (urun.VkPreis - urun.EkPreis) * adet;
            yeniFis.SatisKalem.Add(satisYap);
            int listViewElaman = listView1.Items.Count;
            listView1.Items.Add(position.ToString());
            listView1.Items[listViewElaman].SubItems.Add(urun.ArtikelAd.ToString());
            listView1.Items[listViewElaman].SubItems.Add(satisYap.Toplamtutar.ToString());

            position++;
           // satisYap.Kaydet();
           // MessageBox.Show(urun.ArtikelAd);


        } 

        private void MainForm_Load(object sender, EventArgs e)
        {
           

        }

        private void btnAraToplam_Click(object sender, EventArgs e)
        {
            if (yeniFis != null)
            {
                yeniFis.FisiKapat();
                lblSonuc.Text = yeniFis.toplamtutar.ToString("C");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (yeniFis != null)
            {
                yeniFis.verilenpara = 50.30;
                if (yeniFis.FisiSonlandır(1, yeniFis.SatisKalem) == true)
                {
                    yeniFis = null;
                    satisYap = null;
                    //System.GC.SuppressFinalize(yeniFis);

                    MessageBox.Show("İŞLEM TAMAM , FİİŞ YOK EDİLDİ");
                    listView1.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Bir Sorun VAr, Fiş duruyor, fisNO:" + yeniFis.SatisAnaId.ToString());
                }
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SatisYap satis = new SatisYap();
            int kaysay= yeniFis.SatisKalem.Count;
            foreach (SatisYap satilan in yeniFis.SatisKalem)
            {
                MessageBox.Show(satilan.Toplamtutar.ToString());
                
                }

            

            

            
           
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            Casio casioForm = new Casio();
            casioForm.ShowDialog();
        }

        private void kryptonColorButton1_SelectedColorChanged(object sender, ComponentFactory.Krypton.Toolkit.ColorEventArgs e)
        {

        }
    }
}
