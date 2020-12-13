using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Object를 저장하기 편하게 하기 위한 용도.
/// Object를 Add 하면 Object의 이름을 Key로 만들어서 저장한다.
/// </summary>
public class DictObject<TValue> : DictValue<TValue> where TValue : UnityEngine.Object
{
    #region Constructor
    public DictObject() : base()
    {

    }

    public DictObject(IDictionary<string,TValue> _dict) : base(_dict)
    {

    }

    public DictObject(IEqualityComparer<string> _comparer) : base(_comparer)
    {

    }

    public DictObject(int _capacity) : base(_capacity)
    {

    }

    public DictObject(IDictionary<string,TValue> _dict,IEqualityComparer<string> _comparer) : base(_dict,_comparer)
    {

    }

    public DictObject(int _capacity,IEqualityComparer<string> _comparer) : base(_capacity,_comparer)
    {

    }

    public DictObject(IEnumerable<TValue> _values,Action _option = null) : base()
    {
        AddRange(_values,_option);
    }

    public DictObject(IEnumerable<TValue> _values,Func<TValue,string> _key) : base(_values,_key)
    {

    }

    public DictObject(IEnumerable<TValue> _values,Action<TValue> _option) : base()
    {
        AddRange(_values,_option);
    }
    #endregion

    #region Add

    public void AddRange(IEnumerable<TValue> _values,Action _option = null)
    {
        for(int i=0;i<_values.Count();i++)
        {
            _option?.Invoke();

            Add(_values.ElementAt(i));
        }
    }

    public void AddRange(IEnumerable<TValue> _values,Action<TValue> _option)
    {
        for(int i=0;i<_values.Count();i++)
        {
            var value = _values.ElementAt(i);

            _option?.Invoke(value);

            Add(value);
        }
    }

    public void AddRange<UValue>(DictObject<UValue> _values,Func<UValue,TValue> _option) where UValue : UnityEngine.Object
    {
        for(int i=0;i<_values.Count();i++)
        {
            Add(_option?.Invoke(_values.ElementAt(i).Value));
        }
    }

    public void Add(TValue _value)
    {
        Add(_value.name,_value);
    }
    #endregion
}