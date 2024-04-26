using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterCardSet : CardSet, ICheckDistance
{

    Vector3 originalSize;
    MonsterStat monsterStat;
    [SerializeField] TextMeshProUGUI atkText;

    PlayerCardSet playerCardSet;
    PlayerStat playerStat;

    [SerializeField] GameObject rayPos;

    Animator anim;
    private void Start()
    {
        playerCardSet = FindObjectOfType<PlayerCardSet>();
        playerCardSet.PlayerMoved += Reposition;
        //playerCardSet.LineSort += ActiveMoveForward;
        originalSize = transform.localScale;
        
        playerStat = playerCardSet.gameObject.GetComponent<PlayerStat>();

        anim = GetComponent<Animator>();
    }
    public override void SetUp(int index)
    {
        monsterStat = GetComponent<MonsterStat>();
        string spritePath = csv.monsterDatas[index].characterImage;

        character.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        character.SetNativeSize();

        atkText.text = monsterStat.currentHp.ToString();
    }

    public void UpdateMonsterStat()
    {
        atkText.text = monsterStat.attackPower.ToString();
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
            CheckBottomCard();
            monsterStat.MonsterMode();
        }
        if (currentCardCurrentLine == CardCurrentLine.CardLine0)
        {
            if (posIndex == 0 || posIndex == 1 || posIndex == 2)
            {
                transform.localScale = originalSize;
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

    void CheckBottomCard()
    {
        if (monsterStat.effectiveRange > 0)
        {
            if (currentCardCurrentLine == CardCurrentLine.CardLine1)
            {
                if (playerCardSet.posIndex == posIndex - 3)
                {
                    playerStat.AddDamage(gameObject);
                }
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