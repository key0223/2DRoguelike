using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI stageCountText;
    public TextMeshProUGUI playerHpText;

    public void UpdateGold(float gold)
    {
        goldText.text = gold.ToString("F2") +"G";
    }

    public void UpdateStageCount()
    {
        stageCountText.text = ($"Stage {Managers.Instance.stageManager.currentStage}");

    }
    public void UpdatePlayerHp(float current, float max)
    {
        playerHpText.text = ($"{current}/{max}");
    }
}
