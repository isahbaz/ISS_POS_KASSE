using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IS_KASSE
{
    public partial class F_AchtungAlkohol : Form
    {
        public F_AchtungAlkohol()
        {
            InitializeComponent();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_AchtungAlkohol_Load(object sender, EventArgs e)
        {
            Console.Beep(400, 200);
            Console.Beep(800, 200);
        }
    }
}
