using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositionManager : MonoBehaviour
{
    public List<GameObject> cardPosData;

    public int[,] lines = new int[5, 3]
    {
        {0,1,2 },
        {3,4,5 },
        {6,7,8,},
        {9,10,11},
        {12,13,14}
    };
}
