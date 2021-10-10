using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class EdgeMover : MonoBehaviour
    {
        private RectTransform rectTransform;

        public float speed = 20.0f;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            rectTransform.anchoredPosition = new Vector3(0.0f,-45.0f);

            rectTransform.DOKill();

            MoveRotate();
        }

        void MoveRotate()
        {
            rectTransform.DOMoveX(17.2f,10.0f).SetSpeedBased(true).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
            {
                rectTransform.DOMoveY(-7.2f,10.0f).SetSpeedBased(true).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
                {
                    rectTransform.DOMoveX(42.8f,10.0f).SetSpeedBased(true).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
                    {
                        rectTransform.DOMoveY(3.8f,10.0f).SetSpeedBased(true).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(MoveRotate);
                    });
                });
            });
        }
    }
}