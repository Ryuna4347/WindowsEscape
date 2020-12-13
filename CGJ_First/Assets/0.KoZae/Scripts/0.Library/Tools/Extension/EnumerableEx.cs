using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableEx
{
    public static bool ContainsIndex<T>(this IEnumerable<T> _source,int _idx)
    {
        return (0<=_idx && _idx < _source.Count());
    }

    public static bool TryGetValue<T>(this IEnumerable<T> _source,int _idx,out T _value)
    {
        if(0<=_idx && _idx < _source.Count())
        {
            _value = _source.ElementAt(_idx);

            return true;
        }
        else
        {
            _value = default;

            return false;
        }
    }

    public static bool Exist<T>(this IEnumerable<T> _source,Func<T,bool> _predicate)
    {
        for(int i=0;i<_source.Count();i++)
        {
            if(_predicate(_source.ElementAt(i)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool NotExist<T>(this IEnumerable<T> _source,Func<T,bool> _predicate)
    {
        for(int i=0;i<_source.Count();i++)
        {
            if(_predicate(_source.ElementAt(i)))
            {
                return false;
            }
        }

        return true;
    }

    public static int FindIndex<T>(this IEnumerable<T> _source,Func<T,bool> _predicate)
    {
        for(int i=0;i<_source.Count();i++)
        {
            if(_predicate(_source.ElementAt(i)))
            {
                return i;
            }
        }

        return -1;
    }

    public static DictValue<TSource> ToDictValue<TSource>(this IEnumerable<TSource> _sources,Func<TSource,string> _key)
    {
        var dict = new DictValue<TSource>();

        for(int i=0;i<_sources.Count();i++)
        {
            var source = _sources.ElementAt(i);

            dict.Add(_key(source),source);
        }

        return dict;
    }

    public static DictValue<TElement> ToDictValue<TSource,TElement>(this IEnumerable<TSource> _sources,Func<TSource,string> _key,Func<TSource,TElement> _element)
    {
        var dict = new DictValue<TElement>();

        for(int i=0;i<_sources.Count();i++)
        {
            var source = _sources.ElementAt(i);

            dict.Add(_key(source),_element(source));
        }

        return dict;
    }
}