using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    public int hp = 10;
    public int maxHp = 10;
    [SerializeField] float speed = 4;
    [SerializeField] int damage = 1;
    public float knockbackStreght = 10;

    public bool isAttackOnCooldown = false;
    [SerializeField] float attackCooldown = 0.2f;

    Vector2 moveDirection = new Vector2(0, 0);
    float step = 0.1f;

    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _Rigidbody;

    private Action<Enemy> _killAction;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if(_seeker.IsDone())
            _seeker.StartPath(_Rigidbody.position, playerTransform.position, OnPathComplete);
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

        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - _Rigidbody.position).normalized;
        
        /*
        moveDirection.x = playerTransform.position.x - transform.position.x;
        moveDirection.y = playerTransform.position.y - transform.position.y;
        moveDirection = moveDirection.normalized;
        */

        //Debug.Log(direction);
        step = speed * Time.fixedDeltaTime;

        //_Rigidbody.AddForce(moveDirection * speed * Time.deltaTime, ForceMode.VelocityChange);

        //transform.rotation = Quaternion.LookRotation(moveDirection);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        _Rigidbody.AddForce(moveDirection * step, ForceMode2D.Force);

        float distance = Vector2.Distance(_Rigidbody.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //_Rigidbody.velocity = moveDirection * speed; //* Time.deltaTime;
    }

    public int Attack()
    {
        if (isAttackOnCooldown || !gameObject.activeInHierarchy)
        {
            return 0;
        }

        isAttackOnCooldown = true;
        StartCoroutine("CooldownAttack");
        return damage;
    }

    IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
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
