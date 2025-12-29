using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    class GeraboHistory
    {
        //{"time":"1540553443","comment":null,"change":15,"type":"1"}
        Int32 _time;

        public Int32 Time
        {
            get { return _time; }
            set { _time = value; }
        }
        string _comment;

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        int _change;

        public int Change
        {
            get { return _change; }
            set { _change = value; }
        }
        int _type;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
