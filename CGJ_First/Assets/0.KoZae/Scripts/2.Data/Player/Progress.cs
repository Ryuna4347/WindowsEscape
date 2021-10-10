using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KZLib.Data.Player
{
    //public class Progress : PLHandler<Progress.DataBase>
    //{
    //    public class DataBase : PLDataBase
    //    {
            

            

    //        public override void Init()
    //        {
    //            if(Stages == null)
    //            {
    //                Stages = new List<string>();
    //            }

    //            NowStageName = null;
    //            PlayFade = -1;
    //            StartSFX = false;
    //        }
    //    }

    //    public void StageClear(string _stageName)
    //    {
    //        if (!IsClear(_stageName))
    //        {
    //            DB.Stages.Add(_stageName);
    //        }
    //    }

    //    public bool IsClear(string _stageName)
    //    {
    //        return DB.Stages.Any(any=>any.Equals(_stageName));
    //    }

    //    public void SetNowStageName(string _name) => DB.NowStageName = _name;

    //    public string GetNowStageName() => DB.NowStageName;

    //    public void SetShowIntro() => DB.ShowIntro = true;

    //    public bool GetShowIntro() => DB.ShowIntro;

    //    public void SetPlayFade(int _fade) => DB.PlayFade = _fade;

    //    public int IsPlayFade() => DB.PlayFade;

    //    public void SetSFX(bool _sfx) => DB.StartSFX = _sfx;

    //    public bool IsSFX() => DB.StartSFX;
    //}
}