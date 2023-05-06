using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] Transform weaponsObjectContainer;
    [SerializeField] Transform projectilesObjectContainer;
    [SerializeField] WeaponData startingWeapon;
    //[SerializeField] int WeaponLimit = 5;

    PlayerLevel playerLevel;
    [SerializeField] List<WeaponBase> weapons;


    private void Awake()
    {
        weapons = new List<WeaponBase>();
    }

    private void Start()
    {
        playerLevel = GetComponent<PlayerLevel>();
        AddWeapon(startingWeapon);
    }

    
    public void AddWeapon(WeaponData weaponData)
    {
        GameObject weaponGameObject = GameObject.Instantiate(weaponData.weaponPrefab, weaponsObjectContainer);

        WeaponBase weapon = weaponGameObject.GetComponent<WeaponBase>();
        weapons.Add(weapon);

        weaponGameObject.GetComponent<WeaponBase>().SetData(weaponData);
        weaponGameObject.GetComponent<WeaponBase>().SetProjectileContainer(projectilesObjectContainer);

        playerLevel.AddUpgradesIntoTheListOfUpgrades(weaponData.GetFirstUpgrade());
    }

    public void UpgradeWeapon(UpgradesData upgradesData)
    {
        WeaponBase weapon = weapons.Find(wd => wd.weaponData == upgradesData.weaponData);
        weapon.Upgrade(upgradesData);
        playerLevel.AddUpgradesIntoTheListOfUpgrades(weapon.GetNextUpgradeAndLevelUp());
    }
}
