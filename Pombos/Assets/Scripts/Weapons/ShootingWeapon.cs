using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : WeaponBase
{
    public override void Attack()
    {
        _projectilePool.Get();

        //Debug.Log(name + " attack");
        StartCoroutine("CooldownAttack");
    }
}
