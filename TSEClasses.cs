using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class TSEClasses
    {
        //INSERT INTO `tse`(`id`, `tse_id`, `tse_serial`, `sig_algo`, `zeit_format`, `pd_encoding`, `public_key`, `zertifikat_i`, `zertifikat_ii`, `datum`)
        public int tseID { get; set; }
        public string tseSerial { get; set; }
        public string tseSigAlgo { get; set; }
        public string tseZeitFormat { get; set; }
        public string tsePDEncoding { get; set; }
        public string tsePublicKey { get; set; }
        public string tseZertifikat1 { get; set; }
        public string tseZertifikat2 { get; set; }
        public string tseDatum { get; set; }
    }
    public class TSELocalInfo
    {
        public string pin { get; set; }
        public string puk { get; set; }
        public string clienID { get; set; }
        public string timeadmin { get; set; }
        public string drive { get; set; }
        public string publicKey { get; set; }
        

    }
    public class KasseInfo
    {
        //INSERT INTO `kasa`(`id`, `kasano`, `kasaad`, `makinaad`, `makinaip`, `KasseHerstNr`, `brand`, `model`, `sw_brand`, `sw_version`, `basis_waehrung`, `keine_ust`)
        public string kasano { get; set; }
        public string kasaad { get; set; }
        public string makinaad { get; set; }
        public string makinaip { get; set; }
        public string KasseHerstNr { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string sw_brand { get; set; }
        public string sw_version { get; set; }
        public string basis_waehrung { get; set; }
        public string keine_ust { get; set; }
        
    }

}
