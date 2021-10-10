using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KZLib.Tools;
using DG.Tweening;
using UnityEngine.UI;

namespace KZLib
{
    public class TitleMgr : SingletonOnce<TitleMgr>
    {
        [SerializeField]
        private Image exitImage;

        [SerializeField]
        private GameObject exitText;

        [SerializeField]
        private GameObject exitPrefab;
        private ObjectPool exitPool;

        [SerializeField]
        private GameObject creditPrefab;
        private ObjectPool creditPool;

        [SerializeField]
        private GameObject stagePrefab;
        private ObjectPool stagePool;

        [SerializeField]
        private GameObject inGamePrefab;

        [SerializeField]
        private RectTransform grid;
        [SerializeField]
        private Transform storage;

        private readonly List<WindowsButton> buttons = new List<WindowsButton>();

        public float distance = 0.15f;

        void Awake()
        {
            if(SoundMgr.In.IsPlayingBGM())
            {
                SoundMgr.In.StopBGM();
            }

            exitPool = new ObjectPool(exitPrefab,storage,2);
            creditPool = new ObjectPool(creditPrefab,storage,1);
            stagePool = new ObjectPool(stagePrefab,storage,1);

            exitImage.gameObject.SetActive(false);

            if(PlayerMgr.In.Progress.StartSFX)
            {
                SoundMgr.In.PlaySFX("Windows XP Startup");
            }
        }

        public void OnClickStartBtn()
        {
            var paint = stagePool.Get<StageWindow>(grid);

            SetWindowsButton(paint,false);

            paint.Init((obj) =>
            {
                RemoveWindowsButton(obj,stagePool);
            },PopUpWindowsButton);
        }

        public void OnClickCreditBtn()
        {
            var notepad = creditPool.Get<CreditWindow>(grid);

            SetWindowsButton(notepad,false);

            notepad.Init((obj) =>
            {
                RemoveWindowsButton(obj,creditPool);
            },PopUpWindowsButton);
        }

        public void OnQuitBtn()
        {
            var exit = exitPool.Get<ExitWindow>(grid);

            SetWindowsButton(exit,true);

            exit.Init((obj)=> 
            {
                RemoveWindowsButton(obj,exitPool);
            },PopUpWindowsButton);
        }

        void SetWindowsButton(Window _window,bool _isExit)
        {
            var flag = true;
            var pos = Vector3.zero;

            foreach (var button in buttons)
            {
                if(button.button == null)
                {
                    flag = false;

                    pos = button.pos;

                    button.button = _window.gameObject;

                    break;
                }
            }

            if(flag)
            {
                var data = new WindowsButton
                {
                    button = _window.gameObject,
                    pos = buttons.Count != 0 ? buttons.Last().pos + new Vector3(distance,-distance,0.0f) : Vector3.zero
                };

                pos = data.pos;

                buttons.Add(data);
            }

            _window.GetComponent<RectTransform>().position = _isExit ? pos : Vector3.zero;
        }

        void RemoveWindowsButton(GameObject _obj,ObjectPool _pool)
        {
            var button = FindButton(_obj);

            if(button != null)
            {
                _pool.Put(_obj);

                button.button = null;
            }
        }

        void PopUpWindowsButton(GameObject _obj)
        {
            var button = FindButton(_obj);

            if (button != null)
            {
                button.button.transform.SetAsLastSibling();
            }
        }

        WindowsButton FindButton(GameObject _obj)
        {
            return buttons.Find(fin=>fin.button != null && fin.button.Equals(_obj));
        }

        public Image GetExitImage()
        {
            return exitImage;
        }

        public GameObject GetExitText()
        {
            return exitText;
        }

        public void ActiveInGame(string _name)
        {
            PlayerMgr.In.Progress.NowStageName =_name;
            PlayerMgr.In.Progress.PlayFade = 0;

            Log.System.I($"{_name} 저장 완료");

            var game = Instantiate(inGamePrefab,grid);

            game.transform.localScale = Vector3.one*0.5f;
            game.transform.DOScale(0.99f,0.18f).SetEase(Ease.OutBack,2f).SetUpdate(true).OnComplete(()=> 
            {
                SceneUtil.LoadScene("InGameScene");
            });
        }

        private class WindowsButton
        {
            public Vector3 pos;
            public GameObject button;
        }
    }
}