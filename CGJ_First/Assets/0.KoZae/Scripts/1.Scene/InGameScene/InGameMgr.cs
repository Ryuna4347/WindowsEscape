using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class InGameMgr : SingletonOnce<InGameMgr>
    {
        public bool IsTest { get; set; }

        public bool IsStart { get; set; } = false;

        [SerializeField]
        private MapCon mapCon;

        protected override void DoAwake()
        {
            base.DoAwake();

            mapCon.Init();

            SoundMgr.In.PlayBGM("BGM",0.1f);
        }

        public void ToggleLayerInMapCon(string _layer)
        {
            mapCon.ToggleLayer(_layer);
        }

        public void SendToBoxInMapCon(GameObject _obj)
        {
            mapCon.SendToBox(_obj);
        }

        public void EndGame(bool _isClear)
        {
            if (IsTest)
            {
                Log.System.I($"Stage {(_isClear ? "성공" : "실패")}");
            }
            else
            {
                if(_isClear)
                {
                    var pivot = mapCon.SetStageClear();

                    if (pivot.Equals(+1))
                    {
                        PlayerMgr.In.Progress.PlayFade = 1;

                        UIMgr.In.PlayFadeOut(true);
                    }
                    else if (pivot.Equals(-1))
                    {
                        PlayerMgr.In.Progress.PlayFade = 1;

                        UIMgr.In.PlayFadeOut(false);
                    }
                }
                else
                {
                    SoundMgr.In.PlaySFX("Fail");

                    PlayerMgr.In.Progress.PlayFade = 2;

                    UIMgr.In.PlayFadeOut(true);
                }
            }
        }
    }
}