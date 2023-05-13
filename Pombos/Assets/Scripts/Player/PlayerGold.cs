using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerGold : MonoBehaviour
{
    public int gold = 0;
    public  UIDocument playerGoldUI;
    [SerializeField] TextMeshProUGUI expText;


    private void Start()
    {
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        Label playerGoldLabel = playerGoldUI.rootVisualElement.Q<Label>("GoldCount");
        playerGoldLabel.text = gold.ToString();
        // expText.text = "Gold: " + gold;
    }

    public void AddGold(int expAmount)
    {
        gold += expAmount;

        UpdateGoldText();
    }
}
