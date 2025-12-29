using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class SoldPinlist
    {
        public string cardid { get; set; }
        public string batchnumber { get; set; }
        public string pinnumber { get; set; }
        public string expirydate { get; set; }
        public string purchaseprice { get; set; }
        public string transactionid { get; set; }
        public string CardName { get; set; }
        public string Instruction { get; set; }
        public string rate { get; set; }
        public string custbalance { get; set; }
        public string customercare { get; set; }
      
     //   {"error":{"errorcode":0,"errorstring":""},"data":{"pinslist":[{"cardid":"1079","batchnumber":"123530","pinnumber":"123530","expirydate":"2024-01-09","purchaseprice":"9.6500000","transactionid":"6451571"}],
   // "cardinfo":{"cardid":"1079","cardname":"Test Karte","rate":"10.00","instruction":"*104*Pinnr# und anrufen","cardimage":"https:\/\/www.pjtelesoft.com\/images\/2a33ca0269.jpg"},"custbalance":"984.30"}}
    }
}
