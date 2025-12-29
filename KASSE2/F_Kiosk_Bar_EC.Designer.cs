namespace IS_KASSE
{
    partial class F_Kiosk_Bar_EC
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
            this.btnAbbrechen = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEC = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBar = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnAbbrechen
            // 
            this.btnAbbrechen.Location = new System.Drawing.Point(283, 191);
            this.btnAbbrechen.Name = "btnAbbrechen";
            this.btnAbbrechen.Size = new System.Drawing.Size(276, 163);
            this.btnAbbrechen.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.btnAbbrechen.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.btnAbbrechen.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnAbbrechen.StateCommon.Border.Color1 = System.Drawing.Color.Red;
            this.btnAbbrechen.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAbbrechen.StateCommon.Border.Rounding = 5;
            this.btnAbbrechen.StateCommon.Border.Width = 2;
            this.btnAbbrechen.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Tomato;
            this.btnAbbrechen.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbbrechen.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnAbbrechen.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbbrechen.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbbrechen.TabIndex = 7;
            this.btnAbbrechen.Values.Text = "Abbrechen";
            this.btnAbbrechen.Click += new System.EventHandler(this.btnAbbrechen_Click);
            // 
            // btnEC
            // 
            this.btnEC.Location = new System.Drawing.Point(283, 12);
            this.btnEC.Name = "btnEC";
            this.btnEC.Size = new System.Drawing.Size(276, 163);
            this.btnEC.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.ec_und_giro_Logo;
            this.btnEC.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnEC.StateCommon.Border.Color1 = System.Drawing.Color.Blue;
            this.btnEC.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEC.StateCommon.Border.Rounding = 5;
            this.btnEC.StateCommon.Border.Width = 2;
            this.btnEC.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DodgerBlue;
            this.btnEC.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEC.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnEC.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEC.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnEC.TabIndex = 6;
            this.btnEC.Values.Text = "EC-DEBIT-CREDITKARTE";
            this.btnEC.Click += new System.EventHandler(this.btnEC_Click);
            // 
            // btnBar
            // 
            this.btnBar.Location = new System.Drawing.Point(1, 12);
            this.btnBar.Name = "btnBar";
            this.btnBar.Size = new System.Drawing.Size(276, 163);
            this.btnBar.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.btnBar.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.btnBar.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.Bar;
            this.btnBar.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnBar.StateCommon.Border.Color1 = System.Drawing.Color.SeaGreen;
            this.btnBar.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBar.StateCommon.Border.Rounding = 5;
            this.btnBar.StateCommon.Border.Width = 2;
            this.btnBar.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DodgerBlue;
            this.btnBar.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBar.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnBar.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnBar.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnBar.TabIndex = 5;
            this.btnBar.Values.Text = "Sofort BAR-CASH Bezahlen";
            this.btnBar.Click += new System.EventHandler(this.btnBar_Click);
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(565, 12);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(279, 170);
            this.kryptonButton1.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonButton1.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonButton1.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.bestellen12;
            this.kryptonButton1.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton1.StateCommon.Border.Color1 = System.Drawing.Color.SeaGreen;
            this.kryptonButton1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateCommon.Border.Rounding = 5;
            this.kryptonButton1.StateCommon.Border.Width = 2;
            this.kryptonButton1.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DodgerBlue;
            this.kryptonButton1.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonButton1.StateCommon.Content.ShortText.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton1.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton1.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonButton1.TabIndex = 8;
            this.kryptonButton1.Values.Text = "BESTELLEN \r\nAn der Kasse Bezahlen\r\n";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // F_Kiosk_Bar_EC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(856, 365);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.btnAbbrechen);
            this.Controls.Add(this.btnEC);
            this.Controls.Add(this.btnBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Kiosk_Bar_EC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_Kiosk_Bar_EC";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbbrechen;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEC;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBar;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
    }
}