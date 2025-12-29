namespace IS_KASSE
{
    partial class F_KundenDisplay_V1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_KundenDisplay_V1));
            this.groupBoxTotal = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxPosition = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblLast = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblNeuTotal = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTotal.Panel)).BeginInit();
            this.groupBoxTotal.Panel.SuspendLayout();
            this.groupBoxTotal.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxPosition.Panel)).BeginInit();
            this.groupBoxPosition.Panel.SuspendLayout();
            this.groupBoxPosition.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTotal
            // 
            this.groupBoxTotal.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTotal.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTotal.Margin = new System.Windows.Forms.Padding(50, 3, 50, 0);
            this.groupBoxTotal.Name = "groupBoxTotal";
            // 
            // groupBoxTotal.Panel
            // 
            this.groupBoxTotal.Panel.Controls.Add(this.lblTotal);
            this.groupBoxTotal.Size = new System.Drawing.Size(781, 78);
            this.groupBoxTotal.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.groupBoxTotal.StateCommon.Border.Rounding = 3;
            this.groupBoxTotal.StateNormal.Back.Color1 = System.Drawing.Color.Azure;
            this.groupBoxTotal.StateNormal.Border.Color1 = System.Drawing.Color.DarkBlue;
            this.groupBoxTotal.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.groupBoxTotal.StateNormal.Border.Rounding = 16;
            this.groupBoxTotal.StateNormal.Border.Width = 3;
            this.groupBoxTotal.StateNormal.Content.Padding = new System.Windows.Forms.Padding(-1, 0, -1, 1);
            this.groupBoxTotal.TabIndex = 5;
            this.groupBoxTotal.Values.Heading = "";
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotal.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.Red;
            this.lblTotal.Location = new System.Drawing.Point(0, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(765, 62);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "TOTAL : 109,00€ RUECKGELD: 200,99";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FloralWhite;
            this.panel3.Controls.Add(this.groupBoxPosition);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.groupBoxTotal);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(781, 501);
            this.panel3.TabIndex = 7;
            // 
            // groupBoxPosition
            // 
            this.groupBoxPosition.CaptionStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.groupBoxPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPosition.Location = new System.Drawing.Point(0, 78);
            this.groupBoxPosition.Margin = new System.Windows.Forms.Padding(15, 15, 15, 5);
            this.groupBoxPosition.Name = "groupBoxPosition";
            // 
            // groupBoxPosition.Panel
            // 
            this.groupBoxPosition.Panel.Controls.Add(this.axWindowsMediaPlayer1);
            this.groupBoxPosition.Panel.Controls.Add(this.listView1);
            this.groupBoxPosition.Panel.Controls.Add(this.lblLast);
            this.groupBoxPosition.Size = new System.Drawing.Size(781, 300);
            this.groupBoxPosition.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.groupBoxPosition.StateCommon.Border.Rounding = 3;
            this.groupBoxPosition.StateNormal.Back.Color1 = System.Drawing.Color.OldLace;
            this.groupBoxPosition.StateNormal.Border.Color1 = System.Drawing.Color.DarkGreen;
            this.groupBoxPosition.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.groupBoxPosition.StateNormal.Border.Rounding = 16;
            this.groupBoxPosition.StateNormal.Border.Width = 3;
            this.groupBoxPosition.TabIndex = 8;
            this.groupBoxPosition.Values.Heading = "";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(781, 501);
            this.panel1.TabIndex = 13;
            this.panel1.Visible = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Trebuchet MS", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(182, 320);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 81);
            this.lblTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(96, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(627, 238);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kasse\r\nGeschlossen!\r\n";
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(7, 3);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(267, 227);
            this.axWindowsMediaPlayer1.TabIndex = 14;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(765, 284);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Artikelname";
            this.columnHeader1.Width = 292;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Menge";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 85;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Preis";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 123;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Summe";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 145;
            // 
            // lblLast
            // 
            this.lblLast.BackColor = System.Drawing.Color.Transparent;
            this.lblLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLast.Font = new System.Drawing.Font("Arial Narrow", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLast.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblLast.Location = new System.Drawing.Point(0, 0);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(765, 284);
            this.lblLast.TabIndex = 1;
            this.lblLast.Text = "Herzlich Willkommen!";
            this.lblLast.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblNeuTotal);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 378);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(781, 34);
            this.panel4.TabIndex = 7;
            // 
            // lblNeuTotal
            // 
            this.lblNeuTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNeuTotal.Font = new System.Drawing.Font("Consolas", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNeuTotal.ForeColor = System.Drawing.Color.Red;
            this.lblNeuTotal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNeuTotal.Location = new System.Drawing.Point(131, -7);
            this.lblNeuTotal.Margin = new System.Windows.Forms.Padding(0);
            this.lblNeuTotal.Name = "lblNeuTotal";
            this.lblNeuTotal.Size = new System.Drawing.Size(640, 43);
            this.lblNeuTotal.TabIndex = 0;
            this.lblNeuTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MistyRose;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 412);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(781, 89);
            this.panel2.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // F_KundenDisplay_V1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 501);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_KundenDisplay_V1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_KundenDisplay_V1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_KundenDisplay_V1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTotal.Panel)).EndInit();
            this.groupBoxTotal.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTotal)).EndInit();
            this.groupBoxTotal.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxPosition.Panel)).EndInit();
            this.groupBoxPosition.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxPosition)).EndInit();
            this.groupBoxPosition.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox groupBoxTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblNeuTotal;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox groupBoxPosition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lblLast;
    }
}