using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStats
{
    public int damage;
    public float attackCooldown;
    public float knockbackStrenght;

    public WeaponStats(int damage, float attackCooldown, float knockbackStrenght)
    {
        this.damage = damage;
        this.attackCooldown = attackCooldown;
        this.knockbackStrenght = knockbackStrenght;
    }

    public void Sum(WeaponStats weaponUpgradeStats)
    {
        this.damage += weaponUpgradeStats.damage;
        this.attackCooldown += weaponUpgradeStats.attackCooldown;
        this.knockbackStrenght += weaponUpgradeStats.knockbackStrenght;
    }
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponStats stats;
    public GameObject weaponPrefab;
    public List<UpgradesData> weaponUpgrades;

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
