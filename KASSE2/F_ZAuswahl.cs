using iss_Datev;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_KASSE
{

    public partial class F_ZAuswahl : Form
    {
        public int islem = -1;
        MySqlConnection myConn = new MySqlConnection();
        public bool datevResult=false;
        public string DatevError = "";
        public F_ZAuswahl()
        {
            InitializeComponent();
        }

        private void btnXZ_Click(object sender, EventArgs e)
        {
            islem = 1;
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            islem = 2;
            this.Close();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            islem = -1;
            this.Close();
        }

        private void F_ZAuswahl_Load(object sender, EventArgs e)
        {
            lblDatevError.Text = "";
            if(Program.DatevConnection==true)
            {
                lblDatevStatus.Text = "Connected!";
                lblDatevStatus.ForeColor = Color.Green;
                if (datevResult == true)
                {
                    lblDatevInfo.Text = "DATEV DFÜ OK!\nDie Daten wurden erfolgreich an das Kassenarchiv online übertragen.";
                    lblDatevInfo.ForeColor = Color.Green;
                }
                else
                {
                    lblDatevInfo.Text = "DATEV DFÜ ERROR!";
                    lblDatevInfo.ForeColor = Color.Red;
                    lblDatevError.Text = DatevError;
                    lblDatevError.ForeColor = Color.Red;
                }
                //DatevUbertragung();
            }
            else
            {
                lblDatevStatus.Text = "Not Connected!";
                lblDatevStatus.ForeColor = Color.Red;
                lblDatevInfo.Text = "DATEV DFÜ ERROR!";
                lblDatevInfo.ForeColor = Color.Red;
                //lblDatevInfo.Visible = false;
            }
            YetkiCheck yetkiCheck = new YetkiCheck();
            yetkiCheck.YetkiKontrol("btnZErstellundAusdruck");
            if ((yetkiCheck.YetkiTanimlimi == true && yetkiCheck.Durum == 1) || (yetkiCheck.YetkiTanimlimi == false))
            {
                btnZErstellundAusdruck.Visible = true;
            }
            else
            {
                btnZErstellundAusdruck.Visible = false;
            }
        }

        private void DatevUbertragung()
        {
            db baglanti = new db();
            myConn = baglanti.myconn();
            try
            {
                string readRefTokenSql = "";
                readRefTokenSql = "SELECT refreshtoken from datev_info order by id DESC";
                MySqlDataAdapter myDaRef = new MySqlDataAdapter(readRefTokenSql, myConn);
                DataTable dtRef = new DataTable();
                myDaRef.Fill(dtRef);
                if (dtRef.Rows.Count > 0 && dtRef.Rows[0].ItemArray[0].ToString() != "")
                {
                    //label13.Text = "DATEV Tenand-ID:" + dtTenand.Rows[0].ItemArray[0].ToString();
                    //refresh Token
                    iss_Datev_Main datevMain = new iss_Datev_Main();
                    // datevMain.Token("Localhost", frmConn.Code, codeVerifier, "");
                    DatevToken datevToken = new DatevToken();
                    datevToken = datevMain.RefreshToken(dtRef.Rows[0].ItemArray[0].ToString(), "refresh_token");
                    if (datevToken != null)
                    {

                        /*  AccessTokenVar = datevToken.access_token;
                          refreshTokenVar = datevToken.refresh_token;
                          GetTenand(AccessTokenVar, refreshTokenVar, datevToken.id_token);*/

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception dd)
            {

            }

        }
    }
}
