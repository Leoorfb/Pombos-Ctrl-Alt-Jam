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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            weapon.HitEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }

    public void Init(Action<WeaponProjectile> disableProjectile)
    {
        _DisableProjectile = disableProjectile;
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(lifespan);
        _DisableProjectile(this);
    }
}
