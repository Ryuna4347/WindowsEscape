using UnityEngine;

public static class GameObjectEx
{
	#region MakeInstance
	public static T MakeInstance<T>(this GameObject _prefab) where T : Component
	{
		return _prefab.MakeInstance().GetComponent<T>();
	}

	public static T MakeInstance<T>(this GameObject _prefab,Transform _root) where T : Component
	{
		return _prefab.MakeInstance(_root).GetComponent<T>();
	}

	public static GameObject MakeInstance(this GameObject _prefab,Transform _root = null)
	{
        var obj = Object.Instantiate(_prefab);

		if(_root == null)
        {
			_root = _prefab.transform.parent;
		}

		obj.transform.SetParent(_root,false);

		return obj;
	}
    #endregion

    #region ResetTransform
    public static void ResetTransform(this Transform _tm,Transform _parent = null)
	{
		if(_parent != null)
		{
			_tm.SetParent(_parent);
		}

		ResetTransformInside(_tm);
	}

	public static void ResetTransform(this GameObject _obj)
	{
		ResetTransformInside(_obj.transform);
	}

	public static void ResetTransform(this GameObject _obj,GameObject _parent)
	{
		var tm = _obj.transform;

		tm.SetParent(_parent.transform);

		ResetTransformInside(tm);

		_obj.layer = _parent.layer;
	}

	static void ResetTransformInside(Transform _tm)
	{
		_tm.localPosition = Vector3.zero;
		_tm.localRotation = Quaternion.identity;
		_tm.localScale = Vector3.one;
	}
	#endregion

	#region Child
	public static Transform AppendChild(this Transform _tm,string _name)
	{
		var tm = new GameObject(_name).transform;

		tm.SetParent(_tm,true);

		return tm;
	}

	public static void AddChild(this GameObject _obj,GameObject _child)
	{
		AddChildInside(_obj,_child,true);
	}

	public static void AddUIChild(this GameObject _obj,GameObject _child)
	{
		AddChildInside(_obj,_child,false);
	}

	static void AddChildInside(GameObject _parent,GameObject _child,bool _stay)
	{
		_child.transform.SetParent(_parent.transform,_stay);
		_child.layer = _parent.layer;
	}

    /// <summary>
    /// prefab로부터 Instance를 만들고 Parent의 자식으로 만든다.
    /// 자식의 transform값이 변할 수 있다. (월드상의 현재 위치와 스케일을 유지하기 위해 부모에 대한 상대 값들로 변경된다.)
    /// 자식은 부모의 레이어와 동일하게 설정한다.
    /// </summary>
    /// <returns>prefab를 원본으로 만들어진 instance.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_prefab">Prefab.</param>
    public static GameObject NewChild(GameObject _parent,GameObject _prefab)
	{
        var obj = Object.Instantiate(_prefab);

		AddChildInside(_parent,obj,true);

		return obj;
	}

	/// <summary>
	/// prefab로부터 Instance를 만들고 Parent의 자식으로 만든다.
	/// 자식의 transform값을 유지한다.
	/// 자식은 부모의 레이어와 동일하게 설정한다.
	/// </summary>
	public static GameObject NewUIChild(GameObject _parent,GameObject _prefab)
	{
		var obj = Object.Instantiate(_prefab);

		AddChildInside(_parent,obj,false);

		return obj;
	}
	#endregion
}
