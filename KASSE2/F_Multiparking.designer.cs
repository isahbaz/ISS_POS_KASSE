namespace IS_KASSE
{
    partial class F_Multiparking
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
            this.flp1 = new System.Windows.Forms.FlowLayoutPanel();

            this.flp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flp1
            // 
            this.flp1.Controls.Add(this.btnBon);
            this.flp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp1.Location = new System.Drawing.Point(0, 0);
            this.flp1.Name = "flp1";
            this.flp1.Size = new System.Drawing.Size(1050, 601);
            this.flp1.TabIndex = 0;
            // 
            // btnBon
            // 

            // F_Multiparking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 601);
            this.Controls.Add(this.flp1);
            this.Name = "F_Multiparking";
            this.Text = "F_Multiparking";
            this.Load += new System.EventHandler(this.F_Multiparking_Load);
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.Name = "F_LicenceError";
            //this.Text = "F_LicenceError";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.flp1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBon;
    }
}