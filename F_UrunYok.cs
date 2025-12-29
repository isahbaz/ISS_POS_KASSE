using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace IS_KASSE
{
    public partial class F_UrunYok : Form
    {
        SoundPlayer player = new SoundPlayer();
        Thread t2;
        public F_UrunYok()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_UrunYok_Load(object sender, EventArgs e)
        {
            //string dosyaad = Properties.Resources.miata_horn;
            

           // string path = "C:\\windows\\media\\start.wav"; // Çalmasını istediğiniz ses dosyasının yolu

            //player.SoundLocation = path;
            /*System.Media.SystemSounds.Beep.Play();
            System.Media.SystemSounds.Asterisk.Play();
            System.Media.SystemSounds.Exclamation.Play();
            System.Media.SystemSounds.Question.Play();
            System.Media.SystemSounds.Hand.Play();*/
            t2 = new Thread(new ThreadStart(sescal));
            t2.Start();

            
           /* player.Stream = Properties.Resources.miata_horn;
            //player.
            player.PlayLooping();*/
            //Console.Beep(1000,4000);
            //Console.Beep(200, 100);
            
        }

        private void F_UrunYok_FormClosed(object sender, FormClosedEventArgs e)
        {
           // player.Stop();
          //  t2.Abort();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void F_UrunYok_Activated(object sender, EventArgs e)
        {
            //Console.Beep(1000, 4000);
        }
        private void sescal()
        {
            Console.Beep(1000,1000);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
