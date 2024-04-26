using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryItemDisplay : MonoBehaviour
{
    int maxTempItem = 4;
    Queue<GameObject> tempItemQueue;

    private void Awake()
    {
        tempItemQueue = new Queue<GameObject>();
        CreatTempItemFrame();
    }

   public void CreatTempItemFrame()
    {
        for (int i = 0; i < maxTempItem; i++)
        {
            GameObject newItem = Managers.Resource.InstantiateTempItem("ItemFrame");
            newItem.transform.SetParent(this.transform, false);
            newItem.SetActive(false);
            tempItemQueue.Enqueue(newItem);
        }
    }

    public GameObject GetAvailableTempItemFrame()
    {
        if(tempItemQueue.Count> 0)
        {
            return tempItemQueue.Dequeue();
        }
        return null; 
    }
    
    //public  TempItemExpired()
    //{

    //}
}
