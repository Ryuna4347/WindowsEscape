using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KZLib
{
    public class OptionWindow : Window
    {
        public void OnHomeBtn()
        {
            PlayerMgr.In.Progress.StartSFX = false;

            SceneUtil.LoadScene("TitleScene");
        }

        public void OnCloseBtn()
        {
            OnExitBtn();
        }

        public void OnRestartBtn()
        {
            SoundMgr.In.PlaySFX("Fail");

            PlayerMgr.In.Progress.PlayFade = 2;

            UIMgr.In.PlayFadeOut(true);
        }

        public void OnBackgroundBtn()
        {
            SoundMgr.In.PlaySFX("Windows XP Exclamation");

            transform.DOShakePosition(0.1f).SetUpdate(true).OnComplete(()=> 
            {
                transform.localPosition = Vector3.zero; 
            });
        }
    }
}