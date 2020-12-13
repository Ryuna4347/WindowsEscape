using KZLib.Develop;
using UnityEngine;

namespace KZLib
{
    public class PrefabMgr : Singleton<PrefabMgr>
    {
        private readonly DictObject<GameObject> prefabs;

        public int StageCount { get; private set; }

        PrefabMgr()
        {
            var text = "";

            try
            {
                var datas = Resources.LoadAll<GameObject>("Prefabs/Stages");

                StageCount = datas.Length;

                prefabs = new DictObject<GameObject>(datas);
                prefabs.AddRange(Resources.LoadAll<GameObject>("Prefabs/Else"));

                text = "데이터 로드 성공!!!";
            }
            catch (KZException _kex)
            {
                text = $"에러 \n{_kex}";
            }
            finally
            {
                Log.Prefab.I(text);
            }
        }

        public GameObject FindPrefab(string _name) => prefabs.TryGetValue(_name,out var prefab) ? prefab : null;
    }
}