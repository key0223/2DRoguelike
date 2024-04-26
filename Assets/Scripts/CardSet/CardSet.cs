using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface ICheckDistance
{
    public void CheckPlayerDistance();
}

public enum CardCurrentLine
{
    CardLine0,
    CardLine1,
    CardLine2,
    CardLine3,
    CardLine4,
    CardDisabled,
}
public abstract class CardSet : MonoBehaviour
{
    public Image character;

    protected CvsReader csv;

    protected CardPositionManager cardPositionManager;
   

    [Header("Card Status")]
    [SerializeField] protected GameObject cardBack;
    [SerializeField] protected GameObject cardFront;
    [SerializeField] protected GameObject cardDisabled;


    [Header("Card Info")]
    public CardType cardType;
    public CardCurrentLine currentCardCurrentLine = CardCurrentLine.CardLine0;
    public int posIndex = 0;

    //Card move speed
    protected float moveSpeed = 10f;

    public GameObject originalParent;


    public void Awake()
    {
        csv = Managers.Instance.GetComponent<CvsReader>();
        cardPositionManager = Managers.Instance.cardPositionManager;
        if(gameObject.transform.parent != null)
        {
            originalParent = transform.parent.gameObject;
        }

        CardIndexing();
        UpdateCardCurrentLine(posIndex);
    }

    protected void OnEnable()
    {
        CardIndexing();
        UpdateCardCurrentLine(posIndex);
    }

    public abstract void SetUp(int index);

    public void CardIndexing()
    {
        for (int i = 0; i < cardPositionManager.cardPosData.Count; i++)
        {
            if (transform.position == cardPositionManager.cardPosData[i].transform.position)
            {
                posIndex = i;

            }
        }
    }

    protected void UpdateCardCurrentLine(int index)
    {

        for (int i = 0; i < cardPositionManager.lines.GetLength(0); i++)
        {
            for (int j = 0; j < cardPositionManager.lines.GetLength(1); j++)
            {
                if(cardPositionManager.lines[i,j] == index)
                {
                    currentCardCurrentLine = (CardCurrentLine)i;
                }
            }
        }

        TurnCardOver(currentCardCurrentLine);
    }
    public abstract IEnumerator CoTurnCardOver();

    public void TurnCardOver(CardCurrentLine cardStatus)
    {
        switch (currentCardCurrentLine)
        {
            case CardCurrentLine.CardLine0:

                if(cardType != CardType.CARD_PLAYER )
                {
                    cardBack.gameObject.SetActive(true);
                    cardFront.gameObject.SetActive(true);
                    cardDisabled.gameObject.SetActive(true);
                }
                else
                {
                    cardBack.gameObject.SetActive(false);
                    cardFront.gameObject.SetActive(true);
                    cardDisabled.gameObject.SetActive(false);
                }
                break;

            case CardCurrentLine.CardLine1:
                cardBack.gameObject.SetActive(false);
                cardFront.gameObject.SetActive(true);
                cardDisabled.gameObject.SetActive(false);
                break;

            case CardCurrentLine.CardLine2:

                cardBack.gameObject.SetActive(false);
                cardFront.gameObject.SetActive(true);
                cardDisabled.gameObject.SetActive(false);

                break;

            case CardCurrentLine.CardLine3:
                cardBack.gameObject.SetActive(false);
                cardFront.gameObject.SetActive(true);
                cardDisabled.gameObject.SetActive(false);

                break;
            case CardCurrentLine.CardLine4:
                cardBack.gameObject.SetActive(false);
                cardFront.gameObject.SetActive(true);
                cardDisabled.gameObject.SetActive(false);

                break;

            case CardCurrentLine.CardDisabled:

                if (cardType != CardType.CARD_PLAYER)
                {
                    cardBack.gameObject.SetActive(false);
                    cardFront.gameObject.SetActive(true);
                    cardDisabled.gameObject.SetActive(true);
                }
                break;
        }
    }

    public abstract void Reposition();

    public abstract IEnumerator CoMoveForward ();

    public IEnumerator Shake()
    {
        float t = 1f;
        float shakePower = 1f;
        Vector3 origin = transform.position;

        while(t> 0f)
        {
            t -= 0.05f;
            transform.position = origin + (Vector3)Random.insideUnitCircle * shakePower * t;

            yield return null;
        }
        transform.position = origin;
    }
}
