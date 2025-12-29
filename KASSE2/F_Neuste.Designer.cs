namespace IS_KASSE
{
    partial class F_Neuste
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
            this.btnAbbruch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPausieren = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbmelden = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnAbbruch
            // 
            this.btnAbbruch.Location = new System.Drawing.Point(12, 329);
            this.btnAbbruch.Name = "btnAbbruch";
            this.btnAbbruch.Size = new System.Drawing.Size(569, 93);
            this.btnAbbruch.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.btnAbbruch.StateNormal.Border.ColorAngle = 2F;
            this.btnAbbruch.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAbbruch.StateNormal.Border.Rounding = 6;
            this.btnAbbruch.StateNormal.Border.Width = 3;
            this.btnAbbruch.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Blue;
            this.btnAbbruch.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbbruch.TabIndex = 14;
            this.btnAbbruch.Values.Text = "Abbrechen";
            this.btnAbbruch.Click += new System.EventHandler(this.btnAbbruch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(54, -41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 35);
            this.label1.TabIndex = 13;
            this.label1.Text = "Bitte wählen Sie Ihre Abmeldungstyp ein?";
            // 
            // btnPausieren
            // 
            this.btnPausieren.Location = new System.Drawing.Point(12, 173);
            this.btnPausieren.Name = "btnPausieren";
            this.btnPausieren.Size = new System.Drawing.Size(569, 139);
            this.btnPausieren.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.os_windows_128;
            this.btnPausieren.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnPausieren.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPausieren.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPausieren.StateNormal.Border.Rounding = 2;
            this.btnPausieren.StateNormal.Border.Width = 3;
            this.btnPausieren.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnPausieren.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausieren.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnPausieren.TabIndex = 12;
            this.btnPausieren.Values.Text = "Windows Neustart";
            this.btnPausieren.Click += new System.EventHandler(this.btnPausieren_Click);
            // 
            // btnAbmelden
            // 
            this.btnAbmelden.Location = new System.Drawing.Point(12, 12);
            this.btnAbmelden.Name = "btnAbmelden";
            this.btnAbmelden.Size = new System.Drawing.Size(569, 142);
            this.btnAbmelden.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.iss_pos_kasa;
            this.btnAbmelden.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnAbmelden.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnAbmelden.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAbmelden.StateNormal.Border.Rounding = 2;
            this.btnAbmelden.StateNormal.Border.Width = 3;
            this.btnAbmelden.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbmelden.StateNormal.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnAbmelden.TabIndex = 11;
            this.btnAbmelden.Values.Text = "Sofware Neustart";
            this.btnAbmelden.Click += new System.EventHandler(this.btnAbmelden_Click);
            // 
            // F_Neuste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(592, 435);
            this.Controls.Add(this.btnAbbruch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPausieren);
            this.Controls.Add(this.btnAbmelden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Neuste";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_Neuste";
            this.Load += new System.EventHandler(this.F_Neuste_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbbruch;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPausieren;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbmelden;
    }
}