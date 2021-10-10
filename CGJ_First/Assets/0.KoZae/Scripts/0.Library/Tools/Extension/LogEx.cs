using System.Diagnostics;

public enum Log
{
	Normal,                   // 태그 없는 일반 로그용
	
	Player,
	Prefab,

	Map,
	System,
}

public static class LogEx
{
	public static string Prefix(this Log _kind)
	{
		return _kind.Equals(Log.Normal) ? string.Empty : "[" + _kind + "] ";
	}

	public static string Prefix(this Log _kind,string _text)
	{
		return $"{Prefix(_kind)}{_text.GetSafe()}";
	}

	#region I : Info. 유니티나 DEBUG 빌드일때만 로깅
	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void I(this Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
        {
			UnityEngine.Debug.LogFormat(_kind.Prefix(_text),_args);
		}
		else
        {
			UnityEngine.Debug.Log(_kind.Prefix(_text));
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogI(this UnityEngine.Object _context,Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogFormat(_context,_kind.Prefix(_text),_args);
		}
		else
		{
			UnityEngine.Debug.Log(_kind.Prefix(_text),_context);
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogI(this UnityEngine.Object _context,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogFormat(_context,_text,_args);
		}
		else
		{
			UnityEngine.Debug.Log(_text,_context);
		}
	}
	#endregion

	#region W : Warning. 유니티나 DEBUG 빌드일때만 로깅
	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void W(this Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogWarningFormat(_kind.Prefix(_text),_args);
		}
		else
		{
			UnityEngine.Debug.LogWarning(_kind.Prefix(_text));
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogW(this UnityEngine.Object _context,Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogWarningFormat(_context,_kind.Prefix(_text),_args);
		}
		else
		{
			UnityEngine.Debug.LogWarning(_kind.Prefix(_text),_context);
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogW(this UnityEngine.Object _context,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogWarningFormat(_context,_text,_args);
		}
		else
		{
			UnityEngine.Debug.LogWarning(_text,_context);
		}
	}
	#endregion

	#region E : Error. 유니티나 DEBUG 빌드일때만 로깅
	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void E(this Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogErrorFormat(_kind.Prefix(_text),_args);
		}
		else
		{
			UnityEngine.Debug.LogError(_kind.Prefix(_text));
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogE(this UnityEngine.Object _context,Log _kind,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogErrorFormat(_context,_kind.Prefix(_text),_args);
		}
		else
		{
			UnityEngine.Debug.LogError(_kind.Prefix(_text),_context);
		}
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void LogE(this UnityEngine.Object _context,string _text,params object[] _args)
	{
		if(_args.Length > 0)
		{
			UnityEngine.Debug.LogErrorFormat(_context,_text,_args);
		}
		else
		{
			UnityEngine.Debug.LogError(_text,_context);
		}
	}
	#endregion

	#region A : Assert. 유니티나 DEBUG 빌드일때만 로깅
	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void A(this Log _kind,bool _condition,string _text,params object[] _args)
	{
		UnityEngine.Debug.AssertFormat(_condition,_kind.Prefix(_text),_args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void Assert(this UnityEngine.Object _context,Log _kind,bool _condition,string _text,params object[] _args)
	{
		UnityEngine.Debug.AssertFormat(_condition,_context,_kind.Prefix(_text),_args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
	public static void Assert(this UnityEngine.Object _context,bool _condition,string _text,params object[] _args)
	{
		UnityEngine.Debug.AssertFormat(_condition,_context,_text,_args);
	}
	#endregion
}