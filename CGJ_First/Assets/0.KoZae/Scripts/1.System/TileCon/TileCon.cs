using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KZLib
{
    public class TileCon : MonoBehaviour
    {
        public bool isTest; 
        public int stage;

        public DictValue<GameObject> layers = new DictValue<GameObject>();

        //public List<GameObject>

        void Awake()
        {
            Transform map;

            if (stage != -1)
            {
                isTest = false;
                // 불러올 스테이지 받아오기
                stage = 1;

                map = Instantiate(PrefabMgr.In.FindPrefab($"Stage_{stage}"),transform).transform;
            }
            else
            {
                isTest = true;
                map = transform.GetChild(0);
            }

            var names = Enum.GetNames(typeof(LayerKind));


            for (int i=0;i<names.Length;i++)
            {
                var data = map.Find(names[i]);

                if(data != null)
                {
                    layers.Add(names[i],data.gameObject);

                    var renderer = data.GetComponent<TilemapRenderer>();

                    if(renderer != null)
                    {
                        renderer.sortingLayerName = "MapTile";
                        renderer.sortingOrder = i;

                    }
                }
            }
        }

        void Start()
        {
            
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                // 1번 레이어 변경
                if(layers.TryGetValue(0,out var pair))
                {
                    pair.Value.SetActive(!pair.Value.activeSelf);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // 2번 레이어 변경
                if (layers.TryGetValue(1,out var pair))
                {
                    pair.Value.SetActive(!pair.Value.activeSelf);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // 3번 레이어 변경
                if (layers.TryGetValue(2,out var pair))
                {
                    pair.Value.SetActive(!pair.Value.activeSelf);
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                // 4번 레이어 변경
                if (layers.TryGetValue(3,out var pair))
                {
                    pair.Value.SetActive(!pair.Value.activeSelf);
                }
            }
        }
    }
}