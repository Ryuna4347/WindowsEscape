using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KZLib
{
    public class ExitWindow : Window
    {
        private float dummy = 0.0f;

        public void OnYesBtn()
        {
            TitleMgr.In.GetExitImage().gameObject.SetActive(true);

            SoundMgr.In.PlaySFX("Windows XP Shutdown");

            TitleMgr.In.GetExitImage().DOColor(Color.black,1.0f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                DOTween.To(()=>dummy,x=>dummy=x,0.0f,3.0f).SetDelay(1.0f).OnStart(()=> 
                {
                    TitleMgr.In.GetExitText().gameObject.SetActive(true);

                }).OnComplete(()=> 
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
                });
            });
        }
    }
}