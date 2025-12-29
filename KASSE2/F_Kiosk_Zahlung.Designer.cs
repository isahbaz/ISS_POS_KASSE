namespace IS_KASSE
{
    partial class F_Kiosk_Zahlung
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
            this.btnImhaus = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAuserhaus = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbbrechen = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnImhaus
            // 
            this.btnImhaus.Location = new System.Drawing.Point(12, 12);
            this.btnImhaus.Name = "btnImhaus";
            this.btnImhaus.Size = new System.Drawing.Size(276, 163);
            this.btnImhaus.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.imHaus;
            this.btnImhaus.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnImhaus.StateCommon.Border.Color1 = System.Drawing.Color.BlueViolet;
            this.btnImhaus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnImhaus.StateCommon.Border.Rounding = 5;
            this.btnImhaus.StateCommon.Border.Width = 2;
            this.btnImhaus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DodgerBlue;
            this.btnImhaus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImhaus.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnImhaus.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnImhaus.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnImhaus.TabIndex = 2;
            this.btnImhaus.Values.Text = "Hier zu Essen";
            this.btnImhaus.Click += new System.EventHandler(this.btnImhaus_Click);
            // 
            // btnAuserhaus
            // 
            this.btnAuserhaus.Location = new System.Drawing.Point(369, 12);
            this.btnAuserhaus.Name = "btnAuserhaus";
            this.btnAuserhaus.Size = new System.Drawing.Size(276, 163);
            this.btnAuserhaus.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.zummitnehmen;
            this.btnAuserhaus.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnAuserhaus.StateCommon.Border.Color1 = System.Drawing.Color.BlueViolet;
            this.btnAuserhaus.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAuserhaus.StateCommon.Border.Rounding = 5;
            this.btnAuserhaus.StateCommon.Border.Width = 2;
            this.btnAuserhaus.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DodgerBlue;
            this.btnAuserhaus.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuserhaus.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnAuserhaus.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAuserhaus.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnAuserhaus.TabIndex = 3;
            this.btnAuserhaus.Values.Text = "Zum Mitnehmen";
            this.btnAuserhaus.Click += new System.EventHandler(this.btnAuserhaus_Click);
            // 
            // btnAbbrechen
            // 
            this.btnAbbrechen.Location = new System.Drawing.Point(182, 228);
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
            this.btnAbbrechen.TabIndex = 4;
            this.btnAbbrechen.Values.Text = "Abbrechen";
            this.btnAbbrechen.Click += new System.EventHandler(this.btnAbbrechen_Click);
            // 
            // F_Kiosk_Zahlung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(667, 433);
            this.Controls.Add(this.btnAbbrechen);
            this.Controls.Add(this.btnAuserhaus);
            this.Controls.Add(this.btnImhaus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Kiosk_Zahlung";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_Kiosk_Zahlung";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnImhaus;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAuserhaus;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbbrechen;
    }
}