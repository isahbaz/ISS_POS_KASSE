using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tar;

namespace IS_KASSE
{
    public partial class F_Zarchive : Form
    {
        public MySqlConnection myConn = new MySqlConnection();
        tar.Tarih tarih = new tar.Tarih();
        public F_Zarchive()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHandyAuflade_Click(object sender, EventArgs e)
        {
            FisBarkodlu fisclass = new FisBarkodlu();
            fisclass.zArchiveDruck(mc1.SelectionStart.Date);
            this.Close();
        }

        private void F_Zarchive_Load(object sender, EventArgs e)
        {
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
            string ZArsiveSQL = "SELECT * FROM Zbericht WHERE erstelldatum<>0 AND erstelldatum<>-1 AND erstelldatum <= NOW() - INTERVAL 7 DAY";
            MySqlDataAdapter myDaArsive = new MySqlDataAdapter(ZArsiveSQL, myConn);
            DataTable dtArsive = new DataTable();
            myDaArsive.Fill(dtArsive);
            if (dtArsive.Rows.Count > 0)
            {
                if (dgvZArsive.Rows.Count > 0)
                    dgvZArsive.Rows.Clear();
                for (int i = 0; i < dtArsive.Rows.Count; i++)
                {
                    dgvZArsive.Rows.Add();
                    dgvZArsive.Rows[i].Cells[0].Value = dtArsive.Rows[i].ItemArray[2];
                    dgvZArsive.Rows[i].Cells[1].Value = tarih.tarih(Convert.ToInt32(dtArsive.Rows[i].ItemArray[24]));
                    dgvZArsive.Rows[i].Cells[2].Value = dtArsive.Rows[i].ItemArray[26];
                    dgvZArsive.Rows[i].Cells[3].Value = String.Format("{0:0.00}", dtArsive.Rows[i].ItemArray[3]);
                }
            }
        }
    }
}
