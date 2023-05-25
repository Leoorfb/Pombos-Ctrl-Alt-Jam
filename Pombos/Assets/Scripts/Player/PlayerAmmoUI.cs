using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] RectTransform ammoBar;
    [SerializeField] RectTransform ammoTileStart;
    [SerializeField] RectTransform ammoTile;

    [SerializeField] float ammoContainerWidth;
    [SerializeField] float ammoTileWidth;
    [SerializeField] string ammoTileName;

    [SerializeField] int maxAmmo = 30;
    [SerializeField] int currentAmmo = 30;

    [SerializeField] WeaponBase weapon;

    void Start()
    {
        weapon = GameManager.instance.playerTransform.GetComponent<WeaponsManager>().GetWeaponBase();

        SetUpAmmoUi();
    }

    private void SetUpAmmoUi()
    {
        Debug.Log("set up ammo ui");
        ammoContainerWidth = ammoBar.sizeDelta.x;
        ammoTileWidth = ammoTile.sizeDelta.x;
    }

    private void ResetBulletTiles()
    {
        Debug.Log("reset ammo tiles");

        foreach (RectTransform child in ammoBar)
        {
            if (child.name != ammoTileName) continue;
            child.gameObject.SetActive(false);
        }
    }

    private void FillBulletTiles()
    {
        Debug.Log("fill ammo tiles");

        int count = 0;

        foreach (RectTransform child in ammoBar)
        {
            if (child.name != ammoTileName) continue;

            if (count >= currentAmmo)
                break;

            child.gameObject.SetActive(true);
            count++;
        }
    }

    private void CreateAmmoTiles()
    {
        Debug.Log("create ammo tiles");

        for (int i = 0; i < maxAmmo; i++)
        {
            RectTransform healthTile = Instantiate(ammoTile, ammoBar);
            healthTile.position = ammoTileStart.position;
            healthTile.Translate(Vector2.right * ammoTileWidth * i / 2);
            healthTile.gameObject.SetActive(true);
        }
        ammoTileName = ammoBar.GetChild(ammoBar.childCount - 1).name;
    }

    private void CleanAmmoTiles()
    {
        Debug.Log("clean ammo tiles");

        foreach (RectTransform child in ammoBar)
        {
            if (child.name != ammoTileName) continue;
            Destroy(child.gameObject);
        }
    }

    public void UpdateAmmoUi()
    {
        if (weapon == null) return;

        Debug.Log("update ammo ui");
        currentAmmo = weapon.weaponAmmo;
        ResetBulletTiles();
        FillBulletTiles();
    }

    public void UpdateMaxAmmoContainer()
    {
        if (weapon == null) return;
        Debug.Log("update ammo max container");

        maxAmmo = weapon.weaponAmmoMax;

        ammoBar.sizeDelta = new Vector2(ammoContainerWidth + (ammoTileWidth * (maxAmmo - 0.5f)), ammoBar.sizeDelta.y);
        CleanAmmoTiles();
        CreateAmmoTiles();
    }
}
