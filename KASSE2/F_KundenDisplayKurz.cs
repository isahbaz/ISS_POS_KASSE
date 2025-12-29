using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_KundenDisplayKurz : Form
    {
        int time = 10;
        public F_KundenDisplayKurz()
        {
            InitializeComponent();
          // this.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void F_KundenDisplayKurz_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            StartPosition = FormStartPosition.CenterScreen;
           /**/
           this.Location = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.Location;
           int formWidth = this.Width;
           int formHeight = this.Height;
            //int formWidth = Screen.AllScreens[0].Bounds.Size.Width;
            //int formHeight = Screen.AllScreens[0].Bounds.Size.Height;
            int screen1W= Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].WorkingArea.X;  //Screen.PrimaryScreen.Bounds.Width;
            int screen2W = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].Bounds.Width;
                         
           int screen2H = Screen.AllScreens[(int)Convert.ToInt16(Program.ProgramAyarlar["DispNr"])].Bounds.Height;
                       
           
           int top = screen1W+((screen2W - formWidth) / 2);
           int left = (screen2H - formHeight) / 2;
           this.Location = new Point(top, left);

           //MessageBox.Show(this.Location.X + "- Y:" + this.Location.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            time--;
            if (time == 0)
            {
               
                timer1.Enabled = false;
                this.Close();
            }
        }
    }
}
