using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace IS_KASSE
{
    public partial class F_XBericht : Form
    {
        MySqlConnection conn = new MySqlConnection();
        db baglanti = new db();
        public DataTable dtUserControl;
        Tarih tarih = new Tarih();

        public F_XBericht()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
           
        }
       

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_XBericht_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < dtUserControl.Rows.Count; i++)
            {
                Button buttonA = new Button();
                //buttonA.Location = new System.Drawing.Point(3, 3);
                buttonA.Name = "buttonA";
                buttonA.Size = new System.Drawing.Size(121, 119);
                buttonA.TabIndex = 0;
                buttonA.Text = dtUserControl.Rows[i].ItemArray[1].ToString(); ;
                buttonA.UseVisualStyleBackColor = true;
                buttonA.Location = new System.Drawing.Point(3, 3);
                buttonA.Enabled = true;
                buttonA.Tag = dtUserControl.Rows[i].ItemArray[0];
                buttonA.Click += new System.EventHandler(buttonA_Click);
                this.flowLayoutPanel1.Controls.Add(buttonA);
            }
        }
        private void buttonA_Click(object sender, EventArgs e)
        {
            Button btnTiklanan = sender as Button;
            FisBarkodlu fisdruck = new FisBarkodlu();
            if (rbDetayBasit.Checked == true)
            {
                
                fisdruck.BedinerBerichtDruck(Convert.ToInt32(btnTiklanan.Tag), 0, Program.bedAdSoyad);
            }
            else
            {
                fisdruck.BedinerBerichtDruck(Convert.ToInt32(btnTiklanan.Tag), 1, Program.bedAdSoyad);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void gbDetay_Enter(object sender, EventArgs e)
        {

        }
    }
}
