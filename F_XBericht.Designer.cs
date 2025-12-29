namespace IS_KASSE
{
    partial class F_XBericht
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.gbDetay = new System.Windows.Forms.GroupBox();
            this.rbDetayDetayli = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbDetayBasit = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbDetay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.kryptonButton2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gbDetay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(660, 431);
            this.panel1.TabIndex = 0;
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(176, 325);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(308, 78);
            this.kryptonButton2.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.kryptonButton2.StateNormal.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kryptonButton2.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.kryptonButton2.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateNormal.Border.Rounding = 10;
            this.kryptonButton2.StateNormal.Border.Width = 2;
            this.kryptonButton2.TabIndex = 55;
            this.kryptonButton2.Values.Text = "Schließen";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Location = new System.Drawing.Point(0, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 161);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BEDIENER AUSWAHL";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(652, 141);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // gbDetay
            // 
            this.gbDetay.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.gbDetay.Controls.Add(this.rbDetayDetayli);
            this.gbDetay.Controls.Add(this.rbDetayBasit);
            this.gbDetay.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDetay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbDetay.Location = new System.Drawing.Point(0, 0);
            this.gbDetay.Name = "gbDetay";
            this.gbDetay.Size = new System.Drawing.Size(658, 136);
            this.gbDetay.TabIndex = 52;
            this.gbDetay.TabStop = false;
            this.gbDetay.Text = "Bedienerbericht-Typ";
            this.gbDetay.Enter += new System.EventHandler(this.gbDetay_Enter);
            // 
            // rbDetayDetayli
            // 
            this.rbDetayDetayli.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl;
            this.rbDetayDetayli.Location = new System.Drawing.Point(23, 83);
            this.rbDetayDetayli.Name = "rbDetayDetayli";
            this.rbDetayDetayli.Size = new System.Drawing.Size(135, 29);
            this.rbDetayDetayli.TabIndex = 1;
            this.rbDetayDetayli.Values.Text = "DETAILLIERT";
            // 
            // rbDetayBasit
            // 
            this.rbDetayBasit.Checked = true;
            this.rbDetayBasit.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl;
            this.rbDetayBasit.Location = new System.Drawing.Point(23, 29);
            this.rbDetayBasit.Name = "rbDetayBasit";
            this.rbDetayBasit.Size = new System.Drawing.Size(77, 29);
            this.rbDetayBasit.TabIndex = 0;
            this.rbDetayBasit.Values.Text = "BASIC";
            // 
            // F_XBericht
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(660, 431);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_XBericht";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_XBericht";
            this.Load += new System.EventHandler(this.F_XBericht_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbDetay.ResumeLayout(false);
            this.gbDetay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbDetay;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbDetayDetayli;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbDetayBasit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        

    }
}