namespace IS_KASSE
{
    partial class F_GenericSoru
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_GenericSoru));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHayir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEvet = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblMesaj = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnHayir);
            this.panel1.Controls.Add(this.btnEvet);
            this.panel1.Controls.Add(this.lblMesaj);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Name = "panel1";
            // 
            // btnHayir
            // 
            resources.ApplyResources(this.btnHayir, "btnHayir");
            this.btnHayir.Name = "btnHayir";
            this.btnHayir.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnHayir.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnHayir.Values.ExtraText = resources.GetString("btnHayir.Values.ExtraText");
            this.btnHayir.Values.ImageTransparentColor = ((System.Drawing.Color)(resources.GetObject("btnHayir.Values.ImageTransparentColor")));
            this.btnHayir.Values.Text = resources.GetString("btnHayir.Values.Text");
            this.btnHayir.Click += new System.EventHandler(this.btnHayir_Click);
            // 
            // btnEvet
            // 
            resources.ApplyResources(this.btnEvet, "btnEvet");
            this.btnEvet.Name = "btnEvet";
            this.btnEvet.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Green;
            this.btnEvet.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEvet.Values.ExtraText = resources.GetString("btnEvet.Values.ExtraText");
            this.btnEvet.Values.ImageTransparentColor = ((System.Drawing.Color)(resources.GetObject("btnEvet.Values.ImageTransparentColor")));
            this.btnEvet.Values.Text = resources.GetString("btnEvet.Values.Text");
            this.btnEvet.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // lblMesaj
            // 
            resources.ApplyResources(this.lblMesaj, "lblMesaj");
            this.lblMesaj.BackColor = System.Drawing.Color.LightCyan;
            this.lblMesaj.Name = "lblMesaj";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackgroundImage = global::IS_KASSE.Properties.Resources.soru_isareti;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // F_GenericSoru
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_GenericSoru";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEvet;
        public System.Windows.Forms.Label lblMesaj;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHayir;
    }
}