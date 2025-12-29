using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using RC_V1;

namespace IS_KASSE
{
    public partial class F_LicenceError : Form
    {
        WebReq webreq= new WebReq();
        Tarih tarih = new Tarih();
        public MySqlConnection myCon = new MySqlConnection();
        public F_LicenceError()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MainRC bilgi = new MainRC();
            bilgi.Kod = Program.IsletmeAyarlar["kod"];
            bilgi.ip = Program.ServerIp;
            bilgi.Kod = Program.IsletmeAyarlar["kod"];
            bilgi.Isletme = Program.IsletmeAyarlar["isletme"];
            bilgi.Stadt = Program.IsletmeAyarlar["stadt"];
            bilgi.Tel1 = Program.IsletmeAyarlar["tel1"];
            bilgi.dbName = Program.dbName;
            bilgi.myConn = myCon;
            if (Program.issServer.Count > 0)
            {
                bilgi.url = Program.issServer[0];
            }
            else
            {
                MessageBox.Show("Remote Server wurden nicht identifiziert!");
                return;
            }
            string gelenmesaj=bilgi.Prufung();
            if (gelenmesaj != "OK")
            {
                label2.Text = gelenmesaj;
            }
            else
            {
                this.Close();
            }
        }
    }
}
