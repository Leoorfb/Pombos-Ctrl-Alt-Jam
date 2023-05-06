using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStats
{
    public int armor;
    public int health;
    public int healthRegen;
    public float speed;
    public int projectileSize;
    public int projectileAmount;
    public float attackCooldownPct;

    internal void Sum(ItemStats stats)
    {
        armor += stats.armor;
        health += stats.health;
        healthRegen += stats.healthRegen;
        speed += stats.speed;
        projectileSize += stats.projectileSize;
        projectileAmount += stats.projectileAmount;
        attackCooldownPct += stats.attackCooldownPct;
    }
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemStats stats;
    public List<UpgradesData> upgrades;
    public int itemLevel = 1;

    public void Init(string Name, List<UpgradesData> upgrades)
    {
        this.itemName = Name;
        this.stats = new ItemStats();
        this.upgrades = upgrades;
    }

    public void Equip(Player player)
    {
        player.armor += stats.armor;
        player.hp += stats.health;
        player.maxHp += stats.health;
        //player.healthRegen += stats.healthRegen;
        player.speed += stats.speed;
        //player.projectileSize += stats.projectileSize;
        //player.projectileAmount += stats.projectileAmount;
        player.attackCooldownPct += stats.attackCooldownPct;
    }

    public void Unequip(Player player)
    {
        player.armor -= stats.armor;
        player.hp -= stats.health;
        player.maxHp -= stats.health;
        //player.healthRegen -= stats.healthRegen;
        player.speed -= stats.speed;
        //player.projectileSize -= stats.projectileSize;
        //player.projectileAmount -= stats.projectileAmount;
        player.attackCooldownPct -= stats.attackCooldownPct;
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
}
