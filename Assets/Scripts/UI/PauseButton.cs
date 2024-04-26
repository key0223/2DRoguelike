using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : PopupUI
{
    [SerializeField] GameObject pauseUI;


    public override void Awake()
    {
        cardClickManager = FindObjectOfType<CardClickManager>();
        eventButton = GetComponent<Button>();
        eventButton.onClick.AddListener(OnEventButtonClicked);
    }

    public override void OnEventButtonClicked()
    {
        pauseUI.SetActive(true);
        cardClickManager.marketOn = true;
    }
}
