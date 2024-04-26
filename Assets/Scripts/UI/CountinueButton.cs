using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountinueButton : PopupUI
{

    public override void Awake()
    {
        cardClickManager = FindObjectOfType<CardClickManager>();
        eventButton = GetComponent<Button>();
        eventButton.onClick.AddListener(OnEventButtonClicked);
    }

    public override void OnEventButtonClicked()
    {
        gameObject.SetActive(false);
        cardClickManager.marketOn = false;
    }
}
