using ComponentFactory.Krypton.Toolkit;
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
    public partial class F_KolliAuswahl : Form
    {
        public List<KolliKiste> kolliList = new List<KolliKiste>();
        public KolliKiste secilenkiste = null;
        public F_KolliAuswahl()
        {
            InitializeComponent();
        }

        private void F_KolliAuswahl_Load(object sender, EventArgs e)
        {
            for (int i=0; i<kolliList.Count; i++)
            {
                ComponentFactory.Krypton.Toolkit.KryptonButton btnKolliTyp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
                KolliKiste kolli = new KolliKiste();
                kolli = kolliList[i];
                btnKolliTyp.Location = new System.Drawing.Point(3, 3);
                btnKolliTyp.Name = "btnKolliTyp";
                btnKolliTyp.Size = new System.Drawing.Size(469, 79);
                btnKolliTyp.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                btnKolliTyp.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
                btnKolliTyp.StateCommon.Border.Width = 2;
                btnKolliTyp.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Blue;
                btnKolliTyp.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnKolliTyp.TabIndex = 75;
                btnKolliTyp.Values.Text = kolli.Name+" "+ kolli.PfandName;
                btnKolliTyp.Tag = kolliList[i];
                btnKolliTyp.Click += new System.EventHandler(GenericButton);
                
                this.flowLayoutPanel1.Controls.Add(btnKolliTyp);
            }
        }

        private void GenericButton(object sender, EventArgs e)
        {
            ComponentFactory.Krypton.Toolkit.KryptonButton btnKolliTyp = sender as KryptonButton;
            secilenkiste =(KolliKiste)btnKolliTyp.Tag;
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
