using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenCardSet : CardSet, ICheckDistance
{
    PlayerCardSet playerCardSet;
    GoldenCardSet goldenCardSet;
    GoldenCardInfo goldenCardInfo;
    public string goldenCardType;

    Animator anim;
    private void Start()
    {
        goldenCardSet = GetComponent<GoldenCardSet>();
        goldenCardInfo = GetComponent<GoldenCardInfo>();
        playerCardSet = FindObjectOfType<PlayerCardSet>();
        playerCardSet.PlayerMoved += Reposition;
        anim = GetComponent<Animator>();

        //playerCardSet.LineSort += ActiveMoveForward;
    }
    public override void SetUp(int index)
    {
        string spritePath = csv.goldenBoxDatas[index].image;
        goldenCardType = csv.goldenBoxDatas[index].goldenCardType;
        character.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        character.SetNativeSize();
    }

    public void OnMouseClicked(GameObject gameObject)
    {
        if(goldenCardInfo.goldenCardType == GoldenCardType.GOLDEN_BOX)
        {
            if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
            {
                playerCardSet.StartCoroutine("CoMove", goldenCardSet.posIndex);
            }
        }
        else if(goldenCardInfo.goldenCardType == GoldenCardType.GOLDEN_MERCHANT)
        {
            goldenCardInfo.marketUI.transform.GetChild(0).gameObject.SetActive(true); //수정 필요
            goldenCardInfo.marketUI.transform.GetChild(1).gameObject.SetActive(true);

            playerCardSet.StartCoroutine("CoMove", goldenCardSet.posIndex);
        }
        Managers.Card.DiscardUsedCard(this.gameObject);
    }

    public override void Reposition()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CoTurnCardOver());
        }
    }
    public override IEnumerator CoTurnCardOver()
    {
        if (currentCardCurrentLine == CardCurrentLine.CardLine1)
        {
            anim.SetTrigger("Disabled_tr");
        }
        if (playerCardSet.currentCardCurrentLine == CardCurrentLine.CardLine1 && currentCardCurrentLine == CardCurrentLine.CardLine2)
        {
            currentCardCurrentLine = CardCurrentLine.CardLine1;
            TurnCardOver(currentCardCurrentLine);
        }
        if (currentCardCurrentLine == CardCurrentLine.CardLine0)
        {
            if (posIndex == 0 || posIndex == 1 || posIndex == 2)
            {
                Managers.Card.DiscardUsedCard(gameObject);
            }
        }
        yield return new WaitForSeconds(0.03f);
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(CoMoveDown());
    }

    public IEnumerator CoMoveDown()
    {
        Vector3 movePos = cardPositionManager.cardPosData[posIndex - 3].transform.position;
        float distance = Vector3.Distance(movePos, gameObject.transform.position);

        while (true)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePos, distance * moveSpeed * Time.deltaTime);
            distance = Vector3.Distance(movePos, gameObject.transform.position);

            if (distance < 0.2f)
            {
                gameObject.transform.position = movePos;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;

        CardIndexing();
        UpdateCardCurrentLine(posIndex);
        CheckPlayerDistance();
        gameObject.transform.parent = Managers.Instance.cardPositionManager.cardPosData[posIndex].transform;
        TurnCardOver(currentCardCurrentLine);

        yield return new WaitForEndOfFrame();
    }

    public void CheckPlayerDistance()
    {
        if (playerCardSet.posIndex == 0)
        {
            if (posIndex == 5)
            {
                currentCardCurrentLine = CardCurrentLine.CardDisabled;
            }
        }
        else if (playerCardSet.posIndex == 2)
        {
            if (posIndex == 3)
            {
                currentCardCurrentLine = CardCurrentLine.CardDisabled;
            }
        }
    }
    public void ActiveMoveForward()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CoMoveForward());
        }
    }
    public override IEnumerator CoMoveForward()
    {
        if (currentCardCurrentLine != CardCurrentLine.CardLine0)
        {
            Vector3 movePos = cardPositionManager.cardPosData[posIndex - 3].transform.position;
            float distance = Vector3.Distance(movePos, gameObject.transform.position);

            while (true)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePos, distance * moveSpeed * Time.deltaTime);
                distance = Vector3.Distance(movePos, gameObject.transform.position);

                if (distance < 0.2f)
                {
                    gameObject.transform.position = movePos;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;

            CardIndexing();
            UpdateCardCurrentLine(posIndex);
            CheckPlayerDistance();
            gameObject.transform.parent = Managers.Instance.cardPositionManager.cardPosData[posIndex].transform;
            TurnCardOver(currentCardCurrentLine);
        }
    }
}
