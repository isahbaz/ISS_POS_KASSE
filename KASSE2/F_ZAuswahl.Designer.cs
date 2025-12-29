namespace IS_KASSE
{
    partial class F_ZAuswahl
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
            this.btnZErstellundAusdruck = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDatevStatus = new System.Windows.Forms.Label();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblDatevInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDatevError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnZErstellundAusdruck
            // 
            this.btnZErstellundAusdruck.Location = new System.Drawing.Point(254, 55);
            this.btnZErstellundAusdruck.Margin = new System.Windows.Forms.Padding(0);
            this.btnZErstellundAusdruck.Name = "btnZErstellundAusdruck";
            this.btnZErstellundAusdruck.OverrideDefault.Border.Color1 = System.Drawing.Color.OrangeRed;
            this.btnZErstellundAusdruck.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnZErstellundAusdruck.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.btnZErstellundAusdruck.Size = new System.Drawing.Size(247, 96);
            this.btnZErstellundAusdruck.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnZErstellundAusdruck.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnZErstellundAusdruck.StateNormal.Border.Color1 = System.Drawing.Color.MediumBlue;
            this.btnZErstellundAusdruck.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnZErstellundAusdruck.StateNormal.Border.Rounding = 10;
            this.btnZErstellundAusdruck.StateNormal.Border.Width = 3;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.Show;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnZErstellundAusdruck.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnZErstellundAusdruck.StatePressed.Back.Image = global::IS_KASSE.Properties.Resources.logout;
            this.btnZErstellundAusdruck.StatePressed.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.btnZErstellundAusdruck.TabIndex = 71;
            this.btnZErstellundAusdruck.Values.Text = "Z Bericht\r\n*Ausdrucken*\r\n\r\n";
            this.btnZErstellundAusdruck.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::IS_KASSE.Properties.Resources.Datev_svg;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(103, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(137, 123);
            this.panel1.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(275, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 20);
            this.label1.TabIndex = 73;
            this.label1.Text = "DATEV API Connection:";
            // 
            // lblDatevStatus
            // 
            this.lblDatevStatus.AutoSize = true;
            this.lblDatevStatus.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatevStatus.Location = new System.Drawing.Point(428, 172);
            this.lblDatevStatus.Name = "lblDatevStatus";
            this.lblDatevStatus.Size = new System.Drawing.Size(48, 20);
            this.lblDatevStatus.TabIndex = 74;
            this.lblDatevStatus.Text = "Status";
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(254, 339);
            this.kryptonButton2.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.OverrideDefault.Border.Color1 = System.Drawing.Color.OrangeRed;
            this.kryptonButton2.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonButton2.Size = new System.Drawing.Size(247, 81);
            this.kryptonButton2.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.kryptonButton2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateNormal.Border.Color1 = System.Drawing.Color.OrangeRed;
            this.kryptonButton2.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateNormal.Border.Rounding = 10;
            this.kryptonButton2.StateNormal.Border.Width = 3;
            this.kryptonButton2.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton2.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.StateNormal.Content.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StateNormal.Content.ShortText.Prefix = ComponentFactory.Krypton.Toolkit.PaletteTextHotkeyPrefix.Show;
            this.kryptonButton2.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StateNormal.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton2.StatePressed.Back.Image = global::IS_KASSE.Properties.Resources.logout;
            this.kryptonButton2.StatePressed.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton2.TabIndex = 75;
            this.kryptonButton2.Values.Text = "Abbrechen";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // lblDatevInfo
            // 
            this.lblDatevInfo.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatevInfo.Location = new System.Drawing.Point(275, 192);
            this.lblDatevInfo.Name = "lblDatevInfo";
            this.lblDatevInfo.Size = new System.Drawing.Size(435, 82);
            this.lblDatevInfo.TabIndex = 76;
            this.lblDatevInfo.Text = "Bitte warten... DATEV Übertragung!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(152, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(491, 33);
            this.label2.TabIndex = 77;
            this.label2.Text = "***DER Z-BERICHT WURDE ERSTELLT!***";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-1, 433);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(552, 16);
            this.label3.TabIndex = 78;
            this.label3.Text = "(*) Der Z-Bericht kann jederzeit an der Server-Software unter Archive erneut auf " +
    "A4 ausgedruckt werden.";
            // 
            // lblDatevError
            // 
            this.lblDatevError.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatevError.Location = new System.Drawing.Point(275, 274);
            this.lblDatevError.Name = "lblDatevError";
            this.lblDatevError.Size = new System.Drawing.Size(332, 65);
            this.lblDatevError.TabIndex = 79;
            this.lblDatevError.Text = "Bitte warten... DATEV Übertragung!";
            // 
            // F_ZAuswahl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(782, 462);
            this.Controls.Add(this.lblDatevError);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDatevInfo);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.lblDatevStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnZErstellundAusdruck);
            this.Name = "F_ZAuswahl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Z Bericht-Aktion";
            this.Load += new System.EventHandler(this.F_ZAuswahl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnZErstellundAusdruck;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDatevStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private System.Windows.Forms.Label lblDatevInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDatevError;
    }
}