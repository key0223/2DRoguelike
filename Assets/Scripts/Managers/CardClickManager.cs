using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickManager : MonoBehaviour
{
    PlayerStat playerStat;
    PlayerCardSet playerCardSet;
    PlayerItemManager playerItemManager;

    public bool marketOn { get; set; } = false;

    private void Start()
    {
        playerCardSet = FindObjectOfType<PlayerCardSet>();
        playerStat = FindObjectOfType<PlayerStat>();
        playerItemManager = FindObjectOfType<PlayerItemManager>();
    }

    private void Update()
    {
        LayerMask mask;
        if (marketOn == false)
        {
            mask = LayerMask.GetMask("Default");
        }
        else
        {
            mask = LayerMask.GetMask("StageUI");
        }


        if (playerCardSet.currentCardCurrentLine == CardCurrentLine.CardLine0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 100.0f, mask);
                if (hit.collider == null)
                    return;

                if (hit.collider.gameObject)
                {
                    if (hit.collider.gameObject.CompareTag("Monster"))
                    {
                        MonsterStat monsterstat = hit.collider.gameObject.GetComponent<MonsterStat>();
                        MonsterCardSet monsterCardSet = hit.collider.gameObject.GetComponent<MonsterCardSet>();
                        if (monsterCardSet.currentCardCurrentLine == CardCurrentLine.CardLine1)
                        {
                            if (monsterstat.effectiveRange == 1)
                            {
                                //playerStat.AddDamage(hit.collider.gameObject);
                                monsterstat.AddDamage(playerStat.gameObject);
                            }
                            else
                            {
                                playerStat.AddDamage(hit.collider.gameObject);
                                monsterstat.AddDamage(playerStat.gameObject);
                            }
                        }
                    }

                    if (hit.collider.gameObject.CompareTag("GoldenBox"))
                    {
                        GoldenCardSet goldenCardSet = hit.collider.gameObject.GetComponent<GoldenCardSet>();
                        if (goldenCardSet.currentCardCurrentLine == CardCurrentLine.CardLine1)
                        {
                            goldenCardSet.OnMouseClicked(playerCardSet.gameObject);
                        }
                    }
                    if (hit.collider.gameObject.CompareTag("Item"))
                    {
                        TempItemInfo tempItemInfo = hit.collider.gameObject.GetComponent<TempItemInfo>();
                        ItemCardSet itemCardSet = hit.collider.gameObject.GetComponent<ItemCardSet>();
                        if (itemCardSet.currentCardCurrentLine == CardCurrentLine.CardLine1)
                        {
                            playerItemManager.ActivateTempItem(tempItemInfo);
                            itemCardSet.OnMouseClicked(playerCardSet.gameObject);
                        }
                    }
                    if (hit.collider.gameObject.CompareTag("Weapon"))
                    {
                        WeaponInfo weaponInfo = hit.collider.gameObject.GetComponent<WeaponInfo>();
                        WeaponCardSet weaponCardSet = hit.collider.gameObject.GetComponent<WeaponCardSet>();
                        if (weaponCardSet.currentCardCurrentLine == CardCurrentLine.CardLine1)
                        {
                            playerItemManager.ActivateWeapon(weaponInfo);
                            weaponCardSet.OnMouseClicked(playerCardSet.gameObject);
                        }
                    }
                }
            }
        }

    }
}
