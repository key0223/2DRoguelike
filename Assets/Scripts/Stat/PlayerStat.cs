using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : Stat
{
    [SerializeField] TextMeshProUGUI hpText;

    PlayerItemManager playerItemManager;
    PlayerCardSet playerCardSet;
    PlayerAttackedUI playerAttackedUI;

    public float shiledHp;
    public int potionStack;
    public int stackCount = 0;

    private void Start()
    {
        playerItemManager = gameObject.GetComponent<PlayerItemManager>();
        playerCardSet = gameObject.GetComponent<PlayerCardSet>();
        playerCardSet.PlayerMoved += StackPotionHeal;

        playerAttackedUI = FindObjectOfType<PlayerAttackedUI>();
    }
    public  void OnEnable()
    {
        currentHp = maxHp;
        hpText.text = currentHp.ToString();
    }

    public override void SetStatData(int index)
    {
        maxHp = csv.playerDatas[index].hp;
        effectiveRange = csv.playerDatas[index].effectiveRange;
        attackPower = maxHp;
        currentHp = maxHp;
    }
    public override void AddDamage(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out MonsterStat monsterStat))
        {
            playerAttackedUI.OnAttacked();
            StartCoroutine(AlphaBlink());
            float monsterPow = monsterStat.attackPower;

            if (shiledHp >= monsterPow)
            {
                shiledHp -= monsterPow;
            }
            else if (shiledHp < monsterPow)
            {
                monsterPow -= shiledHp;
                shiledHp = 0;
                currentHp -= monsterPow;
            }

            playerItemManager.TempItemUpdate();
            UpdatePlayerCurrnetHp();
        }

        if (currentHp <= 0f)
        {
            currentHp = 0;
            this.gameObject.SetActive(false);
        }

    }

    public float HealHp(int hp)
    {
        currentHp = currentHp + hp;
        UpdatePlayerCurrnetHp();
        Debug.Log(hp);
        return currentHp;
    }

    public void StackPotionCount(int stackValue)
    {
        playerItemManager.TempItemUpdate();
        if (potionStack > 0)
        {
            int randValue = Random.Range(1, 3);
            int itemValue = stackValue * randValue;

            if (itemValue < 2)
            {
                itemValue = 2;
            }
            if (itemValue > 10)
            {
                itemValue = 10;
            }

            currentHp = currentHp + itemValue;
            playerItemManager.TempItemUpdate();
        }
    }

    public void StackPotionHeal()
    {
        stackCount++;

        if (potionStack >0)
        {
            int randValue = Random.Range(1, 3);
            int itemValue =  stackCount* randValue;

            if (itemValue < 2)
            {
                itemValue = 2;
            }
            if (itemValue > 10)
            {
                itemValue = 10;
            }

            currentHp = currentHp += itemValue;
            Debug.Log("Æ÷¼Ç Èú");
            potionStack--;
            playerItemManager.TempItemUpdate();
            UpdatePlayerCurrnetHp();
        }
        if(potionStack<=0)
        {
            stackCount = 0;
        }
        
    }

    public void Getshiled(int shiledValue)
    {
        if (shiledValue < 0)
        {
            shiledValue = 1;
        }
        if (shiledValue > 7)
        {
            shiledValue = 7;
        }

        shiledHp = shiledHp + shiledValue;
        playerItemManager.TempItemUpdate();
    }


    public void UpdatePlayerCurrnetHp()
    {
        hpText.text = currentHp.ToString();

        Managers.Instance.stageUIManager.UpdatePlayerHp(currentHp, maxHp);
    }
}
