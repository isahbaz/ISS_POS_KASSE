namespace IS_KASSE
{
    partial class F_Licence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Licence));
            this.btnHayir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMesaj = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHayir
            // 
            this.btnHayir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHayir.Location = new System.Drawing.Point(134, 150);
            this.btnHayir.Name = "btnHayir";
            this.btnHayir.Size = new System.Drawing.Size(309, 77);
            this.btnHayir.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnHayir.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnHayir.TabIndex = 5;
            this.btnHayir.Values.Text = "Jetzt Neustarten!";
            this.btnHayir.Click += new System.EventHandler(this.btnHayir_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnHayir);
            this.panel1.Controls.Add(this.lblMesaj);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 266);
            this.panel1.TabIndex = 3;
            // 
            // lblMesaj
            // 
            this.lblMesaj.BackColor = System.Drawing.Color.LightCyan;
            this.lblMesaj.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.lblMesaj.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMesaj.Location = new System.Drawing.Point(25, 19);
            this.lblMesaj.Name = "lblMesaj";
            this.lblMesaj.Size = new System.Drawing.Size(570, 128);
            this.lblMesaj.TabIndex = 3;
            this.lblMesaj.Text = resources.GetString("lblMesaj.Text");
            this.lblMesaj.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // F_Licence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 266);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Licence";
            this.Text = "F_Licence";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHayir;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lblMesaj;
    }
}