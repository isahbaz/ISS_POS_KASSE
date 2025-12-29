using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iss_HandyMopin;

namespace IS_KASSE
{
    public partial class F_HandAufladeReport : Form
    {
        public F_HandAufladeReport()
        {
            InitializeComponent();
        }

        private void F_HandAufladeReport_Load(object sender, EventArgs e)
        {
            dgvListe.Columns.Add("Datum", "DATUM");
            dgvListe.Columns.Add("cardname", "CARD NAME");

            dgvListe.Columns.Add("preis", "PREIS");
            dgvListe.Columns.Add("PIN", "PIN");
            dgvListe.Columns.Add("transid", "TRANS.ID");
            LoadCardVerkauf();
        }

        private void LoadCardVerkauf()
        {
            iss_HandyAuflade.iss_HandyAuflade_Main Handy = new iss_HandyAuflade.iss_HandyAuflade_Main();
            Handy.username = Program.IsletmeAyarlar["HandyAufladeUsername"];
            Handy.password = Program.IsletmeAyarlar["HandyAufladePassword"];
            List<string> Result = Handy.BalanceCheck();

            if (Result[0] == "OK")
            {
                lblGuthaben.Text= Result[1]+"€";

            }
            else
            {
                 lblGuthaben.Text= "Balance Error:" + (Result[1]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
