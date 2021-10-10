using DG.Tweening;
using KZLib.Develop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace KZLib
{
    public class SoundMgr : SingletonMono<SoundMgr>
    {
        [SerializeField] private AudioSource bgm = null;
        [SerializeField] private Transform sfxBox = null;

        private readonly List<AudioSource> sfxs = new List<AudioSource>();
        private readonly DictObject<AudioClip> sounds = new DictObject<AudioClip>();

        protected override void DoAwake()
        {
            var clips = Resources.LoadAll<AudioClip>("Sounds");

            sounds.AddRange(clips);

            if(bgm == null)
            {
                var music = transform.Find("SoundMusic");

                if(music == null)
                {
                    music = new GameObject("SoundMusic").transform;
                    music.parent = transform;
                }

                bgm = music.GetComponent<AudioSource>();

                if(bgm == null)
                {
                    bgm = music.gameObject.AddComponent<AudioSource>();
                }

                bgm.playOnAwake = false;
                bgm.loop = true;
            }
            
            if(sfxBox == null)
            {
                sfxBox = transform.Find("SoundEffectGroup");

                if (sfxBox == null)
                {
                    sfxBox = new GameObject("SoundEffectGroup").transform;
                    sfxBox.parent = transform;
                }
            }
        }

        #region BGM Part

        #region Play
        public bool PlayBGM(string _name,float _volume = 1.0f,float _delay = 0.0f)
        {
            if (bgm.isPlaying)
            {
                if (bgm.clip.name.Equals(_name))
                {
                    return false;
                }

                bgm.Stop();
            }

            if (sounds.TryGetValue(_name,out var audio))
            {
                bgm.clip = audio;
                bgm.loop = true;
                bgm.volume = _volume;

                bgm.name = $"[BGM] {audio.name}";

                if (_delay != 0.0f)
                {
                    bgm.PlayDelayed(_delay);
                }
                else
                {
                    bgm.Play();
                }

                return true;
            }

            return false;
        }
        #endregion

        #region Stop
        public void StopBGM()
        {
            bgm.Stop();
        }
        #endregion

        #region Etc
        public bool IsPlayingBGM()
        {
            return bgm.isPlaying;
        }

        public bool PlayBGMByFadeInOut(string _name,float _in,float _out,float _volume = 1.0f,float _delay = 0.0f)
        {
            if (bgm.isPlaying)
            {
                if (bgm.clip.name.Equals(_name))
                {
                    return false;
                }
            }

            DOTween.Kill(bgm);

            var sequence = DOTween.Sequence().SetId(bgm);

            if (IsPlayingBGM())
            {
                sequence.Append(bgm.DOFade(0.0f,_out).SetEase(Ease.Linear));
            }

            sequence.AppendCallback(() => { PlayBGM(_name,_volume,_delay); });
            sequence.Append(bgm.DOFade(1.0f,_in).SetEase(Ease.Linear));

            return true;
        }

        public void FadeBGM(bool _isIn,float _duration,bool _stopAfterOut = false)
        {
            float volume = _isIn ? 1.0f : 0.0f;

            DOTween.Kill(bgm);

            bgm.DOFade(volume,_duration).OnComplete(() =>
            {
                if (!_isIn && _stopAfterOut)
                {
                    StopBGM();
                }
            });
        }
        #endregion

        #endregion

        #region SFX Part

        #region Play
        public bool PlaySFX(string _no,int _count = 1,float _volume = 1.0f)
        {
            return sounds.TryGetValue(_no,out var clip) && PlaySFX(clip,_count,_volume);
        }

        public bool PlaySFX(AudioClip _clip,int _count = 1,float _volume = 1.0f)
        {
            if (_count.Equals(0))
            {
                return false;
            }
            else
            {
                var source = InitSource(_clip,_volume);

                if (_count != 1)
                {
                    source.loop = true;

                    if (_count > 1)
                    {
                        Scheduler.AddSchedule(new ScheduleData($"[SFX_Sound]{source.name}",0.0f,source.clip.length * _count).OnComplete(() =>
                          {
                              source.loop = false;
                          }));
                    }
                }

                source.Play();

                return true;
            }
        }

        public bool PlaySFXNPlayBGM(string _no,bool _ignoreTimeScale = true,float _volume = 1.0f)
        {
            if (sounds.TryGetValue(_no,out var clip))
            {
                float time = 0.0f;
                var source = InitSource(clip,_volume);
                bgm.Stop();
                source.Play();

                DOTween.To(() => time,x => time = x,clip.length,clip.length).SetUpdate(_ignoreTimeScale).OnComplete(() =>
                  {
                      bgm.Play();
                  });

                return true;
            }

            return false;
        }
        #endregion

        #region Stop
        public void StopSFX(string _name)
        {
            for (int i = 0;i < sfxs.Count;i++)
            {
                if (sfxs[i].clip.name.Equals(_name))
                {
                    sfxs[i].Stop();

                    if (sfxs[i].loop)
                    {
                        Scheduler.RemoveSchedule($"[SFX_Sound]{sfxs[i].name}");
                        sfxs[i].loop = false;
                    }
                }
            }
        }

        public void StopAllSFX()
        {
            for (int i = 0;i < sfxs.Count;i++)
            {
                sfxs[i].Stop();

                if (sfxs[i].loop)
                {
                    Scheduler.RemoveSchedule($"[SFX_Sound]{sfxs[i].name}");
                    sfxs[i].loop = false;
                }
            }
        }
        #endregion

        #region Etc
        AudioSource InitSource(AudioClip _clip,float _volume = 1.0f)
        {
            var source = GetSFX();

            source.name = $"[SFX] {_clip.name}";
            source.clip = _clip;
            source.volume = _volume;
            source.loop = false;

            return source;
        }

        AudioSource GetSFX()
        {
            var sfx = GetEmptySFX();

            return sfx;
        }

        AudioSource GetEmptySFX()
        {
            // 현재 동작중이지 않은 사운드클립의 인덱스를 검색.
            for (int i = 0;i < sfxs.Count;i++)
            {
                if (!sfxs[i].isPlaying)
                {
                    return sfxs[i];
                }
            }

            // 전부다 사용중이므로 신규 생성
            return CreateSFXSource();
        }

        AudioSource CreateSFXSource()
        {
            var sfx = new GameObject("SoundEffect_" + sfxs.Count);
            sfx.transform.parent = sfxBox;

            var source = sfx.AddComponent<AudioSource>();

            sfxs.Add(source);

            return source;
        }
        #endregion

        #endregion

        #region Set Pitch
        public void SetBGMSpeed(float _speed)
        {
            bgm.pitch = _speed;
        }

        public void SetSFXSpeed(string _name,float _speed)
        {
            var sfx = sfxs.Find(fin => fin.clip.name.Equals(_name));

            if (sfx != null)
            {
                sfx.pitch = _speed;
            }
        }
        #endregion

        public void SetBGMVolume(float _volume)
        {
            bgm.volume = _volume;
        }
    }
}