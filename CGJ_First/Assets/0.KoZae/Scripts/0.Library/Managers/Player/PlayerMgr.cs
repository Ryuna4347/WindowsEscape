using KZLib.Data.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KZLib
{
    public class PlayerMgr : Singleton<PlayerMgr>
    {
        //private static readonly string[] DATA_KIND = { "Progress" };
        private const string FILE_NAME = "PlayerData.json";

        //private readonly DictValue<IPLHandler> handlers = new DictValue<IPLHandler>();


        public ProgressData Progress { get; set; }

        //public Progress Progress => handlers["Progress"] as Progress;

        PlayerMgr()
        {
            Progress = new ProgressData();


            if( PlayerPrefs.HasKey("ShowIntro"))
            {
                Progress.ShowIntro = PlayerPrefs.GetInt("ShowIntro") == 1;
            }

            if (PlayerPrefs.HasKey("Stages"))
            {
                Progress.Stages.AddRange(PlayerPrefs.GetString("Stages").Split('&'));
            }
        }

        void InitData()
        {
            ImportData();            
        }

        void ImportData(JObject _object = null)
        {
            Progress = new ProgressData();

            if (_object != null && _object.TryGetValue("Progress",out var token) && token.Type.Equals(JTokenType.Null))
            {
                Progress = token.ToObject<ProgressData>();
            }

            SaveData();
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("ShowIntro",Progress.ShowIntro ? 1 : 0);
            PlayerPrefs.SetString("Stages",string.Join("&",Progress.Stages));
        }


        public class ProgressData
        {
            [JsonIgnore] public string NowStageName { get; set; }
            [JsonIgnore] public bool StartSFX { get; set; }
            [JsonIgnore] public int PlayFade { get; set; }
            public bool ShowIntro { get; set; }
            public List<string> Stages { get; set; }

            public ProgressData()
            {
                Stages = new List<string>();
                NowStageName = null;
                PlayFade = -1;
                StartSFX = false;
            }

            public void StageClear(string _stageName)
            {
                if (!IsClear(_stageName))
                {
                    Stages.Add(_stageName);
                }
            }

            public bool IsClear(string _stageName)
            {
                return Stages.Any(any => any.Equals(_stageName));
            }
        }
    }
}