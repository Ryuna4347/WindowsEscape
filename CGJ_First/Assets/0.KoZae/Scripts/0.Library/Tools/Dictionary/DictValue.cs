using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Collections.Generic
{
    public class DictValue<TValue> : Dictionary<string,TValue>
    {
        #region Constructor
        public DictValue() : base()
        {

        }

        public DictValue(IDictionary<string,TValue> _dict) : base(_dict)
        {

        }

        public DictValue(IEqualityComparer<string> _comparer) : base(_comparer)
        {

        }

        public DictValue(int _capacity) : base(_capacity)
        {

        }

        public DictValue(IDictionary<string,TValue> _dict,IEqualityComparer<string> _comparer) : base(_dict,_comparer)
        {

        }

        public DictValue(int _capacity,IEqualityComparer<string> _comparer) : base(_capacity,_comparer)
        {

        }

        public DictValue(IEnumerable<TValue> _values,Func<TValue,string> _key) : base()
        {
            AddRange(_values,_key);
        }
        #endregion

        #region Add
        public void AddRange(Dictionary<string,TValue> _values)
        {
            var keys = _values.Keys.ToArray();

            for(int i=0;i<keys.Length;i++)
            {
                Add(keys[i],_values[keys[i]]);
            }
        }

        public void AddRange(IEnumerable<TValue> _values,Func<TValue,string> _key)
        {
            for(int i=0;i<_values.Count();i++)
            {
                var value = _values.ElementAt(i);

                Add(_key?.Invoke(value),value);
            }
        }
        #endregion

        #region Get
        public TValue GetOrDefalut(string _key)
        {
            return _key != null && ContainsKey(_key) ? base[_key] : (default);
        }

        public TValue GetNearValue(string _key,int _near)
        {
            var keys = Keys.ToList();

            for(int i=0;i<keys.Count;i++)
            {
                if(keys[i] == _key)
                {
                    var idx = Mathf.Clamp(i+_near,0,keys.Count-1);

                    return this[keys[idx]];
                }
            }

            return default;
        }

        public string GetKeyByIdx(int _idx)
        {
            if(Keys.Count>_idx && _idx>= 0)
            {
                return Keys.ElementAt(_idx);
            }
            else
            {
                return default;
            }
        }

        public TValue GetValueByIdx(int _idx)
        {
            if(Values.Count>_idx && _idx>= 0)
            {
                return Values.ElementAt(_idx);
            }
            else
            {
                return default;
            }
        }

        public KeyValuePair<string,TValue> GetPairByIdx(int _idx)
        {
            if(Values.Count>_idx && _idx>= 0)
            {
                return new KeyValuePair<string,TValue>(Keys.ElementAt(_idx),Values.ElementAt(_idx));
            }
            else
            {
                return default;
            }
        }

        public KeyValuePair<string,TValue> GetMaxPair()
        {
            return new KeyValuePair<string,TValue>(Keys.Max(),Values.Max());
        }
        #endregion

        #region Find
        public List<TValue> FindValuesToList(Func<TValue,bool> _predicate)
        {
            var values = new List<TValue>();

            for(int i=0;i<Values.Count;i++)
            {
                var value = Values.ElementAt(i);

                if(_predicate(value))
                {
                    values.Add(value);
                }
            }

            return values;
        }
        #endregion

        public bool NotContainsKey(string _key)
        {
            return _key != null && !ContainsKey(_key);
        }

        public bool RemoveSafe(string _key)
        {
            return ContainsKey(_key) && Remove(_key);
        }        
    }
}