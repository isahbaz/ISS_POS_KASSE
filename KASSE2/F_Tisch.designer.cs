namespace IS_KASSE
{
    partial class F_Tisch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Tisch));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.kryptonPanel4 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.btnUc = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCiftSifir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAlti = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSekiz = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDokuz = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBes = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnNokta = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnIki = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSifir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnYedi = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDort = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBir = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnMasaOlustur = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtGiris = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton35 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton5 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton6 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).BeginInit();
            this.kryptonPanel4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1016, 380);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // kryptonPanel4
            // 
            this.kryptonPanel4.Controls.Add(this.btnUc);
            this.kryptonPanel4.Controls.Add(this.btnCiftSifir);
            this.kryptonPanel4.Controls.Add(this.btnAlti);
            this.kryptonPanel4.Controls.Add(this.btnSekiz);
            this.kryptonPanel4.Controls.Add(this.btnDokuz);
            this.kryptonPanel4.Controls.Add(this.btnBes);
            this.kryptonPanel4.Controls.Add(this.btnNokta);
            this.kryptonPanel4.Controls.Add(this.btnIki);
            this.kryptonPanel4.Controls.Add(this.btnSifir);
            this.kryptonPanel4.Controls.Add(this.btnYedi);
            this.kryptonPanel4.Controls.Add(this.btnDort);
            this.kryptonPanel4.Controls.Add(this.btnBir);
            this.kryptonPanel4.Location = new System.Drawing.Point(722, 456);
            this.kryptonPanel4.Name = "kryptonPanel4";
            this.kryptonPanel4.Size = new System.Drawing.Size(294, 287);
            this.kryptonPanel4.TabIndex = 46;
            // 
            // btnUc
            // 
            this.btnUc.Location = new System.Drawing.Point(200, 6);
            this.btnUc.Name = "btnUc";
            this.btnUc.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnUc.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUc.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnUc.Size = new System.Drawing.Size(86, 54);
            this.btnUc.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnUc.StateNormal.Border.Rounding = 5;
            this.btnUc.StateNormal.Border.Width = 5;
            this.btnUc.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnUc.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnUc.TabIndex = 29;
            this.btnUc.Values.Text = "3";
            this.btnUc.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnCiftSifir
            // 
            this.btnCiftSifir.Location = new System.Drawing.Point(101, 215);
            this.btnCiftSifir.Name = "btnCiftSifir";
            this.btnCiftSifir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnCiftSifir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCiftSifir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnCiftSifir.Size = new System.Drawing.Size(86, 70);
            this.btnCiftSifir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCiftSifir.StateNormal.Border.Rounding = 5;
            this.btnCiftSifir.StateNormal.Border.Width = 5;
            this.btnCiftSifir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnCiftSifir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCiftSifir.TabIndex = 28;
            this.btnCiftSifir.Values.Text = "00";
            this.btnCiftSifir.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnAlti
            // 
            this.btnAlti.Location = new System.Drawing.Point(200, 76);
            this.btnAlti.Name = "btnAlti";
            this.btnAlti.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnAlti.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAlti.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnAlti.Size = new System.Drawing.Size(86, 54);
            this.btnAlti.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAlti.StateNormal.Border.Rounding = 5;
            this.btnAlti.StateNormal.Border.Width = 5;
            this.btnAlti.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnAlti.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAlti.TabIndex = 30;
            this.btnAlti.Values.Text = "6";
            this.btnAlti.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnSekiz
            // 
            this.btnSekiz.Location = new System.Drawing.Point(101, 145);
            this.btnSekiz.Name = "btnSekiz";
            this.btnSekiz.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnSekiz.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSekiz.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnSekiz.Size = new System.Drawing.Size(86, 54);
            this.btnSekiz.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSekiz.StateNormal.Border.Rounding = 5;
            this.btnSekiz.StateNormal.Border.Width = 5;
            this.btnSekiz.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnSekiz.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSekiz.TabIndex = 27;
            this.btnSekiz.Values.Text = "8";
            this.btnSekiz.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnDokuz
            // 
            this.btnDokuz.Location = new System.Drawing.Point(200, 145);
            this.btnDokuz.Name = "btnDokuz";
            this.btnDokuz.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnDokuz.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDokuz.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnDokuz.Size = new System.Drawing.Size(86, 54);
            this.btnDokuz.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDokuz.StateNormal.Border.Rounding = 5;
            this.btnDokuz.StateNormal.Border.Width = 5;
            this.btnDokuz.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnDokuz.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDokuz.TabIndex = 31;
            this.btnDokuz.Values.Text = "9";
            this.btnDokuz.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnBes
            // 
            this.btnBes.Location = new System.Drawing.Point(101, 76);
            this.btnBes.Name = "btnBes";
            this.btnBes.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnBes.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBes.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnBes.Size = new System.Drawing.Size(86, 54);
            this.btnBes.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBes.StateNormal.Border.Rounding = 5;
            this.btnBes.StateNormal.Border.Width = 5;
            this.btnBes.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnBes.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBes.TabIndex = 26;
            this.btnBes.Values.Text = "5";
            this.btnBes.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnNokta
            // 
            this.btnNokta.Location = new System.Drawing.Point(200, 215);
            this.btnNokta.Margin = new System.Windows.Forms.Padding(0);
            this.btnNokta.Name = "btnNokta";
            this.btnNokta.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnNokta.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnNokta.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnNokta.Size = new System.Drawing.Size(86, 70);
            this.btnNokta.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnNokta.StateNormal.Border.Rounding = 5;
            this.btnNokta.StateNormal.Border.Width = 5;
            this.btnNokta.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnNokta.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnNokta.TabIndex = 32;
            this.btnNokta.Values.Text = ",";
            this.btnNokta.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnIki
            // 
            this.btnIki.Location = new System.Drawing.Point(101, 6);
            this.btnIki.Margin = new System.Windows.Forms.Padding(0);
            this.btnIki.Name = "btnIki";
            this.btnIki.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnIki.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnIki.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnIki.Size = new System.Drawing.Size(86, 54);
            this.btnIki.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnIki.StateNormal.Border.Rounding = 5;
            this.btnIki.StateNormal.Border.Width = 5;
            this.btnIki.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnIki.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIki.TabIndex = 25;
            this.btnIki.Values.Text = "2";
            this.btnIki.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnSifir
            // 
            this.btnSifir.Location = new System.Drawing.Point(2, 215);
            this.btnSifir.Name = "btnSifir";
            this.btnSifir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnSifir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSifir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnSifir.Size = new System.Drawing.Size(86, 70);
            this.btnSifir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSifir.StateNormal.Border.Rounding = 5;
            this.btnSifir.StateNormal.Border.Width = 5;
            this.btnSifir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnSifir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSifir.TabIndex = 24;
            this.btnSifir.Values.Text = "0";
            this.btnSifir.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnYedi
            // 
            this.btnYedi.Location = new System.Drawing.Point(2, 145);
            this.btnYedi.Name = "btnYedi";
            this.btnYedi.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnYedi.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYedi.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnYedi.Size = new System.Drawing.Size(86, 54);
            this.btnYedi.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnYedi.StateNormal.Border.Rounding = 5;
            this.btnYedi.StateNormal.Border.Width = 5;
            this.btnYedi.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnYedi.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYedi.TabIndex = 23;
            this.btnYedi.Values.Text = "7";
            this.btnYedi.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnDort
            // 
            this.btnDort.Location = new System.Drawing.Point(2, 76);
            this.btnDort.Name = "btnDort";
            this.btnDort.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnDort.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDort.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnDort.Size = new System.Drawing.Size(86, 54);
            this.btnDort.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDort.StateNormal.Border.Rounding = 5;
            this.btnDort.StateNormal.Border.Width = 5;
            this.btnDort.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnDort.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDort.TabIndex = 22;
            this.btnDort.Values.Text = "4";
            this.btnDort.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnBir
            // 
            this.btnBir.Location = new System.Drawing.Point(2, 6);
            this.btnBir.Name = "btnBir";
            this.btnBir.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnBir.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBir.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnBir.Size = new System.Drawing.Size(86, 54);
            this.btnBir.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnBir.StateNormal.Border.Rounding = 5;
            this.btnBir.StateNormal.Border.Width = 5;
            this.btnBir.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnBir.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBir.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBir.TabIndex = 21;
            this.btnBir.Values.Text = "1";
            this.btnBir.Click += new System.EventHandler(this.btnBir_Click);
            // 
            // btnMasaOlustur
            // 
            this.btnMasaOlustur.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.btnMasaOlustur.Location = new System.Drawing.Point(722, 381);
            this.btnMasaOlustur.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasaOlustur.Name = "btnMasaOlustur";
            this.btnMasaOlustur.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.btnMasaOlustur.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnMasaOlustur.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.btnMasaOlustur.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnMasaOlustur.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.btnMasaOlustur.Size = new System.Drawing.Size(108, 71);
            this.btnMasaOlustur.StateCommon.Back.Image = ((System.Drawing.Image)(resources.GetObject("btnMasaOlustur.StateCommon.Back.Image")));
            this.btnMasaOlustur.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnMasaOlustur.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMasaOlustur.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.btnMasaOlustur.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnMasaOlustur.StateNormal.Back.ColorAngle = 50F;
            this.btnMasaOlustur.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnMasaOlustur.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.btnMasaOlustur.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.btnMasaOlustur.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.btnMasaOlustur.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.btnMasaOlustur.StateNormal.Border.ColorAngle = 5F;
            this.btnMasaOlustur.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnMasaOlustur.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnMasaOlustur.StateNormal.Border.Rounding = 5;
            this.btnMasaOlustur.StateNormal.Border.Width = 5;
            this.btnMasaOlustur.StateNormal.Content.AdjacentGap = 0;
            this.btnMasaOlustur.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnMasaOlustur.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnMasaOlustur.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.btnMasaOlustur.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMasaOlustur.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.btnMasaOlustur.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.btnMasaOlustur.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnMasaOlustur.StateTracking.Border.Rounding = 2;
            this.btnMasaOlustur.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMasaOlustur.TabIndex = 53;
            this.btnMasaOlustur.Values.Text = "";
            this.btnMasaOlustur.Click += new System.EventHandler(this.btnMasaOlustur_Click);
            // 
            // txtGiris
            // 
            this.txtGiris.Location = new System.Drawing.Point(834, 381);
            this.txtGiris.MaxLength = 4;
            this.txtGiris.Multiline = true;
            this.txtGiris.Name = "txtGiris";
            this.txtGiris.Size = new System.Drawing.Size(182, 71);
            this.txtGiris.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtGiris.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGiris.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtGiris.StateCommon.Content.Color1 = System.Drawing.Color.Black;
            this.txtGiris.StateCommon.Content.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtGiris.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGiris.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGiris.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtGiris.StateNormal.Content.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtGiris.TabIndex = 57;
            this.txtGiris.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.kryptonButton1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 380);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(638, 364);
            this.flowLayoutPanel2.TabIndex = 58;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(3, 3);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonButton1.Size = new System.Drawing.Size(162, 96);
            this.kryptonButton1.StateNormal.Back.Color1 = System.Drawing.SystemColors.HighlightText;
            this.kryptonButton1.StateNormal.Back.Color2 = System.Drawing.SystemColors.ActiveCaption;
            this.kryptonButton1.TabIndex = 0;
            this.kryptonButton1.Values.Text = "kryptonButton1";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // kryptonButton35
            // 
            this.kryptonButton35.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton35.Location = new System.Drawing.Point(641, 382);
            this.kryptonButton35.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton35.Name = "kryptonButton35";
            this.kryptonButton35.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton35.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton35.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton35.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton35.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton35.Size = new System.Drawing.Size(77, 71);
            this.kryptonButton35.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton35.StateCommon.Border.Rounding = 0;
            this.kryptonButton35.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton35.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton35.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton35.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton35.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton35.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton35.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton35.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonButton35.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.kryptonButton35.StateNormal.Border.ColorAngle = 5F;
            this.kryptonButton35.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton35.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton35.StateNormal.Border.Rounding = 5;
            this.kryptonButton35.StateNormal.Border.Width = 5;
            this.kryptonButton35.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton35.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton35.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton35.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton35.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton35.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton35.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton35.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton35.StateTracking.Border.Rounding = 2;
            this.kryptonButton35.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton35.TabIndex = 59;
            this.kryptonButton35.Values.Text = "C";
            this.kryptonButton35.Click += new System.EventHandler(this.kryptonButton35_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton2.Location = new System.Drawing.Point(641, 454);
            this.kryptonButton2.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton2.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton2.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton2.Size = new System.Drawing.Size(77, 71);
            this.kryptonButton2.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.archives;
            this.kryptonButton2.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateCommon.Border.Rounding = 0;
            this.kryptonButton2.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton2.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton2.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton2.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton2.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Back.Image = global::IS_KASSE.Properties.Resources.archives;
            this.kryptonButton2.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton2.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonButton2.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.kryptonButton2.StateNormal.Border.ColorAngle = 5F;
            this.kryptonButton2.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton2.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateNormal.Border.Rounding = 5;
            this.kryptonButton2.StateNormal.Border.Width = 5;
            this.kryptonButton2.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton2.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton2.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton2.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton2.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton2.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton2.StateTracking.Border.Rounding = 2;
            this.kryptonButton2.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton2.TabIndex = 60;
            this.kryptonButton2.Values.Text = "";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton3.Location = new System.Drawing.Point(641, 526);
            this.kryptonButton3.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton3.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton3.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton3.Size = new System.Drawing.Size(77, 71);
            this.kryptonButton3.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.user;
            this.kryptonButton3.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.StateCommon.Border.Rounding = 0;
            this.kryptonButton3.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton3.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton3.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton3.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton3.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton3.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonButton3.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.kryptonButton3.StateNormal.Border.ColorAngle = 5F;
            this.kryptonButton3.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton3.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.StateNormal.Border.Rounding = 5;
            this.kryptonButton3.StateNormal.Border.Width = 5;
            this.kryptonButton3.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton3.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton3.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton3.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton3.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton3.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton3.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton3.StateTracking.Border.Rounding = 2;
            this.kryptonButton3.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton3.TabIndex = 61;
            this.kryptonButton3.Values.Text = "";
            this.kryptonButton3.Click += new System.EventHandler(this.kryptonButton3_Click);
            // 
            // kryptonButton5
            // 
            this.kryptonButton5.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton5.Location = new System.Drawing.Point(642, 598);
            this.kryptonButton5.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton5.Name = "kryptonButton5";
            this.kryptonButton5.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton5.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton5.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton5.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton5.Size = new System.Drawing.Size(77, 71);
            this.kryptonButton5.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.kasse;
            this.kryptonButton5.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton5.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton5.StateCommon.Border.Rounding = 0;
            this.kryptonButton5.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton5.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton5.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton5.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton5.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton5.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton5.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Stretch;
            this.kryptonButton5.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonButton5.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.kryptonButton5.StateNormal.Border.ColorAngle = 5F;
            this.kryptonButton5.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton5.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton5.StateNormal.Border.Rounding = 5;
            this.kryptonButton5.StateNormal.Border.Width = 5;
            this.kryptonButton5.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton5.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton5.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton5.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton5.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton5.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton5.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton5.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton5.StateTracking.Border.Rounding = 2;
            this.kryptonButton5.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton5.TabIndex = 62;
            this.kryptonButton5.Values.Text = "";
            this.kryptonButton5.Click += new System.EventHandler(this.kryptonButton5_Click);
            // 
            // kryptonButton6
            // 
            this.kryptonButton6.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1;
            this.kryptonButton6.Location = new System.Drawing.Point(641, 670);
            this.kryptonButton6.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonButton6.Name = "kryptonButton6";
            this.kryptonButton6.OverrideDefault.Border.Color1 = System.Drawing.Color.Fuchsia;
            this.kryptonButton6.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton6.OverrideFocus.Border.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.kryptonButton6.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton6.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton6.Size = new System.Drawing.Size(77, 71);
            this.kryptonButton6.StateCommon.Back.Image = global::IS_KASSE.Properties.Resources.logout;
            this.kryptonButton6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton6.StateCommon.Border.Rounding = 0;
            this.kryptonButton6.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kryptonButton6.StateNormal.Back.Color2 = System.Drawing.Color.DarkKhaki;
            this.kryptonButton6.StateNormal.Back.ColorAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton6.StateNormal.Back.ColorAngle = 50F;
            this.kryptonButton6.StateNormal.Back.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton6.StateNormal.Back.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton6.StateNormal.Back.Image")));
            this.kryptonButton6.StateNormal.Back.ImageAlign = ComponentFactory.Krypton.Toolkit.PaletteRectangleAlign.Local;
            this.kryptonButton6.StateNormal.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.kryptonButton6.StateNormal.Border.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonButton6.StateNormal.Border.Color2 = System.Drawing.Color.DarkGreen;
            this.kryptonButton6.StateNormal.Border.ColorAngle = 5F;
            this.kryptonButton6.StateNormal.Border.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton6.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton6.StateNormal.Border.Rounding = 5;
            this.kryptonButton6.StateNormal.Border.Width = 5;
            this.kryptonButton6.StateNormal.Content.AdjacentGap = 0;
            this.kryptonButton6.StateNormal.Content.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton6.StateNormal.Content.DrawFocus = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton6.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.Red;
            this.kryptonButton6.StateNormal.Content.ShortText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton6.StateTracking.Border.Color1 = System.Drawing.Color.Red;
            this.kryptonButton6.StateTracking.Border.Color2 = System.Drawing.Color.Red;
            this.kryptonButton6.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton6.StateTracking.Border.Rounding = 2;
            this.kryptonButton6.StateTracking.Content.ShortText.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kryptonButton6.TabIndex = 63;
            this.kryptonButton6.Values.Text = "";
            this.kryptonButton6.Click += new System.EventHandler(this.kryptonButton6_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // F_Tisch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(1016, 744);
            this.Controls.Add(this.kryptonButton6);
            this.Controls.Add(this.kryptonButton5);
            this.Controls.Add(this.kryptonButton3);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.kryptonButton35);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.txtGiris);
            this.Controls.Add(this.btnMasaOlustur);
            this.Controls.Add(this.kryptonPanel4);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Tisch";
            this.Text = "F_Tisch";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_Tisch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).EndInit();
            this.kryptonPanel4.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUc;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCiftSifir;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAlti;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSekiz;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDokuz;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBes;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNokta;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnIki;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSifir;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnYedi;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDort;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBir;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnMasaOlustur;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGiris;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton35;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton6;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}