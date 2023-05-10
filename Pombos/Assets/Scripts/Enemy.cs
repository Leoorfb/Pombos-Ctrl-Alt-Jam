using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    Transform playerTransform;
    Player player;

    public int hp = 10;
    public int maxHp = 10;
    [SerializeField] float speed = 4;
    [SerializeField] int damage = 1;
    public float knockbackStreght = 10;
    [SerializeField] float attackDistance = 10;

    public bool isAttackOnCooldown = false;
    [SerializeField] float attackCooldown = 0.2f;
    [SerializeField] float rotationSpeed = 45;

    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] Transform projectileOrigin;

    [SerializeField] LayerMask playerLayerMask;

    Vector2 moveDirection = new Vector2(0, 0);
    float step = 0.1f;

    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _Rigidbody;

    private Action<Enemy> _killAction;
    private bool isCloseToPlayer;
    private bool hasPlayerSight;


    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
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
        if (path == null) return;

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

    public void Attack()
    {
        if (!isAttackOnCooldown)
        {
            EnemyProjectile projectile = Instantiate(enemyProjectilePrefab, projectileOrigin.position, transform.rotation).GetComponent<EnemyProjectile>();
            projectile.enemy = this;
            StartCoroutine("CooldownAttack");
        }
    }

    IEnumerator CooldownAttack()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }

    public void HitPlayer()
    {
        player.TakeDamage(damage);
    }

    public void Init(Action<Enemy> killAction)
    {
        _killAction = killAction;
    }

    public void TakeDamage(int damage)
    {
        if (damage != 0)
        {
            hp -= damage;
            //Debug.Log(damage + " damage taken");

            if (hp <= 0)
            {
                if (_killAction != null)
                    _killAction(this);
                else Destroy(gameObject);
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
}
