using System;
using System.Collections.Generic;

namespace KZLib.Develop
{
    public delegate void BroadcastCallBack();
    public delegate void BroadcastCallBack<T>(T _value);

    public enum TypeOfMessage
    {
        requireReceiver,
        dontRequireReceiver
    }

    static internal class BroadcasterInner
    {
        public static DictValue<Delegate> table = new DictValue<Delegate>();
        public static readonly TypeOfMessage mode = TypeOfMessage.requireReceiver;

        public static void OnListenerEnable(string _name,Delegate _enabled)
        {
            if(table.NotContainsKey(_name))
            {
                table.Add(_name,null);
            }

            var type = table[_name];

            if(type != null && type.GetType() != _enabled.GetType())
            {
                throw new KZException($"listener is error in enable. [{_name} / {type.GetType().Name} / {_enabled.GetType().Name}]");
            }
        }
        public static void OnListenerDisable(string _name,Delegate _disabled)
        {
            if(table.ContainsKey(_name))
            {
                var type = table[_name];

                if(type == null)
                {
                    throw new KZException($"listener is error in disable [{_name} is null]");
                }
                else if(type.GetType() != _disabled.GetType())
                {
                    throw new KZException($"listener is error in disable [{type.GetType()} is not matched {_disabled.GetType()}]");
                }
            }
        }
        public static void OnListenerDisabled(string _name)
        {
            if(table[_name] == null)
            {
                table.Remove(_name);
            }

        }
        public static KZException GenerateMessageException(string _name)
        {
            return new KZException($"Sending message {_name} but listeners have a different kind of signature than the broadcaster.");
        }
        public static void OnBroadcastMessage(string _name,TypeOfMessage _type)
        {
            if(_type == TypeOfMessage.requireReceiver && table.NotContainsKey(_name))
            {
                throw new KZException($"Sending message {_name} but no listener found.");
            }
        }
    }

    public static class Broadcaster
    {
        private static readonly DictValue<Delegate> table = BroadcasterInner.table;

        public static void EnableListener(string _name,BroadcastCallBack _method)
        {
            BroadcasterInner.OnListenerEnable(_name,_method);

            table[_name] = (BroadcastCallBack) table[_name] + _method;
        }

        public static void DisableListener(string _name,BroadcastCallBack _method)
        {
            if(table.ContainsKey(_name))
            {
                BroadcasterInner.OnListenerDisable(_name,_method);
                table[_name] = (BroadcastCallBack) table[_name] - _method;
                BroadcasterInner.OnListenerDisabled(_name);
            }
        }

        public static void SendEvent(string _name)
        {
            SendEvent(_name,BroadcasterInner.mode);

        }
        public static void SendEvent(string _name,TypeOfMessage _type)
        {
            BroadcasterInner.OnBroadcastMessage(_name,_type);

            if(table.TryGetValue(_name,out var type))
            {
                if(type is BroadcastCallBack callback)
                {
                    callback();
                }
                else
                {
                    throw BroadcasterInner.GenerateMessageException(_name);
                }
            }
        }
    }
    public static class Broadcaster<T>
    {
        private static readonly DictValue<Delegate> table = BroadcasterInner.table;

        public static void EnableListener(string _name,BroadcastCallBack<T> _method)
        {
            BroadcasterInner.OnListenerEnable(_name,_method);
            table[_name] = (BroadcastCallBack<T>) table[_name] + _method;
        }
        public static void DisableListener(string _name,BroadcastCallBack<T> _method)
        {
            if(table.ContainsKey(_name))
            {
                BroadcasterInner.OnListenerDisable(_name,_method);
                table[_name] = (BroadcastCallBack<T>) table[_name] - _method;
                BroadcasterInner.OnListenerDisabled(_name);
            }
        }
        public static void SendEvent(string _name,T _value)
        {
            SendEvent(_name,_value,BroadcasterInner.mode);

        }
        public static void SendEvent(string _name,T _value,TypeOfMessage _type)
        {
            BroadcasterInner.OnBroadcastMessage(_name,_type);

            if(table.TryGetValue(_name,out var type))
            {
                if(type is BroadcastCallBack<T> c)
                {
                    c(_value);
                }
                else
                {
                    throw BroadcasterInner.GenerateMessageException(_name);
                }
            }
        }
    }
}