using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHeldItemData
{
    public string uid;
    public string itemType;
    public int id;
    public string name;
    public int value;
    public string itemImage;
}

public class PlayerItemManager : MonoBehaviour
{
    public float currrentGold { get;  set; } = 100;

    [SerializeField] GameObject tempItem;
    int maxTempItem = 4;
    Queue<GameObject> tempItemQueue;
    public List<PlayerHeldItemData> heldItemList;

    PlayerStat playerStat;
    CsvWriter csvWriter;

    private void Awake()
    {
        tempItemQueue = new Queue<GameObject>();
        heldItemList = new List<PlayerHeldItemData>();
        CreatTempItemFrame();
        playerStat = gameObject.GetComponent<PlayerStat>();
        csvWriter = Managers.Instance.GetComponent<CsvWriter>();
    }

    public void ActivateTempItem(TempItemInfo tempItemInfo)
    {
        if (tempItemInfo.itemType == ItemType.POTION_HEAL)
        {
            int healHp = Managers.Instance.stageManager.currentStage;
            if (healHp <= 0)
            {
                healHp = 1;
            }

            playerStat.HealHp(healHp);
        }
        //else if(tempItemInfo.itemType == ItemType.POTION_CLEAR_STATUS_EFFECT)
        //{

        //}
        else if ((tempItemInfo.itemType == ItemType.POTION_CONTINUOUS_HEAL))
        {
            int stackValue = Random.Range(1, 4);

            if (playerStat.potionStack <= 0)
            {
                playerStat.potionStack += playerStat.potionStack + stackValue;

                GameObject itemFrame;
                itemFrame = GetAvailableTempItemFrame();
                itemFrame.SetActive(true);
                TempItemUI tempItemUI = itemFrame.GetComponent<TempItemUI>();
                tempItemUI.InitFrame(tempItemInfo);
            }
            else
            {
                playerStat.StackPotionCount(stackValue);
            }

        }
        else if (tempItemInfo.itemType == ItemType.SHIELD)
        {
            int shiledValue = Managers.Instance.stageManager.currentStage;

            if (playerStat.shiledHp <= 0)
            {
                if (shiledValue <= 0)
                {
                    shiledValue = 1;
                }

                playerStat.Getshiled(shiledValue);

                GameObject itemFrame;
                itemFrame = GetAvailableTempItemFrame();
                itemFrame.SetActive(true);
                TempItemUI tempItemUI = itemFrame.GetComponent<TempItemUI>();
                tempItemUI.InitFrame(tempItemInfo);
            }
            else
            {
                if (shiledValue <= 0)
                {
                    shiledValue = 1;
                }
                playerStat.Getshiled(shiledValue);
            }
        }
    }

    public void CreatTempItemFrame()
    {
        for (int i = 0; i < maxTempItem; i++)
        {
            GameObject newItem = Managers.Resource.InstantiateTempItem("ItemFrame");
            newItem.transform.SetParent(tempItem.transform, false);
            newItem.SetActive(false);
            tempItemQueue.Enqueue(newItem);
            
        }
    }

    public GameObject GetAvailableTempItemFrame()
    {
        if (tempItemQueue.Count > 0)
        {
            return tempItemQueue.Dequeue();
        }
        return null;
    }

    public void TempItemUpdate()
    {
        for (int i = 0; i < tempItem.transform.childCount; i++)
        {
            if (tempItem.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                tempItem.transform.GetChild(i).GetComponent<TempItemUI>().ItemValueUpdate();
            }
        }
    }


    public void ExpiredTempItem(GameObject itemFrame)
    {
        itemFrame.SetActive(false);
        tempItemQueue.Enqueue(itemFrame);
    }


    public void ActivateWeapon(WeaponInfo weaponInfo)
    {
        if (weaponInfo.weaponType == WeaponType.WEAPON_SWORD)
        {
            if (weaponInfo.gameObject.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
            {
                int targetPos = weaponCardSet.posIndex + 3;

                GameObject target;

                if (Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.childCount > 0)
                {
                    target = Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.GetChild(0).gameObject;

                    if (target.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                    {
                        monsterStat.AddWeaponDamage(weaponInfo);
                    }
                }
            }
        }

        else if (weaponInfo.weaponType == WeaponType.WEAPON_BIG_SWORD)
        {
            int[] targetArea = new int[] { 6, 7, 8 };

            GameObject target;

            if(weaponInfo.gameObject.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
            {
                foreach (int targetPos in targetArea)
                {
                    if(Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.childCount > 0)
                    {
                        target = Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.GetChild(0).gameObject;

                        if (target.CompareTag("Monster"))
                        {
                            if (target.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                            {
                                monsterStat.AddWeaponDamage(weaponInfo);
                            }
                        }

                    }    
                }
            }
        }

        else if (weaponInfo.weaponType == WeaponType.WEAPON_GUN)
        {
            int targetArea = 3;
            

            if (weaponInfo.gameObject.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
            {

                int weaponPos = weaponCardSet.posIndex;

                int targetPos = weaponPos;

                for (int i = 0; i < targetArea; i++)
                {
                    targetPos += 3;

                    GameObject target;

                    if (Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.childCount > 0)
                    {
                        target = Managers.Instance.cardPositionManager.cardPosData[targetPos].transform.GetChild(0).gameObject;
                        if(target.CompareTag("Monster"))
                        {
                            if (target.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                            {
                                monsterStat.AddWeaponDamage(weaponInfo);
                            }
                        }
                    }
                }
            }
        }
        Managers.Card.DiscardUsedCard(weaponInfo.gameObject);
    }


    public void SavedItem(GameObject item)
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            if (other.gameObject.TryGetComponent<GoldUI>(out GoldUI goldUI))
            {
                currrentGold = currrentGold + goldUI.earnedGold;
            }
            Managers.Instance.stageUIManager.UpdateGold(currrentGold);
            Managers.Instance.dropItemManager.DropItemExpired(other.gameObject);
        }
    }
}
