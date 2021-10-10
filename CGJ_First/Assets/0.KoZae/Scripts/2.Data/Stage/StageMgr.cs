using KZLib.Develop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KZLib
{
    public class StageMgr : Singleton<StageMgr>
    {
        private readonly DictObject<GameObject> stages;

        StageMgr()
        {
            try
            {
                stages = new DictObject<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Stages/Use").Where(whe=>whe.name!="Stage_Template"));
            }
            catch (KZException _kex)
            {
                Log.System.E($"Prefab - \n{_kex}");
            }

            if (stages.Count != 0)
            {
                Log.System.I($"Prefab - Load Success!! Count : {stages.Count}");
            }
            else
            {
                Log.System.E("Prefab - Load Fail");
            }
        }

        public DictObject<GameObject> GetAllStages()
        {
            return stages;
        }

        public GameObject FindStage(string _name)
        {
            return stages.TryGetValue(_name,out var data) ? data : null;
        }

        public bool GetNextStage(string _now,out string _result)
        {
            var now = FindStage(_now);
            var next = stages.GetNearValue(_now,1);

            _result = next.name;

            return now != next;
        }
    }
}