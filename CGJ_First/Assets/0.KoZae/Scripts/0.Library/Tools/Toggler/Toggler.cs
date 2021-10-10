using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UI
{
	interface IToggler
	{
		void Apply();
		void SetToggle( bool isOn );
		void Toggle();
	}

	[ExecuteInEditMode]
	public class Toggler : MonoBehaviour, IToggler
	{
		[SerializeField]
		protected bool _toggle;

		[SerializeField]
		protected bool _inverseSelf;

		public bool _dontApplyInEditor;

		[SerializeField]
		Toggler _exclusiveChild;

		public bool _Toggle {
			get { return _inverseSelf ? !_toggle : _toggle; }
		}
		[SerializeField]
		List<Toggler> _child = new List<Toggler>();
		public List<Toggler> child { get { return _child; } }


		public virtual void Apply()
		{
			if( child != null ) {
				if( _exclusiveChild != null ) {
					for( int i=0; i<child.Count; i++ ) {
						if( child[i] != null )
							child[i].SetToggle( child[i] == _exclusiveChild ? _toggle : !_toggle );
					}
				} else {
					for( int i=0; i<child.Count; i++ ) {
						if( child[i] != null )
							child[i].SetToggle( _toggle );
					}
				}
			}
		}

		public virtual void SetToggle( bool isOn )
		{
			_toggle = isOn;
			Apply();
		}

		public virtual void Toggle()
		{
			_toggle = !_toggle;
			Apply();
		}

		#region Child Operation
		public virtual void AddTogglersInChildren()
		{
			GetComponentsInChildren<Toggler>( true, child );
			child.Remove( this );
		}

		public virtual void AddTogglersInChildren<T>() where T : Toggler
		{
			List<T> res = new List<T>();
			GetComponentsInChildren<T>( true, res );
			res.Remove( this as T );

			foreach( var e in res )
				child.Add( e as Toggler );
		}

		public virtual void SetToggleChild( int childIdx, bool isOn )
		{
			Debug.AssertFormat( childIdx >= 0 && childIdx < child.Count, this, "wrong idx[{0}] - count:{1}", childIdx, child.Count );
			child[childIdx].SetToggle( isOn );
		}

		public virtual void SetToggleChildEx( Toggler target, bool isOn )
		{
			_exclusiveChild = target;
			SetToggle( isOn );
		}

		public virtual void SetToggleChildEx( int childIdx, bool isOn )
		{
			Debug.AssertFormat( childIdx >= 0 && childIdx < child.Count, this, "wrong idx[{0}] - count:{1}", childIdx, child.Count );
			SetToggleChildEx( child[childIdx], isOn );
		}
		#endregion

#if UNITY_EDITOR
		bool? _prevToggle;

//		void Update()
//		{
//			if( !Application.isPlaying && !_dontApplyInEditor )
//			{
//				if( _prevToggle != _toggle )
//				{
//					_prevToggle = _toggle;
//					Apply();
//				}
//			}
//		}

		void Reset()
		{
			Apply();
		}

		void OnValidate()
		{
			if( !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !_dontApplyInEditor ) {
				Apply();
			}
		}
#endif
	}
}