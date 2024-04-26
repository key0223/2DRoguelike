using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MonsterRank
{
    MONSTER_RANK_NORMAL,
    SMONSTER_RANK_ELITE,
    SMONSTER_RANK_BOSS,
    SMONSTER_RANK_MAX,
}
public class MonsterStat : Stat
{
    MonsterCardSet monsterCardSet;

    [Header("Monster Type")]
    public MonsterType monsterType;
    public MonsterRank monsterRank;

    [Header("Drop Item")]
    public float monsterGold;

    
    private void Start()
    {
        monsterCardSet = gameObject. GetComponent<MonsterCardSet>();
    }
    public override void SetStatData(int index)
    {
        maxHp = csv.monsterDatas[index].hp;
        monsterType = (MonsterType)Enum.Parse(typeof(MonsterType),csv.monsterDatas[index].monsterType);
        effectiveRange = csv.monsterDatas[index].effectiveRange;
        attackPower = maxHp;
        currentHp = maxHp;

        SetMonsterRank();
    }

    public int WeightedMonsterRank()
    {
        float random = Random.Range(0.0f, 1.0f);
        float randomr = random * 100.0f;

        float[] percent = { 95.0f, 5.0f };

        float cumulative = 0.0f; // ´©Àû

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

    public void SetMonsterRank()
    {
        int Rank = WeightedMonsterRank();
        if (Rank == 0)
        {
            monsterRank = MonsterRank.MONSTER_RANK_NORMAL;
        }
        else
            monsterRank = MonsterRank.SMONSTER_RANK_ELITE;
    }

    public void MonsterMode()
    {
        switch(monsterRank)
        {
            case MonsterRank.MONSTER_RANK_NORMAL:
                break;

            case MonsterRank.SMONSTER_RANK_ELITE:
                monsterCardSet.character.gameObject.transform.localScale += new Vector3(transform.localScale.x * 1.3f, transform.localScale.y * 1.3f, transform.localScale.z * 1.3f);
                currentHp = maxHp * 1.5f;
                attackPower = maxHp * 1.5f;
                monsterCardSet.UpdateMonsterStat();
                Debug.Log("Monster E");
                break;

            case MonsterRank.SMONSTER_RANK_BOSS:
                monsterCardSet.character.gameObject.transform.localScale += new Vector3(transform.localScale.x * 1.3f, transform.localScale.y * 1.3f, transform.localScale.z * 1.3f);
                currentHp = maxHp * 1.5f;
                attackPower = maxHp * 3f;
                monsterCardSet.UpdateMonsterStat();
                break;

            default:
                break;
            
        }
    }

    public IEnumerator OnAddDamage(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.3f);

        if (gameObject.TryGetComponent(out PlayerStat playerStat))
        {
            currentHp -= playerStat.attackPower;
        }

        if (currentHp <= 0f)
        {
            currentHp = 0;

            Managers.Card.DiscardUsedCard(this.gameObject);
            DropGold();
            if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
            {
                playerCardSet.StartCoroutine("CoMove", monsterCardSet.posIndex);
            }
        }
    }    
    public override void AddDamage(GameObject gameObject)
    {
        StartCoroutine(Shake());

        StartCoroutine(OnAddDamage(gameObject));
        //if (gameObject.TryGetComponent(out PlayerStat playerStat))
        //{
        //    currentHp -= playerStat.attackPower;
        //}

        //if(currentHp<=0f)
        //{
        //    currentHp = 0;

        //    Managers.Card.DiscardUsedCard(this.gameObject);
        //    DropGold();
        //    if(gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
        //    {
        //        playerCardSet.StartCoroutine("CoMove", monsterCardSet.posIndex);
        //    }
        //}
    }
    public IEnumerator OnAddWeaponDamage(WeaponInfo weaponInfo)
    {
        yield return new WaitForSeconds(0.3f);

        if (weaponInfo.weaponType == WeaponType.WEAPON_SWORD)
        {
            float randValue = Random.Range(1.2f, 1.9f);
            int attackPow = Mathf.RoundToInt(3 + Managers.Instance.stageManager.currentStage * randValue);

            currentHp -= attackPow;
        }
        else if (weaponInfo.weaponType == WeaponType.WEAPON_BIG_SWORD)
        {
            float randValue = Random.Range(0.8f, 1.3f);
            int attackPow = Mathf.RoundToInt(2 + Managers.Instance.stageManager.currentStage * randValue);
            currentHp -= attackPow;
        }
        else if (weaponInfo.weaponType == WeaponType.WEAPON_GUN)
        {
            float randValue = Random.Range(0.6f, 1.1f);
            int attackPow = Mathf.RoundToInt(2 + Managers.Instance.stageManager.currentStage * randValue);
            currentHp -= attackPow;
        }

        if (currentHp <= 0f)
        {
            currentHp = 0;

            Managers.Card.DiscardUsedCard(this.gameObject);

            Managers.Card.OnMonsterDeadByWeapon(monsterCardSet.posIndex);
            DropGold();
            //if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
            //{
            //    playerCardSet.StartCoroutine("CoMove", monsterCardSet.posIndex);
            //}
        }

    }
    public void AddWeaponDamage(WeaponInfo weaponInfo)
    {
        StartCoroutine(Shake());

        StartCoroutine(OnAddWeaponDamage(weaponInfo));
        //if (weaponInfo.weaponType == WeaponType.WEAPON_SWORD)
        //{
        //    float randValue = Random.Range(1.2f, 1.9f);
        //    int attackPow = Mathf.RoundToInt(3 + Managers.Instance.stageManager.currentStage * randValue);

        //    currentHp -= attackPow;
        //}
        //else if(weaponInfo.weaponType == WeaponType.WEAPON_BIG_SWORD)
        //{
        //    float randValue = Random.Range(0.8f, 1.3f);
        //    int attackPow = Mathf.RoundToInt(2 + Managers.Instance.stageManager.currentStage * randValue);
        //    currentHp -= attackPow;
        //}
        //else if(weaponInfo.weaponType == WeaponType.WEAPON_GUN)
        //{
        //    float randValue = Random.Range(0.6f, 1.1f);
        //    int attackPow = Mathf.RoundToInt(2 + Managers.Instance.stageManager.currentStage * randValue);
        //    currentHp -= attackPow;
        //}

        //if (currentHp <= 0f)
        //{
        //    currentHp = 0;

        //    Managers.Card.DiscardUsedCard(this.gameObject);

        //    Managers.Card.OnMonsterDeadByWeapon(monsterCardSet.posIndex);
        //    DropGold();
        //    //if (gameObject.TryGetComponent(out PlayerCardSet playerCardSet))
        //    //{
        //    //    playerCardSet.StartCoroutine("CoMove", monsterCardSet.posIndex);
        //    //}
        //}
    }

    public void DropGold()
    {

        if (monsterRank == MonsterRank.MONSTER_RANK_NORMAL)
        {
            float rand = Random.Range(1.0f, 1.6f);
            monsterGold = 2 + Managers.Instance.stageManager.currentStage * rand;
        }
        else if (monsterRank == MonsterRank.SMONSTER_RANK_ELITE)
        {
            float rand = Random.Range(1.0f, 1.6f);
            monsterGold = (2 + Managers.Instance.stageManager.currentStage * rand) * 1.5f;

        }
        else if (monsterRank == MonsterRank.SMONSTER_RANK_BOSS)
        {
            monsterGold = 10 + Managers.Instance.stageManager.currentStage * 10;
        }

        if(Managers.Instance.dropItemManager.goldPool.Count>0)
        {
            GameObject gold = Managers.Instance.dropItemManager.goldPool.Dequeue();
            gold.gameObject.SetActive(true);
            gold.transform.position = transform.position;

            if (gold.TryGetComponent<GoldUI>(out GoldUI goldUI))
            {
                goldUI.SetGoldText(monsterGold);
            }

            //gold.transform.position = transform.position;
            
        }
    }
}
