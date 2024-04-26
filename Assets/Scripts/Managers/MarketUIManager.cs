using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUIManager : MonoBehaviour
{
    int maxMarketItem = 3;

    public GameObject marketParent;
    Queue<GameObject> marketItemQueue;


    private void Awake()
    {
        marketItemQueue = new Queue<GameObject>();
        CreatMarketItemFrame();
    }

    public void CreatMarketItemFrame()
    {
        for (int i = 0; i < maxMarketItem; i++)
        {
            GameObject newMarketItem = Managers.Resource.InstantiateTempItem("MarketItemFrame");
            newMarketItem.transform.SetParent(marketParent.transform, false);
            marketItemQueue.Enqueue(newMarketItem);
            newMarketItem.SetActive(false);
        }
    }

    public GameObject GetAvailableFrame()
    {
        if(marketItemQueue.Count>0)
        {
            return marketItemQueue.Dequeue();
        }
        return null;
    }

    public void DiscardItemFrame(GameObject itemFrame)
    {
        marketItemQueue.Enqueue(itemFrame);
    }
}
