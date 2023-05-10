using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStats
{
    public int damage;
    public float fireRate;
    public float knockbackStrenght;
    public float spread = 0;

    public WeaponStats(int damage, float attackCooldown, float knockbackStrenght, float spread)
    {
        this.damage = damage;
        this.fireRate = attackCooldown;
        this.knockbackStrenght = knockbackStrenght;
        this.spread = spread;
    }

    public void Sum(WeaponStats weaponUpgradeStats)
    {
        this.damage += weaponUpgradeStats.damage;
        this.fireRate += weaponUpgradeStats.fireRate;
        this.knockbackStrenght += weaponUpgradeStats.knockbackStrenght;
        this.spread += weaponUpgradeStats.spread;
    }
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponStats stats;
    public GameObject weaponPrefab;
    public List<UpgradesData> weaponUpgrades;
    public Sprite icon;


    public UpgradesData GetFirstUpgrade()
    {
        //Debug.Log("Get first upgrade: " + weaponUpgrades[0].UpgradeDescription);
        return weaponUpgrades[0];
    }

    public UpgradesData GetNextUpgrade(UpgradesData upgradeData)
    {
        for (int i = 0; i+1 < weaponUpgrades.Count; i++)
        {
            if (weaponUpgrades[i].Equals(upgradeData))
            {
                return weaponUpgrades[i+1];
            }
        }
        return null;
    }
}
