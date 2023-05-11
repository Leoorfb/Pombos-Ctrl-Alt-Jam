using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : WeaponBase
{
    public override void Attack()
    {
        if (weaponAmmo > 0 && hasAmmo)
        {
            WeaponProjectile projectile = _projectilePool.Get();

            RandomizeSpread();
            projectile.transform.Rotate(spreadDirection);

            AudioManager.instance.Play("Shoot");
            weaponAmmo -= 1;
        }
        else
        {
            AudioManager.instance.Play("NoAmmo");
            hasAmmo = false;
            Reload();
        }
        //audioSource.PlayOneShot(shootSFX, audioSource.volume);

        //Debug.Log(name + " attack");
    }

    private void RandomizeSpread()
    {
        spreadDirection.z = UnityEngine.Random.Range(-weaponStats.spread, weaponStats.spread);
    }
}
