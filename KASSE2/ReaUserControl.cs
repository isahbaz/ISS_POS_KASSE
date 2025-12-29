using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iss_Rea;
using System.Threading;
using System.IO;

namespace IS_KASSE
{
    public partial class ReaUserControl : UserControl
    {
        isstoRea Rea;
        string line = "", alteline = "";
        bool pr = true;
        Thread ECoku;
        List<string> dizi = new List<string>();
        public string gutscheinbrkd = "";
        int berNo = -1;
        public ReaUserControl()
        {
            InitializeComponent();
        }

        private void kryptonButton10_Click(object sender, EventArgs e)
        {
            if (Program.ProgramAyarlar["zvt"] == "ReaRetail" && Program.ReaGerateTyp == "INGENICO")
            {
                if (berNo == -1)
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Bitte drucken Sie zuerst Z-Bericht aus!\n Dann nochmal versuchen!";
                    frmerror.ShowDialog();
                }
            }
            Thread ReaKassenSchnitt = new Thread(new ThreadStart(ReaProc));
            ReaKassenSchnitt.Start();

            CheckForIllegalCrossThreadCalls = false;
            // label2.Text = "";
            // Rea.WriteInDatei();
            //timer1Tick();
            while (Rea.dllInUse)
            {
            }
            /*Thread.Sleep(200);
            while (!File.Exists((Application.StartupPath + "\\REAZVT.out")))
            {
            }*/
            string erg = "";

            while (erg == "")
            {
                erg = Rea.ZwischenInfo();
            }
            if (erg != "")
            {
                if (erg == "OK")
                {
                    // odemesonuc = true;
                    if (Program.zvt == "ReaRetail")
                    {
                        Thread.Sleep(200);
                        while (!File.Exists((Application.StartupPath + "\\REAZVT.tck")))
                        {
                        }
                        List<String> Ticket = KassenSchnittBelegVorberaiten();
                        if (Ticket.Count > 0)
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.KassenschnittDruck(Ticket,berNo);
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Kassenschnitt-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                            frmerror.ShowDialog();
                        }
                    }

                    //this.Close();

                }


            }
        }

        private void ReaProc()
        {
            Rea.dllInUse = true;
            Rea.Kassenschnitt();
        }
        private List<string> KassenSchnittBelegVorberaiten()
        {
            List<string> returnTicket = new List<string>();
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();

                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "A")
                        {

                            string[] inf = line.Split(';');
                            returnTicket.Add(inf[1]);

                        }


                    }
                    return returnTicket;
                }
            }
            catch
            {
                return returnTicket;
            }
            return returnTicket;
        }

        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            Rea.Redruck();
            CheckForIllegalCrossThreadCalls = false;
            // label2.Text = "";
            // Rea.WriteInDatei();
            //timer1Tick();
            Thread.Sleep(200);
            string erg = "";
            erg = Rea.ZwischenInfo();
           // label2.Text = erg;
            if (erg != "")
            {
                if (erg == "OK")
                {
                    // odemesonuc = true;
                    if (Program.zvt == "ReaRetail")
                    {
                        ReticketBelegVorberaiten();
                        if (Program.reticket.Count > 0)
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.ReticketDruck();
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Reticket-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                            frmerror.ShowDialog();
                        }
                    }

                    ((Form)this.TopLevelControl).Close();

                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = erg;
                    frmerror.ShowDialog();

                }


            }
        }

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
            Rea.Diag();
            CheckForIllegalCrossThreadCalls = false;
            // label2.Text = "";
            // Rea.WriteInDatei();
            //timer1Tick();
            Thread.Sleep(200);
            string erg = "";
            erg = Rea.ZwischenInfo();
            //label2.Text = erg;
            if (erg != "")
            {
                if (erg == "OK")
                {
                    // odemesonuc = true;
                    if (Program.zvt == "ReaRetail")
                    {
                        DiagBelegVorberaiten();
                        if (Program.diagTicket.Count > 0)
                        {
                            FisBarkodlu fisclass = new FisBarkodlu();
                            fisclass.DiagDruck();
                        }
                        else
                        {
                            F_GenericError frmerror = new F_GenericError();
                            frmerror.lblMesaj.Text = "Diag-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                            frmerror.ShowDialog();
                        }
                    }

                    ((Form)this.TopLevelControl).Close();

                }


            }
        }
        private void DiagBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "A")
                        {

                            string[] inf = line.Split(';');
                            Program.diagTicket.Add(inf[1]);

                        }


                    }
                }
            }
            catch
            {
            }
        }
        private void kryptonButton11_Click(object sender, EventArgs e)
        {
            string Belegno = "";
            using (F_RakamForm frakam = new F_RakamForm())
            {
                frakam.ShowDialog();
                if (frakam.islem == true)
                {
                    if (frakam.BelegNo != "")
                    {
                        Rea.Storno(frakam.BelegNo);
                        CheckForIllegalCrossThreadCalls = false;
                        // label2.Text = "";
                        // Rea.WriteInDatei();
                        //timer1Tick();
                        string erg = "";
                        erg = Rea.ZwischenInfo();
                        //label2.Text = erg;
                        if (erg != "")
                        {
                            if (erg == "OK")
                            {
                                // odemesonuc = true;
                                if (Program.zvt == "ReaRetail")
                                {
                                    StornoticketBelegVorberaiten();
                                    if (Program.stornoticketKunde.Count > 0)
                                    {
                                        FisBarkodlu fisclass = new FisBarkodlu();
                                        fisclass.StornoticketDruck();
                                    }
                                    else
                                    {
                                        F_GenericError frmerror = new F_GenericError();
                                        frmerror.lblMesaj.Text = "StornoBeleg-Info ist NULL! Bitte prüfen Sie .TCK Datei";
                                        frmerror.ShowDialog();
                                    }
                                }

                                //this.Close();
                                ((Form)this.TopLevelControl).Close();

                            }
                            else
                            {
                                F_GenericError frmerror = new F_GenericError();
                                frmerror.lblMesaj.Text = erg;
                                frmerror.ShowDialog();

                            }

                        }
                    }
                }
                else
                {
                    F_GenericError frmerror = new F_GenericError();
                    frmerror.lblMesaj.Text = "Beleg-Nr Fehlt!";
                    frmerror.ShowDialog();

                }
            }
        }
        private void StornoticketBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {
                        if (line.Substring(0, 1) == "C")
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.stornoticketKunde.Add(inf[1]);
                            }
                        }
                        else
                        {
                            if (line.Substring(1, 1) != "F")
                            {
                                string[] inf = line.Split(';');
                                Program.stornoticketHandler.Add(inf[1]);
                            }
                        }


                    }
                }
            }
            catch
            {
            }
        }
        private void ReticketBelegVorberaiten()
        {
            try
            {
                if (File.Exists("REAZVT.tck"))
                {
                    List<string> ticket = File.ReadLines("REAZVT.tck", System.Text.Encoding.Default).ToList();
                    /* Program.handlerbeleg = new List<string>();
                     Program.kundenbeleg = new List<string>();*/
                    string islem = "";
                    foreach (string line in ticket)
                    {


                        string[] inf = line.Split(';');
                        Program.reticket.Add(inf[1]);



                    }
                }
            }
            catch
            {
            }
        }

    }
}
