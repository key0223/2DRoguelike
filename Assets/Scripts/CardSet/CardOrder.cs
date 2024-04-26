using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOrder : MonoBehaviour
{
    [SerializeField] public Renderer[] backRenderers;
    [SerializeField] public Renderer[] middleRenderers;
    [SerializeField] public Renderer[] topRenderers;
    [SerializeField] string sortingLayerName;
    void Start()
    {
        SetCardOrder(0);
    }

    public void SetCardOrder(int order)
    {
        int numberingOrder = order * 10;

        foreach(var renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = numberingOrder;
        }

        foreach(var renderer in middleRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = numberingOrder + 1;
        }
        foreach (var renderer in topRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = numberingOrder + 2;
        }
    }
}
