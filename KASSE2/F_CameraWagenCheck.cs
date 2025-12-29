using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_CameraWagenCheck : Form
    {
        VideoCaptureDevice videoSource;
        VideoCaptureDevice videoSource2;
        public F_CameraWagenCheck()
        {
            InitializeComponent();
        }

        private void F_CameraWagenCheck_Load(object sender, EventArgs e)
        {
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videosources.Count == 1)
            {
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                label2.Visible = false;
                pictureBox2.Visible = false;
            }
            else if (videosources.Count > 1)
            {
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                videoSource2 = new VideoCaptureDevice(videosources[1].MonikerString);
            }


            //Create NewFrame event handler
            //(This one triggers every time a new frame/image is captured
            videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame1);
            videoSource2.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame2);
            //Start recording
            videoSource.Start();
            videoSource2.Start();
        }
        void videoSource_NewFrame1(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        void videoSource_NewFrame2(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }
    }
}
