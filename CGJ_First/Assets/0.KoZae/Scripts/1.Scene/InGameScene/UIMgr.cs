using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace KZLib
{
    public class UIMgr : SingletonOnce<UIMgr>
    {
        [Header("Option")]
        [SerializeField]
        private GameObject optionPan;
        [SerializeField]
        private OptionWindow optionWindow;

        [Header("Help")]
        [SerializeField]
        private GameObject helpPan;
        [SerializeField]
        private HelpWindow helpWindow;

        [Header("Fade")]
        [SerializeField]
        private UITransitionEffect fadeEffect;

        [SerializeField]
        private Texture successTexture;
        [SerializeField]
        private Texture failTexture;

        private readonly DictValue<Toggler> togglers = new DictValue<Toggler>();

        protected override void DoAwake()
        {
            base.DoAwake();

            foreach (var data in GetComponentsInChildren<Toggler>(true))
            {
                if(togglers.NotContainsKey(data.name))
                {
                    togglers.Add(data.name,data);
                }
            }

            optionPan.SetActive(false);
            helpPan.SetActive(false);

            SetTime(1.0f);

            optionWindow.Init((obj)=>
            {
                optionPan.SetActive(false);

                SetTime(1.0f);
            });

            helpWindow.Init((obj) =>
            {
                helpPan.SetActive(false);

                SetTime(1.0f);
            });

            if (PlayerMgr.In.Progress.PlayFade > 0)
            {
                fadeEffect.gameObject.SetActive(true);                
                fadeEffect.transitionTexture = PlayerMgr.In.Progress.PlayFade == 1 ? successTexture : failTexture;
                fadeEffect.effectFactor = 1.0f;
            }
            else
            {
                fadeEffect.gameObject.SetActive(false);

                InGameMgr.In.IsStart = true;
            }

        }

        void Start()
        {
            if (PlayerMgr.In.Progress.PlayFade > 0)
            {
                DOTween.To(() => fadeEffect.effectFactor,x => fadeEffect.effectFactor = x,0.0f,1.0f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
                {
                    InGameMgr.In.IsStart = true;
                });
            }
        }


        public void PlayFadeOut(bool _reload)
        {
            fadeEffect.gameObject.SetActive(true);
            fadeEffect.transitionTexture = PlayerMgr.In.Progress.PlayFade == 1 ? successTexture : failTexture;
            fadeEffect.effectFactor = 0.0f;

            DOTween.To(()=>fadeEffect.effectFactor,x=>fadeEffect.effectFactor=x,1.0f,1.0f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.KillAll();

                if(_reload)
                {
                    SceneUtil.ReLoadScene();
                }
                else
                {
                    PlayerMgr.In.Progress.StartSFX = false;

                    SceneUtil.LoadScene("EndingScene");
                }
            });
        }

        public void ActiveColor(string _color,bool _active)
        {
            if (togglers.TryGetValue(_color,out var toggler))
            {
                toggler.SetToggle(_active);

                MoveButton(toggler);
            }
        }

        public void OnCilckedBtn(string _name)
        {
            if (togglers.TryGetValue(_name,out var toggler))
            {
                toggler.Toggle();

                InGameMgr.In.ToggleLayerInMapCon(_name);

                MoveButton(toggler);
            }
        }

        void MoveButton(Toggler _toggler)
        {
            _toggler.transform.DOLocalMoveY(_toggler._Toggle ? 0.0f : -15.0f,0.1f).SetEase(Ease.OutSine);
        }

        void Update()
        {
            if(InGameMgr.In.IsStart)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    // 1번 레이어 변경
                    OnCilckedBtn("Magenta_Layer");
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    // 2번 레이어 변경
                    OnCilckedBtn("Yellow_Layer");
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    // 3번 레이어 변경
                    OnCilckedBtn("Cyan_Layer");
                }
                else if(Input.GetKeyDown(KeyCode.Escape))
                {
                    OnExitBtn();
                }
                else if (Input.GetKeyDown(KeyCode.F1))
                {
                    OnHelpBtn();
                }
            }
        }

        public void OnExitBtn()
        {
            optionPan.SetActive(true);

            SetTime(0.0f);
        }

        public void OnHelpBtn()
        {
            helpPan.SetActive(true);

            SetTime(0.0f);
        }

        void SetTime(float _time)
        {
            Time.timeScale = _time;
            Time.fixedDeltaTime = _time*0.02f;
        }
    }
}