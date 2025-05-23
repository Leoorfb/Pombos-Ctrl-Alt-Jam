using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;


[Serializable]
public class Drop
{
    public int dropRate = 0;
    public GameObject dropObject;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WaveData[] wavesData;
    public int waveIndex = 0;
    [SerializeField] float enemySpawnCooldown;
    [SerializeField] int enemiesAlive = 0;
    public bool isWaveRunning = false;
    [SerializeField] List<Transform> SpawnPoints;
    [SerializeField] float spawnRadius;
    [SerializeField] TextMeshProUGUI WaveTimerText;
    [SerializeField] TextMeshProUGUI WaveText;
    [SerializeField] bool loopLastWave;



    [SerializeField] Transform DropContainer;

    //private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Collectable> _dropPool;

    [SerializeField] GameObject dropGold;
    [SerializeField] List<Drop> dropItems;


    Transform playerTransform;

    IEnumerator waveCoroutine;
    IEnumerator waveTimerCoroutine;

    //private List<ObjectPool<Collectable>> _dropItemsPool;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
        //_enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        _dropPool = new ObjectPool<Collectable>(CreateDrop, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);

        playerTransform = GameManager.instance.playerTransform;

        waveCoroutine = SpawnWave();
        StartWave();

        /*
        foreach (Drop drop in dropItems)
        {
            _dropItemsPool.Add(new ObjectPool<Collectable>(CreateDrop, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject));
        }
        */
    }

    private Enemy CreateEnemy(GameObject enemy)
    {
        Enemy newEnemy = Instantiate(enemy, transform).GetComponent<Enemy>();
        newEnemy.SetPlayerTransform(playerTransform);
        newEnemy.Init(KillEnemy);
        newEnemy.DropContainer = DropContainer;
        return newEnemy;
    }
    //Exclusivo pra object pool (removido por enquanto para simplificar/agilizar)
    /*
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
    */

    private IEnumerator SpawnWave()
    {
        isWaveRunning = true;
        while (true)
        {
            List<GameObject> enemiesToSpawn = wavesData[waveIndex].GetWave();
            float spawnRate = wavesData[waveIndex].spawnRate;
            foreach (GameObject enemy in enemiesToSpawn)
            {
                SpawnEnemy(enemy);
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }

    private IEnumerator TimerWave()
    {
        //Debug.Log("Come�ou timer");
        float timer = wavesData[waveIndex].waveDuration;
        while (timer >= 0)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            WaveTimerText.text = timer.ToString("#00.0");
        }
        StopCoroutine(waveCoroutine);
        isWaveRunning = false;
        waveIndex++;
    }

    public void StartWave()
    {
        //Debug.Log("Wave: "+ waveIndex + " Tamanho " + wavesData.Length);
        if (waveIndex >= wavesData.Length)
        {
            if (loopLastWave)
            {
                waveIndex = wavesData.Length - 1;
            }
            else return;
        }
        WaveText.text = "Wave " + (waveIndex+1).ToString("#00"); 
        StartCoroutine(waveCoroutine);
        StartCoroutine("TimerWave");
            
    }

    private void SpawnEnemy(GameObject enemy)
    {
        //Enemy nEnemy = _enemyPool.Get();
        Enemy nEnemy = CreateEnemy(enemy);
        enemiesAlive++;
        OnSetUpEnemy(nEnemy);

        //StartCoroutine("CooldownSpawn");
    }

    void OnSetUpEnemy(Enemy enemy)
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        enemy.transform.position = spawnPosition;

        enemy.hp = enemy.maxHp;
        enemy.isAttackOnCooldown = false;
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
        enemiesAlive--;
        AudioManager.instance.Play("EnemyDeath");

        enemy.StopAllCoroutines();
        Destroy(enemy.gameObject);
        //_enemyPool.Release(enemy);
    }

    /*
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
    */
    /*
    private IEnumerator CooldownSpawn()
    {
        yield return new WaitForSeconds(enemySpawnCooldown);
        SpawnEnemy();
    }
    */

    Vector3 GetRandomSpawnPoint()
    {
        int random = UnityEngine.Random.Range(0,SpawnPoints.Count);
        Vector3 pos = SpawnPoints[random].position;
        while (Vector3.Distance(pos, playerTransform.position) < spawnRadius)
        {
            random = UnityEngine.Random.Range(0, SpawnPoints.Count);
            pos = SpawnPoints[random].position;
        }
        return pos;
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
