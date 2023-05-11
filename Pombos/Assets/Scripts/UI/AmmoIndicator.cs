using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;

    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text ="Ammo: "+ currentAmmo.ToString("#000") + "/" + maxAmmo.ToString("#000");
    }
}
