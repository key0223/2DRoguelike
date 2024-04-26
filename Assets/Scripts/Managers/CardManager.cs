using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardType
{
    CARD_MONSTER,
    CARD_GOLDENBOX,
    CARD_ITEM,
    CARD_WEAPON,
    CARD_MAX_SIZE,
    CARD_PLAYER,
}


public class CardManager //프리팹 생성 및 데이터 초기화, 위치 선정
{
    List<Queue<GameObject>> cardList = new List<Queue<GameObject>>();

    Queue<GameObject>[] cardTypeQueue = new Queue<GameObject>[(int)CardType.CARD_MAX_SIZE];
    public int createdLine = 0;
    int cardPattern = 0;

    public GameObject playerCard;
    int maxMonsterCard = 30;
    int maxGoldenCard = 3;
    int maxItemCard = 10;
    int maxWeaponCard = 10;

    int checkEmptyCount = 0;
    bool hasEmptyLine = false;


    public void InitializeCardObject()
    {
        cardList = new List<Queue<GameObject>>();

        cardTypeQueue[(int)CardType.CARD_MONSTER] = new Queue<GameObject>();
        cardTypeQueue[(int)CardType.CARD_GOLDENBOX] = new Queue<GameObject>();
        cardTypeQueue[(int)CardType.CARD_ITEM] = new Queue<GameObject>();
        cardTypeQueue[(int)CardType.CARD_WEAPON] = new Queue<GameObject>();

        for (int i = 0; i < (int)CardType.CARD_MAX_SIZE; i++)
        {
            cardList.Add(cardTypeQueue[i]);
        }

        GameObject monster = new GameObject { name = "@MonsterCards" };
        GameObject golden = new GameObject { name = "@GoldenCards" };
        GameObject item = new GameObject { name = "@ItemCards" };
        GameObject weapon = new GameObject { name = "@WeaponCards" };

        playerCard = Managers.Resource.Instantiate("PlayerCard");
        playerCard.gameObject.SetActive(false);
        playerCard.GetComponent<PlayerStat>().SetStatData(0);
        playerCard.GetComponent<PlayerCardSet>().SetUp(0);
        playerCard.GetComponent<PlayerCardSet>().cardType = CardType.CARD_PLAYER;
        playerCard.transform.position = Managers.Instance.cardPositionManager.cardPosData[1].transform.position;

        for (int i = 0; i < maxMonsterCard; i++)
        {
            GameObject card = Managers.Resource.Instantiate("MonsterCard", monster.transform);
            card.gameObject.SetActive(false);
            cardList[(int)CardType.CARD_MONSTER].Enqueue(card);
        }

        for (int i = 0; i < maxGoldenCard; i++)
        {
            GameObject card = Managers.Resource.Instantiate("GoldenCard", golden.transform);
            card.gameObject.SetActive(false);
            cardList[(int)CardType.CARD_GOLDENBOX].Enqueue(card);
        }

        for (int i = 0; i < maxItemCard; i++)
        {
            GameObject card = Managers.Resource.Instantiate("ItemCard", item.transform);
            card.gameObject.SetActive(false);
            cardList[(int)CardType.CARD_ITEM].Enqueue(card);
        }

        for (int i = 0; i < maxWeaponCard; i++)
        {
            GameObject card = Managers.Resource.Instantiate("WeaponCard", weapon.transform);
            card.gameObject.SetActive(false);
            cardList[(int)CardType.CARD_WEAPON].Enqueue(card);
        }

        //CreatPlayerCard();
    }


    public int WeightedRandom()
    {
        float random = Random.Range(0.0f, 1.0f);
        float randomr = random * 100.0f;

        float[] percent = { 66.6f, 6.66f, 13.3f, 13.3f };

        float cumulative = 0.0f; // 누적

        for (int i = 0; i < percent.Length; i++)
        {
            cumulative += percent[i];
            if (randomr <= cumulative)
            {
                return i;
            }
        }

        return -1;
    }

    public void GetCardPattern(ref int left, ref int center, ref int right)
    {
        switch (cardPattern)
        {
            case 0:

                left = 1;
                center = 1;
                right = 1;
                break;
            case 1:

                left = 0;
                center = 1;
                right = 0;
                break;
            case 2:

                left = 1;
                center = 1;
                right = 0;
                break;
            case 3:

                left = 0;
                center = 1;
                right = 1;
                break;
            case 4:

                left = 1;
                center = 0;
                right = 1;
                break;
        }
    }
    public void SetCardPos() // 초기화된 데이터 위치 지정 및 활성화
    {
        int pos = 0;
        int left = 0, center = 0, right = 0;

        for (int i = 0; i < Managers.Instance.cardPositionManager.lines.GetLength(0); i++)
        {
            int randCardPattern = Random.Range(0, 4);
            cardPattern = randCardPattern;
            GetCardPattern(ref left, ref center, ref right);

            if (left != 0)
            {
                int pool = WeightedRandom();
                GameObject card;

                pos = i * 3;

                if (cardList[pool].Count > 0)
                {
                    card = cardList[pool].Dequeue();

                    if (pool == (int)CardType.CARD_MONSTER)
                    {
                        int randomEnemy = Random.Range(0, 3);
                        if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                        {
                            monsterStat.SetStatData(randomEnemy);

                        }
                        if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                        {
                            monsterCardSet.SetUp(randomEnemy);
                            monsterCardSet.cardType = CardType.CARD_MONSTER;
                        }

                    }
                    else if (pool == (int)CardType.CARD_GOLDENBOX)
                    {
                        //int randGold = Random.Range(0, 2);
                        if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                        {
                            goldenCardInfo.SetGoldenCardData(0);
                        }
                        if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                        {
                            goldenCardSet.SetUp(0); //임시
                            goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                        }
                    }
                    else if (pool == (int)CardType.CARD_ITEM)
                    {
                        int randomItem = Random.Range(0, 4);
                        if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                        {
                            itemInfo.SetTempItemData(randomItem);
                        }
                        if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                        {
                            itemCardSet.SetUp(randomItem);
                            itemCardSet.cardType = CardType.CARD_ITEM;
                        }

                    }
                    else if (pool == (int)CardType.CARD_WEAPON)
                    {
                        int randomWeapon = Random.Range(0, 3);
                        if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                        {
                            weaponInfo.SetWeaponData(randomWeapon);
                        }
                        if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                        {
                            weaponCardSet.SetUp(randomWeapon);
                            weaponCardSet.cardType = CardType.CARD_WEAPON;
                        }
                    }

                    card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                    card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                    card.gameObject.SetActive(true);
                }
            }

            if (center != 0)
            {
                pos = i * 3 + 1;

                if (pos != 1)
                {
                    int pool = WeightedRandom();
                    GameObject card;

                    if (cardList[pool].Count > 0)
                    {
                        card = cardList[pool].Dequeue();

                        if (pool == (int)CardType.CARD_MONSTER)
                        {
                            int randomEnemy = Random.Range(0, 3);
                            if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                            {
                                monsterStat.SetStatData(randomEnemy);
                            }
                            if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                            {
                                monsterCardSet.SetUp(randomEnemy);
                                monsterCardSet.cardType = CardType.CARD_MONSTER;
                            }
                        }
                        else if (pool == (int)CardType.CARD_GOLDENBOX)
                        {
                            //int randGold = Random.Range(0, 2);
                            if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                            {
                                goldenCardInfo.SetGoldenCardData(0);
                            }
                            if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                            {
                                goldenCardSet.SetUp(0); //임시
                                goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                            }
                        }
                        else if (pool == (int)CardType.CARD_ITEM)
                        {
                            int randomItem = Random.Range(0, 4);
                            if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                            {
                                itemInfo.SetTempItemData(randomItem);
                            }
                            if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                            {
                                itemCardSet.SetUp(randomItem);
                                itemCardSet.cardType = CardType.CARD_ITEM;
                            }
                        }
                        else if (pool == (int)CardType.CARD_WEAPON)
                        {
                            int randomWeapon = Random.Range(0, 3);
                            if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                            {
                                weaponInfo.SetWeaponData(randomWeapon);
                            }
                            if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                            {
                                weaponCardSet.SetUp(randomWeapon);
                                weaponCardSet.cardType = CardType.CARD_WEAPON;
                            }
                        }

                        card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                        card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                        card.gameObject.SetActive(true);
                    }
                }
            }

            if (right != 0)
            {
                pos = i * 3 + 2;

                int pool = WeightedRandom();
                GameObject card;

                if (cardList[pool].Count > 0)
                {
                    card = cardList[pool].Dequeue();

                    if (pool == (int)CardType.CARD_MONSTER)
                    {
                        int randomEnemy = Random.Range(0, 3);
                        if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                        {
                            monsterStat.SetStatData(randomEnemy);
                        }
                        if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                        {
                            monsterCardSet.SetUp(randomEnemy);
                            monsterCardSet.cardType = CardType.CARD_MONSTER;
                        }

                    }
                    else if (pool == (int)CardType.CARD_GOLDENBOX)
                    {
                        //int randGold = Random.Range(0, 2);
                        if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                        {
                            goldenCardInfo.SetGoldenCardData(0);
                        }
                        if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                        {
                            goldenCardSet.SetUp(0); //임시
                            goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                        }
                    }
                    else if (pool == (int)CardType.CARD_ITEM)
                    {
                        int randomItem = Random.Range(0, 4);
                        if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                        {
                            itemInfo.SetTempItemData(randomItem);
                        }
                        if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                        {
                            itemCardSet.SetUp(randomItem);
                            itemCardSet.cardType = CardType.CARD_ITEM;
                        }
                    }
                    else if (pool == (int)CardType.CARD_WEAPON)
                    {
                        int randomWeapon = Random.Range(0, 3);
                        if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                        {
                            weaponInfo.SetWeaponData(randomWeapon);
                        }
                        if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                        {
                            weaponCardSet.SetUp(randomWeapon);
                            weaponCardSet.cardType = CardType.CARD_WEAPON;
                        }
                    }

                    card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                    card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                    card.gameObject.SetActive(true);
                }
            }

            createdLine++;
            
        }
        playerCard.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[1].transform.position;
        playerCard.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[1].transform);
        playerCard.gameObject.SetActive(true);
        
    }

    public void DiscardUsedCard(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
        {
            cardList[(int)CardType.CARD_MONSTER].Enqueue(gameObject);
            gameObject.transform.parent = monsterCardSet.originalParent.transform;
            gameObject.SetActive(false);

        }
        else if (gameObject.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
        {
            cardList[(int)CardType.CARD_GOLDENBOX].Enqueue(gameObject);
            gameObject.transform.parent = goldenCardSet.originalParent.transform;
            gameObject.SetActive(false);

        }
        else if (gameObject.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
        {
            cardList[(int)CardType.CARD_ITEM].Enqueue(gameObject);
            gameObject.transform.parent = itemCardSet.originalParent.transform;
            gameObject.SetActive(false);

        }
        else if (gameObject.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
        {
            cardList[(int)CardType.CARD_WEAPON].Enqueue(gameObject);
            gameObject.transform.parent = weaponCardSet.originalParent.transform;
            gameObject.SetActive(false);

        }
    }

    public void SetNewLine()
    {
        int pos = 0;
        
        if (createdLine < 20)
        {
            int left = 0, center = 0, right = 0;

            if (createdLine == 19)
            {
                cardPattern = 1;
                GetCardPattern(ref left, ref center, ref right);
                if (cardList[(int)CardType.CARD_GOLDENBOX].Count > 0)
                {
                    GameObject merchant;

                    merchant = cardList[(int)CardType.CARD_GOLDENBOX].Dequeue();

                    if (merchant.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                    {
                        goldenCardInfo.SetGoldenCardData(1);
                    }
                    if (merchant.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                    {
                        goldenCardSet.SetUp(1); //상인
                        goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                    }

                    merchant.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[13].transform.position;
                    merchant.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[13].transform);
                    merchant.gameObject.SetActive(true);
                }
                
                createdLine++;
            }
            else
            {
                int randCardPattern = Random.Range(0, 4);
                cardPattern = randCardPattern;
                GetCardPattern(ref left, ref center, ref right);

                if (left != 0)
                {
                    int pool = WeightedRandom();
                    GameObject card;

                    pos = 12;

                    if (cardList[pool].Count > 0)
                    {
                        card = cardList[pool].Dequeue();

                        if (pool == (int)CardType.CARD_MONSTER)
                        {
                            int randomEnemy = Random.Range(0, 3);
                            if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                            {
                                monsterStat.SetStatData(randomEnemy);
                            }
                            if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                            {
                                monsterCardSet.SetUp(randomEnemy);
                                monsterCardSet.cardType = CardType.CARD_MONSTER;
                            }
                        }
                        else if (pool == (int)CardType.CARD_GOLDENBOX)
                        {
                            //int randGold = Random.Range(0, 2);
                            if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                            {
                                goldenCardInfo.SetGoldenCardData(0);
                            }
                            if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                            {
                                goldenCardSet.SetUp(0); //임시
                                goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                            }
                        }
                        else if (pool == (int)CardType.CARD_ITEM)
                        {
                            int randomItem = Random.Range(0, 4);
                            if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                            {
                                itemInfo.SetTempItemData(randomItem);
                            }
                            if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                            {
                                itemCardSet.SetUp(randomItem);
                                itemCardSet.cardType = CardType.CARD_ITEM;
                            }
                        }
                        else if (pool == (int)CardType.CARD_WEAPON)
                        {
                            int randomWeapon = Random.Range(0, 3);
                            if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                            {
                                weaponInfo.SetWeaponData(randomWeapon);
                            }
                            if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                            {
                                weaponCardSet.SetUp(randomWeapon);
                                weaponCardSet.cardType = CardType.CARD_WEAPON;
                            }
                        }

                        card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                        card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                        card.gameObject.SetActive(true);
                    }
                }

                if (center != 0)
                {
                    pos = 13;

                    if (pos != 1)
                    {
                        int pool = WeightedRandom();
                        GameObject card;

                        if (cardList[pool].Count > 0)
                        {
                            card = cardList[pool].Dequeue();

                            if (pool == (int)CardType.CARD_MONSTER)
                            {
                                int randomEnemy = Random.Range(0, 3);
                                if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                                {
                                    monsterStat.SetStatData(randomEnemy);
                                }
                                if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                                {
                                    monsterCardSet.SetUp(randomEnemy);
                                    monsterCardSet.cardType = CardType.CARD_MONSTER;
                                }

                            }
                            else if (pool == (int)CardType.CARD_GOLDENBOX)
                            {
                                //int randGold = Random.Range(0, 2);
                                if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                                {
                                    goldenCardInfo.SetGoldenCardData(0);
                                }
                                if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                                {
                                    goldenCardSet.SetUp(0); //임시
                                    goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                                }
                            }
                            else if (pool == (int)CardType.CARD_ITEM)
                            {
                                int randomItem = Random.Range(0, 4);
                                if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                                {
                                    itemInfo.SetTempItemData(randomItem);
                                }
                                if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                                {
                                    itemCardSet.SetUp(randomItem);
                                    itemCardSet.cardType = CardType.CARD_ITEM;
                                }
                            }
                            else if (pool == (int)CardType.CARD_WEAPON)
                            {
                                int randomWeapon = Random.Range(0, 3);
                                if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                                {
                                    weaponInfo.SetWeaponData(randomWeapon);
                                }
                                if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                                {
                                    weaponCardSet.SetUp(randomWeapon);
                                    weaponCardSet.cardType = CardType.CARD_WEAPON;
                                }
                            }

                            card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                            card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                            card.gameObject.SetActive(true);
                        }
                    }
                }

                if (right != 0)
                {
                    pos = 14;

                    int pool = WeightedRandom();
                    GameObject card;

                    if (cardList[pool].Count > 0)
                    {
                        card = cardList[pool].Dequeue();

                        if (pool == (int)CardType.CARD_MONSTER)
                        {
                            int randomEnemy = Random.Range(0, 3);
                            if (card.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                            {
                                monsterStat.SetStatData(randomEnemy);
                            }
                            if (card.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                            {
                                monsterCardSet.SetUp(randomEnemy);
                                monsterCardSet.cardType = CardType.CARD_MONSTER;
                            }

                        }
                        else if (pool == (int)CardType.CARD_GOLDENBOX)
                        {
                            //int randGold = Random.Range(0, 2);
                            if (card.TryGetComponent<GoldenCardInfo>(out GoldenCardInfo goldenCardInfo))
                            {
                                goldenCardInfo.SetGoldenCardData(0);
                            }
                            if (card.TryGetComponent<GoldenCardSet>(out GoldenCardSet goldenCardSet))
                            {
                                goldenCardSet.SetUp(0); //임시
                                goldenCardSet.cardType = CardType.CARD_GOLDENBOX;
                            }
                        }
                        else if (pool == (int)CardType.CARD_ITEM)
                        {
                            int randomItem = Random.Range(0, 4);
                            if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                            {
                                itemInfo.SetTempItemData(randomItem);
                            }
                            if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                            {
                                itemCardSet.SetUp(randomItem);
                                itemCardSet.cardType = CardType.CARD_ITEM;
                            }
                        }
                        else if (pool == (int)CardType.CARD_WEAPON)
                        {
                            int randomWeapon = Random.Range(0, 3);
                            if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                            {
                                weaponInfo.SetWeaponData(randomWeapon);
                            }
                            if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                            {
                                weaponCardSet.SetUp(randomWeapon);
                                weaponCardSet.cardType = CardType.CARD_WEAPON;
                            }
                        }

                        card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                        card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                        card.gameObject.SetActive(true);
                        
                    }
                }
                createdLine++;
            }
        }

        else if (createdLine == 20)
        {
            int left = 0, center = 0, right = 0;

            cardPattern = 1;
            GetCardPattern(ref left, ref center, ref right);

            if (cardList[(int)CardType.CARD_MONSTER].Count > 0)
            {
                GameObject boss;
                boss = cardList[(int)CardType.CARD_MONSTER].Dequeue();
                int randomEnemy = Random.Range(0, 2);

                if (boss.TryGetComponent<MonsterStat>(out MonsterStat monsterStat))
                {
                    monsterStat.SetStatData(randomEnemy);
                    monsterStat.monsterRank = MonsterRank.SMONSTER_RANK_BOSS;
                }
                if (boss.TryGetComponent<MonsterCardSet>(out MonsterCardSet monsterCardSet))
                {
                    monsterCardSet.SetUp(randomEnemy);
                    monsterCardSet.cardType = CardType.CARD_MONSTER;
                }

                boss.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[13].transform.position;
                boss.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[13].transform);
                boss.gameObject.SetActive(true);
            }
            createdLine++;
        }
    }

    public void OnMonsterDeadByWeapon(int pos)
    {
        int rand = Random.Range(0, 2);
        GameObject card;

        if (rand == 0)
        {
            if (cardList[(int)CardType.CARD_ITEM].Count > 0)
            {
                card = cardList[(int)CardType.CARD_ITEM].Dequeue();

                int randomItem = Random.Range(0, 4);
                if (card.TryGetComponent<TempItemInfo>(out TempItemInfo itemInfo))
                {
                    itemInfo.SetTempItemData(randomItem);
                }
                if (card.TryGetComponent<ItemCardSet>(out ItemCardSet itemCardSet))
                {
                    itemCardSet.SetUp(randomItem);
                    itemCardSet.cardType = CardType.CARD_ITEM;
                }

                card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                card.gameObject.SetActive(true);
            }
        }
        else if (rand == 1)
        {
            if (cardList[(int)CardType.CARD_WEAPON].Count > 0)
            {
                card = cardList[(int)CardType.CARD_WEAPON].Dequeue();

                int randomWeapon = Random.Range(0, 3);
                if (card.TryGetComponent<WeaponInfo>(out WeaponInfo weaponInfo))
                {
                    weaponInfo.SetWeaponData(randomWeapon);
                }
                if (card.TryGetComponent<WeaponCardSet>(out WeaponCardSet weaponCardSet))
                {
                    weaponCardSet.SetUp(randomWeapon);
                    weaponCardSet.cardType = CardType.CARD_WEAPON;
                }

                card.gameObject.transform.position = Managers.Instance.cardPositionManager.cardPosData[pos].transform.position;
                card.transform.SetParent(Managers.Instance.cardPositionManager.cardPosData[pos].transform);
                card.gameObject.SetActive(true);
            }
        }
    }
}
