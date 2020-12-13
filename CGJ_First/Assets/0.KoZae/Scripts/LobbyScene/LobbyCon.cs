using KZLib.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class LobbyCon : MonoBehaviour
    {
        public GameObject prefab;
        public ObjectPool pool;

        void Awake()
        {
            var count = PrefabMgr.In.StageCount;


        }
    }
}