using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : Enemy
{
    public override void Attack()
    {
        if (!isAttackOnCooldown)
        {
            EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation).GetComponent<EnemyProjectile>();
            projectile.enemy = this;
            StartCoroutine("CooldownAttack");
        }
    }
}
