using System;
using UnityEngine;

namespace KZLib.Data.Effect
{
    public abstract class GEObject<T> : MonoBehaviour, IGEObject where T : IGEObjectData
    {
        public float duration = 0.0f;

        protected T data;
        private Action<GameObject> OnEnd = null;
        
        protected virtual void DoOnEnable() { }

        public float Duration => duration;

        public virtual void InitData(Action<GameObject> _onEnd)
        {
            // duringTime이 -1일 경우 영구적
            OnEnd = _onEnd;
        }

        public virtual void SetData(object _data)
        {
            if(_data is T)
            {
                data = (T) _data;
            }
        }

        void OnEnable()
        {
            DoOnEnable();

            if(duration >= 0.0f)
            {
                Invoke(nameof(EndEffect),duration);
            }
        }

        void OnDisable()
        {
            CancelInvoke("EndEffect");
        }

        public void EndEffect()
        {
            OnEnd?.Invoke(gameObject);
        }

        public virtual void ResetWithParent(Transform _parent,Vector3? _scale = null)
        {
            var scale = transform.localScale;

            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;

            transform.localScale = _scale ?? scale;
        }
    }

    public interface IGEObject
    {
        float Duration { get; }

        void InitData(Action<GameObject> _onEnd);
        void SetData(object _data);

        void ResetWithParent(Transform _parent,Vector3? _scale = null);
        void EndEffect();
    }

    public interface IGEObjectData
    {

    }
}