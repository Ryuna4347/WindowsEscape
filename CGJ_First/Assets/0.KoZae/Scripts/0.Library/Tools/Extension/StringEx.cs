using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;
using System;

public static class StringEx
{
	/// <summary>
	/// 문자열이 null이 아니고 빈 문자열이 아니면 true를 리턴한다.
	/// </summary>
	public static bool IsOk( this string str )
	{
		return !string.IsNullOrEmpty( str );
	}

	public static string GetSafe( this string str )
	{
		return string.IsNullOrEmpty( str ) ? string.Empty : str;
	}

	public static string GetSafe( this string str, string defaultStr )
	{
		return string.IsNullOrEmpty( str ) ? defaultStr : str;
	}

	public static bool IsEmpty( this string str )
	{
		return string.IsNullOrEmpty( str );
	}

	public static bool IsEqual( this string str1, string str2 )
	{
		return string.Equals( str1, str2 );
	}

	public static bool NotEqual( this string str1, string str2 )
	{
		return !string.Equals( str1, str2 );
	}

	public static string ToSafeStr( this object obj )
	{
		return ( obj == null ) ? "NULL" : obj.ToString();
	}

	public static int Sign( this int val )
	{
		return val < 0 ? -1 : val > 0 ? 1 : 0;
	}

	public static float Sign( this float val )
	{
		return val < 0f ? -1f : val > 0f ? 1f : 0f;
	}

	public static T ToEnum<T>(this string _text)
	{
		return (T) Enum.Parse(typeof(T),_text);
	}	
	
	public static T ToEnum<T>(this string _text,T _default) where T : struct
	{
		return Enum.TryParse(_text,true,out T data) ? data : _default;
	}

	public static bool IsEnumDefined<T>(this string _text)
	{
		return Enum.IsDefined(typeof(T),_text);
	}

	public static Color ToColor(this string _hex)
	{
		return ColorUtility.TryParseHtmlString(_hex,out var color) ? color : Color.white;
	}
}