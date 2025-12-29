namespace IS_KASSE
{
    partial class F_ZahlungTypRechnung
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBon
            // 
            this.btnBon.Location = new System.Drawing.Point(411, 92);
            this.btnBon.Name = "btnBon";
            this.btnBon.Size = new System.Drawing.Size(218, 140);
            this.btnBon.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnBon.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBon.StateNormal.Border.Rounding = 2;
            this.btnBon.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBon.TabIndex = 5;
            this.btnBon.Values.Text = "EC";
            this.btnBon.Click += new System.EventHandler(this.btnBon_Click);
            // 
            // btnA4
            // 
            this.btnA4.Location = new System.Drawing.Point(111, 92);
            this.btnA4.Name = "btnA4";
            this.btnA4.Size = new System.Drawing.Size(229, 140);
            this.btnA4.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnA4.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnA4.StateNormal.Border.Rounding = 2;
            this.btnA4.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnA4.TabIndex = 4;
            this.btnA4.Values.Text = " BAR";
            this.btnA4.Click += new System.EventHandler(this.btnA4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(215, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 27);
            this.label1.TabIndex = 6;
            this.label1.Text = "Bitte wählen Sie Zahlungstyp ein?";
            // 
            // F_ZahlungTypRechnung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Crimson;
            this.ClientSize = new System.Drawing.Size(740, 325);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBon);
            this.Controls.Add(this.btnA4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_ZahlungTypRechnung";
            this.Text = "F_ZahlungTypRechnung";
            this.Load += new System.EventHandler(this.F_ZahlungTypRechnung_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBon;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnA4;
        private System.Windows.Forms.Label label1;
    }
}