namespace IS_KASSE
{
    partial class F_HandAufladeDAOnay
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblNr1 = new System.Windows.Forms.Label();
            this.lblNr2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(90, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ihre Handynummer:";
            // 
            // lblNr1
            // 
            this.lblNr1.AutoSize = true;
            this.lblNr1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNr1.ForeColor = System.Drawing.Color.Red;
            this.lblNr1.Location = new System.Drawing.Point(136, 114);
            this.lblNr1.Name = "lblNr1";
            this.lblNr1.Size = new System.Drawing.Size(248, 40);
            this.lblNr1.TabIndex = 1;
            this.lblNr1.Text = "01785422452";
            // 
            // lblNr2
            // 
            this.lblNr2.AutoSize = true;
            this.lblNr2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNr2.ForeColor = System.Drawing.Color.Red;
            this.lblNr2.Location = new System.Drawing.Point(136, 223);
            this.lblNr2.Name = "lblNr2";
            this.lblNr2.Size = new System.Drawing.Size(248, 40);
            this.lblNr2.TabIndex = 3;
            this.lblNr2.Text = "01785422452";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(136, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 40);
            this.label4.TabIndex = 2;
            this.label4.Text = "Bestätigung:";
            // 
            // F_HandAufladeDAOnay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(549, 363);
            this.Controls.Add(this.lblNr2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNr1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "F_HandAufladeDAOnay";
            this.Text = "F_HandAufladeDAOnay";
            this.Load += new System.EventHandler(this.F_HandAufladeDAOnay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNr1;
        private System.Windows.Forms.Label lblNr2;
        private System.Windows.Forms.Label label4;
    }
}