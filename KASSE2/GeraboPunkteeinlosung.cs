using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public class GeraboPunkteeinlosung
    {
        //{"code":"","account":{"points":null,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[],"history":[],"value":347,
        //"meta":{"code":3,"message":"100 Punkte eingel\u00f6st, Pr\u00e4mie \"1. Tespr\u00e4mie\" gutgeschrieben"},"valuediff":-100,"transaction_id":1830,"verifycode":"********PGj"}

        //{"code":"","account":{"points":null,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[],"history":[],"value":3000,
        //"meta":{"code":3,"message":"<b>+15\u20ac<\/b> (15\u20ac Guthaben und 0\u20ac Bonus erfolgreich aufgeladen. Insg: 30\u20ac und 0\u20ac Bonus )"},"valuediff":1500,"transaction_id":1832,"bonus":0,"verifycode":"********PGj"}
        
        //{"code":"","account":{"points":null,"credits":null,"stamps":null,"postpaid_credits":null},"profile":[],"history":[],"value":"",
        //"meta":{"code":3,"message":"347 Punkte eingel\u00f6st, 3,47\u20ac Guthaben aufgeladen"},"points":0,"pointsdiff":-347,"credits":3347,"creditsdiff":347,"verifycode":"********PGj"}
        string _code;

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        Int32 _points;

        public Int32 points
        {
            get { return _points; }
            set { _points = value; }
        }
        Int32 _pointsdiff;

        public Int32 pointsdiff
        {
            get { return _pointsdiff; }
            set { _pointsdiff = value; }
        }
        Int32 _credits;

        public Int32 credits
        {
            get { return _credits; }
            set { _credits = value; }
        }
        Int32 _creditsdiff;

        public Int32 creditsdiff
        {
            get { return _creditsdiff; }
            set { _creditsdiff = value; }
        }
        Int32 _bonus;

        public Int32 bonus
        {
            get { return _bonus; }
            set { _bonus = value; }
        }


        GeraboAccount _account;

        public GeraboAccount account
        {
            get { return _account; }
            set { _account = value; }
        }
        List<string> _profile;

        public List<string> profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        List<string> _history;

        public List<string> history
        {
            get { return _history; }
            set { _history = value; }
        }
        Int32 _value;

        public Int32 value
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
        Int32 _valuediff;

        public Int32 valuediff
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
    }
}
