using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    public int stage = 1;

    [Header("Boss Stage 1 (Pistol) Settings")]
    [SerializeField] protected Color stage1Color;
    [SerializeField] Sprite stage1Sprite;

    [SerializeField] float speed_Stage1 = 4;
    [SerializeField] int damage_Stage1 = 1;
    [SerializeField] float knockbackStreght_Stage1 = 10;
    [SerializeField] float attackDistance_Stage1 = 10;
    [SerializeField] float attackCooldown_Stage1 = 0.2f;
    [SerializeField] GameObject enemyProjectilePrefab_Stage1;
    [SerializeField] Transform projectileOrigin_Stage1;

    [Header("Boss Stage 2 (ShotGun) Settings")]
    [SerializeField] int stage2StartHP = 20;
    [SerializeField] protected Color stage2Color;
    [SerializeField] Sprite stage2Sprite;


    [SerializeField] float speed_Stage2 = 4;
    [SerializeField] int damage_Stage2 = 1;
    [SerializeField] float knockbackStreght_Stage2 = 10;
    [SerializeField] float attackDistance_Stage2 = 10;
    [SerializeField] float attackCooldown_Stage2 = 0.2f;
    [SerializeField] GameObject enemyProjectilePrefab_Stage2;
    [SerializeField] Transform projectileOrigin_Stage2;

    [SerializeField] int shotAmount = 3;
    [SerializeField] float shotAngle = 15f;

    [Header("Boss Stage 3 (Thompson) Settings")]
    [SerializeField] int stage3StartHP = 10;
    [SerializeField] protected Color stage3Color;
    [SerializeField] Sprite stage3Sprite;


    [SerializeField] float speed_Stage3 = 4;
    [SerializeField] int damage_Stage3 = 1;
    [SerializeField] float knockbackStreght_Stage3 = 10;
    [SerializeField] float attackDistance_Stage3 = 10;
    [SerializeField] float attackCooldown_Stage3 = 0.2f;
    [SerializeField] GameObject enemyProjectilePrefab_Stage3;
    [SerializeField] Transform projectileOrigin_Stage3;

    [SerializeField] float baseWeaponSpread = 1;
    private Vector3 spreadDirection;

    public override void Start()
    {
        base.Start();
        SetStage(1);
    }

    public override void GetHit(int damage, bool isCritical, Vector2 direction, float force)
    {
        base.GetHit(damage, isCritical, direction, force);
        CheckStageChange();
    }

    public void CheckStageChange()
    {
        if (hp < stage3StartHP)
        {
            SetStage(3);
        }
        else if (hp < stage2StartHP)
        {
            SetStage(2);
        }
    }

    public void SetStage(int stage)
    {
        this.stage = stage;
        switch (stage)
        {
            case 1:
                spriteRenderer.sprite = stage1Sprite;
                spriteRenderer.color = stage1Color;
                speed = speed_Stage1;
                damage = damage_Stage1;
                knockbackStreght = knockbackStreght_Stage1;
                attackDistance = attackDistance_Stage1;
                attackCooldown = attackCooldown_Stage1;
                enemyProjectilePrefab = enemyProjectilePrefab_Stage1;
                projectileOrigin = projectileOrigin_Stage1;
                break;
            case 2:
                spriteRenderer.sprite = stage2Sprite;
                spriteRenderer.color = stage2Color;
                speed = speed_Stage2;
                damage = damage_Stage2;
                knockbackStreght = knockbackStreght_Stage2;
                attackDistance = attackDistance_Stage2;
                attackCooldown = attackCooldown_Stage2;
                enemyProjectilePrefab = enemyProjectilePrefab_Stage2;
                projectileOrigin = projectileOrigin_Stage2;
                break;
            case 3:
                spriteRenderer.sprite = stage3Sprite;
                spriteRenderer.color = stage3Color;
                speed = speed_Stage3;
                damage = damage_Stage3;
                knockbackStreght = knockbackStreght_Stage3;
                attackDistance = attackDistance_Stage3;
                attackCooldown = attackCooldown_Stage3;
                enemyProjectilePrefab = enemyProjectilePrefab_Stage3;
                projectileOrigin = projectileOrigin_Stage3;
                break;
        }
    }


    public override void Attack()
    {
        switch (stage)
        {
            case 1:
                PistolAttack();
                break;
            case 2:
                ShotGunAttack();
                break;
            case 3:
                ThompsonAttack();
                break;
        }
    }

    private void PistolAttack()
    {
        if (!isAttackOnCooldown)
        {
            EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation).GetComponent<EnemyProjectile>();
            projectile.enemy = this;
            StartCoroutine("CooldownAttack");
        }
    }

    private void ShotGunAttack()
    {
        if (!isAttackOnCooldown)
        {
            float angleBetwenProjectiles = shotAngle / shotAmount;
            float projectileAngle = GetFirstProjectileAngle(shotAmount, shotAngle, angleBetwenProjectiles);
            Vector3 spread = Vector3.zero;
            for (int i = 0; i < shotAmount; i++)
            {
                //Debug.Log(projectileAngle);
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
            angle = Mathf.Floor(shotAmount / 2) * -angleBetwenProjectiles;
            //Debug.Log("Impar (" + shotAmount + "/2) * -" + angleBetwenProjectiles + " = " + angle);
        }
        else
        {
            angle = -((angleBetwenProjectiles / 2) + (((shotAmount / 2) - 1) * angleBetwenProjectiles));
            //Debug.Log("Par -((" + angleBetwenProjectiles + "/2) = |" + (angleBetwenProjectiles / 2) + "| + ((" + shotAmount + " / 2) | " + (shotAmount / 2) + "| - 1) *  - 1)  * " + angleBetwenProjectiles + ")) = " + angle);

        }
        return angle;
    }

    private void ThompsonAttack()
    {
        if (!isAttackOnCooldown)
        {
            EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation).GetComponent<EnemyProjectile>();
            RandomizeSpread();
            projectile.transform.Rotate(spreadDirection);

            projectile.enemy = this;
            StartCoroutine("CooldownAttack");
        }
    }

    private void RandomizeSpread()
    {
        spreadDirection.z = UnityEngine.Random.Range(-baseWeaponSpread, baseWeaponSpread);
    }
}
