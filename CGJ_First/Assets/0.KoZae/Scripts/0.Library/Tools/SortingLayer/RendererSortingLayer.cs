using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RendererSortingLayer : MonoBehaviour
{
    public int orderInLayer;

    void Awake()
    {
        SetSortingLayer();
    }

#if UNITY_EDITOR
    void Update()
    {
        SetSortingLayer();
    }
#endif

    void SetSortingLayer()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.sortingLayerName = gameObject.layer.ToString();
        rend.sortingOrder = orderInLayer;
    }
}
