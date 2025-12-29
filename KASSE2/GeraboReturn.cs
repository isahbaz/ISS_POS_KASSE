using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    //{"code":"","account":{"points":null,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[],"history":[],"value":387,"meta":{"code":3,"message":""},"valuediff":15,"transaction_id":1825,"verifycode":"********PGj"}
    public class GeraboRoot
    {
        string _code;

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        List<string> _profile;

        public List<string> Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }


        List<string> _history;

        public List<string> History
        {
            get { return _history; }
            set { _history = value; }
        }

        
        int _value;

        public int value
        {
            get { return _value; }
            set { _value = value; }
        }
        int _valuediff;

        public int valuediff
        {
            get { return _valuediff; }
            set { _valuediff = value; }
        }
        int _transaction_id;

        public int transaction_id
        {
            get { return _transaction_id; }
            set { _transaction_id = value; }
        }
        string _verifycode;

        public string verifycode
        {
            get { return _verifycode; }
            set { _verifycode = value; }
        }
        GeraboAccount _account;

        public GeraboAccount account
        {
            get { return _account; }
            set { _account = value; }
        }
        GeraboMeta _meta;

        public GeraboMeta meta
        {
            get { return _meta; }
            set { _meta = value; }
        }
    }
    

   
}
