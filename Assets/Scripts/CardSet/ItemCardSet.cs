using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemCardSet : CardSet, ICheckDistance
{
    Vector3 originalSize;
    PlayerCardSet playerCardSet;
    ItemCardSet itemCardSet;
    CsvWriter csvWriter;
    public string itemType;

    Animator anim;
    private void Start()
    {
        itemCardSet = GetComponent<ItemCardSet>();
        playerCardSet = FindObjectOfType<PlayerCardSet>();
        playerCardSet.PlayerMoved += Reposition;
        //playerCardSet.LineSort += ActiveMoveForward;
        originalSize = transform.localScale;
        csvWriter = Managers.Instance.GetComponent<CsvWriter>();
        anim = GetComponent<Animator>();
    }
    public override void SetUp(int index)
    {
        string spritePath = csv.tempItemDatas[index].itemImage;
        itemType = csv.tempItemDatas[index].itemType;

        character.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        character.SetNativeSize();

    }

    public void OnMouseClicked(GameObject gameObject)
    {
        //csvWriter.WritePlayerTempItemData(this.gameObject);
        transform.localScale = originalSize;
        Managers.Card.DiscardUsedCard(this.gameObject);

        if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
        {
            playerCardSet.StartCoroutine("CoMove", itemCardSet.posIndex);
        }
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
