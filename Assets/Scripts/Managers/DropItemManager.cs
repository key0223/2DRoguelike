using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    [SerializeField] GameObject earnGoldObject;

    public Queue<GameObject> goldPool = new Queue<GameObject>();

    Vector3 dropPos;
    float countGold = 0;

    void Awake()
    {
        CreateDropItemPool();
    }

    public void DropItemExpired(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out GoldUI goldUI))
        {
            goldPool.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
    public void CreateDropItemPool()
    {
        GameObject dropItem = new GameObject { name = "@DropItems" };

        for (int i = 0; i < 5; i++)
        {
            GameObject gold = Managers.Resource.InstantiateDropItem("Gold", dropItem.transform);
            gold.gameObject.SetActive(false);
            goldPool.Enqueue(gold);
        }
    }
}
