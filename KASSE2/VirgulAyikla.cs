using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class VirgulAyikla
    {
        public string virgulayikla(double text)
        {
           // text = Convert.ToString(text);   
            if (text.ToString().IndexOf(",") != -1)
            {
                string donen = text.ToString().Replace(",", ".");
                return donen;
            }
            else if (text == null )
            {
                return "0.00";
            }
            else
            {
                return text.ToString();
            }
        }
    }
}
