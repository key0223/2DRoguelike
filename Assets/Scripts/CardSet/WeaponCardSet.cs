using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCardSet : CardSet, ICheckDistance
{
    PlayerCardSet playerCardSet;
    PlayerItemManager playerItemManager;
    WeaponCardSet weaponCardSet;
    CsvWriter csvWriter;
    public string weaponType;

    Animator anim;

    private void Start()
    {
        weaponCardSet = GetComponent<WeaponCardSet>();
        playerCardSet = FindObjectOfType<PlayerCardSet>();
        playerItemManager = playerCardSet.gameObject.GetComponent<PlayerItemManager>();
        playerCardSet.PlayerMoved += Reposition;
        //playerCardSet.LineSort += ActiveMoveForward;
        csvWriter = Managers.Instance.GetComponent<CsvWriter>();

        anim = GetComponent<Animator>();
    }
    public override void SetUp(int index)
    {
        string spritePath = csv.weaponDatas[index].weaponImage;
        weaponType = csv.weaponDatas[index].weaponType;

        character.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        character.SetNativeSize();
    }
    public void OnMouseClicked(GameObject gameObject)
    {
        //csvWriter.WritePlayerTempItemData(this.gameObject);
        if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
        {
            playerCardSet.StartCoroutine("CoMove", weaponCardSet.posIndex);
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
        if(currentCardCurrentLine != CardCurrentLine.CardLine0)
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
