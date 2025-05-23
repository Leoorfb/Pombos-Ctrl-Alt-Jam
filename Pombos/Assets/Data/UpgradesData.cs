using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum UpgradeType
{
    WeaponUpgrade,
    ItemUpgrade,
    WeaponUnlock,
    ItemUnlock
}

[CreateAssetMenu]
public class UpgradesData : ScriptableObject
{
    public UpgradeType UpgradeType;
    public string UpgradeDescription;
    public Sprite icon;

    public WeaponData weaponData;
    public WeaponStats weaponUpgradeStats;

    public Item item;
    public ItemStats itemStats;
    public int cost;

    private void OnEnable()
    {
        if (UpgradeType == UpgradeType.WeaponUpgrade || UpgradeType == UpgradeType.WeaponUnlock)
        {
            icon = weaponData.icon;
        }
        else if (UpgradeType == UpgradeType.ItemUpgrade || UpgradeType == UpgradeType.ItemUnlock)
        {
            icon = item.icon;
        }
    }
}
