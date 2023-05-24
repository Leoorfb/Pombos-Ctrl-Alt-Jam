using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    EnemySpawner enemySpawner;
    bool interactPressed = false;
    bool isPlayerInTrigger = false;
    bool isShopOpen = false;

    [Header("Shop UI Settings")]
    [SerializeField] UpgradePanelManager shopPanel;

    [Header("Shop Upgrades Settings")]
    [SerializeField] List<UpgradesData> upgrades;
    List<UpgradesData> selectedUpgrades;

    [SerializeField] List<UpgradesData> startAvailableUpgrades;

    [Header("Shop Reroll Settings")]
    [SerializeField] int firstRerollCost = 10;
    public int rerollCost = 10;
    public int rerollMaxCost = 80;

    WeaponsManager weaponsManager;
    PassivesManager passivesManager;
    PlayerGold playerGold;

    private void Start()
    {
        enemySpawner = GameManager.instance.enemySpawner;
        weaponsManager = GameManager.instance.playerTransform.GetComponent<WeaponsManager>();
        passivesManager = GameManager.instance.playerTransform.GetComponent<PassivesManager>();
        playerGold = GameManager.instance.playerTransform.GetComponent<PlayerGold>();

        AddUpgradesIntoTheListOfUpgrades(startAvailableUpgrades);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactPressed = true;
            //Debug.Log("Interact: " + interactPressed + "- Wave Running: " + enemySpawner.isWaveRunning + "- ShopOpen: " + isShopOpen);
            if (interactPressed && !enemySpawner.isWaveRunning && !isShopOpen && isPlayerInTrigger)
            {
                //Debug.Log("Abre loja");
                OpenShop();
            }
        }
        else
        {
            interactPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerInTrigger = true;
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerInTrigger = false;
        }
    }


    private void OpenShop()
    {
        rerollCost = firstRerollCost;
        isShopOpen = true;
        SelectUpgrades();
        shopPanel.OpenPanel(selectedUpgrades);
    }

    private void CloseShop()
    {
        shopPanel.ClosePanel();
    }

    public void OnShopClose()
    {
        isShopOpen = false;
        enemySpawner.StartWave();
    }

    public bool RerollUpgrades()
    {
        bool rerollSucceded = playerGold.gold >= rerollCost;
        if (rerollSucceded)
        {
            SelectUpgrades();
            shopPanel.LoadUpgrades(selectedUpgrades);
            playerGold.gold -= rerollCost;
            rerollCost *= 2;
            if (rerollCost > rerollMaxCost) rerollCost = rerollMaxCost;
        }
        return rerollSucceded;
    }

    public void SelectUpgrades()
    {
        if (selectedUpgrades == null) selectedUpgrades = new List<UpgradesData>();
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetUpgrades(3));

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

    public bool Upgrade(int SelectecUpgradeId, out List<UpgradesData> updatedUpgradesData)
    {
        UpgradesData upgradesData = selectedUpgrades[SelectecUpgradeId];

        bool playerHasEnoughtMoney = playerGold.gold >= upgradesData.cost;
        

        if (playerHasEnoughtMoney) {
            UpgradesData nextUpgrade = null;
            switch (upgradesData.UpgradeType)
            {
                case UpgradeType.WeaponUpgrade:
                    weaponsManager.UpgradeWeapon(upgradesData, out nextUpgrade);
                    break;

                case UpgradeType.WeaponUnlock:
                    weaponsManager.AddWeapon(upgradesData.weaponData, out nextUpgrade);

                    nextUpgrade = upgradesData.weaponData.GetNextUpgrade(upgradesData);
                    break;

                case UpgradeType.ItemUpgrade:
                    passivesManager.UpgradeItem(upgradesData, out nextUpgrade);
                    break;

                case UpgradeType.ItemUnlock:
                    passivesManager.Equip(upgradesData.item, out nextUpgrade);
                    break;
            }

            /*
            Debug.Log(nextUpgrade);
            if (nextUpgrade != null)
            {
                selectedUpgrades.Add(nextUpgrade);
            }
            selectedUpgrades.Remove(upgradesData);
            */

            upgrades.Remove(upgradesData);
            playerGold.AddGold(-upgradesData.cost);
        }
        updatedUpgradesData = selectedUpgrades;
        return playerHasEnoughtMoney;
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
