namespace IS_KASSE
{
    partial class F_PunkteOderRabat
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
            this.btnHayir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEvet = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblMesaj = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHayir
            // 
            this.btnHayir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHayir.Location = new System.Drawing.Point(361, 36);
            this.btnHayir.Name = "btnHayir";
            this.btnHayir.Size = new System.Drawing.Size(188, 191);
            this.btnHayir.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnHayir.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnHayir.TabIndex = 5;
            this.btnHayir.Values.Text = "RABATT";
            this.btnHayir.Click += new System.EventHandler(this.btnHayir_Click);
            // 
            // btnEvet
            // 
            this.btnEvet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEvet.Location = new System.Drawing.Point(11, 36);
            this.btnEvet.Name = "btnEvet";
            this.btnEvet.Size = new System.Drawing.Size(188, 191);
            this.btnEvet.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Green;
            this.btnEvet.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEvet.TabIndex = 4;
            this.btnEvet.Values.Text = "PUNKTE ";
            this.btnEvet.Click += new System.EventHandler(this.btnEvet_Click);
            // 
            // lblMesaj
            // 
            this.lblMesaj.BackColor = System.Drawing.Color.LightCyan;
            this.lblMesaj.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.lblMesaj.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMesaj.Location = new System.Drawing.Point(205, 108);
            this.lblMesaj.Name = "lblMesaj";
            this.lblMesaj.Size = new System.Drawing.Size(111, 55);
            this.lblMesaj.TabIndex = 3;
            this.lblMesaj.Text = "ODER";
            this.lblMesaj.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnHayir);
            this.panel1.Controls.Add(this.btnEvet);
            this.panel1.Controls.Add(this.lblMesaj);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 266);
            this.panel1.TabIndex = 2;
            // 
            // F_PunkteOderRabat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 266);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_PunkteOderRabat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_PunkteOderRabat";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHayir;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEvet;
        public System.Windows.Forms.Label lblMesaj;
        private System.Windows.Forms.Panel panel1;
    }
}