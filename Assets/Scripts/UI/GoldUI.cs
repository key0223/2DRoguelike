using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public float earnedGold;
    TextMeshProUGUI goldText;
    PlayerItemManager playerItemManager;

    float smoothTime = 0.1f;
    public float yVelocity = 2f;
    public float xVelocity = 2f;

    void Awake()
    {
        goldText = GetComponentInChildren<TextMeshProUGUI>();
        playerItemManager = FindObjectOfType<PlayerItemManager>();
    }
    private void Update()
    {
        Vector3 playerPos = playerItemManager.transform.position;

        float newPosX = Mathf.SmoothDamp(transform.position.x, playerPos.x, ref xVelocity , smoothTime);
        float newPosY = Mathf.SmoothDamp(transform.position.y, playerPos.y, ref yVelocity, smoothTime);

        transform.position = new Vector3(newPosX, newPosY, transform.position.z);
    }
    public void SetGoldText(float gold)
    {
        earnedGold = gold;
        goldText.text = earnedGold.ToString("F2");
    }
}
