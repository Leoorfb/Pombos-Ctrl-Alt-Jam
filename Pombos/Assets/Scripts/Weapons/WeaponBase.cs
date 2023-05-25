using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] GameEvent onAmmoChange;
    [SerializeField] GameEvent onMaxAmmoChange;

    public Transform projectilesContainer;
    public WeaponData weaponData;
    public int weaponLevel = 1;
    [SerializeField] protected GameObject projectilePrefab;
    protected bool isAttackOnCooldown = false;

    public Transform projectileOrigin;

    public WeaponStats weaponStats;
    public float baseWeaponFirerate = 1;
    public float baseWeaponSpread = 1;
    public int baseWeaponDamage = 1;
    public int baseWeaponAmmoMax = 100;

    protected int _weaponAmmoMax = 100;
    public int weaponAmmoMax
    {
        get { return _weaponAmmoMax; }
        set { _weaponAmmoMax = value;
            onMaxAmmoChange.TriggerEvent();
        }
    }
    protected int _weaponAmmo;
    public int weaponAmmo
    {
        get { return _weaponAmmo; }
        set
        {   _weaponAmmo = value;
            onAmmoChange.TriggerEvent();
        }
    }
    public float reloadTime;
    protected bool hasAmmo = true;

    protected Vector3 spreadDirection = Vector3.zero;

    protected ObjectPool<WeaponProjectile> _projectilePool;

    protected Player _player;

    protected virtual void Start()
    {
        _player = GameManager.instance.playerTransform.GetComponent<Player>();
        _projectilePool = new ObjectPool<WeaponProjectile>(CreateProjectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);

        onMaxAmmoChange.TriggerEvent();
        weaponAmmo = weaponAmmoMax;
    }

    public void Update()
    {
        if (Input.GetMouseButton(0) && !isAttackOnCooldown && _player.isAlive)
        {
            Attack();
            StartCoroutine("CooldownAttack");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponAmmo = 0;
            hasAmmo = false;
            StartCoroutine("Reload");
        }
    }

    public abstract void Attack();


    public virtual void HitEnemy(Enemy enemy, Vector2 knockBackDir)
    {
        if (_player.RollCrit())
        {
            enemy.GetHit(weaponStats.damage * _player.critModifier, true, knockBackDir, weaponStats.knockbackStrenght);
        }
        else
        {
            enemy.GetHit(weaponStats.damage, false, knockBackDir, weaponStats.knockbackStrenght);
        }
    }

    public virtual IEnumerator Reload()
    {
        _player.playerBodyAnimator.SetBool("isReloading", true);
        AudioManager.instance.Play("Reload");
        yield return new WaitForSeconds(reloadTime);
        weaponAmmo = weaponAmmoMax;
        hasAmmo = true;
        _player.playerBodyAnimator.SetBool("isReloading", false);
    }

    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;

        weaponStats = new WeaponStats(weaponData.stats.damage, weaponData.stats.fireRate, weaponData.stats.knockbackStrenght, weaponData.stats.spread, weaponData.stats.ammoMax);
        weaponAmmoMax = weaponStats.ammoMax;
        weaponAmmo = weaponAmmoMax;
        baseWeaponFirerate = weaponStats.fireRate;
        baseWeaponSpread = weaponStats.spread;
        baseWeaponDamage = weaponStats.damage;
        baseWeaponAmmoMax = weaponStats.ammoMax;
    }

    public IEnumerator CooldownAttack()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(.03f);
        _player.playerBodyAnimator.SetBool("isShooting", false);
        yield return new WaitForSeconds(weaponStats.fireRate);
        isAttackOnCooldown = false;
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
        WeaponProjectile projectile = GameObject.Instantiate(projectilePrefab, projectileOrigin.position, transform.rotation, projectilesContainer).GetComponent<WeaponProjectile>();
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

        projectile.transform.position = projectileOrigin.position;
        projectile.transform.rotation = transform.rotation;

        //Debug.Log("Projetil Take From Pool");

    }
    protected virtual void OnDestroyPoolObject(WeaponProjectile projectile)
    {
        Destroy(projectile.gameObject);
        //Debug.Log("Projetil Destroy From Pool");
    }
}
