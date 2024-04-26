using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    protected Button eventButton;

    protected CardClickManager cardClickManager;

    public abstract void Awake();


    public abstract void OnEventButtonClicked();
}
