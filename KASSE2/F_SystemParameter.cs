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
    public partial class F_SystemParameter : Form
    {
        public F_SystemParameter()
        {
            InitializeComponent();
        }

        private void F_SystemParameter_Load(object sender, EventArgs e)
        {
            if (Program.GlobalAyarlar["DruckerMode"] == 1)
            {
                cbbDruckerMode.Visible = true;
                lbldruckerMode.Visible = true;
                //Program.DruckerMode = 1;
            }
            else
            {
                cbbDruckerMode.Visible = false;
                lbldruckerMode.Visible = false;
                //Program.DruckerMode = 1;
            }
            
            dgvMonutore.Columns.Add("name", "DisplayName");
            dgvMonutore.Columns.Add("primery", "PRIMARY");
            dgvMonutore.Columns.Add("primery", "WOR.AREA HEIGHT ");
            dgvMonutore.Columns.Add("primery", "WOR.AREA WIDTH ");
            dgvMonutore.Columns.Add("primery", "WOR.AREA SIZE HEIGHT  ");
            dgvMonutore.Columns.Add("primery", "WOR.AREA SIZE WIDTH ");
            dgvMonutore.Columns.Add("primery", "WOR.AREA LOCATION X");
            dgvMonutore.Columns.Add("primery", "WOR.AREA LOCATION Y");
            dgvMonutore.Columns.Add("primery", "WOR.AREA LEFT");
            dgvMonutore.Columns.Add("primery", "WOR.AREA RIGHT");
            MonutorOrdnung();
            dgvHW.Columns.Add("name", "PARAM NAME");
            dgvHW.Columns.Add("value", "PARAM VALUE");
            HWOrdnung();
            dgvGlobal.Columns.Add("name", "PARAM NAME");
            dgvGlobal.Columns.Add("value", "PARAM VALUE");
            GlobalOrdnung();
            if (Program.StopWatch == 0)
            {
                btnStopWatch.Text = "Stop Watch is OFF";
            }
            else
            {
                btnStopWatch.Text = "Stop Watch is ON";
            }

        }

        private void HWOrdnung()
        {
            try
            {
                if (dgvHW.Rows.Count > 0)
                    dgvHW.Rows.Clear();
                int i = dgvHW.Rows.Count;

                foreach (KeyValuePair<string,string> Scr in Program.ProgramAyarlar)
                {
                    dgvHW.Rows.Add();
                    dgvHW.Rows[i].Cells[0].Value = Scr.Key;
                    dgvHW.Rows[i].Cells[1].Value = Scr.Value;
                   
                    i++;
                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }
        private void GlobalOrdnung()
        {
            try
            {
                if (dgvGlobal.Rows.Count > 0)
                    dgvGlobal.Rows.Clear();
                int i = dgvGlobal.Rows.Count;

                foreach (KeyValuePair<string, int> Scr in Program.GlobalAyarlar)
                {
                    dgvGlobal.Rows.Add();
                    dgvGlobal.Rows[i].Cells[0].Value = Scr.Key;
                    dgvGlobal.Rows[i].Cells[1].Value = Scr.Value;

                    i++;
                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }
        private void cbbDruckerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
             Program.bonDruck = true;
            /*  if (cbbDruckerMode.SelectedIndex == 0)
              {
                  Program.DruckerMode = 1;
              }
              else if (cbbDruckerMode.SelectedIndex == 1)
              {
                  Program.bonDruck = false;
                  Program.DruckerMode = 2;
              }
              else
              {
                  Program.DruckerMode = 3;
                  Program.bonDruck = false;
              }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                MonutorOrdnung();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                GlobalOrdnung();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                HWOrdnung();
            }
        }

        private void MonutorOrdnung()
        {
            try
            {
                if (dgvMonutore.Rows.Count > 0)
                    dgvMonutore.Rows.Clear();
                int i = dgvMonutore.Rows.Count;

                foreach (Screen Scr in Screen.AllScreens)
                {
                    dgvMonutore.Rows.Add();
                    dgvMonutore.Rows[i].Cells[0].Value = Scr.DeviceName;
                    dgvMonutore.Rows[i].Cells[1].Value = Scr.Primary == true ? "JA" : "NEIN";
                    dgvMonutore.Rows[i].Cells[2].Value = Scr.WorkingArea.Height;
                    dgvMonutore.Rows[i].Cells[3].Value = Scr.WorkingArea.Width;
                    dgvMonutore.Rows[i].Cells[4].Value = Scr.WorkingArea.Size.Height;
                    dgvMonutore.Rows[i].Cells[5].Value = Scr.WorkingArea.Size.Width;
                    dgvMonutore.Rows[i].Cells[6].Value = Scr.WorkingArea.Location.X;
                    dgvMonutore.Rows[i].Cells[7].Value = Scr.WorkingArea.Location.Y;
                    dgvMonutore.Rows[i].Cells[8].Value = Scr.WorkingArea.Left;
                    dgvMonutore.Rows[i].Cells[9].Value = Scr.WorkingArea.Right;
                    i++;
                }
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }

        private void btnStopWatch_Click(object sender, EventArgs e)
        {
            if (Program.StopWatch == 0)
            {
                if (MessageBox.Show("Möchten Sie für Messungen StopWatch aktivieren?", "Messungen Aktitivieren", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Program.StopWatch = 1;
                    btnStopWatch.Text = "Stop Watch is ON";
                }
            }
            else
            {
                Program.StopWatch = 0;
                btnStopWatch.Text = "Stop Watch is OFF";
            }
        }
    }
}
