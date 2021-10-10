using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KZLib
{
    public class MapCon : SerializedMonoBehaviour
    {
        private readonly DictValue<GameObject> layers = new DictValue<GameObject>();

        private CharacterMove hero = null;
        private readonly List<BoxGroup> boxes = new List<BoxGroup>();

        private string stageName;

        public Dictionary<LayerKind,string> colorMap = new Dictionary<LayerKind,string>();

        public Transform gameMap;

        public void Init()
        {
            #region Create Map

            var stage = PlayerMgr.In.Progress.NowStageName;

            if (stage.IsOk())
            {
                stageName = $"{stage}";

                var prefab = StageMgr.In.FindStage(stageName);

                if(prefab == null)
                {
                    Log.Map.E($"{stageName} is not Exist");
                }
                else
                {
                    gameMap = Instantiate(prefab,transform).transform;

                    InGameMgr.In.IsTest = false;
                }
            }
            else
            {
                if(transform.childCount > 0)
                {
                    gameMap = transform.GetChild(0);
                }

                InGameMgr.In.IsTest = true;
            }

            if(gameMap != null)
            {
                var names = Enum.GetNames(typeof(LayerKind));

                for (var i=0;i<names.Length;i++)
                {
                    var data = gameMap.Find(names[i]);

                    if (data != null)
                    {
                        layers.Add(names[i],data.gameObject);

                        var renderer = data.GetComponent<TilemapRenderer>();

                        if (renderer != null)
                        {
                            renderer.sortingLayerName = "MapTile";
                            renderer.sortingOrder = i;
                        }

                        if(colorMap.ContainsIndex(i))
                        {
                            data.GetComponent<Tilemap>().color = colorMap[names[i].ToEnum<LayerKind>()].ToColor();
                        }
                    }
                }
            }
            else
            {
                Log.System.E("Map is not Exist");
            }
            #endregion

            #region Check
            var heroes = gameMap.GetComponentsInChildren<CharacterMove>(true);

            if(heroes.Length != 1)
            {
                Log.System.E($"Player is not One [Count : {heroes.Length}]");
            }
            else
            {
                hero = heroes[0];
            }

            foreach(var box in gameMap.GetComponentsInChildren<Box>(true))
            {
                boxes.Add(new BoxGroup() { boxObj = box.gameObject, FirstPos = box.transform.position,});
            }

            var portals = gameMap.GetComponentsInChildren<Portal>(true);

            if (portals.Length != 1)
            {
                Log.System.E($"Portal is not One [Count : {portals.Length}]");
            }
            #endregion

            {
                var layer = gameMap.Find("Default_Layer");

                var renderer = layer.GetComponent<TilemapRenderer>();

                renderer.sortingLayerName = "MapTile";
                renderer.sortingOrder = -1;

                layer.GetComponent<Tilemap>().color = "#808080".ToColor();
            }

            foreach (var layer in layers)
            {
                UIMgr.In.ActiveColor(layer.Key,!layer.Value.activeSelf);
            }
        }

        public void ToggleLayer(string _layer)
        {
            if (layers.TryGetValue(_layer,out var data))
            {
                data.SetActive(!data.activeSelf);

                if(data.activeSelf)
                {
                    var map = data.GetComponent<Tilemap>();

                    {
                        var tile = map.GetTile(map.WorldToCell(hero.transform.position));

                        if (tile != null)
                        {
                            InGameMgr.In.EndGame(false);

                            hero.gameObject.SetActive(false);
                        }
                    }

                    {
                        foreach(var box in boxes)
                        {
                            var tile = map.GetTile(map.WorldToCell(box.boxObj.transform.position));

                            if (tile != null)
                            {
                                StartCoroutine(ResetBox(box));
                            }
                        }
                    }
                }
            }
        }

        public void SendToBox(GameObject _obj)
        {
            var box = boxes.Find(fin=>fin.boxObj.Equals(_obj));

            box.boxObj.transform.position = box.FirstPos;
        }

        public int SetStageClear()
        {
            if(gameMap != null)
            {
                var stageName = gameMap.gameObject.name.Replace("(Clone)","");

                PlayerMgr.In.Progress.StageClear(stageName);

                PlayerMgr.In.SaveData();

                if(StageMgr.In.GetNextStage(stageName,out var result))
                {
                    PlayerMgr.In.Progress.NowStageName = result;

                    Log.System.I($"{result} 저장 완료");

                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }

        IEnumerator ResetBox(BoxGroup _box)
        {
            _box.boxObj.SetActive(false);

            yield return null;

            _box.boxObj.transform.position = _box.FirstPos;

            _box.boxObj.SetActive(true);
        }

        private class BoxGroup
        {
            public GameObject boxObj;
            public Vector3 FirstPos;
        }
    }
}