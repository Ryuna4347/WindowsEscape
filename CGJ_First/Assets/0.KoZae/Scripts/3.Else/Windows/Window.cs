using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KZLib
{
    public abstract class Window : MonoBehaviour
    {
        private Action<GameObject> onExit;
        private Action<GameObject> onClicked;

        private RectTransform rectTransform;
        private Vector2 distance;

        protected virtual void DoAwake()
        {

        }
        protected virtual void DoOnEnable()
        {

        }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            DoAwake();
        }

        void OnEnable()
        {
            transform.localScale = Vector3.one;

            transform.DOScale(0.5f,0.2f).From().SetEase(Ease.OutBack,2f).SetUpdate(true);

            DoOnEnable();
        }

        public void Init(Action<GameObject> _onExit,Action<GameObject> _onClicked = null)
        {
            onExit = _onExit;
            onClicked = _onClicked;
        }


        public void OnExitBtn()
        {
            transform.DOScale(0.5f,0.1f).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(() =>
            {
                onExit?.Invoke(gameObject);
            });
        }

        public void OnDrag(BaseEventData _data)
        {
            rectTransform.anchoredPosition = ((PointerEventData)_data).position - distance;
        }

        public void OnBeginDrag(BaseEventData _data)
        {
            distance = ((PointerEventData)_data).position - rectTransform.anchoredPosition;
        }

        public void OnClicked(BaseEventData _data)
        {
            onClicked?.Invoke(gameObject);
        }
    }
}
