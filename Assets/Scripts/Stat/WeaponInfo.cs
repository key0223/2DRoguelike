using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public string weaponName;
    public WeaponType weaponType;
    public int atkPower;
    public string image;

    CvsReader csv;
    
    void OnEnable()
    {
        csv = Managers.Instance.GetComponent<CvsReader>();
    }

    
   public void SetWeaponData(int index)
    {
        weaponName = csv.weaponDatas[index].name;
        weaponType = (WeaponType)Enum.Parse(typeof(WeaponType), csv.weaponDatas[index].weaponType);
        atkPower = csv.weaponDatas[index].atkPower;
        image = csv.weaponDatas[index].weaponImage;
    }
}
