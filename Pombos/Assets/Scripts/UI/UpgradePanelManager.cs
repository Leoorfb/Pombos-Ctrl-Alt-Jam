using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject UpgradePanel;
    PauseManager pauseManager;

    //Refatorar essa parte
    [SerializeField] List<UpgradeButton> upgradeButtons;
    [SerializeField] Button ResumeButton;
    [SerializeField] TextMeshProUGUI InstructionsText;
    [SerializeField] TextMeshProUGUI RerollCostText;

    Shop shop;

    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    private void Start()
    {
        HideButtons();
        shop = GameManager.instance.shop;
    }

    public void ClosePanel()
    {
        UpgradePanel.SetActive(false);
        pauseManager.UnPauseGame();
        GameManager.instance.shop.OnShopClose();
    }

    private void HideButtons()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    public void OpenPanel(List<UpgradesData> upgradesData)
    {
        UpgradePanel.SetActive(true);
        pauseManager.PauseGame();
        LoadUpgrades(upgradesData);
    }

    public void LoadUpgrades(List<UpgradesData> upgradesData)
    {
        UpdateRerollCostText();
        HideButtons();
        Clean();
        
        if (upgradesData.Count > 0)
        {
            for (int i = 0; i < upgradesData.Count - 1; i++)
            {
                //Debug.Log(i);
                upgradeButtons[i].gameObject.SetActive(true);
                upgradeButtons[i].SetData(upgradesData[i]);
                upgradeButtons[i].SetActiveButton(true);
            }
            //ResumeButton.gameObject.SetActive(false);
            InstructionsText.text = "Click on a Power Up to buy it";
        }
        else
        {
            //ResumeButton.gameObject.SetActive(true);
            InstructionsText.text = "No Power Ups avaible";
        }
    }

    public void Clean()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].Clean();
        }
    }

    public void Upgrade(int pressedButtonId)
    {
        //Debug.Log("Bot�o upgrade");
        List<UpgradesData> updatedUpgrades;
        bool upgradeSucceded = shop.Upgrade(pressedButtonId, out updatedUpgrades);
        if (upgradeSucceded)
        {
            upgradeButtons[pressedButtonId].SetActiveButton(false);
            //Debug.Log("Bot�o upgrade sucedido");

            //ClosePanel();
            //LoadUpgrades(updatedUpgrades);
        }
    }

    public void Reroll()
    {
        bool upgradeSucceded = shop.RerollUpgrades();
        if (upgradeSucceded)
        {
            UpdateRerollCostText();
        }
    }

    public void UpdateRerollCostText()
    {
        RerollCostText.text = "$"+shop.rerollCost.ToString("#000");
    }
}
