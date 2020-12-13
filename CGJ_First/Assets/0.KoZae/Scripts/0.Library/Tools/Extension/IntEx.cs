using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntEx
{
	public static bool IsEnumDefined<T>(this int _num)
	{
		return Enum.IsDefined(typeof(T),_num);
	}
}