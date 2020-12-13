using KZLib;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void PlaySFXBy(string _name)
    {
        SoundMgr.In.PlaySFX(_name);
    }

    public void PlaySFXBy(AudioClip _clip)
    {
        SoundMgr.In.PlaySFX(_clip);
    }
}