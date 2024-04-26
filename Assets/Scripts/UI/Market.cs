using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    MarketUIManager marketUIManager;
    CardClickManager cardClickManager;
    CvsReader csvReader;
    
    private void Awake()
    {
        csvReader = Managers.Instance.GetComponent<CvsReader>();
        marketUIManager = transform.GetComponentInParent<MarketUIManager>();
        cardClickManager = FindObjectOfType<CardClickManager>();
    }

    private void OnEnable()
    {
        cardClickManager.marketOn = true;
        PickHealItem();
        PickWeaponItem();
    }
    private void OnDisable()
    {
        cardClickManager.marketOn = false;
    }
    public void PickHealItem()
    {
        int randomItem = Random.Range(0, 4);

        GameObject oneItem = marketUIManager.GetAvailableFrame();
        oneItem.SetActive(true);

        if (oneItem.TryGetComponent<MarketItem>(out MarketItem marketItem))
        {
            marketItem.itemType = csvReader.tempItemDatas[randomItem].itemType;
            marketItem.itemIndex = randomItem;
            marketItem.itemImagePath = csvReader.tempItemDatas[randomItem].itemImage;
            marketItem.itemName = csvReader.tempItemDatas[randomItem].name;

            int randPrice = Random.Range(1, 3);
            int itemPrice = 30 + Managers.Instance.stageManager.currentStage * randPrice;

            marketItem.itemPrice = itemPrice.ToString();

            marketItem.InitMarket();
        }
    }
    public void PickWeaponItem()
    {
        int randomItem = Random.Range(0, 3);

        GameObject oneItem = marketUIManager.GetAvailableFrame();
        oneItem.SetActive(true);

        if (oneItem.TryGetComponent<MarketItem>(out MarketItem marketItem))
        {
            marketItem.itemType = csvReader.weaponDatas[randomItem].weaponType;
            marketItem.itemIndex = randomItem;
            marketItem.itemImagePath = csvReader.weaponDatas[randomItem].weaponImage;
            marketItem.itemName = csvReader.weaponDatas[randomItem].name;
            marketItem.itemValue = csvReader.weaponDatas[randomItem].atkPower;

            int randPrice = Random.Range(1, 3);
            int itemPrice = 30 + Managers.Instance.stageManager.currentStage * randPrice;

            marketItem.itemPrice = itemPrice.ToString();

            marketItem.InitMarket();
        }
    }
}
