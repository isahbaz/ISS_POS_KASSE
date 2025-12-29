namespace IS_KASSE
{
    partial class F_SystemParameter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbbDruckerMode = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lbldruckerMode = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvMonutore = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvGlobal = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvHW = new System.Windows.Forms.DataGridView();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnStopWatch = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonutore)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobal)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHW)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStopWatch);
            this.groupBox1.Controls.Add(this.cbbDruckerMode);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.lbldruckerMode);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.shapeContainer1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 625);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System Hardware Parameter";
            // 
            // cbbDruckerMode
            // 
            this.cbbDruckerMode.FormattingEnabled = true;
            this.cbbDruckerMode.Items.AddRange(new object[] {
            "Immer drucken!",
            "Nicht drucken!",
            "letzter drucken!"});
            this.cbbDruckerMode.Location = new System.Drawing.Point(654, 517);
            this.cbbDruckerMode.Name = "cbbDruckerMode";
            this.cbbDruckerMode.Size = new System.Drawing.Size(144, 21);
            this.cbbDruckerMode.TabIndex = 80;
            this.cbbDruckerMode.Text = "Bitte wählen Sie aus!";
            this.cbbDruckerMode.SelectedIndexChanged += new System.EventHandler(this.cbbDruckerMode_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(666, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 56);
            this.button2.TabIndex = 82;
            this.button2.Text = "Schließen";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbldruckerMode
            // 
            this.lbldruckerMode.AutoSize = true;
            this.lbldruckerMode.Location = new System.Drawing.Point(561, 520);
            this.lbldruckerMode.Name = "lbldruckerMode";
            this.lbldruckerMode.Size = new System.Drawing.Size(78, 13);
            this.lbldruckerMode.TabIndex = 81;
            this.lbldruckerMode.Text = "Drucker Mode:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 406);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(836, 406);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvMonutore);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(828, 380);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Display-Monutor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvMonutore
            // 
            this.dgvMonutore.AllowUserToAddRows = false;
            this.dgvMonutore.AllowUserToDeleteRows = false;
            this.dgvMonutore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonutore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMonutore.Location = new System.Drawing.Point(3, 3);
            this.dgvMonutore.Name = "dgvMonutore";
            this.dgvMonutore.Size = new System.Drawing.Size(822, 374);
            this.dgvMonutore.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvGlobal);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(593, 380);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "HW und Global Einstellungen";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvGlobal
            // 
            this.dgvGlobal.AllowUserToAddRows = false;
            this.dgvGlobal.AllowUserToDeleteRows = false;
            this.dgvGlobal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlobal.Location = new System.Drawing.Point(3, 3);
            this.dgvGlobal.Name = "dgvGlobal";
            this.dgvGlobal.Size = new System.Drawing.Size(587, 374);
            this.dgvGlobal.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvHW);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(593, 380);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "HW und Local Einstellungen";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvHW
            // 
            this.dgvHW.AllowUserToAddRows = false;
            this.dgvHW.AllowUserToDeleteRows = false;
            this.dgvHW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHW.Location = new System.Drawing.Point(3, 3);
            this.dgvHW.Name = "dgvHW";
            this.dgvHW.Size = new System.Drawing.Size(587, 374);
            this.dgvHW.TabIndex = 1;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 16);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(830, 606);
            this.shapeContainer1.TabIndex = 1;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = -2;
            this.lineShape2.X2 = 500;
            this.lineShape2.Y1 = 287;
            this.lineShape2.Y2 = 287;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = -2;
            this.lineShape1.X2 = 824;
            this.lineShape1.Y1 = 50;
            this.lineShape1.Y2 = 50;
            // 
            // btnStopWatch
            // 
            this.btnStopWatch.Location = new System.Drawing.Point(21, 517);
            this.btnStopWatch.Name = "btnStopWatch";
            this.btnStopWatch.Size = new System.Drawing.Size(144, 46);
            this.btnStopWatch.TabIndex = 83;
            this.btnStopWatch.Text = "button1";
            this.btnStopWatch.UseVisualStyleBackColor = true;
            this.btnStopWatch.Click += new System.EventHandler(this.btnStopWatch_Click);
            // 
            // F_SystemParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 625);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_SystemParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SystemParameter";
            this.Load += new System.EventHandler(this.F_SystemParameter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonutore)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobal)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHW)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label lbldruckerMode;
        private System.Windows.Forms.ComboBox cbbDruckerMode;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvMonutore;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvHW;
        private System.Windows.Forms.DataGridView dgvGlobal;
        private System.Windows.Forms.Button btnStopWatch;
    }
}