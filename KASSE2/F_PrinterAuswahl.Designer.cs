namespace IS_KASSE
{
    partial class F_PrinterAuswahl
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
            this.btnBon = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnA4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnBon
            // 
            this.btnBon.Location = new System.Drawing.Point(354, 76);
            this.btnBon.Name = "btnBon";
            this.btnBon.Size = new System.Drawing.Size(198, 111);
            this.btnBon.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnBon.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBon.StateNormal.Border.Rounding = 2;
            this.btnBon.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBon.TabIndex = 3;
            this.btnBon.Values.Text = "Bondrucker";
            this.btnBon.Click += new System.EventHandler(this.btnBon_Click);
            // 
            // btnA4
            // 
            this.btnA4.Location = new System.Drawing.Point(56, 76);
            this.btnA4.Name = "btnA4";
            this.btnA4.Size = new System.Drawing.Size(198, 111);
            this.btnA4.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnA4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnA4.StateNormal.Border.Rounding = 2;
            this.btnA4.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnA4.TabIndex = 2;
            this.btnA4.Values.Text = "A4 Drucker";
            this.btnA4.Click += new System.EventHandler(this.btnA4_Click);
            // 
            // F_PrinterAuswahl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 262);
            this.Controls.Add(this.btnBon);
            this.Controls.Add(this.btnA4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_PrinterAuswahl";
            this.Text = "F_PrinterAuswahl";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBon;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnA4;
    }
}