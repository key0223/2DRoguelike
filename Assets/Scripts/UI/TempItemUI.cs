using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempItemUI : MonoBehaviour
{

    [SerializeField] public Image itemImage;
    [SerializeField] public ItemType itemType;
    [SerializeField] public TextMeshProUGUI itemValue;

    PlayerStat playerStat;
    PlayerItemManager playerItemManager;

    void Awake()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        playerItemManager = FindObjectOfType<PlayerItemManager>();
    }

    public void InitFrame(TempItemInfo itemInfo)
    {
        string spritePath = itemInfo.image;
        itemImage.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        itemImage.SetNativeSize();
        itemType = itemInfo.itemType;

        if (itemType == ItemType.POTION_CONTINUOUS_HEAL)
        {
            itemValue.text = playerStat.potionStack.ToString();
        }
        else if (itemType == ItemType.SHIELD)
        {
            itemValue.text = playerStat.shiledHp.ToString();
        }
    }

    public void ItemValueUpdate()
    {
        if (itemType == ItemType.POTION_CONTINUOUS_HEAL)
        {
            itemValue.text = playerStat.potionStack.ToString();
            if (playerStat.potionStack <= 0)
            {
                playerItemManager.ExpiredTempItem(gameObject);
            }
        }
        else if (itemType == ItemType.SHIELD)
        {
            itemValue.text = playerStat.shiledHp.ToString();

            if (playerStat.shiledHp <= 0)
            {
                playerItemManager.ExpiredTempItem(gameObject);
            }
        }
    }
}
