using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D _col)
        {
            if(_col.CompareTag("Player"))
            {
                InGameMgr.In.EndGame(false);

                _col.gameObject.SetActive(false);
            }
            else if(_col.CompareTag("Box"))
            {
                InGameMgr.In.SendToBoxInMapCon(_col.gameObject);
            }
        }
    }
}