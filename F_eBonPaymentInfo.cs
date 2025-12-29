using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iss_ebon;

namespace IS_KASSE
{
    public partial class F_eBonPaymentInfo : Form
    {
        public string verifiedCode = "";
        public PaymentCreate payCreate;
        ebon_Main eBonMain = new ebon_Main();
        public int Zahlungsresult = -1;
        public F_eBonPaymentInfo()
        {
            InitializeComponent();
        }

        private void F_eBonPaymentInfo_Load(object sender, EventArgs e)
        {
            label1.Text="Mobile PAYMENT wurde gestartet!\nBitte Kundenhandy folgen!";
            timer1.Enabled = true;
            btnZahlungAbbrechen.Enabled = true;
            btnWiederholen.Enabled = false;
            btnFormSchliessen.Enabled = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            PaymentInfo PayInfo = new PaymentInfo();
            PayInfo = eBonMain.PaymentInfo(verifiedCode, Program.IsletmeAyarlar["eBonBearer"]);
            if (PayInfo.data.status == "0")
            {
                label1.Text = "Kunde wählt Zahlungsmethode ein! Bitte warten!";
                btnWiederholen.Enabled = false;
                btnFormSchliessen.Enabled = false;
                btnZahlungAbbrechen.Enabled = true;
                timer1.Enabled = true;
            }
            else if (PayInfo.data.status == "1")
            {
                label1.Text = "Zahlung Erfolgt!";
                Zahlungsresult = 1;

                this.Close();
            }
            else if (PayInfo.data.status == "2")
            {
                label1.Text = "Eine Fehler aufgetreten!";
                Zahlungsresult = 1;
                btnWiederholen.Enabled = true;
                btnFormSchliessen.Enabled = true;
                btnZahlungAbbrechen.Enabled = false;
                timer1.Enabled = true;
            }
            else if (PayInfo.data.status == "3")
            {
                label1.Text = "Bitte warten bis Ende der Zahlung!";
                btnWiederholen.Enabled = false;
                btnFormSchliessen.Enabled = false;
                btnZahlungAbbrechen.Enabled = true;
                timer1.Enabled = true;
            }
            else if (PayInfo.data.status == "4")
            {
                label1.Text = "Die Zahlung wurde durch der KASSE abgebrochen!";
                btnWiederholen.Enabled = true;
                btnFormSchliessen.Enabled = true;
                btnZahlungAbbrechen.Enabled = false;
                timer1.Enabled = true;
            }
            else if (PayInfo.data.status == "5")
            {
                label1.Text = "Die Zahlung wurde durch der KUNDE abgebrochen!";
                btnWiederholen.Enabled = true;
                btnFormSchliessen.Enabled = true;
                btnZahlungAbbrechen.Enabled = false;
                timer1.Enabled = true;
            }
        }

        private void btnZahlungAbbrechen_Click(object sender, EventArgs e)
        {
            PaymentInfo PayInfo = new PaymentInfo();
            PayInfo = eBonMain.PaymentCancel(verifiedCode, Program.IsletmeAyarlar["eBonBearer"]);
        }

        private void btnFormSchliessen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnWiederholen_Click(object sender, EventArgs e)
        {

        }
    }
}
