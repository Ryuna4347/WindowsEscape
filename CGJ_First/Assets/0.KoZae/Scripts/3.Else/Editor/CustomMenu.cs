#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;

namespace KZLib.CumtomMenu
{
    public static class CustomMenu
    {
        [MenuItem("KZMenu/Clear Game Data")]
        public static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

            Debug.Log("<color=green> GameData Has Been Deleted </color>");
        }

    }
}
#endif