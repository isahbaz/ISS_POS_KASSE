using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Net;
using System.Globalization;



namespace IS_KASSE
{
    public partial class F_ServerOff : Form
    {
        Thread ts;
        public bool sonuc = false;
        public F_ServerOff()
        {
            InitializeComponent();
        }

        private void F_ServerOff_Load(object sender, EventArgs e)
        {
            serverAc();
        }

        private void serverAc()
        {
            timer1.Enabled = true;
            timer1.Interval = 1000;
            lblMesaj.Text = "DER DATENBANK SERVER WIRD GEÖFFNET ! BITTE WARTEN!";
            ts = new Thread(new ThreadStart(AcmaPaketiYolla));
            ts.Start();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        int counter = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            label2.Text = "(Durch. Wartezeit) =" + (180 - counter).ToString() + " ";
            counter++;
            if (counter >80) //orj 100
            {
                MySqlConnection conn = new MySqlConnection();
                db baglanti = new db();
                conn = baglanti.myconn();
                // conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    try
                    {
                        //conn.ConnectionTimeout = 5;
                        baglanti.openConnection();

                        timer1.Stop();
                        ts.Abort();
                        sonuc = true;
                        this.Close();
                    }
                    catch (Exception ee)
                    {
                        string mesaj = ee.Message;
                    }
                }
            }

            if (counter > 180)
            {
                sonuc = false;
            }

        }

        public void AcmaPaketiYolla()
        {
            /*UZEYIR2 KOD:10*9/
            string MAC_ADDRESS = "00219B367C7E";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
            
            //now use this class
            //MAC_ADDRESS should  look like '013FA049'
            /* HUZUR  KOD 30*9/
            
                string MAC_ADDRESS = "0019990B5483";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
            /* Ay MARKET BONN*9/
                string MAC_ADDRESS = "0023AE5A4116";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);


                /* mustafa Grevenbroich 55 *9/

                string MAC_ADDRESS = "00219B7FDEDA";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
            

            //now use this class
            //MAC_ADDRESS should  look like '013FA049'
            /* AKKAUF KOD 20 *0/
            
                string MAC_ADDRESS = "0023AE5A5892";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);

                /* PASAM GMBH KOD 74 *9/

                string MAC_ADDRESS = "C8CBB8129150";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
           
            /* AKDENIZ KOD 40 *9/
             
                string MAC_ADDRESS = "0019993B959E";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
             
            /* }
        
            YIGIT KOD 60 
             *9/
                string MAC_ADDRESS = "001AA0B8C3DD";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
             /*ALEM KOD 70 *9/
             
                string MAC_ADDRESS = "002564BCF5E3";
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
	  
                int reterned_value = client.Send(bytes, 1024);
              //*/
            /*ALEM WUPPERTAL KOD 71 *9/
             
               string MAC_ADDRESS = "00219B842EC8";
               WOLClass client = new WOLClass();
               client.Connect(new
                  IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                  0x2fff); // port=12287 let's use this one 
               client.SetClientToBrodcastMode();
               //set sending bites
               int counter = 0;
               //buffer to be send
               byte[] bytes = new byte[1024];   // more than enough :-)
               //first 6 bytes should be 0xFF
               for (int y = 0; y < 6; y++)
                   bytes[counter++] = 0xFF;
               //now repeate MAC 16 times
               for (int y = 0; y < 16; y++)
               {
                   int i = 0;
                   for (int z = 0; z < 6; z++)
                   {
                       bytes[counter++] =
                           byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                       i += 2;
                   }
               }

               //now send wake up packet
	  
               int reterned_value = client.Send(bytes, 1024);
             //*/
            /* SPAR NEUWIED  KOD 90*9/

               string MAC_ADDRESS = "B8AC6F27EFB7";
               WOLClass client = new WOLClass();
               client.Connect(new
                  IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                  0x2fff); // port=12287 let's use this one 
               client.SetClientToBrodcastMode();
               //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

              /* TOP SONDERPOSTEN  KOD 84*/
            try
            {
                string MAC_ADDRESS = Program.MacID;
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
            }
            catch
            {
            }
            /* ATLAS SPAR  KOD 88*9/

            string MAC_ADDRESS = "08626631294C";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
                /* GULBERAL  KOD 64*9/

            string MAC_ADDRESS = "0023AE61174A";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

            /* HELAL LOKMA DÜREN  KOD 95*9/

            string MAC_ADDRESS = "0023AE59570F";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

            /*9HALK PAZARI HURTH 15 
             *9/
            string MAC_ADDRESS = "448A5BA52A89";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

           /* ALİ KIOSK 20 
                *9/
            string MAC_ADDRESS = "0023AEA2D835";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
            /*guler supermarkt 39 
                   *9/
            string MAC_ADDRESS = "BC305B9BC15B";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
              /*anadolu  41
                   *9/
            string MAC_ADDRESS = "00219B5AA0B7";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }
               /*anadolu Lev supermarkt 41
                   *9/
              string MAC_ADDRESS = "00219B5AA0B7";
              WOLClass client = new WOLClass();
              client.Connect(new
           IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
           0x2fff); // port=12287 let's use this one 
              client.SetClientToBrodcastMode();
              //set sending bites
              int counter = 0;
              //buffer to be send
              byte[] bytes = new byte[1024];   // more than enough :-)
              //first 6 bytes should be 0xFF
              for (int y = 0; y < 6; y++)
            bytes[counter++] = 0xFF;
              //now repeate MAC 16 times
              for (int y = 0; y < 16; y++)
              {
            int i = 0;
            for (int z = 0; z < 6; z++)
            {
                bytes[counter++] =
              byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                i += 2;
            }
              }
              
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
              //*9//*ACIMA KÖLN 25
            string MAC_ADDRESS = "00219B3884C2";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);


            /*DAMLA DUSSELDORF 25
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
             //*9/
            string MAC_ADDRESS = "BC305BCA9A49";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

            /*DAMLA KÖLN  47
          //now send wake up packet
          int reterned_value = client.Send(bytes, 1024);
            //*9/
            string MAC_ADDRESS = "F04DA225D4EE";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
                   
                    
              //
            /*DAMLA KÖLN  62
      //now send wake up packet
      int reterned_value = client.Send(bytes, 1024);
      //*9/
            string MAC_ADDRESS = "00219B508800";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);


                  //
      /*
       * /*DAMLA KÖLN  47
          //now send wake up packet
          int reterned_value = client.Send(bytes, 1024);
            //*9/
            string MAC_ADDRESS = "BC305BCA9D1F";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);


            //
            /*
              //FATISSA TROISDORF 85/
                  string MAC_ADDRESS = "00219B7AD7D2";
                  WOLClass client = new WOLClass();
                  client.Connect(new
                     IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                     0x2fff); // port=12287 let's use this one 
                  client.SetClientToBrodcastMode();
                  //set sending bites
                  int counter = 0;
                  //buffer to be send
                  byte[] bytes = new byte[1024];   // more than enough :-)
                  //first 6 bytes should be 0xFF
                  for (int y = 0; y < 6; y++)
                      bytes[counter++] = 0xFF;
                  //now repeate MAC 16 times
                  for (int y = 0; y < 16; y++)
                  {
                      int i = 0;
                      for (int z = 0; z < 6; z++)
                      {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                      }
                  }

                  //now send wake up packet
                  int reterned_value = client.Send(bytes, 1024);
              /* *
               //RIWA UMIT GELSENKIRSCHEN 35
                   //now send wake up packet
                   int reterned_value = client.Send(bytes, 1024);
                  *)/
                   string MAC_ADDRESS = "00219B500556";
                   WOLClass client = new WOLClass();
                   client.Connect(new
                IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                0x2fff); // port=12287 let's use this one 
                   client.SetClientToBrodcastMode();
                   //set sending bites
                   int counter = 0;
                   //buffer to be send
                   byte[] bytes = new byte[1024];   // more than enough :-)
                   //first 6 bytes should be 0xFF
                   for (int y = 0; y < 6; y++)
                 bytes[counter++] = 0xFF;
                   //now repeate MAC 16 times
                   for (int y = 0; y < 16; y++)
                   {
                 int i = 0;
                 for (int z = 0; z < 6; z++)
                 {
                     bytes[counter++] =
                   byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                     i += 2;
                 }
                   }

                   //now send wake up packet
                   int reterned_value = client.Send(bytes, 1024); /*
                  //AlTAT KOBLENZ 43
                   //now send wake up packet
                   int reterned_value = client.Send(bytes, 1024);
                  *9/
            string MAC_ADDRESS = "F04DA225D4C5";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

             //KAYA CENTER  83
                   //now send wake up packet
                   int reterned_value = client.Send(bytes, 1024);
                  *9/
            string MAC_ADDRESS = "D8CB8A363F69";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
            /*  //Istanbul halk Pazari ---54
             //now send wake up packet
              //KAradeniz Market  58
             //now send wake up packet
	        
		    
            string MAC_ADDRESS = "00199969A49A";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
              //Istanbul halk Pazari ---54
             //now send wake up packet
	         
            /*
            /*
            string MAC_ADDRESS = "000802A8F24F";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
              bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
         //Ekobay Sankt Augustin 45
             //now send wake up packet
           
         *9/
      string MAC_ADDRESS = "0019995C961F";
      WOLClass client = new WOLClass();
      client.Connect(new
   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
   0x2fff); // port=12287 let's use this one 
      client.SetClientToBrodcastMode();
      //set sending bites
      int counter = 0;
      //buffer to be send
      byte[] bytes = new byte[1024];   // more than enough :-)
      //first 6 bytes should be 0xFF
      for (int y = 0; y < 6; y++)
          bytes[counter++] = 0xFF;
      //now repeate MAC 16 times
      for (int y = 0; y < 16; y++)
      {
          int i = 0;
          for (int z = 0; z < 6; z++)
          {
              bytes[counter++] =
            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
              i += 2;
          }
      }

      //now send wake up packet
      int reterned_value = client.Send(bytes, 1024);
      *9/
      //SIDE METTMAN 80
     //now send wake up packet
           
       
     string MAC_ADDRESS = "001E4FC0DC90";
     WOLClass client = new WOLClass();
     client.Connect(new
        IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
        0x2fff); // port=12287 let's use this one 
     client.SetClientToBrodcastMode();
     //set sending bites
     int counter = 0;
     //buffer to be send
     byte[] bytes = new byte[1024];   // more than enough :-)
     //first 6 bytes should be 0xFF
     for (int y = 0; y < 6; y++)
         bytes[counter++] = 0xFF;
     //now repeate MAC 16 times
     for (int y = 0; y < 16; y++)
     {
         int i = 0;
         for (int z = 0; z < 6; z++)
         {
       bytes[counter++] =
           byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
       i += 2;
         }
     }
     //now send wake up packet
     int reterned_value = client.Send(bytes, 1024);
      */
            /*KUDRET EUSKIRCHEN 69 *9/
           string MAC_ADDRESS = "002564968C8A";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);

           /*KUDRET EUSKIRCHEN 69 *9/
           string MAC_ADDRESS = "0024E83D7D3C";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
           /*OCAK BONN 79 /
           string MAC_ADDRESS = "0024E83CAD95";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
           /*KELEBEK MARKET DUIS 72*9/
           string MAC_ADDRESS = "002564C25D9A";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);

           /*KELEBEK MARKET DUIS 72*9/
           string MAC_ADDRESS = "0024E83D7D3C";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);

           /*IBO FEINKOST DUIS 81*)/
           string MAC_ADDRESS = "001921493835";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
           /*DENIZ SUPERMARKT GE 82*9/
           string MAC_ADDRESS = "0025649ED953";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
                   bytes[counter++] =
                       byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                   i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
            /*AVM REİNHAUSEN 20*9/
           string MAC_ADDRESS = "0019DB59ED77";
           WOLClass client = new WOLClass();
           client.Connect(new
              IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
              0x2fff); // port=12287 let's use this one 
           client.SetClientToBrodcastMode();
           //set sending bites
           int counter = 0;
           //buffer to be send
           byte[] bytes = new byte[1024];   // more than enough :-)
           //first 6 bytes should be 0xFF
           for (int y = 0; y < 6; y++)
               bytes[counter++] = 0xFF;
           //now repeate MAC 16 times
           for (int y = 0; y < 16; y++)
           {
               int i = 0;
               for (int z = 0; z < 6; z++)
               {
             bytes[counter++] =
                 byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
             i += 2;
               }
           }
           //now send wake up packet
           int reterned_value = client.Send(bytes, 1024);
                /* */
            /*gbm kalk 33*9/
            string MAC_ADDRESS = "BC305BA21100";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
            /*al - tat  koblenz 37*9/
            string MAC_ADDRESS = "F04DA225D181";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);

             /* *9/
             //EGEMEN DINSLAKEN -38
            string MAC_ADDRESS = "3085A970D22D";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
             /*
            //Bekir Leverkuzen -39/
            string MAC_ADDRESS = "0023AE8017C4";
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x2fff); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                  byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
                   /*SPAR BENDORF*9/
                 string MAC_ADDRESS = "E0469A293CC1";
                 WOLClass client = new WOLClass();
                 client.Connect(new
                    IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                    0x2fff); // port=12287 let's use this one 
                 client.SetClientToBrodcastMode();
                 //set sending bites
                 int counter = 0;
                 //buffer to be send
                 byte[] bytes = new byte[1024];   // more than enough :-)
                 //first 6 bytes should be 0xFF
                 for (int y = 0; y < 6; y++)
                     bytes[counter++] = 0xFF;
                 //now repeate MAC 16 times
                 for (int y = 0; y < 16; y++)
                 {
                     int i = 0;
                     for (int z = 0; z < 6; z++)
                     {
                         bytes[counter++] =
                             byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                         i += 2;
                     }
                 }
                 //now send wake up packet
                 int reterned_value = client.Send(bytes, 1024);
                      /* */
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonButton14_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("TeamViewerQS.exe");
            }
            catch
            {
            }
        }


    }
}
