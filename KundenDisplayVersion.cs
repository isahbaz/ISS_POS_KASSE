using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IS_KASSE
{
    public class KundenDisplayVersion
    {
        private Form FormVers;

        public Form FormVers1
        {
            get { return FormVers; }
            set { FormVers = value; }
        }
        public Form FormTercih()
        {
            return this.FormVers1;
        }
    }
}
