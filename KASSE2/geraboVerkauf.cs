using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    class geraboVerkauf
    {
        private string _apikey;

        public string apikey
        {
            get { return _apikey; }
            set { _apikey = value; }
        }
        private string _kod;

        public string code
        {
            get { return _kod; }
            set { _kod = value; }
        }
        private string _volume;

        public string volume
        {
            get { return _volume; }
            set { _volume = value; }
        }
    }
}
