using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] Transform weaponsObjectContainer;
    [SerializeField] Transform projectilesObjectContainer;
    [SerializeField] WeaponData startingWeapon;

    Shop shop;
    [SerializeField] WeaponBase weapon;

    public Transform projectileOrigin;
    private Player player;


    private void Awake()
    {
        weapon = null;
        player = GetComponent<Player>();
    }

    private void Start()
    {
        shop = GameManager.instance.shop;
        SetWeapon(startingWeapon);
    }

    public void UpdateWeaponsStats()
    {
        weapon.weaponStats.damage = weapon.baseWeaponDamage + player.baseDamage;
        weapon.weaponStats.ammoMax = weapon.baseWeaponAmmoMax + player.baseAmmo;
        weapon.weaponAmmoMax = weapon.weaponStats.ammoMax;
        weapon.weaponStats.spread = weapon.baseWeaponSpread - (weapon.baseWeaponSpread * player.spreadReductionPct);
        weapon.weaponStats.fireRate = weapon.baseWeaponFirerate - (weapon.baseWeaponFirerate * player.fireRateReductionPct);
    }

    public WeaponBase GetWeaponBase() 
    {
        return weapon;
    }

    public void SetWeapon(WeaponData weaponData)
    {
        UpgradesData firstUpgrade;
        ClearWeapon();
        SetWeapon(weaponData, out firstUpgrade);
    }

    private void ClearWeapon()
    {
        if (weapon != null) Destroy(weapon.gameObject);
        weapon = null;
    }

    public void SetWeapon(WeaponData weaponData, out UpgradesData firstUpgrade)
    {
        GameObject weaponGameObject = GameObject.Instantiate(weaponData.weaponPrefab, weaponsObjectContainer);

        weapon = weaponGameObject.GetComponent<WeaponBase>();

        weapon.projectileOrigin = projectileOrigin;
        weaponGameObject.GetComponent<WeaponBase>().SetData(weaponData);
        weaponGameObject.GetComponent<WeaponBase>().SetProjectileContainer(projectilesObjectContainer);

        firstUpgrade = weaponData.GetFirstUpgrade();
        shop.AddUpgradesIntoTheListOfUpgrades(firstUpgrade);
    }
    public void UpgradeWeapon(UpgradesData upgradesData, out UpgradesData nextUpgrade)
    {
        weapon.Upgrade(upgradesData);
        nextUpgrade = weapon.GetNextUpgradeAndLevelUp();
        shop.AddUpgradesIntoTheListOfUpgrades(nextUpgrade);
    }
}
