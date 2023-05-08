using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan;
    public WeaponBase weapon;
    public Vector2 direction = Vector2.zero;

    private Action<WeaponProjectile> _DisableProjectile;

    private void Start()
    {
        StartCoroutine("AttackDuration");
    }

    private void OnEnable()
    {
        StartCoroutine("AttackDuration");
        //direction.x = ;
    }

    protected virtual void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Projetil ENTROU GATILHO");

        if (collision.gameObject.tag == "Enemy")
        {
            weapon.HitEnemy(collision.gameObject.GetComponent<Enemy>(), transform.right);

            //Debug.Log("Projetil atingiu inimigo");
            StopAllCoroutines();
            _DisableProjectile(this);
        }
        
        else if (collision.gameObject.tag == "Obstacle")
        {
            //Debug.Log("Projetil atingiu OBSTACULO 1");
            StopAllCoroutines();
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
}
