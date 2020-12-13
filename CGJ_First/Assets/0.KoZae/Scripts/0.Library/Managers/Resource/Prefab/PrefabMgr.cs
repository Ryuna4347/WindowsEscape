using KZLib.Develop;
using UnityEngine;

namespace KZLib
{
    public class PrefabMgr : Singleton<PrefabMgr>
    {
        private readonly DictObject<GameObject> prefabs = new DictObject<GameObject>();

        PrefabMgr()
        {
            var text = "";

            try
            {
                prefabs.AddRange(Resources.LoadAll<GameObject>("Prefabs"));

                text = "데이터 로드 성공!!!";
            }
            catch (KZException _kex)
            {
                text = $"에러 \n{_kex}";
            }
            finally
            {
                Debug.Log(text);
            }
        }

        public GameObject FindPrefab(string _name) => prefabs.TryGetValue(_name,out var prefab) ? prefab : null;
    }
}