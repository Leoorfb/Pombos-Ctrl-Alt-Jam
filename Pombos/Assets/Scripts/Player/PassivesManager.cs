using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivesManager : MonoBehaviour
{
    [SerializeField] List<Item> items;

    Player player;
    Shop shop;

    [SerializeField] Item itemTest;

    private void Awake()
    {
        player = GetComponent<Player>();
        shop = GameManager.instance.shop;
    }

    private void Start()
    {
        if (itemTest != null)
        {
            Equip(itemTest);
        }
    }

    public void Equip(Item item)
    {
        if (items == null) items = new List<Item>();

        Item newItem = new Item();
        newItem.Init(item.itemName, item.upgrades);
        newItem.stats.Sum(item.stats);
        newItem.upgrades = item.upgrades;

        items.Add(newItem);
        newItem.Equip(player);
        shop.AddUpgradesIntoTheListOfUpgrades(newItem.GetFirstUpgrade());
    }

    internal void UpgradeItem(UpgradesData upgradesData)
    {
        Item itemToUpgrade = items.Find(id => id.itemName == upgradesData.item.itemName);
        itemToUpgrade.Unequip(player);
        itemToUpgrade.stats.Sum(upgradesData.itemStats);
        itemToUpgrade.Equip(player);
        shop.AddUpgradesIntoTheListOfUpgrades(itemToUpgrade.GetNextUpgradeAndLevelUp());
    }
}
