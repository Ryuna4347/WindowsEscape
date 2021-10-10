using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KZLib
{
    public class HelpWindow : Window
    {
        public void OnBackgroundBtn()
        {
            SoundMgr.In.PlaySFX("Windows XP Exclamation");

            transform.DOShakePosition(0.1f).SetUpdate(true).OnComplete(() => { transform.localPosition = Vector3.zero; });
        }
    }
}