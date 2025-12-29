namespace IS_KASSE
{
    partial class F_ObstGemuseManual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_ObstGemuseManual));
            this.panel2 = new System.Windows.Forms.Panel();
            this.flp2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flp = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.MintCream;
            this.panel2.Name = "panel2";
            // 
            // flp2
            // 
            resources.ApplyResources(this.flp2, "flp2");
            this.flp2.BackColor = System.Drawing.Color.MintCream;
            this.flp2.Name = "flp2";
            // 
            // flp
            // 
            resources.ApplyResources(this.flp, "flp");
            this.flp.BackColor = System.Drawing.Color.MintCream;
            this.flp.Name = "flp";
            // 
            // F_ObstGemuseManual
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flp);
            this.Controls.Add(this.flp2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_ObstGemuseManual";
            this.Load += new System.EventHandler(this.F_ObstGemuse_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flp2;
        private System.Windows.Forms.FlowLayoutPanel flp;

    }
}