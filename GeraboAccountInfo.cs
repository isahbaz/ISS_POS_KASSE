using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class GeraboAccountInfo
    {
       // {"code":"UjIg@guBPGj","account":{"points":447,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[],"history":[{"time":"1540553443","comment":null,"change":15,"type":"1"},{"time":"1540552973","comment":null,"change":15,"type":"1"},{"time":"1540552700","comment":null,"change":15,"type":"1"},{"time":"1540551982","comment":null,"change":15,"type":"1"},{"time":"1540550240","comment":null,"change":15,"type":"1"},{"time":"1540550121","comment":null,"change":15,"type":"1"},
    //{"time":"1540550099","comment":null,"change":15,"type":"1"},{"time":"1540549900","comment":null,"change":15,"type":"1"},{"time":"1540549892","comment":null,"change":15,"type":"1"},{"time":"1540549661","comment":null,"change":15,"type":"1"}],"value":"","meta":{"code":3,"message":""},"isvip":false,"verifycode":"********PGj"}
       // {"code":"g9FY@1rHU7k","account":{"points":null,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[{"id":"69","name":"Name","value":""}],"history":[],"value":"","meta":{"code":3,"message":""},"isvip":false,"verifycode":"********U7k"}
        string _code;

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        GeraboAccount _account;

        public GeraboAccount account
        {
            get { return _account; }
            set { _account = value; }
        }
        List<GeraboProfile> _profile;

        public List<GeraboProfile> profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        GeraboHistory _history;

        internal GeraboHistory history
        {
            get { return _history; }
            set { _history = value; }
        }
        string _value;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
        GeraboMeta _meta;

        public GeraboMeta meta
        {
            get { return _meta; }
            set { _meta = value; }
        }
        Boolean _isvip;

        public Boolean isvip
        {
            get { return _isvip; }
            set { _isvip = value; }
        }
        string _verifycode;

        public string verifycode
        {
            get { return _verifycode; }
            set { _verifycode = value; }
        }

    }
}
