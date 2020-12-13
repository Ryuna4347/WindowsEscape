using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib.Develop
{
    public class KZException : Exception
    {
        public KZException() : base()
        {

        }

        public KZException(string _msg) : base(_msg)
        {
            SetMessage(_msg);
        }

        void SetMessage(string _msg)
        {
            _msg = "("+_msg+")";

            if(base.Data.Contains("Message"))
            {
                base.Data["Message"] += _msg;
            }
            else
            {
                base.Data.Add("Message",_msg);
            }
        }

        public override string Message
        {
            get
            {
                if(base.Data.Contains("Message"))
                {
                    return base.Data["Message"].ToString();
                }
                else
                {
                    return base.Message;
                }
            }
        }
    }
}