using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject marketUI;

    Button closeButton;

    private void Awake()
    {
        closeButton = GetComponent<Button>();
        closeButton.onClick.AddListener(CloseButtonClicked);
    }


    void CloseButtonClicked()
    {
        marketUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
