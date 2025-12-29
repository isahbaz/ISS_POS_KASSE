using iss_Artikel;
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
    public partial class F_MultiStafelSelect : Form
    {
        public List<StafelArtikelInfo> StafelInfo = new List<StafelArtikelInfo>();
        public int AngeboiID =0;
        public F_MultiStafelSelect()
        {
            InitializeComponent();
        }

        private void F_MultiStafelSelect_Load(object sender, EventArgs e)
        {
            if(StafelInfo.Count>0)
            {
                for(int i=0; i<StafelInfo.Count; i++)
                {
                   
                    // 
                    // btnnAuswahl
                    // 
                    Button btnnAuswahl = new Button();  
                    btnnAuswahl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnnAuswahl.Location = new System.Drawing.Point(9, 9);
                    btnnAuswahl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
                    btnnAuswahl.Name = StafelInfo[i].AngebotId.ToString(); 
                    btnnAuswahl.Size = new System.Drawing.Size(152, 71);
                    btnnAuswahl.TabIndex = 0;
                    btnnAuswahl.Text = "ANNEHMEN";
                    btnnAuswahl.UseVisualStyleBackColor = true;
                    btnnAuswahl.Tag = StafelInfo[i].AngebotId;
                    btnnAuswahl.Click += new System.EventHandler(this.btnnAuswahl_Click);

                    // 
                    // lblPrName
                    // 
                    Label lblPrName = new Label();
                    lblPrName.AutoSize = true;
                    lblPrName.Location = new System.Drawing.Point(204, 9);
                    lblPrName.Name = "lblPrName";
                    lblPrName.Size = new System.Drawing.Size(132, 22);
                    lblPrName.TabIndex = 1;
                    lblPrName.Text = StafelInfo[i].AngebotName;
                    // 
                    // lblPreis
                    // 
                    Label lblPreis = new Label();
                    lblPreis.AutoSize = true;
                    lblPreis.Location = new System.Drawing.Point(346, 58);
                    lblPreis.Name = "lblPreis";
                    lblPreis.Size = new System.Drawing.Size(125, 22);
                    lblPreis.TabIndex = 2;
                    lblPreis.Text = StafelInfo[i].Preis.ToString("C");
                    // 
                    // lblMenge
                    // 
                    Label lblMenge = new Label();
                    lblMenge.AutoSize = true;
                    lblMenge.Location = new System.Drawing.Point(204, 58);
                    lblMenge.Name = "lblMenge";
                    lblMenge.Size = new System.Drawing.Size(91, 22);
                    lblMenge.TabIndex = 3;
                    lblMenge.Text = StafelInfo[i].Menge.ToString();
                    // 
                    // pnl
                    // 
                    Panel pnl = new Panel();
                    pnl.Controls.Add(lblMenge);
                    pnl.Controls.Add(lblPreis);
                    pnl.Controls.Add(lblPrName);
                    pnl.Controls.Add(btnnAuswahl);
                    pnl.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    pnl.Location = new System.Drawing.Point(4, 4);
                    pnl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
                    pnl.Name = "pnl";
                    pnl.Size = new System.Drawing.Size(1183, 96);
                    pnl.TabIndex = 0;

                    flowLayoutPanel1.Controls.Add(pnl);
                }
            }
        }

       

        private void btnnAuswahl_Click(object sender, EventArgs e)
        {
            Button btnSecilen= (Button)sender as Button;
            AngeboiID =Convert.ToInt16(btnSecilen.Tag);
            this.Close();

        }

        private void btnAuswahl_Click(object sender, EventArgs e)
        {
            AngeboiID = 0;
            this.Close();
        }
    }
}
