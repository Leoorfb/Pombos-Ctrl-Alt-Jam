using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WeaponBase : MonoBehaviour
{
    public Transform projectilesContainer;
    public WeaponData weaponData;
    public int weaponLevel = 1;
    [SerializeField] protected GameObject projectilePrefab;

    public WeaponStats weaponStats;

    protected ObjectPool<WeaponProjectile> _projectilePool;

    protected virtual void Start()
    {
        _projectilePool = new ObjectPool<WeaponProjectile>(CreateProjectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        StartCoroutine("CooldownAttack");
    }

    public abstract void Attack();

    public virtual void HitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(weaponStats.damage);
    }
    public virtual void HitEnemy(Enemy enemy, Vector2 knockBackDir)
    {
        HitEnemy(enemy);
        enemy.TakeKnockback(knockBackDir, weaponStats.knockbackStrenght);
    }

    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;

        weaponStats = new WeaponStats(weaponData.stats.damage, weaponData.stats.attackCooldown, weaponData.stats.knockbackStrenght);
    }

    public IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(weaponStats.attackCooldown);
        Attack();
    }

    public virtual void SetProjectileContainer(Transform nPContainer)
    {
        projectilesContainer = nPContainer;
    }

    public virtual void WeaponLevelUp()
    {
        weaponLevel++;
    }

    public virtual UpgradesData GetNextUpgrade()
    {
        UpgradesData ud = null;


        if (weaponLevel < weaponData.weaponUpgrades.Count)
        {
            ud = weaponData.weaponUpgrades[weaponLevel];
        }
        return ud;
    }

    public virtual UpgradesData GetNextUpgradeAndLevelUp()
    {
        WeaponLevelUp();
        return GetNextUpgrade();
    }

    public virtual void Upgrade(UpgradesData upgradeData)
    {
        weaponStats.Sum(upgradeData.weaponUpgradeStats);
    }

    protected virtual WeaponProjectile CreateProjectile()
    {
        WeaponProjectile projectile = GameObject.Instantiate(projectilePrefab, transform.position, transform.rotation, projectilesContainer).GetComponent<WeaponProjectile>();
        projectile.weapon = this;
        projectile.Init(DisableProjectile);
        //Debug.Log("Projetil Init");

        return projectile;
    }

    protected virtual void DisableProjectile(WeaponProjectile obj)
    {
        _projectilePool.Release(obj);
        //Debug.Log("Projetil Disable");
    }

    protected virtual void OnReturnedToPool(WeaponProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        //Debug.Log("Projetil Returned To Pool");

    }
    protected virtual void OnTakeFromPool(WeaponProjectile projectile)
    {
        projectile.gameObject.SetActive(true);

        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;

        //Debug.Log("Projetil Take From Pool");

    }
    protected virtual void OnDestroyPoolObject(WeaponProjectile projectile)
    {
        Destroy(projectile.gameObject);
        //Debug.Log("Projetil Destroy From Pool");
    }
}
