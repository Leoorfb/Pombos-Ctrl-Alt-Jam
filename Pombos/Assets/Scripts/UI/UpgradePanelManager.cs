using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject UpgradePanel;
    PauseManager pauseManager;

    //Refatorar essa parte
    [SerializeField] List<UpgradeButton> upgradeButtons;
    [SerializeField] Button ResumeButton;
    [SerializeField] TextMeshProUGUI InstructionsText;

    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    private void Start()
    {
        HideButtons();
    }

    public void ClosePanel()
    {
        HideButtons();
        UpgradePanel.SetActive(false);
        pauseManager.UnPauseGame();
    }

    private void HideButtons()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    public void OpenPanel(List<UpgradesData> upgradesDatas)
    {
        Clean();
        UpgradePanel.SetActive(true);
        pauseManager.PauseGame();

        if (upgradesDatas.Count > 0)
        {
            for (int i = 0; i < upgradesDatas.Count; i++)
            {
                upgradeButtons[i].gameObject.SetActive(true);
                upgradeButtons[i].SetData(upgradesDatas[i]);
            }
            ResumeButton.gameObject.SetActive(false);
            InstructionsText.text = "Choose a Power Up";
        }
        else
        {
            ResumeButton.gameObject.SetActive(true);
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
        GameManager.instance.playerTransform.GetComponent<PlayerLevel>().Upgrade(pressedButtonId);
        ClosePanel();
    }
}
