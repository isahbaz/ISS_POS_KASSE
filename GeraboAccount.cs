using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class GeraboAccount
    {
        //{"points":447,"credits":null,"stamps":null,"postpaid_credits":null}
        int _points;

        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }
        int _credits;

        public int Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }
        int _stamps;

        public int Stamps
        {
            get { return _stamps; }
            set { _stamps = value; }
        }
        int _postpaid_credits;

        public int Postpaid_credits
        {
            get { return _postpaid_credits; }
            set { _postpaid_credits = value; }
        }

        
    }
}
