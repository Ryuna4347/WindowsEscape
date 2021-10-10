using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KZLib
{
    
    public class EndingCon : MonoBehaviour
    {
        public EndingPlayer player;
        public Image fade;

        public Text text;
        public Image speek;

        public SpriteRenderer mouse;
        public Transform finish;
        public Transform finish2;

        public Sprite mImage1;
        public Sprite mImage2;

        public GameObject exit_pop;
        public ExitWindow exit;

        public GameObject end;

        void Start()
        {
            SoundMgr.In.PlayBGM("EndingBGM",0.4f);

            mouse.transform.position = Vector3.zero;
            speek.gameObject.SetActive(false);
            
            exit_pop.SetActive(false);

            fade.color = Color.black;

            fade.DOFade(0.0f,1.0f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                StartCoroutine(PlayEnding());
            });
        }

        IEnumerator PlayEnding()
        {
            yield return new WaitForSeconds(0.3f);

            speek.gameObject.SetActive(true);

            text.text = "?";

            yield return player.Play(text);

            text.text = "뭐야!";

            mouse.transform.DOMove(finish.position,1.0f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                text.text = "돌려줘!!!";

                player.GetComponent<Animator>().Play("Hit");

                mouse.sprite = mImage2;

                StartCoroutine(Last());
            });
        }

        IEnumerator Last()
        {
            yield return new WaitForSeconds(0.5f);

            mouse.sprite = mImage1;

            exit_pop.SetActive(true);

            mouse.transform.DOMove(finish2.position,1.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                mouse.sprite = mImage2;

                var dummy = 0.0f;

                fade.DOColor(Color.black,1.0f).SetDelay(1.0f).SetEase(Ease.Linear).OnStart(()=> 
                {
                    mouse.sprite = mImage1;

                    SoundMgr.In.PlaySFX("Windows XP Shutdown");
                }).OnComplete(() =>
                {
                    DOTween.To(()=>dummy,x=>dummy=x,0.0f,2.0f).SetDelay(1.0f).OnStart(() =>
                    {
                        end.SetActive(true);
                    }).OnComplete(() =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
                    });
                });
            });
        }
    }
}
