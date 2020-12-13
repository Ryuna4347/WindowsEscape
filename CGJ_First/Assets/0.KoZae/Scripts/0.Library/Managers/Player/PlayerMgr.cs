using KZLib.Data.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KZLib
{
    public class PlayerMgr : Singleton<PlayerMgr>
    {
        private static readonly string[] DATA_KIND = { "Progress","Option" };
        private const string FILE_NAME = "PlayerData.json";

        private readonly DictValue<IPLHandler> handlers = new DictValue<IPLHandler>();

        public Progress Progress => handlers["Progress"] as Progress;

        PlayerMgr()
        {
            var text = FileUtil.ReadDataFromFile(FILE_NAME);

            if (text.IsOk())
            {
                try
                {
                    var datas = JObject.Parse(text);

                    ImportData(datas);

                    Log.Player.I("데이터 로드 성공!!!");
                }
                catch (JsonException _jex)
                {
                    Log.Player.W($"하지만 제이슨 에러 ㅎㅎ \n{_jex}");

                    InitData();
                }
            }
            else
            {
                InitData();
            }
        }

        void InitData()
        {
            var datas = new JObject();

            foreach (var kind in DATA_KIND)
            {
                datas.Add(kind,null);
            }

            ImportData(datas);            
        }

        void ImportData(JObject _object)
        {
            handlers.Clear();

            foreach (var kind in DATA_KIND)
            {
                var type = Type.GetType(string.Format("Player.{0}",kind));
                var nested = type.GetNestedType("DataBase");

                if (type != null && nested != null)
                {
                    var handler = (IPLHandler)Activator.CreateInstance(type);

                    if (_object.TryGetValue(kind,out var token))
                    {
                        handler.SetDataBase(token.Type.Equals(JTokenType.Null) ? Activator.CreateInstance(nested) : token.ToObject(nested));
                    }
                    else
                    {
                        handler.SetDataBase(Activator.CreateInstance(nested));
                    }

                    handler.InitDataBase();
                    handlers.Add(kind,handler);
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
            }

            SaveData();
        }

        public void SaveData()
        {
            var datas = new JObject();

            foreach (var handler in handlers)
            {
                datas.Add(handler.Key,handler.Value.ParseDataBase());
            }

            var text = datas.ToString();

            FileUtil.WriteDataToFile(text,FILE_NAME);
        }
    }
}