using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KZLib
{
    public class IntroCon : MonoBehaviour
    {
        public Text dateText;

        public GameObject alarmPop_up;

        [Header("Intro")]
        public GameObject introPanel;
        public GameObject mailPop_up;

        [Header("Video")]
        public GameObject videoPanel;

        [Header("Loading")]
        public GameObject loadingPanel;
        public Transform loadingGroup;

        private readonly List<GameObject> loadings = new List<GameObject>();

        [Header("Skip")]
        public GameObject skipPanel;

        IEnumerator Start()
        {
            SoundMgr.In.PlayBGM("InCut",0.3f);

            alarmPop_up.SetActive(false);

            skipPanel.SetActive(PlayerMgr.In.Progress.ShowIntro);

            introPanel.SetActive(true);
            mailPop_up.SetActive(false);

            videoPanel.SetActive(false);
            videoPanel.GetComponent<VideoPlayer>().loopPointReached += OnMovieFinished;

            loadingPanel.SetActive(false);

            for(int i=0;i<loadingGroup.childCount;i++)
            {
                var loading = loadingGroup.GetChild(i).gameObject;

                loading.SetActive(false);

                loadings.Add(loading);
            }

            yield return new WaitForSeconds(1.0f);

            alarmPop_up.SetActive(true);

            SoundMgr.In.SetBGMVolume(0.1f);

            alarmPop_up.transform.DOMoveX(6.2f,0.5f).SetEase(Ease.Linear);
            SoundMgr.In.PlaySFX("Windows Notify");
        }

        void Update()
        {
            dateText.text = DateTime.Now.ToString("tt hh:mm \n yyyy-MM-dd");
        }

        public void OnClickedAlarm()
        {
            mailPop_up.SetActive(true);
        }

        public void OnClickedApp()
        {
            SoundMgr.In.StopBGM();

            videoPanel.SetActive(true);
            introPanel.SetActive(false);
            mailPop_up.SetActive(false);
        }

        void OnMovieFinished(VideoPlayer _player)
        {
            _player.Stop();

            videoPanel.SetActive(false);
            loadingPanel.SetActive(true);

            StartCoroutine(PlayLoading());
        }
        
        IEnumerator PlayLoading()
        {
            for (int i=0;i<loadings.Count;i++)
            {
                loadings[i].SetActive(true);

                if(i.Equals(Mathf.RoundToInt(loadings.Count/3*2)))
                {
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            loadingPanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            PlayerMgr.In.Progress.ShowIntro = true;

            PlayerMgr.In.SaveData();

            OnClickedSkip();
        }

        public void OnClickedSkip()
        {
            PlayerMgr.In.Progress.StartSFX = true;

            SceneUtil.LoadScene("TitleScene");
        }
    }
}