using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public class DictList<TKey, TValue> : Dictionary<TKey,List<TValue>>
    {
        #region Constructor
        public DictList() : base()
        {

        }

        public DictList(IDictionary<TKey,List<TValue>> _dict) : base(_dict)
        {

        }

        public DictList(IEqualityComparer<TKey> _comparer) : base(_comparer)
        {

        }

        public DictList(int _capacity) : base(_capacity)
        {

        }

        public DictList(IDictionary<TKey,List<TValue>> _dict,IEqualityComparer<TKey> _comparer) : base(_dict,_comparer)
        {

        }

        public DictList(int _capacity,IEqualityComparer<TKey> _comparer) : base(_capacity,_comparer)
        {

        }

        public DictList(TKey _key,IEnumerable<TValue> _values) : base()
        {
            Add(_key,_values.ToList());
        }
        #endregion

        #region Add
        public void Add(TKey _key,IEnumerable<TValue> _values)
        {
            base.Add(_key,_values.ToList());
        }

        public void AddOrCreate(TKey _key,TValue _value)
        {
            if(NotContainsKey(_key))
            {
                Add(_key,new List<TValue>());
            }

            base[_key].Add(_value);
        }
        #endregion

        public bool NotContainsKey(TKey _key)
        {
            return !ContainsKey(_key);
        }

        #region Remove
        public bool RemoveSafe(TKey _key)
        {
            return ContainsKey(_key) && Remove(_key);
        }

        public bool RemoveSafeValueInList(TKey _key,TValue _value)
        {
            return ContainsKey(_key) && base[_key].Remove(_value);
        }
        #endregion

        #region Find
        public TValue FindValueInAllList(Predicate<TValue> _match)
        {
            var keys = Keys.ToArray();

            for(int i = 0;i<keys.Length;i++)
            {
                var list = base[keys[i]];

                var result = list.Find(_match);

                if(result != null)
                {
                    return result;
                }
            }

            return default;
        }

        public KeyValuePair<TKey,TValue> FindPairInAllList(Predicate<TValue> _match)
        {
            var keys = Keys.ToArray();

            for(int i = 0;i<keys.Length;i++)
            {
                var list = base[keys[i]];

                var result = list.Find(_match);

                if(result != null)
                {
                    return new KeyValuePair<TKey,TValue>(keys[i],result);
                }
            }

            return default;
        }
        #endregion

        #region Merge
        public List<TValue> MergeAllValuesToList(params TKey[] _exception)
        {
            var keys = Keys.ToArray();
            var values = new List<TValue>();

            for(int i = 0;i<keys.Length;i++)
            {
                if(_exception == null || !_exception.Any(any => any.Equals(keys[i])))
                {
                    values.AddRange(base[keys[i]]);
                }
            }

            return values;
        }

        public List<TValue> MergeAllValuesToList(Func<TValue,bool> _predicate)
        {
            var keys = Keys.ToArray();
            var values = new List<TValue>();

            for(int i = 0;i<keys.Length;i++)
            {
                var inside = base[keys[i]];

                for(int j = 0;j<inside.Count;j++)
                {
                    if(_predicate(inside[j]))
                    {
                        values.Add(inside[j]);
                    }
                }
            }

            return values;
        }

        public int MergeCount(Func<TValue,bool> _predicate = null)
        {
            var keys = Keys.ToArray();
            int count = 0;

            for(int i = 0;i<keys.Length;i++)
            {
                if(_predicate != null)
                {
                    count += base[keys[i]].Count(_predicate);
                }
                else
                {
                    count += base[keys[i]].Count();
                }
            }

            return count;
        }
        #endregion

        #region Get
        public List<TValue> GetValueByIdx(int _idx)
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

        public TKey GetKeyByIdx(int _idx)
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

        public KeyValuePair<TKey,List<TValue>> GetPairByIdx(int _idx)
        {
            if(Values.Count>_idx && _idx>= 0)
            {
                return new KeyValuePair<TKey,List<TValue>>(Keys.ElementAt(_idx),Values.ElementAt(_idx));
            }
            else
            {
                return default;
            }
        }
        #endregion

        #region First
        public TValue FirstValue()
        {
            if(Values.Count > 0)
            {
                return Values.ElementAt(0).First();
            }
            else
            {
                return default;
            }
        }

        public KeyValuePair<TKey,TValue> FirstPair()
        {
            if(Values.Count > 0)
            {
                return new KeyValuePair<TKey,TValue>(Keys.ElementAt(0),Values.ElementAt(0).First());
            }
            else
            {
                return default;
            }
        }

        public TKey FirstKey()
        {
            if(Keys.Count > 0)
            {
                return Keys.ElementAt(0);
            }
            else
            {
                return default;
            }
        }
        #endregion

        #region Sort

        public void SortEachList<UKey>(Func<TValue,UKey> _keySelector)
        {
            for(int i=0;i<Keys.Count;i++)
            {
                base[Keys.ElementAt(i)] = base[Keys.ElementAt(i)].OrderBy(_keySelector).ToList();
            }
        }

        #endregion

    }
}