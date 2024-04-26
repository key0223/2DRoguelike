using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


public class CsvWriter : MonoBehaviour
{
    PlayerItemManager playerItemManager;

    void Start()
    {
        playerItemManager = FindObjectOfType<PlayerItemManager>();
        CreatPlayerHeldItemDataFile();
    }

    void CreatPlayerHeldItemDataFile() //title
    {
        string path = Application.dataPath + "/" + "PlayerHeldItemData.csv";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string[] rowDataTemp = new string[7];

        rowDataTemp[0] = "Uid";
        rowDataTemp[1] = "ItemType";
        rowDataTemp[2] = "Id";
        rowDataTemp[3] = "Name";
        rowDataTemp[4] = "Value";
        rowDataTemp[5] = "ItemImage";

        using (StreamWriter sw = new StreamWriter((Application.dataPath + "/" + "PlayerHeldItemData.csv")))
        {
            foreach (string data in rowDataTemp)
            {
                sw.Write(data + ",");
            }
        }

        //playerItemManager.heldItemList.Add(rowDataTemp);

        //using (StreamWriter sw = new StreamWriter((Application.dataPath + "/" + "PlayerHeldItemData.csv")))
        //{
        //    for (int i = 0; i < playerItemManager.heldItemList.Count; i++)
        //    {
        //        foreach (string data in playerItemManager.heldItemList[i])
        //        {
        //            sw.Write(data + ",");
        //        }
        //    }
        //}
    }

    public void WritePlayerHeldItemData(GameObject gameObject)
    {

        if (gameObject.TryGetComponent(out MarketItem marketItem))
        {
            PlayerHeldItemData playerTempItemData = new PlayerHeldItemData();

            Guid itemUid = Guid.NewGuid();
            string convertedUid = itemUid.ToString();

            playerTempItemData.uid = convertedUid;
            playerTempItemData.itemType = marketItem.itemType;
            playerTempItemData.id = marketItem.itemIndex;
            playerTempItemData.name = marketItem.itemName;
            playerTempItemData.value = marketItem.itemValue;
            playerTempItemData.itemImage = marketItem.itemImagePath;

            playerItemManager.heldItemList.Add(playerTempItemData);


            using (StreamWriter sw = new StreamWriter((Application.dataPath + "/" + "PlayerHeldItemData.csv")))
            {
                for (int i = 0; i < playerItemManager.heldItemList.Count; i++)
                {
                    foreach (PlayerHeldItemData data in playerItemManager.heldItemList)
                    {
                        sw.Write(data + ",");
                    }
                    sw.WriteLine();
                }
            }
        }

    }
}
