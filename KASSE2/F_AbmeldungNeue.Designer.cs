namespace IS_KASSE
{
    partial class F_AbmeldungNeue
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnPausieren = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbmelden = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbbruch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 35);
            this.label1.TabIndex = 9;
            this.label1.Text = "Bitte wählen Sie Ihre Abmeldungstyp ein?";
            // 
            // btnPausieren
            // 
            this.btnPausieren.Location = new System.Drawing.Point(427, 75);
            this.btnPausieren.Name = "btnPausieren";
            this.btnPausieren.Size = new System.Drawing.Size(259, 182);
            this.btnPausieren.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.coffee_256;
            this.btnPausieren.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.coffee_256;
            this.btnPausieren.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnPausieren.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPausieren.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPausieren.StateNormal.Border.Rounding = 2;
            this.btnPausieren.StateNormal.Border.Width = 3;
            this.btnPausieren.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausieren.StateTracking.Back.Image = global::IS_KASSE.Properties.Resources.coffee_256;
            this.btnPausieren.TabIndex = 8;
            this.btnPausieren.Values.Text = "Pause";
            this.btnPausieren.Click += new System.EventHandler(this.btnPausieren_Click);
            // 
            // btnAbmelden
            // 
            this.btnAbmelden.Location = new System.Drawing.Point(100, 75);
            this.btnAbmelden.Name = "btnAbmelden";
            this.btnAbmelden.Size = new System.Drawing.Size(280, 182);
            this.btnAbmelden.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.login_256;
            this.btnAbmelden.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.logout_256;
            this.btnAbmelden.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnAbmelden.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnAbmelden.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAbmelden.StateNormal.Border.Rounding = 2;
            this.btnAbmelden.StateNormal.Border.Width = 3;
            this.btnAbmelden.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbmelden.StateTracking.Back.Image = global::IS_KASSE.Properties.Resources.logout_256;
            this.btnAbmelden.StateTracking.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnAbmelden.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Elephant", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbmelden.TabIndex = 7;
            this.btnAbmelden.Values.Text = "Abmelden";
            this.btnAbmelden.Click += new System.EventHandler(this.btnAbmelden_Click);
            // 
            // btnAbbruch
            // 
            this.btnAbbruch.Location = new System.Drawing.Point(67, 301);
            this.btnAbbruch.Name = "btnAbbruch";
            this.btnAbbruch.Size = new System.Drawing.Size(650, 93);
            this.btnAbbruch.StateNormal.Border.Color1 = System.Drawing.Color.Red;
            this.btnAbbruch.StateNormal.Border.ColorAngle = 2F;
            this.btnAbbruch.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAbbruch.StateNormal.Border.Rounding = 6;
            this.btnAbbruch.StateNormal.Border.Width = 3;
            this.btnAbbruch.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Elephant", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbbruch.TabIndex = 10;
            this.btnAbbruch.Values.Text = "Abbrechen";
            this.btnAbbruch.Click += new System.EventHandler(this.btnAbbruch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Monofonto", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(734, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "10";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // F_AbmeldungNeue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(794, 406);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAbbruch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPausieren);
            this.Controls.Add(this.btnAbmelden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_AbmeldungNeue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_AbmeldungNeue";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.F_AbmeldungNeue_FormClosed);
            this.Load += new System.EventHandler(this.F_AbmeldungNeue_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPausieren;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbmelden;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbbruch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
    }
}