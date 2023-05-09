using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] int level = 1;
    [SerializeField] int exp = 0;
    [SerializeField] int maxExp = 10;

    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] TextMeshProUGUI expText;

    // Upgrades
    
    [SerializeField] UpgradePanelManager upgradePanel;
    [SerializeField] List<UpgradesData> upgrades;
    List<UpgradesData> selectedUpgrades;

    WeaponsManager weaponsManager;
    PassivesManager passivesManager;

    [SerializeField] List<UpgradesData> startAvailableUpgrades;

    private void Awake()
    {
        weaponsManager = GetComponent<WeaponsManager>();
        passivesManager = GetComponent<PassivesManager>();
    }

    
    

    private void Start()
    {
        UpdateEXPText();
        UpdateLevelText();
        AddUpgradesIntoTheListOfUpgrades(startAvailableUpgrades);
    }

    void UpdateEXPText()
    {
        expText.text = "Player  EXP: " + exp + "/" + maxExp;
    }
    void UpdateLevelText()
    {
        lvlText.text = "Player  Level: " + level;
    }

    public void AddExp(int expAmount)
    {
        exp += expAmount;

        if (exp >= maxExp) LevelUp();

        UpdateEXPText();
    }

    public void LevelUp()
    {
        level += 1;
        exp -= maxExp;
        maxExp = level * 10;
        UpdateLevelText();

        
        if (selectedUpgrades == null) selectedUpgrades = new List<UpgradesData>();
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetUpgrades(2));

        upgradePanel.OpenPanel(selectedUpgrades);
        
    }
    
    public List<UpgradesData> GetUpgrades(int count)
    {
        List<UpgradesData> upgradesList = new List<UpgradesData>();
        List<UpgradesData> upgradesAvaible = new List<UpgradesData>(upgrades);


        if (count > upgrades.Count)
        {
            count = upgrades.Count;
        }

        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(0, upgradesAvaible.Count);
            upgradesList.Add(upgradesAvaible[x]);
            upgradesAvaible.RemoveAt(x);

        }

        return upgradesList;
    }

    public void Upgrade(int SelectecUpgradeId)
    {
        UpgradesData upgradesData = selectedUpgrades[SelectecUpgradeId];

        switch (upgradesData.UpgradeType)
        {
            case UpgradeType.WeaponUpgrade:
                weaponsManager.UpgradeWeapon(upgradesData);
                break;

            case UpgradeType.WeaponUnlock:
                weaponsManager.AddWeapon(upgradesData.weaponData);
                break;

            case UpgradeType.ItemUpgrade:
                passivesManager.UpgradeItem(upgradesData);
                break;

            case UpgradeType.ItemUnlock:
                passivesManager.Equip(upgradesData.item);
                break;
        }

        upgrades.Remove(upgradesData);

    }

    public void AddUpgradesIntoTheListOfUpgrades(UpgradesData upgrade)
    {
        if (upgrade != null)
            upgrades.Add(upgrade);
    }

    public void AddUpgradesIntoTheListOfUpgrades(List<UpgradesData> upgradesList)
    {
        if (upgradesList != null)
            upgrades.AddRange(upgradesList);
    }
}
