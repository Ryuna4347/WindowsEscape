using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KZLib
{
    public class StageButton : MonoBehaviour
    {
        public void OnClickBtn()
        {
            var names = EventSystem.current.currentSelectedGameObject.name.Split('_');

            if(int.TryParse(names[1],out var num))
            {
                PlayerMgr.In.Progress.SetNowStage(num);

                //SceneUtil.ChangeScene("InGameScene");

                Log.System.I($"Stage_{num} 저장 완료");
            }
        }
    }
}