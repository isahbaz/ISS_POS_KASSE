namespace IS_KASSE
{
    partial class F_MultiStafelSelect
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProduktName = new System.Windows.Forms.Label();
            this.btnAuswahl = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblProduktName);
            this.panel1.Controls.Add(this.btnAuswahl);
            this.panel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(1183, 72);
            this.panel1.TabIndex = 0;
            // 
            // lblProduktName
            // 
            this.lblProduktName.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduktName.ForeColor = System.Drawing.Color.Red;
            this.lblProduktName.Location = new System.Drawing.Point(30, 9);
            this.lblProduktName.Name = "lblProduktName";
            this.lblProduktName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblProduktName.Size = new System.Drawing.Size(579, 49);
            this.lblProduktName.TabIndex = 1;
            this.lblProduktName.Text = "Welches Angebot wollen Sie annehmen?\r\n(Fragen an der Kunde?)";
            // 
            // btnAuswahl
            // 
            this.btnAuswahl.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuswahl.Location = new System.Drawing.Point(1014, 9);
            this.btnAuswahl.Margin = new System.Windows.Forms.Padding(4);
            this.btnAuswahl.Name = "btnAuswahl";
            this.btnAuswahl.Size = new System.Drawing.Size(152, 49);
            this.btnAuswahl.TabIndex = 0;
            this.btnAuswahl.Text = "Schliessen";
            this.btnAuswahl.UseVisualStyleBackColor = true;
            this.btnAuswahl.Click += new System.EventHandler(this.btnAuswahl_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1200, 623);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // F_MultiStafelSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 623);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_MultiStafelSelect";
            this.Text = "F_MultiStafelSelect";
            this.Load += new System.EventHandler(this.F_MultiStafelSelect_Load);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProduktName;
        private System.Windows.Forms.Button btnAuswahl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}