using KZLib.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class StageWindow : Window
    {
        public GameObject prefab;
        public ObjectPool pool;

        public RectTransform grid;
        public Transform storage;

        private readonly List<StageButton> slots = new List<StageButton>();

        protected override void DoAwake()
        {
            var stages = StageMgr.In.GetAllStages();
            var last = -1;

            pool = new ObjectPool(prefab,storage,stages.Count);

            for(var i=0;i<stages.Count;i++)
            {
                var pair = stages.GetPairByIdx(i);

                var slot = pool.Get<StageButton>(grid);
                var state = PlayerMgr.In.Progress.IsClear(pair.Key) ? Stage_kind.UNLOCK : Stage_kind.LOCK;

                if (state.Equals(Stage_kind.UNLOCK))
                {
                    last = i;
                }

                slot.SetState(state,pair.Key,i);

                slots.Add(slot);
            }

            if (slots.TryGetValue(last+1,out var value))
            {
                value.SetState(Stage_kind.UNLOCK,stages.GetKeyByIdx(last+1),last+1);
            }
        }
    }
}