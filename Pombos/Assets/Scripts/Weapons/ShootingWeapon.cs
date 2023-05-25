using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : WeaponBase
{
    public override void Attack()
    {
        if (hasAmmo)
        {
            if (weaponAmmo > 0)
            {
                _player.playerBodyAnimator.SetBool("isShooting", true);
                _player.playerBodyAnimator.SetInteger("ShootIndex", UnityEngine.Random.Range(0,2));

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
                StartCoroutine("Reload");
            }
        }
    }
        

    private void RandomizeSpread()
    {
        spreadDirection.z = UnityEngine.Random.Range(-weaponStats.spread, weaponStats.spread);
    }
}
