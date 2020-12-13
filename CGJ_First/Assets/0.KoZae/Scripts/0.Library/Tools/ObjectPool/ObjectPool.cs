using System.Collections.Generic;
using UnityEngine;

namespace KZLib.Tools
{
	public class ObjectPool
	{
		private readonly Queue<GameObject> pool;
		private readonly GameObject prefab;
		private readonly Transform storage;

		public ObjectPool(GameObject _prefab,Transform _storage,int _capacity,int _count = 0)
		{
			prefab = _prefab;
			pool = new Queue<GameObject>(_capacity);
			storage = _storage;

			for (int i=0;i<_count;i++)
			{
				InputPool(CreateInstance(storage));
			}
		}

		GameObject CreateInstance(Transform _parent = null)
		{
			return prefab.MakeInstance(_parent);
		}

		/// <summary>
		/// pool에 인스턴스를 환원 시킨다.
		/// </summary>
		#region Put
		void InputPool(GameObject _obj)
		{
			_obj.transform.SetParent(storage);
			_obj.SetActive(false);
			pool.Enqueue(_obj);
		}

		public void Put(GameObject _obj)
		{
			InputPool(_obj);
		}
        #endregion

        /// <summary>
        /// pool에서 인스턴스를 하나 꺼낸다.
        /// 만약 pool에 여유분이 없으면 새로 생성한다.
        /// </summary>
        #region Get
        GameObject GetPool(Transform _parent = null)
		{
			var obj = pool.Dequeue();
			obj.SetActive(true);

			if (_parent != null)
			{
				obj.transform.SetParent(_parent);
			}

			return obj;
		}

		public GameObject Get(Transform _parent = null)
		{
            return pool.Count > 0 ? GetPool(_parent) : CreateInstance(_parent);
        }

		public T Get<T>(Transform _parent = null) where T : Component
		{
            return Get(_parent).GetComponent<T>();
        }
        #endregion
    }
}

