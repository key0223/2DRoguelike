using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    ResourceManager _resource = new ResourceManager();
    CardManager _card = new CardManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static CardManager Card { get { return Instance._card; } }


    //Unit Data
    CvsReader cvsReader;
    CsvWriter csvWriter;
    
    //변수
    public CardPositionManager cardPositionManager;
    public DropItemManager dropItemManager;
    public StageUIManager stageUIManager;
    public StageManager stageManager;

    private void Awake()
    {
        Init();
        cvsReader = GetComponent<CvsReader>();
        csvWriter = GetComponent<CsvWriter>();

        cvsReader.ReadPlayerDataTest("CsvData/PlayerData");
        cvsReader.ReadMonsterDatas("CsvData/MonsterData");
        cvsReader.ReadGoldenBoxDatas("CsvData/GoldenBoxData");
        cvsReader.ReadTempItemDatas("CsvData/TempItemData");
        cvsReader.ReadWeaponDatas("CsvData/WeaponData");

        cardPositionManager = GetComponentInChildren<CardPositionManager>();
        dropItemManager = GetComponentInChildren<DropItemManager>();
        stageManager = GetComponentInChildren<StageManager>();
        stageUIManager = GetComponentInChildren<StageUIManager>();

        Card.InitializeCardObject();
        Card.SetCardPos();
    }
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" }; //새로 만들어 컴포넌트 추가
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}
