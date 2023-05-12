using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : Enemy
{
    [SerializeField] int shotAmount = 3;
    [SerializeField] float shotAngle = 15f;
    public override void Attack()
    {
        if (!isAttackOnCooldown)
        {
            float angleBetwenProjectiles = shotAngle / shotAmount;
            float projectileAngle = GetFirstProjectileAngle(shotAmount, shotAngle, angleBetwenProjectiles);
            Vector3 spread = Vector3.zero;
            for (int i = 0; i < shotAmount; i++)
            {
                EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation).GetComponent<EnemyProjectile>();
                spread.z = projectileAngle;
                projectile.transform.Rotate(spread);
                projectileAngle += angleBetwenProjectiles;
                projectile.enemy = this;
            }
            StartCoroutine("CooldownAttack");
        }
    }

    private float GetFirstProjectileAngle(int shotAmount, float shotAngle, float angleBetwenProjectiles)
    {
        float angle;
        if (shotAmount % 2 == 1)
        {
            angle = Mathf.Floor(shotAmount / shotAngle) * angleBetwenProjectiles;
        }
        else
        {
            angle = (shotAngle / 2) + ((shotAmount / 2 - 1) * angleBetwenProjectiles);
        }
        return angle;
    }
}

/*
15 / 3 = 5

3/2 = 1.5 floor 1

1 * -5

-5, 0, 5


15 / 5 = 3

5/2 = 2.5 floor 2

2 * -5

-6, -3, 0, 3 ,6


20 / 4 = 5
-7.5, -2.5, 2.5, 7.5


30 / 6 = 5

5/2 = 2.5

(6/2)-1 * 5 = 10
10+2,5 = 12,5

-12.5, -7.5, -2.5, 2.5, 7.5, 12.5
*/