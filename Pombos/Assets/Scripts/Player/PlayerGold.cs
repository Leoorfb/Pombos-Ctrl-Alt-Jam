using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public int gold = 0;
    [SerializeField] TextMeshProUGUI expText;


    private void Start()
    {
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        expText.text = "Gold: " + gold;
    }

    public void AddGold(int expAmount)
    {
        gold += expAmount;

        UpdateGoldText();
    }
}
