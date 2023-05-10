using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStats
{
    [Header("Player Health Settings")]
    public int health;
    public int healthRegen;

    [Header("Player Stats Settings")]
    public int armor;
    public float speed;

    [Header("Weapon Stats Settings")]
    public int baseDamage = 0;
    public float fireRateReductionPct = 0;
    public float spreadReductionPct = 0;

    internal void Sum(ItemStats stats)
    {
        health += stats.health;
        healthRegen += stats.healthRegen;

        armor += stats.armor;
        speed += stats.speed;

        fireRateReductionPct += stats.fireRateReductionPct;
        baseDamage += stats.baseDamage;
        spreadReductionPct += stats.spreadReductionPct;
    }
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemStats stats;
    public List<UpgradesData> upgrades;
    public int itemLevel = 1;
    public Sprite icon;

    public void Init(string Name, List<UpgradesData> upgrades)
    {
        this.itemName = Name;
        this.stats = new ItemStats();
        this.upgrades = upgrades;
    }

    public void Equip(Player player)
    {
        player.maxHp += stats.health;
        player.hp += stats.health;
        player.healthRegen += stats.healthRegen;

        player.armor += stats.armor;
        player.speed += stats.speed;

        player.fireRateReductionPct += stats.fireRateReductionPct;
        player.baseDamage += stats.baseDamage;
        player.spreadReductionPct += stats.spreadReductionPct;
    }

    public void Unequip(Player player)
    {
        player.hp -= stats.health;
        player.maxHp -= stats.health;
        player.healthRegen -= stats.healthRegen;

        player.armor -= stats.armor;
        player.speed -= stats.speed;

        player.fireRateReductionPct -= stats.fireRateReductionPct;
        player.baseDamage -= stats.baseDamage;
        player.spreadReductionPct -= stats.spreadReductionPct;
    }

    public UpgradesData GetFirstUpgrade()
    {
        //Debug.Log("Get first upgrade: " + weaponUpgrades[0].UpgradeDescription);
        return upgrades[0];
    }

    public virtual void ItemLevelUp()
    {
        itemLevel++;
    }

    public virtual UpgradesData GetNextUpgrade()
    {
        UpgradesData ud = null;

        if (itemLevel < upgrades.Count)
        {
            ud = upgrades[itemLevel];
        }
        return ud;
    }

    public virtual UpgradesData GetNextUpgradeAndLevelUp()
    {
        ItemLevelUp();
        return GetNextUpgrade();
    }

    public UpgradesData GetNextUpgrade(UpgradesData upgradeData)
    {
        for (int i = 0; i + 1 < upgrades.Count; i++)
        {
            Debug.Log("Ind: " + i + ", total: " + upgrades.Count);
            if (upgrades[i].Equals(upgradeData))
            {
                return upgrades[i + 1];
            }
        }
        return null;
    }
}
