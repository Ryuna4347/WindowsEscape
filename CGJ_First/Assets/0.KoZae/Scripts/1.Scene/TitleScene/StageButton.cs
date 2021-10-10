using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KZLib
{
    public enum Stage_kind { LOCK, UNLOCK}; 

    public class StageButton : MonoBehaviour
    {
        public UnityEngine.UI.Button button;
        public Image stageImg;
        public Text titleText;

        public string stageName;

        public Material grayScale;

        public void SetState(Stage_kind _state,string _name,int _index)
        {
            stageName = _name;

            if (_state.Equals(Stage_kind.LOCK))
            {
                stageImg.material = grayScale;
                button.interactable = false;
                titleText.text = "";
            }
            else
            {
                stageImg.material = null;
                button.interactable = true;
                titleText.text = $"<b>제목없음 {_index}</b>";
            }
        }

        public void OnClickBtn()
        {
            if(stageName.IsOk())
            {
                TitleMgr.In.ActiveInGame(stageName);
            }
        }
    }
}