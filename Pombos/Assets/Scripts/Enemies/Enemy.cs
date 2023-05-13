using System.ComponentModel.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public abstract class Enemy : MonoBehaviour
{
    Transform playerTransform;
    Player player;

    [Header("Enemy Health Settings")]
    public int hp = 10;
    public int maxHp = 10;

    [Header("Enemy Drop Settings")]
    [SerializeField] protected int money = 1;

    [Header("Enemy Movement Settings")]
    [SerializeField] protected float speed = 4;
    [SerializeField] protected float rotationSpeed = 45;

    [Header("Enemy Attack Settings")]
    [SerializeField] protected int damage = 1;
    public float knockbackStreght = 10;
    [SerializeField] protected float attackDistance = 10;
    public bool isAttackOnCooldown = false;
    [SerializeField] protected float attackCooldown = 0.2f;

    [Header("Enemy Refences Settings")]
    [SerializeField] protected GameObject enemyProjectilePrefab;
    [SerializeField] protected Transform projectileOrigin;
    [SerializeField] protected GameObject goldPrefab;
    [SerializeField] protected List<Drop> dropItems;
    public Transform DropContainer;

    [SerializeField] protected GameObject deathParticles;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Color deadColor;


    [SerializeField] protected LayerMask playerLayerMask;

    Vector2 moveDirection = new Vector2(0, 0);
    float step = 0.1f;

    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _Rigidbody;
    Collider2D _Collider;

    private Action<Enemy> _killAction;
    private bool isCloseToPlayer;
    private bool hasPlayerSight;

    private bool isAlive = true;


    public void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Collider = GetComponent<Collider2D>();
    }

    public virtual void Start()
    {
        playerTransform = GameManager.instance.playerTransform;
        player = playerTransform.GetComponent<Player>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(_Rigidbody.position, playerTransform.position, OnPathComplete);

        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive || path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - _Rigidbody.position).normalized;

        //Debug.Log("Is close to player: " + isCloseToPlayer + " - Has player sight: " + hasPlayerSight);
        if (!isCloseToPlayer || !hasPlayerSight)
        {
            MoveToPlayer();
            LookAtDirection(moveDirection);
        }

        CheckPlayerDistance();
        if (isCloseToPlayer)
        {
            CheckPlayerSight();
            if (hasPlayerSight)
            {
                Vector3 playerDir = playerTransform.position - transform.position;
                LookAtDirection(new Vector2(playerDir.x, playerDir.y));
                Attack();
            }
        }
    }

    void MoveToPlayer()
    {
        step = speed * Time.fixedDeltaTime;
        _Rigidbody.AddForce(moveDirection * step, ForceMode2D.Force);

        float distance = Vector2.Distance(_Rigidbody.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void LookAtDirection(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(Vector2.right, dir) - 90f;
        Vector3 targetRotation = new Vector3(0, 0, angle);
        Quaternion lookTo = Quaternion.Euler(targetRotation);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, rotationSpeed * Time.fixedDeltaTime);
    }

    private void CheckPlayerDistance()
    {
        //Debug.Log("Distancia: " + Vector3.Distance(playerTransform.position, transform.position) + " - Attack Distance: " + attackDistance);
        if (Vector3.Distance(playerTransform.position, transform.position) < attackDistance)
        {
            isCloseToPlayer = true;
        }
        else
        {
            isCloseToPlayer = false;
        }
    }

    private void CheckPlayerSight()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast( transform.position, playerTransform.position - transform.position, attackDistance,playerLayerMask, 2);

        //Debug.Log("Raycast hit: " + hit.collider.name + " - Raycast tag: " + hit.collider.gameObject.tag);
        if (hit.collider.gameObject.tag == "Player")
        {
            hasPlayerSight = true;
        }
        else
        {
            hasPlayerSight = false;
        }
    }

    public abstract void Attack();

    IEnumerator CooldownAttack()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }

    public void HitPlayer(Vector2 direction)
    {
        if (!player.isInvulnerable)
        {
            player.GetHit(damage, direction, knockbackStreght);
            AudioManager.instance.Play("HitPlayer");
        }
        //player.TakeDamage(damage);
    }

    public void Init(Action<Enemy> killAction)
    {
        _killAction = killAction;
    }

    public virtual void GetHit(int damage, bool isCritical, Vector2 direction, float force)
    {
        TakeDamage(damage, isCritical);
        TakeKnockback(direction, force);
        //Debug.Log("Enemy is Alive: " + isAlive);

        if (!isAlive)
        {
            //Debug.Log("Death particles intanciou");
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject particle = GameObject.Instantiate(deathParticles, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            //Debug.Log(particle);
        }
    }

    public void TakeDamage(int damage, bool isCritical)
    {
        DamagePopupScript.Create(transform.position, damage, isCritical);
        if (isCritical)
        {
            AudioManager.instance.Play("CritEnemy");
        }
        else
        {
            AudioManager.instance.Play("HitEnemy");
        }

        if (damage != 0)
        {
            hp -= damage;
            //Debug.Log(damage + " damage taken");

            if (hp <= 0)
            {
                Die();
                /*
                if (_killAction != null)
                    _killAction(this);
                else Destroy(gameObject);
                */
            }
            //Debug.Log(name + " hp: " + hp);

        }
        return;
    }

    public void TakeKnockback(Vector2 direction, float force)
    {
        ///Debug.Log(direction * force);
        _Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetPlayerTransform(Transform nPlayerTransform)
    {
        playerTransform = nPlayerTransform;
    }

    private void Die()
    {
        isAlive = false;
        _Rigidbody.velocity = Vector3.zero;
        _Rigidbody.isKinematic = true;


        GameObject gold = GameObject.Instantiate(goldPrefab, transform.position, goldPrefab.transform.rotation, DropContainer);

        //private void Moneydrop()
        //{
        //
        //    return money();
        //}
        GameObject dropPrefab = GetRandomDrop();
        if (dropPrefab != null)
        {
            //MoneyDrop();
            GameObject drop = GameObject.Instantiate(dropPrefab, transform.position, dropPrefab.transform.rotation, DropContainer);
            Debug.Log(drop);
        }

        //enemiesAlive--;
        _Collider.enabled = false;
        AudioManager.instance.Play("EnemyDeath");

        spriteRenderer.color = deadColor;
    }


    /*
    private IEnumerator FadeAway()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            spriteRenderer.color = deadColor;
        }
    }
    */

    private GameObject GetRandomDrop()
    {
        GameObject drop = null;
        int dropNumber = UnityEngine.Random.Range(0, 100);
        foreach (Drop _drop in dropItems)
        {
            dropNumber -= _drop.dropRate;

            if (dropNumber <= 0)
            {
                drop = _drop.dropObject;
                break;
            }
        }

        return drop;
    }
}
