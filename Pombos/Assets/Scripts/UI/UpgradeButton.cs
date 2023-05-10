using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text upgradeName;
    [SerializeField] TMP_Text upgradeDescription;
    [SerializeField] TMP_Text upgradeCost;

    public void SetData(UpgradesData upgradesData)
    {
        if (upgradesData.weaponData != null)
            upgradeName.text = upgradesData.weaponData.weaponName;
        if (upgradesData.item != null)
            upgradeName.text = upgradesData.item.itemName;

        icon.sprite = upgradesData.icon;
        upgradeDescription.text = upgradesData.UpgradeDescription;
        upgradeCost.text = "$" + upgradesData.cost.ToString("#000");
    }

    internal void Clean()
    {
        icon.sprite = null;
        upgradeName.text = "";
        upgradeDescription.text = "";
    }
}
