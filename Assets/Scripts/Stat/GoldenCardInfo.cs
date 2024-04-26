using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenCardInfo : MonoBehaviour
{
    public string cardName;
    public GoldenCardType goldenCardType;
    public string image;

    public GameObject marketUI;

    CvsReader csv;

    private void Awake()
    {
        marketUI = FindObjectOfType<MarketUIManager>().gameObject;
    }
    void OnEnable()
    {
        csv = Managers.Instance.GetComponent<CvsReader>();
    }

    public void SetGoldenCardData(int index)
    {
        cardName = csv.goldenBoxDatas[index].name;
        goldenCardType = (GoldenCardType)Enum.Parse(typeof(GoldenCardType), csv.goldenBoxDatas[index].goldenCardType);
        image = csv.goldenBoxDatas[index].image;
    }
}
