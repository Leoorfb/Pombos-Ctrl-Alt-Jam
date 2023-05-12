using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : Enemy
{
    public override void Attack()
    {
        if (!isAttackOnCooldown)
        {
            EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation, transform).GetComponent<EnemyProjectile>();
            projectile.enemy = this;
            StartCoroutine("CooldownAttack");
        }
    }
}
