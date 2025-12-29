namespace IS_KASSE
{
    partial class F_KundenDisplayKurz
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_KundenDisplayKurz));
            this.panel1 = new System.Windows.Forms.Panel();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblZuZahlen = new System.Windows.Forms.Label();
            this.lblgegeben = new System.Windows.Forms.Label();
            this.lblRuckgeld = new System.Windows.Forms.Label();
            this.lblZahlungsTyp = new System.Windows.Forms.Label();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblZahlungsTyp);
            this.panel1.Controls.Add(this.lblRuckgeld);
            this.panel1.Controls.Add(this.lblgegeben);
            this.panel1.Controls.Add(this.lblZuZahlen);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.shapeContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(551, 301);
            this.panel1.TabIndex = 0;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(549, 299);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderWidth = 3;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 157;
            this.lineShape1.X2 = 157;
            this.lineShape1.Y1 = 15;
            this.lineShape1.Y2 = 251;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 63);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(137, 131);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(180, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "zu Zahlen:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(194, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 37);
            this.label2.TabIndex = 3;
            this.label2.Text = "gegeben:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(183, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 37);
            this.label3.TabIndex = 4;
            this.label3.Text = "Rückgeld:";
            // 
            // lblZuZahlen
            // 
            this.lblZuZahlen.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZuZahlen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblZuZahlen.Location = new System.Drawing.Point(363, 63);
            this.lblZuZahlen.Name = "lblZuZahlen";
            this.lblZuZahlen.Size = new System.Drawing.Size(153, 37);
            this.lblZuZahlen.TabIndex = 5;
            this.lblZuZahlen.Text = "19,99 €";
            this.lblZuZahlen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblgegeben
            // 
            this.lblgegeben.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblgegeben.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblgegeben.Location = new System.Drawing.Point(363, 118);
            this.lblgegeben.Name = "lblgegeben";
            this.lblgegeben.Size = new System.Drawing.Size(153, 47);
            this.lblgegeben.TabIndex = 6;
            this.lblgegeben.Text = "19,99 €";
            this.lblgegeben.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRuckgeld
            // 
            this.lblRuckgeld.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRuckgeld.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRuckgeld.Location = new System.Drawing.Point(379, 196);
            this.lblRuckgeld.Name = "lblRuckgeld";
            this.lblRuckgeld.Size = new System.Drawing.Size(137, 37);
            this.lblRuckgeld.TabIndex = 7;
            this.lblRuckgeld.Text = "19,99 €";
            this.lblRuckgeld.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRuckgeld.Click += new System.EventHandler(this.label6_Click);
            // 
            // lblZahlungsTyp
            // 
            this.lblZahlungsTyp.AutoSize = true;
            this.lblZahlungsTyp.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZahlungsTyp.Location = new System.Drawing.Point(266, 15);
            this.lblZahlungsTyp.Name = "lblZahlungsTyp";
            this.lblZahlungsTyp.Size = new System.Drawing.Size(96, 28);
            this.lblZahlungsTyp.TabIndex = 8;
            this.lblZahlungsTyp.Text = "19,99 €";
            this.lblZahlungsTyp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderWidth = 3;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 515;
            this.lineShape2.X2 = 186;
            this.lineShape2.Y1 = 179;
            this.lineShape2.Y2 = 178;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // F_KundenDisplayKurz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 301);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(250, 150);
            this.Name = "F_KundenDisplayKurz";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "F_KundenDisplayKurz";
            this.Load += new System.EventHandler(this.F_KundenDisplayKurz_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label lblRuckgeld;
        public System.Windows.Forms.Label lblgegeben;
        public System.Windows.Forms.Label lblZuZahlen;
        public System.Windows.Forms.Label lblZahlungsTyp;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.Timer timer1;
    }
}