using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int hp;
    public int effectiveRange;
    public string characterImage;
}
[System.Serializable]
public class MonsterData
{
    public string name;
    public string monsterType;
    public int hp;
    public int effectiveRange;
    public string characterImage;
}

[System.Serializable]
public class GoldenBoxData
{
    public string name;
    public string goldenCardType;
    public string image;
}
[System.Serializable]
public class TempItemData
{
    public string name;
    public string itemType;
    public string itemImage;
}
[System.Serializable]
public class WeaponData
{
    public string name;
    public string weaponType;
    public int atkPower;
    public string weaponImage;
}
[System.Serializable]
public enum MonsterType
{
    MONSTER_NORMAL,
    MONSTER_LONG_RANGED,
    MONSTER_MAX,
}

[System.Serializable]
public enum ItemType
{
    POTION_HEAL,
    POTION_CONTINUOUS_HEAL,
    POTION_CLEAR_STATUS_EFFECT,
    SHIELD,
    ITEM_MAX,
}
[System.Serializable]
public enum WeaponType
{
    WEAPON_SWORD,
    WEAPON_BIG_SWORD,
    WEAPON_GUN,
    WEAPON_MAX,
}

[System.Serializable]
public enum GoldenCardType
{
    GOLDEN_BOX,
    GOLDEN_MERCHANT,
    GOLDEN_MAX,
}

[System.Serializable]
public class CvsReader : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();
    public List<PlayerData> playerDatas = new List<PlayerData>();
    public List<GoldenBoxData> goldenBoxDatas = new List<GoldenBoxData>();
    public List<TempItemData> tempItemDatas = new List<TempItemData>();
    public List<WeaponData> weaponDatas = new List<WeaponData>();

    bool endOfMonsterFile = false;
    bool endOfPlayerFile = false;
    bool endOfGoldenBoxFile = false;
    bool TempTempItemInfo = false;
    bool endOfWeaponFile = false;

    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public void ReadPlayerDataTest(string file)
    {
        TextAsset data =  Resources.Load< TextAsset>(file);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines == null)
        {
            endOfPlayerFile = true;
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] dataValue = lines[i].Split(",");

            PlayerData player = new PlayerData();
            player.name = dataValue[0];
            if (player.name == "")
                continue;
            player.hp = int.Parse(dataValue[1]);
            player.effectiveRange = int.Parse(dataValue[2]);
            player.characterImage = dataValue[3];
            playerDatas.Add(player);            
        }

    }
    //public void ReadPlayerDatas(string file)
    //{
    //    using (StreamReader sr = new StreamReader(Application.dataPath+ "/Resources" +"/CsvData"+ "/" + file))
    //    {
    //        string datas = sr.ReadLine();

    //        if (datas == null)
    //        {
    //            endOfPlayerFile = true;
    //            //sr.Close();
    //            return;
    //        }

    //        while (true)
    //        {
    //            datas = sr.ReadLine();

    //            if (datas == null)
    //            {
    //                endOfPlayerFile = true;
    //                break;
    //            }

    //            string[] dataValue = datas.Split(',');

    //            PlayerData player = new PlayerData();
    //            player.name = dataValue[0];
    //            player.hp = int.Parse(dataValue[1]);
    //            player.effectiveRange = int.Parse(dataValue[2]);
    //            player.characterImage = dataValue[3];
    //            playerDatas.Add(player);
    //        }

    //        //sr.Close();
    //    }
    //}
    public void ReadMonsterDatas(string file)
    {
        TextAsset data = Resources.Load<TextAsset>(file);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines == null)
        {
            endOfPlayerFile = true;
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] dataValue = lines[i].Split(",");

            MonsterData monster = new MonsterData();
            monster.name = dataValue[0];
            if (monster.name == "")
                continue;
            monster.monsterType = dataValue[1];
            monster.hp = int.Parse(dataValue[2]);
            monster.effectiveRange = int.Parse(dataValue[3]);
            monster.characterImage = dataValue[4];
            monsterDatas.Add(monster);
        }

        #region
        //using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources" + "/CsvData" + "/" + file))
        //{
        //    string datas = sr.ReadLine();

        //    if (datas == null)
        //    {
        //        endOfMonsterFile = true;
        //        //sr.Close();
        //        return;
        //    }

        //    while (true)
        //    {
        //        datas = sr.ReadLine();

        //        if (datas == null)
        //        {
        //            endOfMonsterFile = true;
        //            break;
        //        }

        //        string[] dataValue = datas.Split(',');

        //        MonsterData monster = new MonsterData();
        //        monster.name = dataValue[0];
        //        monster.monsterType = dataValue[1];
        //        monster.hp = int.Parse(dataValue[2]);
        //        monster.effectiveRange = int.Parse(dataValue[3]);
        //        monster.characterImage = dataValue[4];
        //        monsterDatas.Add(monster);
        //    }

        //    //sr.Close();
        //}
        #endregion
    }

    public void ReadGoldenBoxDatas(string file)
    {
        TextAsset data = Resources.Load<TextAsset>(file);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines == null)
        {
            endOfGoldenBoxFile = true;
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] dataValue = lines[i].Split(",");

            GoldenBoxData goldenBox = new GoldenBoxData();
            goldenBox.name = dataValue[0];
            if (goldenBox.name == "")
                continue;
            goldenBox.goldenCardType = dataValue[1];
            goldenBox.image = dataValue[2];
            goldenBoxDatas.Add(goldenBox);
        }

        #region
        /*
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources" + "/CsvData" + "/" + file))
        {
            string datas = sr.ReadLine();

            if (datas == null)
            {
                endOfGoldenBoxFile = true;
                //sr.Close();
                return;
            }

            while (true)
            {
                datas = sr.ReadLine();

                if (datas == null)
                {
                    endOfGoldenBoxFile = true;
                    break;
                }

                string[] dataValue = datas.Split(',');

                GoldenBoxData goldenBox = new GoldenBoxData();
                goldenBox.name = dataValue[0];
                goldenBox.boxImage = dataValue[1];
                goldenBox.percentage = int.Parse(dataValue[2]);
                goldenBoxDatas.Add(goldenBox);
            }

            //sr.Close();
        }
        */
        #endregion
    }
    public void ReadTempItemDatas(string file)
    {
        TextAsset data = Resources.Load<TextAsset>(file);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines == null)
        {
            TempTempItemInfo = true;
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] dataValue = lines[i].Split(",");

            TempItemData itemData = new TempItemData();

            itemData.name = dataValue[0];
            if (itemData.name == "")
                continue;
            itemData.itemType = dataValue[1];
            itemData.itemImage = dataValue[2];

            tempItemDatas.Add(itemData);
        }

        #region
        /*
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources" + "/CsvData" + "/" + file))
        {
            string datas = sr.ReadLine();

            if (datas == null)
            {
                TempTempItemInfo = true;
                //sr.Close();
                return;
            }

            while (true)
            {
                datas = sr.ReadLine();

                if (datas == null)
                {
                    TempTempItemInfo = true;
                    break;
                }

                string[] dataValue = datas.Split(',');

                TempItemData itemData = new TempItemData();

                itemData.name = dataValue[0];
                itemData.itemType = dataValue[1];
                itemData.itemImage = dataValue[2];

                tempItemDatas.Add(itemData);
            }
        }
        */
        #endregion
    }

    public void ReadWeaponDatas(string file)
    {
        TextAsset data = Resources.Load<TextAsset>(file);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines == null)
        {
            endOfWeaponFile = true;
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] dataValue = lines[i].Split(",");

            WeaponData weaponData = new WeaponData();

            weaponData.name = dataValue[0];
            if (weaponData.name == "")
                continue;
            weaponData.weaponType = dataValue[1];
            weaponData.atkPower = int.Parse(dataValue[2]);
            weaponData.weaponImage = dataValue[3];

            weaponDatas.Add(weaponData);
        }

        #region
        /*
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources" + "/CsvData" + "/" + file))
        {
            string datas = sr.ReadLine();

            if (datas == null)
            {
                endOfWeaponFile = true;
                //sr.Close();
                return;
            }

            while (true)
            {
                datas = sr.ReadLine();

                if (datas == null)
                {
                    endOfWeaponFile = true;
                    break;
                }

                string[] dataValue = datas.Split(',');

                WeaponData weaponData = new WeaponData();

                weaponData.name = dataValue[0];
                weaponData.weaponType = dataValue[1];
                weaponData.atkPower = int.Parse(dataValue[2]);
                weaponData.effectiveRange_F = int.Parse(dataValue[3]);
                weaponData.effectiveRange_D = int.Parse(dataValue[4]);
                weaponData.weaponImage = dataValue[5];

                weaponDatas.Add(weaponData);
            }
        }
        */
        #endregion
    }
    //public void ReadPlayerTempItemDatas(string file)
    //{
    //    using (StreamReader sr = new StreamReader(Application.dataPath + "/" + file))
    //    {
    //        string datas = sr.ReadLine();

    //        if (datas == null)
    //        {
    //            TempTempItemInfo = true;
    //            //sr.Close();
    //            return;
    //        }

    //        while (true)
    //        {
    //            datas = sr.ReadLine();

    //            if (datas == null)
    //            {
    //                TempTempItemInfo = true;
    //                break;
    //            }

    //            string[] dataValue = datas.Split(',');

    //            TempItemData itemData = new TempItemData();

    //            itemData.name = dataValue[0];
    //            itemData.itemType = dataValue[1];
    //            itemData.itemImage = dataValue[2];

    //        }
    //    }
    //}

}
