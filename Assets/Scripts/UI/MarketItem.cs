using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    PlayerItemManager playerItemManager;
    MarketUIManager marketUIManager;
    CsvWriter csvWriter;

    [SerializeField] public Image itemImage;
    [SerializeField] public TextMeshProUGUI itemPriceUI;
    [SerializeField] GameObject soldOut;
    [SerializeField] GameObject outOfMoney;

    public string itemType;
    public int itemIndex;
    public string itemImagePath;
    public string itemName;
    public int itemValue = 0;

    public string itemPrice;

    Button button;
    private void Awake()
    {
        csvWriter = Managers.Instance.GetComponent<CsvWriter>();
        marketUIManager = FindObjectOfType<MarketUIManager>();
        playerItemManager = FindObjectOfType<PlayerItemManager>();
        button = transform.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnItemClicked);
    }
    public void InitMarket()
    {
        itemImage.sprite = Managers.Resource.GetSprite<Sprite>(itemImagePath);
        itemImage.preserveAspect = true;
        itemPriceUI.text = itemPrice;
    }

    public void OnItemClicked()
    {
        StartCoroutine(ItemClicked());
    }

    public IEnumerator ItemClicked()
    {
        Debug.Log("ItemClicked");

        if(playerItemManager.currrentGold > int.Parse(itemPrice))
        {
            playerItemManager.currrentGold -= int.Parse(itemPrice);
            Managers.Instance.stageUIManager.UpdateGold(playerItemManager.currrentGold);

            csvWriter.WritePlayerHeldItemData(gameObject);
            playerItemManager.SavedItem(gameObject);
            button.interactable = false;
            soldOut.SetActive(true);
        }
        else
        {
            outOfMoney.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            outOfMoney.SetActive(false);
        }

        yield return null;
        
    }
}
