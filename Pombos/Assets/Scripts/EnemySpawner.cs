using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


[Serializable]
public class Drop
{
    public int dropRate = 0;
    public GameObject dropObject;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] float spawnRadius;
    Transform playerTransform;

    [SerializeField] Transform DropContainer;

    [SerializeField] float spawnCooldown;

    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Collectable> _dropPool;

    [SerializeField] GameObject dropGold;
    [SerializeField] List<Drop> dropItems;
    //private List<ObjectPool<Collectable>> _dropItemsPool;

    // Start is called before the first frame update
    void Start()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        _dropPool = new ObjectPool<Collectable>(CreateDrop, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);

        playerTransform = GameManager.instance.playerTransform;
        StartCoroutine("CooldownSpawn");
        /*
        foreach (Drop drop in dropItems)
        {
            _dropItemsPool.Add(new ObjectPool<Collectable>(CreateDrop, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject));
        }
        */
    }

    private Enemy CreateEnemy()
    {
        Enemy newEnemy = GameObject.Instantiate(EnemyPrefab, transform).GetComponent<Enemy>();
        newEnemy.SetPlayerTransform(playerTransform);
        newEnemy.Init(KillEnemy);
        return newEnemy;
    }
    void OnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }
    void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private Collectable CreateDrop()
    {
        Collectable newGold = GameObject.Instantiate(dropGold, DropContainer).GetComponent<Gold>();
        newGold.Init(DisableCollectable);
        return newGold;
    }
    void OnReturnedToPool(Collectable drop)
    {
        drop.gameObject.SetActive(false);
    }
    void OnTakeFromPool(Collectable drop)
    {
        drop.gameObject.SetActive(true);
    }
    void OnDestroyPoolObject(Collectable drop)
    {
        Destroy(drop.gameObject);
    }

    void OnSetUpEnemy(Enemy enemy)
    {
        Vector3 spawnPosition = GeneratePosition();
        spawnPosition += playerTransform.position;
        enemy.transform.position = spawnPosition;

        enemy.hp = enemy.maxHp;
        enemy.isAttackOnCooldown = false;
    }

    private void SpawnEnemy()
    {
        Enemy nEnemy = _enemyPool.Get();
        OnSetUpEnemy(nEnemy);

        StartCoroutine("CooldownSpawn");
    }

    private Vector3 GeneratePosition()
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10),0).normalized;
        //Debug.Log(position + " mag:" + position.magnitude);

        if (position.magnitude == 0)
        {
            position.x = 1;
        }

        position.x *= spawnRadius;
        position.y *= spawnRadius;
        //Debug.Log(position + " mag:" + position.magnitude);

        return position;
    }

    private IEnumerator CooldownSpawn()
    {
        yield return new WaitForSeconds(spawnCooldown);
        SpawnEnemy();
    }

    private void KillEnemy(Enemy enemy)
    {
        Collectable goldDrop = _dropPool.Get();
        goldDrop.transform.position = enemy.transform.position;

        GameObject dropPrefab = GetRandomDrop();
        if (dropPrefab != null)
        {
            Collectable drop = Instantiate(dropPrefab, enemy.transform.position, dropPrefab.transform.rotation, DropContainer).GetComponent<Collectable>();
            drop.Init(DestroyCollectable);
        }

        enemy.StopAllCoroutines();
        _enemyPool.Release(enemy);
    }

    private GameObject GetRandomDrop()
    {
        GameObject drop = null;
        int dropNumber = UnityEngine.Random.Range(0,100);
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

    private void DisableCollectable(Collectable collectable)
    {
        _dropPool.Release(collectable);
    }

    private void DestroyCollectable(Collectable collectable)
    {
        Destroy(collectable.gameObject);
    }
}
