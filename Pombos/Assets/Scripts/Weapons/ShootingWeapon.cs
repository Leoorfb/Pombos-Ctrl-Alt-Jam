using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : WeaponBase
{
    [SerializeField] float spread = 0;
    Vector3 spreadDirection = Vector3.zero;

    public override void Attack()
    {
        WeaponProjectile projectile = _projectilePool.Get();

        RandomizeSpread();
        projectile.transform.Rotate(spreadDirection);

        //Debug.Log(name + " attack");
    }

    private void RandomizeSpread()
    {
        spreadDirection.z = UnityEngine.Random.Range(-spread, spread);
    }
}
