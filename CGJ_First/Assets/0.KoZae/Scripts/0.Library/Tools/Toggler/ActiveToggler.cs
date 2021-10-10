using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace UI
{
	[ExecuteInEditMode]
	public class ActiveToggler : Toggler
	{
		[SerializeField]
		List<GameObject> _group = new List<GameObject>();
		public List<GameObject> group { get { return _group; } }

		[SerializeField]
		GameObject _exclusive;
		public GameObject exclusive { get { return _exclusive; } set { _exclusive = value; } }


		public virtual void SetToggle( int idx, bool isOn )
		{
			if( group.Count > idx && group[idx] != null )
				group[idx].SetActive( isOn );
		}

		public virtual bool GetToggle( int idx )
		{
			if( group.Count > idx && group[idx] != null )
				return group[idx].activeInHierarchy;
			return false;
		}

		public virtual void SetToggleEx( GameObject target, bool isOn )
		{
			_exclusive = target;
			SetToggle( isOn );
		}

		public virtual void SetToggleEx( int idx, bool isOn )
		{
			if( group.Count > idx && group[idx] != null )
				SetToggleEx( group[idx], isOn );
		}

		public virtual void SetToggleAll( bool isOn ) {
			if( group != null ) {
				for( int i=0; i<group.Count; i++ )	{
					if( group[i] != null )
						group[i].SetActive( isOn );
				}
			}
		}

		public override void Apply()
		{
			base.Apply();

			if( group != null )
			{
				if( _exclusive != null )
				{
					for( int i=0; i<group.Count; i++ )	{
						if( group[i] != null )
							group[i].SetActive( group[i] == _exclusive ? _Toggle : !_Toggle );
					}
				}
				else
				{
					for( int i=0; i<group.Count; i++ )	{
						if( group[i] != null )
							group[i].SetActive( _Toggle );
					}
				}
			}
		}

		public void ClearGroup()
		{
			group.Clear();
		}

		public GameObject GetElement( int idx )
		{
			if( idx < group.Count )
				return group[idx];
			return null;
		}
	}
}