using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private Action<Enemy> _killAction;


    Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        moveDirection.x = playerTransform.position.x - transform.position.x;
        moveDirection.y = playerTransform.position.y - transform.position.y;
        moveDirection = moveDirection.normalized;
        //Debug.Log(direction);
        step = speed * Time.fixedDeltaTime;

        //_Rigidbody.AddForce(moveDirection * speed * Time.deltaTime, ForceMode.VelocityChange);

        //transform.rotation = Quaternion.LookRotation(moveDirection);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        _Rigidbody.AddForce(moveDirection * step, ForceMode2D.Force);


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
        Debug.Log(direction * force);
        _Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetPlayerTransform(Transform nPlayerTransform)
    {
        playerTransform = nPlayerTransform;
    }
}
