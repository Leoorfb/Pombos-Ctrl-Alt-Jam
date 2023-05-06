using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan;
    public WeaponBase weapon;

    private Action<WeaponProjectile> _DisableProjectile;

    private void Start()
    {
        StartCoroutine("AttackDuration");
    }

    private void OnEnable()
    {
        StartCoroutine("AttackDuration");
    }

    protected virtual void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            weapon.HitEnemy(collision.gameObject.GetComponent<Enemy>());

            //Debug.Log("Projetil atingiu inimigo");
            _DisableProjectile(this);
        }
    }

    public void Init(Action<WeaponProjectile> disableProjectile)
    {
        _DisableProjectile = disableProjectile;
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(lifespan);
        //Debug.Log("Projetil terminou lifespan");
        _DisableProjectile(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
