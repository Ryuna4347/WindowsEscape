using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    private float baseOrthoSize;
    [SerializeField] private float targetOrthoSize;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Coroutine zoominCoroutine;
    [SerializeField] private Coroutine zoomoutCoroutine;

    private void Start()
    {
        baseOrthoSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(zoomoutCoroutine != null)
            {
                StopCoroutine(zoomoutCoroutine);
            }
            zoominCoroutine = StartCoroutine("ZoomIn");
        }

        if(Input.GetMouseButton(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (zoominCoroutine != null)
            {
                StopCoroutine(zoominCoroutine);
            }
            zoomoutCoroutine = StartCoroutine("ZoomOut");
        }
    }

    private IEnumerator ZoomIn()
    {
        float orthoSize = Camera.main.orthographicSize;

        Time.timeScale = 0.5f;

        while(orthoSize < targetOrthoSize)
        {
            orthoSize = Mathf.Lerp(orthoSize, targetOrthoSize, 0.5f);
            Camera.main.orthographicSize = orthoSize;

            yield return null;
        }

    }

    private IEnumerator ZoomOut()
    {
        float orthoSize = Camera.main.orthographicSize;

        Time.timeScale = 1f;

        while (orthoSize >= baseOrthoSize)
        {
            orthoSize = Mathf.Lerp(orthoSize, baseOrthoSize, 0.5f);
            Camera.main.orthographicSize = orthoSize;

            yield return null;
        }

    }
}
