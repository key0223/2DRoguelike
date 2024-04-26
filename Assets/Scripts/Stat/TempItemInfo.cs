using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempItemInfo : MonoBehaviour
{
    public string itemName;
    public ItemType itemType;
    public string image;

    CvsReader csv;

    private void OnEnable()
    {
        csv = Managers.Instance.GetComponent<CvsReader>();
    }
    public  void SetTempItemData(int index)
    {
        itemName = csv.tempItemDatas[index].name;
        itemType = (ItemType)Enum.Parse(typeof(ItemType),csv.tempItemDatas[index].itemType);
        image = csv.tempItemDatas[index].itemImage;
    }
}
