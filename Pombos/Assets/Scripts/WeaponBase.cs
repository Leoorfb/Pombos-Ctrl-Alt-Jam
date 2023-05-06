using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WeaponBase : MonoBehaviour
{
    public Transform projectilesContainer;
    public WeaponData weaponData;
    public float attackCooldown = 1;
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

    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;
        attackCooldown = weaponData.stats.attackCooldown;

        weaponStats = new WeaponStats(weaponData.stats.damage, attackCooldown);
    }

    public IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
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
        return projectile;
    }

    protected virtual void DisableProjectile(WeaponProjectile obj)
    {
        _projectilePool.Release(obj);
    }

    protected virtual void OnReturnedToPool(WeaponProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    protected virtual void OnTakeFromPool(WeaponProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    protected virtual void OnDestroyPoolObject(WeaponProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
