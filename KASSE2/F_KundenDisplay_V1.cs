using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace IS_KASSE
{
    public partial class F_KundenDisplay_V1 : Form
    {
        public string artikelad, Menge, Preis, total;
        int ZahlungInfo = 0;
        F_KundenDisplayKurz frmInfo;
        F_HandAufladeDAOnay HandyDa;
        int time = 0;
        db baglanti = new db();
        string videoName = "";
        System.Timers.Timer timer2;
        WMPLib.IWMPPlaylist playlist;
        WMPLib.IWMPMedia media;

        public F_KundenDisplay_V1()
        {

            InitializeComponent();
            axWindowsMediaPlayer1.Visible = false;
           // MessageBox.Show("V1 Start");
            if (Screen.AllScreens.Length > 1)
            {
                try
                {
                    int sonuc = 0, kalan = 0;
                    double bolum = 0;
                    bolum = Math.Floor(485 / 765.0);
                    sonuc = Convert.ToInt16(Math.Floor((485 / 765.0) * Convert.ToDouble(Screen.AllScreens[1].WorkingArea.Width)));
                }
                catch (Exception dd)
                {
                    MessageBox.Show("KundenDisplay V1-Const-42"+dd.Message);
                }
                int widht = 0;
                if (!Screen.AllScreens[0].Primary)
                {
                    widht = Screen.AllScreens[0].WorkingArea.Width - 10;
                }
                else
                {
                    widht = Screen.AllScreens[1].WorkingArea.Width - 10;
                }
               // MessageBox.Show(widht.ToString());
                listView1.Width = widht - 10;
                listView1.Columns[0].Width = Convert.ToInt16(Math.Floor((400 / 765.0) * widht - 10));
                listView1.Columns[1].Width = Convert.ToInt16(Math.Floor((125 / 765.0) * widht - 10));
                listView1.Columns[2].Width = Convert.ToInt16((145 / 765.0) * widht - 10);
                listView1.Columns[3].Width = Convert.ToInt16((125 / 765.0) * widht - 10);
                if (listView1.Items.Count > 0)
                {
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }
            }
            
                //System.Windows.SystemParameters.PrimaryScreenWidth
            if (File.Exists(Application.StartupPath + "\\LogoKD.jpg"))
            {
                Image img = Image.FromFile(Application.StartupPath + "\\LogoKD.jpg");
                panel2.BackgroundImage = img;
            }
            this.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
            timer2 = new System.Timers.Timer(1000);
            timer2.Enabled = false;
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Tick);
            timer2.Start();
            
        }
        public void VerkaufInfo(string artikelad, String Menge, string Preis, string total, string Bontotal, int islem, double verilenPara, double paraUstu, int OdemeTur )
        {
            try
            {
                if (Program.GlobalAyarlar["Video"] == 1)
                {
                    time = 0;
                    timer2.Start();
                    if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        axWindowsMediaPlayer1.close();
                        axWindowsMediaPlayer1.Visible = false;
                    }
                }
               // CheckForIllegalCrossThreadCalls = false;
                int ListViewCount = 0;
                ListViewCount = listView1.Items.Count;
                if (islem == 0)
                {
                    if (frmInfo != null)
                        frmInfo.Close();
                    listView1.Items.Add(artikelad);
                    listView1.Items[ListViewCount].BackColor = ListViewCount % 2 == 0 ? Color.Azure : Color.Beige;
                    ///listView1.Items[ListViewCount].Text = artikelad;
                    listView1.Items[ListViewCount].SubItems.Add(Menge);
                    listView1.Items[ListViewCount].SubItems.Add(Preis);
                    listView1.Items[ListViewCount].SubItems.Add(total);
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    }
                    lblNeuTotal.Text= lblTotal.Text = Bontotal;
                    
                }
                else if (islem == 1)
                {
                    
                    listView1.Items.Add(artikelad);
                    listView1.Items[ListViewCount].Font = new Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    listView1.Items[ListViewCount].BackColor =  Color.White;
                    listView1.Items[ListViewCount].ForeColor = Color.Red;
                    ///listView1.Items[ListViewCount].Text = artikelad;
                    listView1.Items[ListViewCount].SubItems.Add(Menge);
                    listView1.Items[ListViewCount].SubItems.Add(Preis);
                    listView1.Items[ListViewCount].SubItems.Add(total);
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    }
                    lblTotal.Text = Bontotal;
                    lblNeuTotal.Text  = Bontotal;
                }
                else if (islem == 2)
                {
                    frmInfo = new F_KundenDisplayKurz();
                   
                    //frmInfo.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
                    //frmInfo.BringToFront();
                    listView1.Items.Clear();
                    frmInfo.lblgegeben.Text = verilenPara.ToString("C");
                    frmInfo.lblRuckgeld.Text = paraUstu.ToString("C");
                    frmInfo.lblZuZahlen.Text = Bontotal;
                    if (OdemeTur == 0)
                    {
                        frmInfo.lblZahlungsTyp.Text = "EC Karte/KK Karte";
                    }
                    else
                    {
                        frmInfo.lblZahlungsTyp.Text = "BAR ZAHLUNG";
                    }
                    frmInfo.Show();
                    return;
                    //lblTotal.Text = ergeb.ToString();
                    //listView1.Items.Clear();
                }
                else if (islem == 3)
                {
                    listView1.Items.Clear();
                    lblTotal.Text = Bontotal;
                    lblNeuTotal.Text = "";
                }
                else if (islem == 4)
                {
                    if (frmInfo != null)
                        frmInfo.Close();
                    panel1.Visible = true;
                    timer1.Enabled = true;
                    label1.Text = "Pausiert!";
                }
                else if (islem == 5)
                {
                    if (frmInfo != null)
                        frmInfo.Close();
                    panel1.Visible = true;
                    timer1.Enabled = true;
                    label1.Text = "Kasse\nGeschlossen!";
                }
                else if (islem == 6)
                {
                    panel1.Visible = false;
                    timer1.Enabled = false;
                    label1.Text = "";
                }
                else if (islem == 8)
                {
                    lblTotal.Text = total+" "+ Bontotal;
                }
                else if (islem == 9)
                {
                    listView1.Items.Clear();
                    lblTotal.Text = "";
                }
                else if (islem == 7)
                {
                    lblTotal.Font = new Font("Impact", 20, FontStyle.Bold);
                    lblTotal.Text = "" + Bontotal;
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.label1.Text = "SOFTWARE_ID:";
                    frmerror.lblMesaj.Text = Bontotal;
                    frmerror.ShowDialog();
                    lblTotal.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    
                    //lblTotal.Text = Bontotal;
                }
                if (islem == 10)
                {
                    if (frmInfo != null)
                        frmInfo.Close();
                   
                    lblTotal.Text = Bontotal;
                }
                if (islem == 11) //Direckt Auflade
                {
                    if (OdemeTur != 3)
                    {

                        if (HandyDa == null)
                        {
                            HandyDa = new F_HandAufladeDAOnay();
                            HandyDa.Show();
                        }

                        HandyDa.TelNrYaz(artikelad, OdemeTur);
                    }
                    else
                    {
                        if (HandyDa != null)
                        {
                            HandyDa.Dispose();
                            HandyDa = null;
                        }
                    }
                   
                  
                }
                if (islem == 12) //reset
                {
                    if (frmInfo != null)
                        frmInfo.Close();
                    listView1.Items.Add(artikelad);
                    listView1.Items[ListViewCount].BackColor = ListViewCount % 2 == 0 ? Color.Azure : Color.Beige;
                    ///listView1.Items[ListViewCount].Text = artikelad;
                    listView1.Items[ListViewCount].SubItems.Add(Menge);
                    listView1.Items[ListViewCount].SubItems.Add(Preis);
                    listView1.Items[ListViewCount].SubItems.Add(total);
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    }
                    lblTotal.Text = Bontotal;
                }
            }
            catch(Exception eee)
            {
                MessageBox.Show("KundenDisplayV1-Load "+eee.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F_KundenDisplayKurz tt = new F_KundenDisplayKurz();
            tt.Location=Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
            tt.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.GlobalAyarlar["Video"] == 1)
                {
                    time++;
                    if (time == 30)
                    {
                       axWindowsMediaPlayer1.BringToFront();
                        timer2.Stop();
                        //axWindowsMediaPlayer1.playlistCollection= pl
                        // axWindowsMediaPlayer1.URL = Application.StartupPath + "\\" + "Wildlife.wmv";
                        axWindowsMediaPlayer1.currentPlaylist = playlist;
                        axWindowsMediaPlayer1.settings.mute = true;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        axWindowsMediaPlayer1.stretchToFit = true;
                        axWindowsMediaPlayer1.settings.setMode("Loop", true);
                        axWindowsMediaPlayer1.Visible = true;

                        //videoName = "Wildlife.wmv";

                    }/**/
                }
            }
            catch(Exception dd)
            {
                MessageBox.Show(dd.Message);
            }
        }

        private void F_KundenDisplay_V1_Load(object sender, EventArgs e)
        {
            
            MySqlConnection myConn;
            int defaultCount = 0;
            string apppath = Application.StartupPath;
            
            if (Program.GlobalAyarlar["Video"] == 1)
            {

                using (myConn = baglanti.myconn())
                {
                    try
                    {
                        MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT * FROM displayvideo ", myConn);
                        DataTable dtUserBul = new DataTable("user");
                        dtUserBul.Rows.Clear();
                        daUserBul.Fill(dtUserBul);
                        if (dtUserBul.Rows.Count > 0)
                        {
                            playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist"); 
                            
                            for (int i = 0; i < dtUserBul.Rows.Count; i++)
                            {
                                if ((int)dtUserBul.Rows[i].ItemArray[3] == 1)
                                {
                                    defaultCount++;
                                    media = axWindowsMediaPlayer1.newMedia(apppath + "\\" + dtUserBul.Rows[i].ItemArray[2]);
                                    playlist.appendItem(media);
                                }
                            }
                            //videoName = dtUserBul.Rows[0].ItemArray[1].ToString();
                            timer2.Enabled = true;
                        }
                    }
                    catch (Exception aa)
                    {
                        MessageBox.Show(aa.Message);
                    }

                }
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }

    }
}
