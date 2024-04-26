using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCardSet : CardSet
{

    Vector3 targetPos;

    public delegate void OnPlayerMoved();
    public event OnPlayerMoved PlayerMoved;

    //public delegate void OnLineSort();
    //public event OnLineSort LineSort;

    Animator anim;
    private void Start()
    {
        cardType = CardType.CARD_PLAYER;
        PlayerMoved += Reposition;

        anim = GetComponent<Animator>();
    }
    public override void SetUp(int index)
    {
        string spritePath = csv.playerDatas[index].characterImage;

        character.sprite = Managers.Resource.GetSprite<Sprite>(spritePath);
        character.SetNativeSize();
    }

    public IEnumerator CoMove(int posIndex)
    {
        targetPos = cardPositionManager.cardPosData[posIndex].transform.position;

        float distance = Vector3.Distance(targetPos, this.gameObject.transform.position);
        Vector3 velocity = Vector3.zero;


        while (true)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, distance*moveSpeed * Time.deltaTime);
            distance = Vector3.Distance(targetPos,gameObject.transform.position);

            if (distance < 0.2f)
            {
                this.gameObject.transform.position = targetPos;
                break;
            }

            yield return new WaitForEndOfFrame();

        }

        yield return null;


        CardIndexing();
        UpdateCardCurrentLine(posIndex);

        
        yield return new WaitForSeconds(1f);

        Managers.Instance.stageManager.currentStage = Managers.Instance.stageManager.currentStage + 1;
        Managers.Instance.stageUIManager.UpdateStageCount();

       PlayerMoved();
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
        if (currentCardCurrentLine == CardCurrentLine.CardLine0)
        {
            gameObject.SetActive(false);
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
        gameObject.transform.parent = Managers.Instance.cardPositionManager.cardPosData[posIndex].transform;

        Managers.Card.SetNewLine();

        //yield return new WaitForSeconds(0.1f);

        //if(currentCardCurrentLine == CardCurrentLine.CardLine0)
        //{
        //    CheckEmptyLine();
        //}
    }
    /*
    public void CheckEmptyLine()
    {
        int count = 0;
        for (int i = 0; i < Managers.Instance.cardPositionManager.lines.GetLength(1); i++)
        {
            int checkPos = Managers.Instance.cardPositionManager.lines[1, i];

            if (Managers.Instance.cardPositionManager.cardPosData[checkPos].transform.childCount > 0)
            {
                break;
            }
            else
            {
                count++;
            }
            if (count == 3)
            {
                LineSort();
                StartCoroutine(CoMoveForward());

                count = 0;
            }
        }
    }
    */
    public override IEnumerator CoMoveForward()
    {
        yield return new WaitForSeconds(0.3f);

        Managers.Card.SetNewLine();
    }

}
