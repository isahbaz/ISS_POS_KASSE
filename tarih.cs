using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace IS_KASSE
{
    public class Tarih
    {
        public Int32 unixdateTSE(DateTime date2)
        {
            DateTime date1 = new DateTime(1970, 1, 1,0,0,0);  //Refernzdatum (festgelegt)
            //DateTime date2 = DateTime.Now;              //jetztiges Datum / Uhrzeit
            TimeSpan ts = new TimeSpan(date2.Ticks - date1.Ticks);  // das Delta ermitteln
            // Das Delta als gesammtzahl der sekunden ist der Timestamp
            return (Convert.ToInt32(ts.TotalSeconds));
        }
       
        public Int32 unixdate(DateTime date2)
        {
            DateTime date1 = new DateTime(1970, 1, 1);  //Refernzdatum (festgelegt)
            //DateTime date2 = DateTime.Now;              //jetztiges Datum / Uhrzeit
            TimeSpan ts = new TimeSpan(date2.Ticks - date1.Ticks);  // das Delta ermitteln
            // Das Delta als gesammtzahl der sekunden ist der Timestamp
            return (Convert.ToInt32(ts.TotalSeconds));
        }
        public string tarih(Int64 tarih)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            DateTime convertedDateTime = new DateTime(1970, 1, 1, 0, 0, 0 ).AddSeconds(tarih);//.ToLocalTime();
            return convertedDateTime.ToShortDateString()+" "+convertedDateTime.ToLongTimeString();

        }
        public string TSEtarih(Int64 tarih)
        {
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            
            DateTime convertedDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tarih);
            return convertedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); // ToShortDateString() + " " + convertedDateTime.ToLongTimeString();

        }
        public double bugunBaslangic()
        {
            int gun = DateTime.Now.Day;
            int ay = DateTime.Now.Month;
            int yil = DateTime.Now.Year;
            DateTime date1 = new DateTime(1970, 1, 1);
            DateTime bugun = new DateTime(yil, ay, gun, 0, 0, 1);
            TimeSpan time = new TimeSpan(bugun.Ticks - date1.Ticks);
            return time.TotalSeconds;
        }
        public double bugunBitis()
        {
            int gun = DateTime.Now.Day;
            int ay = DateTime.Now.Month;
            int yil = DateTime.Now.Year;
            DateTime date1 = new DateTime(1970, 1, 1);
            DateTime bugun = new DateTime(yil, ay, gun, 23, 59, 59);
            TimeSpan time = new TimeSpan(bugun.Ticks - date1.Ticks);
            return time.TotalSeconds;
        }
        public double gunBaslangic(int gun, int ay, int yil)
        {
           
            DateTime date1 = new DateTime(1970, 1, 1);
            DateTime bugun = new DateTime(yil, ay, gun, 0, 0, 1);
            TimeSpan time = new TimeSpan(bugun.Ticks - date1.Ticks);
            return time.TotalSeconds;
        }
        public double gunBitis(int gun, int ay, int yil)
        {
            
            DateTime date1 = new DateTime(1970, 1, 1);
            DateTime bugun = new DateTime(yil, ay, gun, 23, 59, 59);
            TimeSpan time = new TimeSpan(bugun.Ticks - date1.Ticks);
            return time.TotalSeconds;
        }
        public string saat(Int64 tarih)
        {
            DateTime convertedDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tarih).ToLocalTime();
            return  convertedDateTime.ToLongTimeString();

        }
        public DateTime KisatarihDateTime(Int64 tarih)
        {

            DateTime convertedDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tarih);
            //convertedDateTime = convertedDateTime.ToUniversalTime();
            return convertedDateTime;


        }
        public double dunBaslangic()
        {
            var dun = DateTime.Now.AddDays(-1);

            int gun = Convert.ToInt16(dun.Day);
            int ay = dun.Month;
            int yil = dun.Year;
            DateTime date1 = new DateTime(1970, 1, 1);
            DateTime bugun = new DateTime(yil, ay, gun, 0, 0, 1);
            TimeSpan time = new TimeSpan(bugun.Ticks - date1.Ticks);
            return time.TotalSeconds;
        }
    }
}
