using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//using tar;
namespace IS_KASSE
{
    public partial class F_Neuste : Form
    {
        Tarih tarih = new Tarih();
        public F_Neuste()
        {
            InitializeComponent();
        }

        private void F_Neuste_Load(object sender, EventArgs e)
        {

        }

        private void btnAbmelden_Click(object sender, EventArgs e)
        {
            
                F_GenericSoru frmSilmeOnay = new F_GenericSoru();
                frmSilmeOnay.lblMesaj.Text = Program.lang["12"];
                frmSilmeOnay.ShowDialog();
                if (frmSilmeOnay.sonuc == true)
                {
                    MySqlConnection conn = new MySqlConnection();
                    db baglanti = new db();
                    conn = baglanti.myconn();
                    // conn.Open();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();


                    }
                    string userTakipSql = "UPDATE usertakip SET offlinezaman=" + tarih.unixdate(DateTime.Now) + " WHERE userid=" + Program.bedID + " and tarih=" + tarih.bugunBaslangic() + " and  onlinezaman<>0 and offlinezaman=0 and kasaid=" + Program.kasano;
                    MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                    coUserTakip.ExecuteNonQuery();
                    Application.Restart();
                }
        }

        private void btnPausieren_Click(object sender, EventArgs e)
        {
             F_GenericSoru frmSilmeOnay = new F_GenericSoru();
                frmSilmeOnay.lblMesaj.Text = Program.lang["12"];
                frmSilmeOnay.ShowDialog();
                if (frmSilmeOnay.sonuc == true)
                {
                    MySqlConnection conn = new MySqlConnection();
                    db baglanti = new db();
                    conn = baglanti.myconn();
                    // conn.Open();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();


                    }
                    string userTakipSql = "UPDATE usertakip SET offlinezaman=" + tarih.unixdate(DateTime.Now) + " WHERE userid=" + Program.bedID + " and tarih=" + tarih.bugunBaslangic() + " and  onlinezaman<>0 and offlinezaman=0 and kasaid=" + Program.kasano;
                    MySqlCommand coUserTakip = new MySqlCommand(userTakipSql, conn);
                    coUserTakip.ExecuteNonQuery();

                    System.Diagnostics.Process.Start("shutdown", "-r -f -t 0");
                }
        }

        private void btnAbbruch_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
