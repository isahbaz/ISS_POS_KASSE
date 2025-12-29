using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_KASSE
{
    public class KolliKiste
    {
        // SELECT `id`, `artikelbarcode`, `artikelname`, `preis`, `pfandid`, `pfandname`, `inhaltmenge` FROM `kistekoli` WHERE 1
        

        public int Id { get ; set; }
        public string Artikelbarcode { get ; set; }
        public string PfandName { get; set ; }
        public double Preis { get; set ; }
        public double KolliINhalt { get ; set; }
        public string Name { get ; set; }
        public int PfandID { get ; set ; }
    }
}
