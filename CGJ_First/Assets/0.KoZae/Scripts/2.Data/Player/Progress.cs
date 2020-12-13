using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KZLib.Data.Player
{
    public class Progress : PLHandler<Progress.DataBase>
    {
        public class DataBase : PLDataBase
        {
            public int NowStage { get; private set; }

            public List<string> Stages { get; private set; }

            public override void Init()
            {
                if(Stages == null)
                {
                    Stages = new List<string>();
                }
            }

            public void SetStage(int _stage)
            {
                NowStage = _stage;
            }
        }

        public void StageClear(string _stageName)
        {
            if (!IsClear(_stageName))
            {
                DB.Stages.Add(_stageName);
            }
        }

        public bool IsClear(string _stageName)
        {
            return DB.Stages.Any(any=>any.Equals(_stageName));
        }

        public void SetNowStage(int _stage) => DB.SetStage(_stage);
        public int GetNowStage() => DB.NowStage;
    }
}