using AForge.Video.DirectShow;
using Conn;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_CameraWagenCheck : Form
    {
        VideoCaptureDevice videoSource;
        VideoCaptureDevice videoSource2;
        MySqlConnection myConn;
        dbConn dbConn = new dbConn();
        public F_CameraWagenCheck()
        {
            InitializeComponent();
            myConn = dbConn.myconn();
        }

        private void F_CameraWagenCheck_Load(object sender, EventArgs e)
        {
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();
            }

            /*using (var cmd = new MySqlCommand(
    "SELECT id, ad, fiyat FROM urunler", myConn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbbKameras.Items.Add(reader.GetInt16("kamerano"));

                    }
                }
            }*/


            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (int i = 0; i < videosources.Count; i++)
            {
                cbbKameras.Items.Add(videosources[i].Name);
            }
            if (videosources.Count == 1)
            {
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                label2.Visible = false;
                pictureBox2.Visible = false;
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame1);
                videoSource.Start();
            }
            else if (videosources.Count > 1)
            {
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                videoSource2 = new VideoCaptureDevice(videosources[1].MonikerString);
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame1);
                videoSource2.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame2);
                videoSource.Start();
                videoSource2.Start();
            }


            //Create NewFrame event handler
            //(This one triggers every time a new frame/image is captured
            
            //Start recording
            
          
        }
        void videoSource_NewFrame1(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        void videoSource_NewFrame2(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void btnWaage_Click(object sender, EventArgs e)
        {
            MySqlCommand cmdKamera = new MySqlCommand();

        }
    }
}
